//Esta clase satisface al CU085 - CATALOGO INFORMACION SEGURO DE LA UNIDAD
using System;
using System.Collections.Generic;
using BPMO.Basicos.BO;
using BPMO.Facade.SDNI.BR;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Tramites.VIS;
using BPMO.SDNI.Tramites.BO;
using BPMO.SDNI.Tramites.BR;

namespace BPMO.SDNI.Tramites.PRE
{
    public class DetalleSeguroPRE
    {
        #region Atributos
        private IDetalleSeguroVIS vista;
        private IucSeguroDetalleVIS vista1;
        private ucSeguroDetallePRE presentador1;
        private SeguroBR controlador;
        private IDataContext dctx = null;
        private string nombreClase = "DetalleSeguroPRE";
        #endregion

        #region Constructores
        public DetalleSeguroPRE(IDetalleSeguroVIS view, IucSeguroDetalleVIS view1)
        {
            try
            {
                this.vista = view;
                this.vista1 = view1;
                this.presentador1 = new ucSeguroDetallePRE(view1);
                this.controlador = new SeguroBR();
                this.dctx = FacadeBR.ObtenerConexion();
            }
            catch (Exception ex)
            {
                this.vista.MostrarMensaje("Inconsistencias en los parámetros de configuración", ETipoMensajeIU.ERROR, this.nombreClase + ".DetalleSeguroPRE:" + ex.Message);
            }
        }
        #endregion

        #region Métodos
        public void RealizarPrimeraCarga()
        {
            this.EstablecerDatosNavegacion(this.vista.ObtenerDatosNavegacion());
            this.LimpiarSesion();
            this.ConsultarCompleto();
            this.PrepararVisualizacion();
            this.EstablecerSeguridad();
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

        private void DatoAInterfazUsuario(SeguroBO bo)
        {
            this.vista1.DatoAInterfazUsuario(bo);

            this.vista.NumeroPoliza = bo.NumeroPoliza;

            if (bo.Tramitable != null)
                this.vista.DescripcionTramitable = bo.Tramitable.DescripcionTramitable;
            else
                this.vista.DescripcionTramitable = null;
        }

        private void PrepararVisualizacion()
        {
            this.vista1.PrepararVista();
        }

        private void LimpiarSesion()
        {
            this.vista.LimpiarSesion();
            this.vista1.LimpiarSesion();
        }

        private void ConsultarCompleto()
        {
            try 
            {
                SeguroBO bo = (SeguroBO)this.vista1.InterfazUsuarioADato();

                List<SeguroBO> lst = this.controlador.ConsultarCompleto(this.dctx, bo);

                if (lst.Count < 1)
                    throw new Exception("No se encontró ningún registro que corresponda a la información proporcionada.");
                if (lst.Count > 1)
                    throw new Exception("La consulta devolvió más de un registro.");

                this.DatoAInterfazUsuario(lst[0]);
                this.vista1.ActualizarLista();
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".ConsultarCompleto:" + ex.Message);
            }
        }

        public void IrAEdicion()
        {
            vista.EstablecerPaqueteNavegacion("EDITARSEGURO");
        }

        public object InterfazUsuarioADato()
        {
            return this.presentador1.InterfazUsuarioADato();
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
                if (!FacadeBR.ExisteAccion(this.dctx, "CONSULTAR", seguridadBO))
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
                if (this.vista.UsuarioId == null) throw new Exception("El identificador del usuario no debe ser nulo");
                if (this.vista.UnidadOperativaId == null) throw new Exception("El identificador de la Unidad Operativa no debe ser nulo ");

                //Se crea el objeto de seguridad
                UsuarioBO usuario = new UsuarioBO() { Id = this.vista.UsuarioId };
                AdscripcionBO adscripcion = new AdscripcionBO() { UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaId } };
                SeguridadBO seguridadBO = new SeguridadBO(Guid.Empty, usuario, adscripcion);

                //Se obtienen las acciones a las cuales el usuario tiene permiso en este proceso
                List<CatalogoBaseBO> lst = FacadeBR.ConsultarAccion(this.dctx, seguridadBO);

                //Se valida si el usuario tiene permiso para editar seguros
                if (!this.ExisteAccion(lst, "ACTUALIZARCOMPLETO"))
                    this.vista.PermitirEditar(false);
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
        #endregion
    }
}