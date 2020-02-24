//Satisface al caso de uso PLEN.BEP.15.MODMTTO.CU030.Recalendarizar.Mantenimiento
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BPMO.SDNI.Mantenimiento.VIS;
using BPMO.SDNI.MapaSitio.UI;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Mantenimiento.PRE;

namespace BPMO.SDNI.Mantenimiento.UI
{
    /// <summary>
    /// UI de Reprogramacion de citas de mantenimiento
    /// </summary>
    public partial class ReprogramarCitaMantenimientoUI : Page, IReprogramarCitaMantenimientoVIS
    {
        #region Atributos
        /// <summary>
        /// Nombre de la clase en la cual estamos trabajando
        /// </summary>
        private string nombreClase = "ReprogramarCitaMantenimientoUI";
        /// <summary>
        /// Presentador
        /// </summary>
        private ReprogramarCitaMantenimientoPRE presentador;
        #endregion

        #region Constructor
        /// <summary>
        /// Construnctor de la clase
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                presentador = new ReprogramarCitaMantenimientoPRE(this, this.ucDatosCitaMantenimiento);

                if (!this.IsPostBack)
                {
                    this.presentador.ValidarAcceso();

                    this.ucDatosCitaMantenimiento.SucursalFiltroVisible = false;
                    this.ucDatosCitaMantenimiento.TallerFiltroVisible = false;
                    this.ucDatosCitaMantenimiento.BtnBuscarSucursalVisible = false;
                    this.ucDatosCitaMantenimiento.EtiquetaTitulo = "Reprogramar cita de mantenimiento";
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al crear de página", ETipoMensajeIU.ERROR, nombreClase + ".Page_Load:" + ex.Message);
            }
        }
        #endregion

        #region Métodos

        /// <summary>
        /// Registra Script en el cliente
        /// </summary>
        /// <param name="key">Key del script</param>
        /// <param name="script">script a registrar</param>
        private void RegistrarScript(string key, string script)
        {
            ScriptManager.RegisterStartupScript(Page, typeof(Page), key, script, true);
        }

        /// <summary>
        /// Redirige a la pagina informativa de falta de permisos para acceder
        /// </summary>
        public void RedirigirSinPermisoAcceso()
        {
            this.Response.Redirect(this.ResolveUrl("~/MapaSitio.UI/PaginaSinAccesoUI.aspx"));
        }

        /// <summary>
        /// Despliega un mensaje en pantalla
        /// </summary>
        /// <param name="mensaje">Mensaje a desplegar</param>
        /// <param name="tipo">Tipo de mensaje a desplegar</param>
        /// <param name="detalle">Muestra una cadena de texto que contiene los datalles del mensaje a desplegar</param>
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

        #endregion

        #region Eventos
        /// <summary>
        /// Evento clic del botón btnActualizarMantto
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnActualizarMantto_Click(object sender, EventArgs e)
        {
            try
            {
                this.RegistrarScript("PantallaDeConfirmacion", "PresentarDialogConfirm();");
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al reprogramar la cita de mantenimiento", ETipoMensajeIU.ERROR, nombreClase + ".btnActualizarMantto_Click:" + ex.Message);
            }
        }
        /// <summary>
        /// Evento clic del botón btnConfirmar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnConfirmar_Click(object sender, EventArgs e)
        {
            try
            {
                this.presentador.ActualizarCitaMantenimiento();
                this.MostrarMensaje("Cita Recalendarizada", ETipoMensajeIU.EXITO);
                this.Response.Redirect(this.ResolveUrl("~/Mantenimiento.UI/ProgramarMantenimientosUI.aspx"));
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al reprogramar la cita de mantenimiento", ETipoMensajeIU.ERROR, nombreClase + ".btnConfirmar_Click:" + ex.Message);
            }
        }

        #endregion
    }
}