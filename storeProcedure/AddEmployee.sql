
CREATE PROCEDURE [dbo].[Add_Employee] 
@Name nvarchar(70) = null,
@Address nvarchar(250) = null
AS
BEGIN
	Insert into Employee(Name, Address)
	values(@Name, @Address)
	select SCOPE_IDENTITY()
END
GO
