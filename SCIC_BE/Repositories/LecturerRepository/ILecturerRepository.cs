using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SCIC_BE.DTO.LecturerDTOs;
using SCIC_BE.Models;

namespace SCIC_BE.Repositories.LecturerRepository
{
    public interface ILecturerRepository
    {
        Task<List<LecturerModel>> GetAllLecturerAsync();
        Task<LecturerDTO> GetLecturerByIdAsync(Guid id);
        Task<LecturerModel> GetLecturerEntityByIdAsync(Guid id);
        Task AddAsync(LecturerModel lecturerInfo);
        Task UpdateAsync(LecturerModel lecturerInfo);
        Task DeleteAsync(LecturerModel lecturerInfo);
    }
}
