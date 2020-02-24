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
    public class DetalleOperadorPRE
    {
        #region Atributos
        private OperadorBR controlador;
        private IDataContext dctx;
        private string nombreClase = "DetalleOperadorPRE";
        private ucOperadorPRE presentadorDetalle;
        private IDetalleOperadorVIS vista;
        #endregion Atributos

        #region Constructor
        public DetalleOperadorPRE(IDetalleOperadorVIS vista, IucOperadorVIS vistaOperador)
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
                this.vista.MostrarMensaje("Inconsistencias en la configuración", ETipoMensajeIU.ERROR, nombreClase + ".DetalleOperadorPRE: " + ex.Message);
            }
        }
        #endregion Constructor

        #region Métodos
        public void RealizarPrimeraCarga()
        {
            try
            {
                this.PrepararVisualizacion();

                this.EstablecerDatosNavegacion(this.vista.ObtenerPaqueteNavegacion("OperadorBO"));
                this.vista.PermitirRegresar(this.vista.ObtenerPaqueteNavegacion("FiltrosOperador") != null);

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
                OperadorBO bo = (OperadorBO)this.InterfazUsuarioADato();

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
                if (!this.ExisteAccion(lst, "INSERTARCOMPLETO"))
                    this.vista.PermitirRegistrar(false);
                //Se valida si el usuario tiene permiso para editar
                if (!this.ExisteAccion(lst, "ACTUALIZAR"))
                    this.vista.PermitirEditar(false);
                //Se valida si el usuario tiene permiso para activar o desactivar 
                if (!this.ExisteAccion(lst, "UI BORRAR"))
                    this.vista.PermitirDesactivar(false);
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

            if (bo.Estatus != null)
                this.vista.EstablecerActivarDesactivar(!bo.Estatus.Value);
            else
                this.vista.EstablecerActivarDesactivar(false);
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
                bo.FUA = DateTime.Now;
                bo.UUA = this.vista.UsuarioID;
                bo.Estatus = this.vista.EstatusNuevo;
                if (this.vista.EstatusNuevo != null && this.vista.EstatusNuevo == false)
                {
                    bo.UsuarioDesactivacionID = this.vista.UsuarioID;
                    bo.FechaDesactivacion = DateTime.Now;
                }
                else
                {
                    bo.UsuarioDesactivacionID = null;
                    bo.FechaDesactivacion = null;
                    bo.MotivoDesactivacion = null;
                }

                //Se crea el objeto de seguridad
                UsuarioBO usuario = new UsuarioBO() { Id = this.vista.UsuarioID };
                AdscripcionBO adscripcion = new AdscripcionBO() { UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaID } };
                SeguridadBO seguridadBO = new SeguridadBO(Guid.Empty, usuario, adscripcion);

                this.controlador.Actualizar(this.dctx, bo, this.vista.UltimoObjeto as OperadorBO, seguridadBO);

                this.Regresar();
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".Editar:" + ex.Message);
            }
        }
        private string ValidarCampos()
        {
            string s;

            if ((s = this.presentadorDetalle.ValidarCampos()) != null)
                return s;

            if (this.vista.EstatusNuevo == null)
                s += "Estatus Nuevo, ";
            if (this.vista.EstatusNuevo != null && this.vista.EstatusNuevo == false)
            {
                if (this.vista.UsuarioID == null)
                    s += "Usuario de Desactivación, ";
                if (string.IsNullOrEmpty(this.vista.MotivoDesactivacion) || string.IsNullOrWhiteSpace(this.vista.MotivoDesactivacion))
                    s += "Motivo de Desactivación, ";
            }

            if (s != null && s.Trim().CompareTo("") != 0)
                return "Los siguientes campos no pueden estar vacíos: \n" + s.Substring(0, s.Length - 2);

            return null;
        }

        public void Regresar()
        {
            this.LimpiarSesion();
            this.vista.RedirigirAConsulta();
        }
        public void IrAEditar()
        {
            try
            {
                this.vista.EstablecerPaqueteNavegacion("OperadorBO", new OperadorBO() { OperadorID = this.vista.OperadorID });
                this.vista.RedirigirAEditar();
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".IrAEditar: " + ex.Message);
            }
        }

        public void LimpiarSesion()
        {
            this.vista.LimpiarSesion();
            this.presentadorDetalle.LimpiarSesion();
        }
        #endregion
    }
}