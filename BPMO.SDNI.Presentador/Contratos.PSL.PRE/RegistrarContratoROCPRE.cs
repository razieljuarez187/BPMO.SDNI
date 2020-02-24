using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
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
using BPMO.SDNI.Contratos.PSL.BO;
using BPMO.SDNI.Contratos.PSL.BOF;
using BPMO.SDNI.Contratos.PSL.BR;
using BPMO.SDNI.Contratos.PSL.RPT;
using BPMO.SDNI.Contratos.PSL.VIS;
using BPMO.SDNI.Equipos.BO;
using BPMO.SDNI.Facturacion.AplicacionesPago.BR;
using BPMO.SDNI.Facturacion.InfraestructuraPagos.BO;

namespace BPMO.SDNI.Contratos.PSL.PRE {
    /// <summary>
    /// Presentador de Registro de Contrato RO
    /// </summary>
    public class RegistrarContratoROCPRE {
        #region Atributos
        /// <summary>
        /// Contexto de conexión de datos
        /// </summary>
        private IDataContext dctx = null;
        /// <summary>
        /// Controlador Principal
        /// </summary>
        private ContratoPSLBR controlador;
        /// <summary>
        /// Vista del Registro de Contrato
        /// </summary>
        private IRegistrarContratoROCVIS vista;
        /// <summary>
        /// Presentador de los Datos del Contrato
        /// </summary>
        private ucContratoPSLPRE presentadorDatosContrato;
        /// <summary>
        /// Presentador del Control de Documentos
        /// </summary>
        private ucCatalogoDocumentosPRE presentadorDocumentos;
        /// <summary>
        /// Presentador de Línea de Contrato
        /// </summary>
        private readonly ucLineaContratoPSLPRE lineaContratoPRE;
        /// <summary>
        /// Nombre de la Clase
        /// </summary>
        private string nombreClase = "RegistrarContratoROCPRE";
        #endregion

        #region Constructores
        /// <summary>
        /// Constructor por defecto del Presentador
        /// </summary>
        /// <param name="view">Vista de Registro de Contrato</param>
        /// <param name="viewDatosContrato">Vista de los Datos del Contrato</param>
        /// <param name="viewDocumentos">Vista del Control de Documentos</param>
        public RegistrarContratoROCPRE(IRegistrarContratoROCVIS view, IucContratoPSLVIS viewDatosContrato, IucCatalogoDocumentosVIS viewDocumentos, IucLineaContratoPSLVIS viewLineas) {
            try {
                this.vista = view;

                this.presentadorDatosContrato = new ucContratoPSLPRE(viewDatosContrato);
                this.presentadorDocumentos = new ucCatalogoDocumentosPRE(viewDocumentos);
                this.lineaContratoPRE = new ucLineaContratoPSLPRE(viewLineas);

                this.controlador = new ContratoPSLBR();
                this.dctx = FacadeBR.ObtenerConexion();

                //Indicamos al uc el tipo de contrato que se desea dar de alta
                this.presentadorDatosContrato.Vista.EsROC = true;
            } catch (Exception ex) {
                this.vista.MostrarMensaje("Inconsistencia en los parámetros de configuración", ETipoMensajeIU.ERROR, this.nombreClase + ".RegistrarContratoROCPRE:" + ex.Message);
            }
        }
        #endregion

        #region Métodos
        /// <summary>
        /// Prepara la Vista para un Nuevo Registro de Contrato
        /// </summary>
        public void PrepararNuevo() {
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
        private void EstablecerInformacionInicial() {
            //Se establecen los tipos de archivos permitidos para adjuntar al contrato
            List<TipoArchivoBO> lstTiposArchivo = new TipoArchivoBR().Consultar(this.dctx, new TipoArchivoBO());
            this.presentadorDocumentos.EstablecerTiposArchivo(lstTiposArchivo);
            this.presentadorDocumentos.RequiereObservaciones(false);

            #region Estableciendo configuración de días
            DiaPeriodoTarifaBR DiaPerioBR = new DiaPeriodoTarifaBR();
            this.vista.IncluyeSD = DiaPerioBR.IncluyeSabadoDomingo(this.dctx, (int)this.vista.UnidadOperativaID);
            #endregion
            this.EstablecerProductoSATPredeterminado();
        }
        /// <summary>
        /// Precarga el ProductoServicio SAT Configurado como predeterminado
        /// </summary>
        private void EstablecerProductoSATPredeterminado() {
            string clvProdSat = this.vista.ClaveProductoSATPredeterminado;
            if (string.IsNullOrWhiteSpace(clvProdSat)) return;

            CatalogoBaseBO producto = FacadeBR.ConsultarProductoServicio(this.dctx, new ProductoServicioBO() { NombreCorto = clvProdSat }).FirstOrDefault();
            if (producto != null) {
                this.vista.ProductoServicioId = producto.Id;
                this.vista.ClaveProductoServicio = producto.NombreCorto;
                this.vista.DescripcionProductoServicio = producto.Nombre;
            }
        }
        /// <summary>
        /// Establece las opciones permitidas en base a la seguridad del usuario
        /// </summary>
        private void EstablecerSeguridad() {
            try {
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

            } catch (Exception ex) {
                throw new Exception(this.nombreClase + ".EstablecerSeguridad:" + ex.Message);
            }
        }
        /// <summary>
        /// Verifica si existe una acción en una lista de acciones proporcionada
        /// </summary>
        /// <param name="acciones">Lista de Acciones</param>
        /// <param name="nombreAccion">Acción a verificar</param>
        /// <returns></returns>
        private bool ExisteAccion(List<CatalogoBaseBO> acciones, string nombreAccion) {
            if (acciones != null && acciones.Count > 0 && acciones.Exists(p => p != null && p.Nombre != null && p.Nombre.Trim().ToUpper() == nombreAccion.Trim().ToUpper()))
                return true;

            return false;
        }

        /// <summary>
        /// Indica si la unidad seleccionada cuenta con una reservación con base en el cliente seleccionado, la fecha del contrato y de promesa de entrega
        /// </summary>
        /// <returns>True en caso de que cuente con una reservación, False en caso contrario</returns>
        public bool UnidadTieneReservacion() {
            try {
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

                //Se consulta si existen reservaciones conflictivas
                ContratoPSLBOF bof = new ContratoPSLBOF();
                bof.Unidad = new UnidadBO() { UnidadID = this.vista.UnidadID };
                bof.Cliente = new CuentaClienteIdealeaseBO() { Id = this.vista.CuentaClienteID };
                bof.FechaContrato = this.vista.FechaContrato;
                bof.FechaPromesaDevolucion = this.vista.FechaPromesaDevolucion;
                bof.Sucursal = new SucursalBO() { UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaID } };
                List<ReservacionPSLBO> lstConflictivas = this.controlador.ConsultarReservacionesConflictivas(dctx, bof);

                if (lstConflictivas != null && lstConflictivas.Count > 0)
                    return true;

                return false;
            } catch (Exception ex) {
                throw new Exception(this.nombreClase + ".UnidadTieneReservacion: " + "No fue posible comprobar que la unidad está reservada debido a que " + ex.Message);
            }
        }

        /// <summary>
        /// Cancela la captura del contrato
        /// </summary>
        public void CancelarRegistro() {
            this.LimpiarSesion();
            this.vista.RedirigirAConsulta();
        }
        /// <summary>
        /// Registra el contrato con estatus Borrador
        /// </summary>
        public void RegistrarBorrador() {
            string s;
            if ((s = this.ValidarCamposBorrador()) != null) {
                this.vista.MostrarMensaje(s, ETipoMensajeIU.ADVERTENCIA, null);
                return;
            }

            this.vista.EstatusID = (int)EEstatusContrato.Borrador;

            this.Registrar();

            this.vista.EstablecerPaqueteNavegacion("ContratoPSLBO", new ContratoPSLBO() { ContratoID = this.vista.ContratoID });
            this.vista.RedirigirADetalles();
        }
        /// <summary>
        /// Registra el contrato con Estatus En Curso
        /// </summary>
        public void RegistrarTerminada() {
            string s;
            if ((s = this.ValidarCamposRegistro()) != null) {
                this.vista.MostrarMensaje(s, ETipoMensajeIU.ADVERTENCIA, null);
                return;
            }

            this.vista.EstatusID = (int)EEstatusContrato.EnPausa;

            this.Registrar();

            this.vista.EstablecerPaqueteNavegacion("ContratoPSLBO", new ContratoPSLBO() { ContratoID = this.vista.ContratoID });
            this.vista.RedirigirADetalles();
        }
        /// <summary>
        /// Registra un Contrato RO
        /// </summary>
        private void Registrar() {
            #region Se inicia la Transaccion
            dctx.SetCurrentProvider("Outsourcing");
            Guid firma = Guid.NewGuid();
            try {
                dctx.OpenConnection(firma);
                dctx.BeginTransaction(firma);
            } catch (Exception) {
                if (dctx.ConnectionState == ConnectionState.Open)
                    dctx.CloseConnection(firma);
                throw new Exception("Se encontraron inconsistencias al insertar el Contrato.");
            }
            #endregion

            try {
                //Se obtiene la información a partir de la Interfaz de Usuario
                ContratoPSLBO bo = (ContratoPSLBO)this.InterfazUsuarioADato();
                if (bo.Estatus == EEstatusContrato.EnPausa) {
                    #region LLenar el anexo por cambios en la renovación.
                    AnexosContratoPSLBO anexo = new AnexosContratoPSLBO();
                    anexo.FechaInicio = bo.FechaInicioActual;
                    anexo.FechaFin = bo.FechaPromesaActual;
                    anexo.FechaIntercambioUnidad = null;
                    anexo.MontoTotalArrendamiento = bo.MontoTotalArrendamiento;
                    anexo.TipoAnexoContrato = ETipoAnexoContrato.Contrato;
                    anexo.Vigente = true;
                    anexo.ContratoPSLID = this.vista.ContratoID;
                    anexo.FC = this.vista.FUA;
                    anexo.FUA = this.vista.FUA;
                    anexo.UC = this.vista.UUA;
                    anexo.UUA = this.vista.UUA;

                    ArchivoBO archivo = new ArchivoBO();
                    archivo.Activo = true;
                    archivo.Auditoria = new AuditoriaBO { FC = this.vista.FUA, FUA = this.vista.FUA, UC = this.vista.UUA, UUA = this.vista.UUA };
                    archivo.TipoAdjunto = ETipoAdjunto.AnexoContrato;
                    archivo.TipoArchivo = this.presentadorDocumentos.Vista.TiposArchivo.Find(x => x.Extension.ToUpper() == "PDF");

                    Random random = new Random();
                    archivo.Nombre = "Anexo_" + DateTime.Now.ToShortDateString().Replace("/", string.Empty) + random.Next(99999).ToString() + ".pdf";
                    archivo.NombreCorto = archivo.Nombre;

                    Dictionary<string, object> dataSources = new Dictionary<string, object>();
                    #region Consulta periodoTarifario. 
                    List<DiaPeriodoTarifaBO> lstTemp = new List<DiaPeriodoTarifaBO>();
                    DiaPeriodoTarifaBR periodoTarifaBR = new DiaPeriodoTarifaBR();
                    lstTemp = periodoTarifaBR.Consultar(dctx, new DiaPeriodoTarifaBO() { UnidadOperativaID = this.vista.UnidadOperativaID });
                    if (lstTemp.Count == 1)
                        dataSources.Add("PeriodoTarifa", lstTemp[0]);
                    if (!string.IsNullOrEmpty(bo.Divisa.MonedaDestino.Codigo)) {
                        MonedaBO moneda = FacadeBR.ConsultarMoneda(this.dctx, new MonedaBO() { Codigo = bo.Divisa.MonedaDestino.Codigo }).FirstOrDefault();
                        dataSources.Add("Moneda", moneda);
                    }
                    #endregion
                    dataSources["ContratoPSLBO"] = bo;
                    ContratoAnexoROCRPT reporteCorrectivo = new ContratoAnexoROCRPT(dataSources);
                    using (MemoryStream stream = new MemoryStream()) {
                        reporteCorrectivo.CreateDocument();
                        reporteCorrectivo.ExportToPdf(stream);
                        archivo.Archivo = stream.GetBuffer();
                    }
                    anexo.AgregarAnexo(archivo);
                    bo.AgregarAnexoContrato(anexo);
                    #endregion

                    #region Llenando información del pago
                    GeneradorPagoPSLBR generadorPago = new GeneradorPagoPSLBR();
                    //Cuando se genera el pago se llenan los campos de acumulados de tarifas
                    generadorPago.ObtenerPagos(new PagoContratoPSLBO(), bo, null, 0, ETipoPago.NORMAL);
                    #endregion
                }

                //Se crea el objeto de seguridad
                UsuarioBO usuario = new UsuarioBO() { Id = this.vista.UsuarioID };
                AdscripcionBO adscripcion = new AdscripcionBO() { UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaID } };
                SeguridadBO seguridadBO = new SeguridadBO(Guid.Empty, usuario, adscripcion);

                //Se inserta en la base de datos
                this.controlador.InsertarCompleto(this.dctx, bo, seguridadBO);
                //Se consulta lo insertado para recuperar los ID
                DataSet ds = this.controlador.ConsultarSet(this.dctx, bo);
                if (ds.Tables[0].Rows.Count <= 0)
                    throw new Exception("Al consultar lo insertado no se encontraron coincidencias.");
                if (ds.Tables[0].Rows.Count > 1)
                    throw new Exception("Al consultar lo insertado se encontró más de una coincidencia.");

                bo.ContratoID = this.controlador.DataRowToContratoPSLBO(ds.Tables[0].Rows[0]).ContratoID;

                //El proceso de generación de pagos es eliminado de esta sección               
                dctx.CommitTransaction(firma);

                //Se despliega la información en la Interfaz de Usuario
                this.DatoAInterfazUsuario(bo);
            } catch (Exception ex) {
                dctx.RollbackTransaction(firma);
                throw new Exception(this.nombreClase + ".Registrar:" + ex.Message);
            } finally {
                if (dctx.ConnectionState == ConnectionState.Open)
                    dctx.CloseConnection(firma);
            }
        }
        /// <summary>
        /// Obtiene los datos de la Vista
        /// </summary>
        /// <returns></returns>
        private object InterfazUsuarioADato() {
            UnidadOperativaBO unidadOperativa = new UnidadOperativaBO();
            unidadOperativa.Empresa = new EmpresaBO();

            if (this.vista.UnidadOperativaID != null)
                unidadOperativa.Id = this.vista.UnidadOperativaID;

            if (this.vista.UnidadOperativaNombre != null)
                unidadOperativa.Nombre = vista.UnidadOperativaNombre;

            if (this.presentadorDatosContrato.Vista.NombreEmpresa != null)
                unidadOperativa.Empresa.Nombre = this.presentadorDatosContrato.Vista.NombreEmpresa;

            ContratoPSLBO bo = new ContratoPSLBO();
            bo.Cliente = new CuentaClienteIdealeaseBO();
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
            bo.TasaInteres = this.presentadorDatosContrato.Vista.TasaInteres;

            //Configuración de días a cobrar
            bo.IncluyeSD = this.vista.IncluyeSD;

            bo.Tipo = ETipoContrato.ROC;//Se establece el tipo como ROC.

            if (this.vista.SucursalSeleccionada != null) {
                bo.Sucursal.Id = this.vista.SucursalSeleccionada.Id;
                bo.Sucursal.Nombre = this.vista.SucursalSeleccionada.Nombre;
            }

            bo.Sucursal.UnidadOperativa = unidadOperativa;
            bo.Cliente.UnidadOperativa = unidadOperativa;

            if (this.vista.CuentaClienteID != null)
                bo.Cliente.Id = this.vista.CuentaClienteID;
            if (vista.CuentaClienteNombre != null)
                bo.Cliente.Nombre = vista.CuentaClienteNombre;
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
                Id = this.vista.ClienteDireccionId
            };

            bo.Cliente.RemoverDirecciones();
            bo.Cliente.Agregar(direccion);
            #endregion


            if (this.vista.FechaContrato != null)
                bo.FechaContrato = this.vista.FechaContrato;

            if (this.vista.FechaInicioActual != null) {
                bo.FechaInicioActual = this.vista.FechaInicioActual;
                bo.FechaInicioArrendamiento = this.vista.FechaInicioActual;
            }

            if (this.vista.FechaPromesaActual != null) {
                bo.FechaPromesaActual = this.vista.FechaPromesaActual;
                bo.FechaPromesaDevolucion = this.vista.FechaPromesaActual;
            }

            if (this.vista.FormaPagoID != null)
                bo.FormaPago = (EFormaPago)Enum.Parse(typeof(EFormaPago), this.vista.FormaPagoID.ToString());
            if (this.vista.FrecuenciaFacturacionID != null)
                bo.FrecuenciaFacturacion = (EFrecuencia)Enum.Parse(typeof(EFrecuencia), this.vista.FrecuenciaFacturacionID.ToString());
            if (this.vista.AutorizadorTipoConfirmacion != null)
                bo.AutorizadorTipoConfirmacion = this.vista.AutorizadorTipoConfirmacion;
            if (this.vista.AutorizadorOrdenCompra != null)
                bo.AutorizadorOrdenCompra = this.vista.AutorizadorOrdenCompra;
            if (this.vista.DestinoAreaOperacion != null)
                bo.DestinoAreaOperacion = this.vista.DestinoAreaOperacion;
            if (this.vista.MercanciaTransportar != null)
                bo.MercanciaTransportar = this.vista.MercanciaTransportar;
            if (this.vista.TipoConfirmacionID != null)
                bo.TipoConfirmacion = (ETipoConfirmacion)Enum.Parse(typeof(ETipoConfirmacion), this.vista.TipoConfirmacionID.ToString());

            if (this.vista.Observaciones != null)
                bo.Observaciones = this.vista.Observaciones;

            if (this.vista.DiasFacturar != null)
                bo.DiasFacturar = this.vista.DiasFacturar;

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

            if (!string.IsNullOrWhiteSpace(this.vista.ClaveProductoServicio)) {
                if (bo.ProductoServicio == null) bo.ProductoServicio = new ProductoServicioBO();
                bo.ProductoServicio.Id = this.vista.ProductoServicioId;
                bo.ProductoServicio.NombreCorto = this.vista.ClaveProductoServicio;
                bo.ProductoServicio.Nombre = this.vista.DescripcionProductoServicio;
            }

            #region ConfiguracionUO.PorcentajeSeguro
            decimal? porcentajeSeguro = 0;
            ModuloBR configBR = new ModuloBR();
            ConfiguracionUnidadOperativaBO configBO = new ConfiguracionUnidadOperativaBO() { UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaID } };
            List<ConfiguracionUnidadOperativaBO> listConfigUO = new List<ConfiguracionUnidadOperativaBO>();
            listConfigUO = configBR.ConsultarConfiguracionUnidadOperativa(dctx, configBO, this.vista.ModuloID);
            if (listConfigUO != null && listConfigUO.Count > 0)
                porcentajeSeguro = listConfigUO[0].PorcentajeSeguro != null ? listConfigUO[0].PorcentajeSeguro : 0;
            #endregion

            #region Campos de ROC
            bo.MontoTotalArrendamiento = this.vista.MontoTotalArrendamiento;
            bo.FechaPagoRenta = this.vista.FechaPagoRenta;
            bo.Plazo = this.vista.Plazo;
            bo.InversionInicial = this.vista.InversionInicial;
            #endregion

            #region Linea de Contrato
            LineaContratoPSLBO lineaBO = null;
            foreach (LineaContratoPSLBO linea in this.vista.LineasContrato) {
                lineaBO = new LineaContratoPSLBO();

                lineaBO.Equipo = (UnidadBO)linea.Equipo;

                lineaBO.TipoTarifa = linea.TipoTarifa;

                lineaBO.Cobrable = new TarifaContratoPSLBO
                {
                    PeriodoTarifa = ((TarifaContratoPSLBO)linea.Cobrable).PeriodoTarifa,
                    Tarifa = ((TarifaContratoPSLBO)linea.Cobrable).Tarifa,
                    TarifaHrAdicional = ((TarifaContratoPSLBO)linea.Cobrable).TarifaHrAdicional,
                    TarifaTurno = ((TarifaContratoPSLBO)linea.Cobrable).TarifaTurno,
                    Maniobra = ((TarifaContratoPSLBO)linea.Cobrable).Maniobra,
                    TarifaPSLID = ((TarifaContratoPSLBO)linea.Cobrable).TarifaPSLID,
                    DuracionDiasPeriodo = ((TarifaContratoPSLBO)linea.Cobrable).DuracionDiasPeriodo,
                    MaximoHrsTurno = ((TarifaContratoPSLBO)linea.Cobrable).MaximoHrsTurno,
                    Activo = true,
                    PorcentajeDescuento = ((TarifaContratoPSLBO)linea.Cobrable).PorcentajeDescuento,
                    PorcentajeDescuentoMaximo = ((TarifaContratoPSLBO)linea.Cobrable).PorcentajeDescuentoMaximo,
                    EtiquetaDescuento = ((TarifaContratoPSLBO)linea.Cobrable).EtiquetaDescuento,
                    TarifaConDescuento = ((TarifaContratoPSLBO)linea.Cobrable).TarifaConDescuento,
                    PorcentajeSeguro = porcentajeSeguro
                };
                lineaBO.Activo = linea.Activo;
                lineaBO.Devuelta = linea.Devuelta;
                lineaBO.LineaOrigenIntercambioID = linea.LineaOrigenIntercambioID;
                bo.LineasContrato.Add(lineaBO);
            }

            //bo.LineasContrato = this.vista.LineasContrato.ConvertAll(s => (ILineaContrato)s);
            #endregion

            #region Archivos Adjuntos
            List<ArchivoBO> adjuntos = presentadorDocumentos.Vista.ObtenerArchivos() ?? new List<ArchivoBO>();
            foreach (ArchivoBO adjunto in adjuntos) {
                adjunto.TipoAdjunto = ETipoAdjunto.ContratoROC;
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
        private void DatoAInterfazUsuario(object obj) {
            ContratoPSLBO bo = (ContratoPSLBO)obj;

            this.vista.ContratoID = bo.ContratoID;
        }
        /// <summary>
        /// Valida los campos requeridos para un contrato con estatus Borrador
        /// </summary>
        /// <returns></returns>
        public string ValidarCamposBorrador() {
            string s = string.Empty;

            if ((s = this.presentadorDatosContrato.ValidarCamposBorrador()) != null)
                return s;

            if ((s = this.presentadorDatosContrato.ValidarCamposContratoROC(true)) != null)
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
        public string ValidarCamposRegistro() {
            string s = string.Empty;

            if ((s = this.presentadorDatosContrato.ValidarCamposRegistro()) != null)
                return s;

            if ((s = this.presentadorDatosContrato.ValidarCamposContratoROC(true)) != null)
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
        private void LimpiarSesion() {
            this.vista.LimpiarSesion();
            this.presentadorDatosContrato.LimpiarSesion();
            this.presentadorDocumentos.LimpiarSesion();
        }

        /// <summary>
        /// Prepara una nueva línea de contrato para capturar y agregar al contrato
        /// </summary>
        public void PrepararNuevaLinea() {
            try {
                if (presentadorDatosContrato.Vista.DiasRenta != null) {
                    if (presentadorDatosContrato.Vista.UnidadID != null) {
                        if (!presentadorDatosContrato.ExisteUnidadContrato(new UnidadBO { UnidadID = presentadorDatosContrato.Vista.UnidadID })) {
                            if (presentadorDatosContrato.Vista.CodigoMoneda != null) {
                                UnidadBO unidad = presentadorDatosContrato.ObtenerUnidadAgregar();
                                unidad.Sucursal = new SucursalBO
                                {
                                    UnidadOperativa = new UnidadOperativaBO { Id = vista.UnidadOperativaID }
                                };
                                lineaContratoPRE.Vista.ModuloID = this.presentadorDatosContrato.Vista.ModuloID;
                                lineaContratoPRE.Vista.UsuarioID = this.vista.UsuarioID;
                                lineaContratoPRE.Vista.CuentaClienteID = this.presentadorDatosContrato.Vista.CuentaClienteID;
                                lineaContratoPRE.Inicializar(unidad, presentadorDatosContrato.Vista.DiasRenta, presentadorDatosContrato.Vista.CodigoMoneda);
                                vista.CambiarALinea();

                            } else
                                vista.MostrarMensaje("No ha seleccionado la moneda.",
                                                     ETipoMensajeIU.INFORMACION);
                        } else
                            vista.MostrarMensaje("La unidad seleccionada ya existe en el contrato.",
                                                 ETipoMensajeIU.INFORMACION);
                    } else
                        vista.MostrarMensaje("No ha seleccionado una unidad valida para agregar al contrato.",
                                             ETipoMensajeIU.ADVERTENCIA);

                } else
                    vista.MostrarMensaje("Los días de renta deben ser mayor a 0.",
                                         ETipoMensajeIU.INFORMACION);
            } catch (Exception ex) {
                throw new Exception(nombreClase + ".PrepararNuevaLinea: " + ex.Message);
            }
        }

        /// <summary>
        /// Prepara la Línea de contrato para visualización
        /// </summary>
        /// <param name="linea">Línea de Contrato que contiene los datos a mostrar</param>
        public void PrepararLinea(LineaContratoPSLBO linea) {
            try {
                if (presentadorDatosContrato.Vista != null) {
                    if (presentadorDatosContrato.Vista.DiasRenta != null && presentadorDatosContrato.Vista.DiasRenta > 0) {
                        if (presentadorDatosContrato.Vista.CodigoMoneda != null) {
                            lineaContratoPRE.Vista.ModuloID = this.presentadorDatosContrato.Vista.ModuloID;
                            lineaContratoPRE.Vista.UsuarioID = this.vista.UsuarioID;
                            lineaContratoPRE.Vista.CuentaClienteID = this.presentadorDatosContrato.Vista.CuentaClienteID;
                            lineaContratoPRE.Inicializar(linea, presentadorDatosContrato.Vista.DiasRenta, presentadorDatosContrato.Vista.CodigoMoneda);
                            vista.CambiarALinea();
                        } else
                            vista.MostrarMensaje("No ha seleccionado la moneda.",
                                             ETipoMensajeIU.INFORMACION);
                    } else {
                        vista.MostrarMensaje("Los días de renta deben ser mayor a 0.",
                                        ETipoMensajeIU.INFORMACION);
                    }
                } else {
                    vista.MostrarMensaje("No se ha cargado correctamente la vista.",
                                         ETipoMensajeIU.ADVERTENCIA);
                }
            } catch (Exception ex) {
                throw new Exception(nombreClase + ".PrepararLinea:" + ex.Message);
            }
        }
        /// <summary>
        /// Calcula los Pagos Totales y Mensuales
        /// </summary>
        public void CalcularTotales() {
            try {
                decimal maniobra = 0;
                foreach (LineaContratoPSLBO linea in this.vista.LineasContrato) {
                    if (linea.Activo.HasValue && linea.Activo.Value) {
                        if (linea.Cobrable != null) {
                            maniobra += ((TarifaContratoPSLBO)linea.Cobrable).Maniobra.HasValue ? ((TarifaContratoPSLBO)linea.Cobrable).Maniobra.Value : 0;
                        }
                    }
                }
                presentadorDatosContrato.Vista.Maniobra = maniobra;
            } catch (Exception ex) {
                throw new Exception(nombreClase + ".CalcularTotales:" + ex.Message);
            }
        }

        /// <summary>
        /// Cancela el agregar una línea de contrato
        /// </summary>
        public void CancelarLineaContrato() {
            try {
                vista.CambiaAContrato();
                presentadorDatosContrato.InicializarAgregarUnidad();
            } catch (Exception ex) {
                throw new Exception(nombreClase + ".CancelarLineaContrato: " + ex.Message);
            }
        }

        /// <summary>
        /// Pinta de nuevo el grid de líneas de contrato
        /// </summary>
        public void RenderizarGridLineas() {
            try {
                presentadorDatosContrato.Vista.RenderizarGridLineas();
            } catch (Exception ex) {
                throw new Exception(nombreClase + ".CancelarLineaContrato: " + ex.Message);
            }
        }

        /// <summary>
        /// Agrega una Línea de Contrato
        /// </summary>
        public void AgregarLineaContrato() {
            try {

                presentadorDatosContrato.AgregarLineaContrato(lineaContratoPRE.InterfazUsuarioADatos());
                presentadorDatosContrato.InicializarAgregarUnidad();

                CalcularTotales();

                vista.CambiaAContrato();
                presentadorDatosContrato.EstablecerSeleccionarMoneda();
            } catch (Exception ex) {
                throw new Exception(nombreClase + ".AgregarLineaContrato: " + ex.Message);
            }
        }
        public void EliminarTarifaLineasContrato() {
            try {
                foreach (LineaContratoPSLBO linea in vista.LineasContrato) {
                    linea.TipoTarifa = null;
                    (linea.Cobrable) = null;
                }

                CalcularTotales();
                RenderizarGridLineas();
            } catch (Exception ex) {
                throw new Exception(nombreClase + ".AgregarLineaContrato: " + ex.Message);
            }
        }
        #endregion
    }
}
