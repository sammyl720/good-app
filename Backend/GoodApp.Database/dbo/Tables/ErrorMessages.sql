CREATE TABLE [dbo].[ErrorMessages] (
    [ErrorMessageId] UNIQUEIDENTIFIER NOT NULL,
    [Request]        NVARCHAR (MAX)   NULL,
    [Exception]      NVARCHAR (MAX)   NULL,
    [EventDate]      DATETIME         NOT NULL,
    CONSTRAINT [PK_dbo.ErrorMessages] PRIMARY KEY CLUSTERED ([ErrorMessageId] ASC)
);

