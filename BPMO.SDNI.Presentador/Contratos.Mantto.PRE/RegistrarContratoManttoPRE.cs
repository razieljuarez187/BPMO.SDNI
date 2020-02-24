//Satisface al CU027 - Registrar Contrato de Mantenimiento
// Satisface a la solicitud de Cambio SC0001
//Satisface al caso de uso CU003 - Calcular Monto a Facturar CM y SD
// Satisface a la solución del RI0006
// Satisface a la solución del RI0008
using System;
using System.Collections.Generic;
using System.Data;
using BPMO.Basicos.BO;
using BPMO.Facade.SDNI.BR;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Comun.BR;
using BPMO.SDNI.Comun.PRE;
using BPMO.SDNI.Comun.VIS;
using BPMO.SDNI.Contratos.BO;
using BPMO.SDNI.Contratos.Mantto.BO;
using BPMO.SDNI.Contratos.Mantto.BR;
using BPMO.SDNI.Contratos.Mantto.VIS;
using BPMO.SDNI.Facturacion.AplicacionesPago.BR;

namespace BPMO.SDNI.Contratos.Mantto.PRE
{
    /// <summary>
    /// Presentador Registro Contrato de Mantto
    /// </summary>
    public class RegistrarContratoManttoPRE
    {
        #region Atributos
        /// <summary>
        /// Contexto de conexion de datos
        /// </summary>
        private IDataContext dctx = null;
        /// <summary>
        /// Controlador principal
        /// </summary>
        private ContratoManttoBR controlador;
        /// <summary>
        /// Vista del Registro de Contrato
        /// </summary>
        private IRegistrarContratoManttoVIS vista;
        /// <summary>
        /// PResentador de datos del contrato
        /// </summary>
        private ucContratoManttoPRE presentadorDatosContrato;
        /// <summary>
        /// Presentador del Control de Documentos
        /// </summary>
        private ucCatalogoDocumentosPRE presentadorDocumentos;
        /// <summary>
        /// Nombre de la Clase
        /// </summary>
        private string nombreClase = "RegistrarContratoManttoPRE";
        #endregion

        #region Constructores
        /// <summary>
        /// Constructor por defecto del presentador
        /// </summary>
        /// <param name="view">Vista de Registro</param>
        /// <param name="viewDatosContrato">Vista de Datos de Contrato</param>
        /// <param name="viewDocumentos">Vista del Control de Documentos</param>
        /// <param name="viewHerramientas">Vista de la Barra de Herramienta</param>
        public RegistrarContratoManttoPRE(IRegistrarContratoManttoVIS view, IucContratoManttoVIS viewDatosContrato, IucLineaContratoManttoVIS viewLinea, IucCatalogoDocumentosVIS viewDocumentos)
        {
            try
            {
                this.vista = view;
                this.presentadorDatosContrato = new ucContratoManttoPRE(viewDatosContrato, viewLinea);
                this.presentadorDocumentos = new ucCatalogoDocumentosPRE(viewDocumentos);

                this.controlador = new ContratoManttoBR();
                this.dctx = FacadeBR.ObtenerConexion();
            }
            catch (Exception ex)
            {
                this.vista.MostrarMensaje("Inconsistencia en los parámetros de configuración", ETipoMensajeIU.ERROR, this.nombreClase + ".ucContratoManttoPRE:" + ex.Message);
            }
        }
        #endregion

        #region Métodos
        /// <summary>
        /// Prepara la Vista para un Nuevo Registro de Contrato
        /// </summary>
        public void PrepararNuevo()
        {
            this.LimpiarSesion();

            this.vista.PrepararNuevo();
            this.presentadorDatosContrato.PrepararNuevo();
            this.presentadorDocumentos.ModoEditable(true);

            this.EstablecerInformacionInicial();

            this.EstablecerSeguridad();
        }
        /// <summary>
        /// Establece la información Inicial en la Vista
        /// </summary>
        private void EstablecerInformacionInicial()
        {
            //Se establecen los tipos de archivos permitidos para adjuntar al contrato
            List<TipoArchivoBO> lstTiposArchivo = new TipoArchivoBR().Consultar(this.dctx, new TipoArchivoBO());
            this.presentadorDocumentos.EstablecerTiposArchivo(lstTiposArchivo);
            this.presentadorDocumentos.RequiereObservaciones(false);

            #region Configuraciones de Unidad Operativa
            //Obtener las configuraciones de la unidad operativa
            List<ConfiguracionUnidadOperativaBO> lstConfigUO = new ModuloBR().ConsultarConfiguracionUnidadOperativa(this.dctx, new ConfiguracionUnidadOperativaBO() { UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaID } }, this.vista.ModuloID);
            if (lstConfigUO.Count <= 0)
                throw new Exception("No se encontraron las configuraciones del sistema para la unidad operativa en la que trabaja.");

            //Establecer las configuraciones de la unidad operativa
            this.vista.RepresentanteEmpresa = lstConfigUO[0].Representante;
            #endregion
        }
        /// <summary>
        /// Establece las opciones permitidas en base a la seguridad del usuario
        /// </summary>
        private void EstablecerSeguridad()
        {
            try
            {
                //se valida que los datos del usuario y la unidad operativa no sean nulos
                if (this.vista.UsuarioID == null) throw new Exception("El identificador del usuario no debe ser nulo");
                if (this.vista.UnidadOperativaID == null) throw new Exception("El identificador de la Unidad Operativa no debe ser nulo ");

                //Se crea el objeto de seguridad
                UsuarioBO usr = new UsuarioBO { Id = this.vista.UsuarioID };
                AdscripcionBO sdscripcion = new AdscripcionBO { UnidadOperativa = new UnidadOperativaBO { Id = this.vista.UnidadOperativaID } };
                SeguridadBO seguridadBO = new SeguridadBO(Guid.Empty, usr, sdscripcion);

                //Se obtienen las acciones a las cuales el usuario tiene permiso en este proceso
                List<CatalogoBaseBO> lst = FacadeBR.ConsultarAccion(this.dctx, seguridadBO);

                //Se valida si el usuario tiene permiso para registrar una llanta
                if (!this.ExisteAccion(lst, "INSERTARCOMPLETO"))
                    this.vista.RedirigirSinPermisoAcceso();

                #region RI0008
                //Se valida si el usuario tiene permiso para registrar contrato en curso
                if (!this.ExisteAccion(lst, "GENERARPAGOS"))
                    vista.PermitirGuardarTerminado(false);
                #endregion
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".EstablecerSeguridad:" + ex.Message);
            }
        }
        /// <summary>
        /// Verifica si existe una acción en una lista de acciones proporcionada
        /// </summary>
        /// <param name="acciones">Lista de Acciones</param>
        /// <param name="nombreAccion">Acción a verificar</param>
        /// <returns></returns>
        private bool ExisteAccion(List<CatalogoBaseBO> acciones, string nombreAccion)
        {
            if (acciones != null && acciones.Count > 0 && acciones.Exists(p => p != null && p.Nombre != null && p.Nombre.Trim().ToUpper() == nombreAccion.Trim().ToUpper()))
                return true;

            return false;
        }
        /// <summary>
        /// Cancela la captura del contrato
        /// </summary>
        public void CancelarRegistro()
        {
            this.LimpiarSesion();
            this.vista.RedirigirAConsulta();
        }
        /// <summary>
        /// Registra el contrato con estatus Borrador
        /// </summary>
        public void RegistrarBorrador()
        {
            string s;
            if ((s = this.ValidarCamposBorrador()) != null)
            {
                this.vista.MostrarMensaje(s, ETipoMensajeIU.ADVERTENCIA, null);
                return;
            }

            this.vista.EstatusID = (int)EEstatusContrato.Borrador;

            this.Registrar();

            this.vista.EstablecerPaqueteNavegacion("ContratoManttoBO", new ContratoManttoBO() { ContratoID = this.vista.ContratoID });
            this.vista.RedirigirADetalles();
        }
        /// <summary>
        /// Registra el contrato con Estatus En Curso
        /// </summary>
        public void RegistrarTerminada()
        {
            string s;
            if ((s = this.ValidarCamposRegistro()) != null)
            {
                this.vista.MostrarMensaje(s, ETipoMensajeIU.ADVERTENCIA, null);
                return;
            }

            this.vista.EstatusID = (int)EEstatusContrato.EnCurso;

            this.Registrar();

            this.vista.EstablecerPaqueteNavegacion("ContratoManttoBO", new ContratoManttoBO() { ContratoID = this.vista.ContratoID });
            this.vista.RedirigirADetalles();
        }
        /// <summary>
        /// Registra un Contrato RD
        /// </summary>
        private void Registrar()
        {
            #region Se inicia la Transaccion
            dctx.SetCurrentProvider("Outsourcing");
            Guid firma = Guid.NewGuid();
            try
            {
                dctx.OpenConnection(firma);
                dctx.BeginTransaction(firma);
            }

            catch(Exception)
            {
                if(dctx.ConnectionState == ConnectionState.Open)
                    dctx.CloseConnection(firma);
                throw new Exception("Se encontraron inconsistencias al insertar el Contrato.");
            }
            #endregion

            try
            {
                //Se obtiene la información a partir de la Interfaz de Usuario
                ContratoManttoBO bo = (ContratoManttoBO)this.InterfazUsuarioADato();

                //Se crea el objeto de seguridad
                UsuarioBO usuario = new UsuarioBO() { Id = this.vista.UsuarioID };
                AdscripcionBO adscripcion = new AdscripcionBO() { UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaID } };
                SeguridadBO seguridadBO = new SeguridadBO(Guid.Empty, usuario, adscripcion);

                //Se inserta en la base de datos
                this.controlador.InsertarCompleto(this.dctx, bo, seguridadBO);

                //Se consulta lo insertado para recuperar los ID
                DataSet ds = this.controlador.ConsultarSet(this.dctx, bo);
                if (ds.Tables[0].Rows.Count <= 0)throw new Exception("Al consultar lo insertado no se encontraron coincidencias.");
                if (ds.Tables[0].Rows.Count > 1)
                    throw new Exception("Al consultar lo insertado se encontró más de una coincidencia.");

                bo.ContratoID = this.controlador.DataRowToContratoManttoBO(ds.Tables[0].Rows[0]).ContratoID;

                #region SC0001 BEP1401 - Registra los pagos del Contrato
                if (bo.Estatus == EEstatusContrato.EnCurso)
                {
                    GeneradorPagosManttoBR generadorPagos = new GeneradorPagosManttoBR();
                    generadorPagos.GenerarPagos(dctx, bo, seguridadBO,true);
                }

                #endregion

                //Se despliega la información en la Interfaz de Usuario
                this.DatoAInterfazUsuario(bo);

                dctx.CommitTransaction(firma);
            }
            catch (Exception ex)
            {
                dctx.RollbackTransaction(firma);
                throw new Exception(this.nombreClase + ".Registrar:" + ex.Message);
            }
            finally
            {
                if(dctx.ConnectionState == ConnectionState.Open)
                    dctx.CloseConnection(firma);
            }
        }
        /// <summary>
        /// Obtiene los datos de la Vista
        /// </summary>
        /// <returns></returns>
        private object InterfazUsuarioADato()
        {
            ContratoManttoBO bo = new ContratoManttoBO();
            bo.Cliente = new CuentaClienteIdealeaseBO();
            bo.Cliente.UnidadOperativa = new UnidadOperativaBO();
            bo.Divisa = new DivisaBO();
            bo.Divisa.MonedaDestino = new MonedaBO();
            bo.Sucursal = new SucursalBO();
            bo.Sucursal.UnidadOperativa = new UnidadOperativaBO();
            bo.LineasContrato = new List<ILineaContrato>();

            //Información General
            if (vista.TipoContratoID != null)
                bo.Tipo = (ETipoContrato)Enum.Parse(typeof(ETipoContrato), vista.TipoContratoID.ToString());
            if (vista.SucursalID != null)
                bo.Sucursal.Id = vista.SucursalID;
            if (vista.SucursalNombre != null)
                bo.Sucursal.Nombre = vista.SucursalNombre;
            if (vista.UnidadOperativaID != null)
            {
                bo.Sucursal.UnidadOperativa.Id = vista.UnidadOperativaID;
                bo.Cliente.UnidadOperativa.Id = vista.UnidadOperativaID;
            }
            if (!String.IsNullOrEmpty(vista.UnidadOperativaNombre))
                bo.Sucursal.UnidadOperativa.Nombre = vista.UnidadOperativaNombre;
            bo.Representante = vista.RepresentanteEmpresa;
            if (vista.CodigoMoneda != null)
                bo.Divisa.MonedaDestino.Codigo = vista.CodigoMoneda;
            if (vista.FechaContrato != null)
                bo.FechaContrato = vista.FechaContrato;

            //Datos del Cliente
            if (vista.CuentaClienteID != null)
                bo.Cliente.Id = vista.CuentaClienteID;
            if(!string.IsNullOrEmpty(vista.CuentaClienteNombre) && !string.IsNullOrWhiteSpace(vista.CuentaClienteNombre))
                bo.Cliente.Nombre = vista.CuentaClienteNombre;
            if (vista.RepresentantesLegales != null)
                bo.RepresentantesLegales = vista.RepresentantesLegales.ConvertAll(s => (PersonaBO)s);
            if (vista.ObligadosSolidarios != null)
                bo.ObligadosSolidarios = vista.ObligadosSolidarios.ConvertAll(s => (PersonaBO)s);
            bo.SoloRepresentantes = vista.SoloRepresentantes;
            bo.ObligadosComoAvales = vista.ObligadosComoAvales;
            if (vista.Avales != null)
                bo.Avales = vista.Avales;
            if (bo.ObligadosComoAvales != null && bo.ObligadosComoAvales == true)
            {
                if (vista.ObligadosSolidarios != null)
                    bo.Avales = vista.ObligadosSolidarios.ConvertAll(s => ObligadoAAval(s));
            }

            #region Dirección del Cliente
            DireccionClienteBO direccion = new DireccionClienteBO
            {
                Id = vista.DireccionClienteID,
                Ubicacion =
                    new UbicacionBO
                    {
                        Pais = new PaisBO { Codigo = vista.ClienteDireccionPais },
                        Municipio = new MunicipioBO { Codigo = vista.ClienteDireccionMunicipio },
                        Estado = new EstadoBO { Codigo = vista.ClienteDireccionEstado },
                        Ciudad = new CiudadBO { Codigo = vista.ClienteDireccionCiudad }
                    },
                CodigoPostal = vista.ClienteDireccionCodigoPostal,
                Calle = vista.ClienteDireccionCalle,
                Colonia = vista.ClienteDireccionColonia
            };

            bo.Cliente.RemoverDirecciones();
            bo.Cliente.Agregar(direccion);
            #endregion

            bo.Plazo = vista.Plazo;
            bo.FechaInicioContrato = vista.FechaInicioContrato;

            if (vista.LineasContrato != null)
            {
                bo.LineasContrato = vista.LineasContrato.ConvertAll(s => (ILineaContrato) s);
                #region RI0006
                foreach (var linea in bo.LineasContrato)
                {
                    linea.LineaContratoID = null;
                } 
                #endregion
            }
            bo.UbicacionTaller = vista.UbicacionTaller;
            bo.DepositoGarantia = vista.DepositoGarantia;
            bo.ComisionApertura = vista.ComisionApertura;
            if (vista.IncluyeSeguroID != null)
                bo.IncluyeSeguro = (ETipoInclusion)Enum.Parse(typeof(ETipoInclusion), vista.IncluyeSeguroID.ToString());
            if (vista.IncluyeLavadoID != null)
                bo.IncluyeLavado = (ETipoInclusion)Enum.Parse(typeof(ETipoInclusion), vista.IncluyeLavadoID.ToString());
            if (vista.IncluyePinturaRotulacionID != null)
                bo.IncluyePinturaRotulacion = (ETipoInclusion)Enum.Parse(typeof(ETipoInclusion), vista.IncluyePinturaRotulacionID.ToString());
            if (vista.IncluyeLlantasID != null)
                bo.IncluyeLlantas = (ETipoInclusion)Enum.Parse(typeof(ETipoInclusion), vista.IncluyeLlantasID.ToString());
            bo.DireccionAlmacenaje = vista.DireccionAlmacenaje;

            if (vista.DatosAdicionales != null)
                bo.DatosAdicionalesAnexo = vista.DatosAdicionales;

            #region Archivos Adjuntos
            List<ArchivoBO> adjuntos = presentadorDocumentos.Vista.ObtenerArchivos() ?? new List<ArchivoBO>();
            foreach (ArchivoBO adjunto in adjuntos)
            {
                if (bo.Tipo != null && bo.Tipo == ETipoContrato.CM)
                    adjunto.TipoAdjunto = ETipoAdjunto.ContratoMantenimiento;
                if (bo.Tipo != null && bo.Tipo == ETipoContrato.SD)
                    adjunto.TipoAdjunto = ETipoAdjunto.ContratoServicioDedicado;
                adjunto.Auditoria = new AuditoriaBO
                {

                    FC = vista.FC,
                    UC = vista.UC,
                    FUA = vista.FUA,
                    UUA = vista.UUA
                };
            }
            bo.DocumentosAdjuntos = adjuntos;
            #endregion

            if (vista.Observaciones != null)
                bo.Observaciones = vista.Observaciones;
            if (vista.EstatusID != null)
                bo.Estatus = (EEstatusContrato)Enum.Parse(typeof(EEstatusContrato), vista.EstatusID.ToString());
            if (vista.FC != null)
                bo.FC = vista.FC;
            if (vista.UC != null)
                bo.UC = vista.UC;
            if (vista.FUA != null)
                bo.FUA = vista.FUA;
            if (vista.UUA != null)
                bo.UUA = vista.UUA;

            return bo;
        }
        /// <summary>
        /// Despliega los datos a la Vista
        /// </summary>
        /// <param name="obj"></param>
        private void DatoAInterfazUsuario(object obj)
        {
            ContratoManttoBO bo = (ContratoManttoBO)obj;

            this.vista.ContratoID = bo.ContratoID;
            this.vista.NumeroContrato = bo.NumeroContrato;
        }
        /// <summary>
        /// Obtiene un Aval a partir de un Obligado solidario
        /// </summary>
        /// <param name="obligado">obligado solidario a convertir en aval</param>
        /// <returns></returns>
        private AvalBO ObligadoAAval(ObligadoSolidarioBO obligado)
        {
            if (obligado == null) return null;

            AvalBO aval;

            switch (obligado.TipoObligado)
            {
                case ETipoObligadoSolidario.Fisico:
                    aval = new AvalFisicoBO(obligado);
                    break;
                case ETipoObligadoSolidario.Moral:
                    aval = new AvalMoralBO(obligado);
                    if (obligado is ObligadoSolidarioMoralBO && ((ObligadoSolidarioMoralBO)obligado).Representantes != null)
                        ((AvalMoralBO)aval).Representantes = new List<RepresentanteLegalBO>(((ObligadoSolidarioMoralBO)obligado).Representantes);
                    break;
                default:
                    aval = new AvalProxyBO(obligado);
                    break;
            }

            return aval;
        }
        /// <summary>
        /// Valida los campos requeridos para un contrato con estatus Borrador
        /// </summary>
        /// <returns></returns>
        public string ValidarCamposBorrador()
        {
            string s = string.Empty;

            if ((s = this.presentadorDatosContrato.ValidarCamposBorrador()) != null)
                return s;

            if (this.vista.UC == null)
                s += "Usuario de Creación, ";
            if (this.vista.FC == null)
                s += "Fecha de Creación, ";
            if (this.vista.UUA == null)
                s += "Usuario de Última Actualización, ";
            if (this.vista.FUA == null)
                s += "Fecha de Última Actualización, ";
            if (this.vista.TipoContratoID == null)
                s += "Tipo de Contrato";

            if (s != null && s.Trim().CompareTo("") != 0)
                return "Los siguientes campos no pueden estar vacíos: \n" + s.Substring(0, s.Length - 2);

            return null;
        }
        /// <summary>
        /// Valida los campos Requeridos para Registrar un Contrato en Curso
        /// </summary>
        /// <returns></returns>
        public string ValidarCamposRegistro()
        {
            string s = string.Empty;

            if ((s = this.presentadorDatosContrato.ValidarCamposRegistro()) != null)
                return s;

            if (this.vista.UC == null)
                s += "Usuario de Creación, ";
            if (this.vista.FC == null)
                s += "Fecha de Creación, ";
            if (this.vista.UUA == null)
                s += "Usuario de Última Actualización, ";
            if (this.vista.FUA == null)
                s += "Fecha de Última Actualización, ";
            if (this.vista.TipoContratoID == null)
                s += "Tipo de Contrato";

            if (s != null && s.Trim().CompareTo("") != 0)
                return "Los siguientes campos no pueden estar vacíos: \n" + s.Substring(0, s.Length - 2);

            return null;
        }
        /// <summary>
        /// Limpia los datos de la memoria de la Vista
        /// </summary>
        private void LimpiarSesion()
        {
            this.vista.LimpiarSesion();
            this.presentadorDatosContrato.LimpiarSesion();
            this.presentadorDocumentos.LimpiarSesion();
        }
        #endregion
    }
}
