using APIWithDapperTutorial.Data.Context;
using APIWithDapperTutorial.Data.Entities;
using APIWithDapperTutorial.Domain.Interface;
using APIWithDapperTutorial.Domain.Model;
using Dapper;
using System.Data;

namespace APIWithDapperTutorial.Data.Repository
{
    public class SchoolRepository : ISchoolRepostory
    {
        private readonly SchoolContext _schoolContext;

        public SchoolRepository(SchoolContext schoolContext)
        {
            _schoolContext = schoolContext;
        }

        public async Task CreateListOfSchoolsByAsync(List<SchoolDto> schoolList)
        {
            var query = "INSERT INTO School (Name, Address) VALUES (@Name, @Address)";

            using var connection = _schoolContext.CreateConnection();

            connection.Open();

            using var transaction = connection.BeginTransaction();

            foreach (var school in schoolList)
            {
                var parameters = new DynamicParameters();
                parameters.Add("Name", school.Name, DbType.String);
                parameters.Add("Address", school.Address, DbType.String);

                await connection.ExecuteAsync(query, parameters, transaction: transaction);
                //throw new Exception();
            }

            transaction.Commit();


        }

        public async Task<School> CreateSchoolAsync(SchoolDto school)
        {
            var query = "INSERT INTO School (Name, Address) VALUES (@Name, @Address)" +
                "SELECT CAST(SCOPE_IDENTITY() as int)";

            var parameters = new DynamicParameters();
            parameters.Add("Name", school.Name, DbType.String);
            parameters.Add("Address", school.Address, DbType.String);

            using var connection = this._schoolContext.CreateConnection();

            var id = await connection.QuerySingleAsync<int>(query, parameters);

            var createdSchool = new School
            {
                Id = id,
                Name = school.Name,
                Address = school.Address
            };

            return createdSchool;
        }  

        public async Task DeleteSchoolByIdAsync(int id)
        {
            var query = "DELETE FROM School WHERE Id = @Id";

            using var connection = _schoolContext.CreateConnection();
            
            await connection.ExecuteAsync(query, new { id });
            
        }

        public async Task<IEnumerable<School>> GetAllSchoolsAsync()
        {
            var query = "SELECT Id, Name, Address FROM School";
            using var con = _schoolContext.CreateConnection();
            var school = await con.QueryAsync<School>(query);
            return school.ToList();
        }

        public async Task<List<School>> GetMultipleSchoolsAndStudentsAsyn()
        {
            var query = "SELECT * FROM School c JOIN Student e ON c.Id = e.StudentId";

            using var connection = _schoolContext.CreateConnection();
            
                var schoolDict = new Dictionary<int, School>();

                var schools = await connection.QueryAsync<School, Student, School>(
                    query, (school, student) =>
                    {
                        if (!schoolDict.TryGetValue(school.Id, out var currentschool))
                        {
                            currentschool = school;
                            schoolDict.Add(currentschool.Id, currentschool);
                        }

                        currentschool.Students.Add(student);
                        return currentschool;
                    }
                );

                return schools.Distinct().ToList();
            
        }

        public async Task<School> GetSchoolByIdAsync(int id)
        {
            var query = "SELECT Id, Name, Address FROM School Where id=@id";

            var parameters = new DynamicParameters();
            parameters.Add("Id", id, DbType.Int32);

            using var con = _schoolContext.CreateConnection();


            var school = await con.QueryFirstOrDefaultAsync<School>(query,parameters);
            return school;
        }

        public async Task<School> GetSchoolByStudentIdAsync(int studentId)
        {
            var procedureName = "ShowSchoolByStudentId";
            var parameters = new DynamicParameters();
            parameters.Add("Id", studentId, DbType.Int32, ParameterDirection.Input);

            using var connection = _schoolContext.CreateConnection();
            
                var school = await connection.QueryFirstOrDefaultAsync<School>
                    (procedureName, parameters, commandType: CommandType.StoredProcedure);

                return school;            
        }

        public async Task<School> GetSchoolWithStudentsBySchoolId(int schoolId)
        {
            var query = "SELECT * FROM School WHERE Id = @Id;" +
                        "SELECT * FROM Student WHERE School = @Id";

            var connection = _schoolContext.CreateConnection();
            var multi = await connection.QueryMultipleAsync(query, new { schoolId });

            var school = await multi.ReadSingleOrDefaultAsync<School>();
            if (school != null)
                school.Students = (await multi.ReadAsync<Student>()).ToList();

            return school;

        }

        public async Task UpdateSchoolAsync(int id, SchoolDto school)
        {
            var query = "UPDATE School SET Name = @Name, Address = @Address WHERE Id = @Id";

            var parameters = new DynamicParameters();
            parameters.Add("Id", id, DbType.Int32);
            parameters.Add("Name", school.Name, DbType.String);
            parameters.Add("Address", school.Address, DbType.String);

            using var connection = _schoolContext.CreateConnection();

            await connection.ExecuteAsync(query, parameters);

        }
    }
}
