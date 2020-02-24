//Satisface al caso de uso PLEN.BEP.15.MODMTTO.CU030.Recalendarizar.Mantenimiento
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BPMO.SDNI.Mantenimiento.VIS;
using BPMO.Patterns.Creational.DataContext;
using BPMO.SDNI.Mantenimiento.BR;
using BPMO.Facade.SDNI.BR;
using BPMO.SDNI.Mantenimiento.BO;
using BPMO.Basicos.BO;

namespace BPMO.SDNI.Mantenimiento.PRE
{
    public class ReprogramarCitaMantenimientoPRE
    {

        #region Atributos

        /// <summary>
        /// DataContext de la aplicacion
        /// </summary>
        private IDataContext dctx;
        /// <summary>
        /// Vista de Reprogramacion de Citas
        /// </summary>
        IReprogramarCitaMantenimientoVIS vistaCita;
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
        private readonly string nombreClase = typeof(ReprogramarCitaMantenimientoPRE).Name;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor de la clase en uco
        /// </summary>
        /// <param name="vistaCita">Crea una instancia para la Interfaz IReprogramarCitaMantenimientoVIS</param>
        /// <param name="vistaUCDatosCita">Crea una instancia para la Interfaz IucDatosCitaMantenimientoVIS</param>
        public ReprogramarCitaMantenimientoPRE(IReprogramarCitaMantenimientoVIS vistaCita, IucDatosCitaMantenimientoVIS vistaUCDatosCita)
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
                throw new Exception(nombreClase + "ReprogramarCitaMantenimientoPRE: " + ex.Message);
            }
        }

        #endregion

        #region Métodos
        /// <summary>
        /// Método que actualiza la cita de mantenimiento
        /// </summary>
        public void ActualizarCitaMantenimiento()
        {

            try
            {
                CitaMantenimientoBO citaMantto = (CitaMantenimientoBO)this.presentadorDatos.InterfazUsuarioAObjeto();
                controlador.Actualizar(dctx, citaMantto, CrearObjetoSeguridad());
            }
            catch (Exception ex)
            {
                throw new Exception(nombreClase + ".ActualizarCitaMantenimiento: " + ex.Message);
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
                if (!FacadeBR.ExisteAccion(this.dctx, "UI ACTUALIZAR", seguridadBO) ||
                    !FacadeBR.ExisteAccion(this.dctx, "ACTUALIZAR", seguridadBO) ||
                    !FacadeBR.ExisteAccion(this.dctx, "CONSULTAR", seguridadBO))
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
