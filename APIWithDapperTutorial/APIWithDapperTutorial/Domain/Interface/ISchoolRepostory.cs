using APIWithDapperTutorial.Data.Entities;
using APIWithDapperTutorial.Domain.Model;

namespace APIWithDapperTutorial.Domain.Interface
{
    public interface ISchoolRepostory
    {
       Task<IEnumerable<School>> GetAllSchoolsAsync();
        Task<School> GetSchoolByIdAsync(int id);
        Task<School> CreateSchoolAsync(SchoolDto school);
        Task UpdateSchoolAsync(int id,SchoolDto school);
        Task DeleteSchoolByIdAsync(int id);
        Task<School> GetSchoolByStudentIdAsync(int studentId);
        Task<School> GetSchoolWithStudentsBySchoolId(int schoolId);
        Task<List<School>> GetMultipleSchoolsAndStudentsAsyn();
        Task CreateListOfSchoolsByAsync(List<SchoolDto> schoolList);

    }
}
