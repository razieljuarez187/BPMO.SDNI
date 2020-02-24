//Satisface al CU019 - Reporte de Flota Activa de RD Registrados

using System;
using System.Collections.Generic;
using System.Configuration;
using BPMO.SDNI.Reportes.VIS;
using BPMO.Primitivos.Enumeradores;

namespace BPMO.SDNI.Reportes.UI
{
    /// <summary>
    /// Clase que representa una página ajustada para la trabajar con el master page BPMO.SDNI.Reporte.Reportes
    /// </summary>
    /// <remarks>
    /// La clase originalmente se llama ReportPage pero para que la validación de permisos de la clase SecurityBR
    /// pueda funcionar correctamente se debe de cambiar a Page
    /// </remarks>
    public abstract class Page : System.Web.UI.Page, IReportPageVIS
    {
        /// <summary>
        /// Identificador del Modulo
        /// </summary>
        public virtual int? ModuloID
        {
            get { return this.Master.ModuloID; }
        }

        /// <summary>
        /// Identificador del Usuario
        /// </summary>
        public int? UsuarioID
        {
            get { return this.Master.UsuarioID; }
        }

        /// <summary>
        /// Identificador de la Unidad Operativa
        /// </summary>
        public int? UnidadOperativaID
        {
            get { return this.Master.UnidadOperativaID; }
        }

        /// <summary>
        /// Obtiene una referencia hacia la masterpage
        /// </summary>
        public new Reportes Master
        {
            get
            {
                return base.Master as Reportes;
            }
        }

        /// <summary>
        /// Proceso que se ejecuta cuando se va a inicializa la página maestra
        /// </summary>
        /// <param name="e">Argumentos relacionados al proceso</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (!(base.Master is Reportes))
                throw new Exception(String.Format("La página maestra asociada a la página en curso debe de ser de tipo {0}"));
        }

        /// <summary>
        /// Método abstract que se ejecuta cuando 
        /// </summary>
        public abstract void Consultar();

        /// <summary>
        /// Despliega el visor de los formatos a imprimir
        /// </summary>
        public void IrAImprimir()
        {
            const string Url = "~/Buscador.UI/VisorReporteUI.aspx";
            Response.Redirect(Url, true);
        }

        /// <summary>
        /// Este método despliega un mensaje en pantalla
        /// </summary>
        /// <param name="mensaje">Mensaje a desplegar</param>
        /// <param name="tipo">Tipo de mensaje a desplegar</param>
        /// <param name="detalle">Muestra una cadena de texto que contiene los datalles del mensaje a desplegar</param>
        public void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null)
        {
            this.Master.MostrarMensaje(mensaje, tipo, detalle);           
        }

        /// <summary>
        /// Establece el Paquete de Navegacion para imprimir el reporte
        /// </summary>
        /// <param name="clave">Clave del Paquete de Navegacion</param>
        /// <param name="datosReporte">Datos del Reporte</param>
        public void EstablecerPaqueteNavegacionImprimir(string clave, Dictionary<string, object> datosReporte)
        {
            this.Session["NombreReporte"] = clave;
            this.Session["DatosReporte"] = datosReporte;
        }

        /// <summary>
        /// Realiza la redirección al visor correspondiente cuando se intenta acceder a la vista actual sin tener permisos asignados
        /// </summary>
        public void RedirigirSinPermisoAcceso()
        {
            this.Response.Redirect(this.ResolveUrl("~/MapaSitio.UI/PaginaSinAccesoUI.aspx"));
        }
    }
}