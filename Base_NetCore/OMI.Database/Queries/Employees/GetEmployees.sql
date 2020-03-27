SELECT E.Id, E.Name, E.CreatedBy, E.CreatedDate,
				D.Id, D.EmployeeId, D.Description, D.CreatedBy, D.CreatedDate
FROM [dbo].[Employees] E
INNER JOIN [dbo].[EmployeeDetails] D on E.Id = D.EmployeeId
where E.[Name] like '%' + @pattern + '%';
