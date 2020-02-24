// Satisface al caso de uso CU015 - Registrar Contrato  Full Service Leasing
// Satisface al caso de uso CU022 - Consultar Contratos Full Service Leasing
// Satisface al caso de uso CU023 - Editar Contrato Full Service Leasing
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Contratos.FSL.BO;
using BPMO.SDNI.Contratos.FSL.PRE;
using BPMO.SDNI.Contratos.FSL.VIS;
using BPMO.SDNI.MapaSitio.UI;

namespace BPMO.SDNI.Contratos.FSL.UI
{
    public partial class ucDatosAdicionalesAnexoUI : UserControl, IucDatosAdicionalesAnexoVIS
    {
        #region Atributos
        string nombreSesion;
        const string defaultNombreSesion = "DATOS_ADICIONALES_ANEXO";
        const string NombreClase = "ucDatosAdicionalesAnexoUI";
        #endregion Atributos

        #region Propiedades
        /// <summary>
        /// Presentador del Dato Adicional
        /// </summary>
        internal ucDatosAdicionalesAnexoPRE Presentador { get; set; }

        /// <summary>
        /// Nombre de la Variable de Sesion
        /// </summary>
        public string NombreSesion
        {
            get
            {
                if (nombreSesion == null || string.IsNullOrEmpty(nombreSesion.Trim()))
                    nombreSesion = defaultNombreSesion;
                    return nombreSesion;
            }
            set
            {
                nombreSesion = value;
            }
        }

        /// <summary>
        /// Identificador del Contrato
        /// </summary>
        public int? ContratoID
        {
            get
            {
                if (hdnContratoID.Value != null && !string.IsNullOrEmpty(hdnContratoID.Value.Trim()))
                    return int.Parse(hdnContratoID.Value.Trim());
                return null;
            }
            set
            {
                hdnContratoID.Value = (value != null) ? value.ToString() : string.Empty;
            }
        }

        /// <summary>
        /// Identificador del Dato Adicional
        /// </summary>
        public int? Detalle_DatoAdicionalID
        {
            get
            {
                if (hdnDatoAdicionalID.Value != null && !string.IsNullOrEmpty(hdnDatoAdicionalID.Value.Trim()))
                    return int.Parse(hdnDatoAdicionalID.Value.Trim());
                return null;
            }
            set
            {
                hdnDatoAdicionalID.Value = (value != null) ? value.ToString() : string.Empty;
            }
        }

        /// <summary>
        /// Titulo del Dato Adicional
        /// </summary>
        public string Detalle_Titulo
        {
            get
            {
                if (txtTitulo.Text != null && !string.IsNullOrEmpty(txtTitulo.Text.Trim()))
                    return txtTitulo.Text.Trim().ToUpper();//SC_0051
                return null;
            }
            set
            {
                txtTitulo.Text = (value != null && !string.IsNullOrEmpty(value.Trim())) ? value.Trim() : string.Empty;
            }
        }

        /// <summary>
        /// Descripcion del Dato Adicional
        /// </summary>
        public string Detalle_Descripcion
        {
            get
            {
                if (txtDescripcion.Text != null && !string.IsNullOrEmpty(txtDescripcion.Text.Trim()))
                    return txtDescripcion.Text.Trim().ToUpper();//SC_0051
                return null;
            }
            set
            {
                txtDescripcion.Text = (value != null && !string.IsNullOrEmpty(value.Trim()))? value.Trim() :string.Empty;
            }
        }

        /// <summary>
        /// Indica si el Dato Adicional es una Observacion
        /// </summary>
        public bool? Detalle_EsObservacion
        {
            get
            {
                return (cbEsObservacion.Checked);
            }
            set
            {
                cbEsObservacion.Checked = value == true;
            }
        }

        /// <summary>
        /// Listado de Datos Adicionales
        /// </summary>
        public List<DatoAdicionalAnexoBO> DatosAdicionales
        {
            get
            {
                if(Session[NombreSesion] == null)
                    Session[NombreSesion] = new List<DatoAdicionalAnexoBO>();

                return (List < DatoAdicionalAnexoBO > )Session[NombreSesion];
            }
            set
            {
                Session[NombreSesion] = value ?? new List<DatoAdicionalAnexoBO>();
                grdDatosAdicionales.DataSource = Session[NombreSesion];
                grdDatosAdicionales.DataBind();
            }
        }
        /// <summary>
        /// Indica si la Interfaz de Usuario esta en modo Consultar o no.
        /// </summary>
        public bool ModoConsultar
        {
            get
            {
                return hdnModoConsultar.Value == "true";
            }
            set
            {
                hdnModoConsultar.Value = value ? "true" : "false";
            }
        }
        #endregion Propiedades

        #region Constructores
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Presentador = new ucDatosAdicionalesAnexoPRE(this);

                if (!this.IsPostBack)
                    this.txtDescripcion.Attributes.Add("onkeyup", "checkText(this,300);");
            }
            catch (Exception ex)
            {
                MostrarMensaje("Inconsistencias al presentar la información.", ETipoMensajeIU.ERROR,
                               NombreClase + ".Page_Load: " + ex.Message);
            }
        }
        #endregion

        #region Metodos
        /// <summary>
        /// Limpia los datos de Session
        /// </summary>
        public void LimpiarSesion()
        {
            Session.Remove(NombreSesion);
        }

        /// <summary>
        /// Configura la Interfaz de Usuario en Modo Editar
        /// </summary>
        public void ConfigurarModoEditar()
        {
            grdDatosAdicionales.Columns[grdDatosAdicionales.Columns.Count - 3].Visible = true; // Boton Eliminar
            grdDatosAdicionales.Columns[grdDatosAdicionales.Columns.Count - 2].Visible = true; // Boton Editar
            btnAgregarDatoAdicional.Visible = true;
            btnCancelarDatoAdicional.Text = "Cancelar";
            hdnModoConsultar.Value = "false";
            MostrarControlesDetalle(true);
        }

        /// <summary>
        /// Configura la Interfaz de Usuario en Modo Consultar
        /// </summary>
        public void ConfigurarModoConsultar()
        {
            grdDatosAdicionales.Columns[grdDatosAdicionales.Columns.Count - 3].Visible = false; // Boton Eliminar
            grdDatosAdicionales.Columns[grdDatosAdicionales.Columns.Count - 2].Visible = false; // Boton Editar
            btnAgregarDatoAdicional.Visible = false;
            btnCancelarDatoAdicional.Visible = false;
            HabilitarCampos(false);
            hdnModoConsultar.Value = "true";

            if (DatosAdicionales == null || DatosAdicionales.Count == 0)
                MostrarControlesDetalle(false);
            else
                MostrarControlesDetalle(true);
        }

        /// <summary>
        /// Despleiga mensajes en la Interfaz de Usuario
        /// </summary>
        /// <param name="mensaje">Mensaje a Desplehar</param>
        /// <param name="tipo">Tipo de Mensaje</param>
        /// <param name="detalle">Detalle o submensaje</param>
        public void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null)
        {
            var master = (Site)Page.Master;
            if (tipo == ETipoMensajeIU.ERROR)
            {
                if (master != null) master.MostrarMensaje(mensaje, tipo, detalle);
            }
            else
            {
                if (master != null) master.MostrarMensaje(mensaje, tipo);
            }
        }

        /// <summary>
        /// Habilita la Opcion de Editar
        /// </summary>
        /// <param name="p"></param>
        public void HabilitarEditar(bool p)
        {            
            btnActualizar.Enabled = p;
            btnActualizar.Visible = p;
        }

        /// <summary>
        /// Habilita la Opcion de Agregar a Tabla
        /// </summary>
        /// <param name="p"></param>
        public void HabilitarAgregar(bool p)
        {
            btnAgregarDatoAdicional.Enabled = p;
            btnAgregarDatoAdicional.Visible = p;
        }

        public void HabilitarCampos(bool p)
        {
            txtDescripcion.Enabled = p;
            txtTitulo.Enabled = p;
            cbEsObservacion.Enabled = p;
        }

        /// <summary>
        /// Oculta o muestra los controles del Detalle
        /// </summary>
        /// <param name="p"></param>
        public void MostrarControlesDetalle(bool p)
        {
            divDetalle.Visible = p;
        }
       #endregion Metodos

        #region Eventos
        protected void grdDatosAdicionales_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdDatosAdicionales.DataSource = DatosAdicionales;
                grdDatosAdicionales.PageIndex = e.NewPageIndex;
                grdDatosAdicionales.DataBind();
            }
            catch (Exception ex)
            {
                MostrarMensaje("Inconsistencias al cambiar la página de los datos Adicionales", ETipoMensajeIU.ADVERTENCIA, NombreClase + ".grdDatosAdicionales_PageIndexChanging: " + ex.Message);
            }
        }

        protected void grdDatosAdicionales_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                string eCommandNameUpper = e.CommandName.ToUpper();
                int index = Convert.ToInt32(e.CommandArgument);
                DatoAdicionalAnexoBO datoAdicional = DatosAdicionales[index];
                if (eCommandNameUpper == "PAGE" || eCommandNameUpper == "SORT") return;
                switch (eCommandNameUpper)
                {
                    case "CMDELIMINAR":                        
                        Presentador.EliminarDatoAdicional(datoAdicional);
                        break;
                    case "CMDEDITAR":
                        Presentador.DesplegarEditarDatoAdicional(datoAdicional);
                        break;
                    case "CMDDETALLES":
                        Presentador.DesplegarConsultarDatoAdicional(datoAdicional);
                        break;
                    default:
                        throw new Exception("La operación solicitada no esta permitida en este modulo.");
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje("Inconsistencias al presentar la información", ETipoMensajeIU.ERROR, NombreClase + ".grdDatosAdicionales_RowCommand: " + ex.Message);
            }
        }

        protected void btnAgregarDatoAdicional_Click(object sender, EventArgs e)
        {
            try {
                Presentador.AgregarDatoAdicional();
            }
            catch (Exception ex)
            {
                MostrarMensaje("Inconsistencia al Agregar el Dato Adicional.", ETipoMensajeIU.ERROR, NombreClase + ".btnAgregarDatoAdicional_Click:" + ex.Message);
            }
        }

        protected void btnActualizar_Click(object sender, EventArgs e)
        {
            try
            {
                Presentador.ActualizarDatoAdicional();
            }
            catch (Exception ex)
            {
                MostrarMensaje("Inconsistencia al Agregar el Dato Adicional.", ETipoMensajeIU.ERROR, NombreClase + ".btnActualizar_Click:" + ex.Message);
            }
        }

        protected void btnCancelarDatoAdicional_Click(object sender, EventArgs e)
        {
            try
            {
                Presentador.InicializarDetalle();
            }
            catch (Exception ex)
            {
                MostrarMensaje("Inconsistencia al Cancelar la Agregación o Edición del Dato Adicional.", ETipoMensajeIU.ERROR, NombreClase + ".btnCancelarDatoAdicional_Click:" + ex.Message);
            }
        }
        #endregion #Eventos
    }
}