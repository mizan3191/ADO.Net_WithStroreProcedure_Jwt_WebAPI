
CREATE PROCEDURE [dbo].[Update_Employee]
@Id int = 0,
@Name nvarchar(70) = null,
@Address nvarchar(250) =   null

AS
BEGIN
	update Employee 
	set Name = @Name, Address = @Address
	where Id = @Id

END
GO
