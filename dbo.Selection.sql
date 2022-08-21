CREATE TABLE [dbo].[Selection] (
    [Rno]     VARCHAR (50)  NOT NULL,
    [Cno]     INT           NOT NULL,
    [Section] VARCHAR(50)      DEFAULT "A" NOT NULL,
    [Name]    VARCHAR (MAX) NOT NULL,
    [Gender]  VARCHAR (50)  NOT NULL,
    [Age]     INT           NOT NULL,
    [Avatar]  VARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_Selection] PRIMARY KEY CLUSTERED ([Rno] ASC)
);

