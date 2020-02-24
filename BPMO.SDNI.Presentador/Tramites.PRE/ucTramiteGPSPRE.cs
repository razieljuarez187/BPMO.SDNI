//Satisface al CU087 – Catálogo Tramite Unidad
using System;
using BPMO.Basicos.BO;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Tramites.BO;
using BPMO.SDNI.Tramites.VIS;

namespace BPMO.SDNI.Tramites.PRE
{
    public class ucTramiteGPSPRE
    {
        #region Atributos
        private string nombreClase = "ucTramiteGPSPRE";
        private IucTramiteGPSVIS vista;
        #endregion
        #region Constructor
        public ucTramiteGPSPRE(IucTramiteGPSVIS vista)
        {
            try
            {
                this.vista = vista;
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Error al inicializar el presentador",ETipoMensajeIU.ERROR, nombreClase + "ucTramiteGPSPRE : " + ex.Message);
            }
        }
        #endregion
        #region Métodos
        public void Inicializar()
        {
            this.PrepararNuevo();
        }
        public void PrepararNuevo()
        {
            this.LimpiarSesion();
            this.vista.FechaInstalacion = null;
            this.vista.Numero = null;
            this.vista.Compania = null;
        }
        public void LimpiarSesion()
        {
            this.vista.LimpiarSesion();
        }
        public void ModoEdicion(bool habilitar)
        {
            this.vista.ModoEdicion(habilitar);
        }
        public void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle=null)
        {
            this.vista.MostrarMensaje(mensaje,tipo,detalle);
        }
        public GPSBO InterfazUsuarioADato()
        {
            GPSBO gps = null;
            string s=string.Empty;
            if (String.IsNullOrEmpty(s = this.ValidarDatos()))
            {
                gps = new GPSBO();
                
                if (this.vista.UltimoObjetoGPS.TramiteID == null)
                {
                    gps.Auditoria = new AuditoriaBO { FC = this.vista.FC, UC = this.vista.UC, FUA = this.vista.FUA, UUA = this.vista.UUA };
                    gps.Activo = true;
                    gps.FechaInstalacion = this.vista.FechaInstalacion;
                    gps.Numero = this.vista.Numero;
                    gps.Compania = this.vista.Compania;
                    gps.Tipo = ETipoTramite.GPS;
                    gps.Resultado = gps.Numero;
                }
                else
                {
                    gps.TramiteID = this.vista.UltimoObjetoGPS.TramiteID;
                    gps.Auditoria = new AuditoriaBO { FUA=this.vista.FUA,UUA=this.vista.UUA, FC=this.vista.UltimoObjetoGPS.Auditoria.FC, UC=this.vista.UltimoObjetoGPS.Auditoria.UC};
                    gps.Activo = this.vista.UltimoObjetoGPS.Activo;
                    gps.FechaInstalacion = this.vista.FechaInstalacion;
                    gps.Numero = this.vista.Numero;
                    gps.Compania = this.vista.Compania;
                    gps.Tipo = this.vista.UltimoObjetoGPS.Tipo;
                    gps.Resultado = gps.Numero;
                    
                }
                
            }
            else
            {
                throw new Exception("Se requieren los siguientes datos : " + s);
            }
            return gps;
        }
        public string ValidarDatos()
        {
            string s = string.Empty;

            if (this.vista.Numero == null)
                s += " Número ID, ";
            if (this.vista.FechaInstalacion == null)
                s += " Fecha de Instalación, ";
            if (this.vista.Compania == null)
                s += " Compañía, ";

            return s;
        }
        public void DatoAInterfazUsuario(GPSBO gps){
            this.LimpiarSesion();
            this.Inicializar();
            this.vista.UltimoObjetoGPS = gps;
            this.vista.FechaInstalacion = gps.FechaInstalacion;
            this.vista.Numero = gps.Numero;
            this.vista.Compania = gps.Compania;
        }
        #endregion 
    }
}