
namespace BPMO.SDNI.Contratos.PSL.VIS {
    public interface IucPuntosVerificacionEntregaRecepcionPSLVIS {
        #region Existencia (Campos booleanos)

        bool? TieneBandas { get; set; }
        bool? TieneFiltroAceite { get; set; }
        bool? TieneFiltroAgua { get; set; }
        bool? TieneFiltroCombustible { get; set; }
        bool? TieneFiltroAire { get; set; }
        bool? TieneMangueras { get; set; }

        #endregion

        #region Medidores (Campos booleanos)

        bool? TieneAmperimetro { get; set; }
        bool? TieneVoltimetro { get; set; }
        bool? TieneHorometro { get; set; }
        bool? TieneManometro { get; set; }
        bool? TieneInterruptor { get; set; }

        #endregion

        #region Motor (Campos booleanos)

        bool? TieneNivelAceite { get; set; }
        bool? TieneNivelAnticongelante { get; set; }

        #endregion

        #region Voltaje (Campos numericos)

        decimal? VoltajeL1N { get; set; }
        decimal? VoltajeL2N { get; set; }
        decimal? VoltajeL3N { get; set; }
        decimal? VoltajeL1L2 { get; set; }
        decimal? VoltajeL2L3 { get; set; }
        decimal? VoltajeL3L1 { get; set; }

        #endregion

        #region Accesorios (Campos booleanos)

        bool? TieneCables { get; set; }
        bool? TieneTramos { get; set; }
        bool? TieneLineas { get; set; }
        bool? TieneCalibres { get; set; }
        bool? TieneZapatas { get; set; }
        #endregion

        #region Bateria (Campos numericos)

        decimal? BateriaCantidad { get; set; }
        string BateriaMarca { get; set; }
        decimal? BateriaPlacas { get; set; }

        #endregion

        #region Datos Remolque (Campos alfanumericos)

        string Suspension { get; set; }
        string Gancho { get; set; }
        string GatoNivelacion { get; set; }
        string ArnesConexion { get; set; }


        #endregion

        #region Llantas (Campos booleanos)

        bool? TieneEje1LlantaD { get; set; }
        bool? TieneEje2LlantaD { get; set; }
        bool? TieneEje3LlantaD { get; set; }
        bool? TieneEje1LlantaI { get; set; }
        bool? TieneEje2LlantaI { get; set; }
        bool? TieneEje3LlantaI { get; set; }
        bool? TieneTapaLluviaLlantaD { get; set; }
        bool? TieneTapaLluviaLlantaI { get; set; }

        #endregion

        #region Lamparas (Campos booleanos)

        bool? TieneLamparaDerecha { get; set; }
        bool? TieneLamparaIzquierda { get; set; }
        bool? TieneSenalSatelital { get; set; }
        bool? TieneDiodos { get; set; }

        #endregion
    }
}