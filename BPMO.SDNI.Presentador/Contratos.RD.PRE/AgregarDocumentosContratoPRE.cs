// Satisface al CU002 - Editar Contrato Renta Diaria
using System;
using System.Collections.Generic;
using System.Linq;

using BPMO.Facade.SDNI.BR;

using BPMO.Basicos.BO;
using BPMO.Primitivos.Enumeradores;
using BPMO.Patterns.Creational.DataContext;

using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Comun.BR;
using BPMO.SDNI.Comun.VIS;
using BPMO.SDNI.Comun.PRE;
using BPMO.SDNI.Contratos.BO;
using BPMO.SDNI.Contratos.RD.BR;
using BPMO.SDNI.Contratos.RD.BO;
using BPMO.SDNI.Contratos.RD.VIS;

namespace BPMO.SDNI.Contratos.RD.PRE
{
    public class AgregarDocumentosContratoPRE
    {
        #region Atributos
        /// <summary>
        /// El DataContext que proveerá acceso a la base de datos
        /// </summary>
        private readonly IDataContext dctx;

        /// <summary>
        /// Nombre de la clase para agregar a los mensajes de Error
        /// </summary>
        private string nombreClase = "AgregarDocumentosContratoPRE";

        /// <summary>
        /// Presentador de la Barra de Herramientas
        /// </summary>
        private ucHerramientasRDPRE presentadorHerramientas;
        /// <summary>
        /// Presentador de Documentos Adjuntos al Contrato
        /// </summary>
        private ucCatalogoDocumentosPRE presentadorDocumentos;

        private IAgregarDocumentosContratoVIS vista;

        private ContratoRDBR controlador;
        #endregion

        #region Constructores
        /// <summary>
        /// Constructor que recibe la vista sobre la que actuará el presentador
        /// </summary>
        /// <param name="view">vista sobre la que actuará el presentador</param>
        /// <param name="viewHerramientas">Vista de la barra de herramientas</param>
        /// <param name="viewDocumentos">Vista de los documentos</param>
        public AgregarDocumentosContratoPRE(IAgregarDocumentosContratoVIS view, IucHerramientasRDVIS viewHerramientas, IucCatalogoDocumentosVIS viewDocumentos)
        {
            try
            {
                this.vista = view;

                this.presentadorDocumentos = new ucCatalogoDocumentosPRE(viewDocumentos);
                this.presentadorHerramientas = new ucHerramientasRDPRE(viewHerramientas);

                this.controlador = new ContratoRDBR();
                this.dctx = FacadeBR.ObtenerConexion();
            }
            catch (Exception ex)
            {
                this.vista.MostrarMensaje("Inconsistencia en los parámetros de configuración", ETipoMensajeIU.ERROR, this.nombreClase + ".AgregarDocumentosContratoPRE:" + ex.Message);
            }
        }
        #endregion

        #region Métodos
        public void RealizarPrimeraCarga()
        {
            try
            {
                this.EstablecerDatosNavegacion(this.vista.ObtenerPaqueteNavegacion("UltimoContratoRDBO"));

                //Preparar Edición
                this.presentadorHerramientas.Inicializar();

                this.ConsultarCompleto();

                //Configuraciones iniciales
                this.vista.FUA = DateTime.Now;
                this.vista.UUA = this.vista.UsuarioID;
                List<TipoArchivoBO> lstTiposArchivo = new TipoArchivoBR().Consultar(this.dctx, new TipoArchivoBO());
                this.presentadorDocumentos.EstablecerTiposArchivo(lstTiposArchivo);

                this.presentadorDocumentos.ModoEditable(true);
                this.presentadorHerramientas.vista.OcultarImprimirPlantilla();
                this.presentadorHerramientas.vista.OcultarImprimirPlantillaCheckList();
                this.presentadorHerramientas.vista.HabilitarOpcionesEdicion();
                this.presentadorHerramientas.DeshabilitarMenuCerrar();
                this.presentadorHerramientas.DeshabilitarMenuBorrar();
                this.presentadorHerramientas.DeshabilitarMenuImprimir();
                this.presentadorHerramientas.vista.MarcarOpcionAgregarDocumentos();
                this.presentadorHerramientas.vista.OcultarPlantillas();
                this.presentadorDocumentos.RequiereObservaciones(false);

                this.EstablecerSeguridad();
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".Inicializar: " + ex.Message);
            }
        }
        private void EstablecerDatosNavegacion(object paqueteNavegacion)
        {
            try
            {
                if (paqueteNavegacion == null)
                    throw new Exception("Se esperaba un objeto en la navegación. No se puede identificar qué reservación se desea consultar.");
                if (!(paqueteNavegacion is ContratoRDBO))
                    throw new Exception("Se esperaba un Contrato de Renta Diaria.");

                ContratoRDBO bo = (ContratoRDBO)paqueteNavegacion;

                this.DatoAInterfazUsuario(bo);
            }
            catch (Exception ex)
            {
                this.DatoAInterfazUsuario(new ContratoRDBO());
                throw new Exception(this.nombreClase + ".EstablecerDatosNavegacion: " + ex.Message);
            }
        }
        private void ConsultarCompleto()
        {
            try
            {
                ContratoRDBO bo = (ContratoRDBO)this.InterfazUsuarioADato();

                List<ContratoRDBO> lst = new ContratoRDBR().Consultar(this.dctx, bo);

                if (lst.Count < 1)
                    throw new Exception("No se encontró ningún registro que corresponda a la información proporcionada.");
                if (lst.Count > 1)
                    throw new Exception("La consulta devolvió más de un registro.");

                lst[0].DocumentosAdjuntos = new ArchivoBR().Consultar(this.dctx, new ArchivoBO() { Activo = true, TipoAdjunto = ETipoAdjunto.ContratoRD }, lst[0].ContratoID);

                this.DatoAInterfazUsuario(lst[0]);
                this.vista.UltimoObjeto = lst[0];
            }
            catch (Exception ex)
            {
                this.DatoAInterfazUsuario(new ContratoRDBO());
                throw new Exception(this.nombreClase + ".ConsultarCompleto:" + ex.Message);
            }
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
                if (!FacadeBR.ExisteAccion(this.dctx, "ACTUALIZARDOCUMENTO", seguridadBO))
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
                List<CatalogoBaseBO> lst = FacadeBR.ConsultarAccion(dctx, seguridad);

                if (!this.ExisteAcccion(lst, "ACTUALIZARDOCUMENTO"))
                    this.vista.RedirigirSinPermisoAcceso();
                if (!this.ExisteAcccion(lst, "UI INSERTAR"))
                    this.vista.PermitirRegistrar(false);
                if (!this.ExisteAcccion(lst, "ACTUALIZARCOMPLETO"))
                    this.presentadorHerramientas.DeshabilitarMenuDocumentos();
            }
            catch (Exception ex)
            {
                throw new Exception(nombreClase + ".EstablecerSeguridad:" + ex.Message);
            }
        }
        /// <summary>
        /// Valida una acción en especifico dentro de la lista de acciones permtidas para la pagina
        /// </summary>
        /// <param name="acciones">Listado de acciones permitidas para la página</param>
        /// <param name="accion">Acción que se desea validar</param>
        /// <returns>si la acción a evaluar se encuantra dentro de la lista de acciones permitidas se devuelve true. En caso ocntario false. bool</returns>
        private bool ExisteAcccion(List<CatalogoBaseBO> acciones, string accion)
        {
            if (acciones != null && acciones.Count > 0 && acciones.Exists(p => p != null && p.Nombre != null && p.Nombre.Trim().ToUpper() == accion.Trim().ToUpper()))
                return true;

            return false;
        }
        
        public void CancelarEdicion()
        {
            this.LimpiarSesion();
            this.vista.LimpiarPaqueteNavegacion("UltimoContratoRDBO");
            this.vista.EstablecerPaqueteNavegacion("ContratoRDBO", new ContratoRDBO() { ContratoID = this.vista.ContratoID });
            this.vista.RedirigirADetalles();
        }
        public void ActualizarDocumentos()
        {
            try
            {
                //Se obtiene la información a partir de la Interfaz de Usuario
                ContratoRDBO bo = (ContratoRDBO)this.InterfazUsuarioADato();

                //Se crea el objeto de seguridad
                UsuarioBO usuario = new UsuarioBO() { Id = this.vista.UsuarioID };
                AdscripcionBO adscripcion = new AdscripcionBO() { UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaID } };
                SeguridadBO seguridadBO = new SeguridadBO(Guid.Empty, usuario, adscripcion);
                
                // Se actualiza el Contrato
                this.controlador.ActualizarDocumento(this.dctx, bo, (ContratoRDBO)this.vista.UltimoObjeto, seguridadBO);

                this.LimpiarSesion();
                this.vista.LimpiarPaqueteNavegacion("UltimoContratoRDBO");
                this.vista.EstablecerPaqueteNavegacion("ContratoRDBO", new ContratoRDBO() { ContratoID = this.vista.ContratoID });
                this.vista.RedirigirADetalles();
            }
            catch (Exception ex)
            {
                vista.MostrarMensaje("Inconsistencia al actualizar los documentos del contrato", ETipoMensajeIU.ERROR, nombreClase + ".AgregarDocumentos:" + ex.Message);
            }
        }

        /// <summary>
        /// Despliega los datos del contrato en la interfaz de usuario
        /// </summary>
        /// <param name="obj">Contrato que contiene los datos</param>
        public void DatoAInterfazUsuario(object obj)
        {
            ContratoRDBO bo = (ContratoRDBO)obj;

            this.vista.ContratoID = bo.ContratoID;
            if (bo.Estatus != null)
                this.vista.EstatusID = (int)bo.Estatus;
            else
                this.vista.EstatusID = null;

            this.presentadorHerramientas.DatosAInterfazUsuario(bo);
            this.presentadorDocumentos.CargarListaArchivos(bo.DocumentosAdjuntos, presentadorDocumentos.Vista.TiposArchivo);
        }
        private object InterfazUsuarioADato()
        {
            ContratoRDBO bo = new ContratoRDBO();

            if (this.vista.UltimoObjeto != null && this.vista.UltimoObjeto is ContratoRDBO)
                bo = new ContratoRDBO((ContratoRDBO)this.vista.UltimoObjeto);

            bo.ContratoID = this.vista.ContratoID;
            if (this.vista.EstatusID != null)
                bo.Estatus = (EEstatusContrato)Enum.Parse(typeof(EEstatusContrato), this.vista.EstatusID.ToString());
            else
                bo.Estatus = null;
            bo.FUA = this.vista.FUA;
            bo.UUA = this.vista.UUA;

            List<ArchivoBO> adjuntos = this.presentadorDocumentos.Vista.ObtenerArchivos();
            if (adjuntos != null)
            {
                foreach (ArchivoBO adjuntoContratoBo in adjuntos)
                    adjuntoContratoBo.TipoAdjunto = ETipoAdjunto.ContratoRD;
            }
            bo.DocumentosAdjuntos = adjuntos;

            // Se agrega la Auditoria de cada uno de los documentos adjuntos
            if (bo.DocumentosAdjuntos != null)
            {
                foreach (ArchivoBO adjunto in bo.DocumentosAdjuntos)
                {
                    if (adjunto.Id == null)
                    {
                        adjunto.Auditoria = new AuditoriaBO
                        {
                            FC = bo.FUA,
                            UC = bo.UUA,
                            FUA = bo.FUA,
                            UUA = bo.UUA
                        };
                    }
                    else
                    {
                        adjunto.Auditoria.FUA = bo.FUA;
                        adjunto.Auditoria.UUA = bo.UUA;
                    }
                }
            }

            return bo;
        }

        private void LimpiarSesion()
        {
            this.vista.LimpiarSesion();
            this.presentadorDocumentos.LimpiarSesion();
        }
        #endregion
    }
}