//Esta clase satisface al CU085 - CATALOGO INFORMACION SEGURO DE LA UNIDAD
using System.Linq;
using BPMO.Basicos.BO;
using BPMO.SDNI.Equipos.BO;
using BPMO.SDNI.Tramites.VIS;
using BPMO.SDNI.Tramites.BO;

namespace BPMO.SDNI.Tramites.PRE
{
    public class ucSeguroDetallePRE
    {
        #region Atributos
        private IucSeguroDetalleVIS vista;
        #endregion

        #region Constructores
        public ucSeguroDetallePRE(IucSeguroDetalleVIS view)
        {
            this.vista = view;
        }
        #endregion

        #region Métodos
        public void DatoAInterfazUsuario(object obj)
        {
            SeguroBO seguro = (SeguroBO)obj;
            this.vista.Activo = seguro.Activo;
            this.vista.Aseguradora = seguro.Aseguradora;
            this.vista.Contacto = seguro.Contacto;
            this.vista.Deducibles = seguro.Deducibles;
            this.vista.Endosos = seguro.Endosos;
            this.vista.NumeroPoliza = seguro.NumeroPoliza;
            this.vista.Observaciones = seguro.Observaciones;
            this.vista.PrimaAnual = seguro.PrimaAnual;
            this.vista.PrimaSemestral = seguro.PrimaSemestral;
            this.vista.Siniestros = seguro.Siniestros;            
            this.vista.TramiteID = seguro.TramiteID;           
            this.vista.VigenciaFinal = seguro.VigenciaFinal;
            this.vista.VigenciaInicial = seguro.VigenciaInicial;            
            if(seguro.Tramitable != null)
            {
                if (seguro.Tramitable is TramitableProxyBO)
                {
                    this.vista.VIN = seguro.Tramitable.DescripcionTramitable;
                    this.vista.TipoTramitable = seguro.Tramitable.TipoTramitable;
                    this.vista.TramitableID = seguro.Tramitable.TramitableID;
                }
                else
                {
                    this.vista.VIN = seguro.Tramitable.DescripcionTramitable;
                    if ((((UnidadBO)seguro.Tramitable).Modelo) != null)
                        this.vista.Modelo = ((UnidadBO)seguro.Tramitable).Modelo.Nombre;
                    this.vista.TipoTramitable = seguro.Tramitable.TipoTramitable;
                    this.vista.TramitableID = seguro.Tramitable.TramitableID;
                }                
            }        
            if(this.vista.Endosos != null)
                if(this.vista.Endosos.Count > 0)
                {
                    decimal sumaendosos = this.vista.Endosos.Sum(x => x.Importe.Value);
                    this.vista.PrimaAnualTotal = sumaendosos + this.vista.PrimaAnual;
                    this.vista.TotalEndosos = sumaendosos;
                }
                    
        }

        public object InterfazUsuarioADato()
        {
            SeguroBO bo = new SeguroBO();
            bo.Auditoria = new AuditoriaBO();
            TramitableProxyBO tramitable = new TramitableProxyBO();
            bo.Activo = this.vista.Activo;
            bo.Aseguradora = this.vista.Aseguradora;
            bo.Contacto = this.vista.Contacto;
            if (this.vista.Deducibles.Count > 0)
                bo.Deducibles = this.vista.Deducibles;
            if (this.vista.Endosos.Count > 0)
                bo.Endosos = this.vista.Endosos;
            if (this.vista.Siniestros.Count > 0)
                bo.Siniestros = this.vista.Siniestros;
            bo.Auditoria.FC = this.vista.FC;
            bo.Auditoria.FUA = this.vista.FUA;
            bo.NumeroPoliza = this.vista.NumeroPoliza;
            bo.Observaciones = this.vista.Observaciones;
            bo.PrimaAnual = this.vista.PrimaAnual;
            bo.PrimaSemestral = this.vista.PrimaSemestral;
            bo.Resultado = this.vista.NumeroPoliza;
            bo.Tipo = this.vista.TipoTramite;
            if (this.vista.TipoTramitable.HasValue)
                tramitable.TipoTramitable = (ETipoTramitable)this.vista.TipoTramitable;
            else tramitable.TramitableID = null;
            tramitable.TramitableID = this.vista.TramitableID;
            bo.Tramitable = tramitable;
            bo.Auditoria.UC = this.vista.UC;
            bo.Auditoria.UUA = this.vista.UUA;
            bo.VigenciaFinal = this.vista.VigenciaFinal;
            bo.VigenciaInicial = this.vista.VigenciaInicial;
            bo.TramiteID = this.vista.TramiteID;            
            return bo;
        }
        #endregion
    }
}