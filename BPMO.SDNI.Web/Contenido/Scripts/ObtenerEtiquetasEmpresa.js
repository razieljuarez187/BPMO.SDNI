
function ObtenerEtiqueta(cEtiquetaResx, cTipoEmpresa) {
    /// <summary>
    /// Obtiene la etiqueta del proyecto de recursos
    /// </summary>
    /// <param name="cEtiquetaResx">Nombre del identificador de la etiqueta</param>
    /// <param name="_tipoEmpresa">Nombre del tipo de empresa del cual obtendrá el archivo de recursos</param>
    /// <returns type="">Texto de la etiqueta</returns>

    var cEtiqueta = "";
    var Parametros =
    {
        url: "../Comun.UI/ObtenerEtiquetadelResource",
        data: JsonToText({ cEtiquetaResx: cEtiquetaResx, cTipoEmpresa: cTipoEmpresa }),
        type: "post",
        datatype: "json",
        contentType: "application/json",
        async: false,
        success:
            function (data, b, c) {
                var jSonRegreso = eval("(" + data.d + ")");
                cEtiqueta = jSonRegreso.cMensaje != "" ? jSonRegreso.cMensaje : jSonRegreso.cEtiqueta;
            },
        error: function (data, b, c) {
            showhideLoading(false);
        }
    };
    $.ajax(Parametros);

    return cEtiqueta;
}