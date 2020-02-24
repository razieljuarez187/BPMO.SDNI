//Satisface al CU077 - Registrar Acta de Nacimiento de una Unidad
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Equipos.BO;
using BPMO.SDNI.Equipos.PRE;
using BPMO.SDNI.Equipos.VIS;
using BPMO.SDNI.MapaSitio.UI;

namespace BPMO.SDNI.Equipos.UI
{
    public partial class ucDatosTecnicosUI : System.Web.UI.UserControl, IucDatosTecnicosVIS
    {
        #region Atributos
        private ucDatosTecnicosPRE presentador;
        private string nombreClase = "ucDatosTecnicosUI";
        #endregion

        #region Propiedades
        public int? ValorInicialHorometro
        {
            get
            {
                int? id = null;
                if (!String.IsNullOrEmpty(this.txtHorasInicial.Text))
                    id = int.Parse(this.txtHorasInicial.Text.Trim().Replace(",","")); //RI0012
                return id;
            }
            set
            {
                if (value != null)
					this.txtHorasInicial.Text = string.Format("{0:#,##0}", value);//RI0012
                else
                    this.txtHorasInicial.Text = string.Empty;
            }
        }
        public int? ValorFinalHorometro
        {
            get
            {
                int? id = null;
                if (!String.IsNullOrEmpty(this.txtHorasFinal.Text))
                    id = int.Parse(this.txtHorasFinal.Text.Trim().Replace(",","")); //RI0012
                return id;
            }
            set
            {
                if (value != null)
					this.txtHorasFinal.Text = string.Format("{0:#,##0}", value);//RI0012
                else
                    this.txtHorasFinal.Text = string.Empty;
            }
        }
        public bool? EsHorometroActivo
        {
            get
            {
                bool? activo = null;
                if (this.rbHrActivoSi.Checked)
                    activo = true;
                if (this.rbHrActivoNo.Checked)
                    activo = false;
                return activo;
            }
            set
            {
                if (value == null)
                {
                    this.rbHrActivoSi.Checked = false;
                    this.rbHrActivoNo.Checked = false;
                }
                else
                {
                    this.rbHrActivoSi.Checked = value.Value;
                    this.rbHrActivoNo.Checked = !value.Value;
                }
            }
        }
        public int? ValorInicialOdometro
        {
            get
            {
                int? id = null;
                if (!String.IsNullOrEmpty(this.txtKilometrosInicial.Text))
                    id = int.Parse(this.txtKilometrosInicial.Text.Trim().Replace(",","")); //RI0012
                return id;
            }
            set
            {
                if (value != null)
					this.txtKilometrosInicial.Text = string.Format("{0:#,##0}", value);//RI0012
                else
                    this.txtKilometrosInicial.Text = string.Empty;
            }
        }
        public int? ValorFinalOdometro
        {
            get
            {
                int? id = null;
                if (!String.IsNullOrEmpty(this.txtKilometrosFinal.Text))
                    id = int.Parse(this.txtKilometrosFinal.Text.Trim().Replace(",","")); //RI0012
                return id;
            }
            set
            {
                if (value != null)
					this.txtKilometrosFinal.Text = string.Format("{0:#,##0}", value);//RI0012
                else
                    this.txtKilometrosFinal.Text = string.Empty;
            }
        }
        public bool? EsOdometroActivo
        {
            get
            {
                bool? activo = null;
                if (this.rbKmActivoSi.Checked)
                    activo = true;
                if (this.rbKmActivoNo.Checked)
                    activo = false;
                return activo;
            }
            set
            {
                if (value == null)
                {
                    this.rbKmActivoSi.Checked = false;
                    this.rbKmActivoNo.Checked = false;
                }
                else
                {
                    this.rbKmActivoSi.Checked = value.Value;
                    this.rbKmActivoNo.Checked = !value.Value;
                }
            }
        }

        public List<HorometroBO> Horometros
        {
            get
            {
                if ((List<HorometroBO>)Session["ListaHorometros"] == null)
                    return new List<HorometroBO>();
                else
                    return (List<HorometroBO>)Session["ListaHorometros"];
            }
            set
            {
                Session["ListaHorometros"] = value;
            }
        }
        public List<HorometroBO> UltimoHorometros
        {
            get
            {
                if ((List<HorometroBO>)Session["LastListaHorometros"] == null)
                    return new List<HorometroBO>();
                else
                    return (List<HorometroBO>)Session["LastListaHorometros"];
            }
            set
            {
                Session["LastListaHorometros"] = value;
            }
        }
        public List<OdometroBO> Odometros
        {
            get
            {
                if ((List<OdometroBO>)Session["ListaOdometros"] == null)
                    return new List<OdometroBO>();
                else
                    return (List<OdometroBO>)Session["ListaOdometros"];
            }
            set
            {
                Session["ListaOdometros"] = value;
            }
        }
        public List<OdometroBO> UltimoOdometros
        {
            get
            {
                if ((List<OdometroBO>)Session["LastListaOdometros"] == null)
                    return new List<OdometroBO>();
                else
                    return (List<OdometroBO>)Session["LastListaOdometros"];
            }
            set
            {
                Session["LastListaOdometros"] = value;
            }
        }

        public decimal? PBVMaximoRecomendado
        {
            get
            {
                decimal? temp = null;
                if (!String.IsNullOrEmpty(this.txtPBV.Text))
                    temp = decimal.Parse(this.txtPBV.Text.Trim().Replace(",","")); //RI0012
                return temp;
            }
            set
            {
                if (value != null)
					this.txtPBV.Text = string.Format("{0:#,##0.0000}", value);//RI0012
                else
                    this.txtPBV.Text = string.Empty;
            }
        }
        public decimal? PBCMaximoRecomendado
        {
            get
            {
                decimal? temp = null;
                if (!String.IsNullOrEmpty(this.txtPBC.Text))
                    temp = decimal.Parse(this.txtPBC.Text.Trim().Replace(",","")); //RI0012
                return temp;
            }
            set
            {
                if (value != null)
					this.txtPBC.Text = string.Format("{0:#,##0.0000}", value);//RI0012
                else
                    this.txtPBC.Text = string.Empty;
            }
        }
        public decimal? CapacidadTanque
        {
            get
            {
                decimal? temp = null;
                if (!String.IsNullOrEmpty(this.txtCapacidadTanque.Text))
                    temp = decimal.Parse(this.txtCapacidadTanque.Text.Trim().Replace(",",""));
                return temp;
            }
            set
            {
                if (value != null)
					this.txtCapacidadTanque.Text = string.Format("{0:#,##0.0000}", value);//RI0012
                else
                    this.txtCapacidadTanque.Text = string.Empty;
            }
        }
        public decimal? RendimientoTanque
        {
            get
            {
                decimal? temp = null;
                if (!String.IsNullOrEmpty(this.txtRendimientoTanque.Text))
                    temp = decimal.Parse(this.txtRendimientoTanque.Text.Trim().Replace(",",""));
                return temp;
            }
            set
            {
                if (value != null)
					this.txtRendimientoTanque.Text = string.Format("{0:#,##0.0000}", value);//RI0012
                else
                    this.txtRendimientoTanque.Text = string.Empty;
            }
        }
        #endregion

        #region Constructores
        protected void Page_Init(object sender, EventArgs e)
        {
            this.presentador = new ucDatosTecnicosPRE(this);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            this.presentador = new ucDatosTecnicosPRE(this);
        }
        #endregion

        #region Métodos
        public void PrepararNuevo()
        {
            this.txtCapacidadTanque.Text = "";
            this.txtPBC.Text = "";
            this.txtPBV.Text = "";
            this.txtRendimientoTanque.Text = "";

            this.PrepararNuevoHorometro();
            this.PrepararNuevoOdometro();
        }
        public void PrepararNuevoOdometro()
        {
            this.txtKilometrosInicial.Text = "";
            this.txtKilometrosFinal.Text = "";
            this.rbKmActivoSi.Checked = true;
        }
        public void PrepararNuevoHorometro()
        {
            this.txtHorasInicial.Text = "";
            this.txtHorasFinal.Text = "";
            this.rbHrActivoSi.Checked = true;
        }

        public void HabilitarModoEdicion(bool habilitar)
        {
            this.PermitirAgregarHorometro(habilitar);
            this.PermitirAgregarOdometro(habilitar);

            this.grvHorometros.Enabled = habilitar;
            this.grvOdometros.Enabled = habilitar;

            this.txtCapacidadTanque.Enabled = habilitar;
            this.txtPBC.Enabled = habilitar;
            this.txtPBV.Enabled = habilitar;
            this.txtRendimientoTanque.Enabled = habilitar;
        }

        public void PermitirAgregarHorometro(bool permitir)
        {
            this.txtHorasInicial.Enabled = permitir;
            this.txtHorasFinal.Enabled = permitir;
            this.rbHrActivoSi.Enabled = permitir;
            this.rbHrActivoNo.Enabled = permitir;
            this.btnAgregarHorometro.Enabled = permitir;
        }
        public void PermitirAgregarOdometro(bool permitir)
        {
            this.txtKilometrosInicial.Enabled = permitir;
            this.txtKilometrosFinal.Enabled = permitir;
            this.rbKmActivoSi.Enabled = permitir;
            this.rbKmActivoNo.Enabled = permitir;
            this.btnAgregarOdometro.Enabled = permitir;
        }

        public void ActualizarOdometros()
        {
            this.grvOdometros.DataSource = this.Odometros;
            this.grvOdometros.DataBind();
        }
        public void ActualizarHorometros()
        {
            this.grvHorometros.DataSource = this.Horometros;
            this.grvHorometros.DataBind();
        }

        public void LimpiarSesion()
        {
            if (Session["ListaHorometros"] != null)
                Session.Remove("ListaHorometros");
            if (Session["ListaOdometros"] != null)
                Session.Remove("ListaOdometros");

            if (Session["LastListaHorometros"] != null)
                Session.Remove("LastListaHorometros");
            if (Session["LastListaOdometros"] != null)
                Session.Remove("LastListaOdometros");
        }

        /// <summary>
        /// Desplegar mensaje de Error con detalle
        /// </summary>
        /// <param name="mensaje">Descripción del mensaje</param>
        /// <param name="tipo">EMensaje tipo de mensaje a desplegar</param>
        /// <param name="detalle">Detalle del mensaje a desplegar</param>
        public void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string msjDetalle = null)
        {
            if (tipo.Equals(ETipoMensajeIU.CONFIRMACION))
            {
                ((HiddenField)this.Parent.FindControl("hdnTipoMensaje")).Value = ((int)tipo).ToString();
                ((HiddenField)this.Parent.FindControl("hdnMensaje")).Value = mensaje;
            }
            else
            {
                Site masterMsj = (Site)this.Parent.Page.Master;
                if (tipo == ETipoMensajeIU.ERROR)
                    masterMsj.MostrarMensaje(mensaje, tipo, msjDetalle);
                else
                    masterMsj.MostrarMensaje(mensaje, tipo);
            }
        }
        #endregion

        #region Eventos
        protected void btnAgregarOdometro_Click(object sender, EventArgs e)
        {
            try
            {
                this.presentador.AgregarOdometro();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al agregar el odómetro a la unidad", ETipoMensajeIU.ERROR, this.nombreClase + ".btnAgregarOdometro_Click:" + ex.Message);
            }
        }
        protected void btnAgregarHorometro_Click(object sender, EventArgs e)
        {
            try
            {
                this.presentador.AgregarHorometro();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al agregar el horómetro a la unidad", ETipoMensajeIU.ERROR, this.nombreClase + ".btnAgregarHorometro_Click:" + ex.Message);
            }
        }

        protected void grvHorometros_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index;

            if (e.CommandName.ToString().ToUpper() == "PAGE") return;

            try
            {
                index = Convert.ToInt32(e.CommandArgument.ToString());
                switch (e.CommandName.ToString())
                {
                    case "Eliminar":
                        this.presentador.QuitarHorometro(index);
                        break;
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al ejecutar la acción sobre el horómetro", ETipoMensajeIU.ERROR, this.nombreClase + ".grvHorometros_RowCommand:" + ex.Message);
            }
        }
        protected void grvOdometros_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index;

            if (e.CommandName.ToString().ToUpper() == "PAGE") return;

            try
            {
                index = Convert.ToInt32(e.CommandArgument.ToString());
                switch (e.CommandName.ToString())
                {
                    case "Eliminar":
                        this.presentador.QuitarOdometro(index);
                        break;
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al ejecutar la acción sobre el odómetro", ETipoMensajeIU.ERROR, this.nombreClase + ".grvOdometros_RowCommand:" + ex.Message);
            }
        }

        protected void grvOdometros_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    OdometroBO o = e.Row.DataItem != null ? (OdometroBO)e.Row.DataItem : new OdometroBO();

                    ((ImageButton)e.Row.FindControl("imgDelete")).Visible = this.presentador.PermitirQuitarOdometro(o);
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al desplegar el odómetro", ETipoMensajeIU.ERROR, this.nombreClase + ".grvOdometros_RowDataBound:" + ex.Message);
            }
        }
        protected void grvHorometros_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    HorometroBO h = e.Row.DataItem != null ? (HorometroBO)e.Row.DataItem : new HorometroBO();

                    ((ImageButton)e.Row.FindControl("imgDelete")).Visible = this.presentador.PermitirQuitarHorometro(h);
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al desplegar el horómetro", ETipoMensajeIU.ERROR, this.nombreClase + ".grvHorometros_RowDataBound:" + ex.Message);
            }
        }
        #endregion
    }
}