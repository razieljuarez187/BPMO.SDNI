//Satisface al CU077 - Registrar Acta de Nacimiento de una Unidad
//Satisface al CU079 - Consultar Acta de Nacimiento de Unidad
//Satisface al CU080 – Editar Acta de Nacimiento de una Unidad
using System;

using System.Collections.Generic;

using BPMO.Primitivos.Enumeradores;

using BPMO.SDNI.Equipos.BO;

using BPMO.SDNI.Equipos.VIS;

namespace BPMO.SDNI.Equipos.PRE
{
    public class ucNumerosSeriePRE
    {
        #region Atributos
        private IucNumerosSerieVIS vista;
        private string nombreClase = "NumerosSeriePRE";
        #endregion

        #region Constructores
        public ucNumerosSeriePRE(IucNumerosSerieVIS view)
        {
            try
            {
                this.vista = view;
            }
            catch (Exception ex)
            {
                this.vista.MostrarMensaje("Inconsistencias en los parámetros de configuración", ETipoMensajeIU.ERROR, this.nombreClase + ".ucNumerosSeriePRE:" + ex.Message);
            }
        }
        #endregion

        #region Métodos
        public void PrepararNuevo()
        {
            this.vista.PrepararNuevo();
            this.vista.HabilitarModoEdicion(true);
        }
        public void PrepararEdicion()
        {
            this.vista.HabilitarModoEdicion(true);
        }
        public void PrepararVisualizacion()
        {
            this.vista.HabilitarModoEdicion(false);
        }
       
        public void AgregarNumerosSerie(List<NumeroSerieBO> lst) {
            try {
                this.vista.NumerosSerie = lst;

                this.vista.ActualizarNumeroSerie();
            }
            catch (Exception ex) {
                throw new Exception(this.nombreClase + ".AgregarNumerosSerie: " + ex.Message);
            }
        }
        
        public void AgregarNumeroSerie() {
            string s;
            if ((s = this.ValidarCamposNumeroSerie()) != null) {
                this.vista.MostrarMensaje(s, ETipoMensajeIU.ADVERTENCIA, null);
                return;
            }

            try {
                List<NumeroSerieBO> numerosSerie = this.vista.NumerosSerie;

                NumeroSerieBO numeroSerie = new NumeroSerieBO();
                numeroSerie.Nombre = this.vista.Nombre;
                numeroSerie.NumeroSerie = this.vista.Serie;

                if (this.vista.NumerosSerie == null)
                    this.vista.NumerosSerie = new List<NumeroSerieBO>();

                numerosSerie.Add(numeroSerie);

                this.AgregarNumerosSerie(numerosSerie);

                this.vista.PrepararNuevoNumeroSerie();
            }
            catch (Exception ex) {
                throw new Exception(this.nombreClase + ".AgregarNumeroSerie: " + ex.Message);
            }
        }

        private string ValidarCamposNumeroSerie() {
            string s = "";

            if (this.vista.Nombre == null)
                s += "Nombre del articulo, ";
            if (this.vista.Serie == null)
                s += "Numero de serie del articulo, ";

            if (s.Trim().CompareTo("") != 0)
                return "Los siguientes campos no pueden estar vacíos: \n" + s.Substring(0, s.Length - 2);

            if (this.vista.NumerosSerie != null && this.vista.NumerosSerie.Exists(p => p.NumeroSerie != null && this.vista.Serie != null && p.NumeroSerie == this.vista.Serie && p.Nombre == this.vista.Nombre))
                return "Ya existe un articulo asignado con esa serie";

            return null;
        }

        public void QuitarNumeroSerie(int index) {
            try {
                if (index >= this.vista.NumerosSerie.Count || index < 0)
                    throw new Exception("No se encontró el numeroSerie seleccionado");

                List<NumeroSerieBO> numerosSerie = this.vista.NumerosSerie;
                numerosSerie.RemoveAt(index);

                this.vista.NumerosSerie = numerosSerie;
                this.vista.ActualizarNumeroSerie();
            }
            catch (Exception ex) {
                throw new Exception(this.nombreClase + ".QuitarNumeroSerie: " + ex.Message);
            }
        }

        public bool PermitirQuitarNumeroSerie(NumeroSerieBO numeroSerie) {
            if (numeroSerie != null && numeroSerie.NumeroSerie == null)
                return true;
            
            return false;
        }
        
        public string ValidarCamposBorrador()
        {
            //Todos los datos de esta vista son opcionales al guardar un BORRADOR excepto si algun dato de un eje esta capturado
            string s = null;
            if (this.vista.EjeDireccionModelo != null | this.vista.EjeDireccionSerie != null)
            {
                if (this.vista.EjeDireccionModelo == null)
                    s += "Modelo de Eje de Dirección, ";
                if (this.vista.EjeDireccionSerie == null)
                    s += "Serie de Eje de Dirección, ";
            }
            if (this.vista.EjeTraseroDelanteroModelo != null | this.vista.EjeTraseroDelanteroSerie != null)
            {
                if (this.vista.EjeTraseroDelanteroModelo == null)
                    s += "Modelo de Eje Trasero Delantero, ";
                if (this.vista.EjeTraseroDelanteroSerie == null)
                    s += "Serie de Eje Trasero Delantero, ";
            }
            if (this.vista.EjeTraseroTraseroModelo != null | this.vista.EjeTraseroTraseroSerie != null)
            {
                if (this.vista.EjeTraseroTraseroModelo == null)
                    s += "Modelo de Eje Trasero Trasero, ";
                if (this.vista.EjeTraseroTraseroSerie == null)
                    s += "Serie de Eje Trasero Trasero, ";
            }
            if (s != null)
                s = "Los siguientes campos no pueden estar vacíos: \n" + s.Substring(0, s.Length - 2);
            return s;
        }
        public string ValidarCamposRegistro()
        {
            string s = "";

            if (!(this.vista.Radiador != null && this.vista.Radiador.Trim().CompareTo("") != 0))
                s += "Radiador, ";
            if (!(this.vista.PostEnfriador != null && this.vista.PostEnfriador.Trim().CompareTo("") != 0))
                s += "Post-Enfriador, ";
            #region SC0030
            if (!(this.vista.SerieMotor != null && this.vista.SerieMotor.Trim().CompareTo("") != 0))
                s += "Serie del Motor, ";
            #endregion
            if (!(this.vista.SerieTurboCargador != null && this.vista.SerieTurboCargador.Trim().CompareTo("") != 0))
                s += "Serie del Turbo Cargador del Motor, ";
            if (!(this.vista.SerieCompresorAire != null && this.vista.SerieCompresorAire.Trim().CompareTo("") != 0))
                s += "Serie del Compresor de Aire del Motor, ";
            if (!(this.vista.SerieECM != null && this.vista.SerieECM.Trim().CompareTo("") != 0))
                s += "Serie del ECM del Motor, ";

            if (!(this.vista.SerieAlternador != null && this.vista.SerieAlternador.Trim().CompareTo("") != 0))
                s += "Serie del Alternador, ";
            if (!(this.vista.SerieMarcha != null && this.vista.SerieMarcha.Trim().CompareTo("") != 0))
                s += "Serie de la Marcha del Sistema Eléctrico, ";
            if (!(this.vista.SerieBaterias != null && this.vista.SerieBaterias.Trim().CompareTo("") != 0))
                s += "Serie de las Baterías del Sistema Eléctrico, ";

            if (!(this.vista.EjeDireccionModelo != null && this.vista.EjeDireccionModelo.Trim().CompareTo("") != 0))
                s += "Modelo del Eje de Dirección, ";
            if (!(this.vista.EjeDireccionSerie != null && this.vista.EjeDireccionSerie.Trim().CompareTo("") != 0))
                s += "Serie del Eje de Dirección, ";
            if (!(this.vista.EjeTraseroDelanteroModelo != null && this.vista.EjeTraseroDelanteroModelo.Trim().CompareTo("") != 0))
                s += "Modelo del Eje Trasero Delantero, ";
            if (!(this.vista.EjeTraseroDelanteroSerie != null && this.vista.EjeTraseroDelanteroSerie.Trim().CompareTo("") != 0))
                s += "Serie del Eje Trasero Delantero, ";
            if (!(this.vista.EjeTraseroTraseroModelo != null && this.vista.EjeTraseroTraseroModelo.Trim().CompareTo("") != 0))
                s += "Modelo del Eje Trasero Trasero, ";
            if (!(this.vista.EjeTraseroTraseroSerie != null && this.vista.EjeTraseroTraseroSerie.Trim().CompareTo("") != 0))
                s += "Serie del Eje Trasero Trasero, ";

            if (s.Trim().CompareTo("") != 0)
                return "Los siguientes campos no pueden estar vacíos: \n" + s.Substring(0, s.Length - 2);

            return null;
        }

        public void LimpiarSesion()
        {
            this.vista.LimpiarSesion();
        }
        #endregion
    }
}