//Satisface al Caso de uso CU005 - Armar Paquetes Facturacion

using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using BPMO.eFacturacion.Procesos.BO;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Facturacion.AplicacionesFacturacion.PRE;
using BPMO.SDNI.Facturacion.AplicacionesFacturacion.VIS;
using BPMO.SDNI.MapaSitio.UI;

namespace BPMO.SDNI.Facturacion.AplicacionesFacturacion.UI
{
    /// <summary>
    /// Control que visualiza la sección de información adicional para una factura
    /// </summary>
    public partial class ucInformacionAdicionalUI : System.Web.UI.UserControl, IucInformacionAdicionalVIS
    {
        #region Constantes
        /// <summary>
        /// Clave del Guid asignado la instancia de la página
        /// </summary>
        private const String PAGEGUIDINDEX = "__PAGEGUID";
        #endregion

        #region Campos
        /// <summary>
        /// Nombre de la clase para agregar a los mensajes de Error
        /// </summary>
        private string nombreClase = "ucInformacionAdicionalUI";

        /// <summary>
        /// Número de registro que se esta agregando a la lista
        /// </summary>
        public int NumeroCostosAdicionales = 0;

        /// <summary>
        /// Presentador asociado a la vista
        /// </summary>
        private ucInformacionAdicionalPRE presentador;

        /// <summary>
        /// Id único global de la instancia del control
        /// </summary>        
        private Guid _GUID;
        #endregion

        #region Propiedades
        ///<summary>
        ///Obtiene un valor que representa un Id único global de la instancia del control
        ///</summary>
        ///<value>
        ///Objeto GUID con clave única de la instancia
        ///</value>
        internal Guid GUID
        {
            get
            {
                if (this._GUID == Guid.Empty)
                    this.RegisterGuid();

                return this._GUID;
            }
        }

        /// <summary>
        /// Obtiene un valor que representa un identificador único para la UI
        /// </summary>
        /// <value>Objeto de tipo String</value>
        public string ViewState_Guid
        {
            get
            {
                if (this.Page is ConfigurarFacturacionUI)
                    return (this.Page as ConfigurarFacturacionUI).ViewState_Guid;

                return this.GUID.ToString();
            }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa la etiqueta de un dato adicional a agregar
        /// </summary>
        /// <value>Objeto de tipo String</value>
        public string Etiqueta
        {
            get
            {
                if (!String.IsNullOrEmpty(this.txtEtiqueta.Text) && !String.IsNullOrWhiteSpace(this.txtEtiqueta.Text))
                    return this.txtEtiqueta.Text.Trim();

                return null;
            }
            set
            {
                this.txtEtiqueta.Text = value;
            }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa el valor de un dato adicional a agregar
        /// </summary>
        /// <value>Objeto de tipo String</value>
        public String Valor
        {
            get
            {
                if (!String.IsNullOrEmpty(this.txtValor.Text) && !String.IsNullOrWhiteSpace(this.txtValor.Text))
                    return this.txtValor.Text.Trim();

                return null;
            }
            set
            {
                this.txtValor.Text = value;
            }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa los datos adicionales agregados actualmente.
        /// </summary>
        /// <value>Objeto de tipo IList de DatosAdicionalesFacturaBO</value>
        public IList<DatosAdicionalesFacturaBO> DatosAdicionales
        {
            get
            {
                IList<DatosAdicionalesFacturaBO> objeto = null;
                string nombreSession = String.Format("DATOS_ADICIONALES_{0}", this.ViewState_Guid);
                if (!(this.Session[nombreSession] is IList<DatosAdicionalesFacturaBO>))
                    this.Session[nombreSession] = new List<DatosAdicionalesFacturaBO>();

                objeto = (IList<DatosAdicionalesFacturaBO>)this.Session[nombreSession];
                return objeto;
            }
            set
            {
                string nombreSession = String.Format("DATOS_ADICIONALES_{0}", this.ViewState_Guid);
                if (value != null)
                    this.Session[nombreSession] = value;
                else
                    this.Session.Remove(nombreSession);
            }
        }

        /// <summary>
        /// Obtiene o establece un valor que determina si el control se encuentra activo
        /// </summary>
        public bool Enabled
        {
            get
            {
                return this.pnlContenedor.Enabled;
            }
            set
            {
                this.pnlContenedor.Enabled = value;
            }
        }
        #endregion

        #region Métodos       

        /// <summary>
        /// Registra la clave única global del control en la página
        /// </summary>
        private void RegisterGuid()
        {
            string hiddenFieldValue = this.Request.Form[ucInformacionAdicionalUI.PAGEGUIDINDEX];
            if (hiddenFieldValue == null)
            {
                this._GUID = Guid.NewGuid();
                hiddenFieldValue = Convert.ToBase64String(Encoding.UTF8.GetBytes(this._GUID.ToString()));
            }
            else
            {
                string guidValue = Encoding.UTF8.GetString(Convert.FromBase64String(hiddenFieldValue));
                this._GUID = new Guid(guidValue);
            }

            ScriptManager.RegisterHiddenField(this.Page, ucInformacionAdicionalUI.PAGEGUIDINDEX, hiddenFieldValue);
        }

        /// <summary>
        /// Limpia los datos de sesión
        /// </summary>
        public void LimpiarSesion()
        {
            this.DatosAdicionales = null;
            this.MostrarListaDatosAdicionales();
        }

        /// <summary>
        /// Proceso que se ejecuta cuando se crea una nueva factura
        /// </summary>
        public void PrepararNuevo()
        {
            this.DatosAdicionales = null;
            this.MostrarListaDatosAdicionales();
        }

        /// <summary>
        /// Visualiza la lista de datos adicionales agregados actualmente
        /// </summary>
        public void MostrarListaDatosAdicionales()
        {
            this.grvDatosAdicionales.DataSource = this.DatosAdicionales.Count > 0 ? this.DatosAdicionales : null;
            this.grvDatosAdicionales.DataBind();
        }

        /// <summary>
        /// Desplegar mensaje de Error con detalle
        /// </summary>
        /// <param name="mensaje">Descripción del mensaje</param>
        /// <param name="tipo">EMensaje tipo de mensaje a desplegar</param>
        /// <param name="detalle">Detalle del mensaje a desplegar</param>
        public void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string msjDetalle = null)
        {
            Site masterMsj = (Site)Page.Master;
            if (tipo == ETipoMensajeIU.ERROR)
                masterMsj.MostrarMensaje(mensaje, tipo, msjDetalle);
            else
                masterMsj.MostrarMensaje(mensaje, tipo);
        }
        #endregion

        #region Eventos
        /// <summary>
        /// Método delegado para el evento de carga de la página
        /// </summary>
        /// <param name="sender">Objeto que desencadeno el evento</param>
        /// <param name="e">Argumento asociados al evento</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.presentador = new ucInformacionAdicionalPRE(this);

                if (!this.IsPostBack)
                {
                    this.presentador.PrepararNuevo();                  
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al crear la página", ETipoMensajeIU.ERROR, this.nombreClase + ".Page_Load:" + ex.Message);
            }
        }

        /// <summary>
        /// Método delegado para el evento de ejecución de comando de lista de datos adicionales
        /// </summary>
        /// <param name="sender">Objeto que desencadeno el evento</param>
        /// <param name="e">Argumento asociados al evento</param>
        protected void grvDatosAdicionales_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Eliminar")
                {
                    int id = Convert.ToInt32(e.CommandArgument);
                    this.presentador.EliminarInformacionAdicional(id);
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al eliminar elemento", ETipoMensajeIU.ERROR, this.nombreClase + ".btnContinuar_Click:" + ex.Message);
            }
        }

        /// <summary>
        /// Método delegado para el evento de clic al boton de "Agregar"
        /// </summary>
        /// <param name="sender">Objeto que desencadeno el evento</param>
        /// <param name="e">Argumento asociados al evento</param>
        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                this.presentador.AgregarInformacionAdicional();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al agregar elemento", ETipoMensajeIU.ERROR, this.nombreClase + ".btnContinuar_Click:" + ex.Message);
            }
        }
        #endregion
    }
}