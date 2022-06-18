
CREATE PROCEDURE [dbo].[GetById]
@Id int =0

AS
BEGIN
	select * from Employee
	where Id = @Id
END
GO
