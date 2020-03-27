CREATE TABLE [dbo].[EmployeeDetails] (
    [Id]          INT           IDENTITY (1, 1) NOT NULL,
    [EmployeeId]  INT           NULL,
    [Description] NVARCHAR (50) NULL,
    [CreatedBy]   INT           NOT NULL,
    [CreatedDate] DATETIME      NOT NULL,
    CONSTRAINT [PK_EmployeeDetails] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_EmployeeDetails_Employees] FOREIGN KEY ([EmployeeId]) REFERENCES [dbo].[Employees] ([Id]) ON DELETE CASCADE
);

