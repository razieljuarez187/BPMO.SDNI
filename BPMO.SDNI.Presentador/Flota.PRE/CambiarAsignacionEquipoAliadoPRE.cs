﻿//Esta clase satisface los requerimientos especificados en el caso de uso CU082 – REGISTRAR MOVIMIENTO DE FLOTA
using System;
using System.Collections.Generic;
using System.Text;
using BPMO.Basicos.BO;
using BPMO.Facade.SDNI.BR;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Contratos.BO;
using BPMO.SDNI.Equipos.BO;
using BPMO.SDNI.Equipos.BOF;
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
    /// Presentador para la vista de cambio de equipos aliados a la unidad
    /// </summary>
    public class CambiarAsignacionEquipoAliadoPRE
    {
        #region Atributos
        /// <summary>
        /// Vista para la reasignación de equipos aliados
        /// </summary>
        private readonly ICambiarAsignacionEquipoAliadoVIS vista;
        /// <summary>
        /// Provee la conexión a la BD
        /// </summary>
        private readonly IDataContext dctx;
        /// <summary>
        /// Nombre de la clase que se usará en los mensajes de error
        /// </summary>
        private const string nombreClase = "CambiarAsignacionEquipoAliadoPRE";
        /// <summary>
        /// Controlador que ejecutará las accciones
        /// </summary>
        private readonly MantenimientoFlotaBR controlador;
        #endregion

        #region Constructores
        /// <summary>
        /// Cosntructor para el presentador de la asiganción del equipo aliado
        /// </summary>
        /// <param name="view">Vista de la asignación de lso equipos aliados</param>
        public CambiarAsignacionEquipoAliadoPRE(ICambiarAsignacionEquipoAliadoVIS view)
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
                this.vista.MostrarMensaje("Inconsistencia al crear el presentador", ETipoMensajeIU.ERROR, string.Format("{0}.CambiarAsignacionEquipoAliadoPRE:{1}{2}", nombreClase, Environment.NewLine, ex.Message));
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
                this.vista.EstablecerPaqueteNavegacion("UnidadExpedienteBO", new BPMO.SDNI.Equipos.BO.UnidadBO { UnidadID = unidadID });
            this.vista.RedirigirADetalles();
        }
        /// <summary>
        /// Prepara la edición de la unidad
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
                    throw new Exception("El estatus de la unidad no fue recuperado, para continuar con la reasignación es necesario especificar el estatus actual de la unidad.");

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
                    this.vista.MostrarMensaje("No se puede dar de baja a la unidad de la sucursal actual, porque esta se encuentra en uso, verifique su información", ETipoMensajeIU.ADVERTENCIA, null);
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
            
            if (tPlacaFederal != null)
                this.vista.NumeroPlaca = tPlacaFederal.Numero;
            else            
                this.vista.NumeroPlaca = tPlacaEstatal != null ? tPlacaEstatal.Numero : null;            
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

            #region EquiposAliados
            this.vista.EquiposAliados = bo.EquiposAliados.ConvertAll(x => (object)x);
            this.vista.ActualizarEquiposAliados();
            this.vista.CambiosEquipos = null;
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
            #endregion
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
                s += "Usuario realiza el cambio del equipo, ";

            if (s != null && s.Trim().CompareTo("") != 0)
                return "Los siguientes campos no pueden estar vacíos: \n" + s.Substring(0, s.Length - 2);

            if (!this.vista.CambiosEquipos.HasValue)
                return "Aun no se realizan movimientos para los equipos aliados en la unidad.";

            if (!this.vista.CambiosEquipos.Value)
                return "Aun no se realizan movimientos para los equipos aliados en la unidad.";

            return null;
        }
        /// <summary>
        /// Devuelve un objeto de negocio a partir de la información de la vista
        /// </summary>
        /// <returns>BO con la información a editar</returns>
        private UnidadBO InterfazUsuarioADato()
        {
            var bo = new UnidadBO((UnidadBO)this.vista.ObjetoEdicion);

            if (this.vista.EquiposAliados != null)
            {
                var equipos = this.vista.EquiposAliados.ConvertAll(x => (EquipoAliadoBO)x);
                if (this.vista.EquiposAliados.Count > 0)
                {
                    foreach (var aliado in equipos)
                    {
                        aliado.UUA = this.vista.UsuarioID;
                        aliado.FUA = DateTime.Now;
                    }
                }
                bo.EquiposAliados = null;
                bo.EquiposAliados = equipos;
            }

            bo.UUA = this.vista.UsuarioID;
            bo.FUA = DateTime.Now;

            return bo;
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

            this.controlador.ActualizarAsignacionEquiposAliados(dctx, bo, (UnidadBO)this.vista.UltimoObjeto, mnj.ToString(), seguridadBO);
        }
        /// <summary>
        /// Obtiene la inforamción completa del equipo aliado
        /// </summary>
        /// <param name="bo">Equipo aliado que se desea consultar</param>
        /// <returns>Equipo aliado con su información completa</returns>
        private EquipoAliadoBO ObtenerEquipoAliadoCompleto(EquipoAliadoBO bo)
        {
            var br = new EquipoAliadoBR();
            List<EquipoAliadoBO> lstTemp = br.Consultar(this.dctx, bo);
            if (lstTemp.Count > 0)
                bo = lstTemp[0];

            return bo;
        }
        /// <summary>
        /// Valida que el equiop aliado este disponible para su selección
        /// </summary>
        /// <returns></returns>
        private string ValidarCamposEquipoAliado()
        {
            if (this.vista.EquipoAliadoID == null)
                return "Es necesario seleccionar un equipo aliado a agregar.";

            List<EquipoAliadoBO> equiposAliados = this.vista.EquiposAliados.ConvertAll(x => (EquipoAliadoBO)x);
            if (equiposAliados.Exists(p => p.EquipoAliadoID == this.vista.EquipoAliadoID))
                return "El equipo aliado ya se encuentra asignado a la unidad.";

            return null;
        }
        /// <summary>
        /// Agrega los equipos aliados a la lsita de control
        /// </summary>
        /// <param name="lst">Lista de equipos aliados</param>
        public void AgregarEquiposAliado(List<EquipoAliadoBO> lst)
        {
            try
            {
                this.vista.EquiposAliados = lst.ConvertAll(x => (object)x);

                this.vista.ActualizarEquiposAliados();
            }
            catch (Exception ex)
            {
                throw new Exception(nombreClase + ".AgregarEquiposAliado:" + Environment.NewLine + ex.Message);
            }
        }
        /// <summary>
        /// Agrega un equipo aliado a la lista
        /// </summary>
        public void AgregarEquipoAliado()
        {
            string s;
            if ((s = this.ValidarCamposEquipoAliado()) != null)
            {
                this.vista.MostrarMensaje(s, ETipoMensajeIU.INFORMACION, null);
                return;
            }

            try
            {
                List<EquipoAliadoBO> equiposAliados = this.vista.EquiposAliados.ConvertAll(x => (EquipoAliadoBO)x);

                EquipoAliadoBO equipoAliado = new EquipoAliadoBO();
                equipoAliado.Sucursal = new SucursalBO();
                equipoAliado.Sucursal.UnidadOperativa = new UnidadOperativaBO();
                equipoAliado.Mediciones = new MedicionesBO();
                equipoAliado.Modelo = new ModeloBO();

                equipoAliado.NumeroSerie = this.vista.EquipoAliadoNumeroSerie;
                equipoAliado.EquipoAliadoID = this.vista.EquipoAliadoID;
                equipoAliado = this.ObtenerEquipoAliadoCompleto(equipoAliado);

                if (this.vista.EquiposAliados == null)
                    this.vista.EquiposAliados = new List<EquipoAliadoBO>().ConvertAll(x => (object)x);

                equiposAliados.Add(equipoAliado);

                this.AgregarEquiposAliado(equiposAliados);

                this.vista.PrepararNuevoEquipoAliado();
                this.vista.CambiosEquipos = true;
            }
            catch (Exception ex)
            {
                throw new Exception(nombreClase + ".AgregarEquipoAliado:" + Environment.NewLine + ex.Message);
            }
        }
        /// <summary>
        /// Quita un equipo aliado de la lista
        /// </summary>
        /// <param name="index">Indice del equipop aliado que se desea quitar</param>
        public void QuitarEquipoAliado(int index)
        {
            try
            {
                if (index >= this.vista.EquiposAliados.Count || index < 0)
                    throw new Exception("No se encontró el equipo aliado seleccionado");

                List<EquipoAliadoBO> equiposAliados = this.vista.EquiposAliados.ConvertAll(x => (EquipoAliadoBO)x);
                equiposAliados.RemoveAt(index);

                this.vista.EquiposAliados = equiposAliados.ConvertAll(x => (object)x);
                this.vista.ActualizarEquiposAliados();
                this.vista.CambiosEquipos = true;
            }
            catch (Exception ex)
            {
                throw new Exception(nombreClase + ".QuitarEquipoAliado:" + Environment.NewLine + ex.Message);
            }
        }
        /// <summary>
        /// Actualiza la undiad con los movimietnos realizados  alos equipos aliados
        /// </summary>
        public void ActualizarAsignacionEquiposAliados()
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
                if (!FacadeBR.ExisteAccion(this.dctx, "ACTUALIZARASIGNACIONEQUIPOSALIADOS", seguridadBO))
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
                if (!this.ExisteAccion(acciones, "ACTUALIZARASIGNACIONEQUIPOSALIADOS"))
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

        #region Métodos para el Buscador
        public object PrepararBOBuscador(string catalogo)
        {
            object obj = null;

            switch (catalogo)
            {
                case "EquipoAliado":
                    EquipoAliadoBOF ea = new EquipoAliadoBOF();
                    ea.Sucursal = new SucursalBO();
                    ea.Sucursal.UnidadOperativa = new UnidadOperativaBO();
                    ea.Sucursal.UnidadOperativa.Empresa = new EmpresaBO();

                    ea.NumeroSerie = this.vista.EquipoAliadoNumeroSerie;
                    ea.Sucursal.UnidadOperativa.Id = this.vista.UnidadOperativaID;
                    ea.Estatus = EEstatusEquipoAliado.SinAsignar;

                    obj = ea;
                    break;
            }

            return obj;
        }
        public void DesplegarResultadoBuscador(string catalogo, object selecto)
        {
            switch (catalogo)
            {
                case "EquipoAliado":
                    #region Desplegar Propietario
                    EquipoAliadoBOF ea = (EquipoAliadoBOF)selecto;

                    if (ea != null && ea.NumeroSerie != null)
                        this.vista.EquipoAliadoNumeroSerie = ea.NumeroSerie;
                    else
                        this.vista.EquipoAliadoNumeroSerie = null;

                    if (ea != null && ea.EquipoAliadoID != null)
                        this.vista.EquipoAliadoID = ea.EquipoAliadoID;
                    else
                        this.vista.EquipoAliadoID = null;
                    #endregion
                    break;
            }
        }
        #endregion
        #endregion
    }
}