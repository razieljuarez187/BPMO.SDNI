using System;
using System.Collections.Generic;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Contratos.BO;
using BPMO.SDNI.Contratos.PSL.BO;
using BPMO.SDNI.Equipos.BO;


namespace BPMO.SDNI.Contratos.PSL.RPT {
    public partial class ListadoVerificacionSubArrendadosRPT : DevExpress.XtraReports.UI.XtraReport {
        /// <summary>
        /// Nombre de la clase que será usada en los mensajes de error
        /// </summary>
        private const string nombreClase = "ListadoVerificacionSubArrendadosRPT";

        public ListadoVerificacionSubArrendadosRPT(Dictionary<string, Object> datos) {
            InitializeComponent();

            this.ImprimirReporte(datos);
        }

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

                var entrega = (ListadoVerificacionSubArrendadoBO)datos["Entrega"] ?? new ListadoVerificacionSubArrendadoBO();

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
                var recepcion = (ListadoVerificacionSubArrendadoBO)datos["Recepcion"] ?? new ListadoVerificacionSubArrendadoBO();

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

                #region Cuestionario entrega
                xrlNivelesFluidoE.Text = entrega.TieneNivelesFluido.HasValue ? (entrega.TieneNivelesFluido.Value ? "OK" : "NO") : string.Empty;
                xrlTapaFluidosE.Text = entrega.TieneTapaFluidos.HasValue ? (entrega.TieneTapaFluidos.Value ? "OK" : "NO") : string.Empty;
                xrlSistemaElectricoE.Text = entrega.TieneSistemaElectrico.HasValue ? (entrega.TieneSistemaElectrico.Value ? "OK" : "NO") : string.Empty;
                xrlFarosTraserosE.Text = entrega.TieneFarosTraseros.HasValue ? (entrega.TieneFarosTraseros.Value ? "OK" : "NO") : string.Empty;
                xrlFarosDelanterosE.Text = entrega.TieneFarosDelanteros.HasValue ? (entrega.TieneFarosDelanteros.Value ? "OK" : "NO") : string.Empty;
                xrCuartosDireccionalesE.Text = entrega.TieneCuartosDireccionales.HasValue ? (entrega.TieneCuartosDireccionales.Value ? "OK" : "NO") : string.Empty;
                xrLimpiaparabrisasE.Text = entrega.TieneLimpiaparabrisas.HasValue ? (entrega.TieneLimpiaparabrisas.Value ? "OK" : "NO") : string.Empty;
                xrBateriaE.Text = entrega.TieneBateria.HasValue ? (entrega.TieneBateria.Value ? "OK" : "NO") : string.Empty;
                xrChasisE.Text = entrega.TieneChasis.HasValue ? (entrega.TieneChasis.Value ? "OK" : "NO") : string.Empty;
                xrEstabilizadoresE.Text = entrega.TieneEstabilizadores.HasValue ? (entrega.TieneEstabilizadores.Value ? "OK" : "NO") : string.Empty;
                xrZapataE.Text = entrega.TieneZapata.HasValue ? (entrega.TieneZapata.Value ? "OK" : "NO") : string.Empty;
                xrBoteTraseroE.Text = entrega.TieneBoteTrasero.HasValue ? (entrega.TieneBoteTrasero.Value ? "OK" : "NO") : string.Empty;
                xrBoteDelanteroE.Text = entrega.TieneBoteDelantero.HasValue ? (entrega.TieneBoteDelantero.Value ? "OK" : "NO") : string.Empty;
                xrBrazoPlumaE.Text = entrega.TieneBrazoPluma.HasValue ? (entrega.TieneBrazoPluma.Value ? "OK" : "NO") : string.Empty;
                xrContrapesoE.Text = entrega.TieneContrapeso.HasValue ? (entrega.TieneContrapeso.Value ? "OK" : "NO") : string.Empty;
                xrVastagosE.Text = entrega.TieneVastagos.HasValue ? (entrega.TieneVastagos.Value ? "OK" : "NO") : string.Empty;
                xrTensionCadenaE.Text = entrega.TieneTensionCadena.HasValue ? (entrega.TieneTensionCadena.Value ? "OK" : "NO") : string.Empty;
                xrlNivelesFluidosE.Text = entrega.TieneNivelesFluidos.HasValue ? (entrega.TieneNivelesFluidos.Value ? "OK" : "NO") : string.Empty;
                xrlSistemaRemolqueE.Text = entrega.TieneSistemaRemolque.HasValue ? (entrega.TieneSistemaRemolque.Value ? "OK" : "NO") : string.Empty;
                xrlEnsambleRuedaE.Text = entrega.TieneEnsamblajeRueda.HasValue ? (entrega.TieneEnsamblajeRueda.Value ? "OK" : "NO") : string.Empty;
                xrlEstructuraChasisE.Text = entrega.TieneEstructuraChasis.HasValue ? (entrega.TieneEstructuraChasis.Value ? "OK" : "NO") : string.Empty;
                xrlPinturaE.Text = entrega.TienePintura.HasValue ? (entrega.TienePintura.Value ? "OK" : "NO") : string.Empty;
                xrlLlantasE.Text = entrega.TieneLlantas.HasValue ? (entrega.TieneLlantas.Value ? "OK" : "NO") : string.Empty;
                xrlSistemaVibratorioE.Text = entrega.TieneSistemaVibratorio.HasValue ? (entrega.TieneSistemaVibratorio.Value ? "OK" : "NO") : string.Empty;
                xrlZapataRodilloE.Text = entrega.TieneZapataRodillo.HasValue ? (entrega.TieneZapataRodillo.Value ? "OK" : "NO") : string.Empty;
                xrlAsientoOperadorE.Text = entrega.TieneAsientoOperador.HasValue ? (entrega.TieneAsientoOperador.Value ? "OK" : "NO") : string.Empty;
                xrlEsperoRetrovisorE.Text = entrega.TieneEspejoRetrovisor.HasValue ? (entrega.TieneEspejoRetrovisor.Value ? "OK" : "NO") : string.Empty;
                xrlPalancaControlE.Text = entrega.TienePalancaControl.HasValue ? (entrega.TienePalancaControl.Value ? "OK" : "NO") : string.Empty;
                xrlTableroInstruentosE.Text = entrega.TieneTableroInstrumentos.HasValue ? (entrega.TieneTableroInstrumentos.Value ? "OK" : "NO") : string.Empty;
                xrlMoldurasTolvasE.Text = entrega.TieneMoldurasTolvas.HasValue ? (entrega.TieneMoldurasTolvas.Value ? "OK" : "NO") : string.Empty;
                xrlAireAcondicionadoE.Text = entrega.TieneAireAcondicionado.HasValue ? (entrega.TieneAireAcondicionado.Value ? "OK" : "NO") : string.Empty;
                xrlCristalesLateralesE.Text = entrega.TieneCristalesLaterales.HasValue ? (entrega.TieneCristalesLaterales.Value ? "OK" : "NO") : string.Empty;
                xrlPanoramicoE.Text = entrega.TienePanoramico.HasValue ? (entrega.TienePanoramico.Value ? "OK" : "NO") : string.Empty;
                xrlPuertasCerradurasE.Text = entrega.TienePuertasCerraduras.HasValue ? (entrega.TienePuertasCerraduras.Value ? "OK" : "NO") : string.Empty;
                xrlCofreMotorE.Text = entrega.TieneCofreMotor.HasValue ? (entrega.TieneCofreMotor.Value ? "OK" : "NO") : string.Empty;
                xrlParrillaRadiadorE.Text = entrega.TieneParrillaRadiador.HasValue ? (entrega.TieneParrillaRadiador.Value ? "OK" : "NO") : string.Empty;
                xrlAlarmaMovimientoE.Text = entrega.TieneAlarmaMovimiento.HasValue ? (entrega.TieneAlarmaMovimiento.Value ? "OK" : "NO") : string.Empty;
                xrlEstereoE.Text = entrega.TieneEstereo.HasValue ? (entrega.TieneEstereo.Value ? "OK" : "NO") : string.Empty;
                xrlVentiladorElectricoE.Text = entrega.TieneVentiladorElectrico.HasValue ? (entrega.TieneVentiladorElectrico.Value ? "OK" : "NO") : string.Empty;
                xrlIndicadoresInterruptoresE.Text = entrega.TieneIndicadoresInterruptores.HasValue ? (entrega.TieneIndicadoresInterruptores.Value ? "OK" : "NO") : string.Empty;
                xrlPinturaEPE.Text = entrega.TienePinturaEP.HasValue ? (entrega.TienePinturaEP.Value ? "OK" : "NO") : string.Empty;
                xrlKitMartilloE.Text = entrega.TieneKitMartillo.HasValue ? (entrega.TieneKitMartillo.Value ? "OK" : "NO") : string.Empty;
                xrlCentralEngraneE.Text = entrega.TieneCentralEngrane.HasValue ? (entrega.TieneCentralEngrane.Value ? "OK" : "NO") : string.Empty;
                xrlAmperimetroE.Text = entrega.TieneAmperimetro.HasValue ? (entrega.TieneAmperimetro.Value ? "OK" : "NO") : string.Empty;
                xrlVoltimetroE.Text = entrega.TieneVoltimetro.HasValue ? (entrega.TieneVoltimetro.Value ? "OK" : "NO") : string.Empty;
                xrlHorometroE.Text = entrega.TieneHorometro.HasValue ? (entrega.TieneHorometro.Value ? "OK" : "NO") : string.Empty;
                xrlFrecuentometroE.Text = entrega.TieneFrecuentometro.HasValue ? (entrega.TieneFrecuentometro.Value ? "OK" : "NO") : string.Empty;
                xrlInterruptorTermomagneticoE.Text = entrega.TieneInterruptorTermomagnetico.HasValue ? (entrega.TieneInterruptorTermomagnetico.Value ? "OK" : "NO") : string.Empty;
                xrlManometroPresionE.Text = entrega.TieneManometroPresion.HasValue ? (entrega.TieneManometroPresion.Value ? "OK" : "NO") : string.Empty;
                xrlTipoVoltajeE.Text = entrega.TieneTipoVoltaje.HasValue ? (entrega.TieneTipoVoltaje.Value ? "OK" : "NO") : string.Empty;
                xrlLamparasE.Text = entrega.TieneLamparas.HasValue ? (entrega.TieneLamparas.Value ? "OK" : "NO") : string.Empty;
                xrlFuncionamientoE.Text = entrega.TieneFuncionamiento.HasValue ? (entrega.TieneFuncionamiento.Value ? "OK" : "NO") : string.Empty;
                #endregion

                #region Cuestionario recepción
                xrlNivelesFluidoR.Text = recepcion.TieneNivelesFluido.HasValue ? (recepcion.TieneNivelesFluido.Value ? "OK" : "NO") : string.Empty;
                xrlTapaFluidosR.Text = recepcion.TieneTapaFluidos.HasValue ? (recepcion.TieneTapaFluidos.Value ? "OK" : "NO") : string.Empty;
                xrlSistemaElectricoR.Text = recepcion.TieneSistemaElectrico.HasValue ? (recepcion.TieneSistemaElectrico.Value ? "OK" : "NO") : string.Empty;
                xrlFarosTraserosR.Text = recepcion.TieneFarosTraseros.HasValue ? (recepcion.TieneFarosTraseros.Value ? "OK" : "NO") : string.Empty;
                xrlFarosDelanterosR.Text = recepcion.TieneFarosDelanteros.HasValue ? (recepcion.TieneFarosDelanteros.Value ? "OK" : "NO") : string.Empty;
                xrCuartosDireccionalesR.Text = recepcion.TieneCuartosDireccionales.HasValue ? (recepcion.TieneCuartosDireccionales.Value ? "OK" : "NO") : string.Empty;
                xrLimpiaparabrisasR.Text = recepcion.TieneLimpiaparabrisas.HasValue ? (recepcion.TieneLimpiaparabrisas.Value ? "OK" : "NO") : string.Empty;
                xrBateriaR.Text = recepcion.TieneBateria.HasValue ? (recepcion.TieneBateria.Value ? "OK" : "NO") : string.Empty;
                xrChasisR.Text = recepcion.TieneChasis.HasValue ? (recepcion.TieneChasis.Value ? "OK" : "NO") : string.Empty;
                xrEstabilizadoresR.Text = recepcion.TieneEstabilizadores.HasValue ? (recepcion.TieneEstabilizadores.Value ? "OK" : "NO") : string.Empty;
                xrZapataR.Text = recepcion.TieneZapata.HasValue ? (recepcion.TieneZapata.Value ? "OK" : "NO") : string.Empty;
                xrBoteTraseroR.Text = recepcion.TieneBoteTrasero.HasValue ? (recepcion.TieneBoteTrasero.Value ? "OK" : "NO") : string.Empty;
                xrBoteDelanteroR.Text = recepcion.TieneBoteDelantero.HasValue ? (recepcion.TieneBoteDelantero.Value ? "OK" : "NO") : string.Empty;
                xrBrazoPlumaR.Text = recepcion.TieneBrazoPluma.HasValue ? (recepcion.TieneBrazoPluma.Value ? "OK" : "NO") : string.Empty;
                xrContrapesoR.Text = recepcion.TieneContrapeso.HasValue ? (recepcion.TieneContrapeso.Value ? "OK" : "NO") : string.Empty;
                xrVastagosR.Text = recepcion.TieneVastagos.HasValue ? (recepcion.TieneVastagos.Value ? "OK" : "NO") : string.Empty;
                xrTensionCadenaR.Text = recepcion.TieneTensionCadena.HasValue ? (recepcion.TieneTensionCadena.Value ? "OK" : "NO") : string.Empty;
                xrlNivelesFluidosR.Text = recepcion.TieneNivelesFluidos.HasValue ? (recepcion.TieneNivelesFluidos.Value ? "OK" : "NO") : string.Empty;
                xrlSistemaRemolqueR.Text = recepcion.TieneSistemaRemolque.HasValue ? (recepcion.TieneSistemaRemolque.Value ? "OK" : "NO") : string.Empty;
                xrlEnsambleRuedaR.Text = recepcion.TieneEnsamblajeRueda.HasValue ? (recepcion.TieneEnsamblajeRueda.Value ? "OK" : "NO") : string.Empty;
                xrlEstructuraChasisR.Text = recepcion.TieneEstructuraChasis.HasValue ? (recepcion.TieneEstructuraChasis.Value ? "OK" : "NO") : string.Empty;
                xrlPinturaR.Text = recepcion.TienePintura.HasValue ? (recepcion.TienePintura.Value ? "OK" : "NO") : string.Empty;
                xrlLlantasR.Text = recepcion.TieneLlantas.HasValue ? (recepcion.TieneLlantas.Value ? "OK" : "NO") : string.Empty;
                xrlSistemaVibratorioR.Text = recepcion.TieneSistemaVibratorio.HasValue ? (recepcion.TieneSistemaVibratorio.Value ? "OK" : "NO") : string.Empty;
                xrlZapataRodilloR.Text = recepcion.TieneZapataRodillo.HasValue ? (recepcion.TieneZapataRodillo.Value ? "OK" : "NO") : string.Empty;
                xrlAsientoOperadorR.Text = recepcion.TieneAsientoOperador.HasValue ? (recepcion.TieneAsientoOperador.Value ? "OK" : "NO") : string.Empty;
                xrlEsperoRetrovisorR.Text = recepcion.TieneEspejoRetrovisor.HasValue ? (recepcion.TieneEspejoRetrovisor.Value ? "OK" : "NO") : string.Empty;
                xrlPalancaControlR.Text = recepcion.TienePalancaControl.HasValue ? (recepcion.TienePalancaControl.Value ? "OK" : "NO") : string.Empty;
                xrlTableroInstruentosR.Text = recepcion.TieneTableroInstrumentos.HasValue ? (recepcion.TieneTableroInstrumentos.Value ? "OK" : "NO") : string.Empty;
                xrlMoldurasTolvasR.Text = recepcion.TieneMoldurasTolvas.HasValue ? (recepcion.TieneMoldurasTolvas.Value ? "OK" : "NO") : string.Empty;
                xrlAireAcondicionadoR.Text = recepcion.TieneAireAcondicionado.HasValue ? (recepcion.TieneAireAcondicionado.Value ? "OK" : "NO") : string.Empty;
                xrlCristalesLateralesR.Text = recepcion.TieneCristalesLaterales.HasValue ? (recepcion.TieneCristalesLaterales.Value ? "OK" : "NO") : string.Empty;
                xrlPanoramicoR.Text = recepcion.TienePanoramico.HasValue ? (recepcion.TienePanoramico.Value ? "OK" : "NO") : string.Empty;
                xrlPuertasCerradurasR.Text = recepcion.TienePuertasCerraduras.HasValue ? (recepcion.TienePuertasCerraduras.Value ? "OK" : "NO") : string.Empty;
                xrlCofreMotorR.Text = recepcion.TieneCofreMotor.HasValue ? (recepcion.TieneCofreMotor.Value ? "OK" : "NO") : string.Empty;
                xrlParrillaRadiadorR.Text = recepcion.TieneParrillaRadiador.HasValue ? (recepcion.TieneParrillaRadiador.Value ? "OK" : "NO") : string.Empty;
                xrlAlarmaMovimientoR.Text = recepcion.TieneAlarmaMovimiento.HasValue ? (recepcion.TieneAlarmaMovimiento.Value ? "OK" : "NO") : string.Empty;
                xrlEstereoR.Text = recepcion.TieneEstereo.HasValue ? (recepcion.TieneEstereo.Value ? "OK" : "NO") : string.Empty;
                xrlVentiladorElectricoR.Text = recepcion.TieneVentiladorElectrico.HasValue ? (recepcion.TieneVentiladorElectrico.Value ? "OK" : "NO") : string.Empty;
                xrlIndicadoresInterruptoresR.Text = recepcion.TieneIndicadoresInterruptores.HasValue ? (recepcion.TieneIndicadoresInterruptores.Value ? "OK" : "NO") : string.Empty;
                xrlPinturaEPR.Text = recepcion.TienePinturaEP.HasValue ? (recepcion.TienePinturaEP.Value ? "OK" : "NO") : string.Empty;
                xrlKitMartilloR.Text = recepcion.TieneKitMartillo.HasValue ? (recepcion.TieneKitMartillo.Value ? "OK" : "NO") : string.Empty;
                xrlCentralEngraneR.Text = recepcion.TieneCentralEngrane.HasValue ? (recepcion.TieneCentralEngrane.Value ? "OK" : "NO") : string.Empty;
                xrlAmperimetroR.Text = recepcion.TieneAmperimetro.HasValue ? (recepcion.TieneAmperimetro.Value ? "OK" : "NO") : string.Empty;
                xrlVoltimetroR.Text = recepcion.TieneVoltimetro.HasValue ? (recepcion.TieneVoltimetro.Value ? "OK" : "NO") : string.Empty;
                xrlHorometroR.Text = recepcion.TieneHorometro.HasValue ? (recepcion.TieneHorometro.Value ? "OK" : "NO") : string.Empty;
                xrlFrecuentometroR.Text = recepcion.TieneFrecuentometro.HasValue ? (recepcion.TieneFrecuentometro.Value ? "OK" : "NO") : string.Empty;
                xrlInterruptorTermomagneticoR.Text = recepcion.TieneInterruptorTermomagnetico.HasValue ? (recepcion.TieneInterruptorTermomagnetico.Value ? "OK" : "NO") : string.Empty;
                xrlManometroPresionR.Text = recepcion.TieneManometroPresion.HasValue ? (recepcion.TieneManometroPresion.Value ? "OK" : "NO") : string.Empty;
                xrlTipoVoltajeR.Text = recepcion.TieneTipoVoltaje.HasValue ? (recepcion.TieneTipoVoltaje.Value ? "OK" : "NO") : string.Empty;
                xrlLamparasR.Text = recepcion.TieneLamparas.HasValue ? (recepcion.TieneLamparas.Value ? "OK" : "NO") : string.Empty;
                xrlFuncionamientoR.Text = recepcion.TieneFuncionamiento.HasValue ? (recepcion.TieneFuncionamiento.Value ? "OK" : "NO") : string.Empty;
                #endregion

                xrlComentariosGenerales.Text = !string.IsNullOrEmpty(entrega.ComentariosGenerales) ? entrega.ComentariosGenerales : string.Empty;
                xrlComentariosGeneralesR.Text = !string.IsNullOrEmpty(recepcion.ComentariosGenerales) ? recepcion.ComentariosGenerales : string.Empty;
            } catch (Exception ex) {
                throw new Exception(".ImprimirReporte: Error al imprimir el reporte. " + ex.Message);
            }
        }

    }
}