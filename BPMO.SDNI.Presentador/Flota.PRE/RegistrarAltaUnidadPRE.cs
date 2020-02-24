//Satisface al CU082 - Registrar Movimiento de Flota
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
    /// Presentador para la página de alta de unidad
    /// </summary>
    public class RegistrarAltaUnidadPRE
    {
        #region Atributos

        private IDataContext dctx = null;
        private IRegistrarAltaUnidadVIS vista;
        private string nombreClase = "RegistrarAltaUnidadPRE";
        private readonly MantenimientoFlotaBR controlador;
        #endregion

        #region Constructores

        public RegistrarAltaUnidadPRE(IRegistrarAltaUnidadVIS view)
        {
            try
            {
                this.vista = view;
                this.controlador = new MantenimientoFlotaBR();
                this.dctx = FacadeBR.ObtenerConexion();
            }
            catch (Exception ex)
            {
                this.vista.MostrarMensaje("Inconsistencias en los parámetros de configuración", ETipoMensajeIU.ERROR, this.nombreClase + ".RegistrarAltaUnidadPRE:" + ex.Message);
            }
        }
        #endregion

        #region Métodos

        public void RealizarPrimeraCarga()
        {
            this.EstablecerDatosNavegacion(this.vista.ObtenerPaqueteNavegacion("UnidadExpedienteBO"));
            this.EstablecerSeguridad();
        }
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
                if (!FacadeBR.ExisteAccion(this.dctx, "REALIZARALTAUNIDADFLOTA", seguridadBO))
                    this.vista.RedirigirSinPermisoAcceso();
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0}.ValidarAcceso:{1}{2}", nombreClase, Environment.NewLine, ex.Message));
            }
        }

        private void EstablecerDatosNavegacion(object paqueteNavegacion)
        {
            try
            {
                if (paqueteNavegacion == null)
                    throw new Exception("Se esperaba un objeto en la navegación. No se puede identificar qué unidad desea dar de alta.");
                if (!(paqueteNavegacion is UnidadBO))
                    throw new Exception("Se esperaba una unidad.");

                UnidadBO boAnterior = (UnidadBO)paqueteNavegacion;
                var unidadBase = new ElementoFlotaBOF { Unidad = new UnidadBO() };

                this.vista.LimpiarPaqueteNavegacion("UnidadExpedienteBO");

                boAnterior = this.ObtenerUnidad(boAnterior);
                this.vista.UltimoObjeto = boAnterior;

                //Consultamos la unidad para el elemento a editar
                unidadBase.Unidad.UnidadID = boAnterior.UnidadID;
                unidadBase = this.ObtenerElemento(unidadBase.Unidad);

                if (unidadBase == null) unidadBase = new ElementoFlotaBOF();
                if (unidadBase != null && unidadBase.Unidad == null) unidadBase.Unidad = new UnidadBO();

                if (!unidadBase.Unidad.EstatusActual.HasValue)
                    throw new Exception("El estatus de la unidad no fue recuperado, para continuar con la alta de la unidad es necesario especificar el estatus actual de la unidad.");

                //Validar estatus de acuerdo a regla
                if (!boAnterior.ValidarCambioEstatus(unidadBase.Unidad.EstatusActual))
                {
                    string mnj = "No se puede cambiar el estatus de la unidad de " +
                                 (boAnterior.EstatusActual != null ? boAnterior.EstatusActual.ToString() : "") +
                                 " a " +
                                 (unidadBase.Unidad.EstatusActual != null ? unidadBase.Unidad.EstatusActual.ToString() : "");
                    this.vista.PermitirRegistrar(false);
                    this.vista.MostrarMensaje(mnj, ETipoMensajeIU.ADVERTENCIA, null);
                }

                if ((unidadBase.Unidad.EstatusActual.Value != EEstatusUnidad.Terminada))
                {
                    this.vista.PermitirRegistrar(false);
                    this.vista.MostrarMensaje("No se puede dar de alta la unidad, debido a que no cuenta con el estatus Terminada, verifique su información", ETipoMensajeIU.ADVERTENCIA, null);
                }

                this.DatoAInterfazUsuario(unidadBase);
            }
            catch (Exception ex)
            {
                this.DatoAInterfazUsuario(new UnidadBO());
                this.vista.PermitirRegistrar(false);
                throw new Exception(this.nombreClase + ".EstablecerDatosNavegacion: " + ex.Message);
            }

        }

        private UnidadBO ObtenerUnidad(UnidadBO boAnterior)
        {
            UnidadBO unidadBO = boAnterior;
            UnidadBR unidadBR = new UnidadBR();

            //Se consulta la unidad en base al ultimo objeto
            List<UnidadBO> unidades = unidadBR.ConsultarCompleto(dctx, unidadBO, true);

            //Limpiamos la instancia del objeto
            unidadBO = null;

            //Validamos que realmente hay una unidad para su edición
            if (!ReferenceEquals(unidades, null))
                if (unidades.Count > 0)
                    unidadBO = unidades[0];

            //Retornamos el objeto
            return unidadBO;
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

        private object InterfazUsuarioADato()
        {
            UnidadBO bo = new UnidadBO();
            if (this.vista.UltimoObjeto != null)
                bo = new UnidadBO((UnidadBO)this.vista.UltimoObjeto);

            bo.EstatusActual = EEstatusUnidad.Disponible;
            bo.UUA = this.vista.UsuarioID;
            bo.FUA = DateTime.Now;

            return bo;

        }
        /// <summary>
        /// Despliega en la vista la información de la unidad que se haya recuperado
        /// </summary>
        /// <param name="obj">Unidad que se desea visualizar</param>
        private void DatoAInterfazUsuario(object obj)
        {
            var elemento = (ElementoFlotaBOF)obj;

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

            #region Datos unidad
            UnidadBO bo = elemento.Unidad;

            if (bo == null) bo = new UnidadBO();
            if (bo.Sucursal == null) bo.Sucursal = new SucursalBO();
            if (bo.Sucursal.UnidadOperativa == null) bo.Sucursal.UnidadOperativa = new UnidadOperativaBO();
            //if (bo.Sucursal.UnidadOperativa.Empresa == null) bo.Sucursal.UnidadOperativa.Empresa = new EmpresaBO();
            if (bo.Modelo == null) bo.Modelo = new ModeloBO();
            if (bo.Modelo.Marca == null) bo.Modelo.Marca = new MarcaBO();
            if (bo.TipoEquipoServicio == null) bo.TipoEquipoServicio = new TipoUnidadBO();
            if (bo.ActivoFijo == null) bo.ActivoFijo = new ActivoFijoBO();

            //Detalle de la unidad
            this.vista.UnidadID = bo.UnidadID;
            this.vista.VIN = bo.NumeroSerie;
            this.vista.ClaveActivoOracle = bo.ClaveActivoOracle;
            this.vista.LiderID = bo.IDLider;
            this.vista.NumeroEconomico = bo.NumeroEconomico;
            this.vista.TipoUnidadNombre = bo.TipoEquipoServicio.Nombre;
            this.vista.ModeloNombre = bo.Modelo.Nombre;
            this.vista.Anio = bo.Anio;
            this.vista.FechaCompra = bo.ActivoFijo.FechaFacturaCompra;
            this.vista.MontoFactura = bo.ActivoFijo.CostoSinIva;
            this.vista.FolioFactura = string.Empty;
            //Información de la Sucursal
            bool completa = false;
            this.vista.SucursalID = bo.Sucursal.Id;
            this.vista.SucursalNombre = bo.Sucursal.Nombre;

            #region Empresa
            var empresa = new EmpresaBO();
            if (bo.Sucursal.UnidadOperativa != null)
            {
                this.vista.EmpresaID = bo.Sucursal.UnidadOperativa.Id;


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

                    this.vista.NombreEmpresa = empresa.Nombre;
                }
                else
                {
                    if (!string.IsNullOrEmpty(bo.Sucursal.UnidadOperativa.Empresa.Nombre) && !string.IsNullOrWhiteSpace(bo.Sucursal.UnidadOperativa.Empresa.Nombre))
                        this.vista.NombreEmpresa = bo.Sucursal.UnidadOperativa.Empresa.Nombre;
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
                        this.vista.NombreEmpresa = empresa.Nombre;
                    }
                }
            }

            #endregion

            #region Domicilio Sucursal

            if (bo.Sucursal.DireccionesSucursal != null)
            {
                if (bo.Sucursal.DireccionesSucursal.Count > 0)
                {
                    var direccionActual =
                        bo.Sucursal.DireccionesSucursal.Find(p => p.Primaria != null && p.Primaria == true);
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
                            if (direccionActual.Ubicacion.Municipio != null &&
                                !string.IsNullOrEmpty(direccionActual.Ubicacion.Municipio.Nombre))
                                dir += (direccionActual.Ubicacion.Municipio.Nombre + " ");
                            if (direccionActual.Ubicacion.Ciudad != null &&
                                !string.IsNullOrEmpty(direccionActual.Ubicacion.Ciudad.Nombre))
                                dir += (direccionActual.Ubicacion.Ciudad.Nombre + " ");
                            if (direccionActual.Ubicacion.Estado != null &&
                                !string.IsNullOrEmpty(direccionActual.Ubicacion.Estado.Nombre))
                                dir += (direccionActual.Ubicacion.Estado.Nombre + " ");
                            if (direccionActual.Ubicacion.Pais != null &&
                                !string.IsNullOrEmpty(direccionActual.Ubicacion.Pais.Nombre))
                                dir += (direccionActual.Ubicacion.Pais.Nombre + " ");
                        }

                        if (dir != null && dir.Trim().CompareTo("") != 0)
                        {
                            this.vista.DomicilioSucursal = dir;
                            completa = true;
                        }
                        else
                            this.vista.DomicilioSucursal = null;
                    }
                    else
                        this.vista.DomicilioSucursal = null;
                }
                else
                    this.vista.DomicilioSucursal = null;
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
                        if (direccion.Ubicacion.Municipio != null &&
                            !string.IsNullOrEmpty(direccion.Ubicacion.Municipio.Nombre))
                            dir += (direccion.Ubicacion.Municipio.Nombre + " ");
                        if (direccion.Ubicacion.Ciudad != null &&
                            !string.IsNullOrEmpty(direccion.Ubicacion.Ciudad.Nombre))
                            dir += (direccion.Ubicacion.Ciudad.Nombre + " ");
                        if (direccion.Ubicacion.Estado != null &&
                            !string.IsNullOrEmpty(direccion.Ubicacion.Estado.Nombre))
                            dir += (direccion.Ubicacion.Estado.Nombre + " ");
                        if (direccion.Ubicacion.Pais != null && !string.IsNullOrEmpty(direccion.Ubicacion.Pais.Nombre))
                            dir += (direccion.Ubicacion.Pais.Nombre + " ");
                    }

                    if (dir != null && dir.Trim().CompareTo("") != 0)
                        this.vista.DomicilioSucursal = dir;
                    else
                        this.vista.DomicilioSucursal = null;
                }
                else
                    this.vista.DomicilioSucursal = null;
            }
            #endregion
            #endregion
        }

        public void Cancelar()
        {
            this.vista.LimpiarSesion();
            this.vista.LimpiarPaqueteNavegacion("UnidadExpedienteBO");
            var unidadID = this.vista.UnidadID;
            if (unidadID.HasValue)
                this.vista.EstablecerPaqueteNavegacion("UnidadExpedienteBO", new UnidadBO { UnidadID = unidadID });
            this.vista.RedirigirAExpediente();
        }

        public void RealizarAltaUnidadFlota()
        {
            this.RegistrarMovimiento();

            //Redirigimos al detalle de la unidad
            this.vista.LimpiarSesion();
            var unidadID = this.vista.UnidadID;
            if (unidadID.HasValue)
                this.vista.EstablecerPaqueteNavegacion("UnidadExpedienteBO", new UnidadBO { UnidadID = unidadID });

            this.vista.RedirigirAExpediente();
        }

        private void RegistrarMovimiento()
        {
            //Se obtiene la información del contrato a partir de la vista
            var bo = (UnidadBO)this.InterfazUsuarioADato();

            //Se crea el objeto de seguridad
            UsuarioBO usuario = new UsuarioBO() { Id = this.vista.UsuarioID };
            AdscripcionBO adscripcion = new AdscripcionBO() { UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaID } };
            
            SeguridadBO seguridadBO = new SeguridadBO(Guid.Empty, usuario, adscripcion);

            //Se actualiza en la base de datos
            var msj = new StringBuilder(string.Format("Sucursal {0}, Empresa {1}. {2} ", this.vista.SucursalNombre, this.vista.NombreEmpresa, Environment.NewLine));
            msj.Append(this.vista.Observaciones);

            this.controlador.RealizarAltaUnidadFlota(this.dctx, bo, (UnidadBO)this.vista.UltimoObjeto, msj.ToString(), seguridadBO);
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
                if (!this.ExisteAccion(acciones, "REALIZARALTAUNIDADFLOTA"))
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

        public void IrAExpediente()
        {
            this.vista.LimpiarSesion();
            this.vista.LimpiarPaqueteNavegacion("UnidadExpedienteBO");
            var unidadID = this.vista.UnidadID;
            if (unidadID.HasValue)
                this.vista.EstablecerPaqueteNavegacion("UnidadExpedienteBO", new UnidadBO { UnidadID = unidadID });
            this.vista.RedirigirAExpediente();
        }
        #endregion
    }
}