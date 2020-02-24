// Satisface al CU062 - Obtener Orden Servicio Idealease
using System;
using System.Collections.Generic;
using BPMO.SDNI.Mantenimiento.VIS;
using BPMO.Patterns.Creational.DataContext;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Comun.BR;
using BPMO.Basicos.BO;
using BPMO.Facade.SDNI.BR;
using BPMO.Facade.SDNI.BO;
using BPMO.SDNI.Equipos.BOF;
using BPMO.SDNI.Equipos.BO;
using BPMO.SDNI.Mantenimiento.BR;
using BPMO.Primitivos.Enumeradores;
using BPMO.Servicio.Catalogos.BO;
using BPMO.SDNI.Mantenimiento.BO;
using System.Data;
using System.Linq;
using BPMO.Servicio.Procesos.BR;
using BPMO.Servicio.Procesos.BO;
using BPMO.Servicio.Procesos.Enumeradores;
using BPMO.Generales.BR;
using BPMO.Servicio.Catalogos.BR;
using BPMO.SDNI.Equipos.BR;

namespace BPMO.SDNI.Mantenimiento.PRE {

    /// <summary>
    /// Presentador aplicable para la gestión de las peticiones de la Vista Consultar Mantenimientos Idealease.
    /// </summary>
    public class ConsultarMantenimientoPRE {

        #region Propiedades

        /// <summary>
        /// La vista que esta mostrando los datos procesados en el presentador.
        /// </summary>
        private IConsultarMantenimientoVIS vista;

        /// <summary>
        /// Controlador de Mantenimiento Unidad Idealease.
        /// </summary>
        private MantenimientoUnidadBR ctrlMantenimientoUnidadIdealease;

        /// <summary>
        /// Controlador de Mantenimiento Equipo Aliado Idealease.
        /// </summary>
        private MantenimientoEquipoAliadoBR ctrlMatenimientoEquipoAliadoIdealease;

        /// <summary>
        /// El DataContext que proveerá acceso a la base de datos.
        /// </summary>
        private readonly IDataContext dataContext = null;

        /// <summary>
        /// Nombre de la clase en curso.
        /// </summary>
        private readonly string nombreClase = typeof(ConsultarMantenimientoPRE).Name;

        /// <summary>
        /// Enumerador de Tipos de Mantenimientos Idealease.
        /// </summary>
        public enum ETipoMantenimientoIdealease : int { 
            CORRECTIVO = 1,
            PREVENTIVO = 2
        }

        /// <summary>
        /// Enumerador de Tipos de Equipos de Lider.
        /// </summary>
        public enum ETipoEquipoLider : int {
            UNIDAD = 1,
            EQUIPO_ALIADO = 2
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Constructor predeterminado para el presentador.
        /// </summary>
        /// <param name="vista">Vista que será gestionada por el presentador.</param>
        public ConsultarMantenimientoPRE(IConsultarMantenimientoVIS vista) {
            try {
                this.vista = vista;
                this.ctrlMantenimientoUnidadIdealease = new MantenimientoUnidadBR();
                this.ctrlMatenimientoEquipoAliadoIdealease = new MantenimientoEquipoAliadoBR();
                this.dataContext = FacadeBR.ObtenerConexion();
            } catch (Exception ex) {
                this.vista.MostrarMensaje("No se pudieron obtener los datos de conexión", ETipoMensajeIU.ERROR,
                        "No se encontraron los parámetros de conexión en la fuente de datos, póngase en contacto con el administrador del sistema.");
            }
        }
        
        /// <summary>
        /// Prepara la vista para una nueva búsqueda.
        /// </summary>
        public void PrepararBusqueda(){
            this.vista.LimpiarSesion();
            this.vista.PrepararBusqueda();
            this.EstablecerInformacionInicial();
            this.EstablecerSeguridad(); //SC_0008
        }

        /// <summary>
        /// Establece la información Inicial en la Vista.
        /// </summary>
        private void EstablecerInformacionInicial() {
            try {
                #region Configuraciones de Unidad Operativa
                //Obtener las configuraciones de la unidad operativa
                List<ConfiguracionUnidadOperativaBO> lstConfigUO = new ModuloBR().ConsultarConfiguracionUnidadOperativa(this.dataContext, new ConfiguracionUnidadOperativaBO() { UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaId } }, this.vista.ModuloID);
                if (lstConfigUO.Count <= 0)
                    throw new Exception("No se encontraron las configuraciones del sistema para la unidad operativa en la que trabaja.");

                //Establecer las configuraciones de la unidad operativa
                this.vista.LibroActivos = lstConfigUO[0].Libro;
                #endregion
            } catch (Exception ex) {
                throw new Exception(this.nombreClase + " .EstablecerInformacionInicial: " + ex.Message);
            }
        }    

            #region Seguridad

        /// <summary>
        /// Permite configurar el comportamiento de los controles de la vista de acuerdo a los permisos configurados del Usuario.
        /// </summary>
        private void EstablecerSeguridad() {
            try {
                //se valida que los datos del usuario y la unidad operativa no sean nulos
                if (this.vista.UsuarioAutenticado == null) throw new Exception("El identificador del usuario no debe ser nulo");
                if (this.vista.UnidadOperativaId == null) throw new Exception("El identificador de la Unidad Operativa no debe ser nulo ");

                //crea seguridad
                UsuarioBO usuario = new UsuarioBO { Id = this.vista.UsuarioAutenticado };
                AdscripcionBO adscripcion = new AdscripcionBO { UnidadOperativa = new UnidadOperativaBO { Id = this.vista.UnidadOperativaId } };
                SeguridadBO seguridad = new SeguridadBO(Guid.Empty, usuario, adscripcion);

                //consulta lista de acciones permitidas
                List<CatalogoBaseBO> acciones = FacadeBR.ConsultarAccion(dataContext, seguridad);

                //Se valida si el usuario tiene permiso para consultar
                if (!ExisteAccion(acciones, "UI CONSULTAR") || !ExisteAccion(acciones, "CONSULTAR") || !ExisteAccion(acciones, "CONSULTARCOMPLETO"))
                    this.vista.RedirigirSinPermisoAcceso();
                //Se valida si el usuario tiene permiso para insertar cuenta cliente
            }
            catch (Exception ex) {
                throw new Exception(this.nombreClase + ".EstablecerSeguridad:" + ex.Message);
            }
        }

        /// <summary>
        /// Verifica si existe una acción en la lista de acciones permitidas.
        /// </summary>
        /// <param name="acciones">Lista de Acciones permitidas.</param>
        /// <param name="nombreAccion">Acción a verificar.</param>
        /// <returns>Retorna True si encontró resultados, en caso contrario retorna False.</returns>
        private bool ExisteAccion(List<CatalogoBaseBO> acciones, string nombreAccion){
                if (acciones != null && acciones.Count > 0 &&
                    acciones.Exists(
                        p => p != null && p.Nombre != null && p.Nombre.Trim().ToUpper() == nombreAccion.Trim().ToUpper()))
                    return true;

                return false;
            }
            
            #endregion

            #region Form Búsqueda

        /// <summary>
        /// Realiza la búsqueda de los Mantenimientos Unidad Idealease por el Número Económico. En caso de no encontrar resultados despliega un mensaje de Error.
        /// </summary>
        public void MostrarDatosInterfaz() {
            if (vista.FolioOrdenServicio != null && vista.FolioOrdenServicio > 0) {
                bool unidadEncontrada = BuscarPorOrdenServicio();
                if (!unidadEncontrada) {
                    vista.MostrarMensaje("No se encontraron resultados para este Folio Orden de Servicio.", ETipoMensajeIU.ADVERTENCIA);
                }
            } else if(this.vista.NumeroEconomico != null && this.vista.NumeroEconomico.Trim() != null){
                this.vista.EsUnidad = true;
                bool unidadEncontrada = BuscarPorUnidad();
                if (!unidadEncontrada) {
                    this.vista.MostrarMensaje("No se encontraron resultados para esta unidad", ETipoMensajeIU.ADVERTENCIA);
                }
            } else {
                this.vista.EsUnidad = true;
                bool esUnidad = BuscarPorUnidad();
                if (!esUnidad) {
                    this.vista.EsUnidad = false;
                    bool esEquipoAliado = BuscarPorEquipoAliado();
                    if (!esEquipoAliado) {
                        this.vista.MostrarMensaje("No se encontraron coincidencias con los campos solicitados", ETipoMensajeIU.ADVERTENCIA);
                    }
                }
            }
        }

        /// <summary>
        /// Realiza la búsqueda de los Mantenimientos Idealease por el Folio de la Orden de Servicio.
        /// </summary>
        /// <returns>True si encontró resultados, en caso contrario retorna False.</returns>
        private bool BuscarPorOrdenServicio(){
            bool encontrado = false;
            OrdenServicioBO filtroOrdenServicioElider = new OrdenServicioBO() {
                Id = this.vista.FolioOrdenServicio
            };
            OrdenServicioBO ordenServicioElider = FacadeBR.ConsultarOrdenServicio(dataContext, filtroOrdenServicioElider).FirstOrDefault();
            if (ordenServicioElider != null && ordenServicioElider.Id != null) {
                encontrado = BuscarPorOrdenServicioElider(ordenServicioElider);
                vista.CargarListaMantenimientos();
            } 

            return encontrado;
        }

        /// <summary>
        /// Realiza la búsqueda del Mantenimiento Idealease de acuerdo al Tipo de Unidad y la Orden de Servicio de E-Lider.
        /// </summary>
        /// <param name="ordenServicioElider">La Orden de Servicio E-Lider.</param>
        /// <returns>Retorna True si encontró resultados, en caso contrario retorna False.</returns>
        private bool BuscarPorOrdenServicioElider(OrdenServicioBO ordenServicioElider) {
            bool encontrado = false;
            
            BPMO.Servicio.Catalogos.BO.UnidadBO filtroUnidadElider = new BPMO.Servicio.Catalogos.BO.UnidadBO() {
                Id = ordenServicioElider.Unidad.Id
            };
            BPMO.Servicio.Catalogos.BO.UnidadBO unidadElider = FacadeBR.ConsultarUnidad(dataContext, filtroUnidadElider);
            if (unidadElider != null && unidadElider.Id != null) {
                ordenServicioElider.Unidad = unidadElider;
                int? tipoUnidad = ordenServicioElider.Unidad.TipoUnidad.Id;
               
                if (tipoUnidad != null && tipoUnidad <= 2) {
                    switch (tipoUnidad) {
                        case (int)ETipoEquipoLider.UNIDAD:
                            this.vista.EsUnidad = true;
                            encontrado = BuscarUnidadPorOrdenElider(ordenServicioElider);
                            break;
                        case (int)ETipoEquipoLider.EQUIPO_ALIADO:
                            this.vista.EsUnidad = false;
                            encontrado = BuscarEquipoAliadoPorOrdenElider(ordenServicioElider);
                            break;
                    }
                }else if(tipoUnidad > 2){
                    this.vista.EsUnidad = true;
                    encontrado = BuscarUnidadPorOrdenElider(ordenServicioElider);
                    if (!encontrado) {
                        encontrado = BuscarEquipoAliadoPorOrdenElider(ordenServicioElider);
                    }
                }
            }

            return encontrado;
        }

        /// <summary>
        /// Realiza la búsqueda del Mantenimiento Unidad Idealease por la Orden de Servicio E-Lider. Obtiene los Mantenimientos Equipos Aliados Idealease 
        /// del Mantenimiento Unidad Idealease.
        /// </summary>
        /// <param name="ordenServicioElider">La Orden de Servicio E-Lider</param>
        /// <returns>Retorna True si encontró resultados, en caso contrario retorna False.</returns>
        private bool BuscarUnidadPorOrdenElider(OrdenServicioBO ordenServicioElider) {
            Boolean encontrado = false;

            MantenimientoUnidadBO filtroMantenimientoUnidadIdealease = new MantenimientoUnidadBO() {
                OrdenServicio = ordenServicioElider
            };

            MantenimientoUnidadBO mantenimientoUnidadIdealease = ctrlMantenimientoUnidadIdealease.Consultar(dataContext, filtroMantenimientoUnidadIdealease).FirstOrDefault();

            if (mantenimientoUnidadIdealease != null && mantenimientoUnidadIdealease.MantenimientoUnidadId != null) {

                BPMO.SDNI.Equipos.BO.UnidadBO filtroUnidadIdealease = new BPMO.SDNI.Equipos.BO.UnidadBO() {
                    UnidadID = mantenimientoUnidadIdealease.IngresoUnidad.Unidad.UnidadID
                };

                BPMO.SDNI.Equipos.BR.UnidadBR ctrlUnidadIdealease = new BPMO.SDNI.Equipos.BR.UnidadBR();
                BPMO.SDNI.Equipos.BO.UnidadBO unidadIdealease = ctrlUnidadIdealease.Consultar(dataContext, filtroUnidadIdealease).FirstOrDefault();

                if (unidadIdealease != null && unidadIdealease.UnidadID != null) {

                    if (unidadIdealease.Modelo == null || unidadIdealease.Modelo.Id == null) {
                        BPMO.SDNI.Equipos.BO.UnidadBO unidadCompleta = new BPMO.SDNI.Equipos.BO.UnidadBO(unidadIdealease.ActaNacimiento);
                        unidadIdealease.Modelo = unidadCompleta.Modelo;
                    }

                    mantenimientoUnidadIdealease.IngresoUnidad.Unidad = unidadIdealease;
                    mantenimientoUnidadIdealease.OrdenServicio = ordenServicioElider;
                    vista.MantenimientosUnidad = new List<MantenimientoUnidadBO>();
                    vista.MantenimientosUnidad.Add(mantenimientoUnidadIdealease);
                    
                    encontrado = true;
                }

            }

            return encontrado;
        }

        /// <summary>
        /// Realiza la búsqueda del Mantenimiento Equipo Aliado Idealease por la Orden de Servicio E-Lider. 
        /// </summary>
        /// <param name="ordenServicioElider"></param>
        /// <returns></returns>
        private bool BuscarEquipoAliadoPorOrdenElider(OrdenServicioBO ordenServicioElider) {
            bool encontrado = false;
            MantenimientoEquipoAliadoBO filtroMantenientoEquipoIdealease = new MantenimientoEquipoAliadoBO() {
                OrdenServicio = ordenServicioElider
            };
            MantenimientoEquipoAliadoBO mantenimientoEquipoIdealease = ctrlMatenimientoEquipoAliadoIdealease.Consultar(dataContext, filtroMantenientoEquipoIdealease).FirstOrDefault();

            if (mantenimientoEquipoIdealease != null && mantenimientoEquipoIdealease.MantenimientoEquipoAliadoId != null) {

                EquipoAliadoBO filtroEquipoIdealease = new EquipoAliadoBO() {
                    IDLider = ordenServicioElider.Unidad.Id
                };

                EquipoAliadoBR ctrlEquipoIdealease = new EquipoAliadoBR();
                EquipoAliadoBO equipoIdealease = ctrlEquipoIdealease.Consultar(dataContext, filtroEquipoIdealease).FirstOrDefault();

                if (equipoIdealease != null && equipoIdealease.EquipoID != null) {
                    if (equipoIdealease.Modelo == null || equipoIdealease.Modelo.Id == null) {
                        BPMO.Servicio.Catalogos.BO.UnidadBO filtro = new BPMO.Servicio.Catalogos.BO.UnidadBO(){
                            Id = equipoIdealease.IDLider
                        };
                        BPMO.Servicio.Catalogos.BO.UnidadBO unidadLider = FacadeBR.ConsultarUnidad(dataContext, filtro);
                        equipoIdealease.Modelo = unidadLider.ConfiguracionModeloMotorizacion.Modelo;
                    }

                    mantenimientoEquipoIdealease.IngresoEquipoAliado.EquipoAliado = equipoIdealease;
                    mantenimientoEquipoIdealease.OrdenServicio = ordenServicioElider;
                    vista.MantenimientosEquipoAliado = new List<MantenimientoEquipoAliadoBO>();
                    vista.MantenimientosEquipoAliado.Add(mantenimientoEquipoIdealease);
                    encontrado = true;
                }
            }

            return encontrado;
        }

        /// <summary>
        /// Crea y obtiene un nuevo Filtro de la Unidad E-Lider por medio del Modelo seleccionado, el Número de Serie y el Número Económico, para 
        /// realizar la búsqueda de las Unidades E-Lider.
        /// </summary>
        /// <returns>El filtro de la Unidad E-Lider.</returns>
        private BPMO.SDNI.Equipos.BO.UnidadBO getFiltroUnidadPorModelo() {
            BPMO.SDNI.Equipos.BO.UnidadBO unidad = new BPMO.SDNI.Equipos.BO.UnidadBO();
            if(vista.ModeloSeleccionado.Id != null){
                unidad.Modelo = vista.ModeloSeleccionado;
            }
            if (vista.NumeroVIN != null && vista.NumeroVIN.Trim() != null && !vista.NumeroVIN.Equals("")) {
                unidad.NumeroSerie = vista.NumeroVIN;
            }
            if (vista.NumeroEconomico != null && vista.NumeroEconomico != null && !vista.NumeroEconomico.Equals("")) {
                unidad.NumeroEconomico = vista.NumeroEconomico;
            }
            return unidad;
        }

        /// <summary>
        /// Crea y obtiene el Filtro del Mantenimiento Unidad Idealease por medio de la Unidad seleccionada, para realizar la búsqueda de los 
        /// Mantenimientos Unidad Idealease.
        /// </summary>
        /// <param name="unidadEncontrada">La Unidad seleccionada.</param>
        /// <returns>El filtro del Mantenimiento Unidad Idealease.</returns>
        private MantenimientoUnidadBO getFiltroMatenimientoUnidadBO(BPMO.SDNI.Equipos.BO.UnidadBO unidadEncontrada) { 
             MantenimientoUnidadBO filtroMantenimiento = new MantenimientoUnidadBO() {
                Activo = true,
                IngresoUnidad = new IngresoUnidadBO() {
                    Unidad = new BPMO.SDNI.Equipos.BO.UnidadBO(){
                        UnidadID = unidadEncontrada.UnidadID
                    }
                }
            };

            return filtroMantenimiento;
        }

        /// <summary>
        /// Realiza la búsqueda de Mantenimientos Unidades Idealease por el Modelo seleccionado.
        /// </summary>
        /// <returns>Retorna True si encontro resultados, en caso contrario retorna False.</returns>
        private bool BuscarPorUnidad() {
            bool mantenimientoEncontrado = true;
            BPMO.SDNI.Equipos.BR.UnidadBR ctrlUnidad = new BPMO.SDNI.Equipos.BR.UnidadBR();
            BPMO.SDNI.Equipos.BO.UnidadBO unidad = new BPMO.SDNI.Equipos.BO.UnidadBO();
            List<BPMO.SDNI.Equipos.BO.UnidadBO> unidadesEncontradas = ctrlUnidad.ConsultarUnidadBOPorModelo(dataContext, getFiltroUnidadPorModelo());
            if (unidadesEncontradas.Count > 0) {
                List<MantenimientoUnidadBO> nuevaLista = new List<MantenimientoUnidadBO>();
                foreach (BPMO.SDNI.Equipos.BO.UnidadBO unidadEncontrada in unidadesEncontradas) {
                    List<MantenimientoUnidadBO> resultMantenimientos = ctrlMantenimientoUnidadIdealease.Consultar(dataContext, getFiltroMatenimientoUnidadBO(unidadEncontrada));
                    if (resultMantenimientos != null && resultMantenimientos.Count > 0) {
                        foreach (MantenimientoUnidadBO mantenimiento in resultMantenimientos) {
                            try {
                                if (mantenimiento.OrdenServicio.Id != null) {
                                    OrdenServicioBO ordenServicio = FacadeBR.ConsultarOrdenServicio(dataContext, mantenimiento.OrdenServicio).FirstOrDefault();
                                    if(ordenServicio != null){
                                        mantenimiento.OrdenServicio = ordenServicio;
                                        mantenimiento.IngresoUnidad.Unidad = unidadEncontrada;
                                        nuevaLista.Add(mantenimiento);
                                    }
                                }
                            } catch (Exception e) { }
                        }
                    } 
                }
                mantenimientoEncontrado = nuevaLista.Count > 0;
                this.vista.MantenimientosUnidad = nuevaLista;
                this.vista.CargarListaMantenimientos();
            } else {
                mantenimientoEncontrado = false;
            }

            return mantenimientoEncontrado;
        }

        /// <summary>
        /// Crea y obtiene un nuevo Filtro del Mantenimiento Equipo Aliado Idealease por medio del Equipo Aliado Idealease seleccionado, para 
        /// para realizar la búsqueda de los Mantenimientos Equipos Aliados Idealease.
        /// </summary>
        /// <param name="equipoAliado">Equipo Aliado seleccionado</param>
        /// <returns>Un objeto de Tipo MantenimientoEquipoAliadoBO</returns>
        private MantenimientoEquipoAliadoBO getFiltroMantenimientoEquipoAliado(EquipoAliadoBO equipoAliado){
            MantenimientoEquipoAliadoBO filtroMantenimiento = new MantenimientoEquipoAliadoBO() {
                Activo = true,
                IngresoEquipoAliado = new IngresoEquipoAliadoBO(){
                    EquipoAliado = equipoAliado
                }
            };

            return filtroMantenimiento;
        }

        /// <summary>
        /// Realiza la búsqueda de Mantenimientos Equipo Aliado Idealease por el Modelo del Equipo Aliado seleccionado.
        /// </summary>
        /// <returns>Retorna True si encontro resultados, en caso contrario retorna False.</returns>
        private bool BuscarPorEquipoAliado() {
            bool mantenimientoEcontrado = true;
            EquipoAliadoBR ctrlEquipoAliado = new EquipoAliadoBR();
            List<EquipoAliadoBO> equiposAliados = ctrlEquipoAliado.ConsultarEquipoAliadoBOPorModelo(dataContext, getFiltroEquipoAliado());
            
            if(equiposAliados.Count > 0) {
                List<MantenimientoEquipoAliadoBO> nuevaLista = new List<MantenimientoEquipoAliadoBO>();
                foreach (EquipoAliadoBO equipoAliado in equiposAliados) {
                    if (equipoAliado != null && equipoAliado.EquipoID != null) {
                        List<MantenimientoEquipoAliadoBO> resultMantenimientos = ctrlMatenimientoEquipoAliadoIdealease.Consultar(dataContext, getFiltroMantenimientoEquipoAliado(equipoAliado));
                        if (resultMantenimientos != null && resultMantenimientos.Count > 0) {
                            foreach (MantenimientoEquipoAliadoBO mantenimiento in resultMantenimientos) {
                                if (mantenimiento.OrdenServicio.Id != null) {
                                    mantenimiento.OrdenServicio = FacadeBR.ConsultarOrdenServicio(dataContext, mantenimiento.OrdenServicio).FirstOrDefault();
                                    mantenimiento.IngresoEquipoAliado.EquipoAliado = equipoAliado;
                                    nuevaLista.Add(mantenimiento);
                                } 
                            }
                        }
                    }
                }
                mantenimientoEcontrado = nuevaLista.Count > 0;
                this.vista.EsUnidad = false;
                this.vista.MantenimientosEquipoAliado = nuevaLista;
                this.vista.CargarListaMantenimientos();
            } else {
                mantenimientoEcontrado = false;
            }
                
            return mantenimientoEcontrado;
        }

        /// <summary>
        /// Crea y obtiene el Filtro del Equipo Aliado E-Lider por medio del Modelo seleccionado y el Número de Serie, para realizar 
        /// la búsqueda de los Equipos Aliados E-Lider.
        /// </summary>
        /// <returns>El filtro EquipoAliadoBO.</returns>
        private EquipoAliadoBO getFiltroEquipoAliado() {
            EquipoAliadoBO filtroEquipoAliado = new EquipoAliadoBO();
            if(vista.NumeroVIN != null){
                filtroEquipoAliado.NumeroSerie = vista.NumeroVIN;
            }
            if (vista.ModeloSeleccionado.Id != null) {
                filtroEquipoAliado.Modelo = vista.ModeloSeleccionado;
            }
            return filtroEquipoAliado;
        }

        /// <summary>
        /// Construye el Mantenimiento Unidad Idealease seleccionado.
        /// </summary>
        public void ConsultarMantenimientoUnidadCompleto() {
            try {
                MantenimientoUnidadBO seleccionado = this.vista.MantenimientosUnidad[vista.Index];
                seleccionado.IngresoUnidad.Unidad.Cliente = getClienteCompleto(seleccionado.IngresoUnidad.Unidad.Cliente);
                seleccionado.IngresoUnidad.Controlista = getControlistaCompleto(seleccionado.IngresoUnidad.Controlista);
                seleccionado.Taller = getTallerCompleto(seleccionado.Taller);
                seleccionado.IngresoUnidad.Unidad.Sucursal = getSucursalCompleto(seleccionado.IngresoUnidad.Unidad.Sucursal);
                LlenarEquiposAliados(seleccionado);
            } catch(Exception e)
            {
                this.vista.MostrarMensaje("Error al consultar el mantenimiento", ETipoMensajeIU.ERROR, e.Message);
            }
        }

        /// <summary>
        /// Crea y obtiene un nuevo Filtro del Cliente de la Unidad E-Lider, para realizar la búsqueda de los Clientes.
        /// </summary>
        /// <param name="cliente">El filtro para obtener el Cliente.</param>
        /// <returns>Objeto de tipo ClienteBO.</returns>
        private ClienteBO getClienteCompleto(ClienteBO cliente) {
            ClienteBO clienteCompleto = new ClienteBO();
            if (cliente == null)
                cliente = new ClienteBO();
            if(cliente.Id != null) {
                ClienteBO filtroCliente = new ClienteBO() {
                    Id = cliente.Id
                };
                clienteCompleto = FacadeBR.ConsultarCliente(dataContext, filtroCliente).FirstOrDefault();
            }
            return clienteCompleto;
        }

        /// <summary>
        /// Crea y obtiene un nuevo Filtro del Controlista, para realizar la búsqueda de los Usuarios Controlistas.
        /// </summary>
        /// <param name="controlista">Filtro para obtener el Controlista.</param>
        /// <returns>Objeto de tipo UsuarioBO.</returns>
        private UsuarioBO getControlistaCompleto(UsuarioBO controlista) {
            UsuarioBO controlistaCompleto = new UsuarioBO();
            if (controlista == null)
                controlista = new UsuarioBO();
            if(controlista.Id != null) {
                UsuarioBO filtroControlista = new UsuarioBO() { 
                        Id = controlista.Id
                };
                controlistaCompleto = FacadeBR.ConsultarUsuario(dataContext, filtroControlista).FirstOrDefault();
            }
            return controlistaCompleto;
        }

        /// <summary>
        /// Crea y obtiene un nuevo Filtro del Taller, para realizar la búsqueda de los Talleres.
        /// </summary>
        /// <param name="taller">Filtro para obtener el Taller.</param>
        /// <returns>Objeto de Tipo TallerBO</returns>
        private TallerBO getTallerCompleto(TallerBO taller) {
            var tallerCompleto = new TallerBO();
            if (taller == null)
                taller = new TallerBO();

            if (taller.Id != null) {
                TallerBO filtroTaller = new TallerBO(){
                  Id = taller.Id  
                };
                tallerCompleto = FacadeBR.ConsultarTaller(dataContext, filtroTaller).FirstOrDefault();
            }

            return tallerCompleto;
        }

        /// <summary>
        /// Crea y obtiene un nuevo Filtro de la Sucursal, para realizar la búsqueda de las Sucursales.
        /// </summary>
        /// <param name="sucursal">Filtro para obtener la Sucursal.</param>
        /// <returns>Objeto de tipo SucursalBO.</returns>
        private SucursalBO getSucursalCompleto(SucursalBO sucursal) {
            SucursalBO sucursalCompleto = new SucursalBO();
            if (sucursal == null)
                sucursal = new SucursalBO();
            if (sucursal.Id != null) { 
                SucursalBO filtroSucursal = new SucursalBO(){
                    Id = sucursal.Id
                };
                sucursalCompleto = FacadeBR.ConsultarSucursal(dataContext, filtroSucursal).FirstOrDefault();
            }
            return sucursalCompleto;
        }

        /// <summary>
        /// Crea y obtiene los Mantenimientos Equipos Aliados Idealease. Crea un nuevo diccionario de datos para los Mantenimientos 
        /// Equipos Aliados.
        /// </summary>
        /// <param name="seleccionado">Filtro de Mantenimiento Unidad Idealease.</param>
        private void LlenarEquiposAliados(MantenimientoUnidadBO seleccionado) { 
            List<EquipoAliadoBO> equiposAliados = new SDNI.Equipos.BR.UnidadBR().ConsultarEquipoAliado(dataContext, seleccionado.IngresoUnidad.Unidad);
            List<MantenimientoEquipoAliadoBO> mantenimientosEquiposAliados = new MantenimientoUnidadBR().ConsultarMantenimientosEquiposAliados(dataContext, seleccionado).MantenimientoEquiposAliados;
            DataSet mantenimientosEquipoAliado = new DataSet();
            DataTable mantenimientoEquipoAliado = new DataTable();
            mantenimientoEquipoAliado.Columns.Add("NumeroEconomico");
            mantenimientoEquipoAliado.Columns.Add("Modelo");
            mantenimientoEquipoAliado.Columns.Add("Kilometraje");
            mantenimientoEquipoAliado.Columns.Add("Horometro");
            mantenimientoEquipoAliado.Columns.Add("Cliente");
            mantenimientoEquipoAliado.Columns.Add("TipoMantenimiento");
            
            if (equiposAliados != null && equiposAliados.Count > 0) {    
                foreach (EquipoAliadoBO equipoAliado in equiposAliados) {
                    DataRow row = mantenimientoEquipoAliado.NewRow();
                    mantenimientoEquipoAliado.Rows.Add(getEquipoAliadoToDataRow(row, equipoAliado, mantenimientosEquiposAliados, seleccionado));
                }
            }
            if (mantenimientoEquipoAliado.Rows.Count == 0) {
                DataRow row = mantenimientoEquipoAliado.NewRow();
            }
            mantenimientosEquipoAliado.Tables.Add(mantenimientoEquipoAliado);
            vista.EquiposAliadosMantenimiento = mantenimientosEquipoAliado;
            vista.MantenimientoToHash = getMantenimientoUnidadToHash(seleccionado);
        }

        /// <summary>
        /// Convierte el Mantenimiento Equipo Aliado Idealease en un diccionario de datos.
        /// </summary>
        /// <param name="mantenimiento">El direccionario de datos.</param>
        /// <param name="equipoAliado">El Equipo Aliado relacionado.</param>
        /// <param name="mantenimientosEquiposAliados">Lista de Mantenimientos Equipos Aliados Idealease del Mantenimiento Unidad Idealease.</param>
        /// <param name="seleccionado">Mantenimiento Unidad Idealease encontrada.</param>
        /// <returns></returns>
        private DataRow getEquipoAliadoToDataRow(DataRow mantenimiento, EquipoAliadoBO equipoAliado, List<MantenimientoEquipoAliadoBO> mantenimientosEquiposAliados, MantenimientoUnidadBO seleccionado) {
            
            MantenimientoEquipoAliadoBO mantenimientoEquipoAliadoEncontrado = mantenimientosEquiposAliados.Find(x => x.IngresoEquipoAliado.EquipoAliado.EquipoAliadoID == equipoAliado.EquipoAliadoID);
            string kilometraje="", horometro="", tipoMantenimiento="", modelo="";
            if (mantenimientoEquipoAliadoEncontrado != null && mantenimientoEquipoAliadoEncontrado.OrdenServicio.Id != null) {
                OrdenServicioBO ordenLider = FacadeBR.ConsultarOrdenServicio(dataContext, mantenimientoEquipoAliadoEncontrado.OrdenServicio).FirstOrDefault();
                if(ordenLider.Unidad.KmHrs != null) {
                    if((bool)ordenLider.Unidad.KmHrs) {
                        horometro = ordenLider.Unidad.KmHrsInicial.ToString();
                        kilometraje = mantenimientoEquipoAliadoEncontrado.KilometrajeEntrada.ToString();
                    } else {
                        kilometraje = ordenLider.Unidad.KmHrsInicial.ToString();
                    }
                }
                tipoMantenimiento = mantenimientoEquipoAliadoEncontrado.TipoMantenimiento.ToString();
            } else {
                kilometraje = horometro = tipoMantenimiento = "Sin orden de Servicio";
            }
            if (equipoAliado.Modelo != null && equipoAliado.Modelo.Id != null) {
                modelo = equipoAliado.Modelo.Nombre;
            } else {
                BPMO.Servicio.Catalogos.BO.UnidadBO filtro = new BPMO.Servicio.Catalogos.BO.UnidadBO(){
                    Id = equipoAliado.IDLider
                };
                BPMO.Servicio.Catalogos.BO.UnidadBO equipoAliadoElider = FacadeBR.ConsultarUnidad(dataContext, filtro);
                if (equipoAliadoElider != null) {
                    modelo = equipoAliadoElider.ConfiguracionModeloMotorizacion.Modelo != null && equipoAliadoElider.ConfiguracionModeloMotorizacion.Modelo.Nombre != null ? 
                        equipoAliadoElider.ConfiguracionModeloMotorizacion.Modelo.Nombre : "";
                }
            }
            mantenimiento.ItemArray = new object[]{"",modelo, kilometraje, horometro, seleccionado.IngresoUnidad.Unidad.Cliente.NombreCompleto, tipoMantenimiento};
            return mantenimiento;
        }

        /// <summary>
        /// Convierte el Mantenimiento Unidad Idealease en un diccionario de datos.
        /// </summary>
        /// <param name="seleccionado">El Mantenimiento Unidad Idealease seleccionado.</param>
        /// <returns>Un nuevo diccionario de datos.</returns>
        private Dictionary<string, string> getMantenimientoUnidadToHash(MantenimientoUnidadBO seleccionado) {
            Dictionary<string, string> datos = new Dictionary<string, string>();
            BPMO.SDNI.Equipos.BO.UnidadBO unidad = seleccionado.IngresoUnidad.Unidad;
            datos.Add("id", seleccionado.MantenimientoUnidadId.ToString());
            datos.Add("numeroSerie", unidad.NumeroSerie);
            datos.Add("numeroEconomico", unidad.NumeroEconomico);
            datos.Add("modelo", unidad.Modelo.Nombre);
            datos.Add("cliente", unidad.Cliente.NombreCompleto);
            datos.Add("kilometraje", seleccionado.KilometrajeEntrada.ToString());
            datos.Add("horometro", seleccionado.HorasEntrada.ToString());
            datos.Add("totalCombustible", seleccionado.CombustibleTotal.ToString());
            datos.Add("sucursal", unidad.Sucursal.Nombre);
            datos.Add("taller", seleccionado.Taller.Nombre);
            datos.Add("tipoMantenimiento", seleccionado.TipoMantenimiento.ToString());
            datos.Add("combustibleEntrada", seleccionado.CombustibleEntrada.ToString());
            datos.Add("combustibleSalida", seleccionado.CombustibleSalida.ToString());
            if(seleccionado.TipoServicio != null && seleccionado.TipoServicio.Id != null && seleccionado.TipoServicio.Id > 0){
                switch (seleccionado.TipoServicio.Id) {
                    case (int)ETipoMantenimientoIdealease.CORRECTIVO:
                        datos.Add("tipoServicio", "Correctivo");
                        break;
                    case (int)ETipoMantenimientoIdealease.PREVENTIVO:
                        datos.Add("tipoServicio", "Preventivo");
                        break;
                }
            } else {
                datos.Add("tipoServicio", "");
            }
            datos.Add("controlista", seleccionado.IngresoUnidad.Controlista.Nombre);
            datos.Add("operador", seleccionado.IngresoUnidad.Operador);
            datos.Add("inventario", seleccionado.Inventario);
            datos.Add("falla", seleccionado.DescripcionFalla);
            datos.Add("codigosFalla", seleccionado.CodigosFalla);
            datos.Add("observaciones", seleccionado.IngresoUnidad.ObservacionesOperador);
            datos.Add("fechaApertura", seleccionado.FechaArranque.ToString());
            datos.Add("fechaCierre", seleccionado.FechaSalida.ToString());

            return datos;
        }

        /// <summary>
        /// Construye el Mantenimiento Equipo Aliado Idealease seleccionado.
        /// </summary>
        public void ConsultarMantenimientoEquipoCompleto() {
            MantenimientoEquipoAliadoBO seleccionado = vista.MantenimientosEquipoAliado[vista.Index];
            seleccionado.Taller = getTallerCompleto(seleccionado.Taller);
            seleccionado.IngresoEquipoAliado.EquipoAliado.Sucursal = getSucursalCompleto(seleccionado.IngresoEquipoAliado.EquipoAliado.Sucursal);
            vista.MantenimientoToHash = getMantenimientoEquipoToHash(seleccionado);
        }

        /// <summary>
        /// Convierte el Mantenimiento Equipo Aliado Idealease en un diccionario de datos.
        /// </summary>
        /// <param name="seleccionado">El Mantenimiento Equipo Aliado Idealease seleccionado.</param>
        /// <returns>Un nuevo diccionario de datos.</returns>
        private Dictionary<string, string> getMantenimientoEquipoToHash(MantenimientoEquipoAliadoBO seleccionado){
            Dictionary<string, string> datos = new Dictionary<string, string>();
            EquipoAliadoBO equipo = seleccionado.IngresoEquipoAliado.EquipoAliado;
            datos.Add("id", seleccionado.MantenimientoEquipoAliadoId.ToString());
            datos.Add("numeroSerie", equipo.NumeroSerie);
            datos.Add("numeroEconomico", "N/A");
            datos.Add("modelo", equipo.Modelo.Nombre);
            MantenimientoUnidadBO m = ctrlMatenimientoEquipoAliadoIdealease.ConsultarMantenimientoUnidadPorMantenimientoEquipoAliado(dataContext, seleccionado.MantenimientoEquipoAliadoId);
            if (m.IngresoUnidad.Unidad.Cliente.Id != null) {
                m.IngresoUnidad.Unidad.Cliente = getClienteCompleto(m.IngresoUnidad.Unidad.Cliente);
                datos.Add("cliente", m.IngresoUnidad.Unidad.Cliente.NombreCompleto);
            } else {
                datos.Add("cliente", "N/D");
            }
            datos.Add("kilometraje", seleccionado.KilometrajeEntrada != null ? seleccionado.KilometrajeEntrada.ToString() : "N/D");
            datos.Add("horometro", seleccionado.HorasEntrada != null ? seleccionado.HorasEntrada.ToString() : "N/D");
            datos.Add("totalCombustible", "");
            datos.Add("sucursal", equipo.Sucursal.Nombre);
            datos.Add("taller", seleccionado.Taller.Nombre);
            datos.Add("tipoMantenimiento", seleccionado.TipoMantenimiento.ToString());
            datos.Add("combustibleEntrada", "");
            datos.Add("combustibleSalida", "");
            if(seleccionado.TipoServicio != null && seleccionado.TipoServicio.Id != null && seleccionado.TipoServicio.Id > 0){
                switch (seleccionado.TipoServicio.Id) {
                    case (int)ETipoMantenimientoIdealease.CORRECTIVO:
                        datos.Add("tipoServicio", "Correctivo");
                        break;
                    case (int)ETipoMantenimientoIdealease.PREVENTIVO:
                        datos.Add("tipoServicio", "Preventivo");
                        break;
                }
            } else {
                datos.Add("tipoServicio", "N/D");
            }

            if (seleccionado.Auditoria.UC != null) {
                UsuarioBO filtroControlista = new UsuarioBO() {
                    Id = seleccionado.Auditoria.UC
                };
                UsuarioBO controlista = FacadeBR.ConsultarUsuario(dataContext, filtroControlista).FirstOrDefault();
                if (controlista != null && controlista.Id != null) {
                    datos.Add("controlista", controlista.Nombre);
                }
            } else {
                datos.Add("controlista", "N/D");
            }
            datos.Add("operador", m.IngresoUnidad.Operador);
            datos.Add("inventario", seleccionado.Inventario);
            datos.Add("falla", seleccionado.DescripcionFalla);
            datos.Add("codigosFalla", seleccionado.CodigosFalla);
            datos.Add("observaciones", seleccionado.IngresoEquipoAliado.ObservacionesOperador);
            datos.Add("fechaApertura", seleccionado.FechaArranque.ToString());
            datos.Add("fechaCierre", seleccionado.FechaSalida.ToString());

            return datos;
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
                    EquipoBepensaBO ebBO = new EquipoBepensaBO();
                    ebBO.ActivoFijo = new ActivoFijoBO();
                    ebBO.ActivoFijo.Auditoria = new AuditoriaBO();
                    ebBO.Unidad = new Servicio.Catalogos.BO.UnidadBO();
                    ebBO.Unidad.Auditoria = new AuditoriaBO();
                    ebBO.Unidad.ClasificadorAplicacion = new ClasificadorAplicacionBO();
                    ebBO.Unidad.ClasificadorAplicacion.Auditoria = new AuditoriaBO();
                    ebBO.Unidad.Cliente = new ClienteBO();
                    ebBO.Unidad.Cliente.Auditoria = new AuditoriaBO();
                    ebBO.Unidad.ConfiguracionModeloMotorizacion = new ConfiguracionModeloMotorizacionBO();
                    ebBO.Unidad.ConfiguracionModeloMotorizacion.Auditoria = new AuditoriaBO();
                    ebBO.Unidad.ConfiguracionModeloMotorizacion.ClasificadorMotorizacion = new ClasificadorMotorizacionBO();
                    ebBO.Unidad.ConfiguracionModeloMotorizacion.ClasificadorMotorizacion.Auditoria = new AuditoriaBO();
                    ebBO.Unidad.ConfiguracionModeloMotorizacion.Modelo = new ModeloBO();
                    ebBO.Unidad.ConfiguracionModeloMotorizacion.Modelo.Auditoria = new AuditoriaBO();
                    ebBO.Unidad.ConfiguracionModeloMotorizacion.Modelo.Marca = new MarcaBO();
                    ebBO.Unidad.ConfiguracionModeloMotorizacion.Modelo.Marca.Auditoria = new AuditoriaBO();
                    ebBO.Unidad.Distribuidor = new DistribuidorBO();
                    ebBO.Unidad.Distribuidor.Auditoria = new AuditoriaBO();
                    ebBO.Unidad.TipoUnidad = new TipoUnidadBO();
                    ebBO.Unidad.TipoUnidad.Auditoria = new AuditoriaBO();

                    ebBO.Unidad.NumeroSerie = this.vista.NumeroVIN;
                    ebBO.Unidad.Activo = true;
                    ebBO.ActivoFijo.NumeroSerie = this.vista.NumeroVIN;
                    ebBO.ActivoFijo.Libro = this.vista.LibroActivos;
                    obj = ebBO;
                    break;
                case "Modelo":
                    ModeloBO modelo = new ModeloBO();
                    if (vista.ModeloNombre != null && vista.ModeloNombre.Trim() != null)
                        modelo.Nombre = "%"+this.vista.ModeloNombre+"%";
                    obj = modelo;
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
                    EquipoBepensaBO ebBO = (EquipoBepensaBO)selecto;
                    if (ebBO == null) ebBO = new EquipoBepensaBO();

                    if (ebBO.NumeroSerie != null) {
                        this.vista.NumeroVIN = ebBO.NumeroSerie;
                    } else {
                        this.vista.NumeroVIN = null;
                    }
                    
                    if (ebBO.Unidad.ConfiguracionModeloMotorizacion.Modelo.Id != null){
                        if(ebBO.Unidad.ConfiguracionModeloMotorizacion.Modelo.Nombre == null) { 
                            ModeloBO filtroModelo = new ModeloBO(){
                                Id = ebBO.Unidad.ConfiguracionModeloMotorizacion.Modelo.Id
                            };
                            ModeloBO modelo = FacadeBR.ConsultarModelo(dataContext, filtroModelo).FirstOrDefault();
                            if (modelo.Id != null) {
                                vista.ModeloSeleccionado = modelo;
                                vista.ModeloNombre = modelo.Nombre;
                            }
                        }else{
                            vista.ModeloSeleccionado = ebBO.Unidad.ConfiguracionModeloMotorizacion.Modelo;
                            vista.ModeloNombre = vista.ModeloSeleccionado.Nombre;
                        }
                    }
                    break;
            }
        }        

                #endregion
            
            #endregion

        #endregion

    }
}
