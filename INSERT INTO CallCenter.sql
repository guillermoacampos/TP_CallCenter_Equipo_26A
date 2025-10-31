
USE CallCenter

GO
INSERT INTO Perfil (IDPerfil,Nombre,Descripcion)
VALUES
(1,'Telefonista','Atención al cliente y registro inicial de incidencias'),
(2,'Administrativo','Gestión administrativa y mantenimiento de catálogos'),
(3,'Supervisor','Supervisa operaciones, reasigna incidencias y gestiona personal');

GO
INSERT INTO Prioridades (IDPrioridad, Nombre, Nivel, Descripcion)
VALUES
(1, 'Urgente',1, 'Atención inmediata requerida, accion inmediato.'),
(2, 'Alta',   2, 'Requiere atención preferente, resolver en el día hábil.'),
(3, 'Media',  3, 'Atención en horario normal, priorización estándar.'),
(4, 'Baja',   4, 'No requiere atención inmediata, seguimiento según disponibilidad.');

GO
INSERT INTO Usuarios (IDUsuario, Nombre, Apellido, Email, Contraseña, IDPerfil, Activo, FechaDeAlta)
VALUES
(1, 'Maria',   'Gonzalez',   'maria.gonzalez@empresa.local',   'pass12', 1, 1, GETUTCDATE()),
(2, 'Javier',  'Lopez',      'javier.lopez@empresa.local',     'pass12', 1, 1, GETUTCDATE()),
(3, 'Lucia',   'Fernandez',  'lucia.fernandez@empresa.local',  'pass12', 1, 1, GETUTCDATE()),
(4, 'Diego',   'Ramos',      'diego.ramos@empresa.local',      'pass12', 1, 1, GETUTCDATE()),
(5, 'Sofia',   'Alvarez',    'sofia.alvarez@empresa.local',    'pass12', 2, 1, GETUTCDATE()),
(6, 'Martin',  'Suarez',     'martin.suarez@empresa.local',    'pass12', 2, 1, GETUTCDATE()),
(7, 'Laura',   'Perez',      'laura.perez@empresa.local',      'pass12', 3, 1, GETUTCDATE());

GO
INSERT INTO Estados (IDEstado,Descripcion)
VALUES (1,'Abierto'), (2,'En análisis'), (3,'Cerrado'), (4,'Reabierto'), (5,'Asignado'), (6,'Resuelto');

GO
INSERT INTO TiposDeIncidencia (IDTipoIncidencia, Nombre, Descripcion)
VALUES
(1, 'Facturación',      'Problemas con facturas, cobros o pagos.'),
(2, 'Corte de Servicio', 'Cortes, interrupciones o baja de servicio.'),
(3, 'Soporte Técnico',  'Fallas técnicas, configuraciones o errores del servicio.'),
(4, 'Consulta',         'Consultas generales sobre productos, horarios o trámites.'),
(5, 'Reclamo Comercial', 'Quejas relacionadas con atención comercial, contratos o promociones.');

GO
INSERT INTO Clientes (IDCliente, Nombre, Apellido, Documento, Email, Telefono, Direccion, Activo, FechaAlta)
VALUES
(1, 'Lionel',    'Messi',   '20123456', 'lionel.messi@example.com',   '1144441111', 'Calle Falsa 123', 1, GETUTCDATE()),
(2, 'María',    'Lopez',      '27123457', 'maria.lopez@example.com',      '1144442222', 'Av. Siempre Viva 10', 1, GETUTCDATE()),
(3, 'Carlos',   'Gonzalez',   '27123458', 'carlos.gonzalez@example.com',  '1144443333', 'Calle Verde 45', 1, GETUTCDATE()),
(4, 'Ana',      'Martinez',   '29123459', 'ana.martinez@example.com',     '1144444444', 'Ruta 9 Km 12', 1, GETUTCDATE()),
(5, 'Empresa',  'ABC S.A.',   '30123456', 'contacto@empresaabc.com',      '1155552222', 'Av. Industria 500', 1, GETUTCDATE()),
(6, 'Lucía',    'Fernández',  '30111223', 'lucia.fernandez@example.com',  '1166663333', 'Calle Azul 7', 1, GETUTCDATE()),
(7, 'Jorge',    'Ruiz',       '27111224', 'jorge.ruiz@example.com',       '1177774444', 'Boulevard Central 200', 1, GETUTCDATE()),
(8, 'Sofía',    'Álvarez',    '29111225', 'sofia.alvarez@example.com',    '1188885555', 'Callejón 9', 1, GETUTCDATE()),
(9, 'Roberto',  'Santos',     '23123456', 'roberto.santos@example.com',   '1199996666', 'Pasaje Interno 3', 1, GETUTCDATE()),
(10,'Natalia',  'Ramos',      '24123457', 'natalia.ramos@example.com',    '1110107777', 'Plaza Central 1', 1, GETUTCDATE());

GO 
/*PRUEBA
INSERT INTO Incidencias (IDIncidencia,IDCliente,IDCreadorUsuario,IDUsuarioAsignado,IDTipoIncidencia,IDPrioridad,IDEstado,Descripcion,FechaAlta)
VALUES (1,1,1,1,1,1,1,'Probando 123..',GETUTCDATE());

SELECT I.IDCliente, C.Nombre FROM Incidencias AS I 
INNER JOIN Clientes C ON I.IDCliente = C.IDCliente*/