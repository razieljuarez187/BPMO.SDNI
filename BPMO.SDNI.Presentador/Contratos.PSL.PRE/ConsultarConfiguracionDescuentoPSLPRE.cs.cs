using System;
using System.Collections.Generic;
using BPMO.Basicos.BO;
using BPMO.Facade.SDNI.BOF;
using BPMO.Facade.SDNI.BR;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Contratos.PSL.BO;
using BPMO.SDNI.Contratos.PSL.BR;
using BPMO.SDNI.Contratos.PSL.VIS;


namespace BPMO.SDNI.Contratos.PSL.PRE {
    public class ConsultarConfiguracionDescuentoPSLPRE {
        #region propiedades
        private IConsultarConfiuracionDescuentoPSLVIS vista;
        private IDataContext dataContext = null;
        private ConfiguracionDescuentoPSLBR controlador;

        private string nombreClase = "ConsultarConfiguracionDescuento";
        #endregion

        #region Constructor
        public ConsultarConfiguracionDescuentoPSLPRE(IConsultarConfiuracionDescuentoPSLVIS vista) {
            try {
                this.vista = vista;
                this.controlador = new ConfiguracionDescuentoPSLBR();
                this.dataContext = FacadeBR.ObtenerConexion();
            } catch (Exception ex) {
                this.vista.MostrarMensaje("No se pudieron obtener los datos de conexión", ETipoMensajeIU.ERROR,
                        "No se encontraron los parámetros de conexión en la fuente de datos, póngase en contacto con el administrador del sistema.");
            }

        }
        #endregion

        #region Métodos
        public void PrepararBusqueda() {
            this.vista.LimpiarSesion();

            this.vista.PrepararBusqueda();

        }


        /// <summary>
        /// Recupera las sucursales a las que el usuario autenticado tiene permiso e acceder 
        /// </summary>
        private void CargarSucursalesAutorizadas() {

            if (this.vista.SucursalesAutorizadas != null)
                if (this.vista.SucursalesAutorizadas.Count > 0)
                    return;

            var lstSucursales = FacadeBR.ConsultarSucursalesSeguridad(this.dataContext,
                             new SeguridadBO(Guid.Empty, this.vista.Usuario,
                                 new AdscripcionBO { UnidadOperativa = this.vista.UnidadOperativa }));

            this.vista.SucursalesAutorizadas = lstSucursales.ConvertAll(x => (SucursalBO)x);

        }

        /// <summary>
        /// Realiza la búsqueda al la tabla ConfiguraciónDescuentos
        /// </summary>
        public void Consultar() {
            try {
                CargarSucursalesAutorizadas();
                ConfiguracionDescuentoBO bo = (ConfiguracionDescuentoBO)this.InterfazUsuarioADato();
                //bo.Sucursal = null;

                List<ConfiguracionDescuentoBO> lst = controlador.Consultar(dataContext, bo).ConvertAll(s => (ConfiguracionDescuentoBO)s);


                this.vista.Resultado = lst;
                this.vista.ActualizarResultado();

                if (lst.Count < 1)
                    this.vista.MostrarMensaje("La búsqueda no produjo resultados", ETipoMensajeIU.INFORMACION,
                        "No se encontraron coincidencias con los criterios ingresados. Verifique sus datos");

            } catch (Exception ex) {
                throw new Exception(this.nombreClase + ".Consultar:" + ex.Message);
            }
        }


        /// <summary>
        /// Se asignan valores de la lista a las propiedades del BO
        /// </summary>
        /// <returns></returns>
        private object InterfazUsuarioADato() {

            ConfiguracionDescuentoBO bo = new ConfiguracionDescuentoBO();



            bo.Cliente = new CuentaClienteIdealeaseBO();

            bo.Sucursal = new SucursalBO();

            bo.UnidadOperativa = new UnidadOperativaBO();

            bo.UnidadOperativa.Id = this.vista.UnidadOperativaId;


            if (this.vista.ClienteID != null)
                bo.Cliente.Id = this.vista.ClienteID;

            if (this.vista.SucursalID != null) {
                bo.sucursal.Id = this.vista.SucursalID;
            } else {
                //List<SucursalBO> lstSuc = FacadeBR.ConsultarSucursal(dataContext, new SucursalBO() { UnidadOperativa = new UnidadOperativaBO() { Id = bo.UnidadOperativa.Id } });

                bo.Sucursales = this.vista.SucursalesAutorizadas;

            }

            if (this.vista.FechaInicial != null)
                bo.FechaInicio = Convert.ToDateTime(this.vista.FechaInicial);

            if (this.vista.FechaFinal != null)
                bo.FechaFin = Convert.ToDateTime(this.vista.FechaFinal);

            switch (this.vista.Estatus) {
                case 1:
                    bo.Estado = true;
                    break;
                case 2:
                    bo.Estado = false;
                    break;
            }

            return bo;


        }

        /// <summary>
        /// muestra la los detalles del descuento.
        /// </summary>
        /// <param name="index"></param>
        public void VerDetalles(int index) {
            if (index >= this.vista.Resultado.Count || index < 0)
                throw new Exception("No se encontró el la configuración de descuentos.");

            ConfiguracionDescuentoBO bo = this.ObtenerListaClonada(this.vista.Resultado)[index];

            this.vista.LimpiarSesion();

            this.vista.EstablecerPaqueteNavegacion("ConfiguracionDescuentoBO", bo); //]Session[nombre]=value

            this.vista.RedirigirADetalles();
        }

        public void CambiarPaginaResultado(int nuevoIndicePagina) {
            this.vista.IndicePaginaResultado = nuevoIndicePagina;
            this.vista.ActualizarResultado();
        }

        #region Métodos para el Buscador

        public object PrepararBOBuscador(string catalogo) {
            object obj = null;

            switch (catalogo) {
                case "Sucursal":
                    SucursalBOF sucursal = new SucursalBOF();
                    sucursal.UnidadOperativa = new UnidadOperativaBO();
                    sucursal.UnidadOperativa.Id = this.vista.UnidadOperativaId;
                    sucursal.Nombre = this.vista.Sucursal;
                    sucursal.Usuario = new UsuarioBO() { Id = this.vista.UsuarioAutenticado };
                    obj = sucursal;
                    break;

                case "CuentaClienteIdealease":
                    var cliente = new CuentaClienteIdealeaseBO();
                    cliente.Nombre = this.vista.NombreCliente;
                    cliente.UnidadOperativa = new UnidadOperativaBO();
                    cliente.UnidadOperativa.Id = this.vista.UnidadOperativaId;
                    cliente.Activo = true;
                    obj = cliente;
                    break;
            }

            return obj;
        }

        public void DesplegarResultadoBuscador(string catalogo, object selecto) {
            switch (catalogo) {

                case "Sucursal":
                    SucursalBO sucursal = (SucursalBO)selecto;
                    if (sucursal != null && sucursal.Id != null)
                        this.vista.SucursalID = sucursal.Id;
                    else
                        this.vista.SucursalID = null;

                    if (sucursal != null && sucursal.Nombre != null)
                        this.vista.Sucursal = sucursal.Nombre;
                    else
                        this.vista.Sucursal = null;
                    break;

                case "CuentaClienteIdealease":
                    CuentaClienteIdealeaseBO cliente = (CuentaClienteIdealeaseBO)selecto;

                    if (cliente != null && cliente.Id != null)
                        this.vista.ClienteID = cliente.Id;
                    else
                        this.vista.ClienteID = null;

                    if (cliente != null && cliente.Nombre != null)
                        this.vista.NombreCliente = cliente.Nombre;
                    else
                        this.vista.NombreCliente = null;
                    break;
            }
        }


        #endregion

        #endregion

        /// <summary>
        /// Devuelve un clon de una lista de sesión
        /// </summary>
        /// <param name="lstSesion">variable de sesión</param>
        /// <returns>lista clonada</returns>
        public List<ConfiguracionDescuentoBO> ObtenerListaClonada(List<ConfiguracionDescuentoBO> lstSesion) {
            List<ConfiguracionDescuentoBO> lstTemporal = lstSesion;
            List<ConfiguracionDescuentoBO> lstClonada = new List<ConfiguracionDescuentoBO>();

            foreach (ConfiguracionDescuentoBO descuento in lstTemporal) {
                lstClonada.Add(descuento.Clone());
            }

            return lstClonada;

        }
    }
}