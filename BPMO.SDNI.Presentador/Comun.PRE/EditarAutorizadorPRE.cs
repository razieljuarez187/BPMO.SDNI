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
    public class EditarAutorizadorPRE
    {
        #region Atributos
        private AutorizadorBR controlador;
        private IDataContext dctx = null;

        private IEditarAutorizadorVIS vista;
        private IucAutorizadorVIS vistaDetalle;

        private ucAutorizadorPRE presentadorDetalle;

        private string nombreClase = "EditarAutorizadorPRE";
        #endregion

        #region Constructor
        public EditarAutorizadorPRE(IEditarAutorizadorVIS view, IucAutorizadorVIS viewDatosAutorizador)
        {
            try
            {
                this.vista = view;
                this.vistaDetalle = viewDatosAutorizador;

                this.presentadorDetalle = new ucAutorizadorPRE(viewDatosAutorizador);

                this.controlador = new AutorizadorBR();
                this.dctx = FacadeBR.ObtenerConexion();
            }
            catch (Exception ex)
            {
                this.vista.MostrarMensaje("Inconsistencia en los parámetros de configuración", ETipoMensajeIU.ERROR, this.nombreClase + ".EditarAutorizadorPRE:" + ex.Message);
            }
        }
        #endregion

        #region Métodos
        public void RealizarPrimeraCarga()
        {
            try
            {
                this.EstablecerDatosNavegacion(this.vista.ObtenerPaqueteNavegacion("AutorizadorBO"));

                this.PrepararEdicion();
                this.CargarTiposAutorizador();
                this.ConsultarCompleto();

                this.EstablecerConfiguracionInicial();
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
                AutorizadorBO bo = new AutorizadorBO() { AutorizadorID = this.vista.AutorizadorID };

                List<AutorizadorBO> lst = this.controlador.ConsultarCompleto(this.dctx, bo);

                if (lst.Count < 1)
                    throw new Exception("No se encontró ningún registro que corresponda a la información proporcionada.");
                if (lst.Count > 1)
                    throw new Exception("La consulta devolvió más de un registro.");

                this.DatoAInterfazUsuario(lst[0]);
                this.vista.UltimoObjeto = lst[0];
            }
            catch (Exception ex)
            {
                this.DatoAInterfazUsuario(new AutorizadorBO());
                throw new Exception(this.nombreClase + ".ConsultarCompleto:" + ex.Message);
            }
        }
        private void EstablecerConfiguracionInicial()
        {
            this.vista.FUA = DateTime.Now;
            this.vista.UUA = this.vista.UsuarioID;
        }

        public void ValidarAcceso()
        {
            try
            {
                //Valida que usuario y la unidad operativa no sean nulos
                if (this.vista.UsuarioID == null) throw new Exception("El identificador del usuario no debe ser nulo");
                if (this.vista.UnidadOperativaID == null) throw new Exception("El identificador de la Unidad Operativa no debe ser nulo");
                //Se crea el objeto de seguridad
                UsuarioBO usuario = new UsuarioBO() { Id = this.vista.UsuarioID };
                AdscripcionBO adscripcion = new AdscripcionBO() { UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaID } };
                SeguridadBO seguridadBO = new SeguridadBO(Guid.Empty, usuario, adscripcion);

                //Se valida si el usuario tiene permiso a la acción principal
                if (!FacadeBR.ExisteAccion(this.dctx, "ACTUALIZAR", seguridadBO))
                    this.vista.RedirigirSinPermisoAcceso();
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".ValidarAcceso: " + ex.Message);
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
                List<CatalogoBaseBO> acciones = FacadeBR.ConsultarAccion(dctx, seguridad);

                // se valida si el usuario tiene permisos para registrar
                if (!this.ExisteAccion(acciones, "INSERTAR"))
                    this.vista.PermitirRegistrar(false);
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

        public void PrepararEdicion()
        {
            this.vista.PrepararEdicion();
            this.presentadorDetalle.PrepararEdicion();
        }

        public void Cancelar()
        {
            this.LimpiarSesion();
            this.vista.RedirigirADetalle();
        }
        public void Editar()
        {
            string s;

            if ((s = this.ValidarCampos()) != null)
            {
                this.vista.MostrarMensaje(s, ETipoMensajeIU.ADVERTENCIA, null);
                return;
            }

            try
            {
                AutorizadorBO bo = (AutorizadorBO)InterfazUsuarioADato();

                //Se crea el objeto de seguridad
                UsuarioBO usuario = new UsuarioBO() { Id = this.vista.UsuarioID };
                AdscripcionBO adscripcion = new AdscripcionBO() { UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaID } };
                SeguridadBO seguridadBO = new SeguridadBO(Guid.Empty, usuario, adscripcion);

                this.controlador.Actualizar(this.dctx, bo, this.vista.UltimoObjeto as AutorizadorBO, seguridadBO);

                this.vista.LimpiarSesion();
                this.vista.EstablecerPaqueteNavegacion("AutorizadorBO", new AutorizadorBO() { AutorizadorID = bo.AutorizadorID });
                this.vista.RedirigirADetalle();
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".Editar:" + ex.Message);
            }
        }
        
        private void DatoAInterfazUsuario(AutorizadorBO bo)
        {
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
            this.vista.Telefono = this.vista.Telefono = (bo.Empleado.Telefonos == null || bo.Empleado.Telefonos.Count == 0) ? null : bo.Empleado.Telefonos[0].Numero;
            this.vista.Estatus = bo.Estatus;
            this.vista.UC = bo.UC;
            this.vista.FC = bo.FC;
            this.vista.UUA = bo.UUA;
            this.vista.FUA = bo.FUA;           
        }
        private object InterfazUsuarioADato()
        {
            AutorizadorBO bo = new AutorizadorBO();
            bo.Sucursal = new SucursalBO();
            bo.Sucursal.UnidadOperativa = new UnidadOperativaBO();
            bo.Empleado = new EmpleadoBO();

            bo.AutorizadorID = this.vista.AutorizadorID;
            bo.Sucursal.Id = this.vista.SucursalID;
            bo.Sucursal.Nombre = this.vista.SucursalNombre;
            bo.Empleado.Id = this.vista.EmpleadoID;
            bo.Empleado.NombreCompleto = this.vista.EmpleadoNombre;
            if (this.vista.TipoAutorizacion != null)
                switch (this.vista.UnidadOperativaID)
                {
                    case (int)ETipoEmpresa.Generacion:
                        bo.TipoAutorizacion = (ETipoAutorizacionGeneracion)Enum.Parse(typeof(ETipoAutorizacionGeneracion), this.vista.TipoAutorizacion.ToString());
                        break;
                    case (int)ETipoEmpresa.Construccion:
                        bo.TipoAutorizacion = (ETipoAutorizacionConstruccion)Enum.Parse(typeof(ETipoAutorizacionConstruccion), this.vista.TipoAutorizacion.ToString());
                        break;
                    case (int)ETipoEmpresa.Equinova:
                        bo.TipoAutorizacion = (ETipoAutorizacionEquinova)Enum.Parse(typeof(ETipoAutorizacionEquinova), this.vista.TipoAutorizacion.ToString());
                        break;
                    default:
                        bo.TipoAutorizacion = (ETipoAutorizacion)Enum.Parse(typeof(ETipoAutorizacion), this.vista.TipoAutorizacion.ToString());
                        break;
                }
            bo.SoloNotificacion = this.vista.SoloNotificacion;
            bo.Empleado.Email = this.vista.Email;
            bo.Empleado.RFC = this.vista.Telefono;
            bo.Estatus = this.vista.Estatus;
            bo.FUA = this.vista.FUA;
            bo.UUA = this.vista.UUA;
            bo.FC = this.vista.FC;
            bo.UC = this.vista.UC;
            
            return bo;
        }

        public string ValidarCampos()
        {
            string s = null;

            if (this.vista.SucursalID == null)
                s += "Sucursal, ";
            if (this.vista.EmpleadoID == null)
                s += "Empleado, ";
            if (this.vista.Email == null)
                s += "Email, ";
            if (this.vista.TipoAutorizacion == null)
                s += "Tipo de Autorización, ";
            if (this.vista.SoloNotificacion == null)
                s += "Notificación, ";
            if (this.vista.Estatus == null)
                s += "Estatus, ";
            if (this.vista.UC == null)
                s += "Usuario de Creación, ";
            if (this.vista.FC == null)
                s += "Fecha de Creación, ";
            if (this.vista.UUA == null)
                s += "Usuario de Última Actualización, ";
            if (this.vista.FUA == null)
                s += "Fecha de Última Actualización, ";

            if (s != null)
                return "Los siguientes campos no pueden estar vacíos: \n" + s.Substring(0, s.Length - 2);

            return null;
        }

        public void LimpiarSesion()
        {
            this.vista.LimpiarSesion();
            this.presentadorDetalle.LimpiarSesion();
        }

        /// <summary>
        /// Método que cargará los tipos de autorizadores de acuerdo a la unidad operativa en la que se inició sesion
        /// </summary>
        private void CargarTiposAutorizador()
        {
            this.presentadorDetalle.EstablecerConfiguracionInicial(this.vista.UnidadOperativaID);
            this.presentadorDetalle.ConfigurarSoloNotificacion();
        }
        #endregion
    }
}
