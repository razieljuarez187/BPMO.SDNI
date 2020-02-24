// Satisface al caso de uso CU005 - Catálogo de Tarifas de Renta Comercial Diaria.
// Construccion de Mejoras - Cobro de Rangos de Km y Horas.
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using BPMO.SDNI.Contratos.RD.BO;
using BPMO.SDNI.Contratos.RD.PRE;
using BPMO.SDNI.Contratos.RD.VIS;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.MapaSitio.UI;

namespace BPMO.SDNI.Contratos.RD.UI
{
    public partial class ucTarifaRDUI : UserControl, IucTarifaRDVIS
    {
        #region Atributos
        private string nombreClase = "ucTarifaRDUI";
        private ucTarifaRDPRE presentador;
        #endregion

        #region Propiedades
        /// <summary>
        /// Utilizado en la Interfaz para Determinar si se cobran Kilometros u Horas
        /// </summary>
        protected bool? CargoPorKm
        {
            get
            {
                return this.CobraKm;
            }
        }
        /// <summary>
        /// Determina si la interfaz se encuentra en Modo se Consulta
        /// </summary>
        private bool? EsModoConsulta
        {
            get
            {
                if (String.IsNullOrEmpty(hdnModoConsulta.Value)) return null;
                return this.hdnModoConsulta.Value == "1";
            }
            set
            {
                if (value != null)
                    this.hdnModoConsulta.Value = value.Value ? "1" : "0";
                else
                    this.hdnModoConsulta.Value = "0";
            }
        }
        public int? CapacidadCarga
        {
            get { 
                int? capacidad = null;
                if (!String.IsNullOrEmpty(this.txtCapacidadCarga.Text.Trim()))
                    capacidad =int.Parse(this.txtCapacidadCarga.Text.Trim().Replace(",",""));
                return capacidad;
            }
            set { this.txtCapacidadCarga.Text = value != null ? String.Format("{0:#,##0}", value) : String.Empty; }
        }
        public decimal? TarifaDiaria
        {
            get 
            { 
                decimal? tarifaDiaria = null;
                if (!String.IsNullOrEmpty(this.txtTarifaDiaria.Text.Trim()))
                    tarifaDiaria = decimal.Parse(this.txtTarifaDiaria.Text.Trim().Replace(",", ""));
                return tarifaDiaria;
            }
            set { this.txtTarifaDiaria.Text = value != null ? String.Format("{0:#,##0.00##}", value) : String.Empty; }
        }
        public int? KmLibres
        {
            get 
            {
                int? kmLibres = null;
                if (!String.IsNullOrEmpty(this.txtKmLibres.Text.Trim()))
                    kmLibres = int.Parse(this.txtKmLibres.Text.Trim().Replace(",", ""));
                return kmLibres;
            }
            set { this.txtKmLibres.Text = value != null ? String.Format("{0:#,##0}", value) : String.Empty; }
        }
        /// <summary>
        /// Determina si el Tipo de Cargo de la Tarifa es por Kilometros u Horas
        /// </summary>
        public bool? CobraKm
        {
            get
            {
                if (this.ddlTipoCargo.SelectedIndex == 0) return null;
                return this.ddlTipoCargo.SelectedValue == "0";
            }
            set
            {
                if (value != null)
                    this.ddlTipoCargo.SelectedValue = value.Value ? "0" : "1";
                else
                    this.ddlTipoCargo.SelectedIndex = 0;
            }
        }
        /// <summary>
        /// Rangos de la Tarifa de Renta Diaria
        /// </summary>
        public List<RangoTarifaRDBO> RangosTarifa
        {
            get
            {
                if (Session["RangosTarifaRD"] == null) return null;
                return (List<RangoTarifaRDBO>) Session["RangosTarifaRD"];
            }
            set
            {
                Session["RangosTarifaRD"] = value;
            }
        }
        /// <summary>
        /// Horas Libres para la Tarifa
        /// </summary>
        public int? HorasLibres {
            get { 
                int? hrLibres = null;
                if (!String.IsNullOrEmpty(this.txtHrLibres.Text.Trim()))
                    hrLibres = int.Parse(this.txtHrLibres.Text.Trim().Replace(",", ""));
                return hrLibres;
            }
            set { this.txtHrLibres.Text = value != null ? String.Format("{0:#,##0}", value) : String.Empty; }
        }

        /// <summary>
        /// Rango Inicial
        /// </summary>
        public int? RangoInicial
        {
            get
            {
                if (String.IsNullOrEmpty(txtRangoInicial.Text)) return null;
                return Int32.Parse(txtRangoInicial.Text.Trim());
            }
            set
            {
                txtRangoInicial.Text = value != null ? value.ToString() : String.Empty;
            }
        }

        /// <summary>
        /// Rango Final
        /// </summary>
        public int? RangoFinal
        {
            get
            {
                if(String.IsNullOrEmpty(txtRangoFinal.Text)) return null;
                return Int32.Parse(txtRangoFinal.Text.Trim());
            }
            set
            {
                txtRangoFinal.Text = value != null ? value.ToString() : String.Empty;
            }
        }

        /// <summary>
        /// Costo Rango
        /// </summary>
        public decimal? CostoRango
        {
            get
            {
                if (String.IsNullOrEmpty(txtCobroKmHr.Text.Trim())) return null;
                return Decimal.Parse(txtCobroKmHr.Text.Trim());
            }
            set
            {
                txtCobroKmHr.Text = value != null ? value.ToString() : String.Empty;
            } 
        }

        /// <summary>
        /// Determina si sera el Rango Final de la Tarifa
        /// </summary>
        public bool? EsRangoFinal {
            get
            {
                return this.ddlRangoTarifa.SelectedIndex == 1;
            }
            set
            {
                if (value != null)
                {
                    this.ddlRangoTarifa.SelectedIndex = value.Value ? 1 : 0;
                }
                else
                    this.ddlRangoTarifa.SelectedIndex = 0;
            }
        }
        /// <summary>
        /// Determina si se pueden Crear Rangos
        /// </summary>
        public bool? CrearRangos
        {
            get
            {
                if(String.IsNullOrEmpty(hdnCrearRangos.Value)) return null;
                return this.hdnCrearRangos.Value == "1";
            }
            set
            {
                if(value != null)
                    this.hdnCrearRangos.Value = value.Value ? "1" : "0";
                else
                    this.hdnCrearRangos.Value = "0";
            }
        }
        /// <summary>
        /// Tarifa por Km Adicional / Modo Antiguo
        /// </summary>
        public decimal? TarifaKmAdicional
        {
            get
            {
                if (!String.IsNullOrEmpty(txtTarifaKmAdicional.Text.Trim()))
                    return Decimal.Parse(txtTarifaKmAdicional.Text.Trim());
                return null;
            }
            set
            {
                this.txtTarifaKmAdicional.Text = value != null ? value.ToString() : "";
            }
        }
        /// <summary>
        /// Tarifa por Hr Adicional / Modo Antiguo
        /// </summary>
        public decimal? TarifaHrAdicional
        {
            get
            {
                if(!String.IsNullOrEmpty(txtTarifaHrAdicional.Text.Trim()))
                    return Decimal.Parse(txtTarifaHrAdicional.Text.Trim());
                return null;
            }
            set
            {
                this.txtTarifaHrAdicional.Text = value != null ? value.ToString() : "";
            }
        }

        #endregion

        #region Constructores
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                presentador = new ucTarifaRDPRE(this);
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".Page_Load: Error al inicializar el control de usuario: " + ex.Message);
            }
        }
        #endregion

        #region Métodos
        public void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null)
        {
            var master = (Site)Page.Master;
            if(tipo == ETipoMensajeIU.ERROR)
            {
                if(master != null) master.MostrarMensaje(mensaje, tipo, detalle);
            }
            else
            {
                if(master != null) master.MostrarMensaje(mensaje, tipo);
            }
        }
        /// <summary>
        /// Limpia la Session activa
        /// </summary>
        public void LimpiarSesion()
        {
            if(Session["UltimaTarifa"]!=null)
                Session.Remove("UltimaTarifa");
            if(Session["RangosTarifaRD"] != null)
                Session.Remove("RangosTarifaRD");
        }
        /// <summary>
        /// Coloca los controles en Modo de Edicion
        /// </summary>
        /// <param name="activo">Boolean que determina si colocar en TRUE el Modo de Edicion</param>
        public void ModoEdicion(bool activo)
        {
            this.txtCapacidadCarga.Enabled = activo;
            this.txtHrLibres.Enabled = activo;
            this.txtKmLibres.Enabled = activo;
            this.txtTarifaDiaria.Enabled = activo;
            this.EsModoConsulta = !activo;
            this.ddlTipoCargo.Enabled = activo;
            this.txtRangoInicial.Enabled = activo;
            this.txtRangoFinal.Enabled = activo;
            this.ddlRangoTarifa.Enabled = activo;
            this.txtCobroKmHr.Enabled = activo;
            this.btnAgregarRango.Enabled = activo;
            this.txtTarifaKmAdicional.Enabled = activo;
            this.txtTarifaHrAdicional.Enabled = activo;
        }
        /// <summary>
        /// Presenta en Pantalla los Rangos de la Tarifa
        /// </summary>
        /// <param name="rangoTarifa">Rangos que Pertenecen a la Tarifa</param>
        public void PresentarRangos(List<RangoTarifaRDBO> rangoTarifa)
        {
            if(rangoTarifa == null)
                rangoTarifa = new List<RangoTarifaRDBO>();

            this.grvRangos.DataSource = rangoTarifa;
            this.grvRangos.DataBind();
        }

        public void PermitirTipoCargo(bool permitir)
        {
            this.ddlTipoCargo.Enabled = permitir;
        }

        public void PermitirKmLibres(bool permitir)
        {
            this.txtKmLibres.Enabled = permitir;
        }

        public void PermitirHrsLibres(bool permitir)
        {
            this.txtHrLibres.Enabled = permitir;
        }

        public void PermitirRangoInicial(bool permitir)
        {
            this.txtRangoInicial.Enabled = permitir;
        }

        public void PermiritRangoCargo(bool permitir)
        {
            this.ddlRangoTarifa.Enabled = permitir;
        }

        public void PermitirRangoFinal(bool permitir)
        {
            this.txtRangoFinal.Enabled = permitir;
        }

        public void PermitirCargoAdicional(bool permitir)
        {
            this.txtCobroKmHr.Enabled = permitir;
        }

        public void PermitirAgregarRangos(bool permitir)
        {
            this.btnAgregarRango.Enabled = permitir;
        }

        public void PermitirAdicionales(bool permitir)
        {
            this.txtTarifaKmAdicional.Enabled = permitir;
            this.txtTarifaHrAdicional.Enabled = permitir;
        }
        /// <summary>
        /// Mantiene el modo Actual de las Tarifas, sin configuracion de rangos
        /// </summary>
        /// <param name="modoAntiguo">Determina si se utiliza el modo antiguo</param>
        public void ModoAntiguo(bool modoAntiguo)
        {
            this.trAdicionales.Visible = true;
            this.trCobroRangos.Visible = false;
            this.trGridRangos.Visible = false;
        }

        #endregion
        #region Eventos
        protected void grvRangos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                string eCommandNameUpper = e.CommandName.ToUpper();
                if(eCommandNameUpper == "PAGE" || eCommandNameUpper == "SORT") return;
                if(e.CommandArgument == null) return;
                if(this.EsModoConsulta.Value) return;

                int index;

                if(!Int32.TryParse(e.CommandArgument.ToString(), out index))
                    return;
                if(RangosTarifa == null) return;
                var rangoTarifa = RangosTarifa[index];

                switch(eCommandNameUpper)
                {
                    case "CMDELIMINAR":
                        if(RangosTarifa.Count > 1 && (index + 1) != RangosTarifa.Count)
                            throw new Exception("Debe Eliminarse el Último Rango Primero");

                        RangosTarifa.Remove(rangoTarifa);
                        PresentarRangos(RangosTarifa);
                        break;
                }
            }
            catch(Exception ex)
            {
                this.MostrarMensaje("Inconsistencias al eliminar el rango de la Tarifa", ETipoMensajeIU.ERROR, this.nombreClase + ".grvRangos_RowCommand: " + ex.Message);
            }
        }
        protected void ddlTipoCargo_SelectedIndexChanged(object sender, EventArgs e)
        {
             try
            {
                 presentador.CambiarTipoCargo(this.ddlTipoCargo.SelectedIndex == 0);
                if (this.ddlTipoCargo.SelectedValue == "-1")
                {
                    this.ddlTipoCargo.Focus();
                    return;
                }
                 if(this.ddlTipoCargo.SelectedValue == "0")
                     this.txtKmLibres.Focus();
                 if(this.ddlTipoCargo.SelectedValue == "1")
                     this.txtHrLibres.Focus();
            }
             catch(Exception ex)
             {
                 this.MostrarMensaje("Inconsistencias al cambiar el Tipo de Cargos", ETipoMensajeIU.ERROR, this.nombreClase + ".ddlTipoCargo_SelectedIndexChanged: " + ex.Message);
             }
        }
        protected void btnAgregarRango_Click(object sender, EventArgs e)
        {
            try
            {
                presentador.AgregarRangoATarifa();
            }
            catch(Exception ex)
            {
                this.MostrarMensaje("Inconsistencias al agregar un Rango", ETipoMensajeIU.ERROR, this.nombreClase + ".btnAgregarRango_Click: " + ex.Message);
            }
        }
        protected void grvRangos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (this.EsModoConsulta != null)
                    {
                        var boton = e.Row.FindControl("ibtEliminar") as ImageButton;
                        if (boton != null)
                        {
                            boton.Enabled = !this.EsModoConsulta.Value;
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                throw new Exception(this.nombreClase + ".grvRangos_RowDataBound: Inconsistencias al agregar los Rangos: " + ex.Message);
            }
        }
        protected void ddlRangoTarifa_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.RangoFinal = null;
                this.PermitirRangoFinal(this.ddlRangoTarifa.SelectedIndex == 0);
                if(!this.EsRangoFinal.Value)
                    this.txtRangoFinal.Focus();
                else
                    this.txtCobroKmHr.Focus();
            }
            catch(Exception ex)
            {
                this.MostrarMensaje("Inconsistencias al cambiar el Rango", ETipoMensajeIU.ERROR, this.nombreClase + ".ddlRangoTarifa_SelectedIndexChanged: " + ex.Message);
            }
        }
        #endregion
    }
}