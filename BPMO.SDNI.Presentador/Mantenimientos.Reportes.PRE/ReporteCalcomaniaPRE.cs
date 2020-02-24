//Satisface al caso de uso PLEN.BEP.15.MODMTTO.CU017.Imprimir.Calcomania.Mantenimiento
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BPMO.Patterns.Creational.DataContext;
using BPMO.SDNI.Mantenimiento.Reportes.BR;
using BPMO.SDNI.Mantenimiento.Reportes.VIS;
using BPMO.Facade.SDNI.BR;
using BPMO.Primitivos.Enumeradores;
using System.Collections;
using System.Data;
using BPMO.SDNI.Mantenimiento.BR;
using System.Configuration;
using BPMO.Servicio.Procesos.BO;
using BPMO.Basicos.BO;
using BPMO.SDNI.Mantenimiento.BO;

namespace BPMO.SDNI.Mantenimiento.Reportes.PRE
{
    public class ReporteCalcomaniaPRE
    {
        #region Constantes
        /// <summary>
        /// Nombre de la clase para agregar a los mensajes de Error
        /// </summary>
        private static readonly string nombreClase = typeof(ReporteCalcomaniaPRE).Name;
        #endregion

        #region Atributos
        /// <summary>
        /// El DataContext que proveerá acceso a la base de datos
        /// </summary>
        private IDataContext dctx;

        /// <summary>
        /// Vista que se esta gestionando
        /// </summary>
        private IConsultarCalcomaniaVIS vista;

        /// <summary>
        /// Controlador de Consulta de Contratos de Calcomanías
        /// </summary>
        private ReporteCalcomaniaBR controlador;
        #endregion       

        #region Constructor
        /// <summary>
        /// Constructor de la clase en uso
        /// </summary>
        /// <param name="vista"></param>
        public ReporteCalcomaniaPRE(IConsultarCalcomaniaVIS vista)
        {
            try
            {
                this.vista = vista;
                this.dctx = FacadeBR.ObtenerConexion();
                this.controlador = new ReporteCalcomaniaBR();
            }
            catch (Exception ex)
            {
                this.vista.MostrarMensaje("Inconsistencias en la configuracion", ETipoMensajeIU.ERROR, nombreClase + ex.Message);
            }
        }

        #endregion

        #region Métodos 

        /// <summary>
        /// Método que consulta la calcomnía de la unidad seleccionada
        /// </summary>
        /// <param name="serie"></param>
        public void Consultar(string OrdenID)
        {
            
            Hashtable parameters = new Hashtable();
            parameters["UnidadOperativaID"] = this.vista.UnidadOperativaID;
            parameters["ModuloID"] = this.vista.ModuloID;
            bool auxImpreso = false;
            bool catalogo = true;
            parameters["MantenimientoUnidad"] = new MantenimientoUnidadBO { OrdenServicio = new OrdenServicioBO { Id = int.Parse(OrdenID) } };

            Dictionary<String, Object> reportParameters = this.controlador.GenerarReporte(this.dctx, OrdenID, parameters, CrearObjetoSeguridad(), out auxImpreso, catalogo);
            if (((DataSet)reportParameters["DataSource"]).Tables["Mantenimientos"].Rows.Count <= 0)
            {
                this.vista.MostrarMensaje("No se encontraron coincidencias. Favor de verificar", ETipoMensajeIU.ADVERTENCIA);
                return;
            }
            else
            {
                foreach (DataRow row in ((DataSet)reportParameters["DataSource"]).Tables["Calcomania"].Rows)
                {
                    this.vista.EstablecerPaqueteNavegacionImprimir("PLEN.BEP.15.MODMTTO.CU017", reportParameters);
                    this.vista.IrAImprimir();
                }
            }

        }

        /// <summary>
        /// Crea un objeto de seguridad con los datos actuales de la sesión de usuario en curso
        /// </summary>
        /// <returns>Objeto de tipo seguridad</returns>
        private SeguridadBO CrearObjetoSeguridad()
        {
            UsuarioBO usuario = new UsuarioBO() { Id = this.vista.UsuarioID };
            AdscripcionBO adscripcion = new AdscripcionBO() { UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaID } };
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
                if (this.vista.UsuarioID == null) throw new Exception("El identificador del usuario no debe ser nulo");
                if (this.vista.UnidadOperativaID == null) throw new Exception("El identificador de la Unidad Operativa no debe ser nulo");
                //Se crea el objeto de seguridad                
                SeguridadBO seguridadBO = this.CrearObjetoSeguridad();

                //Se valida si el usuario tiene permiso a la acción principal
                if (!FacadeBR.ExisteAccion(this.dctx, "CONSULTAR", seguridadBO))
                    this.vista.RedirigirSinPermisoAcceso();
            }
            catch (Exception ex)
            {
                throw new Exception(nombreClase + ".ValidarAcceso: " + ex.GetBaseException().Message);
            }
        }

        #endregion
    }
}
