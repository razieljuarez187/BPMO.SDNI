using System;
using System.Collections.Generic;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Contratos.BO;
using BPMO.SDNI.Contratos.PSL.BO;

namespace BPMO.SDNI.Contratos.PSL.RPT {
    public partial class ContratoROCRPT : DevExpress.XtraReports.UI.XtraReport {
        /// <summary>
        /// Nombre de la clase que será usada en los mensajes de error
        /// </summary>
        private const string nombreClase = "CheckListRORPT";

        public ContratoROCRPT(Dictionary<string, Object> datos) {
            InitializeComponent();

            this.ImprimirReporte(datos);
        }


        private void ImprimirReporte(Dictionary<string, object> datos) {
            try {
                var contrato = (ContratoPSLBO)datos["ContratoPSLBO"];

                string obligadoSolidario = null;
                if (contrato.RepresentantesLegales.Count > 0) {
                    RepresentanteLegalBO RepresentantesLegales = contrato.RepresentantesLegales.ConvertAll(s => (RepresentanteLegalBO)s)[0];
                    if (RepresentantesLegales != null) {

                        //Cuando todos los representantes legales son avales se 
                        if (contrato.SoloRepresentantes.GetValueOrDefault() == true) {
                            obligadoSolidario = RepresentantesLegales.Nombre;

                        }
                    }
                }

                AvalBO avalbo = contrato.Avales != null && contrato.Avales.Count > 0 ? contrato.Avales.ConvertAll(s => (AvalBO)s)[0] : null;
                if (avalbo != null) {
                    obligadoSolidario = avalbo.Nombre;

                }

                string depositario = null;
                List<RepresentanteLegalBO> listaPersonaDepositario = contrato.RepresentantesLegales.ConvertAll(a => (RepresentanteLegalBO)a);
                foreach (RepresentanteLegalBO personadepositario in listaPersonaDepositario) {
                    if (personadepositario.EsDepositario.HasValue && personadepositario.EsDepositario.Value) {
                        depositario = personadepositario.Nombre;

                        break;
                    }
                }

                #region encabezado
                string barraAux = "                   ";
                string parrafo1 = "<p style='font-size:12px; text-align:justify; font-family: 'Times New Roman', Times, serif;'><b>ADDENDUM</b> INTEGRANTE DEL “CONTRATO DE ARRENDAMIENTO DE BIENES MUEBLES” QUE SE SUSCRIBIÓ ENTRE LA ARRENDADORA <u> ";
                parrafo1 += " " + contrato.Sucursal.UnidadOperativa.Empresa.Nombre + " </u> EL ARRENDATARIO <u> " + contrato.Cliente.Nombre + " </u> EL OBLIGADO SOLIDARIO Y AVAL <u> " + (obligadoSolidario != null ? obligadoSolidario : barraAux);
                parrafo1 += " </u> Y EL DEPOSITARIO <u> " + (depositario != null ? depositario : "") + " </u>, EN FECHA:</p>";

                this.xrlParrafo1.Html = parrafo1;

                #endregion
                #region Fecha
                this.xrtDia.Text = contrato.FechaContrato.Value != null ? contrato.FechaContrato.Value.Day.ToString() : "";
                this.xrtMes.Text = contrato.FechaContrato.Value != null ? contrato.FechaContrato.Value.Month.ToString() : "";
                this.xrtAnio.Text = contrato.FechaContrato.Value != null ? contrato.FechaContrato.Value.Year.ToString() : "";
                #endregion
                #region Bienes

                string listaUnidades = string.Empty;
                for (int i = 0; i < contrato.LineasContrato.Count; i++) {
                    if (i == contrato.LineasContrato.Count - 1) {
                        if (contrato.LineasContrato[i].Equipo != null && contrato.LineasContrato[i].Equipo.TipoEquipoServicio != null && contrato.LineasContrato[i].Equipo.Modelo != null) {
                            listaUnidades += contrato.LineasContrato[i].Equipo.TipoEquipoServicio.Nombre + "- " + contrato.LineasContrato[i].Equipo.Modelo.Nombre + ".";
                        }
                    } else {
                        if (contrato.LineasContrato[i].Equipo != null && contrato.LineasContrato[i].Equipo.TipoEquipoServicio != null && contrato.LineasContrato[i].Equipo.Modelo != null) {
                            listaUnidades += contrato.LineasContrato[i].Equipo.TipoEquipoServicio.Nombre + "- " + contrato.LineasContrato[i].Equipo.Modelo.Nombre + ", ";
                        }
                    }
                }

                this.xrlDescripcionBienes.Text = listaUnidades;
                #endregion
                #region tabla
                this.xrlFechaElaboracion.Text = this.convierteAfechaSinHora(contrato.FechaContrato);

                this.xrlMontoTotalArrendamientoMasImpuestoValorAgregado.Text = "";
                this.xrlFechaInicio.Text = this.convierteAfechaSinHora(contrato.FechaInicioActual);
                this.xrlVau.Text = "";

                this.xrlFechaPagoRenta.Text = "";
                this.xrlPlazo.Text = "";
                this.xrlFechaTerminacion.Text = this.convierteAfechaSinHora(contrato.FechaPromesaActual);
                this.xrlInverionInicial.Text = "";
                #endregion

                #region Firmas
                string empresaFirma = "<p margin:0px; style='text-align: center; font-size:11px; font-family: 'Times New Roman', Times, serif;'>" + contrato.Sucursal.UnidadOperativa.Empresa.Nombre + " , S.A. DE C.V. </p>";
                string obligadoSolidarioFirma = "<p  margin:0px; style='text-align: center; font-size:11px; font-family: 'Times New Roman', Times, serif;'> <b>NOMBRE O RAZÓN SOCIAL :</b> " + (obligadoSolidario != null ? obligadoSolidario : "") + " </p>";
                string depositarioFirma = "<p margin:0px; style='text-align: center; font-size:11px; font-family: 'Times New Roman', Times, serif;'> <b>NOMBRE :</b> " + (depositario != null ? depositario : "") + ", por su propio y personal derecho</p>";
                string cliente = "<p margin:0px; style='text-align: center; font-size:11px; font-family: 'Times New Roman', Times, serif;'> <b>NOMBRE O RAZÓN SOCIAL :</b> " + contrato.Cliente.Nombre + " </p>";

                this.xrrtNombreEmpresaFirma.Html = empresaFirma;
                this.xrrtObligadoSolidarioFirma.Html = obligadoSolidarioFirma;
                this.xrrtArrendatarioFirma.Html = cliente;
                this.xrrtDepositarioFirma.Html = depositarioFirma;


                #endregion
            } catch (Exception e) {

            }
        }


        /// <summary>
        /// Convierte una fecha en formato estándar a formato estándar sin hora
        /// </summary>
        /// <param name="fechaConHora">Una Fecha Con Hora</param>
        /// <returns>Fecha Sin Hora</returns>
        private string convierteAfechaSinHora(DateTime? fechaConHora) {
            DateTime FechaADateTime = Convert.ToDateTime(fechaConHora);
            string fechaContrato = string.Format(FechaADateTime.ToShortDateString(), "dd/mm/aaaa");
            return fechaContrato;
        }
    }
}