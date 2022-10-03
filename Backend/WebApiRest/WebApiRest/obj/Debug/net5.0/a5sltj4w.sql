IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [Usuario] (
    [Id] int NOT NULL IDENTITY,
    [Correo] nvarchar(50) NOT NULL,
    [Contrasena] nvarchar(max) NOT NULL,
    [Token] nvarchar(max) NULL,
    CONSTRAINT [PK_Usuario] PRIMARY KEY ([Id])
);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20220824231527_Initial', N'5.0.17');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO


                CREATE PROCEDURE sp_Usuario
                AS
                BEGIN
                    SELECT Id, Correo FROM dbo.Usuario
                END
            
GO


                CREATE PROCEDURE sp_crearUsuario(@Correo varchar(50), 
                                                 @Contrasena varchar(20), 
                                                 @Token varchar(max))
                AS
                BEGIN
                    INSERT INTO dbo.Usuario
                    VALUES(@Correo, @Contrasena, @Token)
                END
            
GO


                CREATE PROCEDURE sp_obtenerUsuario(@Id int)
                AS
                BEGIN
                    SELECT * FROM dbo.Usuario WHERE Id = @Id
                END
            
GO


                CREATE PROCEDURE sp_editarUsuario(@Id int,
                                                  @Correo nvarchar(50),
                                                  @Contrasena nvarchar(max),
                                                  @Token nvarchar(max))
                AS
                BEGIN
                UPDATE 
                    dbo.Usuario
                SET
                    Correo = @Correo,
                    Contrasena = @Contrasena,
                    Token = @Token
                WHERE 
                    Id = @Id
                END
            
GO


                CREATE PROCEDURE sp_eliminarUsuario(@Id int)
                AS
                BEGIN
                DELETE FROM 
                    dbo.Usuario 
                WHERE 
                    Id = @Id
                END
            
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20220824231616_Procedures', N'5.0.17');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO


                USE Facultad
GO

CREATE VIEW vw_listaUsuarios
                AS
                    SELECT Id as [Id de usuario], Correo as [Correo de usuario] FROM dbo.Usuario
            
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20220824231628_Views', N'5.0.17');
GO

COMMIT;
GO

