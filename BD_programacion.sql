USE [master]
GO

CREATE DATABASE [Gym]
GO

USE [Gym]
GO

CREATE TABLE Rol (
RolID INT PRIMARY KEY IDENTITY (1,1) ,
NombreRol VARCHAR (20) NOT NULL 
);
CREATE TABLE Usuario (
	UsuarioID BIGINT PRIMARY KEY IDENTITY (1,1) ,
	Username  VARCHAR (8) NOT NULL UNIQUE ,
	Contrasenna VARCHAR (10) NOT NULL ,
	Activo BIT NOT NULL ,
	ClaveTemp BIT NOT NULL,
	Vigencia DATETIME NOT NULL,
	RolID INT NOT NULL ,
	FOREIGN KEY (RolID) References Rol (RolID)
);

CREATE TABLE Cargo (
	CargoID INT PRIMARY KEY IDENTITY  (1,1) ,
	NombreCargo VARCHAR (20)
);

CREATE TABLE Empleado (
    EmpleadoID INT PRIMARY KEY IDENTITY  (1,1) ,
	UsuarioID BIGINT NOT NULL,
    Nombre VARCHAR(50) NOT NULL,
	Apellidos VARCHAR (50) NOT NULL,
    FechaNacimiento DATETIME NOT NULL,
    Telefono INT NOT NULL ,
	Email VARCHAR (30) ,
	Direccion VARCHAR (255) ,
	FechaContratacion DATE NOT NULL,
    CargoID INT ,
	FOREIGN KEY (UsuarioID) REFERENCES Usuario(UsuarioID),
	FOREIGN KEY (CargoID) References Cargo (CargoID)
    
);

CREATE TABLE Membresia (
	MembresiaID INT PRIMARY KEY IDENTITY  (1,1),
	TipoMembresia VARCHAR (20),
	Precio DECIMAL(10, 2) NOT NULL,
    Duracion NVARCHAR(50) NOT NULL,
    Beneficios NVARCHAR(MAX) NOT NULL
);

CREATE TABLE Clases (
    ClaseID INT PRIMARY KEY IDENTITY (1,1),
    Nombre NVARCHAR(100) NOT NULL,
    Descripcion NVARCHAR(255),
    Duracion INT NOT NULL,
	HorarioID INT NOT NULL,
    EmpleadoID INT NOT NULL,
    FOREIGN KEY (EmpleadoID) REFERENCES Empleado(EmpleadoID)
);

CREATE TABLE Miembro (
	MiembroID BIGINT PRIMARY KEY IDENTITY (1,1) ,
	UsuarioID BIGINT NOT NULL,
	Nombre VARCHAR(50) NOT NULL,
	Apellidos VARCHAR (50) NOT NULL,
	FechaNacimiento DATETIME NOT NULL,
	Genero CHAR(1) CHECK (Genero IN ('M', 'F' , 'N')),
	Telefono INT NOT NULL ,
	Email VARCHAR (20) UNIQUE ,
	Direccion VARCHAR (255) ,
	FechaRegistro Date NOT NULL,
	MembresiaID INT NOT NULL ,
	FOREIGN KEY (UsuarioID) REFERENCES Usuario(UsuarioID),
	FOREIGN KEY (MembresiaID) References Membresia (MembresiaID),

);

CREATE TABLE MiembroClase (
	ID BIGINT PRIMARY KEY IDENTITY (1,1) ,
	ClaseID INT NOT NULL,
	MiembroID BIGINT NOT NULL,
	FOREIGN KEY (MiembroID) REFERENCES Miembro(MiembroID),
	FOREIGN KEY (ClaseID) REFERENCES Clases(ClaseID)
);

CREATE TABLE Equipos (
    EquipoID INT PRIMARY KEY IDENTITY (1,1),
    Nombre VARCHAR(100) NOT NULL,
    Descripcion VARCHAR(255),
    FechaAdquisicion DATETIME,
	EncargadoID INT NOT NULL,
    Estado VARCHAR(50) CHECK (Estado IN ('Disponible', 'Mantenimiento', 'Fuera de Servicio')),
	FOREIGN KEY (EncargadoID) REFERENCES Empleado(EmpleadoID)
);

CREATE TABLE Pagos (
    PagoID INT PRIMARY KEY IDENTITY (1,1),
    UsuarioID BIGINT NOT NULL,
    FechaID INT NOT NULL,
    Monto DECIMAL(10, 2) NOT NULL,
    MetodoPago VARCHAR(50) CHECK (MetodoPago IN ('Efectivo', 'Tarjeta', 'Transferencia')),
    FOREIGN KEY (UsuarioID) REFERENCES Usuario(UsuarioID)
);

CREATE TABLE Reservas (
    ReservaID INT PRIMARY KEY IDENTITY (1,1),
    UsuarioID BIGINT NOT NULL,
    FechaReserva DATETIME NOT NULL,
    Estado BIT NOT NULL ,
    Notas VARCHAR(255) NULL,
    FechaCreacion DATETIME NOT NULL DEFAULT GETDATE(),
    FechaActualizacion DATETIME NULL , 
    FOREIGN KEY (UsuarioID) REFERENCES Usuario(UsuarioID)
);

CREATE TABLE Errores (
    ErrorID INT PRIMARY KEY IDENTITY (1,1),
    UsuarioID BIGINT NOT NULL,
    FechaError DATETIME NOT NULL DEFAULT GETDATE(),
    Mensaje VARCHAR(255) NOT NULL,
    FOREIGN KEY (UsuarioID) REFERENCES Usuario(UsuarioID)
);

CREATE TABLE PlanEntrenamiento (
    PlanEntrenamientoID INT PRIMARY KEY IDENTITY,
    UsuarioID BIGINT NOT NULL,
    Ejercicio NVARCHAR(100) NOT NULL,
    Repeticiones INT NOT NULL,
    Peso DECIMAL(5,2) NOT NULL,
    Fecha DATE NOT NULL,
    FOREIGN KEY (UsuarioID) REFERENCES Usuario(UsuarioID)
);

INSERT INTO Usuario (Username, Contrasenna, Activo, ClaveTemp, Vigencia, RolID)
VALUES ('usuario1', 'P123', 1, 0, GETDATE(), 2);

INSERT INTO Cargo (NombreCargo)
VALUES ('Entrenador');


INSERT INTO Rol (NombreRol) VALUES ('Administrador');
INSERT INTO Rol (NombreRol) VALUES ('Empleado');
INSERT INTO Rol (NombreRol) VALUES ('Cliente');
GO


INSERT INTO Membresia (TipoMembresia, Precio, Duracion, Beneficios) 
VALUES 
('Sin membresia', 0.00, 'N/A', 'Acceso limitado a servicios b?sicos'),
('Regular', 50.00, '1 mes', 'Acceso a gimnasio, clases grupales est?ndar'),
('Premium', 100.00, '1 mes', 'Acceso a todas las ?reas, clases premium, sesiones con entrenador personal');
GO

