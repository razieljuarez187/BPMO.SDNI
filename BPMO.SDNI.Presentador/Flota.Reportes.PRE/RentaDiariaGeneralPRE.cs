//Satisface al caso de uso CU016 - Reporte de renta diaria general

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BPMO.Basicos.BO;
using BPMO.Facade.SDNI.BOF;
using BPMO.Facade.SDNI.BR;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Flota.Reportes.BR;
using BPMO.SDNI.Flota.Reportes.VIS;

namespace BPMO.SDNI.Flota.Reportes.PRE
{
    /// <summary>
    /// Presentador usado para el reporte general de renta diaria
    /// </summary>
    public class RentaDiariaGeneralPRE
    {
        #region Constantes
        /// <summary>
        /// Nombre de la clase para agregar a los mensajes de Error
        /// </summary>
        private static readonly string nombreClase = typeof(RentaDiariaGeneralPRE).Name;
        #endregion

        #region Atributos
        /// <summary>
        /// El DataContext que proveerá acceso a la base de datos
        /// </summary>
        private IDataContext dctx;

        /// <summary>
        /// Vista que se esta gestionando
        /// </summary>
        private IRentaDiariaGeneralVIS vista;

        /// <summary>
        /// Controlador del reporte general de renta diaria
        /// </summary>
        private RentaDiariaGeneralBR controlador;
        #endregion

        #region Contructor
        /// <summary>
        /// Constructor por default del presentador
        /// </summary>
        /// <param name="vista">Vista que será gestionada por el presentador</param>
        public RentaDiariaGeneralPRE(IRentaDiariaGeneralVIS vista)
        {
            try
            {
                this.vista = vista;
                this.dctx = FacadeBR.ObtenerConexion();
                this.controlador = new RentaDiariaGeneralBR();
            }
            catch (Exception ex)
            {
                this.vista.MostrarMensaje("Inconsistencias en la configuracion", ETipoMensajeIU.ERROR, nombreClase + ex.Message);
            }
        }
        #endregion

        #region Métodos
        /// <summary>
        /// Método para consultar el reporte 
        /// </summary>
        public void Consultar()
        {
            Hashtable parameters = new Hashtable();
            parameters["UnidadOperativaID"] = this.vista.UnidadOperativaID;
            parameters["ModuloID"] = this.vista.ModuloID;

            int? diaCorte = this.vista.DiaCorte.HasValue ? this.vista.DiaCorte : null;
            DateTime? fechaInicio = null;
            DateTime? fechaFin = null;
            try
            {
                switch(this.vista.TipoReporte)
                {
                    case 1:
                        fechaInicio = new DateTime(this.vista.Anio.Value, this.vista.PeriodoReporte.Value, 1);
                        fechaFin = new DateTime(this.vista.Anio.Value, this.vista.PeriodoReporte.Value, diaCorte != null ? (Int32)diaCorte : CalculoDia(1, fechaInicio.Value));
                        break;
                    case 3:
                        switch(this.vista.PeriodoReporte)
                        {
                            case 1:
                                fechaInicio = new DateTime(this.vista.Anio.Value, 1, 1);
                                fechaFin = new DateTime(this.vista.Anio.Value, 3, diaCorte != null ? (Int32)diaCorte : CalculoDia(3, fechaInicio.Value));
                                break;
                            case 2:
                                fechaInicio = new DateTime(this.vista.Anio.Value, 4, 1);
                                fechaFin = new DateTime(this.vista.Anio.Value, 6, diaCorte != null ? (Int32)diaCorte : CalculoDia(3, fechaInicio.Value));
                                break;
                            case 3:
                                fechaInicio = new DateTime(this.vista.Anio.Value, 7, 1);
                                fechaFin = new DateTime(this.vista.Anio.Value, 9, diaCorte != null ? (Int32)diaCorte : CalculoDia(3, fechaInicio.Value));
                                break;
                            case 4:
                                fechaInicio = new DateTime(this.vista.Anio.Value, 10, 1);
                                fechaFin = new DateTime(this.vista.Anio.Value, 12, diaCorte != null ? (Int32)diaCorte : CalculoDia(3, fechaInicio.Value));
                                break;
                        }
                        break;
                    case 6:
                        switch(this.vista.PeriodoReporte)
                        {
                            case 1:
                                fechaInicio = new DateTime(this.vista.Anio.Value, 1, 1);
                                fechaFin = new DateTime(this.vista.Anio.Value, 6, diaCorte != null ? (Int32)diaCorte : 30);
                                break;
                            case 2:
                                fechaInicio = new DateTime(this.vista.Anio.Value, 7, 1);
                                fechaFin = new DateTime(this.vista.Anio.Value, 12, diaCorte != null ? (Int32)diaCorte : 31);
                                break;
                        }
                        break;
                }
            }
            catch(Exception ex)
            {
                this.vista.MostrarMensaje("El día de corte especificado no es valido.", ETipoMensajeIU.ADVERTENCIA);
                return;
            }

            if(fechaInicio != null)
                parameters["FechaInicio"] = fechaInicio;
            if(fechaFin != null)
                parameters["FechaFin"] = fechaFin;

            if(this.vista.SucursalID.HasValue)
                parameters["SucursalID"] = new Int32[] { this.vista.SucursalID.Value };
            else
            {
                SucursalBOF sucursal = new SucursalBOF();
                sucursal.UnidadOperativa = new UnidadOperativaBO { Id = this.vista.UnidadOperativaID };
                sucursal.Usuario = new UsuarioBO { Id = this.vista.UsuarioID };
                sucursal.Activo = true;

                List<SucursalBOF> sucursalesPermitidas = FacadeBR.ConsultarSucursalesSeguridad(this.dctx, sucursal);
                if(sucursalesPermitidas.Count > 0)
                {
                    parameters["SucursalID"] = sucursalesPermitidas
                                                .Select(x => x.Id.Value)
                                                .ToArray();
                }
                else //Sino tiene sucursales asignadas al usuario en curso se manda una sucursal no existente
                    parameters["SucursalID"] = new Int32[] { -1000 };
            }

            Dictionary<String, Object> reportParameters = this.controlador.ConsultarReporteRDGeneral(this.dctx, parameters);
            if(!reportParameters.ContainsKey("Count"))
            {
                this.vista.MostrarMensaje("No se encontraron coincidencias con los filtros especificados", ETipoMensajeIU.ADVERTENCIA);
                return;
            }
            else if ((reportParameters["Count"] as Int32?) == 0)
            {
                this.vista.MostrarMensaje("No se encontraron coincidencias con los filtros especificados", ETipoMensajeIU.ADVERTENCIA);
                return;
            }
            
            this.vista.EstablecerPaqueteNavegacionImprimir("BEP1401.CU016", reportParameters);
            this.vista.IrAImprimir();
        }

        /// <summary>
        /// Crea un objeto de seguridad con los datos actuales de la sesión de usuario en curso
        /// </summary>
        /// <returns>Objeto de tipo seguridad</returns>
        private SeguridadBO CrearObjetoSeguridad()
        {
            UsuarioBO usuario = new UsuarioBO() { Id = this.vista.UsuarioID };
            AdscripcionBO adscripcion = new AdscripcionBO() { UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaID } };
            SeguridadBO seguridadBO = new SeguridadBO(Guid.Empty, usuario, adscripcion);

            return seguridadBO;
        }

        /// <summary>
        /// Valida el acceso a la sección en curso
        /// </summary>
        public void ValidarAcceso()
        {
            try
            {
                //Valida que usuario y la unidad operativa no sean nulos
                if(this.vista.UsuarioID == null) throw new Exception("El identificador del usuario no debe ser nulo");
                if(this.vista.UnidadOperativaID == null) throw new Exception("El identificador de la Unidad Operativa no debe ser nulo");
                //Se crea el objeto de seguridad                
                SeguridadBO seguridadBO = this.CrearObjetoSeguridad();

                //Se valida si el usuario tiene permiso a la acción principal
                if(!FacadeBR.ExisteAccion(this.dctx, "CONSULTAR", seguridadBO))
                    this.vista.RedirigirSinPermisoAcceso();
            }
            catch(Exception ex)
            {
                throw new Exception(nombreClase + ".ValidarAcceso: " + ex.GetBaseException().Message);
            }
        }

        /// <summary>
        /// Obtiene la fecha que sera usada para el dia de corte
        /// </summary>
        /// <param name="mesesAgregar">Cuanto meses se agregan para el calculo</param>
        /// <param name="fecha">Fecha a la que se le agregaran los meses</param>
        /// <returns>El dia de corte</returns>
        private int CalculoDia(int mesesAgregar, DateTime fecha)
        {
            var calculoDia = fecha.AddMonths(mesesAgregar);
            DateTime dia = calculoDia.Subtract(new TimeSpan(1, 0, 0, 0));

            return dia.Day;
        }

        #endregion
    }
}