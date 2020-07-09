
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 07/09/2020 22:04:20
-- Generated from EDMX file: \\mac\Home\Desktop\projects\2019-2020\C#\XsollaSummerSchoolTest\XsollaSummerSchoolTest\NewsDataModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [XsollaSummerSchoolTest];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------


-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[NewsItemSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[NewsItemSet];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'NewsItemSet'
CREATE TABLE [dbo].[NewsItemSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Headline] nvarchar(max)  NOT NULL,
    [Body] nvarchar(max)  NOT NULL,
    [RateSum] smallint  NOT NULL,
    [RateCount] smallint  NOT NULL,
    [Category] nvarchar(max)  NOT NULL
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

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------