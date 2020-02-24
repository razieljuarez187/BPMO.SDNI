//Esta clase satisface al CU085 - CATALOGO INFORMACION SEGURO DE LA UNIDAD
using System;
using System.Collections.Generic;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Tramites.VIS;
using BPMO.SDNI.Tramites.BO;

namespace BPMO.SDNI.Tramites.PRE
{
    public class ucDeducibleSeguroPRE
    {
        #region Atributos
        private IucDeducibleSeguroVIS vista;
        private string nombreClase = "ucDeducibleSeguroPRE";
        #endregion

        #region Constructores
        public ucDeducibleSeguroPRE(IucDeducibleSeguroVIS view)
        {
            try
            {
                this.vista = view;
            }
            catch (Exception ex)
            {
                this.vista.MostrarMensaje("Inconsistencias en los parámetros de configuración", ETipoMensajeIU.ERROR, this.nombreClase + ".ucDeducibleSeguroPRE:" + ex.Message);
            }
        }
        #endregion

        #region Métodos
        public void PrepararNuevo()
        {
            this.vista.PrepararNuevo();
        }

        public void LimpiarSesion()
        {
            this.vista.LimpiarSesion();
        }

        private string ValidarCampos()
        {
            string s = string.Empty;

            if (string.IsNullOrEmpty(this.vista.Concepto) || string.IsNullOrWhiteSpace(this.vista.Concepto))
                s += "Concepto del deducible, ";
            if (!this.vista.Porcentaje.HasValue)
                s += "Porcentaje del deducible  ";            
            if (s.Trim().CompareTo("") != 0)
                return "Los siguientes campos no pueden estar vacíos: \n" + s.Substring(0, s.Length - 2);
            return null;
        }
        private string ValidarFormato()
        {
            string s = string.Empty;

            if (this.vista.Porcentaje.HasValue)
            {
                if (this.vista.Porcentaje.Value > 100)
                    s += "El porcentaje no es Valido, solo puede tomar valores de 0 a 100. ";
            }
            if (s.Trim().CompareTo("") != 0)
                return "Los siguientes campos no pueden estar vacíos: \n" + s.Substring(0, s.Length - 2);
            return null;
        }

        public void AgregarElemento()
        {
            string s;
            if ((s = this.ValidarCampos()) != null)
            {
                this.vista.MostrarMensaje(s, ETipoMensajeIU.ADVERTENCIA, null);
                return;
            }

            if ((s = this.ValidarFormato()) != null)
            {
                this.vista.MostrarMensaje(s, ETipoMensajeIU.ADVERTENCIA, null);
                return;
            }
            try 
            {
                List<DeducibleBO> deducibles = this.vista.Deducibles;
                DeducibleBO deducible = new DeducibleBO{ Concepto = this.vista.Concepto, Porcentaje = this.vista.Porcentaje};
                deducibles.Add(deducible);
                this.vista.Deducibles = deducibles;
                this.vista.ActualizarLista();
                this.vista.LimpiarCampos();
            }
            catch (Exception ex){ throw new Exception(this.nombreClase + ".AgregarItem: " + ex.Message); }
        }

        public void RemoverElemento(int index)
        {
            try 
            {
                if(index >= this.vista.Deducibles.Count || index < 0)
                    throw new Exception("No se encontró el deducible seleccionado");
                
                List<DeducibleBO> deducibles = this.vista.Deducibles;
                DeducibleBO deducible = deducibles[index];

                if (deducible.DeducibleID.HasValue)
                {
                    deducible.DeducibleID = deducible.DeducibleID * -1;
                    List<DeducibleBO> borrados = this.vista.DeduciblesBorrados;
                    borrados.Add(deducible);
                    this.vista.DeduciblesBorrados = borrados;
                }

                deducibles.RemoveAt(index);

                this.vista.Deducibles = deducibles;
                this.vista.ActualizarLista();
            }
            catch (Exception ex) { throw new Exception(this.nombreClase + ".RemoverElemento: " + ex.Message); }
        }

        public void CambiarPaginaResultado(int index)
        {
            this.vista.IndicePaginaResultado = index;
            this.vista.ActualizarLista();
        }
        #endregion
    }
}