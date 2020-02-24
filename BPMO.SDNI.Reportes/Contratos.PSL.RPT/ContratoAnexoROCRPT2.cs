using System;
using System.Collections.Generic;
using System.Linq;
using BPMO.Basicos.BO;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Contratos.BO;
using BPMO.SDNI.Contratos.PSL.BO;
namespace BPMO.SDNI.Contratos.PSL.RPT {
    public partial class ContratoAnexoROCRPT2 : DevExpress.XtraReports.UI.XtraReport {

        /// <summary>
        /// Nombre de la clase que será usada en los mensajes de error
        /// </summary>
        private const string nombreClase = "ContratoRORPT";

        public ContratoAnexoROCRPT2(Dictionary<string, Object> datos) {
            InitializeComponent();

            this.ImprimirReporte(datos);
        }

        private void ImprimirReporte(Dictionary<string, object> datos) {
            try {
                var contrato = (ContratoPSLBO)datos["ContratoPSLBO"];

                #region Carta Depositaria
                string obligadosolidario = string.Empty;
                AvalBO avalbo = contrato.Avales != null && contrato.Avales.Count > 0 ? contrato.Avales.ConvertAll(s => (AvalBO)s)[0] : null;
                if (avalbo != null)
                    obligadosolidario = avalbo.Nombre;

                #region Información del Depositario
                this.xrNombreApellidosDepositario.Text = "";
                this.xrDomicilioDepositario.Text = "";
                var depositario = (contrato.RepresentantesLegales.ConvertAll(a => (RepresentanteLegalBO)a)).Where(x => x.EsDepositario.GetValueOrDefault() == true).FirstOrDefault();
                if (depositario == null) depositario = new RepresentanteLegalBO();
                if (depositario != null)
                {
                    this.xrNombreApellidosDepositario.Text = depositario.Nombre;
                    this.xrDomicilioDepositario.Text = depositario.Direccion;
                }
                #endregion


                this.xrAnexoCartaDepositaria.Text = "Anexo que se suscribe por EL DEPOSITARIO y que forma parte integrante del “CONTRATO DE ARRENDAMIENTO DE BIENES MUEBLES” suscrito entre LA ARRENDADORA " + contrato.Sucursal.UnidadOperativa.Empresa.Nombre +
                    ", EL ARRENDATARIO " + contrato.Cliente.Nombre + ", el OBLIGADO SOLIDARIO Y AVAL " + obligadosolidario + " y el DEPOSITARIO " + depositario.Nombre ?? string.Empty + ".";

                this.xrtDiaCartaDepositaria.Text = "";
                this.xrtMesCartaDepositaria.Text = "";
                this.xrtAnioCartaDepositaria.Text = "";
                if (contrato.FechaInicioActual != null) {
                    this.xrtDiaCartaDepositaria.Text = contrato.FechaInicioActual.Value.Day.ToString();
                    this.xrtMesCartaDepositaria.Text = contrato.FechaInicioActual.Value.Month.ToString();
                    this.xrtAnioCartaDepositaria.Text = contrato.FechaInicioActual.Value.Year.ToString();
                }
                

                List<LineaContratoPSLBO> listaContrato = contrato.LineasContrato.ConvertAll(s => (LineaContratoPSLBO)s);
                string listaunidades = string.Empty;
                foreach (LineaContratoPSLBO lineascnt in listaContrato) {
                    if (listaunidades != "")
                        listaunidades = listaunidades + ", ";
                    if (lineascnt.Equipo != null && lineascnt.Equipo.TipoEquipoServicio != null && lineascnt.Equipo.Modelo != null)
                        listaunidades += lineascnt.Equipo.TipoEquipoServicio.Nombre + "- " + lineascnt.Equipo.Modelo.Nombre;
                }
                this.xrDescripcionBienesMueblesCartaDepositaria.Text = listaunidades;

                this.xrUbicacionEntregaBienCartaDepositaria.Text = contrato.DestinoAreaOperacion;

                #region[Dirección cliente]
                List<DireccionClienteBO> listaclienteinfo = contrato.Cliente.Direcciones.ConvertAll(s => (DireccionClienteBO)s);
                DireccionClienteBO clienteinfo = new DireccionClienteBO();
                foreach (DireccionClienteBO dircliente in listaclienteinfo) {
                    if (dircliente.Primaria.HasValue && dircliente.Primaria.Value)
                        clienteinfo = dircliente;
                }

                string municipiocliente = string.Empty;
                string estadocliente = string.Empty;
                if (clienteinfo != null && clienteinfo.Ubicacion != null && clienteinfo.Ubicacion.Estado != null)
                    estadocliente = clienteinfo.Ubicacion.Estado.Codigo;
                if (clienteinfo != null && clienteinfo.Ubicacion != null && clienteinfo.Ubicacion.Municipio != null)
                    municipiocliente = clienteinfo.Ubicacion.Municipio.Codigo;
                #endregion

                string direccionsucursal = string.Empty;
                if (contrato.Sucursal != null && contrato.Sucursal.DireccionesSucursal != null && contrato.Sucursal.DireccionesSucursal.Count > 0)
                    direccionsucursal = contrato.Sucursal.DireccionesSucursal[0].Nombre;
                this.xrSuscritoCartaDepositaria.Text = "Suscrito en el municipio de " + municipiocliente + ", Estado de " + estadocliente + ", México; en la fecha de " + ((DateTime)contrato.FechaInicioActual).ToShortDateString() + ".";
                this.xrNombreDepostarioFirma.Text = string.Format("{0}{1}", "NOMBRE: ", depositario.Nombre);
                #endregion
            } catch (Exception ex) {
                throw new Exception(".ImprimirReporte: Error al imprimir el reporte. " + ex.Message);
            }
        }


    }
}
