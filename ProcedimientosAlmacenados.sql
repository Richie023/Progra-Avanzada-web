USE [Gym]
GO

CREATE PROCEDURE [dbo].[CrearCliente]
    @Username VARCHAR(8),
    @Contrasenna VARCHAR(255),
    @Nombre VARCHAR(50),
    @Apellidos VARCHAR(50),
    @FechaNacimiento DATETIME,
    @Genero CHAR(1),
    @Telefono INT,
    @Email VARCHAR(20) = NULL,
    @Direccion VARCHAR(255) = NULL
AS
BEGIN
	DECLARE @Activo BIT = 1
	DECLARE @ClaveTemp BIT = 0
    DECLARE @RolID INT = 3
    DECLARE @MembresiaID INT = 1
    DECLARE @NuevoUsuarioID BIGINT

    BEGIN TRY
        BEGIN TRANSACTION

        IF NOT EXISTS(SELECT 1 FROM dbo.Usuario WHERE Username = @Username)
        BEGIN
            INSERT INTO dbo.Usuario (Username, Contrasenna, Activo, ClaveTemp, Vigencia, RolID)
            VALUES (@Username, @Contrasenna, @Activo, @ClaveTemp, GETDATE(), @RolID)
            
            SET @NuevoUsuarioID = SCOPE_IDENTITY()
        END
        ELSE
        BEGIN
            PRINT 'Nombre de usuario en Uso'
            ROLLBACK TRANSACTION
            RETURN
        END

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

CREATE PROCEDURE [dbo].[RegistrarEmpleado]
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
GO

CREATE PROCEDURE [dbo].[ObtenerMembresias]
AS
BEGIN
    SELECT 
      MembresiaID,
	  TipoMembresia,
	  Precio,
	  Duracion,
	  Beneficios
 
    FROM 
       Membresia
END
GO

CREATE PROCEDURE [dbo].[ValidarUsuario]
    @Email VARCHAR(255)
AS
BEGIN
    DECLARE @UsuarioID BIGINT;

    SELECT @UsuarioID = UsuarioID
    FROM Empleado
    WHERE Email = @Email;

    IF @UsuarioID IS NULL
    BEGIN
        SELECT @UsuarioID = UsuarioID
        FROM Miembro
        WHERE Email = @Email;
    END

    IF @UsuarioID IS NOT NULL
    BEGIN
        SELECT U.UsuarioID,
               U.Username,
               U.Contrasenna,
               U.Activo,
               U.ClaveTemp,
               U.Vigencia,
               U.RolID,
               R.NombreRol
        FROM Usuario U
        INNER JOIN Rol R ON U.RolID = R.RolID
        WHERE U.UsuarioID = @UsuarioID;
    END
END
GO

CREATE PROCEDURE [dbo].[ActualizarContrasenna]
	@UsuarioID bigint,
	@Contrasenna varchar(255),
	@ClaveTemp bit,
	@Vigencia datetime
AS
BEGIN
	
	UPDATE	dbo.Usuario
	SET		Contrasenna = @Contrasenna,
			ClaveTemp = @ClaveTemp,
			Vigencia = @Vigencia
	WHERE	UsuarioID = @UsuarioID

END
GO

CREATE PROCEDURE [dbo].[ConsultarClases]
AS
BEGIN
    SELECT 
        c.ClaseID,
        c.Nombre ,
        c.Descripcion,
        c.Duracion,
        c.Horario,
        CONCAT(e.Nombre, ' ', e.Apellidos) AS Entrenador
    FROM 
       Clases c
    JOIN 
        Empleado e
    ON 
        c.EmpleadoID = e.EmpleadoID;
END
GO

CREATE PROCEDURE [dbo].[ConsultarUsuarioClases]
    @UsuarioID INT
AS
BEGIN
    SELECT 
        c.ClaseID,
        c.Nombre,
        c.Descripcion,
        c.Duracion,
        c.Horario,
        CONCAT(e.Nombre, ' ', e.Apellidos) AS Entrenador
    FROM 
        Clases c
    JOIN 
        Empleado e
        ON c.EmpleadoID = e.EmpleadoID
    JOIN 
        MiembroClase mc
        ON c.ClaseID = mc.ClaseID
    JOIN 
		Miembro m
        ON mc.MiembroID = m.MiembroID
    WHERE 
        m.UsuarioID = @UsuarioID;
END
GO

CREATE PROCEDURE [dbo].[RegistrarPlanEntenamiento]
    @UsuarioID BIGINT,
    @Ejercicio NVARCHAR(100),
    @Repeticiones INT,
    @Peso DECIMAL(5,2),
    @FechaCreacion DATETIME
AS
BEGIN
    INSERT INTO PlanEntrenamiento (UsuarioID, Ejercicio, Repeticiones, Peso, FechaCreacion)
    VALUES (@UsuarioID, @Ejercicio, @Repeticiones, @Peso, GETDATE());
END
GO

CREATE PROCEDURE [dbo].[ConsultarPlanEntenamientoAdmin]
AS
BEGIN
    SELECT 
	   UsuarioID,
	   Ejercicio,
	   Repeticiones,
	   Peso,
	   FechaCreacion
    FROM 
       PlanEntrenamiento
END
GO

CREATE PROCEDURE [dbo].[ConsultarPlanEntrenamientoUsuario]
    @UsuarioID BIGINT
AS
BEGIN
    SELECT 
        PlanEntrenamientoID,
        UsuarioID,
        Ejercicio,
        Repeticiones,
        Peso,
        FechaCreacion
    FROM 
        PlanEntrenamiento
    WHERE 
        UsuarioID = @UsuarioID
    ORDER BY 
        FechaCreacion DESC;
END
GO

CREATE PROCEDURE [dbo].[RegistrarProgreso]
    @UsuarioID BIGINT,
    @Peso DECIMAL(5,2),
	@CantidadEjercicios INT,
	@DuracionEntrenamiento INT    
AS
BEGIN    
    INSERT INTO Progreso (UsuarioID, Peso, CantidadEjercicios, DuracionEntrenamiento, FechaRegistro)
    VALUES (@UsuarioID, @Peso, @CantidadEjercicios,	@DuracionEntrenamiento, GETDATE());
END
GO

CREATE PROCEDURE [dbo].[ConsultarProgreso]
    @UsuarioID BIGINT
AS
BEGIN
    SELECT 
        ProgresoID,
        UsuarioID,
        Peso,
		CantidadEjercicios,
		DuracionEntrenamiento,
        FechaRegistro        
    FROM 
		Progreso
    WHERE 
		UsuarioID = @UsuarioID
    ORDER BY 
		FechaRegistro DESC;
END
GO

CREATE PROCEDURE [dbo].[ConsultarProductos]
	
AS
BEGIN
	
	SELECT	ProductoID,
			Nombre,
			Precio,
			Stock,
			Imagen + CONVERT(VARCHAR,ProductoID) + '.png' Imagen,
			Activo,
			CASE WHEN Activo = 1 THEN 'Activo' ELSE 'Inactivo' END Estado
	  FROM	Producto

END
GO

CREATE PROCEDURE [dbo].[ActualizarEstado]
	@ProductoID bigint
AS
BEGIN
	
	UPDATE	Producto
	SET		Activo = CASE WHEN Activo = 1 THEN 0 ELSE 1 END
	WHERE	ProductoID = @ProductoID

END
GO

CREATE PROCEDURE [dbo].[RegistrarProducto]
	@Nombre varchar(50),
	@Precio decimal(18,2),
	@Stock int,
	@Imagen varchar(50)
AS
BEGIN
	
	INSERT INTO Producto (Nombre,Precio,Stock,Imagen,Activo)
	VALUES (@Nombre, @Precio, @Stock, @Imagen, 1)

	SELECT @@IDENTITY ProductoID

END
GO

CREATE PROCEDURE [dbo].[ConsultarProducto]
	@ProductoID BIGINT
AS
BEGIN
	
	SELECT	ProductoID,
			Nombre,
			Precio,
			Stock,
			Imagen + CONVERT(VARCHAR,ProductoID) + '.png' Imagen,
			Activo,
			CASE WHEN Activo = 1 THEN 'Activo' ELSE 'Inactivo' END Estado
	  FROM	Producto
	  WHERE ProductoID = @ProductoID

END
GO

CREATE PROCEDURE [dbo].[ActualizarProducto]
	@ProductoID bigint,
	@Nombre varchar(50),
	@Precio decimal(18,2),
	@Stock int
AS
BEGIN
	
	UPDATE Producto
	SET	Nombre = @Nombre,
		Precio = @Precio,
		Stock = @Stock
	WHERE ProductoID = @ProductoID

END
GO

CREATE PROCEDURE [dbo].[ConsultarProductosActivos]
AS
BEGIN
    SELECT  ProductoID,
            Nombre,
            Precio,
            Stock,
            Imagen + CONVERT(VARCHAR, ProductoID) + '.png' AS Imagen,
            Activo,
            'Activo' AS Estado -- Ya que solo mostrará productos activos, esto siempre será 'Activo'
    FROM    Producto
    WHERE   Activo = 1; -- Filtra solo los productos activos
END
GO

ALTER PROCEDURE [dbo].[ActualizarSinMembresia]
    @UsuarioID bigint
AS
BEGIN
    UPDATE Miembro
    SET MembresiaID = 1
    WHERE UsuarioID = @UsuarioID;
END;
GO

ALTER PROCEDURE [dbo].[ActualizarMembresiaRegular]
    @UsuarioID bigint
AS
BEGIN
    UPDATE Miembro
    SET MembresiaID = 2
    WHERE UsuarioID = @UsuarioID;
END;
GO

ALTER PROCEDURE [dbo].[ActualizarMembresiaPremium]
    @UsuarioID bigint
AS
BEGIN
    UPDATE Miembro
    SET MembresiaID = 3
    WHERE UsuarioID = @UsuarioID;
END;
GO

ALTER PROCEDURE [dbo].[ConsultarMiembro]
	@UsuarioID BIGINT
AS
BEGIN	
	SELECT	MiembroID,
			UsuarioID,
			Nombre,
			Apellidos,
			FechaNacimiento,
			Genero,
			Telefono,
			Email,
			Direccion,
			FechaRegistro,
			MembresiaID
	  FROM	Miembro
	  WHERE UsuarioID = @UsuarioID
END
GO

ALTER PROCEDURE [dbo].[ConsultarMembresiaMiembro]
	@UsuarioID BIGINT
AS
BEGIN
	SELECT  M.MiembroID,
                M.UsuarioID,
                M.Nombre,
                M.Apellidos,
                M.FechaNacimiento,
                M.Genero,
                M.Telefono,
                M.Email,
                M.Direccion,
                M.FechaRegistro,
                M.MembresiaID,
                M.Nombre + ' ' + M.Apellidos AS NombreCompleto,
                E.TipoMembresia,
                E.Precio,
                E.Duracion,
                E.Beneficios
          FROM  Miembro M
          LEFT JOIN Membresia E ON M.MembresiaID = E.MembresiaID
          WHERE UsuarioID = @UsuarioID
END
GO

