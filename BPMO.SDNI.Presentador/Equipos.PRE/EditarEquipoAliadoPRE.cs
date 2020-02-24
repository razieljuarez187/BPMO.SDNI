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
    public class EditarEquipoAliadoPRE
    {
        #region Atributos
        private IEditarEquipoAliadoVIS vista;
        private IucEquipoAliadoVIS vista1;
        private ucEquipoAliadoPRE presentador1;
	    private IDataContext dctx = FacadeBR.ObtenerConexion();
        private const string nombreClase = "EditarEquipoAliadoPRE";
        #endregion

        #region Constructores
        public EditarEquipoAliadoPRE(IEditarEquipoAliadoVIS view, IucEquipoAliadoVIS view1)
        {
            this.vista = view;
            this.vista1 = view1;
            this.presentador1 = new ucEquipoAliadoPRE(view1);
        }
        #endregion

        #region Métodos
        public void CancelarEdicion()
        {
            this.presentador1.CancelarEdicion();
        }

        public void PrepararEditar()
        {
            this.presentador1.CargarObjetoInicio();
        }

        public void Editar()
        {
            this.presentador1.Editar();
        }

        public void CargaInicial()
        {
            this.PrepararVista();
            this.EstablecerDatosNavegacion(this.vista.ObtenerDatosNavegacion());
			this.EstablecerSeguridad(); //SC_0008
            this.EstablecerAcciones();
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

        private void PrepararVista()
        {
            this.vista.PrepararVista();
        }

		#region SC_0008
		public void ValidarAcceso()
		{
            try
            {
                //se valida que los datos del usuario y la unidad operativa no sean nulos
                if (this.vista1.Usuario == null) throw new Exception("El identificador del usuario no debe ser nulo");
                if (this.vista1.UnidadOperativa == null) throw new Exception("La Unidad Operativa no debe ser nula ");
                if (this.vista1.UnidadOperativa.Id == null) throw new Exception("El identificador de la Unidad Operativa no debe ser nulo ");

                //Se crea el objeto de seguridad
                UsuarioBO usuario = vista1.Usuario;
                AdscripcionBO adscripcion = new AdscripcionBO() { UnidadOperativa = vista1.UnidadOperativa };
                SeguridadBO seguridadBO = new SeguridadBO(Guid.Empty, usuario, adscripcion);

                //Se valida si el usuario tiene permiso a la acción principal
                if (!FacadeBR.ExisteAccion(this.dctx, "ACTUALIZARCOMPLETO", seguridadBO))
                    this.vista.RedirigirSinPermisoAcceso();
            }
            catch (Exception ex)
            {
                throw new Exception(nombreClase + ".ValidarAcceso:" + ex.Message);
            }
		}

		private void EstablecerSeguridad()
		{
            try
            {
                //se valida que los datos del usuario y la unidad operativa no sean nulos
                if (this.vista1.Usuario == null) throw new Exception("El identificador del usuario no debe ser nulo");
                if (this.vista1.UnidadOperativa == null) throw new Exception("La Unidad Operativa no debe ser nula ");
                if (this.vista1.UnidadOperativa.Id == null) throw new Exception("El identificador de la Unidad Operativa no debe ser nulo ");

                //Se crea el objeto de seguridad
                UsuarioBO usuario = vista1.Usuario;
                AdscripcionBO adscripcion = new AdscripcionBO() { UnidadOperativa = vista1.UnidadOperativa };
                SeguridadBO seguridadBO = new SeguridadBO(Guid.Empty, usuario, adscripcion);

                //Se obtienen las acciones a las cuales el usuario tiene permiso en este proceso
                this.vista.ListaAcciones = FacadeBR.ConsultarAccion(this.dctx, seguridadBO);

                //Se valida si el usuario tiene permiso para registrar una llanta
                if (!this.ExisteAccion(this.vista.ListaAcciones, "INSERTARCOMPLETO"))
                    this.vista.PermitirRegistrar(false);
            }
            catch (Exception ex)
            {
                throw new Exception(nombreClase + ".EstablecerSeguridad:" + ex.Message);
            }
		}
		private bool ExisteAccion(List<CatalogoBaseBO> acciones, string nombreAccion)
		{
			if (acciones != null && acciones.Count > 0 && acciones.Exists(p => p != null && p.Nombre != null && p.Nombre.Trim().ToUpper() == nombreAccion.Trim().ToUpper()))
				return true;

			return false;
		}
		#endregion

        /// <summary>
        /// Invoca el método EstablecerAcciones de la presentadora ucEquipoAliadoPRE.
        /// </summary>
        public void EstablecerAcciones()
        {
            this.presentador1.EstablecerAcciones(this.vista.ListaAcciones);
        }

        #endregion
    }
}