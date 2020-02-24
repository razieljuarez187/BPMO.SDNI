// Satisface al CU097 - Cargar y descargar formatos y plantillas
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BPMO.Basicos.BO;
using BPMO.Facade.SDNI.BR;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Comun.BR;
using BPMO.SDNI.Contratos.BO;
using BPMO.SDNI.Contratos.BR;
using BPMO.SDNI.Contratos.VIS;

namespace BPMO.SDNI.Contratos.PRE
{
    public class RegistrarPlantillaPRE
    {
        #region Atributos
        /// <summary>
        /// Vista para la página de registro de entrega de unidad
        /// </summary>
        private readonly IRegistrarPlantillaVIS vista;
        /// <summary>
        /// Provee la conexión a la BD
        /// </summary>
        private readonly IDataContext dctx;
        /// <summary>
        /// Nombre de la clase que se usará en los mensajes de error
        /// </summary>
        private const string nombreClase = "RegistrarPlantillaPRE";
        /// <summary>
        /// Controlador que ejecutará las accciones
        /// </summary>
        private readonly PlantillaBR controlador;
        ///// <summary>
        ///// Presentador del catálogo de documentos
        ///// </summary>
        //private ucCatalogoDocumentosPRE presentadorDocumentos;
        #endregion

        #region Constructores
        /// <summary>
        /// Constructor del presentador para el registro de documentos
        /// </summary>
        /// <param name="vista">Vista de la página principal</param>
        public RegistrarPlantillaPRE(IRegistrarPlantillaVIS vista)
        {
            try
            {
                if (ReferenceEquals(vista, null))
                    throw new Exception(String.Format("{0}: La vista proporcionada no puede ser nula", nombreClase));

                this.vista = vista;
                this.controlador = new PlantillaBR();
                this.dctx = FacadeBR.ObtenerConexion();
            }
            catch (Exception ex)
            {
                this.vista.MostrarMensaje("Inconsistencias en los parámetros de configuración", ETipoMensajeIU.ERROR, nombreClase + "RegistrarPlantillaPRE" + Environment.NewLine + ex.Message);
            }
        }
        #endregion

        #region Métodos
        /// <summary>
        /// Genera la lista de opciones para el tipo de plantilla en base a la enumeracion correspondiente
        ///<para>SI deseas visualizar los valores correspondientes para la enumeración consulta:</para>
        /// <seealso cref="BPMO.SDNI.Contratos.BO.EModulo"/>
        /// </summary>
        /// <returns>Diccionario con las posibles opciones de tipo</returns>
        private Dictionary<int, string> ObtenerTiposPlantilla()
        {
            try
            {
                string key = "";
                int value = 0;
                Dictionary<int, string> tipos = new Dictionary<int, string>();
                tipos.Add(-1, "SELECCIONE UNA OPCIÓN");
                foreach (var tipo in Enum.GetValues(typeof(EModulo)))
                {
                    var query =
                        tipo.GetType()
                            .GetField(tipo.ToString())
                            .GetCustomAttributes(true)
                            .Where(a => a.GetType().Equals(typeof(System.ComponentModel.DescriptionAttribute)));
                    value = Convert.ToInt32(tipo);
                    if (query.Any())
                    {
                        key =
                            (tipo.GetType()
                                 .GetField(tipo.ToString())
                                 .GetCustomAttributes(true)
                                 .Where(a => a.GetType().Equals(typeof(System.ComponentModel.DescriptionAttribute)))
                                 .FirstOrDefault() as System.ComponentModel.DescriptionAttribute).Description;
                    }
                    else
                    {
                        key = Enum.GetName(typeof(EModulo), value);
                    }
                    tipos.Add(value, key);
                }
                return tipos;
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0}.ObtenerTiposPlantilla:{1}Ocurrío una inconsistencia al intentar generar el listado de modulos de contrato{2}{3}", nombreClase, Environment.NewLine, Environment.NewLine, ex.Message));
            }
        }
        /// <summary>
        /// Carga en la vista los tipos de archivos configurados para las imagenes
        /// </summary>
        public void CargarTipoArchivosImagen()
        {
            var tipoBR = new TipoArchivoBR();
            var tipo = new TipoArchivoBO();
            this.vista.TiposArchivo = tipoBR.Consultar(this.dctx, tipo);
        }
        /// <summary>
        /// Limpia los campos de la vista
        /// </summary>
        private void LimpiarCampos()
        {
            this.vista.TipoArchivo = null;
            this.vista.TipoPlantilla = null;
        }
        /// <summary>
        /// Prepara la vista para el registro de un nuevo archivo
        /// </summary>
        public void PrepararNuevo()
        {
            this.vista.LimpiarSesion();
            this.LimpiarCampos();
            this.vista.PrepararNuevo();
            this.CargarTipoArchivosImagen();
            this.vista.EstablecerOpcionesTipoPlantilla(this.ObtenerTiposPlantilla());
            this.EstablecerSeguridad();
        }
        /// <summary>
        /// Cancela el registro del archivo
        /// </summary>
        public void CancelarRegistro()
        {
            this.vista.LimpiarSesion();
            this.vista.RedirigirAConsulta();
        }
        /// <summary>
        /// Obtiene la información del archivo seleccionado por el usuario
        /// </summary>
        /// <returns>Archivo que se va a registrar en la BD</returns>
        private object InterfazUsuarioADato()
        {
            PlantillaBO bo = new PlantillaBO();
            bo.TipoArchivo = new TipoArchivoBO();
            bo.Auditoria = new AuditoriaBO { FC = DateTime.Now, FUA = DateTime.Now, UC = this.vista.UsuarioID, UUA = this.vista.UsuarioID };

            if (!string.IsNullOrEmpty(this.vista.NombreArchivo) && !string.IsNullOrWhiteSpace(this.vista.NombreArchivo))
            {
                bo.Nombre = this.vista.NombreArchivo;
                bo.Archivo = this.vista.ObtenerArchivosBytes();
                bo.Activo = true;
            }
            if (!object.ReferenceEquals(this.vista.TipoArchivo, null))
                bo.TipoArchivo = (TipoArchivoBO)this.vista.TipoArchivo;
            if (this.vista.TipoPlantilla.HasValue)
                bo.TipoPlantilla = (EModulo)this.vista.TipoPlantilla.Value;

            return bo;
        }
        /// <summary>
        /// Valida la información proporcionada por el usuario
        /// </summary>
        /// <returns>Inconsistencias en la información</returns>
        private string ValidarCampos()
        {
            StringBuilder s = new StringBuilder();

            if (string.IsNullOrEmpty(this.vista.NombreArchivo) && string.IsNullOrWhiteSpace(this.vista.NombreArchivo))
                s.Append("Nombre, ");
            if (string.IsNullOrEmpty(this.vista.ExtencionArchivo) && string.IsNullOrWhiteSpace(this.vista.ExtencionArchivo))
                s.Append("Tipo Archivo, ");
            if (!this.vista.TipoPlantilla.HasValue)
                s.Append("modulo de contrato, ");

            if (s.Length > 0)
                return "Los siguientes campos no pueden estar vacíos: \n" + s.ToString().Substring(0, s.Length - 2);
            return null;
        }
        /// <summary>
        /// Registra el archivo en la BD
        /// </summary>
        private void Registrar()
        {
            try
            {
                //Se obtiene la información a partir de la interfaz de usuario
                PlantillaBO bo = (PlantillaBO)this.InterfazUsuarioADato();

                //Se crea el objeto seguridad
                UsuarioBO usuario = new UsuarioBO() { Id = this.vista.UsuarioID };
                AdscripcionBO adscripcion = new AdscripcionBO() { UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaID } };
                SeguridadBO seguridadBO = new SeguridadBO(Guid.Empty, usuario, adscripcion);

                //Se inserta en la BD
                this.controlador.Insertar(this.dctx, bo, seguridadBO);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0}.Registrar:{1}{2}", nombreClase, Environment.NewLine, ex.Message));
            }
        }
        /// <summary>
        /// Registra el archivo seleccionado por el usuario
        /// </summary>
        public void RegistrarArchivo()
        {
            try
            {
                string s;
                if ((s = this.ValidarCampos()) != null)
                {
                    this.vista.MostrarMensaje(s, ETipoMensajeIU.ADVERTENCIA, null);
                    return;
                }

                //Validamos la extencion del archivo 
                var tipoArchivo = this.ValidarArchivo(this.vista.ExtencionArchivo);

                if (object.ReferenceEquals(tipoArchivo, null))
                    throw new Exception("El tipo del archivo que se desea agregar no es un tipo permitido para el sistema.");

                this.vista.TipoArchivo = tipoArchivo;
                //Registramos el archivo
                this.Registrar();
                //Preparamos la interfaz para un nuevo registro
                this.PrepararNuevo();
                //Notificamos al usuario que el registro fue exitoso
                this.vista.MostrarMensaje("El documento fue cargado exitosamente.", ETipoMensajeIU.EXITO, null);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0}.RegistrarArchivo{1}{2}", nombreClase, Environment.NewLine, ex.Message));
            }
        }
        /// <summary>
        /// Valida que el tipo de archivo seleccionado este permitido dentro de los tipos configurados
        /// </summary>
        /// <param name="tipo">Tipo de archivo que desea validar</param>
        /// <returns>Verdadero si la extensión se encuentra, falso si no</returns>
        private TipoArchivoBO ValidarArchivo(String tipo)
        {
            List<TipoArchivoBO> tiposArchivo = (List<TipoArchivoBO>)this.vista.TiposArchivo;
            if (tiposArchivo != null)
            {
                TipoArchivoBO tipoArchivoTemp = tiposArchivo.Find(delegate(TipoArchivoBO ta) { return ta.Extension == tipo; });
                
                if (tipoArchivoTemp != null)                
                    return tipoArchivoTemp;

                this.vista.MostrarMensaje("El archivo no fue cargado.", ETipoMensajeIU.ERROR, "La extensión del archivo no se encuentra en la lista de tipos de archivo permitidos.");
            }
            else
            {
                this.vista.MostrarMensaje("El archivo no fue cargado.", ETipoMensajeIU.ERROR, "No hay una lista de tipos de archivo cargada.");
            }
            return null;
        }

        #region Seguridad
        /// <summary>
        /// Valida si el usuario tiene permiso para registrar
        /// </summary>
        private void EstablecerSeguridad()
        {
            try
            {
                //se valida que los datos del usuario y la unidad operativa no sean nulos
                if (this.vista.UsuarioID == null) throw new Exception("El identificador del usuario no debe ser nulo. ");
                if (this.vista.UnidadOperativaID == null) throw new Exception("El identificador de la Unidad Operativa no debe ser nulo. ");

                //Se crea el objeto de seguridad
                UsuarioBO usr = new UsuarioBO { Id = this.vista.UsuarioID };
                AdscripcionBO sdscripcion = new AdscripcionBO { UnidadOperativa = new UnidadOperativaBO { Id = this.vista.UnidadOperativaID } };
                SeguridadBO seguridadBO = new SeguridadBO(Guid.Empty, usr, sdscripcion);

                //Se obtienen las acciones a las cuales el usuario tiene permiso en este proceso
                List<CatalogoBaseBO> lst = FacadeBR.ConsultarAccion(this.dctx, seguridadBO);

                //Se valida si el usuario tiene permiso para registrar las plantillas
                if (!this.ExisteAccion(lst, "INSERTAR"))
                    this.vista.RedirigirSinPermisoAcceso();
                //Se valida si el usuario tiene permiso para consultar
                if (!this.ExisteAccion(lst, "CONSULTAR"))
                    this.vista.BloquearConsulta(false);
            }
            catch (Exception ex)
            {
                throw new Exception(nombreClase + ".EstablecerSeguridad:" + Environment.NewLine + ex.Message);
            }
        }
        /// <summary>
        /// Consulta en el listado de acciones configuradas una acción especifica
        /// </summary>
        /// <param name="acciones">Acciones configuradas</param>
        /// <param name="nombreAccion">Accion que se desea validar</param>
        /// <returns>Verdadero si existe,  falso si no existe</returns>
        private bool ExisteAccion(List<CatalogoBaseBO> acciones, string nombreAccion)
        {
            if (acciones != null && acciones.Count > 0 && acciones.Exists(p => p != null && p.Nombre != null && p.Nombre.Trim().ToUpper() == nombreAccion.Trim().ToUpper()))
                return true;

            return false;
        }
        #endregion
        #endregion       
    }
}