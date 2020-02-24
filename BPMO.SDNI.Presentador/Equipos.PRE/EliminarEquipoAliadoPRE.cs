//Satisface al CU075 - Catálogo de Equipo Aliado
using System;
using System.Collections.Generic;
using BPMO.Basicos.BO;
using BPMO.Facade.SDNI.BR;
using BPMO.Patterns.Creational.DataContext;
using BPMO.SDNI.Equipos.BO;
using BPMO.SDNI.Equipos.VIS;

namespace BPMO.SDNI.Equipos.PRE
{
    public class EliminarEquipoAliadoPRE
    {
        #region Atributos

	    private readonly IDataContext dctx = FacadeBR.ObtenerConexion();
        private IEliminarEquipoAliadoVIS vista;
        private IucEquipoAliadoDetalleVIS vista1;
        private ucEquipoAliadoDetallePRE presentador1;
        private const string nombreClase = "EliminarEquipoAliadoPRE";
        #endregion

        #region Constructores
        public EliminarEquipoAliadoPRE(IEliminarEquipoAliadoVIS view, IucEquipoAliadoDetalleVIS view1)
        {
            this.vista = view;
            this.vista1 = view1;
            this.presentador1 = new ucEquipoAliadoDetallePRE(view1);
        }
        #endregion

        #region Métodos
        public void CancelarBaja()
        {
            this.presentador1.CancelarBaja();
        }

        public void Eliminar()
        {
            this.presentador1.Eliminar();
        }

        public void CargaInicial()
        {
            this.PrepararVista();
            this.EstablecerDatosNavegacion(this.vista.ObtenerDatosNavegacion());
            this.presentador1.CargarObjetoInicio();
            EstablecerAcciones();
            
        }

        private void PrepararVista()
        {
            this.vista.PrepararVista();
        }

        private void EstablecerDatosNavegacion(object paqueteNavegacion)
        {
            try
            {
                if (paqueteNavegacion == null)
                    throw new Exception("Se esperaba un objeto en la navegación. No se puede identificar qué acta de nacimiento se desea consultar.");
                if (!(paqueteNavegacion is EquipoAliadoBO))
                    throw new Exception("Se esperaba una Unidad de Idealease.");

                EquipoAliadoBO bo = (EquipoAliadoBO)paqueteNavegacion;

                this.DatoAInterfazUsuario(bo);
            }
            catch (Exception ex)
            {
                this.DatoAInterfazUsuario(new EquipoAliadoBO());
                throw new Exception(nombreClase + ".EstablecerDatosNavegacion: " + ex.Message);
            }
        }

        private void DatoAInterfazUsuario(EquipoAliadoBO bo)
        {
            EquipoAliadoBO obj = (EquipoAliadoBO)bo;
            if (obj.EquipoAliadoID.HasValue)
                this.vista.EquipoAliadoID = obj.EquipoAliadoID.Value;
        }

		#region SC_0008
		public void ValidarAcceso()
		{
            try
            {
                //se valida que los datos del usuario y la unidad operativa no sean nulos
                if (this.vista1.Usuario == null) throw new Exception("El identificador del usuario no debe ser nulo");
                if (this.vista1.UnidadOperativaID == null) throw new Exception("El identificador de la Unidad Operativa no debe ser nulo ");

                //Se crea el objeto de seguridad
                UsuarioBO usuario = vista1.Usuario;
                AdscripcionBO adscripcion = new AdscripcionBO() { UnidadOperativa = new UnidadOperativaBO() { Id = this.vista1.UnidadOperativaID } };
                SeguridadBO seguridadBO = new SeguridadBO(Guid.Empty, usuario, adscripcion);

                //Se obtienen las acciones a las cuales el usuario tiene permiso en este proceso
                this.vista.ListaAcciones = FacadeBR.ConsultarAccion(this.dctx, seguridadBO);

                //Se valida si el usuario tiene permiso a la acción principal
                if (!ExisteAccion(this.vista.ListaAcciones, "ACTUALIZARCOMPLETO"))
                    this.vista.RedirigirSinPermisoAcceso();
            }
            catch (Exception ex)
            {
                throw new Exception(nombreClase + ".ValidarAcceso:" + ex.Message);
            }
		}
		#endregion

        #region REQ 13596 Métodos relacionado con las acciones dependiendo de la unidad operativa.

        /// <summary>
        /// Verifica que una acción a avaluar exista en el listado de acciones asignadas al usuario.
        /// </summary>
        /// <param name="acciones">Lista de acciones asignadas al usuario.</param>
        /// <param name="nombreAccion">Acción a evaluar</param>
        /// <returns>Devuelve true en caso de existir la acción a evaluar en el listado de acciones, en caso contrario regresa false.</returns>
        private bool ExisteAccion(List<CatalogoBaseBO> acciones, string nombreAccion)
        {
            if (acciones != null && acciones.Count > 0 && acciones.Exists(p => p != null && p.Nombre != null && p.Nombre.Trim().ToUpper() == nombreAccion.Trim().ToUpper()))
                return true;

            return false;
        }

        /// <summary>
        /// Invoca el método EstablecerAcciones de la presentadora ucEquipoAliadoPRE.
        /// </summary>
        public void EstablecerAcciones()
        {
            this.presentador1.EstablecerAcciones(this.vista.ListaAcciones);
        }

        #endregion
        #endregion
    }
}