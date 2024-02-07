go
Create or alter procedure GetEmployeeCredentialByUserName
	                        @UserName as varchar(20) 
                        as
                        begin
                            
                      SELECT top 1 [Id]
							,[QueryId]
							,[UserName]
							,[Password]
							,[CreatedOn]
							,[UpdatedOn]
							,[DeletedOn]
							,[IsDeleted] 
                      FROM [dbo].[Employees]
                      WHERE UserName = @UserName
					  and IsDeleted = 0
                
                        end                    
                    
                GO
                        
                        Create procedure GetEmployeeById
	                        @Id as int 
                        as
                        begin
                            
                      SELECT [Id]
							,[QueryId]
							,[UserName]
							,[Password]
							,[CreatedOn]
							,[UpdatedOn]
							,[DeletedOn]
							,[IsDeleted] 
                      FROM [dbo].[Employees]
                      WHERE Id = @Id
                
                        end                    
                    
                GO
                
                    create or alter procedure CreateEmployees
	                        @QueryId as int 
							,@UserName as varchar(20) 
							,@Password as varchar(500)
                    as
                    begin
                    set nocount on;
                    
                        INSERT INTO [dbo].[Employees]
                               ([QueryId]
								,[UserName]
								,[Password])
                         VALUES
                               (@QueryId
								,@UserName
								,@Password)
                    
                   
                    select SCOPE_IDENTITY()
                    end
                    
                GO
                                    
                GO
                                    
                        create procedure DeleteEmployees
	                        @Id as int 
                        as
                        begin
	                        
                        DELETE FROM [dbo].[Employees]
	                    WHERE Id = @Id
                    
                        end
                        GO
                    
					Create or alter procedure GetUnresolvedTokensByQuryId
	                        @QueryId as int,
							@pageSize int,  
							@pageNo int  
                        as
                        begin
						set nocount on;  
						set @pageNo = @pageNo -1                               
                      SELECT  [Id]
							,[QueryId]
							,[StatusId]
							,[Phone]
							,[Address]
							,[CreatedOn]
							,[UpdatedOn]
							,[DeletedOn]
							,[IsDeleted] 
                      FROM [dbo].[Tokens]
                      WHERE QueryId = @QueryId
					  and IsDeleted = 0
					  and [StatusId] != 3  
					  order by StatusId desc, CreatedOn  
					  offset @pageSize*@pageNo rows  
					  fetch next @pageSize rows only  

					  SELECT count(1) as TotalRecords   
					  FROM [dbo].[Tokens]  
					  where QueryId = @QueryId
					  and [IsDeleted] = 0  
					  and [StatusId] != 3  
					end                    



exec GetUnresolvedTokensByQuryId @QueryId = 2, @pageSize= 50,@pageNo =1

  