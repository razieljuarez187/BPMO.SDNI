// Satisface al Caso de uso CU015 - Registrar Contrato Full Service Leasing
// Satisface al Caso de uso CU022 - Consultar Contrato Full Service Leasing
using System;
using System.Collections.Generic;
using System.Linq;
using BPMO.Basicos.BO;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Contratos.FSL.BO;
using BPMO.SDNI.Contratos.FSL.VIS;
using BPMO.SDNI.Equipos.BO;
using BPMO.SDNI.Equipos.BR;
using BPMO.Servicio.Catalogos.BO;

namespace BPMO.SDNI.Contratos.FSL.PRE
{
    public class TarifasEquipoAliadoPRE
    {
        #region Atributos
        private readonly ITarifasEquipoAliadoVIS vista;
        private readonly IDataContext dataContext;
        
        private const string NombreClase = "TarifasEquipoAdicionalPRE";
        #endregion

        #region Contructores
        /// <summary>
        /// Constructor del Presentado
        /// </summary>
        /// <param name="vistaActual">Vista sobre la cual interactua el presentador</param>
        public TarifasEquipoAliadoPRE(ITarifasEquipoAliadoVIS vistaActual)
        {    
            try
            {
                vista = vistaActual;
                dataContext = Facade.SDNI.BR.FacadeBR.ObtenerConexion();
            }
            catch (Exception ex)
            {
                vista.MostrarMensaje("Inconsistencias en los parametros de configuración", ETipoMensajeIU.ERROR, NombreClase + ".TarifasEquipoAdicionalPRE: " + ex.Message);
            }
        }
        #endregion

        #region Metodos
        /// <summary>
        /// Inicializa el Presentador
        /// </summary>
        public void Inicializar()
        {
            MostrarDatosEquipoAliado(new EquipoAliadoBO { EquipoAliadoID = vista.EquipoAliadoID,  EquipoID = vista.EquipoID, Sucursal = new SucursalBO{ Id = vista.SucursalID, UnidadOperativa = new UnidadOperativaBO{ Id = vista.UnidadOperativaID }  } });

            if (vista.CargoAdicionalID != null)
            {
                if(vista.ModoConsultar)vista.ConfigurarModoConsultar();
                else
                    vista.ConsultarModoEditar();
                
                List<CargoAdicionalEquipoAliadoBO> CargosEquiposAliados = vista.ListadoCargosAdicionalesEquipoAliado;
                CargoAdicionalEquipoAliadoBO cargo = CargosEquiposAliados.Find(caea => caea.CobrableID == vista.CargoAdicionalID);

                if (cargo != null)
                {

                    vista.SinTarifas = (cargo.Tarifas == null || cargo.Tarifas.Count == 0);
                    if (vista.SinTarifas && !vista.ModoConsultar)
                    {
                        vista.EstablecerSinTarifas();
                    }
                    else
                    {
                        if (vista.SinTarifas && vista.ModoConsultar)
                        {
                            vista.DesplegarTarifas(cargo.Tarifas);
                            vista.ConfigurarTipoCargoConsultado(null);
                        }
                        else
                        {
                            vista.DesplegarTarifas(cargo.Tarifas);
                            vista.ConfigurarTipoCargoConsultado(cargo.Tarifas.First().CobraKm);
                        }
                    }
                }
            }
            else
            {
                vista.ConsultarModoEditar();

                List<CargoAdicionalEquipoAliadoBO> CargosEquiposAliados = vista.ListadoCargosAdicionalesEquipoAliado;
                CargoAdicionalEquipoAliadoBO cargo = CargosEquiposAliados.Find(caea => caea.EquipoAliado.EquipoAliadoID == vista.EquipoAliadoID && caea.TipoCotizacion == vista.TipoCotizacion);

                if (cargo != null)
                    vista.DesplegarTarifas(cargo.Tarifas);
                else
                {
                    cargo = new CargoAdicionalEquipoAliadoBO();
                    cargo.Tarifas = new List<TarifaFSLBO>();
                    vista.Inicializar();
                    vista.Inicializar(cargo.Tarifas);
                }                    
            }
        }

        /// <summary>
        /// Muesta la informacion del Equipo Aliado
        /// </summary>
        /// <param name="equipo"></param>
        private void MostrarDatosEquipoAliado(EquipoAliadoBO equipo)
        {
            if (equipo != null && (equipo.EquipoAliadoID != null || equipo.EquipoID != null))
            {

                // Numero de Serie
                var equipoBR = new EquipoAliadoBR();

                List<EquipoAliadoBO> listado = equipoBR.Consultar (dataContext, equipo);

                EquipoAliadoBO resultado = listado.Find(
                    ea => equipo.EquipoAliadoID == ea.EquipoAliadoID && ea.EquipoID == equipo.EquipoID);

                vista.NumeroSerie = resultado != null ? resultado.NumeroSerie : string.Empty;

                // Modelo
                if (resultado.Modelo != null && resultado.Modelo.Id != null)
                {
                    List<ModeloBO> Modelos = Facade.SDNI.BR.FacadeBR.ConsultarModelo(dataContext, resultado.Modelo);

                    ModeloBO resultadoModelo = Modelos.Find(mbo => resultado.Modelo.Id == mbo.Id);

                    vista.NombreModelo = resultadoModelo != null ? resultadoModelo.Nombre : string.Empty;
                }

            }
            else
                throw new Exception("Se requiere proporcionar un Equipo Aliado para desplegar su información.");
        }

        /// <summary>
        /// Agrega los cargos adicionales del equipo aliado a la coleccion principal
        /// </summary>
        public void AgregarCargoAdicionalEquipoAliado()
        {
            try
            {
                var listado = new List<CargoAdicionalEquipoAliadoBO>(vista.ListadoCargosAdicionalesEquipoAliado);

                var cargoViejo = listado.Find(
                    caea => caea.EquipoAliado.EquipoAliadoID == vista.CargoAdicional.EquipoAliado.EquipoAliadoID);

                if (cargoViejo != null)
                    listado.Remove(cargoViejo);

                listado.Add(vista.CargoAdicional);

                vista.ListadoCargosAdicionalesEquipoAliado = listado;
            }
            catch (Exception ex)
            {
                vista.MostrarMensaje("Inconsistencias al agregar las  tarifas por cargos adicionales de equipo aliado", ETipoMensajeIU.ERROR, NombreClase + ".AgregarCargoAdicionalEquipoAliado: " + ex.Message);
            }
        }
        /// <summary>
        /// Despliega los cargos adicionales del equipo aliado
        /// </summary>
        public void DesplegarCargoAdicionalEquipoAliado()
        {
            try
            {
                var listado = new List<CargoAdicionalEquipoAliadoBO>(vista.ListadoCargosAdicionalesEquipoAliado);

                var cargoViejo = listado.Find(
                    caea => caea.EquipoAliado.EquipoAliadoID == vista.CargoAdicional.EquipoAliado.EquipoAliadoID);

                if (cargoViejo != null)
                    vista.DesplegarTarifas(cargoViejo.Tarifas);

            }
            catch (Exception ex)
            {
                vista.MostrarMensaje("Inconsistencias al desplegar las  tarifas por cargos adicionales de equipo aliado", ETipoMensajeIU.ERROR, NombreClase + ".DesplegarCargoAdicionalEquipoAliado: " + ex.Message);
            }
        }
        #endregion
    }
}
