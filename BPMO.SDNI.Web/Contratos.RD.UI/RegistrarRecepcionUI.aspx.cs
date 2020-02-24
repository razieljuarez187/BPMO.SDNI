//Satisface al CU007 - Registrar entrega recepción de unidad
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Contratos.RD.BO;
using BPMO.SDNI.Contratos.RD.PRE;
using BPMO.SDNI.Contratos.RD.VIS;
using BPMO.SDNI.Equipos.BO;
using BPMO.SDNI.MapaSitio.UI;

namespace BPMO.SDNI.Contratos.RD.UI
{
    /// <summary>
    /// Página para registrar la recepción de la unidad
    /// </summary>
    public partial class RegistrarRecepcionUI : System.Web.UI.Page, IRegistrarRecepcionVIS
    {
        #region Atributos
        /// <summary>
        /// Presentador de la página
        /// </summary>
        private RegistrarRecepcionPRE presentador = null;
        /// <summary>
        /// Nombre de la clase para usar en los mensajes
        /// </summary>
        private const string nombreClase = "RegistrarRecepcionUnidadUI";
        #endregion

        #region Propiedades
        /// <summary>
        /// Obtiene el usuario autenticado en el sistema
        /// </summary>
        public int? UsuarioID
        {
            get
            {
                Site masterMsj = (Site)Page.Master;

                return masterMsj != null && masterMsj.Usuario != null ? masterMsj.Usuario.Id : null;
            }
        }
        /// <summary>
        /// Obtiene la Unidad operativa a la cual pertenece el usuario autenticado
        /// </summary>
        public int? UnidadOperativaID
        {
            get
            {
                Site masterMsj = (Site)Page.Master;
                return masterMsj != null && masterMsj.Adscripcion != null && masterMsj.Adscripcion.UnidadOperativa != null
                           ? masterMsj.Adscripcion.UnidadOperativa.Id
                           : null;
            }
        }
        //Administración del Wizard
        /// <summary>
        /// Obtiene la página actual en la que se esta trabajando
        /// </summary>
        public int? PaginaActual
        {
            get
            {
                int val;
                return !string.IsNullOrEmpty(this.hdnPaginaActual.Value) && !string.IsNullOrWhiteSpace(this.hdnPaginaActual.Value)
                           ? (Int32.TryParse(this.hdnPaginaActual.Value, out val) ? (int?)val : null)
                           : null;
            }
        }
        /// <summary>
        /// Obtiene o establece el nombre del usuario que recibe la unidad
        /// </summary>
        public string NombreUsuarioRecibe
        {
            get { return this.txtUsuarioRecibe.Text.Trim().ToUpper(); }
            set
            {
                this.txtUsuarioRecibe.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                                ? value.Trim().ToUpper()
                                                : string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o establece el nombre del usuario que entregó la unidad
        /// </summary>
        public string NombreUsuarioEntrega
        {
            get { return this.txtUsuarioEntrego.Text.Trim().ToUpper(); }
            set
            {
                this.txtUsuarioEntrego.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                                ? value.Trim().ToUpper()
                                                : string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o establece el identificador del contrato
        /// </summary>
        public int? ContratoID
        {
            get
            {
                int val;
                return !string.IsNullOrEmpty(this.hdnContratoID.Value) &&
                       !string.IsNullOrWhiteSpace(this.hdnContratoID.Value)
                           ? (Int32.TryParse(this.hdnContratoID.Value, out val) ? (int?)val : null)
                           : null;
            }
            set
            {
                this.hdnContratoID.Value = value.HasValue
                                               ? value.Value.ToString(CultureInfo.InvariantCulture)
                                               : string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o establece el número del contrato de renta diaría
        /// </summary>
        public string NumeroContrato
        {
            get { return this.txtNumeroContrato.Text.Trim().ToUpper(); }
            set
            {
                this.txtNumeroContrato.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                                  ? value.Trim().ToUpper()
                                                  : string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o establece el estatus del contrato
        /// </summary>
        public int? EstatusContratoID
        {
            get
            {
                int val;
                return !string.IsNullOrEmpty(this.hdnEstatusContratoID.Value) &&
                       !string.IsNullOrWhiteSpace(this.hdnEstatusContratoID.Value)
                           ? (Int32.TryParse(this.hdnEstatusContratoID.Value, out val) ? (int?)val : null)
                           : null;
            }
            set
            {
                this.hdnEstatusContratoID.Value = value.HasValue
                                               ? value.Value.ToString(CultureInfo.InvariantCulture)
                                               : string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o establece el identificador de la línea del contrato
        /// </summary>
        public int? LineaContratoID
        {
            get
            {
                int val;
                return !string.IsNullOrEmpty(this.hdnLineaContratoID.Value) &&
                       !string.IsNullOrWhiteSpace(this.hdnLineaContratoID.Value)
                           ? (Int32.TryParse(this.hdnLineaContratoID.Value, out val) ? (int?)val : null)
                           : null;
            }
            set
            {
                this.hdnLineaContratoID.Value = value.HasValue
                                               ? value.Value.ToString(CultureInfo.InvariantCulture)
                                               : string.Empty;
            }
        }
        /// <summary>
        /// Fecha en la que se ejecuta el contrato
        /// </summary>
        public DateTime? FechaContrato
        {
            get
            {
                if (!string.IsNullOrEmpty(this.hdnFechaContrato.Value) && !string.IsNullOrWhiteSpace(this.hdnFechaContrato.Value))
                {
                    DateTime date = new DateTime();
                    return DateTime.TryParse(this.hdnFechaContrato.Value, out date) ? (DateTime?)date : null;
                }
                return null;
            }
            set
            { this.hdnFechaContrato.Value = value.HasValue ? value.Value.ToString() : string.Empty; }
        }
        /// <summary>
        /// Obtiene o establece la hora del contrato de renta diaria
        /// </summary>
        public TimeSpan? HoraContrato
        {
            get
            {
                if (!string.IsNullOrEmpty(this.hdnHoraContrato.Value) && !string.IsNullOrWhiteSpace(this.hdnHoraContrato.Value))
                {
                    TimeSpan time;
                    return TimeSpan.TryParse(this.hdnHoraContrato.Value, out time) ? (TimeSpan?)time : null;
                }
                return null;
            }
            set { this.hdnHoraContrato.Value = value.HasValue ? value.Value.ToString() : string.Empty; }
        }
        /// <summary>
        /// Obtiene o establece el nombre del cliente
        /// </summary>
        public string NombreCliente
        {
            get { return this.txtNombreCliente.Text.Trim().ToUpper(); }
            set
            {
                this.txtNombreCliente.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                                ? value.Trim().ToUpper()
                                                : string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o establece el nombre del operador
        /// </summary>
        public string NombreOperador
        {
            get { return this.txtNombreOperador.Text.Trim().ToUpper(); }
            set
            {
                this.txtNombreOperador.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                                ? value.Trim().ToUpper()
                                                : string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o establece la licencía del operador
        /// </summary>
        public string NumeroLicencia
        {
            get { return this.txtNumeroLicencia.Text.Trim().ToUpper(); }
            set
            {
                this.txtNumeroLicencia.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                              ? value.Trim().ToUpper()
                                              : string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o establece el número de las placas federales de la unidad
        /// </summary>
        public string PlacasFederales
        {
            get { return this.txtPlacasFederales.Text.Trim().ToUpper(); }
            set
            {
                this.txtPlacasFederales.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                                 ? value.Trim().ToUpper()
                                                 : string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o establece el número de las placas estatales de la unidad
        /// </summary>
        public string PlacasEstatales
        {
            get { return this.txtPlacasEstatales.Text.Trim().ToUpper(); }
            set
            {
                this.txtPlacasEstatales.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                                 ? value.Trim().ToUpper()
                                                 : string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o establece el identificador de la unidad
        /// </summary>
        public int? UnidadID
        {
            get
            {
                if (!string.IsNullOrEmpty(this.hdnUnidadID.Value) && !string.IsNullOrWhiteSpace(this.hdnUnidadID.Value))
                {
                    int val;
                    return Int32.TryParse(this.hdnUnidadID.Value, out val) ? (int?)val : null;
                }
                return null;
            }
            set { this.hdnUnidadID.Value = value.HasValue ? value.Value.ToString() : string.Empty; }
        }
        /// <summary>
        /// obtiene o establece el identificador del equipo
        /// </summary>
        public int? EquipoID
        {
            get
            {
                if (!string.IsNullOrEmpty(this.hdnEquipoID.Value) && !string.IsNullOrWhiteSpace(this.hdnEquipoID.Value))
                {
                    int val;
                    return Int32.TryParse(this.hdnEquipoID.Value, out val) ? (int?)val : null;
                }
                return null;
            }
            set { this.hdnEquipoID.Value = value.HasValue ? value.Value.ToString() : string.Empty; }
        }
        /// <summary>
        /// Obtiene o establece el número de serie de la unidad que será rentada
        /// </summary>
        public string NumeroSerie
        {
            get { return this.txtNumeroSerie.Text.Trim().ToUpper(); }
            set
            {
                this.txtNumeroSerie.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                                ? value.Trim().ToUpper()
                                                : string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o establece el número económico de la unidad que será rentada
        /// </summary>
        public string NumeroEconomico
        {
            get { return this.txtNumeroEconomico.Text.Trim().ToUpper(); }
            set
            {
                this.txtNumeroEconomico.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                                 ? value.Trim().ToUpper()
                                                 : string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o establece la capacidad del tanque
        /// </summary>
        public decimal? CapacidadTanque
        {
            get
            {
                if (!string.IsNullOrEmpty(this.hdnCapacidadTanque.Value) && !string.IsNullOrWhiteSpace(this.hdnCapacidadTanque.Value))
                {
                    decimal val;
                    return Decimal.TryParse(this.hdnCapacidadTanque.Value, out val) ? (decimal?)val : null;
                }
                return null;
            }
            set { this.hdnCapacidadTanque.Value = value.HasValue ? value.Value.ToString() : string.Empty; }
        }
        /// <summary>
        /// Obtiene o establece el tipo de check list
        /// </summary>
        public int? TipoListado
        {
            get
            {
                int val;
                return !string.IsNullOrEmpty(this.hdnTipoCheckList.Value) &&
                       !string.IsNullOrWhiteSpace(this.hdnTipoCheckList.Value)
                           ? (Int32.TryParse(this.hdnTipoCheckList.Value, out val) ? (int?)val : null)
                           : null;
            }
            set { this.hdnTipoCheckList.Value = value.HasValue ? value.Value.ToString() : string.Empty; }
        }
        /// <summary>
        /// Obtiene o establece la fecha en que se realiza el check list
        /// </summary>
        public DateTime? FechaListado
        {
            get
            {
                if (!string.IsNullOrEmpty(this.txtFechaRecepcion.Text) && !string.IsNullOrWhiteSpace(this.txtFechaRecepcion.Text))
                {
                    DateTime date;
                    return DateTime.TryParse(this.txtFechaRecepcion.Text, out date)
                               ? (DateTime?)date
                               : null;
                }
                return null;
            }
            set
            {
                this.txtFechaRecepcion.Text = value.HasValue ? value.Value.ToShortDateString() : string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o estable la hora de salida de la unidad
        /// </summary>
        public TimeSpan? HoraListado
        {
            get
            {
                if (!string.IsNullOrEmpty(this.txtHoraRecepcion.Text) && !string.IsNullOrWhiteSpace(this.txtHoraRecepcion.Text))
                {
                    var time = DateTime.ParseExact(this.txtHoraRecepcion.Text, "hh:mm tt", CultureInfo.InvariantCulture);
                    return time.TimeOfDay;
                }
                return null;
            }
            set
            {
                if (value != null)
                {
                    DateTime d = new DateTime().Add(value.Value);
                    this.txtHoraRecepcion.Text = string.Format("{0:hh:mm tt}", d).ToUpper().Replace(".", "").Replace("A M", "AM").Replace("P M", "PM").Trim();
                }
                else
                    this.txtHoraRecepcion.Text = string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o establece el kilometraje de salida
        /// </summary>
        public int? Kilometraje
        {
            get
            {
                if (!string.IsNullOrEmpty(this.txtKilometraje.Text) && !string.IsNullOrWhiteSpace(this.txtKilometraje.Text))
                {
                    int val = 0;
                    return Int32.TryParse(this.txtKilometraje.Text, out val) ? (int?)val : null;
                }
                return null;
            }
            set
            {
                this.txtKilometraje.Text = value != null ? String.Format("{0:#,##0}", value) : String.Empty;
            }
        }
        /// <summary>
        /// Obtiene o establece las horas de refrigeración de salida
        /// </summary>
        public int? Horometro
        {
            get
            {
                if (!string.IsNullOrEmpty(this.txtHorasRefrigeracion.Text) && !string.IsNullOrWhiteSpace(this.txtHorasRefrigeracion.Text))
                {
                    int val = 0;
                    return Int32.TryParse(this.txtHorasRefrigeracion.Text, out val) ? (int?)val : null;
                }
                return null;
            }
            set
            {
                this.txtHorasRefrigeracion.Text = value != null ? String.Format("{0:#,##0}", value) : String.Empty;
            }
        }
        /// <summary>
        /// Obtiene o establece el combustible de salida
        /// </summary>
        public int? Combustible
        {
            get
            {
                if (!string.IsNullOrEmpty(this.txtCombustible.Text) && !string.IsNullOrWhiteSpace(this.txtCombustible.Text))
                {
                    int val = 0;
                    return Int32.TryParse(this.txtCombustible.Text, out val) ? (int?)val : null;
                }
                return null;
            }
            set
            {
                this.txtCombustible.Text = value != null ? String.Format("{0:#,##0}", value) : String.Empty;
            }
        }
        /// <summary>
        /// Obtiene o establece si la unidad tiene golpes en general
        /// </summary>
        public bool? TieneGolpesGeneral
        {
            get
            {
                bool val;
                return Boolean.TryParse(this.rbtGolpesGeneral.SelectedValue, out val) ? (bool?)val : null;
            }
            set
            {
                if (value.HasValue)
                {
                    this.rbtGolpesGeneral.SelectedValue = value.Value ? Boolean.TrueString : Boolean.FalseString;
                }
            }
        }
        /// <summary>
        /// Obtiene o establece si la documentacion esta completa
        /// </summary>
        public bool? TieneDocumentacionCompleta
        {
            get
            {
                bool val;
                return Boolean.TryParse(this.rbtDocumentacionCompleta.SelectedValue, out val) ? (bool?)val : null;
            }
            set
            {
                if (value.HasValue)
                {
                    this.rbtDocumentacionCompleta.SelectedValue = value.Value ? Boolean.TrueString : Boolean.FalseString;
                }
            }
        }
        /// <summary>
        /// Obtiene o establece si la unidad cuenta con un interior limpio
        /// </summary>
        public bool? TieneInteriorLimpio
        {
            get
            {
                bool val;
                return Boolean.TryParse(this.rbtInteriorLimpio.SelectedValue, out val) ? (bool?)val : null;
            }
            set
            {
                if (value.HasValue)
                {
                    this.rbtInteriorLimpio.SelectedValue = value.Value ? Boolean.TrueString : Boolean.FalseString;
                }
            }
        }
        /// <summary>
        /// Obtiene o establece si la unidad cuenta con vestiduras limpias
        /// </summary>
        public bool? TieneVestidurasLimpias
        {
            get
            {
                bool val;
                return Boolean.TryParse(this.rbtVestidurasLimpias.SelectedValue, out val) ? (bool?)val : null;
            }
            set
            {
                if (value.HasValue)
                {
                    this.rbtVestidurasLimpias.SelectedValue = value.Value ? Boolean.TrueString : Boolean.FalseString;
                }
            }
        }
        /// <summary>
        /// Obtiene o establece si la undiad cuenta con tapetes
        /// </summary>
        public bool? TieneTapetes
        {
            get
            {
                bool val;
                return Boolean.TryParse(this.rbtTapetes.SelectedValue, out val) ? (bool?)val : null;
            }
            set
            {
                if (value.HasValue)
                {
                    this.rbtTapetes.SelectedValue = value.Value ? Boolean.TrueString : Boolean.FalseString;
                }
            }
        }
        /// <summary>
        /// Obtiene o establece si la unidad cuenta con llave original
        /// </summary>
        public bool? TieneLlaveOriginal
        {
            get
            {
                bool val;
                return Boolean.TryParse(this.rbtLLaveOriginal.SelectedValue, out val) ? (bool?)val : null;
            }
            set
            {
                if (value.HasValue)
                {
                    this.rbtLLaveOriginal.SelectedValue = value.Value ? Boolean.TrueString : Boolean.FalseString;
                }
            }
        }
        /// <summary>
        /// Obtiene o establece si la unidad cuenta con encendedor        
        /// </summary>
        public bool? TieneEncendedor
        {
            get
            {
                bool val;
                return Boolean.TryParse(this.rbtEncendedor.SelectedValue, out val) ? (bool?)val : null;
            }
            set
            {
                if (value.HasValue)
                {
                    this.rbtEncendedor.SelectedValue = value.Value ? Boolean.TrueString : Boolean.FalseString;
                }
            }
        }
        /// <summary>
        /// Obtiene o establece si la unidad tiene Estereo o bocinas
        /// </summary>
        public bool? TieneStereoBocinas
        {
            get
            {
                bool val;
                return Boolean.TryParse(this.rbtEstereoBocinas.SelectedValue, out val) ? (bool?)val : null;
            }
            set
            {
                if (value.HasValue)
                {
                    this.rbtEstereoBocinas.SelectedValue = value.Value ? Boolean.TrueString : Boolean.FalseString;
                }
            }
        }
        /// <summary>
        /// Obtiene o establece si tiene alarma de reversa
        /// </summary>
        public bool? TieneAlarmaReversa
        {
            get
            {
                bool val;
                return Boolean.TryParse(this.rbtAlarmasReversa.SelectedValue, out val) ? (bool?)val : null;
            }
            set
            {
                if (value.HasValue)
                {
                    this.rbtAlarmasReversa.SelectedValue = value.Value ? Boolean.TrueString : Boolean.FalseString;
                }
            }
        }
        /// <summary>
        /// Obtiene o establece si tiene extinguidor
        /// </summary>
        public bool? TieneExtinguidor
        {
            get
            {
                bool val;
                return Boolean.TryParse(this.rbtExtinguidor.SelectedValue, out val) ? (bool?)val : null;
            }
            set
            {
                if (value.HasValue)
                {
                    this.rbtExtinguidor.SelectedValue = value.Value ? Boolean.TrueString : Boolean.FalseString;
                }
            }
        }
        /// <summary>
        /// Obtiene o establece si la unidad cuenta con llave de tuerca
        /// </summary>
        public bool? TieneGatoLlaveTuerca
        {
            get
            {
                bool val;
                return Boolean.TryParse(this.rbtGato.SelectedValue, out val) ? (bool?)val : null;
            }
            set
            {
                if (value.HasValue)
                {
                    this.rbtGato.SelectedValue = value.Value ? Boolean.TrueString : Boolean.FalseString;
                }
            }
        }
        /// <summary>
        /// Obtiene o establece si al unidad tiene tres reflejantes
        /// </summary>
        public bool? TieneTresReflejantes
        {
            get
            {
                bool val;
                return Boolean.TryParse(this.rbtTresReflejantes.SelectedValue, out val) ? (bool?)val : null;
            }
            set
            {
                if (value.HasValue)
                {
                    this.rbtTresReflejantes.SelectedValue = value.Value ? Boolean.TrueString : Boolean.FalseString;
                }
            }
        }
        /// <summary>
        /// Obtiene o establece si la unidad tiene lso espejos completos
        /// </summary>
        public bool? TieneEspejosCompletos
        {
            get
            {
                bool val;
                return Boolean.TryParse(this.rbtEspejosCompletos.SelectedValue, out val) ? (bool?)val : null;
            }
            set
            {
                if (value.HasValue)
                {
                    this.rbtEspejosCompletos.SelectedValue = value.Value ? Boolean.TrueString : Boolean.FalseString;
                }
            }
        }
        /// <summary>
        /// Ontiene o establece si el interior de la caja de la unidad esta limpio
        /// </summary>
        public bool? TieneLimpiezaInteriorCaja
        {
            get
            {
                bool val;
                return Boolean.TryParse(this.rbtLimpiezaInteriorCaja.SelectedValue, out val) ? (bool?)val : null;
            }
            set
            {
                if (value.HasValue)
                {
                    this.rbtLimpiezaInteriorCaja.SelectedValue = value.Value ? Boolean.TrueString : Boolean.FalseString;
                }
            }
        }
        /// <summary>
        /// Obtiene o establece si la unidad cuenta con GPS
        /// </summary>
        public bool? TieneGPS
        {
            get
            {
                bool val;
                return Boolean.TryParse(this.rbtGPS.SelectedValue, out val) ? (bool?)val : null;
            }
            set
            {
                if (value.HasValue)
                {
                    this.rbtGPS.SelectedValue = value.Value ? Boolean.TrueString : Boolean.FalseString;
                }
            }
        }
        /// <summary>
        /// Obtiene o establece si las baterias de al unidad estan en buen estado
        /// </summary>
        public bool? BateriasCorrectas
        {
            get
            {
                bool val;
                return Boolean.TryParse(this.rbtBateriasCorrectas.SelectedValue, out val) ? (bool?)val : null;
            }
            set
            {
                if (value.HasValue)
                {
                    this.rbtBateriasCorrectas.SelectedValue = value.Value ? Boolean.TrueString : Boolean.FalseString;
                }
            }
        }
        /// <summary>
        /// Obtiene o establece las observaciones para la documentación completa
        /// </summary>
        public string ObservacionesDocumentacionCompleta
        {
            get { return this.txtObservacionesDocumentacion.Text.Trim().ToUpper(); }
            set
            {
                this.txtObservacionesDocumentacion.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                                ? value.Trim().ToUpper()
                                                : string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o establece las observaciones para las baterias
        /// </summary>
        public string ObservacionesBaterias
        {
            get { return this.txtObservacionesBaterias.Text.Trim().ToUpper(); }
            set
            {
                this.txtObservacionesBaterias.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                                ? value.Trim().ToUpper()
                                                : string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o estable las observaciones para las llantas
        /// </summary>
        public string ObservacionesLlantas
        {
            get { return this.txtObservacionesLlanta.Text.Trim().ToUpper(); }
            set
            {
                this.txtObservacionesLlanta.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                                ? value.Trim().ToUpper()
                                                : string.Empty;
            }
        }

        #region Información de Entrega
        /// <summary>
        /// Obtiene o establece el identificador del check list de entrega
        /// </summary>
        public int? CheckListEntregaID
        {
            get
            {
                if (!string.IsNullOrEmpty(this.hdnCheckListEntregaID.Value) && !string.IsNullOrWhiteSpace(this.hdnCheckListEntregaID.Value))
                {
                    int val = 0;
                    return Int32.TryParse(this.hdnCheckListEntregaID.Value, out val) ? (int?)val : null;
                }
                return null;
            }
            set
            {
                this.hdnCheckListEntregaID.Value = value.HasValue ? value.Value.ToString().Trim().ToUpper() : string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o establece la fecha en que se entego la undiad al cliente
        /// </summary>
        public DateTime? FechaListadoEntrega
        {
            get
            {
                if (!string.IsNullOrEmpty(this.txtFechaEntrega.Text) && !string.IsNullOrWhiteSpace(this.txtFechaEntrega.Text))
                {
                    DateTime date;
                    return DateTime.TryParse(this.txtFechaEntrega.Text, out date)
                               ? (DateTime?)date
                               : null;
                }
                return null;
            }
            set
            {
                this.txtFechaEntrega.Text = value.HasValue ? value.Value.ToShortDateString() : string.Empty;
            }
        }
        /// <summary>
        /// Obtiene y establece la hora en la que se entrego la unidad al cliente
        /// </summary>
        public TimeSpan? HoraListadoEntrega
        {
            get
            {
                if (!string.IsNullOrEmpty(this.txtHoraEntrega.Text) && !string.IsNullOrWhiteSpace(this.txtHoraEntrega.Text))
                {
                    var time = DateTime.ParseExact(this.txtHoraEntrega.Text, "hh:mm tt", CultureInfo.InvariantCulture);
                    return time.TimeOfDay;
                }
                return null;
            }
            set
            {
                if (value != null)
                {
                    DateTime d = new DateTime().Add(value.Value);
                    this.txtHoraEntrega.Text = string.Format("{0:hh:mm tt}", d).ToUpper().Replace(".", "").Replace("A M", "AM").Replace("P M", "PM").Trim();
                }
                else
                    this.txtHoraEntrega.Text = string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o establece el kilometraje con el que fue entregada la unidad
        /// </summary>
        public int? KilometrajeEntrega
        {
            get
            {
                if (!string.IsNullOrEmpty(this.txtKilometrajeEntrega.Text) && !string.IsNullOrWhiteSpace(this.txtKilometrajeEntrega.Text))
                {
                    int val = 0;
                    return Int32.TryParse(this.txtKilometrajeEntrega.Text, out val) ? (int?)val : null;
                }
                return null;
            }
            set
            {
                this.txtKilometrajeEntrega.Text = value.HasValue ? value.Value.ToString().Trim().ToUpper() : string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o establece las horas de refrigeración con la que se entrego la unidad
        /// </summary>
        public int? HorometroEntrega
        {
            get
            {
                if (!string.IsNullOrEmpty(this.txtHorasRefrigeracionEntrega.Text) && !string.IsNullOrWhiteSpace(this.txtHorasRefrigeracionEntrega.Text))
                {
                    int val = 0;
                    return Int32.TryParse(this.txtHorasRefrigeracionEntrega.Text, out val) ? (int?)val : null;
                }
                return null;
            }
            set
            {
                this.txtHorasRefrigeracionEntrega.Text = value.HasValue ? value.Value.ToString().Trim().ToUpper() : string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o establece el combustible de entrega de la unidad
        /// </summary>
        public int? CombustibleEntrega
        {
            get
            {
                if (!string.IsNullOrEmpty(this.txtCombustibleEntrega.Text) && !string.IsNullOrWhiteSpace(this.txtCombustibleEntrega.Text))
                {
                    int val = 0;
                    return Int32.TryParse(this.txtCombustibleEntrega.Text, out val) ? (int?)val : null;
                }
                return null;
            }
            set
            {
                this.txtCombustibleEntrega.Text = value.HasValue ? value.Value.ToString().Trim().ToUpper() : string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o establece si la unidad tiene golpes en general al momento de la entrega al cliente
        /// </summary>
        public bool? TieneGolpesGeneralEntrega
        {
            get
            {
                bool val;
                return Boolean.TryParse(this.rbtGolpesGeneralEntrega.SelectedValue, out val) ? (bool?)val : null;
            }
            set
            {
                if (value.HasValue)
                {
                    this.rbtGolpesGeneralEntrega.SelectedValue = value.Value ? Boolean.TrueString : Boolean.FalseString;
                }
            }
        }
        /// <summary>
        /// Obtiene o establece si la unidad cuenta con docuemtnación completa al momento de la entrega
        /// </summary>
        public bool? TieneDocumentacionCompletaEntrega
        {
            get
            {
                bool val;
                return Boolean.TryParse(this.rbtDocumentacionCompletaEntrega.SelectedValue, out val) ? (bool?)val : null;
            }
            set
            {
                if (value.HasValue)
                {
                    this.rbtDocumentacionCompletaEntrega.SelectedValue = value.Value ? Boolean.TrueString : Boolean.FalseString;
                }
            }
        }
        /// <summary>
        /// Obtiene o establece si la unidad tenía interior limpio al momento de su entrega
        /// </summary>
        public bool? TieneInteriorLimpioEntrega
        {
            get
            {
                bool val;
                return Boolean.TryParse(this.rbtInteriorLimpioEntrega.SelectedValue, out val) ? (bool?)val : null;
            }
            set
            {
                if (value.HasValue)
                {
                    this.rbtInteriorLimpioEntrega.SelectedValue = value.Value ? Boolean.TrueString : Boolean.FalseString;
                }
            }
        }
        /// <summary>
        /// Obtiene o establece si la undidad cuenta con vestiduras limpias la momento de la entrega al cliente
        /// </summary>
        public bool? TieneVestidurasLimpiasEntrega
        {
            get
            {
                bool val;
                return Boolean.TryParse(this.rbtVestidurasLimpiasEntrega.SelectedValue, out val) ? (bool?)val : null;
            }
            set
            {
                if (value.HasValue)
                {
                    this.rbtVestidurasLimpiasEntrega.SelectedValue = value.Value ? Boolean.TrueString : Boolean.FalseString;
                }
            }
        }
        /// <summary>
        /// Obtiene o establece si la unidad cuenta con tapetes al momento de la entrega al cliente
        /// </summary>
        public bool? TieneTapetesEntrega
        {
            get
            {
                bool val;
                return Boolean.TryParse(this.rbtTapetesEntrega.SelectedValue, out val) ? (bool?)val : null;
            }
            set
            {
                if (value.HasValue)
                {
                    this.rbtTapetesEntrega.SelectedValue = value.Value ? Boolean.TrueString : Boolean.FalseString;
                }
            }
        }
        /// <summary>
        /// Obtiene o establece si la unidad cuenta con llave original al momento de la entrega al cliente
        /// </summary>
        public bool? TieneLlaveOriginalEntrega
        {
            get
            {
                bool val;
                return Boolean.TryParse(this.rbtLLaveOriginalEntrega.SelectedValue, out val) ? (bool?)val : null;
            }
            set
            {
                if (value.HasValue)
                {
                    this.rbtLLaveOriginalEntrega.SelectedValue = value.Value ? Boolean.TrueString : Boolean.FalseString;
                }
            }
        }
        /// <summary>
        /// Obtiene o establece si la unidad tiene encendedor al momento de la entrega al cliente
        /// </summary>
        public bool? TieneEncendedorEntrega
        {
            get
            {
                bool val;
                return Boolean.TryParse(this.rbtEncendedorEntrega.SelectedValue, out val) ? (bool?)val : null;
            }
            set
            {
                if (value.HasValue)
                {
                    this.rbtEncendedorEntrega.SelectedValue = value.Value ? Boolean.TrueString : Boolean.FalseString;
                }
            }
        }
        /// <summary>
        /// Obtiene o establece si la unidad tiene estereo y bocinas al momento de la entrega al cliente
        /// </summary>
        public bool? TieneStereoBocinasEntrega
        {
            get
            {
                bool val;
                return Boolean.TryParse(this.rbtEstereoBocinasEntrega.SelectedValue, out val) ? (bool?)val : null;
            }
            set
            {
                if (value.HasValue)
                {
                    this.rbtEstereoBocinasEntrega.SelectedValue = value.Value ? Boolean.TrueString : Boolean.FalseString;
                }
            }
        }
        /// <summary>
        /// Obtiene o establece si la unidad tiene alarma de reversa al momento de la entrega al cliente
        /// </summary>
        public bool? TieneAlarmaReversaEntrega
        {
            get
            {
                bool val;
                return Boolean.TryParse(this.rbtAlarmasReversaEntrega.SelectedValue, out val) ? (bool?)val : null;
            }
            set
            {
                if (value.HasValue)
                {
                    this.rbtAlarmasReversaEntrega.SelectedValue = value.Value ? Boolean.TrueString : Boolean.FalseString;
                }
            }
        }
        /// <summary>
        /// Obtiene o establece si la unidad tiene extinguidor al momento de la entrega al cliente
        /// </summary>
        public bool? TieneExtinguidorEntrega
        {
            get
            {
                bool val;
                return Boolean.TryParse(this.rbtExtinguidorEntrega.SelectedValue, out val) ? (bool?)val : null;
            }
            set
            {
                if (value.HasValue)
                {
                    this.rbtExtinguidorEntrega.SelectedValue = value.Value ? Boolean.TrueString : Boolean.FalseString;
                }
            }
        }
        /// <summary>
        /// Obtiene o establece si la unidad tiene gato, llave y tuerca al momento de la entrega al cliente
        /// </summary>
        public bool? TieneGatoLlaveTuercaEntrega
        {
            get
            {
                bool val;
                return Boolean.TryParse(this.rbtGatoEntrega.SelectedValue, out val) ? (bool?)val : null;
            }
            set
            {
                if (value.HasValue)
                {
                    this.rbtGatoEntrega.SelectedValue = value.Value ? Boolean.TrueString : Boolean.FalseString;
                }
            }
        }
        /// <summary>
        /// Obtiene o establece si la unidad tiene tres reflejantes al momento de la entrega al cliente
        /// </summary>
        public bool? TieneTresReflejantesEntrega
        {
            get
            {
                bool val;
                return Boolean.TryParse(this.rbtTresReflejantesEntrega.SelectedValue, out val) ? (bool?)val : null;
            }
            set
            {
                if (value.HasValue)
                {
                    this.rbtTresReflejantesEntrega.SelectedValue = value.Value ? Boolean.TrueString : Boolean.FalseString;
                }
            }
        }
        /// <summary>
        /// Obtiene o establece si la unidad tiene los espejos completos al momento de la entrega al cliente
        /// </summary>
        public bool? TieneEspejosCompletosEntrega
        {
            get
            {
                bool val;
                return Boolean.TryParse(this.rbtEspejosCompletosEntrega.SelectedValue, out val) ? (bool?)val : null;
            }
            set
            {
                if (value.HasValue)
                {
                    this.rbtEspejosCompletosEntrega.SelectedValue = value.Value ? Boolean.TrueString : Boolean.FalseString;
                }
            }
        }
        /// <summary>
        /// Obtiene o establece si la caja de la unidad tiene el interior limpio al momento de la entrega al cliente
        /// </summary>
        public bool? TieneLimpiezaInteriorCajaEntrega
        {
            get
            {
                bool val;
                return Boolean.TryParse(this.rbtLimpiezaInteriorCajaEntrega.SelectedValue, out val) ? (bool?)val : null;
            }
            set
            {
                if (value.HasValue)
                {
                    this.rbtLimpiezaInteriorCajaEntrega.SelectedValue = value.Value ? Boolean.TrueString : Boolean.FalseString;
                }
            }
        }
        /// <summary>
        /// Obtiene o establece si la unidad cuenta con GPS al momento de la entrega al cliente
        /// </summary>
        public bool? TieneGPSEntrega
        {
            get
            {
                bool val;
                return Boolean.TryParse(this.rbtGPSEntrega.SelectedValue, out val) ? (bool?)val : null;
            }
            set
            {
                if (value.HasValue)
                {
                    this.rbtGPSEntrega.SelectedValue = value.Value ? Boolean.TrueString : Boolean.FalseString;
                }
            }
        }
        /// <summary>
        /// Obtiene o establece si la unidad cuenta con baterias correctas al momento de la entrega al cliente
        /// </summary>
        public bool? BateriasCorrectasEntrega
        {
            get
            {
                bool val;
                return Boolean.TryParse(this.rbtBateriasCorrectasEntrega.SelectedValue, out val) ? (bool?)val : null;
            }
            set
            {
                if (value.HasValue)
                {
                    this.rbtBateriasCorrectasEntrega.SelectedValue = value.Value ? Boolean.TrueString : Boolean.FalseString;
                }
            }
        }
        /// <summary>
        /// Obtiene o establece las observaciones a la documetnación la momento de la entrega al cliente
        /// </summary>
        public string ObservacionesDocumentacionCompletaEntrega
        {
            get { return this.txtObservacionDocumentacionEntrega.Text.Trim().ToUpper(); }
            set
            {
                this.txtObservacionDocumentacionEntrega.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                                ? value.Trim().ToUpper()
                                                : string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o establece las observaciones a las baterias al momento de la entrega al cliente
        /// </summary>
        public string ObservacionesBateriasEntrega
        {
            get { return this.txtObservacionesBateriasEntrega.Text.Trim().ToUpper(); }
            set
            {
                this.txtObservacionesBateriasEntrega.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                                ? value.Trim().ToUpper()
                                                : string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o establece las observaciones a las llantas al momento de la entrega al cliente
        /// </summary>
        public string ObservacionesLlantasEntrega
        {
            get { return this.txtObservacionesLlantaEntrega.Text.Trim().ToUpper(); }
            set
            {
                this.txtObservacionesLlantaEntrega.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                                ? value.Trim().ToUpper()
                                                : string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o establece las observaciones realizadas a las secciones de la unidad al momento de la entrega
        /// </summary>
        public List<object> VerificacionesSeccionEntrega
        {
            get
            {
                List<VerificacionSeccionBO> listValue = Session["VerificacionesSeccionEntrega"] != null
                                     ? ((List<object>)Session["VerificacionesSeccionEntrega"]).ConvertAll(x => (VerificacionSeccionBO)x)
                                     : new List<VerificacionSeccionBO>();

                return listValue.ConvertAll(x => (object)x);
            }
            set { Session["VerificacionesSeccionEntrega"] = value ?? (new List<VerificacionSeccionBO>()).ConvertAll(x => (object)x); }
        }
        /// <summary>
        /// Obtiene o establece las observaciones realizadas a las llantas al momento de la entrega
        /// </summary>
        public List<object> VerificacionesLlantaEntrega
        {
            get
            {
                List<object> listValue = Session["VerificacionesLlantaEntrega"] != null
                                     ? ((List<object>) Session["VerificacionesLlantaEntrega"])
                                     : new List<VerificacionLlantaBO>().ConvertAll(x => (object)x);

                return listValue.ConvertAll(x => (object)x);
            }
            set
            {
                Session["VerificacionesLlantaEntrega"] = value ?? (new List<VerificacionLlantaBO>()).ConvertAll(x => (object)x);
            }
        }
        /// <summary>
        /// Obtiene o establece las imagenes de las secciones registradas al momento de la entrega
        /// </summary>
        public List<object> ImagenesSecciones
        {
            get
            {
                List<object> listValue = Session["ImagenesSecciones"] != null
                                     ? ((List<object>)Session["ImagenesSecciones"])
                                     : new List<ArchivoBO>().ConvertAll(x => (object)x);

                return listValue.ConvertAll(x => (object)x);
            }
            set
            {
                Session["ImagenesSecciones"] = value ?? (new List<ArchivoBO>()).ConvertAll(x => (object)x);
            }
        }
        /// <summary>
        /// Obtiene o establece el codigo de la refaccion en la entrega de la unidad
        /// </summary>
        public string RefaccionCodigoEntrega
        {
            get { return this.txtRefaccionEntrega.Text.Trim().ToUpper(); }
            set
            {
                this.txtRefaccionEntrega.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                              ? value.Trim().ToUpper()
                                              : string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o establece el estado de la llanta de la refacción en la entrega de la unidad
        /// </summary>
        public bool? RefaccionEstadoEntrega
        {
            get
            {
                bool val;
                return Boolean.TryParse(this.rbtRefaccionEntrega.SelectedValue, out val) ? (bool?)val : null;
            }
            set
            {
                if (value.HasValue)
                {
                    this.rbtRefaccionEntrega.SelectedValue = value.Value ? Boolean.TrueString : Boolean.FalseString;
                }
            }
        }
        /// <summary>
        /// Obtiene o establece el indice para el control del grid de secciones de entrega
        /// </summary>
        public int IndicePaginaSeccionesEntrega
        {
            get { return this.grdObservacionesSeccionesEntrega.PageIndex; }
            set { this.grdObservacionesSeccionesEntrega.PageIndex = value; }
        }
        /// <summary>
        /// Obtiene o establece el indice para el control del grid de las iamgenes de las secciones
        /// </summary>
        public int IndicePaginaImagenesSecciones
        {
            get { return this.grdImagenesSecciones.PageIndex; }
            set { this.grdImagenesSecciones.PageIndex = value; }
        }
        #endregion

        /// <summary>
        /// Obtiene o establece los listados de verificación de seccion
        /// </summary>
        public List<object> VerificacionesSeccion
        {
            get
            {
                List<VerificacionSeccionBO> listValue;
                listValue = Session["VerificacionesSeccion"] != null
                                ? ((List<object>)Session["VerificacionesSeccion"]).ConvertAll(x => (VerificacionSeccionBO)x)
                                : new List<VerificacionSeccionBO>();

                return listValue.ConvertAll(x => (object)x);
            }
            set { Session["VerificacionesSeccion"] = value ?? (new List<VerificacionSeccionBO>()).ConvertAll(x => (object)x); }
        }
        /// <summary>
        /// Obtiene o establece los listados de verificación de llanta
        /// </summary>
        public List<object> VerificacionesLlanta
        {
            get
            {
                List<VerificacionLlantaBO> listValue;
                listValue = Session["VerificacionesLlanta"] != null
                                ? (List<VerificacionLlantaBO>)Session["VerificacionesLlanta"]
                                : new List<VerificacionLlantaBO>();

                return listValue.ConvertAll(x => (object)x);
            }
            set
            {
                Session["VerificacionesLlanta"] = value ?? (new List<VerificacionLlantaBO>()).ConvertAll(x => (object)x);
            }
        }
        /// <summary>
        /// Obtiene o establece los archivos que se desean asociar al check list
        /// </summary>
        public object NuevosArchivos
        {
            get { return Session["NewObject"] as List<ArchivoBO>; }
            set { Session["NewObject"] = value; }
        }
        /// <summary>
        /// Obtiene o establece el identificador de la refacción
        /// </summary>
        public int? RefaccionID
        {
            get
            {
                if (!string.IsNullOrEmpty(this.hdnRecepcionRefaccionID.Value) && !string.IsNullOrWhiteSpace(this.hdnRecepcionRefaccionID.Value))
                {
                    int val = 0;
                    return Int32.TryParse(this.hdnRecepcionRefaccionID.Value, out val) ? (int?)val : null;
                }
                return null;
            }
            set
            {
                this.hdnRecepcionRefaccionID.Value = value.HasValue ? value.Value.ToString().Trim().ToUpper() : string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o establece el codigo de la refaccion
        /// </summary>
        public string RefaccionCodigo
        {
            get { return this.txtRefaccion.Text.Trim().ToUpper(); }
            set
            {
                this.txtRefaccion.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                              ? value.Trim().ToUpper()
                                              : string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o establece el estado de la llanta
        /// </summary>
        public bool? RefaccionEstado
        {
            get
            {
                bool val;
                return Boolean.TryParse(this.rbtRefaccionCorrecta.SelectedValue, out val) ? (bool?)val : null;
            }
            set
            {
                if (value.HasValue)
                {
                    this.rbtRefaccionCorrecta.SelectedValue = value.Value ? Boolean.TrueString : Boolean.FalseString;
                }
            }
        }
        /// <summary>
        /// Obtiene las secciones de las unidades configuradas
        /// </summary>
        public string SeccionesUnidades
        {
            get
            {
                string clave = ConfigurationManager.AppSettings["SeccionesUnidad"];
                if (string.IsNullOrEmpty(clave) || string.IsNullOrWhiteSpace(clave))
                    throw new Exception("Aun no se configuran las secciones de la Unidad. Es necesario configurarlas antes de continuar con el registro del check List.");
                return !string.IsNullOrEmpty(clave) && !string.IsNullOrWhiteSpace(clave) ? clave : string.Empty;
            }
        }
        /// <summary>
        /// Obtiene el nombre del archivo seleccionado en el fileupload
        /// </summary>
        public string NombreArchivoImagen
        {
            get { return this.uplArchivoImagen.HasFile ? this.uplArchivoImagen.FileName : string.Empty; }
        }
        /// <summary>
        /// Obtiene la extención del archivo seleccionado en el fileupload
        /// </summary>
        public string ExtencionArchivoImagen
        {
            get { return this.uplArchivoImagen.HasFile ? System.IO.Path.GetExtension(NombreArchivoImagen).Replace(".", "") : string.Empty; }
        }
        /// <summary>
        /// Obtiene la sección de la unidad seleccionada para la observación
        /// </summary>
        public int? SeccionUnidadID
        {
            get
            {
                if (this.ddlSeccionesunidad.SelectedValue.CompareTo("-1") != 0)
                {
                    int val = 0;
                    if (Int32.TryParse(this.ddlSeccionesunidad.SelectedValue, out val))
                        return val;
                }
                return null;
            }
        }
        /// <summary>
        /// Obtiene o establece la observación de la sección seleccionada
        /// </summary>
        public string ObservacionesSeccion
        {
            get { return this.txtObservacionesSeccion.Text.Trim().ToUpper(); }
            set
            {
                this.txtObservacionesSeccion.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                                        ? value.Trim().ToUpper()
                                                        : string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o establece los tipos de archivo de imagen
        /// </summary>
        public object TiposArchivoImagen
        {
            get
            {
                return Session["TiposArchivosImagen"] as List<TipoArchivoBO>;
            }
            set
            {
                Session["TiposArchivosImagen"] = value;
            }
        }
        /// <summary>
        /// Obtiene o establece el indice para el control del grid de secciones
        /// </summary>
        public int IndicePaginaSecciones
        {
            get { return this.grdObservacionesSecciones.PageIndex; }
            set { this.grdObservacionesSecciones.PageIndex = value; }
        }
        /// <summary>
        /// Indica la tolerancia en kilometros para la recepción
        /// </summary>
        public int? KilometrajeDiario {
            get {
                if (!String.IsNullOrEmpty(this.hdnKilometrajeDiario.Value))
                    return int.Parse(this.hdnKilometrajeDiario.Value);
                else
                    return null;
            }
            set {
                this.hdnKilometrajeDiario.Value = value.ToString();
            }
        }
        #endregion

        #region Constructores
        protected void Page_Load(object sender, EventArgs e)
        {
            try{
                this.txtObservacionesBaterias.Attributes.Add("onkeyup", "checkText(this,150);");
                this.txtObservacionesDocumentacion.Attributes.Add("onkeyup", "checkText(this,150);");
                this.txtObservacionesLlanta.Attributes.Add("onkeyup", "checkText(this,150);");
                this.txtObservacionesSeccion.Attributes.Add("onkeyup", "checkText(this,150);");
                this.presentador = new RegistrarRecepcionPRE(this, this.ucCatalogoDocumentos, this.ucucEquiposAliadosUnidadUI, this.UcCatalogoDocumentosUI1);
                if (!Page.IsPostBack)
                    this.presentador.PrepararNuevo();
            }
            catch (Exception ex)
            {
                this.ReestablecerControles();
                this.MostrarMensaje("Inconsistencia al crear la página", ETipoMensajeIU.ERROR, nombreClase + ".Page_Load:" + ex.Message);
            }
        }
        #endregion

        #region Métodos
        /// <summary>
        /// Este método despliega un mensaje en pantalla
        /// </summary>
        /// <param name="mensaje">Mensaje a desplegar</param>
        /// <param name="tipo">Tipo de mensaje a desplegar</param>
        /// <param name="detalle">Muestra una cadena de texto que contiene los datalles del mensaje a desplegar</param>
        public void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string msjDetalle = null)
        {
            var master = (Site)Master;
            if (tipo == ETipoMensajeIU.ERROR)
            {
                if (master != null) master.MostrarMensaje(mensaje, tipo, msjDetalle);
            }
            else
            {
                if (master != null) master.MostrarMensaje(mensaje, tipo);
            }
        }
        /// <summary>
        /// Elimina de la sessión todas las variables que se esten usando para el registro del check list
        /// </summary>
        public void LimpiarSesion()
        {
            Session.Remove("NewObject");
            Session.Remove("TiposArchivosImagen");
            Session.Remove("CheckListEntrega");
            Session.Remove("VerificacionesSeccion");
            Session.Remove("VerificacionesLlanta");
            Session.Remove("VerificacionesSeccionEntrega");
            Session.Remove("VerificacionesLlantaEntrega");
            Session.Remove("ImagenesSecciones");
            Session.Remove("NombreReporte");
            Session.Remove("DatosReporte");
            this.ucCatalogoDocumentos.LimpiarSesion();
            this.UcCatalogoDocumentosUI1.LimpiarSesion();
        }
        /// <summary>
        /// Limpia el contrato al cual se le esta registrando el check list
        /// </summary>
        public void LimpiarPaqueteNavegacion()
        {
            Session.Remove("RegistrarRecepcionUI");
        }
        /// <summary>
        /// Limpia la seción de observaciones para las secciones de la unidad
        /// </summary>
        public void LimpiarDatosSeccionUnidad()
        {
            this.ddlSeccionesunidad.SelectedIndex = 0;
            this.txtObservacionesSeccion.Text = string.Empty;
            this.NuevosArchivos = new List<ArchivoBO>();
            this.CargarImagenes();
        }
        /// <summary>
        /// Establece la configuración inicial para el registro del check list
        /// </summary>
        public void PrepararNuevo()
        {
            #region ReadOnly
            this.txtUsuarioEntrego.ReadOnly = true;
            this.txtUsuarioRecibe.ReadOnly = true;
            this.txtNumeroContrato.ReadOnly = true;
            this.txtNombreCliente.ReadOnly = true;
            this.txtNombreOperador.ReadOnly = true;
            this.txtNumeroLicencia.ReadOnly = true;
            this.txtNumeroEconomico.ReadOnly = true;
            this.txtNumeroSerie.ReadOnly = true;
            this.txtPlacasEstatales.ReadOnly = true;
            this.txtPlacasFederales.ReadOnly = true;

            this.txtCombustibleEntrega.ReadOnly = true;
            this.txtFechaEntrega.ReadOnly = true;
            this.txtHoraEntrega.ReadOnly = true;
            this.txtHorasRefrigeracionEntrega.ReadOnly = true;
            this.txtKilometrajeEntrega.ReadOnly = true;
            this.txtObservacionDocumentacionEntrega.ReadOnly = true;
            this.txtObservacionesBateriasEntrega.ReadOnly = true;

            this.txtRefaccionEntrega.ReadOnly = true;
            this.txtObservacionesLlantaEntrega.ReadOnly = true;
			this.txtFechaRecepcion.ReadOnly = true;
			txtHoraRecepcion.ReadOnly = true;
            #endregion

            #region Enabled
            this.txtUsuarioEntrego.Enabled = false;
            this.txtUsuarioRecibe.Enabled = false;
            this.txtNumeroContrato.Enabled = false;
            this.txtNombreCliente.Enabled = false;
            this.txtNombreOperador.Enabled = false;
            this.txtNumeroLicencia.Enabled = false;
            this.txtNumeroEconomico.Enabled = false;
            this.txtNumeroSerie.Enabled = false;
            this.txtPlacasEstatales.Enabled = false;
            this.txtPlacasFederales.Enabled = false;

            this.txtCombustibleEntrega.Enabled = false;
            this.txtFechaEntrega.Enabled = false;
            this.txtHoraEntrega.Enabled = false;
            this.txtHorasRefrigeracionEntrega.Enabled = false;
            this.txtKilometrajeEntrega.Enabled = false;

            this.rbtAlarmasReversaEntrega.Enabled = false;
            this.rbtBateriasCorrectasEntrega.Enabled = false;
            this.rbtDocumentacionCompletaEntrega.Enabled = false;
            this.rbtEncendedorEntrega.Enabled = false;
            this.rbtEspejosCompletosEntrega.Enabled = false;
            this.rbtEstereoBocinasEntrega.Enabled = false;
            this.rbtExtinguidorEntrega.Enabled = false;
            this.rbtGPSEntrega.Enabled = false;
            this.rbtGatoEntrega.Enabled = false;
            this.rbtGolpesGeneralEntrega.Enabled = false;
            this.rbtInteriorLimpioEntrega.Enabled = false;
            this.rbtLLaveOriginalEntrega.Enabled = false;
            this.rbtLimpiezaInteriorCajaEntrega.Enabled = false;            
            this.rbtTapetesEntrega.Enabled = false;
            this.rbtTresReflejantesEntrega.Enabled = false;
            this.rbtVestidurasLimpiasEntrega.Enabled = false;
            this.txtObservacionDocumentacionEntrega.Enabled = false;
            this.txtObservacionesBateriasEntrega.Enabled = false;

            this.txtRefaccionEntrega.Enabled = false;
            this.rbtRefaccionEntrega.Enabled = false;
            this.txtObservacionesLlantaEntrega.Enabled = false;
            this.txtRefaccion.Enabled = false;
			this.txtFechaRecepcion.Enabled = false;
			txtHoraRecepcion.Enabled = false;
            #endregion
        }

        public void PrepararCancelacion(bool estado)
        {
            this.dvNotaCancelar.Visible = estado;
        }
        /// <summary>
        /// Redirige aa los usuarios que no tienen permisos de ejecutar la acción
        /// </summary>
        public void RedirigirSinPermisoAcceso()
        {
            this.Response.Redirect(this.ResolveUrl("~/MapaSitio.UI/PaginaSinAccesoUI.aspx"), false);
        }
        /// <summary>
        /// Redirige a los usaurios a la página de consulta de Check List
        /// </summary>
        public void RedirigirAConsulta()
        {
            this.Response.Redirect(this.ResolveUrl("~/Contratos.RD.UI/ConsultarListadoVerificacionUI.aspx"), false);
        }
        /// <summary>
        /// Redirige a los usuarios a la página de imprimir el check List
        /// </summary>
        public void RedirigirAImprimir()
        {
            Response.Redirect("../Buscador.UI/VisorReporteUI.aspx", false);
        }
        /// <summary>
        /// Redirige a los usuarios a la pagina de cancelacipon de contratos
        /// </summary>
        public void RedirigirACancelarContrato()
        {
            Response.Redirect("../Contratos.RD.UI/CancelarContratoRDUI.aspx", false);
        }

        public void EstablecerPaqueteNavegacion(string key, object value)
        {
            Session["NombreReporte"] = key;
            Session["DatosReporte"] = value;
        }
        /// <summary>
        /// Guarda un objeto en session para el cancelar contrato
        /// </summary>
        /// <param name="key">Clave del paquete</param>
        /// <param name="value">Objeto que se desea guardar</param>
        public void EstablecerPaqueteNavegacionCancelar(string key, object value)
        {
            if(string.IsNullOrEmpty(key) || string.IsNullOrWhiteSpace(key))
                throw new Exception(nombreClase + ".EstablecerPaqueteNavegacionCancelar: No se ha establecido la clave para el paquete de navegación del contrato a cancelar. ");
            Session[key] = value;
        }        
        /// <summary>
        /// Obtiene de session el contrato al que se le desea registrar el check list
        /// </summary>
        /// <returns>Contrato al que se le registra el check list</returns>
        public object ObtenerPaqueteContrato()
        {
            return Session["RegistrarRecepcionUI"] ?? null;
        }
        /// <summary>
        /// Despleiga en la UI las llantas de la unidad
        /// </summary>
        /// <param name="unidad">Unidad que contiene las llantas</param>
        public void CargarLlantas(object unidad)
        {
            this.grdLlantas.DataSource = ((UnidadBO)unidad).Llantas;
            this.grdLlantas.DataBind();
        }
        /// <summary>
        /// Habilita el control de captura para la reffacción en la sección de llantas
        /// </summary>
        /// <param name="estado">Estado del control</param>
        public void HabilitarRefaccion(bool estado)
        {
            this.rbtRefaccionCorrecta.Enabled = estado;
        }
        /// <summary>
        /// Establece las opciones configuradas para las observaciones de las secciones de la unidad
        /// </summary>
        /// <param name="opciones">Listado de opciones configuradas</param>
        public void EstablecerOpcionesSeccion(Dictionary<int, string> opciones)
        {
            if (ReferenceEquals(opciones, null))
                opciones = new Dictionary<int, string>();
            this.ddlSeccionesunidad.DataSource = opciones;
            this.ddlSeccionesunidad.DataValueField = "key";
            this.ddlSeccionesunidad.DataTextField = "value";
            this.ddlSeccionesunidad.DataBind();
        }
        /// <summary>
        /// Obtiene los valores seleccionados para las llantas
        /// </summary>
        /// <returns>Listado de Verificaciones de llanta</returns>
        public object ObtenerValoresLlanta()
        {
            List<VerificacionLlantaBO> objs = new List<VerificacionLlantaBO>();

            foreach (GridViewRow row in this.grdLlantas.Rows)
            {
                VerificacionLlantaBO obj = new VerificacionLlantaBO();

                Label llantaID = row.FindControl("lblLlantaID") as Label;
                if (llantaID != null)
                {
                    int id;
                    if (Int32.TryParse(llantaID.Text, out id))
                    {
                        obj.Llanta = new LlantaBO { LlantaID = id };
                    }
                }

                RadioButtonList rbtList = row.FindControl("rbtEstadoLlanta") as RadioButtonList;
                if (rbtList != null)
                {
                    bool val;
                    obj.Correcto = Boolean.TryParse(rbtList.SelectedValue, out val) ? (bool?)val : null;
                }

                objs.Add(obj);
            }

            return objs;
        }
        /// <summary>
        /// Obtiene el arreglo e bytes que corresponden a la imagén que se desea subir al servidor
        /// </summary>
        /// <returns>Arreglo de bytes de la iamgen seleccionada</returns>
        public byte[] ObtenerArchivosBytes()
        {
            return this.uplArchivoImagen.HasFile ? this.uplArchivoImagen.FileBytes : null;
        }
        /// <summary>
        /// Despliega en pantalla las iamgenes asociadas a las secciones de la unidad
        /// </summary>
        public void CargarImagenes()
        {
            this.grdArchivosImagen.DataSource = this.NuevosArchivos;
            this.grdArchivosImagen.DataBind();
        }
        /// <summary>
        /// Limpia el contenido del control uploadfile
        /// </summary>
        public void LimpiarUploadFile()
        {
            this.uplArchivoImagen.Dispose();
            this.uplArchivoImagen.PostedFile.InputStream.Dispose();
        }
        /// <summary>
        /// Despliega las observacionee a las llantas al momento de la entrega de la unidad
        /// </summary>
        public void CargarLlantasEntrega()
        {
            this.grdLlantasEntrega.DataSource = this.VerificacionesLlantaEntrega;
            this.grdLlantasEntrega.DataBind();
        }
        /// <summary>
        /// Despleiga las imagenes de las observaciones de entrega a la unidad
        /// </summary>
        public void CargarImagenesSecciones()
        {
            this.grdImagenesSecciones.DataSource = this.ImagenesSecciones;
            this.grdImagenesSecciones.DataBind();
        }
        /// <summary>
        /// Actualiza el resultado en el grid de observaciones de seccción
        /// </summary>
        public void ActualizarImagenesSecciones()
        {
            this.grdImagenesSecciones.DataSource = this.ImagenesSecciones;
            this.grdImagenesSecciones.PageIndex = this.IndicePaginaImagenesSecciones;
            this.grdImagenesSecciones.DataBind();
        }
        /// <summary>
        /// Despliega las observaciones de entrega de la unidad
        /// </summary>
        public void CargarSeccionesEntrega()
        {
            this.grdObservacionesSeccionesEntrega.DataSource = this.VerificacionesSeccionEntrega;
            this.grdObservacionesSeccionesEntrega.DataBind();           
        }
        /// <summary>
        /// Actualiza el resultado en el grid de observaciones de seccción de entrega
        /// </summary>
        public void ActualizarSeccionesEntrega()
        {
            this.grdObservacionesSeccionesEntrega.DataSource = this.VerificacionesSeccionEntrega;
            this.grdObservacionesSeccionesEntrega.PageIndex = this.IndicePaginaSeccionesEntrega;
            this.grdObservacionesSeccionesEntrega.DataBind();
        }
        /// <summary>
        /// Despliega las observaciones de recepción de la unidad
        /// </summary>
        public void CargarSeccionesVerificacion()
        {
            this.grdObservacionesSecciones.DataSource = this.VerificacionesSeccion;
            this.grdObservacionesSecciones.DataBind();
        }
        /// <summary>
        /// Actualiza el resultado en el grid de observaciones de seccción
        /// </summary>
        public void ActualizarSecciones()
        {
            this.grdObservacionesSecciones.DataSource = this.VerificacionesSeccion;
            this.grdObservacionesSecciones.PageIndex = this.IndicePaginaSecciones;
            this.grdObservacionesSecciones.DataBind();
        }
        /// <summary>
        /// Establece el número de página
        /// </summary>
        /// <param name="numeroPagina">Numero de página</param>
        public void EstablecerPagina(int numeroPagina)
        {
            this.mvCU077.SetActiveView((View)this.mvCU077.FindControl("vwPagina" + numeroPagina.ToString()));
            this.hdnPaginaActual.Value = numeroPagina.ToString();
        }
        /// <summary>
        /// Habilita o deshabilita el botón de regresar
        /// </summary>
        /// <param name="habilitar">Estado</param>
        public void PermitirRegresar(bool habilitar)
        {
            this.btnAnterior.Enabled = habilitar;
        }
        /// <summary>
        /// Habilita o deshabilita el botón de continuar
        /// </summary>
        /// <param name="habilitar">Estado</param>
        public void PermitirContinuar(bool habilitar)
        {
            this.btnContinuar.Enabled = habilitar;
        }
        /// <summary>
        /// Habilita o deshabilita el botón de cancelar registro
        /// </summary>
        /// <param name="habilitar">Estado</param>
        public void PermitirCancelar(bool habilitar)
        {
            this.btnCancelar.Enabled = habilitar;
        }
        /// <summary>
        /// Habilita o deshabilita el boton de guardar check list
        /// </summary>
        /// <param name="habilitar">Estado</param>
        public void PermitirGuardarTerminada(bool habilitar)
        {
            this.btnTerminar.Enabled = habilitar;
        }
        /// <summary>
        /// Hace visible o invisible el botón de continuar 
        /// </summary>
        /// <param name="ocultar">Estado</param>
        public void OcultarContinuar(bool ocultar)
        {
            this.btnContinuar.Visible = !ocultar;
        }
        /// <summary>
        /// Hace visible o invisible el botón de terminar
        /// </summary>
        /// <param name="ocultar">Estado</param>
        public void OcultarTerminar(bool ocultar)
        {
            this.btnTerminar.Visible = !ocultar;
        }
        /// <summary>
        /// Reestablece los botones para las acciones en caso de un fallo
        /// </summary>
        private void ReestablecerControles()
        {
            this.btnAnterior.Enabled = false;
            this.btnTerminar.Visible = false;
            this.btnTerminar.Enabled = false;
            this.btnContinuar.Enabled = false;
            this.btnCancelar.Enabled = true;
            this.dvNotaCancelar.Visible = false;
        }
        /// <summary>
        /// Despliega las imagenes de una sección de la unidad
        /// </summary>
        /// <param name="p">Listado de imagenes que se despliegan en el dialogo</param>
        public void CargarDetalleImagenSeccion(object p)
        {
            this.grdImagenesDialog.DataSource = (List<ArchivoBO>)p;
            this.grdImagenesDialog.DataBind();
        }

        #region SCXXXXX

        /// <summary>
        /// Registra Script en el cliente
        /// </summary>
        /// <param name="key">Key del script</param>
        /// <param name="script">script a registrar</param>
        private void RegistrarScript(string key, string script)
        {
            ScriptManager.RegisterStartupScript(Page, typeof(Page), key, script, true);
        }

        #endregion

        #endregion

        #region Eventos
        /// <summary>
        /// Avanza a la siguiente página
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnContinuar_Click(object sender, EventArgs e)
        {
            try
            {
                this.presentador.AvanzarPagina();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al cambiar de página", ETipoMensajeIU.ERROR, nombreClase + ".btnContinuar_Click:" + Environment.NewLine + ex.Message);
            }
        }
        /// <summary>
        /// Regresa a la página anterior
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAnterior_Click(object sender, EventArgs e)
        {
            try
            {
                this.presentador.RetrocederPagina();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al cambiar de página", ETipoMensajeIU.ERROR, nombreClase + ".btnAnterior_Click:" + Environment.NewLine + ex.Message);
            }
        }
        /// <summary>
        /// Redirige a una página en especifico
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBrincarPagina_Click(object sender, EventArgs e)
        {
            try
            {
                int numeroPagina = int.Parse(this.hdnPaginaBrinco.Value);
                this.presentador.IrAPagina(numeroPagina);
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al cambiar de página", ETipoMensajeIU.ERROR, nombreClase + ".btnBrincarPagina_Click:" + Environment.NewLine + ex.Message);
            }
        }
        /// <summary>
        /// Termina el registro del check list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnTerminar_Click(object sender, EventArgs e)
        {
            //bool bandera = false;
            try
            {
                bool redirigir = this.presentador.RegistrarRecepcion();
                if (redirigir == true)
                {
                    try
                    {
                        string OsID = this.presentador.RegistrarOrdenServicioLavado();
                        if (OsID != string.Empty)
                            this.LabelOsId.Text = OsID;
                        string origen = ((Button)sender).ID;
                        this.RegistrarScript("Exitoso", "confirmarLavadoExitoso('" + origen + "');");
                    }
                    catch (Exception ex)
                    {
                        this.LabelError.Text = ex.GetBaseException().Message;
                        string origen = ((Button)sender).ID;
                        this.RegistrarScript("Error", "confirmarLavadoError('" + origen +"');");
                    }
                    
                }
                
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al guardar el Check List.", ETipoMensajeIU.ERROR, nombreClase + ".btnGuardar_Click:" + Environment.NewLine + ex.Message);
                //bandera = true;
            }
            //if (bandera == false)
            //{
                
            //}
           
        }
        /// <summary>
        /// Canclea el registro del check list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            try
            {
                this.presentador.CancelarRegistro();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al cancelar el registro", ETipoMensajeIU.ERROR, nombreClase + ".btnCancelar_Click:" + Environment.NewLine + ex.Message);
            }
        }
        /// <summary>
        /// Agrega una observación para las secciones de la unidad
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAgregarSeccion_Click(object sender, EventArgs e)
        {
            try
            {
                this.presentador.AgregarVerificacionSeccion();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencias al tratar de agregar la imagén de la sección.", ETipoMensajeIU.ERROR, nombreClase + ".btnAgregarSeccion_Click" + Environment.NewLine + ex.Message);
            }
        }
        /// <summary>
        /// Agrega una imagen a la sección de la unidad
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAgregarImagen_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.uplArchivoImagen.HasFile)
                    this.presentador.AgregarArchivoImagen();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencias al tratar de agregar la imagén de la sección.", ETipoMensajeIU.ERROR, nombreClase + ".btnAgregarImagen_Click" + Environment.NewLine + ex.Message);
            }
        }
        /// <summary>
        /// Ejecuta una operación en el grid de secciones de entrega
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdObservacionesSeccionesEntrega_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                switch (e.CommandName.ToLower().Trim())
                {
                    case "detalle":
                        int index = 0;
                        if (Int32.TryParse(e.CommandArgument.ToString(), out index))
                        {
                            this.presentador.MostrarDetalleImagenesSeccionEntrega(index);
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), null, "DialogoDetalleImagenesSeccion();", true);
                        }
                        else this.MostrarMensaje("No se pueden desplegar los detalles de las imagenes para la sección", ETipoMensajeIU.ADVERTENCIA, null);
                        break;
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencias al momento de tratar de ejecutar una acción en el grid.", ETipoMensajeIU.ERROR, nombreClase + ".grdObservacionesSecciones_RowCommand :" + Environment.NewLine + ex.Message);
            }
        }
        /// <summary>
        /// Ejecuta una operación en le grid de observaciones a la sección
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdObservacionesSecciones_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                switch (e.CommandName.ToLower().Trim())
                {
                    case "eliminar":
                        int index = Convert.ToInt32(e.CommandArgument);
                        this.presentador.EliminarSeccion(index);
                        break;
                    case "detalle":
                        index = 0;
                        if (Int32.TryParse(e.CommandArgument.ToString(), out index))
                        {
                            this.presentador.MostrarDetalleImagenesSeccion(index);
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), null, "DialogoDetalleImagenesSeccion();", true);
                        }
                        else this.MostrarMensaje("No se pueden desplegar los detalles de las imagenes para la sección", ETipoMensajeIU.ADVERTENCIA, null);
                        break;
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencias al momento de intentar ejecutar una acción en el grid.", ETipoMensajeIU.ERROR, nombreClase + ".grdObservacionesSecciones_RowCommand :" + Environment.NewLine + ex.Message);
            }
        }
        /// <summary>
        /// Ejecuta uan operación en el grid de verificacion de llantas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdLlantasEntrega_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    VerificacionLlantaBO llanta = (VerificacionLlantaBO) e.Row.DataItem;

                    RadioButtonList rbtList = e.Row.FindControl("rbtEstadoLlanta") as RadioButtonList;
                    if (rbtList != null)
                    {
                        if (llanta.Correcto.HasValue)
                        {
                            rbtList.SelectedValue = llanta.Correcto.Value ? Boolean.TrueString : Boolean.FalseString;
                        }                        
                    }
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al momento de desplegar las llantas en el grid.", ETipoMensajeIU.ERROR, nombreClase + ".grdLlantasEntrega_RowDataBound" + Environment.NewLine + ex.Message);
            }
        }
        /// <summary>
        /// Maneja el cambio de indice del grid de archivos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdArchivos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                this.grdArchivosImagen.PageIndex = e.NewPageIndex;
                this.grdArchivosImagen.DataSource = this.NuevosArchivos;
                this.grdArchivosImagen.DataBind();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al momento de desplegar los resultados en el grid.", ETipoMensajeIU.ERROR, nombreClase + ".grdArchivos_PageIndexChanging: " + Environment.NewLine + ex.Message);
            }
        }
        /// <summary>
        /// Ejecuta una operación en el grid de archivos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdArchivos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                switch (e.CommandName.Trim())
                {
                    case "eliminar":
                    try
                    {

                        int index = Convert.ToInt32(e.CommandArgument);
                        this.presentador.EliminaArchivo(index);
                    }
                    catch (Exception ex)
                    {
                        MostrarMensaje("No fué posible eliminar el elemento", ETipoMensajeIU.ERROR, ex.Message);
                    }
                    break;
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al intentar ejecutar una acción en el Grid.", ETipoMensajeIU.ERROR, nombreClase + ".grdArchivos_RowCommand: " + Environment.NewLine + ex.Message);
            }
        }
        /// <summary>
        /// Carga inforamción en el grid de archivos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdArchivos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    ArchivoBO archivoBO = (ArchivoBO)e.Row.DataItem;
                    Label lblExtension = e.Row.FindControl("lblExtension") as Label;
                    if (lblExtension != null)
                    {
                        string extension = string.Empty;
                        if (archivoBO.TipoArchivo != null)
                            if (archivoBO.TipoArchivo.Extension != null)
                            {
                                extension = archivoBO.TipoArchivo.Extension;
                            }
                        lblExtension.Text = extension;
                    }

                    if (archivoBO.Id != null)
                    {
                        ImageButton imgBtn = (ImageButton)e.Row.FindControl("ibtDescargar");
                        imgBtn.OnClientClick =
                            "javascript:window.open('../Comun.UI/hdlrCatalogoDocumentos.ashx?archivoID=" + archivoBO.Id +
                            "'); return false;";
                    }

                    if (archivoBO.Activo == false)
                        e.Row.Attributes["style"] = "display:none";
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al momento de cargar los archivos en el grid.", ETipoMensajeIU.ERROR, nombreClase + ".grdArchivos_RowDataBound: " + Environment.NewLine + ex.Message);
            }
        }
        /// <summary>
        /// Evalua el kilometraje de recepción respecto al de entrega
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtKilometraje_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.Kilometraje.HasValue && this.KilometrajeEntrega.HasValue)
                {
                    string s;

                    //Validamos si se excede el kilometraje diario configurado
                    if (!string.IsNullOrEmpty(s = this.presentador.ValidarKilometrajeDiario())) 
                    {
                        this.MostrarMensaje(s, ETipoMensajeIU.ADVERTENCIA, null);
                        return;
                    }
                    //Validamos si por casualidad el registro debe redirigir a cancelar
                    if ((s = this.presentador.ValidarKilometraje()) != null)
                    {
                        this.MostrarMensaje(s, ETipoMensajeIU.ADVERTENCIA, null);
                        return;
                    }                    
                }
                this.txtHorasRefrigeracion.Focus();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al calcular el kilometraje.", ETipoMensajeIU.ERROR, nombreClase + ".txtKilometraje_TextChanged: " + Environment.NewLine + ex.Message);
            }
        }
        /// <summary>
        /// Ejecuta una operacion en el grid de imagenes de observaciones
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdImagenesSecciones_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    ArchivoBO archivoBO = (ArchivoBO) e.Row.DataItem;

                    if (archivoBO.Id != null)
                    {
                        ImageButton imgBtn = (ImageButton) e.Row.FindControl("ibtDescargar");
                        imgBtn.OnClientClick =
                            "javascript:window.open('../Comun.UI/hdlrCatalogoDocumentos.ashx?archivoID=" + archivoBO.Id +
                            "'); return false;";
                    }
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al momento de tratar de ejecutar una acción en el grid.", ETipoMensajeIU.ERROR, nombreClase + ".grdImagenesSecciones_RowDataBound: " + Environment.NewLine + ex.Message);
            }
        }
        /// <summary>
        /// Valida el combustible de entrega en base a la capacidad del tanque de la unidad
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtCombustible_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string s = this.presentador.ValidarTanque();
                if (!string.IsNullOrEmpty(s) && !string.IsNullOrWhiteSpace(s))
                {
                    this.txtCombustible.Text = string.Empty;
                    this.txtCombustible.Focus();
                    this.MostrarMensaje(s, ETipoMensajeIU.ADVERTENCIA, null);
                    this.txtCombustible.Focus();
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al momento de evaluar la cantidad de combustible.", ETipoMensajeIU.ERROR, string.Format("{0}.txtCombustible_TextChanged: {1}{2}", nombreClase, Environment.NewLine, ex.Message));
            }
        }
        /// <summary>
        /// Valida las horas refrigeradas de la unidad
        /// </summary>
        protected void txtHorasRefrigeracion_OnTextChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.HorometroEntrega.HasValue && this.Horometro.HasValue)
                {
                    string s;
                    
                    //Se valida que las horas esten correcta
                    if((s = this.presentador.ValidarKilometraje()) != null)
                    {
                        this.MostrarMensaje(s, ETipoMensajeIU.ADVERTENCIA, null);
                        return;
                    }
                }
                this.txtCombustible.Focus();
            }
            catch(Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al calcular las horas.", ETipoMensajeIU.ERROR, nombreClase + ".txtHorasRefrigeracion_OnTextChanged: " + Environment.NewLine + ex.Message);
            }
        }
        /// <summary>
        /// Cambia la página del grid de observaciones de recepción
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdObservacionesSecciones_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                this.presentador.CambiarPaginaSecciones(e.NewPageIndex);
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al cambiar de página", ETipoMensajeIU.ERROR, nombreClase + ".grdObservacionesSecciones_PageIndexChanging:" + ex.Message);
            }
        }
        /// <summary>
        /// Cambia la página del grid de observaciones de entrega
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdObservacionesSeccionesEntrega_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
            this.presentador.CambiarPaginaSeccionesEntrega(e.NewPageIndex);
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al cambiar de página", ETipoMensajeIU.ERROR, nombreClase + ".grdObservacionesSeccionesEntrega_PageIndexChanging:" + ex.Message);
            }
        }
        /// <summary>
        /// Cambia la página del grid de imagenes para las secciones
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdImagenesSecciones_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                this.presentador.CambiarPaginaImagenesSecciones(e.NewPageIndex);
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al cambiar de página", ETipoMensajeIU.ERROR, nombreClase + ".grdImagenesSecciones_PageIndexChanging:" + ex.Message);
            }
        }

        #region SCXXXXX

        protected void btnImprimir_Click(object sender, EventArgs e)
        {
           
            this.RedirigirAImprimir();
        }

        #endregion

        #endregion
    }
}