using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BPMO.Basicos.BO;

namespace BPMO.SDNI.Equipos.VIS
{
    public interface IConsultarOrdenServicioVIS : IConsultarActaNacimientoVIS
    {
        List<DocumentoBaseBO> Resultados { get; set; }
    }
}
