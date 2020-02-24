//Satisface al CU010 - Catálogo de Documentos
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Comun.PRE;
using BPMO.SDNI.Comun.VIS;
using BPMO.SDNI.MapaSitio.UI;

namespace BPMO.SDNI.Comun.UI {
    public partial class ucCatalogoDocumentosUI : System.Web.UI.UserControl, IucCatalogoDocumentosVIS {
        #region Atributos

        internal ucCatalogoDocumentosPRE presentador = null;

        #endregion Atributos

        #region Propiedades

        public string Identificador {
            get {
                return this.hdnIdentificador.Value.Trim().ToUpper();
            }
            set {
                if (value != null)
                    this.hdnIdentificador.Value = value;
                else
                    this.hdnIdentificador.Value = string.Empty;
            }
        }
        public bool? ModoEdicion {
            get {
                bool? id = null;
                if (!String.IsNullOrEmpty(this.hdnModoEdicion.Value))
                    id = bool.Parse(this.hdnModoEdicion.Value.Trim());
                return id;
            }
            set {
                if (value != null)
                    this.hdnModoEdicion.Value = value.ToString();
                else
                    this.hdnModoEdicion.Value = string.Empty;
            }
        }
        public ETipoAdjunto? TipoAdjunto {
            get {
                ETipoAdjunto? area = null;
                if (!String.IsNullOrEmpty(this.hdnTipoAdjunto.Value))
                    area = (ETipoAdjunto)Enum.Parse(typeof(ETipoAdjunto), this.hdnTipoAdjunto.Value);
                return area;
            }
            set {
                if (value == null)
                    this.hdnTipoAdjunto.Value = string.Empty;
                else
                    this.hdnTipoAdjunto.Value = ((int)value).ToString();
            }
        }
        public List<TipoArchivoBO> TiposArchivo {
            get {
                return Session[this.Identificador + "TiposArchivos"] as List<TipoArchivoBO>;
            }
            set {
                Session[this.Identificador + "TiposArchivos"] = value;
            }
        }
        public bool? TieneObservaciones {
            get {
                bool? id = null;
                if (!String.IsNullOrEmpty(this.hdnTieneObservaciones.Value))
                    id = bool.Parse(this.hdnTieneObservaciones.Value.Trim());
                return id;
            }
            set {
                if (value != null)
                    this.hdnTieneObservaciones.Value = value.ToString();
                else
                    this.hdnTieneObservaciones.Value = string.Empty;
            }
        }

        public List<ArchivoBO> NuevosArchivos {
            get { return Session[this.Identificador + "NewObject"] as List<ArchivoBO>; }
            set { Session[this.Identificador + "NewObject"] = value; }
        }
        public List<ArchivoBO> OriginalesArchivos {
            get { return Session[this.Identificador + "LastObject"] as List<ArchivoBO>; }
            set { Session[this.Identificador + "LastObject"] = value; }
        }

        #endregion Propiedaddes

        #region Constructores

        public ucCatalogoDocumentosUI() {
            presentador = new ucCatalogoDocumentosPRE(this);
        }

        protected void Page_Load(object sender, EventArgs e) {
            presentador = new ucCatalogoDocumentosPRE(this);
            if (!IsPostBack) {
                if (this.TieneObservaciones != null) {
                    if (this.TieneObservaciones == false) {
                        this.rfvObservaciones.Enabled = false;
                    } else {
                        this.rfvObservaciones.Enabled = true;
                    }
                }

                if (this.ModoEdicion != null) {
                    if (this.ModoEdicion == false) {
                        this.OcultarElementosNoEdicion();
                    }
                }

                if (this.NuevosArchivos != null)
                    this.DespliegaArchivos();

                this.txtObservaciones.Attributes.Add("onkeyup", "checkText(this,500);");
            }
            lblValidacionTipoArchivo.Visible = false;
        }

        #endregion

        #region Métodos

        public void AgregarArchivo(ArchivoBO archivoBO) {
            if (NuevosArchivos == null) NuevosArchivos = new List<ArchivoBO>();
            NuevosArchivos.Add(archivoBO);
            DespliegaArchivos();
        }

        public void DespliegaArchivos() {
            grdArchivos.DataSource = NuevosArchivos;
            grdArchivos.DataBind();
        }

        public void EliminaArchivo(ArchivoBO archivoBO) {
            if (archivoBO != null)
                if (NuevosArchivos.Contains(archivoBO)) {
                    if (archivoBO.Id != null) {
                        archivoBO.Activo = false;
                    } else {
                        NuevosArchivos.Remove(archivoBO);
                    }
                    DespliegaArchivos();
                }
        }

        public void EstablecerListasArchivos(List<ArchivoBO> listaArchivos, List<TipoArchivoBO> listaTiposArchivos) {
            this.presentador.CargarListaArchivos(listaArchivos, listaTiposArchivos);
        }

        public void EstablecerModoEdicion(bool? edicion) {
            this.presentador.ModoEditable(edicion);
        }

        public void EstablecerRequiereObservaciones(bool? tieneObservaciones) {
            this.presentador.RequiereObservaciones(tieneObservaciones);
        }

        public List<ArchivoBO> ObtenerArchivos() {
            return NuevosArchivos;
        }

        public byte[] ObtenerArchivosBytes() {
            if (uplArchivo.HasFile) {
                return uplArchivo.FileBytes;
            }
            return null;
        }

        public string ObtenerExtension() {
            return uplArchivo.HasFile ? System.IO.Path.GetExtension(ObtenerNombreArchivo()).Replace(".", "") : null;
        }

        public string ObtenerNombreArchivo() {
            if (uplArchivo.HasFile) {
                return uplArchivo.FileName;
            }
            return null;
        }

        public string ObtenerObservaciones() {
            return txtObservaciones.Text;
        }

        public void InicializarControl(List<ArchivoBO> listaArchivos, List<TipoArchivoBO> listaTiposArchivos) {
            presentador = new ucCatalogoDocumentosPRE(this);
            EstablecerListasArchivos(listaArchivos, listaTiposArchivos);
        }

        public void LimpiaCampos() {
            txtObservaciones.Text = "";
        }

        public void OcultarElementosNoEdicion() {
            this.uplArchivo.Visible = false;
            this.updPn.Visible = false;
        }

        public bool ValidaArchivo() {
            return uplArchivo.HasFile;
        }


        public void LimpiarSesion() {
            if (Session[this.Identificador + "LastObject"] != null)
                Session.Remove(this.Identificador + "LastObject");

            if (Session[this.Identificador + "NewObject"] != null)
                Session.Remove(this.Identificador + "NewObject");

            if (Session[this.Identificador + "TiposArchivos"] != null)
                Session.Remove(this.Identificador + "TiposArchivos");
        }

        public void MostrarMensaje(string mensaje, Primitivos.Enumeradores.ETipoMensajeIU tipo, string detalle = null) {

            Site master = (Site)this.Page.Master;
            if (tipo == ETipoMensajeIU.ERROR) {
                if (master != null) master.MostrarMensaje(mensaje, tipo, detalle);
                else {
                    lblValidacionTipoArchivo.Visible = true;
                    lblValidacionTipoArchivo.Text = mensaje + " " + detalle;
                }
            } else {
                if (master != null) master.MostrarMensaje(mensaje, tipo);
                else {
                    lblValidacionTipoArchivo.Visible = true;
                    lblValidacionTipoArchivo.Text = mensaje;
                }
            }
        }

        #endregion Métodos

        #region Eventos

        protected void btnAgregarTabla_Click(object sender, EventArgs e) {
            presentador.AgregarArchivo();
        }

        protected void grdArchivos_PageIndexChanging(object sender, GridViewPageEventArgs e) {
            this.grdArchivos.PageIndex = e.NewPageIndex;
            this.grdArchivos.DataSource = this.NuevosArchivos;
            this.grdArchivos.DataBind();
        }

        protected void grdArchivos_RowCommand(object sender, GridViewCommandEventArgs e) {
            switch (e.CommandName.Trim()) {
                case "eliminar": {
                        try {
                            int index = Convert.ToInt32(e.CommandArgument);
                            ArchivoBO archivo = this.NuevosArchivos[index];
                            this.EliminaArchivo(archivo);
                        } catch (Exception ex) {
                            MostrarMensaje("No fué posible eliminar el elemento", ETipoMensajeIU.ERROR, ex.Message);
                        }
                        break;
                    }
            }
        }

        protected void grdArchivos_RowDataBound(object sender, GridViewRowEventArgs e) {
            if (e.Row.RowType == DataControlRowType.DataRow) {
                ArchivoBO archivoBO = (ArchivoBO)e.Row.DataItem;
                Label lblExtension = e.Row.FindControl("lblExtension") as Label;
                if (lblExtension != null) {
                    string extension = string.Empty;
                    if (archivoBO.TipoArchivo != null)
                        if (archivoBO.TipoArchivo.Extension != null) {
                            extension = archivoBO.TipoArchivo.Extension;
                        }
                    lblExtension.Text = extension;
                }

                if (archivoBO.Id != null) {
                    ImageButton imgBtn = (ImageButton)e.Row.FindControl("ibtDescargar");
                    imgBtn.OnClientClick = "javascript:window.open('../Comun.UI/hdlrCatalogoDocumentos.ashx?archivoID=" + archivoBO.Id + "'); return false;";
                }

                if (this.ModoEdicion != null) {
                    if (this.ModoEdicion == false) {
                        ImageButton imgBtn = (ImageButton)e.Row.FindControl("ibtEliminar");
                        imgBtn.Visible = false;
                    }
                }

                if (archivoBO.Activo == false)
                    e.Row.Attributes["style"] = "display:none";
            }
        }

        #region[REQ: 13285, Integración Generación y Construcción]

        public void MostrarObservaciones(bool mostrar) {
            //this.tdlblObservaciones.Visible = mostrar;
            //this.tdtxtObservaciones.Visible = mostrar;

        }
        public void HabilitarControles(bool habilitar) {
            this.uplArchivo.Enabled = habilitar;
            this.txtObservaciones.Enabled = habilitar;
            this.btnAgregarTabla.Enabled = habilitar;
        }

        public void ReiniciarGrid() {
            this.grdArchivos.DataSource = null;
            this.NuevosArchivos = null;
            DespliegaArchivos();

        }

        public void OcultarCarga() {
            this.updPn.Visible = false;
        }
        #endregion
        #endregion
    }
}