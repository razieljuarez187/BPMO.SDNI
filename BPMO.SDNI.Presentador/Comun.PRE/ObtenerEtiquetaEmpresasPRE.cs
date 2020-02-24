using BPMO.SDNI.Comun.BO;

namespace BPMO.SDNI.Comun.PRE {

    public class ObtenerEtiquetaEmpresasPRE {
        /// <summary>
        /// Obtiene el nombre de la empresa del enumerador, para después ser ocupada al buscar el archivo de recursos
        /// </summary>
        /// <param name="_tipoEmpresa">Identificador del tipo de empresa a buscar</param>
        /// <returns>Un string con el nombre del tipo de empresa a buscar</returns>
        public string RecuperarEmpresa(int _tipoEmpresa) {
            string cRespuesta = string.Empty;
            ETipoEmpresa tipoEmp;

            tipoEmp = (ETipoEmpresa)_tipoEmpresa;
            cRespuesta = tipoEmp.ToString();

            return cRespuesta;
        }
    }

    public class EtiquetaObtenida {
        /// <summary>
        /// Obtiene el nombre de la etiqueta, para deserializar el JSON
        /// </summary>
        public string cEtiqueta { get; set; }

        /// <summary>
        /// Obtiene el mensaje de error en caso de lo haya, en caso contrario llega vacío, para deserializar el JSON
        /// </summary>
        public string cMensaje { get; set; }
    }
}