// Satisface al CU001 - Registrar Contrato de Renta Diaria
// Satisface al CU002 - Editar Contrato Renta Diaria
// Satisface a la solicitud de Cambio SC0001
// Satisface a la solicitud de cambio SC0021
// BEP1401 Satisface a la SC0026
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using BPMO.Basicos.BO;
using BPMO.Facade.SDNI.BOF;
using BPMO.Facade.SDNI.BR;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Comun.BOF;
using BPMO.SDNI.Comun.BR;
using BPMO.SDNI.Contratos.BO;
using BPMO.SDNI.Contratos.RD.BO;
using BPMO.SDNI.Contratos.RD.BOF;
using BPMO.SDNI.Contratos.RD.BR;
using BPMO.SDNI.Contratos.RD.VIS;
using BPMO.SDNI.Equipos.BO;
using BPMO.SDNI.Equipos.BOF;
using BPMO.SDNI.Equipos.BR;
using BPMO.SDNI.Tramites.BO;
using BPMO.SDNI.Tramites.BR;
using BPMO.Servicio.Catalogos.BO;

namespace BPMO.SDNI.Contratos.RD.PRE
{
    public class ucContratoRDPRE
    {
        #region Atributos
        private IDataContext dctx = null;
        private ContratoRDBR controlador;
        private IucContratoRDVIS vista;

        private string nombreClase = "ucContratoRDPRE";
        #endregion

        #region Constructores
        public ucContratoRDPRE(IucContratoRDVIS view)
        {
            try
            {
                this.vista = view;

                this.controlador = new ContratoRDBR();
                this.dctx = FacadeBR.ObtenerConexion();
            }
            catch (Exception ex)
            {
                this.vista.MostrarMensaje("Inconsistencia en los parámetros de configuración", ETipoMensajeIU.ERROR, this.nombreClase + ".ucContratoRDPRE:" + ex.Message);
            }
        }
        #endregion

        #region Métodos
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

        private void CalcularOpcionesHabilitadas()
        {
            try
            {
                //No debe permitir seleccionar una dirección de cliente a menos que se haya seleccionado una cuenta de cliente
                this.vista.PermitirSeleccionarDireccionCliente(this.vista.CuentaClienteID != null);

				//No mostrar representantes legales, avales ni avales si no hay una cuenta de cliente seleccionada
				this.vista.MostrarPersonasCliente(this.vista.CuentaClienteID != null);

                //No debe permitir seleccionar un operador a menos que se haya seleccionado un cliente
                this.vista.PermitirSeleccionarOperador(this.vista.CuentaClienteID != null);

                //Sólo permite seleccionar representantes legales si el cliente ha sido seleccionado y es moral
				vista.MostrarRepresentantesLegales(this.vista.CuentaClienteID != null && this.vista.ClienteEsFisica != null && this.vista.ClienteEsFisica == false);
				this.vista.PermitirSeleccionarRepresentantes(this.vista.CuentaClienteID != null && this.vista.ClienteEsFisica != null && this.vista.ClienteEsFisica == false);
				//Sólo permite agregar representantes legales si el cliente ha sido seleccionado y tiene representantes configurados y es moral
				this.vista.PermitirAgregarRepresentantes(this.vista.RepresentantesTotales != null && this.vista.RepresentantesTotales.Count > 0 && this.vista.ClienteEsFisica != null && this.vista.ClienteEsFisica == false);

				//Sólo permite seleccionar avales si el cliente ha sido seleccionado
				this.vista.PermitirSeleccionarAvales(this.vista.CuentaClienteID != null);
				//Sólo permite agregar avales si el cliente ha sido seleccionado y tiene avales configurados
				this.vista.PermitirAgregarAvales(this.vista.AvalesTotales != null && this.vista.AvalesTotales.Count > 0);
				this.vista.MostrarAvales(!vista.SoloRepresentantes.Value);

                //No debe permitir seleccionar una unidad a menos que ya se haya seleccionado la sucursal, un cliente y fechas de contrato y de promesa de devolución
                this.vista.PermitirSeleccionarUnidad(this.vista.SucursalID != null && this.vista.CuentaClienteID != null && this.vista.FechaContrato != null && this.vista.FechaPromesaDevolucion != null);

                //No se debe permitir seleccionar una tarifa a menos que haya sucursal, moneda, unidad, cliente y unidad operativa
                this.vista.PermitirSeleccionarTarifas(this.vista.UnidadOperativaID != null && this.vista.SucursalID != null && this.vista.CuentaClienteID != null && this.vista.CodigoMoneda != null && this.vista.ModeloID != null && this.vista.CuentaClienteTipoID != null);
				
                //No se debe permitir seleccionar el tipo de confirmación si la forma de pago no es crédito
                this.vista.PermitirSeleccionarTipoConfirmacion(this.vista.FormaPagoID != null && (EFormaPago)Enum.Parse(typeof(EFormaPago), this.vista.FormaPagoID.ToString()) == EFormaPago.CREDITO);

                //No se debe permitir asignar el autorizador de orden de compra si el tipo de confirmación no es orden de compra
                this.vista.PermitirAsignarAutorizadorOrdenCompra(this.vista.TipoConfirmacionID != null && (ETipoConfirmacion)Enum.Parse(typeof(ETipoConfirmacion), this.vista.TipoConfirmacionID.ToString()) == ETipoConfirmacion.ORDEN_DE_COMPRA);

                //No se debe permitir seleccionar los dias a facturar sino se ha seleccionado fecha de contrato y fecha promesa
                this.vista.PemitirSeleccionarDiasFacturar(this.vista.FechaContrato != null && this.vista.FechaPromesaDevolucion != null);
			}
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".CalcularOpcionesHabilitadas: " + ex.Message);
            }
        }

        /// <summary>
        /// presenta la informacion inicial del contrato
        /// </summary>
        private void EstablecerInformacionInicial()
        {
            try
            {
                #region Inicializar Valores
                this.vista.NombreEmpresa = null;
                this.vista.DomicilioEmpresa = null;
                this.vista.RepresentanteEmpresa = null;
                #endregion

                if (this.vista.UnidadOperativaID == null)
                    throw new Exception("No se encontró el identificador de la unidad operativa sobre la que trabaja.");

                #region Unidad Operativa
                //Obtener información de la Unidad Operativa
                List<UnidadOperativaBO> lstUO = new List<UnidadOperativaBO>();
                if (vista.UnidadOperativa.Empresa == null) {
                    lstUO = FacadeBR.ConsultarUnidadOperativaCompleto(this.dctx, new UnidadOperativaBO() { Id = this.vista.UnidadOperativaID });
                    if (lstUO.Count <= 0)
                        throw new Exception("No se encontró la información completa de la unidad operativa sobre la que trabaja.");
                    else
                        if (lstUO[0].Empresa != null)
                            this.vista.NombreEmpresa = lstUO[0].Empresa.Nombre;
                } else
                    this.vista.NombreEmpresa = vista.UnidadOperativa.Empresa.Nombre;

                #endregion
                #region Dirección de la Empresa
                SucursalBO sucMatriz = FacadeBR.ObtenerSucursalMatriz(dctx, new UnidadOperativaBO() { Id = this.vista.UnidadOperativaID });                
                this.vista.DomicilioEmpresa = sucMatriz.DireccionesSucursal[0].Calle;
                #endregion

                #region Configuraciones de Unidad Operativa
                //Obtener las configuraciones de la unidad operativa
                List<ConfiguracionUnidadOperativaBO> lstConfigUO = new ModuloBR().ConsultarConfiguracionUnidadOperativa(this.dctx, new ConfiguracionUnidadOperativaBO() { UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaID } }, this.vista.ModuloID);
                if (lstConfigUO.Count <= 0)
                    throw new Exception("No se encontraron las configuraciones del sistema para la unidad operativa en la que trabaja.");

                //Establecer las configuraciones de la unidad operativa
                this.vista.RepresentanteEmpresa = lstConfigUO[0].Representante;
                this.vista.PorcentajePagoPostFactura = lstConfigUO[0].PorcentajePagoPostFactura;
                this.vista.DiasPagoPostFactura = lstConfigUO[0].DiasPagoPostFactura;
                #endregion

                #region Monedas
                List<MonedaBO> lstMonedas = FacadeBR.ConsultarMoneda(this.dctx, new MonedaBO() { Activo = true });
                this.vista.EstablecerOpcionesMoneda(lstMonedas.ToDictionary(p => p.Codigo, p => p.Nombre));
                #endregion

                #region Frecuencias de Facturacion
                Dictionary<String, String> listaFrecuencias = new Dictionary<String, String>();
                listaFrecuencias.Add(((Int32)EFrecuencia.DIARIA).ToString(), EFrecuencia.DIARIA.ToString());
                listaFrecuencias.Add(((Int32)EFrecuencia.SEMANAL).ToString(), EFrecuencia.SEMANAL.ToString());
                listaFrecuencias.Add(((Int32)EFrecuencia.QUINCENAL).ToString(), EFrecuencia.QUINCENAL.ToString());
                listaFrecuencias.Add(((Int32)EFrecuencia.MENSUAL).ToString(), EFrecuencia.MENSUAL.ToString());
                this.vista.EstablecerOpcionesFrecuenciaFacturacion(listaFrecuencias);
                #endregion
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".EstablecerInformacionInicial: " + ex.Message);
            }
        }

        /// <summary>
        /// Verifica que la tarifa seleccionada, si hay, siga aplicando según los cambios en la sucursal, unidad operativa, cliente, unidad y moneda. En caso de que exista un cambio y ya no aplique la tarifa seleccionada, se limpia la información
        /// </summary>
        private void ValidarAsociacionTarifaSeleccionada()
        {
            try
            {
                bool difSucursal = (this.vista.SucursalID != this.vista.TarifaSucursalID);
                bool difMoneda = (this.vista.CodigoMoneda != this.vista.TarifaCodigoMoneda);
                bool difModelo = (this.vista.ModeloID != this.vista.TarifaModeloID);
                bool difUnidadOperativa = (this.vista.UnidadOperativaID != this.vista.TarifaUnidadOperativaID);
                bool difCuentaCliente = (this.vista.TipoTarifaID != null && this.vista.TipoTarifaID == (int)ETipoTarifa.ESPECIAL && this.vista.CuentaClienteID != this.vista.TarifaCuentaClienteID);

                if (difSucursal || difMoneda || difModelo || difUnidadOperativa || difCuentaCliente)
                    this.SeleccionarTarifa(null);
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".ValidarAsociacionTarifaSeleccionada: " + ex.Message);
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
                if (this.vista.HoraContrato == null)
                    throw new Exception("No se ha seleccionado la hora del contrato.");
                if (this.vista.FechaPromesaDevolucion == null)
                    throw new Exception("No se ha seleccionado la fecha de promesa de devolución.");
                if (this.vista.HoraPromesaDevolucion == null)
                    throw new Exception("No se ha seleccionado la hora de promesa de devolución.");
                if (this.vista.CuentaClienteID == null)
                    throw new Exception("No se ha seleccionado el cliente.");
                if (this.vista.UnidadOperativaID == null)
                    throw new Exception("No se ha seleccionado la unidad operativa.");

                //Se consulta si existen reservaciones conflicitvas
                ContratoRDBOF bof = new ContratoRDBOF();
                bof.Unidad = (Equipos.BO.UnidadBO)selecto;
                bof.Cliente = new CuentaClienteIdealeaseBO() { Id = this.vista.CuentaClienteID };
                bof.FechaContrato = this.CalcularFechaCompleta(this.vista.FechaContrato, this.vista.HoraContrato);
                bof.FechaPromesaDevolucion = this.CalcularFechaCompleta(this.vista.FechaPromesaDevolucion, this.vista.HoraPromesaDevolucion);
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
        /// Indica si la unidad seleccionada cuenta con una órden de servicio abierta, es decir, se encuentra en taller
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

            DateTime? fechaInicio = null;
            DateTime? fechaFinal = null;

            #region Se construye una fecha en base a las fechas y horas de contrato y devolución
            if (this.vista.FechaContrato != null)
            {
                if (this.vista.HoraContrato != null)
                    fechaInicio = new DateTime(this.vista.FechaContrato.Value.Year, this.vista.FechaContrato.Value.Month, this.vista.FechaContrato.Value.Day, this.vista.HoraContrato.Value.Hours, this.vista.HoraContrato.Value.Minutes, this.vista.HoraContrato.Value.Seconds);
                else
                    fechaInicio = this.vista.FechaContrato;
            }
            if (this.vista.FechaPromesaDevolucion != null)
            {
                if (this.vista.HoraPromesaDevolucion != null)
                    fechaFinal = new DateTime(this.vista.FechaPromesaDevolucion.Value.Year, this.vista.FechaPromesaDevolucion.Value.Month, this.vista.FechaPromesaDevolucion.Value.Day, this.vista.HoraPromesaDevolucion.Value.Hours, this.vista.HoraPromesaDevolucion.Value.Minutes, this.vista.HoraPromesaDevolucion.Value.Seconds);
                else
                    fechaFinal = this.vista.FechaPromesaDevolucion;
            }
            #endregion

            #region Cálculo de los días de renta
            if (fechaInicio != null && fechaFinal != null)
            {
                int? diasRenta = null;

                if (fechaInicio.Value.Date == fechaFinal.Value.Date)
                    diasRenta = 1;
                else
                {
                    TimeSpan? tiempoRenta = fechaFinal.Value.Subtract(fechaInicio.Value);
                    if (tiempoRenta != null)
                    {
                        if (tiempoRenta.Value.TotalDays > Math.Truncate(tiempoRenta.Value.TotalDays))
                            diasRenta = Convert.ToInt32(Math.Truncate(tiempoRenta.Value.TotalDays) + 1);
                        else
                            diasRenta = Convert.ToInt32(Math.Truncate(tiempoRenta.Value.TotalDays));
                    }
                }
                this.vista.DiasRenta = diasRenta;
            }
            else
                this.vista.DiasRenta = null;
            #endregion

            this.CalcularOpcionesHabilitadas();
            this.CalcularFrecuenciasPermitidas(this.vista.DiasRenta);
        }
        /// <summary>
        /// Establece la selección de moneda y realiza los cálculos correspondientes a la selección
        /// </summary>
        /// <param name="codigoMoneda">Código de moneda seleccionada</param>
        public void SeleccionarMoneda(string codigoMoneda)
        {
            this.vista.CodigoMoneda = codigoMoneda;

            //Se evalua la tarifa seleccionada, si hay, para ver que con la selección de moneda siga correspondiendo la tarifa
            this.ValidarAsociacionTarifaSeleccionada();

            this.CalcularOpcionesHabilitadas();
        }
        public void SeleccionarSucursal(SucursalBO sucursal)
        {
            #region Dato a Interfaz de Usuario
            if (sucursal != null && sucursal.Nombre != null)
                this.vista.SucursalNombre = sucursal.Nombre;
            else
                this.vista.SucursalNombre = null;

            if (sucursal != null && sucursal.Id != null)
                this.vista.SucursalID = sucursal.Id;
            else
                this.vista.SucursalID = null;
            #endregion

            #region Consultar Completo para obtener la Dirección
            if (sucursal != null && sucursal.Id != null)
            {
                List<SucursalBO> lst = FacadeBR.ConsultarSucursalCompleto(this.dctx, sucursal);

                DireccionSucursalBO direccion = null;
                if (lst.Count > 0 && lst[0].DireccionesSucursal != null)
                    direccion = lst[0].DireccionesSucursal.Find(p => p.Primaria != null && p.Primaria == true);

                if (direccion != null)
                {
                    string dir = "";
                    if (!string.IsNullOrEmpty(direccion.Calle))
                        dir += (direccion.Calle + " ");
                    if (!string.IsNullOrEmpty(direccion.Colonia))
                        dir += (direccion.Colonia + " ");
                    if (!string.IsNullOrEmpty(direccion.CodigoPostal))
                        dir += (direccion.CodigoPostal + " ");
                    if (direccion.Ubicacion != null)
                    {
                        if (direccion.Ubicacion.Municipio != null && !string.IsNullOrEmpty(direccion.Ubicacion.Municipio.Nombre))
                            dir += (direccion.Ubicacion.Municipio.Nombre + " ");
                        if (direccion.Ubicacion.Ciudad != null && !string.IsNullOrEmpty(direccion.Ubicacion.Ciudad.Nombre))
                            dir += (direccion.Ubicacion.Ciudad.Nombre + " ");
                        if (direccion.Ubicacion.Estado != null && !string.IsNullOrEmpty(direccion.Ubicacion.Estado.Nombre))
                            dir += (direccion.Ubicacion.Estado.Nombre + " ");
                        if (direccion.Ubicacion.Pais != null && !string.IsNullOrEmpty(direccion.Ubicacion.Pais.Nombre))
                            dir += (direccion.Ubicacion.Pais.Nombre + " ");
                    }

                    if (dir != null && dir.Trim().CompareTo("") != 0)
                        this.vista.DireccionSucursal = dir;
                    else
                        this.vista.DireccionSucursal = null;
                }
                else
                    this.vista.DireccionSucursal = null;
            }
            else
                this.vista.DireccionSucursal = null;
            #endregion

            //Se evalua la tarifa seleccionada, si hay, para ver que con la selección de sucursal siga correspondiendo la tarifa
            this.ValidarAsociacionTarifaSeleccionada();

            this.CalcularOpcionesHabilitadas();
        }
        public void CargarSucursalesAutorizadas(List<SucursalBO> lstSucursales) {
            try {
                if (lstSucursales != null)
                    this.vista.SucursalesAutorizadas = lstSucursales;
                else
                    throw new Exception("Inconsistencias al cargar las sucursales autorizadas!.");
            } catch (Exception ex) {
                throw new Exception(this.nombreClase + ".CargarSucursalesAutorizadas: " + ex.Message);
            }
        }
        public void SeleccionarCuentaCliente(CuentaClienteIdealeaseBO cuentaCliente)
        {
            try
            {
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

                //Se limpia el operador
                this.vista.OperadorAniosExperiencia = null;
                this.vista.OperadorDireccionCalle = null;
                this.vista.OperadorDireccionCiudad = null;
                this.vista.OperadorDireccionCP = null;
                this.vista.OperadorDireccionEstado = null;
                this.vista.OperadorFechaNacimiento = null;
                this.vista.OperadorLicenciaEstado = null;
                this.vista.OperadorLicenciaFechaExpiracion = null;
                this.vista.OperadorLicenciaNumero = null;
                this.vista.OperadorLicenciaTipoID = null;
                this.vista.OperadorNombre = null;

                this.SeleccionarTarifa(null);
                #endregion


				#region Se obtiene al cliente completo
				if (this.vista.UnidadOperativaID == null)
					throw new Exception("No se encontró la unidad operativa sobre la que trabaja.");
				#endregion

                CuentaClienteIdealeaseBR cuentaClienteBr = new CuentaClienteIdealeaseBR();                
                List<RepresentanteLegalBO> lstRepresentantes = cuentaClienteBr.ConsultarRepresentantesLegales(dctx, new RepresentanteLegalBO(), cuentaCliente);

                #region Obtener los representantes para ir agregando (Si es cliente moral)
                if (this.vista.UnidadOperativaID == null)
                    throw new Exception("No se encontró la unidad operativa sobre la que trabaja.");
				if (cuentaCliente != null && cuentaCliente.Id != null && cuentaCliente.Cliente.Fisica != null && cuentaCliente.Cliente.Fisica == false){


                    List<RepresentanteLegalBO> lstRepActivos = new List<PersonaBO>(lstRepresentantes.Where(persona => persona.Activo == true))
                                                                    .ConvertAll(s => (RepresentanteLegalBO)s);

                        foreach (var representantesActivo in lstRepActivos.Where(representantesActivo => representantesActivo.EsDepositario == true)){
                            representantesActivo.Nombre = "(D) " + representantesActivo.Nombre;
                        }

                        this.vista.RepresentantesTotales = lstRepActivos;
                        this.vista.RepresentantesSeleccionados = null;
                 
                }
                else
                {
                    this.vista.RepresentantesTotales = null;
                    this.vista.RepresentantesSeleccionados = null;
                }

                this.vista.ActualizarRepresentantesLegales();
                #endregion

                //Cuando se haga la SC de Operadores, aquí se cargarán los operadores del cliente

                //Se evalua la tarifa seleccionada, si hay, para ver que con la selección de cliente siga correspondiendo la tarifa
                this.ValidarAsociacionTarifaSeleccionada();

				#region Asignar Representantes, Obligados Solidarios y Avales
                if (cuentaCliente != null) {
					#region Representantes Legales
                    if (cuentaCliente.Cliente.Fisica != null && cuentaCliente.Cliente.Fisica == false){
                        var lstRepActivos = new List<PersonaBO>(lstRepresentantes.Where(persona => persona.Activo == true)).ConvertAll(s => (RepresentanteLegalBO)s);

						this.vista.RepresentantesTotales = lstRepActivos;
						this.vista.RepresentantesSeleccionados = null;
					}
					#endregion

					#region Obligados Solidarios y Avales

                    List<ObligadoSolidarioBO> lstObligados = cuentaClienteBr.ConsultarObligadosSolidarios(dctx,  new ObligadoSolidarioProxyBO { Activo = true }, cuentaCliente);

                    List<ObligadoSolidarioBO> obligadosActivos = new List<PersonaBO>(lstObligados.Where(persona => persona.Activo == true)).ConvertAll(s => (ObligadoSolidarioBO)s);

					List<AvalBO> lstAvales = null;
					if (obligadosActivos != null)
						lstAvales = obligadosActivos.ConvertAll(s => this.ObligadoAAval(s));
					this.vista.AvalesTotales = lstAvales;
					this.vista.AvalesSeleccionados = null;
					#endregion
				}

				this.vista.ActualizarRepresentantesLegales();
				#endregion

                this.CalcularOpcionesHabilitadas();
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".SeleccionarCuentaCliente: " + ex.Message);
            }
        }
        public void SeleccionarUnidad(Equipos.BO.UnidadBO unidad)
        {
            try
            {
                #region Se obtiene la información completa de la unidad y sus trámites
                List<TramiteBO> lstTramites = new List<TramiteBO>();

                if (unidad != null && (unidad.UnidadID != null || unidad.EquipoID != null)) {
                    List<Equipos.BO.UnidadBO> lst = new UnidadBR().ConsultarCompleto(this.dctx, new Equipos.BO.UnidadBO() { UnidadID = unidad.UnidadID, EquipoID = unidad.EquipoID }, true);
                    if (lst.Count <= 0)
                        throw new Exception("No se encontró la información completa de la unidad seleccionada.");

                    unidad = lst[0];

                    lstTramites = new TramiteBR().ConsultarCompleto(this.dctx, new TramiteProxyBO() { Activo = true, Tramitable = unidad }, false);
                }
                #endregion

                #region Dato a Interfaz de Usuario
                //Información de la Unidad
                if (unidad == null) unidad = new Equipos.BO.UnidadBO();
                if (unidad.Modelo == null) unidad.Modelo = new ModeloBO();
                if (unidad.Modelo.Marca == null) unidad.Modelo.Marca = new MarcaBO();
                if (unidad.Sucursal == null) unidad.Sucursal = new SucursalBO();
                if (unidad.CaracteristicasUnidad == null) unidad.CaracteristicasUnidad = new CaracteristicasUnidadBO();

                this.vista.UnidadID = unidad.UnidadID;
                this.vista.EquipoID = unidad.EquipoID;
                this.vista.ModeloID = unidad.Modelo.Id;
                this.vista.UnidadPBC = unidad.CaracteristicasUnidad.PBCMaximoRecomendado;
                this.vista.UnidadRendimientoTanque = unidad.CaracteristicasUnidad.RendimientoTanque;
                this.vista.UnidadCapacidadTanque = unidad.CaracteristicasUnidad.CapacidadTanque;
                this.vista.UnidadAnio = unidad.Anio;

                if (unidad.NumeroEconomico != null && unidad.NumeroEconomico.Trim().CompareTo("") != 0)
                    this.vista.NumeroEconomico = unidad.NumeroEconomico;
                else
                    this.vista.NumeroEconomico = null;

                if (unidad.NumeroSerie != null && unidad.NumeroSerie.Trim().CompareTo("") != 0)
                    this.vista.VIN = unidad.NumeroSerie;
                else
                    this.vista.VIN = null;

                if (unidad.Modelo.Nombre != null && unidad.Modelo.Nombre.Trim().CompareTo("") != 0)
                    this.vista.ModeloNombre = unidad.Modelo.Nombre;
                else
                    this.vista.ModeloNombre = null;

                if (unidad.Sucursal.Nombre != null && unidad.Sucursal.Nombre.Trim().CompareTo("") != 0)
                    this.vista.UnidadSucursalNombre = unidad.Sucursal.Nombre;
                else
                    this.vista.UnidadSucursalNombre = null;

                if (!string.IsNullOrEmpty(unidad.Modelo.Marca.Nombre) && !string.IsNullOrWhiteSpace(unidad.Modelo.Marca.Nombre))
                    this.vista.MarcaNombre = unidad.Modelo.Marca.Nombre.Trim().ToUpper();
                else
                    this.vista.MarcaNombre = null;

                //Información de los Equipos Aliados de la Unidad
                DataTable dt = new DataTable();
                dt.Columns.Add(new DataColumn("NumeroSerie"));
                dt.Columns.Add(new DataColumn("Anio"));
                dt.Columns.Add(new DataColumn("Dimensiones"));
                dt.Columns.Add(new DataColumn("PBV"));
                dt.Columns.Add(new DataColumn("PBC"));
                dt.Columns.Add(new DataColumn("Modelo"));
                if (unidad.EquiposAliados != null)
                {
                    foreach (EquipoAliadoBO ea in unidad.EquiposAliados)
                    {
                        DataRow dr = dt.NewRow();
                        dr["NumeroSerie"] = ea.NumeroSerie;
                        dr["Anio"] = ea.Anio;
                        dr["Dimensiones"] = ea.Dimension;
                        dr["PBV"] = ea.PBV;
                        dr["PBC"] = ea.PBC;

                        if (ea.Modelo != null)
                            dr["Modelo"] = ea.Modelo.Nombre;

                        dt.Rows.Add(dr);
                    }
                }
                dt.AcceptChanges();
                this.vista.EstablecerEquiposAliadoUnidad(dt);

                //Información de los Trámites de la Unidad y Deducible
                TramiteBO tramite = null;
                decimal? porcentajeDeducibleMayor = null;
                #region Placa Federal
                tramite = lstTramites.Find(p => p.Tipo != null && p.Tipo == ETipoTramite.PLACA_FEDERAL && p.Activo != null && p.Activo == true);
                if (tramite != null && tramite.Resultado != null && tramite.Resultado.Trim().CompareTo("") != 0)
                    this.vista.UnidadPlacaFederal = tramite.Resultado;
                else
                    this.vista.UnidadPlacaFederal = null;
                #endregion
                #region Placa Estatal
                tramite = lstTramites.Find(p => p.Tipo != null && p.Tipo == ETipoTramite.PLACA_ESTATAL && p.Activo != null && p.Activo == true);
                if (tramite != null && tramite.Resultado != null && tramite.Resultado.Trim().CompareTo("") != 0)
                    this.vista.UnidadPlacaEstatal = tramite.Resultado;
                else
                    this.vista.UnidadPlacaEstatal = null;
                #endregion
                #region Seguro
                tramite = lstTramites.Find(p => p.Tipo != null && p.Tipo == ETipoTramite.SEGURO && p.Activo != null && p.Activo == true);
                if (tramite != null && tramite.Resultado != null && tramite.Resultado.Trim().CompareTo("") != 0)
                    this.vista.UnidadNumeroPoliza = tramite.Resultado;
                else
                    this.vista.UnidadNumeroPoliza = null;
                if (tramite != null && tramite is SeguroBO && ((SeguroBO)tramite).Aseguradora != null && ((SeguroBO)tramite).Aseguradora.Trim().CompareTo("") != 0)
                    this.vista.UnidadAseguradora = ((SeguroBO)tramite).Aseguradora;
                else
                    this.vista.UnidadAseguradora = null;

                //Monto del Deducible
                if (tramite != null && tramite is SeguroBO)
                {
                    //SC0018
                    porcentajeDeducibleMayor = ((SeguroBO)tramite).ObtenerPorcentajeDeduciblePorConcepto(this.vista.ObtenerClaveDeducible());

                    if (string.IsNullOrEmpty(this.vista.ObtenerClaveDeducible()) || string.IsNullOrWhiteSpace(this.vista.ObtenerClaveDeducible()))
                        this.vista.MostrarMensaje("Aún no se configura el concepto de deducible de la unidad a usar en el contrato, es necesario configurarlo antes de continuar con el registro.", ETipoMensajeIU.ADVERTENCIA, null);

                    string clave = !string.IsNullOrEmpty(this.vista.ObtenerClaveDeducible()) &&
                                   !string.IsNullOrWhiteSpace(this.vista.ObtenerClaveDeducible())
                                       ? this.vista.ObtenerClaveDeducible()
                                       : "DAÑOS MATERIALES";

                    if (!porcentajeDeducibleMayor.HasValue)
                        this.vista.MostrarMensaje(string.Format("No se ha configurado un deducible para {0} de la unidad.", clave), ETipoMensajeIU.ADVERTENCIA, null);

                    this.vista.PorcentajeDeducible = porcentajeDeducibleMayor;
                }

                if (unidad != null && unidad.ActivoFijo != null && unidad.ActivoFijo.CostoSinIva != null && porcentajeDeducibleMayor != null)
                {
                    Decimal? montoDeducibleCalcuado = 0;
                    montoDeducibleCalcuado = unidad.ActivoFijo.CostoSinIva;
                    if (unidad.EquiposAliados.Count > 0)
                    {
                        montoDeducibleCalcuado = unidad.EquiposAliados.Aggregate(montoDeducibleCalcuado, (monto, equipoAliado) =>equipoAliado.ActivoFijo != null ? equipoAliado.ActivoFijo.CostoSinIva != null ? monto + equipoAliado.ActivoFijo.CostoSinIva : monto : monto);
                    }
                    vista.UnidadMontoDeducible = ((montoDeducibleCalcuado*porcentajeDeducibleMayor)/100);
                }
                else
                    this.vista.UnidadMontoDeducible = null;

                this.vista.UnidadMontoDeposito = this.vista.UnidadMontoDeducible;
                #endregion
                #endregion

                //Se evalua la tarifa seleccionada, si hay, para ver que con la selección de unidad siga correspondiendo la tarifa
                this.ValidarAsociacionTarifaSeleccionada();

                this.CalcularOpcionesHabilitadas();
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".SeleccionarUnidad: " + ex.Message);
            }
        }
		public void SeleccionarTarifa(TarifaRDBO tarifa)
        {
            #region Dato a Interfaz de Usuario
            if (tarifa == null) tarifa = new TarifaRDBO()
            {
                RangoTarifas = new List<RangoTarifaRDBO>()
            };

            this.vista.TarifaID = tarifa.TarifaID;
            if (tarifa.Tipo != null)
                this.vista.TipoTarifaID = (int)tarifa.Tipo;
            else
                this.vista.TipoTarifaID = null;
            this.vista.TarifaDescripcion = tarifa.Descripcion;

            this.vista.CapacidadCarga = tarifa.CapacidadCarga;
            this.vista.TarifaDiaria = tarifa.TarifaDiaria;
            this.vista.KmsLibres = tarifa.KmsLibres;
            this.vista.TarifaKmAdicional = tarifa.RangoTarifas != null && tarifa.RangoTarifas.Any() ? tarifa.RangoTarifas.First().CargoKm : 0;
            this.vista.HrsLibres = tarifa.HrsLibres;
            this.vista.TarifaHrAdicional = tarifa.RangoTarifas != null && tarifa.RangoTarifas.Any() ? tarifa.RangoTarifas.First().CargoHr : 0;

            if (tarifa.Sucursal != null)
                this.vista.TarifaSucursalID = tarifa.Sucursal.Id;
            else
                this.vista.TarifaSucursalID = null;
            if (tarifa.Cliente != null)
                this.vista.TarifaCuentaClienteID = tarifa.Cliente.Id;
            else
                this.vista.TarifaCuentaClienteID = null;
            if (tarifa.Modelo != null)
                this.vista.TarifaModeloID = tarifa.Modelo.Id;
            else
                this.vista.TarifaModeloID = null;
            if (tarifa.Divisa != null && tarifa.Divisa.MonedaDestino != null)
                this.vista.TarifaCodigoMoneda = tarifa.Divisa.MonedaDestino.Codigo;
            else
                this.vista.TarifaCodigoMoneda = null;
            if (tarifa.Sucursal != null && tarifa.Sucursal.UnidadOperativa != null)
                this.vista.TarifaUnidadOperativaID = tarifa.Sucursal.UnidadOperativa.Id;
            else
                this.vista.TarifaUnidadOperativaID = null;

            this.vista.CargoAlteracionMedidorKm = this.vista.TarifaDiaria;
            this.vista.CargoEntregaInpuntual = this.vista.TarifaDiaria;
            this.vista.CargoHoraPosecion = this.vista.TarifaDiaria;
            #endregion

            this.CalcularOpcionesHabilitadas();
        }
        #region SC_0013
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

            this.vista.OperadorID = operador.OperadorID;
            this.vista.OperadorNombre = operador.Nombre;
            this.vista.OperadorCuentaClienteID = operador.Cliente.Id;
            this.vista.OperadorAniosExperiencia = operador.AñosExperiencia;
            this.vista.OperadorFechaNacimiento = operador.FechaNacimiento;
            this.vista.OperadorLicenciaNumero = operador.Licencia.Numero;
            this.vista.OperadorLicenciaTipoID = (int?)operador.Licencia.Tipo;
            this.vista.OperadorLicenciaFechaExpiracion = operador.Licencia.FechaExpiracion;
            this.vista.OperadorLicenciaEstado = operador.Licencia.Estado;
            this.vista.OperadorDireccionCalle = operador.Direccion.Calle;
            this.vista.OperadorDireccionCiudad = operador.Direccion.Ubicacion.Ciudad.Nombre;
            this.vista.OperadorDireccionEstado = operador.Direccion.Ubicacion.Estado.Nombre;
            this.vista.OperadorDireccionCP = operador.Direccion.CodigoPostal;
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

			this.vista.PermitirAgregarAvales( !soloRepresentantes);
			this.vista.PermitirSeleccionarAvales( !soloRepresentantes);
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

        #region Métodos para la Personalización de Tarifas
        public void SolicitarAutorizacionTarifaPersonalizada()
        {
            string s;
            if ((s = this.ValidarCamposTarifaPersonalizada()) != null)
            {
                this.vista.MostrarMensaje(s, ETipoMensajeIU.ADVERTENCIA, null);
                vista.PermitirValidarCodigoAutorizacion(false);
                return;
            }

            try
            {
                this.vista.TarifaPersonalizadaCodigoAutorizacion = null;

                //Interfaz de usuario a dato
                TarifaContratoRDBO tarifa = new TarifaContratoRDBO()
                {
                    RangoTarifas = new List<RangoTarifaRDBO>(),
                    TarifasEquipoAliado = new List<TarifaContratoRDEquipoAliadoBO>()
                };
                var rangoTarifa = new RangoTarifaRDBO();

                tarifa.CapacidadCarga = this.vista.TarifaPersonalizadaCapacidadCarga;
                tarifa.HrsLibres = this.vista.TarifaPersonalizadaHrsLibres;
                tarifa.KmsLibres = this.vista.TarifaPersonalizadaKmsLibres;
                tarifa.TarifaDiaria = this.vista.TarifaPersonalizadaTarifaDiaria;

                rangoTarifa.KmRangoInicial = this.vista.TarifaPersonalizadaKmsLibres + 1;
                rangoTarifa.HrRangoInicial = this.vista.TarifaPersonalizadaHrsLibres + 1;
                rangoTarifa.CargoHr = this.vista.TarifaPersonalizadaTarifaHrAdicional;
                rangoTarifa.CargoKm = this.vista.TarifaPersonalizadaTarifaKmAdicional;
                tarifa.RangoTarifas.Add(rangoTarifa);

                this.vista.TarifaPersonalizadaCodigoAutorizacion = this.controlador.SolicitarAutorizacionTarifaPersonalizada(this.dctx, tarifa, this.vista.ModuloID, this.vista.UnidadOperativaID, this.vista.UsuarioID, this.vista.SucursalID, this.vista.CuentaClienteID);
				vista.PermitirValidarCodigoAutorizacion(true);
				vista.PermitirSolicitarCodigoAutorizacion(false);
            }
            catch (Exception ex)
            {
	           throw new Exception(this.nombreClase + ".SolicitarAutorizacionTarifaPersonalizada: " + ex.Message);
            }
        }
        private string ValidarCamposTarifaPersonalizada()
        {
            string s = string.Empty;

            if (this.vista.TarifaPersonalizadaTipoTarifaID == null)
                s += "Tipo de Tarifa, ";
            if (this.vista.TarifaPersonalizadaCapacidadCarga == null)
                s += "Capacidad de Carga, ";
            if (this.vista.TarifaPersonalizadaTarifaDiaria == null)
                s += "Tarifa Diaria, ";
            if (this.vista.TarifaPersonalizadaKmsLibres == null)
                s += "Kilómetros Libres, ";
            if (this.vista.TarifaPersonalizadaTarifaKmAdicional == null)
                s += "Tarifa por Kilómetro Adicional, ";
            if (this.vista.TarifaPersonalizadaHrsLibres == null)
                s += "Horas Libres, ";
            if (this.vista.TarifaPersonalizadaTarifaHrAdicional == null)
                s += "Tarifa por Hora Adicional, ";

            if (s.Trim().CompareTo("") != 0)
                return "Los siguientes campos no pueden estar vacíos: \n" + s.Substring(0, s.Length - 2);

            return null;
        }

        /// <summary>
        /// Valida la confirmación del código con el código enviado en la autorización
        /// </summary>
        /// <param name="confirmacionCodigo">Confirmación del código de autorización</param>
        /// <returns>True en caso de que sea correcto, False en caso contrario</returns>
        public bool ValidarCodigoAutorizacion(string confirmacionCodigo)
        {
            if (confirmacionCodigo.Trim().CompareTo(this.vista.TarifaPersonalizadaCodigoAutorizacion) == 0)
                return true;
            return false;
        }

		/// <summary>
		/// Limpia los campos del dialogo y 
		/// Copia los datos de tarifa de la interfaz de usuario
		/// </summary>
        public void PrepararDialogoTarifaPersonalizada()
        {
            
            vista.TarifaPersonalizadaCapacidadCarga = null;
            vista.TarifaPersonalizadaCodigoAutorizacion = null;
            vista.TarifaPersonalizadaHrsLibres = null;
            vista.TarifaPersonalizadaKmsLibres = null;
            vista.TarifaPersonalizadaTarifaDiaria = null;
            vista.TarifaPersonalizadaTarifaHrAdicional = null;
            vista.TarifaPersonalizadaTarifaKmAdicional = null;
            vista.TarifaPersonalizadaTipoTarifaID = (int) ETipoTarifa.PERSONALIZADA;
			vista.PermitirValidarCodigoAutorizacion(false);
			vista.PermitirSolicitarCodigoAutorizacion(true);

			vista.TarifaPersonalizadaCapacidadCarga = vista.CapacidadCarga;
			vista.TarifaPersonalizadaHrsLibres = vista.HrsLibres;
			vista.TarifaPersonalizadaKmsLibres = vista.KmsLibres;
			vista.TarifaPersonalizadaTarifaDiaria = vista.TarifaDiaria;
			vista.TarifaPersonalizadaTarifaHrAdicional = vista.TarifaHrAdicional;
			vista.TarifaPersonalizadaTarifaKmAdicional = vista.TarifaKmAdicional;
			vista.TarifaPersonalizadaCodigoAutorizacion = null;

        }

		/// <summary>
		/// Actualiza la tarifa seleccionada con los datos de la tarifa personalizada
		/// </summary>
		public void ActualizarTarifa()
		{
			TarifaRDBO tarifaRDBO = new TarifaRDBO()
			{
			    RangoTarifas = new List<RangoTarifaRDBO>()
			};
            RangoTarifaRDBO rangoTarifa = new RangoTarifaRDBO();

			tarifaRDBO.CapacidadCarga = vista.TarifaPersonalizadaCapacidadCarga;
			tarifaRDBO.HrsLibres = vista.TarifaPersonalizadaHrsLibres;
			tarifaRDBO.KmsLibres = vista.TarifaPersonalizadaKmsLibres;
			tarifaRDBO.TarifaDiaria = vista.TarifaPersonalizadaTarifaDiaria;
            tarifaRDBO.Tipo = (ETipoTarifa?)vista.TarifaPersonalizadaTipoTarifaID;

            rangoTarifa.CargoHr = vista.TarifaPersonalizadaTarifaHrAdicional;
            rangoTarifa.CargoKm = vista.TarifaPersonalizadaTarifaKmAdicional;
		    rangoTarifa.KmRangoInicial = vista.TarifaPersonalizadaKmsLibres + 1;
		    rangoTarifa.HrRangoInicial = vista.TarifaPersonalizadaHrsLibres + 1;
			tarifaRDBO.RangoTarifas.Add(rangoTarifa);

			tarifaRDBO.Sucursal = new SucursalBO { Id = vista.SucursalID };
			tarifaRDBO.Cliente = new CuentaClienteIdealeaseBO { Id = vista.CuentaClienteID };
			tarifaRDBO.Modelo = new ModeloBO { Id = vista.ModeloID };
			tarifaRDBO.Divisa = new DivisaBO { MonedaDestino = new MonedaBO { Codigo = vista.CodigoMoneda } };
			tarifaRDBO.Sucursal = new SucursalBO { UnidadOperativa = new UnidadOperativaBO { Id = vista.UnidadOperativaID } };
			SeleccionarTarifa(null); //TODO tal vez borrar
			SeleccionarTarifa(tarifaRDBO);


		}
		#endregion

        public string ValidarCamposBorrador()
        {
            string s = string.Empty;

            if (this.vista.UnidadOperativaID == null)
                s += "Unidad Operativa, ";
            if (this.vista.SucursalID == null)
                s += "Sucursal, ";
            if (string.IsNullOrEmpty(this.vista.CodigoMoneda))
                s += "Moneda, ";
            if (this.vista.CuentaClienteID == null)
                s += "Cuenta del Cliente, ";
            if (this.vista.ClienteEsFisica == null)
                s += "Tipo de Contribuyente del Cliente, ";
            if (string.IsNullOrWhiteSpace(this.vista.ClaveProductoServicio))
                s += "Producto o Servicio, ";

            if (this.vista.TarifaID != null)
            {
                if (this.vista.TipoTarifaID == null)
                    s += "Tipo de Tarifa, ";
                if (this.vista.CapacidadCarga == null)
                    s += "Capacidad de Carga, ";
                if (this.vista.TarifaDiaria == null)
                    s += "Tarifa Diaria, ";
                if (this.vista.KmsLibres == null)
                    s += "Kilómetros Libres, ";
                if (this.vista.TarifaKmAdicional == null)
                    s += "Tarifa por Kilómetro Adicional, ";
                if (this.vista.HrsLibres == null)
                    s += "Horas Libres, ";
                if (this.vista.TarifaHrAdicional == null)
                    s += "Tarifa por Hora Adicional, ";
            }

            if (s != null && s.Trim().CompareTo("") != 0)
                return "Los siguientes campos no pueden estar vacíos: \n" + s.Substring(0, s.Length - 2);

            if ((s = this.ValidarFechasContrato()) != null)
                return s;

            if ((s = this.ValidarFechaExpiracionLicenciaOperador()) != null)
                return s;

            if ((s = this.ValidarFechaNacimientoOperador()) != null)
                return s;
            return null;
        }
        public string ValidarCamposRegistro()
        {
            string s = string.Empty;

            #region Información General
            if (this.vista.UnidadOperativaID == null)
                s += "Unidad Operativa, ";
            if (this.vista.SucursalID == null)
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
            if (string.IsNullOrEmpty(this.vista.OperadorNombre))
                s += "Nombre del Operador, ";
            if (string.IsNullOrEmpty(this.vista.OperadorDireccionCalle))
                s += "Calle de la Dirección del Operador, ";
            if (string.IsNullOrEmpty(this.vista.OperadorDireccionCiudad))
                s += "Ciudad de la Dirección del Operador, ";
            if (string.IsNullOrEmpty(this.vista.OperadorDireccionEstado))
                s += "Estado de la Dirección del Operador, ";
            if (string.IsNullOrEmpty(this.vista.OperadorDireccionCP))
                s += "Código Postal de la Dirección del Operador, ";
            if (this.vista.OperadorLicenciaTipoID == null)
                s += "Tipo de Licencia del Operador, ";
            if (string.IsNullOrEmpty(this.vista.OperadorLicenciaNumero))
                s += "Número de Licencia del Operador, ";
            if (this.vista.OperadorLicenciaFechaExpiracion == null)
                s += "Fecha de Expiración de la Licencia del Operador, ";
            if (string.IsNullOrEmpty(this.vista.OperadorLicenciaEstado))
                s += "Estado de la Licencia del Operador, ";
            #endregion

            //Unidad a Rentar
            if (this.vista.UnidadID == null)
                s += "Unidad, ";
            if (string.IsNullOrWhiteSpace(this.vista.ClaveProductoServicio))
                s += "Producto o Servicio, ";

            #region Condiciones de la Renta
            if (string.IsNullOrEmpty(this.vista.MercanciaTransportar))
                s += "Mercancía a Transportar, ";
            if (string.IsNullOrEmpty(this.vista.CodigoMoneda))
                s += "Moneda, ";
            if (this.vista.FrecuenciaFacturacionID == null)
                s += "Frecuencia de Facturación, ";
            #endregion

            if (this.vista.BitacoraViajeConductor == null)
                s += "Bitacora de Viaje del Conductor, ";
            if (this.vista.FechaPromesaDevolucion == null)
                s += "Fecha de Promesa de Devolución, ";
            if (this.vista.LectorKilometrajeID == null)
                s += "Lector de Kilometraje, ";
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

            //Tarifa
            if (this.vista.TipoTarifaID == null)
                s += "Tipo de Tarifa, ";
            if (this.vista.CapacidadCarga == null)
                s += "Capacidad de Carga, ";
            if (this.vista.TarifaDiaria == null)
                s += "Tarifa Diaria, ";
            if (this.vista.KmsLibres == null)
                s += "Kilómetros Libres, ";
            if (this.vista.TarifaKmAdicional == null)
                s += "Tarifa por Kilómetro Adicional, ";
            if (this.vista.HrsLibres == null)
                s += "Horas Libres, ";
            if (this.vista.TarifaHrAdicional == null)
                s += "Tarifa por Hora Adicional, ";

            if (s != null && s.Trim().CompareTo("") != 0)
                return "Los siguientes campos no pueden estar vacíos: \n" + s.Substring(0, s.Length - 2);

            if (this.vista.ClienteEsFisica != null && this.vista.ClienteEsFisica == false && !(this.vista.RepresentantesSeleccionados != null && this.vista.RepresentantesSeleccionados.Count > 0))
                return "El cliente seleccionado es Moral y, por lo tanto, requiere representantes legales";

            if ((s = this.ValidarFechasContrato()) != null)
                return s;

            if ((s = this.ValidarFechaExpiracionLicenciaOperador()) != null)
                return s;

            if ((s = this.ValidarFechaNacimientoOperador()) != null)
                return s;
            if ((s = this.ValidarDeduciblesSeguro()) != null)
                return s;
            #region BEP1401.SC0026
            if((s = this.ValidarDiasFacturar()) != "")
                return s;
            #endregion
            return null;
        }
        private string ValidarFechasContrato()
        {
            //SC_0037
            if(this.vista.FechaContrato.HasValue && this.vista.FechaContrato.Value.Date < DateTime.Today)
                return "La fecha del contrato no puede ser menor a la fecha actual.";

            if(this.vista.FechaContrato != null && this.vista.FechaPromesaDevolucion != null)
            {
                if(this.vista.FechaContrato > this.vista.FechaPromesaDevolucion)
                    return "La fecha del contrato no puede ser mayor a la de devolución.";

                if(this.vista.FechaContrato == this.vista.FechaPromesaDevolucion && this.vista.HoraContrato != null && this.vista.HoraPromesaDevolucion != null)
                {
                    if(this.vista.HoraContrato > this.vista.HoraPromesaDevolucion)
                        return "La fecha del contrato no puede ser mayor a la de devolución.";
                }
            }

            return null;
        }
        /// <summary>
        /// Valida la fecha de nacimiento del operador
        /// </summary>
        /// <returns>Mensaje indicando la inconsistencia o nulo en caso de que no existan inconsistencias</returns>
        public string ValidarFechaNacimientoOperador()
        {
            if (this.vista.OperadorFechaNacimiento != null && this.vista.OperadorFechaNacimiento.Value.Date >= DateTime.Now.Date)
                return "La fecha de nacimiento no puede ser mayor o igual a la fecha actual.";

            return null;
        }
        /// <summary>
        /// Valida la fecha de expiración de la licencia del operador
        /// </summary>
        /// <returns>Mensaje indicando la inconsistencia o nulo en caso de que no existan inconsistencias</returns>
        public string ValidarFechaExpiracionLicenciaOperador()
        {
            if (this.vista.OperadorLicenciaFechaExpiracion != null)
            {
                if (this.vista.OperadorLicenciaFechaExpiracion.Value.Date <= DateTime.Today)
                    return "La licencia del operador se encuentra vencida; su fecha de expiración es menor a la fecha de hoy.";
                if (this.vista.FechaContrato != null && this.vista.OperadorLicenciaFechaExpiracion.Value.Date <= this.vista.FechaContrato)
                    return "La licencia del operador vence antes de la fecha del contrato.";
                if (this.vista.FechaContrato != null && this.vista.FechaPromesaDevolucion != null)
                {
                    if (this.vista.OperadorLicenciaFechaExpiracion.Value.Date >= this.vista.FechaContrato && this.vista.OperadorLicenciaFechaExpiracion.Value.Date <= this.vista.FechaPromesaDevolucion)
                        return "La licencia del operador se encuentra vencida; su fecha de expiración está entre la fecha del contrato y la fecha de promesa de devolución.";
                }
            }

            return null;
        }
        /// <summary>
        /// SC0018
        /// Valida si la unidad seleccionada tiene configurado un deducible para DAÑOS MATERIALES
        /// </summary>
        /// <returns></returns>
        public string ValidarDeduciblesSeguro()
        {
            string clave = !string.IsNullOrEmpty(this.vista.ObtenerClaveDeducible()) &&
                           !string.IsNullOrWhiteSpace(this.vista.ObtenerClaveDeducible())
                               ? this.vista.ObtenerClaveDeducible()
                               : "DAÑOS MATERIALES";

            if (this.vista.PorcentajeDeducible == null)
                return string.Format("No se ha configurado un deducible para {0} de la unidad.", clave);

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

        /// <summary>
        /// Metodo para calcular las Frecuencias en base a la cantidad de Dias de Renta
        /// </summary>
        private void CalcularFrecuenciasPermitidas(Int32? diasRenta) //BEP1401.SC0026
        {
            if(diasRenta != null)
            {
                Dictionary<String, String> listaFrecuencias = new Dictionary<String, String>();                          
                if(diasRenta.Value < 7)
                {
                    listaFrecuencias.Add(((Int32)EFrecuencia.DIARIA).ToString(), EFrecuencia.DIARIA.ToString());
                }
                if(diasRenta.Value >= 7 && diasRenta.Value < 15)
                {
                    listaFrecuencias.Add(((Int32)EFrecuencia.DIARIA).ToString(), EFrecuencia.DIARIA.ToString());
                    listaFrecuencias.Add(((Int32)EFrecuencia.SEMANAL).ToString(), EFrecuencia.SEMANAL.ToString());
                }
                if(diasRenta.Value >= 15 && diasRenta.Value < 30)
                {
                    listaFrecuencias.Add(((Int32)EFrecuencia.DIARIA).ToString(), EFrecuencia.DIARIA.ToString());
                    listaFrecuencias.Add(((Int32)EFrecuencia.SEMANAL).ToString(), EFrecuencia.SEMANAL.ToString());
                    listaFrecuencias.Add(((Int32)EFrecuencia.QUINCENAL).ToString(), EFrecuencia.QUINCENAL.ToString());
                }
                if(diasRenta.Value >= 30)
                {
                    listaFrecuencias.Add(((Int32)EFrecuencia.DIARIA).ToString(), EFrecuencia.DIARIA.ToString());
                    listaFrecuencias.Add(((Int32)EFrecuencia.SEMANAL).ToString(), EFrecuencia.SEMANAL.ToString());
                    listaFrecuencias.Add(((Int32)EFrecuencia.QUINCENAL).ToString(), EFrecuencia.QUINCENAL.ToString());
                    listaFrecuencias.Add(((Int32)EFrecuencia.MENSUAL).ToString(), EFrecuencia.MENSUAL.ToString());
                }
                this.vista.EstablecerOpcionesFrecuenciaFacturacion(listaFrecuencias);
            }
        }

        public void LimpiarSesion()
        {
            this.vista.LimpiarSesion();
        }

        #region Métodos para el Buscador

        public object PrepararBOBuscador(string catalogo)
        {
            object obj = null;

            switch (catalogo)
            {
                case "Sucursal":
                    SucursalBOF sucursal = new SucursalBOF();
                    sucursal.UnidadOperativa = new UnidadOperativaBO();
                    sucursal.Usuario = new UsuarioBO();

                    sucursal.Nombre = this.vista.SucursalNombre;
                    sucursal.UnidadOperativa.Id = this.vista.UnidadOperativaID;
                    sucursal.Activo = true;
                    sucursal.Usuario.Id = this.vista.UsuarioID;

                    obj = sucursal;
                    break;
                case "CuentaClienteIdealease":
                    var cliente = new CuentaClienteIdealeaseBOF { Cliente = new ClienteBO() };

                    cliente.Nombre = this.vista.CuentaClienteNombre;
                    cliente.UnidadOperativa = new UnidadOperativaBO();
                    cliente.UnidadOperativa.Id = this.vista.UnidadOperativaID;
                    cliente.Activo = true;

                    obj = cliente;
                    break;
                case "DireccionCliente":
                    var cuentaCliente = new CuentaClienteIdealeaseBO();

                    cuentaCliente.Cliente = new ClienteBO();
                    cuentaCliente.Cliente.Id = this.vista.ClienteID;
                    cuentaCliente.Id = this.vista.CuentaClienteID;
                    cuentaCliente.UnidadOperativa = new UnidadOperativaBO();
                    cuentaCliente.UnidadOperativa.Id = this.vista.UnidadOperativaID;

                    var direccionCuentaCliente = new DireccionCuentaClienteBOF { Cuenta = cuentaCliente, Direccion = new DireccionClienteBO{Facturable = true}};

                    obj = direccionCuentaCliente;
                    break;
                case "Unidad":
                    UnidadBOF unidad = new UnidadBOF();
                    unidad.Sucursal = new SucursalBO();

                    if (!string.IsNullOrEmpty(this.vista.NumeroEconomico))
                        unidad.NumeroEconomico = this.vista.NumeroEconomico;

                    unidad.EstatusActual = EEstatusUnidad.Disponible;
                    unidad.Area = EArea.RD;
                    unidad.Sucursal.Id = this.vista.SucursalID;

                    obj = unidad;
                    break;
                case "Tarifa":
                    TarifaRDBO tarifa = new TarifaRDBO();
                    tarifa.Sucursal = new SucursalBO();
                    tarifa.Sucursal.UnidadOperativa = new UnidadOperativaBO();
                    tarifa.Cliente = new CuentaClienteIdealeaseBO();
                    tarifa.Cliente.Cliente = new ClienteBO();
                    tarifa.Divisa = new DivisaBO();
                    tarifa.Divisa.MonedaDestino = new MonedaBO();
                    tarifa.Modelo = new ModeloBO();
                    tarifa.RangoTarifas = new List<RangoTarifaRDBO>();

                    tarifa.Activo = true;
                    tarifa.Sucursal.Id = this.vista.SucursalID;
                    tarifa.Sucursal.UnidadOperativa.Id = this.vista.UnidadOperativaID;
                    tarifa.Cliente.Id = this.vista.CuentaClienteID;
                    tarifa.Modelo.Id = this.vista.ModeloID;
                    tarifa.Divisa.MonedaDestino.Codigo = this.vista.CodigoMoneda;
                    tarifa.Vigencia = DateTime.Today;

                    if (this.vista.CuentaClienteTipoID != null)
                    {
                        ETipoCuenta tipo = (ETipoCuenta)Enum.Parse(typeof(ETipoCuenta), this.vista.CuentaClienteTipoID.ToString());
                        switch (tipo)
                        {
                            case ETipoCuenta.Local:
                                tarifa.Tipo = ETipoTarifa.LOCAL;
                                break;
                            case ETipoCuenta.Nacional:
                                tarifa.Tipo = ETipoTarifa.NACIONAL;
                                break;
                        }
                    }

                    obj = tarifa;
                    break;
                #region SC_0013
                case "Operador":
                    OperadorBO operador = new OperadorBO();
                    operador.Cliente = new CuentaClienteIdealeaseBO();

                    operador.Nombre = this.vista.OperadorNombre;
                    operador.Cliente.Id = this.vista.CuentaClienteID;
                    operador.Estatus = true;

                    obj = operador;
                    break;
                #endregion
                case "ProductoServicio":
                    ProductoServicioBO producto = new ProductoServicioBO() { Activo = true };

                    if (!string.IsNullOrEmpty(vista.ClaveProductoServicio)) {
                        int auxNum = 0;
                        if (Int32.TryParse(vista.ClaveProductoServicio, out auxNum))
                            producto.NombreCorto = vista.ClaveProductoServicio;
                        else
                            producto.Nombre = vista.ClaveProductoServicio;
                    }
                    obj = producto;
                    break;
            }

            return obj;
        }
        public void DesplegarResultadoBuscador(string catalogo, object selecto)
        {
            switch (catalogo)
            {
                case "Sucursal":
                    this.SeleccionarSucursal((SucursalBO)selecto);
                    break;
                case "CuentaClienteIdealease":
                    if (selecto != null) {
                        CuentaClienteIdealeaseBOF cliente = (CuentaClienteIdealeaseBOF)selecto;
                        this.SeleccionarCuentaCliente(cliente);
                        if (vista.CuentaClienteID !=null && vista.ClienteID !=null)
                           vista.DesplegarDireccionCliente();
                    } else {
                        vista.ClienteID = null;
                        vista.CuentaClienteID = null;
                    }
                    break;
                case "DireccionCliente":
                    DireccionCuentaClienteBOF bof = (DireccionCuentaClienteBOF)selecto ?? new DireccionCuentaClienteBOF();
                    if (bof.Direccion == null)
                    {
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
                    this.SeleccionarUnidad((Equipos.BO.UnidadBO)selecto);

                    if (selecto != null && ((Equipos.BO.UnidadBO)selecto).UnidadID != null) {
                        if (this.UnidadTieneReservacion((Equipos.BO.UnidadBO)selecto))
                            this.vista.MostrarMensaje("La unidad ya se encuentra reservada para otro cliente", ETipoMensajeIU.ADVERTENCIA);

                        PermitirAgregarProductoServicio(true);
                    } else {
                        this.vista.ClaveProductoServicio = string.Empty;
                        this.vista.ProductoServicioId = null;
                        PermitirAgregarProductoServicio(false);
                    }
                    break;
                case "Tarifa":
                    this.SeleccionarTarifa((TarifaRDBO)selecto);
                    break;
                #region SC_0013
                case "Operador":
                    this.SeleccionarOperador((OperadorBO)selecto);
                    if (selecto != null && ((Comun.BO.OperadorBO)selecto).OperadorID != null)
                    {
                        if (ValidarFechaExpiracionLicenciaOperador() != null)
                        {
                            this.vista.MostrarMensaje("La licencia del operador se encuentra vencida", ETipoMensajeIU.ADVERTENCIA);
                            return;
                        }
                    }
                    break;
                #endregion
                case "ProductoServicio":
                    ProductoServicioBO producto = (ProductoServicioBO)selecto ?? new ProductoServicioBO();
                    vista.ProductoServicioId = producto.Id;
                    vista.ClaveProductoServicio = producto.NombreCorto;
                    vista.DescripcionProductoServicio = producto.Nombre;
                    break;
            }
        }

        #endregion

        public void PermitirAgregarProductoServicio(bool permitir) {
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
        }

        /// <summary>
        /// Valida la cantidad de Dias a Facturar
        /// </summary>
        /// <returns>Retorna VOID si no se encontro algun error en la cadena ingresada</returns>
        public String ValidarDiasFacturar() //BEP1401.SC0026
        {
            if(this.vista.DiasFacturar == null)
                return "El campo de Días Primera Factura está vacio o NO contiene un valor valido.";

            if(this.vista.DiasFacturar <= 0)
                return "La cantidad de Días Primera Factura no puede ser 0 ó menor. ";

            if(this.vista.DiasFacturar > this.vista.DiasRenta)
                return "La cantidad de Días Primera Factura no puede ser mayor a la cantidad de Días a Rentar. ";
            
            return "";
        }
        #endregion
	}
}
