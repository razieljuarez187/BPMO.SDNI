//Esta clase satisface los requerimientos especificados en el caso de uso CU006 Consultar Flota de Renta Diaria
using System.Collections.Generic;
using System.Globalization;
using BPMO.SDNI.Equipos.BO;
using BPMO.SDNI.Flota.BO;
using BPMO.SDNI.Flota.VIS;
using BPMO.SDNI.Tramites.BO;
using BPMO.Servicio.Catalogos.BO;
using UnidadBO = BPMO.SDNI.Equipos.BO.UnidadBO;
using BPMO.Basicos.BO;

namespace BPMO.SDNI.Flota.PRE
{
    /// <summary>
    /// Presentador del control de usuario de los datos generales de la unidad
    /// </summary>
    public class ucDatosGeneralesElementoPRE
    {
        #region Atributos
        /// <summary>
        /// Vista del UC de datos generales de la unidad
        /// </summary>
        private readonly IucDatosGeneralesElementoVIS vistaDG;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor del presentador del user control de datos generales
        /// </summary>
        /// <param name="vista">vista relacionada al presentador</param>
        public ucDatosGeneralesElementoPRE(IucDatosGeneralesElementoVIS vista)
        {
            this.vistaDG = vista;
        }
        #endregion

        #region Métodos
        /// <summary>
        /// Se inicializan los controles con los valores pos defecto
        /// </summary>
        public void Inicializar()
        {
            this.vistaDG.Anio = string.Empty;
            this.vistaDG.CapacidadCarga = string.Empty;
            this.vistaDG.EquipoID = string.Empty;
            this.vistaDG.LiderID = string.Empty;
            this.vistaDG.Marca = string.Empty;
            this.vistaDG.Modelo = string.Empty;
            this.vistaDG.NumeroEconomico = string.Empty;
            this.vistaDG.Numeroserie = string.Empty;
            this.vistaDG.OracleID = string.Empty;
            this.vistaDG.PlacasEstatales = string.Empty;
            this.vistaDG.PlacasFederales = string.Empty;
            this.vistaDG.RendimientoTanque = string.Empty;
            this.vistaDG.Sucursal = string.Empty;
            this.vistaDG.UnidadID = string.Empty;
            this.vistaDG.ClaveProductoServicio = string.Empty;
            this.vistaDG.DescripcionProductoServicio = string.Empty;

            this.vistaDG.EstablecerAcciones();
        }
        /// <summary>
        /// Despliega un objeto de negocio en la vista
        /// </summary>
        /// <param name="obj">Objeto que se desea desplegar en la vista</param>
        public void DatoAInterfazUsuario(object obj)
        {
            ElementoFlotaBO elemento = obj as ElementoFlotaBO;
            UnidadBO unidad = elemento.Unidad;

            if (ReferenceEquals(unidad.CaracteristicasUnidad, null))
                unidad.CaracteristicasUnidad = new CaracteristicasUnidadBO();
            if (ReferenceEquals(unidad.Modelo, null))
                unidad.Modelo = new ModeloBO();
            if (ReferenceEquals(unidad.Modelo.Marca, null))
                unidad.Modelo.Marca = new MarcaBO();
            if (ReferenceEquals(elemento.Tramites, null))
                elemento.Tramites = new List<TramiteBO>();
            
            this.vistaDG.Anio = unidad.Anio.HasValue ? unidad.Anio.Value.ToString().Trim().ToUpper() : string.Empty;

            this.vistaDG.CapacidadCarga = unidad.CaracteristicasUnidad.PBCMaximoRecomendado.HasValue
                ? unidad.CaracteristicasUnidad.PBCMaximoRecomendado.Value.ToString(CultureInfo.InvariantCulture).Trim().ToUpper()
                : string.Empty;
            this.vistaDG.CapacidadTanque = unidad.CaracteristicasUnidad.CapacidadTanque.HasValue
                ? unidad.CaracteristicasUnidad.CapacidadTanque.Value.ToString(CultureInfo.InvariantCulture).Trim().ToUpper()
                : string.Empty;
            this.vistaDG.EquipoID = unidad.EquipoID.HasValue ? unidad.EquipoID.Value.ToString().Trim().ToUpper() : string.Empty;
            this.vistaDG.LiderID = unidad.IDLider.HasValue ? unidad.IDLider.Value.ToString().Trim().ToUpper() : string.Empty;
            this.vistaDG.Marca = !string.IsNullOrEmpty(unidad.Modelo.Marca.Nombre) && !string.IsNullOrWhiteSpace(unidad.Modelo.Marca.Nombre)
                ? unidad.Modelo.Marca.Nombre.Trim().ToUpper()
                : string.Empty;
            this.vistaDG.Modelo = !string.IsNullOrEmpty(unidad.Modelo.Nombre) && !string.IsNullOrWhiteSpace(unidad.Modelo.Nombre)
                ? unidad.Modelo.Nombre.Trim().ToUpper()
                : string.Empty;
            this.vistaDG.NumeroEconomico = !string.IsNullOrEmpty(unidad.NumeroEconomico) && !string.IsNullOrWhiteSpace(unidad.NumeroEconomico)
                ? unidad.NumeroEconomico.Trim().ToUpper()
                : string.Empty;
            this.vistaDG.Numeroserie = !string.IsNullOrEmpty(unidad.NumeroSerie) && !string.IsNullOrWhiteSpace(unidad.NumeroSerie)
                ? unidad.NumeroSerie.Trim().ToUpper()
                : string.Empty;
            this.vistaDG.OracleID = !string.IsNullOrEmpty(unidad.ClaveActivoOracle) && !string.IsNullOrWhiteSpace(unidad.ClaveActivoOracle)
                ? unidad.ClaveActivoOracle.Trim().ToUpper()
                : string.Empty;
            TramiteBO placarEstatal = elemento.ObtenerTramitePorTipo(ETipoTramite.PLACA_ESTATAL);
            if (ReferenceEquals(placarEstatal, null))
                this.vistaDG.PlacasEstatales = string.Empty;
            else
            {
                this.vistaDG.PlacasEstatales = !string.IsNullOrEmpty(placarEstatal.Resultado) &&
                                               !string.IsNullOrWhiteSpace(placarEstatal.Resultado)
                                                   ? placarEstatal.Resultado.Trim().ToUpper()
                                                   : string.Empty;
            }
            TramiteBO placaFederal = elemento.ObtenerTramitePorTipo(ETipoTramite.PLACA_FEDERAL);
            if (ReferenceEquals(placaFederal, null))
                this.vistaDG.PlacasFederales = string.Empty;
            else
            {
                this.vistaDG.PlacasFederales = !string.IsNullOrEmpty(placaFederal.Resultado) &&
                                               !string.IsNullOrWhiteSpace(placaFederal.Resultado)
                                                   ? placaFederal.Resultado.Trim().ToUpper()
                                                   : string.Empty;
            }
            this.vistaDG.RendimientoTanque = unidad.CaracteristicasUnidad.RendimientoTanque.HasValue
                ? unidad.CaracteristicasUnidad.RendimientoTanque.Value.ToString().Trim().ToUpper()
                : string.Empty;
            this.vistaDG.Sucursal = !string.IsNullOrEmpty(unidad.Sucursal.Nombre) && !string.IsNullOrWhiteSpace(unidad.Sucursal.Nombre)
                ? unidad.Sucursal.Nombre.Trim().ToUpper()
                : string.Empty;
            this.vistaDG.UnidadID = unidad.UnidadID.HasValue ? unidad.UnidadID.Value.ToString().Trim().ToUpper() : string.Empty;
        }
        /// <summary>
        /// Despliega la información del Producto/Servicio
        /// </summary>
        /// <param name="producto">Objeto con la información del producto</param>
        public void ProductoServicioAInterfazUsuario(ProductoServicioBO producto) {
            if (producto != null) {
                this.vistaDG.ClaveProductoServicio = producto.NombreCorto;
                this.vistaDG.DescripcionProductoServicio = producto.Nombre;
            }
        }
        /// <summary>
        /// Despliega la información del Producto/Servicio
        /// </summary>
        /// <param name="producto">Objeto con la información del producto</param>
        public void MostrarProductoServicio(bool mostrar) {
            this.vistaDG.MostrarProductoServicio(mostrar);
        }
        #endregion
    }
}