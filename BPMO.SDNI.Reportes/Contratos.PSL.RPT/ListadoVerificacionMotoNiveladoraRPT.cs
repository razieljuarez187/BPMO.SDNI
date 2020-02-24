using System;
using System.Collections.Generic;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Contratos.BO;
using BPMO.SDNI.Contratos.PSL.BO;
using BPMO.SDNI.Equipos.BO;

namespace BPMO.SDNI.Contratos.PSL.RPT {
    public partial class ListadoVerificacionMotoNiveladoraRPT : DevExpress.XtraReports.UI.XtraReport {
        /// <summary>
        /// Nombre de la clase que será usada en los mensajes de error
        /// </summary>
        private const string nombreClase = "CheckListRORPT";

        public ListadoVerificacionMotoNiveladoraRPT(Dictionary<string, Object> datos) {
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
                    this.lblUbicacion.Text = !string.IsNullOrEmpty(contrato.DestinoAreaOperacion) ? contrato.DestinoAreaOperacion : string.Empty;
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
                    this.lblNombreCliente.Text = cliente.Cliente.Nombre;

                    if (cliente.Telefonos != null && cliente.Telefonos.Count > 0) {
                        lblTelfonoClienteFacturacion.Text = !string.IsNullOrEmpty(cliente.Telefonos[0].Telefono) ? cliente.Telefonos[0].Telefono : string.Empty;
                        lblTelefonoEmpresaEntrega.Text = !string.IsNullOrEmpty(cliente.Telefonos[0].Telefono) ? cliente.Telefonos[0].Telefono : string.Empty;
                    }
                }

                #endregion

                #region Datos entrega

                var entrega = (ListadoVerificacionMotoNiveladoraBO)datos["Entrega"] ?? new ListadoVerificacionMotoNiveladoraBO();

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
                this.lblTanqueHidraulico.Text = "N/A";
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
                var recepcion = (ListadoVerificacionMotoNiveladoraBO)datos["Recepcion"] ?? new ListadoVerificacionMotoNiveladoraBO();

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

                #region Cuestionarios Checklist

                #region Cuestionario Entrega

                #region General
                chbxPresionLlantas.Checked = entrega.TienePresionLlantas;
                chbxBandaVentilador.Checked = entrega.TieneBandaVentilador;
                chbxMangueraAbrazaderaAdmisionAire.Checked = entrega.TieneMaguerasYAbrazaderas;
                chbxCartuchoFiltroAire.Checked = entrega.TieneCartuchoFiltro;
                chbxCuchilla.Checked = entrega.TieneCuchilla;
                chbxAjusteGiraCirculoCuchilla.Checked = entrega.TieneAjusteGiracirculoCuchilla;
                chbxRipperEscarificador.Checked = entrega.TieneRipperEscarificador;
                chbxCinturonSeguridad.Checked = entrega.TieneCinturonSeguridad;
                chbxEspejosRetroVisores.Checked = entrega.TieneEspejosRetrovisores;
                #endregion

                #region Niveles de fluidos
                chbxCombustible.Checked = entrega.TieneCombustible;
                chbxAceiteMotor.Checked = entrega.TieneAceiteMotor;
                chbxLiquidoRefrigerante.Checked = entrega.TieneLiquidoRefrigerante;
                chbxAceiteHidraulico.Checked = entrega.TieneAceiteHidraulico;
                chbxAceiteTransmision.Checked = entrega.TieneAceiteTransmision;
                chbxAceiteDiferencial.Checked = entrega.TieneAceiteDiferencial;
                chbxAceiteMandosFinales.Checked = entrega.TieneAceiteMandosFinales;
                chbxAceiteTandem.Checked = entrega.TieneAceiteTandem;
                chbxAceiteCajaEngranesGiracirculo.Checked = entrega.TieneAceiteEngranesGiracirculo;
                chbxBateria.Checked = entrega.TieneBateria;
                #endregion

                #region Lubricación
                chbxArticulacionesCuchilla.Checked = entrega.TieneArticulacionesCuchilla;
                chbxArticulacionesRipper.Checked = entrega.TieneArticulacionesRiper;
                chbxArticulacionesEscarificador.Checked = entrega.TieneArticulacionesEscarificador;
                chbxArticulacionesDireccion.Checked = entrega.TieneArticulacionesDireccion;
                chbxArticulacionChasis.Checked = entrega.TieneArticulacionesChasis;
                #endregion

                #region Funciones eléctricas
                chbxLucesTrabajoDelantera.Checked = entrega.TieneLucesTrabajoDelantera;
                chbxLucesTrabajoTraseras.Checked = entrega.TieneLucesTrabajoTrasera;
                chbxClaxon.Checked = entrega.TieneClaxon;
                chbxAlarmaReversa.Checked = entrega.TieneAlarmaReversa;
                chbxLucesIntermitentes.Checked = entrega.TieneLucesIntermitentes;
                chbxLucesDireccionales.Checked = entrega.TieneLucesDireccionales;
                #endregion

                #region Controles
                chbxPalancaTransito.Checked = entrega.TienePalancaTransito;
                chbxPalancaFuncionesHidraulicos.Checked = entrega.TienePalancaFuncionesHidraulicos;
                chbxLucesAdvertencia.Checked = entrega.TieneLucesAdvertencia;
                chbxIndicadores.Checked = entrega.TieneIndicadores;
                chbxTacometro.Checked = entrega.TieneTacometro;
                chbxFrenoEstacionamiento.Checked = entrega.TieneFrenoEstacionamiento;
                chbxVelocidadMinimaMotor.Checked = entrega.TieneVelocidadMinMotor;
                chbxVelocidadMaximaMotor.Checked = entrega.TieneVelocidadMaxMotor;
                #endregion

                #region Miscelaneos
                chbxTapaCombustible.Checked = entrega.TieneTapaCombustible;
                chbxTapaHidraulico.Checked = entrega.TieneTapaHidraulico;
                chbxCondicionAsiento.Checked = entrega.TieneCondicionAsiento;
                chbxCondicionLlantas.Checked = entrega.TieneCondicionLlantas;
                chbxCondicionPintura.Checked = entrega.TieneCondicionPintura;
                chbxCondicionCalcas.Checked = entrega.TieneCondicionCalcas;
                chbxSimbolosSeguridadMaquina.Checked = entrega.TieneSimbolosSeguridad;
                chbxEstructuraChasis.Checked = entrega.TieneEstructuraChasis;
                chbxAntenasMonitoreoSatelital.Checked = entrega.TieneAntenaMonitoreo;
                #endregion

                #endregion

                #region Cuestionario Recepción

                #region General
                chbxPresionLlantasRecepcion.Checked = recepcion.TienePresionLlantas;
                chbxBandaVentiladorRecepcion.Checked = recepcion.TieneBandaVentilador;
                chbxManguerasAbrazaderasAdmisionAireRecepcion.Checked = recepcion.TieneMaguerasYAbrazaderas;
                chbxCartuchoFiltroAireRecepcion.Checked = recepcion.TieneCartuchoFiltro;
                chbxCuchillaRecepcion.Checked = recepcion.TieneCuchilla;
                chbxAjusteGiraCirculoCuchillaRecepcion.Checked = recepcion.TieneAjusteGiracirculoCuchilla;
                chbxRipperEscarificadorRecepcion.Checked = recepcion.TieneRipperEscarificador;
                chbxCinturonSeguridadRecepcion.Checked = recepcion.TieneCinturonSeguridad;
                chbxEspejosRetrovisoresRecepcion.Checked = recepcion.TieneEspejosRetrovisores;
                #endregion

                #region Niveles de fluidoss
                chbxCombustibleRecepcion.Checked = recepcion.TieneCombustible;
                chbxAceiteMotorRecepcion.Checked = recepcion.TieneAceiteMotor;
                chbxLiquidoRefrigeranteRecepcion.Checked = recepcion.TieneLiquidoRefrigerante;
                chbxAceiteHidraulicoRecepcion.Checked = recepcion.TieneAceiteHidraulico;
                chbxAceiteTransmisionRecepcion.Checked = recepcion.TieneAceiteTransmision;
                chbxAceiteDiferencialRecepcion.Checked = recepcion.TieneAceiteDiferencial;
                chbxAceiteMandosFinalesRecepcion.Checked = recepcion.TieneAceiteMandosFinales;
                chbxAceiteTandemRecepcion.Checked = recepcion.TieneAceiteTandem;
                chbxAceiteCajaEngranesGiraCirculoRecepcion.Checked = recepcion.TieneAceiteEngranesGiracirculo;
                chbxBateriaRecepcion.Checked = recepcion.TieneBateria;
                #endregion

                #region Lubricación
                chbxArticulacionesCuchillaRecepcion.Checked = recepcion.TieneArticulacionesCuchilla;
                chbxArticulacionesRipperRecepcion.Checked = recepcion.TieneArticulacionesRiper;
                chbxArticulacionesEscarificadorRecepcion.Checked = recepcion.TieneArticulacionesEscarificador;
                chbxArticulacionesDireccionRecepcion.Checked = recepcion.TieneArticulacionesDireccion;
                chbxArticulacionChasisRecepcion.Checked = recepcion.TieneArticulacionesChasis;
                #endregion

                #region Funciones eléctricas
                chbxLucesTrabajoDelanteraRecepcion.Checked = recepcion.TieneLucesTrabajoDelantera;
                chbxLucesTrabajoTraserasRecepcion.Checked = recepcion.TieneLucesTrabajoTrasera;
                chbxClaxonRecepcion.Checked = recepcion.TieneClaxon;
                chbxAlarmaReversaRecepcion.Checked = recepcion.TieneAlarmaReversa;
                chbxLucesIntermitentesRecepcion.Checked = recepcion.TieneLucesIntermitentes;
                chbxLucesDireccionalesRecepcion.Checked = recepcion.TieneLucesDireccionales;
                #endregion

                #region Controles
                chbxPalancaTransitoRecepcion.Checked = recepcion.TienePalancaTransito;
                chbxPalancasFuncionesHidraulicosRecepcion.Checked = recepcion.TienePalancaFuncionesHidraulicos;
                chbxLucesAdvertenciaRecepcion.Checked = recepcion.TieneLucesAdvertencia;
                chbxIndicadoresRecepcion.Checked = recepcion.TieneIndicadores;
                chbxTacometroRecepcion.Checked = recepcion.TieneTacometro;
                chbxFrenoEstacionamientoRecepcion.Checked = recepcion.TieneFrenoEstacionamiento;
                chbxVelocidadMinimaMotorRecepcion.Checked = recepcion.TieneVelocidadMinMotor;
                chbxVelocidadMaximaMotorRecepcion.Checked = recepcion.TieneVelocidadMaxMotor;
                #endregion

                #region Miscelaneos
                chbxTapaCombustibleRecepcion.Checked = recepcion.TieneTapaCombustible;
                chbxTapaHidraulicoRecepcion.Checked = recepcion.TieneTapaHidraulico;
                chbxCondicionAsientoRecepcion.Checked = recepcion.TieneCondicionAsiento;
                chbxCondicionLlantasRecepcion.Checked = recepcion.TieneCondicionLlantas;
                chbxCondicionPinturaRecepcion.Checked = recepcion.TieneCondicionPintura;
                chbxCondicionCalcasRecepcion.Checked = recepcion.TieneCondicionCalcas;
                chbxSimbolosSeguridadMaquinaRecepcion.Checked = recepcion.TieneSimbolosSeguridad;
                chbxEstructuraChasisRecepcion.Checked = recepcion.TieneEstructuraChasis;
                chbxAntenasMonitoreoSatelitalRecepcion.Checked = recepcion.TieneAntenaMonitoreo;
                #endregion

                #endregion

                #endregion
            } catch (Exception ex) {
                throw new Exception(".ImprimirReporte: Error al imprimir el reporte. " + ex.Message);
            }
        }

    }
}