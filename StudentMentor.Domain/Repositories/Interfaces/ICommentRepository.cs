﻿using System.Threading.Tasks;
using StudentMentor.Domain.Abstractions;
using StudentMentor.Domain.Models.ViewModels;

namespace StudentMentor.Domain.Repositories.Interfaces
{
    public interface ICommentRepository
    {
        Task<ResponseResult<CommentModel>> Add(CreateCommentModel model, int? userId = null);
    }
}
