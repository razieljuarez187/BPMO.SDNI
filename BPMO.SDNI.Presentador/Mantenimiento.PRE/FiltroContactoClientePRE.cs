// Satisface al CU027 - Catálogo Contacto Cliente
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BPMO.SDNI.Mantenimiento.VIS;
using BPMO.Facade.SDNI.BOF;
using BPMO.Basicos.BO;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Comun.BOF;

namespace BPMO.SDNI.Mantenimiento.PRE {

    /// <summary>
    /// Presentador aplicable para la gestión de las peticiones del Filto Contactos Clientes Idealease.
    /// </summary>
    public class FiltroContactoClientePRE {

        #region Atributos

        /// <summary>
        /// La vista que esta mostrando los datos procesados en el presentador.
        /// </summary>
        private IFiltroContactoClienteVIS vista;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor predeterminado para el presentador.
        /// </summary>
        /// <param name="vista">Vista que será gestionada por el presentador.</param>
        public FiltroContactoClientePRE(IFiltroContactoClienteVIS vista) {
            this.vista = vista;
        }

        #endregion

        #region Métodos

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
    }
}
