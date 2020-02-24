//Satisface al caso de uso CU098 - Configurar Autorizadores para los Contratos
using System;
using System.Collections.Generic;

using BPMO.Facade.SDNI.BR;

using BPMO.Basicos.BO;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Primitivos.Enumeradores;

using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Comun.BR;
using BPMO.SDNI.Comun.VIS;

namespace BPMO.SDNI.Comun.PRE
{
    public class DetalleAutorizadorPRE
    {
        #region Atributos
        private AutorizadorBR controlador;
        private IDataContext dctx = null;

        private IDetalleAutorizadorVIS vista;
        private IucAutorizadorVIS vistaDetalle;

        private ucAutorizadorPRE presentadorDetalle;

        private string nombreClase = "DetalleAutorizadorPRE";
        #endregion

        #region Constructor
        public DetalleAutorizadorPRE(IDetalleAutorizadorVIS view, IucAutorizadorVIS viewAutorizador)
        {
            try
            {
                this.vista = view;
                this.vistaDetalle = viewAutorizador;

                this.presentadorDetalle = new ucAutorizadorPRE(viewAutorizador);

                this.controlador = new AutorizadorBR();
                this.dctx = FacadeBR.ObtenerConexion();
            }
            catch (Exception ex)
            {
                this.vista.MostrarMensaje("Inconsistencias en los parámetros de configuración", ETipoMensajeIU.ERROR, nombreClase + ".DetalleAutorizadorPRE: " + ex.Message);
            }
        }
        #endregion

        #region Métodos
        public void RealizarPrimeraCarga()
        {
            try
            {
                this.PrepararVisualizacion();

                this.EstablecerDatosNavegacion(this.vista.ObtenerPaqueteNavegacion("AutorizadorBO"));
                this.vista.PermitirRegresar(this.vista.ObtenerPaqueteNavegacion("FiltrosAutorizador") != null);
                this.CargarTiposAutorizador();
                this.ConsultarCompleto();

                this.EstablecerSeguridad();
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".RealizarPrimeraCarga: " + ex.Message);
            }
        }
        private void EstablecerDatosNavegacion(object paqueteNavegacion)
        {
            try
            {
                if (paqueteNavegacion == null)
                    throw new Exception("Se esperaba un objeto en la navegación. No se puede identificar qué autorizador se desea consultar.");
                if (!(paqueteNavegacion is AutorizadorBO))
                    throw new Exception("Se esperaba un Autorizador.");

                AutorizadorBO bo = (AutorizadorBO)paqueteNavegacion;

                this.DatoAInterfazUsuario(bo);
            }
            catch (Exception ex)
            {
                this.DatoAInterfazUsuario(new AutorizadorBO());
                throw new Exception(this.nombreClase + ".EstablecerDatosNavegacion: " + ex.Message);
            }
        }
        private void ConsultarCompleto()
        {
            try
            {
                AutorizadorBO bo = (AutorizadorBO)this.InterfazUsuarioADato();

                List<AutorizadorBO> lst = this.controlador.ConsultarCompleto(this.dctx, bo);

                if (lst.Count < 1)
                    throw new Exception("No se encontró ningún registro que corresponda a la información proporcionada.");
                if (lst.Count > 1)
                    throw new Exception("La consulta devolvió más de un registro.");

                this.DatoAInterfazUsuario(lst[0]);
            }
            catch (Exception ex)
            {
                this.DatoAInterfazUsuario(new AutorizadorBO());
                throw new Exception(this.nombreClase + ".ConsultarCompleto:" + ex.Message);
            }
        }
        
        public void ValidarAcceso()
        {
            try
            {
                //se valida que los datos del usuario y la unidad operativa no sean nulos
                if (this.vista.UsuarioID == null) throw new Exception("El identificador del usuario no debe ser nulo");
                if (this.vista.UnidadOperativaID == null) throw new Exception("El identificador de la Unidad Operativa no debe ser nulo ");

                //Se crea el objeto de seguridad
                UsuarioBO usuario = new UsuarioBO() { Id = this.vista.UsuarioID };
                AdscripcionBO adscripcion = new AdscripcionBO() { UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaID } };
                SeguridadBO seguridadBO = new SeguridadBO(Guid.Empty, usuario, adscripcion);

                //Se valida si el usuario tiene permiso a la acción principal
                if (!FacadeBR.ExisteAccion(dctx, "CONSULTAR", seguridadBO))
                    this.vista.RedirigirSinPermisoAcceso();
            }
            catch (Exception ex)
            {
                throw new Exception(nombreClase + ".ValidarAcceso:" + ex.Message);
            }
        }
        public void EstablecerSeguridad()
        {
            try
            {
                //Valida que el usuario y la unidad operativa no sean nulos
                if (this.vista.UsuarioID == null) throw new Exception("El identificador del usuario no debe ser nulo");
                if (this.vista.UnidadOperativaID == null)
                    throw new Exception("El identificador de la Unidad Operativa no debe ser nulo");
                // Creación del objeto seguridad
                UsuarioBO usuario = new UsuarioBO { Id = this.vista.UsuarioID };
                AdscripcionBO adscripcion = new AdscripcionBO
                {
                    UnidadOperativa = new UnidadOperativaBO { Id = this.vista.UnidadOperativaID }
                };
                SeguridadBO seguridad = new SeguridadBO(Guid.Empty, usuario, adscripcion);

                //consulta de acciones a la cual el usuario tiene permisos
                List<CatalogoBaseBO> lst = FacadeBR.ConsultarAccion(dctx, seguridad);

                //Se valida si el usuario tiene permiso para registrar
                if (!this.ExisteAccion(lst, "INSERTAR"))
                    this.vista.PermitirRegistrar(false);
                //Se valida si el usuario tiene permiso para editar
                if (!this.ExisteAccion(lst, "ACTUALIZAR"))
                    this.vista.PermitirEditar(false);
            }
            catch (Exception ex)
            {

                throw new Exception(this.nombreClase + ".EstablecerSeguridad:" + ex.Message);
            }
        }
        private bool ExisteAccion(List<CatalogoBaseBO> acciones, string nombreAccion)
        {
            if (acciones != null && acciones.Count > 0 &&
                acciones.Exists(
                    p => p != null && p.Nombre != null && p.Nombre.Trim().ToUpper() == nombreAccion.Trim().ToUpper()))
                return true;

            return false;
        }

        private void PrepararVisualizacion()
        {
            this.vista.PrepararVisualizacion();
            this.presentadorDetalle.PrepararVisualizacion();
        }

        private object InterfazUsuarioADato()
        {
            AutorizadorBO bo = new AutorizadorBO();
            bo.Sucursal = new SucursalBO();
            bo.Empleado = new EmpleadoBO();

            bo.AutorizadorID = vista.AutorizadorID;

            return bo;
        }
        private void DatoAInterfazUsuario(object obj)
        {
            AutorizadorBO bo = (AutorizadorBO)obj;

            if (bo.Sucursal == null)
                bo.Sucursal = new SucursalBO();
            if (bo.Empleado == null)
                bo.Empleado = new EmpleadoBO();

            this.vista.AutorizadorID = bo.AutorizadorID;
            this.vista.SucursalID = bo.Sucursal.Id;
            this.vista.SucursalNombre = bo.Sucursal.Nombre;
            this.vista.EmpleadoID = bo.Empleado.Id;
            this.vista.EmpleadoNombre = bo.Empleado.NombreCompleto;
            if (bo.TipoAutorizacion != null)
                this.vista.TipoAutorizacion = bo.TipoAutorizacion;
            this.vista.SoloNotificacion = bo.SoloNotificacion;
            this.vista.Email = bo.Empleado.Email;
            this.vista.Telefono = (bo.Empleado.Telefonos == null || bo.Empleado.Telefonos.Count == 0) ? null : bo.Empleado.Telefonos[0].Numero;
            this.vista.Estatus = bo.Estatus;

            this.vista.UC = bo.UC;
            this.vista.UUA = bo.UUA;
            this.vista.UsuarioCreacion = this.ObtenerNombreEmpleado(bo.UC);
            this.vista.UsuarioEdicion = this.ObtenerNombreEmpleado(bo.UUA);
            this.vista.FC = bo.FC;
            this.vista.FUA = bo.FUA;
        }

        public void Regresar()
        {
            this.vista.LimpiarSesion();
            this.vista.RedirigirAConsulta();
        }
        public void IrAEditar()
        {
            try
            {
                this.vista.EstablecerPaqueteNavegacion("AutorizadorBO", new AutorizadorBO() { AutorizadorID = this.vista.AutorizadorID });
                this.vista.RedirigirAEditar();
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".IrAEditar: " + ex.Message);
            }
        }

        private string ObtenerNombreEmpleado(int? numeroEmpleado)
        {
            if (numeroEmpleado == null)
                return "";

            List<EmpleadoBO> empleadosBO = FacadeBR.ConsultarEmpleadoCompleto(FacadeBR.ObtenerConexion(), new EmpleadoBO() { Numero = numeroEmpleado });

            if (empleadosBO.Count == 0)
                return "";

            return (empleadosBO[0].NombreCompleto != null ? empleadosBO[0].NombreCompleto : "");
        }

        /// <summary>
        /// Método que cargará los tipos de autorizadores de acuerdo a la unidad operativa en la que se inició sesion
        /// </summary>
        private void CargarTiposAutorizador()
        {
            this.presentadorDetalle.EstablecerConfiguracionInicial(this.vista.UnidadOperativaID);
        }
        #endregion
    }
}
