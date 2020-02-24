// Satisface al CU027 - Catálogo Contacto Cliente
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BPMO.SDNI.Mantenimiento.VIS;
using BPMO.Patterns.Creational.DataContext;
using BPMO.SDNI.Mantenimiento.BR;
using BPMO.Facade.SDNI.BR;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Comun.BOF;
using BPMO.Basicos.BO;
using BPMO.Facade.SDNI.BOF;

namespace BPMO.SDNI.Mantenimiento.PRE {

    /// <summary>
    /// Presentador aplicable para la gestión de las peticiones del uc Contactos Clientes Idealease.
    /// </summary>
    public class ucContactoClientePRE {

        #region Atributos

        /// <summary>
        /// La vista que esta mostrando los datos procesados en el presentador.
        /// </summary>
        private IucContactoClienteVIS vista;

        /// <summary>
        /// El DataContext que proveerá acceso a la base de datos.
        /// </summary>
        private IDataContext dataContext;

        /// <summary>
        /// Controlador de Detalle Contactos Cliente Idealease.
        /// </summary>
        private DetalleContactoClienteBR ctrlDetalle;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor predeterminado para el presentador.
        /// </summary>
        /// <param name="vista">Vista que será gestionada por el presentador.</param>
        public ucContactoClientePRE(IucContactoClienteVIS vista) {
            this.vista = vista;
            this.dataContext = FacadeBR.ObtenerConexion();
            ctrlDetalle = new DetalleContactoClienteBR();
        }

        #endregion

        #region Métodos

            #region Buscador

        /// <summary>
        /// Crea el Objeto de filtrado para el buscador.
        /// </summary>
        /// <param name="catalogo">El Tipo de Objeto a filtrar.</param>
        /// <returns>Un Objeto de Tipo Object</returns>
        public object PrepararBOBuscador(string catalogo) {
            object obj = null;

            switch (catalogo) {
                case "Sucursal":
                    SucursalBOF sucursal = new SucursalBOF();
                    sucursal.UnidadOperativa = new UnidadOperativaBO { Id = this.vista.UnidadOperativaId };
                    sucursal.Usuario = new UsuarioBO { Id = this.vista.UsuarioAutenticado };
                    sucursal.Nombre = this.vista.NombreSucursal;
                    sucursal.Activo = true;
                    obj = sucursal;
                    break;
                case "Cliente":
                    CuentaClienteIdealeaseBOF cliente = new CuentaClienteIdealeaseBOF {
                        UnidadOperativa = new UnidadOperativaBO { Id = vista.UnidadOperativaId },
                        Cliente = new ClienteBO()
                    };
                    if (vista.NombreCliente != null)
                        cliente.Nombre = "%" + vista.NombreCliente + "%";
                    obj = cliente;
                    break;
            }

            return obj;
        }

        /// <summary>
        /// Despliega el Objeto seleccionado del buscador.
        /// </summary>
        /// <param name="catalogo">El Tipo de Objeto a filtrar.</param>
        /// <param name="selecto">El Objeto seleccionado del buscador.</param>
        public void DesplegarResultadoBuscador(string catalogo, object selecto) {
            switch (catalogo) {
                case "Sucursal":
                    SucursalBO sucursal = (SucursalBO)selecto;
                    if (sucursal != null) {
                        vista.SucursalSeleccionada = new SucursalBO(){
                          Id = sucursal.Id,
                          Nombre = sucursal.Nombre
                        };
                        if (sucursal.Nombre != null)
                            this.vista.NombreSucursal = sucursal.Nombre;
                        else
                            this.vista.NombreSucursal = null;
                    }
                    break;
                case "Cliente":
                     CuentaClienteIdealeaseBOF cliente = (CuentaClienteIdealeaseBOF)selecto ?? new CuentaClienteIdealeaseBOF();
                    if (cliente.Cliente == null)
                        cliente.Cliente = new ClienteBO();
                    vista.NombreCliente = !string.IsNullOrEmpty(cliente.Nombre) ? cliente.Nombre : string.Empty;
                    vista.ClienteSeleccionado = cliente;
                    break;
            }
        }

            #endregion

        /// <summary>
        /// Realiza el Eliminado del Detalle Contacto Cliente Idealease seleccionado.
        /// </summary>
        public void EliminarDetalle() {
            try {
                ctrlDetalle.Eliminar(dataContext, vista.DetalleSeleccionado);
                vista.MostrarMensaje("Eliminado con exito", ETipoMensajeIU.EXITO);
                vista.RecargarContactoClienteTemp();
            } catch(Exception e){
                vista.MostrarMensaje("Error al intentar eliminar el Detalle Contacto Cliente", ETipoMensajeIU.ERROR, e.Message);
            }
        }

        #endregion
    }
}
