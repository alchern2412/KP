
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 05/18/2019 16:07:21
-- Generated from EDMX file: E:\4Sem\ООП\Курсовой\KP\KP\Hostel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [KPTESTDB];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_StudentRoom]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Students] DROP CONSTRAINT [FK_StudentRoom];
GO
IF OBJECT_ID(N'[dbo].[FK_StudentFaculty]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Students] DROP CONSTRAINT [FK_StudentFaculty];
GO
IF OBJECT_ID(N'[dbo].[FK_FloorRoom]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Rooms] DROP CONSTRAINT [FK_FloorRoom];
GO
IF OBJECT_ID(N'[dbo].[FK_StudentDutyWatch]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DutyFloorWatches] DROP CONSTRAINT [FK_StudentDutyWatch];
GO
IF OBJECT_ID(N'[dbo].[FK_StudentStudSovietMember]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[StudSovietMembers] DROP CONSTRAINT [FK_StudentStudSovietMember];
GO
IF OBJECT_ID(N'[dbo].[FK_StudSovietMemberStudSovietPosition]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[StudSovietMembers] DROP CONSTRAINT [FK_StudSovietMemberStudSovietPosition];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Students]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Students];
GO
IF OBJECT_ID(N'[dbo].[Rooms]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Rooms];
GO
IF OBJECT_ID(N'[dbo].[Faculties]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Faculties];
GO
IF OBJECT_ID(N'[dbo].[Floors]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Floors];
GO
IF OBJECT_ID(N'[dbo].[StudSovietMembers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[StudSovietMembers];
GO
IF OBJECT_ID(N'[dbo].[DutyFloorWatches]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DutyFloorWatches];
GO
IF OBJECT_ID(N'[dbo].[StudSovietPositions]', 'U') IS NOT NULL
    DROP TABLE [dbo].[StudSovietPositions];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Students'
CREATE TABLE [dbo].[Students] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [FirstName] nvarchar(max)  NOT NULL,
    [LastName] nvarchar(max)  NOT NULL,
    [SecondName] nvarchar(max)  NULL,
    [Note] nvarchar(max)  NULL,
    [Course] int  NULL,
    [Group] int  NULL,
    [Birthday] datetime  NOT NULL,
    [DateOfEntry] datetime  NULL,
    [DateOfDeparture] datetime  NULL,
    [Photo] varbinary(max)  NULL,
    [Room_Id] int  NOT NULL,
    [Faculty_Id] int  NOT NULL
);
GO

-- Creating table 'Rooms'
CREATE TABLE [dbo].[Rooms] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Number] int  NOT NULL,
    [Bed] int  NULL,
    [Nightstand] int  NULL,
    [Chair] int  NULL,
    [Floor_Id] int  NOT NULL
);
GO

-- Creating table 'Faculties'
CREATE TABLE [dbo].[Faculties] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [FacultyName] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Floors'
CREATE TABLE [dbo].[Floors] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Number] int  NOT NULL
);
GO

-- Creating table 'StudSovietMembers'
CREATE TABLE [dbo].[StudSovietMembers] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [DateOfEntry] datetime  NULL,
    [Student_Id] int  NOT NULL,
    [StudSovietPosition_Id] int  NOT NULL
);
GO

-- Creating table 'DutyFloorWatches'
CREATE TABLE [dbo].[DutyFloorWatches] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Date] datetime  NOT NULL,
    [TimeStart] time  NOT NULL,
    [TimeFinish] time  NOT NULL,
    [Type] nvarchar(max)  NOT NULL,
    [Student_Id] int  NOT NULL
);
GO

-- Creating table 'StudSovietPositions'
CREATE TABLE [dbo].[StudSovietPositions] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Position] nvarchar(max)  NOT NULL,
    [Description] nvarchar(max)  NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'Students'
ALTER TABLE [dbo].[Students]
ADD CONSTRAINT [PK_Students]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Rooms'
ALTER TABLE [dbo].[Rooms]
ADD CONSTRAINT [PK_Rooms]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Faculties'
ALTER TABLE [dbo].[Faculties]
ADD CONSTRAINT [PK_Faculties]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Floors'
ALTER TABLE [dbo].[Floors]
ADD CONSTRAINT [PK_Floors]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'StudSovietMembers'
ALTER TABLE [dbo].[StudSovietMembers]
ADD CONSTRAINT [PK_StudSovietMembers]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'DutyFloorWatches'
ALTER TABLE [dbo].[DutyFloorWatches]
ADD CONSTRAINT [PK_DutyFloorWatches]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'StudSovietPositions'
ALTER TABLE [dbo].[StudSovietPositions]
ADD CONSTRAINT [PK_StudSovietPositions]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [Room_Id] in table 'Students'
ALTER TABLE [dbo].[Students]
ADD CONSTRAINT [FK_StudentRoom]
    FOREIGN KEY ([Room_Id])
    REFERENCES [dbo].[Rooms]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_StudentRoom'
CREATE INDEX [IX_FK_StudentRoom]
ON [dbo].[Students]
    ([Room_Id]);
GO

-- Creating foreign key on [Faculty_Id] in table 'Students'
ALTER TABLE [dbo].[Students]
ADD CONSTRAINT [FK_StudentFaculty]
    FOREIGN KEY ([Faculty_Id])
    REFERENCES [dbo].[Faculties]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_StudentFaculty'
CREATE INDEX [IX_FK_StudentFaculty]
ON [dbo].[Students]
    ([Faculty_Id]);
GO

-- Creating foreign key on [Floor_Id] in table 'Rooms'
ALTER TABLE [dbo].[Rooms]
ADD CONSTRAINT [FK_FloorRoom]
    FOREIGN KEY ([Floor_Id])
    REFERENCES [dbo].[Floors]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_FloorRoom'
CREATE INDEX [IX_FK_FloorRoom]
ON [dbo].[Rooms]
    ([Floor_Id]);
GO

-- Creating foreign key on [Student_Id] in table 'DutyFloorWatches'
ALTER TABLE [dbo].[DutyFloorWatches]
ADD CONSTRAINT [FK_StudentDutyWatch]
    FOREIGN KEY ([Student_Id])
    REFERENCES [dbo].[Students]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_StudentDutyWatch'
CREATE INDEX [IX_FK_StudentDutyWatch]
ON [dbo].[DutyFloorWatches]
    ([Student_Id]);
GO

-- Creating foreign key on [Student_Id] in table 'StudSovietMembers'
ALTER TABLE [dbo].[StudSovietMembers]
ADD CONSTRAINT [FK_StudentStudSovietMember]
    FOREIGN KEY ([Student_Id])
    REFERENCES [dbo].[Students]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_StudentStudSovietMember'
CREATE INDEX [IX_FK_StudentStudSovietMember]
ON [dbo].[StudSovietMembers]
    ([Student_Id]);
GO

-- Creating foreign key on [StudSovietPosition_Id] in table 'StudSovietMembers'
ALTER TABLE [dbo].[StudSovietMembers]
ADD CONSTRAINT [FK_StudSovietMemberStudSovietPosition]
    FOREIGN KEY ([StudSovietPosition_Id])
    REFERENCES [dbo].[StudSovietPositions]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_StudSovietMemberStudSovietPosition'
CREATE INDEX [IX_FK_StudSovietMemberStudSovietPosition]
ON [dbo].[StudSovietMembers]
    ([StudSovietPosition_Id]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------