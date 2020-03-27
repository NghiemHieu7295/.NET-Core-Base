-- =============================================
-- Author:		CuongND
-- Create date: 2020-03-19
-- Description:	Insert and update EmployeeDetail
-- =============================================
CREATE PROCEDURE [dbo].[sp_UpsertEmployeeDetail] 
	-- Add the parameters for the stored procedure here
	@Id int null, 
	@EmployeeId int,
	@Description nvarchar(255),
	@CreatedBy int,
	@CreatedDate datetime
AS
BEGIN
    MERGE EmployeeDetails AS T
		USING 
		(
			SELECT 
					@Id				AS Id,
					@EmployeeId				AS EmployeeId,
					@Description			AS [Description],
					@CreatedBy				AS CreatedBy,
					@CreatedDate				AS CreatedDate
		 )  S
		ON S.Id  = T.Id and S.EmployeeId = T.EmployeeId
		WHEN MATCHED
			  THEN UPDATE SET  
				T.Description			= S.Description
		WHEN NOT MATCHED
			THEN  INSERT (
				EmployeeId,
				[Description],
				CreatedBy,
				CreatedDate
				)
			VALUES (
				S.EmployeeId,
				S.Description,
				S.CreatedBy,
				S.CreatedDate
				);
END