// Satisface al CU001 - Registrar Contrato de Renta Diaria
// Satisface a la solicitud de Cambio SC0001
// Satisface al Reporte de Inconsistencia RI0008
// Satisface al Reporte de Inconsistencia RI0016
// BEP1401 Satisface a la SC0026
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using BPMO.Basicos.BO;
using BPMO.Facade.SDNI.BR;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Comun.BR;
using BPMO.SDNI.Comun.PRE;
using BPMO.SDNI.Comun.VIS;
using BPMO.SDNI.Contratos.BO;
using BPMO.SDNI.Contratos.RD.BO;
using BPMO.SDNI.Contratos.RD.BOF;
using BPMO.SDNI.Contratos.RD.BR;
using BPMO.SDNI.Contratos.RD.VIS;
using BPMO.SDNI.Equipos.BO;
using BPMO.SDNI.Facturacion.AplicacionesPago.BR;

namespace BPMO.SDNI.Contratos.RD.PRE
{
    /// <summary>
    /// Presentador de Registro de Contrato RD
    /// </summary>
    public class RegistrarContratoRDPRE
    {
        #region Atributos
        /// <summary>
        /// Contexto de conexión de datos
        /// </summary>
        private IDataContext dctx = null;
        /// <summary>
        /// Controlador Principal
        /// </summary>
        private ContratoRDBR controlador;
        /// <summary>
        /// Vista del Registro de Contrato
        /// </summary>
        private IRegistrarContratoRDVIS vista;
        /// <summary>
        /// Presentador de los Datos del Contrato
        /// </summary>
        private ucContratoRDPRE presentadorDatosContrato;
        /// <summary>
        /// Presentador del Control de Documentos
        /// </summary>
        private ucCatalogoDocumentosPRE presentadorDocumentos;
        /// <summary>
        /// Nombre de la Clase
        /// </summary>
        private string nombreClase = "RegistrarContratoRDPRE";
        #endregion

        #region Constructores
        /// <summary>
        /// Constructor por defecto del Presentador
        /// </summary>
        /// <param name="view">Vista de Registro de Contrato</param>
        /// <param name="viewDatosContrato">Vista de los Datos del Contrato</param>
        /// <param name="viewDocumentos">Vista del Control de Documentos</param>
        public RegistrarContratoRDPRE(IRegistrarContratoRDVIS view, IucContratoRDVIS viewDatosContrato, IucCatalogoDocumentosVIS viewDocumentos)
        {
            try
            {
                this.vista = view;

                this.presentadorDatosContrato = new ucContratoRDPRE(viewDatosContrato);
                this.presentadorDocumentos = new ucCatalogoDocumentosPRE(viewDocumentos);

                this.controlador = new ContratoRDBR();
                this.dctx = FacadeBR.ObtenerConexion();
            }
            catch (Exception ex)
            {
                this.vista.MostrarMensaje("Inconsistencia en los parámetros de configuración", ETipoMensajeIU.ERROR, this.nombreClase + ".RegistrarContratoRDPRE:" + ex.Message);
            }
        }
        #endregion

        #region Métodos
        /// <summary>
        /// Prepara la Vista para un Nuevo Registro de Contrato
        /// </summary>
        public void PrepararNuevo()
        {
            this.LimpiarSesion();

            this.vista.PrepararNuevo();
            this.presentadorDatosContrato.PrepararNuevo();
            this.presentadorDocumentos.ModoEditable(true);

            this.EstablecerInformacionInicial();

            this.EstablecerSeguridad();
        }
        /// <summary>
        /// Establece la información Inicial en la Vista
        /// </summary>
        private void EstablecerInformacionInicial()
        {
            //Se establecen los tipos de archivos permitidos para adjuntar al contrato
            List<TipoArchivoBO> lstTiposArchivo = new TipoArchivoBR().Consultar(this.dctx, new TipoArchivoBO());
            this.presentadorDocumentos.EstablecerTiposArchivo(lstTiposArchivo);
            this.presentadorDocumentos.RequiereObservaciones(false);
        }
        /// <summary>
        /// Establece las opciones permitidas en base a la seguridad del usuario
        /// </summary>
        private void EstablecerSeguridad()
        {
            try
            {
                //se valida que los datos del usuario y la unidad operativa no sean nulos
                if (this.vista.UsuarioID == null) throw new Exception("El identificador del usuario no debe ser nulo");
                if (this.vista.UnidadOperativaID == null) throw new Exception("El identificador de la Unidad Operativa no debe ser nulo ");

                //Se crea el objeto de seguridad
                UsuarioBO usr = new UsuarioBO { Id = this.vista.UsuarioID };
                AdscripcionBO sdscripcion = new AdscripcionBO { UnidadOperativa = new UnidadOperativaBO { Id = this.vista.UnidadOperativaID } };
                SeguridadBO seguridadBO = new SeguridadBO(Guid.Empty, usr, sdscripcion);

                //Se obtienen las acciones a las cuales el usuario tiene permiso en este proceso
                List<CatalogoBaseBO> lst = FacadeBR.ConsultarAccion(this.dctx, seguridadBO);

                //Se valida si el usuario tiene permiso para registrar una llanta
                if (!this.ExisteAccion(lst, "INSERTARCOMPLETO"))
                    this.vista.RedirigirSinPermisoAcceso();

                //RI0016, Eliminación de permisos de generar pagos
                #region RI0008                
                #endregion
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".EstablecerSeguridad:" + ex.Message);
            }
        }
        /// <summary>
        /// Verifica si existe una acción en una lista de acciones proporcionada
        /// </summary>
        /// <param name="acciones">Lista de Acciones</param>
        /// <param name="nombreAccion">Acción a verificar</param>
        /// <returns></returns>
        private bool ExisteAccion(List<CatalogoBaseBO> acciones, string nombreAccion)
        {
            if (acciones != null && acciones.Count > 0 && acciones.Exists(p => p != null && p.Nombre != null && p.Nombre.Trim().ToUpper() == nombreAccion.Trim().ToUpper()))
                return true;

            return false;
        }

        /// <summary>
        /// Indica si la unidad seleccionada cuenta con una reservación con base en el cliente seleccionado, la fecha del contrato y de promesa de entrega
        /// </summary>
        /// <returns>True en caso de que cuente con una reservación, False en caso contrario</returns>
        public bool UnidadTieneReservacion()
        {
            try
            {
                if (this.vista.UnidadID == null)
                    return false;
                if (this.vista.FechaContrato == null)
                    return false;
                if (this.vista.FechaPromesaDevolucion == null)
                    return false;
                if (this.vista.CuentaClienteID == null)
                    return false;
                if (this.vista.UnidadOperativaID == null)
                    return false;

                //Se consulta si existen reservaciones conflicitvas
                ContratoRDBOF bof = new ContratoRDBOF();
                bof.Unidad = new UnidadBO() { UnidadID = this.vista.UnidadID };
                bof.Cliente = new CuentaClienteIdealeaseBO() { Id = this.vista.CuentaClienteID };
                bof.FechaContrato = this.vista.FechaContrato;
                bof.FechaPromesaDevolucion = this.vista.FechaPromesaDevolucion;
                bof.Sucursal = new SucursalBO() { UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaID } };
                List<ReservacionRDBO> lstConflictivas = this.controlador.ConsultarReservacionesConflictivas(dctx, bof);

                if (lstConflictivas != null && lstConflictivas.Count > 0)
                    return true;

                return false;
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".UnidadTieneReservacion: " + "No fue posible comprobar que la unidad está reservada debido a que " + ex.Message);
            }
        }

        /// <summary>
        /// Cancela la captura del contrato
        /// </summary>
        public void CancelarRegistro()
        {
            this.LimpiarSesion();
            this.vista.RedirigirAConsulta();
        }
        /// <summary>
        /// Registra el contrato con estatus Borrador
        /// </summary>
        public void RegistrarBorrador()
        {
            string s;
            if ((s = this.ValidarCamposBorrador()) != null)
            {
                this.vista.MostrarMensaje(s, ETipoMensajeIU.ADVERTENCIA, null);
                return;
            }

            this.vista.EstatusID = (int)EEstatusContrato.Borrador;

            this.Registrar();

            this.vista.RedirigirADetalles();
        }
        /// <summary>
        /// Registra el contrato con Estatus En Curso
        /// </summary>
        public void RegistrarTerminada()
        {
            string s;
            if ((s = this.ValidarCamposRegistro()) != null)
            {
                this.vista.MostrarMensaje(s, ETipoMensajeIU.ADVERTENCIA, null);
                return;
            }

            this.vista.EstatusID = (int)EEstatusContrato.EnPausa;

            this.Registrar();

            this.vista.RedirigirADetalles();
        }
        /// <summary>
        /// Registra un Contrato RD
        /// </summary>
        private void Registrar()
        {
            #region Se inicia la Transaccion
            dctx.SetCurrentProvider("Outsourcing");
            Guid firma = Guid.NewGuid();
            try
            {
                dctx.OpenConnection(firma);
                dctx.BeginTransaction(firma);
            }

            catch(Exception)
            {
                if(dctx.ConnectionState == ConnectionState.Open)
                    dctx.CloseConnection(firma);
                throw new Exception("Se encontraron inconsistencias al insertar el Contrato.");
            }
            #endregion

            try
            {
                //Se obtiene la información a partir de la Interfaz de Usuario
                ContratoRDBO bo = (ContratoRDBO)this.InterfazUsuarioADato();

                //Se crea el objeto de seguridad
                UsuarioBO usuario = new UsuarioBO() { Id = this.vista.UsuarioID };
                AdscripcionBO adscripcion = new AdscripcionBO() { UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaID } };
                SeguridadBO seguridadBO = new SeguridadBO(Guid.Empty, usuario, adscripcion);
                
                //Se inserta en la base de datos
                this.controlador.InsertarCompleto(this.dctx, bo, seguridadBO);
                
                dctx.CommitTransaction(firma);

                //Se guarda la información de la Interfaz de Usuario                
                 this.vista.EstablecerPaqueteNavegacion("ContratoRDBO", bo);
           

            }
            catch (Exception ex)
            {
                dctx.RollbackTransaction(firma);
                throw new Exception(this.nombreClase + ".Registrar:" + ex.Message);
            }
            finally
            {
                if(dctx.ConnectionState == ConnectionState.Open)
                    dctx.CloseConnection(firma);
            }
        }
        /// <summary>
        /// Obtiene los datos de la Vista
        /// </summary>
        /// <returns></returns>
        private object InterfazUsuarioADato()
        {
            UnidadOperativaBO unidadOperativa = new UnidadOperativaBO();
            if (this.vista.UnidadOperativaID != null)            
                unidadOperativa.Id = this.vista.UnidadOperativaID;                

            if (this.vista.UnidadOperativaNombre != null)
                unidadOperativa.Nombre = vista.UnidadOperativaNombre;

            ContratoRDBO bo = new ContratoRDBO();
            bo.Cliente = new CuentaClienteIdealeaseBO();
            bo.Cliente.Cliente = new ClienteBO();
            bo.Cliente.UnidadOperativa = new UnidadOperativaBO();
            bo.Divisa = new DivisaBO();
            bo.Divisa.MonedaDestino = new MonedaBO();
            bo.Sucursal = new SucursalBO();
            bo.Sucursal.UnidadOperativa = new UnidadOperativaBO();
            bo.LineasContrato = new List<ILineaContrato>();
            bo.Operador = new OperadorBO();
            bo.Operador.Direccion = new DireccionPersonaBO();
            bo.Operador.Direccion.Ubicacion = new UbicacionBO() { Ciudad = new CiudadBO(), Estado = new EstadoBO(), Municipio = new MunicipioBO(), Pais = new PaisBO() };
            bo.Operador.Licencia = new LicenciaBO();
            bo.Tipo = ETipoContrato.RD;

            if (this.vista.SucursalID != null)
                bo.Sucursal.Id = this.vista.SucursalID;
            if (this.vista.SucursalNombre != null)
                bo.Sucursal.Nombre = this.vista.SucursalNombre;

            bo.Sucursal.UnidadOperativa = unidadOperativa;
            bo.Cliente.UnidadOperativa = unidadOperativa;
            
            if (this.vista.CuentaClienteID != null)
                bo.Cliente.Id = this.vista.CuentaClienteID;
            if (vista.CuentaClienteNombre != null)
                bo.Cliente.Nombre = vista.CuentaClienteNombre;

            bo.Cliente.Cliente.RFC = this.vista.ClienteRFC ?? this.vista.ClienteRFC;
            bo.Cliente.Cliente.Numero = this.vista.CuentaClienteNumeroCuenta?? this.vista.CuentaClienteNumeroCuenta;
            if (this.vista.CodigoMoneda != null)
                bo.Divisa.MonedaDestino.Codigo = this.vista.CodigoMoneda;
            if (this.vista.RepresentantesLegales != null)
                bo.RepresentantesLegales = this.vista.RepresentantesLegales.ConvertAll(s => (PersonaBO)s);
	        bo.SoloRepresentantes = vista.SoloRepresentantes;
	        bo.Avales = vista.Avales;
            #region Dirección del Cliente
            DireccionClienteBO direccion = new DireccionClienteBO
            {
                Ubicacion =
                    new UbicacionBO
                    {
                        Pais = new PaisBO { Codigo = this.vista.ClienteDireccionPais },
                        Municipio = new MunicipioBO { Codigo = this.vista.ClienteDireccionMunicipio },
                        Estado = new EstadoBO { Codigo = this.vista.ClienteDireccionEstado },
                        Ciudad = new CiudadBO { Codigo = this.vista.ClienteDireccionCiudad }
                    },
                CodigoPostal = this.vista.ClienteDireccionCodigoPostal,
                Calle = this.vista.ClienteDireccionCalle,
                Colonia = this.vista.ClienteDireccionColonia,
                Id = this.vista.ClienteDireccionId,
                Primaria = true
            };

            bo.Cliente.RemoverDirecciones();
            bo.Cliente.Agregar(direccion);
            #endregion

            #region Operador
            if (this.vista.OperadorID != null)
                bo.Operador.OperadorID = this.vista.OperadorID;
            if (this.vista.OperadorAniosExperiencia != null)
                bo.Operador.AñosExperiencia = this.vista.OperadorAniosExperiencia;
            if (this.vista.OperadorFechaNacimiento != null)
                bo.Operador.FechaNacimiento = this.vista.OperadorFechaNacimiento;
            if (!string.IsNullOrEmpty(this.vista.OperadorDireccionCalle))
                bo.Operador.Direccion.Calle = this.vista.OperadorDireccionCalle;
            if (!string.IsNullOrEmpty(this.vista.OperadorDireccionCiudad))
                bo.Operador.Direccion.Ubicacion.Ciudad.Nombre = this.vista.OperadorDireccionCiudad;
            if (!string.IsNullOrEmpty(this.vista.OperadorDireccionEstado))
                bo.Operador.Direccion.Ubicacion.Estado.Nombre = this.vista.OperadorDireccionEstado;
            if (!string.IsNullOrEmpty(this.vista.OperadorDireccionCP))
                bo.Operador.Direccion.CodigoPostal = this.vista.OperadorDireccionCP;
            if (!string.IsNullOrEmpty(this.vista.OperadorLicenciaEstado))
                bo.Operador.Licencia.Estado = this.vista.OperadorLicenciaEstado;
            if (this.vista.OperadorLicenciaFechaExpiracion != null)
                bo.Operador.Licencia.FechaExpiracion = this.vista.OperadorLicenciaFechaExpiracion;
            if (!string.IsNullOrEmpty(this.vista.OperadorLicenciaNumero))
                bo.Operador.Licencia.Numero = this.vista.OperadorLicenciaNumero;
            if (this.vista.OperadorLicenciaTipoID != null)
                bo.Operador.Licencia.Tipo = (ETipoLicencia)Enum.Parse(typeof(ETipoLicencia), this.vista.OperadorLicenciaTipoID.ToString());
            if (!string.IsNullOrEmpty(this.vista.OperadorNombre))
                bo.Operador.Nombre = this.vista.OperadorNombre;
            #endregion

            if (this.vista.FechaContrato != null)
                bo.FechaContrato = this.vista.FechaContrato;
            if (this.vista.FechaPromesaDevolucion != null)
                bo.FechaPromesaDevolucion = this.vista.FechaPromesaDevolucion;

            if (this.vista.FormaPagoID != null)
                bo.FormaPago = (EFormaPago)Enum.Parse(typeof(EFormaPago), this.vista.FormaPagoID.ToString());
            if (this.vista.FrecuenciaFacturacionID != null)
                bo.FrecuenciaFacturacion = (EFrecuencia)Enum.Parse(typeof(EFrecuencia), this.vista.FrecuenciaFacturacionID.ToString());
            if (this.vista.AutorizadorTipoConfirmacion != null)
                bo.AutorizadorTipoConfirmacion = this.vista.AutorizadorTipoConfirmacion;
            if (this.vista.AutorizadorOrdenCompra != null)
                bo.AutorizadorOrdenCompra = this.vista.AutorizadorOrdenCompra;
            if (this.vista.BitacoraViajeConductor != null)
                bo.BitacoraViajeConductor = this.vista.BitacoraViajeConductor;
            if (this.vista.DestinoAreaOperacion != null)
                bo.DestinoAreaOperacion = this.vista.DestinoAreaOperacion;
            if (this.vista.LectorKilometrajeID != null)
                bo.LectorKilometraje = (ELectorKilometraje)Enum.Parse(typeof(ELectorKilometraje), this.vista.LectorKilometrajeID.ToString());
            if (this.vista.MercanciaTransportar != null)
                bo.MercanciaTransportar = this.vista.MercanciaTransportar;
            if (this.vista.MotivoRentaID != null)
                bo.MotivoRenta = (EMotivoRenta)Enum.Parse(typeof(EMotivoRenta), this.vista.MotivoRentaID.ToString());
            if (this.vista.PorcentajeDeducible != null)
                bo.PorcentajeDeducible = this.vista.PorcentajeDeducible;
            if (this.vista.TipoConfirmacionID != null)
                bo.TipoConfirmacion = (ETipoConfirmacion)Enum.Parse(typeof(ETipoConfirmacion), this.vista.TipoConfirmacionID.ToString());

            if (this.vista.Observaciones != null)
                bo.Observaciones = this.vista.Observaciones;
            #region BEP1401.SC0026
            if(this.vista.DiasFacturar != null)
                bo.DiasFacturar = this.vista.DiasFacturar;
            #endregion
            if (this.vista.EstatusID != null)
                bo.Estatus = (EEstatusContrato)Enum.Parse(typeof(EEstatusContrato), this.vista.EstatusID.ToString());
            if (this.vista.FC != null)
                bo.FC = this.vista.FC;
            if (this.vista.UC != null)
                bo.UC = this.vista.UC;
            if (this.vista.FUA != null)
                bo.FUA = this.vista.FUA;
            if (this.vista.UUA != null)
                bo.UUA = this.vista.UUA;

            #region Linea de Contrato

            LineaContratoRDBO lineaBO = new LineaContratoRDBO() {Activo = true};

            if (this.vista.EquipoID != null)
            {
                if (lineaBO.Equipo == null) lineaBO.Equipo = new UnidadBO();
                lineaBO.Equipo.EquipoID = this.vista.EquipoID;
            }
            if (this.vista.UnidadID != null)
            {
                if (lineaBO.Equipo == null) lineaBO.Equipo = new UnidadBO();
                ((UnidadBO)lineaBO.Equipo).UnidadID = this.vista.UnidadID;
            }

            if (!string.IsNullOrWhiteSpace(this.vista.ClaveProductoServicio)) {
                if (lineaBO.ProductoServicio == null) lineaBO.ProductoServicio = new ProductoServicioBO();
                lineaBO.ProductoServicio.Id = this.vista.ProductoServicioId;
                lineaBO.ProductoServicio.NombreCorto = this.vista.ClaveProductoServicio;
                lineaBO.ProductoServicio.Nombre = this.vista.DescripcionProductoServicio;
            }

            if (this.vista.TipoTarifaID != null)
                lineaBO.TipoTarifa = (ETipoTarifa)Enum.Parse(typeof(ETipoTarifa), this.vista.TipoTarifaID.ToString());

            lineaBO.Cobrable = new TarifaContratoRDBO()
            {
                RangoTarifas = new List<RangoTarifaRDBO>()
                {
                    new RangoTarifaRDBO()
                },
                TarifasEquipoAliado = new List<TarifaContratoRDEquipoAliadoBO>()
            };

            if(string.IsNullOrEmpty(this.vista.TarifaDescripcion)) {
                ((TarifaContratoRDBO)lineaBO.Cobrable).Descripcion = this.vista.TarifaDescripcion;
            }

            if (this.vista.CapacidadCarga != null)
            {
                ((TarifaContratoRDBO)lineaBO.Cobrable).CapacidadCarga = this.vista.CapacidadCarga;
            }
            if (this.vista.TarifaDiaria != null)
            {
                ((TarifaContratoRDBO)lineaBO.Cobrable).TarifaDiaria = this.vista.TarifaDiaria;
            }
            if (this.vista.KmsLibres != null)
            {
                ((TarifaContratoRDBO)lineaBO.Cobrable).KmsLibres = this.vista.KmsLibres;
                ((TarifaContratoRDBO) lineaBO.Cobrable).RangoTarifas.First().KmRangoInicial = this.vista.KmsLibres + 1;
            }
            if (this.vista.TarifaKmAdicional != null)
            {
                ((TarifaContratoRDBO)lineaBO.Cobrable).RangoTarifas.First().CargoKm = this.vista.TarifaKmAdicional;
            }
            if (this.vista.HrsLibres != null)
            {
                ((TarifaContratoRDBO)lineaBO.Cobrable).HrsLibres = this.vista.HrsLibres;
                ((TarifaContratoRDBO)lineaBO.Cobrable).RangoTarifas.First().HrRangoInicial = this.vista.HrsLibres + 1;
            }
            if (this.vista.TarifaHrAdicional != null)
            {
                ((TarifaContratoRDBO)lineaBO.Cobrable).RangoTarifas.First().CargoHr = this.vista.TarifaHrAdicional;
            }
            if (!String.IsNullOrEmpty(this.vista.NumeroEconomico))
            {
                if(lineaBO.Equipo == null) lineaBO.Equipo = new UnidadBO();
                ((UnidadBO)lineaBO.Equipo).NumeroEconomico = this.vista.NumeroEconomico;
            }
            bo.LineasContrato.Add(lineaBO);
            #endregion

            #region Archivos Adjuntos
            List<ArchivoBO> adjuntos = presentadorDocumentos.Vista.ObtenerArchivos() ?? new List<ArchivoBO>();
            foreach (ArchivoBO adjunto in adjuntos)
            {
                adjunto.TipoAdjunto = ETipoAdjunto.ContratoRD;
                adjunto.Auditoria = new AuditoriaBO
                {

                    FC = this.vista.FC,
                    UC = this.vista.UC,
                    FUA = this.vista.FUA,
                    UUA = this.vista.UUA
                };
            }
            bo.DocumentosAdjuntos = adjuntos;
            #endregion

            return bo;
        }
        /// <summary>
        /// Despliega los datos a la Vista
        /// </summary>
        /// <param name="obj"></param>
        private void DatoAInterfazUsuario(object obj)
        {
            ContratoRDBO bo = (ContratoRDBO)obj;

            this.vista.ContratoID = bo.ContratoID;
        }
        /// <summary>
        /// Valida los campos requeridos para un contrato con estatus Borrador
        /// </summary>
        /// <returns></returns>
        public string ValidarCamposBorrador()
        {
            string s = string.Empty;

            if ((s = this.presentadorDatosContrato.ValidarCamposBorrador()) != null)
                return s;

            if (this.vista.FC == null)
                s += "Fecha de Creación, ";
            if (this.vista.FUA == null)
                s += "Fecha de Última Modificación, ";
            if (this.vista.UC == null)
                s += "Usuario de Creación, ";
            if (this.vista.UUA == null)
                s += "Usuario de Última Modificación, ";

            if (s != null && s.Trim().CompareTo("") != 0)
                return "Los siguientes campos no pueden estar vacíos: \n" + s.Substring(0, s.Length - 2);

            return null;
        }
        /// <summary>
        /// Valida los campos Requeridos para Registrar un Contrato en Curso
        /// </summary>
        /// <returns></returns>
        public string ValidarCamposRegistro()
        {
            string s = string.Empty;

            if ((s = this.presentadorDatosContrato.ValidarCamposRegistro()) != null)
                return s;

            if (this.vista.FC == null)
                s += "Fecha de Creación, ";
            if (this.vista.FUA == null)
                s += "Fecha de Última Modificación, ";
            if (this.vista.UC == null)
                s += "Usuario de Creación, ";
            if (this.vista.UUA == null)
                s += "Usuario de Última Modificación, ";

            if (s != null && s.Trim().CompareTo("") != 0)
                return "Los siguientes campos no pueden estar vacíos: \n" + s.Substring(0, s.Length - 2);

            return null;
        }
        /// <summary>
        /// Limpia los datos de la memoria de la Vista
        /// </summary>
        private void LimpiarSesion()
        {
            this.vista.LimpiarSesion();
            this.presentadorDatosContrato.LimpiarSesion();
            this.presentadorDocumentos.LimpiarSesion();
        }
        #endregion
    }
}
