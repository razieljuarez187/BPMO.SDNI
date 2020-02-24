using System;
using System.Collections.Generic;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Contratos.BO;
using BPMO.SDNI.Contratos.PSL.BO;
using BPMO.SDNI.Equipos.BO;


namespace BPMO.SDNI.Contratos.PSL.RPT {
    public partial class ListadoVerificacionExcavadoraRPT : DevExpress.XtraReports.UI.XtraReport {

        /// <summary>
        /// Nombre de la clase que será usada en los mensajes de error
        /// </summary>
        private const string nombreClase = "CheckListRORPT";

        public ListadoVerificacionExcavadoraRPT(Dictionary<string, Object> datos) {
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
                var entrega = (ListadoVerificacionExcavadoraBO)datos["Entrega"] ?? new ListadoVerificacionExcavadoraBO();
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
                this.lblObservaciones.Text = !string.IsNullOrEmpty(entrega.Observaciones) ? entrega.Observaciones : string.Empty;
                #endregion

                #region Combustible

                this.lblCombustibleSalida.Text = entrega.Combustible.HasValue ? Convert.ToInt32(entrega.Combustible.Value).ToString("#,##0") + " L" : "N/A";

                #endregion

                #region Datos Recepción
                var recepcion = (ListadoVerificacionExcavadoraBO)datos["Recepcion"] ?? new ListadoVerificacionExcavadoraBO();
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
                this.xrObservacionesRecepcion.Text = !string.IsNullOrEmpty(recepcion.Observaciones) ? recepcion.Observaciones : string.Empty;
                #endregion

                #region Combustible
                this.lblCombustibleRetorno.Text = recepcion.Combustible.HasValue ? Convert.ToInt32(recepcion.Combustible.Value).ToString("#,##0") + " L" : string.Empty;
                #endregion

                #endregion

                #region Cuestionarios
                #region Cuestionario Entrega

                #region Observaciones
                this.lblObservaciones.Text = !string.IsNullOrEmpty(entrega.Observaciones) ? entrega.Observaciones : string.Empty;
                #endregion

                if (entrega.TieneZapatas.HasValue) {
                    if (entrega.TieneZapatas.Value)
                        chkTieneZapatas.Checked = true;
                    else
                        chkTieneZapatas.Checked = false;
                }

                if (entrega.TieneZapatas.HasValue) {
                    if (entrega.TieneZapatas.Value)
                        chkTieneZapatas.Checked = true;
                    else
                        chkTieneZapatas.Checked = false;
                }

                if (entrega.TieneBrazoPluma.HasValue) {
                    if (entrega.TieneBrazoPluma.Value)
                        chkBrazoPluma.Checked = true;
                    else
                        chkBrazoPluma.Checked = false;
                }

                if (entrega.TieneContrapeso.HasValue) {
                    if (entrega.TieneContrapeso.Value)
                        chkContrapeso.Checked = true;
                    else
                        chkContrapeso.Checked = false;
                }

                if (entrega.TieneVastagosGatos.HasValue) {
                    if (entrega.TieneVastagosGatos.Value)
                        chkVastagos.Checked = true;
                    else
                        chkVastagos.Checked = false;
                }

                if (entrega.TieneTensionCadena.HasValue) {
                    if (entrega.TieneTensionCadena.Value)
                        chkTensionCadena.Checked = true;
                    else
                        chkTensionCadena.Checked = false;
                }

                if (entrega.TieneRodillosTransito.HasValue) {
                    if (entrega.TieneRodillosTransito.Value)
                        chkRodillosTransito.Checked = true;
                    else
                        chkTensionCadena.Checked = false;
                }

                if (entrega.TieneEspejosRetrovisores.HasValue) {
                    if (entrega.TieneEspejosRetrovisores.Value)
                        chkEspejosRetrovisores.Checked = true;
                    else
                        chkEspejosRetrovisores.Checked = false;
                }

                if (entrega.TieneCristalesCabina.HasValue) {
                    if (entrega.TieneCristalesCabina.Value)
                        chkCristalesCabina.Checked = true;
                    else
                        chkCristalesCabina.Checked = false;
                }

                if (entrega.TienePuertasCerraduras.HasValue) {
                    if (entrega.TienePuertasCerraduras.Value)
                        chkPuertasCerraduras.Checked = true;
                    else
                        chkPuertasCerraduras.Checked = false;
                }

                if (entrega.TieneBisagrasCofreMotor.HasValue) {
                    if (entrega.TieneBisagrasCofreMotor.Value)
                        chkBisagrasCofreMotor.Checked = true;
                    else
                        chkBisagrasCofreMotor.Checked = false;
                }

                if (entrega.TieneBalancinBote.HasValue) {
                    if (entrega.TieneBalancinBote.Value)
                        chkBalancinBote.Checked = true;
                    else
                        chkBalancinBote.Checked = false;
                }

                if (entrega.TieneCombustible.HasValue) {
                    if (entrega.TieneCombustible.Value)
                        chkCombustible.Checked = true;
                    else
                        chkCombustible.Checked = false;
                }

                if (entrega.TieneAceiteMotor.HasValue) {
                    if (entrega.TieneAceiteMotor.Value)
                        chkAceiteMotor.Checked = true;
                    else
                        chkAceiteMotor.Checked = false;
                }

                if (entrega.TieneAceiteHidraulico.HasValue) {
                    if (entrega.TieneAceiteHidraulico.Value)
                        chkAceiteHidraulico.Checked = true;
                    else
                        chkAceiteHidraulico.Checked = false;
                }

                if (entrega.TieneLiquidoRefrigerante.HasValue) {
                    if (entrega.TieneLiquidoRefrigerante.Value)
                        chkLiquidoRefrijerante.Checked = true;
                    else
                        chkLiquidoRefrijerante.Checked = false;
                }

                if (entrega.TieneReductorEngranesTransito.HasValue) {
                    if (entrega.TieneReductorEngranesTransito.Value)
                        chkReductorEngranesTransito.Checked = true;
                    else
                        chkReductorEngranesTransito.Checked = false;
                }

                if (entrega.TieneReductorSwing.HasValue) {
                    if (entrega.TieneReductorSwing.Value)
                        ckhReductorSwing.Checked = true;
                    else
                        ckhReductorSwing.Checked = false;
                }

                if (entrega.TieneBateria.HasValue) {
                    if (entrega.TieneBateria.Value)
                        chkBateria.Checked = true;
                    else
                        chkBateria.Checked = false;
                }

                if (entrega.TienePasadoresBoom.HasValue) {
                    if (entrega.TienePasadoresBoom.Value)
                        chkPasadoresBoom.Checked = true;
                    else
                        chkPasadoresBoom.Checked = false;
                }

                if (entrega.TienePasadoresBrazo.HasValue) {
                    if (entrega.TienePasadoresBrazo.Value)
                        chkPasadoresBrazo.Checked = true;
                    else
                        chkPasadoresBrazo.Checked = false;
                }

                if (entrega.TienePasadoresBote.HasValue) {
                    if (entrega.TienePasadoresBote.Value)
                        chkPasadoresBote.Checked = true;
                    else
                        chkPasadoresBote.Checked = false;
                }

                if (entrega.TieneTornameza.HasValue) {
                    if (entrega.TieneTornameza.Value)
                        chkTornamesa.Checked = true;
                    else
                        chkTornamesa.Checked = false;
                }

                if (entrega.TieneCentralEngrase.HasValue) {
                    if (entrega.TieneCentralEngrase.Value)
                        chkCentralEngrase.Checked = true;
                    else
                        chkCentralEngrase.Checked = false;
                }

                if (entrega.TieneLucesTrabajo.HasValue) {
                    if (entrega.TieneLucesTrabajo.Value)
                        chkLucesTrabajo.Checked = true;
                    else
                        chkLucesTrabajo.Checked = false;
                }

                if (entrega.TieneLamparasTablero.HasValue) {
                    if (entrega.TieneLamparasTablero.Value)
                        chkLamparasTablero.Checked = true;
                    else
                        chkLamparasTablero.Checked = false;
                }

                if (entrega.TieneInterruptorDesconexion.HasValue) {
                    if (entrega.TieneInterruptorDesconexion.Value)
                        chkInterruptorConexion.Checked = true;
                    else
                        chkInterruptorConexion.Checked = false;
                }

                if (entrega.TieneAlarmaReversa.HasValue) {
                    if (entrega.TieneAlarmaReversa.Value)
                        chkAlarmaReversa.Checked = true;
                    else
                        chkAlarmaReversa.Checked = false;
                }

                if (entrega.TieneHorometro.HasValue) {
                    if (entrega.TieneHorometro.Value)
                        chkHorometro.Checked = true;
                    else
                        chkHorometro.Checked = false;
                }

                if (entrega.TieneLimpiaparabrisas.HasValue) {
                    if (entrega.TieneLimpiaparabrisas.Value)
                        chkLimpiaparabrisas.Checked = true;
                    else
                        chkLimpiaparabrisas.Checked = false;
                }

                if (entrega.TieneJoystick.HasValue) {
                    if (entrega.TieneJoystick.Value)
                        chkJoysticks.Checked = true;
                    else
                        chkJoysticks.Checked = false;
                }

                if (entrega.TieneLucesAdvertencia.HasValue) {
                    if (entrega.TieneLucesAdvertencia.Value)
                        chkLucesAdvertencia.Checked = true;
                    else
                        chkLucesAdvertencia.Checked = false;
                }

                if (entrega.TieneIndicadores.HasValue) {
                    if (entrega.TieneIndicadores.Value)
                        chkIndicadores.Checked = true;
                    else
                        chkIndicadores.Checked = false;
                }

                if (entrega.TienePalancaBloqueoPilotaje.HasValue) {
                    if (entrega.TienePalancaBloqueoPilotaje.Value)
                        chkPalancaBloqueoPilotaje.Checked = true;
                    else
                        chkPalancaBloqueoPilotaje.Checked = false;
                }

                if (entrega.TieneAireAcondicionado.HasValue) {
                    if (entrega.TieneAireAcondicionado.Value)
                        chkAireAcondicionado.Checked = true;
                    else
                        chkAireAcondicionado.Checked = false;
                }

                if (entrega.TieneAutoaceleracion.HasValue) {
                    if (entrega.TieneAutoaceleracion.Value)
                        chkAutoaceleracion.Checked = true;
                    else
                        chkAutoaceleracion.Checked = false;
                }

                if (entrega.TieneVelocidadMinimaMotor.HasValue) {
                    if (entrega.TieneVelocidadMinimaMotor.Value)
                        chkVelocidadMinimaMotor.Checked = true;
                    else
                        chkVelocidadMinimaMotor.Checked = false;
                }

                if (entrega.TieneVelocidadMaximaMotor.HasValue) {
                    if (entrega.TieneVelocidadMaximaMotor.Value)
                        chkVelocidadMaximaMotor.Checked = true;
                    else
                        chkVelocidadMaximaMotor.Checked = false;
                }

                if (entrega.TieneTapaCombustible.HasValue) {
                    if (entrega.TieneTapaCombustible.Value)
                        chkTapaCombustible.Checked = true;
                    else
                        chkTapaCombustible.Checked = false;
                }

                if (entrega.TieneCondicionAsiento.HasValue) {
                    if (entrega.TieneCondicionAsiento.Value)
                        chkCondicionAsiento.Checked = true;
                    else
                        chkCondicionAsiento.Checked = false;
                }

                if (entrega.TieneCondicionPintura.HasValue) {
                    if (entrega.TieneCondicionPintura.Value)
                        chkCondicionPintura.Checked = true;
                    else
                        chkCondicionPintura.Checked = false;
                }

                if (entrega.TieneCondicionCalcas.HasValue) {
                    if (entrega.TieneCondicionCalcas.Value)
                        chkCondicionCalcas.Checked = true;
                    else
                        chkCondicionCalcas.Checked = false;
                }

                if (entrega.TieneSimboloSeguridadMaquina.HasValue) {
                    if (entrega.TieneSimboloSeguridadMaquina.Value)
                        chkSimboloSeguridadMaquina.Checked = true;
                    else
                        chkSimboloSeguridadMaquina.Checked = false;
                }

                if (entrega.TieneEstructuraChasis.HasValue) {
                    if (entrega.TieneEstructuraChasis.Value)
                        chkEstructuraChasis.Checked = true;
                    else
                        chkEstructuraChasis.Checked = false;
                }

                if (entrega.TieneAntenasMonitoreoSatelital.HasValue) {
                    if (entrega.TieneAntenasMonitoreoSatelital.Value)
                        chKAntenasMonitoreoSatelital.Checked = true;
                    else
                        chKAntenasMonitoreoSatelital.Checked = false;
                }

                #endregion
                #region Cuestionario Recepción

                if (recepcion.TieneZapatas.HasValue) {
                    if (recepcion.TieneZapatas.Value)
                        chkTieneZapatasRecepcion.Checked = true;
                    else
                        chkTieneZapatasRecepcion.Checked = false;
                }

                if (recepcion.TieneBrazoPluma.HasValue) {
                    if (recepcion.TieneBrazoPluma.Value)
                        chkBrazoPlumaRecepcion.Checked = true;
                    else
                        chkBrazoPlumaRecepcion.Checked = false;
                }

                if (recepcion.TieneContrapeso.HasValue) {
                    if (recepcion.TieneContrapeso.Value)
                        chkContrapesoRecepcion.Checked = true;
                    else
                        chkContrapesoRecepcion.Checked = false;
                }

                if (recepcion.TieneVastagosGatos.HasValue) {
                    if (recepcion.TieneVastagosGatos.Value)
                        chkVastagosRecepcion.Checked = true;
                    else
                        chkVastagosRecepcion.Checked = false;
                }

                if (recepcion.TieneTensionCadena.HasValue) {
                    if (recepcion.TieneTensionCadena.Value)
                        chkTensionCadenaRecepcion.Checked = true;
                    else
                        chkTensionCadenaRecepcion.Checked = false;
                }

                if (recepcion.TieneRodillosTransito.HasValue) {
                    if (recepcion.TieneRodillosTransito.Value)
                        chkRodillosTransitoRecepcion.Checked = true;
                    else
                        chkTensionCadenaRecepcion.Checked = false;
                }

                if (recepcion.TieneEspejosRetrovisores.HasValue) {
                    if (recepcion.TieneEspejosRetrovisores.Value)
                        chkEspejosRetrovisoresRecepcion.Checked = true;
                    else
                        chkEspejosRetrovisoresRecepcion.Checked = false;
                }

                if (recepcion.TieneCristalesCabina.HasValue) {
                    if (recepcion.TieneCristalesCabina.Value)
                        chkCristalesCabinaRecepcion.Checked = true;
                    else
                        chkCristalesCabinaRecepcion.Checked = false;
                }

                if (recepcion.TienePuertasCerraduras.HasValue) {
                    if (recepcion.TienePuertasCerraduras.Value)
                        chkPuertasCerradurasRecepcion.Checked = true;
                    else
                        chkPuertasCerradurasRecepcion.Checked = false;
                }

                if (recepcion.TieneBisagrasCofreMotor.HasValue) {
                    if (recepcion.TieneBisagrasCofreMotor.Value)
                        chkBisagrasCofreMotorRecepcion.Checked = true;
                    else
                        chkBisagrasCofreMotorRecepcion.Checked = false;
                }

                if (recepcion.TieneBalancinBote.HasValue) {
                    if (recepcion.TieneBalancinBote.Value)
                        chkBalancinBoteRecepcion.Checked = true;
                    else
                        chkBalancinBoteRecepcion.Checked = false;
                }

                if (recepcion.TieneCombustible.HasValue) {
                    if (recepcion.TieneCombustible.Value)
                        chkTieneCombustibleRecepcion.Checked = true;
                    else
                        chkTieneCombustibleRecepcion.Checked = false;
                }

                if (recepcion.TieneAceiteMotor.HasValue) {
                    if (recepcion.TieneAceiteMotor.Value)
                        chkAceiteMotorRecepcion.Checked = true;
                    else
                        chkAceiteMotorRecepcion.Checked = false;
                }

                if (recepcion.TieneAceiteHidraulico.HasValue) {
                    if (recepcion.TieneAceiteHidraulico.Value)
                        chkAceiteHidraulicoRecepcion.Checked = true;
                    else
                        chkAceiteHidraulicoRecepcion.Checked = false;
                }

                if (recepcion.TieneLiquidoRefrigerante.HasValue) {
                    if (recepcion.TieneLiquidoRefrigerante.Value)
                        chkLiquidoRefrijeranteRecepcion.Checked = true;
                    else
                        chkLiquidoRefrijeranteRecepcion.Checked = false;
                }

                if (recepcion.TieneReductorEngranesTransito.HasValue) {
                    if (recepcion.TieneReductorEngranesTransito.Value)
                        chkReductorEngranesTransitoRecepcion.Checked = true;
                    else
                        chkReductorEngranesTransitoRecepcion.Checked = false;
                }

                if (recepcion.TieneReductorSwing.HasValue) {
                    if (recepcion.TieneReductorSwing.Value)
                        ckhReductorSwingRecepcion.Checked = true;
                    else
                        ckhReductorSwingRecepcion.Checked = false;
                }

                if (recepcion.TieneBateria.HasValue) {
                    if (recepcion.TieneBateria.Value)
                        chkBateriaRecepcion.Checked = true;
                    else
                        chkBateriaRecepcion.Checked = false;
                }

                if (recepcion.TienePasadoresBoom.HasValue) {
                    if (recepcion.TienePasadoresBoom.Value)
                        chkPasadoresBoomRecepcion.Checked = true;
                    else
                        chkPasadoresBoomRecepcion.Checked = false;
                }

                if (recepcion.TienePasadoresBrazo.HasValue) {
                    if (recepcion.TienePasadoresBrazo.Value)
                        chkPasadoresBrazoRecepcion.Checked = true;
                    else
                        chkPasadoresBrazoRecepcion.Checked = false;
                }

                if (recepcion.TienePasadoresBote.HasValue) {
                    if (recepcion.TienePasadoresBote.Value)
                        chkPasadoresBoteRecepcion.Checked = true;
                    else
                        chkPasadoresBoteRecepcion.Checked = false;
                }

                if (recepcion.TieneTornameza.HasValue) {
                    if (recepcion.TieneTornameza.Value)
                        chkTornamesaRecepcion.Checked = true;
                    else
                        chkTornamesaRecepcion.Checked = false;
                }

                if (recepcion.TieneCentralEngrase.HasValue) {
                    if (recepcion.TieneCentralEngrase.Value)
                        chkCentralEngraseRecepcion.Checked = true;
                    else
                        chkCentralEngraseRecepcion.Checked = false;
                }

                if (recepcion.TieneLucesTrabajo.HasValue) {
                    if (recepcion.TieneLucesTrabajo.Value)
                        chkLucesTrabajoRecepcion.Checked = true;
                    else
                        chkLucesTrabajoRecepcion.Checked = false;
                }

                if (recepcion.TieneLamparasTablero.HasValue) {
                    if (recepcion.TieneLamparasTablero.Value)
                        chkLamparasTableroRecepcion.Checked = true;
                    else
                        chkLamparasTableroRecepcion.Checked = false;
                }

                if (recepcion.TieneInterruptorDesconexion.HasValue) {
                    if (recepcion.TieneInterruptorDesconexion.Value)
                        chkInterruptorConexionRecepcion.Checked = true;
                    else
                        chkInterruptorConexionRecepcion.Checked = false;
                }

                if (recepcion.TieneAlarmaReversa.HasValue) {
                    if (recepcion.TieneAlarmaReversa.Value)
                        chkAlarmaReversaRecepcion.Checked = true;
                    else
                        chkAlarmaReversaRecepcion.Checked = false;
                }

                if (recepcion.TieneHorometro.HasValue) {
                    if (recepcion.TieneHorometro.Value)
                        chkHorometroRecepcion.Checked = true;
                    else
                        chkHorometroRecepcion.Checked = false;
                }

                if (recepcion.TieneLimpiaparabrisas.HasValue) {
                    if (recepcion.TieneLimpiaparabrisas.Value)
                        chkLimpiaparabrisasRecepcion.Checked = true;
                    else
                        chkLimpiaparabrisasRecepcion.Checked = false;
                }

                if (recepcion.TieneJoystick.HasValue) {
                    if (recepcion.TieneJoystick.Value)
                        chkJoysticksRecepcion.Checked = true;
                    else
                        chkJoysticksRecepcion.Checked = false;
                }

                if (recepcion.TieneLucesAdvertencia.HasValue) {
                    if (recepcion.TieneLucesAdvertencia.Value)
                        chkLucesAdvertenciaRecepcion.Checked = true;
                    else
                        chkLucesAdvertenciaRecepcion.Checked = false;
                }

                if (recepcion.TieneIndicadores.HasValue) {
                    if (recepcion.TieneIndicadores.Value)
                        chkIndicadoresRecepcion.Checked = true;
                    else
                        chkIndicadoresRecepcion.Checked = false;
                }

                if (recepcion.TienePalancaBloqueoPilotaje.HasValue) {
                    if (recepcion.TienePalancaBloqueoPilotaje.Value)
                        chkPalancaBloqueoPilotajeRecepcion.Checked = true;
                    else
                        chkPalancaBloqueoPilotajeRecepcion.Checked = false;
                }

                if (recepcion.TieneAireAcondicionado.HasValue) {
                    if (recepcion.TieneAireAcondicionado.Value)
                        chkAireAcondicionadoRecepcion.Checked = true;
                    else
                        chkAireAcondicionadoRecepcion.Checked = false;
                }

                if (recepcion.TieneAutoaceleracion.HasValue) {
                    if (recepcion.TieneAutoaceleracion.Value)
                        chkAutoaceleracionRecepcion.Checked = true;
                    else
                        chkAutoaceleracionRecepcion.Checked = false;
                }

                if (recepcion.TieneVelocidadMinimaMotor.HasValue) {
                    if (recepcion.TieneVelocidadMinimaMotor.Value)
                        chkVelocidadMinimaMotorRecepcion.Checked = true;
                    else
                        chkVelocidadMinimaMotorRecepcion.Checked = false;
                }

                if (recepcion.TieneVelocidadMaximaMotor.HasValue) {
                    if (recepcion.TieneVelocidadMaximaMotor.Value)
                        chkVelocidadMaximaMotorRecepcion.Checked = true;
                    else
                        chkVelocidadMaximaMotorRecepcion.Checked = false;
                }

                if (recepcion.TieneTapaCombustible.HasValue) {
                    if (recepcion.TieneTapaCombustible.Value)
                        chkTapaCombustibleRecepcion.Checked = true;
                    else
                        chkTapaCombustibleRecepcion.Checked = false;
                }

                if (recepcion.TieneCondicionAsiento.HasValue) {
                    if (recepcion.TieneCondicionAsiento.Value)
                        chkCondicionAsientoRecepcion.Checked = true;
                    else
                        chkCondicionAsientoRecepcion.Checked = false;
                }

                if (recepcion.TieneCondicionPintura.HasValue) {
                    if (recepcion.TieneCondicionPintura.Value)
                        chkCondicionPinturaRecepcion.Checked = true;
                    else
                        chkCondicionPinturaRecepcion.Checked = false;
                }

                if (recepcion.TieneCondicionCalcas.HasValue) {
                    if (recepcion.TieneCondicionCalcas.Value)
                        chkCondicionCalcasRecepcion.Checked = true;
                    else
                        chkCondicionCalcasRecepcion.Checked = false;
                }

                if (recepcion.TieneSimboloSeguridadMaquina.HasValue) {
                    if (recepcion.TieneSimboloSeguridadMaquina.Value)
                        chkSimboloSeguridadMaquinaRecepcion.Checked = true;
                    else
                        chkSimboloSeguridadMaquinaRecepcion.Checked = false;
                }

                if (recepcion.TieneEstructuraChasis.HasValue) {
                    if (recepcion.TieneEstructuraChasis.Value)
                        chkEstructuraChasisRecepcion.Checked = true;
                    else
                        chkEstructuraChasisRecepcion.Checked = false;
                }

                if (recepcion.TieneAntenasMonitoreoSatelital.HasValue) {
                    if (recepcion.TieneAntenasMonitoreoSatelital.Value)
                        chKAntenasMonitoreoSatelitalRecepcion.Checked = true;
                    else
                        chKAntenasMonitoreoSatelitalRecepcion.Checked = false;
                }

                #endregion

                #endregion
            } catch (Exception ex) {
                throw new Exception(".ImprimirReporte: Error al imprimir el reporte. " + ex.Message);
            }
        }


    }
}