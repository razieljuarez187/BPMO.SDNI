//Satisface al CU062 - Menú Principal
using System;
using System.Web.UI;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Comun.PRE;
using BPMO.SDNI.Comun.UI;
using BPMO.SDNI.MapaSitio.PRE;
using BPMO.SDNI.MapaSitio.VIS;
using Newtonsoft.Json;

namespace BPMO.SDNI.MapaSitio.UI
{
    public partial class MenuPrincipalUI : System.Web.UI.Page, IMenuPrincipalVIS
    {
        MenuPrincipalPRE presentador;
        private ETipoEmpresa ETipoEmpresa;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            #region [RQM 13285- Modificación -Integración Construcción y Generacción]

            this.ETipoEmpresa = (ETipoEmpresa)Enum.ToObject(typeof(ETipoEmpresa), ((Site)Page.Master).Adscripcion.UnidadOperativa.Id);
            string ImgRutaPortada = "";

            if (this.ETipoEmpresa == ETipoEmpresa.Idealease || this.ETipoEmpresa == ETipoEmpresa.Construccion ||
                this.ETipoEmpresa == ETipoEmpresa.Generacion || this.ETipoEmpresa == ETipoEmpresa.Equinova)            
                ImgRutaPortada = ObtenerConfiguracionResource("RE45", this.ETipoEmpresa, true);  
            else
                ImgRutaPortada = "../Contenido/Imagenes/WorkInProgressDefault.png";
                                  
            this.imgPortadaEmpresa.ImageUrl = ImgRutaPortada;

            #endregion
        }

        #region [RQM 13285- Modificación -Integración Construcción y Generacción]
        /// <summary>
        /// Método que obtiene la configuración de una etiqueta desde el archivo resource correspondiente a la unidad operativa a la cual accedió el usuario.
        /// </summary>
        /// <param name="etiquetaBuscar">Nombre de la etiqueta que será buscada en el archivo resource</param>
        /// <param name="tipoEmpresa">Nombre de la unidad operativa a la cual accedió el usuario.</param>
        /// <param name="esEtiqueta">Indica sí el valor a obtener es una etiqueta, en caso contrario se considera un TAB o CHECKBOX.</param>
        /// <returns>Retorna la configuración correspondiente al valor recibido en el parámetro etiquetaBuscar del archivo resource.</returns>
        private string ObtenerConfiguracionResource(string etiquetaBuscar, ETipoEmpresa tipoEmpresa, bool esEtiqueta)
        {
            string Configuracion = string.Empty;
            //Instanciamos la clase o webmethod que obtiene las etiquetas
            ObtenerEtiquetaEmpresas obtenerEtiqueta = new ObtenerEtiquetaEmpresas();
            string ConfiguracionObtenida = string.Empty;
            EtiquetaObtenida request = null;

            ConfiguracionObtenida = obtenerEtiqueta.ObtenerEtiquetadelResource(etiquetaBuscar, (int)tipoEmpresa);
            request = JsonConvert.DeserializeObject<EtiquetaObtenida>(ConfiguracionObtenida);
            if (string.IsNullOrEmpty(request.cMensaje))
            {
                ConfiguracionObtenida = request.cEtiqueta;
                if (esEtiqueta)
                {
                    if (ConfiguracionObtenida != "NO APLICA")
                    {
                        Configuracion = ConfiguracionObtenida;
                    }
                }
                else
                {
                    Configuracion = ConfiguracionObtenida;
                }
            }
            return Configuracion;
        }
        #endregion
    }
}