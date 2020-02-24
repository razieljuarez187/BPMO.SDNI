//Satisface al caso de uso CU026 - Calendarizar Mantenimiento
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BPMO.SDNI.Mantenimiento.VIS;
using BPMO.SDNI.Mantenimiento.PRE;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.MapaSitio.UI;

namespace BPMO.SDNI.Mantenimiento.UI
{
    /// <summary>
    /// UI de programacion de citas de mantenimiento
    /// </summary>
    public partial class ProgramarCitaMantenimientoUI : Page, IProgramarCitaMantenimientoVIS
    {
        #region Atributos
        /// <summary>
        /// Nombre de la clase en la cual estamos trabajando
        /// </summary>
        private string nombreClase = "ProgramarCitaMantenimientoUI";
        /// <summary>
        /// Enumeración para controlar los objetos del buscador
        /// </summary>
        public enum ECatalogoBuscador
        {
            Sucursal
        }
        /// <summary>
        /// Presentador
        /// </summary>
        private ProgramarCitaMantenimientoPRE presentador;
        #endregion

        #region Constructor
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                presentador = new ProgramarCitaMantenimientoPRE(this, this.ucDatosCitaMantenimiento);
                if (!IsPostBack)
                {
                    this.presentador.ValidarAcceso();
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al crear de página", ETipoMensajeIU.ERROR, nombreClase + ".Page_Load:" + ex.Message);
            }
        } 
        #endregion
        #region Metodos
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
        /// Redirige a la interfaz de consulta
        /// </summary>
        public void RedirigirConsulta()
        {
            this.RegistrarScript("EnviarConsulta", "Redireccion();");
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

        protected void btnRegistrarMantto_Click(object sender, EventArgs e)
        {
            try
            {
                this.presentador.RegistrarCitaMantenimiento();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al programar la cita de mantenimiento", ETipoMensajeIU.ERROR, nombreClase + ".btnRegistrarMantto_Click:" + ex.Message);
            }
        } 
        #endregion

        //protected void btnRegresar_Click(object sender, EventArgs e)
        //{
        //    Response.Redirect("~/Mantenimiento.UI/ProgramarMantenimientosUI.aspx");

        //}
    }
}