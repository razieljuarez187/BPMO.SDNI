// Satisface al CU030 - Registrar Terminación de Contrato de Mantenimiento
// Satisface a la Solución del RI0005
//Satisface a la RI0014
// BEP1401 Satisface a la SC0026

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using BPMO.Basicos.BO;
using BPMO.Facade.SDNI.BR;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Contratos.BO;
using BPMO.SDNI.Contratos.Mantto.BO;
using BPMO.SDNI.Contratos.Mantto.BR;
using BPMO.SDNI.Contratos.Mantto.VIS;
using BPMO.SDNI.Facturacion.AplicacionesPago.BR;
using BPMO.SDNI.Facturacion.InfraestructuraPagos.BO;
using BPMO.SDNI.Facturacion.InfraestructuraPagos.BR;
using BPMO.SDNI.Facturacion.InfraestructuraPagos.BOF;

namespace BPMO.SDNI.Contratos.Mantto.PRE
{
    public class CerrarContratoManttoPRE
    {
        #region Atributos
        /// <summary>
        /// Nombre de la clase para agregar a los mensajes de Error
        /// </summary>
        private string nombreClase = "CerrarContratoManttoPRE";

        /// <summary>
        /// El DataContext que proveerá acceso a la base de datos
        /// </summary>
        private readonly IDataContext dctx;

        /// <summary>
        /// Presentador de la Barra de Herramientas
        /// </summary>
        private readonly ucHerramientasManttoPRE presentadorHerramientas;

        /// <summary>
        /// Presentador del control de usuario del contrato
        /// </summary>
        private readonly ucContratoManttoPRE presentadorContrato;

        /// <summary>
        /// Vista sobre la que actua la interfaz
        /// </summary>
        private readonly ICerrarContratoManttoVIS vista;

        private ContratoManttoBR controlador;

        private PagoUnidadContratoBR pagoCtrl;

        private GeneradorPagosManttoBR generadorPagos;
        #endregion

        #region Constructores

        public CerrarContratoManttoPRE(ICerrarContratoManttoVIS vistaActual, IucHerramientasManttoVIS vistaHerramientas, IucContratoManttoVIS vistaContrato, IucLineaContratoManttoVIS vistaLinea)
        {
            try
            {
                this.vista = vistaActual;

                this.presentadorContrato = new ucContratoManttoPRE(vistaContrato, vistaLinea);
                this.presentadorHerramientas = new ucHerramientasManttoPRE(vistaHerramientas);

                this.controlador = new ContratoManttoBR();
                this.pagoCtrl = new PagoUnidadContratoBR();
                this.generadorPagos = new GeneradorPagosManttoBR();

                this.dctx = FacadeBR.ObtenerConexion();
            }
            catch (Exception ex)
            {
                this.vista.MostrarMensaje("Inconsistencia en los parámetros de configuración", ETipoMensajeIU.ERROR, this.nombreClase + ".CerrarContratoManttoPRE:" + ex.Message);
            }
        }

        #endregion

        #region Métodos
        /// <summary>
        /// Realiza la Primera Carga de Informacion a la Vista
        /// </summary>
        public void RealizarPrimeraCarga()
        {
            this.PrepararVisualizacion();

            this.EstablecerDatosNavegacion(this.vista.ObtenerPaqueteNavegacion("ContratoManttoBO"));

            this.ConsultarCompleto();

            this.EstablecerConfiguracionInicial();
            this.presentadorContrato.OcultarDatosAdicionales(true);
            this.presentadorContrato.OcultarPersonasCliente(true);
            this.presentadorHerramientas.vista.MarcarOpcionCerrarContrato();
            this.ValidarEstatusContrato();
            this.presentadorHerramientas.vista.OcultarPlantillas();
            this.presentadorHerramientas.DeshabilitarMenuEditar();
            this.presentadorHerramientas.DeshabilitarMenuImprimir();
            this.presentadorHerramientas.DeshabilitarMenuBorrar();

            this.EstablecerSeguridad();
        }

        /// <summary>
        /// Valida y los Despliega los datos del paquete de navegación en la Vista
        /// </summary>
        /// <param name="paqueteNavegacion">Paquete de Datos</param>
        private void EstablecerDatosNavegacion(object paqueteNavegacion)
        {
            try
            {
                if (paqueteNavegacion == null)
                    throw new Exception("Se esperaba un objeto en la navegación. No se puede identificar qué reservación se desea consultar.");
                if (!(paqueteNavegacion is ContratoManttoBO))
                    throw new Exception("Se esperaba un Contrato de Mantenimiento.");

                ContratoManttoBO bo = (ContratoManttoBO)paqueteNavegacion;

                this.DatoAInterfazUsuario(bo);
            }
            catch (Exception ex)
            {
                this.DatoAInterfazUsuario(new ContratoManttoBO());
                throw new Exception(this.nombreClase + ".EstablecerDatosNavegacion: " + ex.Message);
            }

        }

        /// <summary>
        /// Realiza la Consulta Completa del Contrato y la Despleiga en la Vista
        /// </summary>
        private void ConsultarCompleto()
        {
            try
            {
                //Se consulta la información del contrato
                ContratoManttoBO bo = (ContratoManttoBO)this.InterfazUsuarioADato();

                List<ContratoManttoBO> lst = this.controlador.ConsultarCompleto(this.dctx, bo, true);

                if (lst.Count < 1)
                    throw new Exception("No se encontró ningún registro que corresponda a la información proporcionada.");
                if (lst.Count > 1)
                    throw new Exception("La consulta devolvió más de un registro.");

                this.DatoAInterfazUsuario(lst[0]);
                this.presentadorHerramientas.DatosAInterfazUsuario(lst[0]);
                this.vista.UltimoObjeto = lst[0];
            }
            catch (Exception ex)
            {
                this.DatoAInterfazUsuario(new ContratoManttoBO());
                throw new Exception(this.nombreClase + ".ConsultarCompleto:" + ex.Message);
            }
        }

        /// <summary>
        ///  Establece la Configuracion Inicial para la Vista
        /// </summary>
        private void EstablecerConfiguracionInicial()
        {
            this.vista.FUA = DateTime.Now;
            this.vista.UUA = this.vista.UsuarioID;
            this.vista.FechaCierre = this.vista.FUA;
        }

        /// <summary>
        /// Valida el acceso a la página de edición
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
                if (!FacadeBR.ExisteAccion(this.dctx, "TERMINAR", seguridadBO))
                    this.vista.RedirigirSinPermisoAcceso();
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".ValidarAcceso: " + ex.Message);
            }
        }
        /// <summary>
        /// Permite configurar el comportamiento de los controles de la vista de acuerdo a los permisos configurados
        /// </summary>
        private void EstablecerSeguridad()
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
                if (!this.ExisteAccion(acciones, "UI INSERTAR"))
                    this.vista.PermitirRegistrar(false);
            }
            catch (Exception ex)
            {

                throw new Exception(this.nombreClase + ".EstablecerSeguridad:" + ex.Message);
            }
        }

        /// <summary>
        /// Indica si existe una acción entre los permisos del usuario
        /// </summary>
        /// <param name="acciones">Acciones permitidas al usuario</param>
        /// <param name="nombreAccion">Nombre de la acción a verifica</param>
        /// <returns></returns>
        private bool ExisteAccion(List<CatalogoBaseBO> acciones, string nombreAccion)
        {
            if (acciones != null && acciones.Count > 0 &&
               acciones.Exists(
                   p => p != null && p.Nombre != null && p.Nombre.Trim().ToUpper() == nombreAccion.Trim().ToUpper()))
                return true;

            return false;
        }

        /// <summary>
        /// Prepara la Visualizacion de los controles
        /// </summary>
        private void PrepararVisualizacion()
        {
            this.presentadorHerramientas.Inicializar();
            this.presentadorContrato.PrepararVisualizacion();
            this.vista.ObservacionObligatoria(false);
        }

        /// <summary>
        /// Limpia los datos y Solicita a la vista el redireccionamiento a la pagina de Detalle
        /// </summary>
        public void Regresar()
        {
            ContratoManttoBO bo = (ContratoManttoBO)this.vista.UltimoObjeto;
            this.LimpiarSesion();

            this.vista.LimpiarPaqueteNavegacion("ContratoManttoBO");
            this.vista.EstablecerPaqueteNavegacion("ContratoManttoBO", bo);

            this.vista.RedirigirADetalles();
        }

        /// <summary>
        /// Realiza el Cierre del Contrato
        /// </summary>
        public void CerrarContrato()
        {
            int numeroPago;
            string s;

            if ((s = ValidarCampos()) != null)
            {
                vista.MostrarMensaje(s, ETipoMensajeIU.ADVERTENCIA);
                return;
            }
            Guid firma = Guid.NewGuid();
            try
            {
                #region Transaccion

                dctx.SetCurrentProvider("Outsourcing");
                dctx.OpenConnection(firma);
                dctx.BeginTransaction(firma);

                #endregion

                //Se obtiene la información a partir de la Interfaz de Usuario
                var bo = (ContratoManttoBO) InterfazUsuarioADato();

                //Se crea el objeto de seguridad
                var usuario = new UsuarioBO {Id = vista.UsuarioID};
                var adscripcion = new AdscripcionBO
                {
                    UnidadOperativa = new UnidadOperativaBO {Id = vista.UnidadOperativaID}
                };
                var seguridadBO = new SeguridadBO(Guid.Empty, usuario, adscripcion);
                bool cierreAnticipado = DateTime.Compare((DateTime) bo.CierreContrato.Fecha, (DateTime) bo.CalcularFechaTerminacionContrato()) < 0;

                #region SC0010

                var pagos = ObtenerPagosContrato(bo.ContratoID);
                numeroPago = (int) ObtenerUltimoNumeroPago(pagos);
                numeroPago = numeroPago + 1;
                if (cierreAnticipado)
                {
                    CancelarPagosPosteriores(pagos, bo.CierreContrato.Fecha, seguridadBO);
                }
                if (bo.Tipo == ETipoContrato.CM)
                {
                    generadorPagos.GenerarPagoAdicional(dctx, bo, numeroPago, seguridadBO, true,true);
                }

                #endregion

                //Se actualiza en la base de datos
                controlador.Terminar(dctx, bo, (ContratoManttoBO) vista.UltimoObjeto, seguridadBO,
                    cierreAnticipado);

                //Se despliega la información en la Interfaz de Usuario
                DatoAInterfazUsuario(bo);
                vista.UltimoObjeto = bo;

                LimpiarSesion();
                vista.LimpiarPaqueteNavegacion("ContratoManttoBO");
                vista.EstablecerPaqueteNavegacion("ContratoManttoBO", new ContratoManttoBO {ContratoID = vista.ContratoID});
                
                dctx.CommitTransaction(firma);
                
                vista.RedirigirADetalles();
            }
            catch (Exception ex)
            {
                dctx.RollbackTransaction(firma);
                throw new Exception(nombreClase + ".CerrarContrato:" + ex.Message);
            }
            finally
            {
                if (dctx.ConnectionState == ConnectionState.Open)
                    dctx.CloseConnection(firma);
            }
        }

        #region SC0010
        /// <summary>
        /// Obtiene los Pagos del Contrato
        /// </summary>
        /// <param name="contratoId">Identificador del Contrato al cual pertencen los pagos</param>
        /// <returns></returns>
        private List<PagoUnidadContratoManttoBO> ObtenerPagosContrato(int? contratoId)
        {
            //SC0026, Utilización de clase concreta segun el tipo de contrato
            var pagoUnidadContrato = new PagoUnidadContratoBOF
            {
                ReferenciaContrato = new ReferenciaContratoBO { ReferenciaContratoID = contratoId }
            };
            List<PagoUnidadContratoBO> pagoUnidad = pagoCtrl.Consultar(dctx, pagoUnidadContrato);

            return pagoUnidad.Cast<PagoUnidadContratoManttoBO>().ToList();
        }

        /// <summary>
        /// Obtiene el Ultimo Numero de Pago de una Lista de Pagos
        /// </summary>
        /// <param name="pagos">Listado de Pagos</param>
        /// <returns></returns>
        private short? ObtenerUltimoNumeroPago(List<PagoUnidadContratoManttoBO> pagos)
        {
            return pagos.OrderByDescending(x => x.NumeroPago).First().NumeroPago;
        }

        /// <summary>
        /// Cancela los pagos restantes de un contrato
        /// </summary>
        /// <param name="pagosList">Listado de Pagos</param>
        /// <param name="fechaCierre">Fecha de Cierre del Contrato</param>
        /// <param name="seguridadBo">Seguridad del Usuario Actual</param>
        private void CancelarPagosPosteriores(List<PagoUnidadContratoManttoBO> pagosList, DateTime? fechaCierre, SeguridadBO seguridadBo)
        {
            foreach (var pago in pagosList)
            {
                if (pago.FechaVencimiento >= fechaCierre && pago.EnviadoFacturacion == false)
                {
                    var anterior = new PagoUnidadContratoManttoBO(pago);
                    pago.Activo = false;
                    anterior.Auditoria = new AuditoriaBO
                        {
                            FC = pago.Auditoria.FC,
                            UC = pago.Auditoria.UC,
                            FUA = pago.Auditoria.FUA,
                            UUA = pago.Auditoria.UUA
                        };
                    pagoCtrl.Actualizar(dctx, pago, anterior, seguridadBo);
                }
            }
        }
        #endregion

        /// <summary>
        /// Obtiene los datos de la Vista
        /// </summary>
        /// <returns></returns>
        private object InterfazUsuarioADato()
        {
            ContratoManttoBO bo = new ContratoManttoBO();
            if (this.vista.UltimoObjeto != null)
                bo = new ContratoManttoBO((ContratoManttoBO)this.vista.UltimoObjeto);

            bo.CierreContrato = new CierreContratoManttoBO();
            bo.CierreContrato.Usuario = new UsuarioBO();

            bo.ContratoID = this.vista.ContratoID;
            bo.UUA = this.vista.UUA;
            bo.FUA = this.vista.FUA;
            bo.Tipo = (ETipoContrato)this.vista.TipoContratoID.Value;

            if (this.vista.EstatusID != null)
                bo.Estatus = (EEstatusContrato)Enum.Parse(typeof(EEstatusContrato), this.vista.EstatusID.ToString());
            else
                bo.Estatus = null;

            bo.CierreContrato.Fecha = this.vista.FechaCierre;
            bo.CierreContrato.Observaciones = this.vista.ObservacionesCierre;
            bo.CierreContrato.Usuario.Id = this.vista.UsuarioID;

            ((CierreContratoManttoBO)bo.CierreContrato).Motivo = this.vista.MotivoCierre;

            return bo;
        }

        /// <summary>
        /// Despliega los datos proporcionado en la Vista
        /// </summary>
        /// <param name="obj">Objetos con Datos a desplegar</param>
        private void DatoAInterfazUsuario(object obj)
        {
            ContratoManttoBO bo = (ContratoManttoBO)obj;
            if (bo == null) bo = new ContratoManttoBO();
            if (bo.Cliente == null) bo.Cliente = new CuentaClienteIdealeaseBO();
            if (bo.Divisa == null) bo.Divisa = new DivisaBO();
            if (bo.Divisa.MonedaDestino == null) bo.Divisa.MonedaDestino = new MonedaBO();
            if (bo.Sucursal == null) bo.Sucursal = new SucursalBO();
            if (bo.Sucursal.UnidadOperativa == null) bo.Sucursal.UnidadOperativa = new UnidadOperativaBO();
            if (bo.CierreContrato == null) bo.CierreContrato = new CierreContratoManttoBO();
            if (bo.CierreContrato.Usuario == null) bo.CierreContrato.Usuario = new UsuarioBO();

            this.vista.ContratoID = bo.ContratoID;
            this.vista.NumeroContrato = bo.NumeroContrato;
            this.vista.CodigoMoneda = bo.Divisa.MonedaDestino.Codigo;
            this.vista.FechaContrato = bo.FechaContrato;
            this.vista.RepresentanteEmpresa = bo.Representante;
            if (bo.Tipo != null)
                this.vista.TipoContratoID = (int)bo.Tipo;
            else
                this.vista.TipoContratoID = null;

            this.vista.SucursalID = bo.Sucursal.Id;
            this.vista.SucursalNombre = bo.Sucursal.Nombre;

            //Cuenta de Cliente Idealease
            this.vista.CuentaClienteID = bo.Cliente.Id;
            this.vista.CuentaClienteNombre = bo.Cliente.Nombre;
            if (bo.Cliente.TipoCuenta != null)
                this.vista.CuentaClienteTipoID = (int)bo.Cliente.TipoCuenta;
            else
                this.vista.CuentaClienteTipoID = null;
            this.presentadorContrato.SeleccionarCuentaCliente(bo.Cliente);

            //Dirección del cliente
            if (bo.Cliente.Direcciones != null && bo.Cliente.Direcciones.Count > 0)
            {
                this.vista.ClienteDireccionCompleta = bo.Cliente.Direcciones[0].Direccion;
                this.vista.ClienteDireccionCalle = bo.Cliente.Direcciones[0].Calle;
                this.vista.ClienteDireccionColonia = bo.Cliente.Direcciones[0].Colonia;
                this.vista.ClienteDireccionCodigoPostal = bo.Cliente.Direcciones[0].CodigoPostal;
                this.vista.ClienteDireccionCiudad = bo.Cliente.Direcciones[0].Ubicacion.Ciudad.Codigo;
                this.vista.ClienteDireccionEstado = bo.Cliente.Direcciones[0].Ubicacion.Estado.Codigo;
                this.vista.ClienteDireccionMunicipio = bo.Cliente.Direcciones[0].Ubicacion.Municipio.Codigo;
                this.vista.ClienteDireccionPais = bo.Cliente.Direcciones[0].Ubicacion.Pais.Codigo;
            }
            else
            {
                this.vista.ClienteDireccionCompleta = null;
                this.vista.ClienteDireccionCalle = null;
                this.vista.ClienteDireccionColonia = null;
                this.vista.ClienteDireccionCodigoPostal = null;
                this.vista.ClienteDireccionCiudad = null;
                this.vista.ClienteDireccionEstado = null;
                this.vista.ClienteDireccionMunicipio = null;
                this.vista.ClienteDireccionPais = null;
            }
            this.vista.Plazo = bo.Plazo;
            this.vista.FechaInicioContrato = bo.FechaInicioContrato;
            this.vista.FechaTerminacionContrato = bo.CalcularFechaTerminacionContrato();

            if (bo.LineasContrato != null)
                this.vista.LineasContrato = bo.LineasContrato.ConvertAll(s => (LineaContratoManttoBO)s);
            else
                this.vista.LineasContrato = null;

            this.vista.UbicacionTaller = bo.UbicacionTaller;
            this.vista.DireccionAlmacenaje = bo.DireccionAlmacenaje;
            this.vista.DepositoGarantia = bo.DepositoGarantia;
            this.vista.ComisionApertura = bo.ComisionApertura;
            if (bo.IncluyeLavado != null)
                this.vista.IncluyeLavadoID = (int)bo.IncluyeLavado;
            else
                this.vista.IncluyeLavadoID = null;
            if (bo.IncluyeLlantas != null)
                this.vista.IncluyeLlantasID = (int)bo.IncluyeLlantas;
            else
                this.vista.IncluyeLlantasID = null;
            if (bo.IncluyePinturaRotulacion != null)
                this.vista.IncluyePinturaRotulacionID = (int)bo.IncluyePinturaRotulacion;
            else
                this.vista.IncluyePinturaRotulacionID = null;
            if (bo.IncluyeSeguro != null)
                this.vista.IncluyeSeguroID = (int)bo.IncluyeSeguro;
            else
                this.vista.IncluyeSeguroID = null;

            this.vista.Observaciones = bo.Observaciones;

            this.vista.UUA = bo.UUA;
            this.vista.FUA = bo.FUA;
            if (bo.Estatus != null)
                this.vista.EstatusID = (int)bo.Estatus;
            else
                this.vista.EstatusID = null;
            this.vista.FechaCierre = bo.CierreContrato.Fecha;
            this.vista.ObservacionesCierre = bo.CierreContrato.Observaciones;
            if (bo.CierreContrato is CierreContratoManttoBO)
                this.vista.MotivoCierre = ((CierreContratoManttoBO)bo.CierreContrato).Motivo;
            else
                this.vista.MotivoCierre = null;
        }

        /// <summary>
        /// Valida los requeridos para el Cierre del Contrato
        /// </summary>
        /// <returns></returns>
        public string ValidarCampos()
        {
            string s = string.Empty;

            if (this.vista.FUA == null)
                s += "Fecha de Última Modificación, ";
            if (this.vista.UUA == null)
                s += "Usuario de Última Modificación, ";
            if (this.vista.EstatusID == null)
                s += "Estatus, ";
            if (this.vista.ContratoID == null)
                s += "Contrato, ";
            if (this.vista.FechaCierre == null)
                s += "Fecha Cierre, ";
            if (string.IsNullOrEmpty(this.vista.MotivoCierre) || string.IsNullOrWhiteSpace(this.vista.MotivoCierre))
                s += "Motivo Cierre, ";

            if (s != null && s.Trim().CompareTo("") != 0)
                return "Los siguientes campos no pueden estar vacíos: \n" + s.Substring(0, s.Length - 2);

            if (vista.FechaCierre < DateTime.Today)
                return "La fecha de cierre no puede ser menor a la fecha actual";
            if (vista.FechaCierre < vista.FechaInicioContrato)
                return "La fecha de cierre no puede ser menor a la fecha de inicio del contrato";
            if (vista.FechaCierre < vista.FechaTerminacionContrato)
            {
                this.vista.ObservacionObligatoria(true);
                if (this.vista.ObservacionesCierre == null)
                    return "La fecha de Cierre es menor a la fecha de Termino del contrato y se considera un cierre anticipado, se requiere el campo Observaciones";

            }

            if ((s = this.ValidarContrato()) != null)
                return s;

            return null;
        }

        /// <summary>
        /// Valida el Cierre del Contrato
        /// </summary>
        /// <returns></returns>
        public string ValidarContrato()
        {
            string s = string.Empty;

            if (this.vista.EstatusID == null)
                s += "Estatus, ";

            if (s != null && s.Trim().CompareTo("") != 0)
                return "Los siguientes campos no pueden estar vacíos: \n" + s.Substring(0, s.Length - 2);

            EEstatusContrato estatus =
                (EEstatusContrato)Enum.Parse(typeof(EEstatusContrato), this.vista.EstatusID.ToString());
            if (!(estatus == EEstatusContrato.EnCurso))
                return "El contrato no puede cerrarse a menos que esté En Curso.";

            return null;
        }

        /// <summary>
        /// Solicita a la Vista Limpiar las Variables de Sesion del Usuario
        /// </summary>
        private void LimpiarSesion()
        {
            this.vista.LimpiarSesion();
        }
        /// <summary>
        /// Valida el Estatus del Contrato a Cerrar
        /// </summary>
        private void ValidarEstatusContrato()
        {
            EEstatusContrato estatus =
                (EEstatusContrato)Enum.Parse(typeof(EEstatusContrato), this.vista.EstatusID.ToString());
            if (!(estatus == EEstatusContrato.EnCurso))
            {
                this.vista.DeshabilitarBotonCerrar();
                this.vista.MostrarMensaje("El contrato no se encuentra en curso, no se puede cerrar", ETipoMensajeIU.ADVERTENCIA, null);
            }
        }
        #endregion
    }
}
