//Esta clase satisface al CU085 - CATALOGO INFORMACION SEGURO DE LA UNIDAD
using System;
using System.Collections.Generic;
using BPMO.Basicos.BO;
using BPMO.Facade.SDNI.BR;
using BPMO.Patterns.Creational.DataContext;
using BPMO.SDNI.Tramites.VIS;

namespace BPMO.SDNI.Tramites.PRE
{
    public class RegistrarSeguroPRE
    {
        #region Atributos
        private IDataContext dctx = null;
        private IRegistrarSeguroVIS vista;
        private IucSeguroVIS vista1;
        private ucSeguroPRE presentador1;
        private string nombreClase = "RegistrarSeguroPRE";
        #endregion               

        #region Constructores
        public RegistrarSeguroPRE(IRegistrarSeguroVIS view, IucSeguroVIS view1)
        {
            this.vista = view;
            this.vista1 = view1;
            this.presentador1 = new ucSeguroPRE(view1);
            this.dctx = FacadeBR.ObtenerConexion();
        }
        #endregion

        #region Métodos
        public void PrepararNuevo()
        {
            this.vista1.VIN = this.vista.VIN;
            this.vista1.PrepararNuevo();
            this.vista1.VIN = this.vista.VIN;
            this.ModoRegistrar();
        }
        #region SC0004
        public void ModoRegistrar()
        {
         this.vista1.ModoRegistrar();
        }
        #endregion
        public void Registrar()
        {
            this.vista1.Registrar();            
        }

        public void LimpiarSesion()
        {
            this.vista.LimpiarSesion();
        }

        #region SC_0008
        public void ValidarAcceso()
        {
            try
            {
                //se valida que los datos del usuario y la unidad operativa no sean nulos
                if (this.vista.UC == null) throw new Exception("El identificador del usuario no debe ser nulo");
                if (this.vista.UnidadOperativaId == null) throw new Exception("El identificador de la Unidad Operativa no debe ser nulo ");

                //Se crea el objeto de seguridad
                UsuarioBO usuario = new UsuarioBO() { Id = this.vista.UC };
                AdscripcionBO adscripcion = new AdscripcionBO() { UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaId } };
                SeguridadBO seguridadBO = new SeguridadBO(Guid.Empty, usuario, adscripcion);

                //Se obtienen las acciones a las cuales el usuario tiene permiso en este proceso
                List<CatalogoBaseBO> lst = FacadeBR.ConsultarAccion(this.dctx, seguridadBO);

                //Se valida si el usuario tiene permiso para insertar cuenta cliente
                if (!this.ExisteAccion(lst, "INSERTARCOMPLETO"))
                    this.vista.RedirigirSinPermisoAcceso();
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".EstablecerSeguridad:" + ex.Message);
            }
        }
        private bool ExisteAccion(List<CatalogoBaseBO> acciones, string nombreAccion)
        {
            if (acciones != null && acciones.Count > 0 && acciones.Exists(p => p != null && p.Nombre != null && p.Nombre.Trim().ToUpper() == nombreAccion.Trim().ToUpper()))
                return true;

            return false;
        }
        #endregion
        #endregion
    }
}