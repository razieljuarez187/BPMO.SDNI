//Satisface al CU075 - Catálogo de Equipo Aliado
using System;
using System.Collections.Generic;
using BPMO.Basicos.BO;
using BPMO.Facade.SDNI.BR;
using BPMO.Patterns.Creational.DataContext;
using BPMO.SDNI.Equipos.VIS;

namespace BPMO.SDNI.Equipos.PRE
{
	public class RegistrarEquipoAliadoPRE
    {
        #region Atributos
        private IDataContext dctx = FacadeBR.ObtenerConexion();

		private IRegistrarEquipoAliadoVIS vista;
		private IucEquipoAliadoVIS vista1;
        private ucEquipoAliadoPRE presentador1;

        private const string nombreClase = "RegistrarEquipoAliadoPRE";
		#endregion

		#region Constructores
		public RegistrarEquipoAliadoPRE(IRegistrarEquipoAliadoVIS view, IucEquipoAliadoVIS view1)
		{
			this.vista = view;
			this.vista1 = view1;
			this.presentador1 = new ucEquipoAliadoPRE(view1);
		}
		#endregion

		#region Métodos

		#region SC_0008
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
                    this.vista.RedirigirSinPermisoAcceso();
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

		public void PrepararNuevo()
		{
			this.vista.LimpiarSesion();
			this.presentador1.PrepararNuevo();
			EstablecerSeguridad();
            EstablecerAcciones();
		}

		public void Registrar()
		{
			this.presentador1.Registrar();
		}

		public void CancelarRegistro()
		{
			this.presentador1.CancelarRegistro();
		}

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