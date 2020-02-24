//Satisface al CU062 - Menú Principal
using System.Collections.Generic;
using BPMO.Basicos.BO;
using BPMO.Facade.SDNI.BR;

namespace BPMO.SDNI.Buscador.PRE
{
    public class BuscadorIdealeasePRE
    {
        /// <summary>
        /// Este método permite obtener los datos de conexión según el ambiente seleccionado por el usuario
        /// </summary>
        /// <param name="xmlAmbientes">Documento que contiene los ambientes</param>
        /// <param name="ambienteId">ID del ambiente a usar</param>
        /// <returns>Retorna un valor verdadero si la operación se realizó con éxito</returns>
        public List<DatosConexionBO> ObtenerDatosDeConexion()
        {
            try
            {
                return FacadeBR.ObtenerDatosConexion();
            }
            catch
            {
                return null;
            }            
        }
    }
}
