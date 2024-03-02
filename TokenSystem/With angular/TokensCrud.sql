go
Create procedure GetTokenById
	@id int
as
begin
SELECT [Id]
      ,[QueryId]
      ,[StatusId]   
      ,[Phone]      
      ,[Address]    
      ,[CreatedOn] 
      ,[UpdatedOn] 
      ,[DeletedOn] 
	  ,[IsDeleted] 
  FROM [dbo].[Tokens]
  where [IsDeleted] = 0
  and [Id] =@id
end

go

create or alter procedure CreateToken
	@QueryId as int,
	@StatusId as int,
	@Phone as varchar(20) = null,
	@Address as varchar(20) = null
as
begin
set nocount on;
INSERT INTO [dbo].[Tokens]
           ([QueryId]
           ,[StatusId]
           ,[Phone]
           ,[Address])
     VALUES
           (@QueryId 
           ,@StatusId
           ,@Phone
           ,@Address)

select SCOPE_IDENTITY()

end
GO


create or alter procedure UpdateToken
	@Id as int,
	@QueryId as int,
	@StatusId as int,
	@Phone as varchar(20) = null,
	@Address as varchar(20) = null
as
begin
set nocount on;
	update [dbo].[Tokens]
    set [QueryId] = @QueryId
           ,[StatusId] = @StatusId
           ,[Phone] = @Phone
           ,[Address] = @Address
		   , UpdatedOn =  GETDATE()
     where Id = @Id and IsDeleted = 0
end
GO

create procedure DeleteToken
	@Id as int
as
begin
	Update [dbo].[Tokens]
	set IsDeleted = 1,
		DeletedOn =  GETDATE()
	where Id = @Id
end
GO


go
Create or alter procedure GetUnresolvedTokens
	@pageSize int,
	@pageNo int
as
begin
set nocount on;
set @pageNo = @pageNo -1 

SELECT [Id]
      ,[QueryId]
      ,[StatusId]   
      ,[Phone]      
      ,[Address]    
      ,[CreatedOn] 
      ,[UpdatedOn] 
      ,[DeletedOn] 
	  ,[IsDeleted] 
  FROM [dbo].[Tokens]
  where [IsDeleted] = 0
  and [StatusId] != 3
  order by StatusId desc, CreatedOn
  offset @pageSize*@pageNo rows
  fetch next @pageSize rows only

SELECT count(1) as TotalRecords 
  FROM [dbo].[Tokens]
  where [IsDeleted] = 0
  and [StatusId] != 3
end

go


exec GetUnresolvedTokens @pageSize = 5,@pageNo = 1
go 
declare @QueryId as int = 1
declare	@StatusId as int = 1
exec CreateToken @QueryId = @QueryId,@StatusId= @StatusId

