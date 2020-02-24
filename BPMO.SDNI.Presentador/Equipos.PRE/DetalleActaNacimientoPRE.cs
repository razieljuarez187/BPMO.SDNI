//Satisface al CU079 - Consultar Acta de Nacimiento de Unidad
//Satisface la solicitud de cambio SC0006

using System;
using System.Collections.Generic;
using BPMO.Basicos.BO;
using BPMO.Facade.SDNI.BR;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Equipos.BO;
using BPMO.SDNI.Equipos.BOF;
using BPMO.SDNI.Equipos.BR;
using BPMO.SDNI.Equipos.VIS;
using BPMO.SDNI.Tramites.PRE;
using BPMO.SDNI.Tramites.VIS;
using BPMO.Servicio.Catalogos.BO;
using UnidadBO = BPMO.SDNI.Equipos.BO.UnidadBO;
using System.Collections;
using System.Linq;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Comun.BR;
using BPMO.SDNI.Comun.PRE;

namespace BPMO.SDNI.Equipos.PRE
{
    public class DetalleActaNacimientoPRE
    {
        #region Atributos
        private UnidadBR controlador;
        private IDataContext dctx = null;

        private IDetalleActaNacimientoVIS vista;
        private IucDatosGeneralesVIS vistaPagina1;
        private IucDatosTecnicosVIS vistaPagina2;
        private IucNumerosSerieVIS vistaPagina3;
        private IucAsignacionLlantasVIS vistaPagina4;
        private IucAsignacionEquiposAliadosVIS vistaPagina5;
        private IucTramitesActivosVIS vistaPagina6;
        private IucResumenActaNacimientoVIS vistaPagina7;
        private IucActaNacimientoVIS vistaActaOriginal;

        private ucDatosGeneralesPRE presentadorPagina1;
        private ucDatosTecnicosPRE presentadorPagina2;
        private ucNumerosSeriePRE presentadorPagina3;
        private ucAsignacionLlantasPRE presentadorPagina4;
        private ucAsignacionEquiposAliadosPRE presentadorPagina5;
        private ucTramitesActivosPRE presentadorPagina6;
        private ucResumenActaNacimientoPRE presentadorPagina7;
        private ucActaNacimientoPRE presentadorActaOriginal;
        private string nombreClase = "DetalleActaNacimientoPRE";
        //RQM 14150, cuando la empresa es diferente de Idealease debe validar el mapeo de los objetos
        private bool validarDatoInterfaz = false;
        private TipoArchivoBR tipoArchivoBR = new TipoArchivoBR(); //RQM 13285
        private ArchivoBR archivoBR = new ArchivoBR(); //RQM 13285

        private ucCatalogoDocumentosPRE documentos;
        #endregion

        #region Constructores
        public DetalleActaNacimientoPRE(IDetalleActaNacimientoVIS view, IucDatosGeneralesVIS viewPage1, IucDatosTecnicosVIS viewPage2, IucNumerosSerieVIS viewPage3, IucAsignacionLlantasVIS viewPage4, IucAsignacionEquiposAliadosVIS viewPage5, IucTramitesActivosVIS viewPage6, IucResumenActaNacimientoVIS viewPage7, IucActaNacimientoVIS viewActaOriginal)
        {
            try
            {
                this.vista = view;
                this.vistaPagina1 = viewPage1;
                this.vistaPagina2 = viewPage2;
                this.vistaPagina3 = viewPage3;
                this.vistaPagina4 = viewPage4;
                this.vistaPagina5 = viewPage5;
                this.vistaPagina6 = viewPage6;
                this.vistaPagina7 = viewPage7;
                this.vistaActaOriginal = viewActaOriginal;

                this.presentadorPagina1 = new ucDatosGeneralesPRE(viewPage1, documentos);
                this.presentadorPagina2 = new ucDatosTecnicosPRE(viewPage2);
                this.presentadorPagina3 = new ucNumerosSeriePRE(viewPage3);
                this.presentadorPagina4 = new ucAsignacionLlantasPRE(viewPage4);
                this.presentadorPagina5 = new ucAsignacionEquiposAliadosPRE(viewPage5);
                this.presentadorPagina6 = new ucTramitesActivosPRE(viewPage6);
                this.presentadorPagina7 = new ucResumenActaNacimientoPRE(viewPage7);
                this.presentadorActaOriginal = new ucActaNacimientoPRE(viewActaOriginal);

                this.controlador = new UnidadBR();
                this.dctx = FacadeBR.ObtenerConexion();
            }
            catch (Exception ex)
            {
                this.vista.MostrarMensaje("Inconsistencias en los parámetros de configuración", ETipoMensajeIU.ERROR, this.nombreClase + ".RegistrarActaNacimientoPRE:" + ex.Message);
            }
        }
        #endregion

        #region Métodos
        public void RealizarPrimeraCarga()
        {
            this.PrepararVisualizacion();
            this.EstablecerDatosNavegacion(this.vista.ObtenerDatosNavegacion());
            this.LimpiarSesion();
            
            this.ConsultarCompleto();
            //this.vistaPagina1.EstablecerEntraMantenimeinto();
            //SC0008
            this.EstablecerSeguridad();
            //RQM 14150, se establecen las acciones por empresa
            this.EstablecerAcciones();
            this.presentadorPagina6.PresentarDocumentos();     
        }
        private void EstablecerDatosNavegacion(object paqueteNavegacion)
        {
            Hashtable parameters = new Hashtable();
            try
            {
                if (paqueteNavegacion == null)
                    throw new Exception("Se esperaba un objeto en la navegación. No se puede identificar qué acta de nacimiento se desea consultar.");
                if (!(paqueteNavegacion is BO.UnidadBO))
                    throw new Exception("Se esperaba una Unidad de Idealease.");

                UnidadBO bo = (BO.UnidadBO)paqueteNavegacion;

                #region SC0006 - Adición de datos de siniestro
                parameters["UnidadBO"] = bo;
                parameters["SiniestroUnidadBO"] = this.RecuperarHistorialSiniestro(bo);
                this.DatoAInterfazUsuario(parameters);
                #endregion
            }
            catch (Exception ex)
            {
                #region SC0006 - Adición de datos de siniestro
                parameters["UnidadBO"] = new BO.UnidadBO();
                this.DatoAInterfazUsuario(parameters);
                #endregion
                throw new Exception(this.nombreClase + ".EstablecerDatosNavegacion: " + ex.Message);
            }
        }

        #region SC0006
        private List<SiniestroUnidadBO> RecuperarHistorialSiniestro(UnidadBO bo)
        {
            if (bo.UnidadID.HasValue)
            {
                SiniestroUnidadBR SiniestroUnidadBR = new SiniestroUnidadBR();
                HistorialBR historialBR = new HistorialBR();
                HistorialBOF filter = new HistorialBOF
                {
                    Unidad = new UnidadBO { UnidadID = bo.UnidadID },
                    Movimiento = EMovimiento.BAJA_DE_LA_FLOTA_POR_SINIESTRO
                };
                List<HistorialBOF> historial = historialBR.ConsultarCompleto(this.dctx, filter);

                return SiniestroUnidadBR.ConsultarCompleto(this.dctx, historial.Cast<HistorialBO>().ToList());
            }

            return null;
        }
        #endregion

        #region SC0008
        public void ValidarAcceso()
        {
            try
            {
                //Creación del objeto seguridad
                UsuarioBO usuario = new UsuarioBO { Id = this.vista.UC };
                AdscripcionBO adscripcion = new AdscripcionBO
                {
                    UnidadOperativa = new UnidadOperativaBO { Id = this.vista.UnidadOperativaId }
                };
                SeguridadBO seguridad = new SeguridadBO(Guid.Empty, usuario, adscripcion);

                //Se valida si el usuario tiene permiso a la acción principal
                if (!FacadeBR.ExisteAccion(this.dctx, "CONSULTAR", seguridad))
                    this.vista.RedirigirSinPermisoAcceso();
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".ValidarAcceso:" + ex.Message);
            }

        }
        private void EstablecerSeguridad()
        {
            try
            {
                //valida que los datos del usuario y de la Unidad Operativa no sean nulos
                if (this.vista.UC == null) throw new Exception("El identificador del usuario no debe ser nulo");
                if (this.vista.UnidadOperativaId == null) throw new Exception("El identificador de la Unidad Operativa no puede ser nulo");

                //Creación del objeto seguridad
                UsuarioBO usuario = new UsuarioBO { Id = this.vista.UC };
                AdscripcionBO adscripcion = new AdscripcionBO { UnidadOperativa = new UnidadOperativaBO { Id = this.vista.UnidadOperativaId } };
                SeguridadBO seguridad = new SeguridadBO(Guid.Empty, usuario, adscripcion);

                //Consulta la lista de acciones a las que tiene permiso el usuario
                List<CatalogoBaseBO> acciones = FacadeBR.ConsultarAccion(dctx, seguridad);

                //Por RQM 14150, se agregan las acciones de manera global para poder enviarse a los controles de usuario.
                this.vista.ListaAcciones = acciones;

                //Valida si el usuario tiene permisos de editar un acta de nacimiento.
                if (!ExisteAccion(acciones, "ACTUALIZARDOCUMENTO"))
                    this.vista.PermitirRedirigirAEdicion(false);

                //Valida si el usuario tiene permisos de registrar un nuevo Acta de Nacimiento.
                if (!ExisteAccion(acciones, "REGISTRARDOCUMENTO"))
                    this.vista.PermitirRegistrar(false);

                //Se valida si el usuario tiene permiso para ver el historial de la unidad
                if (!this.ExisteAccion(acciones, "UI CONSULTAR"))
                    this.vista.PermitirConsultarHistorial(false);
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
        #endregion

        #region RQM14150

        /// <summary>
        /// Invoca el método de EstablcerAcciones de la vista IDetalleActaNacimientoVIS, valida permisos y establece configuración inicial de la pagina
        /// </summary>
        private void EstablecerAcciones()
        {

            //Se establecen las acciones por cada control de usuario.
            this.presentadorPagina1.EstablecerAcciones(this.vista.ListaAcciones, "D");
            this.presentadorPagina5.EstablecerAcciones(this.vista.ListaAcciones);
            this.presentadorPagina6.EstablecerAcciones(this.vista.ListaAcciones);
            this.presentadorPagina7.EstablecerAcciones(this.vista.ListaAcciones);
            this.vista.PermitirActualizar(false);
            if (this.vista.UnidadOperativaId != (int)this.vista.EmpresaConPermiso)
            {
                this.vista.EmpresaConPermiso = ETipoEmpresa.Idealease;
                switch (this.vista.UnidadOperativaId)
                {
                    case (int)ETipoEmpresa.Generacion:
                        if (ExisteAccion(this.vista.ListaAcciones, "UI ACTA GENERACION"))
                            this.vista.EmpresaConPermiso = ETipoEmpresa.Generacion;
                        if (ExisteAccion(this.vista.ListaAcciones, "ACTUALIZAR ACTIVO"))
                            this.vista.PermitirActualizar(true);

                        break;
                    case (int)ETipoEmpresa.Equinova:
                        if (ExisteAccion(this.vista.ListaAcciones, "UI ACTA GENERACION"))
                            this.vista.EmpresaConPermiso = ETipoEmpresa.Equinova;
                        if (ExisteAccion(this.vista.ListaAcciones, "ACTUALIZAR ACTIVO"))
                            this.vista.PermitirActualizar(true);

                        break;
                    case (int)ETipoEmpresa.Construccion:
                        if (ExisteAccion(this.vista.ListaAcciones, "UI ACTA CONSTRUCCION"))
                            this.vista.EmpresaConPermiso = ETipoEmpresa.Construccion;
                        if (ExisteAccion(this.vista.ListaAcciones, "ACTUALIZAR ACTIVO"))
                            this.vista.PermitirActualizar(true);
                        break;

                }
                this.vista.ValoresTabs = string.Empty;

                if (!ExisteAccion(this.vista.ListaAcciones, "UI DATOS TECNICOS"))
                {
                    this.vista.ValoresTabs = "1,";
                }
                if (!ExisteAccion(this.vista.ListaAcciones, "UI NUMEROSERIE"))
                {
                    this.vista.ValoresTabs += "2,";
                }
                if (!ExisteAccion(this.vista.ListaAcciones, "UI LLANTAS"))
                {
                    this.vista.ValoresTabs += "3,";
                }
                if (!ExisteAccion(this.vista.ListaAcciones, "UI EQUIPOALIADO"))
                {
                    this.vista.ValoresTabs += "4";
                }

                this.vista.ValoresTabs = this.vista.ValoresTabs.TrimEnd(',');
            }

            this.vista.EstablecerAcciones();
        }

        /// <summary>
        /// Método que actualiza los datos obtenidos de Oracle en el sistema
        /// </summary>
        /// <returns>Regresa una respuesta en true si se actualizo correctamente y false en caso contrario</returns>
        public bool ActualizarDatosActivoFijo()
        {
           bool respuesta = false;
           if (this.vista.lstUnidades.Any())
            {
                //Se agrega el objeto consultado al nuevo BO de tipo Unidad donde se encuentran los datos de Oracle en el Objeto ActivoFijoBO.
                BO.UnidadBO boOracle = new BO.UnidadBO();
                boOracle = this.vista.lstUnidades.FirstOrDefault();

                //Se crea el objeto de seguridad
                UsuarioBO usuario = new UsuarioBO() { Id = this.vista.UUA };
                AdscripcionBO adscripcion = new AdscripcionBO() { UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaId } };
                SeguridadBO seguridadBO = new SeguridadBO(Guid.Empty, usuario, adscripcion);
              
               respuesta = this.controlador.ActualizarActivoFijoRental(this.dctx, boOracle, seguridadBO);
               if (respuesta)
               {
                   this.vista.ClaveActivoOracle = boOracle.ActivoFijo.NumeroActivo;
                   this.vista.LiderID = boOracle.IDLider;
                   this.vista.Anio = boOracle.Anio;
                   this.vista.ModeloNombre = boOracle.Modelo.Nombre;
                   this.vista.ModeloId = boOracle.Modelo.Id;
                   this.vista.MarcaId = boOracle.Modelo.Marca.Id;
                   this.vista.NumeroEconomico = boOracle.NumeroEconomico;
                   this.vista.TipoUnidadNombre = boOracle.TipoEquipoServicio.Nombre;
                   this.vista.FechaCompra = boOracle.ActivoFijo.FechaFacturaCompra;
                   this.vista.MontoFactura = boOracle.ActivoFijo.CostoSinIva;
               }
            }

            return respuesta;
        }

        #endregion

        public void PrepararVisualizacion()
        {
            this.vista.PrepararVisualizacion();
            this.presentadorPagina1.PrepararVisualizacion();
            this.presentadorPagina2.PrepararVisualizacion();
            this.presentadorPagina3.PrepararVisualizacion();
            this.presentadorPagina4.PrepararVisualizacion();
            this.presentadorPagina5.PrepararVisualizacion();
            this.presentadorPagina6.OcultarRedireccionTramites(false);
            this.presentadorPagina6.HabilitarPedimento(false);              
            this.presentadorPagina7.PrepararVisualizacion();

            #region[REQ: 13285, Integración Generación y Construcción]

            if (this.vista.UnidadOperativaId == (int)ETipoEmpresa.Idealease)
            {
                this.presentadorPagina1.HabilitarCargaArchivoOC(false);
                this.presentadorPagina6.HabilitarCargaArchivo(false);
            }
            if (this.vista.UnidadOperativaId == (int)ETipoEmpresa.Construccion || this.vista.UnidadOperativaId == (int)ETipoEmpresa.Generacion
                || this.vista.UnidadOperativaId == (int)ETipoEmpresa.Equinova)
            {
                List<TipoArchivoBO> lstTipos = tipoArchivoBR.Consultar(dctx, new TipoArchivoBO { Estatus = true });
                this.presentadorPagina1.DesplegarTiposArchivos(lstTipos);
                this.presentadorPagina1.HabilitarCargaArchivoOC(true);
                this.presentadorPagina1.ModoEdicion(false);
                this.presentadorPagina6.EstablecerTiposdeArchivo(lstTipos);
                this.presentadorPagina6.HabilitarCargaArchivo(true);
                this.presentadorPagina6.ModoEdicion(false);
            }

            #endregion
            this.IrAPagina(1);

        }

        public void RetrocederPagina()
        {
            //RQM 14150, variables para determinar los tabs que no deben de ir en generación y construcción o que están ocultos

            int? paginaActual = this.vista.PaginaActual;
            if (paginaActual == null)
                throw new Exception("La página actual es nula.");
            if (paginaActual <= 1)
                throw new Exception("La página actual es menor o igual a 1 y, por lo tanto, no se puede retroceder.");

            this.EstablecerOpcionesSegunPagina(paginaActual.Value - 1);

            this.vista.EstablecerPagina(paginaActual.Value - 1);

            //RQM 14150, modificación para determinar si al retroceder no se tiene acceso o esta oculto el tab.
            if (this.vista.ValoresTabs.Contains((paginaActual.Value - 2).ToString()))
            {
                RetrocederPagina();
            }

        }
        public void AvanzarPagina()
        {
            int? paginaActual = this.vista.PaginaActual;
            if (paginaActual == null)
                throw new Exception("La página actual es nula.");
            if (paginaActual >= 7)
                throw new Exception("La página actual es mayor o igual a 7 y, por lo tanto, no se puede avanzar.");

            this.EstablecerOpcionesSegunPagina(paginaActual.Value + 1);

            this.vista.EstablecerPagina(paginaActual.Value + 1);

            //RQM 14150, modificación para determinar si al avanzar no se tiene acceso o esta oculto el tab.
            if (this.vista.ValoresTabs.Contains(paginaActual.Value.ToString()))
            {
                AvanzarPagina();
            }
        }
        public void IrAPagina(int numeroPagina)
        {
            if (numeroPagina < 1 || numeroPagina > 7)
                throw new Exception("La paginación va de 1 al 7.");

            this.EstablecerOpcionesSegunPagina(numeroPagina);
            this.vista.EstablecerPagina(numeroPagina);

        }
        private void EstablecerOpcionesSegunPagina(int numeroPagina)
        {
            this.vista.PermitirRegresar(true);
            this.vista.PermitirContinuar(true);

            if (numeroPagina <= 1)
                this.vista.PermitirRegresar(false);

            if (numeroPagina >= 7)
                this.vista.PermitirContinuar(false);
        }

        /// <summary>
        /// Consultar completo, método encapsulado para consultar el objeto de Unidad.
        /// </summary>
        /// <returns>Regresa una lista de unidades</returns>
        public List<BO.UnidadBO> ConsultarCompletoPRE()
        {
            try{
                BO.UnidadBO bo = (BO.UnidadBO)this.InterfazUsuarioADato();
                List<BO.UnidadBO> lst = this.controlador.ConsultarCompleto(this.dctx, bo, true, true); // Por el RQM14150, se pone en true la bandera que trae los datos de la depreciación en Oracle.
                return lst;
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".ConsultarCompletoPRE:" + ex.Message);
            }
        }

        private void ConsultarCompleto()
        {
            Hashtable parameters = new Hashtable();
            try
            {
                //Inicializamos lista global de unidades y posterior a la consulta la agregamos al objeto.
                //Para mas adelante actualizar los datos de Oracle.
                this.vista.lstUnidades = null;
                List<BO.UnidadBO> lst = ConsultarCompletoPRE();
                this.vista.lstUnidades = lst;
                BO.UnidadBO unidadConsultada = lst.First();               

                if (lst.Count < 1)
                    throw new Exception("No se encontró ningún registro que corresponda a la información proporcionada.");
                if (lst.Count > 1)
                    throw new Exception("La consulta devolvió más de un registro.");

                #region SC0006 - Adición de datos de siniestro
                parameters["UnidadBO"] = lst[0];
                parameters["SiniestroUnidadBO"] = this.RecuperarHistorialSiniestro(lst[0]);
                this.DatoAInterfazUsuario(parameters);
                #endregion

                //Se obtiene los archivos de orden de compra
                ArrendamientoBO arrendamiento = unidadConsultada.ObtenerArrendamientoVigente();

                this.vista.ArchivosOC = arrendamiento.Adjuntos;          
                
            }
            catch (Exception ex)
            {
                #region SC0006 - Adición de datos de siniestro
                parameters["UnidadBO"] = new BO.UnidadBO();
                this.DatoAInterfazUsuario(parameters);
                #endregion
                throw new Exception(this.nombreClase + ".ConsultarCompleto:" + ex.Message);
            }
        }


        private void DatoAInterfazUsuario(IDictionary parameters)
        {
            ArrendamientoBO boArrendamiento = new ArrendamientoBO();

            if (!parameters.Contains("UnidadBO") || !(parameters["UnidadBO"] is UnidadBO))
                return;

            UnidadBO bo = (UnidadBO)parameters["UnidadBO"];
            #region Inicialización de Propiedades
            if (bo.ActivoFijo == null)
                bo.ActivoFijo = new ActivoFijoBO();

            if (bo.CaracteristicasUnidad == null)
                bo.CaracteristicasUnidad = new CaracteristicasUnidadBO();
            if (bo.CaracteristicasUnidad.Ejes == null)
                bo.CaracteristicasUnidad.Ejes = new List<EjeBO>();
            if (bo.CaracteristicasUnidad.Motor == null)
                bo.CaracteristicasUnidad.Motor = new MotorBO();
            if (bo.CaracteristicasUnidad.SistemaElectrico == null)
                bo.CaracteristicasUnidad.SistemaElectrico = new SistemaElectricoBO();
            if (bo.CaracteristicasUnidad.Transmision == null)
                bo.CaracteristicasUnidad.Transmision = new TransmisionBO();

            if (bo.Cliente == null)
                bo.Cliente = new ClienteBO();
            if (bo.EquiposAliados == null)
                bo.EquiposAliados = new List<EquipoAliadoBO>();

            if (bo.NumerosSerie == null)
                bo.NumerosSerie = new List<NumeroSerieBO>();

            if (bo.Llantas == null)
                bo.LimpiarLlantas();


            if (bo.Mediciones == null)
                bo.Mediciones = new MedicionesBO();
            if (bo.Mediciones.Horometros == null)
                bo.Mediciones.Horometros = new List<HorometroBO>();
            if (bo.Mediciones.Odometros == null)
                bo.Mediciones.Odometros = new List<OdometroBO>();


            if (bo.Modelo == null)
                bo.Modelo = new ModeloBO();
            if (bo.Modelo.Marca == null)
                bo.Modelo.Marca = new MarcaBO();
            if (bo.Sucursal == null)
                bo.Sucursal = new SucursalBO();
            if (bo.Sucursal.UnidadOperativa == null)
                bo.Sucursal.UnidadOperativa = new UnidadOperativaBO();


            if (bo.TipoEquipoServicio == null)
                bo.TipoEquipoServicio = new TipoUnidadBO();

            if (bo.Arrendamientos == null)
                bo.Arrendamientos = new List<ArrendamientoBO>();

            if (bo.Pedimentos == null)
                bo.Pedimentos = new List<ArchivoBO>();

            #endregion

            this.vista.EquipoID = bo.EquipoID;
            this.vista.UnidadID = bo.UnidadID;
            this.vista.NumeroSerie = bo.NumeroSerie;

            this.vista.NumerosSerie = bo.NumerosSerie;

            this.vista.ClaveActivoOracle = bo.ClaveActivoOracle;
            this.vista.LiderID = bo.IDLider;
            this.vista.Anio = bo.Anio;
            this.vista.ModeloNombre = bo.Modelo.Nombre;
            this.vista.ModeloId = bo.Modelo.Id;
            this.vista.MarcaId = bo.Modelo.Marca.Id;
            this.vista.NumeroEconomico = bo.NumeroEconomico;
            this.vista.TipoUnidadNombre = bo.TipoEquipoServicio.Nombre;
            this.vista.FechaCompra = bo.ActivoFijo.FechaFacturaCompra;
            this.vista.MontoFactura = bo.ActivoFijo.CostoSinIva;


            if (bo.Area != null)
            {
                ETipoEmpresa tipoEmpresa;
                tipoEmpresa = (ETipoEmpresa)this.vista.UnidadOperativaId;

                if (tipoEmpresa != null)
                {
                    switch (tipoEmpresa)
                    {
                        case ETipoEmpresa.Construccion:
                            EAreaConstruccion tipoRentaConstruccion = (EAreaConstruccion)bo.Area;
                            this.vista.Area = tipoRentaConstruccion;
                            if (tipoRentaConstruccion == EAreaConstruccion.RE)
                            {
                                boArrendamiento = bo.ObtenerArrendamientoVigente();
                                this.vista.ProveedorID = boArrendamiento.Proveedor.Id; //En lugar del propietario, va el proveedor id, el control debe de obtener la información.
                                this.vista.OrdenCompraProveedor = boArrendamiento.NumeroOrdenCompra;
                                this.vista.MontoArrendamiento = boArrendamiento.MontoArrendamiento;
                                this.vista.CodigoMoneda = boArrendamiento.CodigoMoneda;
                                this.vista.FechaInicioArrendamiento = boArrendamiento.FechaInicioArrendamiento;
                                this.vista.FechaFinArrendamiento = boArrendamiento.FechaFinArrendamiento;
                                this.vistaPagina1.OcultarControlesRE(true);
                            }
                            else
                            {
                                if ((tipoRentaConstruccion == EAreaConstruccion.RO || tipoRentaConstruccion == EAreaConstruccion.ROC) && bo.EstatusActual != null &&
                                    bo.EstatusActual == EEstatusUnidad.Terminada){
                                    this.vista.HabilitaBoton(false, "E");
                                    this.vistaPagina1.OcultarControlesRE(false);
                                }
                            }
                            this.vista.Cliente = bo.Cliente.Nombre;
                            this.vista.ClienteId = bo.Cliente.Id;
                            this.vista.SucursalNombre = bo.Sucursal.Nombre;
                            this.vista.SucursalId = bo.Sucursal.Id;
                            vistaPagina1.UnidadBloqueada = bo.EquipoBloqueado;
                            this.vista.EntraMantenimiento = bo.EntraMantenimiento;
                            this.vistaPagina1.EstablecerEntraMantenimeinto();
                            this.vista.Archivos = bo.ObtenerPedimento();

                            break;
                        case ETipoEmpresa.Generacion:
                            EAreaGeneracion tipoRentaGeneracion = (EAreaGeneracion)bo.Area;
                            this.vista.Area = tipoRentaGeneracion;
                            if (tipoRentaGeneracion == EAreaGeneracion.RE)
                            {
                                boArrendamiento = bo.ObtenerArrendamientoVigente();
                                this.vista.ProveedorID = boArrendamiento.Proveedor.Id; //En lugar del propietario, va el proveedor id, el control debe de obtener la información.
                                this.vista.OrdenCompraProveedor = boArrendamiento.NumeroOrdenCompra;
                                this.vista.MontoArrendamiento = boArrendamiento.MontoArrendamiento;
                                this.vista.CodigoMoneda = boArrendamiento.CodigoMoneda;
                                this.vista.FechaInicioArrendamiento = boArrendamiento.FechaInicioArrendamiento;
                                this.vista.FechaFinArrendamiento = boArrendamiento.FechaFinArrendamiento;
                                this.vistaPagina1.OcultarControlesRE(true);
                            }
                            else
                            {
                                if ((tipoRentaGeneracion == EAreaGeneracion.RO || tipoRentaGeneracion == EAreaGeneracion.ROC) && bo.EstatusActual != null &&
                                    bo.EstatusActual == EEstatusUnidad.Terminada){
                                    this.vista.HabilitaBoton(false, "E");
                                    this.vistaPagina1.OcultarControlesRE(false);
                                }
                            }
                            this.vista.Cliente = bo.Cliente.Nombre;
                            this.vista.ClienteId = bo.Cliente.Id;
                            this.vista.SucursalNombre = bo.Sucursal.Nombre;
                            this.vista.SucursalId = bo.Sucursal.Id;
                            vistaPagina1.UnidadBloqueada = bo.EquipoBloqueado;
                            this.vista.EntraMantenimiento = bo.EntraMantenimiento;
                            this.vistaPagina1.EstablecerEntraMantenimeinto();
                            this.vista.Archivos = bo.ObtenerPedimento();

                            break;
                        case ETipoEmpresa.Equinova:
                            EAreaEquinova tipoRentaEquinova = (EAreaEquinova)bo.Area;
                            this.vista.Area = tipoRentaEquinova;
                            if (tipoRentaEquinova == EAreaEquinova.RE) {
                                boArrendamiento = bo.ObtenerArrendamientoVigente();
                                this.vista.ProveedorID = boArrendamiento.Proveedor.Id; //En lugar del propietario, va el proveedor id, el control debe de obtener la información.
                                this.vista.OrdenCompraProveedor = boArrendamiento.NumeroOrdenCompra;
                                this.vista.MontoArrendamiento = boArrendamiento.MontoArrendamiento;
                                this.vista.CodigoMoneda = boArrendamiento.CodigoMoneda;
                                this.vista.FechaInicioArrendamiento = boArrendamiento.FechaInicioArrendamiento;
                                this.vista.FechaFinArrendamiento = boArrendamiento.FechaFinArrendamiento;
                                this.vistaPagina1.OcultarControlesRE(true);
                            } else {
                                if ((tipoRentaEquinova == EAreaEquinova.RO || tipoRentaEquinova == EAreaEquinova.ROC) && bo.EstatusActual != null &&
                                    bo.EstatusActual == EEstatusUnidad.Terminada) {
                                    this.vista.HabilitaBoton(false, "E");
                                    this.vistaPagina1.OcultarControlesRE(false);
                                }
                            }
                            this.vista.Cliente = bo.Cliente.Nombre;
                            this.vista.ClienteId = bo.Cliente.Id;
                            this.vista.SucursalNombre = bo.Sucursal.Nombre;
                            this.vista.SucursalId = bo.Sucursal.Id;
                            vistaPagina1.UnidadBloqueada = bo.EquipoBloqueado;
                            this.vista.EntraMantenimiento = bo.EntraMantenimiento;
                            this.vistaPagina1.EstablecerEntraMantenimeinto();
                            this.vista.Archivos = bo.ObtenerPedimento();

                            break;
                        default:
                            this.vista.Area = (EArea)bo.Area;
                            this.vista.SucursalNombre = bo.Sucursal.Nombre;
                            this.vista.SucursalId = bo.Sucursal.Id;
                            this.vista.Cliente = bo.Cliente.Nombre;
                            this.vista.ClienteId = bo.Cliente.Id;
                            this.vista.FabricanteNombre = bo.Fabricante;
                            vistaPagina1.UnidadBloqueada = bo.EquipoBloqueado;

                            #region SC0002

                            this.vista.EntraMantenimiento = bo.EntraMantenimiento;
                            this.vistaPagina1.EstablecerEntraMantenimeinto();
                            #endregion

                            break;
                    }


                }
            }

            this.vista.Propietario = bo.Propietario;

            //RQM 14150, datos técnicos, modificación de la visualización
            if (!this.vista.ValoresTabs.Contains("1"))
            {
                this.presentadorPagina2.AgregarHorometros(bo.Mediciones.Horometros);
                this.presentadorPagina2.AgregarOdometros(bo.Mediciones.Odometros);
                this.vista.CapacidadTanque = bo.CaracteristicasUnidad.CapacidadTanque;
                this.vista.RendimientoTanque = bo.CaracteristicasUnidad.RendimientoTanque;
                this.vista.PBCMaximoRecomendado = bo.CaracteristicasUnidad.PBCMaximoRecomendado;
                this.vista.PBVMaximoRecomendado = bo.CaracteristicasUnidad.PBVMaximoRecomendado;
            }

            //RQM 14150, números de serie, modificación de la visualización
            if (!this.vista.ValoresTabs.Contains("2"))
            {
                //
                this.presentadorPagina3.AgregarNumerosSerie(bo.NumerosSerie);
                //************************************************************

                this.vista.Radiador = bo.CaracteristicasUnidad.Radiador;
                this.vista.PostEnfriador = bo.CaracteristicasUnidad.PostEnfriador;
                this.vista.SerieCompresorAire = bo.CaracteristicasUnidad.Motor.SerieCompresorAire;
                this.vista.SerieECM = bo.CaracteristicasUnidad.Motor.SerieECM;

                #region SC0030

                this.vista.SerieMotor = bo.CaracteristicasUnidad.Motor.SerieMotor;

                #endregion

                this.vista.SerieTurboCargador = bo.CaracteristicasUnidad.Motor.SerieTurboCargador;
                this.vista.SerieAlternador = bo.CaracteristicasUnidad.SistemaElectrico.SerieAlternador;
                this.vista.SerieBaterias = bo.CaracteristicasUnidad.SistemaElectrico.SerieBaterias;
                this.vista.SerieMarcha = bo.CaracteristicasUnidad.SistemaElectrico.SerieMarcha;
                this.vista.TransmisionModelo = bo.CaracteristicasUnidad.Transmision.Modelo;
                this.vista.TransmisionSerie = bo.CaracteristicasUnidad.Transmision.Serie;

                EjeBO eje = bo.CaracteristicasUnidad.ObtenerEjePorPosicion(EPosicionEje.Delantero);
                if (eje != null)
                {
                    this.vista.EjeDireccionModelo = eje.Modelo;
                    this.vista.EjeDireccionSerie = eje.Serie;
                }

                eje = bo.CaracteristicasUnidad.ObtenerEjePorPosicion(EPosicionEje.TraseroDelantero);
                if (eje != null)
                {
                    this.vista.EjeTraseroDelanteroModelo = eje.Modelo;
                    this.vista.EjeTraseroDelanteroSerie = eje.Serie;
                }

                eje = bo.CaracteristicasUnidad.ObtenerEjePorPosicion(EPosicionEje.TraseroTrasero);
                if (eje != null)
                {
                    this.vista.EjeTraseroTraseroModelo = eje.Modelo;
                    this.vista.EjeTraseroTraseroSerie = eje.Serie;
                }
            }

            //RQM 14150, Llantas, modificación de la visualización
            if (!this.vista.ValoresTabs.Contains("3"))
            {

                this.vista.EnllantableID = bo.EnllantableID;
                this.vista.TipoEnllantable = (int)bo.TipoEnllantable;
                this.vista.DescripcionEnllantable = bo.DescripcionEnllantable;
                this.presentadorPagina4.AgregarLlantas(bo.ObtenerLlantas());
                LlantaBO refaccion = bo.ObtenerRefaccion();
                if (refaccion == null) refaccion = new LlantaBO() { EsRefaccion = true };
                if (refaccion.Auditoria == null) refaccion.Auditoria = new AuditoriaBO();
                if (refaccion.Sucursal == null) refaccion.Sucursal = new SucursalBO();
                this.vista.RefaccionSucursalID = refaccion.Sucursal.Id;
                this.vista.RefaccionSucursalNombre = refaccion.Sucursal.Nombre;
                this.vista.RefaccionID = refaccion.LlantaID;
                this.vista.RefaccionCodigo = refaccion.Codigo;
                this.vista.RefaccionMarca = refaccion.Marca;
                this.vista.RefaccionModelo = refaccion.Modelo;
                this.vista.RefaccionActivo = refaccion.Activo;
                this.vista.RefaccionFC = refaccion.Auditoria.FC;
                this.vista.RefaccionFUA = refaccion.Auditoria.FUA;
                this.vista.RefaccionMedida = refaccion.Medida;
                this.vista.RefaccionProfundidad = refaccion.Profundidad;
                this.vista.RefaccionRevitalizada = refaccion.Revitalizada;
                this.vista.RefaccionStock = refaccion.Stock;
                this.vista.RefaccionUC = refaccion.Auditoria.UC;
                this.vista.RefaccionUUA = refaccion.Auditoria.UUA;
            }

            //RQM 14150, Equipos Aliados, modificación de la visualización
            if (!this.vista.ValoresTabs.Contains("4"))
                this.presentadorPagina5.AgregarEquiposAliado(bo.EquiposAliados);

            this.vista.EstatusUnidad = bo.EstatusActual;
            if (bo.EstatusActual != null && bo.EstatusActual != EEstatusUnidad.NoDisponible)
                this.vista.MostrarActaNacimientoOriginal(true);
            else
                this.vista.MostrarActaNacimientoOriginal(false);

            //RQM 14150, tramites, modificación de la visualización
                this.presentadorPagina6.CargarTramites(bo.TramitableID, bo.TipoTramitable, bo.DescripcionTramitable);
            
            this.presentadorPagina7.DesplegarInformacion(bo);

            #region SC0006 - Adición de datos de siniestro
            List<SiniestroUnidadBO> historial = parameters.Contains("SiniestroUnidadBO") && parameters["SiniestroUnidadBO"] is List<SiniestroUnidadBO> ? (List<SiniestroUnidadBO>)parameters["SiniestroUnidadBO"] : null;
            this.presentadorPagina7.DesplegarInformacionSiniestro(historial);
            #endregion

            this.presentadorActaOriginal.CargarActaNacimiento(bo.ActaNacimiento);
        }

        private object InterfazUsuarioADato()
        {
            BO.UnidadBO bo = new BO.UnidadBO();
            #region Inicialización de Propiedades
            bo.ActivoFijo = new ActivoFijoBO();
            bo.CaracteristicasUnidad = new CaracteristicasUnidadBO();
            bo.CaracteristicasUnidad.Ejes = new List<EjeBO>();
            bo.CaracteristicasUnidad.Motor = new MotorBO();
            bo.CaracteristicasUnidad.SistemaElectrico = new SistemaElectricoBO();
            bo.CaracteristicasUnidad.Transmision = new TransmisionBO();
            bo.Cliente = new Basicos.BO.ClienteBO();
            bo.EquiposAliados = new List<EquipoAliadoBO>();
            bo.LimpiarLlantas();
            bo.Mediciones = new MedicionesBO();
            bo.Mediciones.Horometros = new List<HorometroBO>();
            bo.Mediciones.Odometros = new List<OdometroBO>();
            bo.Modelo = new ModeloBO();
            bo.Sucursal = new SucursalBO();
            bo.Sucursal.UnidadOperativa = new UnidadOperativaBO();


            //
            bo.NumerosSerie = new List<NumeroSerieBO>();
            //************
            #endregion
            bo.EquipoID = this.vista.EquipoID;
            bo.UnidadID = this.vista.UnidadID;

            return bo;
        }

        public void Editar()
        {
            BO.UnidadBO bo = (BO.UnidadBO)this.InterfazUsuarioADato();

            this.LimpiarSesion();
            this.vista.EstablecerDatosNavegacion("UnidadBO", bo);

            this.vista.RedirigirAEdicion();
        }

        public void LimpiarSesion()
        {
            this.vista.LimpiarSesion();
            this.presentadorPagina1.LimpiarSesion();
            this.presentadorPagina2.LimpiarSesion();
            this.presentadorPagina3.LimpiarSesion();
            this.presentadorPagina4.LimpiarSesion();
            this.presentadorPagina5.LimpiarSesion();
        }

        #region Métodos para el Visor
        /// <summary>
        /// Prepara un BO para la Busqueda en su respectivo catalogo
        /// </summary>
        /// <param name="catalogo">catalogo donde se realizara la busqueda</param>
        /// <returns></returns>
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
