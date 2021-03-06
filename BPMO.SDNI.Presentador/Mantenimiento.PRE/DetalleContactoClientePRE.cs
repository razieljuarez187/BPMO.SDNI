﻿// Satisface al CU027 - Catálogo Contacto Cliente
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BPMO.Patterns.Creational.DataContext;
using BPMO.SDNI.Mantenimiento.VIS;
using BPMO.Facade.SDNI.BR;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Comun.BR;
using BPMO.Basicos.BO;

namespace BPMO.SDNI.Mantenimiento.PRE {

    /// <summary>
    /// Presentador aplicable para la gestión de las peticiones de la Vista Detalle Contacto Cliente Idealease seleccionado, al usuario.
    /// </summary>
    public class DetalleContactoClientePRE {

        #region Atributos

        /// <summary>
        /// La vista que esta mostrando los datos procesados en el presentador.
        /// </summary>
        private IDetalleContactoClienteVIS vista;

        /// <summary>
        /// El DataContext que proveerá acceso a la base de datos.
        /// </summary>
        private readonly IDataContext dataContext;

        /// <summary>
        /// Nombre de la clase en curso.
        /// </summary>
        private readonly string nombreClase = typeof(DetalleContactoClientePRE).Name;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor predeterminado para el presentador.
        /// </summary>
        /// <param name="vista">Vista que será gestionada por el presentador.</param>
        public DetalleContactoClientePRE(IDetalleContactoClienteVIS vista) {
            this.vista = vista;
            dataContext = FacadeBR.ObtenerConexion();
        }

        #endregion

        #region Métodos

            #region Seguridad

        /// <summary>
        /// Verifica que el Usuario tenga los permisos para realizar la acción de Detalle Contacto Cliente Idealease.
        /// </summary>
        public void PrepararSeguridad() {
            EstablecerInformacionInicial();
            EstablecerSeguridad();
        }

        /// <summary>
        /// Establece la información Inicial en la Vista.
        /// </summary>
        private void EstablecerInformacionInicial() { 
            try {
                #region Configuraciones de Unidad Operativa
                //Obtener las configuraciones de la unidad operativa
                List<ConfiguracionUnidadOperativaBO> lstConfigUO = new ModuloBR().ConsultarConfiguracionUnidadOperativa(
                                                                                        this.dataContext, getConfiguracionUnidadOperativa(),  this.vista.ModuloID
                                                                                    );
                if (lstConfigUO.Count <= 0)
                    throw new Exception("No se encontraron las configuraciones del sistema para la unidad operativa en la que trabaja.");

                //Establecer las configuraciones de la unidad operativa
                this.vista.LibroActivos = lstConfigUO[0].Libro;
                #endregion
            } catch (Exception ex) {
                throw new Exception(nombreClase + ".EstablecerInformacionInicial: " + ex.Message);
            }
        }

        /// <summary>
        /// Crea y obtiene un nuevo Filtro de la Configuración Unidad Operativa, para realizar la búsqueda de las 
        /// Configuraciones de Unidades Operativas.
        /// </summary>
        /// <returns>Objeto de Tipo ConfiguracionUnidadOperativaBO</returns>
        private ConfiguracionUnidadOperativaBO getConfiguracionUnidadOperativa() { 
            return new ConfiguracionUnidadOperativaBO() { UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaId } };
        }

        /// <summary>
        /// Permite configurar el comportamiento de los controles de la vista de acuerdo a los permisos configurados del Usuario.
        /// </summary>
        private void EstablecerSeguridad() { 
            try {
                //se valida que los datos del usuario y la unidad operativa no sean nulos
                if (this.vista.UsuarioAutenticado == null) throw new Exception("El identificador del usuario no debe ser nulo");
                if (this.vista.UnidadOperativaId == null) throw new Exception("El identificador de la Unidad Operativa no debe ser nulo ");

                //consulta lista de acciones permitidas
                List<CatalogoBaseBO> acciones = FacadeBR.ConsultarAccion(dataContext, getSeguridad());

                //Se valida si el usuario tiene permiso para Detalle Contactos Clientes Idealease
                if (!ExisteAccion(acciones, "UI CONSULTAR") || !ExisteAccion(acciones, "CONSULTARCOMPLETO") || !ExisteAccion(acciones, "CONSULTAR"))
                    this.vista.RedirigirSinPermisoAcceso();
                
            }
            catch (Exception ex) {
                throw new Exception(nombreClase + ".EstablecerSeguridad:" + ex.Message);
            }
        }

        /// <summary>
        /// Crea y obtiene una nueva instancia de Seguridad.
        /// </summary>
        /// <returns>Objeto de tipo SeguridadBO.</returns>
        private SeguridadBO getSeguridad() {
            UsuarioBO usuario = new UsuarioBO() { Id = this.vista.UsuarioAutenticado };
            AdscripcionBO adscripcion = new AdscripcionBO() { UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaId } };
            return new SeguridadBO(Guid.Empty, usuario, adscripcion);
        }

        /// <summary>
        /// Verifica si existe una acción en la lista de acciones permitidas.
        /// </summary>
        /// <param name="acciones">Lista de Acciones permitidas.</param>
        /// <param name="nombreAccion">Acción a verificar.</param>
        /// <returns>Retorna True si encontró resultados, en caso contrario retorna False.</returns>
        private bool ExisteAccion(List<CatalogoBaseBO> acciones, string nombreAccion){
            if (acciones != null && 
                acciones.Count > 0 &&
                acciones.Exists(
                    p => p != null && p.Nombre != null && p.Nombre.Trim().ToUpper() == nombreAccion.Trim().ToUpper()))
                return true;

            return false;
        }

            #endregion

        #endregion
    }
}
