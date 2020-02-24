//Satisface al CU087 – Catálogo Tramite Unidad
using System;
using BPMO.Basicos.BO;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Tramites.BO;
using BPMO.SDNI.Tramites.VIS;

namespace BPMO.SDNI.Tramites.PRE
{
    public class ucTramitePlacaPRE
    {
        #region Atributos
        private string nombreClase = "ucTramitePlacaPRE";
        private IucTramitePlacaVIS vista;
        #endregion
        #region Constructor
        public ucTramitePlacaPRE(IucTramitePlacaVIS vista)
        {
            try
            {
                this.vista = vista;
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Error en la configuración", ETipoMensajeIU.ERROR, nombreClase + "ucTramitePlacaPRE(): " +ex.Message);
            }
        }
        #endregion
        #region Métodos
        public void Inicializar(ETipoTramite? tipo)
        {
            if (tipo == null)
                throw new Exception(this.nombreClase+".Inicializar: Tipo de tramite no válido");
            this.EstablecerTipoTramite(tipo.Value);
            this.PrepararNuevo();
            
        }
        public void EstablecerTipoTramite(ETipoTramite tipo)
        {
            this.vista.tipo = tipo;
        }
        public void PrepararNuevo()
        {
            this.LimpiarSesion();
            this.vista.Numero = null;
            this.vista.NumeroGuia = null;
            this.vista.FechaEnvio = null;
            this.vista.FechaRecepcion = null;
            if (this.vista.tipo == ETipoTramite.PLACA_ESTATAL)
                this.vista.PlacaEstatal();
            if (this.vista.tipo == ETipoTramite.PLACA_FEDERAL)
                this.vista.PlacaFederal();
        }
        public void LimpiarSesion()
        {
            this.vista.LimpiarSesion();
        }
        public void ModoEdicion(bool habilitar)
        {
            this.vista.ModoEdicion(habilitar);
        }
        public void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null)
        {
            this.vista.MostrarMensaje(mensaje, tipo, detalle);
        }
        public object InterfazUsuarioADato()
        {
            object placa = null;
            if (this.vista.tipo == ETipoTramite.PLACA_ESTATAL)
            {
                string s = string.Empty;
                PlacaEstatalBO placaTemp = null;
                if (String.IsNullOrEmpty(s = this.ValidarDatos()))
                {
                    if (this.vista.UltimoObjeto == null)
                    {
                        placa = new object();
                        placaTemp = new PlacaEstatalBO();
                        placaTemp.Activo = true;
                        placaTemp.Auditoria = new AuditoriaBO { FC = this.vista.FC, FUA = this.vista.FUA, UC = this.vista.UC, UUA = this.vista.UUA };
                        placaTemp.Tipo = ETipoTramite.PLACA_ESTATAL;
                        placaTemp.Numero = this.vista.Numero;
                        placaTemp.Resultado = placaTemp.Numero;
                        placa= placaTemp;
                    }
                    else
                    {
                        if (this.vista.UltimoObjeto is PlacaEstatalBO)
                        {
                            placa = new object();
                            PlacaEstatalBO placaAnterior = (PlacaEstatalBO)this.vista.UltimoObjeto;
                            placaTemp = new PlacaEstatalBO();
                            placaTemp.Activo = placaAnterior.Activo;
                            placaTemp.Auditoria = new AuditoriaBO { FC = placaAnterior.Auditoria.FC, FUA = this.vista.FUA, UC = placaAnterior.Auditoria.UC, UUA = this.vista.UUA };
                            placaTemp.Tipo = placaAnterior.Tipo;
                            placaTemp.Numero = this.vista.Numero;
                            placaTemp.Resultado = placaTemp.Numero;
                            placaTemp.TramiteID = placaAnterior.TramiteID;
                            placa=placaTemp;
                        }
                        else
                            throw new Exception("Objeto no es PlacaEstatalBO");
                    }
                }
                else this.MostrarMensaje("Los siguientes Campos son Requeridos: " + s,ETipoMensajeIU.INFORMACION);
            }
            else if(this.vista.tipo == ETipoTramite.PLACA_FEDERAL)
            {
                string s = string.Empty;
                PlacaFederalBO placaTemp = null;
                if (String.IsNullOrEmpty(s = this.ValidarDatos()))
                {
                    if (this.vista.UltimoObjeto == null)
                    {
                        placa = new object();
                        placaTemp = new PlacaFederalBO();
                        placaTemp.Activo = true;
                        placaTemp.Auditoria = new AuditoriaBO { FC = this.vista.FC, FUA = this.vista.FUA, UC = this.vista.UC, UUA = this.vista.UUA };
                        placaTemp.Tipo = ETipoTramite.PLACA_FEDERAL;
                        placaTemp.Numero = this.vista.Numero;
                        placaTemp.NumeroGuia = this.vista.NumeroGuia;
                        placaTemp.FechaEnvioDocumentos = this.vista.FechaEnvio;
                        placaTemp.FechaRecepcion = this.vista.FechaRecepcion;
                        placaTemp.Resultado = placaTemp.Numero;
                        placa = placaTemp;
                    }
                    else
                    {
                        if (this.vista.UltimoObjeto is PlacaFederalBO)
                        {
                            placa = new object();
                            PlacaFederalBO placaAnterior = (PlacaFederalBO)this.vista.UltimoObjeto;
                            placaTemp = new PlacaFederalBO();							
                            placaTemp.Activo = this.vista.Activo;							
                            placaTemp.Auditoria = new AuditoriaBO { FC = placaAnterior.Auditoria.FC, FUA = this.vista.FUA, UC = placaAnterior.Auditoria.UC, UUA = this.vista.UUA };
                            placaTemp.Tipo = placaAnterior.Tipo;
                            placaTemp.Numero = this.vista.Numero;
                            placaTemp.NumeroGuia = this.vista.NumeroGuia;
                            placaTemp.FechaEnvioDocumentos = this.vista.FechaEnvio;
                            placaTemp.FechaRecepcion = this.vista.FechaRecepcion;
                            placaTemp.Resultado = placaTemp.Numero;
                            placaTemp.TramiteID = placaAnterior.TramiteID;
                            placa = placaTemp;
                        }
                    }
                }
                else
                    this.MostrarMensaje("Se requiere la siguiente información" + s,ETipoMensajeIU.INFORMACION);
            }
            return placa;
        }
        public string ValidarDatos()
        {
            string s = string.Empty;
            if (this.vista.tipo == ETipoTramite.PLACA_FEDERAL)
            {
                if (this.vista.Numero == null)
                    s += " Número, ";
                if (this.vista.FechaEnvio == null)
                    s += " Fecha de envío de documentos, ";
                if (this.vista.FechaRecepcion == null)
                    s += " Fecha de Recepción, ";
                if (this.vista.NumeroGuia == null)
                    s += " Número Guía, ";
                if (this.vista.FechaEnvio != null && this.vista.FechaRecepcion != null)
                {
                    if (this.vista.FechaEnvio > this.vista.FechaRecepcion)
                        s = " La Fecha de Recepción no debe ser menor que la fecha de envío, ";
                }
            }
            if(this.vista.tipo == ETipoTramite.PLACA_ESTATAL)
            {
                if (this.vista.Numero == null)
                    s += "Número, ";
            }
            return s;
        }
        public void DatoAInterfazUsuario(object placa)
        {
            
            if(placa is PlacaEstatalBO)
            {
                PlacaEstatalBO placaBO = (PlacaEstatalBO)placa;
                this.Inicializar(placaBO.Tipo);
                this.LimpiarSesion();
                this.vista.UltimoObjeto = placaBO;
                this.vista.Numero = placaBO.Numero;
            }
            if (placa is PlacaFederalBO)
            {
                PlacaFederalBO placaBO = (PlacaFederalBO)placa;
                this.Inicializar(placaBO.Tipo);
                this.LimpiarSesion();
                this.vista.UltimoObjeto = placaBO;
                this.vista.Numero = placaBO.Numero;
                this.vista.NumeroGuia = placaBO.NumeroGuia;
                this.vista.FechaEnvio = placaBO.FechaEnvioDocumentos;
                this.vista.FechaRecepcion = placaBO.FechaRecepcion;				
                this.vista.Activo = placaBO.Activo;				
            }

        }
        #endregion
    }
}
