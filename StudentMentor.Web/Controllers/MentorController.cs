using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentMentor.Domain.Models.ViewModels.Account;
using StudentMentor.Domain.Repositories.Interfaces;
using StudentMentor.Domain.Services.Interfaces;
using StudentMentor.Web.Infrastructure;
using StudentMentor.Web.Infrastructure.Email;

namespace StudentMentor.Web.Controllers
{
    public class MentorController : ApiController
    {
        private readonly IMentorRepository _mentorRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly IJwtService _jwtService;
        private readonly EmailHandler _emailSender;
        private readonly IClaimProvider _claimProvider;

        public MentorController(
            IMentorRepository mentorRepository,
            IStudentRepository studentRepository,
            IJwtService jwtService,
            EmailHandler emailSender,
            IClaimProvider claimProvider
        ) {
            _mentorRepository = mentorRepository;
            _studentRepository = studentRepository;
            _jwtService = jwtService;
            _emailSender = emailSender;
            _claimProvider = claimProvider;
        }

        [Authorize(Policy = Policies.Admin)]
        [HttpGet]
        public async Task<ActionResult> GetMentors()
        {
            var mentors = await _mentorRepository.GetMentors();

            return Ok(mentors);
        }

        [Authorize(Policy = Policies.Admin)]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteMentor(int id)
        {
            var response = await _mentorRepository.DeleteMentor(id);

            if (response.IsError)
            {
                return NotFound(response.Message);
            }

            return Ok();
        }

        [Authorize(Policy = Policies.Admin)]
        [HttpPost]
        public async Task<ActionResult> InviteMentor(MentorInviteModel model)
        {
            var mentorResponse = await _mentorRepository.CreateMentor(model);
            var token = _jwtService.GetTokenForEmailInvite(mentorResponse.Data.Id);
            await _emailSender.SendMentorInviteEmail(mentorResponse.Data, token);

            return Ok();
        }

        [Authorize(Policy = Policies.Mentor)]
        [HttpPatch("ClaimStudent/{Id}")]
        public async Task<ActionResult> ClaimStudent([FromRoute] int id)
        {
            var response = await _studentRepository.SetMentor(id, _claimProvider.GetUserId());

            if (response.IsError)
                return BadRequest(response.Message);

            return Ok();
        }

        [Authorize(Policy = Policies.Mentor)]
        [HttpPatch("UnClaimStudent/{Id}")]
        public async Task<ActionResult> UnClaimStudent([FromRoute] int id)
        {
            var response = await _studentRepository.SetMentor(id, null);

            if (response.IsError)
                return BadRequest(response.Message);

            return Ok();
        }

        [Authorize(Policy = Policies.Student)]
        [HttpGet(nameof(GetMentor))]
        public async Task<ActionResult> GetMentor()
        {
            var mentorModel = await _mentorRepository.GetStudentsMentor(_claimProvider.GetUserId());

            return Ok(mentorModel);
        }

        [AllowAnonymous]
        [HttpPut("{id}")]
        public async Task<ActionResult> MentorFinishRegistration([FromRoute] int id, MentorRegistrationModel model)
        {
            var response = await _mentorRepository.SetMentorPassword(id, model);

            if (response.IsError)
                return NotFound(response.Message);

            var token = _jwtService.GetJwtTokenForUser(response.Data);
            return Ok(token);
        }

        [AllowAnonymous]
        [HttpGet(nameof(GetInfoFromToken))]
        public async Task<ActionResult> GetInfoFromToken([FromQuery] string token)
        {
            var userId = _jwtService.GetUserIdFromEmailToken(token);
            var mentor = await _mentorRepository.GetMentorById(userId);

            if (mentor == null)
                return NotFound("Cannot find mentor");

            return Ok(mentor);
        }
    }
}
