using System;
using System.Collections.Generic;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Contratos.BO;
using BPMO.SDNI.Contratos.PSL.BO;
using BPMO.SDNI.Equipos.BO;

namespace BPMO.SDNI.Contratos.PSL.RPT {
    public partial class ListadoVerificacionEntregaRecepcionRPT : DevExpress.XtraReports.UI.XtraReport {
        /// <summary>
        /// Nombre de la clase que será usada en los mensajes de error
        /// </summary>
        private const string nombreClase = "CheckListRORPT";

        public ListadoVerificacionEntregaRecepcionRPT(Dictionary<string, Object> datos) {
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
                            xrlDireccionEmpresa.Text = string.Format("{0} {1} {2} {3} CP. {4}",
                                contrato.Sucursal.DireccionesSucursal[0].Calle ?? string.Empty,
                                contrato.Sucursal.DireccionesSucursal[0].Colonia ?? string.Empty,
                                contrato.Sucursal.DireccionesSucursal[0].Ubicacion.Municipio.Codigo ?? string.Empty,
                                contrato.Sucursal.DireccionesSucursal[0].Ubicacion.Estado.Codigo ?? string.Empty, 
                                contrato.Sucursal.DireccionesSucursal[0].CodigoPostal ?? string.Empty
                                ).ToUpper();

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
                //if (unidad.TipoEquipoServicio != null)
                //    descripcion = unidad.TipoEquipoServicio.Nombre + " ";
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

                var entrega = (ListadoVerificacionEntregaRecepcionBO)datos["Entrega"] ?? new ListadoVerificacionEntregaRecepcionBO();

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
                this.lblTanqueCombustible.Text = unidad == null || unidad.CombustibleConsumidoTotal == null ? "N/A" : unidad.CombustibleConsumidoTotal.ToString() + " L";
                #endregion

                #region Cuestionario Entrega

                #region Observaciones
                this.lblObservacionesEntrega.Text = !string.IsNullOrEmpty(entrega.Observaciones) ? entrega.Observaciones : string.Empty;
                #endregion

                #endregion

                #region Combustible

                this.lblCombustibleSalida.Text = entrega.Combustible.HasValue ? Convert.ToInt32(entrega.Combustible.Value).ToString("#,##0") + " L" : "N/A";

                #endregion

                #region Datos Recepción
                var recepcion = (ListadoVerificacionEntregaRecepcionBO)datos["Recepcion"] ?? new ListadoVerificacionEntregaRecepcionBO();

                #region Fecha recepción
                this.lblFechaRecepcion.Text = recepcion.Fecha.HasValue ? recepcion.Fecha.Value.ToShortDateString() : string.Empty;
                #endregion

                #region Horómetro
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

                #region Fecha y Hora salida
                this.lblFechaEntrega.Text = contrato.FechaInicioArrendamiento.HasValue ? contrato.FechaInicioArrendamiento.Value.ToShortDateString() : string.Empty;
                this.lblFechaRecepcion.Text = recepcion.Fecha.HasValue ? recepcion.Fecha.Value.ToShortDateString() : (contrato.FechaPromesaActual.HasValue ? contrato.FechaPromesaActual.Value.ToShortDateString() : string.Empty);
                #endregion
                #endregion

                #region entrega

                xrlBandasE.Text = entrega.TieneBandas.HasValue ? (entrega.TieneBandas.Value ? "SI" : "NO") : "";
                xrlFiltroAceiteE.Text = entrega.TieneFiltroAceite.HasValue ? (entrega.TieneFiltroAceite.Value ? "SI" : "NO") : "";
                xrlFiltroAguaE.Text = entrega.TieneFiltroAgua.HasValue ? (entrega.TieneFiltroAgua.Value ? "SI" : "NO") : "";
                xrlFiltroCombustibleE.Text = entrega.TieneFiltroCombustible.HasValue ? (entrega.TieneFiltroCombustible.Value ? "SI" : "NO") : "";
                xrlFiltroAireE.Text = entrega.TieneFiltroAire.HasValue ? (entrega.TieneFiltroAire.Value ? "SI" : "NO") : "";
                xrlManguerasE.Text = entrega.TieneMangueras.HasValue ? (entrega.TieneMangueras.Value ? "SI" : "NO") : "";

                xrlAmperimetroE.Text = entrega.TieneAmperimetro.HasValue ? (entrega.TieneAmperimetro.Value ? "SI" : "NO") : "";
                xrlVoltimetroE.Text = entrega.TieneVoltimetro.HasValue ? (entrega.TieneVoltimetro.Value ? "SI" : "NO") : "";
                xrlHorometroE.Text = entrega.TieneHorometro.HasValue ? (entrega.TieneHorometro.Value ? "SI" : "NO") : "";
                xrlManometroE.Text = entrega.TieneManometro.HasValue ? (entrega.TieneManometro.Value ? "SI" : "NO") : "";
                xrlInterruptorE.Text = entrega.TieneInterruptor.HasValue ? (entrega.TieneInterruptor.Value ? "SI" : "NO") : "";

                xrlAceiteE.Text = entrega.TieneNivelAceite.HasValue ? (entrega.TieneNivelAceite.Value ? "SI" : "NO") : "";
                xrlAnticongelanteE.Text = entrega.TieneNivelAnticongelante.HasValue ? (entrega.TieneNivelAnticongelante.Value ? "SI" : "NO") : "";

                xrlL1NE.Text = entrega.VoltajeL1N.HasValue ? (entrega.VoltajeL1N.Value).ToString() : "";
                xrlL2NE.Text = entrega.VoltajeL2N.HasValue ? (entrega.VoltajeL2N.Value).ToString() : "";
                xrlL3NE.Text = entrega.VoltajeL3N.HasValue ? (entrega.VoltajeL3N.Value).ToString() : "";
                xrlL1L2E.Text = entrega.VoltajeL1L2.HasValue ? (entrega.VoltajeL1L2.Value).ToString() : "";
                xrlL2L3E.Text = entrega.VoltajeL2L3.HasValue ? (entrega.VoltajeL2L3.Value).ToString() : "";
                xrlL3L1E.Text = entrega.VoltajeL3L1.HasValue ? (entrega.VoltajeL3L1.Value).ToString() : "";

                xrlCablesE.Text = entrega.TieneCables.HasValue ? (entrega.TieneCables.Value ? "SI" : "NO") : "";
                xrlTramosE.Text = entrega.TieneTramos.HasValue ? (entrega.TieneTramos.Value ? "SI" : "NO") : "";
                xrlLineasE.Text = entrega.TieneLineas.HasValue ? (entrega.TieneLineas.Value ? "SI" : "NO") : "";
                xrlCalibresE.Text = entrega.TieneCalibres.HasValue ? (entrega.TieneCalibres.Value ? "SI" : "NO") : "";
                xrlZapatasE.Text = entrega.TieneZapatas.HasValue ? (entrega.TieneZapatas.Value ? "SI" : "NO") : "";

                xrlCantidadE.Text = entrega.BateriaCantidad.HasValue ? (entrega.BateriaCantidad.Value).ToString() : "";
                xrlPlacasE.Text = entrega.BateriaPlacas.HasValue ? (entrega.BateriaPlacas.Value).ToString() : "";
                xrlMarcaE.Text = entrega.BateriaMarca;

                xrlSuspensionE.Text = entrega.Suspension;
                xrlGanchoE.Text = entrega.Gancho;
                xrlGatoNivelacionE.Text = entrega.GatoNivelacion;
                xrlArnesConexionE.Text = entrega.ArnesConexion;

                xrlEje1DE.Text = entrega.TieneEje1LlantaD.HasValue ? (entrega.TieneEje1LlantaD.Value ? "SI" : "NO") : "";
                xrlEje1IE.Text = entrega.TieneEje1LlantaI.HasValue ? (entrega.TieneEje1LlantaI.Value ? "SI" : "NO") : "";
                xrlEje2DE.Text = entrega.TieneEje2LlantaD.HasValue ? (entrega.TieneEje2LlantaD.Value ? "SI" : "NO") : "";
                xrlEje2IE.Text = entrega.TieneEje2LlantaI.HasValue ? (entrega.TieneEje2LlantaI.Value ? "SI" : "NO") : "";
                xrlEje3DE.Text = entrega.TieneEje3LlantaD.HasValue ? (entrega.TieneEje3LlantaD.Value ? "SI" : "NO") : "";
                xrlEje3IE.Text = entrega.TieneEje3LlantaI.HasValue ? (entrega.TieneEje3LlantaI.Value ? "SI" : "NO") : "";
                xrlTapasLluviaDE.Text = entrega.TieneTapaLluviaLlantaD.HasValue ? (entrega.TieneTapaLluviaLlantaD.Value ? "SI" : "NO") : "";
                xrlTapasLluviaIE.Text = entrega.TieneTapaLluviaLlantaI.HasValue ? (entrega.TieneTapaLluviaLlantaI.Value ? "SI" : "NO") : "";

                xrlIzquierdaE.Text = entrega.TieneLamparaIzquierda.HasValue ? (entrega.TieneLamparaIzquierda.Value ? "SI" : "NO") : "";
                xrlDerechaE.Text = entrega.TieneLamparaDerecha.HasValue ? (entrega.TieneLamparaDerecha.Value ? "SI" : "NO") : "";
                xrlSenalSatelitalE.Text = entrega.TieneSenalSatelital.HasValue ? (entrega.TieneSenalSatelital.Value ? "SI" : "NO") : "";
                xrlDiodosE.Text = entrega.TieneDiodos.HasValue ? (entrega.TieneDiodos.Value ? "SI" : "NO") : "";

                #endregion

                #region recepcion

                xrlBandasR.Text = recepcion.TieneBandas.HasValue ? (recepcion.TieneBandas.Value ? "SI" : "NO") : "";
                xrlFiltroAceiteR.Text = recepcion.TieneFiltroAceite.HasValue ? (recepcion.TieneFiltroAceite.Value ? "SI" : "NO") : "";
                xrlFiltroAguaR.Text = recepcion.TieneFiltroAgua.HasValue ? (recepcion.TieneFiltroAgua.Value ? "SI" : "NO") : "";
                xrlFiltroCombustibleR.Text = recepcion.TieneFiltroCombustible.HasValue ? (recepcion.TieneFiltroCombustible.Value ? "SI" : "NO") : "";
                xrlFiltroAireR.Text = recepcion.TieneFiltroAire.HasValue ? (recepcion.TieneFiltroAire.Value ? "SI" : "NO") : "";
                xrlManguerasR.Text = recepcion.TieneMangueras.HasValue ? (recepcion.TieneMangueras.Value ? "SI" : "NO") : "";

                xrlAmperimetroR.Text = recepcion.TieneAmperimetro.HasValue ? (recepcion.TieneAmperimetro.Value ? "SI" : "NO") : "";
                xrlVoltimetroR.Text = recepcion.TieneVoltimetro.HasValue ? (recepcion.TieneVoltimetro.Value ? "SI" : "NO") : "";
                xrlHorometroR.Text = recepcion.TieneHorometro.HasValue ? (recepcion.TieneHorometro.Value ? "SI" : "NO") : "";
                xrlManometroR.Text = recepcion.TieneManometro.HasValue ? (recepcion.TieneManometro.Value ? "SI" : "NO") : "";
                xrlInterruptorR.Text = recepcion.TieneInterruptor.HasValue ? (recepcion.TieneInterruptor.Value ? "SI" : "NO") : "";

                xrlAceiteR.Text = recepcion.TieneNivelAceite.HasValue ? (recepcion.TieneNivelAceite.Value ? "SI" : "NO") : "";
                xrlAnticongelanteR.Text = recepcion.TieneNivelAnticongelante.HasValue ? (recepcion.TieneNivelAnticongelante.Value ? "SI" : "NO") : "";

                xrlL1NR.Text = recepcion.VoltajeL1N.HasValue ? recepcion.VoltajeL1N.Value.ToString() : "";
                xrlL2NR.Text = recepcion.VoltajeL2N.HasValue ? recepcion.VoltajeL2N.Value.ToString() : "";
                xrlL3NR.Text = recepcion.VoltajeL3N.HasValue ? recepcion.VoltajeL3N.Value.ToString() : "";
                xrlL1L2R.Text = recepcion.VoltajeL1L2.HasValue ? recepcion.VoltajeL1L2.Value.ToString() : "";
                xrlL2L3R.Text = recepcion.VoltajeL2L3.HasValue ? recepcion.VoltajeL2L3.Value.ToString() : "";
                xrlL3L1R.Text = recepcion.VoltajeL3L1.HasValue ? recepcion.VoltajeL3L1.Value.ToString() : "";

                xrlCablesR.Text = recepcion.TieneCables.HasValue ? (recepcion.TieneCables.Value ? "SI" : "NO") : "";
                xrlTramosR.Text = recepcion.TieneTramos.HasValue ? (recepcion.TieneTramos.Value ? "SI" : "NO") : "";
                xrlLineasR.Text = recepcion.TieneLineas.HasValue ? (recepcion.TieneLineas.Value ? "SI" : "NO") : "";
                xrlCalibresR.Text = recepcion.TieneCalibres.HasValue ? (recepcion.TieneCalibres.Value ? "SI" : "NO") : "";
                xrlZapatasR.Text = recepcion.TieneZapatas.HasValue ? (recepcion.TieneZapatas.Value ? "SI" : "NO") : "";

                xrlCantidadR.Text = recepcion.BateriaCantidad.HasValue ? recepcion.BateriaCantidad.Value.ToString() : "";
                xrlPlacasR.Text = recepcion.BateriaPlacas.HasValue ? recepcion.BateriaPlacas.Value.ToString() : "";
                xrlMarcaR.Text = recepcion.BateriaMarca;

                xrlSuspensionR.Text = recepcion.Suspension;
                xrlGanchoR.Text = recepcion.Gancho;
                xrlGatoNivelacionR.Text = recepcion.GatoNivelacion;
                xrlArnesConexionR.Text = recepcion.ArnesConexion;

                xrlEje1DR.Text = recepcion.TieneEje1LlantaD.HasValue ? (recepcion.TieneEje1LlantaD.Value ? "SI" : "NO") : "";
                xrlEje1IR.Text = recepcion.TieneEje1LlantaI.HasValue ? (recepcion.TieneEje1LlantaI.Value ? "SI" : "NO") : "";
                xrlEje2DR.Text = recepcion.TieneEje2LlantaD.HasValue ? (recepcion.TieneEje2LlantaD.Value ? "SI" : "NO") : "";
                xrlEje2IR.Text = recepcion.TieneEje2LlantaI.HasValue ? (recepcion.TieneEje2LlantaI.Value ? "SI" : "NO") : "";
                xrlEje3DR.Text = recepcion.TieneEje3LlantaD.HasValue ? (recepcion.TieneEje3LlantaD.Value ? "SI" : "NO") : "";
                xrlEje3IR.Text = recepcion.TieneEje3LlantaI.HasValue ? (recepcion.TieneEje3LlantaI.Value ? "SI" : "NO") : "";
                xrlTapasLluviaDR.Text = recepcion.TieneTapaLluviaLlantaD.HasValue ? (recepcion.TieneTapaLluviaLlantaD.Value ? "SI" : "NO") : "";
                xrlTapasLluviaIR.Text = recepcion.TieneTapaLluviaLlantaI.HasValue ? (recepcion.TieneTapaLluviaLlantaI.Value ? "SI" : "NO") : "";

                xrlIzquierdaR.Text = recepcion.TieneLamparaIzquierda.HasValue ? (recepcion.TieneLamparaIzquierda.Value ? "SI" : "NO") : "";
                xrlDerechaR.Text = recepcion.TieneLamparaDerecha.HasValue ? (recepcion.TieneLamparaDerecha.Value ? "SI" : "NO") : "";
                xrlSenalSatelitalR.Text = recepcion.TieneSenalSatelital.HasValue ? (recepcion.TieneSenalSatelital.Value ? "SI" : "NO") : "";
                xrlDiodosR.Text = recepcion.TieneDiodos.HasValue ? (recepcion.TieneDiodos.Value ? "SI" : "NO") : "";

                #endregion

            } catch (Exception ex) {
                throw new Exception(".ImprimirReporte: Error al imprimir el reporte. " + ex.Message);
            }
        }
    }
}