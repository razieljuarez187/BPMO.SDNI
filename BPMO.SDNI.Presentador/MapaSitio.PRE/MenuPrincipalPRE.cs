//Satisface al CU062 - Menú Principal

//using BPMO.Patterns.Creational.DataContext;

using BPMO.SDNI.MapaSitio.VIS;

namespace BPMO.SDNI.MapaSitio.PRE
{
    public class MenuPrincipalPRE
    {
        #region Atributos
        private IMenuPrincipalVIS vista;
        //private IDataContext dataContext = null;
        #endregion
        
        #region Constructores
        /// <summary>
        /// Crea una instancia de la presentadora
        /// </summary>
        /// <param name="vistaActual"></param>
        public MenuPrincipalPRE(IMenuPrincipalVIS vistaActual)
        {
            this.vista = vistaActual;
            //if (this.vista.ListadoDatosConexion != null) {                            
            //    foreach (DatosConexionBO cnx in this.vista.ListadoDatosConexion) {
            //        if (dataContext == null) {
            //            dataContext = new DataContext(new DataProviderFactoryBPMO().GetProvider(cnx.TipoProveedor,
            //            cnx.BaseDatos, cnx.Usuario, cnx.Servidor, cnx.ServidorLigado), cnx.NombreProveedor);
            //        } else {
            //            dataContext.AddProvider(new DataProviderFactoryBPMO().GetProvider(cnx.TipoProveedor,
            //            cnx.BaseDatos, cnx.Usuario, cnx.Servidor, cnx.ServidorLigado), cnx.NombreProveedor);
            //        }
            //    }
            //}
        }
        #endregion
    }
}
