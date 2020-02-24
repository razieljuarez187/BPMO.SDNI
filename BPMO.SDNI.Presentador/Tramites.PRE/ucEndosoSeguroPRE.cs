//Esta clase satisface al CU085 - CATALOGO INFORMACION SEGURO DE LA UNIDAD
using System;
using System.Collections.Generic;
using System.Linq;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Tramites.VIS;
using BPMO.SDNI.Tramites.BO;

namespace BPMO.SDNI.Tramites.PRE
{
    public class ucEndosoSeguroPRE
    {
        #region Atributos
        private IucEndosoSeguroVIS vista;
        private string nombreClase = "ucEndosoSeguroPRE";
        #endregion

        #region Constructores
        public ucEndosoSeguroPRE(IucEndosoSeguroVIS view)
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
            string s = String.Empty;
            if(string.IsNullOrEmpty(this.vista.Motivo) || string.IsNullOrWhiteSpace(this.vista.Motivo))
                s += "Motivo del endoso, ";
            if(!this.vista.Importe.HasValue)
                s += "Importe del endoso  ";
            if (s.Trim().CompareTo("") != 0)
                return "Los siguientes campos no pueden estar vacíos: \n" + s.Substring(0, s.Length - 2);

            return null;
        }

        public void AgregarItem()
        {
            string s;
            if ((s = this.ValidarCampos()) != null)
            {
                this.vista.MostrarMensaje(s, ETipoMensajeIU.ADVERTENCIA, null);
                return;
            }
            try
            {
                List<EndosoBO> endosos = this.vista.Endosos;
                EndosoBO endoso = new EndosoBO{ Importe = this.vista.Importe.Value, Motivo = this.vista.Motivo };
                endosos.Add(endoso);

                this.vista.Endosos = endosos;

                this.CalcularTotales();//RI0015

                this.vista.ActualizarLista();
                this.vista.LimpiarCampos();
            }
            catch (Exception ex){ throw new Exception(this.nombreClase + ".AgregarItem: " + ex.Message); }
        }

        public void RemoverElemento(int index)
        {
            try
            {
                if (index >= this.vista.Endosos.Count || index < 0)
                    throw new Exception("No se encontró el deducible seleccionado");

                List<EndosoBO> endosos = this.vista.Endosos;
                EndosoBO endoso = endosos[index];

                if (endoso.EndosoID.HasValue)
                {
                    endoso.EndosoID = endoso.EndosoID.Value * -1;
                    List<EndosoBO> borrados = this.vista.EndososBorrados;
                    borrados.Add(endoso);
                    this.vista.EndososBorrados = borrados;
                }

                endosos.RemoveAt(index);

                this.vista.Endosos = endosos;

                this.CalcularTotales();//RI0015

                this.vista.ActualizarLista();
            }
            catch (Exception ex) { throw new Exception(this.nombreClase + ".RemoverElemento: " + ex.Message); }
        }

        public void CambiarPaginaResultado(int index)
        {
            this.vista.IndicePaginaResultado = index;
            this.vista.ActualizarLista();
        }

        /// <summary>
        /// RI0015
        /// Ejecuta el cálculo de los importes totales para los endosos
        /// </summary>
        internal void CalcularTotales()
        {
            if (this.vista.Endosos != null) { 
                if (this.vista.Endosos.Count > 0)
                {
                    decimal sumaendosos = this.vista.Endosos.Sum(x => x.Importe.Value);
                    if (this.vista.PrimaAnual.HasValue)
                        this.vista.PrimaAnualTotal = sumaendosos + this.vista.PrimaAnual;
                    this.vista.TotalEndosos = sumaendosos;
                }
                else
                {
                    this.vista.PrimaAnual = null;
                    this.vista.TotalEndosos = null;
                }
            }
            else
            {
                this.vista.PrimaAnual = null;
                this.vista.TotalEndosos = null;
            }
        }
        #endregion        
    }
}