
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 07/19/2020 23:19:11
-- Generated from EDMX file: \\mac\Home\Desktop\projects\2019-2020\C#\XsollaSummerSchoolTest\XsollaSummerSchoolTest\NewsDataModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [NewsDb];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_NewsItemRate]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[RateSet] DROP CONSTRAINT [FK_NewsItemRate];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[NewsItemSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[NewsItemSet];
GO
IF OBJECT_ID(N'[dbo].[RateSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[RateSet];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'NewsItemSet'
CREATE TABLE [dbo].[NewsItemSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Headline] nvarchar(max)  NOT NULL,
    [Body] nvarchar(max)  NOT NULL,
    [Category] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'RateSet'
CREATE TABLE [dbo].[RateSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [SessionString] nvarchar(max)  NOT NULL,
    [Mark] smallint  NOT NULL,
    [NewsItem_Id] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'NewsItemSet'
ALTER TABLE [dbo].[NewsItemSet]
ADD CONSTRAINT [PK_NewsItemSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'RateSet'
ALTER TABLE [dbo].[RateSet]
ADD CONSTRAINT [PK_RateSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [NewsItem_Id] in table 'RateSet'
ALTER TABLE [dbo].[RateSet]
ADD CONSTRAINT [FK_NewsItemRate]
    FOREIGN KEY ([NewsItem_Id])
    REFERENCES [dbo].[NewsItemSet]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_NewsItemRate'
CREATE INDEX [IX_FK_NewsItemRate]
ON [dbo].[RateSet]
    ([NewsItem_Id]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------