//Satisface al CU077 - Registrar Acta de Nacimiento de una Unidad
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using BPMO.Primitivos.Enumeradores;

using BPMO.SDNI.MapaSitio.UI;

using BPMO.SDNI.Tramites.BO;
using BPMO.SDNI.Tramites.PRE;
using BPMO.SDNI.Tramites.VIS;
using BPMO.Basicos.BO;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Comun.UI;
using BPMO.SDNI.Comun.PRE;
using Newtonsoft.Json;
using DevExpress.XtraPrinting.Native;
using BPMO.SDNI.Comun.VIS;
using DevExpress.PivotGrid.Internal.ThinClientDataSource;

namespace BPMO.SDNI.Tramites.UI
{
    public partial class ucTramitesActivosUI : System.Web.UI.UserControl, IucTramitesActivosVIS
    {
        #region Atributos
        private ucTramitesActivosPRE presentador;
        private string nombreClase = "ucTramitesActivosUI";
        //REQ 13285, variable global para instanciar las etiquetas que se obtendrán a través del archivo de recursos.
        private ObtenerEtiquetaEmpresas obtenerEtiqueta = null;
        private bool habilitarPedimento = false;
        public TextBox txtPedimentoTemp = null;
        #endregion

        #region Propiedades
        public int? TramitableID
        {
            get
            {
                int? id = null;
                if (!String.IsNullOrEmpty(this.hdnTramitableID.Value))
                    id = int.Parse(this.hdnTramitableID.Value.Trim());
                return id;
            }
            set
            {
                if (value != null)
                    this.hdnTramitableID.Value = value.ToString();
                else
                    this.hdnTramitableID.Value = string.Empty;
            }
        }
        public ETipoTramitable? TipoTramitable
        {
            get
            {
                ETipoTramitable? bo = null;
                if (this.hdnTipoTramitable.Value.Trim().CompareTo("") != 0)
                    bo = (ETipoTramitable)Enum.Parse(typeof(ETipoTramitable), this.hdnTipoTramitable.Value);
                return bo;
            }
            set
            {
                if (value == null)
                    this.hdnTipoTramitable.Value = string.Empty;
                else
                    this.hdnTipoTramitable.Value = ((int)value).ToString();
            }
        }
        public string DescripcionEnllantable
        {
            get
            {
                return (String.IsNullOrEmpty(this.hdnDescripcionEnllantable.Value)) ? null : this.hdnDescripcionEnllantable.Value.Trim().ToUpper();
            }
            set
            {
                if (value != null)
                    this.hdnDescripcionEnllantable.Value = value;
                else
                    this.hdnDescripcionEnllantable.Value = string.Empty;
            }
        }
        public List<TramiteBO> Tramites
        {
            get
            {
                if ((List<TramiteBO>)Session["ListaTramites"] == null)
                    return new List<TramiteBO>();
                else
                    return (List<TramiteBO>)Session["ListaTramites"];
            }
            set
            {
                Session["ListaTramites"] = value;
            }
        }

        /// <summary>
        /// REQ: 13285, Modificación acta nacimiento
        /// </summary>
        public string NumeroPedimento
        {
            get
            {
                return (String.IsNullOrEmpty(this.hdnNumeroPedimento.Value)) ? null : this.hdnNumeroPedimento.Value.Trim().ToUpper();
            }
            set
            {
                if (value != null)
                    this.hdnNumeroPedimento.Value = value;
                else
                    this.hdnNumeroPedimento.Value = string.Empty;
            }
        }

        /// <summary>
        /// REQ: 13285, verifica si cambio el pedimento
        /// </summary>
        public bool CambioPedimento
        {
            get
            {
                return (String.IsNullOrEmpty(this.hdnCambioPedimento.Value)) ? false : Convert.ToBoolean(this.hdnCambioPedimento.Value);
            }
            set
            {
                if (value != null)
                    this.hdnCambioPedimento.Value = value.ToString();
                else
                    this.hdnCambioPedimento.Value = string.Empty;
            }
        }



        public List<ArchivoBO> Archivos
        {
            get
            {
                if (ucCatalogoDocumentosTramites.ObtenerArchivos() != null)
                    return ucCatalogoDocumentosTramites.ObtenerArchivos();
                else
                    return new List<ArchivoBO>();
            }
            set { ucCatalogoDocumentosTramites.EstablecerListasArchivos(value, null); }
        }

        public List<TipoArchivoBO> TiposArchivo
        {
            get { return ucCatalogoDocumentosTramites.TiposArchivo; }
            set { ucCatalogoDocumentosTramites.TiposArchivo = value; }
        }
        public IucCatalogoDocumentosVIS VistaDocumentos
        {
            get { return this.ucCatalogoDocumentosTramites; }
        }

        public void EstablecerIdentificadorListaArchivos(string identificador)
        {
            this.ucCatalogoDocumentosTramites.Identificador = identificador;
        }
        public void EstablecerTipoAdjunto(ETipoAdjunto tipo)
        {
            ucCatalogoDocumentosTramites.TipoAdjunto = tipo;
        }

        public AdscripcionBO Adscripcion
        {
            get
            {
                return this.Session["Adscripcion"] != null ? (AdscripcionBO)this.Session["Adscripcion"] : null;
            }
            set
            {
                this.Session["Adscripcion"] = value;
            }
        }

        public List<CatalogoBaseBO> ListaAcciones { get; set; }

        #endregion

        #region Constructores
        protected void Page_Load(object sender, EventArgs e)
        {
                      
            this.presentador = new ucTramitesActivosPRE(this);
            //RQM 13285  
            this.presentador.vistaDocumento = VistaDocumentos;
        }
        #endregion

        #region Métodos
        public void EstablecerPaqueteNavegacion(string nombre, object valor)
        {
            Session[nombre] = valor;
        }

        public void OcultarRedireccionTramites(bool ocultar)
        {
            this.trTramites.Visible = !ocultar;
        }

        public void ActualizarTramites()
        {
            this.lvTramites.DataSource = this.Tramites;
            this.lvTramites.DataBind();
        }

        public void RedirigirACatalogoTramites()
        {
            this.Response.Redirect(this.ResolveUrl("~/Tramites.UI/DetalleTramitesUI.aspx"));
        }

        public void LimpiarSesion()
        {
            if (Session["ListaTramites"] != null)
                Session.Remove("ListaTramites");
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

        #region RQM 13285

        /// <summary>
        /// Método implementado desde la vista IucTramitesActivosVIS, que se encarga de buscar dentro del archivo de recursos las etiquetas y 
        /// configuraciones de cada unidad operativa.
        /// </summary>
        public void EstablecerAcciones()
        {

            #region 

            //Instanciamos la clase o web method que obtiene las etiquetas
            obtenerEtiqueta = new ObtenerEtiquetaEmpresas();
            if(txtPedimentoTemp!=null)
                txtPedimentoTemp.Enabled = this.habilitarPedimento;

            #endregion
        }

        /// <summary>
        /// Método implementado desde la vista IucTramitesActivosVIS,
        /// que se encarga buscar dentro del archivo de recursos las etiquetas de la pagina principal
        /// </summary>
        /// <param name="cEtiquetaBuscar">Valor de la etiqueta a buscar para cambiar el nombre</param>
        /// <returns>Regresa el valor de la etiqueta por empresa</returns>
        public string ConfigurarEtiquetaPrincipal(string cEtiquetaBuscar)
        {
            string cEtiquetaObtenida = string.Empty;
            EtiquetaObtenida request = null;
            string valorEtiqueta = string.Empty;

            cEtiquetaObtenida = obtenerEtiqueta.ObtenerEtiquetadelResource(cEtiquetaBuscar, this.Adscripcion.UnidadOperativa.Id.GetValueOrDefault());
            request = JsonConvert.DeserializeObject<EtiquetaObtenida>(cEtiquetaObtenida);

            if (request.cMensaje.IsEmpty())
            {
                valorEtiqueta = request.cEtiqueta;
            }
            else
            {
                this.MostrarMensaje("Inconsistencia al buscar la etiqueta en el archivo de recursos", ETipoMensajeIU.ERROR, nombreClase + "ConfigurarTab" + request.cMensaje);
            }
            return valorEtiqueta;
        }

        public void HabilitarPedimento(bool habilitar)
        {
            this.habilitarPedimento = habilitar;
        }

        public void HabilitarCargaArchivo(bool habilitar)
        {
            this.ucCatalogoDocumentosTramites.Visible = habilitar;
        }

        private void ConfigurarTramites(ListViewItemEventArgs e)
        {

            //Instanciamos la clase o web method que obtiene las etiquetas
            obtenerEtiqueta = new ObtenerEtiquetaEmpresas();
            bool respuesta = false;

            if ((e.Item.DataItem as TramiteBO).Tipo == ETipoTramite.TENENCIA)
            {
                if (ConfigurarEtiquetaPrincipal("RE35") != "NO APLICA")
                    respuesta = true;
            }

            if ((e.Item.DataItem as TramiteBO).Tipo == ETipoTramite.PLACA_ESTATAL)
            {
                if (ConfigurarEtiquetaPrincipal("RE36") != "NO APLICA")
                    respuesta = true;
            }

            if ((e.Item.DataItem as TramiteBO).Tipo == ETipoTramite.SEGURO)
            {
                if (ConfigurarEtiquetaPrincipal("RE37") != "NO APLICA")
                    respuesta = true;
            }

            if ((e.Item.DataItem as TramiteBO).Tipo == ETipoTramite.PLACA_FEDERAL)
            {
                if (ConfigurarEtiquetaPrincipal("RE38") != "NO APLICA")
                    respuesta = true;
            }

            if ((e.Item.DataItem as TramiteBO).Tipo == ETipoTramite.VERIFICACION_AMBIENTAL)
            {
                if (ConfigurarEtiquetaPrincipal("RE39") != "NO APLICA")
                    respuesta = true;
            }
            if ((e.Item.DataItem as TramiteBO).Tipo == ETipoTramite.VERIFICACION_FISICO_MECANICA)
            {
                if (ConfigurarEtiquetaPrincipal("RE40") != "NO APLICA")
                    respuesta = true;
            }
            if ((e.Item.DataItem as TramiteBO).Tipo == ETipoTramite.GPS)
            {
                if (ConfigurarEtiquetaPrincipal("RE41") != "NO APLICA")
                    respuesta = true;
            }
            if ((e.Item.DataItem as TramiteBO).Tipo == ETipoTramite.FILTRO_AK)
            {
                if (ConfigurarEtiquetaPrincipal("RE42") != "NO APLICA")
                    respuesta = true;
            }

            if ((e.Item.DataItem as TramiteBO).Tipo == ETipoTramite.N_PEDIMENTO)
            {
                txtPedimentoTemp = ((TextBox)e.Item.FindControl("txbResultado"));
                txtPedimentoTemp.MaxLength = 20;
                if (ConfigurarEtiquetaPrincipal("RE43") != "NO APLICA")
                    respuesta = true;
            }

            ((TextBox)e.Item.FindControl("txbResultado")).Visible = respuesta;
            ((Image)e.Item.FindControl("imgEstatus")).Visible = respuesta;
            ((Label)e.Item.FindControl("lblTramite")).Visible = respuesta;
        }

        /// <summary>
        /// Indica el modo del uc
        /// </summary>
        /// <param name="habilitar"></param>
        public void ModoEdicion(bool habilitar)
        {
            this.ucCatalogoDocumentosTramites.ModoEdicion = habilitar;
        }
        #endregion

        #endregion

        #region Eventos
        protected void btnTramites_Click(object sender, EventArgs e)
        {
            try
            {
                this.presentador.VerCatalogoTramites();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al redirigir a trámites", ETipoMensajeIU.ERROR, this.nombreClase + ".btnTramites_Click:" + ex.Message);
            }
        }

        protected void lvTramites_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListViewItemType.DataItem)
                {
                    TramiteBO tramite = e.Item.DataItem != null ? (TramiteBO)e.Item.DataItem : new TramiteProxyBO();

                    if (tramite.Activo != null && tramite.Activo == true)
                        ((Image)e.Item.FindControl("imgEstatus")).ImageUrl = this.ResolveUrl("~/Contenido/Imagenes/exito.png");
                    else
                        ((Image)e.Item.FindControl("imgEstatus")).ImageUrl = this.ResolveUrl("~/Contenido/Imagenes/error.png");


                    ConfigurarTramites(e);
                    
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al mostrar los trámites", ETipoMensajeIU.ERROR, this.nombreClase + ".lvTramites_ItemDataBound:" + ex.Message);
            }
        }

        #endregion

        protected void txbResultado_OnTextChanged(object sender, EventArgs e)
        {
            TextBox pedimento = sender as TextBox;
            this.NumeroPedimento = pedimento.Text;
            this.CambioPedimento = true;

        }
    }
}
