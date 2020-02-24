using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Xml;
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
using BPMO.SDNI.Contratos.PSL.VIS;
using BPMO.SDNI.Equipos.BO;
using BPMO.SDNI.Facturacion.AplicacionesPago.BR;
using BPMO.SDNI.Facturacion.InfraestructuraPagos.BO;
using BPMO.SDNI.Facturacion.InfraestructuraPagos.BOF;
using BPMO.SDNI.Facturacion.InfraestructuraPagos.BR;

namespace BPMO.SDNI.Contratos.PSL.PRE {
    /// <summary>
    /// Presentador de Detalle de Contrato RO
    /// </summary>
    public class DetalleContratoROPRE {
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
        private IDetalleContratoROVIS vista;
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
        private string nombreClase = "DetalleContratoROPRE";

        /// <summary>
        /// Presentador de la Barra de Herramientas
        /// </summary>
        private readonly ucHerramientasPSLPRE presentadorHerramientas;
        #endregion

        #region Constructores
        /// <summary>
        /// Constructor por defecto del Presentador
        /// </summary>
        /// <param name="view">Vista de Registro de Contrato</param>
        /// <param name="viewDatosContrato">Vista de los Datos del Contrato</param>
        /// <param name="viewDocumentos">Vista del Control de Documentos</param>
        public DetalleContratoROPRE(IDetalleContratoROVIS view, IucContratoPSLVIS viewDatosContrato, IucCatalogoDocumentosVIS viewDocumentos, IucLineaContratoPSLVIS viewLineas, IucHerramientasPSLVIS vistaHerramientas) {
            try {
                this.vista = view;

                this.presentadorDatosContrato = new ucContratoPSLPRE(viewDatosContrato);
                this.presentadorDocumentos = new ucCatalogoDocumentosPRE(viewDocumentos);
                this.lineaContratoPRE = new ucLineaContratoPSLPRE(viewLineas);

                //RQM. 15003.
                this.presentadorHerramientas = new ucHerramientasPSLPRE(vistaHerramientas);

                this.controlador = new ContratoPSLBR();
                this.dctx = FacadeBR.ObtenerConexion();
            } catch (Exception ex) {
                this.vista.MostrarMensaje("Inconsistencia en los parámetros de configuración", ETipoMensajeIU.ERROR, this.nombreClase + ".RegistrarContratoROPRE:" + ex.Message);
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

                //Se valida si el usuario tiene permiso para registrar
                if (!this.ExisteAccion(lst, "UI INSERTAR"))
                    this.vista.PermitirRegistrar(false);

                //Se valida si el usuario tiene permiso para editar
                if (!this.ExisteAccion(lst, "UI ACTUALIZAR"))
                    this.vista.PermitirEditar(false);
                else {
                    if (!this.vista.EstatusID.HasValue || (this.vista.EstatusID.HasValue && this.vista.EstatusID.Value != (int)EEstatusContrato.Borrador))
                        this.vista.PermitirEditar(false);
                }

                //Se valida si el usuario tiene permiso para eliminar
                if (!this.ExisteAccion(lst, "BORRARCOMPLETO"))
                    this.presentadorHerramientas.DeshabilitarMenuBorrar();

                //Se valida si el usuario tiene permiso para cerrar un contrato                
                this.ValidarPermisoGenerarPago(lst);

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
        /// Registra un Contrato RO
        /// </summary>
        /// <summary>
        /// Obtiene los datos de la Vista
        /// </summary>
        /// <returns></returns>
        private object InterfazUsuarioADato() {
            UnidadOperativaBO unidadOperativa = new UnidadOperativaBO();
            if (this.vista.UnidadOperativaID != null)
                unidadOperativa.Id = this.vista.UnidadOperativaID;

            if (this.vista.UnidadOperativaNombre != null)
                unidadOperativa.Nombre = vista.UnidadOperativaNombre;

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
            bo.Tipo = this.vista.TipoContrato;

            if (this.vista.ContratoID != null)
                bo.ContratoID = this.vista.ContratoID;

            if (this.vista.EstatusID != null)
                bo.Estatus = (EEstatusContrato)Enum.Parse(typeof(EEstatusContrato), this.vista.EstatusID.ToString());
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
            if (this.vista.DestinoAreaOperacion != null)
                bo.DestinoAreaOperacion = this.vista.DestinoAreaOperacion;
            if (this.vista.MercanciaTransportar != null)
                bo.MercanciaTransportar = this.vista.MercanciaTransportar;
            if (this.vista.TipoConfirmacionID != null)
                bo.TipoConfirmacion = (ETipoConfirmacion)Enum.Parse(typeof(ETipoConfirmacion), this.vista.TipoConfirmacionID.ToString());

            if (this.vista.Observaciones != null)
                bo.Observaciones = this.vista.Observaciones;
            #region BEP1401.SC0026
            if (this.vista.DiasFacturar != null)
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

            if (!string.IsNullOrWhiteSpace(this.vista.ClaveProductoServicio)) {
                if (bo.ProductoServicio == null) bo.ProductoServicio = new ProductoServicioBO();
                bo.ProductoServicio.Id = this.vista.ProductoServicioId;
                bo.ProductoServicio.NombreCorto = this.vista.ClaveProductoServicio;
                bo.ProductoServicio.Nombre = this.vista.DescripcionProductoServicio;
            }

            #region Linea de Contrato

            bo.LineasContrato = this.presentadorDatosContrato.Vista.LineasContrato.ConvertAll(x => x as ILineaContrato);

            #endregion

            #region Archivos Adjuntos
            List<ArchivoBO> adjuntos = presentadorDocumentos.Vista.ObtenerArchivos() ?? new List<ArchivoBO>();
            foreach (ArchivoBO adjunto in adjuntos) {
                adjunto.TipoAdjunto = ETipoAdjunto.ContratoRD;/////////Cambiar a RO
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
        /// <param name="parameters"></param>
        private void DatoAInterfazUsuario(IDictionary parameters) {
            if (!parameters.Contains("ContratoPSLBO") || !(parameters["ContratoPSLBO"] is ContratoPSLBO))
                return;

            ContratoPSLBO bo = (ContratoPSLBO)parameters["ContratoPSLBO"];
            if (bo == null) return;
            
            this.vista.ContratoID = bo.ContratoID;
            if (bo.Estatus != null)
                this.vista.EstatusID = (int)bo.Estatus;
            this.presentadorDatosContrato.Vista.FechaContrato = bo.FechaContrato;
            this.presentadorDatosContrato.Vista.NumeroContrato = bo.NumeroContrato;
            this.presentadorDatosContrato.Vista.DestinoAreaOperacion = bo.DestinoAreaOperacion;
            this.presentadorDatosContrato.Vista.MercanciaTransportar = bo.MercanciaTransportar;
            this.presentadorDatosContrato.Vista.FechaInicioArrendamiento = bo.FechaInicioArrendamiento;
            this.presentadorDatosContrato.Vista.FechaPromesaDevolucion = bo.FechaPromesaDevolucion;
            this.presentadorDatosContrato.Vista.FechaInicioActual = bo.FechaInicioActual;
            this.presentadorDatosContrato.Vista.FechaPromesaActual = bo.FechaPromesaActual;
            this.presentadorDatosContrato.Vista.IncluyeSD = bo.IncluyeSD;
            this.presentadorDatosContrato.Vista.TasaInteres = bo.TasaInteres;
            if (bo.FechaInicioActual != null && bo.FechaPromesaActual != null)
                this.presentadorDatosContrato.CalcularDiasRenta();
            this.presentadorDatosContrato.Vista.Observaciones = bo.Observaciones;
            if (bo.Divisa != null && bo.Divisa.MonedaDestino != null)
                this.presentadorDatosContrato.Vista.CodigoMoneda = bo.Divisa.MonedaDestino.Codigo;
            this.vista.AutorizadorOrdenCompra = bo.AutorizadorOrdenCompra;
            if (bo.LineasContrato != null)
                this.presentadorDatosContrato.Vista.LineasContrato = bo.LineasContrato.ConvertAll(a => (LineaContratoPSLBO)a);
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

            if (bo.Sucursal != null && bo.Sucursal.Id.HasValue)
                this.presentadorDatosContrato.Vista.EstablecerSucursalSeleccionada(bo.Sucursal.Id);

            if (bo.Cliente != null) {
                this.presentadorDatosContrato.Vista.CuentaClienteNombre = bo.Cliente.Nombre;
                this.presentadorDatosContrato.Vista.CuentaClienteNumeroCuenta = bo.Cliente.Numero;
                if (bo.Cliente != null && bo.Cliente.Direcciones != null) {
                    this.presentadorDatosContrato.Vista.ClienteDireccionCompleta = bo.Cliente.Direcciones.Select(a => a.Direccion).FirstOrDefault();
                    this.presentadorDatosContrato.Vista.ClienteDireccionCodigoPostal = bo.Cliente.Direcciones.Select(a => a.CodigoPostal).FirstOrDefault();
                }

                if (bo.Cliente.Cliente != null)
                    this.presentadorDatosContrato.Vista.ClienteRFC = bo.Cliente.Cliente.RFC;
            }
            if ((bo.Divisa != null && bo.Divisa.MonedaOrigen != null))
                this.presentadorDatosContrato.Vista.CodigoMoneda = bo.Divisa.MonedaOrigen.Codigo;
            this.presentadorDatosContrato.Vista.AutorizadorTipoConfirmacion = bo.AutorizadorTipoConfirmacion;
            this.presentadorDatosContrato.Vista.FormaPagoID = (int)bo.FormaPago;

            //Productos servicio
            if (bo.ProductoServicio != null) {
                this.vista.ProductoServicioId = bo.ProductoServicio.Id;
                this.vista.ClaveProductoServicio = bo.ProductoServicio.NombreCorto;
                this.vista.DescripcionProductoServicio = bo.ProductoServicio.Nombre;
            }

            this.vista.TipoContrato = bo.Tipo;
            this.presentadorDocumentos.CargarListaArchivos(bo.DocumentosAdjuntos, this.presentadorDocumentos.Vista.TiposArchivo);
        }

        /// <summary>
        /// Limpia los datos de la memoria de la Vista
        /// </summary>
        private void LimpiarSesion() {
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
                                lineaContratoPRE.Inicializar(unidad, presentadorDatosContrato.Vista.DiasRenta, presentadorDatosContrato.Vista.CodigoMoneda);
                                vista.CambiarALinea();

                            }
                        } else
                            vista.MostrarMensaje("La unidad seleccionada ya existe en el contrato.",
                                                 ETipoMensajeIU.INFORMACION);
                    } else
                        vista.MostrarMensaje("No ha seleccionado una unidad valida para agregar al contrato",
                                             ETipoMensajeIU.ADVERTENCIA);

                } else
                    vista.MostrarMensaje("No ha proporcionado un plazo en meses valido o mayor a 0",
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
                        lineaContratoPRE.Vista.UnidadOperativaID = this.vista.UnidadOperativaID;
                        lineaContratoPRE.Vista.ModuloID = this.presentadorDatosContrato.Vista.ModuloID;
                        lineaContratoPRE.Inicializar();
                        lineaContratoPRE.Vista.PrepararVistaDetalle();
                        lineaContratoPRE.EstablecerUltimoObjeto(linea);
                        lineaContratoPRE.DatosAInterfazUsuario(linea);
                        vista.CambiarALinea();
                    } else {
                        vista.MostrarMensaje("No ha proporcionado un plazo en meses valido o mayor a 0",
                                             ETipoMensajeIU.ADVERTENCIA);
                    }
                } else {
                    vista.MostrarMensaje("No ha proporcionado un plazo en meses valido o mayor a 0",
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
        /// Realizar el cierre de contrato
        /// </summary>
        public void CerrarContrato() {
            ContratoPSLBO bo = (ContratoPSLBO)this.InterfazUsuarioADato();

            this.LimpiarSesion();
            this.vista.EstablecerPaqueteNavegacion("UltimoContratoPSLBO", bo);

            this.vista.RedirigirACierre();
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

        public void Editar() {
            this.Editar(false);
        }

        public void Editar(bool lEnCurso) {
            ContratoPSLBO bo = (ContratoPSLBO)this.InterfazUsuarioADato();

            this.LimpiarSesion();
            this.vista.EstablecerPaqueteNavegacion("UltimoContratoPSLBO", bo);

            this.vista.RedirigirAEdicion(lEnCurso);
        }

        /// <summary>
        /// Elimina el contrato desplegado con estatus de borrador
        /// </summary>
        public void EliminarContrato() {
            try {
                #region InterfazUsuarioADato Personalizado
                ContratoPSLBO bo = (ContratoPSLBO)this.vista.UltimoObjeto;

                if (bo.Estatus != EEstatusContrato.Borrador) {
                    vista.MostrarMensaje("El contrato debe tener estatus Borrador para ser eliminado.", ETipoMensajeIU.ADVERTENCIA);
                    return;
                }

                //finalización del contrato
                FinalizacionContratoProxyBO finalizacionContrato = new FinalizacionContratoProxyBO();
                finalizacionContrato.Fecha = vista.FUA;
                finalizacionContrato.Usuario = new UsuarioBO { Id = vista.UUA };
                finalizacionContrato.Observaciones = vista.Observaciones;

                ContratoPSLBO previous = new ContratoPSLBO(bo);

                bo.CierreContrato = finalizacionContrato;
                bo.FUA = vista.FUA;
                bo.UUA = vista.UUA;
                #endregion

                //Se crea el objeto de seguridad
                UsuarioBO usuario = new UsuarioBO() { Id = this.vista.UsuarioID };
                AdscripcionBO adscripcion = new AdscripcionBO() { UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaID } };
                SeguridadBO seguridadBO = new SeguridadBO(Guid.Empty, usuario, adscripcion);

                this.controlador.BorrarCompleto(dctx, bo, previous, seguridadBO);

                this.LimpiarSesion();
                this.vista.RedirigirAConsulta();
            } catch (Exception ex) {
                throw new Exception(this.nombreClase + ".EliminarContrato: " + ex.Message);
            }
        }

        public void Renovar() {
            ContratoPSLBO bo = (ContratoPSLBO)this.InterfazUsuarioADato();

            this.LimpiarSesion();
            this.vista.EstablecerPaqueteNavegacion("UltimoContratoPSLBO", bo);

            this.vista.RedirigirARenovar();
        }

        /// <summary>
        /// Generar impresión del contrato
        /// </summary>
        public void ImprimirContratoRO() {
            try {
                //ContratoPSLBO bo = (ContratoPSLBO)this.InterfazUsuarioADato();
                this.LimpiarSesion();

                var contrato = new ContratoPSLBO { ContratoID = this.vista.ContratoID };
                string monedaCodigo = string.Empty;
                List<ContratoPSLBO> contratoPSL = this.controlador.ConsultarCompleto(this.dctx, contrato, false);
                Dictionary<string, object> datos = new Dictionary<string, object>();
                if (contratoPSL.Any()) {
                    datos.Add("ContratoPSLBO", contratoPSL.FirstOrDefault());
                    monedaCodigo = contratoPSL.FirstOrDefault().Divisa.MonedaDestino.Codigo;
                }
                #region[Dirección sucursal]
                List<SucursalBO> lstSuc = null;
                DireccionSucursalBO direccionSucursal = new DireccionSucursalBO();
                lstSuc = FacadeBR.ConsultarSucursal(this.dctx, new SucursalBO() { Matriz = true, UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaID } });

                if (lstSuc.Any()) {
                    SucursalBO sucBO = lstSuc[0];
                    sucBO.Agregar(new DireccionSucursalBO() { Primaria = true });
                    lstSuc = FacadeBR.ConsultarSucursalCompleto(this.dctx, sucBO);
                    //Establecer la dirección de la empresa
                    if (lstSuc.Any()) {
                        if (lstSuc[0].DireccionesSucursal != null && lstSuc[0].DireccionesSucursal.Any())
                            direccionSucursal = lstSuc[0].DireccionesSucursal[0];
                    }
                }
                datos.Add("DireccionSucursal", direccionSucursal);
                #endregion

                //Obtener las configuraciones de la unidad operativa
                List<ConfiguracionUnidadOperativaBO> lstConfigUO = new ModuloBR().ConsultarConfiguracionUnidadOperativa(this.dctx, new ConfiguracionUnidadOperativaBO() { UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaID } }, this.presentadorDatosContrato.Vista.ModuloID);
                if (lstConfigUO.Any())
                    datos.Add("RepresentanteEmpresa", lstConfigUO[0]);

                #region Consulta periodoTarifario.
                
                List<DiaPeriodoTarifaBO> lstTemp = new List<DiaPeriodoTarifaBO>();
                DiaPeriodoTarifaBR periodoTarifaBR = new DiaPeriodoTarifaBR();
                lstTemp = periodoTarifaBR.Consultar(dctx,  new DiaPeriodoTarifaBO() { UnidadOperativaID = this.vista.UnidadOperativaID });
                if (lstTemp.Count == 1) 
                    datos.Add("PeriodoTarifa", lstTemp[0]);
                if (!string.IsNullOrEmpty(monedaCodigo)) {
                    MonedaBO moneda = FacadeBR.ConsultarMoneda(this.dctx, new MonedaBO() { Codigo = monedaCodigo }).FirstOrDefault();
                    datos.Add("Moneda" , moneda);
                }
                #endregion

                this.vista.EstablecerPaqueteNavegacionReporte("ContratoRO", datos);

                this.vista.RedirigirAImprimirContrato();
            } catch (Exception ex) {
                throw new Exception(nombreClase + ".Imprimir: " + Environment.NewLine + ex.Message);
            }
        }

        /// <summary>
        /// Generar impresión del pagare
        /// </summary>
        public void ImprimirPagareRO() {
            try {
                this.LimpiarSesion();

                ContratoPSLBO contrato = (ContratoPSLBO)this.vista.UltimoObjeto;

                //int diasRenta = 0;
                if (contrato != null) {

                    DataSet dsReporte = new DataSet();

                    #region DataTable
                    DataTable dtReporte = new DataTable("PagareUnidades");
                    dtReporte.Columns.Add("SeccionMonto", typeof(String));
                    dtReporte.Columns.Add("Seccion1", typeof(String));
                    dtReporte.Columns.Add("Seccion2", typeof(String));
                    dtReporte.Columns.Add("Seccion3", typeof(String));
                    dtReporte.Columns.Add("SeccionDomicilio", typeof(String));
                    dtReporte.Columns.Add("SeccionFecha", typeof(String));
                    dtReporte.Columns.Add("SeccionCliente", typeof(String));
                    dtReporte.Columns.Add("SeccionAval", typeof(String));
                    #endregion /DataTable

                    #region Manejo XML
                    // Se carga el XML
                    XmlDocument xDoc = this.vista.ObtenerXmlReporte();

                    // Se obtienen los nodos
                    string defaultSeccionMonto = this.GetTextXML("MONTO_ENC", xDoc);
                    string defaultSeccion1 = this.GetTextXML("SECCION_1", xDoc);
                    string defaultSeccionDomicilio = this.GetTextXML("DOMICILIO", xDoc);
                    string defaultSeccion2 = this.GetTextXML("SECCION_2", xDoc);
                    string defaultSeccion3 = this.GetTextXML("SECCION_3", xDoc);
                    string defaultSeccionFecha = this.GetTextXML("FECHA", xDoc);
                    string defaultSeccionCliente = this.GetTextXML("PERSONACLIENTE", xDoc);
                    string defaultSeccionAval = this.GetTextXML("PERSONAAVAL", xDoc);
                    #endregion /Manejo XML

                    #region [Dirección sucursal]
                    List<SucursalBO> lstSuc = null;
                    DireccionSucursalBO direccionSucursal = new DireccionSucursalBO();
                    lstSuc = FacadeBR.ConsultarSucursal(this.dctx, new SucursalBO() { Matriz = true, UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaID } });

                    if (lstSuc.Any()) {
                        SucursalBO sucBO = lstSuc[0];
                        sucBO.Agregar(new DireccionSucursalBO() { Primaria = true });
                        lstSuc = FacadeBR.ConsultarSucursalCompleto(this.dctx, sucBO);
                        //Establecer la dirección de la empresa
                        if (lstSuc.Any()) {
                            if (lstSuc[0].DireccionesSucursal != null && lstSuc[0].DireccionesSucursal.Any())
                                direccionSucursal = lstSuc[0].DireccionesSucursal[0];
                        }
                    }
                    #endregion

                    PersonaBO avalBo = contrato.Avales.FirstOrDefault();
                    MonedaBO moneda = FacadeBR.ConsultarMoneda(this.dctx, new MonedaBO() { Codigo = contrato.Divisa.MonedaDestino.Codigo }).FirstOrDefault();
                    DireccionClienteBO clienteinfo = contrato.Cliente.Direcciones.FirstOrDefault(d => d.Primaria == true);

                    string strCiudadEdoSuc = string.Empty;
                    if (direccionSucursal.Ubicacion != null) {
                        if (direccionSucursal.Ubicacion.Ciudad != null)
                            strCiudadEdoSuc += ", " + direccionSucursal.Ubicacion.Ciudad.Codigo;
                        if (direccionSucursal.Ubicacion.Estado != null)
                            strCiudadEdoSuc += ", " + direccionSucursal.Ubicacion.Estado.Codigo;
                        if (!string.IsNullOrWhiteSpace(strCiudadEdoSuc))
                            strCiudadEdoSuc = strCiudadEdoSuc.Substring(2);
                    }

                    foreach (ILineaContrato linea in contrato.LineasContrato) {
                        if (linea.Equipo == null || linea.Equipo.ActivoFijo == null) {
                            this.vista.MostrarMensaje("No se puede obtener información del activo fijo. Actualice la unidad o verifique información de la misma.", ETipoMensajeIU.INFORMACION);
                            return;
                        }

                        decimal dMonto = ((ActivoFijoIDLBO)(linea.Equipo.ActivoFijo)).CostoAdquisicion ?? 0M;

                        DataRow dr = dtReporte.NewRow();
                        dr["SeccionMonto"] = defaultSeccionMonto
                            .Replace("{MONEDA_ENC}", dMonto.ToString("C"))
                            .Replace("{MONEDA_CVE}",moneda.Codigo);
                        dr["Seccion1"] = defaultSeccion1
                            .Replace("{MONTO_DET}", dMonto.ToString("C"))
                            .Replace("{MONEDA_CVE_DET}",moneda.Codigo)
                            .Replace("{EMPRESA_GB}", contrato.Sucursal.UnidadOperativa.Empresa.Nombre)
                            .Replace("{MONTO_LETRAS}", new ConvertirALetrasBR().ConvertirMoneda(dMonto, moneda.ComplementoNombreLegal, moneda.NombreLegal).ToUpper());
                        dr["SeccionDomicilio"] = defaultSeccionDomicilio
                            .Replace("{DOMICILIO_CLIENTE}", clienteinfo.Direccion + ", CODIGO POSTAL " + clienteinfo.CodigoPostal)
                            .Replace("{DOMICILIO_AVAL}", avalBo != null ? avalBo.Direccion : string.Empty);
                        dr["Seccion2"] = defaultSeccion2
                            .Replace("{PORCENTAJE}", (contrato.TasaInteres != null) ? Math.Round((decimal)contrato.TasaInteres,2).ToString() + "%": "0%");
                        dr["Seccion3"] = defaultSeccion3;
                        dr["SeccionFecha"] = defaultSeccionFecha
                            .Replace("{FECHA}", contrato.FechaContrato.Value.ToString("d DE MMMM DE yyyy").ToUpper())
                            .Replace("{CIUDAD_ESTADO}", strCiudadEdoSuc);

                        RepresentanteLegalBO repLegalBo = (RepresentanteLegalBO)contrato.RepresentantesLegales.FirstOrDefault();
                        dr["SeccionCliente"] = defaultSeccionCliente
                            .Replace("{CLIENTE}", contrato.Cliente.Nombre)
                            .Replace("{REPRESENTANTE}", repLegalBo == null ? string.Empty : string.Format("REP.LEG. {0}", repLegalBo.Nombre));

                        dr["SeccionAval"] = avalBo == null ? string.Empty
                            : defaultSeccionAval.Replace("{AVAL}", string.Format("REP.LEG. {0}", avalBo.Nombre));

                        dtReporte.Rows.Add(dr);
                    }

                    dsReporte.Tables.Add(dtReporte);
                    Dictionary<string, object> datos = new Dictionary<string, object>();
                    datos.Add("datosReporte", dsReporte);

                    this.vista.EstablecerPaqueteNavegacionReporte("PagareRO", datos);
                    this.vista.RedirigirAImprimirContrato();
                }
                else {
                    this.vista.MostrarMensaje("Los datos de contrato están vacíos.", ETipoMensajeIU.ADVERTENCIA);
                }
            }
            catch (Exception ex) {
                throw new Exception(nombreClase + ".Imprimir: " + Environment.NewLine + ex.Message);
            }
        }

        private decimal CalcularMonto(ILineaContrato linea, int diasRenta) {
            int? iniSemana;
            int? iniMes;

            List<DiaPeriodoTarifaBO> lstTemp = new List<DiaPeriodoTarifaBO>();
            DiaPeriodoTarifaBR periodoTarifaBR = new DiaPeriodoTarifaBR();
            DiaPeriodoTarifaBO periodoTarifa = new DiaPeriodoTarifaBO() { UnidadOperativaID = this.vista.UnidadOperativaID };

            lstTemp = periodoTarifaBR.Consultar(dctx, periodoTarifa);
            if (lstTemp.Count == 1) {
                periodoTarifa = lstTemp[0];
                iniSemana = periodoTarifa.InicioPeriodoSemana;
                iniMes = periodoTarifa.InicioPeriodoMes;
            }

            decimal? total = 0;
            decimal? tarifaDiaria = 0;
            decimal? montoCobrar = 0;


            if (linea.Cobrable != null) {
                var tarifaContrato = ((TarifaContratoPSLBO)linea.Cobrable);
                if (tarifaContrato.Tarifa > 0) {
                    switch (tarifaContrato.PeriodoTarifa) {
                        case EPeriodosTarifa.Dia:
                            montoCobrar = tarifaContrato.Tarifa * diasRenta;
                            break;
                        case EPeriodosTarifa.Semana:
                            if (diasRenta > periodoTarifa.DiasDuracionSemana) {
                                if (tarifaContrato.PorcentajeDescuento == null)
                                    tarifaContrato.PorcentajeDescuento = 0;

                                tarifaDiaria = tarifaContrato.Tarifa * (1 - (tarifaContrato.PorcentajeDescuento / 100)) / periodoTarifa.DiasDuracionSemana;
                                montoCobrar = tarifaDiaria * diasRenta;
                            }
                            else
                                montoCobrar = tarifaContrato.Tarifa;
                            break;
                        case EPeriodosTarifa.Mes:
                            if (diasRenta > periodoTarifa.DiasDuracionMes) {
                                if (tarifaContrato.PorcentajeDescuento == null)
                                    tarifaContrato.PorcentajeDescuento = 0;

                                tarifaDiaria = tarifaContrato.Tarifa * (1 - (tarifaContrato.PorcentajeDescuento / 100)) / periodoTarifa.DiasDuracionMes;
                                montoCobrar = tarifaDiaria * diasRenta;
                            }
                            else
                                montoCobrar = tarifaContrato.Tarifa;
                            break;
                    }
                }
                total += (tarifaContrato.Maniobra ?? 0) + montoCobrar;
            }
            return (decimal)total;
        }
        #region CheckList
        /// <summary>
        /// Genera el reporte pra la impresión de l check List Completo
        /// </summary>
        public void ImprimirCheckList() {
            try {
                ContratoPSLBO bo = (ContratoPSLBO)this.InterfazUsuarioADato();

                if (this.vista.ModuloID == null)
                    throw new Exception("No se identificó el módulo");
                ETipoUnidad tipoVerificacion = ObtenerETipoUnidad(bo, this.vista.LineaContratoID);
                Dictionary<string, Object> datosImprimir = this.controlador.ObtenerDatosCheckList(dctx, bo, this.vista.ModuloID.Value, tipoVerificacion, this.vista.LineaContratoID);

                this.vista.EstablecerPaqueteNavegacion("NombreReporte", "CheckListEntregaRO");
                this.vista.EstablecerPaqueteNavegacion("DatosReporte", datosImprimir);

                this.vista.RedirigirAImprimir();
            } catch (Exception ex) {
                throw new Exception(this.nombreClase + ".ImprimirCheckList: " + ex.Message);
            }
        }

        /// <summary>
        /// Despliega en la vista las unidad seleccionada en el contrato
        /// </summary>
        /// <param name="unidadBO">Unidad que será desplegada</param>
        private ETipoUnidad ObtenerETipoUnidad(ContratoPSLBO contratoBO, int? linea) {
            UnidadBO unidadBO = contratoBO.LineasContrato.FirstOrDefault(x => x.LineaContratoID == linea).Equipo as UnidadBO;
            int tipoListadoVerificacion = -1;
            ETipoUnidad tipoUnidad = new ETipoUnidad();
            if (ReferenceEquals(unidadBO, null))
                throw new Exception("No es posible recuperar la información de la unidad necesaria para el check list. Intente de nuevo por favor.");

            BPMO.SDNI.Flota.BOF.FlotaBOF bof = new BPMO.SDNI.Flota.BOF.FlotaBOF();
            bof.Unidad = unidadBO;
            bof.Unidad.Sucursal = new SucursalBO { UnidadOperativa = new UnidadOperativaBO { Id = this.vista.UnidadOperativaID } };

            BPMO.SDNI.Flota.BR.SeguimientoFlotaBR flotaBR = new BPMO.SDNI.Flota.BR.SeguimientoFlotaBR();
            BPMO.SDNI.Flota.BO.FlotaBO flota = new BPMO.SDNI.Flota.BO.FlotaBO();
            flota = flotaBR.ConsultarFlotaPSL(dctx, bof);

            if (ReferenceEquals(flota, null))
                throw new Exception("No es posible recuperar la información de la unidad necesaria para el registro del check list. Intente de nuevo por favor.");

            if (ReferenceEquals(flota.ElementosFlota, null))
                throw new Exception("No es posible recuperar la información de la unidad necesaria para el registro del check list. Intente de nuevo por favor.");

            if (flota.ElementosFlota.Count <= 0)
                throw new Exception("No es posible recuperar la información de la unidad necesaria para el registro del check list. Intente de nuevo por favor.");

            BPMO.SDNI.Flota.BO.ElementoFlotaBO elemento = flota.ElementosFlota[0];
            if (!ReferenceEquals(elemento.Unidad, null)) {
                //Aquí se obtendrá el tipo de check list que se mostrará
                switch (this.vista.UnidadOperativaID) {
                    case (int)ETipoEmpresa.Construccion:
                        if (((EAreaConstruccion)elemento.Unidad.Area) == EAreaConstruccion.RO || ((EAreaConstruccion)elemento.Unidad.Area) == EAreaConstruccion.ROC) {
                            tipoListadoVerificacion = (int)controlador.ObtenerTipoUnidadPorClave(this.dctx, (unidadBO).TipoEquipoServicio.NombreCorto, unidadBO);
                        }
                        if (((EAreaConstruccion)elemento.Unidad.Area) == EAreaConstruccion.RE) {
                            tipoListadoVerificacion = (int)ETipoUnidad.LV_SUBARRENDADO;
                            tipoUnidad = ETipoUnidad.LV_SUBARRENDADO;
                        } else {
                            switch (tipoListadoVerificacion) {
                                case 1:
                                    tipoUnidad = ETipoUnidad.LV_COMPRESOR;
                                    break;
                                case 2:
                                    tipoUnidad = ETipoUnidad.LV_MONTACARGA;
                                    break;
                                case 3:
                                    tipoUnidad = ETipoUnidad.LV_MOTONIVELADORA;
                                    break;
                                case 4:
                                    tipoUnidad = ETipoUnidad.LV_MINICARGADOR;
                                    break;
                                case 5:
                                    tipoUnidad = ETipoUnidad.LV_MARTILLO_HIDRAULICO;
                                    break;
                                case 7:
                                    tipoUnidad = ETipoUnidad.LV_EXCAVADORA;
                                    break;
                                case 9:
                                    tipoUnidad = ETipoUnidad.LV_TORRES_LUZ;
                                    break;
                                case 10:
                                    tipoUnidad = ETipoUnidad.LV_PLATAFORMA_TIJERAS;
                                    break;
                                case 11:
                                    tipoUnidad = ETipoUnidad.LV_RETRO_EXCAVADORA;
                                    break;
                                case 12:
                                    tipoUnidad = ETipoUnidad.LV_VIBRO_COMPACTADOR;
                                    break;
                                case 13:
                                    tipoUnidad = ETipoUnidad.LV_PISTOLA_NEUMATICA;
                                    break;
                                case 14:
                                    tipoUnidad = ETipoUnidad.LV_UNIDADES_USADAS;
                                    break;
                                default:
                                    throw new Exception("No es posible recuperar la información del tipo de unidad necesaria para obtener el check list.");
                            }
                        }
                        break;
                    case (int)ETipoEmpresa.Generacion:
                    case (int)ETipoEmpresa.Equinova:
                        tipoListadoVerificacion = (int)ETipoUnidad.LV_ENTREGA_RECEPCION;
                        tipoUnidad = ETipoUnidad.LV_ENTREGA_RECEPCION;
                        break;
                    default:
                        throw new Exception("No es posible recuperar la información del tipo de unidad necesaria para obtener el check list. Intente de nuevo por favor.");

                }
            }
            return tipoUnidad;
        }
        #endregion
        /// <summary>
        /// Método para intercambiar unidades en el contrato
        /// </summary>
        ///// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        ///// <param name="contrato">Contrato que provee el criterio de selección para realizar la consulta</param>
        ///// <param name="conAdjuntos">Indica si en el resultado va a incluir archivos adjuntos</param>
        ///// <returns>Lista de contratos de renta diaria</returns>
        public void IntercambiarUnidadContrato() {
            try {
                ContratoPSLBO bo = (ContratoPSLBO)this.InterfazUsuarioADato();

                if (bo != null && bo.Estatus.HasValue && (bo.Estatus == EEstatusContrato.EnCurso || bo.Estatus == EEstatusContrato.PendientePorCerrar)) {
                    this.LimpiarSesion();
                    this.vista.EstablecerPaqueteNavegacion("UltimoContratoPSLBO", bo);

                    this.vista.RedirigirAIntercambioUnidad();
                    return;
                }

                this.vista.MostrarMensaje("No se puede intercambiar la unidad en el contrato", ETipoMensajeIU.ADVERTENCIA);
                return;
            } catch (Exception ex) {
                throw new Exception(nombreClase + ".IntercambioUnidadContrato: " + ex.Message);
            }
        }


    public void RealizarPrimeraCarga() {
            try {
                this.PrepararVisualizacion();
                this.presentadorDatosContrato.AsignarModoRegistro("DET");
                this.presentadorDatosContrato.PrepararDetalle();
                this.ConsultarCompleto();

                this.CalcularTotales();
                this.presentadorHerramientas.DeshabilitarMenuCerrar();
                this.presentadorHerramientas.vista.OcultarImprimirPlantillaCheckList();
                this.presentadorHerramientas.vista.OcultarPlantillas();

                this.EstablecerSeguridad();
            } catch (Exception ex) {
                throw new Exception(this.nombreClase + ".RealizarPrimeraCarga: " + ex.Message);
            }
        }

        public void ValidarAcceso() {
            try {
                //se valida que los datos del usuario y la unidad operativa no sean nulos
                if (this.vista.UsuarioID == null) throw new Exception("El identificador del usuario no debe ser nulo");
                if (this.vista.UnidadOperativaID == null) throw new Exception("El identificador de la Unidad Operativa no debe ser nulo ");

                //Se crea el objeto de seguridad
                UsuarioBO usuario = new UsuarioBO() { Id = this.vista.UsuarioID };
                AdscripcionBO adscripcion = new AdscripcionBO() { UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaID } };
                SeguridadBO seguridadBO = new SeguridadBO(Guid.Empty, usuario, adscripcion);

                //Se valida si el usuario tiene permiso a la acción principal
                if (!FacadeBR.ExisteAccion(dctx, "CONSULTAR", seguridadBO))
                    this.vista.RedirigirSinPermisoAcceso();
            } catch (Exception ex) {
                throw new Exception(nombreClase + ".ValidarAcceso:" + ex.Message);
            }
        }

        private void PrepararVisualizacion() {
            this.presentadorDatosContrato.Inicializar();            
            this.presentadorDatosContrato.HabilitarControles(false);            
        }

        /// <summary>
        /// Establece la información de los datos de navegación
        /// </summary>
        /// <param name="paqueteNavegacion">información de contratos</param>
        private void EstablecerDatosNavegacion(object paqueteNavegacion) {
            Hashtable parameters = new Hashtable();
            try {
                if (paqueteNavegacion == null)
                    throw new Exception("Se esperaba un objeto en la navegación. No se puede identificar qué contrato se desea consultar.");
                if (!(paqueteNavegacion is ContratoPSLBO))
                    throw new Exception("Se esperaba una contrato.");

                ContratoPSLBO bo = (ContratoPSLBO)paqueteNavegacion;

                parameters["ContratoPSLBO"] = bo;

                this.DatoAInterfazUsuario(parameters);
            } catch (Exception ex) {
                throw new Exception(this.nombreClase + ".EstablecerDatosNavegacion: " + ex.Message);
            }
        }

        /// <summary>
        /// Consultar información completa de contrato PSL
        /// </summary>
        private void ConsultarCompleto() {
            Hashtable parameters = new Hashtable();
            try {
                List<ContratoPSLBO> lst = ConsultarCompletoPRE();

                if (lst.Count < 1)
                    throw new Exception("No se encontró ningún registro que corresponda a la información proporcionada.");
                if (lst.Count > 1)
                    throw new Exception("La consulta devolvió más de un registro.");
                parameters["ContratoPSLBO"] = (ContratoPSLBO)lst[0];
                this.LimpiarSesion();
                this.DatoAInterfazUsuario(parameters);
                if (lst[0] != null && lst[0].DocumentosAdjuntos != null && lst[0].DocumentosAdjuntos.Count > 0)
                    lst[0].DocumentosAdjuntos = lst[0].DocumentosAdjuntos.Where(archivo => archivo.Activo == true).ToList();
                this.presentadorHerramientas.DatosAInterfazUsuario(lst[0]);
                this.vista.UltimoObjeto = lst[0];
            } catch (Exception ex) {
                throw new Exception(this.nombreClase + ".ConsultarCompleto:" + ex.Message);
            }
        }

        /// <summary>
        /// Consultar completo, método encapsulado para consultar el objeto de Unidad.
        /// </summary>
        /// <returns>Regresa una lista de unidades</returns>
        public List<ContratoPSLBO> ConsultarCompletoPRE() {
            try {
                List<ContratoPSLBO> lst = new List<ContratoPSLBO>();
                ContratoPSLBO bo = (ContratoPSLBO)this.vista.ObtenerDatosNavegacion();
                if (bo.FC.HasValue)
                    lst.Add(bo);
                else
                    lst = this.controlador.ConsultarCompleto(this.dctx, bo, true);
                
                return lst;
            } catch (Exception ex) {
                throw new Exception(this.nombreClase + ".ConsultarCompletoPRE:" + ex.Message);
            }
        }
        /// <summary>
        /// Evalúa si se cuenta con el permiso y condiciones para genera el pago, lo cual permite visualizar u ocultar el control.
        /// </summary>
        /// <param name="lst">Lista de permisos</param>
        private void ValidarPermisoGenerarPago(List<CatalogoBaseBO> lst) {
            try {
                //Se de válida el permisos y el estatus del contrato, ya que la generación del pago solamente se aplica cuando el contrato esta en curso, se haya generado previamente un pago y no exista alguno en el visor de pagos pendiente por procesar.
                if (!this.ExisteAccion(lst, "GENERARPAGOS") || !this.vista.EstatusID.HasValue || (this.vista.EstatusID.HasValue && this.vista.EstatusID.Value != (int)EEstatusContrato.EnCurso))
                    this.vista.PermitirGenerarPago(false);
                else {
                    if (ValidarPagosSinProcesar() > 0)
                        this.vista.PermitirGenerarPago(false);
                }
            } catch (Exception ex) {
                throw new Exception(this.nombreClase + ".ValidarPermisoGenerarPago:" + ex.Message);
            }
        }
        /// <summary>
        /// Evalúa si el contrato tiene cuenta con pagos pendientes por procesar en el monitor de pagos
        /// </summary>
        /// <returns>regresa el número de pagos pendientes por procesar</returns>
        private int ValidarPagosSinProcesar() {
            try {
                int numeroPagos = 0;
                var seguridad = new SeguridadBO(Guid.Empty, new UsuarioBO { Id = this.vista.UsuarioID }, new AdscripcionBO { UnidadOperativa = new UnidadOperativaBO { Id = this.vista.UnidadOperativaID } });
                List<SucursalBO> resultado = FacadeBR.ConsultarSucursalesSeguridad(dctx, seguridad) ??
                                              new List<SucursalBO>();
                PagoContratoPSLBR pagoBR = new PagoContratoPSLBR();
                PagoContratoPSLBOF pagoBOF = new PagoContratoPSLBOF();
                pagoBOF.ReferenciaContrato = new ReferenciaContratoBO { FolioContrato = this.vista.NumeroContrato, ReferenciaContratoID = this.vista.ContratoID, UnidadOperativa = new UnidadOperativaBO { Id = this.vista.UnidadOperativaID } };
                pagoBOF.EnviadoFacturacion = false;
                pagoBOF.Sucursales = resultado;
                pagoBOF.Activo = true;
                numeroPagos = pagoBR.ContarPagos(dctx, pagoBOF);
                return numeroPagos;
            } catch (Exception ex) {
                throw new Exception(this.nombreClase + ".ValidarPagosSinProcesar:" + ex.Message);
            }
        }
        /// <summary>
        /// Genera la solicitud de pago, con las mismas condiciones que la del ultimo pago generado
        /// </summary>
        public void GeneraSolicitudPago() {
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
                string Mensaje = string.Empty;
                int numeroPagos = ValidarPagosSinProcesar();
                if (numeroPagos <= 0) {
                    //Se crea el objeto de seguridad
                    UsuarioBO usuario = new UsuarioBO() { Id = this.vista.UsuarioID };
                    AdscripcionBO adscripcion = new AdscripcionBO() { UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaID } };
                    SeguridadBO seguridadBO = new SeguridadBO(Guid.Empty, usuario, adscripcion);

                    #region Creando pago por solicitud
                    //Se obtiene la información a partir de la Interfaz de Usuario
                    ContratoPSLBO bo = (ContratoPSLBO)this.InterfazUsuarioADato();
                    GeneradorPagoPSLBR generadorPago = new GeneradorPagoPSLBR();
                    //Cuando se genera el pago se llenan los campos de acumulados de tarifas
                    generadorPago.GenerarPagos(this.dctx, bo, null, seguridadBO, 3, true);
                    #endregion
                } else {
                    Mensaje = String.Format("No es posible generar la solicitud de pago debido a que existen {0} pago(s) sin procesar en el visor de pagos.", numeroPagos.ToString());
                }
                if (!string.IsNullOrEmpty(Mensaje)) {
                    dctx.RollbackTransaction(firma);
                    this.vista.MostrarMensaje(Mensaje, ETipoMensajeIU.ADVERTENCIA);
                } else {
                    dctx.CommitTransaction(firma);
                    this.presentadorHerramientas.OcultarSolicitudPago();
                    this.vista.MostrarMensaje("La solicitud de pago fue generada satisfactoriamente.", ETipoMensajeIU.EXITO);
                }
            } catch (Exception ex) {
                dctx.RollbackTransaction(firma);
                throw new Exception(this.nombreClase + ".GeneraSolicitudPago:" + ex.Message);
            } finally {
                if (dctx.ConnectionState == ConnectionState.Open)
                    dctx.CloseConnection(firma);
            }
        }

        /// <summary>
        /// Obtiene el contenido de un nodo especificado
        /// </summary>
        /// <param name="nodo">Nombre del nodo a buscar</param>
        /// <param name="documento">Documento XML</param>
        /// <returns>Texto interno del nodo</returns>
        private string GetTextXML(string nodo, XmlDocument documento) {
            XmlNodeList nodoXML = documento.GetElementsByTagName(nodo);
            if (nodoXML.Count < 1) throw new Exception("el formato del archivo XML es incorrecto");
            return nodoXML[0].InnerText;
        }
        #endregion
    }
}
