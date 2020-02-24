// Satisface al CU097 - Cargar y descargar formatos y plantillas
using System.Collections.Generic;
using BPMO.SDNI.Contratos.BO;
using BPMO.SDNI.Contratos.VIS;

namespace BPMO.SDNI.Contratos.PRE
{
    /// <summary>
    /// Presentador para el user control de documentos
    /// </summary>
    public class ucListadoPlantillasPRE
    {
        #region Atributos
        /// <summary>
        /// Vista del user control de docuemntos
        /// </summary>
        private readonly IucListadoPlantillasVIS vista;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor del presentador del user control de documentos
        /// </summary>
        /// <param name="vista">Vista del UserControl de documentos</param>
        public ucListadoPlantillasPRE(IucListadoPlantillasVIS vista)
        {
            this.vista = vista;
        }
        #endregion

        #region Métodos
        /// <summary>
        /// Carga los elementos de la consulta en la vista del User Control
        /// </summary>
        /// <param name="resultado">Lista con los elementos que serán cargados en la  UI</param>
        internal void CargarElementosEncontrados(List<object> resultado)
        {
            if (ReferenceEquals(this.vista.Documentos, null))
            {
                var lista = new List<PlantillaBO>();
                this.vista.Documentos = lista.ConvertAll(p => (object)p);
            }

            this.vista.CargarElementosEncontrados(this.vista.Documentos);
        }
        /// <summary>
        /// Establece el nuevo indice para el despliegue de los resultados
        /// </summary>
        /// <param name="nuevoIndicePagina">Indice que será aplicado a la vista </param>
        public void CambiarPaginaResultado(int nuevoIndicePagina)
        {
            this.vista.IndicePaginaResultado = nuevoIndicePagina;
            this.vista.ActualizarResultado();
        }
        #endregion        
    }
}