//Satisface al CU061 - Acceso al Sistema
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BPMO.Security.BO;
using BPMO.Basicos.BO;
using BPMO.Primitivos.Enumeradores;

namespace BPMO.SDNI.Seguridad.Acceso.VIS
{
    public interface IConfiguracionAccesoVIS
    {
        #region Propiedades
        int? UnidadOperativa { get; }
        UsuarioBO Usuario {get;}
        AdscripcionBO Adscripcion { get; set; }
        List<AdscripcionBO> Adscripciones {get; set;}
        List<ProcesoBO> ListadoProcesos { get; set; }

        //RQM 14078, se agrega el set para que permita en la configuración del modulo asignarle valor
        int? ModuloID { get; set; }
        #endregion

        #region Métodos
        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null);
        void EnviarAInicio();
        void CargarDatosAdscripcion();

        void EstablecerConfiguracionModulo(object configuracion);
        #endregion
    }
}
