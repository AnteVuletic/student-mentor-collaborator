using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using StudentMentor.Data.Entities.Models;
using StudentMentor.Domain.Models.ViewModels;
using StudentMentor.Domain.Repositories.Interfaces;
using StudentMentor.Domain.Services.Interfaces;
using StudentMentor.Web.Hubs;
using StudentMentor.Web.Infrastructure;

namespace StudentMentor.Web.Controllers
{
    public class StudentController : ApiController
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IGithubService _githubService;
        private readonly IClaimProvider _claimProvider;
        private readonly IMessageRepository _messageRepository;
        private readonly IHubContext<MessagesHub> _messageHub;

        public StudentController(
            IStudentRepository studentRepository,
            IGithubService githubService,
            IClaimProvider claimProvider,
            IMessageRepository messageRepository,
            IHubContext<MessagesHub> messageHub
        ) {
            _studentRepository = studentRepository;
            _githubService = githubService;
            _claimProvider = claimProvider;
            _messageRepository = messageRepository;
            _messageHub = messageHub;
        }

        [Authorize(Policy = Policies.Admin)]
        [HttpGet]
        public async Task<ActionResult<ICollection<StudentModel>>> GetStudents()
        {
            var students = await _studentRepository.GetStudents();
            return Ok(students);
        }

        [Authorize(Policy = Policies.Admin)]
        [HttpGet("{id}")]
        public async Task<ActionResult<ICollection<StudentModel>>> GetStudents([FromRoute] int id)
        {
            var student = await _studentRepository.GetStudent(id);
            if (student.IsError)
                return NotFound(student.Message);

            return Ok(student.Data);
        }


        [Authorize(Policy = Policies.Admin)]
        [HttpPut("{studentId}")]
        public async Task<ActionResult> AssignMentorToStudent([FromRoute] int studentId, StudentAssignModel model)
        {
            if (model.MentorId.HasValue && model.MentorId != 0)
            {
                var result = await _studentRepository.SetMentor(studentId, model.MentorId.Value);

                if (result.IsError)
                    return BadRequest(result.Message);

                return Ok();
            }

            var removeMentorResult = await _studentRepository.RemoveMentor(studentId);

            if (removeMentorResult.IsError)
                return BadRequest(removeMentorResult.Message);

            return Ok();
        }

        [Authorize(Policy = Policies.Admin)]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteStudent([FromRoute] int studentId)
        {
            var response = await _studentRepository.DeleteStudent(studentId);
            if (response.IsError)
                return BadRequest(response.Message);

            return Ok();
        }

        [Authorize(Policy = Policies.Student)]
        [HttpPatch(nameof(PatchGithubAccessKey))]
        public async Task<ActionResult> PatchGithubAccessKey(CreateGithubAccess model)
        {
            var response = await _studentRepository.PatchGithubOAuthKey(model.GithubToken);
            var githubAccessToken = await _githubService.GetBearerToken(model.GithubToken);
            await _studentRepository.PatchGithubBearToken(githubAccessToken);

            if (response.IsError)
                return BadRequest(response.Message);

            return Ok();
        }

        [Authorize(Policy = Policies.Student)]
        [HttpGet(nameof(GetFinalsPaper))]
        public async Task<FileModel> GetFinalsPaper()
        {
            var fileModel = await _studentRepository.GetFinalsPaper();

            return fileModel;
        }

        [Authorize(Policy = Policies.Student)]
        [HttpPatch(nameof(PatchRepositoryId))]
        public async Task<ActionResult> PatchRepositoryId(RepositoryModel model)
        {
            var repositoryHook = await _githubService.CreateWebhookForRepositoryId(model.RepositoryId);
            if (repositoryHook.IsError)
                return BadRequest(repositoryHook.Data);

            var response = await _studentRepository.PatchRepositoryId(model.RepositoryId);

            if (response.IsError)
                return BadRequest(response.Message);

            return Ok();
        }

        [Authorize(Policy = Policies.Mentor)]
        [HttpGet(nameof(GetMentoringStudents))]
        public async Task<ActionResult> GetMentoringStudents()
        {
            var students = await _studentRepository.GetMentoringStudents(_claimProvider.GetUserId());

            return Ok(students);
        }

        [Authorize(Policy = Policies.Student)]
        [HttpPatch(nameof(PatchFinalesPaper))]
        public async Task<ActionResult> PatchFinalesPaper(File file)
        {
            var response = await _studentRepository.PatchFinalsPaper(file);
            if (response.IsError)
                return BadRequest();

            var student = response.Data;
            if (!student.MentorId.HasValue)
                return Ok();

            var messageModel = await _messageRepository.SendFileMessage(file, student.Id, student.MentorId.Value);
            if (!messageModel.IsError)
            {
                await _messageHub.Clients.Clients(student.Id.ToString(), student.MentorId.Value.ToString())
                    .SendAsync(MessagesHub.SendMessageMethod, messageModel);
            }

            return Ok();
        }
    }
}
