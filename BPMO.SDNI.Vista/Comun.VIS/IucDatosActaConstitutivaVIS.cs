//Satisface al CU068 - Catalogo de Clientes
using System;
using System.Collections.Generic;
using BPMO.Basicos.BO;

namespace BPMO.SDNI.Comun.VIS
{
    public interface IucDatosActaConstitutivaVIS
    {
        #region Propiedades

        DateTime? FechaEscritura { get; set; }

        DateTime? FechaRPPC { get; set; }

        List<CatalogoBaseBO> ListaAcciones { get; set; }

        string LocalidadNotaria { get; set; }

        string LocalidadRPPC { get; set; }

        string NombreNotario { get; set; }

        string NumeroEscritura { get; set; }

        string NumeroNotaria { get; set; }

        string NumeroRPPC { get; set; }

        int? ActaId { get; set; }

        bool? Activo { get; set; }

        int? UnidadOperativaID { get; set; }

        #endregion Propiedades

        #region Metodos

        void HabilitarCampos(bool habiliar, bool? veractivos=true);

        #endregion Metodos
    }
}