//Esta clase satisface al CU085 - CATALOGO INFORMACION SEGURO DE LA UNIDAD
using System;
using System.Collections.Generic;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Tramites.VIS;
using BPMO.SDNI.Tramites.BO;

namespace BPMO.SDNI.Tramites.PRE
{
    public class ucSiniestroSeguroPRE
    {
        #region Atributos
        private IucSiniestroSeguroVIS vista;
        private string nombreClase = "ucSiniestroSeguroPRE";
        #endregion

        #region Constructores
        public ucSiniestroSeguroPRE(IucSiniestroSeguroVIS view)
        {
            try
            {
                this.vista = view;
            }
            catch (Exception ex)
            {
                this.vista.MostrarMensaje("Inconsistencias en los parámetros de configuración", ETipoMensajeIU.ERROR, this.nombreClase + ".ucSiniestroSeguroPRE:" + ex.Message);
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

            if (string.IsNullOrEmpty(this.vista.Descripcion) || string.IsNullOrWhiteSpace(this.vista.Descripcion))
                s += "Descripción del siniestro, ";
            if (string.IsNullOrEmpty(this.vista.Estatus) || string.IsNullOrWhiteSpace(this.vista.Estatus))
                s += "Estatus, ";
            if(!this.vista.Fecha.HasValue)
                s += "Fecha, ";
            if(string.IsNullOrEmpty(this.vista.Numero) || string.IsNullOrWhiteSpace(this.vista.Numero))
                s += "Número de Siniestro, ";
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
            try 
            {
                List<SiniestroBO> siniestros = this.vista.Siniestros;
                SiniestroBO siniestro = new SiniestroBO { Descripcion = vista.Descripcion, Estatus = this.vista.Estatus, Fecha = this.vista.Fecha.Value, Numero = this.vista.Numero };
                siniestros.Add(siniestro);
                this.vista.Siniestros = siniestros;
                this.vista.ActualizarLista();
                this.vista.LimpiarCampos();
            }
            catch (Exception ex){ throw new Exception(this.nombreClase + ".AgregarItem: " + ex.Message); }
        }

        public void RemoverElemento(int index)
        {
            try
            {
                if (index >= this.vista.Siniestros.Count || index < 0)
                    throw new Exception("No se encontró el siniestro seleccionado");

                List<SiniestroBO> siniestros = this.vista.Siniestros;
                SiniestroBO siniestro = siniestros[index];

                if (siniestro.SiniestroID.HasValue)
                {
                    siniestro.SiniestroID = siniestro.SiniestroID.Value * -1;
                    List<SiniestroBO> borrados = this.vista.SiniestrosBorrados;
                    borrados.Add(siniestro);
                    this.vista.SiniestrosBorrados = borrados;
                }

                siniestros.RemoveAt(index);

                this.vista.Siniestros = siniestros;
                this.vista.ActualizarLista();
            }
            catch (Exception ex) { throw new Exception(this.nombreClase + ".RemoverElemento: " + ex.Message); }
        }

        public void CambiarPaginaResultado(int index)
        {
            this.vista.IndicePaginaResultado = index;
            this.vista.ActualizarLista();
        }
        #region CU0004
        public void ActualizarEstatus(int index)
        {
            if (this.vista.Estatus == null)
            {
                this.vista.MostrarMensaje("Estatus no puede estar vacío", ETipoMensajeIU.ADVERTENCIA, null);
            }
            else
            {
                List<SiniestroBO> siniestros = this.vista.Siniestros;
                SiniestroBO siniestro = siniestros[index];
                siniestro.Estatus = this.vista.Estatus;
                siniestros.RemoveAt(index);
                siniestros.Add(siniestro);
                this.vista.ActualizarLista();
                this.ColumnasModoEdicion();
                this.vista.HabilitarControles();
                this.vista.Estatus = null;
            }
        }

        public void ColumnasModoEdicion()
        {
            this.vista.ReestablecerColumnas();
        }

        public void ColumnasModoRegistrar()
        {
            this.vista.ColumnasModoRegistrar();
        }

        #endregion

        #endregion


    }
}