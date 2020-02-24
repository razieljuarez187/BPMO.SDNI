using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using BPMO.Basicos.BO;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Contratos.PSL.VIS;

namespace BPMO.SDNI.Contratos.PSL.PRE {
    public class ucPeriodoTarifarioPSLPRE {
        #region Atributos
        private string nombreClase = "ucPeriodoTarifarioPSLPRE";
        internal IucPeriodoTarifarioPSLVIS vista;
        #endregion

        #region Constructor
        public ucPeriodoTarifarioPSLPRE(IucPeriodoTarifarioPSLVIS vista) {
            try {
                this.vista = vista;
            } catch (Exception ex) {
                throw new Exception(nombreClase + ".ucPeriodoTarifariosPSLPRE:Error al configurar el presentador.");
            }
        }
        #endregion

        #region Métodos
        public void PrepararNuevo() {
            this.vista.ModoEdicion(true);
            this.vista.EstablecerOpcionesTarifaTurno(this.ObtenerTurnos());
        }

        public void PrepararDetalle() {
            this.vista.ModoEdicion(false);
            this.vista.EstablecerOpcionesTarifaTurno(this.ObtenerTurnos());
        }

        /// <summary>
        /// Método para obtener un diccionario con los valores de los turnos que se envía como parámetro para el llenado del combo correspondiente
        /// </summary>
        /// <returns>Diccionario de tipo string,string</returns>
        private Dictionary<string, string> ObtenerTurnos() {
            try {
                Dictionary<string, string> listaTurnos = new Dictionary<string, string>();
                listaTurnos.Add("-1", "SELECCIONA UNA OPCIÓN");
                Type type = this.vista.UnidadOperativaID == (int)ETipoEmpresa.Construccion ? typeof(ETarifaTurnoConstruccion) :
                this.vista.UnidadOperativaID == (int)ETipoEmpresa.Generacion ? typeof(ETarifaTurnoGeneracion) :
                typeof(ETarifaTurnoEquinova);
                Array values = Enum.GetValues(type);
                foreach (int value in values) {
                    var memInfo = type.GetMember(type.GetEnumName(value));
                    var display = memInfo[0]
                        .GetCustomAttributes(typeof(DescriptionAttribute), false)
                        .FirstOrDefault() as DescriptionAttribute;

                    if (display != null) {
                        listaTurnos.Add(value.ToString(), display.Description);
                    }
                }

                return listaTurnos;
            } catch (Exception ex) {

                throw new Exception(this.nombreClase + ".ListaTurnos:Error al consultar los turnos");
            }
        }

        public string ValidarDatos() {
            string s = "";

            if (this.vista.DiasDuracionSemana == null)
                s += "Duración Semana, ";

            if (this.vista.DiasDuracionMes == null)
                s += "Duración Mes, ";

            if (this.vista.InicioPeriodoDia == null)
                s += "Inicio Período Día, ";

            if (this.vista.InicioPeriodoSemana == null)
                s += "Inicio Período Semana, ";

            if (this.vista.InicioPeriodoMes == null)
                s += "Inicio Período Mes, ";

            if (s != null && s.Trim().CompareTo("") != 0)
                return "Los siguientes campos no pueden estar vacíos: \n" + s.Substring(0, s.Length - 2);

            if (this.vista.InicioPeriodoSemana <= 0)
                s += "Inicio Período Semana, ";

            if (this.vista.InicioPeriodoMes <= 0)
                s += "Inicio Período Mes, ";

            if (s != null && s.Trim().CompareTo("") != 0)
                return "Los siguientes campos no pueden ser menores o iguales a cero: \n" + s.Substring(0, s.Length - 2);

            if (this.vista.InicioPeriodoSemana <= this.vista.InicioPeriodoDia)
                return "El Inicio Período Semana no puede ser menor o igual al Inicio Período Día.";

            if (this.vista.InicioPeriodoMes <= this.vista.InicioPeriodoSemana)
                return "El Inicio Período Mes no puede ser menor o igual al Inicio Período Semana.";

            if (!this.vista.listHorasTurno.Any())
                return "Tiene que existir al menos un registro de horas por turno configurado para el Período Tarifario";

            return null;
        }

        //<summary>
        //Agrega un rango de un PeriodoTarifario
        //</summary>
        public void AgregarHorasTurnoAPeriodoTarifario() {
            string validarRango = ValidarTurno();
            if (!String.IsNullOrEmpty(validarRango)) {
                this.vista.MostrarMensaje(validarRango, ETipoMensajeIU.ADVERTENCIA);
                return;
            }
            this.vista.listHorasTurno.Add(InterfazAHorasTurnoPeriodoTarifarioPSLBO());
            this.vista.PresentarHorasTurno(this.vista.listHorasTurno);
            this.vista.LimpiarCamposHorasTurno();
        }

        /// <summary>
        /// Obtiene el detalle de las horas turno que se está configurando
        /// </summary>
        /// <returns>objeto de tipo DetalleHorasTurnoTarifaBO</returns>
        private DetalleHorasTurnoTarifaBO InterfazAHorasTurnoPeriodoTarifarioPSLBO() {
            DetalleHorasTurnoTarifaBO horasTurno = new DetalleHorasTurnoTarifaBO()
            {
                TurnoTarifa = this.vista.TarifaTurno,
                Dia = this.vista.MaximoHorasDia,
                Semana = this.vista.MaximoHorasSemana,
                Mes = this.vista.MaximoHorasMes,
                Auditoria = new AuditoriaBO() { UC = this.vista.UsuarioID, FC = DateTime.Now, UUA = this.vista.UsuarioID, FUA = DateTime.Now }
            };

            return horasTurno;
        }

        /// <summary>
        /// Validar que el turno que se está configurando pueda agregarse a la tabla
        /// </summary>
        /// <returns>Cadena de texto con el error, en caso de encontrar alguno</returns>
        public string ValidarTurno() {
            string s = string.Empty;

            if (this.vista.listHorasTurno == null)
                this.vista.listHorasTurno = new List<DetalleHorasTurnoTarifaBO>();

            Type type = this.vista.UnidadOperativaID == (int)ETipoEmpresa.Construccion ? typeof(ETarifaTurnoConstruccion) : 
                this.vista.UnidadOperativaID == (int)ETipoEmpresa.Generacion ? typeof(ETarifaTurnoGeneracion) :
                typeof(ETarifaTurnoEquinova);
            if (this.vista.listHorasTurno != null && this.vista.listHorasTurno.Where(x => ((Enum)Enum.ToObject(type, x.TurnoTarifa)).ToString() == ((Enum)Enum.ToObject(type, this.vista.TarifaTurno)).ToString()).Any())
                return "El turno seleccionado ya se encuentra configurado en la tabla actualmente.";

            if (this.vista.TurnoTarifaID == null)
                s += "Turno, ";

            if (this.vista.MaximoHorasDia == null)
                s += "Máximo Horas Día, ";

            if (this.vista.MaximoHorasSemana == null)
                s += "Máximo Horas Semana, ";

            if (this.vista.MaximoHorasMes == null)
                s += "Máximo Horas Mes, ";

            if (s != null && s.Trim().CompareTo("") != 0)
                return "Los siguientes campos no pueden estar vacíos: \n" + s.Substring(0, s.Length - 2);

            if (this.vista.MaximoHorasDia <= 0)
                s += "Máximo Horas Día, ";

            if (this.vista.MaximoHorasSemana <= 0)
                s += "Máximo Horas Semana, ";

            if (this.vista.MaximoHorasMes <= 0)
                s += "Máximo Horas Mes, ";

            if (s != null && s.Trim().CompareTo("") != 0)
                return "Los siguientes campos deben ser mayores a cero: \n" + s.Substring(0, s.Length - 2);

            return null;
        }

        public void DatosAInterfazUsuario(DiaPeriodoTarifaBO PeriodoTarifario) {
            try {
                if (Object.ReferenceEquals(PeriodoTarifario, null))
                    PeriodoTarifario = new DiaPeriodoTarifaBO() { DetalleHorasTurnoTarifa = new List<DetalleHorasTurnoTarifaBO>() { new DetalleHorasTurnoTarifaBO() } };

                this.vista.IncluyeSD = PeriodoTarifario.IncluyeSD;
                this.vista.DiasDuracionSemana = PeriodoTarifario.DiasDuracionSemana;
                this.vista.DiasDuracionMes = PeriodoTarifario.DiasDuracionMes;
                this.vista.InicioPeriodoDia = 1;
                this.vista.InicioPeriodoSemana = PeriodoTarifario.InicioPeriodoSemana;
                this.vista.InicioPeriodoMes = PeriodoTarifario.InicioPeriodoMes;
                this.vista.listHorasTurno = this.ObtenerListaClonada(PeriodoTarifario.DetalleHorasTurnoTarifa);
                this.vista.PresentarHorasTurno(PeriodoTarifario.DetalleHorasTurnoTarifa);

            } catch (Exception ex) {
                throw new Exception(this.nombreClase + ".DatosAInterfazUsuario:Error al intentar establecer los datos de la PeriodoTarifario." + ex.Message);
            }
        }

        /// <summary>
        /// Obtener una lista clonada del objeto para poder guardar como UltimoObjeto
        /// </summary>
        /// <param name="lstTemporal">Lista que será clonada</param>
        /// <returns></returns>
        public List<DetalleHorasTurnoTarifaBO> ObtenerListaClonada(List<DetalleHorasTurnoTarifaBO> lstTemporalSeleccionada) {
            List<DetalleHorasTurnoTarifaBO> lstTemporal = lstTemporalSeleccionada;
            List<DetalleHorasTurnoTarifaBO> lstClonada = new List<DetalleHorasTurnoTarifaBO>();

            foreach (DetalleHorasTurnoTarifaBO detalleHora in lstTemporal) {
                lstClonada.Add(detalleHora.Clone());
            }

            return lstClonada;

        }

        public DiaPeriodoTarifaBO InterfazUsuarioADato() {
            try {
                DiaPeriodoTarifaBO PeriodoTarifario = new DiaPeriodoTarifaBO()
                {
                    DetalleHorasTurnoTarifa = new List<DetalleHorasTurnoTarifaBO>(),
                    IncluyeSD = this.vista.IncluyeSD,
                    DiasDuracionSemana = this.vista.DiasDuracionSemana,
                    DiasDuracionMes = this.vista.DiasDuracionMes,
                    InicioPeriodoDia = 1,
                    InicioPeriodoSemana = this.vista.InicioPeriodoSemana,
                    InicioPeriodoMes = this.vista.InicioPeriodoMes
                };
                PeriodoTarifario.DetalleHorasTurnoTarifa.AddRange(this.vista.listHorasTurno.Select(detalle => new DetalleHorasTurnoTarifaBO(detalle)).ToList());

                foreach (DetalleHorasTurnoTarifaBO detalle in PeriodoTarifario.DetalleHorasTurnoTarifa) {
                    detalle.Auditoria = new AuditoriaBO() { UC = this.vista.UsuarioID, FC = DateTime.Now, UUA = this.vista.UsuarioID, FUA = DateTime.Now };
                }

                return PeriodoTarifario;
            } catch (Exception ex) {
                throw new Exception(this.nombreClase + ".InterfazUsuarioADato:Error al intentar obtener los PeriodoTarifarios." + ex.Message);
            }
        }

        public void ModoConsulta(bool activo) {
            this.vista.ModoEdicion(!activo);
        }
        #endregion
    }
}