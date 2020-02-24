//Satisface al CU077 - Registrar Acta de Nacimiento de una Unidad
//Satisface al CU079 - Consultar Acta de Nacimiento de Unidad
//Satisface al CU080 – Editar Acta de Nacimiento de una Unidad
//Satisface la solicitud de cambio SC0006

using System;
using System.Collections.Generic;
using BPMO.Basicos.BO;
using BPMO.Facade.SDNI.BR;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Equipos.BO;
using BPMO.SDNI.Equipos.VIS;

namespace BPMO.SDNI.Equipos.PRE
{
	public class ucResumenActaNacimientoPRE
    {
        #region Atributos
        private IDataContext dctx = null;

        private IucResumenActaNacimientoVIS vista;
        #endregion

        #region Constructores
        public ucResumenActaNacimientoPRE(IucResumenActaNacimientoVIS view)
		{
			try
			{
                this.vista = view;

                this.dctx = FacadeBR.ObtenerConexion();
			}
			catch (Exception ex)
			{
                this.vista.MostrarMensaje("Inconsistencias en los parámetros de configuración", ETipoMensajeIU.ERROR, "ucResumenActaNacimientoPRE.ucResumenActaNacimientoPRE:" + ex.Message);
			}
		}
        #endregion

        #region Métodos

        /// <summary>
        /// Invoca el método de EstablcerAcciones, establece configuración inicial de la pagina
        /// </summary>
        public void EstablecerAcciones(List<CatalogoBaseBO> ListaAcciones)
        {
            this.vista.EstablecerAcciones();
        }


        public void PrepararNuevo()
        {
            this.vista.PrepararNuevo();

            this.MostrarDatosRegistro(true);
            this.MostrarDatosActualizacion(false);

            this.vista.PermitirRegistrarMantenimientos(true);
            this.vista.PermitirConfigurarMantenimientos(true);
        }
        public void PrepararEdicion()
        {
            this.vista.PrepararNuevo();

            this.MostrarDatosRegistro(false);
            this.MostrarDatosActualizacion(true);

            this.vista.PermitirRegistrarMantenimientos(true);
            this.vista.PermitirConfigurarMantenimientos(true);
        }
        public void PrepararVisualizacion()
        {
            this.vista.PrepararVisualizacion();

            this.MostrarDatosRegistro(true);
            this.MostrarDatosActualizacion(true);

            this.vista.PermitirConfigurarMantenimientos(false);
            this.vista.PermitirRegistrarMantenimientos(false);
        }
        
        public void MostrarDatosRegistro(bool mostrar)
        {
            this.vista.MostrarDatosRegistro(mostrar);
        }
        public void MostrarDatosActualizacion(bool mostrar)
        {
            this.vista.MostrarDatosActualizacion(mostrar);
        }

        public void DesplegarInformacion(UnidadBO unidad)
        {
            this.DatosAInterfazUsuario(unidad);

            this.vista.UsuarioCreacion = this.ObtenerNombreEmpleado(this.vista.UC);
            this.vista.UsuarioModificacion = this.ObtenerNombreEmpleado(this.vista.UUA);
        }

        #region SC0006
        public void DesplegarInformacionSiniestro(List<SiniestroUnidadBO> historialSiniestro)
        {
            this.vista.MostrarDatosSiniestro(historialSiniestro);            
        }
        #endregion

        private string ObtenerNombreEmpleado(int? numeroEmpleado)
		{
            if (numeroEmpleado == null) 
                return "";

            List<EmpleadoBO> empleadosBO = FacadeBR.ConsultarEmpleadoCompleto(FacadeBR.ObtenerConexion(), new EmpleadoBO() { Numero = numeroEmpleado });
            
			if (empleadosBO.Count == 0) 
                return "";

			return (empleadosBO[0].NombreCompleto != null ? empleadosBO[0].NombreCompleto : "");
		}
        private void DatosAInterfazUsuario(object obj)
        {
            UnidadBO bo = (UnidadBO)obj;

            this.vista.EquipoID = bo.EquipoID;
            this.vista.UnidadID = bo.UnidadID;
            this.vista.NumeroSerie = bo.NumeroSerie;

            if(bo.Area!=null)
                this.vista.Area = bo.Area.ToString();

            this.vista.EstatusUnidad = bo.EstatusActual;
            this.vista.UC = bo.UC;
            this.vista.UUA = bo.UUA;
            this.vista.FC = bo.FC;
            this.vista.FUA = bo.FUA;

            if (bo.Sucursal != null)
                this.vista.SucursalNombre = bo.Sucursal.Nombre;

            this.vista.PermitirConfigurarMantenimientos((bo.EstatusActual != null && bo.EstatusActual != EEstatusUnidad.NoDisponible));
            this.vista.PermitirRegistrarMantenimientos((bo.EstatusActual != null && bo.EstatusActual != EEstatusUnidad.NoDisponible));
        }
        #endregion
    }
}