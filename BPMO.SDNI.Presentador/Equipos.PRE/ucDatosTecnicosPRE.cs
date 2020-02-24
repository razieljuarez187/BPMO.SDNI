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
    public class ucDatosTecnicosPRE
    {
        #region Atributos
        private IucDatosTecnicosVIS vista;
        private string nombreClase = "DatosTecnicosPRE";
        #endregion

        #region Constructores
        public ucDatosTecnicosPRE(IucDatosTecnicosVIS view)
        {
            try
            {
                this.vista = view;
            }
            catch (Exception ex)
            {
                this.vista.MostrarMensaje("Inconsistencias en los parámetros de configuración", ETipoMensajeIU.ERROR, this.nombreClase + ".ucDatosTecnicosPRE:" + ex.Message);
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

        public void AgregarOdometros(List<OdometroBO> lst)
        {
            try
            {                
                this.vista.Odometros = lst;

                this.vista.ActualizarOdometros();
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".AgregarOdometros: " + ex.Message);
            }
        }
        public void AgregarOdometro()
        {
            string s;
            if ((s = this.ValidarCamposOdometro()) != null)
            {
                this.vista.MostrarMensaje(s, ETipoMensajeIU.INFORMACION, null);
                return;
            }

            try
            {
                List<OdometroBO> odometros = this.vista.Odometros;

                OdometroBO odometro = new OdometroBO();
                odometro.Activo = this.vista.EsOdometroActivo;
                odometro.KilometrajeInicio = this.vista.ValorInicialOdometro;
                odometro.KilometrajeFin = this.vista.ValorFinalOdometro;

                if (this.vista.Odometros == null)
                    this.vista.Odometros = new List<OdometroBO>();

                if (odometro.Activo != null && odometro.Activo == true)
                {
                    foreach (OdometroBO o in this.vista.Odometros)
                        o.Activo = false;
                }

                odometros.Insert(0, odometro);

                this.AgregarOdometros(odometros);

                this.vista.PrepararNuevoOdometro();
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".AgregarOdometro: " + ex.Message);
            }
        }
        private string ValidarCamposOdometro()
        {
            string s = "";

            if (this.vista.ValorInicialOdometro == null)
                s += "Kilometraje Inicial, ";
            if (this.vista.ValorFinalOdometro == null)
                s += "Kilometraje Final/Actual, ";
            if (this.vista.EsOdometroActivo == null)
                s += "Activo, ";

            if (s.Trim().CompareTo("") != 0)
                return "Los siguientes campos no pueden estar vacíos: \n" + s.Substring(0, s.Length - 2);

            if (this.vista.ValorInicialOdometro > this.vista.ValorFinalOdometro)
                return "El valor inicial no puede ser mayor al final";            

            return null;
        }

        public void AgregarHorometros(List<HorometroBO> lst)
        {
            try
            {                
                this.vista.Horometros = lst;

                this.vista.ActualizarHorometros();
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".AgregarHorometros: " + ex.Message);
            }
        }
        public void AgregarHorometro()
        {
            string s;
            if ((s = this.ValidarCamposHorometro()) != null)
            {
                this.vista.MostrarMensaje(s, ETipoMensajeIU.INFORMACION, null);
                return;
            }

            try
            {
                List<HorometroBO> horometros = this.vista.Horometros;

                HorometroBO horometro = new HorometroBO();
                horometro.Activo = this.vista.EsHorometroActivo;
                horometro.HoraInicio = this.vista.ValorInicialHorometro;
                horometro.HoraFin = this.vista.ValorFinalHorometro;

                if (this.vista.Horometros == null)
                    this.vista.Horometros = new List<HorometroBO>();

                if (horometro.Activo != null && horometro.Activo == true)
                {
                    foreach (HorometroBO h in this.vista.Horometros)
                        h.Activo = false;
                }

                horometros.Insert(0, horometro);

                this.AgregarHorometros(horometros);

                this.vista.PrepararNuevoHorometro();
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".AgregarHorometros: " + ex.Message);
            }
        }
        private string ValidarCamposHorometro()
        {
            string s = "";

            if (this.vista.ValorInicialHorometro == null)
                s += "Horas Inicial, ";
            if (this.vista.ValorFinalHorometro == null)
                s += "Horas Final/Actual, ";
            if (this.vista.EsHorometroActivo == null)
                s += "Activo, ";

            if (s.Trim().CompareTo("") != 0)
                return "Los siguientes campos no pueden estar vacíos: \n" + s.Substring(0, s.Length - 2);

            if (this.vista.ValorInicialHorometro > this.vista.ValorFinalHorometro)
                return "El valor inicial no puede ser mayor al final";     

            return null;
        }

        public void QuitarOdometro(int index)
        {
            try
            {
                if (index >= this.vista.Odometros.Count || index < 0)
                    throw new Exception("No se encontró el odómetro seleccionado");

                List<OdometroBO> odometros = this.vista.Odometros;
                odometros.RemoveAt(index);

                this.vista.Odometros = odometros;
                this.vista.ActualizarOdometros();
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".QuitarOdometro: " + ex.Message);
            }
        }
        public void QuitarHorometro(int index)
        {
            try
            {
                if (index >= this.vista.Horometros.Count || index < 0)
                    throw new Exception("No se encontró el horómetro seleccionado");

                List<HorometroBO> horometros = this.vista.Horometros;
                horometros.RemoveAt(index);

                this.vista.Horometros = horometros;
                this.vista.ActualizarHorometros();
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".QuitarHorometro: " + ex.Message);
            }
        }

        public bool PermitirQuitarOdometro(OdometroBO odometro)
        {
            if (odometro != null && odometro.OdometroID == null)
                return true;

            return false;
        }
        public bool PermitirQuitarHorometro(HorometroBO horometro)
        {
            if (horometro != null && horometro.HorometroID == null)
                return true;

            return false;
        }

        public string ValidarCamposBorrador()
        {
            //Todos los datos de esta vista son opcionales al guardar un BORRADOR
            return null;
        }
        public string ValidarCamposRegistro()
        {
            string s = "";

            if (this.vista.PBCMaximoRecomendado == null)
                s += "PBC Máximo Recomendado por el Fabricante, ";
            if (this.vista.PBVMaximoRecomendado == null)
                s += "PBV Máximo Recomendado por el Fabricante, ";
            if (this.vista.CapacidadTanque == null)
                s += "Capacidad del Tanque, ";
            if (this.vista.RendimientoTanque == null)
                s += "Rendimiento del Tanque, ";
            if (this.vista.Odometros.Count <= 0)
                s += "Odómetro, ";
            if (this.vista.Horometros.Count <= 0)
                s += "Horómetro, ";
            if (this.vista.Odometros != null && this.vista.Odometros.FindAll(p => p.Activo != null && p.Activo == true).Count <= 0)
                s += "Odómetro Activo, ";
            if (this.vista.Horometros != null && this.vista.Horometros.FindAll(p => p.Activo != null && p.Activo == true).Count <= 0)
                s += "Horómetro Activo, ";

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
