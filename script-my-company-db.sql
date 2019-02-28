USE master
GO

CREATE DATABASE MyCompany
GO

USE MyCompany
GO


CREATE TABLE dbo.Employees
(
	ID INT IDENTITY(1,1) CONSTRAINT PK_Employees PRIMARY KEY,
	FirstName VARCHAR(100) NOT NULL,
	LastName VARCHAR(100) NOT NULL,
	Job VARCHAR(100) NOT NULL,
	Salary DECIMAL NOT NULL
);
GO

INSERT INTO dbo.Employees(FirstName,LastName,Job,Salary)
VALUES
('Michael','Hawk','Penetration Tester',9000),
('Rara','Anjani','QA Manager',10000),
('Rahman','Hendrawan','CTO',20000);
GO

select * from dbo.Employees
