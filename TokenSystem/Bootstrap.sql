IF OBJECT_ID(N'dbo.QueryType', N'U') IS NULL
begin
create table QueryType
(
Id int identity(1,1) primary key,
Query varchar(50) not null
)
end

if  not exists(select top 1 id from QueryType) 
begin

truncate table QueryType

insert into QueryType
(Query)
values
('General'),
('New Product'),
('New Product'),
('Others')

end

IF OBJECT_ID(N'dbo.QueryStatus', N'U') IS NULL
begin
create table QueryStatus
(
Id int identity(1,1) primary key,
Status varchar(50) not null
)
end

if  not exists(select top 1 id from QueryStatus) 
begin

truncate table QueryStatus

insert into QueryStatus
(Status)
values
('Pending'),
('Processing'),
('Resolved')

end


IF OBJECT_ID(N'dbo.Tokens', N'U') IS NULL
begin
Create table Tokens
(
Id int identity(1,1) primary key,
QueryId int not null foreign key references QueryType(Id),
StatusId int not null default(0) foreign key references QueryStatus(Id),
Phone varchar(20) null,
Address varchar(200) null,
CreatedOn datetime2 default(getdate()),
UpdatedOn datetime2 null,
DeletedOn datetime2 null,
IsDeleted bit default(0)
)
end
go

IF OBJECT_ID(N'dbo.Employee', N'U') IS NULL
begin
Create table Employees
(
Id int identity(1,1) primary key,
QueryId int not null foreign key references QueryType(Id),
UserName varchar(20) not null,
Password varchar(500) not null,
CreatedOn datetime2 default(getdate()),
UpdatedOn datetime2 null,
DeletedOn datetime2 null,
IsDeleted bit default(0)
)
end
go