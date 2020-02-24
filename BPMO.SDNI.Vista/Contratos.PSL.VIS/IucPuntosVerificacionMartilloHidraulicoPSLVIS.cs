using BPMO.Primitivos.Enumeradores;

namespace BPMO.SDNI.Contratos.PSL.VIS {
    public interface IucPuntosVerificacionMartilloHidraulicoPSLVIS {
        #region propiedades

        #region GENERAL

        /// <summary>
        /// Verifica si tienen condición las mangueras
        /// </summary>
        bool? TieneCondicionMangueras { get; set; }
        /// <summary>
        /// Verifica si tiene tapones las mangueras
        /// </summary>
        bool? TieneTaponesMangueras { get; set; }
        /// <summary>
        /// Verifica si tiene condición los bujes
        /// </summary>
        bool? TieneCondicionBujes { get; set; }
        /// <summary>
        /// Verifica si tienen condición los pasadores
        /// </summary>
        bool? TieneCondicionPasadores { get; set; }
        /// <summary>
        /// Verifica si tiene condición el pica
        /// </summary>
        bool? TieneCondicionPica { get; set; }
        /// <summary>
        /// Verifica si tiene el torque tornillos
        /// </summary>
        bool? TieneTorqueTornillos { get; set; }
        /// <summary>
        /// Verifica si tiene grasera manual
        /// </summary>
        bool? TieneGraseraManual { get; set; }
        #endregion

        #region LUBRICACIÓN
        /// <summary>
        /// Verifica si tiene bujes
        /// </summary>
        bool? TieneBujes { get; set; }
        /// <summary>
        /// Verifica si tiene pasadores
        /// </summary>
        bool? TienePasadores { get; set; }
        /// <summary>
        /// Verifica si tiene pica
        /// </summary>
        bool? TienePica { get; set; }

        #endregion

        #region MISCELANEOS
        /// <summary>
        /// Verifica si tiene condición la pintura
        /// </summary>
        bool? TieneCondicionPintura { get; set; }
        /// <summary>
        /// Verifica si tienen condición las calcas
        /// </summary>
        bool? TieneCondicionCalcas { get; set; }
        /// <summary>
        /// Verifica si tiene símbolos de seguridad
        /// </summary>
        bool? TieneSimbolosSeguridad { get; set; }
        /// <summary>
        /// Verifica si tiene estructura
        /// </summary>
        bool? TieneEstructura { get; set; }

        #endregion














        #endregion

        #region Métodos

        void ModoEdicion(bool activo);
        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null);

        #endregion

    }
}