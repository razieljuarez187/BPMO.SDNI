// Satisface al CU092 - Catálogo de Operadores
using System;
using System.Collections.Generic;
using BPMO.Basicos.BO;
using BPMO.Facade.SDNI.BR;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Comun.BR;
using BPMO.SDNI.Comun.VIS;

namespace BPMO.SDNI.Comun.PRE
{
    public class EditarOperadorPRE
    {
        #region Atributos
        private OperadorBR controlador;
        private IDataContext dctx;
        private string nombreClase = "EditarOperadorPRE";
        private ucOperadorPRE presentadorDetalle;
        private IEditarOperadorVIS vista;
        #endregion Atributos

        #region Constructor
        public EditarOperadorPRE(IEditarOperadorVIS vista, IucOperadorVIS vistaOperador)
        {
            try
            {
                this.vista = vista;
                this.controlador = new OperadorBR();
                this.presentadorDetalle = new ucOperadorPRE(vistaOperador);

                this.dctx = FacadeBR.ObtenerConexion();
            }
            catch (Exception ex)
            {
                this.vista.MostrarMensaje("Inconsistencias en la configuración", ETipoMensajeIU.ERROR, this.nombreClase + ".EditarOperadorPRE: " + ex.Message);
            }
        }
        #endregion Constructor

        #region Métodos
        public void RealizarPrimeraCarga()
        {
            try
            {
                this.EstablecerDatosNavegacion(this.vista.ObtenerPaqueteNavegacion("OperadorBO"));

                this.PrepararEdicion();

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
                    throw new Exception("Se esperaba un objeto en la navegación. No se puede identificar qué operador se desea consultar.");
                if (!(paqueteNavegacion is OperadorBO))
                    throw new Exception("Se esperaba un Operador.");

                OperadorBO bo = (OperadorBO)paqueteNavegacion;

                this.DatoAInterfazUsuario(bo);
            }
            catch (Exception ex)
            {
                this.DatoAInterfazUsuario(new OperadorBO());
                throw new Exception(this.nombreClase + ".EstablecerDatosNavegacion: " + ex.Message);
            }
        }
        private void ConsultarCompleto()
        {
            try
            {
                OperadorBO bo = new OperadorBO() { OperadorID = this.vista.OperadorID, Cliente = new CuentaClienteIdealeaseBO() { UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaID } } };

                List<OperadorBO> lst = this.controlador.ConsultarCompleto(this.dctx, bo);

                if (lst.Count < 1)
                    throw new Exception("No se encontró ningún registro que corresponda a la información proporcionada.");
                if (lst.Count > 1)
                    throw new Exception("La consulta devolvió más de un registro.");

                this.DatoAInterfazUsuario(lst[0]);
                this.vista.UltimoObjeto = lst[0];
            }
            catch (Exception ex)
            {
                this.DatoAInterfazUsuario(new OperadorBO());
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
                if (!this.ExisteAccion(acciones, "INSERTARCOMPLETO"))
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
                OperadorBO bo = (OperadorBO)InterfazUsuarioADato();

                //Se crea el objeto de seguridad
                UsuarioBO usuario = new UsuarioBO() { Id = this.vista.UsuarioID };
                AdscripcionBO adscripcion = new AdscripcionBO() { UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaID } };
                SeguridadBO seguridadBO = new SeguridadBO(Guid.Empty, usuario, adscripcion);

                this.controlador.Actualizar(this.dctx, bo, this.vista.UltimoObjeto as OperadorBO, seguridadBO);

                this.vista.LimpiarSesion();
                this.vista.EstablecerPaqueteNavegacion("OperadorBO", new OperadorBO() { OperadorID = bo.OperadorID });
                this.vista.RedirigirADetalle();
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".Editar:" + ex.Message);
            }
        }

        private void DatoAInterfazUsuario(OperadorBO bo)
        {
            if (bo.Cliente == null)
                bo.Cliente = new CuentaClienteIdealeaseBO();
            if (bo.Direccion == null)
                bo.Direccion = new DireccionPersonaBO();
            if (bo.Direccion.Ubicacion == null)
                bo.Direccion.Ubicacion = new UbicacionBO();
            if (bo.Direccion.Ubicacion.Ciudad == null)
                bo.Direccion.Ubicacion.Ciudad = new CiudadBO();
            if (bo.Direccion.Ubicacion.Estado == null)
                bo.Direccion.Ubicacion.Estado = new EstadoBO();
            if (bo.Direccion.Ubicacion.Municipio == null)
                bo.Direccion.Ubicacion.Municipio = new MunicipioBO();
            if (bo.Direccion.Ubicacion.Pais == null)
                bo.Direccion.Ubicacion.Pais = new PaisBO();
            if (bo.Licencia == null)
                bo.Licencia = new LicenciaBO();
                            
            this.vista.OperadorID = bo.OperadorID;
            this.vista.CuentaClienteID = bo.Cliente.Id;
            this.vista.CuentaClienteNombre = bo.Cliente.Nombre;
            this.vista.Nombre = bo.Nombre;
            this.vista.AñosExperiencia = bo.AñosExperiencia;
            this.vista.FechaNacimiento = bo.FechaNacimiento;
            this.vista.DireccionCalle = bo.Direccion.Calle;
            this.vista.DireccionCiudad = bo.Direccion.Ubicacion.Ciudad.Nombre;
            this.vista.DireccionCP = bo.Direccion.CodigoPostal;
            this.vista.DireccionEstado = bo.Direccion.Ubicacion.Estado.Nombre;
            if (bo.Licencia.Tipo != null)
                this.vista.LicenciaTipoID = (int?)bo.Licencia.Tipo.Value;
            else
                this.vista.LicenciaTipoID = null;
            this.vista.LicenciaFechaExpiracion = bo.Licencia.FechaExpiracion;
            this.vista.LicenciaNumero = bo.Licencia.Numero;
            this.vista.LicenciaEstado = bo.Licencia.Estado;
            this.vista.Estatus = bo.Estatus;

            this.vista.UUA = bo.UUA;
            this.vista.FUA = bo.FUA;
            this.vista.UC = bo.UC;
            this.vista.FC = bo.FC;
            
            this.vista.FechaDesactivacion = bo.FechaDesactivacion;
            this.vista.MotivoDesactivacion = bo.MotivoDesactivacion;
            this.vista.UsuarioDesactivacionID = bo.UsuarioDesactivacionID;
        }
        private object InterfazUsuarioADato()
        {
            OperadorBO bo = new OperadorBO();
            bo.Cliente = new CuentaClienteIdealeaseBO();
            bo.Cliente.UnidadOperativa = new UnidadOperativaBO();
            bo.Direccion = new DireccionPersonaBO();
            bo.Direccion.Ubicacion = new UbicacionBO();
            bo.Direccion.Ubicacion.Ciudad = new CiudadBO();
            bo.Direccion.Ubicacion.Estado = new EstadoBO();
            bo.Direccion.Ubicacion.Municipio = new MunicipioBO();
            bo.Direccion.Ubicacion.Pais = new PaisBO();
            bo.Licencia = new LicenciaBO();

            bo.OperadorID = this.vista.OperadorID;
            bo.Cliente.Id = this.vista.CuentaClienteID;
            bo.Cliente.Nombre = this.vista.CuentaClienteNombre;
            bo.Cliente.UnidadOperativa.Id = this.vista.UnidadOperativaID;
            bo.Nombre = this.vista.Nombre;
            bo.AñosExperiencia = this.vista.AñosExperiencia;
            bo.FechaNacimiento = this.vista.FechaNacimiento;
            bo.Direccion.Calle = this.vista.DireccionCalle;
            bo.Direccion.Ubicacion.Ciudad.Nombre = this.vista.DireccionCiudad;
            bo.Direccion.CodigoPostal = this.vista.DireccionCP;
            bo.Direccion.Ubicacion.Estado.Nombre = this.vista.DireccionEstado;
            if (this.vista.LicenciaTipoID != null)
                bo.Licencia.Tipo = (ETipoLicencia)Enum.Parse(typeof(ETipoLicencia), this.vista.LicenciaTipoID.ToString());
            else
                bo.Licencia.Tipo = null;
            bo.Licencia.FechaExpiracion = this.vista.LicenciaFechaExpiracion;
            bo.Licencia.Numero = this.vista.LicenciaNumero;
            bo.Licencia.Estado = this.vista.LicenciaEstado;
            bo.Estatus = this.vista.Estatus;

            bo.FUA = this.vista.FUA;
            bo.UUA = this.vista.UUA;
            bo.UC = this.vista.UC;
            bo.FC = this.vista.FC;

            bo.FechaDesactivacion = this.vista.FechaDesactivacion;
            bo.MotivoDesactivacion = this.vista.MotivoDesactivacion;
            bo.UsuarioDesactivacionID = this.vista.UsuarioDesactivacionID;

            return bo;
        }

        public string ValidarCampos()
        {
            string s;

            if ((s = this.presentadorDetalle.ValidarCampos()) != null)
                return s;

            return null;
        }

        public void LimpiarSesion()
        {
            this.vista.LimpiarSesion();
            this.presentadorDetalle.LimpiarSesion();
        }
        #endregion
    }
}