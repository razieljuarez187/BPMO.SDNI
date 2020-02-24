// Satisface al CU002 - Editar Contrato Renta Diaria
// Satisface a la solicitud de Cambio SC0001
// Satisface al Reporte de Inconsistencia RI0008
// Satisface al Reporte de Inconsistencia RI0016
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
using BPMO.SDNI.Comun.BR;
using BPMO.SDNI.Comun.PRE;
using BPMO.SDNI.Comun.VIS;
using BPMO.SDNI.Contratos.BO;
using BPMO.SDNI.Contratos.BR;
using BPMO.SDNI.Contratos.RD.BO;
using BPMO.SDNI.Contratos.RD.BOF;
using BPMO.SDNI.Contratos.RD.BR;
using BPMO.SDNI.Contratos.RD.VIS;
using BPMO.SDNI.Equipos.BO;

namespace BPMO.SDNI.Contratos.RD.PRE
{
    /// <summary>
    /// Presentador de la Edición del Contrato RD
    /// </summary>
    public class EditarContratoRDPRE
    {
        #region Atributos
        /// <summary>
        /// Contexto de conexión a los datos
        /// </summary>
        private IDataContext dctx = null;
        /// <summary>
        /// controlador principal
        /// </summary>
        private ContratoRDBR controlador;
        /// <summary>
        /// Vista de la Edición
        /// </summary>
        private IEditarContratoRDVIS vista;
        /// <summary>
        /// Presentador de los Datos del Contrato
        /// </summary>
        private ucContratoRDPRE presentadorDatosContrato;
        /// <summary>
        /// Presentador del Control de Documento
        /// </summary>
        private ucCatalogoDocumentosPRE presentadorDocumentos;
        /// <summary>
        /// Presentador de la Barra de Herramientas
        /// </summary>
        private ucHerramientasRDPRE presentadorHerramientas;
        /// <summary>
        /// Nombre de la Clase
        /// </summary>
        private string nombreClase = "EditarContratoRDPRE";
        #endregion

        #region Constructores
        /// <summary>
        /// Constructor por defecto del presentador
        /// </summary>
        /// <param name="view">Vista de Edición</param>
        /// <param name="viewDatosContrato">Vista de Datos de Contrato</param>
        /// <param name="viewDocumentos">Vista del Control de Documentos</param>
        /// <param name="viewHerramientas">Vista de la Barra de Herramienta</param>
        public EditarContratoRDPRE(IEditarContratoRDVIS view, IucContratoRDVIS viewDatosContrato, IucCatalogoDocumentosVIS viewDocumentos, IucHerramientasRDVIS viewHerramientas)
        {
            try
            {
                this.vista = view;

                this.presentadorDatosContrato = new ucContratoRDPRE(viewDatosContrato);
                this.presentadorDocumentos = new ucCatalogoDocumentosPRE(viewDocumentos);
                this.presentadorHerramientas = new ucHerramientasRDPRE(viewHerramientas);

                this.controlador = new ContratoRDBR();
                this.dctx = FacadeBR.ObtenerConexion();
            }
            catch (Exception ex)
            {
                this.vista.MostrarMensaje("Inconsistencia en los parámetros de configuración", ETipoMensajeIU.ERROR, this.nombreClase + ".EditarContratoRDPRE:" + ex.Message);
            }
        }
        #endregion

        #region Métodos
        /// <summary>
        /// Prepara la primer carga de información del contrato a editar
        /// </summary>
        public void RealizarPrimeraCarga()
        {
            try {
                ContratoRDBO bo = (ContratoRDBO)this.vista.ObtenerPaqueteNavegacion("UltimoContratoRDBO");

               
                this.presentadorDatosContrato.CargarSucursalesAutorizadas((List<SucursalBO>)vista.ObtenerPaqueteNavegacion("SucursalesAutorizadas"));
                this.DatoAInterfazUsuario(bo);
                this.PrepararEdicion();
                this.presentadorDatosContrato.PermitirAgregarProductoServicio(this.vista.EquipoID != null && this.vista.UnidadID != null);
                
                this.presentadorHerramientas.vista.OcultarImprimirPlantilla();
                this.presentadorHerramientas.vista.OcultarImprimirPlantillaCheckList();
                this.presentadorHerramientas.vista.HabilitarOpcionesEdicion();
                this.presentadorHerramientas.DeshabilitarMenuCerrar();
                this.presentadorHerramientas.DeshabilitarMenuBorrar();
                this.presentadorHerramientas.DeshabilitarMenuImprimir();
                this.presentadorHerramientas.DeshabilitarMenuDocumentos();
                this.presentadorHerramientas.vista.MarcarOpcionEditarContrato();
                this.presentadorDocumentos.RequiereObservaciones(false);

                this.EstablecerConfiguracionInicial();
                this.EstablecerSeguridad();
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".RealizarPrimeraCarga: " + ex.Message);
            }
        }
        private ContratoRDBO ConsultarCompleto() {
            try {
                ContratoRDBO bo = (ContratoRDBO)this.InterfazUsuarioADato();

                List<ContratoRDBO> lst = this.controlador.ConsultarCompleto(this.dctx, bo, true);

                if (lst.Count < 1)
                    throw new Exception("No se encontró ningún registro que corresponda a la información proporcionada.");
                if (lst.Count > 1)
                    throw new Exception("La consulta devolvió más de un registro.");

                var obj = (ContratoRDBO)lst[0];

                this.DatoAInterfazUsuario(obj);
                this.vista.UltimoObjeto = obj;

                #region SC043
                if (!object.ReferenceEquals(obj.Operador, null)) {
                    if (obj.Operador.Estatus.HasValue) {
                        if (!obj.Operador.Estatus.Value)
                            this.vista.MostrarMensaje("El operador asociado al contrato, ya no está activo en el sistema, por favor verifique su información.", ETipoMensajeIU.ADVERTENCIA, null);
                    }
                }
                #endregion

                return obj;
            } catch (Exception ex) {
                this.DatoAInterfazUsuario(new ContratoRDBO());
                throw new Exception(this.nombreClase + ".ConsultarCompleto:" + ex.Message);
            }
        }

        /// <summary>
        /// Establece la configuracion inicial de la Vista
        /// </summary>
        private void EstablecerConfiguracionInicial()
        {
            this.vista.FUA = DateTime.Now;
            this.vista.UUA = this.vista.UsuarioID;

            //Se establecen los tipos de archivos permitidos para adjuntar al contrato
            List<TipoArchivoBO> lstTiposArchivo = new TipoArchivoBR().Consultar(this.dctx, new TipoArchivoBO());
            this.presentadorDocumentos.EstablecerTiposArchivo(lstTiposArchivo);
        }
        /// <summary>
        /// Valida el acceso a las funcionalidades del Sistema
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
                if (!FacadeBR.ExisteAccion(dctx, "ACTUALIZARCOMPLETO", seguridadBO))
                    vista.RedirigirSinPermisoAcceso();

                #region RI0008
                //Se valida si el usuario tiene permiso para registrar contrato en curso
                // RI0016 - Eliminación de permisos de generación de pagos                
                #endregion
            }
            catch (Exception ex)
            {
                throw new Exception(nombreClase + ".ValidarAcceso: " + ex.Message);
            }
        }
        /// <summary>
        /// Establece las opciones permitidas en base a la Seguridad del usuario
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
                if (!this.ExisteAccion(acciones, "UI INSERTAR"))
                    this.vista.PermitirRegistrar(false);

                #region RI0008
                //Se valida si el usuario tiene permiso para registrar contrato en curso
                if (!this.ExisteAccion(acciones, "GENERARPAGOS"))
                    vista.PermitirGuardarTerminado(false);
                #endregion
            }
            catch (Exception ex)
            {

                throw new Exception(this.nombreClase + ".EstablecerSeguridad:" + ex.Message);
            }
        }
        /// <summary>
        /// Verifica la accion en un listado de acciones proporcionado
        /// </summary>
        /// <param name="acciones"></param>
        /// <param name="nombreAccion"></param>
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
        /// Prepara la Vista para la Edicion del Contrato
        /// </summary>
        private void PrepararEdicion()
        {
            this.vista.PrepararEdicion();
            this.presentadorDatosContrato.PrepararEdicion();
            this.presentadorDocumentos.ModoEditable(true);
            this.presentadorHerramientas.Inicializar();
        }

        /// <summary>
        /// Indica si la unidad seleccionada cuenta con una reservación con base en el cliente seleccionado, la fecha del contrato y de promesa de entrega
        /// </summary>
        /// <returns>True en caso de que cuente con una reservación, False en caso contrario</returns>
        public bool UnidadTieneReservacion()
        {
            try
            {
                if (this.vista.UnidadID == null)
                    return false;
                if (this.vista.FechaContrato == null)
                    return false;
                if (this.vista.FechaPromesaDevolucion == null)
                    return false;
                if (this.vista.CuentaClienteID == null)
                    return false;
                if (this.vista.UnidadOperativaID == null)
                    return false;

                //Se consulta si existen reservaciones conflicitvas
                ContratoRDBOF bof = new ContratoRDBOF();
                bof.Unidad = new UnidadBO() { UnidadID = this.vista.UnidadID };
                bof.Cliente = new CuentaClienteIdealeaseBO() { Id = this.vista.CuentaClienteID };
                bof.FechaContrato = this.vista.FechaContrato;
                bof.FechaPromesaDevolucion = this.vista.FechaPromesaDevolucion;
                bof.Sucursal = new SucursalBO() { UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaID } };
                List<ReservacionRDBO> lstConflictivas = this.controlador.ConsultarReservacionesConflictivas(dctx, bof);

                if (lstConflictivas != null && lstConflictivas.Count > 0)
                    return true;

                return false;
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".UnidadTieneReservacion: " + "No fue posible comprobar que la unidad está reservada debido a que " + ex.Message);
            }
        }
        /// <summary>
        /// Cancela la edicion del contrato
        /// </summary>
        public void CancelarEdicion()
        {
            object ultimoContrato = this.vista.UltimoObjeto;
            this.LimpiarSesion();
            this.vista.LimpiarPaqueteNavegacion("UltimoContratoRDBO");
            this.vista.EstablecerPaqueteNavegacion("ContratoRDBO", ultimoContrato);  // new ContratoRDBO() { ContratoID = this.vista.ContratoID });
            this.vista.RedirigirADetalles();
        }
        /// <summary>
        /// Actualiza la Edicion del Contrato en Borrador
        /// </summary>
        public void ActualizarContrato() {
            string s;
            if ((s = this.ValidarCamposBorrador()) != null) {
                this.vista.MostrarMensaje(s, ETipoMensajeIU.ADVERTENCIA, null);
                return;
            }

            this.Editar();

            object contratoEditado = this.vista.UltimoObjeto;
            this.LimpiarSesion();
            this.vista.LimpiarPaqueteNavegacion("UltimoContratoRDBO");
            this.vista.EstablecerPaqueteNavegacion("ContratoRDBO", contratoEditado);
            this.vista.RedirigirADetalles();
        }
        
        /// <summary>
        /// Actualiza la información del contrato
        /// </summary>
        private void Editar()  {
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
                throw new Exception("Se encontraron inconsistencias al editar el Contrato.");
            }
            #endregion

            try {
                //Se obtiene la información a partir de la Interfaz de Usuario y el objeto en session
                ContratoRDBO bo = (ContratoRDBO)this.InterfazUsuarioADato();

                //Se crea el objeto de seguridad
                UsuarioBO usuario = new UsuarioBO() { Id = this.vista.UsuarioID };
                AdscripcionBO adscripcion = new AdscripcionBO() { UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaID } };
                SeguridadBO seguridadBO = new SeguridadBO(Guid.Empty, usuario, adscripcion);

                //Se actualiza en la base de datos
                this.controlador.ActualizarCompleto(this.dctx, bo, (ContratoRDBO)this.vista.UltimoObjeto, seguridadBO);

                dctx.CommitTransaction(firma);
                this.vista.UltimoObjeto = bo;
            }
            catch (Exception ex) {
                dctx.RollbackTransaction(firma);
                throw new Exception(this.nombreClase + ".Editar:" + ex.Message);
            }
            finally {
                if(dctx.ConnectionState == ConnectionState.Open)
                    dctx.CloseConnection(firma);
            }
        }
        /// <summary>
        /// Obtiene los datos a partir de la Vista
        /// </summary>
        /// <returns></returns>
        private object InterfazUsuarioADato(){
           
            ContratoRDBO bo = new ContratoRDBO ((ContratoRDBO)vista.UltimoObjeto); 
            bo.Tipo = ETipoContrato.RD;

            if (this.vista.ContratoID != null)
                bo.ContratoID = this.vista.ContratoID;
            if (this.vista.SucursalID != null)
                bo.Sucursal.Id = this.vista.SucursalID;
            if (this.vista.SucursalNombre != null)
                bo.Sucursal.Nombre = this.vista.SucursalNombre;

            if (this.vista.CuentaClienteID != null)
                bo.Cliente.Id = this.vista.CuentaClienteID;
            if (this.vista.CuentaClienteNombre != null)
                bo.Cliente.Nombre = this.vista.CuentaClienteNombre;

            if (this.vista.CodigoMoneda != null)
                bo.Divisa.MonedaDestino.Codigo = this.vista.CodigoMoneda;
            if (this.vista.RepresentantesLegales != null)
                bo.RepresentantesLegales = this.vista.RepresentantesLegales.ConvertAll(s => (PersonaBO)s);
	        bo.SoloRepresentantes = vista.SoloRepresentantes;
	        bo.Avales = vista.Avales;

            #region Dirección del Cliente

            DireccionClienteBO direccion = new DireccionClienteBO
            {
                Ubicacion =
                    new UbicacionBO
                    {
                        Pais = new PaisBO { Codigo = this.vista.ClienteDireccionPais },
                        Municipio = new MunicipioBO { Codigo = this.vista.ClienteDireccionMunicipio },
                        Estado = new EstadoBO { Codigo = this.vista.ClienteDireccionEstado },
                        Ciudad = new CiudadBO { Codigo = this.vista.ClienteDireccionCiudad }
                    },
                CodigoPostal = this.vista.ClienteDireccionCodigoPostal,
                Calle = this.vista.ClienteDireccionCalle,
                Colonia = this.vista.ClienteDireccionColonia,
                Id = this.vista.ClienteDireccionId,
                Primaria = true
            };

            bo.Cliente.RemoverDirecciones();
            bo.Cliente.Agregar(direccion);
            
            #endregion

            #region Operador
            #region SC_0013
            if (this.vista.OperadorID != null)
                bo.Operador.OperadorID = this.vista.OperadorID;
            #endregion
            if (this.vista.OperadorAniosExperiencia != null)
                bo.Operador.AñosExperiencia = this.vista.OperadorAniosExperiencia;
            if (this.vista.OperadorFechaNacimiento != null)
                bo.Operador.FechaNacimiento = this.vista.OperadorFechaNacimiento;
            if (!string.IsNullOrEmpty(this.vista.OperadorDireccionCalle))
                bo.Operador.Direccion.Calle = this.vista.OperadorDireccionCalle;
            if (!string.IsNullOrEmpty(this.vista.OperadorDireccionCiudad))
                bo.Operador.Direccion.Ubicacion.Ciudad.Nombre = this.vista.OperadorDireccionCiudad;
            if (!string.IsNullOrEmpty(this.vista.OperadorDireccionEstado))
                bo.Operador.Direccion.Ubicacion.Estado.Nombre = this.vista.OperadorDireccionEstado;
            if (!string.IsNullOrEmpty(this.vista.OperadorDireccionCP))
                bo.Operador.Direccion.CodigoPostal = this.vista.OperadorDireccionCP;
            if (!string.IsNullOrEmpty(this.vista.OperadorLicenciaEstado))
                bo.Operador.Licencia.Estado = this.vista.OperadorLicenciaEstado;
            if (this.vista.OperadorLicenciaFechaExpiracion != null)
                bo.Operador.Licencia.FechaExpiracion = this.vista.OperadorLicenciaFechaExpiracion;
            if (!string.IsNullOrEmpty(this.vista.OperadorLicenciaNumero))
                bo.Operador.Licencia.Numero = this.vista.OperadorLicenciaNumero;
            if (this.vista.OperadorLicenciaTipoID != null)
                bo.Operador.Licencia.Tipo = (ETipoLicencia)Enum.Parse(typeof(ETipoLicencia), this.vista.OperadorLicenciaTipoID.ToString());
            if (!string.IsNullOrEmpty(this.vista.OperadorNombre))
                bo.Operador.Nombre = this.vista.OperadorNombre;
            #endregion

            if (this.vista.FechaContrato != null)
                bo.FechaContrato = this.vista.FechaContrato;
            if (this.vista.FechaPromesaDevolucion != null)
                bo.FechaPromesaDevolucion = this.vista.FechaPromesaDevolucion;
            if (this.vista.FormaPagoID != null)
                bo.FormaPago = (EFormaPago)Enum.Parse(typeof(EFormaPago), this.vista.FormaPagoID.ToString());
            if (this.vista.FrecuenciaFacturacionID != null)
                bo.FrecuenciaFacturacion = (EFrecuencia)Enum.Parse(typeof(EFrecuencia), this.vista.FrecuenciaFacturacionID.ToString());
            if (this.vista.AutorizadorTipoConfirmacion != null)
                bo.AutorizadorTipoConfirmacion = this.vista.AutorizadorTipoConfirmacion;
            if (this.vista.AutorizadorOrdenCompra != null)
                bo.AutorizadorOrdenCompra = this.vista.AutorizadorOrdenCompra;
            if (this.vista.BitacoraViajeConductor != null)
                bo.BitacoraViajeConductor = this.vista.BitacoraViajeConductor;
            if (this.vista.DestinoAreaOperacion != null)
                bo.DestinoAreaOperacion = this.vista.DestinoAreaOperacion;
            if (this.vista.LectorKilometrajeID != null)
                bo.LectorKilometraje = (ELectorKilometraje)Enum.Parse(typeof(ELectorKilometraje), this.vista.LectorKilometrajeID.ToString());
            if (this.vista.MercanciaTransportar != null)
                bo.MercanciaTransportar = this.vista.MercanciaTransportar;
            if (this.vista.MotivoRentaID != null)
                bo.MotivoRenta = (EMotivoRenta)Enum.Parse(typeof(EMotivoRenta), this.vista.MotivoRentaID.ToString());
            if (this.vista.PorcentajeDeducible != null)
                bo.PorcentajeDeducible = this.vista.PorcentajeDeducible;
            if (this.vista.TipoConfirmacionID != null)
                bo.TipoConfirmacion = (ETipoConfirmacion)Enum.Parse(typeof(ETipoConfirmacion), this.vista.TipoConfirmacionID.ToString());

            #region BEP1401.SC0026
            if(this.vista.DiasFacturar != null)
                bo.DiasFacturar = this.vista.DiasFacturar;
            #endregion
            if (this.vista.Observaciones != null)
                bo.Observaciones = this.vista.Observaciones;
            if (this.vista.EstatusID != null)
                bo.Estatus = (EEstatusContrato)Enum.Parse(typeof(EEstatusContrato), this.vista.EstatusID.ToString());
            if (this.vista.FC != null)
                bo.FC = this.vista.FC;
            if (this.vista.UC != null)
                bo.UC = this.vista.UC;
            if (this.vista.FUA != null)
                bo.FUA = this.vista.FUA;
            if (this.vista.UUA != null)
                bo.UUA = this.vista.UUA;

            #region Linea de Contrato
            LineaContratoRDBO lineaBO = new LineaContratoRDBO();
            lineaBO.Activo = true;

            if (this.vista.LineaContratoID != null)
                lineaBO.LineaContratoID = this.vista.LineaContratoID;
            if (this.vista.EquipoID != null)
            {
                if (lineaBO.Equipo == null) lineaBO.Equipo = new UnidadBO();
                lineaBO.Equipo.EquipoID = this.vista.EquipoID;
            }
            if (this.vista.UnidadID != null)
            {
                if (lineaBO.Equipo == null) lineaBO.Equipo = new UnidadBO();
                ((UnidadBO)lineaBO.Equipo).UnidadID = this.vista.UnidadID;
            }
            if (!string.IsNullOrWhiteSpace(this.vista.ClaveProductoServicio)){
                if (lineaBO.ProductoServicio == null) lineaBO.ProductoServicio = new ProductoServicioBO();
                lineaBO.ProductoServicio.Id = this.vista.ProductoServicioId;
                lineaBO.ProductoServicio.NombreCorto = this.vista.ClaveProductoServicio;
                lineaBO.ProductoServicio.Nombre = this.vista.DescripcionProductoServicio;
            }

            if (this.vista.TipoTarifaID != null)
                lineaBO.TipoTarifa = (ETipoTarifa)Enum.Parse(typeof(ETipoTarifa), this.vista.TipoTarifaID.ToString());

            if(lineaBO.Cobrable == null) 
                lineaBO.Cobrable = new TarifaContratoRDBO()
                {
                    RangoTarifas = new List<RangoTarifaRDBO>()
                    {
                        new RangoTarifaRDBO()
                    }, 
                    TarifasEquipoAliado = new List<TarifaContratoRDEquipoAliadoBO>()
                };

            if (this.vista.CapacidadCarga != null)
            {
                ((TarifaContratoRDBO)lineaBO.Cobrable).CapacidadCarga = this.vista.CapacidadCarga;
            }
            if (this.vista.TarifaDiaria != null)
            {
                ((TarifaContratoRDBO)lineaBO.Cobrable).TarifaDiaria = this.vista.TarifaDiaria;
            }
            if (this.vista.KmsLibres != null)
            {
                ((TarifaContratoRDBO)lineaBO.Cobrable).KmsLibres = this.vista.KmsLibres;
                ((TarifaContratoRDBO)lineaBO.Cobrable).RangoTarifas.First().KmRangoInicial = this.vista.KmsLibres + 1;
            }
            if (this.vista.TarifaKmAdicional != null)
            {
                ((TarifaContratoRDBO)lineaBO.Cobrable).RangoTarifas.First().CargoKm = this.vista.TarifaKmAdicional;
            }
            if (this.vista.HrsLibres != null)
            {
                ((TarifaContratoRDBO)lineaBO.Cobrable).HrsLibres = this.vista.HrsLibres;
                ((TarifaContratoRDBO)lineaBO.Cobrable).RangoTarifas.First().HrRangoInicial = this.vista.HrsLibres + 1;
            }
            if (this.vista.TarifaHrAdicional != null)
            {
                ((TarifaContratoRDBO)lineaBO.Cobrable).RangoTarifas.First().CargoHr = this.vista.TarifaHrAdicional;
            }
            if (this.vista.TarifaDescripcion != null)
                ((TarifaContratoRDBO)lineaBO.Cobrable).Descripcion = this.vista.TarifaDescripcion;

            #region SC0001 Referencia para el Registro de los pagos
            if(!String.IsNullOrEmpty(this.vista.NumeroEconocimico))
            {
                if(lineaBO.Equipo == null) lineaBO.Equipo = new UnidadBO();
                ((UnidadBO)lineaBO.Equipo).NumeroEconomico = this.vista.NumeroEconocimico;
            }
            #endregion
            bo.LineasContrato.Add(lineaBO);
            #endregion

            #region Archivos Adjuntos
            List<ArchivoBO> adjuntos = presentadorDocumentos.Vista.ObtenerArchivos() ?? new List<ArchivoBO>();
            foreach (ArchivoBO adjunto in adjuntos)
            {
                adjunto.TipoAdjunto = ETipoAdjunto.ContratoRD;
                adjunto.Auditoria = new AuditoriaBO
                {

                    FC = this.vista.FC,
                    UC = this.vista.UC,
                    FUA = this.vista.FUA,
                    UUA = this.vista.UUA
                };
            }
            bo.DocumentosAdjuntos = adjuntos;
            #endregion

            return bo;
        }
        /// <summary>
        /// Despliega los datos en la Vista
        /// </summary>
        /// <param name="obj"></param>
        private void DatoAInterfazUsuario(ContratoRDBO bo) {
            try {
                if (bo == null)
                    throw new Exception("Inconsistencias al cargar los datos.");

                if (!bo.FC.HasValue) {
                    this.vista.UltimoObjeto = bo;
                    bo = this.ConsultarCompleto();
                } else {
                    if (!object.ReferenceEquals(bo.Operador, null)) {
                        if (bo.Operador.Estatus.HasValue) {
                            if (!bo.Operador.Estatus.Value)
                                this.vista.MostrarMensaje("El operador asociado al contrato, ya no está activo en el sistema, por favor verifique su información.", ETipoMensajeIU.ADVERTENCIA, null);
                        }
                    }
                }
                LineaContratoRDBO linea = bo.ObtenerLineaContrato();
                this.vista.ContratoID = bo.ContratoID;
                this.vista.NumeroContrato = bo.NumeroContrato;
                this.vista.CodigoMoneda = bo.Divisa.MonedaDestino.Codigo;
                this.vista.FechaContrato = bo.FechaContrato;
                //SC_0037
                if (this.vista.FechaContrato.HasValue && this.vista.FechaContrato.Value.Date < DateTime.Today)
                    this.vista.FechaContrato = DateTime.Today;
                this.vista.FechaPromesaDevolucion = bo.FechaPromesaDevolucion;
                if (bo.FechaContrato != null && bo.FechaPromesaDevolucion != null)
                    this.presentadorDatosContrato.SeleccionarFechasContrato();
                
                this.vista.SucursalID = bo.Sucursal.Id;
                this.vista.AutorizadorOrdenCompra = bo.AutorizadorOrdenCompra;
                this.vista.AutorizadorTipoConfirmacion = bo.AutorizadorTipoConfirmacion;
                this.vista.BitacoraViajeConductor = bo.BitacoraViajeConductor;
                this.vista.DestinoAreaOperacion = bo.DestinoAreaOperacion;
                this.vista.MercanciaTransportar = bo.MercanciaTransportar;
                this.vista.PorcentajeDeducible = bo.PorcentajeDeducible;
                if (bo.FormaPago != null)
                    this.vista.FormaPagoID = (int)bo.FormaPago;
                else
                    this.vista.FormaPagoID = null;
                if (bo.FrecuenciaFacturacion != null)
                    this.vista.FrecuenciaFacturacionID = (int)bo.FrecuenciaFacturacion;
                else
                    this.vista.FrecuenciaFacturacionID = null;
                if (bo.LectorKilometraje != null)
                    this.vista.LectorKilometrajeID = (int)bo.LectorKilometraje;
                else
                    this.vista.LectorKilometrajeID = null;
                if (bo.MotivoRenta != null)
                    this.vista.MotivoRentaID = (int)bo.MotivoRenta;
                else
                    this.vista.MotivoRentaID = null;
                if (bo.TipoConfirmacion != null)
                    this.vista.TipoConfirmacionID = (int)bo.TipoConfirmacion;
                else
                    this.vista.TipoConfirmacionID = null;

                //Cuenta de Cliente Idealease
                this.vista.CuentaClienteID = bo.Cliente.Id;
                this.vista.CuentaClienteNombre = bo.Cliente.Nombre;
                if (bo.Cliente.TipoCuenta != null)
                    this.vista.CuentaClienteTipoID = (int)bo.Cliente.TipoCuenta;
                else
                    this.vista.CuentaClienteTipoID = null;
                this.presentadorDatosContrato.SeleccionarCuentaCliente(bo.Cliente);

                //Dirección del cliente
                if (bo.Cliente.Direcciones != null && bo.Cliente.Direcciones.Count > 0) {
                    this.vista.ClienteDireccionId = bo.Cliente.Direcciones[0].Id;
                    this.vista.ClienteDireccionCompleta = bo.Cliente.Direcciones[0].Direccion;
                    this.vista.ClienteDireccionCalle = bo.Cliente.Direcciones[0].Calle;
                    this.vista.ClienteDireccionColonia = bo.Cliente.Direcciones[0].Colonia;
                    this.vista.ClienteDireccionCodigoPostal = bo.Cliente.Direcciones[0].CodigoPostal;
                    this.vista.ClienteDireccionCiudad = bo.Cliente.Direcciones[0].Ubicacion.Ciudad.Codigo;
                    this.vista.ClienteDireccionEstado = bo.Cliente.Direcciones[0].Ubicacion.Estado.Codigo;
                    this.vista.ClienteDireccionMunicipio = bo.Cliente.Direcciones[0].Ubicacion.Municipio.Codigo;
                    this.vista.ClienteDireccionPais = bo.Cliente.Direcciones[0].Ubicacion.Pais.Codigo;
                } else {
                    this.vista.ClienteDireccionId = null;
                    this.vista.ClienteDireccionCompleta = null;
                    this.vista.ClienteDireccionCalle = null;
                    this.vista.ClienteDireccionColonia = null;
                    this.vista.ClienteDireccionCodigoPostal = null;
                    this.vista.ClienteDireccionCiudad = null;
                    this.vista.ClienteDireccionEstado = null;
                    this.vista.ClienteDireccionMunicipio = null;
                    this.vista.ClienteDireccionPais = null;
                }

                if (bo.RepresentantesLegales != null) {
                    this.vista.RepresentantesLegales = bo.RepresentantesLegales.ConvertAll(s => (RepresentanteLegalBO)s);
                    this.presentadorDatosContrato.ActualizarRepresentantesLegales();
                } else
                    this.vista.RepresentantesLegales = null;
                vista.SoloRepresentantes = bo.SoloRepresentantes;
                if (bo.Avales != null)
                    vista.Avales = new List<AvalBO>(bo.Avales);
                else vista.Avales = null;
                presentadorDatosContrato.ActualizarAvales();
                this.vista.DiasFacturar = bo.DiasFacturar; //BEP1401.SC0026
                //Operador
                #region SC_0013
                this.vista.OperadorID = bo.Operador.OperadorID;
                #endregion
                this.vista.OperadorAniosExperiencia = bo.Operador.AñosExperiencia;
                this.vista.OperadorDireccionCalle = bo.Operador.Direccion.Calle;
                this.vista.OperadorDireccionCiudad = bo.Operador.Direccion.Ubicacion.Ciudad.Nombre;
                this.vista.OperadorDireccionCP = bo.Operador.Direccion.CodigoPostal;
                this.vista.OperadorDireccionEstado = bo.Operador.Direccion.Ubicacion.Estado.Nombre;
                this.vista.OperadorFechaNacimiento = bo.Operador.FechaNacimiento;
                this.vista.OperadorLicenciaEstado = bo.Operador.Licencia.Estado;
                this.vista.OperadorLicenciaFechaExpiracion = bo.Operador.Licencia.FechaExpiracion;
                this.vista.OperadorLicenciaNumero = bo.Operador.Licencia.Numero;
                if (bo.Operador.Licencia.Tipo != null)
                    this.vista.OperadorLicenciaTipoID = (int)bo.Operador.Licencia.Tipo;
                else
                    this.vista.OperadorLicenciaTipoID = null;
                this.vista.OperadorNombre = bo.Operador.Nombre;

                this.vista.LineaContratoID = linea.LineaContratoID;

                //Unidad
                this.vista.EquipoID = linea.Equipo.EquipoID;
                this.vista.UnidadID = ((UnidadBO)linea.Equipo).UnidadID;
                this.presentadorDatosContrato.SeleccionarUnidad((UnidadBO)linea.Equipo);

                //Producto-Servicio
                if (linea.ProductoServicio != null) {
                    this.vista.ProductoServicioId = linea.ProductoServicio.Id;
                    this.vista.ClaveProductoServicio = linea.ProductoServicio.NombreCorto;
                    this.vista.DescripcionProductoServicio = linea.ProductoServicio.Nombre;
                }

                //Tarifa
                TarifaRDBO tarifa = ((TarifaContratoRDBO)linea.Cobrable);
                tarifa.Tipo = linea.TipoTarifa;
                this.presentadorDatosContrato.SeleccionarTarifa(tarifa);

                this.vista.Observaciones = bo.Observaciones;
                this.vista.UC = bo.UC;
                this.vista.UUA = bo.UUA;
                this.vista.FC = bo.FC;
                this.vista.FUA = bo.FUA;
                if (bo.Estatus != null)
                    this.vista.EstatusID = (int)bo.Estatus;
                else
                    this.vista.EstatusID = null;

                this.presentadorDocumentos.CargarListaArchivos(bo.DocumentosAdjuntos, this.presentadorDocumentos.Vista.TiposArchivo);
                this.presentadorHerramientas.DatosAInterfazUsuario(bo);
            } catch (Exception ex) {
                throw new Exception(this.nombreClase + ".DatoAInterfazUsuario: " + ex.Message);
            }
        }
        private void DatoAInterfazUsuario_(object obj)
        {
            ContratoRDBO bo = (ContratoRDBO)obj;
            if (bo == null) bo = new ContratoRDBO();
            if (bo.Cliente == null) bo.Cliente = new CuentaClienteIdealeaseBO();
            if (bo.Divisa == null) bo.Divisa = new DivisaBO();
            if (bo.Divisa.MonedaDestino == null) bo.Divisa.MonedaDestino = new MonedaBO();
            if (bo.Operador == null) bo.Operador = new OperadorBO();
            if (bo.Operador.Direccion == null) bo.Operador.Direccion = new DireccionPersonaBO();
            if (bo.Operador.Direccion.Ubicacion == null) bo.Operador.Direccion.Ubicacion = new UbicacionBO();
            if (bo.Operador.Direccion.Ubicacion.Ciudad == null) bo.Operador.Direccion.Ubicacion.Ciudad = new CiudadBO();
            if (bo.Operador.Direccion.Ubicacion.Estado == null) bo.Operador.Direccion.Ubicacion.Estado = new EstadoBO();
            if (bo.Operador.Direccion.Ubicacion.Pais == null) bo.Operador.Direccion.Ubicacion.Pais = new PaisBO();
            if (bo.Operador.Licencia == null) bo.Operador.Licencia = new LicenciaBO();
            if (bo.Sucursal == null) bo.Sucursal = new SucursalBO();
            if (bo.Sucursal.UnidadOperativa == null) bo.Sucursal.UnidadOperativa = new UnidadOperativaBO();

            LineaContratoRDBO linea = bo.ObtenerLineaContrato();
            if (linea == null) linea = new LineaContratoRDBO();
            if (linea.Equipo == null) linea.Equipo = new UnidadBO();
            if (linea.Cobrable == null) linea.Cobrable = new TarifaContratoRDBO() {RangoTarifas = new List<RangoTarifaRDBO>(), TarifasEquipoAliado = new List<TarifaContratoRDEquipoAliadoBO>()};
            if (linea.ProductoServicio == null) linea.ProductoServicio = new ProductoServicioBO();

            this.vista.ContratoID = bo.ContratoID;
            this.vista.NumeroContrato = bo.NumeroContrato;
            this.vista.CodigoMoneda = bo.Divisa.MonedaDestino.Codigo;
            this.vista.FechaContrato = bo.FechaContrato;
            //SC_0037
            if (this.vista.FechaContrato.HasValue && this.vista.FechaContrato.Value.Date < DateTime.Today)
                this.vista.FechaContrato = DateTime.Today;
            this.vista.FechaPromesaDevolucion = bo.FechaPromesaDevolucion;
            if (bo.FechaContrato != null && bo.FechaPromesaDevolucion != null)
                this.presentadorDatosContrato.SeleccionarFechasContrato();

            this.vista.SucursalID = bo.Sucursal.Id;
            this.vista.SucursalNombre = bo.Sucursal.Nombre;

            this.vista.AutorizadorOrdenCompra = bo.AutorizadorOrdenCompra;
            this.vista.AutorizadorTipoConfirmacion = bo.AutorizadorTipoConfirmacion;
            this.vista.BitacoraViajeConductor = bo.BitacoraViajeConductor;
            this.vista.DestinoAreaOperacion = bo.DestinoAreaOperacion;
            this.vista.MercanciaTransportar = bo.MercanciaTransportar;
            this.vista.PorcentajeDeducible = bo.PorcentajeDeducible;
            if (bo.FormaPago != null)
                this.vista.FormaPagoID = (int)bo.FormaPago;
            else
                this.vista.FormaPagoID = null;
            if (bo.FrecuenciaFacturacion != null)
                this.vista.FrecuenciaFacturacionID = (int)bo.FrecuenciaFacturacion;
            else
                this.vista.FrecuenciaFacturacionID = null;
            if (bo.LectorKilometraje != null)
                this.vista.LectorKilometrajeID = (int)bo.LectorKilometraje;
            else
                this.vista.LectorKilometrajeID = null;
            if (bo.MotivoRenta != null)
                this.vista.MotivoRentaID = (int)bo.MotivoRenta;
            else
                this.vista.MotivoRentaID = null;
            if (bo.TipoConfirmacion != null)
                this.vista.TipoConfirmacionID = (int)bo.TipoConfirmacion;
            else
                this.vista.TipoConfirmacionID = null;

            //Cuenta de Cliente Idealease
            this.vista.CuentaClienteID = bo.Cliente.Id;
            this.vista.CuentaClienteNombre = bo.Cliente.Nombre;
            if (bo.Cliente.TipoCuenta != null)
                this.vista.CuentaClienteTipoID = (int)bo.Cliente.TipoCuenta;
            else
                this.vista.CuentaClienteTipoID = null;
            this.presentadorDatosContrato.SeleccionarCuentaCliente(bo.Cliente);

            //Dirección del cliente
            if (bo.Cliente.Direcciones != null && bo.Cliente.Direcciones.Count > 0) {
                this.vista.ClienteDireccionId = bo.Cliente.Direcciones[0].Id;
                this.vista.ClienteDireccionCompleta = bo.Cliente.Direcciones[0].Direccion;
                this.vista.ClienteDireccionCalle = bo.Cliente.Direcciones[0].Calle;
                this.vista.ClienteDireccionColonia = bo.Cliente.Direcciones[0].Colonia;
                this.vista.ClienteDireccionCodigoPostal = bo.Cliente.Direcciones[0].CodigoPostal;
                this.vista.ClienteDireccionCiudad = bo.Cliente.Direcciones[0].Ubicacion.Ciudad.Codigo;
                this.vista.ClienteDireccionEstado = bo.Cliente.Direcciones[0].Ubicacion.Estado.Codigo;
                this.vista.ClienteDireccionMunicipio = bo.Cliente.Direcciones[0].Ubicacion.Municipio.Codigo;
                this.vista.ClienteDireccionPais = bo.Cliente.Direcciones[0].Ubicacion.Pais.Codigo;
            }
            else {
                this.vista.ClienteDireccionId = null;
                this.vista.ClienteDireccionCompleta = null;
                this.vista.ClienteDireccionCalle = null;
                this.vista.ClienteDireccionColonia = null;
                this.vista.ClienteDireccionCodigoPostal = null;
                this.vista.ClienteDireccionCiudad = null;
                this.vista.ClienteDireccionEstado = null;
                this.vista.ClienteDireccionMunicipio = null;
                this.vista.ClienteDireccionPais = null;
            }

            if (bo.RepresentantesLegales != null)
            {
                this.vista.RepresentantesLegales = bo.RepresentantesLegales.ConvertAll(s => (RepresentanteLegalBO)s);
                this.presentadorDatosContrato.ActualizarRepresentantesLegales();
            }
            else
                this.vista.RepresentantesLegales = null;
	        vista.SoloRepresentantes = bo.SoloRepresentantes;
			if (bo.Avales != null)
				vista.Avales = new List<AvalBO>(bo.Avales);
			else vista.Avales = null;
			presentadorDatosContrato.ActualizarAvales();
            this.vista.DiasFacturar = bo.DiasFacturar; //BEP1401.SC0026
            //Operador
            #region SC_0013
            this.vista.OperadorID = bo.Operador.OperadorID;
            #endregion
            this.vista.OperadorAniosExperiencia = bo.Operador.AñosExperiencia;
            this.vista.OperadorDireccionCalle = bo.Operador.Direccion.Calle;
            this.vista.OperadorDireccionCiudad = bo.Operador.Direccion.Ubicacion.Ciudad.Nombre;
            this.vista.OperadorDireccionCP = bo.Operador.Direccion.CodigoPostal;
            this.vista.OperadorDireccionEstado = bo.Operador.Direccion.Ubicacion.Estado.Nombre;
            this.vista.OperadorFechaNacimiento = bo.Operador.FechaNacimiento;
            this.vista.OperadorLicenciaEstado = bo.Operador.Licencia.Estado;
            this.vista.OperadorLicenciaFechaExpiracion = bo.Operador.Licencia.FechaExpiracion;
            this.vista.OperadorLicenciaNumero = bo.Operador.Licencia.Numero;
            if (bo.Operador.Licencia.Tipo != null)
                this.vista.OperadorLicenciaTipoID = (int)bo.Operador.Licencia.Tipo;
            else
                this.vista.OperadorLicenciaTipoID = null;
            this.vista.OperadorNombre = bo.Operador.Nombre;

            this.vista.LineaContratoID = linea.LineaContratoID;

            //Unidad
            this.vista.EquipoID = linea.Equipo.EquipoID;
            this.vista.UnidadID = ((UnidadBO)linea.Equipo).UnidadID;
            this.presentadorDatosContrato.SeleccionarUnidad((UnidadBO)linea.Equipo);

            //Producto-Servicio
            if (linea.ProductoServicio != null) {
                this.vista.ProductoServicioId = linea.ProductoServicio.Id;
                this.vista.ClaveProductoServicio = linea.ProductoServicio.NombreCorto;
                this.vista.DescripcionProductoServicio = linea.ProductoServicio.Nombre; 
            }

            //Tarifa
            TarifaRDBO tarifa = new TarifaRDBO()
            {
                Tipo = linea.TipoTarifa,
                CapacidadCarga = ((TarifaContratoRDBO) linea.Cobrable).CapacidadCarga,
                HrsLibres = ((TarifaContratoRDBO) linea.Cobrable).HrsLibres,
                KmsLibres = ((TarifaContratoRDBO) linea.Cobrable).KmsLibres,
                TarifaDiaria = ((TarifaContratoRDBO) linea.Cobrable).TarifaDiaria,
                RangoTarifas = new List<RangoTarifaRDBO>()
                {
                    new RangoTarifaRDBO()
                    {
                        CargoKm = ((TarifaContratoRDBO) linea.Cobrable).RangoTarifas != null && ((TarifaContratoRDBO) linea.Cobrable).RangoTarifas.Any() ? ((TarifaContratoRDBO) linea.Cobrable).RangoTarifas.First().CargoKm : null,
                        CargoHr = ((TarifaContratoRDBO) linea.Cobrable).RangoTarifas != null && ((TarifaContratoRDBO) linea.Cobrable).RangoTarifas.Any() ? ((TarifaContratoRDBO) linea.Cobrable).RangoTarifas.First().CargoHr : null
                    }
                }
            };
            this.presentadorDatosContrato.SeleccionarTarifa(tarifa);

            this.vista.Observaciones = bo.Observaciones;
            this.vista.UC = bo.UC;
            this.vista.UUA = bo.UUA;
            this.vista.FC = bo.FC;
            this.vista.FUA = bo.FUA;
            if (bo.Estatus != null)
                this.vista.EstatusID = (int)bo.Estatus;
            else
                this.vista.EstatusID = null;

            this.presentadorDocumentos.CargarListaArchivos(bo.DocumentosAdjuntos, this.presentadorDocumentos.Vista.TiposArchivo);
            this.presentadorHerramientas.DatosAInterfazUsuario(bo);
        }

        #region SC0043
        /// <summary>
        /// Valida el estatus del operador para verificar que realmente no se este guardando un contrato con un operador inactivo
        /// </summary>
        /// <param name="operadorID">Identificador del operador</param>
        /// <returns>Cadena que especifica el error</returns>
        private string ValidarOperador(int? operadorID)
        {
            if (!operadorID.HasValue)
                return null;

            var operadorBR = new OperadorBR();
            var operadores = operadorBR.Consultar(this.dctx, new OperadorBO { OperadorID = operadorID });
            if (!object.ReferenceEquals(operadores, null))
            {
                if (operadores.Count > 0)
                {
                    var operador = operadores.FirstOrDefault(x => x.Estatus != null && !x.Estatus.Value);
                    if (!ReferenceEquals(operador, null))
                        return "El operador asociado al contrato, ya no está activo en el sistema, por favor verifique su información.";
                }
            }
            return null;
        }
        #endregion

        /// <summary>
        /// Valida los campos requeridos para actualizar el contrato en borrador
        /// </summary>
        /// <returns></returns>
        public string ValidarCamposBorrador()
        {
            string s = string.Empty;

            if ((s = this.presentadorDatosContrato.ValidarCamposBorrador()) != null)
                return s;
            
            //SC0043
            if ((s = this.ValidarOperador(this.vista.OperadorID)) != null)
                return s;
            //SC0043

            if (this.vista.FC == null)
                s += "Fecha de Creación, ";
            if (this.vista.FUA == null)
                s += "Fecha de Última Modificación, ";
            if (this.vista.UC == null)
                s += "Usuario de Creación, ";
            if (this.vista.UUA == null)
                s += "Usuario de Última Modificación, ";

            if (s != null && s.Trim().CompareTo("") != 0)
                return "Los siguientes campos no pueden estar vacíos: \n" + s.Substring(0, s.Length - 2);

            return null;
        }
        /// <summary>
        /// Valida los campos requeridos para actualizar el contrato en curso
        /// </summary>
        /// <returns></returns>
        public string ValidarCamposRegistro()
        {
            string s = string.Empty;

            if ((s = this.presentadorDatosContrato.ValidarCamposRegistro()) != null)
                return s;

            //SC0043
            if ((s = this.ValidarOperador(this.vista.OperadorID)) != null)
                return s;
            //SC0043
            if (this.vista.FC == null)
                s += "Fecha de Creación, ";
            if (this.vista.FUA == null)
                s += "Fecha de Última Modificación, ";
            if (this.vista.UC == null)
                s += "Usuario de Creación, ";
            if (this.vista.UUA == null)
                s += "Usuario de Última Modificación, ";

            if (s != null && s.Trim().CompareTo("") != 0)
                return "Los siguientes campos no pueden estar vacíos: \n" + s.Substring(0, s.Length - 2);

            return null;
        }
        /// <summary>
        /// Limpia los datos en la memoria de la vista
        /// </summary>
        private void LimpiarSesion()
        {
            this.vista.LimpiarSesion();
            this.presentadorDatosContrato.LimpiarSesion();
            this.presentadorDocumentos.LimpiarSesion();
        }

        #region SC0038
        /// <summary>
        /// Despliega en la vista las plantillas correspondientes al módulo
        /// </summary>
        public void CargarPlantillas()
        {
            var controlador = new PlantillaBR();

            var precargados = this.vista.ObtenerPlantillas("ucContratosRentaDiaria");
            var resultado = new List<object>();

            if (precargados != null)
                if (precargados.Count > 0)
                    resultado = precargados;

            if (resultado.Count <= 0)
            {
                var lista = controlador.Consultar(this.dctx, new PlantillaBO { Activo = true, TipoPlantilla = EModulo.RD });

                if (ReferenceEquals(lista, null))
                    lista = new List<PlantillaBO>();

                resultado = lista.ConvertAll(p => (object)p);
            }

            this.presentadorHerramientas.CargarArchivos(resultado);
        }
        #endregion
        #endregion
    }
}
