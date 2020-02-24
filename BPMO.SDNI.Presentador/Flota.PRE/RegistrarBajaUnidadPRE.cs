//Esta clase satisface los requerimientos especificados en el caso de uso CU082 – REGISTRAR MOVIMIENTO DE FLOTA
//Satisface la solicitud de cambio SC0006

using System;
using System.Data;
using System.Collections.Generic;
using System.Text;
using BPMO.Basicos.BO;
using BPMO.Facade.SDNI.BR;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Contratos.BO;
using BPMO.SDNI.Equipos.BO;
using BPMO.SDNI.Equipos.BR;
using BPMO.SDNI.Flota.BOF;
using BPMO.SDNI.Flota.BR;
using BPMO.SDNI.Flota.VIS;
using BPMO.SDNI.Tramites.BO;
using BPMO.Servicio.Catalogos.BO;
using UnidadBO = BPMO.SDNI.Equipos.BO.UnidadBO;
using System.Collections;
using BPMO.SDNI.Comun.PRE;
using BPMO.SDNI.Comun.BR;
using BPMO.SDNI.Flota.RPT;
using System.IO;
using BPMO.SDNI.Comun.DA;
using BPMO.SDNI.Comun.DAO;

namespace BPMO.SDNI.Flota.PRE
{
    /// <summary>
    /// Presentador para el registro de la baja de la unidad
    /// </summary>
    public class RegistrarBajaUnidadPRE
    {
        #region Atributos
        /// <summary>
        /// Vista para la baja de unidad
        /// </summary>
        private readonly IRegistrarBajaUnidadVIS vista;
        /// <summary>
        /// Provee la conexión a la BD
        /// </summary>
        private readonly IDataContext dctx;
        /// <summary>
        /// Nombre de la clase que se usará en los mensajes de error
        /// </summary>
        private const string nombreClase = "RegistrarBajaUnidadPRE";
        /// <summary>
        /// Controlador que ejecutará las accciones
        /// </summary>
        private readonly MantenimientoFlotaBR controlador;

        private ucCatalogoDocumentosPRE presentadorDocumentos;

        /// <summary>
        /// Presentador para el catálogo de documentos.
        /// </summary>
        public ucCatalogoDocumentosPRE PresentadorDocumentos
        {
            get { return presentadorDocumentos; }
            set { presentadorDocumentos = value; }
        }
        #endregion

        #region Constructores
        /// <summary>
        /// Constructor del presentador
        /// </summary>
        /// <param name="view">vista de la página de reasignación de sucursal</param>
        public RegistrarBajaUnidadPRE(IRegistrarBajaUnidadVIS view)
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
                this.vista.MostrarMensaje("Inconsistencia al crear el presentador", ETipoMensajeIU.ERROR, string.Format("{0}.RegistrarBajaUnidadPRE:{1}{2}", nombreClase, Environment.NewLine, ex.Message));
            }
        }
        #endregion

        #region Métodos
        /// <summary>
        /// Cancela la baja de la unidad a la sucursal
        /// </summary>
        public void Cancelar()
        {
            this.vista.LimpiarSesion();
            this.vista.LimpiarPaqueteNavegacion("UnidadExpedienteBO");
            var unidadID = this.vista.UnidadID;
            if (unidadID.HasValue)
                this.vista.EstablecerPaqueteNavegacion("UnidadExpedienteBO", new UnidadBO { UnidadID = unidadID });
            this.vista.RedirigirADetalles(false);
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
                var unidadBase = new ElementoFlotaBOF { Unidad = new UnidadBO() };

                //Eliminamos el paquete de navegación
                this.vista.LimpiarPaqueteNavegacion("UnidadExpedienteBO");

                #region Se agrega la sección de documentos adjuntos
                switch (this.vista.UnidadOperativaID)
                {
                    case (int)ETipoEmpresa.Generacion:
                    case (int)ETipoEmpresa.Construccion:
                    case (int)ETipoEmpresa.Equinova:
                        //Se establece el modo edición al catálogo de documentos
                        this.presentadorDocumentos.ModoEditable(true);

                        //Se establecen los tipos de archivos que serán permitidos para adjuntar al contrato
                        List<TipoArchivoBO> lstTipoArchivo = new TipoArchivoBR().Consultar(this.dctx, new TipoArchivoBO());
                        var o = new object();
                        this.presentadorDocumentos.Vista.Identificador = o.GetHashCode().ToString();
                        this.presentadorDocumentos.EstablecerTiposArchivo(lstTipoArchivo);

                        this.vista.MostrarSeccionDocumentos(true);
                        this.vista.MostrarSeccionCentroCostos(true);
                        break;
                    default:
                        this.presentadorDocumentos.LimpiarSesion();
                        this.vista.MostrarSeccionDocumentos(false);
                        this.vista.MostrarSeccionCentroCostos(false);
                        break;
                }
               
                #endregion

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
                    throw new Exception("El estatus de la unidad no fue recuperado, para continuar con la baja de unidad es necesario especificar el estatus actual de la unidad.");

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

                #region SC0006 - Adición de validación para el estatus de Siniestro
                bool lPermitir = true;
                switch (this.vista.UnidadOperativaID)
                {
                    case (int)ETipoEmpresa.Generacion:
                    case (int)ETipoEmpresa.Equinova:
                    case (int)ETipoEmpresa.Construccion:
                        lPermitir = unidadBase.Unidad.EstatusActual != null && (unidadBase.Unidad.EstatusActual == EEstatusUnidad.Disponible || unidadBase.Unidad.EstatusActual == EEstatusUnidad.EnVenta || unidadBase.Unidad.EstatusActual == EEstatusUnidad.Siniestro);
                        break;
                    default:
                        if ((unidadBase.Unidad.EstatusActual.Value != EEstatusUnidad.Disponible) &&
                            (unidadBase.Unidad.EstatusActual.Value != EEstatusUnidad.EnTaller) &&
                            (unidadBase.Unidad.EstatusActual.Value != EEstatusUnidad.Seminuevo) &&
                            (unidadBase.Unidad.EstatusActual.Value != EEstatusUnidad.Siniestro))
                        {
                            lPermitir = false;
                            this.vista.MostrarMensaje("No se puede dar de baja a la unidad de la sucursal actual, porque esta se encuentra en uso, verifique su información", ETipoMensajeIU.ADVERTENCIA, null);
                        }
                        break;
                }
                this.vista.PermitirRegistrar(lPermitir);
                #endregion

                //Validamos los equipos aliados
                if (unidadBase.TieneEquipoAliado.HasValue)
                    if (unidadBase.TieneEquipoAliado.Value)
                    {
                        this.vista.PermitirRegistrar(false);
                        this.vista.MostrarMensaje("No se puede dar de baja a la unidad de la sucursal actual, porque esta aun cuenta con equipos aliados asociados, elimine los equipos e intente de nuevo.", ETipoMensajeIU.ADVERTENCIA, null);
                    }

                //Desplegamos la unidad obtenida
                this.DatoAInterfazUsuario(unidadBase);
                this.EstablecerSeguridad();
            }
            catch (Exception ex)
            {
                this.vista.PermitirRegistrar(false);
                throw new Exception(string.Format("{0}.PrepararEdicion:{1}{2}", nombreClase, Environment.NewLine, ex.Message));
            }
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
                        this.vista.DomicilioSucursalActual = dir;
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

            this.vista.MostrarSeccionSiniestro(unidadBase.Unidad.EstatusActual.Value != EEstatusUnidad.Siniestro);
            #endregion
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
            if (!this.vista.UsuarioID.HasValue)
                s += "Usuario realiza baja, ";
            if (string.IsNullOrEmpty(this.vista.Observaciones) || string.IsNullOrWhiteSpace(this.vista.Observaciones))
                s += "Observaciones, ";

            if (this.vista.UnidadOperativaID == (int)ETipoEmpresa.Construccion || this.vista.UnidadOperativaID == (int)ETipoEmpresa.Generacion
                 || this.vista.UnidadOperativaID == (int)ETipoEmpresa.Equinova)
            {
                if (string.IsNullOrEmpty(this.vista.CentroCostos) || string.IsNullOrWhiteSpace(this.vista.CentroCostos))
                    s += "Centro de Costos, ";
            }

            #region SC0006 - Adición de validaciones cuando se da de baja por siniestro
            if (this.vista.Siniestro)
            {
                if (!this.vista.FechaSiniestro.HasValue)
                    s += "Fecha Siniestro, ";

                if (!this.vista.FechaDictamen.HasValue)
                    s += "Fecha Dictamen, ";

                if (string.IsNullOrEmpty(this.vista.ObservacionesSiniestro) || string.IsNullOrWhiteSpace(this.vista.ObservacionesSiniestro))
                    s += "Observaciones Siniestro, ";
            }
            #endregion

            if (s != null && s.Trim().CompareTo("") != 0)
                return "Los siguientes campos no pueden estar vacíos: \n" + s.Substring(0, s.Length - 2);

            return null;
        }
        /// <summary>
        /// Devuelve un objeto de negocio a partir de la información de la vista
        /// </summary>
        /// <returns>BO con la información a editar</returns>
        private UnidadBO InterfazUsuarioADato()
        {
            var bo = new UnidadBO((UnidadBO)this.vista.ObjetoEdicion);

            #region SC0006 - Adición de estatus de siniestro cuando se marca que es una baja por siniestro
            bo.EstatusActual = this.vista.Siniestro ? EEstatusUnidad.Siniestro : EEstatusUnidad.Baja;
            #endregion

            bo.Sucursal.Id = this.vista.SucursalActualID;

            #region Agregar los documentos adjuntos
            bo.Adjuntos = this.presentadorDocumentos.Vista.NuevosArchivos;
            #endregion

            //bo.UUA = this.vista.UsuarioID;
            bo.FUA = DateTime.Now;

            return bo;
        }

        /// <summary>
        ///  Devuelve un objeto de negocio a partir de la información del siniestro capturado
        /// </summary>
        /// <returns></returns>
        private SiniestroUnidadBO InterfazUsuarioASiniestro()
        {
            SiniestroUnidadBO historial = new SiniestroUnidadBO();
            historial.FechaSiniestro = this.vista.FechaSiniestro;
            historial.FechaDictamen = this.vista.FechaDictamen;
            historial.Observaciones = this.vista.ObservacionesSiniestro;

            return historial;
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
            var mnj = new StringBuilder(string.Format("Sucursal {0}, Empresa {1}. ", this.vista.SucursalActualNombre, this.vista.EmpresaActualNombre));
            mnj.Append(this.vista.Observaciones);

            #region SC0006 - Adición de lógica para las bajas por siniestro
            IDictionary parameters = new Hashtable();
            parameters["Observaciones"] = mnj.ToString();

            if (this.vista.Siniestro)
            {
                parameters["HistorialSiniestro"] = this.InterfazUsuarioASiniestro();                
                this.controlador.RealizarBajaPorSiniestroUnidadFlota(dctx, bo, (UnidadBO)this.vista.UltimoObjeto, parameters, seguridadBO);
            }
            else
                this.controlador.RealizarBajaUnidadFlota(dctx, bo, (UnidadBO)this.vista.UltimoObjeto, parameters, seguridadBO);
            #endregion
        }
        /// <summary>
        /// Realiza la baja de la unidad
        /// </summary>
        public void RealizarBajaUnidadFlota()
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

            //Se envía los datos de la interfaz al diccionario del reporte 
            //Se obtiene los Datos para el reporte.
            DataSet DsBajas = new DataSet();
            DataTable table = DsBajas.Tables["DatosBaja"];

            UnidadBO oBajas = new UnidadBO() { UnidadID = unidadID, UUA = this.vista.UsuarioID };
   
            string Empresa = this.vista.EmpresaActualNombre;
            bool siniestro = this.vista.Siniestro;
            string Tipo = this.vista.TipoUnidadNombre;
            int? IdUsuario = this.vista.UsuarioID;

            int? ModuloID = this.vista.ModuloID;

            Dictionary<string, Object> datos = this.controlador.ObtenerDatosReporte(dctx, oBajas, unidadID, ModuloID, Empresa, siniestro, Tipo, IdUsuario);
            #region Asignar la Unidad

            if (datos.ContainsKey("Unidad") && datos["Unidad"] != null)
                oBajas = (UnidadBO)datos["Unidad"];
            #endregion
            #region Obtener el folio del reporte
            ConsultarFolioDA consultarFolio = new ConsultarFolioDA();
            InsertarFolioDAO insertarFolio = new InsertarFolioDAO();
            
            int folio = 1;
            string serie = "AF";
            DataSet dsFolio = new DataSet();
            dsFolio = consultarFolio.Action(dctx, oBajas.Sucursal.Id, ETipoFoliador.Baja_Unidad);
            if (dsFolio.Tables.Contains("Folios") && dsFolio.Tables["Folios"].Rows.Count > 0)
            {
                folio = dsFolio.Tables["Folios"].Rows[0].Field<int>("Folio") + 1;

                AuditoriaBO auditoriaBO = new AuditoriaBO
                {
                    FC = DateTime.Now,
                    FUA = DateTime.Now,
                    UC = this.vista.UsuarioID,
                    UUA = this.vista.UsuarioID
                };

                insertarFolio.Action(dctx, this.vista.SucursalActualID, serie, folio, ETipoFoliador.Baja_Unidad, auditoriaBO);
            }
            else
            {
                AuditoriaBO auditoriaBO = new AuditoriaBO
                {
                    FC = DateTime.Now,
                    FUA = DateTime.Now,
                    UC = this.vista.UsuarioID,
                    UUA = this.vista.UsuarioID
                };

                insertarFolio.Action(dctx, this.vista.SucursalActualID, serie, folio, ETipoFoliador.Baja_Unidad, auditoriaBO);
            }
            datos.Add("Folio", serie + folio);
            #endregion
            datos.Add("CentroCostos", this.vista.CentroCostos.Trim().ToUpper());
            datos.Add("UnidadBO", datos);
            this.vista.EstablecerPaqueteNavegacionReporte("BajaUnidad", datos);

            if (unidadID.HasValue)
                this.vista.EstablecerPaqueteNavegacion("UnidadExpedienteBO", new UnidadBO { UnidadID = unidadID });

            #region Generar el correo para enviar a los autorizadores
            if (this.vista.UnidadOperativaID == (int)ETipoEmpresa.Generacion || this.vista.UnidadOperativaID == (int)ETipoEmpresa.Construccion 
                || this.vista.UnidadOperativaID == (int)ETipoEmpresa.Equinova)
            {
                int moduloID = (int)this.vista.ModuloID;
                byte[] reporte = this.GenerarReporteAdjuntar(datos);
                Enum TipoAutorizacion = this.vista.UnidadOperativaID == (int)ETipoEmpresa.Generacion ? (Enum)ETipoAutorizacionGeneracion.Baja_Unidad : 
                    this.vista.UnidadOperativaID == (int)ETipoEmpresa.Construccion ? (Enum)ETipoAutorizacionConstruccion.Baja_Unidad :
                    (Enum)ETipoAutorizacionEquinova.Baja_Unidad;

                AutorizadorBR oAutorizador = new AutorizadorBR();
                #region Crear el Diccionario para reemplazar los valores del formato con los de la entrega.
                Dictionary<string, string> datosAutorizacion = new Dictionary<string, string>();
                #endregion
                string error = string.Empty;
                try
                {
                    oAutorizador.SolicitarAutorizacion(dctx, TipoAutorizacion, datosAutorizacion, moduloID, this.vista.UnidadOperativaID, this.vista.SucursalActualID, reporte);
                }
                catch (Exception ex)
                {
                    error = ex.Message;
                }
            }
            #endregion

            this.vista.RedirigirADetalles(true);
        }

        #region Generar Reporte Adjunto
        /// <summary>
        /// Genera el array de bytes del reporte de checklist de Entrega
        /// </summary>
        /// <param name="datosReporte">Diccionario de datos con la información del reporte.</param>
        /// <param name="tipoReporte">Tipo de Reporte que se generará.</param>
        /// <returns></returns>
        private byte[] GenerarReporteAdjuntar(Dictionary<string, object> datosReporte)
        {
            using (BajaActivoFijoRPT oReporte = new BajaActivoFijoRPT(datosReporte))
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    oReporte.CreateDocument();
                    oReporte.ExportToPdf(stream);
                    return stream.GetBuffer();
                }
            }
        }
        #endregion

        /// <summary>
        /// Valida que el tipo de archivo seleccionado este permitido dentro de los tipos configurados
        /// </summary>
        /// <param name="tipo">Tipo de archivo que desea validar</param>
        /// <returns>Verdadero si la extensión se encuentra, falso si no</returns>
        private TipoArchivoBO ValidarArchivo(String tipo)
        {
            List<TipoArchivoBO> tiposArchivo = (List<TipoArchivoBO>)this.vista.TiposArchivo;
            if (tiposArchivo != null)
            {
                TipoArchivoBO tipoArchivoTemp = tiposArchivo.Find(delegate(TipoArchivoBO ta) { return ta.Extension == tipo; });
                if (tipoArchivoTemp != null)
                {
                    return tipoArchivoTemp;
                }
                else
                {
                    this.vista.MostrarMensaje("El archivo no fue cargado.", ETipoMensajeIU.ERROR,
                        "La extensión del archivo no se encuentra en la lista de tipos de archivo permitidos.");
                }
            }
            else
            {
                this.vista.MostrarMensaje("El archivo no fue cargado.", ETipoMensajeIU.ERROR, "No hay una lista de tipos de archivo cargada.");
            }
            return null;
        }

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
                if (!FacadeBR.ExisteAccion(this.dctx, "REALIZARBAJAUNIDADFLOTA", seguridadBO))
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
                if (!this.ExisteAccion(acciones, "REALIZARBAJAUNIDADFLOTA"))
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
