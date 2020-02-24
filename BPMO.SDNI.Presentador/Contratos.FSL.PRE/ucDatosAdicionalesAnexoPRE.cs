// Satisface al CU15 - Registrar Contrato Full Service Leasing
// Satisface al CU22 - Consultar Contratos Full Service Leasing
using System;
using System.Collections.Generic;
using System.Linq;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Contratos.FSL.BO;
using BPMO.SDNI.Contratos.FSL.VIS;

namespace BPMO.SDNI.Contratos.FSL.PRE
{
    public class ucDatosAdicionalesAnexoPRE
    {
        #region Atributos
        private readonly IucDatosAdicionalesAnexoVIS vista;
        private const string NombreClase = "ucDatosAdicionalesAnexoPRE";
        #endregion

        #region Propiedades
        /// <summary>
		/// Vista sobre la que actua el Presentador de solo lectura
		/// </summary>
        public IucDatosAdicionalesAnexoVIS Vista { get{ return vista; } }
        #endregion 

        #region Constructores
        /// <summary>
		/// Contructor que recibe la vista sobre la que actuara el presentador
		/// </summary>
		/// <param name="vistaActual">vista sobre la que actuara el presentador</param>
        public ucDatosAdicionalesAnexoPRE(IucDatosAdicionalesAnexoVIS vistaActual)
        {
            if(vistaActual != null) vista = vistaActual;
            else throw new Exception(NombreClase + "." + NombreClase + ": La vista proporcionada no puede ser nula.");
        }
        #endregion

        #region Metodos
        /// <summary>
        /// Inicializa el control sin datos
        /// </summary>
        public void Inicializar(){
            InicializarDetalle();
            vista.LimpiarSesion();
        }

        public void InicializarDetalle()
        {
            LimpiarDetalle();
            vista.HabilitarAgregar(!vista.ModoConsultar);
            vista.HabilitarEditar(false);
            vista.HabilitarCampos(!vista.ModoConsultar);
        }

        internal DatoAdicionalAnexoBO InterfazUsuarioADatos(){
            // Construccion del Objeto DatoAdicionalAnexoBO
            return new DatoAdicionalAnexoBO { 
                EsObservacion = vista.Detalle_EsObservacion, 
                Descripcion = vista.Detalle_Descripcion, 
                Titulo = vista.Detalle_Titulo, 
                DatoAdicionalID = vista.Detalle_DatoAdicionalID 
            };
        }

        /// <summary>
        /// Agrega un nuevo Dato Adicional al Listado de Datos Adicionales
        /// </summary>
        public void AgregarDatoAdicional(){
            try
            {
                string mensaje = ValidarDatos();

                if (string.IsNullOrEmpty(mensaje))
                {

                    // Validar que no exista un dato adicional con los mismos datos.
                    DatoAdicionalAnexoBO datoAdicionalRepetido = vista.DatosAdicionales.Find(dato => dato.EsObservacion == vista.Detalle_EsObservacion && String.Compare(((dato.Titulo != null)?dato.Titulo.ToUpper():string.Empty), ((vista.Detalle_Titulo != null)?vista.Detalle_Titulo.ToUpper(): string.Empty), StringComparison.Ordinal) == 0 && String.Compare(dato.Descripcion.ToUpper(), vista.Detalle_Descripcion.ToUpper(), StringComparison.Ordinal) == 0);

                    if (datoAdicionalRepetido == null)
                    {

                        // Calcular el Maximo DatoAdicionalID
                        int UltimoDatoAdicionalID = 0;

                        if (vista.DatosAdicionales.Count > 0) UltimoDatoAdicionalID = vista.DatosAdicionales.Max(dato => (dato.DatoAdicionalID != null) ? dato.DatoAdicionalID.Value : 0);

                        vista.Detalle_DatoAdicionalID = ++UltimoDatoAdicionalID;

                        // Obtener la Información del Dato Adicional de la Interfaz
                        DatoAdicionalAnexoBO NuevoDatoAdicionalAnexoBO = InterfazUsuarioADatos();

                        // Agregar el nuevo dato Adicional
                        List<DatoAdicionalAnexoBO> Lista = new List<DatoAdicionalAnexoBO>(vista.DatosAdicionales);
                        Lista.Add(NuevoDatoAdicionalAnexoBO);
                        vista.DatosAdicionales = Lista;

                        // Cerrar el Dialogo
                        LimpiarDetalle();
                    }
                    else
                    {
                        vista.MostrarMensaje("No puede agregar el Dato Adicional debido a que existe uno con la misma información.", ETipoMensajeIU.ADVERTENCIA);
                    }
                }
                else
                {
                    vista.MostrarMensaje(mensaje, ETipoMensajeIU.ADVERTENCIA);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(NombreClase + ".AgregarDatoAdicional: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Limpia los campos de detalle de la vista.
        /// </summary>
        private void LimpiarDetalle()
        {
            vista.Detalle_DatoAdicionalID = null;
            vista.Detalle_Descripcion = null;
            vista.Detalle_EsObservacion = null;
            vista.Detalle_Titulo = null;
        }

        /// <summary>
        /// Actualiza un Dato Adicional dentro del Listado de Datos Adicionales
        /// </summary>
        public void ActualizarDatoAdicional(){
            try
            {
                if (vista.Detalle_DatoAdicionalID == null) throw new Exception("El Dato Adicional a Editar no cuenta con Identificador.");

                string mensaje = ValidarDatos();

                if (string.IsNullOrEmpty(mensaje))
                {

                    // Validar que no exista un dato adicional con los mismos datos.
                    DatoAdicionalAnexoBO datoAdicionalRepetido = vista.DatosAdicionales.Find(dato =>
                            dato.EsObservacion == vista.Detalle_EsObservacion &&
                            String.Compare(((dato.Titulo != null) ? dato.Titulo.ToUpper() : string.Empty), ((vista.Detalle_Titulo != null) ? vista.Detalle_Titulo.ToUpper() : string.Empty), StringComparison.Ordinal) == 0 &&
                            String.Compare(dato.Descripcion.ToUpper(), vista.Detalle_Descripcion.ToUpper(), StringComparison.Ordinal) == 0 &&
                            dato.DatoAdicionalID != vista.Detalle_DatoAdicionalID);

                    if (datoAdicionalRepetido == null)
                    {

                        // Obtener la Información del Dato Adicional de la Interfaz
                        DatoAdicionalAnexoBO NuevoDatoAdicionalAnexoBO = InterfazUsuarioADatos();

                        DatoAdicionalAnexoBO datoActualizable = vista.DatosAdicionales.Find(dato => dato.DatoAdicionalID == NuevoDatoAdicionalAnexoBO.DatoAdicionalID);

                        datoActualizable.DatoAdicionalID = NuevoDatoAdicionalAnexoBO.DatoAdicionalID;
                        datoActualizable.EsObservacion = NuevoDatoAdicionalAnexoBO.EsObservacion;
                        datoActualizable.Descripcion = NuevoDatoAdicionalAnexoBO.Descripcion;
                        datoActualizable.Titulo = NuevoDatoAdicionalAnexoBO.Titulo;


                        // Agregar el nuevo dato Adicional
                        List<DatoAdicionalAnexoBO> Lista = new List<DatoAdicionalAnexoBO>(vista.DatosAdicionales);
                        vista.DatosAdicionales = Lista;

                        // Cerrar el Dialogo
                        InicializarDetalle();
                    }
                    else
                    {
                        vista.MostrarMensaje("No puede actualizar el Dato Adicional debido a que existe uno con la misma información.", ETipoMensajeIU.ADVERTENCIA);
                    }
                }
                else
                {
                    vista.MostrarMensaje(mensaje, ETipoMensajeIU.ADVERTENCIA);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(NombreClase + ".ActualizarDatoAdicional: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un Dato Adicional de la Lista de Datos Adicionales
        /// </summary>
        /// <param name="datoEliminar">Dato Adicional a Eliminar</param>        
        public void EliminarDatoAdicional(DatoAdicionalAnexoBO datoEliminar){
            try {
                // Buscar el Dato Adicional en el Listado
                DatoAdicionalAnexoBO datoAdicionalEncontrado = vista.DatosAdicionales.Find(dato => dato.DatoAdicionalID == datoEliminar.DatoAdicionalID);

                if (datoAdicionalEncontrado != null)
                {
                    List<DatoAdicionalAnexoBO> Lista = new List<DatoAdicionalAnexoBO>(vista.DatosAdicionales);
                    Lista.Remove(datoAdicionalEncontrado);

                    vista.DatosAdicionales = Lista;

                    InicializarDetalle();
                }
                else
                {
                    vista.MostrarMensaje("El Dato Adicional no se encuentra o ha sido removido previamente.", ETipoMensajeIU.ADVERTENCIA);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(NombreClase + ".EliminarDatoAdicional: " + ex.Message, ex);
            }
        }


        /// <summary>
        /// Validacion del Dato Adicional
        /// </summary>
        /// <returns>Los mensajes de Validacion encontrados</returns>
        public string ValidarDatos()
        {
            string mensaje = string.Empty;

            if (vista.Detalle_EsObservacion != true && (vista.Detalle_Titulo == null || string.IsNullOrEmpty(vista.Detalle_Titulo)))
                mensaje += "Título, ";

            if (vista.Detalle_Descripcion == null || string.IsNullOrEmpty(vista.Detalle_Descripcion))
                mensaje += "Descripción, ";

            if (!string.IsNullOrEmpty(mensaje))
                return "Los siguientes campos de Dato Adicional no pueden estar vacíos: \n" + mensaje.Substring(0, mensaje.Length - 2);

            return mensaje;
        }

        /// <summary>
        /// Despliega el Dato Adicional en el Detalle
        /// </summary>
        /// <param name="datoAdicional"></param>
        internal void DatosAInterfazUsuario(DatoAdicionalAnexoBO datoAdicional)
        {
            vista.Detalle_DatoAdicionalID = datoAdicional.DatoAdicionalID;
            vista.Detalle_Descripcion = datoAdicional.Descripcion;
            vista.Detalle_EsObservacion = datoAdicional.EsObservacion;
            vista.Detalle_Titulo = datoAdicional.Titulo;
        }

        internal void DatosAInterfazUsuario(ContratoFSLBO contrato)
        {
            vista.DatosAdicionales = contrato.DatosAdicionalesAnexo;
            vista.ContratoID = contrato.ContratoID;
        }

        /// <summary>
        /// Despliega y configura el detalle de un Dato Adicional en modo Editar
        /// </summary>
        /// <param name="datoEditar"></param>
        public void DesplegarEditarDatoAdicional(DatoAdicionalAnexoBO datoEditar)
        {
            DatosAInterfazUsuario(datoEditar);
            vista.HabilitarAgregar(false);
            vista.HabilitarEditar(true);
            vista.HabilitarCampos(true);
        }

        /// <summary>
        /// Despleiga y configura el detalle de un dato Adicional en Modo Consultar
        /// </summary>
        /// <param name="datoConsultar"></param>
        public void DesplegarConsultarDatoAdicional(DatoAdicionalAnexoBO datoConsultar)
        {
            DatosAInterfazUsuario(datoConsultar);
            vista.HabilitarAgregar(false);
            vista.HabilitarEditar(false);
            vista.HabilitarCampos(false);
        }
        #endregion
    }
}
