using System;
using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Web;
using BPMO.SDNI.Comun.PRE;
using DevExpress.XtraPrinting.Native;
using Newtonsoft.Json.Linq;


namespace BPMO.SDNI.Comun.UI
{
    public partial class ObtenerEtiquetaEmpresas : System.Web.UI.Page
    {

        /// <summary>
        /// Presentador de la obtención de etiquetas
        /// </summary>
        public ObtenerEtiquetaEmpresasPRE presentador;
       

        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        /// <summary>
        /// Obtiene la etiqueta de un archivo de recursos (*.resx)
        /// </summary>
        /// <param name="_cEtiquetaResx">Nombre del identificador de la etiqueta</param>
        /// <param name="_tipoEmpresa">Nombre del tipo de empresa del cual obtendrá el archivo de recursos</param>
        /// <returns>Json con la etiqueta y el error del identificador del recurso solicitado si hubiese.</returns>
        [System.Web.Services.WebMethod(EnableSession = true)]
        public string ObtenerEtiquetadelResource(string _cEtiquetaResx, int _tipoEmpresa)
        {
            this.presentador = new ObtenerEtiquetaEmpresasPRE();
            string cEtiqueta = string.Empty, cMensajeError = string.Empty;
            string cArchivo  = string.Empty;
            try
            {
                if (_tipoEmpresa != 0)
                {
                    //obtenemos del enumerador la empresa correspondiente.
                    cArchivo = presentador.RecuperarEmpresa(_tipoEmpresa);
                    cEtiqueta = ObtenerEtiquetadelResource(_cEtiquetaResx, ref cMensajeError, cArchivo);
                }
                else
                {
                    throw new Exception("No se ha establecido el valor del tipo de empresa");
                }
            }
            catch (Exception cError)
            {
                cMensajeError = cError.Message;
            }

            JObject JSON = new JObject();
            JSON.Add("cEtiqueta", cEtiqueta);
            JSON.Add("cMensaje", cMensajeError);

            return JSON.ToString(Newtonsoft.Json.Formatting.None);
        }


        /// <summary>
        /// Obtiene la etiqueta de un archivo de recursos (*.resx)
        /// </summary>
        /// <param name="_cEtiquetaResx">Identificador de la etiqueta</param>
        /// <param name="_cMensajeError">Mensaje de error</param>
        /// <param name="_cArchivo">Nombre del archivo .resx que se cargará</param>
        /// <returns></returns>
        public static string ObtenerEtiquetadelResource(string _cEtiquetaResx, ref string _cMensajeError, string _cArchivo)
        {
            string cValor = string.Empty;
            try
            {
                ResourceManager manejadorRecurso = null;
                if (HttpContext.Current.Session["Recurso"] == null)
                {
                    manejadorRecurso = ObtenerArchivoResource(ref _cMensajeError, _cArchivo);
                    HttpContext.Current.Session["Recurso"] = manejadorRecurso;
                }
                else
                {
                    manejadorRecurso = (ResourceManager)HttpContext.Current.Session["Recurso"];
                }
                cValor = manejadorRecurso.GetString(_cEtiquetaResx, CultureInfo.CurrentUICulture);
            }
            catch (Exception cExcepcion)
            {
                _cMensajeError = cExcepcion.Message;
            }
            return cValor;
        }

        /// <summary>
        /// Obtiene un archivo de recursos (*.resx)
        /// </summary>
        /// <param name="_cMensajeError">Mensaje de error</param>
        /// <param name="_cArchivo">Nombre del archivo .resx que se cargará</param>
        /// <returns>Regresa un ResourceManager del archivo de recursos (*.resx)</returns>
        private static ResourceManager ObtenerArchivoResource(ref string _cMensajeError, string _cArchivo)
        {
            //archivo de recurso por defecto
            string cAsembly = "BPMO.SDNI.RECURSOS.Idealease", cURL = System.AppDomain.CurrentDomain.BaseDirectory + @"bin\BPMO.SDNI.RECURSOS.dll";
            ResourceManager manejadorRecurso= null;
            try
            {
                if (!_cArchivo.IsEmpty() && _cArchivo.Length > 0)
                    cAsembly = "BPMO.SDNI.RECURSOS."+_cArchivo;

                Assembly GetExecutingAssembly = Assembly.LoadFile(cURL);
                manejadorRecurso = new ResourceManager(cAsembly, GetExecutingAssembly);

                return manejadorRecurso;
            }
            catch (Exception cExcepcion)
            {
                _cMensajeError = cExcepcion.Message;
                return null;
            }
        }

    }
}