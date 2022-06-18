
CREATE PROCEDURE [dbo].[Detele]
@id int = 0

AS
BEGIN
	delete from Employee
	where Id= @id
END
GO
