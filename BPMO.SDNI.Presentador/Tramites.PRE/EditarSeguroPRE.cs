//Esta clase satisface al CU085 - CATALOGO INFORMACION SEGURO DE LA UNIDAD
using System;
using BPMO.Basicos.BO;
using BPMO.Facade.SDNI.BR;
using BPMO.Patterns.Creational.DataContext;
using BPMO.SDNI.Tramites.VIS;
using BPMO.SDNI.Tramites.BO;
using BPMO.SDNI.Tramites.BR;

namespace BPMO.SDNI.Tramites.PRE
{
    public class EditarSeguroPRE
    {
        #region Atributos
        private SeguroBR controlador;
        private IDataContext dctx = null;
        private IEditarSeguroVIS vista;
        private IucSeguroVIS vista1;
        private ucSeguroPRE presentador1;
        private string nombreClase = "EditarSeguroPRE";
        #endregion

        #region Constructores
        public EditarSeguroPRE(IEditarSeguroVIS view, IucSeguroVIS view1)
        {
            this.vista = view;
            this.vista1 = view1;
            this.presentador1 = new ucSeguroPRE(view1);
            this.dctx = FacadeBR.ObtenerConexion();
        }
        #endregion

        #region Métodos
        public void CargaInicial()
        {
            this.EstablecerDatosNavegacion(this.vista.ObtenerDatosNavegacion());
            this.LimpiarSesion();
            this.PrepararEdicion();
            this.ConsultarCompleto();
        }

        private void ConsultarCompleto()
        {
            this.vista1.Modo = 1;
            this.vista1.CargarObjeto("EDITARSEGURO");
            this.vista1.Modo = 1;

            this.vista.NumeroPoliza = this.vista1.NumeroPoliza;
            this.vista.DescripcionTramitable = this.vista1.VIN;
        }

        public object InterfazUsuarioADato()
        {
            return this.presentador1.InterfazUsuarioADato();
        }

        private void PrepararEdicion()
        {
            this.LimpiarSesion();
            this.vista1.PrepararEdicion();
            this.ColumnasModoEdicion();
        }
        #region CU0004

        public void ColumnasModoEdicion()
        {
            this.vista1.ModoEditar();
        }
        #endregion
        private void LimpiarSesion()
        {
            this.vista.LimpiarSesion();
            this.vista1.LimpiarSesion();
        }

        private void EstablecerDatosNavegacion(object paqueteNavegacion)
        {
            if (paqueteNavegacion == null)
                throw new Exception(this.nombreClase + ".EstablecerDatosNavegacion: Se esperaba un objeto en la navegación. No se puede identificar qué seguro se desea consultar.");
            if (!(paqueteNavegacion is SeguroBO))
                throw new Exception(this.nombreClase + ".EstablecerDatosNavegacion: Se esperaba un seguro.");

            SeguroBO bo = (SeguroBO)paqueteNavegacion;

            this.DatoAInterfazUsuario(bo);
        }

        private void DatoAInterfazUsuario(object bo)
        {
            SeguroBO obj = (SeguroBO)bo;
            this.vista1.Activo = obj.Activo;
            this.vista1.Aseguradora = obj.Aseguradora;
            this.vista1.Contacto = obj.Contacto;            
            this.vista1.NumeroPoliza = obj.NumeroPoliza;
            this.vista1.Observaciones = obj.Observaciones;
            this.vista1.PrimaAnual = obj.PrimaAnual;
            this.vista1.PrimaSemestral = obj.PrimaSemestral;
            this.vista1.TramiteID = obj.TramiteID;
            this.vista1.VigenciaFinal = obj.VigenciaFinal;
            this.vista1.VigenciaInicial = obj.VigenciaInicial;
            this.vista1.TramiteID = obj.TramiteID;
        }

        public void Editar()
        {
            this.vista1.Editar();
        }

        public void IrADetalle(int? tramiteID)
        {
            this.vista.LimpiarSesion();
            this.vista.LimpiarSesionEditar();
            this.vista.EstablecerPaqueteNavegacion("TramiteSeguro", tramiteID.Value);
            this.vista.IrADetalle();
        }

        #region SC_0008
        public void ValidarAcceso()
        {
            try
            {
                //se valida que los datos del usuario y la unidad operativa no sean nulos
                if (this.vista.UsuarioId == null) throw new Exception("El identificador del usuario no debe ser nulo");
                if (this.vista.UnidadOperativaId == null) throw new Exception("El identificador de la Unidad Operativa no debe ser nulo ");

                //Se crea el objeto de seguridad
                UsuarioBO usuario = new UsuarioBO() { Id = this.vista.UsuarioId };
                AdscripcionBO adscripcion = new AdscripcionBO() { UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaId } };
                SeguridadBO seguridadBO = new SeguridadBO(Guid.Empty, usuario, adscripcion);

                //Se valida si el usuario tiene permiso a la acción principal
                if (!FacadeBR.ExisteAccion(this.dctx, "ACTUALIZARCOMPLETO", seguridadBO))
                    this.vista.RedirigirSinPermisoAcceso();
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".ValidarAcceso:" + ex.Message);
            }
        }
        #endregion
        #endregion
    }
}