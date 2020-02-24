//Satisface al caso de uso CU098 - Configurar Autorizadores para los Contratos
using System;
using System.Collections.Generic;
using System.Data;
using BPMO.Basicos.BO;
using BPMO.Facade.SDNI.BR;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Comun.BR;
using BPMO.SDNI.Comun.VIS;

namespace BPMO.SDNI.Comun.PRE
{
    public class RegistrarAutorizadorPRE
    {
        #region Atributos
        private AutorizadorBR controlador;
        private IDataContext dctx = null;

        private IRegistrarAutorizadorVIS vista;
        private IucAutorizadorVIS vistaDetalle;

        private ucAutorizadorPRE presentadorDetalle;

        private string nombreClase = "RegistrarAutorizadorPRE";
        #endregion

        #region Constructores
        public RegistrarAutorizadorPRE(IRegistrarAutorizadorVIS view, IucAutorizadorVIS viewDetalle)
        {
            try
            {
                this.vista = view;
                this.vistaDetalle = viewDetalle;

                this.presentadorDetalle = new ucAutorizadorPRE(viewDetalle);

                this.controlador = new AutorizadorBR();
                this.dctx = FacadeBR.ObtenerConexion();
            }
            catch (Exception ex)
            {
                this.vista.MostrarMensaje("Inconsistencia en los parámetros de configuración", ETipoMensajeIU.ERROR, this.nombreClase + ".RegistrarAutorizadorPRE: " + ex.Message);
            }
        }
        #endregion

        #region Métodos
        public void PrepararNuevo()
        {
            this.LimpiarSesion();

            this.vista.PrepararNuevo();
            this.presentadorDetalle.PrepararNuevo();
            this.EstablecerConfiguracionInicial();
            this.presentadorDetalle.ConfigurarSoloNotificacion();
            this.EstablecerSeguridad();
        }
        private void EstablecerConfiguracionInicial()
        {
            this.vista.Estatus = true;
            this.vista.SoloNotificacion = false;
            this.presentadorDetalle.EstablecerConfiguracionInicial(this.vista.UnidadOperativaID);
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

                //Se valida si el usuario tiene permisos para registrar un nuevo acta de nacimiento
                if (!this.ExisteAccion(acciones, "INSERTAR"))
                    this.vista.RedirigirSinPermisoAcceso();
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

        private object InterfazUsuarioADato()
        {
            AutorizadorBO bo = new AutorizadorBO
            {
                Sucursal = new SucursalBO { Id = vista.SucursalID },
                Empleado = new EmpleadoBO { Id = vista.EmpleadoID },

            };
            bo.Sucursal.UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaID };
            #region Tipo de Autorización
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
            #endregion
            bo.SoloNotificacion = this.vista.SoloNotificacion;
            bo.Estatus = this.vista.Estatus;
            bo.FC = this.vista.FC;
            bo.FUA = this.vista.FUA;
            bo.UC = this.vista.UC;
            bo.UUA = this.vista.UUA;

            return bo;
        }
        private void DatoAInterfazUsuario(object obj)
        {
            AutorizadorBO bo = (AutorizadorBO)obj;

            this.vista.AutorizadorID = bo.AutorizadorID;
        }

        public void Registrar()
        {
            string s;

            if ((s = this.ValidarCampos()) != null)
            {
                this.vista.MostrarMensaje(s, ETipoMensajeIU.ADVERTENCIA, null);
                return;
            }

            try
            {
                AutorizadorBO bo = (AutorizadorBO)this.InterfazUsuarioADato();

                //Se crea el objeto de seguridad
                UsuarioBO usuario = new UsuarioBO() { Id = this.vista.UsuarioID };
                AdscripcionBO adscripcion = new AdscripcionBO() { UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaID } };
                SeguridadBO seguridadBO = new SeguridadBO(Guid.Empty, usuario, adscripcion);

                this.controlador.Insertar(this.dctx, bo, seguridadBO);

                //Se consulta lo insertado para recuperar los ID
                DataSet ds = this.controlador.ConsultarSet(this.dctx, bo);
                if (ds.Tables[0].Rows.Count <= 0)
                    throw new Exception("Al consultar lo insertado no se encontraron coincidencias.");
                if (ds.Tables[0].Rows.Count > 1)
                    throw new Exception("Al consultar lo insertado se encontró más de una coincidencia.");

                bo.AutorizadorID = this.controlador.DataRowToAutorizadorBO(ds.Tables[0].Rows[0]).AutorizadorID;

                this.LimpiarSesion();
                this.vista.EstablecerPaqueteNavegacion("AutorizadorBO", bo);
                this.vista.RedirigirADetalles();
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".Registrar: " + ex.Message);
            }
        }
        public void Cancelar()
        {
            this.LimpiarSesion();
            this.vista.RedirigirAConsulta();
        }
        
        private string ValidarCampos()
        {
            string s;

            if ((s = this.presentadorDetalle.ValidarCampos()) != null)
                return s;

            if (this.vista.UnidadOperativaID == null)
                s += "Unidad Operativa, ";
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

        private void LimpiarSesion()
        {
            this.vista.LimpiarSesion();
            this.presentadorDetalle.LimpiarSesion();
        }
        #endregion
    }
}
