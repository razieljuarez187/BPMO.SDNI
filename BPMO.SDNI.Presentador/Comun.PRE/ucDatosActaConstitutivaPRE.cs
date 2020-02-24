//Satisface al CU068 - Catalogo de Clientes
using System;
using System.Linq;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Comun.VIS;

namespace BPMO.SDNI.Comun.PRE
{
    public class ucDatosActaConstitutivaPRE
    {
        #region atributos

        private IucDatosActaConstitutivaVIS vista;

        #endregion atributos

        #region Constructores

        public ucDatosActaConstitutivaPRE(IucDatosActaConstitutivaVIS vista)
        {
            this.vista = vista;
        }

        #endregion Constructores

        #region Metodos

        public void HabilitarCampos(bool habilitar)
        {
            vista.HabilitarCampos(habilitar);
        }

        public void LimpiarCampos()
        {
            vista.FechaEscritura = null;
            vista.FechaRPPC = null;
            vista.LocalidadNotaria = null;
            vista.LocalidadRPPC = null;
            vista.NombreNotario = null;
            vista.NumeroEscritura = null;
            vista.NumeroNotaria = null;
            vista.NumeroRPPC = null;            
            this.vista.ActaId = null;
            this.vista.Activo = null;
        }

        public void MostrarDatosActaConstitutiva(ActaConstitutivaBO actaConstitutiva)
        {
            if (actaConstitutiva == null)
            {
                LimpiarCampos();
                return;
            }
            vista.FechaEscritura = actaConstitutiva.FechaEscritura;
            vista.FechaRPPC = actaConstitutiva.FechaRPPC;
            vista.LocalidadNotaria = actaConstitutiva.LocalidadNotaria;
            vista.LocalidadRPPC = actaConstitutiva.LocalidadRPPC;
            vista.NombreNotario = actaConstitutiva.NombreNotario;
            vista.NumeroEscritura = actaConstitutiva.NumeroEscritura;
            vista.NumeroNotaria = actaConstitutiva.NumeroNotaria;
            vista.NumeroRPPC = actaConstitutiva.NumeroRPPC;
            this.vista.ActaId = actaConstitutiva.Id;
            this.vista.Activo = actaConstitutiva.Activo;
        }

        public ActaConstitutivaBO ObtenerActaConstitutiva()
        {
            return new ActaConstitutivaBO
                {
                    NumeroEscritura = vista.NumeroEscritura,
                    FechaEscritura = vista.FechaEscritura,
                    NombreNotario = vista.NombreNotario,
                    NumeroNotaria = vista.NumeroNotaria,
                    LocalidadNotaria = vista.LocalidadNotaria,
                    NumeroRPPC = vista.NumeroRPPC,
                    FechaRPPC = vista.FechaRPPC,
                    LocalidadRPPC = vista.LocalidadRPPC,
                    Id = this.vista.ActaId,
                    Activo = this.vista.Activo
                };
        }

        public string ValidarCampos(bool? validarEscritura = false, bool? representanteLegal = false)
        {
            string sError = string.Empty;
            ETipoEmpresa empresa = (ETipoEmpresa) this.vista.UnidadOperativaID;

            if (empresa == ETipoEmpresa.Idealease)
            {
                validarEscritura = true;
                //Si llega valor false es por que viene la ejecución de la sección de representantes del catálogo de clientes donde no es obligatoria esta información
                if (validarEscritura.Value)
                {
                    if (vista.NumeroEscritura == null)
                        sError += ", Número de Escritura";
                    if (vista.FechaEscritura == null)
                        sError += ", Fecha de Escritura";
                }
            }
            if (empresa == ETipoEmpresa.Idealease)
            {
                if (vista.NombreNotario == null)
                    sError += ", Nombre Notario";
                if (vista.NumeroNotaria == null)
                    sError += ", Número de Notaría";
                if (vista.LocalidadNotaria == null)
                    sError += ", Localidad Notaría";
                if (vista.LocalidadNotaria != null)
                {
                    if (vista.LocalidadNotaria.Split(',').Count() != 2)
                        sError += ", El formato de la Localidad Notaría no es válida";
                    else
                    {
                        String[] datos = vista.LocalidadNotaria.Split(',');
                        if (String.IsNullOrEmpty(datos[0].Trim()) || String.IsNullOrEmpty(datos[1].Trim()))
                            sError += ", El formato de la Localidad Notaría no es válida";
                    }
                }
                if (vista.LocalidadRPPC != null)
                {
                    if (vista.LocalidadRPPC.Split(',').Count() != 2)
                        sError += ", El formato de la Localidad de RPPC no es válida";
                    else
                    {
                        String[] datos = vista.LocalidadRPPC.Split(',');
                        if (String.IsNullOrEmpty(datos[0].Trim()) || String.IsNullOrEmpty(datos[1].Trim()))
                            sError += ", El formato de la Localidad de RPPC no es válida";
                    }
                }
            }

            if (empresa == ETipoEmpresa.Generacion || empresa == ETipoEmpresa.Construccion || empresa == ETipoEmpresa.Equinova)
            {
                if (this.vista.NumeroEscritura == null || this.vista.FechaEscritura == null )
                {
                    if (representanteLegal == false)
                        sError = "NOAPLICA";
                }
            }
            return sError;
        }

        #endregion Metodos
    }
}