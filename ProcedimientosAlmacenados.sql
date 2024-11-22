CREATE PROCEDURE [dbo].[CrearCliente]
    @Username VARCHAR(8),
    @Contrasenna VARCHAR(10),
    @Nombre VARCHAR(50),
    @Apellidos VARCHAR(50),
    @FechaNacimiento DATETIME,
    @Genero CHAR(1),
    @Telefono INT,
    @Email VARCHAR(20) = NULL,
    @Direccion VARCHAR(255) = NULL
AS
BEGIN
	DECLARE @Activo BIT = 1 --
	DECLARE @ClaveTemp BIT = 0 -- Sin clave
    DECLARE @RolID INT = 3  -- Rol Cliente
    DECLARE @MembresiaID INT = 1 -- Id de la membresia
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
        INSERT INTO dbo.Miembro (UsuarioID, Nombre, Apellidos, FechaNacimiento, Genero, Telefono, Email, Direccion, FechaRegistro, MembresiaID)
        VALUES (@NuevoUsuarioID, @Nombre, @Apellidos, @FechaNacimiento, @Genero, @Telefono, @Email, @Direccion, GETDATE(), @MembresiaID)

        COMMIT TRANSACTION
    END TRY
    BEGIN CATCH
     
        ROLLBACK TRANSACTION
        PRINT 'Ocurrio un error realizando el registro'
        RETURN
    END CATCH
END
GO



CREATE PROCEDURE [dbo].[IniciarSesion]
	@Username varchar(80),
	@Contrasenna varchar(255)
AS
BEGIN
	SELECT	UsuarioID ,
			Username , 
			Activo ,
			ClaveTemp ,
			Vigencia ,
			NombreRol
	  FROM	dbo.Usuario U
	  INNER JOIN dbo.Rol R ON U.RolID = R.RolID
	  WHERE Username = @Username
		AND Contrasenna = @Contrasenna
		AND Activo = 1
END
GO

CREATE PROCEDURE RegistrarEmpleado
    @Nombre NVARCHAR(100),
    @Apellidos NVARCHAR(100),
    @FechaNacimiento DATE,
    @Telefono INT,
    @Email NVARCHAR(100),
    @Direccion NVARCHAR(255),
    @FechaContratacion DATE,
    @CargoID INT,
    @UsuarioID BIGINT
AS
BEGIN
    INSERT INTO Empleado (Nombre, Apellidos, FechaNacimiento, Telefono, Email, Direccion, FechaContratacion, CargoID, UsuarioID)
    VALUES (@Nombre, @Apellidos, @FechaNacimiento, @Telefono, @Email, @Direccion, @FechaContratacion, @CargoID, @UsuarioID);
END;


DELETE FROM Miembro;

DELETE FROM Usuario;