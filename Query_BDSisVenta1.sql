-----------------------------------------CREACIÓN DE BASE DE DATOS-----------------------------------------
CREATE DATABASE DBVENTASINVENTARIO
GO

-----------------------------------------USO BASE DE DATOS-----------------------------------------
USE DBVENTASINVENTARIO
GO

-----------------------------------------CREACIÓN DE TABLAS-----------------------------------------
CREATE TABLE ROL(
	IdRol INT PRIMARY KEY IDENTITY,
	Descripcion VARCHAR (50),
	FechaRegistro DATETIME DEFAULT GETDATE()
)
GO

CREATE TABLE PERMISO(
	IdPermiso INT PRIMARY KEY IDENTITY,
	IdRol INT REFERENCES ROL(IdRol),
	NombreMenu VARCHAR (100),
	FechaRegistro DATETIME DEFAULT GETDATE()
)
GO

CREATE TABLE PROVEEDOR(
	IdProveedor INT PRIMARY KEY IDENTITY,
	Documento VARCHAR (50),
	RazonSocial VARCHAR (50),
	Correo VARCHAR (50),
	Telefono VARCHAR (50),
	Estado BIT,
	FechaRegistro DATETIME DEFAULT GETDATE()
)
GO

CREATE TABLE CLIENTE(
	IdCliente INT PRIMARY KEY IDENTITY,
	Documento VARCHAR (50),
	NombreCompleto VARCHAR (50),
	Correo VARCHAR (50),
	Telefono VARCHAR (50),
	Estado BIT,
	FechaRegistro DATETIME DEFAULT GETDATE()
)
GO

CREATE TABLE USUARIO(
	IdUsuario INT PRIMARY KEY IDENTITY,
	Documento VARCHAR (50),
	NombreCompleto VARCHAR (50),
	Correo VARCHAR (50),
	Clave VARCHAR (50),
	IdRol INT REFERENCES ROL(IdRol),
	Estado BIT,
	FechaRegistro DATETIME DEFAULT GETDATE()
)
GO

CREATE TABLE CATEGORIA(
	IdCategoria INT PRIMARY KEY IDENTITY,
	Descripcion VARCHAR (100),
	Estado BIT,
	FechaRegistro DATETIME DEFAULT GETDATE()
)
GO

CREATE TABLE PRODUCTO(
	IdProducto INT PRIMARY KEY IDENTITY,
	Codigo VARCHAR(50),
	Nombre VARCHAR(50),
	Descripcion VARCHAR(50),
	IdCategoria INT REFERENCES CATEGORIA(IdCategoria),
	Stock INT NOT NULL DEFAULT 0,
	PrecioCompra DECIMAL (10,2) DEFAULT 0,
	PrecioVenta DECIMAL (10,2) DEFAULT 0,
	Estado BIT,
	FechaRegistro DATETIME DEFAULT GETDATE()
)
GO

CREATE TABLE COMPRA(
	IdCompra INT PRIMARY KEY IDENTITY,
	IdUsuario INT REFERENCES USUARIO(IdUsuario),
	IdProveedor INT REFERENCES PROVEEDOR(IdProveedor),
	TipoDocumento VARCHAR (50),
	NumeroDocumento VARCHAR (50),
	MontoTotal DECIMAL(10,2),
	FechaRegistro DATETIME DEFAULT GETDATE()
)
GO

CREATE TABLE DETALLE_COMPRA(
	IdDetalleCompra INT PRIMARY KEY IDENTITY,
	IdCompra INT REFERENCES COMPRA(IdCompra),
	IdProducto INT REFERENCES PRODUCTO(IdProducto),
	PrecioCompra DECIMAL (10,2) DEFAULT 0,
	PrecioVenta DECIMAL (10,2) DEFAULT 0,
	Cantidad INT,
	MontoTotal DECIMAL(10,2),
	FechaRegistro DATETIME DEFAULT GETDATE()
)
GO

CREATE TABLE VENTA(
	IdVenta INT PRIMARY KEY IDENTITY,
	IdUsuario INT REFERENCES USUARIO(IdUsuario),
	TipoDocumento VARCHAR (50),
	NumeroDocumento VARCHAR (50),
	DocumentoCliente VARCHAR (50),
	NombreCliente VARCHAR (100),
	MontoPago DECIMAL(10,2),
	MontoCambio DECIMAL(10,2),
	MontoTotal DECIMAL(10,2),
	FechaRegistro DATETIME DEFAULT GETDATE()
)
GO

CREATE TABLE DETALLE_VENTA(
	IdDetalleVenta INT PRIMARY KEY IDENTITY,
	IdVenta INT REFERENCES VENTA(IdVenta),
	IdProducto INT REFERENCES PRODUCTO(IdProducto),
	PrecioVenta DECIMAL (10,2),
	Cantidad INT,
	SubTotal DECIMAL(10,2),
	FechaRegistro DATETIME DEFAULT GETDATE()
)
GO

--Para Documentos Generados
CREATE TABLE NEGOCIO(
	IdNegocio INT PRIMARY KEY,
	Nombre VARCHAR(60),
	RUC VARCHAR(60),
	Direccion VARCHAR(60),
	Logo VARBINARY(MAX) NULL
)
GO


-----------------------------------------INSERCIONES A LAS TABLAS-----------------------------------------
--Inserciones a la Tabla ROL
INSERT INTO ROL(Descripcion)
VALUES
('ADMINISTRADOR')
GO

INSERT INTO ROL(Descripcion)
VALUES
('EMPLEADO')
GO


--Inserciones a la Tabla PERMISO
INSERT INTO PERMISO(IdRol, NombreMenu)
VALUES
(1,'menuusuarios'),
(1,'menumantenedor'),
(1,'menuventas'),
(1,'menucompras'),
(1,'menuclientes'),
(1,'menuproveedores'),
(1,'menureportes'),
(1,'menuacercade')
GO

INSERT INTO PERMISO(IdRol, NombreMenu)
VALUES
(2,'menuventas'),
(2,'menucompras'),
(2,'menuclientes'),
(2,'menuproveedores'),
(2,'menuacercade')
GO


--Inserciones a la Tabla ROL
INSERT USUARIO(Documento, NombreCompleto, Correo, Clave, IdRol, Estado)
VALUES
('73874060','Sergio Suarez','asebala.br15@gmail.com','233008',1,1)
GO


-----------------------------------------PROCEDIMIENTOS ALMACENADOS-----------------------------------------
/**PROCEDIMIENTOS PARA USUARIO**/
	/**Para registrar usuario**/
CREATE PROC SP_REGISTRARUSUARIO(
	@Documento VARCHAR(50),
	@NombreCompleto VARCHAR(100),
	@Correo VARCHAR(100),
	@Clave VARCHAR(100),
	@IdRol INT,
	@Estado BIT,
	@IdUsuarioResultado INT OUTPUT,
	@Mensaje VARCHAR(500) OUTPUT
)
AS
BEGIN
	SET @IdUsuarioResultado = 0
	SET @Mensaje = ''

	IF NOT EXISTS(SELECT * FROM USUARIO WHERE Documento = @Documento)

	BEGIN 
		INSERT INTO USUARIO(Documento, NombreCompleto, Correo, Clave, IdRol, Estado) VALUES
		(@Documento, @NombreCompleto, @Correo, @Clave, @IdRol, @Estado)
		
		SET @IdUsuarioResultado = SCOPE_IDENTITY()
		
	END
	ELSE
		SET @Mensaje = 'Ya hay un Usuario con este DNI registrado'
END
GO


/**Para editar usuario**/
CREATE PROC SP_EDITARUSUARIO(
	@IdUsuario INT,
	@Documento VARCHAR(50),
	@NombreCompleto VARCHAR(100),
	@Correo VARCHAR(100),
	@Clave VARCHAR(100),
	@IdRol INT,
	@Estado BIT,
	@Respuesta BIT OUTPUT,
	@Mensaje VARCHAR(500) OUTPUT
)
AS
BEGIN
	SET @Respuesta = 0
	SET @Mensaje = ''

	IF NOT EXISTS(SELECT * FROM USUARIO WHERE Documento = @Documento AND IdUsuario != @IdUsuario)

	BEGIN 
		UPDATE USUARIO SET
			Documento = @Documento, 
			NombreCompleto = @NombreCompleto, 
			Correo = @Correo, 
			Clave = @Clave, 
			IdRol = @IdRol, 
			Estado = @Estado 
		WHERE IdUsuario = @IdUsuario
				
		SET @Respuesta = 1
		
	END
	ELSE
		SET @Mensaje = 'Ya hay un Usuario con este DNI registrado'
END
GO


	/**Para eliminar usuario**/
CREATE PROC SP_ELIMINARUSUARIO(
	@IdUsuario INT,
	@Respuesta BIT OUTPUT,
	@Mensaje VARCHAR(500) OUTPUT
)
AS
BEGIN
	SET @Respuesta = 0
	SET @Mensaje = ''
	declare @pasoreglas BIT = 1

	IF EXISTS(SELECT * FROM COMPRA C
	INNER JOIN USUARIO U ON U.IdUsuario = C.IdUsuario
	WHERE U.IdUsuario = @IdUsuario)
	BEGIN
		SET @pasoreglas = 0
		SET @Respuesta = 0
		SET @Mensaje = 'No se puede eliminar, el usuario esta relacionado con al menos una compra\n'
	END

	IF EXISTS(SELECT * FROM VENTA V
	INNER JOIN USUARIO U ON U.IdUsuario = V.IdUsuario
	WHERE U.IdUsuario = @IdUsuario)
	BEGIN
		SET @pasoreglas = 0
		SET @Respuesta = 0
		SET @Mensaje = 'No se puede eliminar, el usuario esta relacionado con al menos una venta\n'
	END

	IF (@pasoreglas = 1)
	BEGIN
		DELETE FROM USUARIO WHERE IdUsuario = @IdUsuario
		SET @Respuesta = 1
	END
END
GO


/**PROCEDIMIENTOS PARA CATEGORÍA**/
	/**Para agregar categoría**/
CREATE PROC SP_REGISTRARCATEGORIA(
	@Descripcion VARCHAR(50),
	@Estado BIT,
	@Resultado INT OUTPUT,
	@Mensaje VARCHAR(500) OUTPUT
)
AS
BEGIN
	SET @Resultado = 0

	IF NOT EXISTS (SELECT * FROM CATEGORIA WHERE Descripcion = @Descripcion)
		BEGIN
			INSERT INTO CATEGORIA(Descripcion, Estado) VALUES (@Descripcion,@Estado)
			SET @Resultado = SCOPE_IDENTITY()
		END
	ELSE
		SET @Mensaje = 'Una categoria ya tiene la misma descripción'
END
GO


	/**Para editar categoría**/
CREATE PROC SP_EDITARCATEGORIA(
	@IdCategoria INT,
	@Descripcion VARCHAR(50),
	@Estado BIT,
	@Resultado BIT OUTPUT,
	@Mensaje VARCHAR(500) OUTPUT
)
AS
BEGIN
	SET @Resultado = 1

	IF NOT EXISTS (SELECT * FROM CATEGORIA WHERE Descripcion = @Descripcion AND IdCategoria != @IdCategoria)
		UPDATE CATEGORIA SET 
		Descripcion = @Descripcion,
		Estado = @Estado
		WHERE IdCategoria = @IdCategoria
	ELSE
		BEGIN
			SET @Resultado = 0
			SET @Mensaje = 'Una categoria ya tiene la misma descripción'
		END

END
GO


	/**Para eliminar categoría**/
CREATE PROC SP_ELIMINARCATEGORIA(
	@IdCategoria INT,
	@Resultado BIT OUTPUT,
	@Mensaje VARCHAR(500) OUTPUT
)
AS
BEGIN
	SET @Resultado = 1

	IF NOT EXISTS (
		SELECT * FROM CATEGORIA c
		INNER JOIN PRODUCTO p ON p.IdCategoria = c.IdCategoria
		WHERE c.IdCategoria = @IdCategoria
	)
	BEGIN
		DELETE TOP(1) FROM CATEGORIA WHERE IdCategoria = @IdCategoria
	END
	ELSE
		BEGIN
			SET @Resultado = 0
			SET @Mensaje = 'Esta categoria esta relacionada con algún producto'
		END

END
GO


/**PROCEDIMIENTOS PARA PRODUCTO**/
	/**Para agregar producto**/
CREATE PROC SP_REGISTRARPRODUCTO(
	@Codigo VARCHAR(20),
	@Nombre VARCHAR(30),
	@Descripcion VARCHAR(30),
	@IdCategoria INT,
	@Estado BIT,
	@Resultado INT OUTPUT,
	@Mensaje VARCHAR(500) OUTPUT
)
AS
BEGIN
	SET @Resultado = 0
	IF NOT EXISTS (SELECT * FROM PRODUCTO WHERE Codigo = @Codigo)
	BEGIN
		INSERT INTO PRODUCTO(Codigo, Nombre, Descripcion, IdCategoria, Estado) VALUES (@Codigo, @Nombre, @Descripcion, @IdCategoria, @Estado)
		SET @Resultado = SCOPE_IDENTITY()
	END
	ELSE
		SET @Mensaje = 'Un producto ya tiene el mismo código'
END
GO


	/**Para editar producto**/
CREATE PROC SP_EDITARPRODUCTO(
	@IdProducto INT,
	@Codigo VARCHAR(20),
	@Nombre VARCHAR(30),
	@Descripcion VARCHAR(30),
	@IdCategoria INT,
	@Estado BIT,
	@Resultado BIT OUTPUT,
	@Mensaje VARCHAR(500) OUTPUT
)
AS
BEGIN
	SET @Resultado = 1
	IF NOT EXISTS (SELECT * FROM PRODUCTO WHERE Codigo = @Codigo AND IdProducto != @IdProducto)
		UPDATE PRODUCTO SET
		Codigo = @Codigo,
		Nombre = @Nombre,
		Descripcion = @Descripcion,
		IdCategoria = @IdCategoria,
		Estado = @Estado
		WHERE IdProducto = @IdProducto
	ELSE
	BEGIN
		SET @Resultado  = 0
		SET @Mensaje = 'Un producto ya tiene el mismo código'
	END
END
GO


	/**Para eliminar producto**/
CREATE PROC SP_ELIMINARPRODUCTO(
	@IdProducto INT,
	@Respuesta BIT OUTPUT,
	@Mensaje VARCHAR(500) OUTPUT
)
AS
BEGIN
	SET @Respuesta = 0
	SET @Mensaje = ''
	DECLARE @pasoreglas BIT = 1

	IF EXISTS (SELECT * FROM DETALLE_COMPRA dc
		INNER JOIN PRODUCTO p ON p.IdProducto = dc.IdProducto
		WHERE p.IdProducto = @IdProducto
	)
	BEGIN
		SET @pasoreglas = 0
		SET @Respuesta = 0
		SET @Mensaje = @Mensaje + 'Este producto está relacionado con alguna compra'
	END

	IF EXISTS (SELECT * FROM DETALLE_VENTA dv
		INNER JOIN PRODUCTO p ON p.IdProducto = dv.IdProducto
		WHERE p.IdProducto = @IdProducto
	)
	BEGIN
		SET @pasoreglas = 0
		SET @Respuesta = 0
		SET @Mensaje = @Mensaje + 'Este producto está relacionado con alguna venta'
	END

	IF (@pasoreglas = 1)
	BEGIN
		DELETE FROM PRODUCTO WHERE IdProducto = @IdProducto
		SET @Respuesta = 1
	END
END
GO


/**PROCEDIMIENTOS PARA CLIENTE**/
	/**Para agregar cliente**/
CREATE PROC SP_REGISTRARCLIENTE(
	@Documento VARCHAR(50),
	@NombreCompleto VARCHAR(50),
	@Correo VARCHAR(50),
	@Telefono VARCHAR(50),
	@Estado BIT,
	@Resultado INT OUTPUT,
	@Mensaje VARCHAR(500) OUTPUT
)
AS
BEGIN
	SET @Resultado = 0
	DECLARE @IDPERSONA INT
	IF NOT EXISTS (SELECT * FROM CLIENTE WHERE Documento = @Documento)
	BEGIN 
		INSERT INTO CLIENTE(Documento, NombreCompleto, Correo, Telefono, Estado) 
		VALUES (@Documento, @NombreCompleto, @Correo, @Telefono, @Estado)

		SET @Resultado = SCOPE_IDENTITY()
	END
	ELSE
		SET @Mensaje = 'Ya existe este número de DNI'
END
GO


	/**Para editar cliente**/
CREATE PROC SP_EDITARCLIENTE(
	@IdCliente INT,
	@Documento VARCHAR(50),
	@NombreCompleto VARCHAR(50),
	@Correo VARCHAR(50),
	@Telefono VARCHAR(50),
	@Estado BIT,
	@Resultado BIT OUTPUT,
	@Mensaje VARCHAR(500) OUTPUT
)
AS
BEGIN
	SET @Resultado = 1
	DECLARE @IDPERSONA INT

	IF NOT EXISTS (SELECT * FROM CLIENTE WHERE Documento = @Documento AND IdCliente != @IdCliente)
	BEGIN
		UPDATE CLIENTE SET
			Documento = @Documento,
			NombreCompleto = @NombreCompleto,
			Correo = @Correo,
			Telefono = @Telefono,
			Estado = @Estado
		WHERE IdCliente = @IdCliente
	END
	ELSE
	BEGIN
		SET @Resultado = 0
		SET @Mensaje = 'Ya existe este número de DNI'
	END
END
GO


	/**Para eliminar cliente**/
CREATE PROC SP_ELIMINARCLIENTE(
	@IdCliente INT,
	@Mensaje VARCHAR(500) OUTPUT
)
AS
BEGIN
	DELETE FROM CLIENTE WHERE IdCliente = @IdCliente
	SET @Mensaje = 'Cliente Eliminado con éxito!'
END
GO


/**PROCEDIMIENTOS PARA PROVEEDOR**/
	/**Para agregar proveedor**/
CREATE PROC SP_REGISTRARPROVEEDOR(
	@Documento VARCHAR(50),
	@RazonSocial VARCHAR(50),
	@Correo VARCHAR(50),
	@Telefono VARCHAR(50),
	@Estado BIT,
	@Resultado INT OUTPUT,
	@Mensaje VARCHAR(500) OUTPUT
)
AS
BEGIN
	SET @Resultado = 0
	DECLARE @IDPERSONA INT

	IF NOT EXISTS(SELECT * FROM PROVEEDOR WHERE Documento = @Documento)
	BEGIN
		INSERT INTO PROVEEDOR(Documento, RazonSocial, Correo, Telefono, Estado)
		VALUES (@Documento, @RazonSocial, @Correo, @Telefono, @Estado)

		SET @Resultado = SCOPE_IDENTITY()
	END
	ELSE
		SET @Mensaje = 'Ya existe este número de Documento'
END
GO


	/**Para editar proveedor**/
CREATE PROC SP_EDITARPROVEEDOR(
	@IdProveedor INT,
	@Documento VARCHAR(50),
	@RazonSocial VARCHAR(50),
	@Correo VARCHAR(50),
	@Telefono VARCHAR(50),
	@Estado BIT,
	@Resultado BIT OUTPUT,
	@Mensaje VARCHAR (500) OUTPUT
)
AS
BEGIN
	SET @Resultado = 1
	DECLARE @IDPERSONA INT

	IF NOT EXISTS (SELECT * FROM PROVEEDOR WHERE Documento = @Documento AND IdProveedor != @IdProveedor)
	BEGIN
		UPDATE PROVEEDOR SET
		Documento = @Documento,
		RazonSocial = @RazonSocial,
		Correo = @Correo,
		Telefono = @Telefono,
		Estado = @Estado
		WHERE IdProveedor = @IdProveedor
		
	END
	ELSE
	BEGIN
		SET @Resultado = 0
		SET @Mensaje = 'Ya existe este número de Documento'
	END
END
GO


	/**Para eliminar proveedor**/
CREATE PROCEDURE SP_ELIMINARPROVEEDOR(
	@IdProveedor INT,
	@Resultado BIT OUTPUT,
	@Mensaje VARCHAR (500) OUTPUT
)
AS
BEGIN
	SET @Resultado = 1
	IF NOT EXISTS (SELECT * FROM PROVEEDOR p
		INNER JOIN COMPRA c ON p.IdProveedor = c.IdProveedor
		WHERE p.IdProveedor = @IdProveedor
	)
	BEGIN
		DELETE TOP(1) FROM PROVEEDOR WHERE IdProveedor = @IdProveedor
	END
	ELSE
	BEGIN
		SET @Resultado = 0
		SET @Mensaje = 'Este proveedor está relacionado con alguna compra'
	END
END
GO


/**PROCEDIMIENTOS PARA COMPRA**/
	/**Parámetro**/
create TYPE [dbo].[EDetalle_Compra] AS TABLE(
	[IdProducto] INT NULL,
	[PrecioCompra] DECIMAL(18,2) NULL,
	[PrecioVenta] DECIMAL (18,2) NULL,
	[Cantidad] INT NULL,
	[MontoTotal] DECIMAL (18,2) NULL
)
GO


	/**Para agregar compra**/
CREATE PROC SP_REGISTRARCOMPRA(
	@IdUsuario INT,
	@IdProveedor INT,
	@TipoDocumento VARCHAR(500),
	@NumeroDocumento VARCHAR(500),
	@MontoTotal DECIMAL(18,2),
	@DetalleCompra [EDetalle_Compra] READONLY,
	@Resultado BIT OUTPUT,
	@Mensaje VARCHAR(500) OUTPUT
)
AS
BEGIN
	BEGIN TRY
		DECLARE @IdCompra INT = 0
		SET @Resultado = 1
		SET @Mensaje = ''

		BEGIN TRANSACTION Registro
			
			INSERT INTO COMPRA(IdUsuario, IdProveedor, TipoDocumento, NumeroDocumento, MontoTotal)
			VALUES (@IdUsuario, @IdProveedor, @TipoDocumento, @NumeroDocumento, @MontoTotal)
			
			SET @IdCompra = SCOPE_IDENTITY()


			INSERT INTO DETALLE_COMPRA(IdCompra, IdProducto, PrecioCompra, PrecioVenta, Cantidad, MontoTotal)
			SELECT @IdCompra, IdProducto, PrecioCompra, PrecioVenta, Cantidad, MontoTotal FROM @DetalleCompra



			UPDATE p SET p.Stock = p.Stock + dc.Cantidad,
			p.PrecioCompra = dc.PrecioCompra,
			p.PrecioVenta = dc.PrecioVenta
			FROM PRODUCTO p
			INNER JOIN @DetalleCompra dc ON dc.IdProducto = p.IdProducto


		COMMIT TRANSACTION Registro

	END TRY
	BEGIN CATCH
		SET @Resultado = 0
		SET @Mensaje = SCOPE_IDENTITY()

		ROLLBACK TRANSACTION Registro
	END CATCH
	
END
GO


/**PROCEDIMIENTOS PARA VENTA**/
	/**Parámetro**/
CREATE TYPE [dbo].[EDetalle_Venta] AS TABLE(
	[IdProducto] INT NULL,
	[PrecioVenta] DECIMAL(18,2) NULL,
	[Cantidad] INT NULL,
	[SubTotal] DECIMAL(18,2) NULL
)
GO


	/**Para agregar venta**/
CREATE PROC SP_REGISTRARVENTA(
	@IdUsuario INT,
	@TipoDocumento VARCHAR(500),
	@NumeroDocumento VARCHAR(500),
	@DocumentoCliente VARCHAR(500),
	@NombreCliente VARCHAR(500),
	@MontoPago DECIMAL(18,2),
	@MontoCambio DECIMAL(18,2),
	@MontoTotal DECIMAL(18,2),
	@DetalleVenta [EDetalle_Venta] READONLY,
	@Resultado BIT OUTPUT,
	@Mensaje VARCHAR(500) OUTPUT
)
AS
BEGIN
	BEGIN TRY
		DECLARE @IdVenta INT = 0
		SET @Resultado = 1
		SET @Mensaje = ''

		BEGIN TRANSACTION Registro
			INSERT INTO VENTA(IdUsuario, TipoDocumento, NumeroDocumento, DocumentoCliente, NombreCliente, MontoPago, MontoCambio, MontoTotal)
			VALUES (@IdUsuario, @TipoDocumento, @NumeroDocumento, @DocumentoCliente, @NombreCliente, @MontoPago, @MontoCambio, @MontoTotal)

			SET @IdVenta = SCOPE_IDENTITY()

			INSERT INTO DETALLE_VENTA(IdVenta, IdProducto, PrecioVenta, Cantidad, SubTotal)
			SELECT @IdVenta, IdProducto, PrecioVenta, Cantidad, SubTotal FROM @DetalleVenta

		COMMIT TRANSACTION Registro
	END TRY

	BEGIN CATCH
		SET @Resultado = 0
		SET @Mensaje = ERROR_MESSAGE()
		ROLLBACK TRANSACTION Registro
	END CATCH
END
GO


/**PROCEDIMIENTOS PARA REPORTE DE COMPRAS**/
CREATE PROC SP_REPORTECOMPRAS(
	@FechaInicio VARCHAR(10),
	@FechaFin VARCHAR(10),
	@IdProveedor INT
)
AS
BEGIN
	SET DATEFORMAT dmy;

	SELECT
	CONVERT(char(10), c.FechaRegistro, 103)[FechaRegistro], c.TipoDocumento, c.NumeroDocumento, c.MontoTotal,
	u.NombreCompleto[UsuarioRegistro],
	pr.Documento[DocumentoProveedor], pr.RazonSocial,
	p.Codigo[CodigoProducto], p.Nombre[NombreProducto], ca.Descripcion[Categoria], dc.PrecioCompra, dc.PrecioVenta, dc.Cantidad, dc.MontoTotal[SubTotal]
	FROM COMPRA c
	INNER JOIN USUARIO u ON u.IdUsuario = c.IdUsuario
	INNER JOIN PROVEEDOR pr ON pr.IdProveedor = c.IdProveedor
	INNER JOIN DETALLE_COMPRA dc ON dc.IdCompra = c.IdCompra
	INNER JOIN PRODUCTO p ON p.IdProducto = dc.IdProducto
	INNER JOIN CATEGORIA ca ON ca.IdCategoria = p.IdCategoria
	WHERE CONVERT(DATE, c.FechaRegistro) 
	BETWEEN @FechaInicio 
	AND @FechaFin
	AND pr.IdProveedor = IIF(@IdProveedor = 0, pr.IdProveedor, @IdProveedor)

END
GO


/**PROCEDIMIENTOS PARA REPORTE DE VENTAS**/
CREATE PROC SP_REPORTEVENTAS(
	@FechaInicio VARCHAR(10),
	@FechaFin VARCHAR(10)
)
AS
BEGIN
	SET DATEFORMAT dmy;

	SELECT
	CONVERT(char(10), v.FechaRegistro, 103)[FechaRegistro], v.TipoDocumento, v.NumeroDocumento, v.MontoTotal,
	u.NombreCompleto[UsuarioRegistro],
	v.DocumentoCliente, v.NombreCliente,
	p.Codigo[CodigoProducto], p.Nombre[NombreProducto], 
	ca.Descripcion[Categoria], 
	dv.PrecioVenta, dv.Cantidad, dv.SubTotal
	FROM VENTA v
	INNER JOIN USUARIO u ON u.IdUsuario = v.IdUsuario
	INNER JOIN DETALLE_VENTA dv ON dv.IdVenta = v.IdVenta
	INNER JOIN PRODUCTO p ON p.IdProducto = dv.IdProducto
	INNER JOIN CATEGORIA ca ON ca.IdCategoria = p.IdCategoria
	WHERE CONVERT(date, v.FechaRegistro) BETWEEN @FechaInicio AND @FechaFin

END
GO
