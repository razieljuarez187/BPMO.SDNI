//Esta clase satisface los requerimientos especificados en el caso de uso CU082 – REGISTRAR MOVIMIENTO DE FLOTA
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BPMO.Basicos.BO;
using BPMO.Facade.SDNI.BR;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Comun.BR;
using BPMO.SDNI.Contratos.BO;
using BPMO.SDNI.Equipos.BO;
using BPMO.SDNI.Equipos.BR;
using BPMO.SDNI.Flota.BOF;
using BPMO.SDNI.Flota.BR;
using BPMO.SDNI.Flota.VIS;
using BPMO.SDNI.Tramites.BO;
using BPMO.Servicio.Catalogos.BO;
using UnidadBO = BPMO.SDNI.Equipos.BO.UnidadBO;

namespace BPMO.SDNI.Flota.PRE
{
    /// <summary>
    /// Presentador para la reasignación de departamento de la unidad
    /// </summary>
    public class CambiarDepartamentoUnidadPRE
    {
        #region Atributos
        /// <summary>
        /// Vista para la reasignación del departamento
        /// </summary>
        private readonly ICambiarDepartamentoUnidadVIS vista;
        /// <summary>
        /// Provee la conexión a la BD
        /// </summary>
        private readonly IDataContext dctx;
        /// <summary>
        /// Nombre de la clase que se usará en los mensajes de error
        /// </summary>
        private const string nombreClase = "CambiarDepartamentoUnidadPRE";
        /// <summary>
        /// Controlador que ejecutará las accciones
        /// </summary>
        private readonly MantenimientoFlotaBR controlador;
        #endregion

        #region Constructores
        /// <summary>
        /// Constructor del presentador
        /// </summary>
        /// <param name="view">Vista de la página de cambio de departamento</param>
        public CambiarDepartamentoUnidadPRE(ICambiarDepartamentoUnidadVIS view)
        {
            try
            {
                if (ReferenceEquals(view, null))
                    throw new Exception("La vista asociada no puede ser nula");

                this.vista = view;
                this.controlador = new MantenimientoFlotaBR();
                this.dctx = FacadeBR.ObtenerConexion();
            }
            catch (Exception ex)
            {
                this.vista.MostrarMensaje("Inconsistencia al crear el presentador", ETipoMensajeIU.ERROR, string.Format("{0}.CambiarDepartamentoUnidadPRE:{1}{2}", nombreClase, Environment.NewLine, ex.Message));
            }
        }
        #endregion        

        #region Métodos
        /// <summary>
        /// Cancela el cambio de departamento de la unidad
        /// </summary>
        public void Cancelar()
        {
            this.vista.LimpiarSesion();
            this.vista.LimpiarPaqueteNavegacion("UnidadExpedienteBO");
            var unidadID = this.vista.UnidadID;
            if (unidadID.HasValue)
                this.vista.EstablecerPaqueteNavegacion("UnidadExpedienteBO", new UnidadBO { UnidadID = unidadID });
            this.vista.RedirigirADetalles();
        }
        /// <summary>
        /// Prepara la vista para la reasignacion de la unidad
        /// </summary>
        public void PrepararEdicion()
        {
            try
            {
                //Obtenemos el paquete correspondiente al contrato a editar
                var unidad = this.vista.ObtenerPaqueteNavegacion("UnidadExpedienteBO");

                //Validaciones iniciales
                if (ReferenceEquals(unidad, null))
                    throw new Exception("Se esperaba un objeto en la navegación. No fue posible identificar la unidad que se desea mover.");
                if (!(unidad is UnidadBO))
                    throw new Exception("El paquete recuperado no corresponde a una unidad, verifique su información.");

                var unidadAnterior = new UnidadBO((UnidadBO)unidad);
                var unidadBase = new ElementoFlotaBOF{Unidad = new UnidadBO()};

                //Eliminamos el paquete de navegación
                this.vista.LimpiarPaqueteNavegacion("UnidadExpedienteBO");

                //Consultamos la unidad para el último objeto
                unidadAnterior = this.ObtenerUnidad(unidadAnterior);
                this.vista.UltimoObjeto = unidadAnterior;

                //Consultamos la unidad para el elemento a editar
                unidadBase.Unidad.UnidadID = unidadAnterior.UnidadID;
                unidadBase = this.ObtenerElemento(unidadBase.Unidad);

                if (unidadBase == null) unidadBase = new ElementoFlotaBOF();
                if (unidadBase != null && unidadBase.Unidad == null) unidadBase.Unidad = new UnidadBO();

                //Cargamos el objeto a edición
                this.vista.ObjetoEdicion = unidadBase.Unidad;

                if (!unidadBase.Unidad.EstatusActual.HasValue)
                    throw new Exception("El estatus de la unidad no fue recuperado, para continuar con el cambio de departamento es necesario especificar el estatus actual de la unidad.");

                //Validar estatus de acuerdo a regla
                if (!unidadAnterior.ValidarCambioEstatus(unidadBase.Unidad.EstatusActual))
                {
                    string mnj = "No se puede cambiar el estatus de la unidad de " +
                                 (unidadAnterior.EstatusActual != null ? unidadAnterior.EstatusActual.ToString() : "") +
                                 " a " +
                                 (unidadBase.Unidad.EstatusActual != null ? unidadBase.Unidad.EstatusActual.ToString() : "");
                    this.vista.PermitirRegistrar(false);
                    this.vista.MostrarMensaje(mnj, ETipoMensajeIU.ADVERTENCIA, null);
                }

                if ((unidadBase.Unidad.EstatusActual.Value != EEstatusUnidad.Disponible) && (unidadBase.Unidad.EstatusActual.Value != EEstatusUnidad.Seminuevo))
                {
                    this.vista.PermitirRegistrar(false);
                    this.vista.MostrarMensaje("No se puede cambiar la unidad del departamento actual, porque esta se encuentra en uso, verifique su información", ETipoMensajeIU.ADVERTENCIA, null);
                }

                //Cargamos los departamentos disponibles
                this.vista.CargarDepartamentos(this.ObtenerDepartamentos());

                //Obtener las configuraciones de la unidad operativa
                List<ConfiguracionUnidadOperativaBO> lstConfigUO = new ModuloBR().ConsultarConfiguracionUnidadOperativa(this.dctx, new ConfiguracionUnidadOperativaBO() { UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaID } }, this.vista.ModuloID);
                if (lstConfigUO.Count <= 0)
                    throw new Exception("No se encontraron las configuraciones del sistema para la unidad operativa en la que trabaja.");
               
                //Establecer las configuraciones de la unidad operativa
                this.vista.NombreClienteUnidadOperativa = lstConfigUO[0].NombreCliente;
                
                if (string.IsNullOrEmpty(this.vista.NombreClienteUnidadOperativa) || string.IsNullOrWhiteSpace(this.vista.NombreClienteUnidadOperativa))
                    throw new Exception("No se encuentra configurado el nombre del cliente de la unidad operativa.");

                //Desplegamos la unidad obtenida
                this.DatoAInterfazUsuario(unidadBase);

                this.vista.PrepararEdicion();
                
                this.EstablecerSeguridad();

            }
            catch (Exception ex)
            {
                this.vista.PermitirRegistrar(false);
                throw new Exception(string.Format("{0}.PrepararEdicion:{1}{2}", nombreClase, Environment.NewLine, ex.Message));
            }
        }
        /// <summary>
        /// Obtiene la unidad que deseas editar
        /// </summary>
        /// <param name="unidadAnterior">Unidad que se desea editar</param>
        /// <returns>Unidad Completa para su edicion</returns>
        private UnidadBO ObtenerUnidad(UnidadBO unidadAnterior)
        {
            var obj = unidadAnterior;
            UnidadBR unidadBR = new UnidadBR();

            //Consultamos el objeto
            var unidades = unidadBR.ConsultarCompleto(dctx, obj, true);

            //Limpiamso la instancia del objeto
            obj = null;

            //Validamos que realmente hay una unidad para su edición
            if (!ReferenceEquals(unidades, null))
                if (unidades.Count > 0)
                    obj = unidades[0];

            //Retornamos el objeto
            return obj;
        }
        /// <summary>
        /// Consulta el elemento en la flota correspondiente a la unidad
        /// </summary>
        /// <param name="unidad">Unidad de la que se desea obtener la inforamción</param>
        /// <returns>Elemento de la flota que corresponde a la unidad</returns>
        private ElementoFlotaBOF ObtenerElemento(UnidadBO unidad)
        {
            //Se consulta la información del contrato
            FlotaBOF bo = new FlotaBOF { Unidad = unidad };

            SeguimientoFlotaBR seguimientoBR = new SeguimientoFlotaBR();
            List<ElementoFlotaBOF> lst = seguimientoBR.ConsultarSeguimientoFlotaCompleto(this.dctx, bo);

            if (lst.Count < 1)
                throw new Exception("No se encontró ningún registro que corresponda a la información proporcionada.");
            if (lst.Count > 1)
                throw new Exception("La consulta devolvió más de un registro.");

            return lst[0];
        }
        /// <summary>
        /// Despliega en la vista la inforamciónd e la unidad que se haya recuperado
        /// </summary>
        /// <param name="unidadBase">Unidad que se desea visualizar</param>
        private void DatoAInterfazUsuario(ElementoFlotaBOF unidadBase)
        {
            var elemento = unidadBase;

            #region Datos Elemento
            #region Nuevo elemento
            if (elemento == null)
            {
                elemento.Unidad = new UnidadBO
                {
                    Cliente = new ClienteBO(),
                    Sucursal = new SucursalBO { UnidadOperativa = new UnidadOperativaBO() },
                    Modelo = new ModeloBO { Marca = new MarcaBO() },
                    ActivoFijo = new ActivoFijoBO(),
                    CaracteristicasUnidad = new CaracteristicasUnidadBO(),
                    EquiposAliados = new List<EquipoAliadoBO>(),
                    TipoEquipoServicio = new TipoUnidadBO(),
                };
                elemento.Tramites = new List<TramiteBO>();
                elemento.Contrato = new ContratoProxyBO { Cliente = new CuentaClienteIdealeaseBO() };
            }
            #endregion

            #region Instanciar Propiedades
            if (elemento.Unidad == null)
            {
                elemento.Unidad = new UnidadBO
                {
                    Cliente = new ClienteBO(),
                    Sucursal = new SucursalBO { UnidadOperativa = new UnidadOperativaBO() },
                    Modelo = new ModeloBO { Marca = new MarcaBO() },
                    ActivoFijo = new ActivoFijoBO(),
                    CaracteristicasUnidad = new CaracteristicasUnidadBO(),
                    EquiposAliados = new List<EquipoAliadoBO>(),
                    TipoEquipoServicio = new TipoUnidadBO(),
                };
            }
            if (elemento.Unidad.Cliente == null)
                elemento.Unidad.Cliente = new ClienteBO();
            if (elemento.Unidad.Sucursal == null)
                elemento.Unidad.Sucursal = new SucursalBO { UnidadOperativa = new UnidadOperativaBO() };
            if (elemento.Unidad.Sucursal.UnidadOperativa == null)
                elemento.Unidad.Sucursal.UnidadOperativa = new UnidadOperativaBO();
            if (elemento.Unidad.Modelo == null)
                elemento.Unidad.Modelo = new ModeloBO { Marca = new MarcaBO() };
            if (elemento.Unidad.Modelo.Marca == null)
                elemento.Unidad.Modelo.Marca = new MarcaBO();
            if (elemento.Unidad.ActivoFijo == null)
                elemento.Unidad.ActivoFijo = new ActivoFijoBO();
            if (elemento.Unidad.CaracteristicasUnidad == null)
                elemento.Unidad.CaracteristicasUnidad = new CaracteristicasUnidadBO();
            if (elemento.Unidad.EquiposAliados == null)
                elemento.Unidad.EquiposAliados = new List<EquipoAliadoBO>();
            if (elemento.Unidad.TipoEquipoServicio == null)
                elemento.Unidad.TipoEquipoServicio = new TipoUnidadBO();
            if (elemento.Tramites == null)
                elemento.Tramites = new List<TramiteBO>();
            if (elemento.Contrato == null)
                elemento.Contrato = new ContratoProxyBO { Cliente = new CuentaClienteIdealeaseBO() };
            if (elemento.Contrato.Cliente == null)
                elemento.Contrato.Cliente = new CuentaClienteIdealeaseBO();
            #endregion

            #region Información de la Unidad en barra
            this.vista.EstaDisponible = elemento.EstaDisponible;
            this.vista.EstaEnContrato = elemento.EstaEnRenta;
            this.vista.TieneEquipoAliado = elemento.TieneEquipoAliado;

            #region Tramites
            PlacaEstatalBO tPlacaEstatal = (PlacaEstatalBO)elemento.Tramites.Find(p => p.Tipo != null && p.Tipo == ETipoTramite.PLACA_ESTATAL);
            PlacaFederalBO tPlacaFederal = (PlacaFederalBO)elemento.Tramites.Find(p => p.Tipo != null && p.Tipo == ETipoTramite.PLACA_FEDERAL);

            this.vista.NumeroPlaca = tPlacaFederal != null
                                         ? tPlacaFederal.Numero
                                         : (tPlacaEstatal != null ? tPlacaEstatal.Numero : null);
            #endregion
            #endregion
            #endregion

            #region Datos Unidad
            var bo = unidadBase.Unidad;

            #region Unidad Nueva
            if (ReferenceEquals(bo, null))
            {
                bo = new UnidadBO
                {
                    ActivoFijo = new ActivoFijoBO(),
                    EquiposAliados = new List<EquipoAliadoBO>(),
                    Modelo = new ModeloBO { Marca = new MarcaBO() },
                    Sucursal = new SucursalBO { UnidadOperativa = new UnidadOperativaBO() },
                    TipoEquipoServicio = new TipoUnidadBO()
                };
            }
            #endregion

            #region Instancias de propiedades
            if (bo.TipoEquipoServicio == null) bo.TipoEquipoServicio = new TipoUnidadBO();
            if (bo.ActivoFijo == null) bo.ActivoFijo = new ActivoFijoBO();
            if (bo.EquiposAliados == null) bo.EquiposAliados = new List<EquipoAliadoBO>();
            if (bo.Modelo == null) bo.Modelo = new ModeloBO { Marca = new MarcaBO() };
            if (bo.Sucursal == null) bo.Sucursal = new SucursalBO { UnidadOperativa = new UnidadOperativaBO() };
            #endregion

            #region DetalleUnidad
            this.vista.UnidadID = bo.UnidadID;
            this.vista.NumeroSerie = bo.NumeroSerie;
            this.vista.ClaveActivoOracle = bo.ClaveActivoOracle;
            this.vista.LiderID = bo.IDLider;
            this.vista.NumeroEconomico = bo.NumeroEconomico;
            this.vista.TipoUnidadNombre = bo.TipoEquipoServicio.Nombre;
            this.vista.ModeloNombre = bo.Modelo.Nombre;
            this.vista.Anio = bo.Anio;
            this.vista.FechaCompra = bo.ActivoFijo.FechaFacturaCompra;
            this.vista.MontoFactura = bo.ActivoFijo.CostoSinIva;
            this.vista.FolioFactura = string.Empty;
            #endregion

            #region Info Sucursal
            bool completa = false;
            this.vista.SucursalActualID = bo.Sucursal.Id;
            this.vista.SucursalActualNombre = bo.Sucursal.Nombre;

            #region Empresa
            var empresa = new EmpresaBO();
            if (bo.Sucursal.UnidadOperativa != null)
            {
                this.vista.EmpresaActualID = bo.Sucursal.UnidadOperativa.Id;


                if (bo.Sucursal.UnidadOperativa.Empresa == null)
                {
                    #region Unidad Operativa
                    //Obtener información de la Unidad Operativa
                    List<UnidadOperativaBO> lstUO = FacadeBR.ConsultarUnidadOperativaCompleto(this.dctx,
                                                                                              new UnidadOperativaBO()
                                                                                              {
                                                                                                  Id =
                                                                                                      this.vista
                                                                                                          .UnidadOperativaID
                                                                                              });
                    if (lstUO.Count <= 0)
                        throw new Exception(
                            "No se encontró la información completa de la unidad operativa sobre la que trabaja.");
                    //Establecer la información de la Unidad Operativa
                    if (lstUO[0].Empresa != null)
                        empresa = lstUO[0].Empresa;
                    #endregion

                    this.vista.EmpresaActualNombre = empresa.Nombre;
                }
                else
                {
                    if (!string.IsNullOrEmpty(bo.Sucursal.UnidadOperativa.Empresa.Nombre) && !string.IsNullOrWhiteSpace(bo.Sucursal.UnidadOperativa.Empresa.Nombre))
                        this.vista.EmpresaActualNombre = bo.Sucursal.UnidadOperativa.Empresa.Nombre;
                    else
                    {
                        #region Unidad Operativa
                        //Obtener información de la Unidad Operativa
                        List<UnidadOperativaBO> lstUO = FacadeBR.ConsultarUnidadOperativaCompleto(this.dctx,
                                                                                                  new UnidadOperativaBO()
                                                                                                  {
                                                                                                      Id =
                                                                                                          this.vista
                                                                                                              .UnidadOperativaID
                                                                                                  });
                        if (lstUO.Count <= 0)
                            throw new Exception(
                                "No se encontró la información completa de la unidad operativa sobre la que trabaja.");
                        //Establecer la información de la Unidad Operativa
                        if (lstUO[0].Empresa != null)
                            empresa = lstUO[0].Empresa;

                        #endregion
                        this.vista.EmpresaActualNombre = empresa.Nombre;
                    }
                }
            }

            #endregion

            #region Domicilio Sucursal
            if (bo.Sucursal.DireccionesSucursal != null)
            {
                if (bo.Sucursal.DireccionesSucursal.Count > 0)
                {
                    var direccionActual = bo.Sucursal.DireccionesSucursal.Find(p => p.Primaria != null && p.Primaria == true);
                    if (direccionActual != null)
                    {
                        string dir = "";
                        if (!string.IsNullOrEmpty(direccionActual.Calle))
                            dir += (direccionActual.Calle + " ");
                        if (!string.IsNullOrEmpty(direccionActual.Colonia))
                            dir += (direccionActual.Colonia + " ");
                        if (!string.IsNullOrEmpty(direccionActual.CodigoPostal))
                            dir += (direccionActual.CodigoPostal + " ");
                        if (direccionActual.Ubicacion != null)
                        {
                            if (direccionActual.Ubicacion.Municipio != null && !string.IsNullOrEmpty(direccionActual.Ubicacion.Municipio.Nombre))
                                dir += (direccionActual.Ubicacion.Municipio.Nombre + " ");
                            if (direccionActual.Ubicacion.Ciudad != null && !string.IsNullOrEmpty(direccionActual.Ubicacion.Ciudad.Nombre))
                                dir += (direccionActual.Ubicacion.Ciudad.Nombre + " ");
                            if (direccionActual.Ubicacion.Estado != null && !string.IsNullOrEmpty(direccionActual.Ubicacion.Estado.Nombre))
                                dir += (direccionActual.Ubicacion.Estado.Nombre + " ");
                            if (direccionActual.Ubicacion.Pais != null && !string.IsNullOrEmpty(direccionActual.Ubicacion.Pais.Nombre))
                                dir += (direccionActual.Ubicacion.Pais.Nombre + " ");
                        }

                        if (dir != null && dir.Trim().CompareTo("") != 0)
                        {
                            this.vista.DomicilioSucursalActual = dir;
                            completa = true;
                        }
                        else
                            this.vista.DomicilioSucursalActual = null;
                    }
                    else
                        this.vista.DomicilioSucursalActual = null;
                }
                else
                    this.vista.DomicilioSucursalActual = null;
            }
            #endregion

            #region Consultar Completo para obtener la Dirección
            if (bo.Sucursal != null && bo.Sucursal.Id != null && !completa)
            {
                List<SucursalBO> lst = FacadeBR.ConsultarSucursalCompleto(this.dctx, bo.Sucursal);

                DireccionSucursalBO direccion = null;
                if (lst.Count > 0 && lst[0].DireccionesSucursal != null)
                    direccion = lst[0].DireccionesSucursal.Find(p => p.Primaria != null && p.Primaria == true);

                if (direccion != null)
                {
                    string dir = "";
                    if (!string.IsNullOrEmpty(direccion.Calle))
                        dir += (direccion.Calle + " ");
                    if (!string.IsNullOrEmpty(direccion.Colonia))
                        dir += (direccion.Colonia + " ");
                    if (!string.IsNullOrEmpty(direccion.CodigoPostal))
                        dir += (direccion.CodigoPostal + " ");
                    if (direccion.Ubicacion != null)
                    {
                        if (direccion.Ubicacion.Municipio != null && !string.IsNullOrEmpty(direccion.Ubicacion.Municipio.Nombre))
                            dir += (direccion.Ubicacion.Municipio.Nombre + " ");
                        if (direccion.Ubicacion.Ciudad != null && !string.IsNullOrEmpty(direccion.Ubicacion.Ciudad.Nombre))
                            dir += (direccion.Ubicacion.Ciudad.Nombre + " ");
                        if (direccion.Ubicacion.Estado != null && !string.IsNullOrEmpty(direccion.Ubicacion.Estado.Nombre))
                            dir += (direccion.Ubicacion.Estado.Nombre + " ");
                        if (direccion.Ubicacion.Pais != null && !string.IsNullOrEmpty(direccion.Ubicacion.Pais.Nombre))
                            dir += (direccion.Ubicacion.Pais.Nombre + " ");
                    }

                    if (dir != null && dir.Trim().CompareTo("") != 0)
                    {
                        this.vista.DomicilioSucursalActual = dir;
                        completa = true;
                    }
                    else
                        this.vista.DomicilioSucursalActual = null;
                }
                else
                    this.vista.DomicilioSucursalActual = null;
            }
            else if (!completa)
                this.vista.DomicilioSucursalActual = null;
            #endregion

            #endregion

            #region Info departamento actual
            this.vista.DepartamentoActualID = bo.Area!=null ? (int?) ((int) (EArea)bo.Area) : null;
            this.vista.HabilitarCliente(true);
            this.vista.HabilitarPropietario(false);
            this.vista.ClienteID = bo.Cliente.Id.HasValue ? bo.Cliente.Id : null;
            this.vista.Cliente = string.Empty; 
            this.vista.Propietario = !string.IsNullOrEmpty(bo.Propietario) && !string.IsNullOrWhiteSpace(bo.Propietario)
                                         ? bo.Propietario
                                         : string.Empty;
            #endregion

            #endregion
        }
        /// <summary>
        /// Valida que la selecciòn de los clientes sea la adecuada
        /// </summary>
        /// <returns>Cadena con las inconsistencias encontradas</returns>
        public string ValidarCamposConsultaCliente()
        {
            string s = "";

            if (!(this.vista.Cliente != null && this.vista.Cliente.Trim().CompareTo("") != 0))
                s += "Cliente, ";

            if (s.Trim().CompareTo("") != 0)
                return "Los siguientes campos no pueden estar vacíos: \n" + s.Substring(0, s.Length - 2);

            return null;
        }
        /// <summary>
        /// Valida que la selecciòn de los ppropietarios sea la adecuada
        /// </summary>
        /// <returns>Cadena con las inconsistencias encontradas</returns>
        public string ValidarCamposConsultaPropietario()
        {
            string s = "";

            if (!(this.vista.Propietario != null && this.vista.Propietario.Trim().CompareTo("") != 0))
                s += "Propietario, ";

            if (s.Trim().CompareTo("") != 0)
                return "Los siguientes campos no pueden estar vacíos: \n" + s.Substring(0, s.Length - 2);

            return null;
        }
        /// <summary>
        /// Obtiene los departamentos configurados en el sistema
        /// </summary>
        /// <returns></returns>
        private Dictionary<int, string> ObtenerDepartamentos()
        {
            try
            {
                this.vista.EnumTipoEmpresa = (ETipoEmpresa)this.vista.UnidadOperativaID;
                Dictionary<int, string> tipos = new Dictionary<int, string>();
                switch (this.vista.EnumTipoEmpresa)
                {
                    case ETipoEmpresa.Idealease:
                        string keyI = "";
                        int valueI = 0;
                        tipos.Add(-1, "SELECCIONAR");
                        foreach (var tipo in Enum.GetValues(typeof(EArea)))
                        {
                            var query =
                                tipo.GetType()
                                    .GetField(tipo.ToString())
                                    .GetCustomAttributes(true)
                                    .Where(a => a.GetType().Equals(typeof(System.ComponentModel.DescriptionAttribute)));
                            valueI = Convert.ToInt32(tipo);
                            if (query.Any())
                            {
                                keyI =
                                    (tipo.GetType()
                                         .GetField(tipo.ToString())
                                         .GetCustomAttributes(true)
                                         .Where(a => a.GetType().Equals(typeof(System.ComponentModel.DescriptionAttribute)))
                                         .FirstOrDefault() as System.ComponentModel.DescriptionAttribute).Description;
                            }
                            else
                            {
                                keyI = Enum.GetName(typeof(EArea), valueI);
                            }
                            tipos.Add(valueI, keyI);
                        }
                        break;
                    case ETipoEmpresa.Construccion:
                        string keyC = "";
                        int valueC = 0;
                        tipos.Add(-1, "SELECCIONAR");
                        foreach (var tipo in Enum.GetValues(typeof(EAreaConstruccion)))
                        {
                            var query =
                                tipo.GetType()
                                    .GetField(tipo.ToString())
                                    .GetCustomAttributes(true)
                                    .Where(a => a.GetType().Equals(typeof(System.ComponentModel.DescriptionAttribute)));
                            valueC = Convert.ToInt32(tipo);
                            if (query.Any())
                            {
                                keyC =
                                    (tipo.GetType()
                                         .GetField(tipo.ToString())
                                         .GetCustomAttributes(true)
                                         .Where(a => a.GetType().Equals(typeof(System.ComponentModel.DescriptionAttribute)))
                                         .FirstOrDefault() as System.ComponentModel.DescriptionAttribute).Description;
                            }
                            else
                            {
                                keyC = Enum.GetName(typeof(EAreaConstruccion), valueC);
                            }
                            tipos.Add(valueC, keyC);
                        }
                        break;
                    case ETipoEmpresa.Generacion:
                        string keyG = "";
                        int valueG = 0;
                        tipos.Add(-1, "SELECCIONAR");
                        foreach (var tipo in Enum.GetValues(typeof(EAreaGeneracion)))
                        {
                            var query =
                                tipo.GetType()
                                    .GetField(tipo.ToString())
                                    .GetCustomAttributes(true)
                                    .Where(a => a.GetType().Equals(typeof(System.ComponentModel.DescriptionAttribute)));
                            valueG = Convert.ToInt32(tipo);
                            if (query.Any())
                            {
                                keyG =
                                    (tipo.GetType()
                                         .GetField(tipo.ToString())
                                         .GetCustomAttributes(true)
                                         .Where(a => a.GetType().Equals(typeof(System.ComponentModel.DescriptionAttribute)))
                                         .FirstOrDefault() as System.ComponentModel.DescriptionAttribute).Description;
                            }
                            else
                            {
                                keyG = Enum.GetName(typeof(EAreaGeneracion), valueG);
                            }
                            tipos.Add(valueG, keyG);
                        }
                        break;
                    case ETipoEmpresa.Equinova:
                        string keyEqu = "";
                        int valueEqu = 0;
                        tipos.Add(-1, "SELECCIONAR");
                        foreach (var tipo in Enum.GetValues(typeof(EAreaEquinova))) {
                            var query =
                                tipo.GetType()
                                    .GetField(tipo.ToString())
                                    .GetCustomAttributes(true)
                                    .Where(a => a.GetType().Equals(typeof(System.ComponentModel.DescriptionAttribute)));
                            valueEqu = Convert.ToInt32(tipo);
                            if (query.Any()) {
                                keyEqu =
                                    (tipo.GetType()
                                         .GetField(tipo.ToString())
                                         .GetCustomAttributes(true)
                                         .Where(a => a.GetType().Equals(typeof(System.ComponentModel.DescriptionAttribute)))
                                         .FirstOrDefault() as System.ComponentModel.DescriptionAttribute).Description;
                            } else {
                                keyEqu = Enum.GetName(typeof(EAreaEquinova), valueEqu);
                            }
                            tipos.Add(valueEqu, keyEqu);
                        }
                        break;
                }
                return tipos;
            }
            catch (Exception ex)
            {
                throw new Exception(nombreClase + ".ObtenerTiposListado: Ocurrío una inconsistencia al intentar generar el listado de tipos de Check List." + Environment.NewLine + ex.Message);
            }
        }
        /// <summary>
        /// Habilita las opciònes de selecciòn del operador al cambiar la selecciòn del àrea
        /// </summary>
        /// <param name="valor">Departamento seleccionado</param>
        public void SeleccionarArea(int? valor)
        {
            EArea? area = null;
            if (valor != null) area = (EArea)Enum.Parse(typeof(EArea), valor.ToString());

            switch (area)
            {
                case null:
                    this.vista.HabilitarCliente(false);
                    this.vista.HabilitarPropietario(false);

                    this.vista.Propietario = "";
                    this.vista.Cliente = "";
                    this.vista.PropietarioID = null;
                    this.vista.ClienteID = null;
                    break;
                case EArea.CM:
                case EArea.SD:
                    this.vista.HabilitarPropietario(true);
                    this.vista.HabilitarCliente(false);

                    this.vista.Propietario = "";
                    this.vista.Cliente = "";
                    this.vista.PropietarioID = null;
                    this.vista.ClienteID = null;
                    break;
                case EArea.RD:
                    this.vista.HabilitarPropietario(false);
                    this.vista.HabilitarCliente(false);

                    int? id = this.ObtenerClienteId(this.vista.NombreClienteUnidadOperativa);
                    string nombre = (id != null) ? this.vista.NombreClienteUnidadOperativa : "";

                    this.vista.Propietario = nombre;
                    this.vista.Cliente = nombre;
                    this.vista.PropietarioID = id;
                    this.vista.ClienteID = id;
                    break;
                case EArea.FSL:
                    this.vista.HabilitarPropietario(false);
                    this.vista.HabilitarCliente(true);

                    this.vista.Propietario = this.vista.NombreClienteUnidadOperativa;
                    this.vista.Cliente = "";
                    this.vista.ClienteID = null;
                    break;
                case EArea.Seminuevos:
                    this.vista.HabilitarPropietario(false);
                    this.vista.HabilitarCliente(false);

                    var lastBO = (UnidadBO)this.vista.UltimoObjeto;
                    this.vista.ClienteID = lastBO != null && lastBO.Cliente.Id.HasValue ? lastBO.Cliente.Id : null;
                    this.vista.Cliente = lastBO != null &&
                                         (!string.IsNullOrEmpty(lastBO.Cliente.Nombre) &&
                                          !string.IsNullOrWhiteSpace(lastBO.Cliente.Nombre))
                                             ? lastBO.Cliente.NombreCompleto
                                             : string.Empty;
                    this.vista.PropietarioID = lastBO != null && lastBO.Cliente.Id.HasValue ? lastBO.Cliente.Id : null;
                    this.vista.Propietario = lastBO != null ? lastBO.Propietario : string.Empty;
                    break;
            }
        }
        /// <summary>
        /// Obtiene el identificador del clietne a partir de su nomnre
        /// </summary>
        /// <param name="nombre">Nombre del cliente seleccionado</param>
        /// <returns>Identificador del cliente seleccionado</returns>
        private int? ObtenerClienteId(string nombre)
        {
            int? id = null;

            List<ClienteBO> lst = FacadeBR.ConsultarCliente(this.dctx, new ClienteBO() { Nombre = nombre });
            if (lst.Count > 0)
                id = lst[0].Id;

            return id;
        }
        /// <summary>
        /// Devuelve un objeto de negocio a partir de la información de la vista
        /// </summary>
        /// <returns>BO con la información a editar</returns>
        private UnidadBO InterfazUsuarioADato()
        {
            var bo = new UnidadBO((UnidadBO)this.vista.ObjetoEdicion);

            if (this.vista.DepartamentoDestinoID.HasValue && (EArea)this.vista.DepartamentoDestinoID.Value == EArea.Seminuevos)
                bo.EstatusActual = EEstatusUnidad.Seminuevo;
            else if ((this.vista.DepartamentoActualID.HasValue) && ((EArea)this.vista.DepartamentoActualID.Value == EArea.Seminuevos) && ((EArea)this.vista.DepartamentoDestinoID.Value != EArea.Seminuevos))
                bo.EstatusActual = EEstatusUnidad.Disponible;

            bo.Cliente.Id = this.vista.ClienteID.HasValue ? this.vista.ClienteID.Value : bo.Cliente.Id = null;

            bo.Propietario = !string.IsNullOrEmpty(this.vista.Propietario) &&
                             !string.IsNullOrWhiteSpace(this.vista.Propietario)
                                 ? this.vista.Propietario.Trim().ToUpper()
                                 : null;

            bo.Area = (EArea)this.vista.DepartamentoDestinoID.Value;
            bo.UUA = this.vista.UsuarioID;
            bo.FUA = DateTime.Now;

            return bo;
        }
        /// <summary>
        /// Valida la información proporcionada por el usuario
        /// </summary>
        /// <returns>Inconsistencias en la inforamción proporcionada</returns>
        private string ValidarCamposRegistro()
        {
            string s = string.Empty;

            if (!this.vista.UnidadOperativaID.HasValue)
                s += "Unidad Operativa, ";
            if (!this.vista.SucursalActualID.HasValue)
                s += "Sucursal, ";
            if (!this.vista.UnidadID.HasValue)
                s += "Unidad, ";
            if (!this.vista.ClienteID.HasValue)
                s += "Cliente, ";
            if (string.IsNullOrEmpty(this.vista.Propietario) || string.IsNullOrWhiteSpace(this.vista.Propietario))
                s += "Propietario, ";
            if (!this.vista.DepartamentoDestinoID.HasValue)
                s += "Área/Departamento destino, ";
            if (!this.vista.UsuarioID.HasValue)
                s += "Usuario realiza el cambio, ";

            if (s != null && s.Trim().CompareTo("") != 0)
                return "Los siguientes campos no pueden estar vacíos: \n" + s.Substring(0, s.Length - 2);

            if(this.vista.DepartamentoActualID.HasValue && this.vista.DepartamentoDestinoID.HasValue)
            {
                if (this.vista.DepartamentoDestinoID.Value == this.vista.DepartamentoActualID.Value)
                    return "El Àrea/Departamento al que se desea mover la unidad, es el mismo que actualmente esta configurado.";
                
                switch (this.vista.DepartamentoActualID.Value) {
                    case 11:
                        if(string.IsNullOrEmpty(this.vista.Cliente))
                            s += "Cliente, ";
                        if (this.vista.DepartamentoDestinoID.Value != 10)
                            return "No se puede mover la unidad al área/departamento seleccionado, solo se puede mover a RO";
                        break;
                    case 21:
                        if (string.IsNullOrEmpty(this.vista.Cliente))
                            s += "Cliente, ";
                        if (this.vista.DepartamentoDestinoID.Value != 20)
                            return "No se puede mover la unidad al área/departamento seleccionado, solo se puede mover a RO";
                        break;
                    case 31:
                        if (string.IsNullOrEmpty(this.vista.Cliente))
                            s += "Cliente, ";
                        if (this.vista.DepartamentoDestinoID.Value != 30)
                            return "No se puede mover la unidad al área/departamento seleccionado, solo se puede mover a RO";
                        break;
                    case 10:
                        if (string.IsNullOrEmpty(this.vista.Cliente))
                            s += "Cliente, ";
                        if (this.vista.DepartamentoDestinoID.Value != 11)
                            return "No se puede mover la unidad al área/departamento seleccionado, solo se puede mover a ROC";
                        break;
                    case 20:
                        if (string.IsNullOrEmpty(this.vista.Cliente))
                            s += "Cliente, ";
                        if (this.vista.DepartamentoDestinoID.Value != 21)
                            return "No se puede mover la unidad al área/departamento seleccionado, solo se puede mover a ROC";
                        break;
                    case 30:
                        if (string.IsNullOrEmpty(this.vista.Cliente))
                            s += "Cliente, ";
                        if (this.vista.DepartamentoDestinoID.Value != 31)
                            return "No se puede mover la unidad al área/departamento seleccionado, solo se puede mover a ROC";
                        break;
                }
                
            }

            if ((((EArea)this.vista.DepartamentoDestinoID.Value) == EArea.FSL) && (string.IsNullOrEmpty(this.vista.ClaveActivoOracle) || string.IsNullOrWhiteSpace(this.vista.ClaveActivoOracle)))
                return "La unidad no puede ser movida al Àrea de Full Service Leasing por que no cuenta con una clave de activo fijo.";

            if ((((EArea)this.vista.DepartamentoDestinoID.Value) == EArea.RD) && (string.IsNullOrEmpty(this.vista.ClaveActivoOracle) || string.IsNullOrWhiteSpace(this.vista.ClaveActivoOracle)))
                return "La unidad no puede ser movida al Àrea de Renta Diarìa por que no cuenta con una clave de activo fijo.";

            this.vista.EnumTipoEmpresa = (ETipoEmpresa)this.vista.UnidadOperativaID;
            switch (this.vista.EnumTipoEmpresa)
            {
                case ETipoEmpresa.Construccion:
                    if (this.vista.DepartamentoActualID.HasValue && this.vista.DepartamentoDestinoID.HasValue)
                    {
                        if ((((EAreaConstruccion)this.vista.DepartamentoActualID.Value) == EAreaConstruccion.RE) && ((EAreaConstruccion)this.vista.DepartamentoDestinoID.Value) == EAreaConstruccion.ROC)
                            return "No se puede cambiar de departamento a una unidad RE a ROC.";
                    }
                    break;
                case ETipoEmpresa.Generacion:
                    if ((((EAreaGeneracion)this.vista.DepartamentoActualID.Value) == EAreaGeneracion.RE) && ((EAreaGeneracion)this.vista.DepartamentoDestinoID.Value) == EAreaGeneracion.ROC)
                        return "No se puede cambiar de departamento a una unidad RE a ROC.";
                    break;
                case ETipoEmpresa.Equinova:
                    if ((((EAreaEquinova)this.vista.DepartamentoActualID.Value) == EAreaEquinova.RE) && ((EAreaEquinova)this.vista.DepartamentoDestinoID.Value) == EAreaEquinova.ROC)
                        return "No se puede cambiar de departamento a una unidad RE a ROC.";
                    break;
            }

            return null;
        }
        /// <summary>
        /// Edita la unidad
        /// </summary>
        private void Editar()
        {
            //Se obtiene la información del contrato a partir de la vista
            var bo = (UnidadBO)this.InterfazUsuarioADato();

            //Se crea el objeto de seguridad
            UsuarioBO usuario = new UsuarioBO() { Id = this.vista.UsuarioID };
            AdscripcionBO adscripcion = new AdscripcionBO() { UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaID } };
            
            SeguridadBO seguridadBO = new SeguridadBO(Guid.Empty, usuario, adscripcion);

            //Se actualiza en la base de datos
            var mnj = new StringBuilder(string.Format("Departamento {0} a Departamento {1}. {2}", (EArea)this.vista.DepartamentoActualID.Value, (EArea)this.vista.DepartamentoDestinoID.Value, Environment.NewLine));
            mnj.Append(this.vista.Observaciones);

            this.controlador.ActualizarUnidadDepartamento(dctx, bo, (UnidadBO)this.vista.UltimoObjeto, mnj.ToString(), seguridadBO);
        }
        /// <summary>
        /// Actualiza el departamento de la unidad
        /// </summary>
        public void ActualizarUnidadDepartamento()
        {
            string s;
            if ((s = this.ValidarCamposRegistro()) != null)
            {
                this.vista.MostrarMensaje(s, ETipoMensajeIU.ADVERTENCIA, null);
                return;
            }

            //Editamos la unidad
            this.Editar();

            //Redirigimos al detalle de la unidad
            this.vista.LimpiarSesion();
            this.vista.LimpiarPaqueteNavegacion("UnidadExpedienteBO");
            var unidadID = this.vista.UnidadID;
            if (unidadID.HasValue)
                this.vista.EstablecerPaqueteNavegacion("UnidadExpedienteBO", new UnidadBO { UnidadID = unidadID });
            this.vista.RedirigirADetalles();
        }

        #region Métodos para el Buscador
        public object PrepararBOBuscador(string catalogo)
        {
            object obj = null;

            switch (catalogo)
            {
                case "Propietario":
                    ClienteBO cPropietario = new ClienteBO();

                    cPropietario.Nombre = this.vista.Propietario;
                    cPropietario.Activo = true;

                    obj = cPropietario;
                    break;
                case "Cliente":
                    ClienteBO cCliente = new ClienteBO();

                    cCliente.Nombre = this.vista.Cliente;
                    cCliente.Activo = true;

                    obj = cCliente;
                    break;
            }

            return obj;
        }
        public void DesplegarResultadoBuscador(string catalogo, object selecto)
        {
            switch (catalogo)
            {
                case "Propietario":
                    #region Desplegar Propietario
                    ClienteBO cPropietario = (ClienteBO)selecto;

                    if (cPropietario != null && cPropietario.Nombre != null)
                        this.vista.Propietario = cPropietario.Nombre;
                    else
                        this.vista.Propietario = null;

                    if (cPropietario != null && cPropietario.Id != null)
                        this.vista.PropietarioID = cPropietario.Id;
                    else
                        this.vista.PropietarioID = null;

                    EArea? Area = null;
                    
                    if(this.vista.DepartamentoDestinoID.HasValue)
                        Area = (EArea) this.vista.DepartamentoDestinoID.Value;

                    if (Area != null && (Area == EArea.SD || Area == EArea.CM || Area == EArea.RD))
                    {
                        this.vista.Cliente = this.vista.Propietario;
                        this.vista.ClienteID = this.vista.PropietarioID;
                    }
                    #endregion
                    break;
                case "Cliente":
                    #region Desplegar Cliente
                    ClienteBO cCliente = (ClienteBO)selecto;

                    if (cCliente != null && cCliente.Nombre != null)
                        this.vista.Cliente = cCliente.Nombre;
                    else
                        this.vista.Cliente = null;

                    if (cCliente != null && cCliente.Id != null)
                        this.vista.ClienteID = cCliente.Id;
                    else
                        this.vista.ClienteID = null;
                    #endregion
                    break;
            }
        }
        #endregion

        #region Seguridad
        /// <summary>
        /// Valida el acceso a la página de edición del contrato
        /// </summary>
        public void ValidarAcceso()
        {
            try
            {
                //Valida que usuario y la unidad operativa no sean nulos
                if (this.vista.UsuarioID == null) throw new Exception("El identificador del usuario no debe ser nulo");
                if (this.vista.UnidadOperativaID == null) throw new Exception("El identificador de la Unidad Operativa no debe ser nulo");
                //Se crea el objeto de seguridad
                UsuarioBO usuario = new UsuarioBO() { Id = this.vista.UsuarioID };
                AdscripcionBO adscripcion = new AdscripcionBO() { UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaID } };
                SeguridadBO seguridadBO = new SeguridadBO(Guid.Empty, usuario, adscripcion);

                //Se valida si el usuario tiene permiso a la acción principal
                if (!FacadeBR.ExisteAccion(this.dctx, "ACTUALIZARUNIDADDEPARTAMENTO", seguridadBO))
                    this.vista.RedirigirSinPermisoAcceso();
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0}.ValidarAcceso:{1}{2}", nombreClase, Environment.NewLine, ex.Message));
            }
        }
        /// <summary>
        /// Verifica los permisos de los usuarios y establece las opciones a las cuales tiene permiso el acceso
        /// </summary>
        public void EstablecerSeguridad()
        {
            try
            {
                //Valida que el usuario y la unidad operativa no sean nulos
                if (this.vista.UsuarioID == null) throw new Exception("El identificador del usuario no debe ser nulo");
                if (this.vista.UnidadOperativaID == null)
                    throw new Exception("El identificador de la Unidad Operativa no debe ser nulo");
                // Creación del objeto seguridad
                UsuarioBO usuario = new UsuarioBO { Id = this.vista.UsuarioID };
                AdscripcionBO adscripcion = new AdscripcionBO
                {
                    UnidadOperativa = new UnidadOperativaBO { Id = this.vista.UnidadOperativaID }
                };
                SeguridadBO seguridad = new SeguridadBO(Guid.Empty, usuario, adscripcion);

                //consulta de acciones a la cual el usuario tiene permisos
                List<CatalogoBaseBO> acciones = FacadeBR.ConsultarAccion(dctx, seguridad);

                // se valida si el usuario tiene permisos para registrar
                if (!this.ExisteAccion(acciones, "ACTUALIZARUNIDADDEPARTAMENTO"))
                    this.vista.PermitirRegistrar(false);

                if (!this.ExisteAccion(acciones, "UI CONSULTAR"))
                    this.vista.PermitirConsultar(false);
            }
            catch (Exception ex)
            {

                throw new Exception(nombreClase + ".EstablecerSeguridad:" + ex.Message);
            }
        }
        /// <summary>
        /// Verifica si la acción que el usuario desea ejecutar esta configurada
        /// </summary>
        /// <param name="acciones">Listado de acciones configuradas</param>
        /// <param name="nombreAccion">Nombre de la acción que se desea validar</param>
        /// <returns>Verdadero si la accion esta configurada; en otro caso falso</returns>
        private bool ExisteAccion(List<CatalogoBaseBO> acciones, string nombreAccion)
        {
            if (acciones != null && acciones.Count > 0 &&
                acciones.Exists(
                    p => p != null && p.Nombre != null && p.Nombre.Trim().ToUpper() == nombreAccion.Trim().ToUpper()))
                return true;

            return false;
        }
        #endregion        
        #endregion        
    }
}