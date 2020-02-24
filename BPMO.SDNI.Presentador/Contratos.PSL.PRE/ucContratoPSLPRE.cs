using System;
using System.Collections.Generic;
using System.Linq;
using BPMO.Basicos.BO;
using BPMO.Facade.SDNI.BR;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Comun.BOF;
using BPMO.SDNI.Comun.BR;
using BPMO.SDNI.Contratos.BO;
using BPMO.SDNI.Contratos.PSL.BO;
using BPMO.SDNI.Contratos.PSL.BOF;
using BPMO.SDNI.Contratos.PSL.BR;
using BPMO.SDNI.Contratos.PSL.VIS;
using BPMO.SDNI.Equipos.BO;
using BPMO.SDNI.Equipos.BOF;

namespace BPMO.SDNI.Contratos.PSL.PRE
{
    public class ucContratoPSLPRE
    {
        #region Atributos
        private IDataContext dctx = null;
        private ContratoPSLBR controlador;
        private IucContratoPSLVIS vista;

        private string nombreClase = "ucContratoPSLPRE";
        #endregion
        #region Propiedades
        /// <summary>
        /// Vista sobre la que actúa el Presentador de solo lectura
        /// </summary>
        internal IucContratoPSLVIS Vista { get { return vista; } }
        #endregion

        #region Constructores
        public ucContratoPSLPRE(IucContratoPSLVIS view)
        {
            try
            {
                this.vista = view;

                this.controlador = new ContratoPSLBR();
                this.dctx = FacadeBR.ObtenerConexion();
            }
            catch (Exception ex)
            {
                this.vista.MostrarMensaje("Inconsistencia en los parámetros de configuración", ETipoMensajeIU.ERROR, this.nombreClase + ".ucContratoPSLPRE:" + ex.Message);
            }
        }
        #endregion

        #region Métodos
        public void PrepararDetalle() {
            this.EstablecerInformacionInicial();
        }

        public void PrepararNuevo()
        {
            this.LimpiarSesion();
            this.vista.PrepararNuevo();
            this.vista.MostrarRepresentantesAval(false);
            this.EstablecerInformacionInicial();
            this.CalcularOpcionesHabilitadas();            
        }

        public void PrepararEdicion()
        {
            this.vista.PrepararEdicion();
            this.vista.MostrarRepresentantesAval(false);
            this.EstablecerInformacionInicial();
            this.CalcularOpcionesHabilitadas();
        }

        public void PrepararRenovacion()
        {
            this.vista.PrepararRenovacion();
            this.vista.MostrarRepresentantesAval(false);
            this.EstablecerInformacionInicial();            
        }

        private void CalcularOpcionesHabilitadas()
        {
            try
            {
                bool esRenovacion = this.vista.ModoRegistro == "REN";
                bool esDetalle = this.vista.ModoRegistro == "DET";
                bool esEdicionTerminada = this.vista.ModoRegistro == "EDEC";

                //No debe permitir seleccionar una dirección de cliente a menos que se haya seleccionado una cuenta de cliente
                if (!esRenovacion)
                    this.vista.PermitirSeleccionarDireccionCliente(this.vista.CuentaClienteID != null);

                //No mostrar representantes legales, avales ni avales si no hay una cuenta de cliente seleccionada
                if (!esDetalle)
                    this.vista.MostrarPersonasCliente(this.vista.CuentaClienteID != null);

                //Sólo permite seleccionar representantes legales si el cliente ha sido seleccionado y es moral
                if (!esDetalle)
                    vista.MostrarRepresentantesLegales(this.vista.CuentaClienteID != null && this.vista.ClienteEsFisica != null && this.vista.ClienteEsFisica == false);

                if (!esRenovacion)
                    this.vista.PermitirSeleccionarRepresentantes(this.vista.CuentaClienteID != null && this.vista.ClienteEsFisica != null && this.vista.ClienteEsFisica == false);
                //Sólo permite agregar representantes legales si el cliente ha sido seleccionado y tiene representantes configurados y es moral
                if (!esRenovacion)
                    this.vista.PermitirAgregarRepresentantes(this.vista.RepresentantesTotales != null && this.vista.RepresentantesTotales.Count > 0 && this.vista.ClienteEsFisica != null && this.vista.ClienteEsFisica == false);

                //Sólo permite seleccionar avales si el cliente ha sido seleccionado
                if (!esRenovacion)
                    this.vista.PermitirSeleccionarAvales(this.vista.CuentaClienteID != null);
                //Sólo permite agregar avales si el cliente ha sido seleccionado y tiene avales configurados
                if (!esRenovacion)
                    this.vista.PermitirAgregarAvales(this.vista.AvalesTotales != null && this.vista.AvalesTotales.Count > 0);
                this.vista.MostrarAvales(!vista.SoloRepresentantes.Value);

                //No debe permitir seleccionar una unidad a menos que ya se haya seleccionado la sucursal, un cliente y fechas de inicio y de promesa de devolución
                if (!esRenovacion)
                    this.vista.PermitirSeleccionarUnidad(
                        this.vista.SucursalSeleccionada != null && this.vista.SucursalSeleccionada.Id != null && this.vista.CuentaClienteID != null
                        && this.vista.FechaInicioArrendamiento != null && this.vista.FechaPromesaDevolucion != null && this.vista.CodigoMoneda != null);

                //No se debe permitir seleccionar el tipo de confirmación si la forma de pago no es crédito
                if (!esRenovacion)
                    this.vista.PermitirSeleccionarTipoConfirmacion(this.vista.FormaPagoID != null && (EFormaPago)Enum.Parse(typeof(EFormaPago), this.vista.FormaPagoID.ToString()) == EFormaPago.CREDITO);

                //No se debe permitir asignar el autorizador de orden de compra si el tipo de confirmación no es orden de compra
                if (!esRenovacion)
                    this.vista.PermitirAsignarAutorizadorOrdenCompra(this.vista.TipoConfirmacionID != null && (ETipoConfirmacion)Enum.Parse(typeof(ETipoConfirmacion), this.vista.TipoConfirmacionID.ToString()) == ETipoConfirmacion.ORDEN_DE_COMPRA);

                //Siempre debe estar habilitado
                if (!esRenovacion && !esDetalle && !esEdicionTerminada)
                    this.PermitirAgregarProductoServicio(true);

                // Habilitar / Deshabilitar Moneda
                this.EstablecerSeleccionarMoneda();

                //Siempre ocultar el trRepresentantesAval
                this.vista.MostrarRepresentantesAval(false);

            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".CalcularOpcionesHabilitadas: " + ex.Message);
            }
        }

        /// <summary>
        /// Establece el estado de Habilitación del campo de Moneda
        /// </summary>
        public void EstablecerSeleccionarMoneda() {
            this.vista.PermitirSeleccionarMoneda(this.vista.ModoRegistro != "DET" && 
                (this.vista.LineasContrato == null || !this.vista.LineasContrato.Any()));
        }

        private void EstablecerInformacionInicial() {
            #region Conexión a BD
            Guid firma = Guid.NewGuid();
            dctx.SetCurrentProvider("ORACLE");
            dctx.OpenConnection(firma);
            #endregion

            try {
                #region Inicializar Valores
                this.vista.NombreEmpresa = null;
                this.vista.DomicilioEmpresa = null;
                this.vista.RepresentanteEmpresa = null;
                #endregion

                if (this.vista.UnidadOperativaID == null)
                    throw new Exception("No se encontró el identificador de la unidad operativa sobre la que trabaja.");

                #region Unidad Operativa
                string nombreEmpresa = string.Empty;
                if (this.vista.UnidadOperativa == null || this.vista.UnidadOperativa.Empresa == null || string.IsNullOrWhiteSpace(this.vista.UnidadOperativa.Empresa.Nombre)) {
                    //Obtener información de la Unidad Operativa
                    UnidadOperativaBO UO = FacadeBR.ConsultarUnidadOperativaCompleto(this.dctx, new UnidadOperativaBO() { Id = this.vista.UnidadOperativaID }).FirstOrDefault();
                    if (UO == null)
                        throw new Exception("No se encontró la información completa de la unidad operativa sobre la que trabaja.");

                    nombreEmpresa = UO.Empresa.Nombre;
                } else {
                    nombreEmpresa = this.vista.UnidadOperativa.Empresa.Nombre;
                }
                this.vista.NombreEmpresa = this.vista.UnidadOperativa.Empresa.Nombre;
                #endregion

                #region Dirección de la Empresa
                if (string.IsNullOrWhiteSpace(this.vista.Session_DomicilioEmpresa)) {
                    //Obtener la dirección de la empresa
                    SucursalBO sucMatriz = FacadeBR.ObtenerSucursalMatriz(this.dctx, new UnidadOperativaBO() { Id = this.vista.UnidadOperativaID });

                    if (sucMatriz != null && sucMatriz.DireccionesSucursal != null && sucMatriz.DireccionesSucursal.Any())
                        this.vista.Session_DomicilioEmpresa = sucMatriz.DireccionesSucursal[0].Calle;
                }
                this.vista.DomicilioEmpresa = this.vista.Session_DomicilioEmpresa;
                #endregion

                #region Configuraciones de Unidad Operativa
                ConfiguracionUnidadOperativaBO configUO = new ConfiguracionUnidadOperativaBO();
                configUO = this.ObtenerConfiguracionUnidadOperativa(this.vista.UnidadOperativaID, this.vista.ModuloID);
                //Establecer las configuraciones de la unidad operativa
                this.vista.RepresentanteEmpresa = configUO.Representante;
                this.vista.DiasAnterioridadContrato = configUO.DiasAnterioridadContrato ?? (short)0;
                this.vista.PorcentajeSeguro = configUO.PorcentajeSeguro != null ? configUO.PorcentajeSeguro : 0;
                #endregion

                #region Monedas
                if (this.vista.ListaMonedas == null || !this.vista.ListaMonedas.Any())
                    this.vista.ListaMonedas = FacadeBR.ConsultarMoneda(this.dctx, new MonedaBO() { Activo = true });
                this.vista.EstablecerOpcionesMoneda(this.vista.ListaMonedas.ToDictionary(p => p.Codigo, p => p.Nombre));
                #endregion

                #region Sucursales
                this.DesplegarSucursalesAutorizadas();
                #endregion

                #region Frecuencias de Facturacion
                Dictionary<String, String> listaFrecuencias = new Dictionary<String, String>();
                listaFrecuencias.Add(((Int32)EFrecuencia.DIARIA).ToString(), EFrecuencia.DIARIA.ToString());
                listaFrecuencias.Add(((Int32)EFrecuencia.SEMANAL).ToString(), EFrecuencia.SEMANAL.ToString());
                listaFrecuencias.Add(((Int32)EFrecuencia.QUINCENAL).ToString(), EFrecuencia.QUINCENAL.ToString());
                listaFrecuencias.Add(((Int32)EFrecuencia.MENSUAL).ToString(), EFrecuencia.MENSUAL.ToString());
                #endregion
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".EstablecerInformacionInicial: " + ex.Message);
            } finally {
                #region Cierre de Conexión
                dctx.SetCurrentProvider("ORACLE");
                dctx.CloseConnection(firma);
                #endregion /Cierre de Conexión
            }
        }

        /// <summary>
        /// Recupera las sucursales a las que el usuario autenticado tiene permiso e acceder 
        /// </summary>
        private void DesplegarSucursalesAutorizadas() {
            if (this.vista.SucursalesAutorizadas == null || this.vista.SucursalesAutorizadas.Count == 0) {
                SeguridadBO seguridad = new SeguridadBO(Guid.Empty, new UsuarioBO() { Id = this.vista.UsuarioID },
                        new AdscripcionBO { UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaID } });
                this.vista.SucursalesAutorizadas = FacadeBR.ConsultarSucursalesSeguridadSimple(this.dctx, seguridad);
            }
            this.vista.CargarSucursales(this.vista.SucursalesAutorizadas);
        }
        /// <summary>
        /// Obtiene la configuracion de unidad operativa
        /// </summary>
        /// <param name="uoId"> id de la unidad operativa que se trabaja</param>
        /// <param name="moduloId">id del modulo del sistema</param>
        /// <returns> configuracionUnidadOperativa</returns>
        public ConfiguracionUnidadOperativaBO ObtenerConfiguracionUnidadOperativa(int? uoId, int? moduloId) {
            try {
                //Obtener las configuraciones de la unidad operativa
                List<ConfiguracionUnidadOperativaBO> lstConfigUO = new ModuloBR().ConsultarConfiguracionUnidadOperativa(this.dctx, new ConfiguracionUnidadOperativaBO() { UnidadOperativa = new UnidadOperativaBO() { Id = uoId } }, moduloId);
                if (lstConfigUO != null && lstConfigUO.Count <= 0)
                    throw new Exception("No se encontraron las configuraciones del sistema para la unidad operativa en la que trabaja.");
                return lstConfigUO[0];
            } catch  {                
                throw;
            }
        }
        /// <summary>
        /// Indica si la unidad seleccionada cuenta con una reservación con base en el cliente seleccionado, la fecha del contrato y de promesa de entrega
        /// </summary>
        /// <param name="selecto">Objeto seleccionado en el buscador que se espera sea una UnidadBO</param>
        /// <returns>True en caso de que cuente con una reservación, False en caso contrario</returns>
        public bool UnidadTieneReservacion(object selecto)
        {
            try
            {
                if (!(selecto is Equipos.BO.UnidadBO))
                    throw new Exception("El objeto seleccionado no es una Unidad.");
                if (((Equipos.BO.UnidadBO)selecto).UnidadID == null)
                    throw new Exception("No se ha seleccionado la unidad.");
                if (this.vista.FechaContrato == null)
                    throw new Exception("No se ha seleccionado la fecha del contrato.");
                if (this.vista.FechaPromesaDevolucion == null)
                    throw new Exception("No se ha seleccionado la fecha de promesa de devolución.");
                if (this.vista.CuentaClienteID == null)
                    throw new Exception("No se ha seleccionado el cliente.");
                if (this.vista.UnidadOperativaID == null)
                    throw new Exception("No se ha seleccionado la unidad operativa.");

                //Se consulta si existen reservaciones conflictivas
                ContratoPSLBOF bof = new ContratoPSLBOF();
                bof.Unidad = (Equipos.BO.UnidadBO)selecto;
                bof.Cliente = new CuentaClienteIdealeaseBO() { Id = this.vista.CuentaClienteID };
                bof.FechaContrato = this.CalcularFechaCompleta(this.vista.FechaContrato, DateTime.MinValue.Date.TimeOfDay);
                bof.FechaPromesaDevolucion = this.CalcularFechaCompleta(this.vista.FechaPromesaDevolucion, DateTime.MinValue.Date.TimeOfDay);
                bof.Sucursal = new SucursalBO() { UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaID } };
                List<ReservacionPSLBO> lstConflictivas = this.controlador.ConsultarReservacionesConflictivas(dctx, bof);

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
        /// Indica si la unidad seleccionada cuenta con una orden de servicio abierta, es decir, se encuentra en taller
        /// </summary>
        /// <param name="selecto">Objeto seleccionado en el buscador que se espera sea una UnidadBO</param>
        /// <returns>True en caso de que se encuentre en taller, False en caso contrario</returns>
        public bool UnidadEstaEnTaller(object selecto)
        {
            if (!(selecto is Equipos.BO.UnidadBO))
                throw new Exception("El objeto seleccionado no es una Unidad.");

            Equipos.BO.UnidadBO unidad = (Equipos.BO.UnidadBO)selecto;

            //Todavía no se tiene la forma de consultar las órdenes de servicio

            return false;
        }

        #region Métodos para la Selección de Información
        /// <summary>
        /// Establece la selección de forma de pago y realiza los cálculos correspondientes a la selección
        /// </summary>
        /// <param name="formaPagoID">Identificador de la forma de pago</param>
        public void SeleccionarFormaPago(int? formaPagoID)
        {
            this.vista.FormaPagoID = formaPagoID;
            if (!(formaPagoID != null && (EFormaPago)Enum.Parse(typeof(EFormaPago), formaPagoID.ToString()) == EFormaPago.CREDITO))
            {
                this.vista.TipoConfirmacionID = null;
                this.vista.AutorizadorTipoConfirmacion = null;
                this.vista.AutorizadorOrdenCompra = null;
            }
            else
            {
                this.vista.AutorizadorTipoConfirmacion = null;
                this.vista.AutorizadorOrdenCompra = null;
            }

            this.CalcularOpcionesHabilitadas();
        }
        /// <summary>
        /// Establece la selección de tipo de confirmación y realiza los cálculos correspondientes a la selección
        /// </summary>
        /// <param name="tipoConfirmacionID">Identificador del tipo de confirmación</param>
        public void SeleccionarTipoConfirmacion(int? tipoConfirmacionID)
        {
            this.vista.TipoConfirmacionID = tipoConfirmacionID;
            if (!(tipoConfirmacionID != null && (ETipoConfirmacion)Enum.Parse(typeof(ETipoConfirmacion), tipoConfirmacionID.ToString()) == ETipoConfirmacion.ORDEN_DE_COMPRA))
                this.vista.AutorizadorOrdenCompra = null;

            this.CalcularOpcionesHabilitadas();
        }
        /// <summary>
        /// Establece la selección de la fecha de contrato y de devolución y realiza los cálculos correspondientes a la selección
        /// </summary>
        public void SeleccionarFechasContrato()
        {
            string s;
            if ((s = this.ValidarFechasContrato()) != null)
            {
                this.vista.MostrarMensaje(s, ETipoMensajeIU.ADVERTENCIA, null);
                return;
            }

            this.CalcularDiasRenta();

        }
        /// <summary>
        /// Calcula los días de renta de acuerdo tomando en cuenta la fecha de inicio y devolución
        /// </summary>
        public void CalcularDiasRenta()
        {
            DateTime? fechaInicio = null;
            DateTime? fechaFinal = null;
            bool enRenovacion = this.vista.ModoRegistro == "REN";
            bool periodoActual = true;
            #region Se construye una fecha en base a las fechas y horas de contrato y devolución
            if (this.vista.FechaInicioActual != null)
            {
                if (this.vista.EstatusID.HasValue && this.vista.EstatusID.Value != (int)EEstatusContrato.Borrador)
                {
                    fechaInicio = this.vista.FechaInicioArrendamiento;
                    periodoActual = false;
                }
                else
                    fechaInicio = this.vista.FechaInicioActual;
            }
            if (this.vista.FechaPromesaActual != null)
            {
                fechaFinal = this.vista.FechaPromesaActual;
            }
            #endregion

            #region Cálculo de los días de renta
            if (fechaInicio != null && fechaFinal != null)
            {
                ContratoPSLBO contrato = new ContratoPSLBO { FechaInicioArrendamiento = this.vista.FechaInicioArrendamiento, FechaInicioActual = this.vista.FechaInicioActual, FechaPromesaActual = this.vista.FechaPromesaActual, IncluyeSD = this.vista.IncluyeSD };
                this.vista.DiasRenta = contrato.DiasRenta(periodoActual);
            }
            else
                this.vista.DiasRenta = null;
            #endregion

            if (!enRenovacion)
                this.CalcularOpcionesHabilitadas();
        }
        /// <summary>
        /// Dispara el evento de validación de opciones habilitadas al cambiar de Sucursal
        /// </summary>
        public void SeleccionarSucursal() {
            this.CalcularOpcionesHabilitadas();
        }
        /// <summary>
        /// Establece la selección de moneda y realiza los cálculos correspondientes a la selección
        /// </summary>
        /// <param name="codigoMoneda">Código de moneda seleccionada</param>
        public void SeleccionarMoneda(string codigoMoneda)
        {
            this.vista.CodigoMoneda = codigoMoneda;

            this.CalcularOpcionesHabilitadas();
        }

        public void SeleccionarCuentaCliente(CuentaClienteIdealeaseBO cuentaCliente) {
            try {
                #region Dato a Interfaz de Usuario
                if (cuentaCliente.Cliente == null)
                    cuentaCliente.Cliente = new ClienteBO();

                this.vista.CuentaClienteID = cuentaCliente.Id;
                this.vista.ClienteID = cuentaCliente.Cliente.Id;
                this.vista.CuentaClienteNombre = cuentaCliente.Nombre;
                if (cuentaCliente.TipoCuenta != null)
                    this.vista.CuentaClienteTipoID = (int)cuentaCliente.TipoCuenta;
                else
                    this.vista.CuentaClienteTipoID = null;
                this.vista.ClienteRFC = cuentaCliente.Cliente.RFC;
                this.vista.ClienteEsFisica = cuentaCliente.Cliente.Fisica;
                this.vista.CuentaClienteNumeroCuenta = cuentaCliente.Numero;

                this.vista.ClienteDireccionClienteID = null;
                this.vista.ClienteDireccionCalle = null;
                this.vista.ClienteDireccionCiudad = null;
                this.vista.ClienteDireccionCodigoPostal = null;
                this.vista.ClienteDireccionColonia = null;
                this.vista.ClienteDireccionCompleta = null;
                this.vista.ClienteDireccionEstado = null;
                this.vista.ClienteDireccionMunicipio = null;
                this.vista.ClienteDireccionPais = null;

                //Se limpian los representantes legales
                this.vista.RepresentantesSeleccionados = null;
                this.vista.RepresentantesTotales = null;
                vista.SoloRepresentantes = null;

                //Se limpian los avales
                this.vista.AvalesSeleccionados = null;
                this.vista.AvalesTotales = null;
                this.vista.RepresentantesAvalSeleccionados = null;
                this.vista.RepresentantesAvalTotales = null;
                this.vista.ActualizarAvales();
                #endregion

                #region Obtener los representantes y avales (moral)
                if (this.vista.UnidadOperativaID == null)
                    throw new Exception("No se encontró la unidad operativa sobre la que trabaja.");

                CuentaClienteIdealeaseBR cuentaClienteBR = new CuentaClienteIdealeaseBR();
                if (cuentaCliente != null && cuentaCliente.Id != null && cuentaCliente.Cliente.Fisica != null && cuentaCliente.Cliente.Fisica == false) {
                    #region Representantes legales
                    List<RepresentanteLegalBO> lstRepLegales = cuentaClienteBR.ConsultarRepresentantesLegales(dctx,
                                    new RepresentanteLegalBO() { Activo = true }, cuentaCliente).ConvertAll(p => (RepresentanteLegalBO)p);

                    //Marcar depositarios
                    lstRepLegales.Where(r => r.EsDepositario == true).ToList().ForEach(d => d.Nombre = "(D) " + d.Nombre);

                    this.vista.RepresentantesTotales = lstRepLegales;
                    this.vista.RepresentantesSeleccionados = null; 
                    #endregion /Representantes legales

                    #region Obligados Solidarios y Avales
                    List<ObligadoSolidarioBO> lstObligados = cuentaClienteBR.ConsultarObligadosSolidarios(dctx, 
                        new ObligadoSolidarioProxyBO() { Activo = true }, cuentaCliente).ConvertAll(p => (ObligadoSolidarioBO)p);
                                        
                    List<AvalBO> lstAvales = null;
                    if (lstObligados != null && lstObligados.Any())
                        lstAvales = lstObligados.ConvertAll(s => this.ObligadoAAval(s));

                    this.vista.AvalesTotales = lstAvales;
                    this.vista.AvalesSeleccionados = null;
                    #endregion
                } else {
                    this.vista.RepresentantesTotales = null;
                    this.vista.RepresentantesSeleccionados = null;
                    this.vista.AvalesTotales = null;
                    this.vista.AvalesSeleccionados = null;
                }

                this.vista.ActualizarRepresentantesLegales();
                #endregion
                
                this.CalcularOpcionesHabilitadas();
                #region Desplegar la dirección del cliente
                if (this.vista.CuentaClienteID.HasValue && this.vista.ClienteID.HasValue) {
                    this.vista.DesplegarDireccionCliente();
                }
                #endregion /Desplegar dirección
            } catch (Exception ex) {
                throw new Exception(this.nombreClase + ".SeleccionarCuentaCliente: " + ex.Message);
            }
        }
        public bool UnidadCheckList(Equipos.BO.UnidadBO unidad)
        {
            try
            {
                bool valido = true;
                #region Se obtiene la información completa de la unidad y sus trámites

                if (unidad != null && (unidad.UnidadID != null || unidad.EquipoID != null))
                {
                    if (unidad.TipoEquipoServicio == null)
                        throw new Exception("No se encontró la información del tipo de equipo de la unidad seleccionada.");

                    bool esValido = ((EAreaConstruccion)unidad.Area == EAreaConstruccion.RO || (EAreaConstruccion)unidad.Area == EAreaConstruccion.ROC);
                    if (this.vista.UnidadOperativaID == (int)ETipoEmpresa.Construccion && esValido)
                    {

                        CatalogoBaseBO catalogoBase = unidad.TipoEquipoServicio;
                        unidad.TipoEquipoServicio = FacadeBR.ConsultarTipoUnidad(dctx, catalogoBase).FirstOrDefault();

                        ContratoPSLBR Contratobr = new ContratoPSLBR();
                        ETipoUnidad? tipo = Contratobr.ObtenerTipoUnidadPorClave(dctx, unidad.TipoEquipoServicio.NombreCorto, null);

                        if (tipo == null)
                        {
                            valido = false;
                        }

                    }
                }
                else
                {
                    throw new Exception("No se encontró la información de la unidad seleccionada.");
                }
                #endregion

                return valido;
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".UnidadCheckList: " + ex.Message);
            }
        }
       
        #region Operador
        public void SeleccionarOperador(OperadorBO operador)
        {
            #region Dato a Interfaz de Usuario
            if (operador == null) operador = new OperadorBO();
            if (operador.Cliente == null) operador.Cliente = new CuentaClienteIdealeaseBO();
            if (operador.Licencia == null) operador.Licencia = new LicenciaBO();
            if (operador.Direccion == null) operador.Direccion = new DireccionPersonaBO();
            if (operador.Direccion.Ubicacion == null) operador.Direccion.Ubicacion = new UbicacionBO();
            if (operador.Direccion.Ubicacion.Ciudad == null) operador.Direccion.Ubicacion.Ciudad = new CiudadBO();
            if (operador.Direccion.Ubicacion.Estado == null) operador.Direccion.Ubicacion.Estado = new EstadoBO();
            #endregion
            
            this.CalcularOpcionesHabilitadas();
        }
        #endregion
        #endregion

        #region Métodos para el manejo de Representantes Legales
        public void AgregarRepresentantesLegales(List<RepresentanteLegalBO> lst)
        {
            try
            {
                this.vista.RepresentantesSeleccionados = lst;

                this.vista.ActualizarRepresentantesLegales();
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".AgregarRepresentantesLegales: " + ex.Message);
            }
        }

        public void ActualizarRepresentantesLegales()
        {
            this.vista.ActualizarRepresentantesLegales();
        }
        public void AgregarRepresentanteLegal()
        {
            string s;
            if ((s = this.ValidarCamposRepresentanteLegal()) != null)
            {
                this.vista.MostrarMensaje(s, ETipoMensajeIU.INFORMACION, null);
                return;
            }

            try
            {
                if (this.vista.RepresentantesSeleccionados == null)
                    this.vista.RepresentantesSeleccionados = new List<RepresentanteLegalBO>();

                List<RepresentanteLegalBO> representantesSeleccionados = this.vista.RepresentantesSeleccionados;

                //Obtengo el representante legal de la lista de totales
                RepresentanteLegalBO bo = new RepresentanteLegalBO(this.vista.RepresentantesTotales.Find(p => p.Id == this.vista.RepresentanteLegalSeleccionadoID));
                if (bo == null)
                    throw new Exception("No se encontró el representante seleccionado.");

                representantesSeleccionados.Add(bo);

                this.AgregarRepresentantesLegales(representantesSeleccionados);
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".AgregarRepresentanteLegal: " + ex.Message);
            }
        }
        private string ValidarCamposRepresentanteLegal()
        {
            string s = "";

            if (this.vista.RepresentanteLegalSeleccionadoID == null)
                s += "Representante Legal, ";

            if (s.Trim().CompareTo("") != 0)
                return "Los siguientes campos no pueden estar vacíos: \n" + s.Substring(0, s.Length - 2);

            if (this.vista.RepresentantesSeleccionados != null && this.vista.RepresentantesSeleccionados.Exists(p => p.Id == this.vista.RepresentanteLegalSeleccionadoID))
                return "El representante legal seleccionado ya ha sido agregado.";

            return null;
        }

        public void QuitarRepresentanteLegal(int index)
        {
            try
            {
                if (index >= this.vista.RepresentantesSeleccionados.Count || index < 0)
                    throw new Exception("No se encontró el representante legal seleccionado.");

                List<RepresentanteLegalBO> representantesLegales = this.vista.RepresentantesSeleccionados;
                representantesLegales.RemoveAt(index);

                this.vista.RepresentantesSeleccionados = representantesLegales;
                this.vista.ActualizarRepresentantesLegales();
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".QuitarRepresentanteLegal: " + ex.Message);
            }
        }
        #endregion

        #region Métodos para el manejo de Obligados Solidarios
        /// <summary>
        /// Configura las opciones entre avales, avales y representantes legales con base en 'SoloRepresentantes' y 'ObligadosComoAvales'
        /// </summary>
        public void ConfigurarOpcionesPersonas()
        {


            #region Sólo Representantes
            bool soloRepresentantes = this.vista.SoloRepresentantes != null && this.vista.SoloRepresentantes.Value;

            this.vista.PermitirAgregarAvales(!soloRepresentantes);
            this.vista.PermitirSeleccionarAvales(!soloRepresentantes);
            this.vista.MostrarAvales(!soloRepresentantes);

            if (soloRepresentantes)
            {
                this.vista.MostrarRepresentantesAval(false);

                this.vista.AvalesSeleccionados = null;
                this.vista.ActualizarAvales();
            }
            #endregion
        }
        /// <summary>
        /// Realiza cálculos en base al aval seleccionado con base en, por ejemplo, su tipo
        /// </summary>
        public void SeleccionarAval()
        {
            try
            {
                this.vista.MostrarRepresentantesAval(false);
                this.vista.RepresentantesAvalTotales = null;
                this.vista.RepresentantesAvalSeleccionados = null;

                if (this.vista.AvalSeleccionadoID != null)
                {
                    AvalBO bo = this.vista.AvalesTotales.Find(p => p.Id == this.vista.AvalSeleccionadoID).Clonar();
                    if (bo == null)
                        throw new Exception("No se encontró el aval seleccionado.");

                    if (bo.TipoAval != null && bo.TipoAval == ETipoAval.Moral)
                    {
                        this.vista.MostrarRepresentantesAval(true);

                        this.vista.RepresentantesAvalTotales = ((AvalMoralBO)bo).Representantes;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".SeleccionarAval: " + ex.Message);
            }
        }
        /// <summary>
        /// Establece una lista de avales
        /// </summary>
        /// <param name="lst">Lista de avales a establecer</param>
        public void AgregarAvales(List<AvalBO> lst)
        {
            try
            {
                this.vista.AvalesSeleccionados = lst;

                this.vista.ActualizarAvales();
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".AgregarAvales: " + ex.Message);
            }
        }
        /// <summary>
        /// Agrega el aval seleccionado en la vista
        /// </summary>
        public void AgregarAval()
        {
            string s;
            if ((s = this.ValidarCamposAval()) != null)
            {
                this.vista.MostrarMensaje(s, ETipoMensajeIU.INFORMACION, null);
                return;
            }

            try
            {
                if (this.vista.AvalesSeleccionados == null)
                    this.vista.AvalesSeleccionados = new List<AvalBO>();

                List<AvalBO> avalesSeleccionados = this.vista.AvalesSeleccionados;

                //Obtengo el aval de la lista de totales
                AvalBO bo = this.vista.AvalesTotales.Find(p => p.Id == this.vista.AvalSeleccionadoID).Clonar();
                if (bo == null)
                    throw new Exception("No se encontró el aval seleccionado.");

                //Si el Obligado Solidario es Moral, se completa el objeto antes de agregarlo a la lista
                if (bo.TipoAval != null && bo.TipoAval == ETipoAval.Moral)
                {
                    ((AvalMoralBO)bo).Representantes = this.vista.RepresentantesAvalSeleccionados;

                    this.vista.MostrarRepresentantesAval(false);
                    this.vista.RepresentantesAvalSeleccionados = null;
                    this.vista.RepresentantesAvalTotales = null;
                }

                avalesSeleccionados.Add(bo);

                this.AgregarAvales(avalesSeleccionados);
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".AgregarObligadoSolidario: " + ex.Message);
            }
        }
        private string ValidarCamposAval()
        {
            string s = "";

            if (this.vista.AvalSeleccionadoID == null)
                s += "Obligado Solidario, ";

            if (s.Trim().CompareTo("") != 0)
                return "Los siguientes campos no pueden estar vacíos: \n" + s.Substring(0, s.Length - 2);

            if (this.vista.AvalesSeleccionados != null && this.vista.AvalesSeleccionados.Exists(p => p.Id == this.vista.AvalSeleccionadoID))
                return "El aval seleccionado ya ha sido agregado.";

            if (this.vista.AvalSeleccionadoID != null)
            {
                AvalBO bo = this.vista.AvalesTotales.Find(p => p.Id == this.vista.AvalSeleccionadoID);
                if (bo != null && bo.TipoAval != null && bo.TipoAval == ETipoAval.Moral)
                    if (!(this.vista.RepresentantesAvalSeleccionados != null && this.vista.RepresentantesAvalSeleccionados.Count > 0))
                        return "Es necesario seleccionar al menos un representante legal para el aval.";
            }

            return null;
        }
        /// <summary>
        /// Quita un aval de los seleccionados
        /// </summary>
        /// <param name="index">Índice del aval a quitar</param>
        public void QuitarAval(int index)
        {
            try
            {
                if (index >= this.vista.AvalesSeleccionados.Count || index < 0)
                    throw new Exception("No se encontró el aval seleccionado.");

                List<AvalBO> avales = this.vista.AvalesSeleccionados;
                avales.RemoveAt(index);

                this.vista.AvalesSeleccionados = avales;
                this.vista.ActualizarAvales();
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".QuitarObligadoSolidario: " + ex.Message);
            }
        }

        public void AgregarRepresentanteObligado()
        {
            if (this.vista.AvalSeleccionadoID == null)
            {
                this.vista.MostrarMensaje("Es necesario seleccionar un representante para el aval.", ETipoMensajeIU.INFORMACION, null);
                return;
            }

            try
            {
                if (this.vista.RepresentantesAvalSeleccionados == null)
                    this.vista.RepresentantesAvalSeleccionados = new List<RepresentanteLegalBO>();

                List<RepresentanteLegalBO> seleccionados = this.vista.RepresentantesAvalSeleccionados;

                //Obtengo el representante de la lista de totales
                RepresentanteLegalBO bo = new RepresentanteLegalBO(this.vista.RepresentantesAvalTotales.Find(p => p.Id == this.vista.RepresentanteAvalSeleccionadoID));
                if (bo == null)
                    throw new Exception("No se encontró el representante seleccionado.");

                seleccionados.Add(bo);

                this.vista.RepresentantesAvalSeleccionados = seleccionados;
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".AgregarRepresentanteAval: " + ex.Message);
            }
        }
        public void QuitarRepresentanteAval()
        {
            try
            {
                //Obtengo el representante de la lista de totales
                if ((this.vista.RepresentantesAvalSeleccionados.Find(p => p.Id == this.vista.RepresentanteAvalSeleccionadoID)) == null)
                    throw new Exception("No se encontró el representante seleccionado.");

                int index = this.vista.RepresentantesAvalSeleccionados.FindIndex(p => p.Id == this.vista.RepresentanteAvalSeleccionadoID);

                List<RepresentanteLegalBO> representantes = this.vista.RepresentantesAvalSeleccionados;
                representantes.RemoveAt(index);

                this.vista.RepresentantesAvalSeleccionados = representantes;
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".QuitarRepresentanteAval: " + ex.Message);
            }
        }
        public void PrepararVisualizacionRepresentantesAval(int index)
        {
            try
            {
                if (index >= this.vista.AvalesSeleccionados.Count || index < 0)
                    throw new Exception("No se encontró el aval seleccionado.");

                AvalBO bo = this.vista.AvalesSeleccionados[index];
                if (bo is AvalMoralBO)
                    this.vista.MostrarDetalleRepresentantesAval(((AvalMoralBO)bo).Representantes);
                else
                    this.vista.MostrarDetalleRepresentantesAval(null);
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".PrepararVisualizacionRepresentantesAval: " + ex.Message);
            }
        }
        #endregion

        public string ValidarCamposBorrador()
        {
            string s = string.Empty;
            #region Información General
            if (this.vista.UnidadOperativaID == null)
                s += "Unidad Operativa, ";
            if (this.vista.SucursalSeleccionada == null || !this.vista.SucursalSeleccionada.Id.HasValue)
                s += "Sucursal, ";
            if (this.vista.FechaContrato == null)
                s += "Fecha del Contrato, ";
            #endregion

            #region Datos del Cliente
            if (this.vista.CuentaClienteID == null)
                s += "Cuenta del Cliente, ";
            if (this.vista.ClienteEsFisica == null)
                s += "Tipo de Contribuyente del Cliente, ";
            #region Dirección Cliente
            if (string.IsNullOrEmpty(this.vista.ClienteDireccionCalle) || string.IsNullOrEmpty(this.vista.ClienteDireccionCiudad)
                || string.IsNullOrEmpty(this.vista.ClienteDireccionEstado) || string.IsNullOrEmpty(this.vista.ClienteDireccionCodigoPostal)
                || string.IsNullOrEmpty(this.vista.ClienteDireccionPais) || string.IsNullOrEmpty(this.vista.ClienteDireccionMunicipio) || this.vista.ClienteDireccionClienteID == null)
                s += "Dirección del Cliente, ";
            #endregion
            #endregion

            //Unidad a Rentar
            if (!this.vista.LineasContrato.Any())
            {
                s += "Unidades a Renta, ";
            }

            #region Condiciones de la Renta
            if (string.IsNullOrWhiteSpace(this.vista.ClaveProductoServicio))
                s += "Producto o Servicio, ";
            if (string.IsNullOrEmpty(this.vista.DestinoAreaOperacion))
                s += "Ubicación de entrega del bien, ";
            if (string.IsNullOrEmpty(this.vista.MercanciaTransportar))
                s += "Uso del Bien, ";
            if (string.IsNullOrEmpty(this.vista.CodigoMoneda))
                s += "Moneda, ";
            #endregion

            
            if (this.vista.FechaPromesaDevolucion == null)
                s += "Fecha de Promesa de Devolución, ";
            if (this.vista.FormaPagoID == null)
                s += "Forma de Pago, ";
            if (this.vista.FormaPagoID != null && (EFormaPago)Enum.Parse(typeof(EFormaPago), this.vista.FormaPagoID.ToString()) == EFormaPago.CREDITO)
            {
                if (this.vista.TipoConfirmacionID == null)
                    s += "Tipo de Confirmación, ";
                if (string.IsNullOrEmpty(this.vista.AutorizadorTipoConfirmacion))
                    s += "Autorizador del Crédito, ";
            }
            if (this.vista.TipoConfirmacionID != null && (ETipoConfirmacion)Enum.Parse(typeof(ETipoConfirmacion), this.vista.TipoConfirmacionID.ToString()) == ETipoConfirmacion.ORDEN_DE_COMPRA
                && (EFormaPago)Enum.Parse(typeof(EFormaPago), this.vista.FormaPagoID.ToString()) == EFormaPago.CREDITO)
            {
                if (string.IsNullOrEmpty(this.vista.AutorizadorOrdenCompra))
                    s += "Orden de Compra, ";
            }

            if (s != null && s.Trim().CompareTo("") != 0)
                return "Los siguientes campos no pueden estar vacíos: \n" + s.Substring(0, s.Length - 2);

            if (this.vista.ClienteEsFisica != null && this.vista.ClienteEsFisica == false && !(this.vista.RepresentantesSeleccionados != null && this.vista.RepresentantesSeleccionados.Count > 0))
                return "El cliente seleccionado es Moral y, por lo tanto, requiere representantes legales";

            if ((s = this.ValidarFechasContrato()) != null)
                return s;           

            return null;
        }

        public string ValidarCamposRegistro()
        {
            string s = string.Empty;
            bool enRenovacion = this.vista.ModoRegistro == "REN";
            #region Información General
            if (this.vista.UnidadOperativaID == null)
                s += "Unidad Operativa, ";
            if (this.vista.SucursalSeleccionada == null || !this.vista.SucursalSeleccionada.Id.HasValue)
                s += "Sucursal, ";          
            if (this.vista.FechaContrato == null)
                s += "Fecha del Contrato, ";
            #endregion

            #region Datos del Cliente
            if (this.vista.CuentaClienteID == null)
                s += "Cuenta del Cliente, ";
            if (this.vista.ClienteEsFisica == null)
                s += "Tipo de Contribuyente del Cliente, ";
            #region Dirección Cliente
            if (string.IsNullOrEmpty(this.vista.ClienteDireccionCalle) || string.IsNullOrEmpty(this.vista.ClienteDireccionCiudad)
                || string.IsNullOrEmpty(this.vista.ClienteDireccionEstado) || string.IsNullOrEmpty(this.vista.ClienteDireccionCodigoPostal)
                || string.IsNullOrEmpty(this.vista.ClienteDireccionPais) || string.IsNullOrEmpty(this.vista.ClienteDireccionMunicipio) || this.vista.ClienteDireccionClienteID == null)
                s += "Dirección del Cliente, ";
            #endregion
            #endregion

            //Unidad a Rentar
            if (!this.vista.LineasContrato.Any())
            {
                s += "Unidades a Renta, ";
            }
            else
            {
                //Tarifas
                int lineasActiva = 0;
                string mensaje = string.Empty;
                TarifaContratoPSLBO tarifaContra = new TarifaContratoPSLBO();
                foreach (LineaContratoPSLBO linea in this.vista.LineasContrato)
                {
                    if (linea.Activo.HasValue && linea.Activo.Value)
                    {
                        mensaje = string.Empty;
                        if (linea.TipoTarifa == null) mensaje += " Tipo Tarifa, ";

                        if (linea.Cobrable != null)
                        {
                            tarifaContra = (TarifaContratoPSLBO)linea.Cobrable;                            
                            if (tarifaContra.PeriodoTarifa == null) mensaje += " Periodo, ";
                            if (tarifaContra.TarifaTurno == null) mensaje += " Turno, ";
                            if (tarifaContra.TarifaHrAdicional == null) mensaje += " Tarifa Hora Adicional, ";
                            if (tarifaContra.Tarifa == null) mensaje += " Tarifa, ";
                            if (tarifaContra.DuracionDiasPeriodo == null) mensaje += " Duración días período, ";
                            if (tarifaContra.MaximoHrsTurno == null) mensaje += " Máximo horas del turno, ";



                            if (!string.IsNullOrEmpty(mensaje))
                                s += "No han sido configurados los siguientes datos de la unidad " + ((BPMO.SDNI.Equipos.BO.UnidadBO)linea.Equipo).NumeroSerie + " : \n" + mensaje;

                            if (!string.IsNullOrEmpty(mensaje))
                            {
                                lineasActiva++;
                            }

                        }
                        else
                        {
                            s += "No han sido configurados las tarifas para la unidad " + ((BPMO.SDNI.Equipos.BO.UnidadBO)linea.Equipo).NumeroSerie + " \n";
                        }
                    }
                }
            }
            

            #region Condiciones de la Renta
            if (string.IsNullOrWhiteSpace(this.vista.ClaveProductoServicio))
                s += "Producto o Servicio, ";
            if (string.IsNullOrEmpty(this.vista.DestinoAreaOperacion))
                s += "Ubicación de entrega del bien, ";
            if (string.IsNullOrEmpty(this.vista.MercanciaTransportar))
                s += "Uso del Bien, ";
            if (string.IsNullOrEmpty(this.vista.CodigoMoneda))
                s += "Moneda, ";
            if (!this.vista.TasaInteres.HasValue)
                s += "Tasa Interés, ";
            #endregion

            
            if (this.vista.FechaPromesaDevolucion == null)
                s += "Fecha de Promesa de Devolución, ";
            if (this.vista.FormaPagoID == null)
                s += "Forma de Pago, ";
            if (this.vista.FormaPagoID != null && (EFormaPago)Enum.Parse(typeof(EFormaPago), this.vista.FormaPagoID.ToString()) == EFormaPago.CREDITO)
            {
                if (this.vista.TipoConfirmacionID == null)
                    s += "Tipo de Confirmación, ";
                if (string.IsNullOrEmpty(this.vista.AutorizadorTipoConfirmacion))
                    s += "Autorizador del Tipo de Confirmación, ";
            }
            if (this.vista.TipoConfirmacionID != null && (ETipoConfirmacion)Enum.Parse(typeof(ETipoConfirmacion), this.vista.TipoConfirmacionID.ToString()) == ETipoConfirmacion.ORDEN_DE_COMPRA
                && (EFormaPago)Enum.Parse(typeof(EFormaPago), this.vista.FormaPagoID.ToString()) == EFormaPago.CREDITO)
            {
                if (string.IsNullOrEmpty(this.vista.AutorizadorOrdenCompra))
                    s += "Autorizador de la Orden de Compra, ";
            }

            if (s != null && s.Trim().CompareTo("") != 0)
                return "Los siguientes campos no pueden estar vacíos: \n" + s.Substring(0, s.Length - 2);

            if (this.vista.ClienteEsFisica != null && this.vista.ClienteEsFisica == false && !(this.vista.RepresentantesSeleccionados != null && this.vista.RepresentantesSeleccionados.Count > 0))
                return "El cliente seleccionado es Moral y, por lo tanto, requiere representantes legales";

            bool enCurso = this.vista.ModoRegistro == "EDEC";
            if (!enCurso && (s = this.ValidarFechasContrato()) != null)
                return s;

            return null;
        }
        private string ValidarFechasContrato()
        {
            bool enRenovacion = this.vista.ModoRegistro == "REN";
            DateTime fechaMin = DateTime.Today.AddDays(-(double)this.vista.DiasAnterioridadContrato);

            if (!enRenovacion && this.vista.FechaContrato.HasValue && this.vista.FechaContrato.Value.Date < fechaMin)
                return "La fecha del contrato no puede ser menor a " + fechaMin.ToString("dd/MMM/yyyy") + ".";

            if (!enRenovacion && this.vista.FechaInicioActual.HasValue && this.vista.FechaInicioActual.Value.Date < fechaMin)
                return "La fecha del inicio del arrendamiento no puede ser menor a " + fechaMin.ToString("dd/MMM/yyyy") + ".";

            if (this.vista.FechaPromesaActual.HasValue && this.vista.FechaPromesaActual.Value.Date < fechaMin)
                return "La fecha de devolución no puede ser menor a " + fechaMin.ToString("dd/MMM/yyyy") + ".";

            if (this.vista.FechaContrato != null && this.vista.FechaInicioActual != null && this.vista.FechaPromesaActual != null)
            {
                if (this.vista.FechaContrato > this.vista.FechaInicioActual)
                    return "La fecha del contrato no puede ser mayor a la de inicio de arrendamiento.";

                if (this.vista.FechaContrato > this.vista.FechaPromesaActual)
                    return "La fecha del contrato no puede ser mayor a la de devolución.";

                if (this.vista.FechaInicioActual < this.vista.FechaContrato)
                    return "La fecha de inicio de arrendamiento no puede ser menor a la del contrato.";

                if (this.vista.FechaInicioActual > this.vista.FechaPromesaActual)
                    return "La fecha de inicio de arrendamiento no puede ser mayor a la de devolución.";

                if (this.vista.FechaPromesaActual < this.vista.FechaInicioActual)
                    return "La fecha de devolución no puede ser menor a la de inicio de contrato.";

                if (this.vista.FechaPromesaActual < this.vista.FechaContrato)
                    return "La fecha de devolución no puede ser menor a la del contrato.";
            }

            return null;
        }

        private DateTime? CalcularFechaCompleta(DateTime? fecha, TimeSpan? hora)
        {
            DateTime? fechaFinal = null;

            if (fecha != null)
            {
                if (hora != null)
                    fechaFinal = new DateTime(fecha.Value.Year, fecha.Value.Month, fecha.Value.Day, hora.Value.Hours, hora.Value.Minutes, hora.Value.Seconds, hora.Value.Milliseconds);
                else
                    fechaFinal = fecha;
            }

            return fechaFinal;
        }

        public void AsignarModoRegistro(string Modo)
        {
            this.vista.ModoRegistro = Modo;
        }

        public void LimpiarSesion()
        {
            this.vista.LimpiarSesion();
        }

        #region Métodos para el Buscador

        public object PrepararBOBuscador(string catalogo) {
            object obj = null;
            int aux = 0;

            switch (catalogo) {
                case "CuentaClienteIdealease":
                    CuentaClienteIdealeaseBOF cuentaCliente = new CuentaClienteIdealeaseBOF { UnidadOperativa = vista.UnidadOperativa, Activo = true };
                    if (int.TryParse(this.vista.CuentaClienteNombre, out aux))
                        cuentaCliente.Id = aux;
                    else
                        cuentaCliente.Nombre = vista.CuentaClienteNombre;
                    obj = cuentaCliente;
                    break;
                case "DireccionCliente":
                    DireccionCuentaClienteBOF dirCuenta = new DireccionCuentaClienteBOF() {
                        Cuenta = new CuentaClienteIdealeaseBO() {
                            Id = this.vista.CuentaClienteID,
                            UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaID },
                            Cliente = new ClienteBO() { Id = this.vista.ClienteID }
                        },
                        Direccion = new DireccionClienteBO() { Facturable = true }
                    };

                    obj = dirCuenta;
                    break;
                case "Unidad":
                    bool esROC = (this.vista.EsROC.HasValue && this.vista.EsROC.Value);
                    UnidadBOF unidad = new UnidadBOF() {
                        EstatusActual = EEstatusUnidad.Disponible,
                        Sucursal = new SucursalBO() { Id = this.vista.SucursalSeleccionada != null ? this.vista.SucursalSeleccionada.Id : null }
                    };
                    
                    #region Area
                    unidad.TiposContrato = new List<int>();
                    switch (this.vista.UnidadOperativaID) {
                        case (int)ETipoEmpresa.Construccion:
                            if (esROC) {
                                unidad.TiposContrato.Add((int)EAreaConstruccion.ROC);
                            } else {
                                unidad.TiposContrato.Add((int)EAreaConstruccion.RO);
                                unidad.TiposContrato.Add((int)EAreaConstruccion.RE);
                            }
                            break;
                        case (int)ETipoEmpresa.Generacion:
                            if (esROC) {
                                unidad.TiposContrato.Add((int)EAreaGeneracion.ROC);
                            } else {
                                unidad.TiposContrato.Add((int)EAreaGeneracion.RO);
                                unidad.TiposContrato.Add((int)EAreaGeneracion.RE);
                            }
                            break;
                        case (int)ETipoEmpresa.Equinova:
                            if (esROC) {
                                unidad.TiposContrato.Add((int)EAreaEquinova.ROC);
                            } else {
                                unidad.TiposContrato.Add((int)EAreaEquinova.RO);
                                unidad.TiposContrato.Add((int)EAreaEquinova.RE);
                            }
                            break;
                        case (int)ETipoEmpresa.Idealease:
                            unidad.TiposContrato = null;
                            unidad.Area = EArea.RD;
                            break;
                    }
                    #endregion

                    if (!string.IsNullOrEmpty(vista.NumeroSerie))
                        unidad.NumeroSerie = vista.NumeroSerie;
                    
                    obj = unidad;
                    break;
                case "ProductoServicio":
                    ProductoServicioBO producto = new ProductoServicioBO() { Activo = true };
                    if (Int32.TryParse(vista.ClaveProductoServicio, out aux))
                        producto.NombreCorto = vista.ClaveProductoServicio;
                    else
                        producto.Nombre = vista.ClaveProductoServicio;

                    obj = producto;
                    break;
            }

            return obj;
        }
        public void DesplegarResultadoBuscador(string catalogo, object selecto) {
            switch (catalogo) {
                case "CuentaClienteIdealease":
                    CuentaClienteIdealeaseBOF cliente = (CuentaClienteIdealeaseBOF)selecto ?? new CuentaClienteIdealeaseBOF();
                    this.SeleccionarCuentaCliente(cliente);
                    break;
                case "DireccionCliente":
                    DireccionCuentaClienteBOF bof = (DireccionCuentaClienteBOF)selecto ?? new DireccionCuentaClienteBOF();
                    if (bof.Direccion == null) {
                        vista.MostrarMensaje("No se ha seleccionado una dirección Facturable.", ETipoMensajeIU.INFORMACION, null);
                        bof.Direccion = new DireccionClienteBO();
                    }
                    if (bof.Direccion.Ubicacion == null)
                        bof.Direccion.Ubicacion = new UbicacionBO();
                    if (bof.Direccion.Ubicacion.Ciudad == null)
                        bof.Direccion.Ubicacion.Ciudad = new CiudadBO();
                    if (bof.Direccion.Ubicacion.Estado == null)
                        bof.Direccion.Ubicacion.Estado = new EstadoBO();
                    if (bof.Direccion.Ubicacion.Municipio == null)
                        bof.Direccion.Ubicacion.Municipio = new MunicipioBO();
                    if (bof.Direccion.Ubicacion.Pais == null)
                        bof.Direccion.Ubicacion.Pais = new PaisBO();

                    this.vista.ClienteDireccionClienteID = bof.Direccion.Id;
                    this.vista.ClienteDireccionCompleta = bof.Direccion.Direccion;
                    this.vista.ClienteDireccionCalle = bof.Direccion.Calle;
                    this.vista.ClienteDireccionColonia = bof.Direccion.Colonia;
                    this.vista.ClienteDireccionCodigoPostal = bof.Direccion.CodigoPostal;
                    this.vista.ClienteDireccionCiudad = bof.Direccion.Ubicacion.Ciudad.Codigo;
                    this.vista.ClienteDireccionEstado = bof.Direccion.Ubicacion.Estado.Codigo;
                    this.vista.ClienteDireccionMunicipio = bof.Direccion.Ubicacion.Municipio.Codigo;
                    this.vista.ClienteDireccionPais = bof.Direccion.Ubicacion.Pais.Codigo;
                    break;
                case "Unidad":
                    UnidadBOF unidadBO = (UnidadBOF)selecto ?? new UnidadBOF();
                    if (selecto != null && (unidadBO.UnidadID != null)) {
                        if (this.UnidadTieneReservacion(unidadBO))
                            this.vista.MostrarMensaje("La unidad ya se encuentra reservada para otro cliente", ETipoMensajeIU.ADVERTENCIA);

                        if (this.UnidadCheckList(unidadBO)) {
                            vista.NumeroSerie = !string.IsNullOrWhiteSpace(unidadBO.NumeroSerie) ? unidadBO.NumeroSerie : string.Empty;
                            vista.UnidadID = unidadBO.UnidadID;
                            vista.EquipoID = unidadBO.EquipoID;
                            vista.HabilitarAgregarUnidad(vista.UnidadID != null);
                        } else
                            this.vista.MostrarMensaje("No es posible seleccionar la unidad " + unidadBO.NumeroSerie + " debido a que no existe un checklist para su tipo (" + unidadBO.TipoEquipoServicio.NombreCorto + ")", ETipoMensajeIU.ADVERTENCIA);
                    }

                    break;
                case "ProductoServicio":
                    ProductoServicioBO producto = (ProductoServicioBO)selecto ?? new ProductoServicioBO();
                    vista.ProductoServicioId = producto.Id;
                    vista.ClaveProductoServicio = producto.NombreCorto;
                    vista.DescripcionProductoServicio = producto.Nombre;
                    break;
            }
        }
        #endregion

        public void PermitirAgregarProductoServicio(bool permitir)
        {
            this.vista.PermitirAgregarProductoServicio(permitir);
        }

        private AvalBO ObligadoAAval(ObligadoSolidarioBO obligado)
        {
            if (obligado == null) return null;

            AvalBO aval;

            switch (obligado.TipoObligado)
            {
                case ETipoObligadoSolidario.Fisico:
                    aval = new AvalFisicoBO(obligado);
                    break;
                case ETipoObligadoSolidario.Moral:
                    aval = new AvalMoralBO(obligado);
                    if (obligado is ObligadoSolidarioMoralBO && ((ObligadoSolidarioMoralBO)obligado).Representantes != null)
                        ((AvalMoralBO)aval).Representantes = new List<RepresentanteLegalBO>(((ObligadoSolidarioMoralBO)obligado).Representantes);
                    break;
                default:
                    aval = new AvalProxyBO(obligado);
                    break;
            }

            return aval;
        }
        public void AgregarRepresentanteAval()
        {
            if (this.vista.RepresentanteAvalSeleccionadoID == null)
            {
                this.vista.MostrarMensaje("Es necesario seleccionar un representante para el aval.", ETipoMensajeIU.INFORMACION, null);
                return;
            }

            try
            {
                if (this.vista.RepresentantesAvalSeleccionados == null)
                    this.vista.RepresentantesAvalSeleccionados = new List<RepresentanteLegalBO>();

                List<RepresentanteLegalBO> seleccionados = this.vista.RepresentantesAvalSeleccionados;

                //Obtengo el representante de la lista de totales
                RepresentanteLegalBO bo = new RepresentanteLegalBO(this.vista.RepresentantesAvalTotales.Find(p => p.Id == this.vista.RepresentanteAvalSeleccionadoID));
                if (bo == null)
                    throw new Exception("No se encontró el representante seleccionado.");

                seleccionados.Add(bo);

                this.vista.RepresentantesAvalSeleccionados = seleccionados;
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".AgregarRepresentanteAval: " + ex.Message);
            }
        }
        public void ActualizarAvales()
        {
            vista.ActualizarAvales();
            this.vista.MostrarRepresentantesAval(false);
        }

        /// <summary>
        /// Indica si la unidad pertenece al Contrato en Captura
        /// </summary>
        /// <param name="unidad"></param>
        /// <returns></returns>
        public bool ExisteUnidadContrato(SDNI.Equipos.BO.UnidadBO unidad)
        {
            return (vista.LineasContrato.Find(li => ((SDNI.Equipos.BO.UnidadBO)li.Equipo).UnidadID == unidad.UnidadID) != null);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public SDNI.Equipos.BO.UnidadBO ObtenerUnidadAgregar()
        {
            return new SDNI.Equipos.BO.UnidadBO
            {
                EquipoID = vista.EquipoID,
                NumeroSerie = vista.NumeroSerie,
                UnidadID = vista.UnidadID
            };
        }
        /// <summary>
        /// Agrega una línea de Contrato
        /// </summary>
        /// <param name="linea">Línea de Contrato a Agregar</param>
        public void AgregarLineaContrato(LineaContratoPSLBO linea)
        {
            try
            {
                if (linea != null)
                {
                    var unidad = linea.Equipo as SDNI.Equipos.BO.UnidadBO;
                    if (unidad != null && unidad.UnidadID != null)
                    {
                        // Verificar Unidad en Líneas de Contrato
                        LineaContratoPSLBO lineaRepetida =
                            vista.LineasContrato.Find(li => ((SDNI.Equipos.BO.UnidadBO)li.Equipo).UnidadID == unidad.UnidadID);
                        if (lineaRepetida != null)
                        {
                            linea.LineaContratoID = lineaRepetida.LineaContratoID;
                            vista.LineasContrato.Remove(lineaRepetida);
                        }

                        var lineasContrato = new List<LineaContratoPSLBO>(vista.LineasContrato) { linea };

                        vista.LineasContrato = lineasContrato;
                    }
                    else
                        throw new Exception("Se requiere una Unidad valida para agregarla al detalle del contrato.");
                }
                else
                    throw new Exception("No se ha proporcionado una línea de contrato");
            }
            catch (Exception ex)
            {
                vista.MostrarMensaje("Inconsistencias al Agregar una Unidad al contrato.", ETipoMensajeIU.ERROR, nombreClase + ".AgregarLineaContrato: " + ex.Message);
            }
        }
        /// <summary>
        /// Remueve una línea de contrato
        /// </summary>
        /// <param name="linea">Línea de Contrato a remover</param>
        public void RemoverLineaContrato(LineaContratoPSLBO linea)
        {
            try
            {
                if (linea != null)
                {
                    var unidad = linea.Equipo as SDNI.Equipos.BO.UnidadBO;
                    if (unidad != null && unidad.UnidadID != null)
                    {
                        // Verificar Unidad en Líneas de Contrato
                        if (vista.LineasContrato.Find(li => ((SDNI.Equipos.BO.UnidadBO)li.Equipo).UnidadID == unidad.UnidadID) != null)
                        {
                            var lineasContrato = new List<LineaContratoPSLBO>(vista.LineasContrato);
                            lineasContrato.Remove(linea);

                            vista.LineasContrato = lineasContrato;
                            this.EstablecerSeleccionarMoneda();
                        }
                        else
                            throw new Exception("La unidad ya esta asignada al contrato");
                    }
                    else
                        throw new Exception("Se requiere una Unidad valida para agregarla al detalle del contrato.");
                }
                else
                    throw new Exception("No se ha proporcionado una línea de contrato");
            }
            catch (Exception ex)
            {
                vista.MostrarMensaje("Inconsistencias en al remover la Unidad del contrato.", ETipoMensajeIU.ERROR, nombreClase + ".RemoverLineaContrato: " + ex.Message);
            }
        }
        public void InicializarAgregarUnidad()
        {
            vista.UnidadID = null;
            vista.EquipoID = null;
            vista.NumeroSerie = null;
            vista.HabilitarAgregarUnidad(false);
        }

        /// <summary>
        /// Inicializa la vista
        /// </summary>
        public void Inicializar() {
            vista.FechaContrato = null;
            vista.NombreEmpresa = null;
            vista.EstablecerSucursalSeleccionada(0);
            vista.RepresentanteEmpresa = null;
            vista.DomicilioEmpresa = null;

            DesplegarEmpresa();
        }

        /// <summary>
        /// Despliega el nombre de la Empresa
        /// </summary>
        private void DesplegarEmpresa() {
            try {
                string nombreEmpresa = string.Empty;
                if (this.vista.UnidadOperativa == null || this.vista.UnidadOperativa.Empresa == null || string.IsNullOrWhiteSpace(this.vista.UnidadOperativa.Empresa.Nombre)) {
                    //Obtener información de la Unidad Operativa
                    UnidadOperativaBO UO = FacadeBR.ConsultarUnidadOperativaCompleto(this.dctx, new UnidadOperativaBO() { Id = this.vista.UnidadOperativaID }).FirstOrDefault();
                    if (UO == null || UO.Empresa == null)
                        throw new Exception("No se encontró la información completa de la unidad operativa sobre la que trabaja.");

                    nombreEmpresa = UO.Empresa.Nombre;
                } else {
                    nombreEmpresa = this.vista.UnidadOperativa.Empresa.Nombre;
                }
                this.vista.NombreEmpresa = this.vista.UnidadOperativa.Empresa.Nombre;
            } catch (Exception ex) {
                vista.MostrarMensaje("Inconsistencias al presentar la información de la empresa.", ETipoMensajeIU.ERROR, nombreClase + ".DesplegarEmpresa: " + ex.Message);
            }
        }
        /// <summary>
        /// Valida si existen líneas activas sin tarifa
        /// </summary>
        /// <returns>Indica si la lista está vacía o existen líneas activas sin tarifa</returns>
        public bool ExistenLineasNoValidas() {
            // Si la lista está vacía o no hay una sola línea activa
            if (this.vista.LineasContrato == null || !this.vista.LineasContrato.Any(l => l.Activo.Value == true))
                return true;

            // Si existe al menos una tarifa activa sin cobrable retorna true == Inválido
            return this.vista.LineasContrato.Any(x => x.Activo.Value && (x.Cobrable == null || ((TarifaContratoPSLBO)x.Cobrable).Tarifa == null));
        }

        /// <summary>
        /// Permite habilitar/deshabilitar los controles de los datos de un contrato
        /// </summary>
        /// <param name="habilitar">Indica si se habilita o deshabilitan los controles </param>
        public void HabilitarControles(bool habilitar)
        {
            this.vista.PrepararVistaDetalle();
        }
        #region Cálculo de subtotales
        public System.Data.DataSet ObtenerMontoContrato() {
            try {
                #region Consulta periodoTarifario.
                int? iniSemana;
                int? iniMes;
                int? diasPeriodo = this.vista.DiasRenta;
                diasPeriodo = (((TimeSpan)(this.vista.FechaPromesaActual - this.vista.FechaInicioActual)).Days) + 1;
                List<DiaPeriodoTarifaBO> lstTemp = new List<DiaPeriodoTarifaBO>();
                DiaPeriodoTarifaBR periodoTarifaBR = new DiaPeriodoTarifaBR();
                DiaPeriodoTarifaBO periodoTarifa = new DiaPeriodoTarifaBO() { UnidadOperativaID = this.vista.UnidadOperativaID };
                lstTemp = periodoTarifaBR.Consultar(dctx, periodoTarifa);
                if (lstTemp.Count == 1) {
                    periodoTarifa = lstTemp[0];
                    iniSemana = periodoTarifa.InicioPeriodoSemana;
                    iniMes = periodoTarifa.InicioPeriodoMes;
                } else
                    throw new Exception("No se encontró la configuración de período tarifario.");
                #endregion
                #region Calculo de cobros
                System.Data.DataSet dsLineas = new System.Data.DataSet();
                #region Construir estructura del contenedor
                dsLineas.Tables.Add("LineasCobro");
                dsLineas.Tables[0].Columns.Add("Serie", typeof(string));
                dsLineas.Tables[0].Columns.Add("TarifaBase", typeof(string));
                dsLineas.Tables[0].Columns.Add("Tarifa", typeof(decimal));
                dsLineas.Tables[0].Columns.Add("Maniobra", typeof(decimal));
                dsLineas.Tables[0].Columns.Add("Seguro", typeof(decimal));
                dsLineas.Tables[0].Columns.Add("Monto", typeof(decimal));
                #endregion
                decimal? suma = 0;
                decimal? montoCobrar = 0;
                foreach (LineaContratoPSLBO linea in this.vista.LineasContrato.Where(l => l.Activo.Value == true)) {
                    System.Data.DataRow rowLineas = dsLineas.Tables[0].NewRow();
                    if (linea.Cobrable != null) {
                        var tarifaContrato = ((TarifaContratoPSLBO)linea.Cobrable);
                        rowLineas["Serie"] = linea.Equipo.NumeroSerie;
                        decimal? tarifaCalculada = null;
                        if(tarifaContrato.TarifaConDescuento != null && tarifaContrato.TarifaConDescuento > 0)
                            tarifaCalculada = tarifaContrato.TarifaConDescuento;
                        else
                            tarifaCalculada = tarifaContrato.Tarifa != null ?  tarifaContrato.Tarifa  : 0;
                        if (tarifaCalculada > 0) {
                            montoCobrar = tarifaCalculada;
                            switch (tarifaContrato.PeriodoTarifa){ 
                                case EPeriodosTarifa.Dia:
                                    montoCobrar = Math.Round((decimal)(tarifaCalculada * diasPeriodo), 2);
                                    break;
                                case EPeriodosTarifa.Semana:
                                    montoCobrar = Math.Round((decimal)(tarifaCalculada / periodoTarifa.DiasDuracionSemana * diasPeriodo), 2);
                                    break;
                                case EPeriodosTarifa.Mes:
                                    montoCobrar = Math.Round((decimal)(tarifaCalculada / periodoTarifa.DiasDuracionMes * diasPeriodo), 2);
                                    break;
                            }
                        }
                        rowLineas["TarifaBase"] = string.Format("{0:c2}", tarifaContrato.Tarifa);
                        rowLineas["Tarifa"] = montoCobrar;
                        rowLineas["Maniobra"] = tarifaContrato.Maniobra ?? 0;
                        rowLineas["Seguro"] = Math.Round( (decimal)(montoCobrar * (tarifaContrato.PorcentajeSeguro / 100)), 2);
                        decimal? subtotal =  montoCobrar + (tarifaContrato.Maniobra ?? 0) + Math.Round((decimal)(montoCobrar * (tarifaContrato.PorcentajeSeguro/100)),2);
                        rowLineas["Monto"] = subtotal;
                        dsLineas.Tables[0].Rows.Add(rowLineas);
                        suma += subtotal;
                    }
                }
                this.vista.MontoTotal = suma;
                #endregion
            return dsLineas;
            } catch (Exception ex) {
                this.vista.MostrarMensaje("Se encontraron inconsistencas al obtener los subtotales.", ETipoMensajeIU.ADVERTENCIA,ex.Message);
                return null;
            }
        }
       
        #endregion
        #endregion

        public string ValidarCamposContratoROC(bool? validarCamposContratoROC)
        {
            string sError = string.Empty;

            //Si llega valor false es por que viene la ejecución de la sección de representantes del catálogo de clientes donde no es obligatoria esta información
            if (validarCamposContratoROC.Value)
            {
                if (vista.MontoTotalArrendamiento == null)
                    sError += "Monto Total de Arrendamiento + IVA, ";
                if (vista.FechaPagoRenta == null)
                    sError += "Fecha Pago Renta, ";
                if (vista.Plazo == null)
                    sError += "Plazo, ";
                if (vista.InversionInicial == null)
                    sError += "Inversión Inicial, ";
            }

            if (sError != null && sError.Trim().CompareTo("") != 0)
                return "Los siguientes campos no pueden estar vacíos: \n" + sError.Substring(0, sError.Length - 2);

            return null;
        }
        /// <summary>
        /// Valida si la fecha es un día hábil
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        private bool EsHabil(DateTime date)
        {
            return date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday;
        }
    }
}
