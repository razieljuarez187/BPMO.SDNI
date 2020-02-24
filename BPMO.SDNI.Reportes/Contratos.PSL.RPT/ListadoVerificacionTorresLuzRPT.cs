using System;
using System.Collections.Generic;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Contratos.BO;
using BPMO.SDNI.Contratos.PSL.BO;
using BPMO.SDNI.Equipos.BO;

namespace BPMO.SDNI.Contratos.PSL.RPT {
    public partial class ListadoVerificacionTorresLuzRPT : DevExpress.XtraReports.UI.XtraReport {

        /// <summary>
        /// Nombre de la clase que será usada en los mensajes de error
        /// </summary>
        private const string nombreClase = "CheckListRORPT";

        public ListadoVerificacionTorresLuzRPT(Dictionary<string, Object> datos) {
            InitializeComponent();

            this.ImprimirReporte(datos);
        }

        /// <summary>
        /// Genera el reporte para el check list
        /// </summary>
        /// <param name="datos">Datos del check list</param>
        private void ImprimirReporte(Dictionary<string, object> datos) {
            try {
                #region Parte Generica
                #region Cabecera

                #region Logo
                var modulo = (ModuloBO)datos["Modulo"];
                int? unidadOperativaID = (int)datos["UnidadOperativaID"];
                if (unidadOperativaID != null && modulo.ObtenerConfiguracion(unidadOperativaID.Value) != null) {
                    ConfiguracionModuloBO configuracion = modulo.ObtenerConfiguracion(unidadOperativaID.Value);
                    if (!string.IsNullOrEmpty(configuracion.URLLogoEmpresa) && !string.IsNullOrWhiteSpace(configuracion.URLLogoEmpresa))
                        this.picLogoEmpresa.ImageUrl = configuracion.URLLogoEmpresa;
                }
                #endregion

                #region Folio
                string folio = (string)datos["Folio"];
                lblNumeroContrato.Text = folio;
                #endregion

                #region Contrato
                var contrato = (ContratoPSLBO)datos["Contrato"];
                if (contrato != null) {
                    if (contrato.Sucursal != null)
                        if (contrato.Sucursal.DireccionesSucursal != null && contrato.Sucursal.DireccionesSucursal.Count > 0)
                            xrlDireccionEmpresa.Text = string.Format("{0} {1} {2} {3} CP. {4}", contrato.Sucursal.DireccionesSucursal[0].Calle, contrato.Sucursal.DireccionesSucursal[0].Colonia, contrato.Sucursal.DireccionesSucursal[0].Ubicacion.Municipio.Codigo, contrato.Sucursal.DireccionesSucursal[0].Ubicacion.Estado.Codigo, contrato.Sucursal.DireccionesSucursal[0].CodigoPostal);
                    lblUbicacion.Text = !string.IsNullOrEmpty(contrato.DestinoAreaOperacion) ? contrato.DestinoAreaOperacion : string.Empty;
                }
                #endregion

                #region Fecha
                lblFechaCreacion.Text = DateTime.Now.ToShortDateString();
                #endregion

                #region Unidad

                var unidad = (UnidadBO)datos["Unidad"];
                this.lblNumeroSerie.Text = unidad.NumeroSerie.ToString();
                this.lblModelo.Text = unidad.Modelo.Nombre;
                this.lblAnio.Text = unidad.Anio.ToString();
                this.lblItemEcode.Text = unidad.NumeroEconomico.ToString();
                if (unidad.Modelo != null)
                    if (unidad.Modelo.Marca != null)
                        this.lblMarca.Text = unidad.Modelo.Marca.Nombre.ToString();

                var descripcion = string.Empty;
                if (unidad.TipoEquipoServicio != null)
                    descripcion = unidad.TipoEquipoServicio.Nombre + " ";
                if (unidad.Modelo != null)
                    descripcion += unidad.Modelo.Nombre;
                this.lblTipoUnidadModelo.Text = descripcion;


                #endregion

                #endregion

                #region Estatus contrato

                EEstatusContrato? estatus = null;
                if (datos.ContainsKey("EstatusContrato") && datos["EstatusContrato"] != null)
                    estatus = (EEstatusContrato)datos["EstatusContrato"];

                #endregion

                #region Cliente

                var cliente = (CuentaClienteIdealeaseBO)datos["Cliente"];
                this.lblClienteFacturacion.Text = cliente.Id.ToString();
                this.lblDireccion.Text = cliente.Direccion;
                if (cliente.Cliente != null) {
                    this.lblCorreoClienteFacturacion.Text = cliente.Correo;
                    this.lblEmailEmpresaEntrega.Text = cliente.Correo;
                    this.lblNombreCliente.Text = cliente.Nombre.ToUpper();

                    if (cliente.Telefonos != null && cliente.Telefonos.Count > 0) {
                        lblTelfonoClienteFacturacion.Text = !string.IsNullOrEmpty(cliente.Telefonos[0].Telefono) ? cliente.Telefonos[0].Telefono : string.Empty;
                        lblTelefonoEmpresaEntrega.Text = !string.IsNullOrEmpty(cliente.Telefonos[0].Telefono) ? cliente.Telefonos[0].Telefono : string.Empty;
                    }
                }

                #endregion

                #region Datos entrega

                var entrega = (ListadoVerificacionTorresLuzBO)datos["Entrega"] ?? new ListadoVerificacionTorresLuzBO();

                #endregion

                #region Linea
                var linea = (LineaContratoPSLBO)datos["Linea"];
                if (linea != null)
                    lblFolio.Text = !string.IsNullOrEmpty(linea.FolioCheckList) ? linea.FolioCheckList : string.Empty;
                #endregion

                #region Kilómetros y horas
                this.lblKilometrajeEntrega.Text = entrega.Horometro.HasValue
                                                      ? entrega.Horometro.Value.ToString("#,##0")
                                                      : string.Empty;

                #endregion

                #region Capacidad
                this.lblTanqueCombustible.Text = unidad == null || unidad.CombustibleConsumidoTotal == null ? "N/A" : unidad.CombustibleConsumidoTotal.ToString() + " L"; this.lblTanqueHidraulico.Text = "N/A";
                this.lblCarterMotor.Text = "N/A";
                this.lblDepositoRefrigerante.Text = "N/A";
                #endregion

                #region Cuestionario Entrega

                #region Observaciones
                this.lblObservaciones.Text = !string.IsNullOrEmpty(entrega.Observaciones) ? entrega.Observaciones : string.Empty;
                #endregion


                #endregion

                #region Combustible

                this.lblCombustibleSalida.Text = entrega.Combustible.HasValue ? Convert.ToInt32(entrega.Combustible.Value).ToString("#,##0") + " L" : "N/A";

                #endregion

                #region Datos Recepción
                var recepcion = (ListadoVerificacionTorresLuzBO)datos["Recepcion"] ?? new ListadoVerificacionTorresLuzBO();

                #region Fecha recepción
                this.lblFechaRecepcion.Text = recepcion.Fecha.HasValue ? recepcion.Fecha.Value.ToShortDateString() : string.Empty;
                #endregion

                #region Horómetro
                this.lblHorometroRetorno.Text = recepcion.Horometro.HasValue
                                                ? recepcion.Horometro.Value.ToString("#,##0")
                                                : string.Empty;
                #endregion



                #region Observaciones recepción
                this.xrObservacionesRecepcion.Text = !string.IsNullOrEmpty(recepcion.Observaciones) ? recepcion.Observaciones : string.Empty;
                #endregion

                #region Combustible
                this.lblCombustibleRetorno.Text = recepcion.Combustible.HasValue ? Convert.ToInt32(recepcion.Combustible.Value).ToString("#,##0") + " L" : string.Empty;

                #endregion
                #endregion

                #region Fecha y Hora salida
                this.lblFechaEntrega.Text = contrato.FechaInicioArrendamiento.HasValue ? contrato.FechaInicioArrendamiento.Value.ToShortDateString() : string.Empty;
                this.lblFechaRecepcion.Text = recepcion.Fecha.HasValue ? recepcion.Fecha.Value.ToShortDateString() : (contrato.FechaPromesaActual.HasValue ? contrato.FechaPromesaActual.Value.ToShortDateString() : string.Empty);
                #endregion
                #endregion
                #region Cuestionarios

                #region Cuestionario Entrega
                if (entrega.TieneAceiteMotor.HasValue) {
                    if (entrega.TieneAceiteMotor.Value)
                        chbxAceiteMotor.Checked = true;
                    else
                        chbxAceiteMotor.Checked = false;
                }

                if (entrega.TieneAntenasMonitoreo.HasValue) {
                    if (entrega.TieneAntenasMonitoreo.Value)
                        chbxAntenasMonitoreoSatelital.Checked = true;
                    else
                        chbxAntenasMonitoreoSatelital.Checked = false;
                }

                if (entrega.TieneBandaVentilador.HasValue) {
                    if (entrega.TieneBandaVentilador.Value)
                        chbxBandaVentilador.Checked = true;
                    else
                        chbxBandaVentilador.Checked = false;
                }

                if (entrega.TieneBateria.HasValue) {
                    if (entrega.TieneBateria.Value)
                        chbxBateria.Checked = true;
                    else
                        chbxBateria.Checked = false;
                }

                if (entrega.TieneCableLevanteTorre.HasValue) {
                    if (entrega.TieneCableLevanteTorre.Value)
                        chbxCableLevanteTorre.Checked = true;
                    else
                        chbxCableLevanteTorre.Checked = false;
                }

                if (entrega.TieneCartuchoFiltroAire.HasValue) {
                    if (entrega.TieneCartuchoFiltroAire.Value)
                        chbxCartuchoFiltroAire.Checked = true;
                    else
                        chbxCartuchoFiltroAire.Checked = false;
                }

                if (entrega.TieneCombustible.HasValue) {
                    if (entrega.TieneCombustible.Value)
                        chbxCombustible.Checked = true;
                    else
                        chbxCombustible.Checked = false;
                }

                if (entrega.TieneCondicionCalcas.HasValue) {
                    if (entrega.TieneCondicionCalcas.Value)
                        chbxCondicionCalcas.Checked = true;
                    else
                        chbxCondicionCalcas.Checked = false;
                }

                if (entrega.TieneCondicionLlantas.HasValue) {
                    if (entrega.TieneCondicionLlantas.Value)
                        chbxCondicionLlantas.Checked = true;
                    else
                        chbxCondicionLlantas.Checked = false;
                }

                if (entrega.TieneCondicionPintura.HasValue) {
                    if (entrega.TieneCondicionPintura.Value)
                        chbxCondicionPintura.Checked = true;
                    else
                        chbxCondicionPintura.Checked = false;
                }

                if (entrega.TieneEstructuraChasis.HasValue) {
                    if (entrega.TieneEstructuraChasis.Value)
                        chbxEstructuraChasis.Checked = true;
                    else
                        chbxEstructuraChasis.Checked = false;
                }

                if (entrega.TieneLamparasTablero.HasValue) {
                    if (entrega.TieneLamparasTablero.Value)
                        chbxLamparasTablero.Checked = true;
                    else
                        chbxLamparasTablero.Checked = false;
                }

                //if (entrega.Combustible.HasValue)
                //{
                //    if (entrega.Combustible.Value)
                //        chkCombustible.Checked = true;
                //    else
                //        chkCombustible.Checked = false;
                //}

                if (entrega.TieneLiquidoRefrigerante.HasValue) {
                    if (entrega.TieneLiquidoRefrigerante.Value)
                        chbxLiquidoRefrigerante.Checked = true;
                    else
                        chbxLiquidoRefrigerante.Checked = false;
                }

                if (entrega.TieneLucesAdvertencia.HasValue) {
                    if (entrega.TieneLucesAdvertencia.Value)
                        chbxLucesAdvertencia.Checked = true;
                    else
                        chbxLucesAdvertencia.Checked = false;
                }

                if (entrega.TieneLucesTorre.HasValue) {
                    if (entrega.TieneLucesTorre.Value)
                        chbxLucesTorre.Checked = true;
                    else
                        chbxLucesTorre.Checked = false;
                }

                if (entrega.TieneManguerasAbrazaderas.HasValue) {
                    if (entrega.TieneManguerasAbrazaderas.Value)
                        chbxManguerasAbrazaderasAdmisionAire.Checked = true;
                    else
                        chbxManguerasAbrazaderasAdmisionAire.Checked = false;
                }

                if (entrega.TienePresionLlantas.HasValue) {
                    if (entrega.TienePresionLlantas.Value)
                        chbxPresionLlantas.Checked = true;
                    else
                        chbxPresionLlantas.Checked = false;
                }

                if (entrega.TieneSimbolosSeguridad.HasValue) {
                    if (entrega.TieneSimbolosSeguridad.Value)
                        chbxSimbolosSeguridadMaquina.Checked = true;
                    else
                        chbxSimbolosSeguridadMaquina.Checked = false;
                }

                if (entrega.TieneSwitchEncendido.HasValue) {
                    if (entrega.TieneSwitchEncendido.Value)
                        chbxSwitchEncendido.Checked = true;
                    else
                        chbxSwitchEncendido.Checked = false;
                }

                if (entrega.TieneTapaCombustible.HasValue) {
                    if (entrega.TieneTapaCombustible.Value)
                        chbxTapaCombustible.Checked = true;
                    else
                        chbxTapaCombustible.Checked = false;
                }

                if (entrega.TieneVelocidadMaxima.HasValue) {
                    if (entrega.TieneVelocidadMaxima.Value)
                        chbxVelocidadMaximaMotor.Checked = true;
                    else
                        chbxVelocidadMaximaMotor.Checked = false;
                }

                if (entrega.TieneVelocidadMinima.HasValue) {
                    if (entrega.TieneVelocidadMinima.Value)
                        chbxVelocidadMinimaMotor.Checked = true;
                    else
                        chbxVelocidadMinimaMotor.Checked = false;
                }

                #endregion

                #region Cuestionario Recepción
                if (recepcion.TieneAceiteMotor.HasValue) {
                    if (recepcion.TieneAceiteMotor.Value)
                        chbxAceiteMotorRecepcion.Checked = true;
                    else
                        chbxAceiteMotorRecepcion.Checked = false;
                }

                if (recepcion.TieneAntenasMonitoreo.HasValue) {
                    if (recepcion.TieneAntenasMonitoreo.Value)
                        chbxAntenasMonitoreoSatelitalRecepcion.Checked = true;
                    else
                        chbxAntenasMonitoreoSatelitalRecepcion.Checked = false;
                }

                if (recepcion.TieneBandaVentilador.HasValue) {
                    if (recepcion.TieneBandaVentilador.Value)
                        chbxBandaVentiladorRecepcion.Checked = true;
                    else
                        chbxBandaVentiladorRecepcion.Checked = false;
                }

                if (recepcion.TieneBateria.HasValue) {
                    if (recepcion.TieneBateria.Value)
                        chbxBateriaRecepcion.Checked = true;
                    else
                        chbxBateriaRecepcion.Checked = false;
                }

                if (recepcion.TieneCableLevanteTorre.HasValue) {
                    if (recepcion.TieneCableLevanteTorre.Value)
                        chbxCableLevanteTorreRecepcion.Checked = true;
                    else
                        chbxCableLevanteTorreRecepcion.Checked = false;
                }

                if (recepcion.TieneCartuchoFiltroAire.HasValue) {
                    if (recepcion.TieneCartuchoFiltroAire.Value)
                        chbxCartuchoFiltroAireRecepcion.Checked = true;
                    else
                        chbxCartuchoFiltroAireRecepcion.Checked = false;
                }

                if (recepcion.TieneCombustible.HasValue) {
                    if (recepcion.TieneCombustible.Value)
                        chbxCombustibleRecepcion.Checked = true;
                    else
                        chbxCombustibleRecepcion.Checked = false;
                }

                if (recepcion.TieneCondicionCalcas.HasValue) {
                    if (recepcion.TieneCondicionCalcas.Value)
                        chbxCondicionCalcasRecepcion.Checked = true;
                    else
                        chbxCondicionCalcasRecepcion.Checked = false;
                }

                if (recepcion.TieneCondicionLlantas.HasValue) {
                    if (recepcion.TieneCondicionLlantas.Value)
                        chbxCondicionLlantasRecepcion.Checked = true;
                    else
                        chbxCondicionLlantasRecepcion.Checked = false;
                }

                if (recepcion.TieneCondicionPintura.HasValue) {
                    if (recepcion.TieneCondicionPintura.Value)
                        chbxCondicionPinturaRecepcion.Checked = true;
                    else
                        chbxCondicionPinturaRecepcion.Checked = false;
                }

                if (recepcion.TieneEstructuraChasis.HasValue) {
                    if (recepcion.TieneEstructuraChasis.Value)
                        chbxEstructuraChasisRecepcion.Checked = true;
                    else
                        chbxEstructuraChasisRecepcion.Checked = false;
                }

                if (recepcion.TieneLamparasTablero.HasValue) {
                    if (recepcion.TieneLamparasTablero.Value)
                        chbxLamparasTableroRecepcion.Checked = true;
                    else
                        chbxLamparasTableroRecepcion.Checked = false;
                }

                //if (recepcion.Combustible.HasValue)
                //{
                //    if (recepcion.Combustible.Value)
                //        chkCombustibleRecepcion.Checked = true;
                //    else
                //        chkCombustibleRecepcion.Checked = false;
                //}

                if (recepcion.TieneLiquidoRefrigerante.HasValue) {
                    if (recepcion.TieneLiquidoRefrigerante.Value)
                        chbxLiquidoRefrigeranteRecepcion.Checked = true;
                    else
                        chbxLiquidoRefrigeranteRecepcion.Checked = false;
                }

                if (recepcion.TieneLucesAdvertencia.HasValue) {
                    if (recepcion.TieneLucesAdvertencia.Value)
                        chbxLucesAdvertenciaRecepcion.Checked = true;
                    else
                        chbxLucesAdvertenciaRecepcion.Checked = false;
                }

                if (recepcion.TieneLucesTorre.HasValue) {
                    if (recepcion.TieneLucesTorre.Value)
                        chbxLucesTorreRecepcion.Checked = true;
                    else
                        chbxLucesTorreRecepcion.Checked = false;
                }

                if (recepcion.TieneManguerasAbrazaderas.HasValue) {
                    if (recepcion.TieneManguerasAbrazaderas.Value)
                        chbxManguerasAbrazaderasAdmisionAireRecepcion.Checked = true;
                    else
                        chbxManguerasAbrazaderasAdmisionAireRecepcion.Checked = false;
                }

                if (recepcion.TienePresionLlantas.HasValue) {
                    if (recepcion.TienePresionLlantas.Value)
                        chbxPresionLlantasRecepcion.Checked = true;
                    else
                        chbxPresionLlantasRecepcion.Checked = false;
                }

                if (recepcion.TieneSimbolosSeguridad.HasValue) {
                    if (recepcion.TieneSimbolosSeguridad.Value)
                        chbxSimbolosSeguridadMaquinaRecepcion.Checked = true;
                    else
                        chbxSimbolosSeguridadMaquinaRecepcion.Checked = false;
                }

                if (recepcion.TieneSwitchEncendido.HasValue) {
                    if (recepcion.TieneSwitchEncendido.Value)
                        chbxSwitchEncendidoRecepcion.Checked = true;
                    else
                        chbxSwitchEncendidoRecepcion.Checked = false;
                }

                if (recepcion.TieneTapaCombustible.HasValue) {
                    if (recepcion.TieneTapaCombustible.Value)
                        chbxTapaCombustibleRecepcion.Checked = true;
                    else
                        chbxTapaCombustibleRecepcion.Checked = false;
                }

                if (recepcion.TieneVelocidadMaxima.HasValue) {
                    if (recepcion.TieneVelocidadMaxima.Value)
                        chbxVelocidadMaximaMotorRecepcion.Checked = true;
                    else
                        chbxVelocidadMaximaMotorRecepcion.Checked = false;
                }

                if (recepcion.TieneVelocidadMinima.HasValue) {
                    if (recepcion.TieneVelocidadMinima.Value)
                        chbxVelocidadMinimaMotorRecepcion.Checked = true;
                    else
                        chbxVelocidadMinimaMotorRecepcion.Checked = false;
                }

                #endregion
                #endregion
            } catch (Exception ex) {
                throw new Exception(".ImprimirReporte: Error al imprimir el reporte. " + ex.Message);
            }
        }
    }
}