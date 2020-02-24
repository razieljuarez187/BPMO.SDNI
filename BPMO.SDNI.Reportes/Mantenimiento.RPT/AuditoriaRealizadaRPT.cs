//Satisfce al CU020 - Imprimir Auditoria Realizada

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Collections.Generic;

namespace BPMO.SDNI.Mantenimiento.RPT
{
    public partial class AuditoriaRealizadaRPT : DevExpress.XtraReports.UI.XtraReport
    {
        public AuditoriaRealizadaRPT(Dictionary<String, Object> datosReporte)
        {
            InitializeComponent();
            this.ImprimirReporte(datosReporte);
        }

        /// <summary>
        /// Imprime el reporte actual
        /// </summary>
        /// <param name="parametrosReporte">Parametros del reporte</param>
        private void ImprimirReporte(Dictionary<String, Object> parametrosReporte)
        {
            lblClienteValue.Text = parametrosReporte["Cliente"].ToString();
            lblVinValue.Text = parametrosReporte["Vin"].ToString();
            try
            {
                lblUnidadNoValue.Text = parametrosReporte["UnidadId"].ToString();
            }
            catch{}
            
            lblFechaVallue.Text = parametrosReporte["Fecha"].ToString();
            lblOrdenServicioValue.Text = parametrosReporte["OrdenServicioId"].ToString();
            if ((object)parametrosReporte["Kilometraje"] != null) 
            lblKilometrajeValue.Text = parametrosReporte["Kilometraje"].ToString();
            if ((object)parametrosReporte["Horas"] != null) 
            lblHorasValue.Text = parametrosReporte["Horas"].ToString();
            lblTipoMantenimiento.Text = parametrosReporte["TipoMantenimiento"].ToString();
            lblInfoAdicionalValue.Text = parametrosReporte["Observaciones"].ToString();

            List<ActividadDTO> dtoActividades = ((List<Dictionary<string, string>>)parametrosReporte["Actividades"]).ConvertAll(actividad => new ActividadDTO(actividad));
            List<TecnicoDTO> dtoTecnicos = ((List<string>)parametrosReporte["Tecnicos"]).ConvertAll(tecnico => new TecnicoDTO(tecnico));

            this.drAuditoria.DataSource = dtoActividades;
            this.lblActividad.DataBindings.Add(new XRBinding("Text", dtoActividades, "Actividad"));
            this.lblResultado1.DataBindings.Add(new XRBinding("Text", dtoActividades, "Resultado1"));
            this.lblResultado2.DataBindings.Add(new XRBinding("Text", dtoActividades, "Resultado2"));
            this.lblResultado3.DataBindings.Add(new XRBinding("Text", dtoActividades, "Resultado3"));
            this.lblCriterio.DataBindings.Add(new XRBinding("Text", dtoActividades, "Criterio"));
            this.lblComentario.DataBindings.Add(new XRBinding("Text", dtoActividades, "Comentarios"));

            this.drTecnicos.DataSource = dtoTecnicos;
            this.lblTecnico.DataBindings.Add(new XRBinding("Text", dtoTecnicos, "Tecnico"));
        }

        /// <summary>
        /// DTO para desplegar las acitvidades
        /// </summary>
        private class ActividadDTO
        {
            public string Actividad { get; set; }
            public string Resultado1 { get; set; }
            public string Resultado2 { get; set; }
            public string Resultado3 { get; set; }
            public string Criterio { get; set; }
            public string Comentarios { get; set; }

            public ActividadDTO(Dictionary<string, string> actividades)
            {
                this.Actividad = actividades["Actividad"];
                this.Criterio = actividades["Criterio"];
                this.Comentarios = actividades["Comentarios"];

                switch (actividades["Resultado"])
                {
                    case "Satisfactoria":
                        this.Resultado1 = "√";
                        break;
                    case "Reparar":
                        this.Resultado2 = "√";
                    break;
                    case "Ajustado":
                        this.Resultado3 = "√";
                    break;
                }
            }
        }

        /// <summary>
        /// DTO para desplegar los tecnicos
        /// </summary>
        private class TecnicoDTO
        {
            public string Tecnico { get; set; }

            public TecnicoDTO(string tecnico)
            {
                this.Tecnico = tecnico;
            }
        }
    }
}
