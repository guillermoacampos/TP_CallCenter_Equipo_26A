CREATE DATABASE CallCenter
GO
USE CallCenter
GO
CREATE TABLE Perfil(
    
    IDPerfil int PRIMARY KEY,
    Nombre varchar(50) NOT NULL,
    Descripcion VARCHAR(255) NOT NULL
)
GO
CREATE TABLE Usuarios(

    IDUsuario int PRIMARY KEY,
    Nombre VARCHAR(50) NOT NULL,
    Apellido VARCHAR(50) NOT NULL,
    Email Varchar (100) NOT NULL,
    Contraseña varchar(6) NOT NULL,
    IDPerfil INT NOT NULL FOREIGN KEY REFERENCES Perfil(IDPerfil),
    Activo BIT NOT NULL,
    FechaDeAlta DATE NOT NULL
)
GO
CREATE TABLE Prioridades(

    IDPrioridad INT PRIMARY KEY,
    Nombre VARCHAR(10) NOT NULL,
    Nivel INT NOT NULL,
    Descripcion VARCHAR(255) NOT NULL
)
GO
CREATE TABLE Clientes(

    IDCliente INT PRIMARY KEY,
    Nombre VARCHAR(50) NOT NULL,
    Apellido VARCHAR(50) NOT NULL,
    Documento VARCHAR(8) NOT NULL,
    Email  VARCHAR(50) NOT NULL,
    Telefono VARCHAR(10) NOT NULL,
    Direccion VARCHAR(100) NOT NULL,
    Activo BIT NOT NULL,
    FechaAlta DATE NOT NULL
)
GO
CREATE TABLE TiposDeIncidencia(

    IDTipoIncidencia INT PRIMARY KEY,
    Nombre VARCHAR(50) NOT NULL,
    Descripcion VARCHAR(255) NOT NULL
)
GO
CREATE TABLE Estados(

    IDEstado INT PRIMARY KEY,
    Descripcion VARCHAR(15) NOT NULL
)
GO
CREATE TABLE Incidencias(

    IDIncidencia INT PRIMARY KEY,
    NumeroReclamo INT IDENTITY (1,1),
    IDCliente INT NOT NULL FOREIGN KEY REFERENCES Clientes (IDCliente),
    IDCreadorUsuario INT NOT NULL FOREIGN KEY REFERENCES Usuarios (IDUsuario),
    IDUsuarioAsignado  INT NOT NULL FOREIGN KEY REFERENCES Usuarios (IDUsuario),
    IDTipoIncidencia INT NOT NULL FOREIGN KEY REFERENCES TiposDeIncidencia (IDTipoIncidencia),
    IDPrioridad INT NOT NULL FOREIGN KEY REFERENCES Prioridades (IDPrioridad),
    IDEstado INT NOT NULL FOREIGN KEY REFERENCES Estados (IDEstado),
    Descripcion VARCHAR(255) NULL,
    FechaAlta DATE NOT NULL,
    FechaResolucion DATE NULL,
    ComentarioResolucion VARCHAR(255) NULL,
    ComentarioCierre VARCHAR(255) NULL
)
