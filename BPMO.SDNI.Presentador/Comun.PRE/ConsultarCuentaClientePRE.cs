//Satiface al caso de uso CU068 - Catálogo de Clientes
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
    public class ConsultarCuentaClientePRE
    {
        #region Atributos
        private IDataContext dctx;
        private IConsultarCuentaClienteVIS vista;
        private CuentaClienteIdealeaseBR clienteBR;
        private const string nombreClase = "ConsultarClientePRE";
        #endregion

        #region Constructor
        public ConsultarCuentaClientePRE(IConsultarCuentaClienteVIS vista)
        {
            try
            {
                this.vista = vista;
                dctx = FacadeBR.ObtenerConexion();
                clienteBR = new CuentaClienteIdealeaseBR();

            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencias en la configuracion",ETipoMensajeIU.ERROR, nombreClase + ex.Message);
            }

        }
        #endregion

        #region Métodos
        public void Inicializar()
        {
            this.vista.Nombre = null;
            this.vista.RFC = null;
            this.vista.Fisica = null;
            this.vista.NombreCuenta = null;
            this.LimpiarSesion();
            this.EstablecerSeguridad();
        }

        public CuentaClienteIdealeaseBO PrepararBO()
        {
            CuentaClienteIdealeaseBO cliente = new CuentaClienteIdealeaseBO
            {
                Id = this.vista.CuentaClienteID,
                UnidadOperativa = this.vista.UnidadOperativa,
                Nombre=this.vista.Nombre,
                Cliente = new ClienteBO { Fisica = this.vista.Fisica, RFC = this.vista.RFC, Id = this.vista.ClienteID }
            };

            return cliente;
        }

        public void ConsultarClientes()
        {
            try
            {
                CuentaClienteIdealeaseBO cliente = this.PrepararBO();                
                List<CuentaClienteIdealeaseBO> lstClientes = clienteBR.Consultar(dctx,cliente);                
                this.vista.ListaClientes = lstClientes;                
                this.MostrarDatos();
                
                if (lstClientes.Count == 0)
                    this.vista.MostrarMensaje("La búsqueda no produjo resultados", ETipoMensajeIU.INFORMACION,
                        "No se encontraron coincidencias con los criterios ingresados. Verifique sus datos");
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Error al intentar consultar los clientes",ETipoMensajeIU.ERROR,nombreClase + ".ConsultarClientes:" + ex.Message);
            }
        }

        public void VerDetalles(int index)
        {
            if (index >= this.vista.ListaClientes.Count || index < 0)
                throw new Exception("No se encontró el cliente seleccionado.");
            CuentaClienteIdealeaseBO bo = this.vista.ListaClientes[index];
            this.LimpiarSesion();
            this.vista.EstablecerPaqueteNavegacion("DatosCuentaClienteIdealeaseBO",bo);
            this.vista.RedirigirADetalles();
        }
        public void MostrarDatos()
        {
            this.vista.MostrarDatos();
        }

        public void LimpiarSesion()
        {
            this.vista.LimpiarSesion();
        }

        public void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null)
        {
            this.vista.MostrarMensaje(mensaje, tipo, detalle);
        }

        #region SC_0008
        private void EstablecerSeguridad()
        {
            try
            {
                //se valida que los datos del usuario y la unidad operativa no sean nulos
                if (this.vista.UsuarioId == null) throw new Exception("El identificador del usuario no debe ser nulo");
                if (this.vista.UnidadOperativa == null) throw new Exception("La Unidad Operativa no debe ser nula ");
                if (this.vista.UnidadOperativa.Id == null) throw new Exception("El identificador de la Unidad Operativa no debe ser nulo ");

                //Se crea el objeto de seguridad
                UsuarioBO usuario = new UsuarioBO() { Id = this.vista.UsuarioId };
                AdscripcionBO adscripcion = new AdscripcionBO() { UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativa.Id } };
                SeguridadBO seguridadBO = new SeguridadBO(Guid.Empty, usuario, adscripcion);

                //Se obtienen las acciones a las cuales el usuario tiene permiso en este proceso
                List<CatalogoBaseBO> lst = FacadeBR.ConsultarAccion(this.dctx, seguridadBO);

                //Se valida si el usuario tiene permiso para consultar
                if (!this.ExisteAccion(lst, "CONSULTAR"))
                    this.vista.RedirigirSinPermisoAcceso();
                //Se valida si el usuario tiene permiso para insertar cuenta cliente
                if (!this.ExisteAccion(lst, "INSERTARCOMPLETO"))
                    this.vista.PermitirRegistrar(false);
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

        #region MetodosBuscador
        /// <summary>
        /// Prepara un BO para la Busqueda en su respectivo catalogo
        /// </summary>
        /// <param name="catalogo">catalogo donde se realizara la busqueda</param>
        /// <returns></returns>
        public object PrepararBOBuscador(string catalogo)
        {
            object obj = null;

            switch (catalogo)
            {
                case "Cliente":
                    CuentaClienteBO cliente = new CuentaClienteBO { Nombre = vista.NombreCuenta, UnidadOperativa = vista.UnidadOperativa, Cliente = new ClienteBO(), Activo = true}; // Se agrega que la Cuenta del Cliente este Activa
                    obj = cliente;
                    break;
            }

            return obj;
        }

        /// <summary>
        /// Despliega el Resultado del Buscador
        /// </summary>
        /// <param name="catalogo">Catalogo en el que se realizo la busqueda</param>
        /// <param name="selecto">Objeto Resultante</param>
        public void DesplegarResultadoBuscador(string catalogo, object selecto)
        {
            switch (catalogo)
            {
                case "Cliente":
                    CuentaClienteBO cliente = (CuentaClienteBO)selecto ?? new CuentaClienteBO();
                    if (cliente.Cliente == null)
                        cliente.Cliente = new ClienteBO();
                    this.vista.Nombre = !string.IsNullOrEmpty(cliente.Nombre) ? cliente.Nombre : string.Empty;
                    this.vista.RFC = cliente.Cliente.RFC;
                    this.vista.Fisica = cliente.Cliente.Fisica;
                    this.vista.NombreCuenta = !string.IsNullOrEmpty(cliente.Nombre) ? cliente.Nombre : string.Empty;
                    this.vista.CuentaClienteID = cliente.Id;
                    this.vista.ClienteID = cliente.Cliente.Id;
                    break;
            }
        }
        #endregion
        #endregion
    }
}