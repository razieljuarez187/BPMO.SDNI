
using System;
using System.Text;
using BPMO.Facade.SDNI.BR;
using BPMO.Patterns.Creational.DataContext;
using BPMO.SDNI.Contratos.PSL.BO;
using BPMO.SDNI.Contratos.PSL.VIS;

namespace BPMO.SDNI.Contratos.PSL.PRE {
    public class ucPuntosVerificacionPlataformaTijerasPSLPRE {
        #region Atributos
        /// <summary>
        /// Nombre de la Clase, usado para Excepciones
        /// </summary>
        private string nombreClase = "ucPuntosVerificacionPlataformaTijerasPSLPRE";
        /// <summary>
        /// Vista de Consulta de Tarifas
        /// </summary>
        private IucPuntosVerificacionPlataformaTijerasPSLVIS vista;
        internal IucPuntosVerificacionPlataformaTijerasPSLVIS Vista { get { return vista; } }

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
        public ucPuntosVerificacionPlataformaTijerasPSLPRE(IucPuntosVerificacionPlataformaTijerasPSLVIS vistaActual) {
            try {
                this.vista = vistaActual;
                dctx = FacadeBR.ObtenerConexion();
            } catch (Exception ex) {
                throw new Exception(nombreClase + ".ucPuntosVerificacionPlataformaTijerasPSLPRE:Error al configurar el presentador");
            }
        }
        #endregion

        #region Métodos
        public void Registrar() {
            ListadoVerificacionPlataformaTijerasBO bo = (ListadoVerificacionPlataformaTijerasBO)this.InterfazUsuarioADato();

        }
        /// <summary>
        /// Limpia la Interfaz de Consulta al Iniciar la misma
        /// </summary>
        public void PrepararNuevo() {
            this.vista.tieneAceiteHidraulico = null;
            this.vista.tieneAceiteMotor = null;
            this.vista.tieneAlmohadillas = null;
            this.vista.tieneBarrasDireccion = null;
            this.vista.tieneBateria = null;
            this.vista.tieneBrazosTijera = null;
            this.vista.tieneChasis = null;
            this.vista.tieneCilindroDireccion = null;
            this.vista.tieneCilindroElevador = null;
            this.vista.tieneCombustible = null;
            this.vista.tieneConjuntoBarandillas = null;
            this.vista.tieneConjuntoNeumaticosRuedas = null;
            this.vista.tieneControlesPlataforma = null;
            this.vista.tieneControlesTierra = null;
            this.vista.tieneFarosEmergencia = null;
            this.vista.tienePasadoresPivote = null;
            this.vista.tienePivotesDireccion = null;
            this.vista.tienePlataforma = null;
            this.vista.tienePruebaSwitchPothole = null;
            this.vista.tieneReductoresTransito = null;
            this.vista.tieneRefrigerante = null;
            this.vista.tieneVelocidadTransitoExtendida = null;
            this.vista.tieneVelocidadTransitoRetraida = null;

        }

        public object InterfazUsuarioADato() {
            ListadoVerificacionPlataformaTijerasBO bo = new ListadoVerificacionPlataformaTijerasBO();

            #region General (Campos booleanos)
            if (this.vista.tieneConjuntoBarandillas != null)
                bo.TieneConjuntoBarandillas = this.vista.tieneConjuntoBarandillas.Value;
            if (this.vista.tienePlataforma != null)
                bo.TienePlataforma = this.vista.tienePlataforma.Value;
            if (this.vista.tieneBrazosTijera != null)
                bo.TieneBrazosTijera = this.vista.tieneBrazosTijera.Value;
            if (this.vista.tienePasadoresPivote != null)
                bo.TienePasadoresPivote = this.vista.tienePasadoresPivote.Value;
            if (this.vista.tieneCilindroElevador != null)
                bo.TieneCilindroElevador = this.vista.tieneCilindroElevador.Value;
            if (this.vista.tieneChasis != null)
                bo.TieneChasis = this.vista.tieneChasis.Value;
            if (this.vista.tieneConjuntoNeumaticosRuedas != null)
                bo.TieneConjuntoNeumaticosRuedas = this.vista.tieneConjuntoNeumaticosRuedas.Value;
            if (this.vista.tieneAlmohadillas != null)
                bo.TieneAlmohadillas = this.vista.tieneAlmohadillas.Value;
            if (this.vista.tieneCilindroDireccion != null)
                bo.TieneCilindroDireccion = this.vista.tieneCilindroDireccion.Value;
            if (this.vista.tieneBarrasDireccion != null)
                bo.TieneBarrasDireccion = this.vista.tieneBarrasDireccion.Value;
            if (this.vista.tieneControlesTierra != null)
                bo.TieneControlesTierra = this.vista.tieneControlesTierra.Value;
            if (this.vista.tieneControlesPlataforma != null)
                bo.TieneControlesPlataforma = this.vista.tieneControlesPlataforma.Value;
            if (this.vista.tieneFarosEmergencia != null)
                bo.TieneFarosEmergencia = this.vista.tieneFarosEmergencia.Value;
            if (this.vista.tieneVelocidadTransitoRetraida != null)
                bo.TieneVelocidadTransitoRetraida = this.vista.tieneVelocidadTransitoRetraida.Value;
            if (this.vista.tieneVelocidadTransitoExtendida != null)
                bo.TieneVelocidadTransitoExtendida = this.vista.tieneVelocidadTransitoExtendida.Value;
            if (this.vista.tienePruebaSwitchPothole != null)
                bo.TienePruebaSwitchPothole = this.vista.tienePruebaSwitchPothole.Value;
            #endregion

            #region Niveles Flujos (Campos booleanos)
            if (this.vista.tieneBateria != null)
                bo.TieneBateria = this.vista.tieneBateria.Value;
            if (this.vista.tieneAceiteHidraulico != null)
                bo.TieneAceiteHidraulico = this.vista.tieneAceiteHidraulico.Value;
            if (this.vista.tieneReductoresTransito != null)
                bo.TieneReductoresTransito = this.vista.tieneReductoresTransito.Value;
            if (this.vista.tieneAceiteMotor != null)
                bo.TieneAceiteMotor = this.vista.tieneAceiteMotor.Value;
            if (this.vista.tieneRefrigerante != null)
                bo.TieneRefrigerante = this.vista.tieneRefrigerante.Value;

            #endregion

            #region Lubricación (Campos booleanos)
            if (this.vista.tienePivotesDireccion != null)
                bo.TienePivotesDireccion = this.vista.tienePivotesDireccion.Value;
            #endregion

            #region Funciones Electricias (Campos numericos)

            #endregion

            #region Controles (Campos booleanos)

            #endregion

            #region Miscelaneo (Campos numericos)

            #endregion


            return bo;
        }

        public string ValidarCampos() {
            StringBuilder s = new StringBuilder();

            #region 1 - Generales
            if (!vista.tieneConjuntoBarandillas.Value)
                s.Append("Conjunto de Barandillas, ");
            if (!vista.tienePlataforma.Value)
                s.Append("Plataforma, ");
            if (!vista.tieneBrazosTijera.Value)
                s.Append("Brazos de Tijera, ");
            if (!vista.tienePasadoresPivote.Value)
                s.Append("Pasadores de Pivote de Tijera, ");
            if (!vista.tieneCilindroElevador.Value)
                s.Append("Cilindro Elevador, ");
            if (!vista.tieneChasis.Value)
                s.Append("Chasis, ");
            if (!vista.tieneConjuntoNeumaticosRuedas.Value)
                s.Append("Conjunto de Neumáticos/Ruedas, ");
            if (!vista.tieneAlmohadillas.Value)
                s.Append("Almohadillas de Desgaste Deslizantes, ");
            if (!vista.tieneCilindroDireccion.Value)
                s.Append("Cilindro de Dirección, ");
            if (!vista.tieneBarrasDireccion.Value)
                s.Append("Barras de Dirección, ");
            if (!vista.tieneControlesTierra.Value)
                s.Append("Funcionamiento de Controles de Tierra, ");
            if (!vista.tieneControlesPlataforma.Value)
                s.Append("Funcionamiento de Controles de Plataforma, ");
            if (!vista.tieneFarosEmergencia.Value)
                s.Append("Faros de Emergencia, ");
            if (!vista.tieneVelocidadTransitoRetraida.Value)
                s.Append("Velocidad de Tránsito con Plataforma Retraida, ");
            if (!vista.tieneVelocidadTransitoExtendida.Value)
                s.Append("Velocidad de Tránsito con Plataforma Extendida, ");
            if (!vista.tienePruebaSwitchPothole.Value)
                s.Append("Prueba de los Switches de Límite del Pothole, ");
            #endregion
            #region 2 - Niveles de Fluidos
            if (!vista.tieneBateria.Value)
                s.Append("Batería, ");
            if (!vista.tieneAceiteHidraulico.Value)
                s.Append("Aceite Hidráulico, ");
            if (!vista.tieneReductoresTransito.Value)
                s.Append("Reductores de tránsito, ");
            if (!vista.tieneAceiteMotor.Value)
                s.Append("Aceite de motor, ");
            if (!vista.tieneRefrigerante.Value)
                s.Append("Refrigerante, ");
            #endregion
            #region 3 - Lubricación
            if (!vista.tienePivotesDireccion.Value)
                s.Append("Pivotes de la dirección, ");
            #endregion

            if (s.Length > 0)
                return "Es necesario capturar observaciones para los siguientes puntos de verificación: \n" + s.ToString().Substring(0, s.Length - 2);

            return null;
        }

        public void DatoToInterfazUsuario(ListadoVerificacionPlataformaTijerasBO bo) {
            #region General (Campos booleanos)
            this.vista.tieneConjuntoBarandillas = bo.TieneConjuntoBarandillas.HasValue ? bo.TieneConjuntoBarandillas : false;
            this.vista.tienePlataforma = bo.TienePlataforma.HasValue ? bo.TienePlataforma : false;
            this.vista.tieneBrazosTijera = bo.TieneBrazosTijera.HasValue ? bo.TieneBrazosTijera : false;
            this.vista.tienePasadoresPivote = bo.TienePasadoresPivote.HasValue ? bo.TienePasadoresPivote : false;
            this.vista.tieneCilindroElevador = bo.TieneCilindroElevador.HasValue ? bo.TieneCilindroElevador : false;
            this.vista.tieneChasis = bo.TieneChasis.HasValue ? bo.TieneChasis : false;
            this.vista.tieneConjuntoNeumaticosRuedas = bo.TieneConjuntoNeumaticosRuedas.HasValue ? bo.TieneConjuntoNeumaticosRuedas : false;
            this.vista.tieneAlmohadillas = bo.TieneAlmohadillas.HasValue ? bo.TieneAlmohadillas : false;
            this.vista.tieneCilindroDireccion = bo.TieneCilindroDireccion.HasValue ? bo.TieneCilindroDireccion : false;
            this.vista.tieneBarrasDireccion = bo.TieneBarrasDireccion.HasValue ? bo.TieneBarrasDireccion : false;
            this.vista.tieneControlesTierra = bo.TieneControlesTierra.HasValue ? bo.TieneControlesTierra : false;
            this.vista.tieneControlesPlataforma = bo.TieneControlesPlataforma.HasValue ? bo.TieneControlesPlataforma : false;
            this.vista.tieneFarosEmergencia = bo.TieneFarosEmergencia.HasValue ? bo.TieneFarosEmergencia : false;
            this.vista.tieneVelocidadTransitoRetraida = bo.TieneVelocidadTransitoRetraida.HasValue ? bo.TieneVelocidadTransitoRetraida : false;
            this.vista.tieneVelocidadTransitoExtendida = bo.TieneVelocidadTransitoExtendida.HasValue ? bo.TieneVelocidadTransitoExtendida : false;
            this.vista.tienePruebaSwitchPothole = bo.TienePruebaSwitchPothole.HasValue ? bo.TienePruebaSwitchPothole : false;
            #endregion

            #region Niveles Flujos (Campos booleanos)
            this.vista.tieneBateria = bo.TieneBateria.HasValue ? bo.TieneBateria : false;
            this.vista.tieneAceiteHidraulico = bo.TieneAceiteHidraulico.HasValue ? bo.TieneAceiteHidraulico : false;
            this.vista.tieneReductoresTransito = bo.TieneReductoresTransito.HasValue ? bo.TieneReductoresTransito : false;
            this.vista.tieneAceiteMotor = bo.TieneAceiteMotor.HasValue ? bo.TieneAceiteMotor : false;
            this.vista.tieneRefrigerante = bo.TieneRefrigerante.HasValue ? bo.TieneRefrigerante : false;

            #endregion

            #region Lubricación (Campos booleanos)
            this.vista.tienePivotesDireccion = bo.TienePivotesDireccion.HasValue ? bo.TienePivotesDireccion : false;
            #endregion

            #region Funciones Electricias (Campos numericos)

            #endregion

            #region Controles (Campos booleanos)

            #endregion

            #region Miscelaneo (Campos numericos)

            #endregion
        }
        #endregion

    }
}