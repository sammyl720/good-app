CREATE TABLE [dbo].[EmailTemplates] (
    [EmailTemplateId] UNIQUEIDENTIFIER NOT NULL,
    [FromEmail]       NVARCHAR (MAX)   NULL,
    [Type]            NVARCHAR (MAX)   NULL,
    [Title]           NVARCHAR (MAX)   NULL,
    [Content]         NVARCHAR (MAX)   NULL,
    CONSTRAINT [PK_dbo.EmailTemplates] PRIMARY KEY CLUSTERED ([EmailTemplateId] ASC)
);

