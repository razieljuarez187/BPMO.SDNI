using System;
using System.Collections.Generic;
using BPMO.Basicos.BO;
using BPMO.Facade.SDNI.BR;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Contratos.PSL.BO;
using BPMO.SDNI.Contratos.PSL.BR;
using BPMO.SDNI.Contratos.PSL.VIS;

namespace BPMO.SDNI.Contratos.PSL.PRE {
    public class DetalleConfiguracionDescuentoPSLPRE {

        #region Atributos
        /// <summary>
        /// Nombre de la Clase, usado para Excepciones
        /// </summary>
        private string nombreClase = "DetalleConfiguracionDescuentoPSLPRE";

        private IDetalleConfiguracionDescuentoPSLVIS vista;
        /// <summary>
        /// Vista de Detalle de descuentos
        /// </summary>
        internal IDetalleConfiguracionDescuentoPSLVIS Vista { get { return vista; } }
        /// <summary>
        /// El DataContext que provee acceso a la Base de Datos
        /// </summary>
        private IDataContext dctx;
        /// <summary>
        /// BR de configuración descuentos
        /// </summary>
        private ConfiguracionDescuentoPSLBR configuracionDescuentoBR;

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor del detalle de configuración de descuento
        /// </summary>
        /// <param name="vistaActual">Vista de la interfaz de detalle</param>
        public DetalleConfiguracionDescuentoPSLPRE(IDetalleConfiguracionDescuentoPSLVIS vistaActual) {
            try {
                this.vista = vistaActual;
                dctx = FacadeBR.ObtenerConexion();
            } catch (Exception ex) {
                throw new Exception(nombreClase + ".DetalleConfiguracionDescuentoPSLPRE:Error al configurar el presentador");
            }
        }
        #endregion

        #region Métodos
        /// <summary>
        /// Cambia el índice de la tabla
        /// </summary>
        /// <param name="nuevoIndicePagina">numero de índice</param>
        public void CambiarPaginaResultado(int nuevoIndicePagina) {
            this.vista.IndicePaginaResultado = nuevoIndicePagina;
            this.vista.ActualizarResultado();
        }
        /// <summary>
        /// Método que se realizara al inicializar el UI de detalle
        /// </summary>
        public void RealizarPrimeraCarga() {
            try {
                if (this.vista.ObtenerDatosNavegacion() == null)
                    throw new Exception("Se esperaba Datos de Navegación");
                if (!(this.vista.ObtenerDatosNavegacion() is ConfiguracionDescuentoBO))
                    throw new Exception("Se esperaba un objeto de ConfiguracionDescuentoBO");

                ConfiguracionDescuentoBO configuracionDescuento = (ConfiguracionDescuentoBO)this.vista.ObtenerDatosNavegacion();
                // Agarramos los valores que enviaremos a la consulta
                ConfiguracionDescuentoBO configuracionDescuentoDetalle = new ConfiguracionDescuentoBO();
                configuracionDescuentoDetalle.Cliente = new CuentaClienteIdealeaseBO() { Id = configuracionDescuento.Cliente.Id };
                configuracionDescuentoDetalle.ContactoComercial = configuracionDescuento.ContactoComercial;

                List<ConfiguracionDescuentoBO> lst = new List<ConfiguracionDescuentoBO>();
                this.configuracionDescuentoBR = new ConfiguracionDescuentoPSLBR();
                //Obtenemos una lista con las filas que tengan el mismo cliente y contacto comercial
                lst = configuracionDescuentoBR.Consultar(dctx, configuracionDescuentoDetalle).ConvertAll(s => (ConfiguracionDescuentoBO)s);
                //pasamos los valores a la tabla
                this.vista.Resultado = lst;
                this.vista.ActualizarResultado();

                //Pasamos a la interfaz de usuario el BO de descuentos
                this.DatoAInterfazUsuario(configuracionDescuento);
                this.vista.LimpiarSesion();
                this.EstablecerSeguridad();
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencias al momento de obtener los datos de Configuración descuento", ETipoMensajeIU.ERROR, nombreClase + ".RealizarPrimeraCarga:" + ex.Message);
                DatoAInterfazUsuario(new ConfiguracionDescuentoBO());
                this.vista.PermitirEditar(false);
            }
        }
        /// <summary>
        /// Valida si el usuario tiene permiso para consultar
        /// </summary>
        public void ValidarAcceso() {
            try {
                //se valida que los datos del usuario y la unidad operativa no sean nulos
                if (this.vista.UsuarioID == null) throw new Exception("El identificador del usuario no debe ser nulo");
                if (this.vista.UnidadOperativaId == null) throw new Exception("El identificador de la Unidad Operativa no debe ser nulo ");

                //Se crea el objeto de seguridad
                UsuarioBO usuario = new UsuarioBO() { Id = this.vista.UsuarioID };
                AdscripcionBO adscripcion = new AdscripcionBO() { UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaId } };
                SeguridadBO seguridadBO = new SeguridadBO(Guid.Empty, usuario, adscripcion);

                //Se valida si el usuario tiene permiso a la acción principal
                if (!FacadeBR.ExisteAccion(dctx, "CONSULTAR", seguridadBO))
                    this.vista.RedirigirSinPermisoAcceso();
            } catch (Exception ex) {
                throw new Exception(nombreClase + ".ValidarAcceso:" + ex.Message);
            }
        }
        /// <summary>
        /// Verifica si tiene permiso para editar
        /// </summary>
        public void EstablecerSeguridad() {
            try {
                //se valida que los datos del usuario y la unidad operativa no sean nulos
                if (this.vista.UsuarioID == null) throw new Exception("El identificador del usuario no debe ser nulo");
                if (this.vista.UnidadOperativaId == null) throw new Exception("El identificador de la Unidad Operativa no debe ser nulo ");

                //Se crea el objeto de seguridad
                UsuarioBO usuario = new UsuarioBO() { Id = this.vista.UsuarioID };
                AdscripcionBO adscripcion = new AdscripcionBO() { UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaId } };
                SeguridadBO seguridadBO = new SeguridadBO(Guid.Empty, usuario, adscripcion);

                //Se obtienen las acciones a las cuales el usuario tiene permiso en este proceso
                List<CatalogoBaseBO> lst = FacadeBR.ConsultarAccion(dctx, seguridadBO);

                //Verifica si tiene permiso para editar
                if (!this.ExisteAccion(lst, "ACTUALIZARCOMPLETO"))
                    this.vista.PermitirEditar(false);

            } catch (Exception ex) {
                throw new Exception(nombreClase + ".EstablecerSeguridad:" + ex.Message);
            }
        }
        /// <summary>
        /// Valida si el usuario puede hacer esa acción
        /// </summary>
        /// <param name="acciones">Lista de acciones</param>
        /// <param name="accion">Acción a comprobar</param>
        /// <returns></returns>
        private bool ExisteAccion(List<CatalogoBaseBO> acciones, string accion) {
            if (acciones != null && acciones.Count > 0 && acciones.Exists(p => p != null && p.Nombre != null && p.Nombre.Trim().ToUpper() == accion.Trim().ToUpper()))
                return true;

            return false;
        }
        /// <summary>
        /// Mapea el objeto de descuento a la UI de detalle
        /// </summary>
        /// <param name="configuracionDescuento">Objeto de descuento a mapear</param>
        private void DatoAInterfazUsuario(ConfiguracionDescuentoBO configuracionDescuento) {
            if (configuracionDescuento.Cliente.Id != null)
                this.vista.ClienteID = configuracionDescuento.Cliente.Id;
            this.vista.Cliente = configuracionDescuento.Cliente.Nombre;
            if (configuracionDescuento.ContactoComercial != null) {
                this.vista.ContactoComercial = configuracionDescuento.ContactoComercial;

            }
            if (configuracionDescuento.Sucursal.Id != null) {
                this.vista.SucursalID = configuracionDescuento.Sucursal.Id;

            } if (configuracionDescuento.ConfiguracionDescuentoID != null) {
                this.vista.ConfiguracionDescuentoID = configuracionDescuento.ConfiguracionDescuentoID;

            }
            //if (configuracionDescuento.Modelo.Id != null)
            //{
            //    this.vista.ModeloID = configuracionDescuento.Modelo.Id;

            //}
            this.vista.ModoDetalle(true);
        }
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
        /// <summary>
        /// Muestra un mensaje en la UI
        /// </summary>
        /// <param name="mensaje">leyenda del mensaje</param>
        /// <param name="tipo">tipo de mensaje</param>
        /// <param name="detalle"></param>
        private void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null) {
            this.vista.MostrarMensaje(mensaje, tipo, detalle);
        }
        #region Métodos de los eventos de los botones
        /// <summary>
        /// Manda el descuento a editar y redirecciona a la UI de editar
        /// </summary>
        public void IrAEditar() {
            try {
                ConfiguracionDescuentoBO oConfiguracionDescuento = this.ObtenerListaClonada(this.vista.Resultado)[0];
                this.vista.EstablecerDatosNavegacion(oConfiguracionDescuento);
                this.vista.RedirigirAEditar();
            } catch (Exception ex) {
                this.MostrarMensaje("Error al intentar ir a editar la configuración de descuento ", ETipoMensajeIU.ERROR, nombreClase + "IrAEditar" + ex.Message);
            }
        }
        /// <summary>
        /// SC0024
        /// Redirige a la página principal de consulta
        /// </summary>
        public void RetrocederPagina() {
            this.vista.RegresarAConsultar();
        }
        #endregion


        #endregion


    }
}