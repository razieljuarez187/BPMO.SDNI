//Satisface al CU027 – Catálogo Contacto Cliente
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BPMO.Basicos.BO;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Mantenimiento.BO;
using BPMO.Primitivos.Enumeradores;

namespace BPMO.SDNI.Mantenimiento.VIS {

    /// <summary>
    /// Interface que define las acciones que debe de contener una vista que ejecuta la funcionalidad de filtrado y selección para Contactos Cliente Idealease.
    /// </summary>
    public interface IConsultarContactoClienteVIS {
        
        #region Atributos

            #region Seguridad

        /// <summary>
        /// Obtiene un valor que representa el Identificador del Módulo desde el cual se está accesando.
        /// </summary>
        int? ModuloID { get; }

        /// <summary>
        /// Obtiene un valor que representa el Identificador de la Unidad Operativa Actual de la sesión en curso.
        /// </summary>
        int? UnidadOperativaId { get; }

        /// <summary>
        /// Obtiene o establece un valor que representa la Configuración de la Unidad Operativa que indica a que libro corresponden los activos.
        /// </summary>
        string LibroActivos { get; set; }

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

        /// <summary>
        /// Obtiene un valor que representa el Estado del Contacto Cliente Idealease a buscar.
        /// </summary>
        bool? Activo { get; }

            #endregion

            #region Grid Resultado Búsqueda

        /// <summary>
        /// Obtiene o establece un valor que representa el Contacto Cliente Idealease seleccionado.
        /// </summary>
        ContactoClienteBO ContactoClienteSeleccionado { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que representa la Lista de Contactos Clientes Idealease encontrados.
        /// </summary>
        List<ContactoClienteBO> Contactos { get; set; }
            
            #endregion

        #endregion

        #region Métodos

            #region Grid Resultado Búsqueda

        /// <summary>
        /// Realiza la vinculación de la lista de Contactos Clientes Idealease encontrados con la UI.
        /// </summary>
        void DesplegarListaContactosCliente();

            #endregion

        /// <summary>
        /// Despliega un mensaje en pantalla.
        /// </summary>
        /// <param name="mensaje">Mensaje a desplegar.</param>
        /// <param name="tipo">Tipo de mensaje a desplegar.</param>
        /// <param name="msjDetalle">Muestra una cadena de texto que contiene los datalles del mensaje a desplegar.</param>
        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string msjDetalle = null);

        /// <summary>
        /// Realiza la redirección a la página de advertencia por falta de permisos.
        /// </summary>
        void RedirigirSinPermisoAcceso();
        
        #endregion

    }
}
