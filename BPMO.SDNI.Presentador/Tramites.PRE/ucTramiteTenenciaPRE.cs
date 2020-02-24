//Satisface al CU087 – Catálogo Tramite Unidad
using System;
using System.Collections.Generic;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Comun.PRE;
using BPMO.SDNI.Comun.VIS;
using BPMO.SDNI.Tramites.BO;
using BPMO.SDNI.Tramites.VIS;

namespace BPMO.SDNI.Tramites.PRE
{
    public class ucTramiteTenenciaPRE
    {
        #region Atributos
        private IucTramiteTenenciaVIS vista;
        private ucCatalogoDocumentosPRE presentadorDocumentos;
        private string nombreClase = "ucTramiteTenenciaPRE";
        #endregion
        #region Contructores
        public ucTramiteTenenciaPRE(IucTramiteTenenciaVIS vista, IucCatalogoDocumentosVIS vistaDocumento)
        {
            try
            {
                this.vista = vista;
                presentadorDocumentos = new ucCatalogoDocumentosPRE(vistaDocumento);

            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Error al intentar inicializar el presentador", ETipoMensajeIU.ERROR, nombreClase + ".ucTramiteTenenciaPRE: " + ex.Message);
            }
        }
        public ucTramiteTenenciaPRE(IucTramiteTenenciaVIS vista)
        {
            try
            {
                this.vista = vista;
                

            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Error al intentar inicializar el presentador", ETipoMensajeIU.ERROR, nombreClase + ".ucTramiteTenenciaPRE: " + ex.Message);
            }
        }
        #endregion
        #region Métodos
        public void Inicializar()
        {
            this.LimpiarSesion();
            this.PrepararNuevo();
            this.presentadorDocumentos = new ucCatalogoDocumentosPRE(this.vista.VistaDocumentos);
            this.EstablecerTipoAdjunto();
            this.vista.EstablecerIdentificadorListaArchivos(nombreClase);
            this.presentadorDocumentos.LimpiarSesion();
            this.presentadorDocumentos.RequiereObservaciones(false);
        }

        public void EstablecerTiposdeArchivo(List<TipoArchivoBO> tipos)
        {
            this.vista.TiposArchivo = tipos;
        }
        public void EstablecerTipoAdjunto()
        {
            this.vista.EstablecerTipoAdjunto(ETipoAdjunto.Tramite);
        }
        public void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null)
        {
            this.vista.MostrarMensaje(mensaje,tipo,detalle);
        }
        public void ModoEdicion(bool Habilitar)
        {
            this.vista.ModoEdicion(Habilitar);
        }
        public void LimpiarSesion()
        {
            this.vista.LimpiarSesion();
        }
        public void PrepararNuevo()
        {
            this.vista.Archivos = null;
            this.vista.FechaPago = null;
            this.vista.Folio = null;
            this.vista.Importe = null;
            this.vista.UltimoObjetoTenencia = null;
        }
        public TenenciaBO InterfazUsuarioADato()
        {
            string validar = this.validarDatos();
            if (validar.Length < 1)
            {
                TenenciaBO tenencia = null;

                if (this.vista.UltimoObjetoTenencia.TramiteID == null)
                {
                    tenencia = new TenenciaBO();
                    if (this.vista.Archivos != null)
                        tenencia.Archivos = this.vista.Archivos;
                    else
                        tenencia.Archivos = new List<ArchivoBO>();

                    tenencia.Auditoria = new Basicos.BO.AuditoriaBO { FC = this.vista.FC, FUA = this.vista.FUA, UC = this.vista.UC, UUA = this.vista.UUA };
                    tenencia.FechaPago = this.vista.FechaPago;
                    tenencia.Folio = this.vista.Folio;
                    tenencia.Importe = this.vista.Importe;
                    tenencia.Tipo = ETipoTramite.TENENCIA;
                    tenencia.Resultado = tenencia.Folio;
                    tenencia.Activo = true;
                    if (tenencia.Archivos != null)
                    {
                        foreach (ArchivoBO archivo in tenencia.Archivos)
                        {
                            if (archivo.Id == null)
                            {
                                archivo.Auditoria = new Basicos.BO.AuditoriaBO { FC = tenencia.Auditoria.FC, UC = tenencia.Auditoria.UC, FUA = tenencia.Auditoria.FUA, UUA = tenencia.Auditoria.UUA };
                            }
                            else
                            {
                                archivo.Auditoria = new Basicos.BO.AuditoriaBO { FUA = tenencia.Auditoria.FUA, UUA = tenencia.Auditoria.UUA };
                            }
                        }
                    }
                }
                else
                {

                    tenencia = new TenenciaBO();
                    tenencia.Archivos = this.vista.Archivos;
                    tenencia.Auditoria = new Basicos.BO.AuditoriaBO { FUA = this.vista.FUA, UUA = this.vista.UUA, FC = this.vista.UltimoObjetoTenencia.Auditoria.FC, UC = this.vista.UltimoObjetoTenencia.Auditoria.UC };
                    tenencia.FechaPago = this.vista.FechaPago;
                    tenencia.Folio = this.vista.Folio;
                    tenencia.Importe = this.vista.Importe;
                    tenencia.Tipo = this.vista.UltimoObjetoTenencia.Tipo;
                    tenencia.Resultado = tenencia.Folio;
                    tenencia.Activo = this.vista.UltimoObjetoTenencia.Activo;
                    tenencia.TramiteID = this.vista.UltimoObjetoTenencia.TramiteID;
                    if (tenencia.Archivos != null)
                    {
                        foreach (ArchivoBO archivo in tenencia.Archivos)
                        {
                            if (archivo.Id == null)
                            {
                                archivo.Auditoria = new Basicos.BO.AuditoriaBO { FC = tenencia.Auditoria.FC, UC = tenencia.Auditoria.UC, FUA = tenencia.Auditoria.FUA, UUA = tenencia.Auditoria.UUA };
                            }
                            else
                            {
                                archivo.Auditoria = new Basicos.BO.AuditoriaBO { FUA = tenencia.Auditoria.FUA, UUA = tenencia.Auditoria.UUA };
                            }
                        }
                    }
                }

                return tenencia;
            }
            else
            {
                throw new Exception("Se requieren los siguientes datos : " + validar);
            }

        }
        public void DatoAInterfazUsuario(TenenciaBO tenencia)
        {
            this.LimpiarSesion();
            this.Inicializar();
            this.vista.UltimoObjetoTenencia = tenencia;
            this.vista.FechaPago = tenencia.FechaPago;
            this.vista.Folio = tenencia.Folio;
            this.vista.Importe = tenencia.Importe;
            this.presentadorDocumentos.RequiereObservaciones(false);
            if (tenencia.Archivos.Count > 0)
            {
                this.presentadorDocumentos.CargarListaArchivos(tenencia.Archivos, new List<TipoArchivoBO>());
                this.presentadorDocumentos.Vista.DespliegaArchivos();
            }
        }
        public string validarDatos()
        {
            string s="";
            if (this.vista.Importe == null)
                s += "Importe, ";
            if (this.vista.FechaPago == null)
                s += "Fecha de Pago, ";
            if (this.vista.Folio == null)
                s += "Folio, ";
            return s;
        }
        #endregion
    }
}
