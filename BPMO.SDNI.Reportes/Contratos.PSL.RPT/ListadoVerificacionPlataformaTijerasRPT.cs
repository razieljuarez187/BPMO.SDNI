using System;
using System.Collections.Generic;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Contratos.BO;
using BPMO.SDNI.Contratos.PSL.BO;
using BPMO.SDNI.Equipos.BO;

namespace BPMO.SDNI.Contratos.PSL.RPT {
    public partial class ListadoVerificacionPlataformaTijerasRPT : DevExpress.XtraReports.UI.XtraReport {

        /// <summary>
        /// Nombre de la clase que será usada en los mensajes de error
        /// </summary>
        private const string nombreClase = "CheckListRORPT";

        public ListadoVerificacionPlataformaTijerasRPT(Dictionary<string, Object> datos) {
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

                var entrega = (ListadoVerificacionPlataformaTijerasBO)datos["Entrega"] ?? new ListadoVerificacionPlataformaTijerasBO();

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
                var recepcion = (ListadoVerificacionPlataformaTijerasBO)datos["Recepcion"] ?? new ListadoVerificacionPlataformaTijerasBO();

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

                #region Fecha y Hora salida
                this.lblFechaEntrega.Text = contrato.FechaInicioArrendamiento.HasValue ? contrato.FechaInicioArrendamiento.Value.ToShortDateString() : string.Empty;
                this.lblFechaRecepcion.Text = recepcion.Fecha.HasValue ? recepcion.Fecha.Value.ToShortDateString() : (contrato.FechaPromesaActual.HasValue ? contrato.FechaPromesaActual.Value.ToShortDateString() : string.Empty);
                #endregion

                #endregion
                #endregion

                #region Cuestionarios
                #region Cuestionario Entrega

                if (entrega.TieneAceiteHidraulico.HasValue) {
                    if (entrega.TieneAceiteHidraulico.Value)
                        chbxAceiteHidraulico.Checked = true;
                    else
                        chbxAceiteHidraulico.Checked = false;
                }

                if (entrega.TieneAceiteMotor.HasValue) {
                    if (entrega.TieneAceiteMotor.Value)
                        chbxAceiteMotor.Checked = true;
                    else
                        chbxAceiteMotor.Checked = false;
                }

                if (entrega.TieneAlmohadillas.HasValue) {
                    if (entrega.TieneAlmohadillas.Value)
                        chbxAlmohadillasDesgasteDeslizantes.Checked = true;
                    else
                        chbxAlmohadillasDesgasteDeslizantes.Checked = false;
                }

                if (entrega.TieneBarrasDireccion.HasValue) {
                    if (entrega.TieneBarrasDireccion.Value)
                        chbxBarrasDireccion.Checked = true;
                    else
                        chbxBarrasDireccion.Checked = false;
                }

                if (entrega.TieneBateria.HasValue) {
                    if (entrega.TieneBateria.Value)
                        chbxBateria.Checked = true;
                    else
                        chbxBateria.Checked = false;
                }

                if (entrega.TieneBrazosTijera.HasValue) {
                    if (entrega.TieneBrazosTijera.Value)
                        chbxBrazosTijera.Checked = true;
                    else
                        chbxBrazosTijera.Checked = false;
                }

                if (entrega.TieneChasis.HasValue) {
                    if (entrega.TieneChasis.Value)
                        chbxChasis.Checked = true;
                    else
                        chbxChasis.Checked = false;
                }

                if (entrega.TieneCilindroDireccion.HasValue) {
                    if (entrega.TieneCilindroDireccion.Value)
                        chbxCilindroDireccion.Checked = true;
                    else
                        chbxCilindroDireccion.Checked = false;
                }

                if (entrega.TieneCilindroElevador.HasValue) {
                    if (entrega.TieneCilindroElevador.Value)
                        chbxCilindroElevador.Checked = true;
                    else
                        chbxCilindroElevador.Checked = false;
                }



                if (entrega.TieneConjuntoBarandillas.HasValue) {
                    if (entrega.TieneConjuntoBarandillas.Value)
                        chbxConjuntoBarandillas.Checked = true;
                    else
                        chbxConjuntoBarandillas.Checked = false;
                }

                if (entrega.TieneConjuntoNeumaticosRuedas.HasValue) {
                    if (entrega.TieneConjuntoNeumaticosRuedas.Value)
                        chbxConjuntoNeumaticosRuedas.Checked = true;
                    else
                        chbxConjuntoNeumaticosRuedas.Checked = false;
                }


                if (entrega.TieneControlesPlataforma.HasValue) {
                    if (entrega.TieneControlesPlataforma.Value)
                        chbxFuncionamientoControlesPlataforma.Checked = true;
                    else
                        chbxFuncionamientoControlesPlataforma.Checked = false;
                }

                if (entrega.TieneControlesTierra.HasValue) {
                    if (entrega.TieneControlesTierra.Value)
                        chbxFuncionamientoControlesTierra.Checked = true;
                    else
                        chbxFuncionamientoControlesTierra.Checked = false;
                }

                if (entrega.TieneFarosEmergencia.HasValue) {
                    if (entrega.TieneFarosEmergencia.Value)
                        chbxParosEmergencia.Checked = true;
                    else
                        chbxParosEmergencia.Checked = false;
                }
                if (entrega.TienePasadoresPivote.HasValue) {
                    if (entrega.TienePasadoresPivote.Value)
                        chbxPasadoresPivoteTijera.Checked = true;
                    else
                        chbxPasadoresPivoteTijera.Checked = false;
                }

                if (entrega.TienePivotesDireccion.HasValue) {
                    if (entrega.TienePivotesDireccion.Value)
                        chbxPivotesDireccion.Checked = true;
                    else
                        chbxPivotesDireccion.Checked = false;
                }

                if (entrega.TienePlataforma.HasValue) {
                    if (entrega.TienePlataforma.Value)
                        chbxPlataforma.Checked = true;
                    else
                        chbxPlataforma.Checked = false;
                }

                if (entrega.TienePruebaSwitchPothole.HasValue) {
                    if (entrega.TienePruebaSwitchPothole.Value)
                        chbxPruebaSwichesLimitePothole.Checked = true;
                    else
                        chbxPruebaSwichesLimitePothole.Checked = false;
                }

                if (entrega.TieneReductoresTransito.HasValue) {
                    if (entrega.TieneReductoresTransito.Value)
                        chbxReductoresTransito.Checked = true;
                    else
                        chbxReductoresTransito.Checked = false;
                }

                if (entrega.TieneRefrigerante.HasValue) {
                    if (entrega.TieneRefrigerante.Value)
                        chbxRefrigerante.Checked = true;
                    else
                        chbxRefrigerante.Checked = false;
                }

                if (entrega.TieneVelocidadTransitoExtendida.HasValue) {
                    if (entrega.TieneVelocidadTransitoExtendida.Value)
                        chbxTransitoPlataformaExtendida.Checked = true;
                    else
                        chbxTransitoPlataformaExtendida.Checked = false;
                }

                if (entrega.TieneVelocidadTransitoRetraida.HasValue) {
                    if (entrega.TieneVelocidadTransitoRetraida.Value)
                        chbxVelocidadTransitoPlataformaRetraida.Checked = true;
                    else
                        chbxVelocidadTransitoPlataformaRetraida.Checked = false;
                }

                #endregion

                #region Cuestionario Recepción

                if (recepcion.TieneAceiteHidraulico.HasValue) {
                    if (recepcion.TieneAceiteHidraulico.Value)
                        chbxAceiteHidraulicoRecepcion.Checked = true;
                    else
                        chbxAceiteHidraulicoRecepcion.Checked = false;
                }

                if (recepcion.TieneAceiteMotor.HasValue) {
                    if (recepcion.TieneAceiteMotor.Value)
                        chbxAceiteMotorRecepcion.Checked = true;
                    else
                        chbxAceiteMotorRecepcion.Checked = false;
                }

                if (recepcion.TieneAlmohadillas.HasValue) {
                    if (recepcion.TieneAlmohadillas.Value)
                        chbxAlmohadillasDesgasteDeslizantesRecepcion.Checked = true;
                    else
                        chbxAlmohadillasDesgasteDeslizantesRecepcion.Checked = false;
                }

                if (recepcion.TieneBarrasDireccion.HasValue) {
                    if (recepcion.TieneBarrasDireccion.Value)
                        chbxBarrasDireccionRecepcion.Checked = true;
                    else
                        chbxBarrasDireccionRecepcion.Checked = false;
                }

                if (recepcion.TieneBateria.HasValue) {
                    if (recepcion.TieneBateria.Value)
                        chbxBateriaRecepcion.Checked = true;
                    else
                        chbxBateriaRecepcion.Checked = false;
                }

                if (recepcion.TieneBrazosTijera.HasValue) {
                    if (recepcion.TieneBrazosTijera.Value)
                        chbxBrazosTijeraRecepcion.Checked = true;
                    else
                        chbxBrazosTijeraRecepcion.Checked = false;
                }

                if (recepcion.TieneChasis.HasValue) {
                    if (recepcion.TieneChasis.Value)
                        chbxChasisRecepcion.Checked = true;
                    else
                        chbxChasisRecepcion.Checked = false;
                }

                if (recepcion.TieneCilindroDireccion.HasValue) {
                    if (recepcion.TieneCilindroDireccion.Value)
                        chbxCilindroDireccionRecepcion.Checked = true;
                    else
                        chbxCilindroDireccionRecepcion.Checked = false;
                }

                if (recepcion.TieneCilindroElevador.HasValue) {
                    if (recepcion.TieneCilindroElevador.Value)
                        chbxCilindroElevadorRecepcion.Checked = true;
                    else
                        chbxCilindroElevadorRecepcion.Checked = false;
                }



                if (recepcion.TieneConjuntoBarandillas.HasValue) {
                    if (recepcion.TieneConjuntoBarandillas.Value)
                        chbxConjuntoBarandillasRecepcion.Checked = true;
                    else
                        chbxConjuntoBarandillasRecepcion.Checked = false;
                }

                if (recepcion.TieneConjuntoNeumaticosRuedas.HasValue) {
                    if (recepcion.TieneConjuntoNeumaticosRuedas.Value)
                        chbxConjuntoNeumaticosRuedasRecepcion.Checked = true;
                    else
                        chbxConjuntoNeumaticosRuedasRecepcion.Checked = false;
                }


                if (recepcion.TieneControlesPlataforma.HasValue) {
                    if (recepcion.TieneControlesPlataforma.Value)
                        chbxFuncionamientoControlesPlataformaRecepcion.Checked = true;
                    else
                        chbxFuncionamientoControlesPlataformaRecepcion.Checked = false;
                }

                if (recepcion.TieneControlesTierra.HasValue) {
                    if (recepcion.TieneControlesTierra.Value)
                        chbxFuncionamientoControlesTierraRecepcion.Checked = true;
                    else
                        chbxFuncionamientoControlesTierraRecepcion.Checked = false;
                }

                if (recepcion.TieneFarosEmergencia.HasValue) {
                    if (recepcion.TieneFarosEmergencia.Value)
                        chbxParosEmergenciaRecepcion.Checked = true;
                    else
                        chbxParosEmergenciaRecepcion.Checked = false;
                }
                if (recepcion.TienePasadoresPivote.HasValue) {
                    if (recepcion.TienePasadoresPivote.Value)
                        chbxPasadoresPivoteTijeraRecepcion.Checked = true;
                    else
                        chbxPasadoresPivoteTijeraRecepcion.Checked = false;
                }

                if (recepcion.TienePivotesDireccion.HasValue) {
                    if (recepcion.TienePivotesDireccion.Value)
                        chbxPivotesDireccionRecepcion.Checked = true;
                    else
                        chbxPivotesDireccionRecepcion.Checked = false;
                }

                if (recepcion.TienePlataforma.HasValue) {
                    if (recepcion.TienePlataforma.Value)
                        chbxPlataformaRecepcion.Checked = true;
                    else
                        chbxPlataformaRecepcion.Checked = false;
                }

                if (recepcion.TienePruebaSwitchPothole.HasValue) {
                    if (recepcion.TienePruebaSwitchPothole.Value)
                        chbxPruebaSwitchesLimitePotholeRecepcion.Checked = true;
                    else
                        chbxPruebaSwitchesLimitePotholeRecepcion.Checked = false;
                }

                if (recepcion.TieneReductoresTransito.HasValue) {
                    if (recepcion.TieneReductoresTransito.Value)
                        chbxReductoresTransitoRecepcion.Checked = true;
                    else
                        chbxReductoresTransitoRecepcion.Checked = false;
                }

                if (recepcion.TieneRefrigerante.HasValue) {
                    if (recepcion.TieneRefrigerante.Value)
                        chbxRefrigeranteRecepcion.Checked = true;
                    else
                        chbxRefrigeranteRecepcion.Checked = false;
                }

                if (recepcion.TieneVelocidadTransitoExtendida.HasValue) {
                    if (recepcion.TieneVelocidadTransitoExtendida.Value)
                        chbxVelocidadTransitoPlataformaExtendidaRecepcion.Checked = true;
                    else
                        chbxVelocidadTransitoPlataformaExtendidaRecepcion.Checked = false;

                }

                if (recepcion.TieneVelocidadTransitoRetraida.HasValue) {
                    if (recepcion.TieneVelocidadTransitoRetraida.Value)
                        chbxVelocidadTransitoPlataformaRetraidaRecepcion.Checked = true;
                    else
                        chbxVelocidadTransitoPlataformaRetraidaRecepcion.Checked = false;
                }

                #endregion
                #endregion
            } catch (Exception ex) {
                throw new Exception(".ImprimirReporte: Error al imprimir el reporte. " + ex.Message);
            }
        }

    }
}