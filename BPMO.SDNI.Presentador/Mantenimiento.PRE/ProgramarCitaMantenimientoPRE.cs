//Satisface al caso de uso CU026 - Calendarizar Mantenimiento
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BPMO.SDNI.Mantenimiento.VIS;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Facade.SDNI.BR;
using BPMO.SDNI.Mantenimiento.BO;
using BPMO.SDNI.Mantenimiento.BR;
using BPMO.Basicos.BO;
using BPMO.Primitivos.Enumeradores;

namespace BPMO.SDNI.Mantenimiento.PRE
{
    public class ProgramarCitaMantenimientoPRE
    {
        #region Atributos
        /// <summary>
        /// DataContext de la aplicacion
        /// </summary>
        private IDataContext dctx;
        /// <summary>
        /// Vista de Programacion de Citas
        /// </summary>
        private IProgramarCitaMantenimientoVIS vistaCita;
        /// <summary>
        /// Vista de datos de cita de mantto
        /// </summary>
        private IucDatosCitaMantenimientoVIS vistaUCDatosCita;
        /// <summary>
        /// Presentador de citas de mantto
        /// </summary>
        private ucDatosCitaMantenimientoPRE presentadorDatos;
        /// <summary>
        /// Controlador utilizado para conexiones con las citas de mantenimiento
        /// </summary>
        private CitaMantenimientoBR controlador;
        /// <summary>
        /// Nombre de la Clase, usado en excepciones
        /// </summary>
        private readonly string nombreClase = typeof(ProgramarCitaMantenimientoPRE).Name;
        #endregion
        #region Constructor
        public ProgramarCitaMantenimientoPRE(IProgramarCitaMantenimientoVIS vistaCita, IucDatosCitaMantenimientoVIS vistaUCDatosCita)
        {
            try
            {
                this.vistaCita = vistaCita;
                this.vistaUCDatosCita = vistaUCDatosCita;
                this.presentadorDatos = new ucDatosCitaMantenimientoPRE(vistaUCDatosCita);
                controlador = new CitaMantenimientoBR();
                dctx = FacadeBR.ObtenerConexion();
            }
            catch (Exception ex)
            {
                throw new Exception(nombreClase + ".ProgramarCitaMantenimientoPRE: " + ex.Message);
            }
        } 
        #endregion
        #region Metodos
        public void RegistrarCitaMantenimiento()
        {
            try
            {
                string s;
                if ((s = this.presentadorDatos.ValidarCamposRegistro()) != null)
                {
                    this.vistaCita.MostrarMensaje(s, ETipoMensajeIU.ADVERTENCIA, null);
                    return;
                }
                this.presentadorDatos.ValidarCamposRegistro();
                CitaMantenimientoBO citaMantto = (CitaMantenimientoBO)this.presentadorDatos.InterfazUsuarioAObjeto();
                citaMantto.EstatusCita = EEstatusCita.CALENDARIZADA;
                controlador.Insertar(dctx, citaMantto, CrearObjetoSeguridad());
                this.vistaCita.MostrarMensaje("Cita Registrada", ETipoMensajeIU.EXITO);
                this.vistaCita.RedirigirConsulta();
            }
            catch (Exception ex)
            {
                throw new Exception(nombreClase + ".RegistrarCitaMantenimiento: " + ex.Message);
            }
        }

        /// <summary>
        /// Crea un objeto de seguridad con los datos actuales de la sesión de usuario en curso
        /// </summary>
        /// <returns>Objeto de tipo seguridad</returns>
        private SeguridadBO CrearObjetoSeguridad()
        {
            UsuarioBO usuario = new UsuarioBO() { Id = this.vistaUCDatosCita.UsuarioID };
            AdscripcionBO adscripcion = new AdscripcionBO() { UnidadOperativa = new UnidadOperativaBO() { Id = this.vistaUCDatosCita.UnidadOperativaID } };
            SeguridadBO seguridadBO = new SeguridadBO(Guid.Empty, usuario, adscripcion);

            return seguridadBO;
        }
        /// <summary>
        /// Valida el acceso a la sección en curso
        /// </summary>
        public void ValidarAcceso()
        {
            try
            {
                //Valida que usuario y la unidad operativa no sean nulos
                if (this.vistaUCDatosCita.UsuarioID == null) throw new Exception("El identificador del usuario no debe ser nulo");
                if (this.vistaUCDatosCita.UnidadOperativaID == null) throw new Exception("El identificador de la Unidad Operativa no debe ser nulo");
                //Se crea el objeto de seguridad                
                SeguridadBO seguridadBO = this.CrearObjetoSeguridad();

                //Se valida si el usuario tiene permiso a la acción principal
                if (!FacadeBR.ExisteAccion(this.dctx, "UI INSERTAR", seguridadBO) && !FacadeBR.ExisteAccion(this.dctx, "INSERTAR", seguridadBO))
                    this.vistaCita.RedirigirSinPermisoAcceso();
            }
            catch (Exception ex)
            {
                throw new Exception(nombreClase + ".ValidarAcceso: " + ex.GetBaseException().Message);
            }
        }
        #endregion
    }
}
