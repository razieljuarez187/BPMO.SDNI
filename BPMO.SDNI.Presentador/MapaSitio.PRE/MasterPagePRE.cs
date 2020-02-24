//Satisface al CU062 - Menú Principal
using System;
using System.Collections.Generic;
using System.Linq;
using BPMO.Basicos.BO;
using BPMO.Facade.SDNI.BR;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.MapaSitio.VIS;
using BPMO.Security.BO;
using System.Data;

namespace BPMO.SDNI.MapaSitio.PRE
{
    public class MasterPagePRE
    {
        #region Atributos
        private IMasterPageVIS vista;
        private IDataContext dataContext = null;
        #endregion
        
        #region Constructores
        /// <summary>
        /// Crea una instancia de la presentadora de la MasterPage principal
        /// </summary>
        /// <param name="vistaActual"></param>
        public MasterPagePRE(IMasterPageVIS vistaActual)
        {
            this.vista = vistaActual;
            if (this.vista.ListadoDatosConexion != null) {                            
                foreach (DatosConexionBO cnx in this.vista.ListadoDatosConexion) {
                    if (dataContext == null) {
                        dataContext = new DataContext(new DataProviderFactoryBPMO().GetProvider(cnx.TipoProveedor,
                        cnx.BaseDatos, cnx.Usuario, cnx.Servidor, cnx.ServidorLigado), cnx.NombreProveedor);
                    } else {
                        dataContext.AddProvider(new DataProviderFactoryBPMO().GetProvider(cnx.TipoProveedor,
                        cnx.BaseDatos, cnx.Usuario, cnx.Servidor, cnx.ServidorLigado), cnx.NombreProveedor);
                    }
                }
            }
        }
        #endregion

        #region Métodos
        /// <summary>
        /// Este método permite obtener los datos de conexión según el ambiente seleccionado por el usuario
        /// </summary>
        /// <param name="xmlAmbientes">Documento que contiene los ambientes</param>
        /// <param name="ambienteId">ID del ambiente a usar</param>
        /// <returns>Retorna un valor verdadero si la operación se realizó con éxito</returns>
        public bool ObtenerDatosDeConexion()
        {
            try
            {
                this.vista.ListadoDatosConexion = FacadeBR.ObtenerDatosConexion();
                return true;
            }
            catch (Exception)
            {
                this.vista.MostrarMensaje("No se pudieron obtener los datos de conexión", ETipoMensajeIU.ERROR,
                    "No se encontraron los parámetros de conexión en la fuente de datos, póngase en contacto con el administrador del sistema.");
            }
            return false;
        }
        /// <summary>
        /// Este método permite obtener los procesos a los que el usuario tiene permiso
        /// </summary>
        public void ObtenerProcesos()
        {
            try
            {
                if (this.vista.Adscripcion == null) return;
                SeguridadBO seguridadBO = new SeguridadBO(Guid.Empty, this.vista.Usuario, this.vista.Adscripcion);
                #region SC0008
                List<CatalogoBaseBO> listaPermisos = FacadeBR.ConsultarProceso(this.dataContext, new ProcesoBO(), seguridadBO);
                #endregion SC0008
                if (listaPermisos.Count > 0)
                {
                    this.vista.ListadoProcesos = listaPermisos.ConvertAll(p => (ProcesoBO)p);
                    this.vista.CargarProcesos();
                }
                else
                {
                    #region SC0008
                    this.vista.MostrarMensaje("Lo sentimos, usted no tiene permiso para ejecutar ninguna operación.", ETipoMensajeIU.ERROR,
                        "Usted no cuenta con permisos. Para mas información póngase en contacto con el administrador del sistema.");
                    this.vista.MenuPredeterminado();
                    #endregion SC0008
                }
            }
            catch (Exception)
            {
                this.vista.MostrarMensaje("Error de procesamiento", ETipoMensajeIU.ERROR, "La base de datos produjo una Excepción al intentar accederla. " +
                    "Intente de nuevo y si el problema persiste póngase en contacto con el administrador del sistema.");
            }
        }

        #region SC0008
        /// <summary>
        /// Obtiene los procesos raíz de la lista de procesos 
        /// </summary>
        public List<string> ObtenerProcesoRaiz(List<ProcesoBO> procesos)
        {
            var query = procesos.Where(p => p.UI == "#" || p.MenuPrincipal == true);
                foreach (var e in query)
                {
                    procesos = procesos.Where(p => p.ProcesoPadre != e.NombreCorto).ToList();
                }
                List<string> procesosRaiz = procesos.Where(p => p.ProcesoPadre != "#").Select(p => p.ProcesoPadre).Distinct().ToList();

                return procesosRaiz;
        }
        #endregion SC0008
        #endregion
    }
}
