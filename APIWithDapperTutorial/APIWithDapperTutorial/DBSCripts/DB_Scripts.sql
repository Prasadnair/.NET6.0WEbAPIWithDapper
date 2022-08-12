
/* 
CREATE A DATABASE 'SchoolManagement  
then execute the scripts for School and Student
*/

CREATE TABLE [dbo].[School]
(
 Id INT IDENTITY(1,1) NOT NULL,
 Name VARCHAR(100),
 Address VARCHAR(200),
 CONSTRAINT PK_School_Id PRIMARY KEY(Id)
)

GO

CREATE TABLE [dbo].[Student]
(
 Id INT IDENTITY(1,1) NOT NULL,
 SchoolId INT NOT NULL,
 Name VARCHAR(100),
 Grade VARCHAR(10),
 CONSTRAINT PK_STudent_Id PRIMARY KEY(Id),
 CONSTRAINT FK_Student_Id FOREIGN KEY(SchoolId) REFERENCES School(Id)
)

GO

CREATE OR ALTER PROCEDURE [dbo].[ShowSchoolByStudentId]
(
 @Id INT
)
AS
BEGIN
SELECT sh.*  FROM dbo.School sh
inner join dbo.Student st
on st.Id =@Id

END

