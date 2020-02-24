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
    public class EditarConfiguracionDescuentoPSLPRE {
        #region Atributos
        /// <summary>
        /// contexto para la consulta
        /// </summary>
        private IDataContext dctx = null;

        /// <summary>
        /// Interfaz de descuento
        /// </summary>
        private IEditarConfiguracionDescuentoPSLVIS vista;
        /// <summary>
        /// Presentador el UC de descuentos
        /// </summary>
        private ucConfiguracionDescuentoPSLPRE presentadorDescuento;
        /// <summary>
        /// BR de descuento
        /// </summary>
        private ConfiguracionDescuentoPSLBR configuracionDescuentoBR;
        /// <summary>
        /// nombre de la clase para errores
        /// </summary>
        private string nombreClase = "EditarConfiguracionDescuentoPSLPRE";
        #endregion

        #region Constructores
        public EditarConfiguracionDescuentoPSLPRE(IEditarConfiguracionDescuentoPSLVIS vistaActual, IucConfiguracionDescuentoPSLVIS vistaDescuento) {
            try {
                this.vista = vistaActual;
                presentadorDescuento = new ucConfiguracionDescuentoPSLPRE(vistaDescuento, "EDITAR");
                this.configuracionDescuentoBR = new ConfiguracionDescuentoPSLBR();
                this.dctx = FacadeBR.ObtenerConexion();
            } catch (Exception ex) {
                this.vista.MostrarMensaje("Inconsistencia en los parámetros de configuración", ETipoMensajeIU.ERROR, this.nombreClase + ".EditarConfiguracionDescuentoPSLPRE:" + ex.Message);
            }
        }
        #endregion

        #region Métodos
        /// <summary>
        /// Valida si el usuario puede hacer esa acción
        /// </summary>
        /// <param name="acciones">Lista de acciones</param>
        /// <param name="accion">Acción a comprobar</param>
        /// <returns></returns>
        private bool ExisteAccion(List<CatalogoBaseBO> acciones, string nombreAccion) {
            if (acciones != null && acciones.Count > 0 &&
                acciones.Exists(
                    p => p != null && p.Nombre != null && p.Nombre.Trim().ToUpper() == nombreAccion.Trim().ToUpper()))
                return true;

            return false;
        }
        /// <summary>
        /// Método que se realizara al inicializar el UI de editar
        /// </summary>
        public void RealizarPrimeraCarga() {
            try {
                if (this.vista.ObtenerPaqueteNavegacion("ConfiguracionDescuentoBO") == null)
                    throw new Exception("Se esperaba Datos de Navegación");
                if (!(this.vista.ObtenerPaqueteNavegacion("ConfiguracionDescuentoBO") is ConfiguracionDescuentoBO))
                    throw new Exception("Se esperaba un objeto de ConfiguracionDescuentoBO");

                ConfiguracionDescuentoBO configuracionDescuento = (ConfiguracionDescuentoBO)this.vista.ObtenerPaqueteNavegacion("ConfiguracionDescuentoBO");
                // Agarramos los valores que enviaremos a la consulta
                ConfiguracionDescuentoBO configuracionDescuentoEditar = new ConfiguracionDescuentoBO();
                configuracionDescuentoEditar.Cliente = new CuentaClienteIdealeaseBO() { Id = configuracionDescuento.Cliente.Id };
                configuracionDescuentoEditar.ContactoComercial = configuracionDescuento.ContactoComercial;

                List<ConfiguracionDescuentoBO> lst = new List<ConfiguracionDescuentoBO>();
                this.configuracionDescuentoBR = new ConfiguracionDescuentoPSLBR();
                //Obtenemos una lista con las filas que tengan el mismo cliente y contacto comercial
                lst = configuracionDescuentoBR.Consultar(dctx, configuracionDescuentoEditar).ConvertAll(s => (ConfiguracionDescuentoBO)s);

                this.vista.LimpiarSesiones();
                this.EstablecerSeguridad();
                this.EstablecerDatosaUC(this.ObtenerListaClonada(lst));

            } catch (Exception ex) {
                throw new Exception(this.nombreClase + ".RealizarPrimeraCarga: " + ex.Message);
            }
        }
        /// <summary>
        /// Establecerá el descuento al UC de descuentos
        /// </summary>
        /// <param name="lstConsultada"></param>
        private void EstablecerDatosaUC(List<ConfiguracionDescuentoBO> lstConsultada) {
            try {
                if (lstConsultada == null)
                    throw new Exception("Se esperaba una Lista de descuentos .");

                this.vista.ListaDetalle = this.ObtenerListaClonada(lstConsultada);
                this.presentadorDescuento.datoAinterfazUsuarioEditar(lstConsultada);
            } catch (Exception ex) {
                this.presentadorDescuento.datoAinterfazUsuarioEditar(new List<ConfiguracionDescuentoBO>());
                throw new Exception(this.nombreClase + ".EstablecerDatosNavegacion: " + ex.Message);
            }
        }
        /// <summary>
        /// Valida si el usuario tiene permiso para consultar
        /// </summary>
        public void ValidarAcceso() {
            try {
                //Valida que usuario y la unidad operativa no sean nulos
                if (this.vista.UsuarioAutenticado == null) throw new Exception("El identificador del usuario no debe ser nulo");
                if (this.vista.UnidadOperativaId == null) throw new Exception("El identificador de la Unidad Operativa no debe ser nulo");
                //Se crea el objeto de seguridad
                UsuarioBO usuario = new UsuarioBO() { Id = this.vista.UsuarioAutenticado };
                AdscripcionBO adscripcion = new AdscripcionBO() { UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaId } };
                SeguridadBO seguridadBO = new SeguridadBO(Guid.Empty, usuario, adscripcion);

                //Se valida si el usuario tiene permiso a la acción principal
                if (!FacadeBR.ExisteAccion(this.dctx, "ACTUALIZAR", seguridadBO))
                    this.vista.RedirigirSinPermisoAcceso();
            } catch (Exception ex) {
                throw new Exception(this.nombreClase + ".ValidarAcceso: " + ex.Message);
            }


        }
        /// <summary>
        /// Establece seguridad a la vista
        /// </summary>
        public void EstablecerSeguridad() {
            try {
                //se valida que los datos del usuario y la unidad operativa no sean nulos
                if (this.vista.UsuarioAutenticado == null) throw new Exception("El identificador del usuario no debe ser nulo");
                if (this.vista.UnidadOperativaId == null) throw new Exception("El identificador de la Unidad Operativa no debe ser nulo ");

                //Se crea el objeto de seguridad
                UsuarioBO usuario = new UsuarioBO() { Id = this.vista.UsuarioAutenticado };
                AdscripcionBO adscripcion = new AdscripcionBO() { UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaId } };
                SeguridadBO seguridadBO = new SeguridadBO(Guid.Empty, usuario, adscripcion);

                //Se obtienen las acciones a las cuales el usuario tiene permiso en este proceso
                List<CatalogoBaseBO> lst = FacadeBR.ConsultarAccion(dctx, seguridadBO);


                //Se valida si el usuario tiene permiso para registrar el check list         
                if (!this.ExisteAccion(lst, "ACTUALIZARCOMPLETO")) {
                    this.vista.RedirigirSinPermisoAcceso();
                }
            } catch (Exception ex) {
                throw new Exception(nombreClase + ".EstablecerSeguridad:" + ex.Message);
            }
        }

        #region Métodos de los Eventos de los botones de la UI
        /// <summary>
        /// Método para guardar cuando mandan a editar
        /// </summary>
        public void GuardarEditar() {
            try {

                this.Editar();



            } catch (Exception ex) {
                this.vista.MostrarMensaje("Error al intentar actualizar los datos", ETipoMensajeIU.ERROR,
                                    nombreClase + ".Guardar:" + ex.Message);
            }
        }
        /// <summary>
        /// Método que se realizara cuando se apreté el botón de guardar en editar
        /// </summary>
        public void Editar() {

            try {
                if (this.vista.UsuarioAutenticado == null) { throw new Exception("El identificador del usuario no debe ser nulo"); }
                if (this.vista.UnidadOperativaId == null) {
                    throw new Exception("El identificador de la Unidad Operativa no debe ser nulo");
                }
                //Se crea el objeto de seguridad
                UsuarioBO usuario = new UsuarioBO() { Id = this.vista.UsuarioAutenticado };
                AdscripcionBO adscripcion = new AdscripcionBO() { UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaId } };
                SeguridadBO seguridadBO = new SeguridadBO(Guid.Empty, usuario, adscripcion);
                //Validamos si al menos se hizo alguna modificación a alguna fila de descuentos
                if (this.presentadorDescuento.EstaCampoContactoComercialVacio()) {
                    return;
                }
                //Lista que tiene la lista de descuentos sin modificar
                List<ConfiguracionDescuentoBO> lstDetalle = this.ObtenerListaClonada();
                //Validamos si hizo una modificación a todas la lista de descuentos

                string nuevoContactoComercial = this.presentadorDescuento.obtenerContactoComercial();
                int cont = 0;
                foreach (ConfiguracionDescuentoBO descuentoActualizado in this.vista.ListaDescuentos) {
                    descuentoActualizado.ContactoComercial = nuevoContactoComercial;
                    this.configuracionDescuentoBR.ActualizarCompleto(this.dctx, descuentoActualizado, lstDetalle[cont], seguridadBO);
                    cont++;
                }

                this.vista.EstablecerPaqueteNavegacion("ConfiguracionDescuentoBO", this.ObtenerListaClonada(this.vista.ListaDescuentos)[0]);
                this.vista.LimpiarSesiones();
                this.vista.RedirigirADetalle();
            } catch (Exception ex) {
                throw new Exception(this.nombreClase + ".Editar: " + ex.Message);
            }
        }
        /// <summary>
        /// Método que se realizara cuando se apreté el botón de cancelar en editar
        /// </summary>
        public void Cancelar() {
            #region RI0075
            List<ConfiguracionDescuentoBO> lst = this.ObtenerListaClonada();
            this.vista.EstablecerPaqueteNavegacion("ConfiguracionDescuentoBO", lst[0]);
            this.vista.LimpiarSesiones();
            this.vista.RedirigirADetalle();

            #endregion
        }

        #endregion
        /// <summary>
        /// Obtiene una lista clonada
        /// </summary>
        /// <param name="lstTemporalSeleccionada">lista a clonar</param>
        /// <returns>lista clonada</returns>
        public List<ConfiguracionDescuentoBO> ObtenerListaClonada(List<ConfiguracionDescuentoBO> lstTemporalSeleccionada) {
            List<ConfiguracionDescuentoBO> lstTemporal = lstTemporalSeleccionada;
            List<ConfiguracionDescuentoBO> lstClonada = new List<ConfiguracionDescuentoBO>();

            foreach (ConfiguracionDescuentoBO descuento in lstTemporal) {
                lstClonada.Add(descuento.Clone());
            }

            return lstClonada;

        }
        /// <summary>
        /// Obtiene lista clonada de la lista de detalle
        /// </summary>
        /// <returns>lista clonada de la lista de detalle</returns>
        public List<ConfiguracionDescuentoBO> ObtenerListaClonada() {
            List<ConfiguracionDescuentoBO> lstTemporal = this.vista.ListaDetalle;
            List<ConfiguracionDescuentoBO> lstClonada = new List<ConfiguracionDescuentoBO>();

            foreach (ConfiguracionDescuentoBO descuento in lstTemporal) {
                lstClonada.Add(descuento.Clone());
            }

            return lstClonada;

        }
        #endregion
    }
}