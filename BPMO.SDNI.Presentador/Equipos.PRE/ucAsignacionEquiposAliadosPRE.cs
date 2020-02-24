//Satisface al CU077 - Registrar Acta de Nacimiento de una Unidad
//Satisface al CU079 - Consultar Acta de Nacimiento de Unidad
//Satisface al CU080 – Editar Acta de Nacimiento de una Unidad
using System;
using System.Collections.Generic;
using BPMO.Basicos.BO;
using BPMO.Facade.SDNI.BR;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Equipos.BO;
using BPMO.SDNI.Equipos.BOF;
using BPMO.SDNI.Equipos.BR;
using BPMO.SDNI.Equipos.VIS;
using BPMO.Servicio.Catalogos.BO;

using BPMO.SDNI.Comun.BO;

namespace BPMO.SDNI.Equipos.PRE
{
    public class ucAsignacionEquiposAliadosPRE
    {
        #region Atributos
        private IDataContext dctx = null;

        private IucAsignacionEquiposAliadosVIS vista;
        private string nombreClase = "AsignacionEquiposAliadosPRE";
        #endregion

        #region Constructores
        public ucAsignacionEquiposAliadosPRE(IucAsignacionEquiposAliadosVIS view)
        {
            try
            {
                this.vista = view;

                this.dctx = FacadeBR.ObtenerConexion();
            }
            catch (Exception ex)
            {
                this.vista.MostrarMensaje("Inconsistencias en los parámetros de configuración", ETipoMensajeIU.ERROR, this.nombreClase + ".ucAsignacionEquiposAliadosPRE:" + ex.Message);
            }
        }
        #endregion

        #region Métodos
        public void PrepararNuevo()
        {
            this.vista.PrepararNuevo();
            this.vista.HabilitarModoEdicion(true);
        }
        public void PrepararEdicion()
        {
            this.vista.HabilitarModoEdicion(true);
        }
        public void PrepararVisualizacion()
        {
            this.vista.HabilitarModoEdicion(false);
        }

        public void BloquearCamposPorEstatus(EEstatusUnidad? estatus)
        {
            if (estatus == null) return;

            if (estatus != EEstatusUnidad.NoDisponible)
            {
                this.vista.HabilitarModoEdicion(false);
            }
        }

        public void AgregarEquiposAliado(List<EquipoAliadoBO> lst)
        {
            try
            {
                this.vista.EquiposAliados = lst;

                this.vista.ActualizarEquiposAliados();
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".AgregarEquiposAliado: " + ex.Message);
            }
        }
        public void AgregarEquipoAliado()
        {
            string s;
            if ((s = this.ValidarCamposEquipoAliado()) != null)
            {
                this.vista.MostrarMensaje(s, ETipoMensajeIU.INFORMACION, null);
                return;
            }

            try
            {
                if(this.vista.EquiposAliados == null)
                    this.vista.EquiposAliados = new List<EquipoAliadoBO>();

                List<EquipoAliadoBO> equiposAliados = this.vista.EquiposAliados;

                EquipoAliadoBO equipoAliado = new EquipoAliadoBO();
                equipoAliado.Sucursal = new SucursalBO();
                equipoAliado.Sucursal.UnidadOperativa = new UnidadOperativaBO();
                equipoAliado.Mediciones = new MedicionesBO();
                equipoAliado.Modelo = new ModeloBO();
                
                equipoAliado.NumeroSerie = this.vista.EquipoAliadoNumeroSerie;
                equipoAliado.EquipoAliadoID = this.vista.EquipoAliadoId;
                equipoAliado = this.ObtenerEquipoAliadoCompleto(equipoAliado);

                #region Validacion Comentada que sera utilizada en el modulo de reportes
                //// Se valida que el equipo Aliado tenga clave de Activo de Oracle
                //if (equipoAliado != null)
                //{
                //    const string mensajeError = "Es necesario que el Equipo Aliado posea Clave de Activo de Oracle";
                //    var sError = equipoAliado.ActivoFijo == null ? mensajeError : String.IsNullOrEmpty(equipoAliado.ActivoFijo.NumeroActivo) ? mensajeError : String.IsNullOrEmpty(equipoAliado.ClaveActivoOracle) ? mensajeError : String.Empty;
                    
                //    if (!String.IsNullOrEmpty(sError))
                //    {
                //        this.vista.MostrarMensaje(sError, ETipoMensajeIU.INFORMACION, null);
                //        return;
                //    }
                //}
                #endregion

                equiposAliados.Add(equipoAliado);

                this.AgregarEquiposAliado(equiposAliados);

                this.vista.PrepararNuevoEquipoAliado();
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".AgregarEquipoAliado: " + ex.Message);
            }
        }
        private string ValidarCamposEquipoAliado()
        {
            if (this.vista.EquipoAliadoId == null)
                return "Es necesario seleccionar un equipo aliado a agregar.";

            List<EquipoAliadoBO> equiposAliados = this.vista.EquiposAliados;
            if (equiposAliados.Exists(p => p.EquipoAliadoID == this.vista.EquipoAliadoId))
                return "El equipo aliado ya se encuentra asignado la unidad.";

            return null;
        }
        private EquipoAliadoBO ObtenerEquipoAliadoCompleto(EquipoAliadoBO bo)
        {
            EquipoAliadoBR br = new EquipoAliadoBR();
            List<EquipoAliadoBO> lstTemp = br.Consultar(this.dctx, bo);
            if (lstTemp.Count > 0)
                bo = lstTemp[0];

            return bo;
        }

        public void QuitarEquipoAliado(int index)
        {
            try
            {
                if (index >= this.vista.EquiposAliados.Count || index < 0)
                    throw new Exception("No se encontró el equipo aliado seleccionado");

                List<EquipoAliadoBO> equiposAliados = this.vista.EquiposAliados;
                equiposAliados.RemoveAt(index);

                this.vista.EquiposAliados = equiposAliados;
                this.vista.ActualizarEquiposAliados();
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".QuitarEquipoAliado: " + ex.Message);
            }
        }

        public string ValidarCamposBorrador()
        {
            //Todos los datos de esta vista son opcionales al guardar un BORRADOR
            return null;
        }
        public string ValidarCamposRegistro()
        {

            return null;
        }

        public void LimpiarSesion()
        {
            this.vista.LimpiarSesion();
        }

        #region Métodos para el Buscador
        public object PrepararBOBuscador(string catalogo)
        {
            object obj = null;

            switch (catalogo)
            {
                case "EquipoAliado":
                    EquipoAliadoBOF ea = new EquipoAliadoBOF();					
                    ea.Activo = true;					
                    ea.Sucursal = new SucursalBO();
                    ea.Sucursal.UnidadOperativa = new UnidadOperativaBO();
                    ea.Sucursal.UnidadOperativa.Empresa = new EmpresaBO();

                    ea.NumeroSerie = this.vista.EquipoAliadoNumeroSerie;
                    ea.Sucursal.UnidadOperativa.Id = this.vista.UnidadOperativaId;
                    ea.Estatus = EEstatusEquipoAliado.SinAsignar;

                    if (this.vista.SucursalesSeguridad != null)
                        ea.Sucursales = this.vista.SucursalesSeguridad.ConvertAll(p => new SucursalBO() { Id = p });

                    obj = ea;
                    break;
            }

            return obj;
        }
        public void DesplegarResultadoBuscador(string catalogo, object selecto)
        {
            switch (catalogo)
            {
                case "EquipoAliado":
                    #region Desplegar Propietario
                    EquipoAliadoBOF ea = (EquipoAliadoBOF)selecto;

                    if (ea != null && ea.NumeroSerie != null)
                        this.vista.EquipoAliadoNumeroSerie = ea.NumeroSerie;
                    else
                        this.vista.EquipoAliadoNumeroSerie = null;

                    if (ea != null && ea.EquipoAliadoID != null)
                        this.vista.EquipoAliadoId = ea.EquipoAliadoID;
                    else
                        this.vista.EquipoAliadoId = null;
                    #endregion
                    break;
            }
        }
        #endregion

        #region REQ 13285 Métodos relacionado con las acciones dependiendo de la unidad operativa.

        /// <summary>
        /// Determina las acciones relacionadas con el comportamiento de las vistas.
        /// </summary>
        /// <param name="listaAcciones">Lista de objetos de tipo CatalogoBaseBO que contiene las acciones a las cuales el usuario tiene permiso.</param>
        public void EstablecerAcciones(List<CatalogoBaseBO> listaAcciones) {
            ETipoEmpresa EmpresaConPermiso = ETipoEmpresa.Idealease;
            switch (this.vista.UnidadOperativaId) {
                case (int)ETipoEmpresa.Generacion:
                    if (ExisteAccion(listaAcciones, "UI ACTA GENERACION")) {
                        EmpresaConPermiso = ETipoEmpresa.Generacion;
                    }
                    break;
                case (int)ETipoEmpresa.Equinova:
                    if (ExisteAccion(listaAcciones, "UI ACTA GENERACION")) {
                        EmpresaConPermiso = ETipoEmpresa.Equinova;
                    }
                    break;
                case (int)ETipoEmpresa.Construccion:
                    if (ExisteAccion(listaAcciones, "UI ACTA CONSTRUCCION")) {
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
