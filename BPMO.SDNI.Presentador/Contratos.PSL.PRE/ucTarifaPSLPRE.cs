// Satisface al caso de uso CU005 - Catálogo de Tarifas de Renta Comercial Diaria.
// Construcción de Mejoras - Cobro de Rangos de Km y Horas.
using System;
using System.Collections.Generic;
using System.Linq;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Contratos.PSL.BO;
using BPMO.SDNI.Contratos.PSL.VIS;

namespace BPMO.SDNI.Contratos.PSL.PRE {
    public class ucTarifaPSLPRE {
        #region Atributos
        private string nombreClase = "ucTarifaPSLPRE";
        private IucTarifaPSLVIS vista;
        #endregion

        #region Constructor
        public ucTarifaPSLPRE(IucTarifaPSLVIS vista) {
            try {
                this.vista = vista;
                // Esto conserva el Modo antiguo de Tarifas
                if (this.vista.CrearRangos != null) {
                    if (!this.vista.CrearRangos.Value)
                        this.vista.ModoAntiguo(true);
                }
            } catch (Exception ex) {
                throw new Exception(nombreClase + ".ucTarifasPSLPRE:Error al configurar el presentador.");
            }
        }
        #endregion

        #region Métodos
        public void PrepararNuevo() {
            this.vista.LimpiarSesion();
            this.vista.ModoEdicion(true);
            this.vista.Tarifa = null;
            this.vista.TarifaHrAdicional = null;
            this.vista.RangoInicial = null;
            this.vista.RangoFinal = null;
            this.vista.CostoRango = null;
            this.vista.RangosTarifa = new List<RangoTarifaPSLBO>();
            if (this.vista.CrearRangos != null) {
                if (this.vista.CrearRangos.Value)
                    this.CambiarTipoCargo(true);
            }
        }
        public string ValidarDatos() {
            string s = "";

            if (this.vista.Tarifa == null)
                s += "Tarifa, ";
            if (this.vista.CrearRangos.Value) {
                if (this.vista.RangosTarifa == null)
                    s += "Rangos de Tarifa, ";
            } else {
                if (this.vista.TarifaHrAdicional == null)
                    s += "Tarifa Hr Adicional, ";
            }

            if (s != null && s.Trim().CompareTo("") != 0)
                return "Los siguientes campos no pueden estar vacíos: \n" + s.Substring(0, s.Length - 2);

            if (s != null && s.Trim().CompareTo("") != 0)
                return "Los siguientes campos no pueden ser menores a cero: \n" + s.Substring(0, s.Length - 2);

            if (this.vista.CrearRangos.Value) {
                if (!this.vista.RangosTarifa.Any())
                    return "Tiene que existir al menos un Rango Configurado para la Tarifa";

                if (!this.vista.RangosTarifa.Any(x => x.RangoFinal == null && x.RangoInicial > 0))
                    return "Tiene que existir un Rango de Tarifa sin Rango de Final";
            }

            return null;
        }

        public void CambiarTipoCargo(bool bloquearControles) {
            this.vista.RangoInicial = null;
            this.vista.RangoFinal = null;
            this.vista.EsRangoFinal = false;
            this.vista.CostoRango = null;
            this.vista.RangosTarifa = new List<RangoTarifaPSLBO>();

            var noPermitir = !bloquearControles;
            this.vista.PermitirRangoInicial(noPermitir);
            this.vista.PermiritRangoCargo(noPermitir);
            this.vista.PermitirRangoFinal(noPermitir);
            this.vista.PermitirCargoAdicional(noPermitir);
            this.vista.PermitirAgregarRangos(noPermitir);
            this.vista.PresentarRangos(this.vista.RangosTarifa);

        }

        /// <summary>
        /// Agrega un rango de un Tarifa
        /// </summary>
        public void AgregarRangoATarifa() {
            var validarRango = ValidarRango();
            if (!String.IsNullOrEmpty(validarRango)) {
                this.vista.MostrarMensaje(validarRango, ETipoMensajeIU.ADVERTENCIA);
                return;
            }
            this.vista.RangosTarifa.Add(InterfazARangoTarifaPSLBo());

            this.vista.RangoInicial = null;
            this.vista.RangoFinal = null;
            this.vista.EsRangoFinal = false;
            this.vista.CostoRango = null;

            this.vista.PresentarRangos(this.vista.RangosTarifa);
        }
        /// <summary>
        /// Obtiene el Rango de Tarifa que se encuentre en la interfaz para agregar
        /// </summary>
        /// <returns></returns>
        private RangoTarifaPSLBO InterfazARangoTarifaPSLBo() {
            RangoTarifaPSLBO rango = new RangoTarifaPSLBO();

            rango.RangoInicial = this.vista.RangoInicial;
            rango.RangoFinal = this.vista.EsRangoFinal.Value ? null : this.vista.RangoFinal;
            rango.Cargo = this.vista.CostoRango;

            return rango;
        }
        /// <summary>
        /// Valida que se pueda agregar el rango a la lista de Rangos
        /// </summary>
        /// <returns>Cadena que contiene una advertencia sobre el rango ingresado</returns>
        public string ValidarRango() {
            if (this.vista.RangosTarifa == null)
                this.vista.RangosTarifa = new List<RangoTarifaPSLBO>();
            if (this.vista.RangoInicial == null)
                return "El Rango Inicial no pueder ser nulo";
            if (this.vista.RangoInicial < 0)
                return "El Rango Inicial no puede ser Menor a 0";
            if (this.vista.RangosTarifa != null && this.vista.RangosTarifa.Any()) {
                var ultima = this.vista.RangosTarifa.OrderBy(x => x.RangoInicial).LastOrDefault();
                if (ultima.RangoFinal == null)
                    return "Ya se ha configurado un rango final, Elimine el Último Rango para agregar un nuevo rango.";
                if (ultima.RangoFinal > this.vista.RangoInicial)
                    return "El Rango Inicial debe ser mayor al Rango Final Anterior";
                if (ultima.RangoFinal == vista.RangoInicial || ultima.RangoFinal + 1 != this.vista.RangoInicial)
                    return "El Rango Inicial tiene que se mayor en UNO a los Kilometros Finales Anteriores";
            }
            if (this.vista.RangoFinal == null && !this.vista.EsRangoFinal.Value)
                return "El Rango Final no puede ser Nulo, si este NO es el Último Rango de la Tarifa";
            if (!this.vista.EsRangoFinal.Value && this.vista.RangoFinal != null && this.vista.RangoFinal < 0)
                return "El Rango final no puede ser menor a 0";
            if (!this.vista.EsRangoFinal.Value && this.vista.RangoFinal != null && this.vista.RangoFinal <= this.vista.RangoInicial)
                return "El Rango Final no puede ser menor o igual que el Rango Inicial";
            if (this.vista.CostoRango == null)
                return "El costo del Rango no puede ser nulo";
            if (this.vista.CostoRango < 0)
                return "El costo del Rango no puede ser Menor a 0";

            return null;
        }
        public void DatosAInterfazUsuario(TarifaPSLBO tarifa) {
            try {
                if (Object.ReferenceEquals(tarifa, null))
                    tarifa = new TarifaPSLBO() { RangoTarifas = new List<RangoTarifaPSLBO>() { new RangoTarifaPSLBO() } };

                this.vista.Tarifa = tarifa.Tarifa;
                this.vista.TarifaHrAdicional = tarifa.TarifaHrAdicional;
                this.vista.RangosTarifa = tarifa.RangoTarifas;
                if (this.vista.CrearRangos.Value) {
                    this.vista.PresentarRangos(tarifa.RangoTarifas);
                }

            } catch (Exception ex) {
                throw new Exception(this.nombreClase + ".DatosAInterfazUsuario:Error al intentar establecer los datos de la tarifa." + ex.Message);
            }
        }
        public TarifaPSLBO InterfazUsuarioADato() {
            try {
                TarifaPSLBO tarifa = new TarifaPSLBO();
                tarifa.RangoTarifas = new List<RangoTarifaPSLBO>();
                tarifa.Tarifa = this.vista.Tarifa;
                tarifa.TarifaHrAdicional = this.vista.TarifaHrAdicional;
                if (this.vista.CrearRangos.Value)
                    tarifa.RangoTarifas.AddRange(this.vista.RangosTarifa.Select(rango => new RangoTarifaPSLBO(rango)).ToList());
                else {
                    var rangoTarifa = new RangoTarifaPSLBO()
                    {
                        RangoInicial = 1,
                        RangoFinal = null,
                        Cargo = 0
                    };
                    tarifa.RangoTarifas.Add(rangoTarifa);
                }

                return tarifa;
            } catch (Exception ex) {
                throw new Exception(this.nombreClase + ".InterfazUsuarioADato:Error al intentar obtener las tarifas." + ex.Message);
            }
        }
        public void ModoConsulta(bool activo) {
            this.vista.ModoEdicion(false);
        }
        #endregion
    }
}