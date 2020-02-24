using System;
using System.Collections.Generic;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Contratos.BO;
using BPMO.SDNI.Contratos.PSL.BO;
using BPMO.SDNI.Equipos.BO;

namespace BPMO.SDNI.Contratos.PSL.RPT {
    public partial class ListadoVerificacionMontaCargasRPT : DevExpress.XtraReports.UI.XtraReport {

        /// <summary>
        /// Nombre de la clase que será usada en los mensajes de error
        /// </summary>
        private const string nombreClase = "CheckListRORPT";

        public ListadoVerificacionMontaCargasRPT(Dictionary<string, Object> datos) {
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

                var entrega = (ListadoVerificacionMontaCargaBO)datos["Entrega"] ?? new ListadoVerificacionMontaCargaBO();

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
                var recepcion = (ListadoVerificacionMontaCargaBO)datos["Recepcion"] ?? new ListadoVerificacionMontaCargaBO();

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


                #region general
                if (entrega.TienePresionEnLlantas.HasValue) {
                    if (entrega.TienePresionEnLlantas.Value)
                        chbxPresionLlantas.Checked = true;
                    else
                        chbxPresionLlantas.Checked = false;
                }

                if (entrega.TieneBandaVentilador.HasValue) {
                    if (entrega.TieneBandaVentilador.Value)
                        chbxBandaVentilador.Checked = true;
                    else
                        chbxBandaVentilador.Checked = false;
                }

                if (entrega.TieneManguerasYAbrazaderas.HasValue) {
                    if (entrega.TieneManguerasYAbrazaderas.Value)
                        chbxMangueraAbrazaderaAdmisionAire.Checked = true;
                    else
                        chbxMangueraAbrazaderaAdmisionAire.Checked = false;
                }

                if (entrega.TieneCartuchoFiltro.HasValue) {
                    if (entrega.TieneCartuchoFiltro.Value)
                        chbxCartuchoFiltroAire.Checked = true;
                    else
                        chbxCartuchoFiltroAire.Checked = false;
                }

                if (entrega.TieneOperacionMastil.HasValue) {
                    if (entrega.TieneOperacionMastil.Value)
                        chbxOperacionMastil.Checked = true;
                    else
                        chbxOperacionMastil.Checked = false;
                }

                if (entrega.TieneCinturonSeguridad.HasValue) {
                    if (entrega.TieneCinturonSeguridad.Value)
                        chbxCinturonSeguridad.Checked = true;
                    else
                        chbxCinturonSeguridad.Checked = false;
                }

                if (entrega.TieneTensionCadena.HasValue) {
                    if (entrega.TieneTensionCadena.Value)
                        chbxTensionCadena.Checked = true;
                    else
                        chbxTensionCadena.Checked = false;
                }

                if (entrega.TieneEspejoRetrovisor.HasValue) {
                    if (entrega.TieneEspejoRetrovisor.Value)
                        chbxEspejoRetrovisor.Checked = true;
                    else
                        chbxEspejoRetrovisor.Checked = false;
                }





                #endregion


                #region Nivele de flujo

                if (entrega.TieneCombustible.HasValue) {
                    if (entrega.TieneCombustible.Value)
                        chbxCombustible.Checked = true;
                    else
                        chbxCombustible.Checked = false;
                }

                if (entrega.TieneAceiteMotor.HasValue) {
                    if (entrega.TieneAceiteMotor.Value)
                        chbxAceiteMotor.Checked = true;
                    else
                        chbxAceiteMotor.Checked = false;
                }






                if (entrega.TieneAceiteHidraulico.HasValue) {
                    if (entrega.TieneAceiteHidraulico.Value)
                        chbxAceiteHidraulico.Checked = true;
                    else
                        chbxAceiteHidraulico.Checked = false;
                }

                if (entrega.TieneLiquidoRefrigerante.HasValue) {
                    if (entrega.TieneLiquidoRefrigerante.Value)
                        chbxLiquidoRefrigerante.Checked = true;
                    else
                        chbxLiquidoRefrigerante.Checked = false;
                }


                if (entrega.TieneAceiteTransmision.HasValue) {
                    if (entrega.TieneAceiteTransmision.Value)
                        chbxAceiteTransmision.Checked = true;
                    else
                        chbxAceiteTransmision.Checked = false;
                }

                if (entrega.TieneAceiteEjeDelantero.HasValue) {
                    if (entrega.TieneAceiteEjeDelantero.Value)
                        chbxAceiteEjeDelantero.Checked = true;
                    else
                        chbxAceiteEjeDelantero.Checked = false;
                }

                if (entrega.TieneAceiteFrenos.HasValue) {
                    if (entrega.TieneAceiteFrenos.Value)
                        chbxAceiteFrenos.Checked = true;
                    else
                        chbxAceiteFrenos.Checked = false;
                }



                if (entrega.TieneBateria.HasValue) {
                    if (entrega.TieneBateria.Value)
                        chbxBateria.Checked = true;
                    else
                        chbxBateria.Checked = false;
                }












                #endregion

                #region Lubricacion

                if (entrega.TienePivoteArticulacion.HasValue) {
                    if (entrega.TienePivoteArticulacion.Value)
                        chbxPivotesArticulacionDireccion.Checked = true;
                    else
                        chbxPivotesArticulacionDireccion.Checked = false;
                }

                if (entrega.TieneCofreMotor.HasValue) {
                    if (entrega.TieneCofreMotor.Value)
                        chbxCofreMotor.Checked = true;
                    else
                        chbxCofreMotor.Checked = false;
                }

                #endregion


                #region Funciones electricas

                if (entrega.TieneLucesTrabajoDelantera.HasValue) {
                    if (entrega.TieneLucesTrabajoDelantera.Value)
                        chbxLucesTrabajoDelantera.Checked = true;
                    else
                        chbxLucesTrabajoDelantera.Checked = false;
                }

                if (entrega.TieneLucesTrabajoTrasera.HasValue) {
                    if (entrega.TieneLucesTrabajoTrasera.Value)
                        chbxLucesTrabajoTraseras.Checked = true;
                    else
                        chbxLucesTrabajoTraseras.Checked = false;
                }

                if (entrega.TieneLamparasTablero.HasValue) {
                    if (entrega.TieneLamparasTablero.Value)
                        chbxLamparasTablero.Checked = true;
                    else
                        chbxLamparasTablero.Checked = false;
                }


                if (entrega.TieneAlarmaReversa.HasValue) {
                    if (entrega.TieneAlarmaReversa.Value)
                        chbxAlarmaReversa.Checked = true;
                    else
                        chbxAlarmaReversa.Checked = false;
                }

                #endregion


                #region Controles


                if (entrega.TienePalancaAvanceReversa.HasValue) {
                    if (entrega.TienePalancaAvanceReversa.Value)
                        chbxPalancaAvanceReversa.Checked = true;
                    else
                        chbxPalancaAvanceReversa.Checked = false;
                }


                if (entrega.TieneLucesAdvertencia.HasValue) {
                    if (entrega.TieneLucesAdvertencia.Value)
                        chbxLucesAdvertencia.Checked = true;
                    else
                        chbxLucesAdvertencia.Checked = false;
                }


                if (entrega.TieneIndicadores.HasValue) {
                    if (entrega.TieneIndicadores.Value)
                        chbxIndicadores.Checked = true;
                    else
                        chbxIndicadores.Checked = false;
                }


                if (entrega.TieneFrenoEstacionamiento.HasValue) {
                    if (entrega.TieneFrenoEstacionamiento.Value)
                        chbxFrenoEstacionamiento.Checked = true;
                    else
                        chbxFrenoEstacionamiento.Checked = false;
                }


                if (entrega.TieneVelocidadMinMotor.HasValue) {
                    if (entrega.TieneVelocidadMinMotor.Value)
                        chbxVelocidadMinimaMotor.Checked = true;
                    else
                        chbxVelocidadMinimaMotor.Checked = false;
                }


                if (entrega.TieneVelocidadMaxMotor.HasValue) {
                    if (entrega.TieneVelocidadMaxMotor.Value)
                        chbxVelocidadMaximaMotor.Checked = true;
                    else
                        chbxVelocidadMaximaMotor.Checked = false;
                }



















                #endregion

                #region Miscelaneos

                if (entrega.TieneTapaCombustible.HasValue) {
                    if (entrega.TieneTapaCombustible.Value)
                        chbxTapaCombustible.Checked = true;
                    else
                        chbxTapaCombustible.Checked = false;
                }

                if (entrega.TieneTapaHidraulico.HasValue) {
                    if (entrega.TieneTapaHidraulico.Value)
                        chbxTapaHidraulico.Checked = true;
                    else
                        chbxTapaHidraulico.Checked = false;
                }


                if (entrega.TieneCondicionAsiento.HasValue) {
                    if (entrega.TieneCondicionAsiento.Value)
                        chbxCondicionAsiento.Checked = true;
                    else
                        chbxCondicionAsiento.Checked = false;
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

                if (entrega.TieneCondicionCalcas.HasValue) {
                    if (entrega.TieneCondicionCalcas.Value)
                        chbxCondicionCalcas.Checked = true;
                    else
                        chbxCondicionCalcas.Checked = false;
                }



                if (entrega.TieneSimbolosSeguridad.HasValue) {
                    if (entrega.TieneSimbolosSeguridad.Value)
                        chbxSimbolosSeguridadMaquina.Checked = true;
                    else
                        chbxSimbolosSeguridadMaquina.Checked = false;
                }



                if (entrega.TieneEstructuraChasis.HasValue) {
                    if (entrega.TieneEstructuraChasis.Value)
                        chbxEstructuraChasis.Checked = true;
                    else
                        chbxEstructuraChasis.Checked = false;
                }

                if (entrega.TieneAntenasMonitoreo.HasValue) {
                    if (entrega.TieneAntenasMonitoreo.Value)
                        chbxAntenasMonitoreoSatelital.Checked = true;
                    else
                        chbxAntenasMonitoreoSatelital.Checked = false;
                }



                #endregion






























                #endregion


                #region Cuestionario recepcion


                #region general
                if (recepcion.TienePresionEnLlantas.HasValue) {
                    if (recepcion.TienePresionEnLlantas.Value)
                        chbxPresionLlantasRecepcion.Checked = true;
                    else
                        chbxPresionLlantasRecepcion.Checked = false;
                }

                if (recepcion.TieneBandaVentilador.HasValue) {
                    if (recepcion.TieneBandaVentilador.Value)
                        chbxBandaVentiladorRecepcion.Checked = true;
                    else
                        chbxBandaVentiladorRecepcion.Checked = false;
                }

                if (recepcion.TieneManguerasYAbrazaderas.HasValue) {
                    if (recepcion.TieneManguerasYAbrazaderas.Value)
                        chbxManguerasAbrazaderasAdmisionAireRecepcion.Checked = true;
                    else
                        chbxManguerasAbrazaderasAdmisionAireRecepcion.Checked = false;
                }

                if (recepcion.TieneCartuchoFiltro.HasValue) {
                    if (recepcion.TieneCartuchoFiltro.Value)
                        chbxCartuchoFiltroAireRecepcion.Checked = true;
                    else
                        chbxCartuchoFiltroAireRecepcion.Checked = false;
                }

                if (recepcion.TieneOperacionMastil.HasValue) {
                    if (recepcion.TieneOperacionMastil.Value)
                        chbxOperacionMastilRecepcion.Checked = true;
                    else
                        chbxOperacionMastilRecepcion.Checked = false;
                }

                if (recepcion.TieneCinturonSeguridad.HasValue) {
                    if (recepcion.TieneCinturonSeguridad.Value)
                        chbxCinturonSeguridadRecepcion.Checked = true;
                    else
                        chbxCinturonSeguridadRecepcion.Checked = false;
                }

                if (recepcion.TieneTensionCadena.HasValue) {
                    if (recepcion.TieneTensionCadena.Value)
                        chbxTensionCadenaRecepcion.Checked = true;
                    else
                        chbxTensionCadenaRecepcion.Checked = false;
                }

                if (recepcion.TieneEspejoRetrovisor.HasValue) {
                    if (recepcion.TieneEspejoRetrovisor.Value)
                        chbxEspejoRetrovisorRecepcion.Checked = true;
                    else
                        chbxEspejoRetrovisorRecepcion.Checked = false;
                }

                #endregion


                #region Nivele de flujo

                if (recepcion.TieneCombustible.HasValue) {
                    if (recepcion.TieneCombustible.Value)
                        chbxCombustibleRecepcion.Checked = true;
                    else
                        chbxCombustibleRecepcion.Checked = false;
                }

                if (recepcion.TieneAceiteMotor.HasValue) {
                    if (recepcion.TieneAceiteMotor.Value)
                        chbxAceiteMotorRecepcion.Checked = true;
                    else
                        chbxAceiteMotorRecepcion.Checked = false;
                }






                if (recepcion.TieneAceiteHidraulico.HasValue) {
                    if (recepcion.TieneAceiteHidraulico.Value)
                        chbxAceiteHidraulicoRecepcion.Checked = true;
                    else
                        chbxAceiteHidraulicoRecepcion.Checked = false;
                }

                if (recepcion.TieneLiquidoRefrigerante.HasValue) {
                    if (recepcion.TieneLiquidoRefrigerante.Value)
                        chbxLiquidoRefrigeranteRecepcion.Checked = true;
                    else
                        chbxLiquidoRefrigeranteRecepcion.Checked = false;
                }


                if (recepcion.TieneAceiteTransmision.HasValue) {
                    if (recepcion.TieneAceiteTransmision.Value)
                        chbxAceiteTransmisionRecepcion.Checked = true;
                    else
                        chbxAceiteTransmisionRecepcion.Checked = false;
                }

                if (recepcion.TieneAceiteEjeDelantero.HasValue) {
                    if (recepcion.TieneAceiteEjeDelantero.Value)
                        chbxAceiteEjeDelanteroRecepcion.Checked = true;
                    else
                        chbxAceiteEjeDelanteroRecepcion.Checked = false;
                }

                if (recepcion.TieneAceiteFrenos.HasValue) {
                    if (recepcion.TieneAceiteFrenos.Value)
                        chbxAceiteFrenosRecepcion.Checked = true;
                    else
                        chbxAceiteFrenosRecepcion.Checked = false;
                }



                if (recepcion.TieneBateria.HasValue) {
                    if (recepcion.TieneBateria.Value)
                        chbxBateriaRecepcion.Checked = true;
                    else
                        chbxBateriaRecepcion.Checked = false;
                }


                #endregion

                #region Lubricacion

                if (recepcion.TienePivoteArticulacion.HasValue) {
                    if (recepcion.TienePivoteArticulacion.Value)
                        chbxPivotesArticulacionDireccionRecepcion.Checked = true;
                    else
                        chbxPivotesArticulacionDireccionRecepcion.Checked = false;
                }

                if (recepcion.TieneCofreMotor.HasValue) {
                    if (recepcion.TieneCofreMotor.Value)
                        chbxCofreMotorRecepcion.Checked = true;
                    else
                        chbxCofreMotorRecepcion.Checked = false;
                }

                #endregion


                #region Funciones electricas

                if (recepcion.TieneLucesTrabajoDelantera.HasValue) {
                    if (recepcion.TieneLucesTrabajoDelantera.Value)
                        chbxLucesTrabajoDelanteraRecepcion.Checked = true;
                    else
                        chbxLucesTrabajoDelanteraRecepcion.Checked = false;
                }

                if (recepcion.TieneLucesTrabajoTrasera.HasValue) {
                    if (recepcion.TieneLucesTrabajoTrasera.Value)
                        chbxLucesTrabajoTraserasRecepcion.Checked = true;
                    else
                        chbxLucesTrabajoTraserasRecepcion.Checked = false;
                }

                if (recepcion.TieneLamparasTablero.HasValue) {
                    if (recepcion.TieneLamparasTablero.Value)
                        chbxLamparasTableroRecepcion.Checked = true;
                    else
                        chbxLamparasTableroRecepcion.Checked = false;
                }


                if (recepcion.TieneAlarmaReversa.HasValue) {
                    if (recepcion.TieneAlarmaReversa.Value)
                        chbxAlarmaReversaRecepcion.Checked = true;
                    else
                        chbxAlarmaReversaRecepcion.Checked = false;
                }

                #endregion


                #region Controles


                if (recepcion.TienePalancaAvanceReversa.HasValue) {
                    if (recepcion.TienePalancaAvanceReversa.Value)
                        chbxPalancaAvanceReversaRecepcion.Checked = true;
                    else
                        chbxPalancaAvanceReversaRecepcion.Checked = false;
                }


                if (recepcion.TieneLucesAdvertencia.HasValue) {
                    if (recepcion.TieneLucesAdvertencia.Value)
                        chbxLucesAdvertenciaRecepcion.Checked = true;
                    else
                        chbxLucesAdvertenciaRecepcion.Checked = false;
                }


                if (recepcion.TieneIndicadores.HasValue) {
                    if (recepcion.TieneIndicadores.Value)
                        chbxIndicadoresRecepcion.Checked = true;
                    else
                        chbxIndicadoresRecepcion.Checked = false;
                }


                if (recepcion.TieneFrenoEstacionamiento.HasValue) {
                    if (recepcion.TieneFrenoEstacionamiento.Value)
                        chbxFrenoEstacionamientoRecepcion.Checked = true;
                    else
                        chbxFrenoEstacionamientoRecepcion.Checked = false;
                }


                if (recepcion.TieneVelocidadMinMotor.HasValue) {
                    if (recepcion.TieneVelocidadMinMotor.Value)
                        chbxVelocidadMinimaMotorRecepcion.Checked = true;
                    else
                        chbxVelocidadMinimaMotorRecepcion.Checked = false;
                }


                if (recepcion.TieneVelocidadMaxMotor.HasValue) {
                    if (recepcion.TieneVelocidadMaxMotor.Value)
                        chbxVelocidadMaximaMotorRecepcion.Checked = true;
                    else
                        chbxVelocidadMaximaMotorRecepcion.Checked = false;
                }



















                #endregion

                #region Miscelaneos

                if (recepcion.TieneTapaCombustible.HasValue) {
                    if (recepcion.TieneTapaCombustible.Value)
                        chbxTapaCombustibleRecepcion.Checked = true;
                    else
                        chbxTapaCombustibleRecepcion.Checked = false;
                }

                if (recepcion.TieneTapaHidraulico.HasValue) {
                    if (recepcion.TieneTapaHidraulico.Value)
                        chbxTapaHidraulicoRecepcion.Checked = true;
                    else
                        chbxTapaHidraulicoRecepcion.Checked = false;
                }


                if (recepcion.TieneCondicionAsiento.HasValue) {
                    if (recepcion.TieneCondicionAsiento.Value)
                        chbxCondicionAsientoRecepcion.Checked = true;
                    else
                        chbxCondicionAsientoRecepcion.Checked = false;
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

                if (recepcion.TieneCondicionCalcas.HasValue) {
                    if (recepcion.TieneCondicionCalcas.Value)
                        chbxCondicionCalcasRecepcion.Checked = true;
                    else
                        chbxCondicionCalcasRecepcion.Checked = false;
                }



                if (recepcion.TieneSimbolosSeguridad.HasValue) {
                    if (recepcion.TieneSimbolosSeguridad.Value)
                        chbxSimbolosSeguridadMaquinaRecepcion.Checked = true;
                    else
                        chbxSimbolosSeguridadMaquinaRecepcion.Checked = false;
                }



                if (recepcion.TieneEstructuraChasis.HasValue) {
                    if (recepcion.TieneEstructuraChasis.Value)
                        chbxEstructuraChasisRecepcion.Checked = true;
                    else
                        chbxEstructuraChasisRecepcion.Checked = false;
                }

                if (recepcion.TieneAntenasMonitoreo.HasValue) {
                    if (recepcion.TieneAntenasMonitoreo.Value)
                        chbxAntenasMonitoreoSatelitalRecepcion.Checked = true;
                    else
                        chbxAntenasMonitoreoSatelitalRecepcion.Checked = false;
                }



                #endregion

                #endregion
                #endregion
            } catch (Exception ex) {
                throw new Exception(".ImprimirReporte: Error al imprimir el reporte. " + ex.Message);
            }
        }


    }
}