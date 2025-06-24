using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SCIC_BE.DTO.LecturerDTOs;
using SCIC_BE.DTO.StudentDTOs;

namespace SCIC_BE.Interfaces.IServices
{
    public interface ILecturerService
    {
        Task<List<LecturerDTO>> GetListLecturerAsync();
        Task CreateLecturerAsync(Guid id, CreateLecturerDTO dto);
        Task CreateLecturerInfoAsync(Guid userId, string lecturerCode, DateTime hireDate);
        Task DeleteLecturerAsync(Guid userId);
        Task<LecturerDTO> GetLecturerByIdAsync(Guid lecturerId);
        Task UpdateLecturerInfoAsync(Guid userId, string lecturerCode);
    }
}
