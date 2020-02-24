//Satisface al CU087 – Catálogo Tramite Unidad
using System;
using BPMO.Basicos.BO;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Tramites.BO;
using BPMO.SDNI.Tramites.VIS;

namespace BPMO.SDNI.Tramites.PRE
{
    public class ucTramiteFiltroAKPRE
    {
        #region Atributos
        private string nombreClase = "ucTramiteFiltroAKPRE";
        private IucTramiteFiltroAKVIS vista;
        #endregion
        #region Constructor
        public ucTramiteFiltroAKPRE(IucTramiteFiltroAKVIS vista)
        {
            try
            {
                this.vista = vista;
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Error al inicializar el presentador",ETipoMensajeIU.ERROR, nombreClase + "ucTramiteFiltroAKPRE : " + ex.Message);
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
            this.vista.NumeroSerie = null;
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
        public FiltroAKBO InterfazUsuarioADato()
        {
            FiltroAKBO filtro = null;
            string s=string.Empty;
            if (String.IsNullOrEmpty(s = this.ValidarDatos()))
            {
                filtro = new FiltroAKBO();

                if (this.vista.UltimoObjetoFiltroAK.TramiteID == null)
                {
                    filtro.Auditoria = new AuditoriaBO { FC = this.vista.FC, UC = this.vista.UC, FUA = this.vista.FUA, UUA = this.vista.UUA };
                    filtro.Activo = true;
                    filtro.FechaInstalacion = this.vista.FechaInstalacion;
                    filtro.NumeroSerie = this.vista.NumeroSerie;
                    filtro.Tipo = ETipoTramite.FILTRO_AK;
                    filtro.Resultado = filtro.NumeroSerie;
                }
                else
                {
                    filtro.TramiteID = this.vista.UltimoObjetoFiltroAK.TramiteID;
                    filtro.Auditoria = new AuditoriaBO { FUA = this.vista.FUA, UUA = this.vista.UUA, FC = this.vista.UltimoObjetoFiltroAK.Auditoria.FC, UC = this.vista.UltimoObjetoFiltroAK.Auditoria.UC };
                    filtro.Activo = this.vista.UltimoObjetoFiltroAK.Activo;
                    filtro.FechaInstalacion = this.vista.FechaInstalacion;
                    filtro.NumeroSerie = this.vista.NumeroSerie;
                    filtro.Tipo = this.vista.UltimoObjetoFiltroAK.Tipo;
                    filtro.Resultado = filtro.NumeroSerie;
                }

            }
            else
            {
                throw new Exception("Se requieren los siguientes datos : " + s);
            }
            return filtro;
        }
        public string ValidarDatos()
        {
            string s = string.Empty;

            if (this.vista.NumeroSerie == null)
                s += " Número de Serie, ";
            if (this.vista.FechaInstalacion == null)
                s += " Fecha de Instalación, ";

            return s;
        }
        public void DatoAInterfazUsuario(FiltroAKBO filtro){
            this.LimpiarSesion();
            this.Inicializar();
            this.vista.UltimoObjetoFiltroAK = filtro;
            this.vista.FechaInstalacion = filtro.FechaInstalacion;
            this.vista.NumeroSerie = filtro.NumeroSerie;
        }
        #endregion
    }
}
