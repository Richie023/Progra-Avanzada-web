CREATE PROCEDURE [dbo].[CrearCliente]
    @Username VARCHAR(8),
    @Contrasenna VARCHAR(10),
    @Activo BIT = 1,
    @ClaveTemp BIT = 0,
    @Vigencia DATETIME ,
    @Nombre VARCHAR(50),
    @Apellidos VARCHAR(50),
    @FechaNacimiento DATE,
    @Genero CHAR(1),
    @Telefono INT,
    @Email VARCHAR(20) = NULL,
    @Direccion VARCHAR(255) = NULL,
    @MembresiaID INT,
    @ClaseID INT
AS
BEGIN
    DECLARE @RolID INT = 3  -- Rol Cliente
    DECLARE @NuevoUsuarioID BIGINT

    BEGIN TRY
        BEGIN TRANSACTION

        IF NOT EXISTS(SELECT 1 FROM dbo.Usuario WHERE Username = @Username)
        BEGIN
            INSERT INTO dbo.Usuario (Username, Contrasenna, Activo, ClaveTemp, Vigencia, RolID)
            VALUES (@Username, @Contrasenna, @Activo, @ClaveTemp, GETDATE(), @RolID)
            
            -- Se obtiene el ID del usuario creado
            SET @NuevoUsuarioID = SCOPE_IDENTITY()
        END
        ELSE
        BEGIN
            -- Manejar el caso en el que el usuario ya existe
            PRINT 'Nombre de usuario en Uso'
            ROLLBACK TRANSACTION
            RETURN
        END

        -- Insertar nuevo miembro
        INSERT INTO dbo.Miembro (UsuarioID, Nombre, Apellidos, FechaNacimiento, Genero, Telefono, Email, Direccion, FechaRegistro, MembresiaID, ClaseID)
        VALUES (@NuevoUsuarioID, @Nombre, @Apellidos, @FechaNacimiento, @Genero, @Telefono, @Email, @Direccion, GETDATE(), @MembresiaID, @ClaseID)

        COMMIT TRANSACTION
    END TRY
    BEGIN CATCH
     
        ROLLBACK TRANSACTION
        PRINT 'Ocurrio un error realizando el registro'
        RETURN
    END CATCH
END
GO

