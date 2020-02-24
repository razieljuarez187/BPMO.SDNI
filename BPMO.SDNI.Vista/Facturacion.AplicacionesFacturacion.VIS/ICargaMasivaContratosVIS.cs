// Satisface el caso de uso CU015 – Carga Masiva Facturación Contratos

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Contratos.BO;
using BPMO.SDNI.Equipos.BO;

namespace BPMO.SDNI.Facturacion.AplicacionesFacturacion.VIS
{
    /// <summary>
    /// Interface que define las acciones que debe de contener una vista para carga de masiva de pagos ya facturados de contratos 
    /// </summary>
    public interface ICargaMasivaContratosVIS
    {
        /// <summary>
        /// Obtiene un valor que representa el identificador del usuario actual de la sesión en curso
        /// </summary>
        /// <value>Objeto de tipo Nullable de Int32</value>
        int? UsuarioID { get; }

        /// <summary>
        /// Obtiene un valor que representa el identificador de la unidad operativa actual de la sesión en curso
        /// </summary>
        /// <value>Objeto de tipo Nullable de Int32</value>
        int? UnidadOperativaID { get; }

        /// <summary>
        /// Obtiene o establece un valor que representa la sucursal donde se cargarán los pagos
        /// </summary>
        /// <value>Valor de tipo Int32</value>
        int? SucursalID { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que representa el nombre de la sucursal
        /// </summary>
        /// <value>Objeto de tipo String</value>
        String SucursalNombre { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que representa el departamento donde se cargarán los pagos
        /// </summary>
        /// <value>Valor de tipo Int32</value>
        ETipoContrato? Departamento { get; set; }

        /// <summary>
        /// Obtiene el archivo cargado que contiene los pagos
        /// </summary>
        /// <value>Valor de tipo byte[]</value>
        byte[] Archivo { get; }

        /// <summary>
        /// Obtiene el nombre del archivo que fue cargado
        /// </summary>
        /// <value>Valor de tipo String</value>
        String NombreArchivo { get; }

        /// <summary>
        /// Lista de eventos ocurridos durante la carga del archivo de pagos
        /// </summary>
        /// <value>Valor de tipo Lista</value>
        IList Eventos { get; set; }

        /// <summary>
        /// Obtiene los datos extraidos del archivo de carga
        /// </summary>
        /// <value>Objeto de tipo DataSet</value>
        DataSet PagosCargados { get; set; }

        /// <summary>
        /// Obtiene los contratos ya procesados del archivo de carga
        /// </summary>
        /// <value>Valor de tipo Lista</value>
        IList<ContratoBO> Contratos { get; set; }

        /// <summary>
        /// Obtiene las unidades ya procesadas del archivo de carga
        /// </summary>
        /// <value>Valor de tipo Lista</value>
        IList<EquipoBO> Equipos { get; set; }

        /// <summary>
        /// Inicializa la carga inicial de la vista
        /// </summary>
        void Inicializar();

        /// <summary>
        /// Libera o elimina los valores en sesión guardados en el visor
        /// </summary>
        void LimpiarSesion();

        /// <summary>
        /// Realiza la validación de los datos capturados para detectar errores o inconsistencias
        /// </summary>        
        bool ValidarCampos();

        /// <summary>
        /// Este método despliega un mensaje en pantalla
        /// </summary>
        /// <param name="mensaje">Mensaje a desplegar</param>
        /// <param name="tipo">Tipo de mensaje a desplegar</param>
        /// <param name="detalle">Muestra una cadena de texto que contiene los datalles del mensaje a desplegar</param>
        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null);

        /// <summary>
        /// Realiza la redirección al visor correspondiente cuando se intenta acceder a la vista actual sin tener permisos asignados
        /// </summary>
        void RedirigirSinPermisoAcceso();

        /// <summary>
        /// Despliega el Detalle del registro Seleccionado
        /// </summary>
        void RedirigirAResultadoCarga();
    }
}
