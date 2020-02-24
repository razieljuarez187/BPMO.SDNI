//Satisface al CU030 - Registrar Terminación de Contrato de Mantenimiento
using System;
using System.Collections.Generic;
using BPMO.Basicos.BO;
using BPMO.Facade.SDNI.BR;

using BPMO.Primitivos.Enumeradores;
using BPMO.Patterns.Creational.DataContext;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Contratos.Mantto.BO;
using BPMO.SDNI.Contratos.BO;
using BPMO.SDNI.Contratos.Mantto.BR;
using BPMO.SDNI.Contratos.Mantto.VIS;

namespace BPMO.SDNI.Contratos.Mantto.PRE
{
    public class CancelarContratoManttoPRE
    {
        #region Atributos
        /// <summary>
        /// Nombre de la clase para agregar a los mensajes de Error
        /// </summary>
        private string nombreClase = "CancelarContratoManttoPRE";

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
        private readonly ICancelarContratoManttoVIS vista;

        private ContratoManttoBR controlador;
        #endregion

        #region Constructores

        public CancelarContratoManttoPRE(ICancelarContratoManttoVIS vistaActual, IucHerramientasManttoVIS vistaHerramientas, IucContratoManttoVIS vistaContrato, IucLineaContratoManttoVIS vistaLinea)
        {
            try
            {
                this.vista = vistaActual;

                this.presentadorContrato = new ucContratoManttoPRE(vistaContrato, vistaLinea);
                this.presentadorHerramientas = new ucHerramientasManttoPRE(vistaHerramientas);

                this.controlador = new ContratoManttoBR();
                this.dctx = FacadeBR.ObtenerConexion();
            }
            catch (Exception ex)
            {
                this.vista.MostrarMensaje("Inconsistencia en los parámetros de configuración", ETipoMensajeIU.ERROR, this.nombreClase + ".CancelarContratoManttoPRE:" + ex.Message);
            }
        }

        #endregion

        #region Métodos

        public void RealizarPrimeraCarga()
        {
            this.PrepararVisualizacion();

            this.EstablecerDatosNavegacion(this.vista.ObtenerPaqueteNavegacion("ContratoManttoBO"));

            this.ConsultarCompleto();

            this.EstablecerConfiguracionInicial();
            this.presentadorContrato.OcultarDatosAdicionales(true);
            this.presentadorContrato.OcultarPersonasCliente(true);
            this.presentadorHerramientas.vista.MarcarOpcionCancelarContrato();
            this.presentadorHerramientas.vista.OcultarPlantillas();
            this.ValidarEstatusContrato();
            this.presentadorHerramientas.DeshabilitarMenuEditar();
            this.presentadorHerramientas.DeshabilitarMenuImprimir();
            this.presentadorHerramientas.DeshabilitarMenuBorrar();

            this.EstablecerSeguridad();
        }

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

        private void EstablecerConfiguracionInicial()
        {
            this.vista.FUA = DateTime.Now;
            this.vista.UUA = this.vista.UsuarioID;
            this.vista.FechaCancelacion = this.vista.FUA;
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
                if (!FacadeBR.ExisteAccion(this.dctx, "CANCELAR", seguridadBO))
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
            this.presentadorHerramientas.Inicializar();
            this.presentadorContrato.PrepararVisualizacion();
        }

        public void Regresar()
        {
            ContratoManttoBO bo = (ContratoManttoBO)this.vista.UltimoObjeto;
            this.LimpiarSesion();

            this.vista.LimpiarPaqueteNavegacion("ContratoManttoBO");
            this.vista.EstablecerPaqueteNavegacion("ContratoManttoBO", bo);

            this.vista.RedirigirADetalles();
        }

        public void CancelarContrato()
        {
            string s;
            if ((s = this.ValidarCampos()) != null)
            {
                this.vista.MostrarMensaje(s, ETipoMensajeIU.ADVERTENCIA, null);
                return;
            }

            try
            {
                //Se obtiene la información a partir de la Interfaz de Usuario
                ContratoManttoBO bo = (ContratoManttoBO)this.InterfazUsuarioADato();

                //Se crea el objeto de seguridad
                UsuarioBO usuario = new UsuarioBO() { Id = this.vista.UsuarioID };
                AdscripcionBO adscripcion = new AdscripcionBO() { UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaID } };
                SeguridadBO seguridadBO = new SeguridadBO(Guid.Empty, usuario, adscripcion);

                //Se actualiza en la base de datos
                this.controlador.Cancelar(this.dctx, bo, (ContratoManttoBO)this.vista.UltimoObjeto, seguridadBO);

                //Se despliega la información en la Interfaz de Usuario
                this.DatoAInterfazUsuario(bo);
                this.vista.UltimoObjeto = bo;

                this.LimpiarSesion();
                this.vista.LimpiarPaqueteNavegacion("ContratoManttoBO");
                this.vista.EstablecerPaqueteNavegacion("ContratoManttoBO", new ContratoManttoBO() { ContratoID = this.vista.ContratoID });
                this.vista.RedirigirADetalles();
            }
            catch (Exception ex)
            {
                throw new Exception(nombreClase + ".CancelarContrato:" + ex.Message);
            }
        }

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

            bo.CierreContrato.Fecha = this.vista.FechaCancelacion;
            bo.CierreContrato.Observaciones = this.vista.ObservacionesCancelacion;
            bo.CierreContrato.Usuario.Id = this.vista.UsuarioID;

            ((CierreContratoManttoBO)bo.CierreContrato).Motivo = this.vista.MotivoCancelacion;

            return bo;
        }

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
            this.vista.FechaCancelacion = bo.CierreContrato.Fecha;
            this.vista.ObservacionesCancelacion = bo.CierreContrato.Observaciones;
            if (bo.CierreContrato is CierreContratoManttoBO)
                this.vista.MotivoCancelacion = ((CierreContratoManttoBO)bo.CierreContrato).Motivo;
            else
                this.vista.MotivoCancelacion = null;
        }

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
            if (string.IsNullOrEmpty(this.vista.ObservacionesCancelacion) || string.IsNullOrWhiteSpace(this.vista.ObservacionesCancelacion))
                s += "Observaciones Cancelación, ";
            if (this.vista.FechaCancelacion == null)
                s += "Fecha Cancelación, ";
            if (string.IsNullOrEmpty(this.vista.MotivoCancelacion) || string.IsNullOrWhiteSpace(this.vista.MotivoCancelacion))
                s += "Motivo Cancelación, ";

            if (s != null && s.Trim().CompareTo("") != 0)
                return "Los siguientes campos no pueden estar vacíos: \n" + s.Substring(0, s.Length - 2);

            if (vista.FechaCancelacion < DateTime.Today)
                return "La fecha de cancelación no puede ser menor a la fecha actual";
            if (vista.FechaCancelacion < vista.FechaInicioContrato)
                return "La fecha de cancelación no puede ser menor a la fecha de inicio del contrato";

            if ((s = this.ValidarContrato()) != null)
                return s;

            return null;
        }

        public string ValidarContrato()
        {
            string s = string.Empty;

            if (this.vista.EstatusID == null)
                s += "Estatus, ";

            if (s != null && s.Trim().CompareTo("") != 0)
                return "Los siguientes campos no pueden estar vacíos: \n" + s.Substring(0, s.Length - 2);

            EEstatusContrato estatus =
                (EEstatusContrato)Enum.Parse(typeof(EEstatusContrato), this.vista.EstatusID.ToString());
            if (!(estatus == EEstatusContrato.Borrador))
                return
                    "El contrato no puede cancelarse a menos que esté En Borrador.";

            return null;
        }

        private void LimpiarSesion()
        {
            this.vista.LimpiarSesion();
        }

        private void ValidarEstatusContrato()
        {
            EEstatusContrato estatus =
                (EEstatusContrato)Enum.Parse(typeof(EEstatusContrato), this.vista.EstatusID.ToString());
            if (!(estatus == EEstatusContrato.Borrador))
            {
                this.vista.DeshabilitarBotonCancelar();
                this.vista.MostrarMensaje("El contrato no se encuentra en Borrador, no se puede Cancelar", ETipoMensajeIU.ADVERTENCIA, null);
            }
        }
        #endregion
    }
}
