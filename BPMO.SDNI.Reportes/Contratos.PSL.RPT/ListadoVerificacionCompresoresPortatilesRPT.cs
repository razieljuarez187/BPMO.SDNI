using System;
using System.Collections.Generic;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Contratos.BO;
using BPMO.SDNI.Contratos.PSL.BO;
using BPMO.SDNI.Equipos.BO;

namespace BPMO.SDNI.Contratos.PSL.RPT {
    public partial class ListadoVerificacionCompresoresPortatilesRPT : DevExpress.XtraReports.UI.XtraReport {

        /// <summary>
        /// Nombre de la clase que será usada en los mensajes de error
        /// </summary>
        private const string nombreClase = "CheckListRORPT";

        public ListadoVerificacionCompresoresPortatilesRPT(Dictionary<string, Object> datos) {
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

                var entrega = (ListadoVerificacionCompresorPortatilBO)datos["Entrega"] ?? new ListadoVerificacionCompresorPortatilBO();

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
                var recepcion = (ListadoVerificacionCompresorPortatilBO)datos["Recepcion"] ?? new ListadoVerificacionCompresorPortatilBO();

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

                if (entrega.TieneAceiteCompresor.HasValue) {
                    if (entrega.TieneAceiteCompresor.Value)
                        chbxAceiteCompresor.Checked = true;
                    else
                        chbxAceiteCompresor.Checked = false;
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

                if (entrega.TieneBarraTiro.HasValue) {
                    if (entrega.TieneBarraTiro.Value)
                        chbxBarraTiro.Checked = true;
                    else
                        chbxBarraTiro.Checked = false;
                }

                if (entrega.TieneBateria.HasValue) {
                    if (entrega.TieneBateria.Value)
                        chbxBateria.Checked = true;
                    else
                        chbxBateria.Checked = false;
                }

                if (entrega.TieneBtnServicioAire.HasValue) {
                    if (entrega.TieneBtnServicioAire.Value)
                        chbxBotonServicioAire.Checked = true;
                    else
                        chbxBotonServicioAire.Checked = false;
                }

                if (entrega.TieneCartuchoFiltro.HasValue) {
                    if (entrega.TieneCartuchoFiltro.Value)
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

                if (entrega.TieneIndicadores.HasValue) {
                    if (entrega.TieneIndicadores.Value)
                        chbxIndicadores.Checked = true;
                    else
                        chbxIndicadores.Checked = false;
                }

                if (entrega.TieneLamparasTablero.HasValue) {
                    if (entrega.TieneLamparasTablero.Value)
                        chbxLamparasTablero.Checked = true;
                    else
                        chbxLamparasTablero.Checked = false;
                }

                if (entrega.TieneLiquidoRefrigerante.HasValue) {
                    if (entrega.TieneLiquidoRefrigerante.Value)
                        chbxLiquidoRefrigerante.Checked = true;
                    else
                        chbxLiquidoRefrigerante.Checked = false;
                }

                if (entrega.TieneLucesTransito.HasValue) {
                    if (entrega.TieneLucesTransito.Value)
                        chbxLucesTransito.Checked = true;
                    else
                        chbxLucesTransito.Checked = false;
                }

                if (entrega.TieneManguerasYAbrazaderas.HasValue) {
                    if (entrega.TieneManguerasYAbrazaderas.Value)
                        chbxManguerasAbrazaderasAdmisionAire.Checked = true;
                    else
                        chbxManguerasAbrazaderasAdmisionAire.Checked = false;
                }

                if (entrega.TieneManometroPresion.HasValue) {
                    if (entrega.TieneManometroPresion.Value)
                        chbxManometroPresion.Checked = true;
                    else
                        chbxManometroPresion.Checked = false;
                }

                if (entrega.TienePresionEnLlantas.HasValue) {
                    if (entrega.TienePresionEnLlantas.Value)
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

                if (entrega.TieneSwitchArranque.HasValue) {
                    if (entrega.TieneSwitchArranque.Value)
                        chbxSwitchArranque.Checked = true;
                    else
                        chbxSwitchArranque.Checked = false;
                }


                if (entrega.TieneTacometro.HasValue) {
                    if (entrega.TieneTacometro.Value)
                        chbxTacometro.Checked = true;
                    else
                        chbxTacometro.Checked = false;
                }

                if (entrega.TieneTapaCombustible.HasValue) {
                    if (entrega.TieneTapaCombustible.Value)
                        chbxTapaCombustible.Checked = true;
                    else
                        chbxTapaCombustible.Checked = false;
                }

                if (entrega.TieneVelocidadMaxMotor.HasValue) {
                    if (entrega.TieneVelocidadMaxMotor.Value)
                        chbxVelocidadMaximaMotor.Checked = true;
                    else
                        chbxVelocidadMaximaMotor.Checked = false;
                }

                if (entrega.TieneVelocidadMinMotor.HasValue) {
                    if (entrega.TieneVelocidadMinMotor.Value)
                        chbxVelocidadMinimaMotor.Checked = true;
                    else
                        chbxVelocidadMinimaMotor.Checked = false;
                }

                this.xrlLubricacionEntrega.Text = !string.IsNullOrEmpty(entrega.Lubricacion) ? entrega.Lubricacion : string.Empty;
                #endregion
                #region Cuestionario Recepción

                if (recepcion.TieneAceiteMotor.HasValue) {
                    if (recepcion.TieneAceiteMotor.Value)
                        chbxAceiteMotorRecepcion.Checked = true;
                    else
                        chbxAceiteMotorRecepcion.Checked = false;
                }

                if (recepcion.TieneAceiteCompresor.HasValue) {
                    if (recepcion.TieneAceiteCompresor.Value)
                        chbxAceiteCompresorRecepcion.Checked = true;
                    else
                        chbxAceiteCompresorRecepcion.Checked = false;
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

                if (recepcion.TieneBarraTiro.HasValue) {
                    if (recepcion.TieneBarraTiro.Value)
                        chbxBarraTiroRecepcion.Checked = true;
                    else
                        chbxBarraTiroRecepcion.Checked = false;
                }

                if (recepcion.TieneBateria.HasValue) {
                    if (recepcion.TieneBateria.Value)
                        chbxBateriaRecepcion.Checked = true;
                    else
                        chbxBateriaRecepcion.Checked = false;
                }

                if (recepcion.TieneBtnServicioAire.HasValue) {
                    if (recepcion.TieneBtnServicioAire.Value)
                        chbxBotonServicioAireRecepcion.Checked = true;
                    else
                        chbxBotonServicioAireRecepcion.Checked = false;
                }

                if (recepcion.TieneCartuchoFiltro.HasValue) {
                    if (recepcion.TieneCartuchoFiltro.Value)
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

                if (recepcion.TieneIndicadores.HasValue) {
                    if (recepcion.TieneIndicadores.Value)
                        chbxIndicadoresRecepcion.Checked = true;
                    else
                        chbxIndicadoresRecepcion.Checked = false;
                }

                if (recepcion.TieneLamparasTablero.HasValue) {
                    if (recepcion.TieneLamparasTablero.Value)
                        chbxLamparasTableroRecepcion.Checked = true;
                    else
                        chbxLamparasTableroRecepcion.Checked = false;
                }

                if (recepcion.TieneLiquidoRefrigerante.HasValue) {
                    if (recepcion.TieneLiquidoRefrigerante.Value)
                        chbxLiquidoRefrigeranteRecepcion.Checked = true;
                    else
                        chbxLiquidoRefrigeranteRecepcion.Checked = false;
                }

                if (recepcion.TieneLucesTransito.HasValue) {
                    if (recepcion.TieneLucesTransito.Value)
                        chbxLucesTransitoRecepcion.Checked = true;
                    else
                        chbxLucesTransitoRecepcion.Checked = false;
                }

                if (recepcion.TieneManguerasYAbrazaderas.HasValue) {
                    if (recepcion.TieneManguerasYAbrazaderas.Value)
                        chbxManguerasAbrazaderasAdmisionAireRecepcion.Checked = true;
                    else
                        chbxManguerasAbrazaderasAdmisionAireRecepcion.Checked = false;
                }

                if (recepcion.TieneManometroPresion.HasValue) {
                    if (recepcion.TieneManometroPresion.Value)
                        chbxManometroPresionRecepcion.Checked = true;
                    else
                        chbxManometroPresionRecepcion.Checked = false;
                }

                if (recepcion.TienePresionEnLlantas.HasValue) {
                    if (recepcion.TienePresionEnLlantas.Value)
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

                if (recepcion.TieneSwitchArranque.HasValue) {
                    if (recepcion.TieneSwitchArranque.Value)
                        chbxSwitchArranqueRecepcion.Checked = true;
                    else
                        chbxSwitchArranqueRecepcion.Checked = false;
                }


                if (recepcion.TieneTacometro.HasValue) {
                    if (recepcion.TieneTacometro.Value)
                        chbxTacometroRecepcion.Checked = true;
                    else
                        chbxTacometroRecepcion.Checked = false;
                }

                if (recepcion.TieneTapaCombustible.HasValue) {
                    if (recepcion.TieneTapaCombustible.Value)
                        chbxTapaCombustibleRecepcion.Checked = true;
                    else
                        chbxTapaCombustibleRecepcion.Checked = false;
                }

                if (recepcion.TieneVelocidadMaxMotor.HasValue) {
                    if (recepcion.TieneVelocidadMaxMotor.Value)
                        chbxVelocidadMaximaMotorRecepcion.Checked = true;
                    else
                        chbxVelocidadMaximaMotorRecepcion.Checked = false;
                }

                if (recepcion.TieneVelocidadMinMotor.HasValue) {
                    if (recepcion.TieneVelocidadMinMotor.Value)
                        chbxVelocidadMinimaMotorRecepcion.Checked = true;
                    else
                        chbxVelocidadMinimaMotorRecepcion.Checked = false;
                }

                this.xrlLubricacionRecepcion.Text = !string.IsNullOrEmpty(recepcion.Lubricacion) ? recepcion.Lubricacion : string.Empty;

                #endregion

                #endregion

            } catch (Exception ex) {
                throw new Exception(".ImprimirReporte: Error al imprimir el reporte. " + ex.Message);
            }
        }


    }
}