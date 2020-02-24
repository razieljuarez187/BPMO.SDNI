using System;
using System.Collections.Generic;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Contratos.BO;
using BPMO.SDNI.Contratos.PSL.BO;
using BPMO.SDNI.Equipos.BO;


namespace BPMO.SDNI.Contratos.PSL.RPT {
    public partial class ListadoVerificacionRetroExcavadoraRPT : DevExpress.XtraReports.UI.XtraReport {

        /// <summary>
        /// Nombre de la clase que será usada en los mensajes de error
        /// </summary>
        private const string nombreClase = "CheckListRORPT";

        public ListadoVerificacionRetroExcavadoraRPT(Dictionary<string, Object> datos) {
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

                #region Linea
                var linea = (LineaContratoPSLBO)datos["Linea"];
                if (linea != null)
                    lblFolio.Text = !string.IsNullOrEmpty(linea.FolioCheckList) ? linea.FolioCheckList : string.Empty;
                #endregion

                #region Datos entrega
                var entrega = (ListadoVerificacionRetroExcavadoraBO)datos["Entrega"] ?? new ListadoVerificacionRetroExcavadoraBO();
                #endregion

                #region Fecha y Horaentrega
                this.lblFechaEntrega.Text = contrato.FechaInicioArrendamiento.HasValue ? contrato.FechaInicioArrendamiento.Value.ToShortDateString() : string.Empty;
                #endregion

                #region Hórometro Entrega
                this.lblKilometrajeEntrega.Text = entrega.Horometro.HasValue
                                                      ? entrega.Horometro.Value.ToString("#,##0")
                                                      : string.Empty;
                #endregion

                #region Capacidad
                this.lblTanqueCombustible.Text = unidad == null || unidad.CombustibleConsumidoTotal == null ? "N/A" : unidad.CombustibleConsumidoTotal.ToString() + " L"; this.lblTanqueHidraulico.Text = "N/A";
                this.lblCarterMotor.Text = "N/A"; 
                this.lblDepositoRefrigerante.Text = "N/A"; 
                #endregion

                #region Observaciones
                this.lblObservacionesEntrega.Text = !string.IsNullOrEmpty(entrega.Observaciones) ? entrega.Observaciones : string.Empty;
                #endregion

                #region Combustible

                this.lblCombustibleSalida.Text = entrega.Combustible.HasValue ? Convert.ToInt32(entrega.Combustible.Value).ToString("#,##0") + " L" : "N/A";

                #endregion

                #region Datos Recepción
                var recepcion = (ListadoVerificacionRetroExcavadoraBO)datos["Recepcion"] ?? new ListadoVerificacionRetroExcavadoraBO();
                #endregion

                #region Fecha y Hora salida
                this.lblFechaRecepcion.Text = recepcion.Fecha.HasValue ? recepcion.Fecha.Value.ToShortDateString() : (contrato.FechaPromesaActual.HasValue ? contrato.FechaPromesaActual.Value.ToShortDateString() : string.Empty);
                #endregion

                #region Horómetro Recepción
                this.lblHorometroRetorno.Text = recepcion.Horometro.HasValue
                                                ? recepcion.Horometro.Value.ToString("#,##0")
                                                : string.Empty;
                #endregion

                #region Observaciones recepción
                this.lblObservacionesRecepcion.Text = !string.IsNullOrEmpty(recepcion.Observaciones) ? recepcion.Observaciones : string.Empty;
                #endregion

                #region Combustible
                this.lblCombustibleRetorno.Text = recepcion.Combustible.HasValue ? Convert.ToInt32(recepcion.Combustible.Value).ToString("#,##0") + " L" : string.Empty;
                #endregion

                #endregion

                #region Cuestionarios

                #region Cuestionario Entrega
                chbxPresionLlantas.Checked = entrega.TienePresionLLantas;
                chbxBandaVentilador.Checked = entrega.TieneBandaVentilador;
                chbxManguerasAbrazaderasAdmisionAire.Checked = entrega.TieneManguerasAbrazaderas;
                chbxCartuchoFiltroAire.Checked = entrega.TieneCartuchoFiltroAire;
                chbxBoteDelantero.Checked = entrega.TieneBoteDelantero;
                chbxBoteTrasero.Checked = entrega.TieneBoteTrasero;
                chbxZapatasEstabilizadores.Checked = entrega.TieneZapatasEstabilizadores;
                chbxCinturonSeguridad.Checked = entrega.TieneCinturonSeguridad;
                chbxEspejoRetrovisor.Checked = entrega.TieneEspejoRetrovisor;
                chbxCombustible.Checked = entrega.TieneCombustible;
                chbxAceiteMotor.Checked = entrega.TieneAceiteMotor;
                chbxLiquidoRefrigerante.Checked = entrega.TieneLiquidoRefrigerante;
                chbxAceiteHidraulico.Checked = entrega.TieneAceiteHidraulico;
                chbxAceiteTransmision.Checked = entrega.TieneAceiteTransmision;
                chbxAceiteDiferencialTrasero.Checked = entrega.TieneAceiteDiferencialTrasero;
                chbxAceiteDiferencialDel4x4.Checked = entrega.TieneAceiteDiferencialDelantero;
                chbxAceitePlanetariosDel4x4.Checked = entrega.TieneAceitePlanetariosDelantero;
                chbxBateria.Checked = entrega.TieneBateria;
                chbxBujesPasadoresCargador.Checked = entrega.TieneBujesPasadoresCargador;
                chbxBujesPasadoresRetro.Checked = entrega.TieneBujesPasadoresRetro;
                chbxAsientoOperador.Checked = entrega.TieneAsientoOperador;
                chbxAlarmaReversa.Checked = entrega.TieneAlarmaReversa;
                chbxAntenasMonitoreoSatelital.Checked = entrega.TieneAntenaMonitoreo;
                chbxBloqueoDiferencial.Checked = entrega.TieneBloqueoDiferencial;
                chbxClaxon.Checked = entrega.TieneClaxon;
                chbxCondicionAsiento.Checked = entrega.TieneCondicionAsiento;
                chbxCondicionCalcas.Checked = entrega.TieneCondicionCalcas;
                chbxCondicionLlantas.Checked = entrega.TieneCondicionLlantas;
                chbxCondicionPintura.Checked = entrega.TieneCondicionPintura;
                chbxDesacople4x4.Checked = entrega.TieneDesacople;
                chbxEstructuraChasis.Checked = entrega.TieneEstructuraChasis;
                chbxFrenoEstacionamiento.Checked = entrega.TieneFrenoEstacionamiento;
                chbxIndicadores.Checked = entrega.TieneIndicadores;
                chbxJoysticks.Checked = entrega.TieneJoystick;
                chbxLucesAdvertencia.Checked = entrega.TieneLucesAdvertencia;
                chbxLucesDireccionales.Checked = entrega.TieneLucesDireccionales;
                chbxLucesIntermitentes.Checked = entrega.TieneLucesIntermitentes;
                chbxLucesTrabajoDelantera.Checked = entrega.TieneLucesTrabajoDelantera;
                chbxLucesTrabajoTraseras.Checked = entrega.TieneLucesTrabajoTraseras;
                chbxNeutralizadorTransmision.Checked = entrega.TieneNeutralizadorTransmision;
                chbxPalancaTransito.Checked = entrega.TienePalancaTransito;
                chbxSimbolosSeguridadMaquina.Checked = entrega.TieneSimbolosSeguridad;
                chbxTacometro.Checked = entrega.TieneTacometro;
                chbxTapaCombustible.Checked = entrega.TieneTapaCombustible;
                chbxTapaHidraulico.Checked = entrega.TieneTapaHidraulico;
                chbxVelocidadMinimaMotor.Checked = entrega.TieneVelocidadMinima;
                chbxVelocidadMaximaMotor.Checked = entrega.TieneVelocidadMaxima;

                #endregion

                #region Cuestionario Recepción
                chbxAceiteDiferencialDel4x4Recepcion.Checked = recepcion.TieneAceiteDiferencialDelantero;
                chbxAceiteDiferencialTraseroRecepcion.Checked = recepcion.TieneAceiteDiferencialTrasero;
                chbxAceiteHidraulicoRecepcion.Checked = recepcion.TieneAceiteHidraulico;
                chbxAceiteMotorRecepcion.Checked = recepcion.TieneAceiteMotor;
                chbxAceitePlanetariosDel4x4Recepcion.Checked = recepcion.TieneAceitePlanetariosDelantero;
                chbxAceiteTransmisionRecepcion.Checked = recepcion.TieneAceiteTransmision;
                chbxAlarmaReversaRecepcion.Checked = recepcion.TieneAlarmaReversa;
                chbxAntenasMonitoreoSatelitalRecepcion.Checked = recepcion.TieneAntenaMonitoreo;
                chbxAsientoOperadorRecepcion.Checked = recepcion.TieneAsientoOperador;
                chbxBandaVentiladorRecepcion.Checked = recepcion.TieneBandaVentilador;
                chbxBateriaRecepcion.Checked = recepcion.TieneBateria;
                chbxBloqueoDiferencialRecepcion.Checked = recepcion.TieneBloqueoDiferencial;
                chbxCombustibleRecepcion.Checked = recepcion.TieneCombustible;
                chbxBoteDelanteroRecepcion.Checked = recepcion.TieneBoteDelantero;
                chbxBoteTraseroRecepcion.Checked = recepcion.TieneBoteTrasero;
                chbxBujesPasadoresCargadorRecepcion.Checked = recepcion.TieneBujesPasadoresCargador;
                chbxBujesPasadoresRetroRecepcion.Checked = recepcion.TieneBujesPasadoresRetro;
                chbxCartuchoFiltroAireRecepcion.Checked = recepcion.TieneCartuchoFiltroAire;
                chbxCinturonSeguridadRecepcion.Checked = recepcion.TieneCinturonSeguridad;
                chbxClaxonRecepcion.Checked = recepcion.TieneClaxon;
                chbxCondicionAsientoRecepcion.Checked = recepcion.TieneCondicionAsiento;
                chbxCondicionCalcasRecepcion.Checked = recepcion.TieneCondicionCalcas;
                chbxCondicionLlantasRecepcion.Checked = recepcion.TieneCondicionLlantas;
                chbxCondicionPinturaRecepcion.Checked = recepcion.TieneCondicionPintura;
                chbxDesacople4x4Recepcion.Checked = recepcion.TieneDesacople;
                chbxEspejoRetrovisorRecepcion.Checked = recepcion.TieneEspejoRetrovisor;
                chbxEstructuraChasisRecepcion.Checked = recepcion.TieneEstructuraChasis;
                chbxFrenoEstacionamientoRecepcion.Checked = recepcion.TieneFrenoEstacionamiento;
                chbxIndicadoresRecepcion.Checked = recepcion.TieneIndicadores;
                chbxJoysticksRecepcion.Checked = recepcion.TieneJoystick;
                chbxLiquidoRefrigeranteRecepcion.Checked = recepcion.TieneLiquidoRefrigerante;
                chbxLucesAdvertenciaRecepcion.Checked = recepcion.TieneLucesAdvertencia;
                chbxLucesDireccionalesRecepcion.Checked = recepcion.TieneLucesDireccionales;
                chbxLucesIntermitentesRecepcion.Checked = recepcion.TieneLucesIntermitentes;
                chbxLucesTrabajoDelanteraRecepcion.Checked = recepcion.TieneLucesTrabajoDelantera;
                chbxLucesTrabajoTraserasRecepcion.Checked = recepcion.TieneLucesTrabajoTraseras;
                chbxManguerasAbrazaderasAdmisionAireRecepcion.Checked = recepcion.TieneManguerasAbrazaderas;
                chbxNeutralizadorTransmisionRecepcion.Checked = recepcion.TieneNeutralizadorTransmision;
                chbxPalancaTransitoRecepcion.Checked = recepcion.TienePalancaTransito;
                chbxPresionLlantasRecepcion.Checked = recepcion.TienePresionLLantas;
                chbxSimbolosSeguridadMaquinaRecepcion.Checked = recepcion.TieneSimbolosSeguridad;
                chbxTacometroRecepcion.Checked = recepcion.TieneTacometro;
                chbxTapaCombustibleRecepcion.Checked = recepcion.TieneTapaCombustible;
                chbxTapaHidraulicoRecepcion.Checked = recepcion.TieneTapaHidraulico;
                chbxVelocidadMinimaMotorRecepcion.Checked = recepcion.TieneVelocidadMinima;
                chbxVelocidadMaximaMotorRecepcion.Checked = recepcion.TieneVelocidadMaxima;
                chbxZapatasEstabilizadoresRecepcion.Checked = recepcion.TieneZapatasEstabilizadores;
                #endregion

                #endregion
            } catch (Exception ex) {
                throw new Exception(".ImprimirReporte: Error al imprimir el reporte. " + ex.Message);
            }
        }
    }
}