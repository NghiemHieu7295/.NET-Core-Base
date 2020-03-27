-- =============================================
-- Author:		CuongND
-- Create date: 2020-03-19
-- Description:	Insert and update Employee
-- =============================================
CREATE PROCEDURE [dbo].[sp_UpsertEmployee] 
	-- Add the parameters for the stored procedure here
	@Id int null, 
	@Name nvarchar(50),
	@CreatedBy int,
	@CreatedDate datetime
AS
BEGIN
    MERGE Employees AS T
		USING 
		(
			SELECT 
					@Id				AS Id,
					@Name			AS [Name],
					@CreatedBy				AS CreatedBy,
					@CreatedDate				AS CreatedDate
		 )  S
		ON S.Id  = T.Id
		WHEN MATCHED
			  THEN UPDATE SET  
				T.Name			= S.Name
		WHEN NOT MATCHED
			THEN  INSERT (
				[Name],
				CreatedBy,
				CreatedDate
				)
			VALUES (
				S.Name,
				S.CreatedBy,
				S.CreatedDate
				)
		WHEN NOT MATCHED BY SOURCE
			THEN DELETE;
END