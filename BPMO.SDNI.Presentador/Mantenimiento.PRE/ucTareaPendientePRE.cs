//Satisface el CU063 - Administrar Tareas Pendientes
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BPMO.SDNI.Mantenimiento.VIS;
using BPMO.SDNI.Mantenimiento.BR;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Facade.SDNI.BO;
using BPMO.Basicos.BO;
using BPMO.Servicio.Catalogos.BO;
using BPMO.SDNI.Mantenimiento.BO;
using BPMO.Primitivos.Enumeradores;

namespace BPMO.SDNI.Mantenimiento.PRE
{
    /// <summary>
    /// Presentador general para la UI de tareas pendientes
    /// </summary>
    public class ucTareaPendientePRE
    {
        #region Atributos
        /// <summary>
        /// Vista general de tarea pendiente
        /// </summary>
        IucTareaPendienteVIS vista;

        /// <summary>
        /// Nombre de la clase
        /// </summary>
        private const String nombreClase = "ucTareaPendientePRE";

        /// <summary>
        /// Controlador de tareas pendientes
        /// </summary>
        private readonly TareaPendienteBR controlador;

        /// <summary>
        /// El DataContext que proveerá acceso a la base de datos
        /// </summary>
        private readonly IDataContext dctx;
        #endregion

        #region Constructores
        public ucTareaPendientePRE(IucTareaPendienteVIS view)
        {
            this.vista = view;
            this.controlador = new TareaPendienteBR();
            this.dctx = Facade.SDNI.BR.FacadeBR.ObtenerConexion();
        }
        #endregion

        #region Métodos para el Buscador

        /// <summary>
        /// Metodo que prepara el objeto para el buscador
        /// <param name="catalogo">Nombre del catalogo por buscar</param>
        /// </summary>
        public object PrepararBOBuscador(string catalogo)
        {
            object obj = null;

            switch (catalogo)
            {
                case "UnidadIdealease":
                    BPMO.SDNI.Equipos.BO.UnidadBO unidad = new BPMO.SDNI.Equipos.BO.UnidadBO();
                    unidad.NumeroEconomico = this.vista.NumeroEconomico;
                    unidad.NumeroSerie = this.vista.NumeroSerie;
                    obj = unidad;
                    break;
                case "Modelo":
                    ModeloBO modelo = new ModeloBO();
                    modelo.Nombre = this.vista.Modelo;
                    obj = modelo;
                    break;
            }

            return obj;
        }

        /// <summary>
        /// Metodo para desplegar resultado de buscador
        /// <param name="catalogo">Nombre del catalogo por buscar</param>
        /// <param name="selecto">Objeto obtenido por el buscador</param>
        /// </summary>
        public void DesplegarResultadoBuscador(string catalogo, object selecto)
        {
            switch (catalogo)
            {
                case "UnidadIdealease":
                    if (selecto != null)
                    {
                        var unidad = (BPMO.SDNI.Equipos.BO.UnidadBO)selecto;
                        if (unidad.UnidadID != null)
                        {
                            this.vista.UnidadID = unidad.UnidadID;
                        }
                        if (unidad.NumeroEconomico != null)
                        {
                            this.vista.NumeroEconomico = unidad.NumeroEconomico;
                        }
                        if (unidad.NumeroSerie != null)
                        {
                            this.vista.NumeroSerie = unidad.NumeroSerie;
                        }
                    }
                    break;
                case "Modelo":
                    if (selecto != null)
                    {
                        var modelo = (ModeloBO)selecto;
                        if (modelo.Id != null)
                        {
                            this.vista.ModeloID = modelo.Id;
                        }
                        if (modelo.Nombre != null)
                        {
                            this.vista.Modelo = modelo.Nombre;
                        }
                    }
                    break;
            }
        }
        #endregion

        #region Metodos

        /// <summary>
        /// Prepara los datos iniciales de la UI
        /// </summary>
        internal void CargarObjetoInicio()
        {
            this.EstablecerDatosNavegacion(this.vista.ObtenerDatosNavegacion());
            vista.PrepararVista();
        }

        /// <summary>
        /// Metodo que permite desplegar un objeto existente en la UI
        /// <param name="paqueteNavegacion">Objeto por desplegar en la UI</param>
        /// </summary>
        private void EstablecerDatosNavegacion(object paqueteNavegacion)
        {
            try
            {
                if (paqueteNavegacion == null)
                    throw new Exception("Se esperaba un objeto en la navegación. No se puede identificar qué tarea pendiente se desea consultar.");
                if (!(paqueteNavegacion is TareaPendienteBO))
                    throw new Exception("Se esperaba una tarea pendiente.");

                TareaPendienteBO bo = (TareaPendienteBO)paqueteNavegacion;                    

                this.DatoAInterfazUsuario(bo);
            }
            catch (Exception ex)
            {
                this.vista.RedirigirAConsulta();
            }
        }

        /// <summary>
        /// Metodo que permite mostrar en la UI un objeto de tipo tarea pendiente
        /// <param name="bo">Objeto que se desplegará en la UI</param>
        /// </summary>
        private void DatoAInterfazUsuario(object bo)
        {
            TareaPendienteBO obj = (TareaPendienteBO)bo;
            vista.TareaPendienteID = obj.TareaPendienteID;
            if (obj.Unidad != null)
            {
                vista.UnidadID = obj.Unidad.UnidadID;
                vista.NumeroSerie = obj.Unidad.NumeroSerie;
                vista.NumeroEconomico = obj.Unidad.NumeroEconomico;
            }
            if (obj.Modelo != null)
            {
                vista.ModeloID = obj.Modelo.Id;
                vista.Modelo = obj.Modelo.Nombre;
            }
            vista.Descripcion = obj.Descripcion;
            vista.Activo = obj.Activo;
        }

        /// <summary>
        /// Cancela el registro de una tarea pendiente y redirige a consulta
        /// </summary>
        internal void CancelarRegistro()
        {
            this.LimpiarSesion();
            this.vista.RedirigirAConsulta();
        }

        /// <summary>
        /// Limpia los datos en sesion
        /// </summary>
        private void LimpiarSesion()
        {
            this.vista.LimpiarSesion();
        }

        /// <summary>
        /// Cancela la edicion de una tarea pendiente y redirige a consulta
        /// </summary>
        internal void CancelarEdicion()
        {
            this.vista.RedirigirADetalles();
        }

        /// <summary>
        /// Metodo que realiza la edición de una tarea pendiente por medio del controlador
        /// </summary>
        internal void Editar()
        {
            string s;
            if (!String.IsNullOrEmpty((s = this.ValidarDatos())))
            {
                this.vista.MostrarMensaje("Los siguientes datos son incorrectos:" + s.Substring(1), ETipoMensajeIU.ADVERTENCIA, null);
                return;
            }

            //Se obtiene la información a partir de la Interfaz de Usuario
            TareaPendienteBO obj = (TareaPendienteBO) this.InterfazUsuarioADato();

            //Se crea el objeto de seguridad
            UsuarioBO usuario = this.vista.Usuario;
            AdscripcionBO adscripcion = new AdscripcionBO() { UnidadOperativa = this.vista.UnidadOperativa };
            SeguridadBO seguridadBO = new SeguridadBO(Guid.Empty, usuario, adscripcion);

            this.controlador.Actualizar(dctx, obj, seguridadBO);
            this.vista.MostrarMensaje("Edición Exitosa", ETipoMensajeIU.EXITO, null);
            this.vista.EstablecerPaqueteNavegacion("RecargarTareas", null);
            this.vista.DeshabilitarCamposEdicion();
            this.vista.PermitirEdicionEstatus(false);
        }



        /// <summary>
        /// Pasa los datos de la interfaz de usuario a un objeto para consultar
        /// </summary>
        private TareaPendienteBO InterfazUsuarioADato() {
            TareaPendienteBO bo = new TareaPendienteBO();
            bo.TareaPendienteID = vista.TareaPendienteID;
            bo.Unidad = new BPMO.SDNI.Equipos.BO.UnidadBO();
            bo.Unidad.UnidadID = vista.UnidadID;
            bo.Unidad.NumeroSerie = vista.NumeroSerie;
            bo.Unidad.NumeroEconomico = vista.NumeroEconomico;
            bo.Modelo = new ModeloBO();
            bo.Modelo.Id = vista.ModeloID;
            bo.Modelo.Nombre = vista.Modelo;
            bo.Descripcion = vista.Descripcion;
            bo.Activo = vista.Activo;
            bo.Auditoria = new AuditoriaBO();
            bo.Auditoria.FUA = vista.FUA;
            bo.Auditoria.UUA = vista.UUA;
            return bo;
        }

        /// <summary>
        /// Realiza la validación de los datos ingresados en la UI
        /// </summary>
        public string ValidarDatos()
        {
            string s = string.Empty;
            try
            {
                if (this.vista.NumeroSerie == null)
                    s += ", Número de Serie es requerido";
                if (this.vista.Descripcion == null)
                    s += ", Descripción es requerido";
                if (this.vista.Activo == null)
                    s += ", Estatus es requerido";
            }
            catch (Exception ex)
            {
                vista.MostrarMensaje("Inconsistencias al validar los datos", ETipoMensajeIU.ERROR, nombreClase + ".ValidarDatos: " + ex.Message);
            }

            return s;
        }
        #endregion
    }
}
