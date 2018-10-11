
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 10/11/2018 14:57:26
-- Generated from EDMX file: C:\Users\NIP\source\repos\Uni7ReservasBackend\Uni7ReservasBackend\Models\Uni7ReservasEDM.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [Uni7ReservasBD];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_UsuarioReserva]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Reservas] DROP CONSTRAINT [FK_UsuarioReserva];
GO
IF OBJECT_ID(N'[dbo].[FK_ReservaLocal]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Reservas] DROP CONSTRAINT [FK_ReservaLocal];
GO
IF OBJECT_ID(N'[dbo].[FK_CategoriaEquipamentoEquipamento]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Equipamentos] DROP CONSTRAINT [FK_CategoriaEquipamentoEquipamento];
GO
IF OBJECT_ID(N'[dbo].[FK_Restricoes_Local]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Restricoes] DROP CONSTRAINT [FK_Restricoes_Local];
GO
IF OBJECT_ID(N'[dbo].[FK_Restricoes_CategoriaEquipamento]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Restricoes] DROP CONSTRAINT [FK_Restricoes_CategoriaEquipamento];
GO
IF OBJECT_ID(N'[dbo].[FK_UsuarioChamado]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Chamados] DROP CONSTRAINT [FK_UsuarioChamado];
GO
IF OBJECT_ID(N'[dbo].[FK_LocalRecursoLocal]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[RecursosLocais] DROP CONSTRAINT [FK_LocalRecursoLocal];
GO
IF OBJECT_ID(N'[dbo].[FK_RecursoRecursoLocal]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[RecursosLocais] DROP CONSTRAINT [FK_RecursoRecursoLocal];
GO
IF OBJECT_ID(N'[dbo].[FK_ReservaCategoriaEquipamento_Reserva]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ReservaCategoriaEquipamento] DROP CONSTRAINT [FK_ReservaCategoriaEquipamento_Reserva];
GO
IF OBJECT_ID(N'[dbo].[FK_ReservaCategoriaEquipamento_CategoriaEquipamento]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ReservaCategoriaEquipamento] DROP CONSTRAINT [FK_ReservaCategoriaEquipamento_CategoriaEquipamento];
GO
IF OBJECT_ID(N'[dbo].[FK_BolsistaReserva]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Reservas] DROP CONSTRAINT [FK_BolsistaReserva];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Usuarios]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Usuarios];
GO
IF OBJECT_ID(N'[dbo].[Reservas]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Reservas];
GO
IF OBJECT_ID(N'[dbo].[Locais]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Locais];
GO
IF OBJECT_ID(N'[dbo].[Equipamentos]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Equipamentos];
GO
IF OBJECT_ID(N'[dbo].[Categorias]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Categorias];
GO
IF OBJECT_ID(N'[dbo].[Chamados]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Chamados];
GO
IF OBJECT_ID(N'[dbo].[Recursos]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Recursos];
GO
IF OBJECT_ID(N'[dbo].[RecursosLocais]', 'U') IS NOT NULL
    DROP TABLE [dbo].[RecursosLocais];
GO
IF OBJECT_ID(N'[dbo].[Restricoes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Restricoes];
GO
IF OBJECT_ID(N'[dbo].[ReservaCategoriaEquipamento]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ReservaCategoriaEquipamento];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Usuarios'
CREATE TABLE [dbo].[Usuarios] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Nome] nvarchar(max)  NOT NULL,
    [Email] nvarchar(max)  NOT NULL,
    [Senha] nvarchar(max)  NOT NULL,
    [Tipo] int  NOT NULL,
    [Token] nvarchar(max)  NULL
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
    [Atraso] bit  NULL,
    [ComentarioUsuario] nvarchar(max)  NULL,
    [Satisfacao] int  NULL,
    [FoiUsado] bit  NULL,
    [ObsControle] nvarchar(max)  NULL,
    [Usuario_Id] int  NOT NULL,
    [Local_Id] int  NOT NULL,
    [Bolsista_Id] int  NOT NULL
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
    [Serie] nvarchar(max)  NOT NULL,
    [Disponivel] bit  NOT NULL,
    [CategoriaEquipamento_Id] int  NOT NULL
);
GO

-- Creating table 'Categorias'
CREATE TABLE [dbo].[Categorias] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Nome] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Chamados'
CREATE TABLE [dbo].[Chamados] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Descricao] nvarchar(max)  NOT NULL,
    [Status] int  NOT NULL,
    [Observacoes] nvarchar(max)  NOT NULL,
    [DataLimite] nvarchar(max)  NOT NULL,
    [Usuario_Id] int  NOT NULL
);
GO

-- Creating table 'Recursos'
CREATE TABLE [dbo].[Recursos] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Nome] nvarchar(max)  NOT NULL,
    [Detalhes] nvarchar(max)  NOT NULL,
    [Tipo] int  NOT NULL,
    [Vencimento] datetime  NOT NULL
);
GO

-- Creating table 'RecursosLocais'
CREATE TABLE [dbo].[RecursosLocais] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Qtde] int  NOT NULL,
    [Local_Id] int  NOT NULL,
    [Recurso_Id] int  NOT NULL
);
GO

-- Creating table 'Restricoes'
CREATE TABLE [dbo].[Restricoes] (
    [RestricoesLocais_Id] int  NOT NULL,
    [RestricoesCategoriaEquipamento_Id] int  NOT NULL
);
GO

-- Creating table 'ReservaCategoriaEquipamento'
CREATE TABLE [dbo].[ReservaCategoriaEquipamento] (
    [Reservas_Id] int  NOT NULL,
    [CategoriasEquipamentos_Id] int  NOT NULL
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

-- Creating primary key on [Id] in table 'Chamados'
ALTER TABLE [dbo].[Chamados]
ADD CONSTRAINT [PK_Chamados]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Recursos'
ALTER TABLE [dbo].[Recursos]
ADD CONSTRAINT [PK_Recursos]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'RecursosLocais'
ALTER TABLE [dbo].[RecursosLocais]
ADD CONSTRAINT [PK_RecursosLocais]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [RestricoesLocais_Id], [RestricoesCategoriaEquipamento_Id] in table 'Restricoes'
ALTER TABLE [dbo].[Restricoes]
ADD CONSTRAINT [PK_Restricoes]
    PRIMARY KEY CLUSTERED ([RestricoesLocais_Id], [RestricoesCategoriaEquipamento_Id] ASC);
GO

-- Creating primary key on [Reservas_Id], [CategoriasEquipamentos_Id] in table 'ReservaCategoriaEquipamento'
ALTER TABLE [dbo].[ReservaCategoriaEquipamento]
ADD CONSTRAINT [PK_ReservaCategoriaEquipamento]
    PRIMARY KEY CLUSTERED ([Reservas_Id], [CategoriasEquipamentos_Id] ASC);
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

-- Creating foreign key on [Usuario_Id] in table 'Chamados'
ALTER TABLE [dbo].[Chamados]
ADD CONSTRAINT [FK_UsuarioChamado]
    FOREIGN KEY ([Usuario_Id])
    REFERENCES [dbo].[Usuarios]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UsuarioChamado'
CREATE INDEX [IX_FK_UsuarioChamado]
ON [dbo].[Chamados]
    ([Usuario_Id]);
GO

-- Creating foreign key on [Local_Id] in table 'RecursosLocais'
ALTER TABLE [dbo].[RecursosLocais]
ADD CONSTRAINT [FK_LocalRecursoLocal]
    FOREIGN KEY ([Local_Id])
    REFERENCES [dbo].[Locais]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_LocalRecursoLocal'
CREATE INDEX [IX_FK_LocalRecursoLocal]
ON [dbo].[RecursosLocais]
    ([Local_Id]);
GO

-- Creating foreign key on [Recurso_Id] in table 'RecursosLocais'
ALTER TABLE [dbo].[RecursosLocais]
ADD CONSTRAINT [FK_RecursoRecursoLocal]
    FOREIGN KEY ([Recurso_Id])
    REFERENCES [dbo].[Recursos]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_RecursoRecursoLocal'
CREATE INDEX [IX_FK_RecursoRecursoLocal]
ON [dbo].[RecursosLocais]
    ([Recurso_Id]);
GO

-- Creating foreign key on [Reservas_Id] in table 'ReservaCategoriaEquipamento'
ALTER TABLE [dbo].[ReservaCategoriaEquipamento]
ADD CONSTRAINT [FK_ReservaCategoriaEquipamento_Reserva]
    FOREIGN KEY ([Reservas_Id])
    REFERENCES [dbo].[Reservas]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [CategoriasEquipamentos_Id] in table 'ReservaCategoriaEquipamento'
ALTER TABLE [dbo].[ReservaCategoriaEquipamento]
ADD CONSTRAINT [FK_ReservaCategoriaEquipamento_CategoriaEquipamento]
    FOREIGN KEY ([CategoriasEquipamentos_Id])
    REFERENCES [dbo].[Categorias]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ReservaCategoriaEquipamento_CategoriaEquipamento'
CREATE INDEX [IX_FK_ReservaCategoriaEquipamento_CategoriaEquipamento]
ON [dbo].[ReservaCategoriaEquipamento]
    ([CategoriasEquipamentos_Id]);
GO

-- Creating foreign key on [Bolsista_Id] in table 'Reservas'
ALTER TABLE [dbo].[Reservas]
ADD CONSTRAINT [FK_BolsistaReserva]
    FOREIGN KEY ([Bolsista_Id])
    REFERENCES [dbo].[Usuarios]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_BolsistaReserva'
CREATE INDEX [IX_FK_BolsistaReserva]
ON [dbo].[Reservas]
    ([Bolsista_Id]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------