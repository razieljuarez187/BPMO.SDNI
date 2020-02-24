using System;
using System.Collections.Generic;
using System.Linq;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Contratos.BO;
using BPMO.SDNI.Contratos.PSL.BO;
using BPMO.SDNI.Equipos.BO;

namespace BPMO.SDNI.Contratos.PSL.RPT {
    public partial class ContratoAnexoROCRPT : DevExpress.XtraReports.UI.XtraReport {
        /// <summary>
        /// Nombre de la clase que será usada en los mensajes de error
        /// </summary>
        private const string nombreClase = "ContratoAnexoROCRPT";
        private string Nombre = "<b>NOMBRE: </b>";
        private string Nombre_Razon = "<b>NOMBRE O RAZÓN SOCIAL: </b>";
        private string Representada = "<b>REPRESENTADA POR: </b>";
        //
        public ContratoAnexoROCRPT(Dictionary<string, Object> datos) {
            InitializeComponent();

            this.ImprimirReporte(datos);
        }

        /// <summary>
        /// Genera el reporte para el check list
        /// </summary>
        /// <param name="datos">Datos del check list</param>
        private void ImprimirReporte(Dictionary<string, object> datos) {
            try {
                var contrato = (ContratoPSLBO)datos["ContratoPSLBO"];
                if (contrato == null)
                    throw new Exception("No se encontró información para imprimir el contrato");

                #region #Anexo
                int numeroAnexo = 1;
                if (datos.ContainsKey("NumeroAnexo")) {
                    numeroAnexo = (int)datos["NumeroAnexo"];
                }
                this.xrlblAnexo.Text = "ANEXO " + GetRomanNumber(numeroAnexo);
                #endregion

                #region Fecha
                this.xrtDia.Text = "";
                this.xrtMes.Text = "";
                this.xrtAnio.Text = "";
                if (contrato.FechaInicioActual != null) {
                    this.xrtDia.Text = contrato.FechaInicioActual.Value.Day.ToString();
                    this.xrtMes.Text = contrato.FechaInicioActual.Value.Month.ToString();
                    this.xrtAnio.Text = contrato.FechaInicioActual.Value.Year.ToString();
                }
                #endregion

                #region Obtener Porcentaje de Impuesto 
                decimal porcentajeImp = 0;
                if (contrato.Sucursal != null && contrato.Sucursal.Impuesto != null && contrato.Sucursal.Impuesto.PorcentajeImpuesto != null)
                {
                    porcentajeImp = (decimal)contrato.Sucursal.Impuesto.PorcentajeImpuesto / 100;
                }
                #endregion

                #region Datos de identificación
                List<LineaContratoPSLBO> listaContrato = contrato.LineasContrato.ConvertAll(s => (LineaContratoPSLBO)s);
                ConfiguracionUnidadOperativaBO configEmpresa = datos.ContainsKey("RepresentanteEmpresa") ? (ConfiguracionUnidadOperativaBO)datos["RepresentanteEmpresa"] : new ConfiguracionUnidadOperativaBO() ;
                decimal? importeCadaRenta = null;
                List<string> listaunidades = new List<string>();
                decimal? montoTotalContrato = 0;
                decimal? montoTotalUnidad = 0;
                decimal? importeSeguro = 0;
                #region PeriodoTarifario
                BPMO.Basicos.BO.MonedaBO moneda = new BPMO.Basicos.BO.MonedaBO();
                if (datos.ContainsKey("Moneda"))
                    moneda = (BPMO.Basicos.BO.MonedaBO)datos["Moneda"];
                decimal? tarifaCalculada = 0;
                DiaPeriodoTarifaBO periodoTarifa = new DiaPeriodoTarifaBO();
                if (datos.ContainsKey("PeriodoTarifa"))
                    periodoTarifa = (DiaPeriodoTarifaBO)datos["PeriodoTarifa"];
                int? iniSemana = periodoTarifa.InicioPeriodoSemana;
                int? iniMes = periodoTarifa.InicioPeriodoMes;
                int diasRenta = ((int)contrato.DiasRenta(true));
                #endregion
                foreach (LineaContratoPSLBO lineascnt in listaContrato.Where(lc => lc.Activo == true))
                {
                    if (lineascnt.Equipo != null && lineascnt.Equipo.TipoEquipoServicio != null && lineascnt.Equipo.Modelo != null)
                        listaunidades.Add(lineascnt.Equipo.TipoEquipoServicio.Nombre + "- " + lineascnt.Equipo.Modelo.Nombre);

                    if (((TarifaContratoPSLBO)lineascnt.Cobrable).TarifaCobradaEnPago != null)
                    importeCadaRenta = ((TarifaContratoPSLBO)lineascnt.Cobrable).TarifaCobradaEnPago.GetValueOrDefault();

                    #region Tarifa 
                    if (((TarifaContratoPSLBO)lineascnt.Cobrable).TarifaConDescuento != null && ((TarifaContratoPSLBO)lineascnt.Cobrable).TarifaConDescuento > 0 )
                        tarifaCalculada = ((TarifaContratoPSLBO)lineascnt.Cobrable).TarifaConDescuento;
                    else
                        tarifaCalculada = ((TarifaContratoPSLBO)lineascnt.Cobrable).Tarifa != null ? ((TarifaContratoPSLBO)lineascnt.Cobrable).Tarifa : 0;
                    if (tarifaCalculada > 0)
                        switch (((TarifaContratoPSLBO)lineascnt.Cobrable).PeriodoTarifa) {
                            case EPeriodosTarifa.Dia:
                                montoTotalUnidad = Math.Round((decimal)(tarifaCalculada * diasRenta), 2);
                                break;
                            case EPeriodosTarifa.Semana:
                                montoTotalUnidad = Math.Round((decimal)(tarifaCalculada / periodoTarifa.DiasDuracionSemana * diasRenta), 2);
                                break;
                            case EPeriodosTarifa.Mes:
                                montoTotalUnidad = Math.Round((decimal)(tarifaCalculada / periodoTarifa.DiasDuracionMes * diasRenta), 2);
                                break;
                        }
                    importeSeguro = (montoTotalUnidad * (((TarifaContratoPSLBO)lineascnt.Cobrable).PorcentajeSeguro != null ? ((TarifaContratoPSLBO)lineascnt.Cobrable).PorcentajeSeguro : 0) / 100);
                    decimal? subTotalUnidad = montoTotalUnidad + ((TarifaContratoPSLBO)lineascnt.Cobrable).Maniobra.GetValueOrDefault() + importeSeguro;
                    montoTotalContrato += subTotalUnidad;
                    #endregion
                }

                this.lblValorBien.Text = "";
                this.xrListadoUnidades.Text = "";

                if (listaunidades.Count > 0)
                    this.xrListadoUnidades.Text = string.Join("\n", listaunidades);
                
                this.lblUsoBien.Text = contrato.MercanciaTransportar;
                this.lblUbicacionEntrega.Text = contrato.DestinoAreaOperacion;

                var lineacontrato = contrato.LineasContrato.ConvertAll(s => (LineaContratoPSLBO)s);
                this.lblArrendador.Text = string.Empty;
                this.xrArrendador.Text = MontoArrendamiento(listaContrato) ? "X" : string.Empty;
                this.xrArrendatario.Text = this.xrArrendador.Text == string.Empty ? "X" : string.Empty;
                this.lblArrendatario.Text = string.Empty;

                this.xrArrendadoraRepPor.Text = "";
                this.xrArrendatarioRepPor.Text = "";
                this.xrObligadoRepPor.Text = "";

                if (configEmpresa != null)
                    this.xrArrendadoraRepPor.Text += "REPRESENTADA POR: " + configEmpresa.Representante;

                if (contrato.RepresentantesLegales.Count > 0) {
                    RepresentanteLegalBO RepresentantesLegales = contrato.RepresentantesLegales.ConvertAll(s => (RepresentanteLegalBO)s).FirstOrDefault();
                    if (RepresentantesLegales != null) {
                        this.xrArrendatarioRepPor.Text = "REPRESENTADA POR: " + RepresentantesLegales.Nombre;
                        this.xrObligadoRepPor.Text = "REPRESENTADA POR: " + RepresentantesLegales.Nombre;
                    }
                }
                #endregion
                
                #region Monto de Contrato
                montoTotalContrato = (montoTotalContrato != null ? montoTotalContrato : 0);
                string montoLetras = new BPMO.SDNI.Comun.BR.ConvertirALetrasBR().ConvertirMoneda((decimal)montoTotalContrato, moneda.ComplementoNombreLegal, moneda.NombreLegal);

                this.lblMontoTotalContrato.Text = "$" + Convert.ToDouble(montoTotalContrato).ToString("N2") + " \r\n (" + montoLetras.ToUpper() + ") ";
                    
                this.lblFechaInicioTermino.Text = "INICIO: "+ (contrato.FechaInicioActual != null ? contrato.FechaInicioActual.Value.ToShortDateString() : "") + " \r\n" + "TERMINACIÓN: " +
                    (contrato.FechaPromesaActual != null ? ((DateTime)contrato.FechaPromesaActual).ToShortDateString() : "") + " \r\n" 
                    + "(En caso de ser indefinida la fecha de terminación. será la fecha de devolución del BIEN, haciendo constar mediante la firma de la carta check list de recepción)";
               
                #endregion

                #region Firmas
                List<AvalBO> listaAval = contrato.Avales.ConvertAll(s => (AvalBO)s);
                var depositario = (contrato.RepresentantesLegales.ConvertAll(s => (RepresentanteLegalBO)s)).Where(x => x.EsDepositario.GetValueOrDefault() == true).FirstOrDefault();
                this.xrlNombreEmpresa.Text = contrato.Sucursal.UnidadOperativa.Empresa.Nombre;
                this.xrRichArrendatario.Html = "<div style=\"text-align: center;\"><a style=\"font-family:Times New Roman; font-size:10px;\">" + Nombre_Razon + contrato.Cliente.Nombre + "</a></div>";
                this.xrRichObligado.Html = "<div style=\"text-align: center;\"><a style=\"font-family:Times New Roman; font-size:10px;\">" + Nombre_Razon + ((listaAval != null && listaAval.Count > 0) ? listaAval[0].Nombre : "") + "</a></div>";
                
                this.xrRichDepositario.Html = "<div style=\"text-align: center;\"><a style=\"font-family:Times New Roman; font-size:10px;\">" + Nombre + (depositario != null ? depositario.Nombre : "") + " , por su propio y personal derecho." + "</a></div>";
                #endregion

            } catch (Exception ex) {
                throw new Exception(".ImprimirReporte: Error al imprimir el reporte. " + ex.Message);
            }
        }

        /// <summary>
        /// Indica si existe un monto de maniobra para las unidades
        /// </summary>
        /// <param name="lineasContrato">Lineas del contrato</param>
        /// <returns>booleano que indica si tiene o no monto arrendamiento</returns>
        private bool MontoArrendamiento(List<LineaContratoPSLBO> lineasContrato) {
            foreach (LineaContratoPSLBO lineacontrato in lineasContrato) {
                if (lineacontrato.Activo != null && lineacontrato.Activo.Value == true) {
                    if (lineacontrato.Cobrable != null) {
                        if (((TarifaContratoPSLBO)lineacontrato.Cobrable).Maniobra.HasValue)
                            return true;
                    }
                }
            }
            return false;
        }

        private void xrLabel13_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e) {

        }
        private string Roman(int Numero, int nt) {
            string cd = "";
            switch (Numero) {
                case 0: break;
                case 1:
                    switch (nt) {
                        case 1:
                            cd = "I";
                            break;
                        case 2:
                            cd = "X";
                            break;
                        case 3:
                            cd = "C";
                            break;
                        case 4:
                            cd = "M";
                            break;
                        case 5:
                            cd = cd.PadRight(10, Convert.ToChar("M"));
                            break;
                    }
                    break;
                case 2:
                    switch (nt) {
                        case 1:
                            cd = "II";
                            break;
                        case 2:
                            cd = "XX";
                            break;
                        case 3:
                            cd = "CC";
                            break;
                        case 4:
                            cd = "MM";
                            break;
                        case 5:
                            cd = cd.PadRight(20, Convert.ToChar("M"));
                            break;
                    }
                    break;
                case 3:
                    switch (nt) {
                        case 1:
                            cd = "III";
                            break;
                        case 2:
                            cd = "XXX";
                            break;
                        case 3:
                            cd = "CCC";
                            break;
                        case 4:
                            cd = "MMM";
                            break;
                        case 5:
                            cd = cd.PadRight(30, Convert.ToChar("M"));
                            break;
                    }
                    break;
                case 4:
                    switch (nt) {
                        case 1:
                            cd = "IV";
                            break;
                        case 2:
                            cd = "LX";
                            break;
                        case 3:
                            cd = "CD";
                            break;
                        case 4:
                            cd = "MMMM";
                            break;
                        case 5:
                            cd = cd.PadRight(40, Convert.ToChar("M"));
                            break;
                    }
                    break;
                case 5:
                    switch (nt) {
                        case 1:
                            cd = "V";
                            break;
                        case 2:
                            cd = "X";
                            break;
                        case 3:
                            cd = "D";
                            break;
                        case 4:
                            cd = "MMMMM";
                            break;
                        case 5:
                            cd = cd.PadRight(50, Convert.ToChar("M"));
                            break;
                    }
                    break;
                case 6:
                    switch (nt) {
                        case 1:
                            cd = "VI";
                            break;
                        case 2:
                            cd = "LX";
                            break;
                        case 3:
                            cd = "DC";
                            break;
                        case 4:
                            cd = "MMMMMM";
                            break;
                        case 5:
                            cd = cd.PadRight(60, Convert.ToChar("M"));
                            break;
                    }
                    break;
                case 7:
                    switch (nt) {
                        case 1:
                            cd = "VII";
                            break;
                        case 2:
                            cd = "LXX";
                            break;
                        case 3:
                            cd = "DCC";
                            break;
                        case 4:
                            cd = "MMMMMMM";
                            break;
                        case 5:
                            cd = cd.PadRight(70, Convert.ToChar("M"));
                            break;
                    }
                    break;
                case 8:
                    switch (nt) {
                        case 1:
                            cd = "VIII";
                            break;
                        case 2:
                            cd = "LXXX";
                            break;
                        case 3:
                            cd = "DCCC";
                            break;
                        case 4:
                            cd = "MMMMMMMM";
                            break;
                        case 5:
                            cd = cd.PadRight(80, Convert.ToChar("M"));
                            break;
                    }
                    break;
                case 9:
                    switch (nt) {
                        case 1:
                            cd = "IX";
                            break;
                        case 2:
                            cd = "XC";
                            break;
                        case 3:
                            cd = "CM";
                            break;
                        case 4:
                            cd = "MMMMMMMMM";
                            break;
                        case 5:
                            cd = cd.PadRight(90, Convert.ToChar("M"));
                            break;
                    }
                    break;

            }


            return cd;
        }

        private string GetRomanNumber(int Number) {
            int n1, x, ln;
            string stri, cd, dgt;
            cd = Convert.ToString(Number);
            ln = cd.Length; stri = "";
            x = 0;
            while (x < ln) {
                dgt = cd.Substring(x, 1);
                n1 = Convert.ToInt32(dgt);
                stri = stri + Roman(n1, ln - x);
                x++;
            }
            return stri;
        }

        private void xrPageInfo1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e) {

        }
    }
}
