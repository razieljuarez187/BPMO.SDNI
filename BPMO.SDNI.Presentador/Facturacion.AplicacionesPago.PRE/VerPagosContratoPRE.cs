// Satisface a la Solicitud de Cambios SC0008
// Satisface al caso de uso CU038 Ver Pagos de Contrato

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using BPMO.Basicos.BO;
using BPMO.Facade.SDNI.BR;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Contratos.BO;
using BPMO.SDNI.Contratos.BR;
using BPMO.SDNI.Facturacion.AplicacionesPago.VIS;
using BPMO.SDNI.Facturacion.InfraestructuraPagos.BO;
using BPMO.SDNI.Facturacion.InfraestructuraPagos.BOF;
using BPMO.SDNI.Facturacion.InfraestructuraPagos.BR;

namespace BPMO.SDNI.Facturacion.AplicacionesPago.PRE
{
    /// <summary>
    /// Clase Presentador para la Vista de Ver Pagos de Contrato
    /// </summary>
    public class VerPagosContratoPRE
    {
        #region Atributos
        /// <summary>
        /// Codigo del Paquete de Navegación
        /// </summary>
        private const string codigoImprimirHistorico = "BEP1401.CU006";
        /// <summary>
        /// Nombre de la Clase
        /// </summary>
        private const string nombreClase = "VerPagosContratoPRE";
        /// <summary>
        /// Vista del Presentador
        /// </summary>
        private readonly IVerPagosContratoVIS Vista;
        /// <summary>
        /// Contexto de Conexión a Datos
        /// </summary>
        private readonly IDataContext dataContext;

        #endregion

        #region Constructores
        /// <summary>
        /// Contructor del Presentador
        /// </summary>
        /// <param name="vista"></param>
        public VerPagosContratoPRE(IVerPagosContratoVIS vista)
        {
            if(vista == null) throw new ArgumentNullException("vista","La vista proporcionada no puede ser nulo");
            Vista = vista;

            dataContext = FacadeBR.ObtenerConexion();
        }
        #endregion

        #region Metodos
        /// <summary>
        /// Realiza la impresión del Historico de Pagos para una referencia de Contrato Seleccionada
        /// </summary>
        public void ImprimirPagos()
        {
            try
            {
                var numeroContrato = Vista.NumeroContrato;

                // Validar la Informacion de la Referencia de Contrato Seleccionada
                var msg = ValidarDatos(numeroContrato);

                if (!string.IsNullOrEmpty(msg))
                {
                    Vista.MostrarMensaje(msg, ETipoMensajeIU.ADVERTENCIA);
                    return;
                }


                // Validar la existencia de folio del contrato
                var contratoBR = new ContratoBR();
                var contratos = contratoBR.Consultar(dataContext, new ContratoProxyBO{ NumeroContrato = numeroContrato.ToUpper()});
                if (!contratos.Any())
                {
                    Vista.MostrarMensaje("No existe un Contrato con el Folio proporcionado.", ETipoMensajeIU.ADVERTENCIA);
                    return;
                }

                var referencia = contratos.First();
                var usuario = new UsuarioBO { Id = Vista.UsuarioId };
                var unidadOperativa = new UnidadOperativaBO { Id = Vista.UnidadOperativaId};
                var sucursales = ConsultarSucursales(usuario, unidadOperativa);
                
                // Validar que el contrato pertenezca a una sucursal del usuario
                if(sucursales.All(x => x.Id != referencia.Sucursal.Id))
                {
                    Vista.MostrarMensaje("El Folio del Contrato proporcionado pertence a una sucursal no asignada al usuario.", ETipoMensajeIU.ADVERTENCIA);
                    return;
                }
                
                // Validar la existencia de Pagos del Contrato
                int conteo = 0;
                ReferenciaContratoBO referenciaContrato = new ReferenciaContratoBO { ReferenciaContratoID = referencia.ContratoID, UnidadOperativa = unidadOperativa };

                switch (referencia.Tipo) { 
                    case ETipoContrato.RO:
                    case ETipoContrato.RE:
                    case ETipoContrato.ROC:
                        PagoContratoPSLBOF pagoPsl = new PagoContratoPSLBOF() { ReferenciaContrato = referenciaContrato, Sucursales = sucursales };
                        PagoContratoPSLBR pagosPslBR = new PagoContratoPSLBR();
                        conteo = pagosPslBR.ContarPagos(dataContext, pagoPsl);
                        break;
                    default:
                        PagoUnidadContratoBOF pago = new PagoUnidadContratoBOF { ReferenciaContrato = referenciaContrato, Sucursales = sucursales };
                        PagoUnidadContratoBR pagosBR = new PagoUnidadContratoBR();
                        conteo = pagosBR.ContarPagos(dataContext, pago);
                        break;
                }

                if (conteo == 0)
                {
                    Vista.MostrarMensaje("El contrato no cuenta con pagos generados.", ETipoMensajeIU.ADVERTENCIA);
                    return;
                }

                //Obtener Datos de Reporte
                var adscripcion = new AdscripcionBO { UnidadOperativa = unidadOperativa };
                var seguridad = new SeguridadBO(Guid.Empty, usuario, adscripcion);
                var imprmirPagosBR = new ImprimirPagosBR();
                var datosReporte = imprmirPagosBR.ObtenerDatosHistoricoPagos(dataContext,
                    referenciaContrato, unidadOperativa, seguridad, this.Vista.URLLogoEmpresa);

                Vista.EstablecerPaqueteNavegacionImprimir(codigoImprimirHistorico, datosReporte);
                Vista.IrAImprimirHistorico();
            }
            catch (Exception ex)
            {
                var strMetodo = new StackFrame().GetMethod().Name;
                var strMsg = string.Format("{0}.{1}: {2}", nombreClase, strMetodo, ex.Message);
                Vista.MostrarMensaje("Inconsistencias al Desplegar los Pagos del Contrato", ETipoMensajeIU.ERROR, strMsg);
            }
        }
        /// <summary>
        /// Realiza la Validacion de los Datos de la Referencia de Contrato
        /// </summary>
        /// <param name="numeroContrato">Contrato que contiene la información a Validar</param>
        /// <returns></returns>
        private string ValidarDatos(string numeroContrato)
        {
            if (numeroContrato == null) return "Proporcione una Referencia de Contrato";

            if (string.IsNullOrEmpty(numeroContrato.Trim())) return "Proporcione el Folio del contrato";  
                  
            var regex = new Regex(ConfigurationManager.AppSettings["FormatoNumeroContrato"]);


            if (!regex.IsMatch(numeroContrato))
                return "El formato del Folio de Contrato proporcioando es invalido";
            return string.Empty;
        }
        /// <summary>
        /// Consulta las sucursales asignadas al usuario por unidad operativa
        /// </summary>
        /// <param name="usuario">Usuario al que estan asignadas las sucursales</param>
        /// <param name="unidadOperativa">Unidad Operativa a la cual pertenecen las sucursales</param>
        /// <returns></returns>
        private List<SucursalBO> ConsultarSucursales(UsuarioBO usuario, UnidadOperativaBO unidadOperativa)
        {
            try
            {
                var seguridad = new SeguridadBO(Guid.Empty, usuario, new AdscripcionBO { UnidadOperativa = unidadOperativa });
                List<SucursalBO> resultado = FacadeBR.ConsultarSucursalesSeguridad(dataContext, seguridad) ??
                                              new List<SucursalBO>();

                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception(nombreClase + ".ConsultarSucursales: Inconsistencias al consultar la lista de Sucursales del usuario." + ex.Message);
            }
        }
        /// <summary>
        /// Prepara la Vista para una nueva consulta
        /// </summary>
        public void PrepararBusqueda()
        {
            Vista.ConfigurarValidadorFormato();
            //EstablecerSeguridad();
        }
        /// <summary>
        /// Establecer las reglas de seguridad que se tendran para los controles y navegación del visor
        /// </summary>
        private void EstablecerSeguridad()
        {
            try
            {
                //se valida que los datos del usuario y la unidad operativa no sean nulos
                if (Vista.UsuarioId == null)
                {
                    Vista.PermitirVerPagos(false);
                    throw new Exception("El identificador del usuario no debe ser nulo");
                }
                if (Vista.UnidadOperativaId == null)
                {
                    Vista.PermitirVerPagos(false);
                    throw new Exception("El identificador de la Unidad Operativa no debe ser nulo ");
                }

                var usr = new UsuarioBO { Id = Vista.UsuarioId };
                var adscripcion = new AdscripcionBO { UnidadOperativa = new UnidadOperativaBO { Id = Vista.UnidadOperativaId } };
                var seguridadBO = new SeguridadBO(Guid.Empty, usr, adscripcion);

                //Se obtienen las acciones a las cuales el usuario tiene permiso en este proceso
                var lst = FacadeBR.ConsultarAccion(dataContext, seguridadBO);

                //Se valida si el usuario tiene permiso para consultar
                if (!ExisteAccion(lst, "CONSULTAR"))
                    Vista.RedirigirSinPermisoAcceso();
            }
            catch (Exception ex)
            {
                throw new Exception(nombreClase + ".EstablecerSeguridad:" + ex.GetBaseException().Message);
            }
        }
        /// <summary>
        /// Evalua si una acción se cuentra definida
        /// </summary>
        /// <param name="acciones">Lista de acciones donde realizará la búsqueda</param>
        /// <param name="accion">Acción a evaluar</param>
        /// <returns>Devuelve true si la acción está definida, de lo contrario devolverá false</returns>
        private bool ExisteAccion(List<CatalogoBaseBO> acciones, string accion)
        {
            if (acciones != null && acciones.Count > 0 && acciones.Exists(p => p != null && p.Nombre != null && p.Nombre.Trim().ToUpper() == accion.Trim().ToUpper()))
                return true;

            return false;
        }
        #endregion
    }
}
