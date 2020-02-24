//Satisface al caso de uso CU026 - Registrar Finalización de Contrato Full Service Leasing
using System;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Contratos.FSL.BO;
using BPMO.SDNI.Contratos.FSL.VIS;

namespace BPMO.SDNI.Contratos.FSL.PRE {
	public class ucFinalizacionContratoFSLPRE {
		#region atributos

		private IucFinalizacionContratoFSLVIS vista;

		#endregion

		#region Constructores

		public ucFinalizacionContratoFSLPRE(IucFinalizacionContratoFSLVIS vistaActual) {
			vista = vistaActual;
		}

		#endregion

		#region Métodos

		public void Inicializar(ContratoFSLBO contrato) {
			try {
				vista.Mensualidad = contrato.CalcularMensualidad();
				vista.Plazo = contrato.Plazo;
				vista.FechaInicioContrato = contrato.FechaInicioContrato;
				vista.FechaFinContrato = ((DateTime)contrato.FechaInicioContrato).AddMonths((int)contrato.Plazo);
				vista.PorcentajePenalizacion = contrato.PorcentajePenalizacion;
				if (contrato.CierreContrato != null && contrato.CierreContrato.Fecha != null) {
					vista.FechaCierre = contrato.CierreContrato.Fecha;
					vista.ObservacionesCierre = contrato.Observaciones;
					if (DateTime.Compare((DateTime)contrato.CierreContrato.Fecha, (DateTime)contrato.CalcularFechaTerminacionContrato()) < 0) {
						vista.Penalizacion = ((CierreAnticipadoContratoFSLBO)contrato.CierreContrato).CantidadPenalizacion;
						vista.MotivoCierreAnticipado = ((CierreAnticipadoContratoFSLBO) contrato.CierreContrato).Motivo;
					}

				}
			} catch (Exception ex) {
				vista.MostrarMensaje("Error al desplegar los datos de cierre", ETipoMensajeIU.ADVERTENCIA);
			}

		}

		public void ValidarFechaCierre() {
			//No validar si esta en modo consulta
			if (!(bool)vista.ModoEdicion) return;
			vista.MostrarPenalizacion(false);
			vista.MostrarMotivos(false);
			vista.Penalizacion = null;
			vista.ObservacionObligatoria(false);
			//La fecha de cierre es anterior al inicio del contrato
			if (vista.FechaInicioContrato != null && vista.FechaCierre != null)
				if (DateTime.Compare((DateTime)vista.FechaCierre, (DateTime)vista.FechaInicioContrato) < 0) {
					vista.MostrarMensaje("Fecha de Cierre No Válida", ETipoMensajeIU.INFORMACION, "La fecha de cierre no puede ser menor a la fecha de inicio del contrato");
					vista.FechaCierre = null;
					return;
				}
			if (vista.FechaCierre != null && vista.FechaFinContrato != null)
				if (DateTime.Compare((DateTime)vista.FechaFinContrato, (DateTime)vista.FechaCierre) > 0) {
					vista.MostrarMotivos(true);
					vista.MostrarPenalizacion(true);
					vista.Penalizacion = CalcularPenalizacion();
					vista.ObservacionObligatoria(true);
				}
		}

		public decimal? CalcularPenalizacion() {
			try {
				decimal Mensualidad = (decimal)vista.Mensualidad;
				int meses = CalcularMeses((DateTime)vista.FechaCierre, (DateTime)vista.FechaFinContrato);
				decimal penalizacion = meses * (decimal)vista.Mensualidad * ((decimal)vista.PorcentajePenalizacion/100);
				decimal tresMeses = Mensualidad * 3;
				return penalizacion > tresMeses ? penalizacion : tresMeses;
			} catch (Exception ex) {
				vista.MostrarMensaje("Error al calcular la penalización",ETipoMensajeIU.INFORMACION);
				return null;
			}

		}

		private int CalcularMeses(DateTime fechaInicial, DateTime FechaFinal) {
			int meses = 0;
			while (fechaInicial < FechaFinal) {
				meses++;
				fechaInicial = fechaInicial.AddMonths(1);
			}
			return meses;
		}

		public void EstablecerModoEdicion(bool edicion) {
			vista.ModoEdicion = edicion;
			if (edicion) vista.ConfigurarModoEdicion();
			else vista.ConfigurarModoConsulta();
		}

		public CierreAnticipadoContratoFSLBO InterfazUsuarioADatos() {
			CierreAnticipadoContratoFSLBO cierre = new CierreAnticipadoContratoFSLBO();
			cierre.CantidadPenalizacion = vista.Penalizacion;
			cierre.Fecha = vista.FechaCierre;
			cierre.Observaciones = vista.ObservacionesCierre;
			cierre.Motivo = vista.MotivoCierreAnticipado;
			return cierre;
		}

		public string ValidarDatosCierre() {
			string campos = string.Empty;
			if (vista.FechaCierre == null) campos = ", Fecha de Cierre";
			else {
				if (DateTime.Compare((DateTime)vista.FechaCierre, (DateTime)vista.FechaFinContrato) < 0) {
					if (vista.ObservacionesCierre == null) campos += ", Observaciones de cierre";
					if (vista.Penalizacion == null) campos += ", Penalización";
					if (vista.MotivoCierreAnticipado == null) campos += ", Motivo de Cierre";
				}
			}
			return campos.Length > 0 ? "Los siguientes campos no pueden estar vacíos: \n" + campos.Substring(2) : "";
		}

		#endregion

		
	}
}
