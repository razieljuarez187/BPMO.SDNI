// Satisface al caso de uso CU005 - Catálogo de Tarifas de Renta Comercial Diaria.
// Construccion de Mejoras - Cobro de Rangos de Km y Horas.
using System;
using System.Collections.Generic;
using System.Linq;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Contratos.RD.BO;
using BPMO.SDNI.Contratos.RD.VIS;

namespace BPMO.SDNI.Contratos.RD.PRE
{
    public class ucTarifaRDPRE
    {
        #region Atributos
        private string nombreClase = "ucTarifaRDPRE";
        private IucTarifaRDVIS vista;
        #endregion

        #region Constructor
        public ucTarifaRDPRE(IucTarifaRDVIS vista)
        {
            try
            {
                this.vista = vista;
                // Esto conserva el Modo antiguo de Tarifas
                if (this.vista.CrearRangos != null)
                {
                    if(!this.vista.CrearRangos.Value)
                        this.vista.ModoAntiguo(true);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(nombreClase + ".ucTarifasRDPRE:Error al configurar el presentador.");
            }
        }
        #endregion

        #region Métodos
        public void PrepararNuevo()
        {
            this.vista.LimpiarSesion();
            this.vista.ModoEdicion(true);
            this.vista.CapacidadCarga = null;
            this.vista.CobraKm = null;
            this.vista.HorasLibres = null;
            this.vista.KmLibres = null;
            this.vista.TarifaDiaria = null;
            this.vista.RangoInicial = null;
            this.vista.RangoFinal = null;
            this.vista.CostoRango = null;
            this.vista.TarifaKmAdicional = null;
            this.vista.TarifaHrAdicional = null;

            if (this.vista.CrearRangos != null)
            {
                if(this.vista.CrearRangos.Value)
                    this.CambiarTipoCargo(true);
            }
        }
        public string ValidarDatos()
        {
            string s = "";

            if (this.vista.CapacidadCarga == null)
                s += "Capacidad de Carga, ";
            if (this.vista.TarifaDiaria == null)
                s += "Tarifa Diaria";
            if (this.vista.CrearRangos.Value)
            {
                if(this.vista.CobraKm == null)
                    s += "Tipo de Cargo, ";
                if (this.vista.RangosTarifa == null)
                    s += "Rangos de Tarifa";
            }
            else
            {
                if(this.vista.TarifaKmAdicional == null)
                    s += "Tarifa por Km Adicional";
                if(this.vista.TarifaHrAdicional == null)
                    s += "Tarifa por Hr Adicional";
            }

            if (s != null && s.Trim().CompareTo("") != 0)
                return "Los siguientes campos no pueden estar vacíos: \n" + s.Substring(0, s.Length - 2);

            if (this.vista.CrearRangos.Value)
            {
                if (this.vista.CobraKm.Value)
                {
                    if (this.vista.KmLibres == null)
                        s += "Kilómetros Libres, ";
                }
                else
                {
                    if (this.vista.HorasLibres == null)
                        s += "Horas Libres, ";
                }
                if (s != null && s.Trim().CompareTo("") != 0)
                    return "Los siguientes campos no pueden estar vacíos: \n" + s.Substring(0, s.Length - 2);
            }

            if (this.vista.CapacidadCarga < 0)
                s += "Capacidad de carga, ";
            if (this.vista.CrearRangos.Value)
            {
                if (!this.vista.CobraKm.Value)
                {
                    if (this.vista.HorasLibres < 0)
                        s += "Horas Libres, ";
                }
                else
                {
                    if (this.vista.KmLibres < 0)
                        s += "Kilometros Libres, ";
                }
            }
            else
            {
                if(this.vista.HorasLibres < 0)
                    s += "Horas Libres, ";
                if(this.vista.KmLibres < 0)
                    s += "Kilometros Libres, ";
                if(this.vista.TarifaKmAdicional < 0)
                    s += "Tarifa por Km Adicional, ";
                if(this.vista.TarifaHrAdicional < 0)
                    s += "Tarifa por Hr Adicional, ";
            }
            if (this.vista.TarifaDiaria < 0)
                s += "Tarifa Diaria, ";

            if (s != null && s.Trim().CompareTo("") != 0)
                return "Los siguientes campos no pueden ser menores a cero: \n" + s.Substring(0, s.Length - 2);
           
            if (this.vista.CrearRangos.Value)
            {
                if (!this.vista.RangosTarifa.Any())
                    return "Tiene que existir al menos un Rango Configurado para la Tarifa";

                if (this.vista.CobraKm.Value)
                {
                    if (!this.vista.RangosTarifa.Any(x => x.KmRangoFinal == null && x.KmRangoInicial > 0))
                        return "Tiene que existir un Rango de Tarifa sin Rango de Kilometraje Final";
                }
                else
                {
                    if (!this.vista.RangosTarifa.Any(x => x.HrRangoFinal == null && x.HrRangoInicial > 0))
                        return "Tiene que existir un Rango de Tarifa sin Rango de Horas Final";
                }
            }

            return null;
        }

        public void CambiarTipoCargo(bool bloquearControles)
        {
            this.vista.KmLibres = null;
            this.vista.HorasLibres = null;
            this.vista.RangoInicial = null;
            this.vista.RangoFinal = null;
            this.vista.EsRangoFinal = false;
            this.vista.CostoRango = null;
            this.vista.RangosTarifa = new List<RangoTarifaRDBO>();

            var noPermitir = !bloquearControles;

            this.vista.PermitirKmLibres(noPermitir);
            this.vista.PermitirHrsLibres(noPermitir);
            this.vista.PermitirRangoInicial(noPermitir);
            this.vista.PermiritRangoCargo(noPermitir);
            this.vista.PermitirRangoFinal(noPermitir);
            this.vista.PermitirCargoAdicional(noPermitir);
            this.vista.PermitirAgregarRangos(noPermitir);
            this.vista.PresentarRangos(this.vista.RangosTarifa);

            if (this.vista.CobraKm != null)
                BloquearKmsHrs(this.vista.CobraKm.Value);
        }
        public void BloquearKmsHrs(bool cargoPorKm)
        {
            if (cargoPorKm)
            {
                this.vista.PermitirHrsLibres(false);
                this.vista.PermitirKmLibres(true);
            }
            else
            {
                this.vista.PermitirKmLibres(false);
                this.vista.PermitirHrsLibres(true);
            }
        }
        /// <summary>
        /// Agrega un rango de un Tarifa
        /// </summary>
        public void AgregarRangoATarifa()
        {
            var validarRango = ValidarRango();
            if (!String.IsNullOrEmpty(validarRango))
            {
                this.vista.MostrarMensaje(validarRango, ETipoMensajeIU.ADVERTENCIA);
                return;
            }
            this.vista.RangosTarifa.Add(InterfazARangoTarifaRdBo());

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
        private RangoTarifaRDBO InterfazARangoTarifaRdBo()
        {
            RangoTarifaRDBO rango = new RangoTarifaRDBO();
            if (this.vista.CobraKm.Value)
            {
                rango.KmRangoInicial = this.vista.RangoInicial;
                rango.KmRangoFinal = this.vista.EsRangoFinal.Value ? null : this.vista.RangoFinal;
                rango.CargoKm = this.vista.CostoRango;
            }
            if(!this.vista.CobraKm.Value)
            {
                rango.HrRangoInicial = this.vista.RangoInicial;
                rango.HrRangoFinal = this.vista.EsRangoFinal.Value ? null : this.vista.RangoFinal;
                rango.CargoHr = this.vista.CostoRango;
            }

            return rango;
        }
        /// <summary>
        /// Valida que se pueda agregar el rango a la lista de Rangos
        /// </summary>
        /// <returns>Cadena que contiene una advertencia sobre el rango ingresado</returns>
        public string ValidarRango()
        {
            if (this.vista.RangoInicial == null)
                return "El Rango Inicial no pueder ser nulo";
            if (this.vista.RangoInicial < 0)
                return "El Rango Inicial no puede ser Menor a 0";
            if (!this.vista.RangosTarifa.Any())
            {
                if (this.vista.CobraKm.Value)
                {
                    if (this.vista.KmLibres > this.vista.RangoInicial)
                        return "El Rango Inicial debe ser mayor a los Kilometros Libres";
                    if (this.vista.KmLibres == this.vista.RangoInicial || this.vista.KmLibres + 1 != this.vista.RangoInicial)
                        return "El Rango Inicial tiene que se mayor en UNO a los Kilometros Libres";
                }
                else
                {
                    if(this.vista.HorasLibres > this.vista.RangoInicial)
                        return "El Rango Inicial debe ser mayor a las Horas Libres";
                    if(this.vista.HorasLibres == this.vista.RangoInicial || this.vista.HorasLibres + 1 != this.vista.RangoInicial)
                        return "El Rango Inicial tiene que se mayor en UNO a las Horas Libres";
                }

            }
            else
            {
                var ultima = this.vista.RangosTarifa.OrderBy(x=>this.vista.CobraKm.Value ? x.KmRangoInicial : x.HrRangoInicial).LastOrDefault();
                if (this.vista.CobraKm.Value)
                {
                    if (ultima.KmRangoFinal == null)
                        return "Ya se ha configurado un rango final, Elimine el Último Rango para agregar un nuevo rango.";
                    if (ultima.KmRangoFinal > this.vista.RangoInicial)
                        return "El Rango Inicial debe ser mayor al Rango Final Anterior";
                    if(ultima.KmRangoFinal == vista.RangoInicial || ultima.KmRangoFinal + 1 != this.vista.RangoInicial)
                        return "El Rango Inicial tiene que se mayor en UNO a los Kilometros Finales Anteriores";
                }
                else
                {
                   if(ultima.HrRangoFinal == null)
                       return "Ya se ha configurado un rango final, Elimine el Último para agregar un nuevo rango.";
                   if(ultima.HrRangoFinal > this.vista.RangoInicial)
                       return "El Rango Inicial debe ser mayor al Rango Final Anterior";
                   if(ultima.HrRangoFinal == vista.RangoInicial || ultima.HrRangoFinal + 1 != this.vista.RangoInicial)
                       return "El Rango Inicial tiene que se mayor en UNO a los Kilometros Finales Anteriores";
                }
            }
            if(this.vista.RangoFinal == null && !this.vista.EsRangoFinal.Value)
                return "El Rango Final no puede ser Nulo, si este NO es el Último Rango de la Tarifa";
            if(!this.vista.EsRangoFinal.Value && this.vista.RangoFinal != null && this.vista.RangoFinal < 0)
                return "El Rango final no puede ser menor a 0";
            if(!this.vista.EsRangoFinal.Value && this.vista.RangoFinal != null && this.vista.RangoFinal <= this.vista.RangoInicial)
                return "El Rango Final no puede ser menor o igual que el Rango Inicial";
            if (this.vista.CostoRango == null)
                return "El costo del Rango no puede ser nulo";
            if (this.vista.CostoRango < 0)
                return "El costo del Rango no puede ser Menor a 0";

            return null;
        }
        public void DatosAInterfazUsuario(TarifaRDBO tarifa)
        {
            try
            {
                if (Object.ReferenceEquals(tarifa, null))
                    tarifa = new TarifaRDBO(){RangoTarifas = new List<RangoTarifaRDBO>(){new RangoTarifaRDBO()}};
                
                this.vista.CapacidadCarga = tarifa.CapacidadCarga;
                this.vista.HorasLibres = tarifa.HrsLibres;
                this.vista.KmLibres = tarifa.KmsLibres;
                this.vista.TarifaDiaria = tarifa.TarifaDiaria;
                this.vista.RangosTarifa = tarifa.RangoTarifas;
                this.vista.CobraKm = tarifa.CobraKm;
                if (!this.vista.CrearRangos.Value)
                {
                    if (tarifa.RangoTarifas != null && tarifa.RangoTarifas.Any())
                    {
                        this.vista.TarifaKmAdicional = tarifa.RangoTarifas.First().CargoKm;
                        this.vista.TarifaHrAdicional = tarifa.RangoTarifas.First().CargoHr;
                    }
                }
                else
                    this.vista.PresentarRangos(tarifa.RangoTarifas);
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".DatosAInterfazUsuario:Error al intentar establecer los datos de la tarifa." + ex.Message);
            }
        }
        public TarifaRDBO InterfazUsuarioADato()
        {
            try
            {
                TarifaRDBO tarifa = new TarifaRDBO();
                tarifa.RangoTarifas = new List<RangoTarifaRDBO>();

                tarifa.CapacidadCarga = this.vista.CapacidadCarga;
                tarifa.HrsLibres = this.vista.HorasLibres;
                tarifa.KmsLibres = this.vista.KmLibres;
                tarifa.TarifaDiaria = this.vista.TarifaDiaria;
                tarifa.CobraKm = this.vista.CobraKm;
                if (this.vista.CrearRangos.Value)
                    tarifa.RangoTarifas.AddRange(this.vista.RangosTarifa.Select(rango => new RangoTarifaRDBO(rango)).ToList());
                else
                {
                    var rangoTarifa = new RangoTarifaRDBO()
                    {
                        KmRangoInicial = this.vista.KmLibres +1,
                        KmRangoFinal = null,
                        CargoKm = this.vista.TarifaKmAdicional,
                        HrRangoInicial = this.vista.HorasLibres +1,
                        HrRangoFinal = null,
                        CargoHr = this.vista.TarifaHrAdicional
                    };
                    tarifa.RangoTarifas.Add(rangoTarifa);
                }

                return tarifa;
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".InterfazUsuarioADato:Error al intentar obtener las tarifas." + ex.Message);
            }
        }
        public void ModoConsulta(bool activo)
        {
            this.vista.ModoEdicion(false);
        }
        #endregion
    }
}
