CREATE TABLE [dbo].[Employees] (
    [Id]          INT          IDENTITY (1, 1) NOT NULL,
    [Name]        VARCHAR (50) NULL,
    [CreatedBy]   INT          NOT NULL,
    [CreatedDate] DATETIME     NOT NULL,
    CONSTRAINT [PK__Employee__3214EC07D3E50C6C] PRIMARY KEY CLUSTERED ([Id] ASC)
);



