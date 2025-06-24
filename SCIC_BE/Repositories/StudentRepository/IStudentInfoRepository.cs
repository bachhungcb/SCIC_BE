using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SCIC_BE.DTO.StudentDTOs;
using SCIC_BE.Models;

namespace SCIC_BE.Repository.StudentRepository
{
    public interface IStudentInfoRepository
    {
        Task<List<StudentModel>> GetAllStudentsAsync();
        Task<StudentModel> GetByStudentIdAsync(Guid id);
        Task AddAsync(StudentModel studentInfo);
        Task UpdateAsync(StudentModel studentInfo);
        Task DeleteAsync(StudentModel student);
    }

}
