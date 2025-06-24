using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SCIC_BE.DTO.StudentDTOs;
using SCIC_BE.Models;

namespace SCIC_BE.Interfaces.IServices
{
    public interface IStudentService
    {
        Task<List<StudentDTO>> GetListStudentAsync();
        Task CreateStudentAsync(Guid id, CreateStudentDTO dto);
        Task CreateStudentInfoAsync(Guid userId, string studentCode, DateTime enrollDate);
        Task DeleteStudentAsync(Guid userId);
        Task<StudentDTO> GetStudentByIdAsync(Guid studentId);
        Task UpdateStudentInfoAsync(Guid userId, string newStudentCode);
    }

}
