// Satisface al CU027 – Catálogo Contacto Cliente
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BPMO.Basicos.BO;
using BPMO.SDNI.Comun.BO;

namespace BPMO.SDNI.Mantenimiento.VIS {

    /// <summary>
    /// Interface que define las acciones que debe de contener una vista que ejecuta la funcionalidad de filtrado y selección para Contactos Cliente Idealease.
    /// </summary>
    public interface IFiltroContactoClienteVIS {
        
        #region Atributos

            #region Seguridad

        /// <summary>
        /// Obtiene un valor que representa el Identificador de la Unidad Operativa Actual de la sesión en curso.
        /// </summary>
        int? UnidadOperativaId { get; }

        /// <summary>
        /// Obtiene un valor que representa el identificador del Usuario Actual de la sesión en curso.
        /// </summary>
        int? UsuarioAutenticado { get; }

            #endregion

            #region Form Búsqueda

                #region Filtro Sucursal

        /// <summary>
        /// Obtiene o establece un valor que representa la Sucursal seleccionada.
        /// </summary>
        SucursalBO SucursalSeleccionada { get; set; }

        /// <summary>
        /// Obtiene o estable el Nombre de la Sucursal a buscar o la seleccionada.
        /// </summary>
        string NombreSucursal { get; set; }

                #endregion

                #region Filtro Cliente Idealease

        /// <summary>
        /// Obtiene o establece un valor que representa el Cliente Idealease seleccionado.
        /// </summary>
        CuentaClienteIdealeaseBO ClienteSeleccionado { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que representa el Nombre del Cliente Idelaease a buscar o el seleccionado.
        /// </summary>
        string NombreCliente { get; set; }

                #endregion

            #endregion

        #endregion
    }
}
