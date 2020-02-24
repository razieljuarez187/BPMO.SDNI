// Satisface al caso de uso CU022 - Consultar Contratos Full Service Leasing
// Satisface al Caso de Uso CU023 - Editar Contrato Full Service Leasing
using System;
using System.Collections.Generic;
using System.Linq;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Contratos.FSL.BO;
using BPMO.SDNI.Contratos.FSL.VIS;
using BPMO.SDNI.Equipos.BO;
using BPMO.SDNI.Equipos.BOF;

namespace BPMO.SDNI.Contratos.FSL.PRE
{
    public class ucDatosRentaPRE
    {
        #region Atributos
        /// <summary>
        /// Vista sobre la que actua el presentador
        /// </summary>
        private readonly IucDatosRentaVIS vista;

        /// <summary>
        /// Nombre de la clase para agregar a los mensajes de error
        /// </summary>
        private const string NombreClase = "ucDatosRentaPRE";

        #endregion Atributos

        #region Propiedades
        /// <summary>
        /// Vista sobre la que actua el Presentador de solo lectura
        /// </summary>
        internal  IucDatosRentaVIS Vista { get { return vista; } }
        #endregion

        #region Constructores
        /// <summary>
        /// Constructor que recibe la vista sobre la que actuara el presentador
        /// </summary>
        /// <param name="vistaActual"></param>
        public ucDatosRentaPRE(IucDatosRentaVIS vistaActual)
        {
            if (vistaActual != null)
                vista = vistaActual;
        }

        #endregion

        #region Metodos

        /// <summary>
        /// Despliega la informacion del contrato en la Vista
        /// </summary>
        /// <param name="contrato"></param>
        public void DatosAInterfazUsuario(ContratoFSLBO contrato)
        {
            if (contrato == null) contrato = new ContratoFSLBO();

            vista.LineasContrato = (contrato.LineasContrato != null) ? contrato.LineasContrato.ConvertAll(s => (LineaContratoFSLBO)s) : null;
            vista.PlazoMeses = contrato.Plazo;
            int? plazo = contrato.CalcularPlazoEnAños();
            vista.PlazoAnios = plazo != null ? plazo.Value : 0;
            vista.UbicacionTaller = contrato.UbicacionTaller;
            vista.EstablecerIncluyeLavadoSeleccionado(contrato.IncluyeLavado);
            vista.EstablecerIncluyeLlantasSeleccionado(contrato.IncluyeLlantas);
            vista.EstablecerIncluyePinturaSeleccionado(contrato.IncluyePinturaRotulacion);
            vista.EstablecerIncluyeSeguroSeleccionado(contrato.IncluyeSeguro);
 
            vista.EstablecerFrecuenciaSeguro(contrato.FrecuenciaSeguro);

            vista.PermitirFrecuenciaSeguro(contrato.IncluyeSeguro != null && contrato.IncluyeSeguro == ETipoInclusion.NoIncluidoCargoCliente);
            vista.PorcentajeSeguro = contrato.PorcentajeSeguro;

        }

        /// <summary>
        /// Inicializa la vista
        /// </summary>
        public void Inicializar()
        {
            vista.EquipoID = null;
            vista.LineasContrato = null;
            vista.CargarListadoIncluyeLavado(null);
            vista.CargarListadoIncluyeLlantas(null);
            vista.CargarListadoIncluyePintura(null);
            vista.CargarListadoIncluyeSeguro(null);

            vista.CargarListadoFrecuenciaSeguro(null);
            vista.PorcentajeSeguro = null;

            vista.NumeroSerie = null;
            vista.PlazoMeses = null;
            vista.UbicacionTaller = null;
            vista.UnidadID = null;

            DesplegarTipoInclusiones();
        }

        /// <summary>
        /// Despliega el Listado de Tipo de Inclusiones
        /// </summary>
        private void DesplegarTipoInclusiones()
        {
            var tipos = new List<ETipoInclusion>(Enum.GetValues(typeof(ETipoInclusion)).Cast<ETipoInclusion>());

            var frecuenciasSeguro = new List<EFrecuenciaSeguro>(Enum.GetValues(typeof(EFrecuenciaSeguro)).Cast<EFrecuenciaSeguro>());

            vista.CargarListadoIncluyeLavado(tipos);
            vista.CargarListadoIncluyeLlantas(tipos);
            vista.CargarListadoIncluyePintura(tipos);
            vista.CargarListadoIncluyeSeguro(tipos);

            vista.CargarListadoFrecuenciaSeguro(frecuenciasSeguro);

        }

        /// <summary>
        /// Prepara un BO para la Busqueda en su respectivo catalogo
        /// </summary>
        /// <param name="catalogo">catalogo donde se realizara la busqueda</param>
        /// <returns></returns>
        public object PrepararBOBuscador(string catalogo)
        {
            object obj = null;

            switch (catalogo)
            {
                case "UnidadIdealease":
                    var unidad = new UnidadBOF();

                    if (!string.IsNullOrEmpty(vista.NumeroSerie))
                        unidad.NumeroSerie = vista.NumeroSerie;

                    unidad.EstatusActual = EEstatusUnidad.Disponible;
                    unidad.Area = EArea.FSL;

                    obj = unidad;
                    break;
            }

            return obj;
        }

        /// <summary>
        /// Despliega el Resultado del Buscador
        /// </summary>
        /// <param name="catalogo">Catalogo en el que se realizo la busqueda</param>
        /// <param name="selecto">Objeto Resultante</param>
        public void DesplegarResultadoBuscador(string catalogo, object selecto)
        {
            switch (catalogo)
            {
                case "UnidadIdealease":
                    var unidad = (UnidadBOF)selecto ?? new UnidadBOF();

                    vista.NumeroSerie = unidad.NumeroSerie ?? string.Empty;

                    vista.UnidadID = unidad.UnidadID;

                    vista.EquipoID = unidad.EquipoID;

                    vista.HabilitarAgregarUnidad(vista.UnidadID != null);
                    break;
            }
        }

        /// <summary>
        /// Agrega una linea de Contrato
        /// </summary>
        /// <param name="linea">Linea de Contrato a Agregar</param>
        public void AgregarLineaContrato(LineaContratoFSLBO linea)
        {
            try
            {
                if (linea != null)
                {
                    var unidad = linea.Equipo as UnidadBO;
                    if (unidad != null && unidad.UnidadID != null)
                    {
                        // Verificar Unidad en Lineas de Contrato
                        LineaContratoFSLBO lineaRepetida =
                            vista.LineasContrato.Find(li => ((UnidadBO) li.Equipo).UnidadID == unidad.UnidadID);
                        if (lineaRepetida != null)
                        {
                            linea.LineaContratoID = lineaRepetida.LineaContratoID;
                            vista.LineasContrato.Remove(lineaRepetida);
                        }

                        var lineasContrato = new List<LineaContratoFSLBO>(vista.LineasContrato) { linea };

                        vista.LineasContrato = lineasContrato;
                    }
                    else
                        throw new Exception("Se requiere una Unidad valida para agregarla al detalle del contrato.");
                }
                else
                    throw new Exception("No se ha proporcionado una linea de contrato");
            }
            catch (Exception ex)
            {
                vista.MostrarMensaje("Inconsistencias al Agregar una Unidad al contrato.", ETipoMensajeIU.ERROR, NombreClase + ".AgregarLineaContrato: " + ex.Message);
            }
        }

        /// <summary>
        /// Indica si la unidad pertenece al Contrato en Captura
        /// </summary>
        /// <param name="unidad"></param>
        /// <returns></returns>
        public bool ExisteUnidadContrato(UnidadBO unidad)
        {
            return (vista.LineasContrato.Find(li => ((UnidadBO)li.Equipo).UnidadID == unidad.UnidadID) != null);
        }

        /// <summary>
        /// Remueve una linea de contrato
        /// </summary>
        /// <param name="linea">Linea de Contrato a remover</param>
        public void RemoverLineaContrato(LineaContratoFSLBO linea)
        {
            try
            {
                if (linea != null)
                {
                    var unidad = linea.Equipo as UnidadBO;
                    if (unidad != null && unidad.UnidadID != null)
                    {
                        // Verificar Unidad en Lineas de Contrato
                        if (vista.LineasContrato.Find(li => ((UnidadBO) li.Equipo).UnidadID == unidad.UnidadID) != null)
                        {
                            var lineasContrato = new List<LineaContratoFSLBO>(vista.LineasContrato);
                            lineasContrato.Remove(linea);

                            vista.LineasContrato = lineasContrato;
                        }
                        else
                            throw new Exception("La unidad ya esta asignada al contrato");
                    }
                    else
                        throw new Exception("Se reguiere una Unidad valida para agregarla al detalle del contrato.");
                }
                else
                    throw new Exception("No se ha proporcionado una linea de contrato");
            }
            catch (Exception ex)
            {
                vista.MostrarMensaje("Inconsistencias en al remover la Unidad del contrato.", ETipoMensajeIU.ERROR, NombreClase + ".RemoverLineaContrato: " + ex.Message);
            }
        }

        public void InicializarAgregarUnidad()
        {
            vista.UnidadID = null;
            vista.EquipoID = null;
            vista.NumeroSerie = null;
            vista.HabilitarAgregarUnidad(false);
        }

        public void CalcularPlazoAnios()
        {
            try
            {
                if (vista.PlazoMeses != null)
                {
                    var calcularPlazoEnAños = new ContratoFSLBO {Plazo = vista.PlazoMeses}.CalcularPlazoEnAños();
                    if (calcularPlazoEnAños != null)
                        vista.PlazoAnios = calcularPlazoEnAños.Value;
                }
                else
                {
                    vista.PlazoAnios = 0;
                }
            }
            catch (Exception ex)
            {
                vista.MostrarMensaje("Inconsistencias al calcular el plazo en años.", ETipoMensajeIU.ERROR, NombreClase + ".CalcularPlazoAnios: " + ex.Message);
            }
        }

        public UnidadBO ObtenerUnidadAgregar()
        {
            return new UnidadBO
            {
                EquipoID = vista.EquipoID,
                NumeroSerie = vista.NumeroSerie,
                UnidadID = vista.UnidadID
            };
        }

        public void CambiarInclusionSeguro(ETipoInclusion? tipoInclusion)
        {
            try
            {
                vista.PermitirFrecuenciaSeguro(tipoInclusion == ETipoInclusion.NoIncluidoCargoCliente);
                vista.EstablecerFrecuenciaSeguro(null);
            }
            catch (Exception ex)
            {
                throw new Exception(NombreClase + ".CambiarInclusionSeguro(): " + ex.Message);
            }
        }

        #endregion Metodos
    }
}
