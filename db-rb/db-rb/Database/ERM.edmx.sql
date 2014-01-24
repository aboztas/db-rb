
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 01/24/2014 14:15:49
-- Generated from EDMX file: E:\02_c#_workspace\db-rb\db-rb\db-rb\Database\ERM.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [Database];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------


-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------


-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Reservation'
CREATE TABLE [dbo].[Reservation] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Code] nvarchar(max)  NOT NULL,
    [Date] datetime  NOT NULL,
    [Flight_Id] int  NOT NULL
);
GO

-- Creating table 'Passenger'
CREATE TABLE [dbo].[Passenger] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Surname] nvarchar(max)  NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Birthdate] datetime  NOT NULL,
    [Sex] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Flight'
CREATE TABLE [dbo].[Flight] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Code] nvarchar(max)  NOT NULL,
    [Date] datetime  NOT NULL,
    [Company] nvarchar(max)  NOT NULL,
    [Arrival_Id] int  NOT NULL,
    [Departure_Id] int  NOT NULL
);
GO

-- Creating table 'Airport'
CREATE TABLE [dbo].[Airport] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Code] nvarchar(max)  NOT NULL,
    [Name] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'ReservationPassenger'
CREATE TABLE [dbo].[ReservationPassenger] (
    [Reservations_Id] int  NOT NULL,
    [Passenger_Id] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'Reservation'
ALTER TABLE [dbo].[Reservation]
ADD CONSTRAINT [PK_Reservation]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Passenger'
ALTER TABLE [dbo].[Passenger]
ADD CONSTRAINT [PK_Passenger]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Flight'
ALTER TABLE [dbo].[Flight]
ADD CONSTRAINT [PK_Flight]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Airport'
ALTER TABLE [dbo].[Airport]
ADD CONSTRAINT [PK_Airport]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Reservations_Id], [Passenger_Id] in table 'ReservationPassenger'
ALTER TABLE [dbo].[ReservationPassenger]
ADD CONSTRAINT [PK_ReservationPassenger]
    PRIMARY KEY CLUSTERED ([Reservations_Id], [Passenger_Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [Arrival_Id] in table 'Flight'
ALTER TABLE [dbo].[Flight]
ADD CONSTRAINT [FK_FlightAirport]
    FOREIGN KEY ([Arrival_Id])
    REFERENCES [dbo].[Airport]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_FlightAirport'
CREATE INDEX [IX_FK_FlightAirport]
ON [dbo].[Flight]
    ([Arrival_Id]);
GO

-- Creating foreign key on [Departure_Id] in table 'Flight'
ALTER TABLE [dbo].[Flight]
ADD CONSTRAINT [FK_AirportFlight]
    FOREIGN KEY ([Departure_Id])
    REFERENCES [dbo].[Airport]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_AirportFlight'
CREATE INDEX [IX_FK_AirportFlight]
ON [dbo].[Flight]
    ([Departure_Id]);
GO

-- Creating foreign key on [Flight_Id] in table 'Reservation'
ALTER TABLE [dbo].[Reservation]
ADD CONSTRAINT [FK_ReservationFlight]
    FOREIGN KEY ([Flight_Id])
    REFERENCES [dbo].[Flight]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ReservationFlight'
CREATE INDEX [IX_FK_ReservationFlight]
ON [dbo].[Reservation]
    ([Flight_Id]);
GO

-- Creating foreign key on [Reservations_Id] in table 'ReservationPassenger'
ALTER TABLE [dbo].[ReservationPassenger]
ADD CONSTRAINT [FK_ReservationPassenger_Reservation]
    FOREIGN KEY ([Reservations_Id])
    REFERENCES [dbo].[Reservation]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Passenger_Id] in table 'ReservationPassenger'
ALTER TABLE [dbo].[ReservationPassenger]
ADD CONSTRAINT [FK_ReservationPassenger_Passenger]
    FOREIGN KEY ([Passenger_Id])
    REFERENCES [dbo].[Passenger]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ReservationPassenger_Passenger'
CREATE INDEX [IX_FK_ReservationPassenger_Passenger]
ON [dbo].[ReservationPassenger]
    ([Passenger_Id]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------