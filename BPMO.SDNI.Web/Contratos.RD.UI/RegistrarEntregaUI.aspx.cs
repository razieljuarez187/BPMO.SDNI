//Satisface al CU007 - Registrar entrega recepción de unidad
//Satisface a la solicitud de cambio SC0035
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
    public partial class RegistrarEntregaUI : System.Web.UI.Page, IRegistrarEntregaVIS
    {
        #region Atributos
        /// <summary>
        /// Presentador de la página
        /// </summary>
        private RegistrarEntregaPRE presentador = null;
        /// <summary>
        /// Nombre de la clase para usar en los mensajes
        /// </summary>
        private const string nombreClase = "RegistrarEntregaUnidadUI";
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
        /// <summary>
        /// Obtiene o establece el nombre del usuario que entrega la unidad
        /// </summary>
        public string NombreUsuarioEntrega
        {
            get { return this.txtNombreUsuarioEntrega.Text.Trim().ToUpper(); }
            set
            {
                this.txtNombreUsuarioEntrega.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
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
        public string NumeroLicencia {
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
                if (!string.IsNullOrEmpty(this.txtFechaSalida.Text) && !string.IsNullOrWhiteSpace(this.txtFechaSalida.Text))
                {
                    DateTime date;
                    return DateTime.TryParse(this.txtFechaSalida.Text, out date)
                               ? (DateTime?)date
                               : null;
                }
                return null;
            }
            set
            {
                this.txtFechaSalida.Text = value.HasValue ? value.Value.ToShortDateString() : string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o estable la hora de salida de la unidad
        /// </summary>
        public TimeSpan? HoraListado
        {
            get
            {
                if (!string.IsNullOrEmpty(this.txtHoraSalida.Text) && !string.IsNullOrWhiteSpace(this.txtHoraSalida.Text))
                {
                    var time = DateTime.ParseExact(this.txtHoraSalida.Text, "hh:mm tt", CultureInfo.InvariantCulture);
                    return time.TimeOfDay;
                }
                return null;
            }
            set
            {
                if (value != null)
                {
                    DateTime d = new DateTime().Add(value.Value);
                    this.txtHoraSalida.Text = string.Format("{0:hh:mm tt}", d).ToUpper().Replace(".", "").Replace("A M", "AM").Replace("P M", "PM").Trim();
                }
                else
                    this.txtHoraSalida.Text = string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o establece el kilometraje de salida
        /// </summary>
        public int? Kilometraje
        {
            get
            {
                if (!string.IsNullOrEmpty(this.txtKilometrajeSalida.Text) && !string.IsNullOrWhiteSpace(this.txtKilometrajeSalida.Text))
                {
                    int val = 0;
                    return Int32.TryParse(this.txtKilometrajeSalida.Text, out val) ? (int?)val : null;
                }
                return null;
            }
            set
            {
                this.txtKilometrajeSalida.Text = value.HasValue ? value.Value.ToString().Trim().ToUpper() : string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o establece las horas de refrigeración de salida
        /// </summary>
        public int? Horometro
        {
            get
            {
                if (!string.IsNullOrEmpty(this.txtHoraRefrigeracion.Text) && !string.IsNullOrWhiteSpace(this.txtHoraRefrigeracion.Text))
                {
                    int val = 0;
                    return Int32.TryParse(this.txtHoraRefrigeracion.Text, out val) ? (int?)val : null;
                }
                return null;
            }
            set
            {
                this.txtHoraRefrigeracion.Text = value.HasValue ? value.Value.ToString().Trim().ToUpper() : string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o establece el combustible de salida
        /// </summary>
        public int? Combustible
        {
            get
            {
                if (!string.IsNullOrEmpty(this.txtCombustibleSalida.Text) && !string.IsNullOrWhiteSpace(this.txtCombustibleSalida.Text))
                {
                    int val = 0;
                    return Int32.TryParse(this.txtCombustibleSalida.Text, out val) ? (int?)val : null;
                }
                return null;
            }
            set
            {
                this.txtCombustibleSalida.Text = value.HasValue ? value.Value.ToString().Trim().ToUpper() : string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o establece si la unidad tiene golpes en general
        /// </summary>
        public bool? TieneGolpesGeneral {
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
                return Boolean.TryParse(this.rbtDocumentacionCompleta.SelectedValue, out val) ? (bool?) val : null;
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
                return Boolean.TryParse(this.rbtLimpiezaInteriosCaja.SelectedValue, out val) ? (bool?)val : null;
            }
            set
            {
                if (value.HasValue)
                {
                    this.rbtLimpiezaInteriosCaja.SelectedValue = value.Value ? Boolean.TrueString : Boolean.FalseString;
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
        /// <summary>
        /// Obtiene o establece los listados de verificación de sección
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
            set { Session["VerificacionesSeccion"] = value ?? (new List<VerificacionSeccionBO>()).ConvertAll(x => (object) x); }
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
                                ? (List<VerificacionLlantaBO>) Session["VerificacionesLlanta"]
                                : new List<VerificacionLlantaBO>();

                return listValue.ConvertAll(x => (object)x);
            }
            set
            {
                Session["VerificacionesLlanta"] = value ?? (new List<VerificacionLlantaBO>()).ConvertAll(x => (object)x);
            }
        }
        /// <summary>
        /// Obtiene o establece el identificador de la refacción
        /// </summary>
        public int? RefaccionID
        {
            get
            {
                if (!string.IsNullOrEmpty(this.hdnRefaccionID.Value) && !string.IsNullOrWhiteSpace(this.hdnRefaccionID.Value))
                {
                    int val = 0;
                    return Int32.TryParse(this.hdnRefaccionID.Value, out val) ? (int?)val : null;
                }
                return null;
            }
            set
            {
                this.hdnRefaccionID.Value = value.HasValue ? value.Value.ToString().Trim().ToUpper() : string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o establece el codigo de la refaccion
        /// </summary>
        public string RefaccionCodigo {
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
        /// Obtiene o establece los archivos de las secciones de la unidad
        /// </summary>
        public object NuevosArchivos
        {
            get { return Session["NewObject"] as List<ArchivoBO>; }
            set { Session["NewObject"] = value; }
        }
        /// <summary>
        /// Obtiene el nombre del archivo seleccionado
        /// </summary>
        public string NombreArchivoImagen {
            get { return this.uplArchivoImagen.HasFile ? this.uplArchivoImagen.FileName : string.Empty; }
        }
        /// <summary>
        /// Obtiene la extensión del archivo seleccionado
        /// </summary>
        public string ExtencionArchivoImagen
        {
            get { return this.uplArchivoImagen.HasFile ? System.IO.Path.GetExtension(NombreArchivoImagen).Replace(".", "") : string.Empty; }
        }
        /// <summary>
        /// Obtiene la sección de unidad seleccionada
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
        /// Obtiene o establece las oobservaciones para una seccion de la unidad
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
        /// Obtiene o Asigna el valor del Ultimo Kilometraje de Entrega de la Unidad
        /// </summary>
        public Int32? KilometrajeAnterior
        {
            get
            {
                if(!string.IsNullOrEmpty(this.hdnKilometrajeAnterior.Value) && !string.IsNullOrWhiteSpace(this.hdnKilometrajeAnterior.Value))
                {
                    int val = 0;
                    return Int32.TryParse(this.hdnKilometrajeAnterior.Value, out val) ? (int?)val : null;
                }
                return null;
            }
            set
            {
                this.hdnKilometrajeAnterior.Value = value.HasValue ? value.Value.ToString().Trim().ToUpper() : string.Empty;
            }
        }
        /// <summary>
        ///  Obtiene o Asigna el ultimo valor del Horometro de Entrega de la Unidad
        /// </summary>
        public Int32? HorometroAnterior
        {
            get
            {
                if(!string.IsNullOrEmpty(this.hdnHorometroAnterior.Value) && !string.IsNullOrWhiteSpace(this.hdnHorometroAnterior.Value))
                {
                    int val = 0;
                    return Int32.TryParse(this.hdnHorometroAnterior.Value, out val) ? (int?)val : null;
                }
                return null;
            }
            set
            {
                this.hdnHorometroAnterior.Value = value.HasValue ? value.Value.ToString().Trim().ToUpper() : string.Empty;
            }
        }

        #endregion

        #region Constructores
        /// <summary>
        /// Constructor de la página
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.txtObservacionesBaterias.Attributes.Add("onkeyup", "checkText(this,150);");
                this.txtObservacionesDocumentacion.Attributes.Add("onkeyup", "checkText(this,150);");
                this.txtObservacionesLlanta.Attributes.Add("onkeyup", "checkText(this,150);");
                this.txtObservacionesSeccion.Attributes.Add("onkeyup", "checkText(this,150);");
                this.presentador = new RegistrarEntregaPRE(this, this.ucCatalogoDocumentos, this.ucucEquiposAliadosUnidadUI);
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
        /// Reestablece los controles en caso de alguna inconsistencia
        /// </summary>
        private void ReestablecerControles()
        {
            this.btnCancelar.Enabled = true;
            this.btnGuardar.Enabled = false;
        }
        /// <summary>
        /// Este método despliega un mensaje en pantalla
        /// </summary>
        /// <param name="mensaje">Mensaje a desplegar</param>
        /// <param name="tipo">Tipo de mensaje a desplegar</param>
        /// <param name="detalle">Muestra una cadena de texto que contiene los datalles del mensaje a desplegar</param>
        public void MostrarMensaje(string mensaje, Primitivos.Enumeradores.ETipoMensajeIU tipo, string msjDetalle = null)
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
        /// Elimina de la Session las variables usadas en el caso de uso
        /// </summary>
        public void LimpiarSesion()
        {
            Session.Remove("NewObject");
            Session.Remove("TiposArchivosImagen");
            Session.Remove("CheckListEntrega");
            Session.Remove("VerificacionesSeccion");
            Session.Remove("VerificacionesLlanta");
            Session.Remove("NombreReporte");
            Session.Remove("DatosReporte");            
        }
        /// <summary>
        /// Limpia el contrato al cual se le esta registrando el check list
        /// </summary>
        public void LimpiarPaqueteNavegacion()
        {
            Session.Remove("RegistrarEntregaUI");
        }
        /// <summary>
        /// Prepara la interfaz para el registro de un CheckList
        /// </summary>
        public void PrepararNuevo()
        {
            this.txtNumeroContrato.ReadOnly = true;
            this.txtNombreCliente.ReadOnly = true;
            this.txtNombreOperador.ReadOnly = true;
            this.txtNumeroEconomico.ReadOnly = true;
            this.txtNumeroSerie.ReadOnly = true;
            this.txtPlacasEstatales.ReadOnly = true;
            this.txtPlacasFederales.ReadOnly = true;
            this.txtNombreUsuarioEntrega.ReadOnly = true;
            this.txtRefaccion.ReadOnly = true;
            this.txtNumeroLicencia.ReadOnly = true;
			this.txtFechaSalida.ReadOnly = true;

            this.txtNumeroContrato.Enabled = false;
            this.txtNumeroSerie.Enabled = false;
            this.txtNombreCliente.Enabled = false;
            this.txtNombreOperador.Enabled = false;
            this.txtNumeroEconomico.Enabled = false;
            this.txtPlacasEstatales.Enabled = false;
            this.txtPlacasFederales.Enabled = false;
            this.txtNombreUsuarioEntrega.Enabled = false;
            this.txtRefaccion.Enabled = false;
            this.txtNumeroLicencia.Enabled = false;
			this.txtFechaSalida.Enabled = false;
        }
        /// <summary>
        /// Redirige a la página de acceso denegado
        /// </summary>
        public void RedirigirSinPermisoAcceso()
        {
            this.Response.Redirect(this.ResolveUrl("~/MapaSitio.UI/PaginaSinAccesoUI.aspx"), false);
        }
        /// <summary>
        /// Redirige a la página de consulta de check list
        /// </summary>
        public void RedirigirAConsulta()
        {
            this.Response.Redirect(this.ResolveUrl("~/Contratos.RD.UI/ConsultarListadoVerificacionUI.aspx"), false);
        }
        /// <summary>
        /// Redirige a la página de impresión del reporte
        /// </summary>
        public void RedirigirAImprimir()
        {
            Response.Redirect("../Buscador.UI/VisorReporteUI.aspx", false);
        }
        /// <summary>
        /// Guarda en la variable de Session un paquete para su posterior consulta
        /// </summary>
        /// <param name="key">Clave del paquete de navegación</param>
        /// <param name="value">Paquete que se desea guardar en la session</param>
        public void EstablecerPaqueteNavegacion(string key, object value)
        {
            Session["NombreReporte"] = key;
            Session["DatosReporte"] = value;
        }
        /// <summary>
        /// Obtiene un paquete guardado en la session
        /// </summary>
        /// <returns>Objeto guardado en la session</returns>
        public object ObtenerPaqueteContrato()
        {
            return Session["RegistrarEntregaUI"] ?? null;
        }
        /// <summary>
        /// Despliega en pantalla las llantas de la unidad seleccionada
        /// </summary>
        /// <param name="unidad">Unidad de la cual se desea conocer las llantas</param>
        public void CargarLlantas(object unidad)
        {
            this.grdLlantas.DataSource = ((UnidadBO) unidad).Llantas;
            this.grdLlantas.DataBind();
        }
        /// <summary>
        /// Establece las opciones configuradas para las observaciones de las secciones de la unidad
        /// </summary>
        /// <param name="opciones">Listado de opciones configuradas</param>
        public void EstablecerOpcionesSeccion(Dictionary<int, string> opciones)
        {
            if(ReferenceEquals(opciones, null))
                opciones = new Dictionary<int, string>();
            
            this.ddlSeccionesunidad.DataSource = opciones;
            this.ddlSeccionesunidad.DataValueField = "key";
            this.ddlSeccionesunidad.DataTextField = "value";
            this.ddlSeccionesunidad.DataBind();
        }
        /// <summary>
        /// Obtiene el valor del estado de las llantas proporcionados por el usuario
        /// </summary>
        /// <returns>Listado con los estados de las llanta</returns>
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
        /// Obtiene el arreglo de bytes para la imagén seleccionada
        /// </summary>
        /// <returns>Arreglo de bytes</returns>
        public byte[] ObtenerArchivosBytes()
        {
            return this.uplArchivoImagen.HasFile ? this.uplArchivoImagen.FileBytes : null;
        }
        /// <summary>
        /// Despliega en el grid de imagenes las imagenes seleccionadas por el usuario
        /// </summary>
        public void CargarImagenes()
        {
            this.grdArchivosImagen.DataSource = this.NuevosArchivos;
            this.grdArchivosImagen.DataBind();
        }
        /// <summary>
        /// Limpia la información de lso controles para la captura de observaciones de sección
        /// </summary>
        public void LimpiarDatosSeccionUnidad()
        {
            this.ddlSeccionesunidad.SelectedIndex = 0;
            this.txtObservacionesSeccion.Text = string.Empty;
            NuevosArchivos = new List<ArchivoBO>();
            this.CargarImagenes();
        }
        /// <summary>
        /// Despliega en el grid de observaciones la inforamción capturada por el usuario
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
        /// Despliega en el grid las imagenes seleccionadas para una sección
        /// </summary>
        /// <param name="p">Listado de imagenes que se desea desplegar</param>
        public void CargarDetalleImagenSeccion(object p)
        {
            this.grdImagenesDialog.DataSource = (List<ArchivoBO>) p;
            this.grdImagenesDialog.DataBind();
        }
        /// <summary>
        /// Habilita o deshabilita el control de refacción
        /// </summary>
        /// <param name="estado">Estado que toma los controles</param>
        public void HabilitarRefaccion(bool estado)
        {
            this.rbtRefaccionCorrecta.Enabled = estado;
        }

		public void BloquearRegistro() {
			btnGuardar.Enabled = false;
			txtCombustibleSalida.Enabled = false;
			txtHoraSalida.Enabled = false;
			txtKilometrajeSalida.Enabled = false;
			txtHoraRefrigeracion.Enabled = false;
			rbtInteriorLimpio.Enabled = false;
			rbtVestidurasLimpias.Enabled = false;
			rbtLLaveOriginal.Enabled = false;
			rbtEstereoBocinas.Enabled = false;
			rbtExtinguidor.Enabled = false;
			rbtTresReflejantes.Enabled = false;
			rbtLimpiezaInteriosCaja.Enabled = false;
			rbtGolpesGeneral.Enabled = false;
			rbtDocumentacionCompleta.Enabled = false;
			rbtTapetes.Enabled = false;
			rbtEncendedor.Enabled = false;
			rbtAlarmasReversa.Enabled = false;
			rbtGato.Enabled = false;
			rbtEspejosCompletos.Enabled = false;
			rbtGPS.Enabled = false;
			rbtBateriasCorrectas.Enabled = false;
			txtObservacionesDocumentacion.Enabled = false;
			txtObservacionesBaterias.Enabled = false;
			grdLlantas.Enabled = false;
			rbtRefaccionCorrecta.Enabled = false;
			txtObservacionesLlanta.Enabled = false;
			ddlSeccionesunidad.Enabled = false;
			txtObservacionesSeccion.Enabled = false;
			btnAgregarSeccion.Enabled = false;
			uplArchivoImagen.Enabled = false;
			btnAgregarImagen.Enabled = false;
			ucCatalogoDocumentos.presentador.ModoEditable(false);

			txtCombustibleSalida.ReadOnly = true;
			txtHoraSalida.ReadOnly = true;
			txtKilometrajeSalida.ReadOnly = true;
			txtHoraRefrigeracion.ReadOnly = true;
		}
        #endregion

        #region Eventos
        /// <summary>
        /// Guarda los listados de verificacion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                presentador.RegistrarEntrega();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al guardar el Check List.", ETipoMensajeIU.ERROR, nombreClase + ".btnGuardar_Click:" + ex.Message);
            }

        }
        /// <summary>
        /// Cancela el registro del listado de verificación
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            try
            {
                presentador.CancelarRegistro();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencias al cancelar el registro del Check List.", ETipoMensajeIU.ERROR, nombreClase + ".btnCancelar_Click: " + ex.Message);
            }
        }
        /// <summary>
        /// Depsliega en pantalla los archivos agregados al Check List
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdArchivos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.grdArchivosImagen.PageIndex = e.NewPageIndex;
            this.grdArchivosImagen.DataSource = this.NuevosArchivos;
            this.grdArchivosImagen.DataBind();
        }
        /// <summary>
        /// Ejecuta un operación en el grid de archivos
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
                this.MostrarMensaje("Inconsistencia al intentar ejecutar una acción en el Grid.", ETipoMensajeIU.ERROR, nombreClase + ".grdArchivos_RowCommand: " + ex.Message);
            }
        }
        /// <summary>
        /// Ejecuta una operación en el grid de archivos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdArchivos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    ArchivoBO archivoBO = (ArchivoBO) e.Row.DataItem;
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
                        ImageButton imgBtn = (ImageButton) e.Row.FindControl("ibtDescargar");
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
                this.MostrarMensaje("Inconsistencia al momento de cargar los archivos en el grid.", ETipoMensajeIU.ERROR, nombreClase + ".grdArchivos_RowDataBound: " + ex.Message);
            }
        }
        /// <summary>
        /// Agrega una sección al listado de observaciones
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
                this.MostrarMensaje("Inconsistencias al tratar de agregar la imagén de la sección.", ETipoMensajeIU.ERROR, nombreClase + ".btnAgregarSeccion_Click" + ex.Message);
            }
        }
        /// <summary>
        /// Agrega una imagén a las obsrvaciones de sección de la unidad
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAgregarImagen_Click(object sender, EventArgs e)
        {
            try
            {
                if(this.uplArchivoImagen.HasFile)
                    this.presentador.AgregarArchivoImagen();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencias al tratar de agregar la imagén de la sección.", ETipoMensajeIU.ERROR, nombreClase + ".btnAgregarImagen_Click" + ex.Message);
            }
        }
        /// <summary>
        /// Ejecuta una operación en el grid de observaciones a la sección
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
                this.MostrarMensaje("Ocurrio una inconsistencia al momento de intentar ejecutar una acción en el grid", ETipoMensajeIU.ERROR, nombreClase + ".grdObservacionesSecciones_RowCommand :" + ex.Message);
            }
        }
        /// <summary>
        /// Cambia la página del grid
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
        /// Valida el combustible de salida en base a la capacidad del tanque de la unidad
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtCombustibleSalida_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string s = this.presentador.ValidarTanque();
                if (!string.IsNullOrEmpty(s) && !string.IsNullOrWhiteSpace(s))
                {                    
                    this.txtCombustibleSalida.Text = string.Empty;
                    this.txtCombustibleSalida.Focus();
                    this.MostrarMensaje(s, ETipoMensajeIU.ADVERTENCIA, null);
                    this.txtCombustibleSalida.Focus();
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al momento de evaluar la cantidad de combustible.", ETipoMensajeIU.ERROR, string.Format("{0}.txtCombustibleSalida_TextChanged: {1}{2}", nombreClase, Environment.NewLine, ex.Message));
            }
        }
        /// <summary>
        /// Valida que el kilometraje colocado no sea menor a ultimo kilometra de la unidad
        /// </summary>
        protected void txtKilometrajeSalida_OnTextChanged(object sender, EventArgs e)
        {
            try
            {
                string s = this.presentador.ValidarKilometros();
                if(!string.IsNullOrEmpty(s) && !string.IsNullOrWhiteSpace(s))
                {
                    this.txtKilometrajeSalida.Text = string.Empty;
                    this.txtKilometrajeSalida.Focus();
                    this.MostrarMensaje(s, ETipoMensajeIU.ADVERTENCIA, null);
                    this.txtKilometrajeSalida.Focus();
                }
            }
            catch(Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al momento de evaluar el Kilometraje.", ETipoMensajeIU.ERROR, string.Format("{0}.txtKilometrajeSalida_OnTextChanged: {1}{2}", nombreClase, Environment.NewLine, ex.Message));
            }
        }
        /// <summary>
        /// Valida que el valor de las horas colocado no sea menor a ultimo de la unidad
        /// </summary>
        protected void txtHoraRefrigeracion_OnTextChanged(object sender, EventArgs e)
        {
            try
            {
                string s = this.presentador.ValidarHoras();
                if(!string.IsNullOrEmpty(s) && !string.IsNullOrWhiteSpace(s))
                {
                    this.MostrarMensaje(s, ETipoMensajeIU.ADVERTENCIA, null);
                }
            }
            catch(Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al momento de evaluar las Horas de la Unidad.", ETipoMensajeIU.ERROR, string.Format("{0}.txtHoraRefrigeracion_OnTextChanged: {1}{2}", nombreClase, Environment.NewLine, ex.Message));
            }
        }
        #endregion
    }
}