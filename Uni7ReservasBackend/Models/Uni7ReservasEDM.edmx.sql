
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 09/19/2018 10:02:57
-- Generated from EDMX file: C:\Users\NIP\source\repos\Uni7ReservasBackend\Uni7ReservasBackend\Models\Uni7ReservasEDM.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [UNI7RESERVAS];
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

-- Creating table 'Usuarios'
CREATE TABLE [dbo].[Usuarios] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Nome] nvarchar(max)  NOT NULL,
    [Email] nvarchar(max)  NOT NULL,
    [Senha] nvarchar(max)  NOT NULL,
    [Tipo] int  NOT NULL
);
GO

-- Creating table 'Reservas'
CREATE TABLE [dbo].[Reservas] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Data] datetime  NOT NULL,
    [ReservadoEm] datetime  NOT NULL,
    [Obs] nvarchar(max)  NOT NULL,
    [Horario] nvarchar(max)  NOT NULL,
    [Turno] nvarchar(max)  NOT NULL,
    [Usuario_Id] int  NOT NULL,
    [Local_Id] int  NOT NULL
);
GO

-- Creating table 'Locais'
CREATE TABLE [dbo].[Locais] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Nome] nvarchar(max)  NOT NULL,
    [Reservavel] bit  NOT NULL,
    [Disponivel] bit  NOT NULL,
    [Tipo] int  NOT NULL
);
GO

-- Creating table 'Equipamentos'
CREATE TABLE [dbo].[Equipamentos] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Modelo] nvarchar(max)  NOT NULL,
    [Disponivel] bit  NOT NULL,
    [CategoriaEquipamento_Id] int  NOT NULL
);
GO

-- Creating table 'Categorias'
CREATE TABLE [dbo].[Categorias] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Nome] nvarchar(max)  NOT NULL,
    [Reservas_Id] int  NOT NULL
);
GO

-- Creating table 'Controles'
CREATE TABLE [dbo].[Controles] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Abertura] datetime  NOT NULL,
    [Fechamento] datetime  NOT NULL,
    [ObsBolsista] nvarchar(max)  NOT NULL,
    [ObsUsuario] nvarchar(max)  NOT NULL,
    [Satisfacao] int  NOT NULL,
    [Bolsista_Id] int  NOT NULL,
    [Reserva_Id] int  NOT NULL
);
GO

-- Creating table 'Restricoes'
CREATE TABLE [dbo].[Restricoes] (
    [RestricoesLocais_Id] int  NOT NULL,
    [RestricoesCategoriaEquipamento_Id] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'Usuarios'
ALTER TABLE [dbo].[Usuarios]
ADD CONSTRAINT [PK_Usuarios]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Reservas'
ALTER TABLE [dbo].[Reservas]
ADD CONSTRAINT [PK_Reservas]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Locais'
ALTER TABLE [dbo].[Locais]
ADD CONSTRAINT [PK_Locais]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Equipamentos'
ALTER TABLE [dbo].[Equipamentos]
ADD CONSTRAINT [PK_Equipamentos]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Categorias'
ALTER TABLE [dbo].[Categorias]
ADD CONSTRAINT [PK_Categorias]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Controles'
ALTER TABLE [dbo].[Controles]
ADD CONSTRAINT [PK_Controles]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [RestricoesLocais_Id], [RestricoesCategoriaEquipamento_Id] in table 'Restricoes'
ALTER TABLE [dbo].[Restricoes]
ADD CONSTRAINT [PK_Restricoes]
    PRIMARY KEY CLUSTERED ([RestricoesLocais_Id], [RestricoesCategoriaEquipamento_Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [Usuario_Id] in table 'Reservas'
ALTER TABLE [dbo].[Reservas]
ADD CONSTRAINT [FK_UsuarioReserva]
    FOREIGN KEY ([Usuario_Id])
    REFERENCES [dbo].[Usuarios]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UsuarioReserva'
CREATE INDEX [IX_FK_UsuarioReserva]
ON [dbo].[Reservas]
    ([Usuario_Id]);
GO

-- Creating foreign key on [Local_Id] in table 'Reservas'
ALTER TABLE [dbo].[Reservas]
ADD CONSTRAINT [FK_ReservaLocal]
    FOREIGN KEY ([Local_Id])
    REFERENCES [dbo].[Locais]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ReservaLocal'
CREATE INDEX [IX_FK_ReservaLocal]
ON [dbo].[Reservas]
    ([Local_Id]);
GO

-- Creating foreign key on [CategoriaEquipamento_Id] in table 'Equipamentos'
ALTER TABLE [dbo].[Equipamentos]
ADD CONSTRAINT [FK_CategoriaEquipamentoEquipamento]
    FOREIGN KEY ([CategoriaEquipamento_Id])
    REFERENCES [dbo].[Categorias]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CategoriaEquipamentoEquipamento'
CREATE INDEX [IX_FK_CategoriaEquipamentoEquipamento]
ON [dbo].[Equipamentos]
    ([CategoriaEquipamento_Id]);
GO

-- Creating foreign key on [Reservas_Id] in table 'Categorias'
ALTER TABLE [dbo].[Categorias]
ADD CONSTRAINT [FK_ReservaCategoriaEquipamento]
    FOREIGN KEY ([Reservas_Id])
    REFERENCES [dbo].[Reservas]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ReservaCategoriaEquipamento'
CREATE INDEX [IX_FK_ReservaCategoriaEquipamento]
ON [dbo].[Categorias]
    ([Reservas_Id]);
GO

-- Creating foreign key on [Bolsista_Id] in table 'Controles'
ALTER TABLE [dbo].[Controles]
ADD CONSTRAINT [FK_UsuarioControle]
    FOREIGN KEY ([Bolsista_Id])
    REFERENCES [dbo].[Usuarios]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UsuarioControle'
CREATE INDEX [IX_FK_UsuarioControle]
ON [dbo].[Controles]
    ([Bolsista_Id]);
GO

-- Creating foreign key on [Reserva_Id] in table 'Controles'
ALTER TABLE [dbo].[Controles]
ADD CONSTRAINT [FK_ReservaControle]
    FOREIGN KEY ([Reserva_Id])
    REFERENCES [dbo].[Reservas]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ReservaControle'
CREATE INDEX [IX_FK_ReservaControle]
ON [dbo].[Controles]
    ([Reserva_Id]);
GO

-- Creating foreign key on [RestricoesLocais_Id] in table 'Restricoes'
ALTER TABLE [dbo].[Restricoes]
ADD CONSTRAINT [FK_Restricoes_Local]
    FOREIGN KEY ([RestricoesLocais_Id])
    REFERENCES [dbo].[Locais]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [RestricoesCategoriaEquipamento_Id] in table 'Restricoes'
ALTER TABLE [dbo].[Restricoes]
ADD CONSTRAINT [FK_Restricoes_CategoriaEquipamento]
    FOREIGN KEY ([RestricoesCategoriaEquipamento_Id])
    REFERENCES [dbo].[Categorias]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Restricoes_CategoriaEquipamento'
CREATE INDEX [IX_FK_Restricoes_CategoriaEquipamento]
ON [dbo].[Restricoes]
    ([RestricoesCategoriaEquipamento_Id]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------