using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using StudentMentor.Data.Entities;
using StudentMentor.Data.Entities.Models;
using StudentMentor.Domain.Abstractions;
using StudentMentor.Domain.Models.ViewModels;
using StudentMentor.Domain.Repositories.Interfaces;
using StudentMentor.Domain.Services.Interfaces;

namespace StudentMentor.Domain.Repositories.Implementations
{
    public class CommentRepository : ICommentRepository
    {
        private readonly StudentMentorDbContext _context;
        private readonly IClaimProvider _claimProvider;
        private readonly IMapper _mapper;

        public CommentRepository(StudentMentorDbContext context, IClaimProvider claimProvider, IMapper mapper)
        {
            _context = context;
            _claimProvider = claimProvider;
            _mapper = mapper;
        }

        public async Task<ResponseResult<CommentModel>> Add(CreateCommentModel model)
        {
            var comment = _mapper.Map<Comment>(model);
            comment.UserId = _claimProvider.GetUserId();
            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();

            var commentModel = await _context
                .Comments
                .Where(c => c.Id == comment.Id)
                .ProjectTo<CommentModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();

            return new ResponseResult<CommentModel>(commentModel);
        }
    }
}
