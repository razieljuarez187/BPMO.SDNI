using System;
using System.Data;
using System.Linq;
using BPMO.Basicos.BO;
using BPMO.Facade.SDNI.BR;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Contratos.BO;
using BPMO.SDNI.Contratos.BR;
using BPMO.SDNI.Contratos.FSL.BO;
using BPMO.SDNI.Contratos.FSL.BR;
using BPMO.SDNI.Contratos.Mantto.BO;
using BPMO.SDNI.Contratos.Mantto.BR;
using BPMO.SDNI.Contratos.RD.BO;
using BPMO.SDNI.Contratos.RD.BR;
using BPMO.SDNI.Contratos.VIS;
using BPMO.SDNI.Equipos.BO;
using BPMO.SDNI.Facturacion.AplicacionesPago.BR;
using BPMO.SDNI.Facturacion.InfraestructuraPagos.BO;
using BPMO.SDNI.Facturacion.InfraestructuraPagos.BOF;
using BPMO.SDNI.Facturacion.InfraestructuraPagos.BR;

namespace BPMO.SDNI.Contratos.PRE
{
    /// <summary>
    /// Presentador usado para Cambiar un contrato de Sucursal
    /// </summary>
    public class CambiarContratoSucursalPRE
    {
        #region Atributos
        /// <summary>
        /// Vista con la que interactua el presentador
        /// </summary>
        private readonly ICambiarContratoSucursalVIS vista;
        /// <summary>
        /// DataContext que proporciona acceso a la Base de Datos
        /// </summary>
        private readonly IDataContext dataContext;
        /// <summary>
        /// Controlador General de los Contratos
        /// </summary>
        private ContratoBR controlador;
        /// <summary>
        /// Nombre de la Clase, utilizada para excepciones
        /// </summary>
        private static readonly string nombreClase = typeof(CambiarContratoSucursalPRE).Name;
        #endregion
        #region Constructores
        public CambiarContratoSucursalPRE(ICambiarContratoSucursalVIS view)
        {
            if(view != null) vista = view;
            else
                throw new Exception(nombreClase + ".CambiarContratoSucursalPRE: La vista no puede ser null");
            this.dataContext = FacadeBR.ObtenerConexion();
            controlador = new ContratoBR();
        }
        #endregion
        #region Metodos
        /// <summary>
        /// Metodo que se ejecuta al inicio del proceso para preparar la interfaz y los valores iniciales
        /// </summary>
        public void PrepararCambioSucursal()
        {
            this.EstablecerSeguridad();
            this.vista.EstablecerPaqueteNavegacion(this.vista.ObtenerPaqueteNavegacion());
            this.ConsultarCompleto();
            this.vista.DesactivarCamposIniciales();
        }
        /// <summary>
        /// Obtiene el Contrato que sera Editado
        /// </summary>
        private void ConsultarCompleto()
        {
            ContratoBO contrato = new ContratoProxyBO() { ContratoID = this.vista.ContratoId };
            ContratoBO contratoCompleto = null;
            switch(this.vista.TipoContrato)
            {
                case ETipoContrato.RD:
                    ContratoRDBR controladorRD = new ContratoRDBR();
                    var contratoRd = new ContratoRDBO((ContratoBO)contrato);
                    var contratosRd = controladorRD.ConsultarCompleto(this.dataContext, contratoRd);
                    contratoCompleto = contratosRd.FirstOrDefault();

                    this.controlador = new ContratoRDBR();
                    this.vista.ObjetoAnterior = contratoCompleto != null ? controladorRD.ConsultarCompleto(this.dataContext, contratoRd).FirstOrDefault() : null;
                    break;
                case ETipoContrato.FSL:
                    ContratoFSLBR controladorFsl = new ContratoFSLBR();
                    var contratoFsl = new ContratoFSLBO() { ContratoID = this.vista.ContratoId };
                    var contratosFsl = controladorFsl.ConsultarCompleto(this.dataContext, contratoFsl);
                    contratoCompleto = contratosFsl.FirstOrDefault();

                    this.controlador = new ContratoFSLBR();
                    this.vista.ObjetoAnterior = contratoCompleto != null ? controladorFsl.ConsultarCompleto(this.dataContext, contratoFsl).FirstOrDefault() : null;
                    break;
                case ETipoContrato.CM:
                case ETipoContrato.SD:
                    ContratoManttoBR controladorMantto = new ContratoManttoBR();
                    var contratoMantto = new ContratoManttoBO((ContratoBO)contrato);
                    var contratosMantto = controladorMantto.ConsultarCompleto(this.dataContext, contratoMantto, false);
                    contratoCompleto = contratosMantto.FirstOrDefault();

                    this.controlador = new ContratoManttoBR();
                    this.vista.ObjetoAnterior = contratoCompleto != null ? controladorMantto.ConsultarCompleto(this.dataContext, contratoMantto, false).FirstOrDefault() : null;
                    break;
                default:
                    contratoCompleto = null;
                    break;
            }
            if(contratoCompleto != null)
            {
                this.vista.ContratoId = contratoCompleto.ContratoID;
                this.vista.NumeroContrato = contratoCompleto.NumeroContrato;
                this.vista.SucursalIdAntigua = contratoCompleto.Sucursal.Id;
                this.vista.SucursalNombreAntigua = contratoCompleto.Sucursal.Nombre;
                this.vista.TipoContrato = contratoCompleto.Tipo;
                this.vista.ContratoConsultado = contratoCompleto;
            }
            else
                throw new Exception(nombreClase + ".ConsultarCompleto: No se encontro el Contrato solicitado");
        }
        /// <summary>
        /// Cambia el contrato de Sucursal
        /// </summary>
        public void CambiarSucursal()
        {
            try
            {
                string s = this.ValidarCampos();
                if(!String.IsNullOrEmpty(s))
                {
                    this.vista.MostrarMensaje(s, ETipoMensajeIU.ADVERTENCIA);
                    return;
                }

                ContratoBO contrato = (ContratoBO)InterfazUsuarioDato();
                if(contrato == null)
                    throw new Exception("No se pudo obtener el contrato que sera cambiado de sucursal desde la interfaz");

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
                    throw new Exception("Se encontraron inconsistencias al Cambiar la Sucursal del Contrato.");
                }

                #endregion
                try
                {
                    bool cambioExitoso = false;
                    switch(this.vista.TipoContrato)
                    {
                        case ETipoContrato.RD:
                            this.controlador = new ContratoRDBR();
                            cambioExitoso = ((ContratoRDBR)controlador).CambiarSucursalContrato(this.dataContext, (ContratoRDBO)contrato, (ContratoRDBO)this.vista.ObjetoAnterior, this.vista.Observaciones, seguridadBo);
                            break;
                        case ETipoContrato.FSL:
                            this.controlador = new ContratoFSLBR();
                            cambioExitoso = ((ContratoFSLBR)controlador).CambiarSucursalContrato(this.dataContext, (ContratoFSLBO)contrato, (ContratoFSLBO)this.vista.ObjetoAnterior, this.vista.Observaciones, seguridadBo);
                            break;
                        case ETipoContrato.CM:
                        case ETipoContrato.SD:
                            this.controlador = new ContratoManttoBR();
                            cambioExitoso = ((ContratoManttoBR)controlador).CambiarSucursalContrato(this.dataContext, (ContratoManttoBO)contrato, (ContratoManttoBO)this.vista.ObjetoAnterior, this.vista.Observaciones, seguridadBo);
                            break;
                    }

                    if(!cambioExitoso)
                        throw new Exception("Ocurrio un problema al cambiar El contrato y las Unidades de Sucursal");

                    ModificarPagosBR modificarPagosBr = new ModificarPagosBR();
                    cambioExitoso = modificarPagosBr.CambiarPagosSucursal(this.dataContext, contrato, seguridadBo);

                    if(!cambioExitoso)
                        throw new Exception("Ocurrio un problema al cambiar Los Pagos de Sucursal");

                    this.dataContext.CommitTransaction(firma);
                    this.vista.PermitirGuardar(false);

                    this.vista.MostrarMensaje("El Contrato cambio de Sucursal con Éxito", ETipoMensajeIU.EXITO);
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
                throw new Exception(nombreClase + ".CambiarSucursal: " + ex.Message, ex.InnerException);
            }
        }
        /// <summary>
        /// Cancela el cambio de Sucursal y Redirecciona a Consultar Contratos
        /// </summary>
        public void CancelarCambio()
        {
            this.vista.LimpiarSesion();
            this.vista.RedirigirAConsulta();
        }
        /// <summary>
        /// Valida si tiene permiso para acceder a la interfaz de consulta
        /// </summary>
        private void EstablecerSeguridad()
        {
            try
            {
                //se valida que los datos del usuario y la unidad operativa no sean nulos
                if(this.vista.UsuarioId == null) throw new Exception("El identificador del usuario no debe ser nulo");
                if(this.vista.UnidadOperativa.Id == null) throw new Exception("El identificador de la Unidad Operativa no debe ser nulo ");
                //Se crea el objeto de Seguridad
                SeguridadBO seguridadBO = this.CrearObjetoSeguridad();
                //Se valida si el usuario tiene permisos para realizar la acción
                if(!FacadeBR.ExisteAccion(this.dataContext, "CAMBIARSUCURSALCONTRATO", seguridadBO) || !FacadeBR.ExisteAccion(this.dataContext, "CONSULTARCOMPLETO", seguridadBO) || !FacadeBR.ExisteAccion(this.dataContext, "CAMBIARPAGOSSUCURSAL", seguridadBO))
                    this.vista.RedirigirSinPermisoAcceso();
            }
            catch(Exception ex)
            {
                throw new Exception(nombreClase + ".EstablecerSeguridad: " + ex.Message);
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
        /// Valida los campos necesarios para poder realizar el cambio de sucursal
        /// </summary>
        /// <returns>Cadena que contiene los elementos que faltan por completar</returns>
        private string ValidarCampos()
        {
            String s = String.Empty;

            if(this.vista.SucursalIdNueva == null)
                s = String.IsNullOrEmpty(s) ? "SucursalId" : ", SucursalId";
            if(this.vista.SucursalNombreNueva == null)
                s = String.IsNullOrEmpty(s) ? "Nombre Sucursal" : ", Nombre Sucursal";
            if(!String.IsNullOrEmpty(s))
                return "Los siguientes campos no pueden estar vacios: " + s;

            if(this.vista.SucursalIdAntigua == this.vista.SucursalIdNueva)
                return "El contrato ya pertenece a la Sucursal de " + this.vista.SucursalNombreAntigua + ", seleccione otra Sucursal";

            PagoUnidadContratoBR pagoBr = new PagoUnidadContratoBR();
            PagoUnidadContratoBOF pago = new PagoUnidadContratoBOF() { ReferenciaContrato = new ReferenciaContratoBO() { ReferenciaContratoID = this.vista.ContratoId }, EnviadoFacturacion = false, Activo = true };
            DateTime fechaHoy = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 23, 59, 59);
            var listaPagosPendientes = pagoBr.Consultar(this.dataContext, pago);
            if(listaPagosPendientes.Any())
            {
                if(listaPagosPendientes.Any(x => x.FechaVencimiento < fechaHoy))
                    return "Aún existen pagos pendientes por enviar a Facturar del Día de Hoy. Envie los pagos a facturar para poder cambiar el Contrato de Sucursal";
            }

            return String.Empty;
        }
        /// <summary>
        /// Obtiene los datos que seran enviados a Actualizar
        /// </summary>
        /// <returns>Objeto que sera actualizado</returns>
        private Object InterfazUsuarioDato()
        {
            ContratoBO contrato = this.vista.ContratoConsultado;
            SucursalBO nuevaSucursal = new SucursalBO() {Auditoria = new AuditoriaBO(), UnidadOperativa = this.vista.UnidadOperativa };

            nuevaSucursal.Id = this.vista.SucursalIdNueva != null ? this.vista.SucursalIdNueva : null;
            nuevaSucursal.Nombre = this.vista.SucursalNombreNueva != null ? this.vista.SucursalNombreNueva : null;
            DateTime? fua = DateTime.Now;
            Int32? uua = this.vista.UsuarioId;

            contrato.Sucursal = nuevaSucursal;
            contrato.FUA = fua;
            contrato.UUA = uua;
            foreach(ILineaContrato linea in contrato.LineasContrato)
            {
                linea.Equipo.Sucursal = nuevaSucursal;
                linea.Equipo.FUA = fua;
                linea.Equipo.UUA = uua;
                if((linea.Equipo as UnidadBO).EquiposAliados != null && (linea.Equipo as UnidadBO).EquiposAliados.Any())
                {
                    UnidadBO unidad = (linea.Equipo as UnidadBO);
                    foreach(EquipoAliadoBO equipoAliado in unidad.EquiposAliados)
                    {
                        equipoAliado.Sucursal = nuevaSucursal;
                        equipoAliado.FUA = fua;
                        equipoAliado.UUA = uua;
                    }
                }
            }

            return contrato;
        }
        #region Métodos para el Buscador
        /// <summary>
        /// Prepara un BO para la Busqueda en su respectivo catalogo
        /// </summary>
        /// <param name="catalogo">catalogo donde se realizara la busqueda</param>
        /// <returns>Objeto con los parámetros de búsqueda</returns>
        public object PrepararBOBuscador(string catalogo)
        {
            object obj = null;

            switch(catalogo)
            {
                case "Sucursal":
                    Facade.SDNI.BOF.SucursalBOF sucursal = new Facade.SDNI.BOF.SucursalBOF();
                    sucursal.UnidadOperativa = this.vista.UnidadOperativa;
                    sucursal.Nombre = this.vista.SucursalNombreNueva;
                    sucursal.Usuario = new UsuarioBO() { Id = this.vista.UsuarioId };
                    obj = sucursal;
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
            switch(catalogo)
            {
                case "Sucursal":
                    SucursalBO sucursal = (SucursalBO)selecto;
                    if(sucursal != null && sucursal.Id != null)
                        this.vista.SucursalIdNueva = sucursal.Id;
                    else
                        this.vista.SucursalIdNueva = null;

                    if(sucursal != null && sucursal.Nombre != null)
                        this.vista.SucursalNombreNueva = sucursal.Nombre;
                    else
                        this.vista.SucursalNombreNueva = null;
                    break;
            }
        }
        #endregion
        #endregion
    }
}
