using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using BPMO.Basicos.BO;
using BPMO.Facade.SDNI.BR;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Contratos.RD.BO;
using BPMO.SDNI.Contratos.RD.BR;
using BPMO.SDNI.Contratos.RD.VIS;
using BPMO.SDNI.Facturacion.AplicacionesPago.BR;
using BPMO.SDNI.Facturacion.InfraestructuraPagos.BO;
using BPMO.SDNI.Facturacion.InfraestructuraPagos.BOF;
using BPMO.SDNI.Facturacion.InfraestructuraPagos.BR;

namespace BPMO.SDNI.Contratos.RD.PRE
{
    /// <summary>
    /// Presentador para interactuar con la Interfaz de Cambio de Frecuencia de un Contrato de RD
    /// </summary>
    public class CambiarFrecuenciaContratosRDPRE
    {
        #region Atributos
        /// <summary>
        /// Vista con las propiedades usadas en la Interfaz de Usuario
        /// </summary>
        private readonly ICambiarFrecuenciaContratosRDVIS vista;
        /// <summary>
        /// Controlador para realizar las acciones de la Interfaz
        /// </summary>
        private readonly ContratoRDBR controladorContrato;
        /// <summary>
        /// Controlador de pagos
        /// </summary>
        private readonly PagoUnidadContratoRDBR controladorPagos;
        /// <summary>
        /// DataContext que provee acceso a la base de datos
        /// </summary>
        private readonly IDataContext dataContext;
        /// <summary>
        /// Nombre de la Clase, usada para Reporte de Errores
        /// </summary>
        private static readonly string nombreClase = typeof(CambiarFrecuenciaContratosRDPRE).Name;
        #endregion
        #region Constructores
        public CambiarFrecuenciaContratosRDPRE(ICambiarFrecuenciaContratosRDVIS view)
        {
            if(view == null) throw new Exception(nombreClase + ".CambiarFrecuenciaContratosRDPRE: La vista usada no puede ser NULL");
            try
            {
                this.vista = view;
                this.controladorContrato = new ContratoRDBR();
                this.controladorPagos = new PagoUnidadContratoRDBR();
                this.dataContext = FacadeBR.ObtenerConexion();
            }
            catch(Exception ex)
            {
                throw new Exception(nombreClase + ".CambiarFrecuenciaContratosRDPRE: Ocurrio un problema al obtener la informacion Inicial", ex.InnerException);
            }
        }
        #endregion
        #region Metodos
        public void PrepararEdicion()
        {
            try
            {
                this.vista.PermitirGuardar(true);
                this.vista.PermitirCancelar(true);
                this.vista.EstablecerInformacionInicial((ContratoRDBO)this.vista.ObtenerPaqueteNavegacion("ContratoRDBO"));
                this.ConsultarCompleto();
                this.vista.InactivarCamposIniciales();
            }
            catch(Exception ex)
            {
                throw new Exception(nombreClase + ".PrepararEdicion()." + ex.Message, ex.InnerException);
            }
        }
        /// <summary>
        /// Consulta El contrato y los pagos facturados y Pendientes por Facturar
        /// </summary>
        private void ConsultarCompleto()
        {
            try
            {
                var contratoRd = new ContratoRDBO() { Sucursal = new SucursalBO(), Cliente = new CuentaClienteIdealeaseBO(), ContratoID = this.vista.ContratoId };
                var contratosRd = this.controladorContrato.ConsultarCompleto(this.dataContext, contratoRd);
                if(!contratosRd.Any())
                    throw new Exception("No se encontro el contrato Solicitado");
                if(contratosRd.Count > 1)
                    throw new Exception("El contrato proporcionado devolvio mas de un resultado");

                contratoRd = contratosRd.First();
                this.vista.ContratoOriginal = contratoRd;
                this.vista.ContratoAntiguo = new ContratoRDBO(contratoRd);
                this.DatoInterfazUsuario(contratoRd);

                var pagos = controladorPagos.ConsultarCompleto(this.dataContext, new PagoUnidadContratoRDBO() { ReferenciaContrato = new ReferenciaContratoBO() { ReferenciaContratoID = contratoRd.ContratoID }, Activo = true });
                if(!pagos.Any())
                    throw new Exception("No se encontraton los pagos del Contrato");
                
                DateTime fechaHoy = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 23, 59, 59);
                var listaPagosVencidos = pagos.Where(x => x.FechaVencimiento < fechaHoy).OrderBy(y => y.NumeroPago).ToList().ConvertAll(x => (PagoUnidadContratoRDBO)x);
                var listaPagosFaltantes = pagos.Where(x => x.FechaVencimiento > fechaHoy).OrderBy(y => y.NumeroPago).ToList().ConvertAll(x => (PagoUnidadContratoRDBO)x);

                this.vista.EstablecerPagosFacturados(listaPagosVencidos);
                this.vista.EstablecerPagosPendientes(listaPagosFaltantes);

                this.vista.ListaPagosFacturados = listaPagosVencidos;
                this.vista.ListaPagosFaltantes = listaPagosFaltantes;

                var diasUsados = listaPagosVencidos.Sum(x => x.DiasFacturar);
                var diasContrato = contratoRd.FechaPromesaDevolucion.Value.Subtract(contratoRd.FechaContrato.Value).Days;
                this.vista.DiasRestantes = diasContrato > diasUsados ? (diasContrato - diasUsados) : 0;

                this.CalcularFrecuenciasPermitidas(contratoRd.FrecuenciaFacturacion);
            }
            catch(Exception ex)
            {
                throw new Exception("ConsultarCompleto(): Ocurrio un problema al consultar completo el contrato y sus pagos. " + ex.Message, ex.InnerException);
            }
        }
        /// <summary>
        /// Obtiene la informacion de la interfaz
        /// </summary>
        /// <returns>Objeto con la Informacion de la Interfaz</returns>
        private Object InterfazUsuarioDato()
        {
            ContratoRDBO contrato = this.vista.ContratoOriginal != null ? this.vista.ContratoOriginal : new ContratoRDBO() { Sucursal = new SucursalBO() { UnidadOperativa = this.vista.UnidadOperativa }, Cliente = new CuentaClienteIdealeaseBO() };

            contrato.ContratoID = this.vista.ContratoId != null ? this.vista.ContratoId : null;
            contrato.Sucursal.Id = this.vista.SucursalId != null ? this.vista.SucursalId : null;
            contrato.Cliente.Id = this.vista.ClienteId != null ? this.vista.ClienteId : null;
            contrato.FrecuenciaFacturacion = this.vista.FrecuenciaNueva != null ? this.vista.FrecuenciaNueva : null;

            return contrato;
        }
        /// <summary>
        /// Coloca la Información del Contrato en la Interfaz de Usuario
        /// </summary>
        /// <param name="objeto">Objeto con la informacion que se presentara</param>
        private void DatoInterfazUsuario(Object objeto)
        {
            ContratoRDBO contrato = objeto as ContratoRDBO;

            this.vista.ContratoId = contrato.ContratoID != null ? contrato.ContratoID : null;
            this.vista.NumeroContrato = !String.IsNullOrEmpty(contrato.NumeroContrato) ? contrato.NumeroContrato : null;
            if(contrato.Sucursal != null)
            {
                this.vista.SucursalId = contrato.Sucursal.Id != null ? contrato.Sucursal.Id : null;
                this.vista.SucursalNombre = !String.IsNullOrEmpty(contrato.Sucursal.Nombre) ? contrato.Sucursal.Nombre : null;
            }
            else
            {
                this.vista.SucursalId = null;
                this.vista.SucursalNombre = null;
            }
            if(contrato.Cliente != null)
            {
                this.vista.ClienteId = contrato.Cliente.Id != null ? contrato.Cliente.Id : null;
                this.vista.ClienteNombre = !String.IsNullOrEmpty(contrato.Cliente.Nombre) ? contrato.Cliente.Nombre : null;
            }
            else
            {
                this.vista.ClienteId = null;
                this.vista.ClienteNombre = null;
            }
            this.vista.FechaContrato = contrato.FechaContrato != null ? contrato.FechaContrato : null;
            this.vista.FechaPromesaDevolucion = contrato.FechaPromesaDevolucion != null ? contrato.FechaPromesaDevolucion : null;
           this.vista.FrecuenciaActual = contrato.FrecuenciaFacturacion != null ? contrato.FrecuenciaFacturacion : null;
        }
        /// <summary>
        /// Valida el acceso y los permisos del  usuario
        /// </summary>
        public void ValidarAcceso()
        {
            try
            {
                //se valida que los datos del usuario y la unidad operativa no sean nulos
                if(this.vista.UsuarioId == null) throw new Exception("El identificador del usuario no debe ser nulo");
                if(this.vista.UnidadOperativa.Id == null) throw new Exception("El identificador de la Unidad Operativa no debe ser nulo ");
                //Se crea el objeto de Seguridad
                SeguridadBO seguridadBO = this.CrearObjetoSeguridad();
                var acciones = FacadeBR.ConsultarAccion(this.dataContext, seguridadBO);
                //Se valida si el usuario tiene permisos para realizar la acción
                if(!FacadeBR.ExisteAccion(acciones, "CONSULTAR") || !FacadeBR.ExisteAccion(acciones, "CONSULTARCOMPLETO"))
                    this.vista.RedirigirSinPermisoAcceso();
                if(!FacadeBR.ExisteAccion(acciones, "ACTUALIZAR") || !FacadeBR.ExisteAccion(acciones, "ACTUALIZARCOMPLETO") ||
                    !FacadeBR.ExisteAccion(acciones, "ELIMINAR") || !FacadeBR.ExisteAccion(acciones, "CAMBIARFRECUENCIAPAGOS") ||
                    !FacadeBR.ExisteAccion(acciones, "INSERTARCOMPLETO") || !FacadeBR.ExisteAccion(acciones, "INSERTAR"))
                    this.vista.PermitirGuardar(false);
            }
            catch(Exception ex)
            {
                throw new Exception(nombreClase + ".ValidarAcceso: " + ex.Message);
            }
        }
        /// <summary>
        /// Crea un objeto de seguridad con los datos actuales de la sesión de usuario en curso
        /// </summary>
        /// <returns>Objeto de tipo seguridad</returns>
        private SeguridadBO CrearObjetoSeguridad()
        {
            UsuarioBO usuario = new UsuarioBO() { Id = this.vista.UsuarioId };
            AdscripcionBO adscripcion = new AdscripcionBO() { UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativa.Id } };
            SeguridadBO seguridadBO = new SeguridadBO(Guid.Empty, usuario, adscripcion);

            return seguridadBO;
        }
        /// <summary>
        /// Calcula la frecuencias a las cuales puede cambiar el contrato
        /// </summary>
        /// <param name="frecuenciaContrato"></param>
        private void CalcularFrecuenciasPermitidas(EFrecuencia? frecuenciaContrato)
        {
            Dictionary<String, String> listaFrecuencias = new Dictionary<String, String>();
            listaFrecuencias.Add((-1).ToString(), "-- SELECCIONE --");

            if(frecuenciaContrato != null)
            {
                if(frecuenciaContrato == EFrecuencia.DIARIA)
                {
                    listaFrecuencias.Add(((Int32)EFrecuencia.SEMANAL).ToString(), EFrecuencia.SEMANAL.ToString());
                    listaFrecuencias.Add(((Int32)EFrecuencia.QUINCENAL).ToString(), EFrecuencia.QUINCENAL.ToString());
                    listaFrecuencias.Add(((Int32)EFrecuencia.MENSUAL).ToString(), EFrecuencia.MENSUAL.ToString());
                }
                if(frecuenciaContrato == EFrecuencia.SEMANAL)
                {
                    listaFrecuencias.Add(((Int32)EFrecuencia.DIARIA).ToString(), EFrecuencia.DIARIA.ToString());
                    listaFrecuencias.Add(((Int32)EFrecuencia.QUINCENAL).ToString(), EFrecuencia.QUINCENAL.ToString());
                    listaFrecuencias.Add(((Int32)EFrecuencia.MENSUAL).ToString(), EFrecuencia.MENSUAL.ToString());
                }
                if(frecuenciaContrato == EFrecuencia.QUINCENAL)
                {
                    listaFrecuencias.Add(((Int32)EFrecuencia.DIARIA).ToString(), EFrecuencia.DIARIA.ToString());
                    listaFrecuencias.Add(((Int32)EFrecuencia.SEMANAL).ToString(), EFrecuencia.SEMANAL.ToString());
                    listaFrecuencias.Add(((Int32)EFrecuencia.MENSUAL).ToString(), EFrecuencia.MENSUAL.ToString());
                }
                if(frecuenciaContrato == EFrecuencia.MENSUAL)
                {
                    listaFrecuencias.Add(((Int32)EFrecuencia.DIARIA).ToString(), EFrecuencia.DIARIA.ToString());
                    listaFrecuencias.Add(((Int32)EFrecuencia.SEMANAL).ToString(), EFrecuencia.SEMANAL.ToString());
                    listaFrecuencias.Add(((Int32)EFrecuencia.QUINCENAL).ToString(), EFrecuencia.QUINCENAL.ToString());
                }                
            }

            this.vista.EstablecerFrecuenciasFacturacion(listaFrecuencias);
        }
        /// <summary>
        /// Cambia la Frecuencia de Facturacion de un contrato de RD
        /// </summary>
        public void CambiarFrecuencia()
        {
            try
            {
                var s = this.ValidarCampos();
                if(!String.IsNullOrEmpty(s))
                {
                    this.vista.MostrarMensaje(s, ETipoMensajeIU.ADVERTENCIA);
                    return;
                }

                var contratoRd = (ContratoRDBO)this.InterfazUsuarioDato();
                contratoRd.FUA = DateTime.Now;
                contratoRd.UUA = this.vista.UsuarioId;

                SeguridadBO seguridadBo = this.CrearObjetoSeguridad();

                #region Conexion a BD

                Guid firma = Guid.NewGuid();
                try
                {
                    this.dataContext.OpenConnection(firma);
                    this.dataContext.BeginTransaction(firma);
                }
                catch(Exception)
                {
                    if(this.dataContext.ConnectionState == ConnectionState.Open)
                        this.dataContext.CloseConnection(firma);
                    throw new Exception("Se encontraron inconsistencias al Cambiar la Frecuencia del Contrato.");
                }

                #endregion
                try
                {
                    controladorContrato.Actualizar(this.dataContext, contratoRd, this.vista.ContratoAntiguo, seguridadBo);
                    ModificarPagosBR modificarPagosBr = new ModificarPagosBR();
                    modificarPagosBr.CambiarFrecuenciaPagos(this.dataContext, contratoRd, this.vista.CantidadPagosFaltantes != 0, this.vista.CantidadPagosFaltantes, seguridadBo);

                    this.dataContext.CommitTransaction(firma);

                    this.vista.PermitirCancelar(false);
                    this.vista.PermitirGuardar(false);
                    this.vista.PermitirFrecuencia(false);

                    this.vista.MostrarMensaje("La Frecuencia de Facturación del contrato se cambió con ÉXITO", ETipoMensajeIU.EXITO);
                }
                catch(Exception ex)
                {
                    this.dataContext.RollbackTransaction(firma);
                    throw;
                }
                finally
                {
                    if(this.dataContext.ConnectionState == ConnectionState.Open)
                        this.dataContext.CloseConnection(firma);
                }
            }
            catch(Exception ex)
            {
                throw new Exception(nombreClase + ".CambiarFrecuencia(): " + ex.Message, ex.InnerException);
            }
        }
        /// <summary>
        /// Cancela el Cambio de Frecuencia y redirige a la pantalla de Consulta
        /// </summary>
        public void Cancelar()
        {
             try
            {
                this.vista.LimpiarSesion();
                this.vista.RedirigirAConsulta();
            }
             catch(Exception ex)
             {
                 throw new Exception("Cancelar(): Ocurrio un problema al Cancelar el Cambio de Frecuencia. " + ex.Message, ex.InnerException);
             }
        }
        /// <summary>
        /// Valida la informacion de la interfaz
        /// </summary>
        /// <returns></returns>
        public string ValidarCampos()
        {
            if(this.vista.FrecuenciaNueva == null)
                return "No se ha seleccionado la Nueva Frecuencia de Facturacion";

            if(this.vista.ListaPagosFacturados != null && this.vista.ListaPagosFacturados.Any())
            {
                if(this.vista.CantidadPagosFaltantes == null)
                    return "No se establecio cuantos pagos se deberán de crear";

                DateTime fechaHoy = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 23, 59, 59);
                if(this.vista.ListaPagosFacturados.Any(x => x.FechaVencimiento < fechaHoy && x.EnviadoFacturacion == false))
                    return "Aun existen pagos para enviar a Facturar Pendientes, Envie a Facturar los pagos para proceder con el cambio de Frecuencia";
            }

            return String.Empty;
        }
        /// <summary>
        /// Calcula los pagos pendientes a partir de la nueva frecuencia
        /// </summary>
        /// <param name="frecuenciaNueva"></param>
        public void CalcularPagosPendientes(EFrecuencia? frecuenciaNueva)
        {
            if(!this.vista.ListaPagosFaltantes.Any())
            {
                this.vista.ListaPagosFaltantes = new List<PagoUnidadContratoRDBO>();
                this.vista.EstablecerPagosPendientes(this.vista.ListaPagosFaltantes); 
                this.vista.CantidadPagosFaltantes = 0; 
                return;
            }

            if(frecuenciaNueva == null) frecuenciaNueva = this.vista.ContratoAntiguo.FrecuenciaFacturacion;

            List<PagoUnidadContratoRDBO> listaPagosPendientes = new List<PagoUnidadContratoRDBO>();
            DateTime? fechaContrato = this.vista.ContratoOriginal.FechaContrato;
            DateTime? fechaPromesa = this.vista.ContratoOriginal.FechaPromesaDevolucion;
            DateTime fechaHoy = DateTime.Now;
            if(fechaContrato == fechaPromesa) fechaPromesa = (DateTime?)fechaPromesa.Value.AddDays(1);
            if(this.vista.DiasRestantes <= 0)
            {
                this.vista.ListaPagosFaltantes = listaPagosPendientes;
                this.vista.EstablecerPagosPendientes(listaPagosPendientes); 
                this.vista.CantidadPagosFaltantes = 0; 
                return;
            }

            if(this.vista.ListaPagosFacturados.Any())
            {
                var ultimoPago = this.vista.ListaPagosFacturados.Last();
                Int32 diasAAgregar = 0;
                Int32 diasFaltantes = this.vista.DiasRestantes.Value;
                switch(frecuenciaNueva.Value)
                {
                    case EFrecuencia.DIARIA:
                        diasAAgregar = 1;
                        break;
                    case EFrecuencia.SEMANAL:
                        diasAAgregar = 7;
                        break;
                    case EFrecuencia.QUINCENAL:
                        diasAAgregar = 15;
                        break;
                    case EFrecuencia.MENSUAL:
                        diasAAgregar = 30;
                        break;
                }
                DateTime fechaVencimientoSiguiente = ultimoPago.FechaVencimiento.Value.AddDays((Double)ultimoPago.DiasFacturar.Value);
                while(diasFaltantes > 0)
                {
                    var pagoNuevo = new PagoUnidadContratoRDBO()
                    {
                        ReferenciaContrato = new ReferenciaContratoBO(ultimoPago.ReferenciaContrato),
                        NumeroPago = listaPagosPendientes.Any() ? (short)(listaPagosPendientes.Last().NumeroPago + 1) : (short)(ultimoPago.NumeroPago + 1),
                        Origen = EOrigenPago.SISTEMA,
                        TipoPago = ETipoPago.NORMAL,
                        FechaVencimiento = listaPagosPendientes.Any() ? listaPagosPendientes.Last().FechaVencimiento.Value.AddDays(diasAAgregar) : fechaVencimientoSiguiente,
                        DiasFacturar = (diasFaltantes - diasAAgregar) > 0 ? diasAAgregar : diasFaltantes
                    };

                    diasFaltantes = (diasFaltantes - diasAAgregar);
                    listaPagosPendientes.Add(pagoNuevo);
                }
            }

            this.vista.ListaPagosFaltantes = listaPagosPendientes;
            this.vista.EstablecerPagosPendientes(listaPagosPendientes);
            this.vista.CantidadPagosFaltantes = listaPagosPendientes.Count;
        }
        #endregion
    }
}
