// Satisface al CU092 - Catálogo de Operadores
using System;
using System.Collections.Generic;
using System.Linq;
using BPMO.Basicos.BO;
using BPMO.Facade.SDNI.BR;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Comun.BR;
using BPMO.SDNI.Comun.VIS;

namespace BPMO.SDNI.Comun.PRE
{
    public class RegistrarOperadorPRE
    {
        #region Atributos
        private string nombreClase = "RegistrarOperadorPRE";
        private readonly IDataContext dctx;
        private readonly IRegistrarOperadorVIS vista;
        private OperadorBR controlador;
        private ucOperadorPRE presentadorDetalle;
        #endregion

        #region Constructor
        public RegistrarOperadorPRE(IRegistrarOperadorVIS vista, IucOperadorVIS vistaOperador)
        {
            try
            {
                this.vista = vista;
                this.presentadorDetalle = new ucOperadorPRE(vistaOperador);
                this.controlador = new OperadorBR();
                this.dctx = FacadeBR.ObtenerConexion();
            }
            catch (Exception ex)
            {
                this.vista.MostrarMensaje("Inconsistencias en los parámetros de configuración", ETipoMensajeIU.ERROR, this.nombreClase + ".RegistrarOperadorPRE: " + ex.Message);
            }
        }
        #endregion Constructor

        #region Métodos
        public void PrepararNuevo()
        {
            this.LimpiarSesion();

            this.vista.PrepararNuevo();
            this.presentadorDetalle.PrepararNuevo();

            this.EstablecerSeguridad();
        }

        private void EstablecerSeguridad()
        {
            try
            {
                //se valida que los datos del usuario y la unidad operativa no sean nulos
                if (this.vista.UC == null) throw new Exception("El identificador del usuario no debe ser nulo");
                if (this.vista.UnidadOperativaID == null) throw new Exception("La Unidad Operativa no debe ser nula ");

                //Se crea el objeto de seguridad
                UsuarioBO usuario = new UsuarioBO() { Id = this.vista.UC };
                AdscripcionBO adscripcion = new AdscripcionBO() { UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaID } };
                SeguridadBO seguridadBO = new SeguridadBO(Guid.Empty, usuario, adscripcion);

                //Se obtienen las acciones a las cuales el usuario tiene permiso en este proceso
                List<CatalogoBaseBO> lst = FacadeBR.ConsultarAccion(this.dctx, seguridadBO);

                //Se valida si el usuario tiene permiso para insertar operador
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

        private void DatoAInterfazUsuario(OperadorBO bo)
        {
            if (bo == null)
                bo = new OperadorBO();

            this.vista.OperadorID = bo.OperadorID;
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

            return bo;
        }

        public void AgregarOperador()
        {
            string s;

            if ((s = this.ValidarCampos()) != null)
            {
                this.vista.MostrarMensaje(s, ETipoMensajeIU.ADVERTENCIA, null);
                return;
            }

            try
            {
                List<OperadorBO> operadores = new List<OperadorBO>(vista.Operadores);
                OperadorBO operador = (OperadorBO)this.InterfazUsuarioADato();

                operador.FC = this.vista.FC;
                operador.UC = this.vista.UC;
                operador.FUA = this.vista.FUA;
                operador.UUA = this.vista.UUA;
                operador.Estatus = true;

                operadores.Add(operador);

                this.vista.Operadores = operadores;

                this.presentadorDetalle.PrepararNuevo();
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".AgregarOperador: " + ex.Message);
            }
        }
        public void QuitarOperador(int index)
        {
            try
            {
                if (index >= this.vista.Operadores.Count || index < 0)
                    throw new Exception("No se encontró el representante legal seleccionado.");

                List<OperadorBO> lst = this.vista.Operadores;
                lst.RemoveAt(index);

                this.vista.Operadores = lst;
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".QuitarOperador: " + ex.Message);
            }
        }

        public void Cancelar()
        {
            this.LimpiarSesion();
            this.vista.RedirigirAConsulta();
        }
        public void Registrar()
        {
            if (this.vista.Operadores.Count <= 0)
            {
                this.vista.MostrarMensaje("No hay operadores para guardar", ETipoMensajeIU.INFORMACION);
                return;
            }

            try
            {
                //Se crea el objeto de seguridad
                UsuarioBO usuario = new UsuarioBO() { Id = this.vista.UC };
                AdscripcionBO adscripcion = new AdscripcionBO() { UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaID } };
                SeguridadBO seguridadBO = new SeguridadBO(Guid.Empty, usuario, adscripcion);

                this.controlador.InsertarCompleto(this.dctx, this.vista.Operadores, seguridadBO);

                this.vista.RedirigirAConsulta();
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".Registrar: " + ex.Message);
            }
        }

        private bool ExisteLicencia(string NumeroLicencia)
        {
            List<OperadorBO> operadores = new List<OperadorBO>(vista.Operadores);
            return operadores.Any(x => x.Licencia.Numero == NumeroLicencia);
        }
        public string ValidarCampos()
        {
            string s;

            if ((s = this.presentadorDetalle.ValidarCampos()) != null)
                return s;

            List<OperadorBO> operadores = new List<OperadorBO>(vista.Operadores);
            if (!(!this.ExisteLicencia(this.vista.LicenciaNumero) && !controlador.ExisteLicencia(dctx, new OperadorBO { Licencia = new LicenciaBO { Numero = this.vista.LicenciaNumero } })))
                return "El número de licencia proporcionado ya ha sido registrado.";

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