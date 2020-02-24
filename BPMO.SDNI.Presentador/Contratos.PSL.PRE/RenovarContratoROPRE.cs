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


namespace BPMO.SDNI.Contratos.PSL.PRE {
    /// <summary>
    /// Presentador de la Renovar del Contrato PSL
    /// </summary>
    public class RenovarContratoROPRE {
        #region Atributos
        /// <summary>
        /// Contexto de conexión a los datos
        /// </summary>
        private IDataContext dctx = null;
        /// <summary>
        /// controlador principal
        /// </summary>
        private ContratoPSLBR controlador;
        /// <summary>
        /// Vista de la Edición
        /// </summary>
        private IRenovarContratoROVIS vista;
        /// <summary>
        /// Presentador de los Datos del Contrato
        /// </summary>
        private ucContratoPSLPRE presentadorDatosContrato;
        /// <summary>
        /// Presentador del Control de Documento
        /// </summary>
        private ucCatalogoDocumentosPRE presentadorDocumentos;
        /// <summary>
        /// Presentador de la Barra de Herramientas
        /// </summary>
        private ucHerramientasPSLPRE presentadorHerramientas;
        /// <summary>
        /// Presentador de la Línea de Contrato
        /// </summary>
        private readonly ucLineaContratoPSLPRE lineaContratoPRE;
        /// <summary>
        /// Nombre de la Clase
        /// </summary>
        private string nombreClase = "RenovarContratoROPRE";
        #endregion

        #region Constructores
        /// <summary>
        /// Constructor por defecto del presentador
        /// </summary>
        /// <param name="view">Vista de Edición</param>
        /// <param name="viewDatosContrato">Vista de Datos de Contrato</param>
        /// <param name="viewDocumentos">Vista del Control de Documentos</param>
        /// <param name="viewHerramientas">Vista de la Barra de Herramienta</param>
        public RenovarContratoROPRE(IRenovarContratoROVIS view, IucContratoPSLVIS viewDatosContrato, IucCatalogoDocumentosVIS viewDocumentos, IucHerramientasPSLVIS viewHerramientas, IucLineaContratoPSLVIS viewLineas) {
            try {
                this.vista = view;

                this.presentadorDatosContrato = new ucContratoPSLPRE(viewDatosContrato);
                this.presentadorDocumentos = new ucCatalogoDocumentosPRE(viewDocumentos);
                this.presentadorHerramientas = new ucHerramientasPSLPRE(viewHerramientas);
                this.lineaContratoPRE = new ucLineaContratoPSLPRE(viewLineas);

                this.controlador = new ContratoPSLBR();
                this.dctx = FacadeBR.ObtenerConexion();
            } catch (Exception ex) {
                this.vista.MostrarMensaje("Inconsistencia en los parámetros de configuración", ETipoMensajeIU.ERROR, this.nombreClase + ".RenovarContratoROPRE:" + ex.Message);
            }
        }
        #endregion

        #region Métodos
        /// <summary>
        /// Prepara la primer carga de información del contrato a editar
        /// </summary>
        public void RealizarPrimeraCarga() {
            try {
                ContratoPSLBO contratoSesion = (ContratoPSLBO)this.vista.ObtenerPaqueteNavegacion("UltimoContratoPSLBO");
                if (contratoSesion != null) {
                    this.LimpiarSesion();
                    this.vista.ContratoID = contratoSesion.ContratoID;
                }

                this.presentadorDatosContrato.AsignarModoRegistro("REN");
                this.PrepararRenovacion();
                this.EstablecerConfiguracionInicial();
                this.ConsultarCompleto();
                this.CalcularTotales();

                this.presentadorDocumentos.Vista.OcultarCarga();
                this.presentadorHerramientas.vista.OcultarImprimirPlantilla();
                this.presentadorHerramientas.vista.OcultarImprimirPlantillaCheckList();
                this.presentadorHerramientas.vista.HabilitarOpcionesEdicion();
                this.presentadorHerramientas.vista.OcultarSolicitudPago();
                this.presentadorHerramientas.DeshabilitarMenuCerrar();
                this.presentadorHerramientas.DeshabilitarMenuBorrar();
                this.presentadorHerramientas.DeshabilitarMenuImprimir();
                this.presentadorHerramientas.DeshabilitarMenuDocumentos();
                this.presentadorHerramientas.vista.MarcarOpcionEditarContrato();
                this.presentadorDocumentos.RequiereObservaciones(false);

                this.EstablecerSeguridad();
                this.RecalcularTarifasRenovacion();
            } catch (Exception ex) {
                throw new Exception(this.nombreClase + ".RealizarPrimeraCarga: " + ex.Message);
            }
        }
        /// <summary>
        /// Establece le paquete de datos de navegación
        /// </summary>
        /// <param name="paqueteNavegacion">Paquete de Navegación</param>
        private void EstablecerDatosNavegacion(object paqueteNavegacion) {
            try {
                if (paqueteNavegacion == null)
                    throw new Exception("Se esperaba un objeto en la navegación. No se puede identificar qué contrato se desea consultar.");
                if (!(paqueteNavegacion is ContratoPSLBO))
                    throw new Exception("Se esperaba un Contrato RO y RE.");

                ContratoPSLBO bo = (ContratoPSLBO)paqueteNavegacion;

                this.DatoAInterfazUsuario(bo);
            } catch (Exception ex) {
                this.DatoAInterfazUsuario(new ContratoPSLBO());
                throw new Exception(this.nombreClase + ".EstablecerDatosNavegacion: " + ex.Message);
            }
        }
        /// <summary>
        /// Consulta la información completa del usuario y la despliega en pantalla
        /// </summary>
        private void ConsultarCompleto() {
            try {
                ContratoPSLBO bo = (ContratoPSLBO)this.InterfazUsuarioADato();

                List<ContratoPSLBO> lst = this.controlador.ConsultarCompleto(this.dctx, bo, true);

                if (lst.Count < 1)
                    throw new Exception("No se encontró ningún registro que corresponda a la información proporcionada.");
                if (lst.Count > 1)
                    throw new Exception("La consulta devolvió más de un registro.");

                var obj = (ContratoPSLBO)lst[0];

                this.DatoAInterfazUsuario(obj);
                this.vista.UltimoObjeto = obj;

            } catch (Exception ex) {
                this.DatoAInterfazUsuario(new ContratoPSLBO());
                throw new Exception(this.nombreClase + ".ConsultarCompleto:" + ex.Message);
            }
        }
        /// <summary>
        /// Establece la configuración inicial de la Vista
        /// </summary>
        private void EstablecerConfiguracionInicial() {
            this.vista.FUA = DateTime.Now;
            this.vista.UUA = this.vista.UsuarioID;

            //Se establecen los tipos de archivos permitidos para adjuntar al contrato
            List<TipoArchivoBO> lstTiposArchivo = new TipoArchivoBR().Consultar(this.dctx, new TipoArchivoBO());
            this.presentadorDocumentos.EstablecerTiposArchivo(lstTiposArchivo);
        }
        /// <summary>
        /// Valida el acceso a las funcionalidades del Sistema
        /// </summary>
        public void ValidarAcceso() {
            try {
                //Valida que usuario y la unidad operativa no sean nulos
                if (this.vista.UsuarioID == null) throw new Exception("El identificador del usuario no debe ser nulo");
                if (this.vista.UnidadOperativaID == null) throw new Exception("El identificador de la Unidad Operativa no debe ser nulo");
                //Se crea el objeto de seguridad
                UsuarioBO usuario = new UsuarioBO() { Id = this.vista.UsuarioID };
                AdscripcionBO adscripcion = new AdscripcionBO() { UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaID } };
                SeguridadBO seguridadBO = new SeguridadBO(Guid.Empty, usuario, adscripcion);

                //Se valida si el usuario tiene permiso a la acción principal
                if (!FacadeBR.ExisteAccion(dctx, "ACTUALIZARCOMPLETO", seguridadBO))
                    vista.RedirigirSinPermisoAcceso();

            } catch (Exception ex) {
                throw new Exception(nombreClase + ".ValidarAcceso: " + ex.Message);
            }
        }
        /// <summary>
        /// Establece las opciones permitidas en base a la Seguridad del usuario
        /// </summary>
        public void EstablecerSeguridad() {
            try {
                //Valida que el usuario y la unidad operativa no sean nulos
                if (this.vista.UsuarioID == null) throw new Exception("El identificador del usuario no debe ser nulo");
                if (this.vista.UnidadOperativaID == null)
                    throw new Exception("El identificador de la Unidad Operativa no debe ser nulo");
                // Creación del objeto seguridad
                UsuarioBO usuario = new UsuarioBO { Id = this.vista.UsuarioID };
                AdscripcionBO adscripcion = new AdscripcionBO
                {
                    UnidadOperativa = new UnidadOperativaBO { Id = this.vista.UnidadOperativaID }
                };
                SeguridadBO seguridad = new SeguridadBO(Guid.Empty, usuario, adscripcion);

                //consulta de acciones a la cual el usuario tiene permisos
                List<CatalogoBaseBO> acciones = FacadeBR.ConsultarAccion(dctx, seguridad);

                // se valida si el usuario tiene permisos para registrar
                if (!this.ExisteAccion(acciones, "UI INSERTAR"))
                    this.vista.PermitirRegistrar(false);

                #region RI0008
                //Se valida si el usuario tiene permiso para registrar contrato en curso
                if (!this.ExisteAccion(acciones, "GENERARPAGOS"))
                    vista.PermitirGuardarRenovacion(false);
                #endregion
            } catch (Exception ex) {

                throw new Exception(this.nombreClase + ".EstablecerSeguridad:" + ex.Message);
            }
        }
        /// <summary>
        /// Verifica la acción en un listado de acciones proporcionado
        /// </summary>
        /// <param name="acciones"></param>
        /// <param name="nombreAccion"></param>
        /// <returns></returns>
        private bool ExisteAccion(List<CatalogoBaseBO> acciones, string nombreAccion) {
            if (acciones != null && acciones.Count > 0 &&
                acciones.Exists(
                    p => p != null && p.Nombre != null && p.Nombre.Trim().ToUpper() == nombreAccion.Trim().ToUpper()))
                return true;

            return false;
        }
        /// <summary>
        /// Prepara la Vista para la Edición del Contrato
        /// </summary>
        private void PrepararRenovacion() {
            this.vista.PrepararRenovacion();
            this.presentadorDatosContrato.PrepararRenovacion();
            this.presentadorDocumentos.ModoEditable(false);
            this.presentadorHerramientas.Inicializar();
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
        /// Cancela la edición del contrato
        /// </summary>
        public void CancelarEdicion() {
            this.LimpiarSesion();
            this.vista.LimpiarPaqueteNavegacion("UltimoContratoPSLBO");
            this.vista.EstablecerPaqueteNavegacion("ContratoPSLBO", new ContratoPSLBO() { ContratoID = this.vista.ContratoID });
            this.vista.RedirigirADetalles();
        }
        /// <summary>
        /// Actualiza la información del contrato en curso
        /// </summary>
        public void RenovarTerminada() {
            string s;
            if ((s = this.ValidarCamposRegistro()) != null) {
                this.vista.MostrarMensaje(s, ETipoMensajeIU.ADVERTENCIA, null);
                return;
            }

            this.vista.EstatusID = (int)EEstatusContrato.EnCurso;
            this.Renovar();
            ContratoPSLBO contratoRenovado = (ContratoPSLBO)this.vista.UltimoObjeto;

            this.LimpiarSesion();
            this.vista.LimpiarPaqueteNavegacion("UltimoContratoPSLBO");
            this.vista.EstablecerPaqueteNavegacion("ContratoPSLBO", contratoRenovado);
            this.vista.RedirigirADetalles();
        }
        /// <summary>
        /// Actualiza la información del contrato
        /// </summary>
        private void Renovar() {
            #region Se inicia la Transaccion
            dctx.SetCurrentProvider("Outsourcing");
            Guid firma = Guid.NewGuid();
            try {
                dctx.OpenConnection(firma);
                dctx.BeginTransaction(firma);
            } catch (Exception) {
                if (dctx.ConnectionState == ConnectionState.Open)
                    dctx.CloseConnection(firma);
                throw new Exception("Se encontraron inconsistencias al editar el Contrato.");
            }
            #endregion

            try {
                //Se obtiene la información a partir de la Interfaz de Usuario
                ContratoPSLBO bo = (ContratoPSLBO)this.InterfazUsuarioADato();

                #region LLenar el anexo por cambios en la renovación.
                AnexosContratoPSLBO anexo = new AnexosContratoPSLBO();
                anexo.FechaInicio = bo.FechaInicioActual;
                anexo.FechaFin = bo.FechaPromesaActual;
                anexo.FechaIntercambioUnidad = null;
                anexo.MontoTotalArrendamiento = bo.MontoTotalArrendamiento;
                anexo.TipoAnexoContrato = ETipoAnexoContrato.Renovacion;
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
                dataSources["ContratoPSLBO"] = bo;

                #region Consulta periodoTarifario
                string monedaCodigo = string.Empty;
                List<DiaPeriodoTarifaBO> lstTemp = new List<DiaPeriodoTarifaBO>();
                DiaPeriodoTarifaBR periodoTarifaBR = new DiaPeriodoTarifaBR();
                lstTemp = periodoTarifaBR.Consultar(dctx, new DiaPeriodoTarifaBO() { UnidadOperativaID = this.vista.UnidadOperativaID });
                if (lstTemp.Count == 1)
                    dataSources.Add("PeriodoTarifa", lstTemp[0]);
                if (!string.IsNullOrEmpty(bo.Divisa.MonedaDestino.Codigo)) {
                    MonedaBO moneda = this.presentadorDatosContrato.Vista.ListaMonedas.FirstOrDefault(m => m.Codigo == bo.Divisa.MonedaDestino.Codigo);
                    if (moneda == null)
                        moneda = FacadeBR.ConsultarMoneda(this.dctx, new MonedaBO() { Codigo = bo.Divisa.MonedaDestino.Codigo }).FirstOrDefault();
                    dataSources.Add("Moneda", moneda);
                }
                #endregion
                
                ContratoAnexoROCRPT reporteCorrectivo = new ContratoAnexoROCRPT(dataSources);
                using (MemoryStream stream = new MemoryStream()) {
                    reporteCorrectivo.CreateDocument();
                    reporteCorrectivo.ExportToPdf(stream);
                    archivo.Archivo = stream.GetBuffer();
                }
                anexo.AgregarAnexo(archivo);
                bo.AgregarAnexoContrato(anexo);
                #endregion

                //Se crea el objeto de seguridad
                UsuarioBO usuario = new UsuarioBO() { Id = this.vista.UsuarioID };
                AdscripcionBO adscripcion = new AdscripcionBO() { UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaID } };
                SeguridadBO seguridadBO = new SeguridadBO(Guid.Empty, usuario, adscripcion);

                #region Creando el pago por renovacion
                GeneradorPagoPSLBR generadorPago = new GeneradorPagoPSLBR();
                //Cuando se genera el pago se llenan los campos de acumulados de tarifas
                generadorPago.GenerarPagos(this.dctx, bo, (ContratoPSLBO)this.vista.UltimoObjeto, seguridadBO, 1, true);
                #endregion

                //Se actualiza en la base de datos
                this.controlador.ActualizarCompleto(this.dctx, bo, (ContratoPSLBO)this.vista.UltimoObjeto, seguridadBO);

                dctx.CommitTransaction(firma);
                //Se despliega la información en la Interfaz de Usuario
                this.DatoAInterfazUsuario(bo);
                this.vista.UltimoObjeto = bo;
            } catch (Exception ex) {
                dctx.RollbackTransaction(firma);
                throw new Exception(this.nombreClase + ".Editar:" + ex.Message);
            } finally {
                if (dctx.ConnectionState == ConnectionState.Open)
                    dctx.CloseConnection(firma);
            }
        }
        /// <summary>
        /// Obtiene los datos a partir de la Vista
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
            bo.Cliente = new CuentaClienteIdealeaseBO() { Cliente = new ClienteBO() };
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

            if (this.vista.ContratoID != null)
                bo.ContratoID = this.vista.ContratoID;
            if (!string.IsNullOrWhiteSpace(this.vista.NumeroContrato))
                bo.NumeroContrato = this.vista.NumeroContrato;

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
            bo.Cliente.Numero = this.vista.CuentaClienteNumeroCuenta;
            bo.Cliente.Cliente.Id = this.vista.ClienteID;
            bo.Cliente.Cliente.RFC = this.vista.ClienteRFC;
            bo.Cliente.TipoCuenta = (ETipoCuenta?)this.vista.CuentaClienteTipoID;
            bo.Cliente.EsFisico = this.vista.ClienteEsFisica;

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

            if (this.vista.FechaInicioArrendamiento != null)
                bo.FechaInicioArrendamiento = this.vista.FechaInicioArrendamiento;

            if (this.vista.FechaPromesaDevolucion != null)
                bo.FechaPromesaDevolucion = this.vista.FechaPromesaDevolucion;

            if (this.vista.FechaInicioActual != null)
                bo.FechaInicioActual = this.vista.FechaInicioActual;

            if (this.vista.FechaPromesaActual != null)
                bo.FechaPromesaActual = this.vista.FechaPromesaActual;

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

            //Se define el tipo de contrato como RE, posteriormente se validará las unidades y en caso que todas sean de tipo RE 
            //Se define el tipo de contrato como RE en caso de haber unidades RO y RE o solo RO, el contrato será RO. 
            bo.Tipo = ETipoContrato.RE;
            #region Linea de Contrato
            LineaContratoPSLBO lineaBO = null;
            foreach (LineaContratoPSLBO linea in this.vista.LineasContrato) {
                lineaBO = new LineaContratoPSLBO();
                lineaBO.LineaContratoID = linea.LineaContratoID;
                lineaBO.Equipo = (UnidadBO)linea.Equipo;

                lineaBO.TipoTarifa = linea.TipoTarifa;

                lineaBO.Cobrable = new TarifaContratoPSLBO
                {
                    PeriodoTarifa = ((TarifaContratoPSLBO)linea.Cobrable).PeriodoTarifa,
                    Tarifa = ((TarifaContratoPSLBO)linea.Cobrable).Tarifa,
                    TarifaHrAdicional = ((TarifaContratoPSLBO)linea.Cobrable).TarifaHrAdicional,
                    TarifaTurno = ((TarifaContratoPSLBO)linea.Cobrable).TarifaTurno,
                    Maniobra = ((TarifaContratoPSLBO)linea.Cobrable).Maniobra,
                    TarifaPSLID = null,
                    DuracionDiasPeriodo = ((TarifaContratoPSLBO)linea.Cobrable).DuracionDiasPeriodo,
                    MaximoHrsTurno = ((TarifaContratoPSLBO)linea.Cobrable).MaximoHrsTurno,
                    Activo = true,
                    PorcentajeDescuento = ((TarifaContratoPSLBO)linea.Cobrable).PorcentajeDescuento,
                    PorcentajeDescuentoMaximo = ((TarifaContratoPSLBO)linea.Cobrable).PorcentajeDescuentoMaximo,
                    EtiquetaDescuento = ((TarifaContratoPSLBO)linea.Cobrable).EtiquetaDescuento,
                    TarifaConDescuento = ((TarifaContratoPSLBO)linea.Cobrable).TarifaConDescuento,
                    PorcentajeSeguro = ((TarifaContratoPSLBO)linea.Cobrable).PorcentajeSeguro
                };
                lineaBO.Activo = linea.Activo;
                lineaBO.Devuelta = linea.Devuelta;
                lineaBO.LineaOrigenIntercambioID = linea.LineaOrigenIntercambioID;

                //Si el tipo de contrato es RO, significa que ya paso al menos por aquí y no será necesario realizar las validaciones de nuevo
                if (bo.Tipo != ETipoContrato.RO) {
                    switch (vista.UnidadOperativaID) {
                        case (int)ETipoEmpresa.Construccion:
                            if ((EAreaConstruccion)((UnidadBO)linea.Equipo).Area == EAreaConstruccion.RO) {
                                bo.Tipo = ETipoContrato.RO;
                            }
                            break;
                        case (int)ETipoEmpresa.Generacion:
                            if ((EAreaGeneracion)((UnidadBO)linea.Equipo).Area == EAreaGeneracion.RO) {
                                bo.Tipo = ETipoContrato.RO;
                            }
                            break;
                        case (int)ETipoEmpresa.Equinova:
                            if ((EAreaEquinova)((UnidadBO)linea.Equipo).Area == EAreaEquinova.RO) {
                                bo.Tipo = ETipoContrato.RO;
                            }
                            break;
                        default:
                            break;
                    }
                }

                bo.LineasContrato.Add(lineaBO);
            }

            //bo.LineasContrato = this.vista.LineasContrato.ConvertAll(s => (ILineaContrato)s);
            #endregion

            #region Archivos Adjuntos
            //Validamos el tipo de documento adjunto
            ETipoAdjunto tipoAdjunto = ETipoAdjunto.ContratoRO;
            //Validamos el tipo de documento adjunto
            switch (bo.Tipo) {
                case ETipoContrato.RO:
                    tipoAdjunto = ETipoAdjunto.ContratoRO;
                    break;
                case ETipoContrato.RE:
                    tipoAdjunto = ETipoAdjunto.ContratoRE;
                    break;
                case ETipoContrato.ROC:
                    tipoAdjunto = ETipoAdjunto.ContratoROC;
                    break;
            }
            List<ArchivoBO> adjuntos = presentadorDocumentos.Vista.ObtenerArchivos() ?? new List<ArchivoBO>();
            foreach (ArchivoBO adjunto in adjuntos) {
                adjunto.TipoAdjunto = tipoAdjunto;
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
        /// Despliega los datos en la Vista
        /// </summary>
        /// <param name="obj"></param>
        private void DatoAInterfazUsuario(object obj) {
            ContratoPSLBO bo = (ContratoPSLBO)obj;
            if (bo == null) bo = new ContratoPSLBO();
            if (bo.Cliente == null) bo.Cliente = new CuentaClienteIdealeaseBO() { Cliente = new ClienteBO() };
            if (bo.Divisa == null) bo.Divisa = new DivisaBO();
            if (bo.Divisa.MonedaDestino == null) bo.Divisa.MonedaDestino = new MonedaBO();
            if (bo.Operador == null) bo.Operador = new OperadorBO();
            if (bo.Operador.Direccion == null) bo.Operador.Direccion = new DireccionPersonaBO();
            if (bo.Operador.Direccion.Ubicacion == null) bo.Operador.Direccion.Ubicacion = new UbicacionBO();
            if (bo.Operador.Direccion.Ubicacion.Ciudad == null) bo.Operador.Direccion.Ubicacion.Ciudad = new CiudadBO();
            if (bo.Operador.Direccion.Ubicacion.Estado == null) bo.Operador.Direccion.Ubicacion.Estado = new EstadoBO();
            if (bo.Operador.Direccion.Ubicacion.Pais == null) bo.Operador.Direccion.Ubicacion.Pais = new PaisBO();
            if (bo.Operador.Licencia == null) bo.Operador.Licencia = new LicenciaBO();
            if (bo.Sucursal == null) bo.Sucursal = new SucursalBO();
            if (bo.Sucursal.UnidadOperativa == null) bo.Sucursal.UnidadOperativa = new UnidadOperativaBO();

            this.vista.ContratoID = bo.ContratoID;
            this.vista.NumeroContrato = bo.NumeroContrato;
            this.vista.CodigoMoneda = bo.Divisa.MonedaDestino.Codigo;
            this.presentadorDatosContrato.Vista.CodigoMonedaElegidoPrevio = bo.Divisa.MonedaDestino.Codigo;
            this.vista.FechaContrato = bo.FechaContrato;
            this.vista.FechaInicioArrendamiento = bo.FechaInicioArrendamiento;
            this.vista.FechaPromesaDevolucion = bo.FechaPromesaDevolucion;
            this.vista.FechaInicioActual = bo.FechaInicioActual;
            this.vista.FechaPromesaActual = bo.FechaPromesaActual;
            this.presentadorDatosContrato.Vista.FechaInicioElegidoPrevio = bo.FechaInicioActual;
            this.presentadorDatosContrato.Vista.FechaDevolucionElegidoPrevio = bo.FechaPromesaActual;
            this.presentadorDatosContrato.Vista.IncluyeSD = bo.IncluyeSD;
            if (bo.FechaInicioActual != null && bo.FechaPromesaActual != null)
                this.presentadorDatosContrato.CalcularDiasRenta();

            this.vista.EstablecerSucursalSeleccionada(bo.Sucursal.Id);
            this.vista.TipoContrato = bo.Tipo;

            this.vista.AutorizadorOrdenCompra = bo.AutorizadorOrdenCompra;
            this.vista.AutorizadorTipoConfirmacion = bo.AutorizadorTipoConfirmacion;
            this.vista.DestinoAreaOperacion = bo.DestinoAreaOperacion;
            this.vista.MercanciaTransportar = bo.MercanciaTransportar;
            if (bo.FormaPago != null)
                this.vista.FormaPagoID = (int)bo.FormaPago;
            else
                this.vista.FormaPagoID = null;

            if (bo.TipoConfirmacion != null)
                this.vista.TipoConfirmacionID = (int)bo.TipoConfirmacion;
            else
                this.vista.TipoConfirmacionID = null;

            //Cuenta de Cliente Idealease
            this.vista.CuentaClienteID = bo.Cliente.Id;
            this.vista.CuentaClienteNombre = bo.Cliente.Nombre;
            this.vista.CuentaClienteNumeroCuenta = bo.Cliente.Numero;
            if (bo.Cliente.Cliente != null) {
                this.vista.ClienteID = bo.Cliente.Cliente.Id;
                this.vista.ClienteRFC = bo.Cliente.Cliente.RFC;
            }
            if (bo.Cliente.TipoCuenta != null)
                this.vista.CuentaClienteTipoID = (int)bo.Cliente.TipoCuenta;
            else
                this.vista.CuentaClienteTipoID = null;
            if (bo.Cliente.EsFisico != null)
                this.vista.ClienteEsFisica = (bool)bo.Cliente.EsFisico;
            else
                this.vista.ClienteEsFisica = null;

            //Dirección del cliente
            if (bo.Cliente.Direcciones != null && bo.Cliente.Direcciones.Count > 0) {
                this.vista.ClienteDireccionId = bo.Cliente.Direcciones[0].Id;
                this.presentadorDatosContrato.Vista.ClienteDireccionCompleta = bo.Cliente.Direcciones[0].Direccion;
                this.vista.ClienteDireccionCalle = bo.Cliente.Direcciones[0].Calle;
                this.vista.ClienteDireccionColonia = bo.Cliente.Direcciones[0].Colonia;
                this.vista.ClienteDireccionCodigoPostal = bo.Cliente.Direcciones[0].CodigoPostal;
                this.vista.ClienteDireccionCiudad = bo.Cliente.Direcciones[0].Ubicacion.Ciudad.Codigo;
                this.vista.ClienteDireccionEstado = bo.Cliente.Direcciones[0].Ubicacion.Estado.Codigo;
                this.vista.ClienteDireccionMunicipio = bo.Cliente.Direcciones[0].Ubicacion.Municipio.Codigo;
                this.vista.ClienteDireccionPais = bo.Cliente.Direcciones[0].Ubicacion.Pais.Codigo;
            } else {
                this.vista.ClienteDireccionId = null;
                this.presentadorDatosContrato.Vista.ClienteDireccionCompleta = null;
                this.vista.ClienteDireccionCalle = null;
                this.vista.ClienteDireccionColonia = null;
                this.vista.ClienteDireccionCodigoPostal = null;
                this.vista.ClienteDireccionCiudad = null;
                this.vista.ClienteDireccionEstado = null;
                this.vista.ClienteDireccionMunicipio = null;
                this.vista.ClienteDireccionPais = null;
            }

            if (bo.RepresentantesLegales != null) {
                this.vista.RepresentantesLegales = bo.RepresentantesLegales.ConvertAll(s => (RepresentanteLegalBO)s);
                this.presentadorDatosContrato.ActualizarRepresentantesLegales();
            } else
                this.vista.RepresentantesLegales = null;
            vista.SoloRepresentantes = bo.SoloRepresentantes;
            if (bo.Avales != null)
                vista.Avales = new List<AvalBO>(bo.Avales);
            else vista.Avales = null;
            presentadorDatosContrato.ActualizarAvales();

            //Productos servicio
            if (bo.ProductoServicio != null) {
                this.vista.ProductoServicioId = bo.ProductoServicio.Id;
                this.vista.ClaveProductoServicio = bo.ProductoServicio.NombreCorto;
                this.vista.DescripcionProductoServicio = bo.ProductoServicio.Nombre;
            }

            //Líneas de contrato
            if (bo.LineasContrato != null) {
                List<LineaContratoPSLBO> lineasContrato = new List<LineaContratoPSLBO>();
                LineaContratoPSLBO lineaBO = new LineaContratoPSLBO();
                foreach (LineaContratoPSLBO linea in bo.LineasContrato.ConvertAll(a => (LineaContratoPSLBO)a)) {
                    lineaBO = new LineaContratoPSLBO();
                    lineaBO.LineaContratoID = linea.LineaContratoID;
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
                        Activo = ((TarifaContratoPSLBO)linea.Cobrable).Activo,
                        PorcentajeDescuento = ((TarifaContratoPSLBO)linea.Cobrable).PorcentajeDescuento,
                        PorcentajeDescuentoMaximo = ((TarifaContratoPSLBO)linea.Cobrable).PorcentajeDescuentoMaximo,
                        EtiquetaDescuento = ((TarifaContratoPSLBO)linea.Cobrable).EtiquetaDescuento,
                        TarifaConDescuento = ((TarifaContratoPSLBO)linea.Cobrable).TarifaConDescuento,
                        PorcentajeSeguro = ((TarifaContratoPSLBO)linea.Cobrable).PorcentajeSeguro
                    };
                    lineaBO.Activo = linea.Activo;
                    lineaBO.Devuelta = linea.Devuelta;
                    lineaBO.LineaOrigenIntercambioID = linea.LineaOrigenIntercambioID;
                    lineasContrato.Add(lineaBO);
                }
                this.presentadorDatosContrato.Vista.LineasContrato = lineasContrato;
            }


            this.vista.Observaciones = bo.Observaciones;
            this.vista.UC = bo.UC;
            this.vista.UUA = bo.UUA;
            this.vista.FC = bo.FC;
            this.vista.FUA = bo.FUA;
            if (bo.Estatus != null)
                this.vista.EstatusID = (int)bo.Estatus;
            else
                this.vista.EstatusID = null;

            this.presentadorDocumentos.CargarListaArchivos(bo.DocumentosAdjuntos, this.presentadorDocumentos.Vista.TiposArchivo);
            this.presentadorHerramientas.DatosAInterfazUsuario(bo);
        }

        /// <summary>
        /// Valida los campos requeridos para actualizar el contrato en borrador
        /// </summary>
        /// <returns></returns>
        public string ValidarCamposBorrador() {
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
        /// Valida los campos requeridos para actualizar el contrato en curso
        /// </summary>
        /// <returns></returns>
        public string ValidarCamposRegistro() {
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
        /// Limpia los datos en la memoria de la vista
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
                            lineaContratoPRE.Inicializar(linea, presentadorDatosContrato.Vista.DiasRenta, presentadorDatosContrato.Vista.CodigoMoneda, presentadorDatosContrato.Vista.ModoRegistro);
                            if (linea.Activo.GetValueOrDefault())
                                lineaContratoPRE.Vista.PrepararVistaDetalle(true);
                            else
                                lineaContratoPRE.Vista.PrepararVistaDetalle(false);
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
            } catch (Exception ex) {
                throw new Exception(nombreClase + ".AgregarLineaContrato: " + ex.Message);
            }
        }
        /// <summary>
        /// Recalcula la tarifas de las líneas del contrato
        /// </summary>
        public void EliminarTarifaLineasContrato() {
            try {
                string Mensaje = string.Empty;
                string SerieUnidad = string.Empty;
                DiaPeriodoTarifaBR DiaPerioBR = new DiaPeriodoTarifaBR();
                List<Object> listValues = DiaPerioBR.ObtenerPeriodoTarifa(this.dctx, this.vista.UnidadOperativaID, this.presentadorDatosContrato.Vista.DiasRenta.Value);
                if (listValues.Any()) {
                    EPeriodosTarifa PeriodoTarifa = (EPeriodosTarifa)listValues[0];
                    int? DuracionDiasPeriodo = Convert.ToInt32(listValues[1]);
                    int? ModeloID = null;
                    ETipoTarifa? TipoTarifa = null;


                    foreach (LineaContratoPSLBO linea in vista.LineasContrato.Where(x => x.Activo.GetValueOrDefault() && !x.Devuelta).ToList()) {
                        ModeloID = null;
                        TipoTarifa = null;
                        if (((BPMO.SDNI.Equipos.BO.UnidadBO)linea.Equipo).Modelo != null)
                            ModeloID = ((BPMO.SDNI.Equipos.BO.UnidadBO)linea.Equipo).Modelo.Id;
                        SerieUnidad = ((BPMO.SDNI.Equipos.BO.UnidadBO)linea.Equipo).NumeroSerie;
                        TipoTarifa = linea.TipoTarifa;

                        TarifaContratoPSLBO TarifaContratoPSLBO = (TarifaContratoPSLBO)linea.Cobrable;
                        TarifaContratoPSLBO.PeriodoTarifa = PeriodoTarifa;
                        TarifaContratoPSLBO.DuracionDiasPeriodo = DuracionDiasPeriodo;
                        try {
                            TarifaContratoPSLBO.MaximoHrsTurno = DiaPerioBR.ObtenerMaximoHorasTurnoTarifa(this.dctx, this.vista.UnidadOperativaID, (EPeriodosTarifa)PeriodoTarifa, TarifaContratoPSLBO.TarifaTurno); //Cambiar el 2 por el turno
                            if (PeriodoTarifa != null && TarifaContratoPSLBO.TarifaTurno != null && TipoTarifa != null && this.vista.UnidadOperativaID != null && ModeloID != null) {
                                TarifaPSLBR tarifaBR = new TarifaPSLBR();
                                TarifaPSLBO tarifaBO = new TarifaPSLBO();
                                tarifaBO.PeriodoTarifa = (EPeriodosTarifa)PeriodoTarifa;
                                tarifaBO.TipoTarifaID = TipoTarifa.ToString();
                                tarifaBO.Divisa = new DivisaBO { MonedaDestino = new MonedaBO { Codigo = this.vista.CodigoMoneda } };
                                tarifaBO.TarifaTurno = TarifaContratoPSLBO.TarifaTurno;
                                tarifaBO.Modelo = new Servicio.Catalogos.BO.ModeloBO { Id = ModeloID };
                                tarifaBO.Sucursal = new SucursalBO { Id = this.vista.SucursalSeleccionada.Id, UnidadOperativa = new UnidadOperativaBO { Id = this.vista.UnidadOperativaID } };
                                tarifaBO.Activo = true;
                                List<TarifaPSLBO> lstTarifas = tarifaBR.Consultar(this.dctx, tarifaBO);
                                if (lstTarifas.Any()) {
                                    TarifaContratoPSLBO.Tarifa = lstTarifas[0].Tarifa;
                                    TarifaContratoPSLBO.TarifaHrAdicional = lstTarifas[0].TarifaHrAdicional;
                                    //Se calcula la tarifa con descuento
                                    if (TarifaContratoPSLBO.PorcentajeDescuento != null)
                                        TarifaContratoPSLBO.TarifaConDescuento = Math.Truncate((TarifaContratoPSLBO.Tarifa.GetValueOrDefault() - (TarifaContratoPSLBO.Tarifa.GetValueOrDefault() * (TarifaContratoPSLBO.PorcentajeDescuento.GetValueOrDefault() / 100))) * 10) / 10;//Redondea a dos posiciones
                                    else
                                        TarifaContratoPSLBO.TarifaConDescuento = null;
                                } else {
                                    TarifaContratoPSLBO.Tarifa = null;
                                    TarifaContratoPSLBO.TarifaHrAdicional = null;
                                    Mensaje += SerieUnidad + ", ";
                                }
                            } else {
                                TarifaContratoPSLBO.Tarifa = null;
                                TarifaContratoPSLBO.TarifaHrAdicional = null;
                                Mensaje += SerieUnidad + ", ";
                            }
                        } catch (Exception ex) {
                            TarifaContratoPSLBO.Tarifa = null;
                            TarifaContratoPSLBO.TarifaHrAdicional = null;
                            Mensaje += SerieUnidad + ", ";
                        }
                    }
                } else {
                    foreach (LineaContratoPSLBO linea in vista.LineasContrato.Where(x => x.Activo.GetValueOrDefault() && !x.Devuelta).ToList()) {
                        TarifaContratoPSLBO TarifaContratoPSLBO = (TarifaContratoPSLBO)linea.Cobrable;
                        TarifaContratoPSLBO.Tarifa = null;
                        TarifaContratoPSLBO.TarifaHrAdicional = null;
                        SerieUnidad = ((BPMO.SDNI.Equipos.BO.UnidadBO)linea.Equipo).NumeroSerie;
                        Mensaje += SerieUnidad + ", ";
                    }
                }
                if (!String.IsNullOrEmpty(Mensaje))
                    this.vista.MostrarMensaje("No se encontró una tarifa configurada para las siguientes unidades: \n" + Mensaje.Substring(0, Mensaje.Length - 2), ETipoMensajeIU.ADVERTENCIA);
                CalcularTotales();
                RenderizarGridLineas();
            } catch (Exception ex) {
                throw new Exception(nombreClase + ".EliminarTarifaLineasContrato: " + ex.Message);
            }
        }
        private void RecalcularTarifasRenovacion() {
            this.vista.FechaInicioActual = this.vista.FechaPromesaActual.Value.AddDays(1);
            this.vista.FechaPromesaActual = this.vista.FechaPromesaActual.Value.AddDays(1);
            this.presentadorDatosContrato.Vista.FechaInicioElegidoPrevio = this.vista.FechaInicioActual;
            this.presentadorDatosContrato.Vista.FechaDevolucionElegidoPrevio = this.vista.FechaPromesaActual;
            if (this.vista.FechaInicioActual != null && this.vista.FechaPromesaActual != null)
                this.presentadorDatosContrato.CalcularDiasRenta();
            this.EliminarTarifaLineasContrato();
        }
        #endregion
    }
}