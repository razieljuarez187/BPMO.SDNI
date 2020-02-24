
using System;
using System.Text;
using BPMO.Facade.SDNI.BR;
using BPMO.Patterns.Creational.DataContext;
using BPMO.SDNI.Contratos.PSL.BO;
using BPMO.SDNI.Contratos.PSL.VIS;

namespace BPMO.SDNI.Contratos.PSL.PRE {
    public class ucPuntosVerificacionEntregaRecepcionPSLPRE {
        #region Atributos
        /// <summary>
        /// Nombre de la Clase, usado para Excepciones
        /// </summary>
        private string nombreClase = "ucPuntosVerificacionEntregaRecepcionPSLPRE";
        /// <summary>
        /// Vista de Consulta de Tarifas
        /// </summary>
        private IucPuntosVerificacionEntregaRecepcionPSLVIS vista;
        internal IucPuntosVerificacionEntregaRecepcionPSLVIS Vista { get { return vista; } }

        /// <summary>
        /// El DataContext que provee acceso a la Base de Datos
        /// </summary>
        private IDataContext dctx;
        /// <summary>
        /// Controlador de Punto de verificación de excavadora
        /// </summary>
        //private TarifaPSLBR tarifaBr;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor del Presentador
        /// </summary>
        /// <param name="vistaActual">Vista a la cual estará asociado el Presentador</param>
        public ucPuntosVerificacionEntregaRecepcionPSLPRE(IucPuntosVerificacionEntregaRecepcionPSLVIS vistaActual) {
            try {
                this.vista = vistaActual;
                dctx = FacadeBR.ObtenerConexion();
            } catch (Exception ex) {
                throw new Exception(nombreClase + ".ucPuntosVerificacionEntregaRecepcionPSLPRE:Error al configurar el presentador");
            }
        }
        #endregion

        #region Métodos
        public void Registrar() {
            ListadoVerificacionEntregaRecepcionBO bo = (ListadoVerificacionEntregaRecepcionBO)this.InterfazUsuarioADato();

        }
        /// <summary>
        /// Limpia la Interfaz de Consulta al Iniciar la misma
        /// </summary>
        public void PrepararNuevo() {
            #region Existencia (Campos booleanos)
            this.vista.TieneBandas = null;
            this.vista.TieneFiltroAceite = null;
            this.vista.TieneFiltroAgua = null;
            this.vista.TieneFiltroCombustible = null;
            this.vista.TieneFiltroAire = null;
            this.vista.TieneMangueras = null;
            #endregion

            #region Medidores (Campos booleanos)
            this.vista.TieneAmperimetro = null;
            this.vista.TieneVoltimetro = null;
            this.vista.TieneHorometro = null;
            this.vista.TieneManometro = null;
            this.vista.TieneManometro = null;
            #endregion

            #region Motor (Campos booleanos)

            this.vista.TieneNivelAceite = null;
            this.vista.TieneNivelAnticongelante = null;
            #endregion

            #region Voltaje (Campos numericos)

            this.vista.VoltajeL1N = null;
            this.vista.VoltajeL2N = null;
            this.vista.VoltajeL3N = null;
            this.vista.VoltajeL1L2 = null;
            this.vista.VoltajeL2L3 = null;
            this.vista.VoltajeL3L1 = null;

            #endregion

            #region Accesorios (Campos booleanos)
            this.vista.TieneCables = null;
            this.vista.TieneTramos = null;
            this.vista.TieneLineas = null;
            this.vista.TieneCalibres = null;
            this.vista.TieneCalibres = null;
            #endregion

            #region Bateria (Campos numericos)
            this.vista.BateriaCantidad = null;
            this.vista.BateriaMarca = string.Empty;
            this.vista.BateriaPlacas = null;
            #endregion

            #region Datos Remolque (Campos alfanumericos)
            this.vista.Suspension = null;
            this.vista.Gancho = null;
            this.vista.GatoNivelacion = null;
            this.vista.ArnesConexion = null;
            #endregion

            #region Llantas (Campos booleanos)
            this.vista.TieneEje1LlantaD = null;
            this.vista.TieneEje2LlantaD = null;
            this.vista.TieneEje3LlantaD = null;
            this.vista.TieneEje1LlantaI = null;
            this.vista.TieneEje2LlantaI = null;
            this.vista.TieneEje3LlantaI = null;
            this.vista.TieneTapaLluviaLlantaD = null;
            this.vista.TieneTapaLluviaLlantaD = null;
            #endregion

            #region Lamparas (Campos booleanos)
            this.vista.TieneLamparaDerecha = null;
            this.vista.TieneLamparaIzquierda = null;
            this.vista.TieneSenalSatelital = null;
            this.vista.TieneDiodos = null;
            #endregion

        }
        #endregion

        public string ValidarCampos() {
            StringBuilder s = new StringBuilder();

            #region Voltaje
            if (!this.vista.VoltajeL1N.HasValue)
                s.Append("Voltaje L1-N, ");
            if (!this.vista.VoltajeL2N.HasValue)
                s.Append("Voltaje L2-N, ");
            if (!this.vista.VoltajeL3N.HasValue)
                s.Append("Voltaje L3-N, ");
            if (!this.vista.VoltajeL1L2.HasValue)
                s.Append("Voltaje L1-L2, ");
            if (!this.vista.VoltajeL2L3.HasValue)
                s.Append("Voltaje L2-L3, ");
            if (!this.vista.VoltajeL3L1.HasValue)
                s.Append("Voltaje L3-L1, ");
            #endregion

            #region Batería
            if (!this.vista.BateriaCantidad.HasValue)
                s.Append("Batería Cantidad, ");
            if (!this.vista.BateriaPlacas.HasValue)
                s.Append("Batería Placas, ");
            if (string.IsNullOrWhiteSpace(this.vista.BateriaMarca))
                s.Append("Batería Marca, ");
            #endregion

            #region Datos Remolque
            if (String.IsNullOrEmpty(this.vista.Suspension))
                s.Append("Suspensión, ");
            if (String.IsNullOrEmpty(this.vista.Gancho))
                s.Append("Gancho, ");
            if (String.IsNullOrEmpty(this.vista.GatoNivelacion))
                s.Append("Gato de Nivelación, ");
            if (String.IsNullOrEmpty(this.vista.ArnesConexion))
                s.Append("Arnés de conexión, ");
            #endregion

            if (s.Length > 0)
                return "Los siguientes campos no pueden estar vacíos: \n" + s.ToString().Substring(0, s.Length - 2);

            #region Existencias
            if (!this.vista.TieneBandas.Value)
                s.Append("Bandas, ");

            if (!this.vista.TieneFiltroAceite.Value)
                s.Append("Filtro Aceite, ");

            if (!this.vista.TieneFiltroAgua.Value)
                s.Append("Filtro Agua, ");

            if (!this.vista.TieneFiltroCombustible.Value)
                s.Append("Filtro Combustible, ");

            if (!this.vista.TieneFiltroAire.Value)
                s.Append("Filtro de Aire, ");

            if (!this.vista.TieneMangueras.Value)
                s.Append("Mangueras, ");
            #endregion

            #region Medidores
            if (!this.vista.TieneAmperimetro.Value)
                s.Append("Amperímetro, ");

            if (!this.vista.TieneVoltimetro.Value)
                s.Append("Voltímetro, ");

            if (!this.vista.TieneHorometro.Value)
                s.Append("Horómetro, ");

            if (!this.vista.TieneManometro.Value)
                s.Append("Manómetro, ");

            if (!this.vista.TieneInterruptor.Value)
                s.Append("Interruptor, ");

            #endregion

            #region Motor
            if (!this.vista.TieneNivelAceite.Value)
                s.Append("Aceite, ");

            if (!this.vista.TieneNivelAnticongelante.Value)
                s.Append("Anticongelante, ");
            #endregion

            #region Accesorios
            if (!this.vista.TieneCables.Value)
                s.Append("Cables (MTS), ");

            if (!this.vista.TieneTramos.Value)
                s.Append("Tramos, ");

            if (!this.vista.TieneLineas.Value)
                s.Append("Líneas, ");

            if (!this.vista.TieneCalibres.Value)
                s.Append("Calibres, ");

            if (!this.vista.TieneZapatas.Value)
                s.Append("Zapatas, ");
            #endregion

            #region Llantas
            if (!this.vista.TieneEje1LlantaD.Value)
                s.Append("Eje 1 Llanta Derecha, ");

            if (!this.vista.TieneEje1LlantaI.Value)
                s.Append("Eje 1 Llanta Izquierda, ");

            if (!this.vista.TieneEje2LlantaD.Value)
                s.Append("Eje 2 Llanta Derecha, ");

            if (!this.vista.TieneEje2LlantaI.Value)
                s.Append("Eje 2 Llanta Izquierda, ");

            if (!this.vista.TieneEje3LlantaD.Value)
                s.Append("Eje 3 Llanta Derecha, ");

            if (!this.vista.TieneEje3LlantaI.Value)
                s.Append("Eje 3 Llanta Izquierda, ");

            if (!this.vista.TieneTapaLluviaLlantaD.Value)
                s.Append("Tapa Lluvia Llanta Derecha, ");

            if (!this.vista.TieneTapaLluviaLlantaI.Value)
                s.Append("Tapa Lluvia Llanta Izquierda, ");
            #endregion

            #region Lámparas
            if (!this.vista.TieneLamparaIzquierda.Value)
                s.Append("Lámpara Izquierda, ");

            if (!this.vista.TieneLamparaDerecha.Value)
                s.Append("Lámpara Derecha, ");

            if (!this.vista.TieneSenalSatelital.Value)
                s.Append("Lámpara Señal Satelital, ");

            if (!this.vista.TieneDiodos.Value)
                s.Append("Lámpara Diodos, ");
            #endregion

            if (s.Length > 0)
                return "Es necesario capturar observaciones para los siguientes puntos de verificación: \n" + s.ToString().Substring(0, s.Length - 2);

            return null;
        }

        public object InterfazUsuarioADato() {
            ListadoVerificacionEntregaRecepcionBO bo = new ListadoVerificacionEntregaRecepcionBO();

            #region Existencia (Campos booleanos)
            if (this.vista.TieneBandas != null)
                bo.TieneBandas = this.vista.TieneBandas;
            if (this.vista.TieneFiltroAceite != null)
                bo.TieneFiltroAceite = this.vista.TieneFiltroAceite;
            if (this.vista.TieneFiltroAgua != null)
                bo.TieneFiltroAgua = this.vista.TieneFiltroAgua;
            if (this.vista.TieneFiltroCombustible != null)
                bo.TieneFiltroCombustible = this.vista.TieneFiltroCombustible;
            if (this.vista.TieneFiltroAire != null)
                bo.TieneFiltroAire = this.vista.TieneFiltroAire;
            if (this.vista.TieneMangueras != null)
                bo.TieneMangueras = this.vista.TieneMangueras;
            #endregion

            #region Medidores (Campos booleanos)

            if (this.vista.TieneAmperimetro != null)
                bo.TieneAmperimetro = this.vista.TieneAmperimetro;
            if (this.vista.TieneVoltimetro != null)
                bo.TieneVoltimetro = this.vista.TieneVoltimetro;
            if (this.vista.TieneHorometro != null)
                bo.TieneHorometro = this.vista.TieneHorometro;
            if (this.vista.TieneManometro != null)
                bo.TieneManometro = this.vista.TieneManometro;
            if (this.vista.TieneInterruptor != null)
                bo.TieneInterruptor = this.vista.TieneInterruptor;

            #endregion

            #region Motor (Campos booleanos)

            if (this.vista.TieneNivelAceite != null)
                bo.TieneNivelAceite = this.vista.TieneNivelAceite;
            if (this.vista.TieneNivelAnticongelante != null)
                bo.TieneNivelAnticongelante = this.vista.TieneNivelAnticongelante;
            #endregion

            #region Voltaje (Campos numericos)

            if (this.vista.VoltajeL1N != null)
                bo.VoltajeL1N = this.vista.VoltajeL1N;
            if (this.vista.VoltajeL2N != null)
                bo.VoltajeL2N = this.vista.VoltajeL2N;
            if (this.vista.VoltajeL3N != null)
                bo.VoltajeL3N = this.vista.VoltajeL3N;
            if (this.vista.VoltajeL1L2 != null)
                bo.VoltajeL1L2 = this.vista.VoltajeL1L2;
            if (this.vista.VoltajeL2L3 != null)
                bo.VoltajeL2L3 = this.vista.VoltajeL2L3;
            if (this.vista.VoltajeL3L1 != null)
                bo.VoltajeL3L1 = this.vista.VoltajeL3L1;

            #endregion

            #region Accesorios (Campos booleanos)

            if (this.vista.TieneCables != null)
                bo.TieneCables = this.vista.TieneCables;
            if (this.vista.TieneTramos != null)
                bo.TieneTramos = this.vista.TieneTramos;
            if (this.vista.TieneLineas != null)
                bo.TieneLineas = this.vista.TieneLineas;
            if (this.vista.TieneCalibres != null)
                bo.TieneCalibres = this.vista.TieneCalibres;
            if (this.vista.TieneZapatas != null)
                bo.TieneZapatas = this.vista.TieneZapatas;
            #endregion

            #region Bateria (Campos numericos)

            if (this.vista.BateriaCantidad != null)
                bo.BateriaCantidad = this.vista.BateriaCantidad;
            if (!string.IsNullOrEmpty(this.vista.BateriaMarca))
                bo.BateriaMarca = this.vista.BateriaMarca;
            if (this.vista.BateriaPlacas != null)
                bo.BateriaPlacas = this.vista.BateriaPlacas;

            #endregion

            #region Datos Remolque (Campos alfanumericos)

            if (this.vista.Suspension != null)
                bo.Suspension = this.vista.Suspension;
            if (this.vista.Gancho != null)
                bo.Gancho = this.vista.Gancho;
            if (this.vista.GatoNivelacion != null)
                bo.GatoNivelacion = this.vista.GatoNivelacion;
            if (this.vista.ArnesConexion != null)
                bo.ArnesConexion = this.vista.ArnesConexion;

            #endregion

            #region Llantas (Campos booleanos)

            if (this.vista.TieneEje1LlantaD != null)
                bo.TieneEje1LlantaD = this.vista.TieneEje1LlantaD;
            if (this.vista.TieneEje2LlantaD != null)
                bo.TieneEje2LlantaD = this.vista.TieneEje2LlantaD;
            if (this.vista.TieneEje3LlantaD != null)
                bo.TieneEje3LlantaD = this.vista.TieneEje3LlantaD;
            if (this.vista.TieneEje1LlantaI != null)
                bo.TieneEje1LlantaI = this.vista.TieneEje1LlantaI;
            if (this.vista.TieneEje2LlantaI != null)
                bo.TieneEje2LlantaI = this.vista.TieneEje2LlantaI;
            if (this.vista.TieneEje3LlantaI != null)
                bo.TieneEje3LlantaI = this.vista.TieneEje3LlantaI;
            if (this.vista.TieneTapaLluviaLlantaD != null)
                bo.TieneTapaLluviaLlantaD = this.vista.TieneTapaLluviaLlantaD;
            if (this.vista.TieneTapaLluviaLlantaI != null)
                bo.TieneTapaLluviaLlantaI = this.vista.TieneTapaLluviaLlantaI;
            #endregion

            #region Lamparas (Campos booleanos)

            if (this.vista.TieneLamparaDerecha != null)
                bo.TieneLamparaDerecha = this.vista.TieneLamparaDerecha;
            if (this.vista.TieneLamparaIzquierda != null)
                bo.TieneLamparaIzquierda = this.vista.TieneLamparaIzquierda;
            if (this.vista.TieneSenalSatelital != null)
                bo.TieneSenalSatelital = this.vista.TieneSenalSatelital;
            if (this.vista.TieneDiodos != null)
                bo.TieneDiodos = this.vista.TieneDiodos;
            #endregion

            return bo;
        }
        public void DatoToInterfazUsuario(ListadoVerificacionEntregaRecepcionBO bo) {

            #region Existencia (Campos booleanos)
            this.vista.TieneBandas = bo.TieneBandas;
            this.vista.TieneFiltroAceite = bo.TieneFiltroAceite;
            this.vista.TieneFiltroAgua = bo.TieneFiltroAgua;
            this.vista.TieneFiltroCombustible = bo.TieneFiltroCombustible;
            this.vista.TieneFiltroAire = bo.TieneFiltroAire;
            this.vista.TieneMangueras = bo.TieneMangueras;
            #endregion

            #region Medidores (Campos booleanos)
            this.vista.TieneAmperimetro = bo.TieneAmperimetro;
            this.vista.TieneVoltimetro = bo.TieneVoltimetro;
            this.vista.TieneHorometro = bo.TieneHorometro;
            this.vista.TieneManometro = bo.TieneManometro;
            this.vista.TieneInterruptor = bo.TieneInterruptor;
            #endregion

            #region Motor (Campos booleanos)
            this.vista.TieneNivelAceite = bo.TieneNivelAceite;
            this.vista.TieneNivelAnticongelante = bo.TieneNivelAnticongelante;
            #endregion

            #region Voltaje (Campos numericos)
            this.vista.VoltajeL1N = bo.VoltajeL1N;
            this.vista.VoltajeL2N = bo.VoltajeL2N;
            this.vista.VoltajeL3N = bo.VoltajeL3N;
            this.vista.VoltajeL1L2 = bo.VoltajeL1L2;
            this.vista.VoltajeL2L3 = bo.VoltajeL2L3;
            this.vista.VoltajeL3L1 = bo.VoltajeL3L1;
            #endregion

            #region Accesorios (Campos booleanos)
            this.vista.TieneCables = bo.TieneCables;
            this.vista.TieneTramos = bo.TieneTramos;
            this.vista.TieneLineas = bo.TieneLineas;
            this.vista.TieneCalibres = bo.TieneCalibres;
            this.vista.TieneZapatas = bo.TieneZapatas;
            #endregion

            #region Bateria (Campos numericos)
            this.vista.BateriaCantidad = bo.BateriaCantidad;
            this.vista.BateriaMarca = bo.BateriaMarca;
            this.vista.BateriaPlacas = bo.BateriaPlacas;
            #endregion

            #region Datos Remolque (Campos alfanumericos)
            this.vista.Suspension = bo.Suspension;
            this.vista.Gancho = bo.Gancho;
            this.vista.GatoNivelacion = bo.GatoNivelacion;
            this.vista.ArnesConexion = bo.ArnesConexion;
            #endregion

            #region Llantas (Campos booleanos)
            this.vista.TieneEje1LlantaD = bo.TieneEje1LlantaD;
            this.vista.TieneEje2LlantaD = bo.TieneEje2LlantaD;
            this.vista.TieneEje3LlantaD = bo.TieneEje3LlantaD;
            this.vista.TieneEje1LlantaI = bo.TieneEje1LlantaI;
            this.vista.TieneEje2LlantaI = bo.TieneEje2LlantaI;
            this.vista.TieneEje3LlantaI = bo.TieneEje3LlantaI;
            this.vista.TieneTapaLluviaLlantaD = bo.TieneTapaLluviaLlantaD;
            this.vista.TieneTapaLluviaLlantaI = bo.TieneTapaLluviaLlantaI;
            #endregion

            #region Lamparas (Campos booleanos)
            this.vista.TieneLamparaDerecha = bo.TieneLamparaDerecha;
            this.vista.TieneLamparaIzquierda = bo.TieneLamparaIzquierda;
            this.vista.TieneSenalSatelital = bo.TieneSenalSatelital;
            this.vista.TieneDiodos = bo.TieneDiodos;
            #endregion
        }
    }
}