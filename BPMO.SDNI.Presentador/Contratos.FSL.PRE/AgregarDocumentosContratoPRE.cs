// Satisface al Caso de Uso CU023 - Editar Contrato Full Service Leasing
using System;
using System.Collections.Generic;
using System.Linq;
using BPMO.Basicos.BO;
using BPMO.Facade.SDNI.BR;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Comun.BR;
using BPMO.SDNI.Comun.PRE;
using BPMO.SDNI.Contratos.BR;
using BPMO.SDNI.Contratos.FSL.BO;
using BPMO.SDNI.Contratos.FSL.VIS;

namespace BPMO.SDNI.Contratos.FSL.PRE
{
    public class AgregarDocumentosContratoPRE
    {
        #region Atributos

        /// <summary>
        /// Vista sobre la que actua la interfaz
        /// </summary>
        private readonly IAgregarDocumentosContratoVIS vista;

        /// <summary>
        /// El DataContext que proveerá acceso a la base de datos
        /// </summary>
        private readonly IDataContext dataContext;

        /// <summary>
        /// Nombre de la clase para agregar a los mensajes de Error
        /// </summary>
        private const string nombreClase = "AgregarDocumentosContratoPRE";

        /// <summary>
        /// Presentador de la Barra de Herramientas
        /// </summary>
        private readonly ucHerramientasFSLPRE herramientasPRE;
        /// <summary>
        /// Presentador de Documentos Adjuntos al Contrato
        /// </summary>
        private readonly ucCatalogoDocumentosPRE documentosPRE;

        #endregion

        #region Constructores

        /// <summary>
        /// Constructor que recibe la vista sobre la que actuará el presentador
        /// </summary>
        /// <param name="vistaActual">vista sobre la que actuará el presentador</param>
        /// <param name="herramientas">Presentador de la barra de herramientas</param>
        /// <param name="general">Presentador de la Informacion General</param>
        /// <param name="cliente">Presentador de los datos del Cliente</param>
        /// <param name="datosRenta">Presentador de los datos de Renta</param>
        /// <param name="pago">Presentador de la informacion de Pago</param>
        /// <param name="lineaContrato">Presentador de las lineas de contrato</param>
        /// <param name="documentos">Presentador de las documentos</param>
        public AgregarDocumentosContratoPRE(IAgregarDocumentosContratoVIS vistaActual, ucHerramientasFSLPRE herramientas,
                                    ucCatalogoDocumentosPRE documentos)
        {
            if (vistaActual != null)
                vista = vistaActual;

            dataContext = Facade.SDNI.BR.FacadeBR.ObtenerConexion();

            herramientasPRE = herramientas;
            documentosPRE = documentos;
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Inicializa la vista para editar un contrato
        /// </summary>
        public void Inicializar()
        {
            herramientasPRE.Inicializar();

            vista.CodigoUltimoObjeto = "UltimoContratoFSLBO";

            DesplegarTiposArchivos();

            ContratoFSLBO contrato = vista.UltimoObjeto;

            documentosPRE.ModoEditable(true);

            if (contrato != null && contrato.DocumentosAdjuntos != null && contrato.DocumentosAdjuntos.Count > 0)
                contrato.DocumentosAdjuntos =
                    contrato.DocumentosAdjuntos.Where(archivo => archivo.Activo == true).ToList();

            DatosAInterfazUsuario(contrato);

            herramientasPRE.vista.HabilitarOpcionesEdicion();
            herramientasPRE.vista.DeshabilitarOpcionesEditarContratoFSL();
            herramientasPRE.vista.MarcarOpcionAgregarDocumentos();
            herramientasPRE.vista.OcultarFormatosContrato();
            herramientasPRE.vista.OcultarPlantillas();

            this.EstablecerSeguridad();//SC_0008
        }

        /// <summary>
        /// Despliega los datos del contrato en la interfaz de usuario
        /// </summary>
        /// <param name="contrato">Contrato que contiene los datos</param>
        public void DatosAInterfazUsuario(ContratoFSLBO contrato)
        {
            vista.ContratoID = contrato.ContratoID;
            vista.Estatus = contrato.Estatus;
            if (contrato.Sucursal != null && contrato.Sucursal.UnidadOperativa != null)
                vista.UnidadOperativaContratoID = contrato.Sucursal.UnidadOperativa.Id;
            herramientasPRE.DatosAInterfazUsuario(contrato);
            documentosPRE.CargarListaArchivos(contrato.DocumentosAdjuntos, documentosPRE.Vista.TiposArchivo);
        }

        /// <summary>
        /// Despliega los tipos de archivo
        /// </summary>
        public void DesplegarTiposArchivos()
        {
            var tipoBR = new TipoArchivoBR();

            var tipo = new TipoArchivoBO { EsImagen = false };

            vista.CargarTiposArchivos(tipoBR.Consultar(dataContext, tipo));
        }

        /// <summary>
        /// Actualiza la lista de documentos
        /// </summary>
        public void ActualizarDocumentos()
        {
            try
            {
                //SC_0008
                SeguridadBO seguridadBO = new SeguridadBO(Guid.Empty, new UsuarioBO { Id = this.vista.UsuarioID }, new AdscripcionBO
                {
                    Departamento = new DepartamentoBO(),
                    Sucursal = new SucursalBO(),
                    UnidadOperativa = new UnidadOperativaBO { Id = this.vista.UnidadAdscripcionID }
                });
                //End SC_0008

                ContratoFSLBO contrato = vista.UltimoObjeto;
                List<ArchivoBO> adjuntos = documentosPRE.Vista.ObtenerArchivos();
                foreach (ArchivoBO adjuntoContratoBo in adjuntos)
                {
                    adjuntoContratoBo.TipoAdjunto = ETipoAdjunto.ContratoFSL;
                }
                contrato.DocumentosAdjuntos = adjuntos;

                // Se agrega la Auditoria de cada uno de los documentos adjuntos
                foreach (ArchivoBO adjunto in contrato.DocumentosAdjuntos)
                {
                    if (adjunto.Id == null)
                    {
                        adjunto.Auditoria = new AuditoriaBO
                        {

                            FC = contrato.FUA,
                            UC = contrato.UUA,
                            FUA = contrato.FUA,
                            UUA = contrato.UUA
                        };
                    }
                    else
                    {
                        adjunto.Auditoria.FUA = contrato.FUA;
                        adjunto.Auditoria.UUA = contrato.UUA;
                    }
                }

                // Se actualiza el Contrato
                var contratoBR = new ContratoBR();
                contratoBR.ActualizarDocumento(dataContext, contrato, vista.UltimoObjeto, seguridadBO);//SC_0008
                vista.EstablecerPaqueteNavegacion(vista.Codigo, contrato);
                vista.IrADetalleContrato();
                vista.MostrarMensaje("Se han actualizado los documentos del contrato exitosamente.", ETipoMensajeIU.EXITO);
            }
            catch (Exception ex)
            {
                vista.MostrarMensaje("Inconsistencia al actualizar los documentos del contrato", ETipoMensajeIU.ERROR, nombreClase + ".AgregarDocumentos:" + ex.Message);
            }
        }

        /// <summary>
        /// Regresar a la pantalla de Detalles
        /// </summary>
        public void RegresarADetalles()
        {
            try
            {
                vista.EstablecerPaqueteNavegacion(vista.Codigo, vista.UltimoObjeto);
                vista.LimpiarSesion();
                vista.IrADetalleContrato();
            }
            catch (Exception ex)
            {
                vista.MostrarMensaje("Inconsistencias al intentar regresar al Detalle del Contrato", ETipoMensajeIU.ERROR, nombreClase + ".RegresarADetalles: " + ex.Message);
            }
        }

        #region SC_0008
        /// <summary>
        /// Valida el acceso a la página de edición
        /// </summary>
        public void ValidarAcceso()
        {
            try
            {
                //se valida que los datos del usuario y la unidad operativa no sean nulos
                if (this.vista.UsuarioID == null) throw new Exception("El identificador del usuario no debe ser nulo");
                if (this.vista.UnidadAdscripcionID == null) throw new Exception("El identificador de la Unidad Operativa no debe ser nulo ");

                UsuarioBO usr = new UsuarioBO { Id = this.vista.UsuarioID };
                AdscripcionBO adscripcion = new AdscripcionBO { UnidadOperativa = new UnidadOperativaBO { Id = this.vista.UnidadAdscripcionID } };
                SeguridadBO seguridadBo = new SeguridadBO(Guid.Empty, usr, adscripcion);

                if (!FacadeBR.ExisteAccion(this.dataContext, "ACTUALIZARDOCUMENTO", seguridadBo))
                    this.vista.RedirigirSinPermisoAcceso();
            }
            catch (Exception ex)
            {
                throw new Exception(nombreClase + ".ValidarAcceso:" + ex.Message);
            }
        }
        /// <summary>
        /// Permite configurar el comportamiento de los controles de la vista de acuerdo a los permisos configurados
        /// </summary>
        private void EstablecerSeguridad()
        {
            try
            {
                //se valida que los datos del usuario y la unidad operativa no sean nulos
                if (this.vista.UsuarioID == null) throw new Exception("El identificador del usuario no debe ser nulo");
                if (this.vista.UnidadAdscripcionID == null) throw new Exception("El identificador de la Unidad Operativa no debe ser nulo ");

                //Se crea el objeto de seguridad
                UsuarioBO usr = new UsuarioBO {Id = this.vista.UsuarioID};
                AdscripcionBO adscripcion = new AdscripcionBO { UnidadOperativa = new UnidadOperativaBO { Id = this.vista.UnidadAdscripcionID } };
                SeguridadBO seguridadBO = new SeguridadBO(Guid.Empty, usr, adscripcion);

                //Se obtiene las acciones a las cuales el usuario tiene permiso en este proceso
                List<CatalogoBaseBO> lst = FacadeBR.ConsultarAccion(this.dataContext, seguridadBO);

                if (!this.ExisteAcccion(lst, "ACTUALIZARDOCUMENTO"))
                    this.vista.RedirigirSinPermisoAcceso();
                if (!this.ExisteAcccion(lst, "UI INSERTAR"))
                    this.vista.PermitirRegistrar(false);
                if (!this.ExisteAcccion(lst, "ACTUALIZARCOMPLETO"))
                    this.PermitirActualizarDocumento();
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
        /// Deshabilita la opción de agergar docuemntos de la barra de herramientas de acuerdo a los permisos configurados
        /// </summary>
        private void PermitirActualizarDocumento()
        {
            this.herramientasPRE.DeshabilitarMenuDocumentos();
        }
        #endregion
        #endregion
    }
}