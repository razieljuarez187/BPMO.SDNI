//Satisface al CU081 - Consultar Seguimiento Flota
//Satisface al CU074 - Consultar Expediente de Unidades
//Satisface la solicitud de cambio SC0006

using System;
using System.Collections.Generic;
using BPMO.Basicos.BO;
using BPMO.Facade.SDNI.BR;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Comun.BR;
using BPMO.SDNI.Contratos.BO;
using BPMO.SDNI.Contratos.FSL.BO;
using BPMO.SDNI.Contratos.Mantto.BO;
using BPMO.SDNI.Contratos.RD.BO;
using BPMO.SDNI.Equipos.BOF;
using BPMO.SDNI.Flota.BOF;
using BPMO.SDNI.Flota.BR;
using BPMO.SDNI.Flota.VIS;
using BPMO.SDNI.Tramites.BO;
using BPMO.Servicio.Catalogos.BO;
using UnidadBO = BPMO.SDNI.Equipos.BO.UnidadBO;

namespace BPMO.SDNI.Flota.PRE
{
    public class DetalleExpedienteUnidadPRE
    {
        #region Atributos

        /// <summary>
        /// Vista sobre la que actua el presentador
        /// </summary>
        private readonly IDetalleExpedienteUnidadVIS vista;

        /// <summary>
        /// El DataContext que proveerá acceso a la base de datos
        /// </summary>
        private readonly IDataContext dctx;

        /// <summary>
        /// Nombre de la clase para agregar a los mensajes de Error
        /// </summary>
        private string nombreClase = "DetalleExpedienteUnidadPRE";

        private SeguimientoFlotaBR controlador;

        #endregion

        #region Constructores

        public DetalleExpedienteUnidadPRE(IDetalleExpedienteUnidadVIS view)
        {
            try
            {
                this.vista = view;

                this.controlador = new SeguimientoFlotaBR();
                this.dctx = FacadeBR.ObtenerConexion();
            }
            catch (Exception ex)
            {
                this.vista.MostrarMensaje("Inconsistencia en los parámetros de configuración", ETipoMensajeIU.ERROR, this.nombreClase + ".DetalleExpedienteUnidadPRE:" + ex.Message);
            }
        }

        #endregion

        #region Método
        public void RealizarPrimeraCarga()
        {
            try
            {
                this.PrepararVisualizacion();

                this.EstablecerDatosNavegacion(this.vista.ObtenerPaqueteNavegacion("UnidadExpedienteBO"));
                this.vista.PermitirRegresar(this.vista.ObtenerPaqueteNavegacion("FiltrosSeguimientoFlota") != null);
                this.EstablecerInformacionInicial();

                this.ConsultarCompleto();

                this.EstablecerSeguridad();
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".RealizarPrimeraCarga: " + ex.Message);
            }
        }
        private void EstablecerDatosNavegacion(object paqueteNavegacion)
        {
            try
            {
                if (paqueteNavegacion == null)
                    throw new Exception("Se esperaba un objeto en la navegación. No se puede identificar qué Contrato se desea consultar.");
                if (!(paqueteNavegacion is UnidadBO))
                    throw new Exception("Se esperaba una unidad.");

                UnidadBO bo = new UnidadBO { UnidadID = ((UnidadBO)paqueteNavegacion).UnidadID };

                this.DatoAInterfazUsuario(bo);
            }
            catch (Exception ex)
            {
                this.DatoAInterfazUsuario(new UnidadBO());
                throw new Exception(this.nombreClase + ".EstablecerDatosNavegacion: " + ex.Message);
            }
        }
        private void EstablecerInformacionInicial()
        {
            try
            {
                #region Configuraciones de Unidad Operativa
                //Obtener las configuraciones de la unidad operativa
                List<ConfiguracionUnidadOperativaBO> lstConfigUO = new ModuloBR().ConsultarConfiguracionUnidadOperativa(this.dctx, new ConfiguracionUnidadOperativaBO() { UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaID } }, this.vista.ModuloID);
                if (lstConfigUO.Count <= 0)
                    throw new Exception("No se encontraron las configuraciones del sistema para la unidad operativa en la que trabaja.");

                //Establecer las configuraciones de la unidad operativa
                this.vista.TiempoUsoActivos = lstConfigUO[0].TiempoUsoActivos;
                #endregion
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".EstablecerInformacionInicial: " + ex.Message);
            }
        }
        private void ConsultarCompleto()
        {
            try
            {
                //Se consulta la información del contrato
                FlotaBOF bo = (FlotaBOF)this.InterfazUsuarioADato();

                List<ElementoFlotaBOF> lst = this.controlador.ConsultarSeguimientoFlotaCompleto(this.dctx, bo, true, true);

                if (lst.Count < 1)
                    throw new Exception("No se encontró ningún registro que corresponda a la información proporcionada.");
                if (lst.Count > 1)
                    throw new Exception("La consulta devolvió más de un registro.");

                this.DatoAInterfazUsuario(lst[0]);
            }
            catch (Exception ex)
            {
                this.DatoAInterfazUsuario(new ElementoFlotaBOF());
                throw new Exception(this.nombreClase + ".ConsultarCompleto:" + ex.Message);
            }
        }

        private void ConfiguracionOpciones()
        {
            this.vista.PermitirConsultarContrato(this.vista.ContratoID != null);
            this.vista.PermitirConsultarSeguro(this.vista.SeguroID != null);

            this.vista.PermitirConsultarActaNacimiento(this.vista.UnidadID != null);
            this.vista.PermitirConsultarHistorial(this.vista.UnidadID != null);
            this.vista.PermitirConsultarSeguro(this.vista.UnidadID != null);
            this.vista.PermitirConsultarTramites(this.vista.UnidadID != null);

            this.vista.PermitirRealizarAltaFlota(this.vista.EstatusID != null && this.vista.EstatusID == (int)EEstatusUnidad.Terminada);

            #region SC0006 - Adición de validaciones para el estatus de siniestro
            #region Agregar validaciones para los tipos de unidad y cuando debe permitir la baja de unidad
            bool lPermitir = false;

            //RQM 14608 ERS3 - RF10
            //Agregar validación para que cuando sea generación y construcción permita dar de baja cuando se encuentre en los estatus: Dispobible, En Venta o Siniestro.
            switch (this.vista.UnidadOperativaID)
            {
                case (int)ETipoEmpresa.Generacion:
                case (int)ETipoEmpresa.Equinova:
                case (int)ETipoEmpresa.Construccion:
                    lPermitir = this.vista.EstatusID != null && (this.vista.EstatusID == (int)EEstatusUnidad.Disponible || this.vista.EstatusID == (int)EEstatusUnidad.EnVenta || this.vista.EstatusID == (int)EEstatusUnidad.Siniestro);
                    break;
                default:
                    lPermitir = this.vista.EstatusID != null && (this.vista.EstatusID == (int)EEstatusUnidad.Disponible || this.vista.EstatusID == (int)EEstatusUnidad.Seminuevo || this.vista.EstatusID == (int)EEstatusUnidad.Siniestro);
                    break;
            }
            
            this.vista.PermitirRealizarBajaFlota(lPermitir);
            #endregion
            this.vista.PermitirRealizarReactivacionFlota(this.vista.EstatusID != null && (this.vista.EstatusID == (int)EEstatusUnidad.Baja || this.vista.EstatusID == (int)EEstatusUnidad.Siniestro));
            #endregion

            #region Se agrego durante estaffing permitri quitar aliados estatus taller
            this.vista.PermitirRealizarCambioAsignacionEquiposAliados(this.vista.EstatusID != null && (this.vista.EstatusID == (int)EEstatusUnidad.Disponible || this.vista.EstatusID == (int)EEstatusUnidad.Seminuevo) || this.vista.EstatusID == (int)EEstatusUnidad.EnTaller);
            #endregion         
            this.vista.PermitirRealizarCambioDepartamentoUnidad(this.vista.EstatusID != null && (this.vista.EstatusID == (int)EEstatusUnidad.Disponible || this.vista.EstatusID == (int)EEstatusUnidad.Seminuevo));            
            this.vista.PermitirRealizarCambioSucursalUnidad(this.vista.EstatusID != null && (this.vista.EstatusID == (int)EEstatusUnidad.Disponible || this.vista.EstatusID == (int)EEstatusUnidad.Seminuevo));
        }

        public void ValidarAcceso()
        {
            try
            {
                //se valida que los datos del usuario y la unidad operativa no sean nulos
                if (this.vista.UsuarioID == null) throw new Exception("El identificador del usuario no debe ser nulo");
                if (this.vista.UnidadOperativaID == null) throw new Exception("El identificador de la Unidad Operativa no debe ser nulo ");

                //Se crea el objeto de seguridad
                UsuarioBO usuario = new UsuarioBO() { Id = this.vista.UsuarioID };
                AdscripcionBO adscripcion = new AdscripcionBO() { UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaID } };
                SeguridadBO seguridadBO = new SeguridadBO(Guid.Empty, usuario, adscripcion);

                //Se valida si el usuario tiene permiso a la acción principal
                if (!FacadeBR.ExisteAccion(dctx, "CONSULTAR", seguridadBO))
                    this.vista.RedirigirSinPermisoAcceso();
            }
            catch (Exception ex)
            {
                throw new Exception(nombreClase + ".ValidarAcceso:" + ex.Message);
            }
        }
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
                List<CatalogoBaseBO> lst = FacadeBR.ConsultarAccion(dctx, seguridad);

                //Se valida si el usuario tiene permiso para ver el detalle del acta de nacimiento
                if (!this.ExisteAccion(lst, "UI CONSULTARUNIDAD"))
                    this.vista.PermitirConsultarActaNacimiento(false);
                //Se valida si el usuario tiene permiso para ver el detalle del contrato
                if (!this.ExisteAccion(lst, "UI CONSULTARCONTRATO"))
                    this.vista.PermitirConsultarContrato(false);
                //Se valida si el usuario tiene permiso para ver el detalle del seguro
                if (!this.ExisteAccion(lst, "UI CONSULTARSEGURO"))
                    this.vista.PermitirConsultarSeguro(false);
                //Se valida si el usuario tiene permiso para ver el detalle de los trámites
                if (!this.ExisteAccion(lst, "UI CONSULTARTRAMITES"))
                    this.vista.PermitirConsultarTramites(false);
                //Se valida si el usuario tiene permiso para ver el historial de la unidad
                if (!this.ExisteAccion(lst, "UI CONSULTARHISTORIAL"))
                    this.vista.PermitirConsultarHistorial(false);
                //Se valida si el usuario tiene permiso para dar de alta una unidad en la flota
                if (!this.ExisteAccion(lst, "UI REALIZARALTAUNIDADFLOTA"))
                    this.vista.PermitirRealizarAltaFlota(false);
                //Se valida si el usuario tiene permiso para dar de baja una unidad en la flota
                if (!this.ExisteAccion(lst, "UI REALIZARBAJAUNIDADFLOTA"))
                    this.vista.PermitirRealizarBajaFlota(false);
                //Se valida si el usuario tiene permiso para reactivar una unidad en la flota
                if (!this.ExisteAccion(lst, "UI REALIZARREACTIVACIONUNIDADFLOTA"))
                    this.vista.PermitirRealizarReactivacionFlota(false);
                //Se valida si el usuario tiene permiso para cambiar de sucursal una unidad
                if (!this.ExisteAccion(lst, "UI ACTUALIZARUNIDADSUCURSAL"))
                    this.vista.PermitirRealizarCambioSucursalUnidad(false);
                //Se valida si el usuario tiene permiso para cambiar de departamento una unidad
                if (!this.ExisteAccion(lst, "UI ACTUALIZARUNIDADDEPARTAMENTO"))
                    this.vista.PermitirRealizarCambioDepartamentoUnidad(false);
                //Se valida si el usuario tiene permiso para cambiar de equipos aliados a una unidad
                if (!this.ExisteAccion(lst, "UI ACTUALIZARASIGNACIONEQUIPOSALIADOS"))
                    this.vista.PermitirRealizarCambioAsignacionEquiposAliados(false);
                //Se valida si el usuario tiene permiso para cambiar de sucursal los equipos aliados
                if (!this.ExisteAccion(lst, "UI ACTUALIZAREQUIPOALIADOSUCURSAL"))
                    this.vista.PermitirRealizarCambioSucursalEquipoAliado(false);
            }
            catch (Exception ex)
            {

                throw new Exception(this.nombreClase + ".EstablecerSeguridad:" + ex.Message);
            }
        }
        private bool ExisteAccion(List<CatalogoBaseBO> acciones, string nombreAccion)
        {
            if (acciones != null && acciones.Count > 0 &&
                acciones.Exists(
                    p => p != null && p.Nombre != null && p.Nombre.Trim().ToUpper() == nombreAccion.Trim().ToUpper()))
                return true;

            return false;
        }

        private void PrepararVisualizacion()
        {
            this.vista.PrepararVisualizacion();
        }

        private object InterfazUsuarioADato()
        {
            FlotaBOF bof = new FlotaBOF();
            bof.Unidad = new UnidadBO();
            bof.Unidad.Sucursal = new SucursalBO();
            bof.Unidad.Sucursal.UnidadOperativa = new UnidadOperativaBO();

            bof.Unidad.UnidadID = this.vista.UnidadID;
            bof.Unidad.EquipoID = this.vista.EquipoID;
            bof.Unidad.Sucursal.UnidadOperativa.Id = this.vista.UnidadOperativaID;

            return bof;
        }
        private void DatoAInterfazUsuario(object obj)
        {
            if (obj is ElementoFlotaBOF)
            {
                ElementoFlotaBOF elemento = (ElementoFlotaBOF)obj;
                if (elemento == null) elemento = new ElementoFlotaBOF();
                if (elemento.Unidad == null) elemento.Unidad = new UnidadBO();
                if (elemento.Contrato == null) elemento.Contrato = new ContratoProxyBO();
                if (elemento.Contrato.Cliente == null) elemento.Contrato.Cliente = new CuentaClienteIdealeaseBO();
                if (elemento.Tramites == null) elemento.Tramites = new List<TramiteBO>();

                //Información de la unidad
                this.DatoAInterfazUsuario(elemento.Unidad);
                this.vista.AreaNombre = elemento.AreaText;
                this.vista.EstatusNombre = elemento.EstatusText;
                this.vista.EstaDisponible = elemento.EstaDisponible;
                this.vista.EstaEnContrato = elemento.EstaEnRenta;
                this.vista.TieneEquipoAliado = elemento.TieneEquipoAliado;

                //Información de trámites
                PlacaEstatalBO tPlacaEstatal = (PlacaEstatalBO)elemento.Tramites.Find(p => p.Tipo != null && p.Tipo == ETipoTramite.PLACA_ESTATAL);
                PlacaFederalBO tPlacaFederal = (PlacaFederalBO)elemento.Tramites.Find(p => p.Tipo != null && p.Tipo == ETipoTramite.PLACA_FEDERAL);
                SeguroBO tSeguro = (SeguroBO)elemento.Tramites.Find(p => p.Tipo != null && p.Tipo == ETipoTramite.SEGURO);
                if (tSeguro == null) tSeguro = new SeguroBO();

                if (tPlacaFederal != null)
                {
                    this.vista.NumeroPlaca = tPlacaFederal.Numero;
                    this.vista.TipoPlaca = "FEDERAL";
                }
                else
                {
                    if (tPlacaEstatal != null)
                    {
                        this.vista.NumeroPlaca = tPlacaEstatal.Numero;
                        this.vista.TipoPlaca = "ESTATAL";
                    }
                    else
                    {
                        this.vista.NumeroPlaca = null;
                        this.vista.TipoPlaca = null;
                    }
                }
                this.vista.SeguroID = tSeguro.TramiteID;
                this.vista.FechaVigenciaSeguroInicial = tSeguro.VigenciaInicial;
                this.vista.FechaVigenciaSeguroFinal = tSeguro.VigenciaFinal;
                this.vista.Aseguradora = tSeguro.Aseguradora;
                this.vista.NumeroPoliza = tSeguro.NumeroPoliza;

                //Información del contrato
                this.vista.ContratoID = elemento.Contrato.ContratoID;
                this.vista.NumeroContrato = elemento.Contrato.NumeroContrato;
                if (elemento.Contrato.Tipo != null)
                    this.vista.TipoContratoID = (int)elemento.Contrato.Tipo;
                else
                    this.vista.TipoContratoID = null;
                this.vista.CuentaClienteNombre = elemento.Contrato.Cliente.Nombre;
                this.vista.FechaInicioContrato = elemento.FechaInicioContrato;
                this.vista.FechaVencimientoContrato = elemento.FechaVencimientoContrato;
                if (elemento.FechaInicioContrato != null & elemento.FechaVencimientoContrato != null)
                    this.vista.MesesFaltantesContrato = ((elemento.FechaVencimientoContrato.Value.Year - elemento.FechaInicioContrato.Value.Year) * 12) + elemento.FechaVencimientoContrato.Value.Month - elemento.FechaInicioContrato.Value.Month;
                else
                    this.vista.MesesFaltantesContrato = null;

                this.ConfiguracionOpciones();
            }
            if (obj is UnidadBO)
            {
                UnidadBO unidad = (UnidadBO)obj;
                if (unidad == null) unidad = new UnidadBO();
                if (unidad.ActivoFijo == null) unidad.ActivoFijo = new ActivoFijoBO();
                if (unidad.Sucursal == null) unidad.Sucursal = new SucursalBO();
                if (unidad.TipoEquipoServicio == null) unidad.TipoEquipoServicio = new TipoUnidadBO();
                if (unidad.Modelo == null) unidad.Modelo = new ModeloBO();
                if (unidad.Modelo.Marca == null) unidad.Modelo.Marca = new MarcaBO();

                this.vista.UnidadID = unidad.UnidadID;
                this.vista.EquipoID = unidad.EquipoID;
                this.vista.IDLider = unidad.IDLider;
                this.vista.ClaveActivoOracle = unidad.ClaveActivoOracle;
                this.vista.NumeroSerie = unidad.NumeroSerie;
                this.vista.NumeroEconomico = unidad.NumeroEconomico;
                this.vista.Anio = unidad.Anio;
                if (unidad.Area != null)
                    this.vista.AreaID = Convert.ToInt32(unidad.Area);
                else
                    this.vista.AreaID = null;
                if (unidad.EstatusActual != null)
                    this.vista.EstatusID = (int)unidad.EstatusActual;
                else
                    this.vista.EstatusID = null;

                this.vista.FechaCompra = unidad.ActivoFijo.FechaFacturaCompra;
                this.vista.MontoFactura = unidad.ActivoFijo.CostoSinIva;
                if (this.vista.TiempoUsoActivos != null && unidad.ActivoFijo.FechaFacturaCompra != null && unidad.Area != null && (EArea)unidad.Area == EArea.RD)
                    this.vista.FechaSustitucion = unidad.ActivoFijo.FechaFacturaCompra.Value.AddMonths(this.vista.TiempoUsoActivos.Value);
                else
                    this.vista.FechaSustitucion = null;
                //Valores que en un futuro se obtendrán de Oracle pero que por ahora no tenemos
                this.vista.FolioFactura = null;
                this.vista.ValorLibros = unidad.ActivoFijo.ImporteResidual;
                this.vista.ResidualMonto = unidad.ActivoFijo.ImporteResidual;
                this.vista.ResidualPorcentaje = unidad.ActivoFijo.PorcentajeResidual;
                this.vista.DepreciacionMensualMonto = unidad.ActivoFijo.ImporteDepreciacion;
                this.vista.DepreciacionMensualPorcentaje = unidad.ActivoFijo.PorcentajeDepreciacion;
                this.vista.MesesVidaUtilTotal = null;
                this.vista.MesesVidaUtilRestante = null;

                this.vista.ModeloNombre = unidad.Modelo.Nombre;
                this.vista.TipoUnidadNombre = unidad.TipoEquipoServicio.Nombre;
                this.vista.SucursalNombre = unidad.Sucursal.Nombre;

                this.vista.Llantas = unidad.Llantas;
                this.vista.EquiposAliados = unidad.EquiposAliados;
            }
        }

        public void RealizarAltaFlota()
        {
            try
            {
                UnidadBO bo = new UnidadBO() { UnidadID = this.vista.UnidadID };

                this.LimpiarSesion();
                this.vista.EstablecerPaqueteNavegacion("UnidadExpedienteBO", bo);

                this.vista.RedirigirAAltaFlota();
            }
            catch (Exception ex)
            {
                throw new Exception(nombreClase + ".RealizarAltaFlota: " + ex.Message);
            }
        }
        public void RealizarBajaFlota()
        {
            try
            {
                UnidadBO bo = new UnidadBO() { UnidadID = this.vista.UnidadID };

                this.LimpiarSesion();
                this.vista.EstablecerPaqueteNavegacion("UnidadExpedienteBO", bo);

                this.vista.RedirigirABajaFlota();
            }
            catch (Exception ex)
            {
                throw new Exception(nombreClase + ".RealizarBajaFlota: " + ex.Message);
            }
        }
        public void RealizarReactivacionFlota()
        {
            try
            {
                UnidadBO bo = new UnidadBO() { UnidadID = this.vista.UnidadID };

                this.LimpiarSesion();
                this.vista.EstablecerPaqueteNavegacion("UnidadExpedienteBO", bo);

                this.vista.RedirigirAReactivacionFlota();
            }
            catch (Exception ex)
            {
                throw new Exception(nombreClase + ".RealizarReactivacionFlota: " + ex.Message);
            }
        }
        public void RealizarCambioSucursalUnidad()
        {
            try
            {
                UnidadBO bo = new UnidadBO() { UnidadID = this.vista.UnidadID };

                this.LimpiarSesion();
                this.vista.EstablecerPaqueteNavegacion("UnidadExpedienteBO", bo);

                this.vista.RedirigirACambioSucursalUnidad();
            }
            catch (Exception ex)
            {
                throw new Exception(nombreClase + ".RealizarCambioSucursalUnidad: " + ex.Message);
            }
        }
        public void RealizarCambioDepartamentoUnidad()
        {
            try
            {
                UnidadBO bo = new UnidadBO() { UnidadID = this.vista.UnidadID };

                this.LimpiarSesion();
                this.vista.EstablecerPaqueteNavegacion("UnidadExpedienteBO", bo);

                this.vista.RedirigirACambioDepartamentoUnidad();
            }
            catch (Exception ex)
            {
                throw new Exception(nombreClase + ".RealizarCambioDepartamentoUnidad: " + ex.Message);
            }
        }
        public void RealizarCambioAsignacionEquiposAliados()
        {
            try
            {
                UnidadBO bo = new UnidadBO() { UnidadID = this.vista.UnidadID };

                this.LimpiarSesion();
                this.vista.EstablecerPaqueteNavegacion("UnidadExpedienteBO", bo);

                this.vista.RedirigirACambioAsignacionEquiposAliados();
            }
            catch (Exception ex)
            {
                throw new Exception(nombreClase + ".RealizarCambioAsignacionEquiposAliados: " + ex.Message);
            }
        }
        public void ConsultarActaNacimiento()
        {
            try
            {
                UnidadBO bo = new UnidadBO() { UnidadID = this.vista.UnidadID };

                this.LimpiarSesion();
                this.vista.EstablecerPaqueteNavegacion("UnidadBO", bo);

                this.vista.RedirigirADetalleActaNacimiento();
            }
            catch (Exception ex)
            {
                throw new Exception(nombreClase + ".ConsultarActaNacimiento: " + ex.Message);
            }
        }
        public void ConsultarTramites()
        {
            try
            {
                UnidadBO bo = new UnidadBO() { UnidadID = this.vista.UnidadID };

                this.LimpiarSesion();
                this.vista.EstablecerPaqueteNavegacion("DatosTramitable", bo);

                this.vista.RedirigirADetalleTramites();
            }
            catch (Exception ex)
            {
                throw new Exception(nombreClase + ".ConsultarTramites: " + ex.Message);
            }
        }
        public void ConsultarSeguro()
        {
            try
            {
                if (this.vista != null && this.vista.SeguroID != null) {
                    SeguroBO bo = new SeguroBO() { TramiteID = this.vista.SeguroID };

                    this.LimpiarSesion();
                    this.vista.EstablecerPaqueteNavegacion("TRAMITESEGURO", bo);

                    this.vista.RedirigirADetalleSeguro();
                } else {
                    this.vista.MostrarMensaje("La Unidad no cuenta con un Seguro Configurado", ETipoMensajeIU.ADVERTENCIA, null);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(nombreClase + ".ConsultarSeguro: " + ex.Message);
            }
        }
        public void ConsultarContrato()
        {
            try
            {
                if (this.vista.TipoContratoID == null)
                    throw new Exception("No se identificó el tipo de contrato que desea ver");

                ETipoContrato tipo = (ETipoContrato)Enum.Parse(typeof(ETipoContrato), this.vista.TipoContratoID.ToString());
                switch (tipo)
                {
                    case ETipoContrato.CM:
                        #region Ir a CM
                        ContratoManttoBO boCM = new ContratoManttoBO() { ContratoID = this.vista.ContratoID };

                        this.LimpiarSesion();
                        this.vista.EstablecerPaqueteNavegacion("ContratoManttoBO", boCM);

                        this.vista.RedirigirADetalleContratoCM();
                        #endregion
                        break;
                    case ETipoContrato.FSL:
                        #region Ir a FSL
                        ContratoFSLBO boFSL = new ContratoFSLBO() { ContratoID = this.vista.ContratoID };

                        this.LimpiarSesion();
                        this.vista.EstablecerPaqueteNavegacion("ContratoFSLBO", boFSL);

                        this.vista.RedirigirADetalleContratoFSL();
                        #endregion
                        break;
                    case ETipoContrato.RD:
                        #region Ir a RD
                        ContratoRDBO boRD = new ContratoRDBO() { ContratoID = this.vista.ContratoID };

                        this.LimpiarSesion();
                        this.vista.EstablecerPaqueteNavegacion("ContratoRDBO", boRD);

                        this.vista.RedirigirADetalleContratoRD();
                        #endregion
                        break;
                    case ETipoContrato.SD:
                        #region Ir a SD
                        ContratoManttoBO boSD = new ContratoManttoBO() { ContratoID = this.vista.ContratoID };

                        this.LimpiarSesion();
                        this.vista.EstablecerPaqueteNavegacion("ContratoManttoBO", boSD);

                        this.vista.RedirigirADetalleContratoSD();
                        #endregion
                        break;
                    default:
                        throw new Exception("No se identificó el tipo de contrato que desea ver");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".ConsultarContrato: " + ex.Message);
            }
        }

        public void Regresar()
        {
            this.LimpiarSesion();
            this.vista.LimpiarPaqueteNavegacion("UnidadExpedienteBO");
            this.vista.RedirigirAConsulta();
        }

        private void LimpiarSesion()
        {
            this.vista.LimpiarSesion();
        }

        #region Métodos para el Buscador
        public object PrepararBOVisor(string catalogo)
        {
            object obj = null;

            switch (catalogo)
            {
                case "HistorialUnidad":
                    HistorialBOF historial = new HistorialBOF();
                    historial.Unidad = new UnidadBO();
                    historial.Unidad.UnidadID = this.vista.UnidadID;

                    obj = historial;
                    break;
            }

            return obj;
        }
        #endregion
        #endregion
    }
}
