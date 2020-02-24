//Satisface el CU063 - Administrar Tareas Pendientes
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BPMO.SDNI.Mantenimiento.PRE;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.MapaSitio.UI;
using BPMO.SDNI.Mantenimiento.VIS;
using BPMO.SDNI.Mantenimiento.BOF;

namespace BPMO.SDNI.Mantenimiento.UI
{
    /// <summary>
    /// Clase para el control de la UI de registrar tarea pendiente
    /// </summary>
    public partial class RegistrarTareaPendienteUI : System.Web.UI.Page, IRegistrarTareaPendienteVIS
    {
        #region Atributos
        /// <summary>
        /// Presentador de registrar tareas pendientes
        /// </summary>
        private RegistrarTareaPendientePRE presentador;

        /// <summary>
        /// Nombre de clase
        /// </summary>
        private string nombreClase = "RegistrarTareaPendienteUI";
        #endregion

        #region Metodos
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                presentador = new RegistrarTareaPendientePRE(this, this.ucTareasPendientesUI);
                if (!this.IsPostBack)
                {
                    this.presentador.ValidarAcceso();
                    #region Estatus
                    // Deshabilitar los controles de estatus
                    this.ucTareasPendientesUI.rbtnActivo.Checked = true;
                    this.ucTareasPendientesUI.rbtnActivo.Enabled = false;
                    this.ucTareasPendientesUI.rbtnInactivo.Enabled = false;
                    #endregion /Estatus

                    if (Session["TareaPendienteBOF"] != null) {
                        TareaPendienteBOF tarea = Session["TareaPendienteBOF"] as TareaPendienteBOF;
                        this.ucTareasPendientesUI.UnidadID = tarea.UnidadID;
                        this.ucTareasPendientesUI.NumeroEconomico = tarea.NumeroEconomico;
                        this.ucTareasPendientesUI.NumeroSerie = tarea.Serie;
                        this.ucTareasPendientesUI.Modelo = tarea.Modelo;
                        this.ucTareasPendientesUI.ModeloID = tarea.ModeloID;
                        Session["TareaPendienteBOF"] = null;
                    }
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al crear de página", ETipoMensajeIU.ERROR, nombreClase + ".Page_Load:" + ex.Message);
            }
        }

        /// <summary>
        /// Este método despliega un mensaje en pantalla
        /// </summary>
        /// <param name="mensaje">Mensaje a desplegar</param>
        /// <param name="tipo">Tipo de mensaje a desplegar</param>
        /// <param name="detalle">Muestra una cadena de texto que contiene los datalles del mensaje a desplegar</param>
        public void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string msjDetalle)
        {
            Site masterMsj = (Site)Page.Master;
            if (tipo == ETipoMensajeIU.ERROR)
            {
                if (masterMsj != null)
                    masterMsj.MostrarMensaje(mensaje, tipo, msjDetalle);
            }
            else
            {
                if (masterMsj != null)
                    masterMsj.MostrarMensaje(mensaje, tipo);
            }
        }

        /// <summary>
        /// Guarda el mensaje exitoso para desplegarlo en la UI
        /// </summary>
        public void GuardarMensajeExitoso()
        {
            Session["RegistroExitoso"] = true;
        }

        /// <summary>
        /// Redirige a la UI de detalles
        /// </summary>
        public void RedirigirADetalle()
        {
            this.Response.Redirect(this.ResolveUrl("~/Mantenimiento.UI/EditarTareaPendienteUI.aspx"));
        }

        /// <summary>
        /// Redirecciona a PaginaSinAccesoUI en caso no se tengan permisos para la pagina
        /// </summary>
        public void RedirigirSinPermisoAcceso()
        {
            Response.Redirect(ResolveUrl("~/MapaSitio.UI/PaginaSinAccesoUI.aspx"));
        }

        /// <summary>
        /// Establece datos por desplegar en la UI
        /// <param name="nombre">Identificador de objeto</param>
        /// <param name="valor">Valor por desplegar</param>
        /// </summary>
        public void EstablecerPaqueteNavegacion(string nombre, object valor)
        {
            Session[nombre] = valor;
        }
        #endregion

        #region Eventos
        /// <summary>
        /// Evento para el boton guardar
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Argumentos</param>
        /// </summary>
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                this.presentador.Registrar();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al guardar la tarea pendiente", ETipoMensajeIU.ERROR, nombreClase + ".btnGuardar_Click:" + ex.Message);
            }
        }

        /// <summary>
        /// Evento para el boton cancelar
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Argumentos</param>
        /// </summary>
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            try
            {
                this.presentador.CancelarRegistro();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al cancelar el registro", ETipoMensajeIU.ERROR, nombreClase + ".btnCancelar_Click:" + ex.Message);
            }
        }
        #endregion
    }
}
