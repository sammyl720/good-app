CREATE TABLE [dbo].[Applications] (
    [ApplicationId] UNIQUEIDENTIFIER NOT NULL,
    [Name]          NVARCHAR (MAX)   NOT NULL,
    [Token]         NVARCHAR (MAX)   NOT NULL,
    [Status]        NVARCHAR (MAX)   NOT NULL,
    [Type]          NVARCHAR (MAX)   NOT NULL,
    [CreateDate]    DATETIME         NOT NULL,
    [AllowedOrigin] NVARCHAR (MAX)   NOT NULL,
    CONSTRAINT [PK_dbo.Applications] PRIMARY KEY CLUSTERED ([ApplicationId] ASC)
);

