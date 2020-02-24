using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BPMO.SDNI.Mantenimiento.VIS;
using BPMO.Servicio.Catalogos.BO;
using BPMO.Primitivos.Enumeradores;
using BPMO.Basicos.BO;
using BPMO.Facade.SDNI.BOF;
using BPMO.Facade.SDNI.BR;
using BPMO.Patterns.Creational.DataContext;

namespace BPMO.SDNI.Mantenimiento.PRE
{
    public class ucConfiguracionParametrosAuditoriaPRE
    {
        #region Propiedades
        private IucConfiguracionParametrosAuditoriaVIS vista;
        private IDataContext dataContext = null;
        #endregion

        #region Constructores

        public ucConfiguracionParametrosAuditoriaPRE(IucConfiguracionParametrosAuditoriaVIS vista)
        {
            try
            {
                this.vista = vista;
                this.dataContext = FacadeBR.ObtenerConexion();
            }
            catch (Exception ex)
            {
                this.vista.MostrarMensaje("No se pudieron obtener los datos de conexion", ETipoMensajeIU.ERROR,
                   "No se encontraron los parametros de conexion en la fuente de datos, póngase en contacto con el administrador del sistema. " + ex.ToString());
            }

        }

        #endregion

        #region Metodos

        private void CrearTalleres(SucursalBO sucursal)
        {
            List<TallerBO> talleres = FacadeBR.ConsultarTalleresPorSucursal(dataContext,sucursal);
            this.vista.enlazarControles(talleres);
        }

        #region Metodos buscador

        public object PrepararBOBuscador(string catalogo)
        {
            object obj = null;

            switch (catalogo)
            {
                case "Modelo":
                    ModeloBO modelo = new ModeloBO();
                    modelo.Nombre = this.vista.Modelo;
                    obj = modelo;
                    break;
                case "Sucursal":
                    SucursalBOF sucursal = new SucursalBOF();
                    sucursal.Nombre = this.vista.SucursalNombre;
                    sucursal.UnidadOperativa = new UnidadOperativaBO { Id = this.vista.UnidadOperativaId };
                    sucursal.Usuario = new UsuarioBO { Id = this.vista.UsuarioAutenticado };
                    obj = sucursal;
                    break;
            }

            return obj;
        }

        public void DesplegarResultadoBuscador(string catalogo, object selecto)
        {
            switch (catalogo)
            {
                case "Modelo":
                    ModeloBO modelo = (ModeloBO)selecto;

                    if (modelo != null && modelo.Id != null)
                    {
                        this.vista.Modelo = modelo.Nombre;
                        this.vista.ModeloID = (int)modelo.Id;
                      
                    }
                    else
                        this.vista.Modelo = null;

                    break;

                case "Sucursal":
                    SucursalBO sucursal = (SucursalBO)selecto;

                    if (sucursal != null && sucursal.Id != null)
                    {
                        this.vista.SucursalNombre = sucursal.Nombre;
                        this.vista.SucursalID = sucursal.Id;
                        this.CrearTalleres(sucursal);
                    }
                    else
                        this.vista.SucursalNombre = null;

                    break;
             
            }
        }

        #endregion

        #endregion
    }
}
