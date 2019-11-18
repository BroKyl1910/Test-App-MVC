
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
	Published BIT Default(1) NOT NULL,
	PublishDate DATETIME
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
	ResultPercentage DECIMAL NOT NULL,
	ResultDate DATETIME
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

-- Creating Student Users
INSERT INTO [User] VALUES ('RedOti1900', '$2a$10$EwUV1ZZeKvh4j1KvmDzXP.9b4lJ4eS5FYfFG42aW56Dro5Y687qKC', 'Otis', 'Redding', 0, '1800DOCK');
INSERT INTO StudentAssignment VALUES('RedOti1900','BCAD2'); -- DONE

INSERT INTO [User] VALUES ('SheeEd1800', '$2a$10$EwUV1ZZeKvh4j1KvmDzXP.9b4lJ4eS5FYfFG42aW56Dro5Y687qKC', 'Ed', 'Sheeran', 0, '1800SING');
INSERT INTO StudentAssignment VALUES('SheeEd1800','BCAD2');

INSERT INTO [User] VALUES ('JoyVan1800', '$2a$10$EwUV1ZZeKvh4j1KvmDzXP.9b4lJ4eS5FYfFG42aW56Dro5Y687qKC', 'Vance', 'Joy', 0, '1800RIPT');
INSERT INTO StudentAssignment VALUES('JoyVan1800','BCAD2');

select * from [User]

-- Creating Tests
INSERT [dbo].[Test] ([Username], [ModuleID], [Title], [DueDate]) VALUES (N'BroKyl1910', N'PROG6212', N'Test 1', CAST(0x0000AACB00000000 AS DateTime))
INSERT [dbo].[Question] ([TestID], [QuestionText], [Answer1], [Answer2], [Answer3], [CorrectAnswer]) VALUES (1, N'Question 1?', N'Yes', N'Ya', N'Yah', 2)
INSERT [dbo].[Question] ([TestID], [QuestionText], [Answer1], [Answer2], [Answer3], [CorrectAnswer]) VALUES (1, N'Grand Ole Opry', N'Rascal Flatts', N'Brought to you by American Idol', N'God bless ''Murica', 1)
INSERT [dbo].[Question] ([TestID], [QuestionText], [Answer1], [Answer2], [Answer3], [CorrectAnswer]) VALUES (1, N'What is my name?', N'JC', N'Brett', N'Kyle', 2)
INSERT [dbo].[Question] ([TestID], [QuestionText], [Answer1], [Answer2], [Answer3], [CorrectAnswer]) VALUES (1, N'How many times have I remade this damn test??', N'3', N'A lot (21 Savage ft. J Cole)', N'Too many', 1)

INSERT [dbo].[Test] ([Username], [ModuleID], [Title], [DueDate], [PublishDate]) VALUES (N'BroKyl1910', N'PROG6212', N'Test 2', '2019-08-17', '2019-07-17')
SELECT * FROM [Test]
INSERT [dbo].[Question] ([TestID], [QuestionText], [Answer1], [Answer2], [Answer3], [CorrectAnswer]) VALUES (2, N'Question 1?', N'Yes', N'Ya', N'Yah', 2)
INSERT [dbo].[Question] ([TestID], [QuestionText], [Answer1], [Answer2], [Answer3], [CorrectAnswer]) VALUES (2, N'Grand Ole Opry', N'Rascal Flatts', N'Brought to you by American Idol', N'God bless ''Murica', 1)
INSERT [dbo].[Question] ([TestID], [QuestionText], [Answer1], [Answer2], [Answer3], [CorrectAnswer]) VALUES (2, N'What is my name?', N'JC', N'Brett', N'Kyle', 2)
INSERT [dbo].[Question] ([TestID], [QuestionText], [Answer1], [Answer2], [Answer3], [CorrectAnswer]) VALUES (2, N'How many times have I remade this damn test??', N'3', N'A lot (21 Savage ft. J Cole)', N'Too many', 1)

INSERT [dbo].[Test] ([Username], [ModuleID], [Title], [DueDate], [PublishDate]) VALUES (N'BroKyl1910', N'PROG6212', N'Test 3', '2019-08-17', '2019-07-17')
SELECT * FROM [Test]
INSERT [dbo].[Question] ([TestID], [QuestionText], [Answer1], [Answer2], [Answer3], [CorrectAnswer]) VALUES (3, N'Question 1?', N'Yes', N'Ya', N'Yah', 2)
INSERT [dbo].[Question] ([TestID], [QuestionText], [Answer1], [Answer2], [Answer3], [CorrectAnswer]) VALUES (3, N'Grand Ole Opry', N'Rascal Flatts', N'Brought to you by American Idol', N'God bless ''Murica', 1)
INSERT [dbo].[Question] ([TestID], [QuestionText], [Answer1], [Answer2], [Answer3], [CorrectAnswer]) VALUES (3, N'What is my name?', N'JC', N'Brett', N'Kyle', 2)
INSERT [dbo].[Question] ([TestID], [QuestionText], [Answer1], [Answer2], [Answer3], [CorrectAnswer]) VALUES (3, N'How many times have I remade this damn test??', N'3', N'A lot (21 Savage ft. J Cole)', N'Too many', 1)


INSERT [dbo].[Test] ([Username], [ModuleID], [Title], [DueDate], [PublishDate]) VALUES (N'BroKyl1910', N'PROG6212', N'Test 4', '2019-08-17', '2019-07-17')
SELECT * FROM [Test]
INSERT [dbo].[Question] ([TestID], [QuestionText], [Answer1], [Answer2], [Answer3], [CorrectAnswer]) VALUES (4, N'Question 1?', N'Yes', N'Ya', N'Yah', 2)
INSERT [dbo].[Question] ([TestID], [QuestionText], [Answer1], [Answer2], [Answer3], [CorrectAnswer]) VALUES (4, N'Grand Ole Opry', N'Rascal Flatts', N'Brought to you by American Idol', N'God bless ''Murica', 1)
INSERT [dbo].[Question] ([TestID], [QuestionText], [Answer1], [Answer2], [Answer3], [CorrectAnswer]) VALUES (4, N'What is my name?', N'JC', N'Brett', N'Kyle', 2)
INSERT [dbo].[Question] ([TestID], [QuestionText], [Answer1], [Answer2], [Answer3], [CorrectAnswer]) VALUES (4, N'How many times have I remade this damn test??', N'3', N'A lot (21 Savage ft. J Cole)', N'Too many', 1)

INSERT [dbo].[Test] ([Username], [ModuleID], [Title], [DueDate], [PublishDate]) VALUES (N'BroKyl1910', N'PROG6212', N'Test 5', '2019-08-17', '2019-07-17')
SELECT * FROM [Test]
INSERT [dbo].[Question] ([TestID], [QuestionText], [Answer1], [Answer2], [Answer3], [CorrectAnswer]) VALUES (5, N'Question 1?', N'Yes', N'Ya', N'Yah', 2)
INSERT [dbo].[Question] ([TestID], [QuestionText], [Answer1], [Answer2], [Answer3], [CorrectAnswer]) VALUES (5, N'Grand Ole Opry', N'Rascal Flatts', N'Brought to you by American Idol', N'God bless ''Murica', 1)
INSERT [dbo].[Question] ([TestID], [QuestionText], [Answer1], [Answer2], [Answer3], [CorrectAnswer]) VALUES (5, N'What is my name?', N'JC', N'Brett', N'Kyle', 2)
INSERT [dbo].[Question] ([TestID], [QuestionText], [Answer1], [Answer2], [Answer3], [CorrectAnswer]) VALUES (5, N'How many times have I remade this damn test??', N'3', N'A lot (21 Savage ft. J Cole)', N'Too many', 1)

INSERT [dbo].[Test] ([Username], [ModuleID], [Title], [DueDate], [PublishDate]) VALUES (N'BroKyl1910', N'CLDV6212', N'Test 1', '2019-08-17', '2019-07-17')
SELECT * FROM [Test]
INSERT [dbo].[Question] ([TestID], [QuestionText], [Answer1], [Answer2], [Answer3], [CorrectAnswer]) VALUES (6, N'Question 1?', N'Yes', N'Ya', N'Yah', 2)
INSERT [dbo].[Question] ([TestID], [QuestionText], [Answer1], [Answer2], [Answer3], [CorrectAnswer]) VALUES (6, N'Grand Ole Opry', N'Rascal Flatts', N'Brought to you by American Idol', N'God bless ''Murica', 1)
INSERT [dbo].[Question] ([TestID], [QuestionText], [Answer1], [Answer2], [Answer3], [CorrectAnswer]) VALUES (6, N'What is my name?', N'JC', N'Brett', N'Kyle', 2)
INSERT [dbo].[Question] ([TestID], [QuestionText], [Answer1], [Answer2], [Answer3], [CorrectAnswer]) VALUES (6, N'How many times have I remade this damn test??', N'3', N'A lot (21 Savage ft. J Cole)', N'Too many', 1)

SELECT * FROM [Test]
SELECT * FROM [Question]

INSERT INTO LecturerAssignment VALUES('brokyl1910','CLDV6212');
select * from [user]
select * from [StudentAssignment]
select * from [LecturerAssignment]

delete from StudentAssignment where Username = 'brookyl1910'
delete from [LecturerAssignment] where Username = 'brookyl1910'
delete from [user] where Username = 'brookyl1910'

update test set DueDate = '2019-09-18' where testID = 1
select * from result

select * from test
select * from result join test on result.TestID = test.testid

update result set ResultDate = dateadd(day, rand(checksum(newid()))*(1+datediff(day, '2019-08-17 00:00:00', '2019-09-17 00:00:00')),'2019-08-17 00:00:00')
where ResultDate is null

update result set ResultDate = '2019-09-06'
where ResultID = 12

select * from StudentAssignment join Course
on StudentAssignment.CourseID = Course.CourseID

select * from [user]

update test set published = 1 where TestID = 4

