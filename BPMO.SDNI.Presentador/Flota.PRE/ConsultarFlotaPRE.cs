//Esta clase satisface los requerimientos especificados en el caso de uso CU006 Consultar Flota de Renta Diaria
using System;
using System.Collections.Generic;
using BPMO.Basicos.BO;
using BPMO.Facade.SDNI.BOF;
using BPMO.Facade.SDNI.BR;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Equipos.BOF;
using BPMO.SDNI.Flota.BO;
using BPMO.SDNI.Flota.BOF;
using BPMO.SDNI.Flota.BR;
using BPMO.SDNI.Flota.VIS;
using BPMO.Servicio.Catalogos.BO;
using UnidadBO = BPMO.SDNI.Equipos.BO.UnidadBO;

namespace BPMO.SDNI.Flota.PRE
{
    /// <summary>
    /// Presentador de la página de consulta
    /// </summary>
    public class ConsultarFlotaPRE
    {
        #region Atributos
        private readonly IConsultarFlotaVIS vista;
        private readonly IDataContext dctx;
        private const string nombreClase = "FlotaPRE";
        private readonly SeguimientoFlotaBR controlador;
        #endregion

        #region Constructor
        public ConsultarFlotaPRE(IConsultarFlotaVIS vista)
        {
            if(ReferenceEquals(vista, null))
                throw new Exception(String.Format("{0}: La vista proporcionada no puede ser nula",nombreClase));
            this.vista = vista;
            this.controlador = new SeguimientoFlotaBR();
            this.dctx = FacadeBR.ObtenerConexion();
        }
        #endregion

        #region Métodos
        /// <summary>
        /// Asigna los valores para la vista cuando se prepare la consulta
        /// </summary>
        public void PrepararBusqueda()
        {
            this.LimpiarCampos();
            this.EstablecerFiltros();
            
            this.EstablecerSeguridad(); 
        }  
        /// <summary>
        /// Convierte los datos de la vista a un objeto de negocio
        /// </summary>
        /// <returns></returns>
        public FlotaBOF InterfazUsuarioADatos()
        {
            FlotaBOF flota = new FlotaBOF();
            flota.Unidad = new UnidadBO{Modelo = new ModeloBO{ Marca = new MarcaBO()}};
            flota.Sucursales = new List<SucursalBO>();
            

            if (this.vista.SucursalID != null)
                flota.Sucursales.Add(new SucursalBO {Id = this.vista.SucursalID});
            if (!string.IsNullOrEmpty(this.vista.NumeroEconomico) && !string.IsNullOrWhiteSpace(this.vista.NumeroEconomico))
                flota.Unidad.NumeroEconomico = this.vista.NumeroEconomico;
            if (!string.IsNullOrEmpty(this.vista.NumeroSerie) && !string.IsNullOrWhiteSpace(this.vista.NumeroSerie))
                flota.Unidad.NumeroSerie = this.vista.NumeroSerie;
            if (this.vista.MarcaID.HasValue)
                flota.Unidad.Modelo.Marca.Id = this.vista.MarcaID;
            if (this.vista.ModeloID.HasValue)
                flota.Unidad.Modelo.Id = this.vista.ModeloID;
            if (this.vista.Anio.HasValue)
                flota.Unidad.Anio = this.vista.Anio.Value;
            if (!string.IsNullOrEmpty(this.vista.Placas) && !string.IsNullOrWhiteSpace(this.vista.Placas))
                flota.Placa = this.vista.Placas;
            if (this.vista.ModeloEAID.HasValue)//SC0019
                flota.ModeloEquipoAliado = new ModeloBO {Id = this.vista.ModeloEAID.Value};

            return flota;
        }
        /// <summary>
        /// Consulta la flota
        /// </summary>
        public void ConsultarFlota()
        {
            try
            {
                FlotaBO flota = this.controlador.ConsultarFlotaRentaDiaria(this.dctx,this.InterfazUsuarioADatos());
                if (flota == null) flota = new FlotaBO();

                this.vista.Resultado = flota.ElementosFlota;
                this.vista.CargarElementosFlotaEncontrados(flota.ElementosFlota);

                if (!(flota.ElementosFlota != null && flota.ElementosFlota.Count > 0))
                    this.vista.MostrarMensaje("La búsqueda no produjo resultados", ETipoMensajeIU.INFORMACION, "No se encontraron coincidencias con los criterios ingresados. Verifique sus datos");
            }
            catch(Exception ex)
            {
                this.vista.MostrarMensaje(string.Format("{0}.{1}: Inconsistencias al consultar la flota", nombreClase, "ConsultarFlota"), ETipoMensajeIU.ERROR, ex.Message);
            }
        }
        /// <summary>
        /// Establece el nuevo indice para la página
        /// </summary>
        /// <param name="nuevoIndicePagina"></param>
        public void CambiarPaginaResultado(int nuevoIndicePagina)
        {
            this.vista.IndicePaginaResultado = nuevoIndicePagina;
            this.vista.ActualizarResultado();
        }
        /// <summary>
        /// Redirige a la página del detalle
        /// </summary>
        /// <param name="UnidadID"></param>
        public void IrADetalle(int? UnidadID)
        {
            if (UnidadID.HasValue)
            {
                this.vista.EstablecerPaqueteNavegacion("DetalleFlotaUI", UnidadID);
                this.vista.IrADetalle();
            }
            else
            {
                this.vista.MostrarMensaje("No se ha proporcionado el identificador de la unidad que se quiere consultar a detalle.", ETipoMensajeIU.ADVERTENCIA);
            }
        }
        /// <summary>
        /// Verifica los permisos para los usuarios autenticados en el sistema
        /// </summary>
        public void EstablecerSeguridad()
        {
            try
            {
                //se valida que los datos del usuario y la unidad operativa no sean nulos
                if(ReferenceEquals(this.vista.Usuario, null))
                    throw new Exception("Es necesario proporcionar el usuario que esta autenticado en el sistema.");
                if(!this.vista.Usuario.Id.HasValue)
                    throw new Exception("Es necesario proporcionar el usuario que esta identificado en el sistema.");
                if (!this.vista.UnidadOperativa.Id.HasValue)
                    throw new Exception("Es necesario proporcionar la unidad operativa del usuario autenticado en el sistema.");

                UsuarioBO usr = new UsuarioBO {Id = this.vista.Usuario.Id.Value};
                AdscripcionBO adscripcion = new AdscripcionBO
                    {
                        UnidadOperativa = new UnidadOperativaBO {Id = this.vista.UnidadOperativa.Id.Value}
                    };
                SeguridadBO seguridadBO = new SeguridadBO(Guid.Empty, usr, adscripcion);

                //Se obtienen las acciones a las cuales el usuario tiene permiso en este proceso
                List<CatalogoBaseBO> lst = FacadeBR.ConsultarAccion(this.dctx, seguridadBO);

                //Se valida si el usuario tiene permiso para consultar
                if (!this.ExisteAccion(lst, "CONSULTAR"))
                    this.vista.RedirigirSinPermisoAcceso();
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0}.{1} Excepción interna:{2}{3}", this.ToString(),
                                                  "EstablecerSeguridad", Environment.NewLine, ex.Message));
            }
        }
        /// <summary>
        /// Valida una acción en especifico dentro de la lista de acciones permtidas para la pagina
        /// </summary>
        /// <param name="acciones">Listado de acciones permitidas para la página</param>
        /// <param name="accion">Acción que se desea validar</param>
        /// <returns>si la acción a evaluar se encuantra dentro de la lista de acciones permitidas se devuelve true. En caso ocntario false. bool</returns>                
        private bool ExisteAccion(List<CatalogoBaseBO> acciones, string accion)
        {
            if (acciones != null && acciones.Count > 0 && acciones.Exists(p => p != null && p.Nombre != null && p.Nombre.Trim().ToUpper() == accion.Trim().ToUpper()))
                return true;

            return false;
        }
        #region SC0019
        /// <summary>
        /// SC019
        /// Establece los filtros iniciales en caso de regresar a la página de la consulta
        /// </summary>
        private void EstablecerFiltros()
        {
            Dictionary<string, object> temp = this.ObtenerPaquetenavegacion() as Dictionary<string, object>;
            if (!ReferenceEquals(temp, null))
            {
                if (temp.ContainsKey("ObjetoFiltro"))
                {
                    if(temp["ObjetoFiltro"].GetType() == typeof(FlotaBOF))                        
                        this.DatosAIntefazUsuario(temp["ObjetoFiltro"]);
                    else
                        throw new Exception("Se esperaba un objeto FlotaBOF, el objeto proporcionado no cumple con esa caracteristica, intente de nuevo por favor.");
                }                

                if (temp.ContainsKey("Bandera"))
                {
                    if ((bool)temp["Bandera"])                    
                        this.ConsultarFlota();
                    if (temp.ContainsKey("PagActGrid"))
                        this.CambiarPaginaResultado((int)temp["PagActGrid"]);
                }
            }
            this.vista.LimpiarPaqueteNavegacion();
        }
        /// <summary>
        /// SC0019
        /// Obtiene la información capturada en la vista y la convierte en un objeto
        /// </summary>
        /// <returns>Objeto con los datos para la interfaz de usuario</returns>
        public object InterfazUsuarioADatoNavegacion()
        {
            FlotaBOF flota = new FlotaBOF();
            flota.ModeloEquipoAliado = new ModeloBO();
            flota.Unidad = new UnidadBO{Modelo = new ModeloBO{ Marca = new MarcaBO()}};
            flota.Sucursales = new List<SucursalBO>();
            SucursalBO sucursal = new SucursalBO();

            if (this.vista.SucursalID != null)
                sucursal.Id = this.vista.SucursalID.Value;
            if (!string.IsNullOrEmpty(this.vista.SucursalNombre) && !string.IsNullOrWhiteSpace(this.vista.SucursalNombre))
                sucursal.Nombre = this.vista.SucursalNombre.Trim().ToUpper();
            flota.Sucursales.Add(sucursal);
            if (!string.IsNullOrEmpty(this.vista.NumeroEconomico) && !string.IsNullOrWhiteSpace(this.vista.NumeroEconomico))
                flota.Unidad.NumeroEconomico = this.vista.NumeroEconomico;
            if (!string.IsNullOrEmpty(this.vista.NumeroSerie) && !string.IsNullOrWhiteSpace(this.vista.NumeroSerie))
                flota.Unidad.NumeroSerie = this.vista.NumeroSerie;
            if (this.vista.MarcaID.HasValue)
                flota.Unidad.Modelo.Marca.Id = this.vista.MarcaID;
            if (!string.IsNullOrEmpty(flota.Unidad.Modelo.Marca.Nombre) && !string.IsNullOrWhiteSpace(flota.Unidad.Modelo.Marca.Nombre))
                flota.Unidad.Modelo.Marca.Nombre = this.vista.MarcaNombre.Trim().ToUpper();
            if (this.vista.ModeloID.HasValue)
                flota.Unidad.Modelo.Id = this.vista.ModeloID;
            if (!string.IsNullOrEmpty(this.vista.ModeloNombre) && !string.IsNullOrWhiteSpace(this.vista.ModeloNombre))
                flota.Unidad.Modelo.Nombre = this.vista.ModeloNombre.Trim().ToUpper();
            if (this.vista.Anio.HasValue)
                flota.Unidad.Anio = this.vista.Anio.Value;
            if (!string.IsNullOrEmpty(this.vista.Placas) && !string.IsNullOrWhiteSpace(this.vista.Placas))
                flota.Placa = this.vista.Placas;
            if (this.vista.ModeloEAID.HasValue)
                flota.ModeloEquipoAliado.Id = this.vista.ModeloEAID.Value;
            if (!string.IsNullOrEmpty(this.vista.ModeloEANombre) && !string.IsNullOrWhiteSpace(this.vista.ModeloEANombre))
                flota.ModeloEquipoAliado.Nombre = this.vista.ModeloEANombre.Trim().ToUpper();
            return flota;        
        }
        /// <summary>
        /// SC0019
        /// Despleiga en la vista un objeto recuperado
        /// </summary>
        /// <param name="obj">Objeto que se desea desplegar</param>
        private void DatosAIntefazUsuario(object obj)
        {
            FlotaBOF flota = (FlotaBOF) obj;

            if (!ReferenceEquals(flota, null))
            {
                #region Sucursal
                if (!ReferenceEquals(flota.Sucursales, null))
                {
                    
                    if (flota.Sucursales.Count > 0)
                    {
                        SucursalBO sucTemp = flota.Sucursales[0];
                        if (!ReferenceEquals(sucTemp, null))
                        {
                            this.vista.SucursalID = sucTemp.Id.HasValue ? sucTemp.Id : null;
                            this.vista.SucursalNombre = !string.IsNullOrEmpty(sucTemp.Nombre) && !string.IsNullOrWhiteSpace(sucTemp.Nombre)
                                                            ? sucTemp.Nombre.Trim().ToUpper()
                                                            : string.Empty;

                        }
                    }
                }
                #endregion

                #region Unidad
                if (!ReferenceEquals(flota.Unidad, null))
                {
                    if (ReferenceEquals(flota.Unidad.Modelo, null))
                        flota.Unidad.Modelo = new ModeloBO {Marca = new MarcaBO()};

                    this.vista.NumeroEconomico = !string.IsNullOrEmpty(flota.Unidad.NumeroEconomico) && !string.IsNullOrWhiteSpace(flota.Unidad.NumeroEconomico)
                                                     ? flota.Unidad.NumeroEconomico.Trim().ToUpper()
                                                     : string.Empty;
                    this.vista.NumeroSerie = !string.IsNullOrEmpty(flota.Unidad.NumeroSerie) && !string.IsNullOrWhiteSpace(flota.Unidad.NumeroSerie)
                                                 ? flota.Unidad.NumeroSerie
                                                 : string.Empty;                    
                    this.vista.MarcaID = flota.Unidad.Modelo.Marca.Id.HasValue ? flota.Unidad.Modelo.Marca.Id : null;
                    this.vista.MarcaNombre = !string.IsNullOrEmpty(flota.Unidad.Modelo.Marca.Nombre) && !string.IsNullOrWhiteSpace(flota.Unidad.Modelo.Marca.Nombre)
                                                 ? flota.Unidad.Modelo.Marca.Nombre.Trim().ToUpper()
                                                 : string.Empty;
                    this.vista.ModeloID = flota.Unidad.Modelo.Id.HasValue ? flota.Unidad.Modelo.Id : null;
                    this.vista.ModeloNombre = !string.IsNullOrEmpty(flota.Unidad.Modelo.Nombre) && !string.IsNullOrWhiteSpace(flota.Unidad.Modelo.Nombre)
                                                 ? flota.Unidad.Modelo.Nombre.Trim().ToUpper()
                                                 : string.Empty;
                    this.vista.Anio = flota.Unidad.Anio.HasValue ? flota.Unidad.Anio : null;                        
                }
                #endregion

                #region ModeloEquipoAliado
                if (!ReferenceEquals(flota.ModeloEquipoAliado, null))
                {
                    this.vista.ModeloEAID = flota.ModeloEquipoAliado.Id.HasValue ? flota.ModeloEquipoAliado.Id : null;
                    this.vista.ModeloEANombre = !string.IsNullOrEmpty(flota.ModeloEquipoAliado.Nombre) && !string.IsNullOrWhiteSpace(flota.ModeloEquipoAliado.Nombre)
                                                    ? flota.ModeloEquipoAliado.Nombre.Trim().ToUpper()
                                                    : string.Empty;
                }
                #endregion

                this.vista.Placas = !string.IsNullOrEmpty(flota.Placa) && !string.IsNullOrWhiteSpace(flota.Placa)
                                        ? flota.Placa.Trim()
                                        : string.Empty;
            }
            else
            {
                this.LimpiarCampos();
            }
        }
        /// <summary>
        /// SC0019
        /// Obtiene de la vista el paquete de navegación
        /// </summary>
        /// <returns>Objeto con el paquete de navegación</returns>
        private object ObtenerPaquetenavegacion()
        {
            return this.vista.ObtenerPaqueteNavegacion();
        }
        /// <summary>
        /// SC0019
        /// Limpia las propiedades de la vista
        /// </summary>
        private void LimpiarCampos()
        {
            this.vista.LimpiarSesion();
            this.vista.LimpiarFlotaEncontrada();
            this.vista.Anio = null;
            this.vista.MarcaID = null;
            this.vista.MarcaNombre = string.Empty;
            this.vista.ModeloID = null;
            this.vista.ModeloNombre = string.Empty;
            this.vista.NumeroEconomico = string.Empty;
            this.vista.NumeroSerie = string.Empty;
            this.vista.Placas = string.Empty;
            this.vista.SucursalID = null;
            this.vista.SucursalNombre = string.Empty;
        }
        #endregion
        #region Métodos para el Buscador
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
                case "Unidad":
                    UnidadBOF unidad = new UnidadBOF();
                    unidad.Sucursal = new SucursalBO();

                    if (!string.IsNullOrEmpty(this.vista.NumeroSerie))
                        unidad.NumeroSerie = this.vista.NumeroSerie;
                    unidad.Sucursal.UnidadOperativa = this.vista.UnidadOperativa;

                    obj = unidad;
                    break;
                case "Sucursal":
                    SucursalBOF sucursal = new SucursalBOF();
                    sucursal.UnidadOperativa = this.vista.UnidadOperativa;
                    sucursal.Nombre = this.vista.SucursalNombre;
                    sucursal.Usuario = this.vista.Usuario;
                    obj = sucursal;
                    break;
                case "Marca":
                    MarcaBO marca = new MarcaBO();
                    marca.Nombre = this.vista.MarcaNombre;
                    marca.Activo = true;
                    obj = marca;
                    break;
                case "Modelo":
                    ModeloBO modelo = new ModeloBO();
                    modelo.Auditoria = new AuditoriaBO();
                    modelo.Marca = new MarcaBO();
                    modelo.Marca.Id = this.vista.MarcaID;
                    modelo.Nombre = this.vista.ModeloNombre;
                    modelo.Activo = true;
                    obj = modelo;
                    break;
                case "ModeloEquipoAliado"://SC0019
                    ModeloBO modeloEA = new ModeloBO();
                    modeloEA.Auditoria = new AuditoriaBO();
                    modeloEA.Marca = new MarcaBO();
                    modeloEA.Nombre = this.vista.ModeloEANombre;
                    modeloEA.Activo = true;
                    obj = modeloEA;
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
                case "Unidad":
                    #region Desplegar Unidad
					UnidadBOF unidad = (UnidadBOF)selecto ?? new UnidadBOF();
					if (unidad.NumeroSerie != null)
						this.vista.NumeroSerie = unidad.NumeroSerie;
					else
						this.vista.NumeroSerie = string.Empty;
                    #endregion                    
                    break;
                case "Sucursal":
                    #region Desplegar Sucursal
                    SucursalBO sucursal = (SucursalBO)selecto;
                    if (sucursal != null && sucursal.Id != null)
                        this.vista.SucursalID = sucursal.Id;
                    else
                        this.vista.SucursalID = null;

                    if (sucursal != null && sucursal.Nombre != null)
                        this.vista.SucursalNombre = sucursal.Nombre;
                    else
                        this.vista.SucursalNombre = null;
                    #endregion
                    break;
                case "Marca":
                    #region Desplegar Marca
                    MarcaBO marca = (MarcaBO)selecto;

                    if (marca != null && marca.Id.HasValue)
                    {
                        this.vista.MarcaID = marca.Id.Value;
                        this.vista.HabilitarModelo(true);
                    }
                    else
                    {
                        this.vista.MarcaID = null;
                        this.vista.HabilitarModelo(false);
                    }

                    if (marca != null && (!string.IsNullOrEmpty(marca.Nombre) && !string.IsNullOrWhiteSpace(marca.Nombre)))
                        this.vista.MarcaNombre = marca.Nombre;
                    else
                        this.vista.MarcaNombre = string.Empty;

                    this.vista.ModeloID = null;
                    this.vista.ModeloNombre = string.Empty;
                    #endregion
                    break;
                case "Modelo":
                    #region Desplegar Modelo
                    ModeloBO modelo = (ModeloBO)selecto;

                    if (modelo != null && modelo.Id != null)
                        this.vista.ModeloID = modelo.Id;
                    else
                        this.vista.ModeloID = null;

                    if (modelo != null && modelo.Nombre != null)
                        this.vista.ModeloNombre = modelo.Nombre;
                    else
                        this.vista.ModeloNombre = null;
                    #endregion
                    break;
                case "ModeloEquipoAliado"://SC0019
                    ModeloBO modeloEA = (ModeloBO)selecto;

                    if (modeloEA != null && modeloEA.Id != null)
                        this.vista.ModeloEAID = modeloEA.Id;
                    else
                        this.vista.ModeloEAID = null;

                    if (modeloEA != null && modeloEA.Nombre != null)
                        this.vista.ModeloEANombre = modeloEA.Nombre;
                    else
                        this.vista.ModeloEANombre = null;
                    break;
            }
        }
        #endregion
        #endregion
    }
}