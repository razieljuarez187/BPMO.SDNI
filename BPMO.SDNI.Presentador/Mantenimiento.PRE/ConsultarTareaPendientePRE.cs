//Satisface el CU063 - Administrar Tareas Pendientes
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BPMO.Patterns.Creational.DataContext;
using BPMO.SDNI.Mantenimiento.VIS;
using BPMO.SDNI.Mantenimiento.BR;
using BPMO.Facade.SDNI.BR;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Mantenimiento.BO;
using BPMO.Servicio.Catalogos.BO;
using BPMO.SDNI.Equipos.BR;
using BPMO.Basicos.BO;

namespace BPMO.SDNI.Mantenimiento.PRE
{
    /// <summary>
    /// Presentador para la UI de consultar tareas pendientes
    /// </summary>
    public class ConsultarTareaPendientePRE
    {
        #region Atributos
        /// <summary>
        /// Vista de consultar tarea pendiente
        /// </summary>
        private IConsultarTareaPendienteVIS vista;

        /// <summary>
        /// Vista general de tarea pendiente
        /// </summary>
        private IucTareaPendienteVIS vista1;

        /// <summary>
        /// Contexto de la aplicacion
        /// </summary>
        private IDataContext dctx = null;

        /// <summary>
        /// Controlador de tareas pendientes
        /// </summary>
        private TareaPendienteBR controlador;

        /// <summary>
        /// Nombre de la clase
        /// </summary>
        private string nombreClase = "ConsultarTareaPendientePRE";
        #endregion

        #region Constructores
        public ConsultarTareaPendientePRE(IConsultarTareaPendienteVIS view, IucTareaPendienteVIS view2)
        {
            try
            {
                this.vista = view;
                this.vista1 = view2;
                this.controlador = new TareaPendienteBR();
                this.dctx = FacadeBR.ObtenerConexion();
            }
            catch (Exception ex)
            {
                this.vista.MostrarMensaje("No se pudieron obtener los datos de conexión", ETipoMensajeIU.ERROR,
                        "No se encontraron los parámetros de conexión en la fuente de datos, póngase en contacto con el administrador del sistema." + ex.Message);
            }
        }
        #endregion

        #region Metodos

        /// <summary>
        /// Crea un objeto de seguridad con los datos actuales de la sesión de usuario en curso
        /// </summary>
        /// <returns>Objeto de tipo seguridad</returns>
        private SeguridadBO CrearObjetoSeguridad()
        {
            UsuarioBO usuario = new UsuarioBO() { Id = this.vista1.Usuario.Id };
            AdscripcionBO adscripcion = new AdscripcionBO() { UnidadOperativa = new UnidadOperativaBO() { Id = this.vista1.UnidadOperativaID } };
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
                if (this.vista1.Usuario.Id == null) throw new Exception("El identificador del usuario no debe ser nulo");
                if (this.vista1.UnidadOperativaID == null) throw new Exception("El identificador de la Unidad Operativa no debe ser nulo");
                //Se crea el objeto de seguridad                
                SeguridadBO seguridadBO = this.CrearObjetoSeguridad();

                //Se valida si el usuario tiene permiso a la acción principal
                if (!FacadeBR.ExisteAccion(this.dctx, "UI CONSULTAR", seguridadBO) ||
                    !FacadeBR.ExisteAccion(this.dctx, "CONSULTAR", seguridadBO))
                    this.vista.RedirigirSinPermisoAcceso();
            }
            catch (Exception ex)
            {
                throw new Exception(nombreClase + ".ValidarAcceso: " + ex.GetBaseException().Message);
            }
        }

        /// <summary>
        /// Llama al controlador para realizar la consulta
        /// </summary>
        public void Consultar()
        {
            try
            {
                TareaPendienteBO bo = (TareaPendienteBO)this.InterfazUsuarioADato();
                List<TareaPendienteBO> tareas = controlador.Consultar(dctx, bo).ConvertAll(s => (TareaPendienteBO)s);
                this.vista.Tareas = tareas;
                this.vista.ActualizarResultado();

                if (tareas.Count < 1)
                    this.vista.MostrarMensaje("No se encontraron coincidencias", ETipoMensajeIU.INFORMACION,
                        "No se encontraron coincidencias con los criterios ingresados. Verifique sus datos");
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".Consultar: " + ex.Message);
            }
        }

        /// <summary>
        /// Pasa los datos de la interfaz de usuario a un objeto para consultar
        /// </summary>
        private TareaPendienteBO InterfazUsuarioADato()
        {
           TareaPendienteBO bo = new TareaPendienteBO();
           bo.Unidad = new SDNI.Equipos.BO.UnidadBO();
           if (this.vista1.NumeroSerie != null)
           {
               bo.Unidad.UnidadID = this.vista1.UnidadID;
               bo.Unidad.NumeroSerie = this.vista1.NumeroSerie;
           }
           if (this.vista1.NumeroEconomico != null)
           {
               bo.Unidad.UnidadID = this.vista1.UnidadID;
               bo.Unidad.NumeroEconomico = this.vista1.NumeroEconomico;
           }
           bo.Modelo = new Servicio.Catalogos.BO.ModeloBO();
           if (this.vista1.Modelo != null)
           {
               bo.Modelo.Id = this.vista1.ModeloID;
               bo.Modelo.Nombre = this.vista1.Modelo;
           }
           bo.Activo = this.vista1.Activo;
           return bo;
        }

        /// <summary>
        /// Realiza el cambio de pagina en el datagrid y actualiza los registros
        /// <param name="p">Numero de pagina</param>
        /// </summary>
        public void CambiarPaginaResultado(int p)
        {
            this.vista.IndicePaginaResultado = p;
            this.vista.ActualizarResultado();
        }

        /// <summary>
        /// Se redirige al detalle
        /// <param name="index">Indice de tarea por ver detalle</param>
        /// </summary>
        public void VerDetalles(int index)
        {
            if (index >= this.vista.Tareas.Count || index < 0)
                throw new Exception("No se encontró la tarea pendiente seleccionada.");

            TareaPendienteBO bo = controlador.Consultar(dctx, new TareaPendienteBO() { TareaPendienteID = vista.Tareas[index].TareaPendienteID }).FirstOrDefault();
            this.vista.EstablecerPaqueteNavegacion("TareaPendienteBO", bo);
            this.vista.RedirigirADetalles();
        }
        #endregion
    }
}
