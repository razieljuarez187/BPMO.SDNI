// Esta clase satisface los requerimientos del CU028 - Editar Contrato de Mantenimiento
using System;
using System.Collections.Generic;
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

namespace BPMO.SDNI.Contratos.Mantto.PRE
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
        private ucHerramientasManttoPRE presentadorHerramientas;
        /// <summary>
        /// Presentador de Documentos Adjuntos al Contrato
        /// </summary>
        private ucCatalogoDocumentosPRE presentadorDocumentos;
        /// <summary>
        /// Vista para agregar o quitar documentos
        /// </summary>
        private IAgregarDocumentosContratoVIS vista;
        /// <summary>
        /// COntrolador que ejecuta la acciones
        /// </summary>
        private ContratoManttoBR controlador;
        #endregion

        #region Constructores
        /// <summary>
        /// Constructor que recibe la vista sobre la que actuará el presentador
        /// </summary>
        /// <param name="view">vista sobre la que actuará el presentador</param>
        /// <param name="viewHerramientas">Vista de la barra de herramientas</param>
        /// <param name="viewDocumentos">Vista de los documentos</param>
        public AgregarDocumentosContratoPRE(IAgregarDocumentosContratoVIS view, IucHerramientasManttoVIS viewHerramientas, IucCatalogoDocumentosVIS viewDocumentos)
        {
            try
            {
                this.vista = view;

                this.presentadorDocumentos = new ucCatalogoDocumentosPRE(viewDocumentos);
                this.presentadorHerramientas = new ucHerramientasManttoPRE(viewHerramientas);

                this.controlador = new ContratoManttoBR();

                this.dctx = FacadeBR.ObtenerConexion();
            }
            catch (Exception ex)
            {
                this.vista.MostrarMensaje("Inconsistencia en los parámetros de configuración", ETipoMensajeIU.ERROR, this.nombreClase + ".AgregarDocumentosContratoPRE:" + ex.Message);
            }
        }
        #endregion

        #region Métodos
        /// <summary>
        /// Realiza la carga inicial para la edición del contrato
        /// </summary>
        public void RealizarPrimeraCarga()
        {
            try
            {
                this.EstablecerDatosNavegacion(this.vista.ObtenerPaqueteNavegacion("ContratoManttoBO"));

                //Preparar Edición
                this.presentadorHerramientas.Inicializar();

                this.ConsultarCompleto();

                //Configuraciones iniciales
                this.vista.FUA = DateTime.Now;
                this.vista.UUA = this.vista.UsuarioID;
                //Cargamos los tipos de archivo validos para adjuntar al contrato
                List<TipoArchivoBO> lstTiposArchivo = new TipoArchivoBR().Consultar(this.dctx, new TipoArchivoBO());
                this.presentadorDocumentos.EstablecerTiposArchivo(lstTiposArchivo);

                this.presentadorDocumentos.ModoEditable(true);
                this.presentadorHerramientas.vista.HabilitarOpcionesEdicion();
                this.presentadorHerramientas.DeshabilitarMenuCerrar();
                this.presentadorHerramientas.DeshabilitarMenuBorrar();
                this.presentadorHerramientas.DeshabilitarMenuImprimir();
                this.presentadorHerramientas.vista.MarcarOpcionAgregarDocumentos();
                this.presentadorDocumentos.RequiereObservaciones(false);

                this.EstablecerSeguridad();
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".Inicializar: " + ex.Message);
            }
        }
        /// <summary>
        /// Establece el paquete de navegación que será retornado a las vistas
        /// </summary>
        /// <param name="paqueteNavegacion">contrato de mantenimiento que se desea usar</param>
        private void EstablecerDatosNavegacion(object paqueteNavegacion)
        {
            try
            {
                if (paqueteNavegacion == null)
                    throw new Exception("Se esperaba un objeto en la navegación. No se puede identificar qué contrato se desea consultar.");
                if (!(paqueteNavegacion is ContratoManttoBO))
                    throw new Exception("Se esperaba un Contrato de mantenimiento.");

                var bo = (ContratoManttoBO)paqueteNavegacion;

                this.DatoAInterfazUsuario(bo);
            }
            catch (Exception ex)
            {
                this.DatoAInterfazUsuario(new ContratoManttoBO());
                throw new Exception(this.nombreClase + ".EstablecerDatosNavegacion: " + ex.Message);
            }
        }
        /// <summary>
        /// Consulta la información completa del contrato para su despliegue en la vista
        /// </summary>
        private void ConsultarCompleto()
        {
            try
            {
                var bo = (ContratoManttoBO)this.InterfazUsuarioADato();
                var temp = new ContratoManttoBO();

                if (bo.ContratoID.HasValue)
                    temp.ContratoID = bo.ContratoID.Value;
                else
                    temp = bo;

                List<ContratoManttoBO> lst = new ContratoManttoBR().Consultar(this.dctx, temp);

                if (lst.Count < 1)
                    throw new Exception("No se encontró ningún registro que corresponda a la información proporcionada.");
                if (lst.Count > 1)
                    throw new Exception("La consulta devolvió más de un registro.");

                var contrato = new ContratoManttoBO();

                if (lst != null && lst.Count > 0)
                    contrato = lst[0];

                var tipoContrato = ETipoAdjunto.ContratoMantenimiento;

                if (contrato != null)
                {
                    if (contrato.Tipo.HasValue)
                    {
                        switch (contrato.Tipo.Value)
                        {
                            case ETipoContrato.CM:
                                tipoContrato = ETipoAdjunto.ContratoMantenimiento;
                                break;
                            case ETipoContrato.SD:
                                tipoContrato = ETipoAdjunto.ContratoServicioDedicado;
                                break;
                        }
                    }
                }

                lst[0].DocumentosAdjuntos = new ArchivoBR().Consultar(this.dctx, new ArchivoBO() { Activo = true, TipoAdjunto = tipoContrato }, contrato.ContratoID.Value);

                this.DatoAInterfazUsuario(lst[0]);
                this.vista.UltimoObjeto = lst[0];
            }
            catch (Exception ex)
            {
                this.DatoAInterfazUsuario(new ContratoManttoBO());
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
        /// <summary>
        /// Cancela la edición del contrato
        /// </summary>
        public void CancelarEdicion()
        {
            this.LimpiarSesion();
            this.vista.LimpiarPaqueteNavegacion("ContratoManttoBO");
            this.vista.EstablecerPaqueteNavegacion("ContratoManttoBO", new ContratoManttoBO() { ContratoID = this.vista.ContratoID });
            this.vista.RedirigirADetalles();
        }
        /// <summary>
        /// Despleiga en la vista los documentos asociados al contrato
        /// </summary>
        public void ActualizarDocumentos()
        {
            try
            {
                //Se obtiene la información a partir de la Interfaz de Usuario
                var bo = (ContratoManttoBO)this.InterfazUsuarioADato();

                //Se crea el objeto de seguridad
                UsuarioBO usuario = new UsuarioBO() { Id = this.vista.UsuarioID };
                AdscripcionBO adscripcion = new AdscripcionBO() { UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaID } };
                SeguridadBO seguridadBO = new SeguridadBO(Guid.Empty, usuario, adscripcion);

                // Se actualiza el Contrato
                this.controlador.ActualizarDocumento(this.dctx, bo, (ContratoManttoBO)this.vista.UltimoObjeto, seguridadBO);

                this.LimpiarSesion();
                this.vista.LimpiarPaqueteNavegacion("ContratoManttoBO");
                this.vista.EstablecerPaqueteNavegacion("ContratoManttoBO", new ContratoManttoBO() { ContratoID = this.vista.ContratoID, Tipo = ETipoContrato.CM });
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
            var bo = (ContratoManttoBO)obj;

            this.vista.ContratoID = bo.ContratoID;
            if (bo.Estatus != null)
                this.vista.EstatusID = (int)bo.Estatus;
            else
                this.vista.EstatusID = null;

            this.presentadorHerramientas.DatosAInterfazUsuario(bo);

            if (this.presentadorDocumentos.Vista.TiposArchivo == null)
                this.presentadorDocumentos.Vista.TiposArchivo = new List<TipoArchivoBO>();

            this.presentadorDocumentos.CargarListaArchivos(bo.DocumentosAdjuntos, presentadorDocumentos.Vista.TiposArchivo);
        }
        /// <summary>
        /// Obtiene la inforamción del contrato proporcionada por el usaurio para su actualización
        /// </summary>
        /// <returns>Contrato que será actualizado</returns>
        private object InterfazUsuarioADato()
        {
            var bo = new ContratoManttoBO();

            if (this.vista.UltimoObjeto is ContratoManttoBO)
                bo = new ContratoManttoBO((ContratoManttoBO)this.vista.UltimoObjeto);

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
                {
                    if (bo != null)
                    {
                        if (bo.Tipo.HasValue)
                        {
                            switch (bo.Tipo.Value)
                            {
                                case ETipoContrato.CM:
                                    adjuntoContratoBo.TipoAdjunto = ETipoAdjunto.ContratoMantenimiento;
                                    break;
                                case ETipoContrato.SD:
                                    adjuntoContratoBo.TipoAdjunto = ETipoAdjunto.ContratoServicioDedicado;
                                    break;
                            }
                        }
                    }
                }
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
        /// <summary>
        /// Limpia la variable de session de la vista
        /// </summary>
        private void LimpiarSesion()
        {
            this.vista.LimpiarSesion();
            this.presentadorDocumentos.LimpiarSesion();
        }
        #endregion
    }
}