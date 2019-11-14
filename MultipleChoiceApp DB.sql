
use master
DROP DATABASE TestApp;
go

CREATE DATABASE TestApp;
go

use TestApp;

-- User
CREATE TABLE [User](
	Username VARCHAR(50) NOT NULL PRIMARY KEY,
	[Password] VARCHAR(60) NOT NULL,
	FirstName VARCHAR(25) NOT NULL,
	Surname VARCHAR(25) NOT NULL,
	UserType INTEGER NOT NULL, -- 0 = Student, 1 = Lecturer
	UniversityIdentification VARCHAR(15) NOT NULL UNIQUE -- Could be a student number or lecturer code
);

-- Module
CREATE TABLE Module(
	ModuleID VARCHAR(10) NOT NULL PRIMARY KEY,
	ModuleName VARCHAR(50) NOT NULL
);

-- Course
CREATE TABLE Course(
	CourseID VARCHAR(10) NOT NULL PRIMARY KEY,
	CourseName VARCHAR(100) NOT NULL
);

-- ModuleCourse
CREATE TABLE ModuleCourse(
	ModuleCourseID INTEGER IDENTITY(1,1) NOT NULL PRIMARY KEY,
	ModuleID VARCHAR(10)  NOT NULL FOREIGN KEY REFERENCES Module(ModuleID),
	CourseID VARCHAR(10) NOT NULL FOREIGN KEY REFERENCES Course(CourseID)
);

-- StudentAssignment
CREATE TABLE StudentAssignment(
	StudentAssignmentID INTEGER IDENTITY(1,1) NOT NULL PRIMARY KEY,
	Username VARCHAR(50) NOT NULL FOREIGN KEY REFERENCES [User](Username),
	CourseID VARCHAR(10) NOT NULL FOREIGN KEY REFERENCES Course(CourseID)
);

-- LecturerAssignment
CREATE TABLE LecturerAssignment(
	LecturerAssignmentID INTEGER IDENTITY(1,1) NOT NULL PRIMARY KEY,
	Username VARCHAR(50) NOT NULL FOREIGN KEY REFERENCES [User](Username),
	ModuleID VARCHAR(10) NOT NULL FOREIGN KEY REFERENCES Module(ModuleID)
);

-- Test
CREATE TABLE Test(
	TestID INTEGER IDENTITY(1,1) PRIMARY KEY NOT NULL,
	Username VARCHAR(50) NOT NULL FOREIGN KEY REFERENCES [User](Username), -- Lecturer who created test
	ModuleID VARCHAR(10) NOT NULL FOREIGN KEY REFERENCES Module(ModuleID),
	Title VARCHAR(50) NOT NULL,
	DueDate DateTime NOT NULL,
	Published BIT Default(1) NOT NULL
);

-- Question
CREATE TABLE Question(
	QuestionID INTEGER IDENTITY(1,1) NOT NULL PRIMARY KEY,
	TestID INTEGER NOT NULL FOREIGN KEY REFERENCES Test(TestID),
	QuestionText VARCHAR(200) NOT NULL,
	Answer1 VARCHAR(100) NOT NULL,
	Answer2 VARCHAR(100) NOT NULL,
	Answer3 VARCHAR(100) NOT NULL,
	CorrectAnswer INT NOT NULL
);

-- Answer
CREATE TABLE Answer(
	AnswerID INTEGER IDENTITY(1,1) NOT NULL PRIMARY KEY,
	TestID INTEGER NOT NULL FOREIGN KEY REFERENCES Test(TestID),
	QuestionID INTEGER NOT NULL FOREIGN KEY REFERENCES Question(QuestionID),
	Username VARCHAR(50) NOT NULL FOREIGN KEY REFERENCES [User](Username),
	AttemptNumber INTEGER NOT NULL,
	UserAnswer INT NOT NULL,
	Correct BIT NOT NULL
);

-- Result
CREATE TABLE Result(
	ResultID INTEGER IDENTITY(1,1) NOT NULL PRIMARY KEY,
	TestID INTEGER FOREIGN KEY REFERENCES Test(TestID),
	Username VARCHAR(50) FOREIGN KEY REFERENCES [User](Username),
	AttemptNumber INTEGER NOT NULL,
	UserResult INT NOT NULL,
	ResultPercentage DECIMAL NOT NULL
);

-- Test Data
INSERT INTO [User] VALUES ('BroKyl1910', '$2a$10$EwUV1ZZeKvh4j1KvmDzXP.9b4lJ4eS5FYfFG42aW56Dro5Y687qKC', 'Kyle', 'Brooks', 1, '18003144');
INSERT INTO [User] VALUES ('WonSte1900', '$2a$10$EwUV1ZZeKvh4j1KvmDzXP.9b4lJ4eS5FYfFG42aW56Dro5Y687qKC', 'Stevie', 'Wonder', 0, '18003143');
INSERT INTO [User] VALUES ('test123', '$2a$10$EwUV1ZZeKvh4j1KvmDzXP.9b4lJ4eS5FYfFG42aW56Dro5Y687qKC', 'Test', 'Student', 0, '18005484');
INSERT INTO [User] VALUES ('testL123', '$2a$10$EwUV1ZZeKvh4j1KvmDzXP.9b4lJ4eS5FYfFG42aW56Dro5Y687qKC', 'Test', 'Lecturer', 1, '18045484');
INSERT INTO [User] VALUES ('testBind', '$2a$10$EwUV1ZZeKvh4j1KvmDzXP.9b4lJ4eS5FYfFG42aW56Dro5Y687qKC', 'Test', 'Lecturer', 1, '18045488');
DELETE FROM [User] WHERE Username = 'testBind';
INSERT INTO Course VALUES('BCAD2', 'Bachelor of Computer Information Sciences in Application Development');
INSERT INTO Course VALUES('BEdFP', 'Bachelor of Education Foundation Phase');
INSERT INTO Module VALUES('PROG6212', 'Programming 2B');
INSERT INTO Module VALUES('CLDV6212', 'Cloud Development 2B');
INSERT INTO ModuleCourse VALUES('PROG6212','BCAD2');
INSERT INTO ModuleCourse VALUES('CLDV6212','BCAD2');
INSERT INTO LecturerAssignment VALUES('BroKyl1910','PROG6212');
INSERT INTO LecturerAssignment VALUES('testL123','PROG6212');
INSERT INTO StudentAssignment VALUES('WonSte1900','BCAD2');
INSERT INTO StudentAssignment VALUES('test123','BCAD2');

DBCC CHECKIDENT ('[Test]', RESEED, 1);
DBCC CHECKIDENT ('[Question]', RESEED, 1);
GO
INSERT [dbo].[Test] ([Username], [ModuleID], [Title], [DueDate]) VALUES (N'BroKyl1910', N'PROG6212', N'Test 1', CAST(0x0000AACB00000000 AS DateTime))
INSERT [dbo].[Question] ([TestID], [QuestionText], [Answer1], [Answer2], [Answer3], [CorrectAnswer]) VALUES (1, N'Question 1?', N'Yes', N'Ya', N'Yah', 2)
INSERT [dbo].[Question] ([TestID], [QuestionText], [Answer1], [Answer2], [Answer3], [CorrectAnswer]) VALUES (1, N'Grand Ole Opry', N'Rascal Flatts', N'Brought to you by American Idol', N'God bless ''Murica', 1)
INSERT [dbo].[Question] ([TestID], [QuestionText], [Answer1], [Answer2], [Answer3], [CorrectAnswer]) VALUES (1, N'What is my name?', N'JC', N'Brett', N'Kyle', 2)
INSERT [dbo].[Question] ([TestID], [QuestionText], [Answer1], [Answer2], [Answer3], [CorrectAnswer]) VALUES (1, N'How many times have I remade this damn test??', N'3', N'A lot (21 Savage ft. J Cole)', N'Too many', 1)

SELECT * FROM [Test]
SELECT * FROM [Question]

INSERT INTO LecturerAssignment VALUES('brookyl1910','PROG6212');
select * from [user]
select * from [StudentAssignment]
select * from [LecturerAssignment]

delete from StudentAssignment where Username = 'brookyl1910'
delete from [LecturerAssignment] where Username = 'brookyl1910'
delete from [user] where Username = 'brookyl1910'

