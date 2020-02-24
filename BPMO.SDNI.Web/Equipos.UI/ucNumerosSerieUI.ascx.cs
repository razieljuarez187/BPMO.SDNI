//Satisface al CU077 - Registrar Acta de Nacimiento de una Unidad
using System;

using System.Collections.Generic;

using System.Web.UI.WebControls;

using BPMO.Primitivos.Enumeradores;

using BPMO.SDNI.Equipos.BO;

using BPMO.SDNI.MapaSitio.UI;

using BPMO.SDNI.Equipos.VIS;
using BPMO.SDNI.Equipos.PRE;

namespace BPMO.SDNI.Equipos.UI
{
    public partial class ucNumerosSerieUI : System.Web.UI.UserControl, IucNumerosSerieVIS
    {
        #region Atributos
        private ucNumerosSeriePRE presentador;
        private string nombreClase = "ucNumerosSerieUI";
        #endregion

        #region Propiedades
        public string Radiador
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtRadiador.Text)) ? null : this.txtRadiador.Text.Trim().ToUpper();
            }
            set
            {
                if (value != null)
                    this.txtRadiador.Text = value;
                else
                    this.txtRadiador.Text = string.Empty;
            }
        }
        public string PostEnfriador
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtPostEnfriador.Text)) ? null : this.txtPostEnfriador.Text.Trim().ToUpper();
            }
            set
            {
                if (value != null)
                    this.txtPostEnfriador.Text = value;
                else
                    this.txtPostEnfriador.Text = string.Empty;
            }
        }
        
        public string Nombre {

            get {
                return (String.IsNullOrEmpty(this.txtNombre.Text)) ? null : this.txtNombre.Text.Trim().ToUpper();
            }
            set {
                if (value != null)
                    this.txtNombre.Text = value;
                else
                    this.txtNombre.Text = string.Empty;
            }
        }

        public string Serie {
            get {
                return (String.IsNullOrEmpty(this.txtSerie.Text)) ? null : this.txtSerie.Text.Trim().ToUpper();
            }
            set {
                if (value != null)
                    this.txtSerie.Text = value;
                else
                    this.txtSerie.Text = string.Empty;
            }
        }

        public List<NumeroSerieBO> NumerosSerie {
            get {
                if ((List<NumeroSerieBO>)Session["ListaNumerosSerie"] == null)
                    return new List<NumeroSerieBO>();
                else
                    return (List<NumeroSerieBO>)Session["ListaNumerosSerie"];
            }
            set {
                Session["ListaNumerosSerie"] = value;
                this.grvNumeroSerie.DataSource = value;
                this.grvNumeroSerie.DataBind();
            }
        }
        public List<NumeroSerieBO> UltimoNumerosSerie {
            get {
                if ((List<NumeroSerieBO>)Session["LastListaNumerosSerie"] == null)
                    return new List<NumeroSerieBO>();
                else
                    return (List<NumeroSerieBO>)Session["LastListaNumerosSerie"];
            }
            set {
                Session["LastListaNumerosSerie"] = value;
            }
        }
        
        #region SC0030
        public string SerieMotor {
            get {
                return (String.IsNullOrEmpty(this.txtSerieMotor.Text)) ? null : this.txtSerieMotor.Text.Trim().ToUpper();
            }
            set {
                if (value != null)
                    this.txtSerieMotor.Text = value;
                else
                    this.txtSerieMotor.Text = string.Empty;
            }
        }
        #endregion
        public string SerieTurboCargador
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtSerieTurboCargador.Text)) ? null : this.txtSerieTurboCargador.Text.Trim().ToUpper();
            }
            set
            {
                if (value != null)
                    this.txtSerieTurboCargador.Text = value;
                else
                    this.txtSerieTurboCargador.Text = string.Empty;
            }
        }
        public string SerieCompresorAire
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtCompresorAire.Text)) ? null : this.txtCompresorAire.Text.Trim().ToUpper();
            }
            set
            {
                if (value != null)
                    this.txtCompresorAire.Text = value;
                else
                    this.txtCompresorAire.Text = string.Empty;
            }
        }
        public string SerieECM
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtECM.Text)) ? null : this.txtECM.Text.Trim().ToUpper();
            }
            set
            {
                if (value != null)
                    this.txtECM.Text = value;
                else
                    this.txtECM.Text = string.Empty;
            }
        }
        public string SerieAlternador
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtAlternador.Text)) ? null : this.txtAlternador.Text.Trim().ToUpper();
            }
            set
            {
                if (value != null)
                    this.txtAlternador.Text = value;
                else
                    this.txtAlternador.Text = string.Empty;
            }
        }
        public string SerieMarcha
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtMarcha.Text)) ? null : this.txtMarcha.Text.Trim().ToUpper();
            }
            set
            {
                if (value != null)
                    this.txtMarcha.Text = value;
                else
                    this.txtMarcha.Text = string.Empty;
            }
        }
        public string SerieBaterias
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtBaterias.Text)) ? null : this.txtBaterias.Text.Trim().ToUpper();
            }
            set
            {
                if (value != null)
                    this.txtBaterias.Text = value;
                else
                    this.txtBaterias.Text = string.Empty;
            }
        }
        public string TransmisionSerie
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtSerieTransmision.Text)) ? null : this.txtSerieTransmision.Text.Trim().ToUpper();
            }
            set
            {
                if (value != null)
                    this.txtSerieTransmision.Text = value;
                else
                    this.txtSerieTransmision.Text = string.Empty;
            }
        }
        public string TransmisionModelo
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtModeloTransmision.Text)) ? null : this.txtModeloTransmision.Text.Trim().ToUpper();
            }
            set
            {
                if (value != null)
                    this.txtModeloTransmision.Text = value;
                else
                    this.txtModeloTransmision.Text = string.Empty;
            }
        }
        public string EjeDireccionSerie
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtSerieEjeDireccion.Text)) ? null : this.txtSerieEjeDireccion.Text.Trim().ToUpper();
            }
            set
            {
                if (value != null)
                    this.txtSerieEjeDireccion.Text = value;
                else
                    this.txtSerieEjeDireccion.Text = string.Empty;
            }
        }
        public string EjeDireccionModelo
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtModeloEjeDireccion.Text)) ? null : this.txtModeloEjeDireccion.Text.Trim().ToUpper();
            }
            set
            {
                if (value != null)
                    this.txtModeloEjeDireccion.Text = value;
                else
                    this.txtModeloEjeDireccion.Text = string.Empty;
            }
        }
        public string EjeTraseroDelanteroSerie
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtSerieEjeTraseroDelantero.Text)) ? null : this.txtSerieEjeTraseroDelantero.Text.Trim().ToUpper();
            }
            set
            {
                if (value != null)
                    this.txtSerieEjeTraseroDelantero.Text = value;
                else
                    this.txtSerieEjeTraseroDelantero.Text = string.Empty;
            }
        }
        public string EjeTraseroDelanteroModelo
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtModeloEjeTraseroDelantero.Text)) ? null : this.txtModeloEjeTraseroDelantero.Text.Trim().ToUpper();
            }
            set
            {
                if (value != null)
                    this.txtModeloEjeTraseroDelantero.Text = value;
                else
                    this.txtModeloEjeTraseroDelantero.Text = string.Empty;
            }
        }
        public string EjeTraseroTraseroSerie
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtSerieEjeTraseroTrasero.Text)) ? null : this.txtSerieEjeTraseroTrasero.Text.Trim().ToUpper();
            }
            set
            {
                if (value != null)
                    this.txtSerieEjeTraseroTrasero.Text = value;
                else
                    this.txtSerieEjeTraseroTrasero.Text = string.Empty;
            }
        }
        public string EjeTraseroTraseroModelo
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtModeloEjeTraseroTrasero.Text)) ? null : this.txtModeloEjeTraseroTrasero.Text.Trim().ToUpper();
            }
            set
            {
                if (value != null)
                    this.txtModeloEjeTraseroTrasero.Text = value;
                else
                    this.txtModeloEjeTraseroTrasero.Text = string.Empty;
            }
        }
        #endregion

        #region Constructores
        protected void Page_Load(object sender, EventArgs e)
        {
            this.presentador = new ucNumerosSeriePRE(this);
        }
        #endregion

        #region Métodos
        public void PrepararNuevo()
        {
            this.txtAlternador.Text = "";
            this.txtBaterias.Text = "";
            this.txtCompresorAire.Text = "";
            this.txtECM.Text = "";
            this.txtMarcha.Text = "";
            this.txtModeloEjeDireccion.Text = "";
            this.txtModeloEjeTraseroDelantero.Text = "";
            this.txtModeloEjeTraseroTrasero.Text = "";
            this.txtModeloTransmision.Text = "";
            this.txtPostEnfriador.Text = "";
            
            this.PrepararNuevoNumeroSerie();
            
            this.txtRadiador.Text = "";
            this.txtSerieEjeDireccion.Text = "";
            this.txtSerieEjeTraseroDelantero.Text = "";
            this.txtSerieEjeTraseroTrasero.Text = "";
            this.txtSerieTransmision.Text = "";
            #region SC0030
            this.txtSerieMotor.Text = "";
            #endregion
            this.txtSerieTurboCargador.Text = "";
        }
        
        public void PrepararNuevoNumeroSerie() {
            this.txtNombre.Text = "";
            this.txtSerie.Text = "";
        }
        
        public void HabilitarModoEdicion(bool habilitar)
        {
            this.txtAlternador.Enabled = habilitar;
            this.txtBaterias.Enabled = habilitar;
            this.txtCompresorAire.Enabled = habilitar;
            this.txtECM.Enabled = habilitar;
            this.txtMarcha.Enabled = habilitar;
            this.txtModeloEjeDireccion.Enabled = habilitar;
            this.txtModeloEjeTraseroDelantero.Enabled = habilitar;
            this.txtModeloEjeTraseroTrasero.Enabled = habilitar;
            this.txtModeloTransmision.Enabled = habilitar;
            this.txtPostEnfriador.Enabled = habilitar;
            
            this.PermitirAgregarNumeroSerie(habilitar);
            this.grvNumeroSerie.Enabled = habilitar;
            
            this.txtRadiador.Enabled = habilitar;
            this.txtSerieEjeDireccion.Enabled = habilitar;
            this.txtSerieEjeTraseroDelantero.Enabled = habilitar;
            this.txtSerieEjeTraseroTrasero.Enabled = habilitar;
            this.txtSerieTransmision.Enabled = habilitar;
            #region SC0030
            this.txtSerieMotor.Enabled = habilitar;
            #endregion
            this.txtSerieTurboCargador.Enabled = habilitar;
        }
        
        public void PermitirAgregarNumeroSerie(bool permitir) {
            this.txtNombre.Enabled = permitir;
            this.txtSerie.Enabled = permitir;
            this.btnAgregarNumeroSerie.Enabled = permitir;
        }

        public void ActualizarNumeroSerie() {
            this.grvNumeroSerie.DataSource = this.NumerosSerie;
            this.grvNumeroSerie.DataBind();
        }
        
        public void LimpiarSesion()
        {
        
            if (Session["ListaNumerosSerie"] != null)
                Session.Remove("ListaNumerosSerie");

            if (Session["LastListaNumerosSerie"] != null)
                Session.Remove("LastListaNumerosSerie");
        
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
        protected void btnAgregarNumeroSerie_Click(object sender, EventArgs e) {
            try {
                this.presentador.AgregarNumeroSerie();
            }
            catch (Exception ex) {
                this.MostrarMensaje("Inconsistencia al agregar el NumeroSerie a la unidad", ETipoMensajeIU.ERROR, this.nombreClase + ".btnAgregarNumeroSerie_Click:" + ex.Message);
            }
        }

        protected void grvNumeroSerie_RowCommand(object sender, GridViewCommandEventArgs e) {
            int index;

            if (e.CommandName.ToString().ToUpper() == "PAGE") return;

            try {
                index = Convert.ToInt32(e.CommandArgument.ToString());
                switch (e.CommandName.ToString()) {
                    case "Eliminar":
                        this.presentador.QuitarNumeroSerie(index);
                        break;
                }
            }
            catch (Exception ex) {
                this.MostrarMensaje("Inconsistencia al ejecutar la acción sobre el horómetro", ETipoMensajeIU.ERROR, this.nombreClase + ".grvNumeroSerie_RowCommand:" + ex.Message);
            }
        }

        protected void grvNumeroSerie_RowDataBound(object sender, GridViewRowEventArgs e) {
            try {
                if (e.Row.RowType == DataControlRowType.DataRow) {
                    NumeroSerieBO o = e.Row.DataItem != null ? (NumeroSerieBO)e.Row.DataItem : new NumeroSerieBO();

                    ((ImageButton)e.Row.FindControl("imgDelete")).Visible = o != null && !string.IsNullOrWhiteSpace(o.NumeroSerie);
                }
            }
            catch (Exception ex) {
                this.MostrarMensaje("Inconsistencia al desplegar el odómetro", ETipoMensajeIU.ERROR, this.nombreClase + ".grvNumeroSerie_RowDataBound:" + ex.Message);
            }
        }
        
        #endregion
    }
}