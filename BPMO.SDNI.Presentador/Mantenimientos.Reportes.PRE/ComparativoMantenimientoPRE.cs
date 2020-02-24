// Satisface al CU075 - Reporte Comparativo Mantenimiento
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Facade.SDNI.BO;
using BPMO.Basicos.BO;
using BPMO.Servicio.Catalogos.BO;
using BPMO.Facade.SDNI.BR;
using BPMO.Facade.SDNI.BOF;
using BPMO.SDNI.Comun.BOF;
using System.Collections;
using BPMO.Servicio.Procesos.Enumeradores;
using BPMO.SDNI.Mantenimiento.Reportes.BR;
using System.Data;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Mantenimiento.Reportes.VIS;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Comun.BR;

namespace BPMO.SDNI.Mantenimientos.Reportes.PRE {

    /// <summary>
    /// Presentador aplicable para la gestión de las peticiones de la Vista Consulta para el Reporte Comparativo 
    /// de Mantenimientos.
    /// </summary>
    public class ComparativoMantenimientoPRE {
        
        #region Atributos

        /// <summary>
        /// El DataContext que proveerá acceso a la base de datos.
        /// </summary>
        private IDataContext dataContext;

        /// <summary>
        /// La vista que esta mostrando los datos procesados en el presentador.
        /// </summary>
        private IComparativoMantenimientoVIS vista;

        /// <summary>
        /// Controlador del Reporte Comparativo de Mantenimientos.
        /// </summary>
        private ReporteComparativoMantenimientoBR ctrl;

        /// <summary>
        /// Nombre de la Clase en curso
        /// </summary>
        private readonly string nombreClase = typeof(ComparativoMantenimientoPRE).Name;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor predeterminado para el presentador.
        /// </summary>
        /// <param name="vista">Vista que será gestionada por el presentador.</param>
        public ComparativoMantenimientoPRE(IComparativoMantenimientoVIS vista) {
            this.vista = vista;
            dataContext = FacadeBR.ObtenerConexion();
            ctrl = new ReporteComparativoMantenimientoBR();
        }

        #endregion

        #region Métodos

            #region Seguridad

        /// <summary>
        /// Verifica que el Usuario tenga los permisos para realizar la acción de Reporte Comaparativo de Mantenimientos.
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
                                                                                        this.dataContext, getConfiguracionUnidadOperativa(),  this.vista.ModuloId
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

                //Se valida si el usuario tiene permiso para el Reporte Comparativo de Mantenimientos
                if (!ExisteAccion(acciones, "CONSULTAR"))
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

        /// <summary>
        /// Realiza la búsqueda de los Mantenimientos por los Filtros solicitados. Si la Fecha de Inicio y Fecha Fin
        /// son nulas, se realiza la búsqueda por el Mes anterior a la Fecha Actual, en caso de que la Fecha Actual sea Fin de Mes,
        /// la búsqueda se realiza por el Mes actual.
        /// </summary>
        public void BuscarMantenimientos() {
            DateTime? fechaInicio = null, fechaFin = null;
            DateTime fechaActual = new DateTime();
            Hashtable parametros = new Hashtable();
            if (vista.FechaInicio == null && vista.FechaFin == null) {
                fechaActual = DateTime.Now;
                int dias = DateTime.DaysInMonth(fechaActual.Year, fechaActual.Month);
                if (dias == fechaActual.Day) {
                    fechaInicio = new DateTime(fechaActual.Year, fechaActual.Month, 1);
                    fechaFin = new DateTime(fechaActual.Year, fechaActual.Month, dias);
                    fechaFin = fechaFin.Value.AddDays(1).AddSeconds(-1);
                } else {
                    DateTime mesAnterior = DateTime.Now.AddMonths(-1);
                    dias = DateTime.DaysInMonth(mesAnterior.Year, mesAnterior.Month);
                    int anio = mesAnterior.Year == fechaActual.Year ? fechaActual.Year : mesAnterior.Year;
                    fechaInicio = new DateTime(anio, mesAnterior.Month, 1);
                    fechaFin = new DateTime(anio, mesAnterior.Month, dias);
                    fechaFin = fechaFin.Value.AddDays(1).AddSeconds(-1);
                }
            } else {
                if (vista.FechaInicio != null) {
                    fechaInicio = new DateTime(vista.FechaInicio.Value.Year, vista.FechaInicio.Value.Month, vista.FechaInicio.Value.Day);
                }
                if (vista.FechaFin != null) {
                    fechaFin = new DateTime(vista.FechaFin.Value.Year, vista.FechaFin.Value.Month, vista.FechaFin.Value.Day);
                    fechaFin = fechaFin.Value.AddDays(1).AddSeconds(-1);
                }
            }

            if(fechaInicio != null){
                parametros["FechaInicio"] = fechaInicio;
            }

            if(fechaFin != null){
                parametros["FechaFin"] = fechaFin;
            }

            parametros["UnidadOperativaId"] = vista.UnidadOperativaId;
            parametros["Estatus"] = new Int32[] { (int)EEstatusOrdenServicio.ASIGNADA,
                                                  (int)EEstatusOrdenServicio.FACTURADO
                                                };
            if (vista.SucursalId != null) {
                parametros["SucursalId"] = vista.SucursalId.Value;
            }

            if (vista.ModeloId != null) {
                parametros["ModeloId"] = vista.ModeloId;
            }

            if (vista.VIN != null) {
                parametros["NumeroSerie"] = vista.VIN;
            }

            if (vista.ClienteId != null) {
                parametros["ClienteId"] = vista.ClienteId;
            }
            parametros["Areas"] = new Int32[] { (int)EArea.RD, (int)EArea.FSL, (int)EArea.CM, (int)EArea.SD };
            parametros["ModuloId"] = vista.ModuloId;
            parametros["Activo"] = true;
            Dictionary<String, Object> parametrosReporte = ctrl.ConsultarMantenimientos(dataContext, parametros);
            if(((DataSet)parametrosReporte["DataSource"]).Tables[0].Rows.Count <= 0)  {
                this.vista.MostrarMensaje("No se encontraron coincidencias con los filtros especificados", ETipoMensajeIU.ADVERTENCIA);
                return;
            }
            this.vista.EstablecerPaqueteNavegacionImprimir("PLEN.BEP.15.MODMTTO.CU075", parametrosReporte);
            this.vista.IrAImprimir();
        }

            #region Buscador

        /// <summary>
        /// Crea el Objeto de filtrado para el buscador.
        /// </summary>
        /// <param name="catalogo">El Tipo de Objeto a filtrar.</param>
        /// <returns>Un Objeto de Tipo Object</returns>
        public object PrepararBOBuscador(string catalogo) {
            object obj = null;

            switch (catalogo) {
                case "Unidad":
                    BPMO.SDNI.Equipos.BO.UnidadBO ebBO = new BPMO.SDNI.Equipos.BO.UnidadBO();
                    ebBO.NumeroSerie = vista.VIN;
                    obj = ebBO;
                    break;
            }

            return obj;
        }

        /// <summary>
        /// Despliega el Objeto seleccionado del buscador.
        /// </summary>
        /// <param name="catalogo">El Tipo de Objeto a filtrar.</param>
        /// <param name="selecto">El Objeto seleccionado del buscador.</param>
        public void DesplegarResultadoBuscador(string catalogo, object selecto) {
            switch (catalogo) {
                case "Unidad":
                    BPMO.SDNI.Equipos.BO.UnidadBO ebBO = (BPMO.SDNI.Equipos.BO.UnidadBO)selecto;
                    if (ebBO == null) ebBO = new BPMO.SDNI.Equipos.BO.UnidadBO();

                    if (ebBO.NumeroSerie != null) {
                        this.vista.VIN = ebBO.NumeroSerie;
                    } else {
                        this.vista.VIN = null;
                    }
                    
                break;
            }
        }

            #endregion

        #endregion
    }
}
