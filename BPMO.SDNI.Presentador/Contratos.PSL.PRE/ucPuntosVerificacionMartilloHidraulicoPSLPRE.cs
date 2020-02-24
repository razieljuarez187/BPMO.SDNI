
using System;
using System.Text;
using BPMO.Facade.SDNI.BR;
using BPMO.Patterns.Creational.DataContext;
using BPMO.SDNI.Contratos.PSL.BO;
using BPMO.SDNI.Contratos.PSL.VIS;

namespace BPMO.SDNI.Contratos.PSL.PRE {
    public class ucPuntosVerificacionMartilloHidraulicoPSLPRE {
        #region Atributos
        /// <summary>
        /// Nombre de la Clase, usado para Excepciones
        /// </summary>
        private string nombreClase = "ucPuntosVerificacionMartilloHidraulicoPSLPRE";
        /// <summary>
        /// Vista de Consulta de Tarifas
        /// </summary>
        private IucPuntosVerificacionMartilloHidraulicoPSLVIS vista;
        internal IucPuntosVerificacionMartilloHidraulicoPSLVIS Vista { get { return vista; } }

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
        public ucPuntosVerificacionMartilloHidraulicoPSLPRE(IucPuntosVerificacionMartilloHidraulicoPSLVIS vistaActual) {
            try {
                this.vista = vistaActual;
                dctx = FacadeBR.ObtenerConexion();
            } catch (Exception ex) {
                throw new Exception(nombreClase + ".ucPuntosVerificacionMartilloHidraulicoPSLPRE:Error al configurar el presentador");
            }
        }
        #endregion

        #region Métodos
        public void Registrar() {
            ListadoVerificacionMartilloHidraulicoBO bo = (ListadoVerificacionMartilloHidraulicoBO)this.InterfazUsuarioADato();

        }
        /// <summary>
        /// Limpia la Interfaz de Consulta al Iniciar la misma
        /// </summary>
        public void PrepararNuevo() {

            #region General
            this.vista.TieneCondicionMangueras = null;
            this.vista.TieneTaponesMangueras = null;
            this.vista.TieneCondicionBujes = null;
            this.vista.TieneCondicionPasadores = null;
            this.vista.TieneCondicionPica = null;
            this.vista.TieneTorqueTornillos = null;
            this.vista.TieneGraseraManual = null;
            #endregion
            #region Lubricación
            this.vista.TieneBujes = null;
            this.vista.TienePasadores = null;
            this.vista.TienePica = null;
            #endregion
            #region Miscelaneos
            this.vista.TieneCondicionCalcas = null;
            this.vista.TieneCondicionPintura = null;
            this.vista.TieneSimbolosSeguridad = null;
            this.vista.TieneEstructura = null;
            #endregion
        }

        public string ValidarCampos() {
            StringBuilder s = new StringBuilder();

            #region General
            if (!this.vista.TieneCondicionMangueras.Value)
                s.Append("Condición de Mangueras, ");
            if (!this.vista.TieneTaponesMangueras.Value)
                s.Append("Tapones de las Mangueras de Alimentación, ");
            if (!this.vista.TieneCondicionBujes.Value)
                s.Append("Condición de Bujes, ");
            if (!this.vista.TieneCondicionPasadores.Value)
                s.Append("Condición de Pasadores, ");
            if (!this.vista.TieneCondicionPica.Value)
                s.Append("Condición de la Pica, ");
            if (!this.vista.TieneTorqueTornillos.Value)
                s.Append("Torque de Tornillos de la Base del Martillo, ");
            if (!this.vista.TieneGraseraManual.Value)
                s.Append("Grasera Manual, ");
            #endregion
            #region Lubricación
            if (!this.vista.TieneBujes.Value)
                s.Append("Bujes, ");
            if (!this.vista.TienePasadores.Value)
                s.Append("Pasadores, ");
            if (!this.vista.TienePica.Value)
                s.Append("Pica, ");
            #endregion
            #region Miscelaneos
            if (!this.vista.TieneCondicionPintura.Value)
                s.Append("Condición de Pintura, ");
            if (!this.vista.TieneCondicionCalcas.Value)
                s.Append("Condición de Calcas, ");
            if (!this.vista.TieneSimbolosSeguridad.Value)
                s.Append("Símbolos de Seguridad de la Máquina, ");
            if (!this.vista.TieneEstructura.Value)
                s.Append("Estructura , ");
            #endregion

            if (s.Length > 0)
                return "Es necesario capturar observaciones para los siguientes puntos de verificación: \n" + s.ToString().Substring(0, s.Length - 2);

            return null;
        }

        #endregion


        public void DatoToInterfazUsuario(ListadoVerificacionMartilloHidraulicoBO bo) {


            #region General


            this.vista.TieneCondicionMangueras = bo.TieneCondicionMangueras.HasValue ? bo.TieneCondicionMangueras : null;

            this.vista.TieneTaponesMangueras = bo.TieneTaponesMangueras.HasValue ? bo.TieneTaponesMangueras : null;

            this.vista.TieneCondicionBujes = bo.TieneCondicionBujes.HasValue ? bo.TieneCondicionBujes : null;


            this.vista.TieneCondicionPasadores = bo.TieneCondicionPasadores.HasValue ? bo.TieneCondicionPasadores : null;


            this.vista.TieneCondicionPica = bo.TieneCondicionPica.HasValue ? bo.TieneCondicionPica : null;


            this.vista.TieneTorqueTornillos = bo.TieneTorqueTornillos.HasValue ? bo.TieneTorqueTornillos : null;


            this.vista.TieneGraseraManual = bo.TieneGraseraManual.HasValue ? bo.TieneGraseraManual : null;


            #endregion

            #region Lubricación

            this.vista.TieneBujes = bo.TieneBujes.HasValue ? bo.TieneBujes : null;

            this.vista.TienePasadores = bo.TienePasadores.HasValue ? bo.TienePasadores : null;

            this.vista.TienePica = bo.TienePica.HasValue ? bo.TienePica : null;

            #endregion

            #region Miscelaneos

            this.vista.TieneCondicionPintura = bo.TieneCondicionPintura.HasValue ? bo.TieneCondicionPintura : null;

            this.vista.TieneCondicionCalcas = bo.TieneCondicionCalcas.HasValue ? bo.TieneCondicionCalcas : null;

            this.vista.TieneSimbolosSeguridad = bo.TieneSimbolosSeguridad.HasValue ? bo.TieneSimbolosSeguridad : null;

            this.vista.TieneEstructura = bo.TieneEstructura.HasValue ? bo.TieneEstructura : null;
            #endregion
        }

        public object InterfazUsuarioADato() {
            ListadoVerificacionMartilloHidraulicoBO bo = new ListadoVerificacionMartilloHidraulicoBO();


            #region General

            if (this.vista.TieneCondicionMangueras.HasValue)
                bo.TieneCondicionMangueras = this.vista.TieneCondicionMangueras.Value;

            if (this.vista.TieneTaponesMangueras.HasValue)
                bo.TieneTaponesMangueras = this.vista.TieneTaponesMangueras.Value;

            if (this.vista.TieneCondicionBujes.HasValue)
                bo.TieneCondicionBujes = this.vista.TieneCondicionBujes.Value;

            if (this.vista.TieneCondicionPasadores.HasValue)
                bo.TieneCondicionPasadores = this.vista.TieneCondicionPasadores.Value;

            if (this.vista.TieneCondicionPica.HasValue)
                bo.TieneCondicionPica = this.vista.TieneCondicionPica.Value;

            if (this.vista.TieneTorqueTornillos.HasValue)
                bo.TieneTorqueTornillos = this.vista.TieneTorqueTornillos.Value;

            if (this.vista.TieneGraseraManual.HasValue)
                bo.TieneGraseraManual = this.vista.TieneGraseraManual.Value;


            #endregion

            #region Lubricación
            if (this.vista.TieneBujes.HasValue)
                bo.TieneBujes = this.vista.TieneBujes.Value;
            if (this.vista.TienePasadores.HasValue)
                bo.TienePasadores = this.vista.TienePasadores.Value;
            if (this.vista.TienePica.HasValue)
                bo.TienePica = this.vista.TienePica.Value;

            #endregion

            #region Miscelaneos
            if (this.vista.TieneCondicionPintura.HasValue)
                bo.TieneCondicionPintura = this.vista.TieneCondicionPintura.Value;
            if (this.vista.TieneCondicionCalcas.HasValue)
                bo.TieneCondicionCalcas = this.vista.TieneCondicionCalcas.Value;
            if (this.vista.TieneSimbolosSeguridad.HasValue)
                bo.TieneSimbolosSeguridad = this.vista.TieneSimbolosSeguridad.Value;
            if (this.vista.TieneEstructura.HasValue)
                bo.TieneEstructura = this.vista.TieneEstructura.Value;
            #endregion


            return bo;
        }
    }
}