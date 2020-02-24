//Satisface el CU063 - Administrar Tareas Pendientes
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BPMO.SDNI.Mantenimiento.PRE;
using BPMO.SDNI.Mantenimiento.VIS;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.MapaSitio.UI;
using BPMO.SDNI.Mantenimiento.BO;

namespace BPMO.SDNI.Mantenimiento.UI
{
    /// <summary>
    /// Clase para el control de la UI de editar tarea pendiente
    /// </summary>
    public partial class EditarTareaPendienteUI : System.Web.UI.Page, IEditarTareaPendienteVIS
    {
        #region Atributos
        /// <summary>
        /// Presentador de editar tareas pendientes
        /// </summary>
        private EditarTareaPendientePRE presentador;

        /// <summary>
        /// Nombre de clase
        /// </summary>
        private const string nombreClase = "EditarTareaPendienteUI";
        #endregion

        #region Metodos
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.presentador = new EditarTareaPendientePRE(this, this.ucTareasPendientesUI);
                if (!this.IsPostBack)
                {
                    this.presentador.ValidarAcceso();
                    this.PrepararVista();
                }
                if (Session["RegistroExitoso"] != null && (bool)Session["RegistroExitoso"])
                {
                    this.MostrarMensaje("Registro Exitoso", ETipoMensajeIU.EXITO, null);
                    this.btnGuardar.Visible = false;
                    this.ucTareasPendientesUI.TextDescripcion.Enabled = false;
                    Session["RegistroExitoso"] = null;
                }
            }
            catch (Exception ex) { }
        }

        /// <summary>
        /// Redirecciona a PaginaSinAccesoUI en caso no se tengan permisos para la pagina
        /// </summary>
        public void RedirigirSinPermisoAcceso()
        {
            Response.Redirect(ResolveUrl("~/MapaSitio.UI/PaginaSinAccesoUI.aspx"));
        }

        /// <summary>
        /// Prepara la UI inicial
        /// </summary>
        public void PrepararVista()
        {
            this.presentador.PrepararEditar();
            this.btnGuardar.Visible = false;

            if (!this.ucTareasPendientesUI.rbtnActivo.Checked) {
                this.btnEditar.Enabled = false;
                this.mTareaPendiente.Items[1].Enabled = false;
            }
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
                this.hdnTipoMensaje.Value = ((int)tipo).ToString();
                this.hdnMensaje.Value = mensaje;
            }
            else
            {
                Site masterMsj = (Site)Page.Master;
                if (tipo == ETipoMensajeIU.ERROR)
                    masterMsj.MostrarMensaje(mensaje, tipo, msjDetalle);
                else
                    masterMsj.MostrarMensaje(mensaje, tipo);
            }
        }

        /// <summary>
        /// Prepara los campos para editar
        /// </summary>
        private void PrepararEdicion()
        {
            presentador.PrepararEditar();
            this.btnCancelar.Visible = true;
            this.btnGuardar.Visible = true;
            this.btnEditar.Visible = false;
            this.lblEncabezadoLeyenda.Text = "EDITAR TAREA PENDIENTE";
            this.ucTareasPendientesUI.TextDescripcion.Enabled = true;
            this.ucTareasPendientesUI.rbtnActivo.Enabled = true;
            this.ucTareasPendientesUI.rbtnInactivo.Enabled = true;
            divOpciones.Attributes.Remove("class");
            divOpciones.Attributes.Add("class", "GroupHeaderOpciones Ancho2Opciones");
        }

        #endregion

        #region Eventos
        /// <summary>
        /// Evento para el boton cancelar
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Argumentos</param>
        /// </summary>
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            try
            {
                this.presentador.CancelarEdicion();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al cancelar la edición", ETipoMensajeIU.ERROR, nombreClase + ".btnCancelar_Click:" + ex.Message);
            }

        }

        /// <summary>
        /// Evento para el boton guardar
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Argumentos</param>
        /// </summary>
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                this.presentador.Editar();
                this.btnGuardar.Visible = false;
                this.btnCancelar.Visible = false;
                this.btnEditar.Visible = true;

                if (this.ucTareasPendientesUI.rbtnInactivo.Checked) {
                    this.btnEditar.Enabled = false;
                    this.mTareaPendiente.Items[1].Enabled = false;
                }

                divOpciones.Attributes.Remove("class");
                divOpciones.Attributes.Add("class", "GroupHeaderOpciones Ancho1Opciones");
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al guardar la edición", ETipoMensajeIU.ERROR, nombreClase + ".btnGuardar_Click:" + ex.Message);
            }
        }

        /// <summary>
        /// Evento para el boton editar
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Argumentos</param>
        /// </summary>
        protected void btnEditar_Click(object sender, EventArgs e)
        {
            try
            {
                this.PrepararEdicion();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al guardar la edición", ETipoMensajeIU.ERROR, nombreClase + ".btnEditar_Click:" + ex.Message);
            }
        }

        /// <summary>
        /// Evento para barra de herramientas
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Argumentos</param>
        /// </summary>
        protected void mTareaPendiente_MenuItemClick(object sender, MenuEventArgs e)
        {
            switch (e.Item.Value)
            {
                case "EditarTareaPendiente":
                    PrepararEdicion();
                    break;
            }
        }
        #endregion
    }
}