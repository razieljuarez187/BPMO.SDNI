//Satisface al caso de uso PLEN.BEP.15.MODMTTO.CU017.Imprimir.Calcomania.Mantenimiento
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BPMO.SDNI.Reportes.VIS;
using BPMO.SDNI.Equipos.BO;
using BPMO.Primitivos.Enumeradores;
using System.Data;
using System.Web.UI.WebControls;
using BPMO.SDNI.Contratos.BO;

namespace BPMO.SDNI.Mantenimiento.Reportes.VIS
{
    public interface IConsultarCalcomaniaVIS : IReportPageVIS
    {
        #region Propiedades
        /// <summary>
        /// Obtiene el índice de la página del Grid
        /// </summary>
        int Index { get; set; }

        /// <summary>
        /// Obtiene o establece el valor de los libros activos
        /// </summary>
        string LibroActivos { get; set; }
        /// <summary>
        /// Obtiene o establece el valor del VIN/Serie
        /// </summary>
        String NumeroVIN { get; set; }
        /// <summary>
        /// Obtiene o establece un valor que representa el nombre de la sucursal
        /// </summary>
        /// <value>Objeto de tipo String</value>
        String SucursalNombre { get; set; }
        /// <summary>
        /// Obtiene o establece un valor que representa el identificador de sucursal
        /// </summary>
        /// <value>Objeto de tipo Nullable de Int32</value>
        int? SucursalID { get; set; }
        /// <summary>
        /// Obtiene o establece el valor que represanta el Numero económico de la unidad
        /// </summary>
        String NumeroEconomico { get; set; }
        /// <summary>
        /// Obtiene o establece un valor que representa el identificador del cliente
        /// </summary>
        /// <value>Objeto de tipo Nullable de Int32</value>
        int? ClienteID { get; set; }
        /// <summary>
        /// Obtiene o establece un valor que representa el nombre del cliente
        /// </summary>
        /// <value>Objeto de tipo String</value>
        String ClienteNombre { get; set; }
        /// <summary>
        /// Obtiene los datos del datagrid
        /// </summary>
        GridView GvUnidadesCtes { get; }
        /// <summary>
        /// Obtiene o establece el valor que repreanta la página actual del grid
        /// </summary>
        int IndicePaginaResultado { get; set; }
        /// <summary>
        /// Obtiene o establece el valor que representa el Departamento al que pertenece la unidad
        /// </summary>
        ETipoContrato? Departamento {get; set; }
        /// <summary>
        /// Obtiene o establece el valor que representa el resulado de la consulta
        /// </summary>
        DataSet Resultado { get; set; }

        #endregion

        #region Métodos
        /// <summary>
        /// Método que e utiliza para preparar el buscador general
        /// </summary>
        void PrepararBusqueda();
        /// <summary>
        /// Método que ctualiza el resultado del grid cuando cambia de página
        /// </summary>
        void ActualizarResultado();
        /// <summary>
        /// Método que se utiliza para limpiar las variables de sesión
        /// </summary>
        void LimpiarSesion();
        /// <summary>
        /// Método que despliega los mensajes en el sistema
        /// </summary>
        /// <param name="mensaje"></param>
        /// <param name="tipo"></param>
        /// <param name="detalle"></param>
        //void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null);

        #endregion
        
    }
}
