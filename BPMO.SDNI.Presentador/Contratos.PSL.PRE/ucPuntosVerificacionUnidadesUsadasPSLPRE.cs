
using System;
using BPMO.Facade.SDNI.BR;
using BPMO.Patterns.Creational.DataContext;
using BPMO.SDNI.Contratos.PSL.BO;
using BPMO.SDNI.Contratos.PSL.VIS;

namespace BPMO.SDNI.Contratos.PSL.PRE
{
    public class ucPuntosVerificacionUnidadesUsadasPSLPRE
    {
        #region Atributos
        /// <summary>
        /// Nombre de la Clase, usado para Excepciones
        /// </summary>
        private string nombreClase = "ucPuntosVerificacionUnidadesUsadasPSLPRE";
        /// <summary>
        /// Vista de Consulta de Tarifas
        /// </summary>
        private IucPuntosVerificacionUnidadesUsadasPSLVIS vista;
        internal IucPuntosVerificacionUnidadesUsadasPSLVIS Vista { get { return vista; } }

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
        public ucPuntosVerificacionUnidadesUsadasPSLPRE(IucPuntosVerificacionUnidadesUsadasPSLVIS vistaActual)
        {
            try
            {
                this.vista = vistaActual;
                dctx = FacadeBR.ObtenerConexion();
            }
            catch (Exception ex)
            {
                throw new Exception(nombreClase + ".ucPuntosVerificacionUnidadesUsadasPSLPRE:Error al configurar el presentador");
            }
        }
        #endregion

        #region Métodos
        public void Registrar()
        {
            ListadoVerificacionUnidadesUsadasBO bo = (ListadoVerificacionUnidadesUsadasBO)this.InterfazUsuarioADato();

        }
        /// <summary>
        /// Limpia la Interfaz de Consulta al Iniciar la misma
        /// </summary>
        public void PrepararNuevo()
        {
            this.vista.tieneCopiaBitacoraServicio = null;
            this.vista.tieneCopiaPedimento = null;
            this.vista.tieneFacturaYXmlFiscal = null;
            this.vista.tieneFotografiasInternaExternaUnidad = null;
            this.vista.tieneLavadoExterior = null;
            this.vista.tieneLavadoInterior = null;
        }
        #endregion

        private object InterfazUsuarioADato()
        {
            ListadoVerificacionUnidadesUsadasBO bo = new ListadoVerificacionUnidadesUsadasBO();


            //this.vista.tieneZapatas;
            //this.vista.tieneBrazoPluma;
            //this.vista.tienesContrapeso;
            //this.vista.tieneVastagosGatos;
            //this.vista.tieneTensionCadena;
            //this.vista.tieneRodillosTransito;
            //this.vista.tieneEspejosRetrovisores;
            //this.vista.tieneCristalesCabina;
            //this.vista.tienePuertasCerraduras;
            //this.vista.tieneBisagrasCofreMotor;
            //this.vista.tieneBalancinBote;
            //this.vista.tieneLucesTrabajo;
            //this.vista.tieneLamparasTablero;
            //this.vista.tieneInterruptorDesconexion;
            //this.vista.tieneAlarmaReversa;
            //this.vista.tieneHorometro;
            //this.vista.tieneLimpiaparabrisas;
            //this.vista.tieneCombustible;
            //this.vista.tieneAceiteMotor;
            //this.vista.tieneAceiteHidraulico;
            //this.vista.tieneLiquidoRefrigerante;
            //this.vista.tieneReductorEngranesTransito;
            //this.vista.tieneReductorSwing;
            //this.vista.tieneBateria;
            //this.vista.tieneJoystick;
            //this.vista.tieneLucesAdvertencia;
            //this.vista.tieneIndicadores;
            //this.vista.tienePalancaBloqueoPilotaje;
            //this.vista.tieneAireAcondicionado;
            //this.vista.tieneAutoaceleracion;
            //this.vista.tieneVelocidadMinimaMotor;
            //this.vista.tieneVelocidadMaximaMotor;

            return bo;
        }
    }
}
