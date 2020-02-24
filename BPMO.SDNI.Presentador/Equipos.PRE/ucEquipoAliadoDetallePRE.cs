//Satisface al CU075 - Catálogo de Equipo Aliado
// Satisface a la SC0005
using System;
using System.Collections.Generic;
using BPMO.Basicos.BO;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Equipos.BO;
using BPMO.SDNI.Equipos.BR;
using BPMO.SDNI.Equipos.VIS;


namespace BPMO.SDNI.Equipos.PRE
{
    public class ucEquipoAliadoDetallePRE
    {
        #region Atributos
        private IucEquipoAliadoDetalleVIS vista;
        private const String nombreClase = "ucEquipoAliadoDetallePRE";
        
        private readonly EquipoAliadoBR controlador;

        /// <summary>
        /// El DataContext que proveerá acceso a la base de datos
        /// </summary>
        private readonly IDataContext dctx;
        #endregion

        #region Constructores
        public ucEquipoAliadoDetallePRE(IucEquipoAliadoDetalleVIS view)
        {
            this.vista = view;
            this.controlador = new EquipoAliadoBR();
            this.dctx = Facade.SDNI.BR.FacadeBR.ObtenerConexion();
        }
        #endregion

        #region Métodos
        internal void PrepararVista()
        {
            this.vista.PrepararVista();
        }

        internal void CargarObjetoInicio()
        {
            this.PrepararVista();
            this.EstablecerDatosNavegacion(this.vista.ObtenerDatosNavegacion());
            this.ConsultarCompleto();
        }

        private void ConsultarCompleto()
        {
            try
            {
                EquipoAliadoBO bo = (EquipoAliadoBO)this.InterfazUsuarioADato();

                List<EquipoAliadoBO> lst = this.controlador.ConsultarCompleto(this.dctx, bo, true);

                if (lst.Count < 1)
                    throw new Exception("No se encontró ningún registro que corresponda a la información proporcionada.");
                if (lst.Count > 1)
                    throw new Exception("La consulta devolvió más de un registro.");

                this.DatoAInterfazUsuario(lst[0]);

                this.vista.UltimoObjeto = lst[0];
            }
            catch (Exception ex)
            {
                this.DatoAInterfazUsuario(new EquipoAliadoBO());
                throw new Exception(nombreClase + ".ConsultarCompleto:" + ex.Message);
            }
        }

        private void EstablecerDatosNavegacion(object paqueteNavegacion)
        {
            try
            {
                if (paqueteNavegacion == null)
                    throw new Exception("Se esperaba un objeto en la navegación. No se puede identificar qué acta de nacimiento se desea consultar.");
                if (!(paqueteNavegacion is EquipoAliadoBO))
                    throw new Exception("Se esperaba una Unidad de Idealease.");

                EquipoAliadoBO bo = (EquipoAliadoBO)paqueteNavegacion;

                this.DatoAInterfazUsuario(bo);
            }
            catch (Exception ex)
            {
                this.DatoAInterfazUsuario(new EquipoAliadoBO());
                throw new Exception(nombreClase + ".EstablecerDatosNavegacion: " + ex.Message);
            }
        }

        private void DatoAInterfazUsuario(object bo)
        {
            EquipoAliadoBO obj = (EquipoAliadoBO)bo;
            if (obj.Anio.HasValue)
                this.vista.AnioModelo = obj.Anio.Value.ToString();
            if (!string.IsNullOrEmpty(obj.ClaveActivoOracle) && !string.IsNullOrWhiteSpace(obj.ClaveActivoOracle))            
                this.vista.OracleID = obj.ClaveActivoOracle;            
            if (!string.IsNullOrEmpty(obj.Dimension) && !string.IsNullOrWhiteSpace(obj.Dimension))
                this.vista.Dimension = obj.Dimension;
            if(obj.EquipoAliadoID.HasValue)
                this.vista.EquipoAliadoID = obj.EquipoAliadoID.Value;
            if(obj.EquipoID.HasValue)
                this.vista.EquipoID = obj.EquipoID;
            if(obj.EsActivo.HasValue)
                this.vista.ActivoOracle = obj.EsActivo.Value;
            if(obj.Estatus.HasValue)
            {
                this.vista.EstatusNombre = obj.Estatus.Value.ToString();
                this.vista.EstatusID = (int)obj.Estatus.Value;
            }
            if (obj.TipoEquipoAliado.HasValue)
            {
                this.vista.TipoEquipoAliado = obj.TipoEquipoAliado.Value.ToString();
            }
            if (!string.IsNullOrEmpty(obj.Fabricante) && !string.IsNullOrWhiteSpace(obj.Fabricante))
                this.vista.Fabricante = obj.Fabricante;
            if(obj.HorasIniciales.HasValue)
                this.vista.HorasIniciales = obj.HorasIniciales.Value;
            if (obj.KilometrajeInicial.HasValue)
                this.vista.KilometrosIniciales = obj.KilometrajeInicial.Value;
            if (obj.IDLider.HasValue)
                this.vista.EquipoLiderID = obj.IDLider;
            if(obj.Modelo != null)
            {
                if(obj.Modelo.Id.HasValue)
                    this.vista.ModeloID = obj.Modelo.Id.Value;
                if (!string.IsNullOrEmpty(obj.Modelo.Nombre) && !string.IsNullOrWhiteSpace(obj.Modelo.Nombre))
                    this.vista.Modelo = obj.Modelo.Nombre;
                if(obj.Modelo.Marca!= null)
                {
                    if (!string.IsNullOrEmpty(obj.Modelo.Marca.Nombre) && !string.IsNullOrWhiteSpace(obj.Modelo.Marca.Nombre))
                        this.vista.Marca = obj.Modelo.Marca.Nombre;
                }
            }
            if (!string.IsNullOrEmpty(obj.NumeroSerie) && !string.IsNullOrWhiteSpace(obj.NumeroSerie))
                this.vista.NumeroSerie = obj.NumeroSerie;
            if(obj.PBC.HasValue)
                this.vista.PBC = obj.PBC.Value;
            if(obj.PBV.HasValue)
                this.vista.PBV = obj.PBV.Value;
            if(obj.Sucursal != null)
            {
                if(obj.Sucursal.Id.HasValue)
                    this.vista.SucursalID = obj.Sucursal.Id.Value;
                if (!string.IsNullOrEmpty(obj.Sucursal.Nombre) && !string.IsNullOrWhiteSpace(obj.Sucursal.Nombre))
                    this.vista.SucursalNombre = obj.Sucursal.Nombre;
                if(obj.Sucursal.UnidadOperativa != null)
                {
                    if(obj.Sucursal.UnidadOperativa.Id.HasValue)
                        this.vista.UnidadOperativaID = obj.Sucursal.UnidadOperativa.Id.Value;
                    if (!string.IsNullOrEmpty(obj.Sucursal.UnidadOperativa.Nombre) && !string.IsNullOrWhiteSpace(obj.Sucursal.UnidadOperativa.Nombre))
                        this.vista.UnidadOperativaNombre = obj.Sucursal.UnidadOperativa.Nombre;
                    else
                    {
                        if (this.vista.UnidadOperativa != null)
                        {
                            if (obj.Sucursal.UnidadOperativa.Id.Value == this.vista.UnidadOperativa.Id)
                                this.vista.UnidadOperativaNombre = this.vista.UnidadOperativa.Nombre;
                        }
                    }
                }
            }

            if (obj.TipoEquipoServicio != null)
            {
                this.vista.TipoEquipoID = obj.TipoEquipoServicio.Id;
                this.vista.TipoEquipoNombre = obj.TipoEquipoServicio.Nombre;
            }			
            this.vista.Activo = obj.Activo;			
        }

        internal void Editar()
        {
            EquipoAliadoBO obj = (EquipoAliadoBO)this.InterfazUsuarioADato();
            this.LimpiarSesion();
            this.vista.EstablecerDatosNavegacion("EquipoAliadoEditar", obj);
            this.vista.RedirigirAEdicion();
        }

        private void LimpiarSesion()
        {
            this.vista.LimpiarSesion();
        }

        private object InterfazUsuarioADato()
        {
            EquipoAliadoBO obj = new EquipoAliadoBO();

            obj.EquipoAliadoID = this.vista.EquipoAliadoID;
            obj.EquipoID = this.vista.EquipoID;
            obj.NumeroSerie = this.vista.NumeroSerie;
            obj.Sucursal = new Basicos.BO.SucursalBO();
            obj.Sucursal.Id = this.vista.SucursalID;
            obj.Sucursal.UnidadOperativa = new Basicos.BO.UnidadOperativaBO();
            obj.Sucursal.UnidadOperativa.Id = this.vista.UnidadOperativaID;

            return obj;
        }

        internal void Eliminar()
        {
            EquipoAliadoBO obj = (EquipoAliadoBO)this.InterfazUsuarioADato();

            List<EquipoAliadoBO> lst = this.controlador.ConsultarCompleto(this.dctx, obj, true);

            if (lst.Count < 1)
                throw new Exception("No se encontró ningún registro que corresponda a la información proporcionada.");
            if (lst.Count > 1)
                throw new Exception("La consulta devolvió más de un registro.");

            obj = lst[0];

            //Evaluar estatus
            if (obj.Estatus != EEstatusEquipoAliado.SinAsignar)
            {
                this.vista.MostrarMensaje("El equipo aliado esta asignado a una unidad actualmente no puede ser dado de baja.", ETipoMensajeIU.ADVERTENCIA, null);
                return;
            }

            obj.Estatus = EEstatusEquipoAliado.Inactivo;

            //Se crea el objeto de seguridad
            UsuarioBO usuario = this.vista.Usuario;
            AdscripcionBO adscripcion = new AdscripcionBO() { UnidadOperativa = this.vista.UnidadOperativa };
            SeguridadBO seguridadBO = new SeguridadBO(Guid.Empty, usuario, adscripcion); 

            this.controlador.ActualizarCompleto(dctx, obj, this.vista.UltimoObjeto, seguridadBO);

            this.vista.RedirigirAConsulta();
        }

        internal void CancelarBaja()
        {
            this.LimpiarSesion();
            this.vista.RedirigirAConsulta();
        }

        internal void EliminarEquipoAliado()
        {
            EquipoAliadoBO obj = (EquipoAliadoBO)this.InterfazUsuarioADato();
            this.LimpiarSesion();
            this.vista.EstablecerDatosNavegacion("EquipoAliadoBODetalle", obj);
            this.vista.RedirigirAEliminar();
        }

        #region REQ 13596 Métodos relacionado con las acciones dependiendo de la unidad operativa.

        /// <summary>
        /// Determina las acciones relacionadas con el comportamiento de las vistas.
        /// </summary>
        /// <param name="listaAcciones">Lista de objetos de tipo CatalogoBaseBO que contiene las acciones a las cuales el usuario tiene permiso.</param>
        public void EstablecerAcciones(List<CatalogoBaseBO> listaAcciones) {
            ETipoEmpresa EmpresaConPermiso = ETipoEmpresa.Idealease;
            switch (this.vista.UnidadOperativaID) {
                case (int)ETipoEmpresa.Generacion:
                    if (ExisteAccion(listaAcciones, "UI EQUIPO GENERACION")) {
                        EmpresaConPermiso = ETipoEmpresa.Generacion;
                    }
                    break;
                case (int)ETipoEmpresa.Equinova:
                    if (ExisteAccion(listaAcciones, "UI EQUIPO GENERACION")) {
                        EmpresaConPermiso = ETipoEmpresa.Equinova;
                    }
                    break;
                case (int)ETipoEmpresa.Construccion:
                    if (ExisteAccion(listaAcciones, "UI EQUIPO CONSTRUCCION")) {
                        EmpresaConPermiso = ETipoEmpresa.Construccion;
                    }
                    break;
            }
            this.vista.EstablecerAcciones(EmpresaConPermiso);
        }

        /// <summary>
        /// Verifica que una acción a avaluar exista en el listado de acciones asignadas al usuario.
        /// </summary>
        /// <param name="acciones">Lista de acciones asignadas al usuario.</param>
        /// <param name="nombreAccion">Acción a evaluar</param>
        /// <returns>Devuelve true en caso de existir la acción a evaluar en el listado de acciones, en caso contrario regresa false.</returns>
        private bool ExisteAccion(List<CatalogoBaseBO> acciones, string nombreAccion)
        {
            if (acciones != null && acciones.Count > 0 && acciones.Exists(p => p != null && p.Nombre != null && p.Nombre.Trim().ToUpper() == nombreAccion.Trim().ToUpper()))
                return true;

            return false;
        }

        #endregion
        #endregion
    }
}