// Satisface al CU072 - Obtener Auditoría
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BPMO.SDNI.Mantenimiento.VIS;
using BPMO.Facade.SDNI.BOF;
using BPMO.Basicos.BO;
using BPMO.SDNI.Mantenimiento.BO;
using BPMO.Servicio.Catalogos.BO;
using BPMO.Servicio.Catalogos.BR;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Facade.SDNI.BR;
using BPMO.Primitivos.Enumeradores;
using BPMO.Servicio.Procesos.BO;
using BPMO.SDNI.Mantenimiento.BR;
using BPMO.Servicio.Procesos.BR;
using BPMO.Generales.BR;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Comun.BR;

namespace BPMO.SDNI.Mantenimiento.PRE {
    
    /// <summary>
    /// Presentador aplicable para la gestión de las peticiones de una Vista de Consulta de Auditorías de Mantenimientos Idealease.
    /// </summary>
    public class ConsultarAuditoriaPRE {
        
        #region Propiedades

        /// <summary>
        /// La vista que esta mostrando los datos procesados en el presentador.
        /// </summary>
        private IConsultarAuditoriaVIS vista;

        /// <summary>
        /// Nombre de la clase en curso.
        /// </summary>
        private static readonly string nombreClase = typeof(ConsultarAuditoriaPRE).Name;

        /// <summary>
        /// El DataContext que proveerá acceso a la base de datos.
        /// </summary>
        private readonly IDataContext dataContext = null;

        /// <summary>
        /// Controlador de Auditorías de Mantenimientos Idealease.
        /// </summary>
        private readonly AuditoriaMantenimientoBR ctrlAuditoria;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor predeterminado para el presentador.
        /// </summary>
        /// <param name="vista">Vista que será gestionada por el presentador.</param>
        public ConsultarAuditoriaPRE(IConsultarAuditoriaVIS vista) {
            try {
                    this.vista = vista;
                    this.dataContext = FacadeBR.ObtenerConexion();
                    ctrlAuditoria = new AuditoriaMantenimientoBR();
                } catch (Exception ex) {
                    this.vista.MostrarMensaje("No se pudieron obtener los datos de conexión", ETipoMensajeIU.ERROR,
                        "No se encontraron los parámetros de conexión en la fuente de datos, póngase en contacto con el administrador del sistema.");
                }
        }

        #endregion

        #region Métodos

            #region Seguridad

        /// <summary>
        /// Verifica que el Usuario logueado tenga los permisos para realizar la acción de Consulta de Auditorías Mantenimiento Idealease.
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
                throw new Exception(nombreClase + " .EstablecerInformacionInicial: " + ex.Message);
            }
        }

        /// <summary>
        /// Crea y obtiene un nuevo Filtro de la Configuración Unidad Operativa, para realizar la búsqueda de las 
        /// Configuraciones de Unidades Operativas.
        /// </summary>
        /// <returns>Objeto de Tipo ConfiguracionUnidadOperativaBO</returns>
        private ConfiguracionUnidadOperativaBO getConfiguracionUnidadOperativa() { 
            return new ConfiguracionUnidadOperativaBO() { UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaID } };
        }

        /// <summary>
        /// Permite configurar el comportamiento de los controles de la vista de acuerdo a los permisos configurados del Usuario.
        /// </summary>
        private void EstablecerSeguridad() { 
            try {
                //se valida que los datos del usuario y la unidad operativa no sean nulos
                if (this.vista.UsuarioAutenticado == null) throw new Exception("El identificador del usuario no debe ser nulo");
                if (this.vista.UnidadOperativaID == null) throw new Exception("El identificador de la Unidad Operativa no debe ser nulo ");

                //consulta lista de acciones permitidas
                List<CatalogoBaseBO> acciones = FacadeBR.ConsultarAccion(dataContext, getSeguridad());
                
                //Se valida si el usuario tiene permiso para consultar las Auditorías Mantenimientos Idealease
                if (!ExisteAccion(acciones, "CONSULTAR") || !ExisteAccion(acciones, "UI CONSULTAR"))
                    this.vista.RedirigirSinPermisoAcceso();
                        
            }
            catch (Exception ex) {
                throw new Exception(nombreClase + " .EstablecerSeguridad:" + ex.Message);
            }
        }

        /// <summary>
        /// Crea y obtiene una nueva instancia de Seguridad.
        /// </summary>
        /// <returns>Objeto de tipo SeguridadBO.</returns>
        private SeguridadBO getSeguridad() {
            UsuarioBO usuario = new UsuarioBO() { Id = this.vista.UsuarioAutenticado };
            AdscripcionBO adscripcion = new AdscripcionBO() { UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaID } };
            return new SeguridadBO(Guid.Empty, usuario, adscripcion);
        }

        /// <summary>
        /// Verifica si existe la acción en la lista de acciones permitidas.
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

            #region Form Búsqueda

        /// <summary>
        /// Realiza la búsqueda de Auditorías de Mantenimientos Idealease por los filtros, Técnico seleccionado, Sucursal seleccionada, Orden de Servicio o Tipo de 
        /// Mantenimiento seleccionado. En caso de encontrarse resultados se despliega la lista de coincidencias, en caso contrario despliega un mensaje de error.
        /// </summary>
        public void MostrarDatosInterfaz() {
            bool encontrado = false;
            if (vista.NombreTecnico != null && vista.NombreTecnico.Trim() != null && !vista.NombreTecnico.Equals("")) {
                encontrado = BuscarAuditoriaPorTecnicos();
            } else if (vista.NombreSucursal != null && vista.NombreSucursal.Trim() != null && !vista.NombreSucursal.Equals("")) {
                encontrado = BuscarPorSucursal();
            } else if (vista.OrdenServicioID != null && vista.OrdenServicioID > 0) {
                encontrado = BuscarPorOrdenServicio();
            } else  {
                encontrado = Buscar();
            }

            if (!encontrado) {
                vista.MostrarMensaje("No se encontraron coincidencias", ETipoMensajeIU.ADVERTENCIA);
            }
            vista.DesplegarListaAuditorias();
        }

        /// <summary>
        /// Realiza la búsqueda de Auditorías de Mantenimientos Idealease por, la Sucursal seleccionada y el Tipo de Mantenimiento seleccionado.
        /// Si el Tipo de Mantenimiento seleccionado es nulo, realiza la búsqueda para todos los Tipos de Mantenimientos y la Sucursal seleccionada.
        /// </summary>
        /// <returns>Retorna True si encontró resultados, en caso contrario retorna False</returns>
        private bool BuscarPorSucursal() {
            bool encontrado = false;
            List<AuditoriaMantenimientoBO> auditorias = new List<AuditoriaMantenimientoBO>();
            if (vista.TipoMantenimiento == null) {
                for (int i = 1; i < 4; i++) {
                    ETipoMantenimiento tipoMantenimiento = (ETipoMantenimiento)Enum.Parse(typeof(ETipoMantenimiento), i.ToString());
                    List<AuditoriaMantenimientoBO> result = ctrlAuditoria.ConsultarAuditoria(dataContext, getFiltroSucursal(tipoMantenimiento)).ConvertAll(x => (AuditoriaMantenimientoBO)x);
                    if(result != null && result.Count > 0){
                        auditorias.AddRange(result);
                    }
                }
            } else {
                auditorias = ctrlAuditoria.ConsultarAuditoria(dataContext, getFiltroSucursal(vista.TipoMantenimiento.Value));
            }
               
            if(auditorias != null && auditorias.Count > 0){
                foreach (AuditoriaMantenimientoBO auditoria in auditorias) {
                    auditoria.OrdenServicio = getOrdenServicioCompleta((int)auditoria.OrdenServicio.Id);
                }
                vista.Auditorias = auditorias;
                encontrado = true;
            }

            return encontrado;
        }

        /// <summary>
        /// Crea y obtiene un nuevo Filtro de AuditoriaMantenimientoBO con la Sucursal seleccionada, el Tipo de Mantenimiento seleccionado y la Orden de Servicio.
        /// Si la Orden de Servicio es nula, crea la nueva instancia de AuditoriaMantenimientoBO con la Sucursal seleccionada y el Tipo de Mantenimiento seleccionado,
        /// para realizar la búsqueda de las Auditorías Mantenimientos Idealease.
        /// </summary>
        /// <param name="tipoMantenimiento">Tipo de Mantenimiento Seleccionado.</param>
        /// <returns>Objeto de tipo AuditoriaMantenimientoBO</returns>
        private AuditoriaMantenimientoBO getFiltroSucursal(ETipoMantenimiento tipoMantenimiento) {
            AuditoriaMantenimientoBO filtro = new AuditoriaMantenimientoBO() {
                TipoMantenimiento = tipoMantenimiento,
                OrdenServicio  = new OrdenServicioBO() {
                    Adscripcion = new AdscripcionBO() {
                        Sucursal = new SucursalBO() {
                            Id = vista.SucursalID
                        }
                    }
                }
            };
            
            if (vista.OrdenServicioID != null && vista.OrdenServicioID > 0) {
                filtro.OrdenServicio.Id = vista.OrdenServicioID;
            }
            return filtro;
        }

        /// <summary>
        /// Realiza la búsqueda de Auditorías de Mantenimientos Idealease por, Orden de Servicio y Tipo de Mantenimiento seleccionado. 
        /// Si el Tipo de Mantenimiento seleccionado es nulo, realiza la búsqueda para todos los Tipos de Mantenimientos con la Orden de Servicio.
        /// </summary>
        /// <returns>Retorna True si encontró resultados, en caso contrario retorna False</returns>
        private bool BuscarPorOrdenServicio() {
            bool encontrado = false;
            if (vista.TipoMantenimiento == null) {
                vista.Auditorias = new List<AuditoriaMantenimientoBO>();
                for (int i = 1; i < 4; i++) {
                    ETipoMantenimiento tipoMantenimiento = (ETipoMantenimiento)Enum.Parse(typeof(ETipoMantenimiento), i.ToString());
                    AuditoriaMantenimientoBO result = ctrlAuditoria.ConsultarAuditoria(dataContext, getFiltroAuditoriaOrdenServicio(tipoMantenimiento)).FirstOrDefault();
                    if(result != null && result.AuditoriaMantenimientoID != null){
                        result.OrdenServicio = getOrdenServicioCompleta((int)result.OrdenServicio.Id);
                        vista.Auditorias.Add(result);
                        encontrado = true;
                    }
                }
            } else {
                AuditoriaMantenimientoBO result = ctrlAuditoria.ConsultarAuditoria(dataContext, getFiltroAuditoriaOrdenServicio(vista.TipoMantenimiento.Value)).FirstOrDefault();
                if (result != null && result.AuditoriaMantenimientoID != null) {
                    result.OrdenServicio = getOrdenServicioCompleta((int)result.OrdenServicio.Id);
                    vista.Auditorias = new List<AuditoriaMantenimientoBO>();
                    vista.Auditorias.Add(result);
                    encontrado = true;
                }
            }

            return encontrado;
        }

        /// <summary>
        /// Crea y obtiene un nuevo Filtro de AuditoriaMantenimientoBO con el Tipo de Mantenimiento Seleccionado y la Orden de Servicio, para realizar la 
        /// búsqueda de las Auditorías Mantenimientos Idealease.
        /// </summary>
        /// <param name="tipoMantenimiento">El Tipo de Mantenimiento seleccionado</param>
        /// <returns>Objeto de tipo AuditoriaMantenimientoBO</returns>
        private AuditoriaMantenimientoBO getFiltroAuditoriaOrdenServicio(ETipoMantenimiento tipoMantenimiento) {
            AuditoriaMantenimientoBO filtro = new AuditoriaMantenimientoBO() {
                TipoMantenimiento = tipoMantenimiento,
                OrdenServicio = new OrdenServicioBO(){
                    Id = vista.OrdenServicioID
                }
            };

            return filtro;
        }

        /// <summary>
        /// Realiza la búsqueda de Auditorías de Mantenimientos Idealease por, el Técnico seleccionado y el Tipo de Mantenimiento seleccionado. 
        /// Si el Tipo de Mantenimiento seleccionado es nulo, realiza la búsqueda para todos los Tipos de Mantenimientos y el Técnico seleccionado.
        /// </summary>
        /// <returns>Retorna True si encontró resultados, en caso contrario retorna False</returns>
        private bool BuscarAuditoriaPorTecnicos() {
            bool encontrado = false;
            List<AuditoriaMantenimientoTecnicoBO> auditoriasPorTecnico = new List<AuditoriaMantenimientoTecnicoBO>();
            List<AuditoriaMantenimientoTecnicoBO> result = new List<AuditoriaMantenimientoTecnicoBO>();
            if (vista.TipoMantenimiento == null) {
                for (int i = 1; i < 4; i++) {
                    ETipoMantenimiento tipoMantenimiento = (ETipoMantenimiento)Enum.Parse(typeof(ETipoMantenimiento), i.ToString());
                    List<AuditoriaMantenimientoTecnicoBO> result2 = getAuditoriasPorTecnicos(auditoriasPorTecnico, tipoMantenimiento);
                    if(result2 != null && result2.Count > 0){
                        result.AddRange(result2);
                    }
                }
            } else {
                result = getAuditoriasPorTecnicos(auditoriasPorTecnico, vista.TipoMantenimiento.Value);
            }
            if(result.Count > 0){
                auditoriasPorTecnico.AddRange(result);
            }

            if(auditoriasPorTecnico.Count > 0){
                List<AuditoriaMantenimientoBO> auditorias = new List<AuditoriaMantenimientoBO>();
                foreach (AuditoriaMantenimientoTecnicoBO auditoriaTecnico in auditoriasPorTecnico) {
                    AuditoriaMantenimientoBO auditoria = ctrlAuditoria.ConsultarAuditoria(dataContext,auditoriaTecnico.AuditoriaMantenimiento).FirstOrDefault();
                    if (auditoria != null && auditoria.AuditoriaMantenimientoID != null && auditoria.OrdenServicio != null && auditoria.OrdenServicio.Id != null) {
                        auditoria.OrdenServicio = getOrdenServicioCompleta((int)auditoria.OrdenServicio.Id);
                        auditorias.Add(auditoria);
                    }
                }
                if (auditorias.Count > 0) {
                    vista.Auditorias = auditorias;
                    encontrado = true;
                }
            }            

            return encontrado;
        }

        /// <summary>
        /// Obtiene las Auditorías de Mantenimientos Idealease por, el Técnico seleccionado y el Tipo de Mantenimiento seleccionado.
        /// </summary>
        /// <param name="auditoriasPorTecnico">Auditorías del Técnico seleccionado</param>
        /// <param name="tipoMantenimiento">Tipo de Mantenimiento seleccionado</param>
        /// <returns>Objeto de tipo List&lt;AuditoriaMantenimientoTecnicoBO&gt;.</returns>
        private List<AuditoriaMantenimientoTecnicoBO> getAuditoriasPorTecnicos(List<AuditoriaMantenimientoTecnicoBO> auditoriasPorTecnico, ETipoMantenimiento tipoMantenimiento) { 
            AuditoriaMantenimientoTecnicoBO filtroTecnicoSeleccionado = getFiltroAuditoriaPorTecnicos(vista.TecnicoSeleccionado, tipoMantenimiento);
            List<AuditoriaMantenimientoTecnicoBO> result = ctrlAuditoria.ConsultarAuditoriasMantenimientosTecnicos(dataContext, filtroTecnicoSeleccionado);
            if (auditoriasPorTecnico.Count > 0) {
                foreach (AuditoriaMantenimientoTecnicoBO auditoria in auditoriasPorTecnico){
                    AuditoriaMantenimientoTecnicoBO auditoriaTecnicoEncontrado = result.Find(x => x.AuditoriaMantenimientoTecnicoId == auditoria.AuditoriaMantenimientoTecnicoId);
                    if (auditoriaTecnicoEncontrado != null) {
                        result.Remove(auditoriaTecnicoEncontrado);
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Crea y obtiene un nuevo Filtro de AuditoriaMantenimientoTecnicoBO con el Técnico Seleccionado, la Orden de Servicio y la Sucursal Seleccionada, 
        /// para realizar la búsqueda de las Auditorías Mantenimientos Idealease.
        /// </summary>
        /// <param name="tecnico">El Técnico Seleccionado</param>
        /// <param name="tipoMantenimiento">El Tipo de Mantenimiento Seleccionado</param>
        /// <returns>Objeto de tipo AuditoriaMantenimientoTecnicoBO</returns>
        private AuditoriaMantenimientoTecnicoBO getFiltroAuditoriaPorTecnicos(TecnicoBO tecnico, ETipoMantenimiento tipoMantenimiento) {
            AuditoriaMantenimientoTecnicoBO filtro = new AuditoriaMantenimientoTecnicoBO(){
                Tecnico = tecnico,
                AuditoriaMantenimiento = new AuditoriaMantenimientoBO(){
                    TipoMantenimiento = tipoMantenimiento,
                    OrdenServicio = new OrdenServicioBO() {
                        Adscripcion = new AdscripcionBO() {
                            Sucursal = new SucursalBO()
                        }
                    }
                }
            };
            
            if (vista.OrdenServicioID != null) {
                filtro.AuditoriaMantenimiento.OrdenServicio.Id = vista.OrdenServicioID;
            }

            if (vista.SucursalID != null && vista.SucursalID > 0) {
                filtro.AuditoriaMantenimiento.OrdenServicio.Adscripcion.Sucursal.Id = vista.SucursalID;
            }
            return filtro;
        }

        /// <summary>
        /// Obtiene las Auditorías de Mantenimientos Idealease por, el Tipo de Mantenimiento seleccionado.
        /// Si el Tipo de Mantenimiento seleccionado es nulo, realiza la búsqueda para todos los Tipos de Mantenimientos.
        /// </summary>
        /// <returns>Retorna True si encontró resultados, en caso contrario retorna False</returns>
        private bool Buscar() {
            bool encontrado = false;
            if (vista.TipoMantenimiento == null) {
                List<AuditoriaMantenimientoBO> auditorias = new List<AuditoriaMantenimientoBO>();
                for (int i = 1; i < 4; i++) { 
                    ETipoMantenimiento tipoMantenimiento = (ETipoMantenimiento)Enum.Parse(typeof(ETipoMantenimiento), i.ToString());
                    List<AuditoriaMantenimientoBO> result = ctrlAuditoria.ConsultarAuditoria(dataContext, getFiltroAuditoria(tipoMantenimiento));
                    if(result != null && result.Count > 0) {
                        foreach (AuditoriaMantenimientoBO item in result) {
                            item.OrdenServicio = getOrdenServicioCompleta((int)item.OrdenServicio.Id);
			            }
                        auditorias.AddRange(result);
                    }
                }

                if(auditorias.Count > 0){
                    vista.Auditorias = auditorias;
                    encontrado = true;
                }
            } else {
                List<AuditoriaMantenimientoBO> auditorias = ctrlAuditoria.ConsultarAuditoria(dataContext, getFiltroAuditoria(vista.TipoMantenimiento.Value));
                if(auditorias != null && auditorias.Count > 0) {
                    foreach (AuditoriaMantenimientoBO item in auditorias) {
                        item.OrdenServicio = getOrdenServicioCompleta((int)item.OrdenServicio.Id);
			        }
                    vista.Auditorias = auditorias;
                    encontrado = true;
                }
            }

            return encontrado;
        }

        /// <summary>
        /// Crea y obtiene un nuevo Filtro de AuditoriaMantenimientoBO con el Tipo de Mantenimiento Seleccionado, para realizar la búsqueda de las
        /// Auditorías Mantenimientos Idealease.
        /// </summary>
        /// <param name="tipoMantenimiento">Tipo de Mantenimiento Seleccionado</param>
        /// <returns>Un Objeto de tipo AuditoriaMantenimientoBO</returns>
        private AuditoriaMantenimientoBO getFiltroAuditoria(ETipoMantenimiento tipoMantenimiento) {
            AuditoriaMantenimientoBO filtro = new AuditoriaMantenimientoBO(){
                TipoMantenimiento = tipoMantenimiento
            };
            return filtro;
        }

        /// <summary>
        /// Crea y obtiene un nuevo Filtro de OrdenServicioBO, para realizar la búsqueda de las Ordenes de Servicio.
        /// </summary>
        /// <param name="ordenServicioId">Folio de la Orden de Servicio</param>
        /// <returns>Un Objeto de tipo OrdenServicioBO</returns>
        private OrdenServicioBO getOrdenServicioCompleta(int ordenServicioId) { 
            OrdenServicioBO filtro = new OrdenServicioBO() {
                Id = ordenServicioId
            };
            OrdenServicioBO ordenCompleta = FacadeBR.ConsultarOrdenServicio(dataContext, filtro).FirstOrDefault();
            if (ordenCompleta.AdscripcionServicio.Sucursal.Id != null) {
                SucursalBO filtroSucursal = new SucursalBO(){
                    Id = ordenCompleta.AdscripcionServicio.Sucursal.Id
                };
                SucursalBO sucursal = FacadeBR.ConsultarSucursal(dataContext, filtroSucursal).FirstOrDefault();
                ordenCompleta.AdscripcionServicio.Sucursal = sucursal;
            }
            return ordenCompleta;
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
                case "Sucursal":
                    SucursalBOF sucursal = new SucursalBOF();
                    sucursal.UnidadOperativa = new UnidadOperativaBO();
                    sucursal.UnidadOperativa.Id = this.vista.UnidadOperativaID;
                    sucursal.Nombre = this.vista.NombreSucursal;
                    sucursal.Usuario = new UsuarioBO() { Id = this.vista.UsuarioAutenticado };
                    obj = sucursal;
                    break;
                case "Tecnico":
                    TecnicoBO t = new TecnicoBO() { 
                            Empleado = new EmpleadoBO() {
                            }
                    };
                    if (vista.NombreTecnico != null) {
                        t.Empleado.NombreCompleto = vista.NombreTecnico;
                    }
                    obj = t;
                    
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
                case "Sucursal":
                    SucursalBO sucursal = (SucursalBO)selecto;
                    
                    if (sucursal != null && sucursal.Id != null) {
                        this.vista.SucursalID = sucursal.Id;
                        this.vista.SucursalSeleccionada = sucursal;
                    } else
                        this.vista.SucursalID = null;

                    if (sucursal != null && sucursal.Nombre != null)
                        this.vista.NombreSucursal = sucursal.Nombre;
                    else
                        this.vista.NombreSucursal = null;

                    break;
                case "Tecnico":
                    TecnicoBO tecnico = (TecnicoBO)selecto;
                    if (tecnico != null && tecnico.Id != null) {
                        this.vista.TecnicoSeleccionado = tecnico;
                        this.vista.NombreTecnico = tecnico.Empleado.NombreCompleto;
                    } else {
                        this.vista.NombreTecnico = null;
                    }
                    break;
            }
        }

                #endregion

            #endregion

            #region Grid Resultado Búsqueda

        /// <summary>
        /// Obtiene la Auditoría Mantenimiento Idealease seleccionado.
        /// </summary>
        /// <param name="seleccionado">La Auditoría Mantenimiento Idealease seleccionado.</param>
        /// <returns>Retorna True si encontró la Auditoría Mantenimiento Idealease.</returns>
        public bool VerAuditoriaCompleta(AuditoriaMantenimientoBO seleccionado) {
            bool cargarAuditoria = false;
            try{
                AuditoriaMantenimientoBO filtro = seleccionado;
                ConsultarAuditoriaMantenimientoBR ctrlAuditoriaCompleta = new ConsultarAuditoriaMantenimientoBR();
                AuditoriaMantenimientoBO auditoriaCompleta = ctrlAuditoriaCompleta.ConsultarAuditoriaCompleta(dataContext, filtro);
                auditoriaCompleta.OrdenServicio.AdscripcionServicio = seleccionado.OrdenServicio.AdscripcionServicio;
                if(auditoriaCompleta != null && auditoriaCompleta.AuditoriaMantenimientoID != null){
                    vista.AuditoriaSeleccionada = auditoriaCompleta;
                    cargarAuditoria = true;
                }else{
                    vista.MostrarMensaje("No se encontraron resultados para esta auditoria", ETipoMensajeIU.ADVERTENCIA);
                }
            } catch(Exception e){
                  if(!e.Message.Equals("")){
                      vista.MostrarMensaje(e.Message, ETipoMensajeIU.ADVERTENCIA);
                  }
            }
            return cargarAuditoria;
        }

            #endregion

        #endregion

    }
}
