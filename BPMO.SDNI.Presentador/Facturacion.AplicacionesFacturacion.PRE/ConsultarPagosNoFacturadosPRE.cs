//Satisface al caso de uso CU012 - Ver Pagos No Facturados
// Satisface a la solicitud de cambio SC0015
// Satisface al Reporte de Inconsistencia RI0008
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using BPMO.Basicos.BO;
using BPMO.Facade.SDNI.BR;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Facturacion.AplicacionesFacturacion.VIS;
using BPMO.SDNI.Facturacion.InfraestructuraPagos.BO;
using BPMO.SDNI.Facturacion.InfraestructuraPagos.BOF;
using BPMO.SDNI.Facturacion.InfraestructuraPagos.BR;
using BPMO.SDNI.Facturacion.MonitoreoPagos.BR;

namespace BPMO.SDNI.Facturacion.AplicacionesFacturacion.PRE
{
    /// <summary>
    /// Presentador de Consulta de Pagos No Facturados
    /// </summary>
    public class ConsultarPagosNoFacturadosPRE
    {
        #region Atributos
        /// <summary>
        /// Contexto de conexion de datos
        /// </summary>
        private readonly IDataContext dataContext;
        /// <summary>
        /// Controlador Principal
        /// </summary>
        private readonly PagoUnidadContratoBR Controlador;
        /// <summary>
        /// Controlador Principal PSL
        /// </summary>
        private readonly PagoContratoPSLBR ControladorPSL;
        /// <summary>
        /// Vista de la Consulta
        /// </summary>
        private readonly IConsultarPagosNoFacturadosVIS Vista;
        /// <summary>
        /// Nombre del Permisos de la Consulta
        /// </summary>
        private const string PermisoConsultar = "CONSULTAR";
        /// <summary>
        /// Nombre de Permiso para Mover a Consulta de Pagos por Facturar
        /// </summary>
        private const string PermisoMover = "LiberarPagoCredito";
        /// <summary>
        /// Nombre de la Clase
        /// </summary>
        private const string nombreClase = "ConsultarPagosNoFacturadosPRE";
        /// <summary>
        /// Nombre del Permisos de Poder Cancelar Pagos
        /// </summary>
        private const string PermisoCancelarPagos = "CANCELARPAGOSUI";

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor por defecto de la Consulta
        /// </summary>
        /// <param name="vista">Vista de la Consulta</param>
        public ConsultarPagosNoFacturadosPRE(IConsultarPagosNoFacturadosVIS vista)
        {
            if (vista == null) throw new ArgumentNullException("vista");
            Vista = vista;
            dataContext = FacadeBR.ObtenerConexion();

            Controlador = new PagoUnidadContratoBR();
            ControladorPSL = new PagoContratoPSLBR();
        }
        #endregion

        #region Metodos
        /// <summary>
        /// Primera carga de información de la consulta
        /// </summary>
        public void PrimeraCarga()
        {
            Vista.MarcarPagoNoFacturados();
            EstablecerSeguridad();

            var bof = ObtenerBaseBOF();
            if (bof is PagoUnidadContratoBOF)
            {
                ((PagoUnidadContratoBOF)bof).Sucursales = Vista.SucursalesUsuario;
                Consultar((PagoUnidadContratoBOF)bof);
            }
            else
            {
                ((PagoContratoPSLBOF)bof).Sucursales = Vista.SucursalesUsuario;
                Consultar((PagoContratoPSLBOF)bof);
            }

        }
        /// <summary>
        /// Obtiene el BO Base de Filtros
        /// </summary>
        /// <returns></returns>
        private object ObtenerBaseBOF()
        {
            if (ETipoEmpresa.Idealease == (ETipoEmpresa)this.Vista.UnidadOperativaID)
            {
                return new PagoUnidadContratoBOF
                {
                    ReferenciaContrato = new ReferenciaContratoBO
                    {
                        UnidadOperativa = new UnidadOperativaBO { Id = Vista.UnidadOperativaID },
                        CuentaCliente = new Comun.BO.CuentaClienteIdealeaseBO()
                    },
                    BloqueadoCredito = true,
                    Activo = true,
                    FacturaEnCeros = false
                };
            }
            else
            {
                return new PagoContratoPSLBOF
                {
                    ReferenciaContrato = new ReferenciaContratoBO
                    {
                        UnidadOperativa = new UnidadOperativaBO { Id = Vista.UnidadOperativaID },
                        CuentaCliente = new Comun.BO.CuentaClienteIdealeaseBO()
                    },
                    BloqueadoCredito = true,
                    Activo = true,
                    FacturaEnCeros = false
                };
            }
        }
        /// <summary>
        /// Consultar los pagos en base a filtro proporcionados
        /// </summary>
        /// <param name="bof"></param>
        private void Consultar(PagoUnidadContratoBOF bof)
        {
            var resultado = Controlador.ConsultarFiltroSinCuentas(dataContext, bof);
            Vista.PagosConsultados = resultado;

            Vista.CargarPagosConsultados();
        }
        /// <summary>
        /// Consultar los pagos en base a filtro proporcionados
        /// </summary>
        /// <param name="bof"></param>
        private void Consultar(PagoContratoPSLBOF bof) {
            var resultado = ControladorPSL.ConsultarFiltroSinCuentas(dataContext, bof, this.Vista.ModuloID);
            Vista.PagosConsultadosPSL = resultado;

            Vista.CargarPagosConsultados();
        }
        /// <summary>
        /// Consultar los Pagos
        /// </summary>
        public void Consultar()
        {
            var bof = InterfazUsuarioADato();
            if (bof is PagoUnidadContratoBOF)
            {
                Consultar((PagoUnidadContratoBOF)bof);
            }
            else
            {
                Consultar((PagoContratoPSLBOF)bof);
            }
        }
        /// <summary>
        /// Obtiene los datos de la Vista
        /// </summary>
        /// <returns></returns>
        private object InterfazUsuarioADato()
        {
            var bof = ObtenerBaseBOF();
            if (bof is PagoUnidadContratoBOF)
            {

                ((PagoUnidadContratoBOF)bof).Sucursales = Vista.SucursalSeleccionadaID != null
                    ? new List<SucursalBO> { new SucursalBO { Id = Vista.SucursalSeleccionadaID } }
                    : Vista.SucursalesUsuario;
                ((PagoUnidadContratoBOF)bof).Departamento = Vista.DepartamentoSeleccionado;
                #region SC0035
                ((PagoUnidadContratoBOF)bof).ReferenciaContrato.FolioContrato = Vista.NumeroContrato != null ? Vista.NumeroContrato : null;
                ((PagoUnidadContratoBOF)bof).ReferenciaContrato.CuentaCliente.Id = Vista.CuentaClienteID != null ? Vista.CuentaClienteID : null;
                ((PagoUnidadContratoBOF)bof).ReferenciaContrato.CuentaCliente.Nombre = Vista.NombreCuentaCliente != null ? Vista.NombreCuentaCliente : null;
                ((PagoUnidadContratoBOF)bof).Referencia = Vista.VinNumeroEconomico != null ? Vista.VinNumeroEconomico : null;
                #endregion
                if (Vista.FechaVencimientoInicio != null || Vista.FechaVencimientoFin != null)
                {
                    #region RI0008
                    if (Vista.FechaVencimientoFin != null)
                    {
                        var fechaFinal = new DateTime(Vista.FechaVencimientoFin.Value.Year,
                            Vista.FechaVencimientoFin.Value.Month, Vista.FechaVencimientoFin.Value.Day, 23, 59, 59, 999);
                        ((PagoUnidadContratoBOF)bof).FechaVencimientoFinal = fechaFinal;
                    }

                    if (Vista.FechaVencimientoInicio != null)
                    {
                        ((PagoUnidadContratoBOF)bof).FechaVencimientoInicial = Vista.FechaVencimientoInicio;
                    }
                    #endregion
                }
            }
            else
            {
                ((PagoContratoPSLBOF)bof).Sucursales = Vista.SucursalSeleccionadaID != null
                    ? new List<SucursalBO> { new SucursalBO { Id = Vista.SucursalSeleccionadaID } }
                    : Vista.SucursalesUsuario;
                ((PagoContratoPSLBOF)bof).Departamento = Vista.DepartamentoSeleccionado;
                #region
                ((PagoContratoPSLBOF)bof).ReferenciaContrato.FolioContrato = Vista.NumeroContrato != null ? Vista.NumeroContrato : null;
                ((PagoContratoPSLBOF)bof).ReferenciaContrato.CuentaCliente.Id = Vista.CuentaClienteID != null ? Vista.CuentaClienteID : null;
                ((PagoContratoPSLBOF)bof).ReferenciaContrato.CuentaCliente.Nombre = Vista.NombreCuentaCliente != null ? Vista.NombreCuentaCliente : null;
                ((PagoContratoPSLBOF)bof).ReferenciaFiltro = Vista.VinNumeroEconomico != null ? Vista.VinNumeroEconomico : null;
                #endregion
                if (Vista.FechaVencimientoInicio != null || Vista.FechaVencimientoFin != null)
                {
                    #region
                    if (Vista.FechaVencimientoFin != null)
                    {
                        var fechaFinal = new DateTime(Vista.FechaVencimientoFin.Value.Year,
                            Vista.FechaVencimientoFin.Value.Month, Vista.FechaVencimientoFin.Value.Day, 23, 59, 59, 999);
                        ((PagoContratoPSLBOF)bof).FechaVencimientoFinal = fechaFinal;
                    }

                    if (Vista.FechaVencimientoInicio != null)
                    {
                        ((PagoContratoPSLBOF)bof).FechaVencimientoInicial = Vista.FechaVencimientoInicio;
                    }
                    #endregion
                }
            }

            return bof;
        }
        /// <summary>
        /// Establece las opciones permitidas en base a la seguridad del usuario
        /// </summary>
        private void EstablecerSeguridad()
        {
            try
            {
                //se valida que los datos del usuario y la unidad operativa no sean nulos
                if (Vista.UsuarioID == null) throw new Exception("El identificador del usuario no debe ser nulo");
                if (Vista.UnidadOperativaID == null) throw new Exception("El identificador de la Unidad Operativa no debe ser nulo ");

                var usr = new UsuarioBO { Id = Vista.UsuarioID };
                var adscripcion = new AdscripcionBO { UnidadOperativa = new UnidadOperativaBO { Id = Vista.UnidadOperativaID } };
                var seguridadBO = new SeguridadBO(Guid.Empty, usr, adscripcion);

                //Se obtienen las acciones a las cuales el usuario tiene permiso en este proceso
                List<CatalogoBaseBO> lst = FacadeBR.ConsultarAccion(dataContext, seguridadBO);

                //Se valida si el usuario tiene permiso para consultar
                if (!ExisteAccion(lst, PermisoConsultar))
                    Vista.RedirigirSinPermisoAcceso();

                // Se valida si el usuario tiene permisos para mover el pago
                if (!ExisteAccion(lst, PermisoMover))
                    Vista.PermitirMover(false);

                //Se valida si el usuario tiene permiso para Cancelar la Facturación
                if (!ExisteAccion(lst, PermisoCancelarPagos))
                    Vista.PermitirCancelarPago(false);
            }
            catch (Exception ex)
            {
                throw new Exception(nombreClase + ".EstablecerSeguridad:" + ex.Message);
            }
        }
        /// <summary>
        /// Verifica si existe una acción en una lista de acciones proporcionada
        /// </summary>
        /// <param name="acciones">Lista de Acciones</param>
        /// <param name="accion">Acción a verificar</param>
        /// <returns></returns>
        private bool ExisteAccion(List<CatalogoBaseBO> acciones, string accion)
        {
            if (acciones != null && acciones.Count > 0 && acciones.Exists(p => p != null && p.Nombre != null && p.Nombre.Trim().ToUpper() == accion.Trim().ToUpper()))
                return true;

            return false;
        }
        /// <summary>
        /// Mueve el pago a la Consulta de Pagos Por Facturar
        /// </summary>
        /// <param name="pagoID"></param>
        public void MoverAFacturar(int pagoID)
        {
            object pago = null;
            string mensaje = "No se puede actualizar el pago, el cliente no cuenta con crédito en tipo de moneda ";
            if (ETipoEmpresa.Idealease == (ETipoEmpresa)this.Vista.UnidadOperativaID)
                pago = new PagoUnidadContratoBOF { PagoID = pagoID };
            else
                pago = new PagoContratoPSLBOF { PagoContratoID = pagoID };

            MonitorCreditoClienteBR monitorCreditoBR = new MonitorCreditoClienteBR();
            var usr = new UsuarioBO { Id = Vista.UsuarioID };
            var adscripcion = new AdscripcionBO { UnidadOperativa = new UnidadOperativaBO { Id = Vista.UnidadOperativaID } };
            var seguridadBO = new SeguridadBO(Guid.Empty, usr, adscripcion);
            if (monitorCreditoBR.LiberarPagoCredito(FacadeBR.ObtenerConexion(), ref pago, seguridadBO)) {
                // Si es correcto, se quita el pago de la lista de pagos no-facturados.
                if (pago is PagoUnidadContratoBOF || pago is PagoUnidadContratoBO)
                    this.Vista.PagosConsultados.Remove(Vista.PagosConsultados.First(p => p.PagoID == (pago as PagoUnidadContratoBO).PagoID));
                else if (pago is PagoContratoPSLBO)
                    this.Vista.PagosConsultadosPSL.Remove(Vista.PagosConsultadosPSL.First(p => p.PagoContratoID == (pago as PagoContratoPSLBO).PagoContratoID));

                Vista.ActualizarMarcadoresEnviarAFacturar();
                Vista.CargarPagosConsultados();
            } else {
                if (pago is PagoUnidadContratoBOF || pago is PagoUnidadContratoBO)
                    mensaje += (pago as PagoUnidadContratoBO).Divisa.MonedaDestino.Codigo;
                else if (pago is PagoContratoPSLBO)
                    mensaje += (pago as PagoContratoPSLBO).Divisa.MonedaDestino.Codigo;

                Vista.MostrarMensaje(mensaje, ETipoMensajeIU.INFORMACION);
            }
        }

        #region Métodos para la cancelación de pagos
        public void SolicitarAutorizacion()
        {
            string s;
            if ((s = this.ValidarCamposCancelarPago()) != null)
            {
                this.Vista.MostrarMensaje(s, ETipoMensajeIU.ADVERTENCIA, null);
                this.Vista.PermitirValidarCodigoAutorizacion(false);
                this.Vista.PermitirSolicitarCodigoAutorizacion(true);
                return;
            }

            try
            {
                this.Vista.CancelaPagoCodigoAutorizacion = null;
                this.Vista.CancelaPagoCodigoAutorizacion = this.ControladorPSL.SolicitarAutorizacionCancelarPago(this.dataContext, this.Vista.PagoACancelarID, this.Vista.ModuloID, this.Vista.UnidadOperativaID, this.Vista.UsuarioID, this.Vista.MotivoCancelarPago, this.Vista.Adscripcion.UnidadOperativa.Empresa.Nombre);
                this.Vista.PermitirValidarCodigoAutorizacion(true);
                this.Vista.PermitirSolicitarCodigoAutorizacion(false);
            }
            catch (Exception ex)
            {
                throw new Exception(nombreClase + ".SolicitarAutorizacionTarifaPersonalizada: " + ex.Message);
            }
        }

        private string ValidarCamposCancelarPago()
        {
            string s = string.Empty;

            if (this.Vista.MotivoCancelarPago == null)
                s += "Motivo, ";


            if (s.Trim().CompareTo("") != 0)
                return "Los siguientes campos no pueden estar vacíos: \n" + s.Substring(0, s.Length - 2);

            return null;
        }

        /// <summary>
        /// Valida la confirmación del código con el código enviado en la autorización
        /// </summary>
        /// <param name="confirmacionCodigo">Confirmación del código de autorización</param>
        /// <returns>True en caso de que sea correcto, False en caso contrario</returns>
        public bool ValidarCodigoAutorizacion(string confirmacionCodigo)
        {
            if (confirmacionCodigo.Trim().CompareTo(this.Vista.CancelaPagoCodigoAutorizacion) == 0)
                return true;
            return false;
        }

        /// <summary>
        /// Limpia los campos del dialogo y 
        /// Copia los datos de tarifa de la interfaz de usuario
        /// </summary>
        public void PrepararDialogo()
        {

            this.Vista.CancelaPagoCodigoAutorizacion = null;
            this.Vista.MotivoCancelarPago = null;
            this.Vista.PagoACancelarID = null;

            this.Vista.PermitirValidarCodigoAutorizacion(false);
            this.Vista.PermitirSolicitarCodigoAutorizacion(true);

        }

        /// <summary>
        /// Cancela el pago
        /// </summary>
        public void CancelarPago()
        {
            #region Se inicia la Transaccion
            dataContext.SetCurrentProvider("Outsourcing");
            Guid firma = Guid.NewGuid();
            try
            {
                dataContext.OpenConnection(firma);
                dataContext.BeginTransaction(firma);
            }

            catch (Exception)
            {
                if (dataContext.ConnectionState == ConnectionState.Open)
                    dataContext.CloseConnection(firma);
                throw new Exception("Se encontraron inconsistencias al cancelar el pago.");
            }
            #endregion

            try
            {
                var usr = new UsuarioBO { Id = Vista.UsuarioID };
                var adscripcion = new AdscripcionBO { UnidadOperativa = new UnidadOperativaBO { Id = Vista.UnidadOperativaID } };
                var seguridadBO = new SeguridadBO(Guid.Empty, usr, adscripcion);
                PagoContratoPSLBR pagoBR = new PagoContratoPSLBR();
                pagoBR.CancelarPago(dataContext, this.Vista.PagoACancelarID, seguridadBO);
                dataContext.CommitTransaction(firma);
                this.Vista.MostrarMensaje("Se ha eliminado el pago con éxito", ETipoMensajeIU.EXITO);
                //Se actualiza el grid de pagos
                var pago = this.Vista.PagosConsultadosPSL.Where(x => x.PagoContratoID == this.Vista.PagoACancelarID).FirstOrDefault();
                this.Vista.PagosConsultadosPSL.Remove(pago);
                this.Vista.CargarPagosConsultados();
            }
            catch (Exception ex)
            {
                dataContext.RollbackTransaction(firma);
                throw new Exception(nombreClase + ".CancelarPago:" + ex.Message);
            }
            finally
            {
                if (dataContext.ConnectionState == ConnectionState.Open)
                    dataContext.CloseConnection(firma);
            }

        }
        /// <summary>
        /// Establece el identificador del pago a cancelar
        /// </summary>
        /// <param name="pagoACancelarID">Identificar del pago a cancelar</param>
        public void EstablecerPagoACancelar(int? pagoACancelarID)
        {
            this.Vista.PagoACancelarID = pagoACancelarID;
        }
        #endregion
        #endregion
    }
}
