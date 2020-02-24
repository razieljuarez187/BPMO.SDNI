

using System;
using System.Text;
using BPMO.Facade.SDNI.BR;
using BPMO.Patterns.Creational.DataContext;
using BPMO.SDNI.Contratos.PSL.BO;
using BPMO.SDNI.Contratos.PSL.VIS;

namespace BPMO.SDNI.Contratos.PSL.PRE {
    public class ucPuntosVerificacionPistolaNeumaticaPSLPRE {
        #region Atributos
        /// <summary>
        /// Nombre de la Clase, usado para Excepciones
        /// </summary>
        private string nombreClase = "ucPuntosVerificacionPistolaNeumaticaPSLPRE";
        /// <summary>
        /// Vista de Consulta de Tarifas
        /// </summary>
        private IucPuntosVerificacionPistolaNeumaticaPSLVIS vista;
        internal IucPuntosVerificacionPistolaNeumaticaPSLVIS Vista { get { return vista; } }

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
        public ucPuntosVerificacionPistolaNeumaticaPSLPRE(IucPuntosVerificacionPistolaNeumaticaPSLVIS vistaActual) {
            try {
                this.vista = vistaActual;
                dctx = FacadeBR.ObtenerConexion();
            } catch (Exception ex) {
                throw new Exception(nombreClase + ".ucPuntosVerificacionPistolaNeumaticaPSLPRE:Error al configurar el presentador");
            }
        }
        #endregion

        #region Métodos
        public void Registrar() {
            ListadoVerificacionPistolaNeumaticaBO bo = (ListadoVerificacionPistolaNeumaticaBO)this.InterfazUsuarioADato();

        }
        /// <summary>
        /// Limpia la Interfaz de Consulta al Iniciar la misma
        /// </summary>
        /// 
        public void PrepararNuevo() {
            this.vista.TieneAjustePresionCompresor = null;
            this.vista.TieneNivelAceiteLubricador = null;
            this.vista.TieneCondicionBujes = null;
            this.vista.TieneCondicionLubricador = null;
            this.vista.TieneCondicionMangueras = null;
            this.vista.TieneCondicionPica = null;
            this.vista.TieneCondicionPintura = null;
            this.vista.TieneEstructura = null;
        }

        public string ValidarCampos() {
            StringBuilder s = new StringBuilder();

            #region 1 - General
            if (!this.vista.TieneCondicionMangueras.Value)
                s.Append("Condición de Mangueras, ");
            if (!this.vista.TieneCondicionLubricador.Value)
                s.Append("Condición del Lubricador, ");
            if (!this.vista.TieneCondicionBujes.Value)
                s.Append("Condición de Bujes, ");
            if (!this.vista.TieneCondicionPica.Value)
                s.Append("Condición de la Pica, ");
            if (!vista.TieneAjustePresionCompresor.Value)
                s.Append("Ajuste de Presión del Compresor (Max. 90 PSI), ");
            #endregion
            #region 2 - Fluidos
            if (!this.vista.TieneNivelAceiteLubricador.Value)
                s.Append("Nivel de Aceite en el Lubricador, ");
            #endregion
            #region 3 - Lubricacion
            if (!this.vista.TieneCondicionPintura.Value)
                s.Append("Condición de Pintura, ");
            if (!this.vista.TieneEstructura.Value)
                s.Append("Estructura, ");
            #endregion

            if (s.Length > 0)
                return "Es necesario capturar observaciones para los siguientes puntos de verificación: \n" + s.ToString().Substring(0, s.Length - 2);

            return null;
        }


        #endregion

        public object InterfazUsuarioADato() {
            ListadoVerificacionPistolaNeumaticaBO bo = new ListadoVerificacionPistolaNeumaticaBO();

            if (this.vista.TieneAjustePresionCompresor.HasValue)
                bo.TieneAjustePresionCompresor = this.vista.TieneAjustePresionCompresor.Value;
            if (this.vista.TieneCondicionBujes.HasValue)
                bo.TieneCondicionBujes = this.vista.TieneCondicionBujes.Value;
            if (this.vista.TieneCondicionLubricador.HasValue)
                bo.TieneCondicionLubricador = this.vista.TieneCondicionLubricador.Value;
            if (this.vista.TieneCondicionMangueras.HasValue)
                bo.TieneCondicionMangueras = this.vista.TieneCondicionMangueras.Value;
            if (this.vista.TieneCondicionPica.HasValue)
                bo.TieneCondicionPica = this.vista.TieneCondicionPica.Value;
            if (this.vista.TieneCondicionPintura.HasValue)
                bo.TieneCondicionPintura = this.vista.TieneCondicionPintura.Value;
            if (this.vista.TieneEstructura.HasValue)
                bo.TieneEstructura = this.vista.TieneEstructura.Value;
            if (this.vista.TieneNivelAceiteLubricador.HasValue)
                bo.TieneNivelAceiteLubricador = this.vista.TieneNivelAceiteLubricador.Value;

            return bo;
        }

        public void DatoToInterfazUsuario(ListadoVerificacionPistolaNeumaticaBO bo) {

            #region GENERAL (Campos booleanos)
            this.vista.TieneAjustePresionCompresor = bo.TieneAjustePresionCompresor.HasValue ? bo.TieneAjustePresionCompresor : null;
            this.vista.TieneCondicionBujes = bo.TieneCondicionBujes.HasValue ? bo.TieneCondicionBujes : null;
            this.vista.TieneCondicionLubricador = bo.TieneCondicionLubricador.HasValue ? bo.TieneCondicionLubricador : null;
            this.vista.TieneCondicionMangueras = bo.TieneCondicionMangueras.HasValue ? bo.TieneCondicionMangueras : null;
            this.vista.TieneCondicionPica = bo.TieneCondicionPica.HasValue ? bo.TieneCondicionPica : null;

            #endregion

            #region LUBRICACIÓN (Campos booleanos)
            this.vista.TieneNivelAceiteLubricador = bo.TieneNivelAceiteLubricador.HasValue ? bo.TieneNivelAceiteLubricador : null;

            #endregion

            #region MISCELANEOS (Campos booleanos)

            this.vista.TieneCondicionPintura = bo.TieneCondicionPintura.HasValue ? bo.TieneCondicionPintura : null;
            this.vista.TieneEstructura = bo.TieneEstructura.HasValue ? bo.TieneEstructura : null;
            #endregion

        }
    }
}