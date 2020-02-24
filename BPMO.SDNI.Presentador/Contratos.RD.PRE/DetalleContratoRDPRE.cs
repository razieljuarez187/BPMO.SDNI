// Satisface el Caso de uso CU003 - Consultar Contratos Renta Diaria
// Satisface al Caso de Uso CU014 - Imprimir Contrato de Renta Diaria
// Satisface al CU012 - Imprimir Check List de Entrega Recepción de Unidad
// Satisface al CU013 - Cerrar Contrato Renta Diaria
// Satisface al CU094 - Imprimir Pagaré Contrato RD
using System;
using System.Collections.Generic;
using System.Linq;
using BPMO.Basicos.BO;
using BPMO.Facade.SDNI.BR;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Comun.PRE;
using BPMO.SDNI.Comun.VIS;
using BPMO.SDNI.Contratos.BO;
using BPMO.SDNI.Contratos.BR;
using BPMO.SDNI.Contratos.RD.BO;
using BPMO.SDNI.Contratos.RD.BR;
using BPMO.SDNI.Contratos.RD.VIS;
using BPMO.SDNI.Equipos.BO;
using BPMO.SDNI.Flota.BO;
using BPMO.SDNI.Flota.PRE;
using BPMO.SDNI.Flota.VIS;
using BPMO.SDNI.Tramites.BO;
using BPMO.SDNI.Tramites.BR;

namespace BPMO.SDNI.Contratos.RD.PRE
{
    public class DetalleContratoRDPRE
    {
        #region Atributos

        /// <summary>
        /// Vista sobre la que actua el presentador
        /// </summary>
        private readonly IDetalleContratoRDVIS vista;

        /// <summary>
        /// El DataContext que proveerá acceso a la base de datos
        /// </summary>
        private readonly IDataContext dctx;

        /// <summary>
        /// Nombre de la clase para agregar a los mensajes de Error
        /// </summary>
        private string nombreClase = "DetalleContratoRDPRE";

        /// <summary>
        /// Presentador de la Barra de Herramientas
        /// </summary>
        private readonly ucHerramientasRDPRE presentadorHerramientas;

        /// <summary>
        /// Presentador de la Información Contrato
        /// </summary>
        private readonly ucResumenContratoRDPRE presentadorResumen;

        /// <summary>
        /// Presentador del UC de datoas generales
        /// </summary>
        private readonly ucDatosGeneralesElementoPRE presentadorDG;

        /// <summary>
        /// Presentador del UC de euipos aliados
        /// </summary>
        private readonly ucEquiposAliadosUnidadPRE presentadorEA;

        private ucCatalogoDocumentosPRE presentadorDocumentosContrato;
        private ucCatalogoDocumentosPRE presentadorDocumentosEntrega;
        private ucCatalogoDocumentosPRE presentadorDocumentosRecepcion;

        private ContratoRDBR controlador;

        #endregion

        #region Constructores

        /// <summary>
        /// Constructor que recibe la vista sobre la que actuará el presentador
        /// </summary>
        /// <param name="vistaActual">vista sobre la que actuará el presentador</param>
        /// <param name="herramientas">Presentador de la barra de herramientas</param>
        /// <param name="infoContrato">Presentador de la Informacion General</param>
        /// <param name="vistadg">Vista de los datos generales de la unidad</param>
        /// <param name="vistaea">Vista de los datos de los equipos aliados</param>
        public DetalleContratoRDPRE(IDetalleContratoRDVIS view, IucHerramientasRDVIS vistaHerramientas, IucResumenContratoRDVIS vistaInfoContrato, IucDatosGeneralesElementoVIS vistadg, IucEquiposAliadosUnidadVIS vistaea, IucCatalogoDocumentosVIS viewDocsContrato, IucCatalogoDocumentosVIS viewDocsEntrega, IucCatalogoDocumentosVIS viewDocsRecepcion)
        {
            try
            {
                this.vista = view;

                this.presentadorResumen = new ucResumenContratoRDPRE(vistaInfoContrato);
                this.presentadorDG = new ucDatosGeneralesElementoPRE(vistadg);
                this.presentadorEA = new ucEquiposAliadosUnidadPRE(vistaea);
                this.presentadorHerramientas = new ucHerramientasRDPRE(vistaHerramientas);
                this.presentadorDocumentosContrato = new ucCatalogoDocumentosPRE(viewDocsContrato);
                this.presentadorDocumentosEntrega = new ucCatalogoDocumentosPRE(viewDocsEntrega);
                this.presentadorDocumentosRecepcion = new ucCatalogoDocumentosPRE(viewDocsRecepcion);

                this.controlador = new ContratoRDBR();
                this.dctx = FacadeBR.ObtenerConexion();
            }
            catch (Exception ex)
            {
                this.vista.MostrarMensaje("Inconsistencia en los parámetros de configuración", ETipoMensajeIU.ERROR, this.nombreClase + ".CerrarContratoRDPRE:" + ex.Message);
            }
        }

        #endregion

        #region Métodos
        public void RealizarPrimeraCarga()
        {
            try {
                this.PrepararVisualizacion();

                var sucursalTemp = this.vista.ObtenerPaqueteNavegacion("SucursalesAutorizadas"); 
                ContratoRDBO bo = (ContratoRDBO)this.vista.ObtenerPaqueteNavegacion("ContratoRDBO");
                this.vista.PermitirRegresar(this.vista.ObtenerPaqueteNavegacion("FiltroConsultaContratoRD") != null);
                
                if (!bo.FC.HasValue)
                    bo = this.ConsultarCompleto( bo);


                this.DatoAInterfazUsuario(bo);
                this.EstablecerConfiguracionInicial();
                this.EstablecerSeguridad();
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".RealizarPrimeraCarga: " + ex.Message);
            }
        }
       

        private ContratoRDBO ConsultarCompleto(ContratoRDBO bo) {
            try {
                List<ContratoRDBO> lst = this.controlador.ConsultarCompleto(this.dctx, bo, true);

                if (lst.Count < 1)
                    throw new Exception("No se encontró ningún registro que corresponda a la información proporcionada.");
                if (lst.Count > 1)
                    throw new Exception("La consulta devolvió más de un registro.");

                return lst[0];
            }
            catch (Exception ex) {
                this.DatoAInterfazUsuario(new ContratoRDBO());
                throw new Exception(this.nombreClase + ".ConsultarCompleto:" + ex.Message);
            }
        }
        private void EstablecerConfiguracionInicial()
        {
            try
            {
                this.vista.UUA = this.vista.UsuarioID;
                this.vista.FUA = DateTime.Now;

                this.presentadorDocumentosContrato.EstablecerTiposArchivo(new List<TipoArchivoBO>());
                this.presentadorDocumentosEntrega.EstablecerTiposArchivo(new List<TipoArchivoBO>());
                this.presentadorDocumentosRecepcion.EstablecerTiposArchivo(new List<TipoArchivoBO>());
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".EstablecerConfiguracionInicial: " + ex.Message);
            }
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

                //Se valida si el usuario tiene permiso para registrar
                if (!this.ExisteAccion(lst, "UI INSERTAR"))
                    this.vista.PermitirRegistrar(false);
                //Se valida si el usuario tiene permiso para editar
                if (!this.ExisteAccion(lst, "UI ACTUALIZAR"))
                    this.vista.PermitirEditar(false);
                //Se valida si el usuario tiene permiso para eliminar
                if (!this.ExisteAccion(lst, "BORRARCOMPLETO"))
                    this.vista.PermitirEliminar(false);
                //Se valida si el usuario tiene permiso para cerrar un contrato
                if (!this.ExisteAccion(lst, "UI TERMINARDOCUMENTO"))
                    this.vista.PermitirCerrar(false);
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
            this.presentadorDG.Inicializar();
            this.presentadorEA.Inicializar();
            this.presentadorHerramientas.Inicializar();
            this.presentadorResumen.Inicializar();
        }

        private object InterfazUsuarioADato()
        {
            ContratoRDBO bo = new ContratoRDBO();
            bo.Sucursal = new SucursalBO();
            bo.Sucursal.UnidadOperativa = new UnidadOperativaBO();

            bo.ContratoID = this.vista.ContratoID;
            if (this.vista.EstatusID != null)
                bo.Estatus = (EEstatusContrato)Enum.Parse(typeof(EEstatusContrato), this.vista.EstatusID.ToString());
            else
                bo.Estatus = null;
            bo.Sucursal.UnidadOperativa.Id = this.vista.UnidadOperativaID;

            return bo;
        }
        /// <summary>
        /// Asocia los datos del objeto Contrato a la interfaz
        /// </summary>
        /// <param name="bo">Origen de los datos</param>
        private void DatoAInterfazUsuario(ContratoRDBO bo) {
             try{

                 if (bo == null) {
                     bo = new ContratoRDBO();
                     if (bo.Sucursal == null) bo.Sucursal = new SucursalBO();
                     if (bo.Sucursal.UnidadOperativa == null) bo.Sucursal.UnidadOperativa = new UnidadOperativaBO();
                     return;
                 }
                this.vista.ContratoID = bo.ContratoID;
                if (bo.Estatus != null)
                    this.vista.EstatusID = (int)bo.Estatus;
                else
                    this.vista.EstatusID = null;

                this.presentadorHerramientas.DatosAInterfazUsuario(bo);

                if (bo != null && bo.DocumentosAdjuntos != null && bo.DocumentosAdjuntos.Count > 0)
                    bo.DocumentosAdjuntos = bo.DocumentosAdjuntos.Where(archivo => archivo.Activo == true).ToList();
                #region Lineas
                if (bo.ObtenerLineaContrato() != null && bo.ObtenerLineaContrato().Equipo != null && bo.ObtenerLineaContrato().Equipo is UnidadBO && ((UnidadBO)bo.ObtenerLineaContrato().Equipo).UnidadID != null){
                    ElementoFlotaBO elemento = new ElementoFlotaBO() { Unidad = (UnidadBO)bo.ObtenerLineaContrato().Equipo };
                    elemento.Tramites = new TramiteBR().ConsultarCompleto(this.dctx, new TramiteProxyBO() { Activo = true, Tramitable = elemento.Unidad }, false);
                    if (elemento != null && elemento.Unidad != null && elemento.Unidad.Sucursal == null) elemento.Unidad.Sucursal = new SucursalBO();

                    this.presentadorDG.DatoAInterfazUsuario(elemento as object);
                    this.presentadorDG.ProductoServicioAInterfazUsuario(bo.ObtenerLineaContrato().ProductoServicio);
                    this.presentadorDG.MostrarProductoServicio(true);
                    this.presentadorEA.DatoAInterfazUsuario(elemento as object);
                    this.presentadorEA.CargarEquiposAliados();
                }
                else {
                    this.presentadorDG.Inicializar();
                    this.presentadorEA.Inicializar();
                }
                
                int? kmRecorridos = null;
                if (bo.ObtenerLineaContrato() != null)
                    kmRecorridos = bo.ObtenerLineaContrato().CalcularKilometrajeRecorrido();
                #endregion
                //En cualquiera de estos casos, es Cancelación
                bool casoPermitido1 = bo.Estatus != null && bo.Estatus == EEstatusContrato.EnPausa;
                bool casoPermitido2 = bo.Estatus != null && bo.Estatus == EEstatusContrato.PendientePorCerrar && kmRecorridos != null && kmRecorridos == 0;
                //En este caso, es Cierre
                bool casoPermitido3 = bo.Estatus != null && bo.Estatus == EEstatusContrato.PendientePorCerrar && kmRecorridos != null && kmRecorridos > 0;

                this.vista.Cancelable = casoPermitido1 || casoPermitido2;
                this.vista.Cerrable = casoPermitido3;

                this.vista.PermitirCerrar(casoPermitido1 || casoPermitido2 || casoPermitido3);

                this.presentadorDocumentosContrato.Vista.EstablecerListasArchivos(bo.DocumentosAdjuntos, new List<TipoArchivoBO>());
                LineaContratoRDBO linea = bo.ObtenerLineaContrato();
                if (linea != null) {
                    ListadoVerificacionBO chkEntrega = new ListadoVerificacionBO();
                    ListadoVerificacionBO chkRecepcion = new ListadoVerificacionBO();
                    chkEntrega = linea.ObtenerListadoVerificacionPorTipo(ETipoListadoVerificacion.ENTREGA);
                    if(chkEntrega!= null)
                        this.presentadorDocumentosEntrega.Vista.EstablecerListasArchivos(chkEntrega.Adjuntos, new List<TipoArchivoBO>());
                    chkRecepcion = linea.ObtenerListadoVerificacionPorTipo(ETipoListadoVerificacion.RECEPCION);
                    if(chkRecepcion!=null)
                        this.presentadorDocumentosRecepcion.Vista.EstablecerListasArchivos(chkRecepcion.Adjuntos, new List<TipoArchivoBO>());
                }
                this.presentadorResumen.DatosAInterfazUsuario(bo);
                this.vista.UltimoObjeto = bo;

             } catch (Exception ex) {                 
                 throw new Exception(this.nombreClase + ".DatoAInterfazUsuario: " + ex.Message);
             }
        }
        

        /// <summary>
        /// Envia el contrato a Editar Contrato
        /// </summary>
        public void EditarContrato() {
            try {
                this.LimpiarSesion();
                this.vista.EstablecerPaqueteNavegacion("UltimoContratoRDBO", (ContratoRDBO) this.vista.UltimoObjeto);

                this.vista.RedirigirAEditar();
            } catch (Exception ex)  {
                throw new Exception(nombreClase + ".EditarContrato: " + ex.Message);
            }
        }
        /// <summary>
        /// Elimina el contrato desplegado con estatus de borrador
        /// </summary>
        public void EliminarContrato()
        {
            try
            {
                #region InterfazUsuarioADato Personalizado
                ContratoRDBO bo = (ContratoRDBO)this.vista.UltimoObjeto;

                if (bo.Estatus != EEstatusContrato.Borrador)
                {
                    vista.MostrarMensaje("El contrato debe tener estatus Borrador para ser eliminado.", ETipoMensajeIU.INFORMACION);
                    return;
                }

                //finalización del contrato
                FinalizacionContratoProxyBO finalizacionContrato = new FinalizacionContratoProxyBO();
                finalizacionContrato.Fecha = vista.FUA;
                finalizacionContrato.Usuario = new UsuarioBO { Id = vista.UUA };
                finalizacionContrato.Observaciones = vista.Observaciones;

                ContratoRDBO previous = new ContratoRDBO(bo);

                bo.CierreContrato = finalizacionContrato;
                bo.FUA = vista.FUA;
                bo.UUA = vista.UUA;
                #endregion

                //Se crea el objeto de seguridad
                UsuarioBO usuario = new UsuarioBO() { Id = this.vista.UsuarioID };
                AdscripcionBO adscripcion = new AdscripcionBO() { UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaID } };
                SeguridadBO seguridadBO = new SeguridadBO(Guid.Empty, usuario, adscripcion);

                this.controlador.BorrarCompleto(dctx, bo, previous, seguridadBO);

                this.LimpiarSesion();
                this.vista.LimpiarPaqueteNavegacion("ContratoRDBO");
                this.vista.RedirigirAConsulta();
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".EliminarContrato: " + ex.Message);
            }
        }
        public void CerrarContrato()
        {
            try
            {
                ContratoRDBO bo = (ContratoRDBO)this.vista.UltimoObjeto; 
                this.LimpiarSesion();
                this.vista.EstablecerPaqueteNavegacion("UltimoContratoRDBO", bo);

                if (this.vista.Cancelable != null && this.vista.Cancelable == true) {
                    this.vista.RedirigirACancelacion();
                    return;
                }
                if (this.vista.Cerrable != null && this.vista.Cerrable == true) {
                    this.vista.RedirigirACierre();
                    return;
                }

                this.vista.MostrarMensaje("El contrato no se puede cancelar ni cerrar", ETipoMensajeIU.ADVERTENCIA);
                return;
            }
            catch (Exception ex)
            {
                throw new Exception(nombreClase + ".CerrarContrato: " + ex.Message);
            }
        }
        /// <summary>
        /// Envia el contrato a Agregar Documentos
        /// </summary>
        public void AgregarDocumentos()
        {
            try
            {
                ContratoRDBO bo = (ContratoRDBO)this.InterfazUsuarioADato();

                this.LimpiarSesion();
                this.vista.EstablecerPaqueteNavegacion("UltimoContratoRDBO", bo);

                this.vista.RedirigirAAgregarDocumentos();
            }
            catch (Exception ex)
            {
                throw new Exception(nombreClase + ".AgregarDocumentos: " + ex.Message);
            }
        }
        /// <summary>
        /// Imprime el contrato de Renta Diaria
        /// </summary>
        public void ImprimirContrato()
        {
            try
            {
                ContratoRDBO bo = (ContratoRDBO)this.InterfazUsuarioADato();

                if (this.vista.ModuloID == null)
                    throw new Exception("No se identificó el módulo");

                Dictionary<string, Object> datosImprimir = this.controlador.ObtenerDatosContrato(dctx, bo, this.vista.ModuloID.Value);

                this.vista.EstablecerPaqueteNavegacion("NombreReporte", "CU014");
                this.vista.EstablecerPaqueteNavegacion("DatosReporte", datosImprimir);

                this.vista.RedirigirAImprimir();
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".ImprimirContrato: " + ex.Message);
            }
        }
        /// <summary>
        /// Imprime el contrato Maestro  de Renta Diaria
        /// </summary>
        public void ImprimirContratoMaestro()
        {
            try
            {
                ContratoRDBO bo = (ContratoRDBO)this.InterfazUsuarioADato();

                if (this.vista.ModuloID == null)
                    throw new Exception("No se identificó el módulo");

                Dictionary<string, Object> datosImprimir = this.controlador.ObtenerPlantillaContrato(dctx, bo, this.vista.ModuloID.Value);

                this.vista.EstablecerPaqueteNavegacion("NombreReporte", "CU014");
                this.vista.EstablecerPaqueteNavegacion("DatosReporte", datosImprimir);

                this.vista.RedirigirAImprimir();
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".ImprimirContratoMaestro: " + ex.Message);
            }
        }
        #region CU012
        /// <summary>
        /// Genera el reporte pra la impresión de l check List Completo
        /// </summary>
        public void ImprimirCheckList()
        {
            try
            {
                ContratoRDBO bo = (ContratoRDBO)this.InterfazUsuarioADato();

                if (this.vista.ModuloID == null)
                    throw new Exception("No se identificó el módulo");

                Dictionary<string, Object> datosImprimir = this.controlador.ObtenerDatosCheckList(dctx, bo, this.vista.ModuloID.Value);

                this.vista.EstablecerPaqueteNavegacion("NombreReporte", "CU012");
                this.vista.EstablecerPaqueteNavegacion("DatosReporte", datosImprimir);

                this.vista.RedirigirAImprimir();
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".ImprimirCheckList: " + ex.Message);
            }
        }
        /// <summary>
        /// Genera el reporte para la impresión solo con los datos de la cabecera
        /// </summary>
        public void ImprimirCabeceraCheckList()
        {
            try
            {
                ContratoRDBO bo = (ContratoRDBO)this.InterfazUsuarioADato();

                if (this.vista.ModuloID == null)
                    throw new Exception("No se identificó el módulo");

                Dictionary<string, Object> datosImprimir = this.controlador.ObtenerDatosCabeceraCheckList(dctx, bo, this.vista.ModuloID.Value);

                this.vista.EstablecerPaqueteNavegacion("NombreReporte", "CU012");
                this.vista.EstablecerPaqueteNavegacion("DatosReporte", datosImprimir);

                this.vista.RedirigirAImprimir();
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".ImprimirCabeceraCheckList: " + ex.Message);
            }
        }
        #endregion

        public void Regresar()
        {
            this.LimpiarSesion();
            this.vista.LimpiarPaqueteNavegacion("ContratoRDBO");
            this.vista.RedirigirAConsulta();
        }

        private void LimpiarSesion()
        {
            this.vista.LimpiarSesion();
            this.presentadorDocumentosContrato.LimpiarSesion();
            this.presentadorDocumentosEntrega.LimpiarSesion();
            this.presentadorDocumentosRecepcion.LimpiarSesion();
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
		public void ImprimirPagare()
		{
			try
			{
				if (this.vista.ContratoID == null)
					throw new Exception("No se puede imprimir el pagaré sin el identificador del contrato");
				ContratoRDBR controlador = new ContratoRDBR();
				Dictionary<string, Object> datosPagare = controlador.ObtenerDatosPagare(FacadeBR.ObtenerConexion(), new ContratoRDBO() { ContratoID = this.vista.ContratoID });
				vista.EstablecerPaqueteNavegacion("NombreReporte", "CU094");
				vista.EstablecerPaqueteNavegacion("DatosReporte", datosPagare);
				vista.RedirigirAImprimir();

			}
			catch (Exception ex)
			{
				throw new Exception("DetalleContratoRDPRE.ImprimirPagare: " + ex.Message);
			}
		}
        #endregion
    }
}