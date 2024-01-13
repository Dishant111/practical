CREATE TABLE Designation (
    Id INT PRIMARY KEY identity(1,1),
    CreatedOn DATETIME,
    UpdatedOn DATETIME,
    Name NVARCHAR(50) not null
);

CREATE TABLE EmployeeMaster (
    Id INT PRIMARY KEY identity(1,1),
    CreatedOn DATETIME default(getdate()),
    UpdatedOn DATETIME null,
    DeletedOn DATETIME null,
    IsDeleted BIT default(0),
    Name NVARCHAR(50) not null,
    Salary DECIMAL(18, 2) default(0.0),
    Designation_Id INT foreign key references Designation(id)
);
Go 

CREATE or Alter PROCEDURE CreateEmployee
    @Name NVARCHAR(50),
    @Salary DECIMAL(18, 2),
    @Designation_Id INT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO EmployeeMaster (CreatedOn, Name, Salary, Designation_Id)
    VALUES (GETDATE(), @Name, @Salary, @Designation_Id);
    select SCOPE_IDENTITY();
END;

go 

CREATE or Alter PROCEDURE UpdateEmployee
    @Id INT,
    @Name NVARCHAR(50),
    @Salary DECIMAL(18, 2),
    @Designation_Id INT
AS
BEGIN
    UPDATE EmployeeMaster
    SET UpdatedOn = GETDATE(),
        Name = @Name,
        Salary = @Salary,
        Designation_Id = @Designation_Id
    WHERE Id = @Id AND IsDeleted = 0;
END;

go 

CREATE or Alter PROCEDURE DeleteEmployee
    @Id INT
AS
BEGIN
    UPDATE EmployeeMaster
    SET DeletedOn = GETDATE(),
        IsDeleted = 1
    WHERE Id = @Id AND IsDeleted = 0;
END;

go

CREATE or Alter PROCEDURE GetEmployeeListing
AS
BEGIN
    SELECT
        E.Id AS EmployeeId,
        E.Name AS EmployeeName,
        E.Salary,
		d.Id as DesignationId,
        D.Name AS Designation
    FROM
        EmployeeMaster E
    INNER JOIN
        Designation D ON E.Designation_Id = D.Id
    WHERE
        E.IsDeleted = 0;
END;

go

CREATE or Alter PROCEDURE GetEmployeeById
    @EmployeeId INT
AS
BEGIN
    SELECT
        E.Id AS EmployeeId,
        E.Name AS EmployeeName,
        E.Salary,
		d.Id as DesignationId,
        D.Name AS Designation
    FROM
        EmployeeMaster E
    INNER JOIN
        Designation D ON E.Designation_Id = D.Id
    WHERE
        E.Id = @EmployeeId AND
        E.IsDeleted = 0; 
END;
--_________intial Dumy Data____

INSERT INTO Designation (CreatedOn, UpdatedOn, Name)
VALUES 
    (GETDATE(), GETDATE(), 'HR'),
    (GETDATE(), GETDATE(), 'Software Engineer');

-- Insert data for HR
INSERT INTO EmployeeMaster (CreatedOn, UpdatedOn, IsDeleted, Name, Salary, Designation_Id)
SELECT 
    GETDATE(), 
    NULL, 
    0, 
    'Jane Smith', 
    55000.00, 
    Id
FROM 
    Designation
WHERE 
    Name = 'HR';

-- Insert data for Software Engineer
INSERT INTO EmployeeMaster (CreatedOn, UpdatedOn, IsDeleted, Name, Salary, Designation_Id)
SELECT 
    GETDATE(), 
    NULL, 
    0, 
    'Bob Johnson', 
    65000.00, 
    Id
FROM 
    Designation
WHERE 
    Name = 'Software Engineer';
go 

CREATE or Alter PROCEDURE GetAllDesignations
AS
BEGIN
    SELECT Id, CreatedOn, UpdatedOn, Name
    FROM Designation;
END;
go