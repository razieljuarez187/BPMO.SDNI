// Satisface al CU0009 - Iniciar Mantenimiento
using System;
using System.Linq;
using BPMO.Facade.SDNI.BR;
using BPMO.Patterns.Creational.DataContext;
using BPMO.SDNI.Mantenimiento.VIS;
using BPMO.SDNI.Mantenimiento.BO.BOF;
using BPMO.Servicio.Procesos.BO;
using BPMO.Servicio.Procesos.BR;

namespace BPMO.SDNI.Mantenimiento.PRE
{
    public class SincronizaEstatusPRE
    {
        #region Propiedades
        private IDataContext dataContext = null;
        private IRegisitrarUnidadVIS vista = null;
        #endregion

        #region Constructor
        public SincronizaEstatusPRE(IRegisitrarUnidadVIS vista)
        {
            this.vista = vista;

            try
            {
                this.dataContext = FacadeBR.ObtenerConexion();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Metodos
        /// <summary>
        /// Sincroniza el estatus de la orden de servicio
        /// </summary>
        public void SincronizaEstatus()
        {
            var mantenimientosVisita = this.vista.Mantenimientos;
            var mantenimientos = mantenimientosVisita.Where(x=> x.MantenimientoUnidad.OrdenServicio.Id != null).ToList();

            if (mantenimientos.Count > 0)
            {
                foreach (MantenimientoBOF mantenimientoBOF in mantenimientos)
                {
                    var mantenimiento = mantenimientoBOF.MantenimientoUnidad;
                    var osConsultadoUnidad = new OrdenServicioBR().Consultar(dataContext, mantenimiento.OrdenServicio).ConvertAll(x => (OrdenServicioBO)x).FirstOrDefault();
                    mantenimiento.OrdenServicio = osConsultadoUnidad;
                    if (mantenimientoBOF.Detalles != null
                        && mantenimientoBOF.Detalles.Count > 0)
                    {
                        var mantenimientosAliados = mantenimientoBOF.Detalles.Where(x=> x.MantenimientoAliado.OrdenServicio.Id != null).ToList();

                        foreach (MantenimientoBOF aliado in mantenimientosAliados)
                        {
                            var osConsultadoAliado = new OrdenServicioBR().Consultar(dataContext, aliado.MantenimientoAliado.OrdenServicio).ConvertAll(x => (OrdenServicioBO)x).FirstOrDefault();
                            aliado.MantenimientoAliado.OrdenServicio = osConsultadoAliado;
                        }
                    }
                }

                this.vista.CargarDatos();
            }
        }
        #endregion
    }
}
