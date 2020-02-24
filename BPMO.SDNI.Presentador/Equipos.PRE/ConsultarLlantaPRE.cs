//Satisface al CU089 - Bitácora de Llantas
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
using BPMO.Facade.SDNI.BOF;

namespace BPMO.SDNI.Equipos.PRE
{
	public class ConsultarLlantaPRE
	{
		#region Atributos

		private LlantaBR controlador;
		private IDataContext dctx;
		private string nombreClase = "ConsultarLlantaPRE";
		private IConsultarLlantaVIS vista;

		#endregion Atributos

		#region Constructor

		public ConsultarLlantaPRE(IConsultarLlantaVIS view)
		{
			try
			{
				this.vista = view;

				this.controlador = new LlantaBR();
				this.dctx = FacadeBR.ObtenerConexion();
			}
			catch (Exception ex)
			{
				this.vista.MostrarMensaje("No se pudieron obtener datos de conexión", ETipoMensajeIU.ERROR,
					"No se encontraron los parámetros de conexión en la fuente de datos, póngase en contacto con el administrador del sistema.");
			}
		}

		#endregion Constructor

		#region Métodos
		public void CambiarPaginaResultado(int nuevoIndicePagina)
		{
			this.vista.IndicePaginaResultado = nuevoIndicePagina;
			this.vista.ActualizarResultado();
		}

		public void Consultar()
		{
			try 
            {
                BOF.LlantaBOF bo = (BOF.LlantaBOF)this.InterfazUsuarioADato();
                List<LlantaBO> lst = controlador.ConsultarFiltro(dctx, bo); //RI0055

                this.vista.Resultado = lst;
                this.vista.ActualizarResultado();

				if (lst.Count < 1)
                    this.vista.MostrarMensaje("La búsqueda no produjo resultados", ETipoMensajeIU.INFORMACION,
                        "No se encontraron coincidencias con los criterios ingresados. Verifique sus datos");
			}
			catch (Exception ex)
			{
				throw new Exception(this.nombreClase + ".Consultar:" + ex.Message);
			}
		}
        
		public void PrepararBusqueda()
		{
			this.vista.LimpiarSession();
			this.vista.PrepararBusqueda();

            this.EstablecerSeguridad(); //SC_0008
		}
        
        #region SC_0008
        private void EstablecerSeguridad()
        {
            try
            {
                //se valida que los datos del usuario y la unidad operativa no sean nulos
                if (this.vista.UsuarioID == null) throw new Exception("El identificador del usuario no debe ser nulo");
                if (this.vista.UnidadOperativaID == null) throw new Exception("El identificador de la Unidad Operativa no debe ser nulo ");

                //Se crea el objeto de seguridad
                UsuarioBO usuario = new UsuarioBO() { Id = this.vista.UsuarioID };
                AdscripcionBO adscripcion = new AdscripcionBO() { UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaID } };
                SeguridadBO seguridadBO = new SeguridadBO(Guid.Empty, usuario, adscripcion);

                //Se obtienen las acciones a las cuales el usuario tiene permiso en este proceso
                List<CatalogoBaseBO> lst = FacadeBR.ConsultarAccion(this.dctx, seguridadBO);

                //Se valida si el usuario tiene permiso para consultar
                if (!this.ExisteAccion(lst, "CONSULTAR"))
                    this.vista.RedirigirSinPermisoAcceso();
                //Se valida si el usuario tiene permiso para registrar una llanta
                if (!this.ExisteAccion(lst, "INSERTAR"))
                    this.vista.PermitirRegistrar(false);
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".EstablecerSeguridad:" + ex.Message);
            }
        }
        private bool ExisteAccion(List<CatalogoBaseBO> acciones, string nombreAccion)
        {
            if (acciones != null && acciones.Count > 0 && acciones.Exists(p => p != null && p.Nombre != null && p.Nombre.Trim().ToUpper() == nombreAccion.Trim().ToUpper()))
                return true;

            return false;
        }
        #endregion

		public void VerDetalles(int index)
		{
			if (index >= this.vista.Resultado.Count || index < 0)
				throw new Exception("No se encontró la llanta seleccionada");

			BO.LlantaBO bo = this.vista.Resultado[index];

			this.vista.LimpiarSession();
			this.vista.EstablecerPaqueteNavegacion("LlantaBO", bo);

			this.vista.RedirigirADetalles();
		}

		private object InterfazUsuarioADato() 
        {
            BOF.LlantaBOF bo = new BOF.LlantaBOF();
            bo.Activo = vista.Activo;
			bo.Stock = vista.EnStock;
			bo.Codigo = vista.Codigo;
			bo.Medida = vista.Medida;
			bo.Revitalizada = vista.Revitalizada;
			bo.MontadoEn = new EnllantableProxyBO { EnllantableID = vista.UnidadID, TipoEnllantable = vista.TipoEnllantable };
            bo.Sucursal = new SucursalBO();            
            if (vista.SucursalID.HasValue) {
                bo.Sucursal.Id = vista.SucursalID;
                bo.Sucursal.UnidadOperativa = new UnidadOperativaBO();
                bo.Sucursal.UnidadOperativa.Id = vista.UnidadOperativaID;
            } else {
                AdscripcionBO adscripcion = new AdscripcionBO() { UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaID } };
                UsuarioBO usuario = new UsuarioBO() { Id = this.vista.UsuarioID };
                bo.Sucursales = FacadeBR.ConsultarSucursalesSeguridad(this.dctx, new SeguridadBO(Guid.Empty, usuario, adscripcion));
            }

            return bo;
        }

        #region Métodos para el Buscador
        /// <summary>
        /// Despliega el Resultado del Buscador
        /// </summary>
        /// <param name="catalogo">Catalogo en el que se realizo la búsqueda</param>
        /// <param name="selecto">Objeto Resultante</param>
        public void DesplegarResultadoBuscador(string catalogo, object selecto)
        {
            switch (catalogo)
            {
                case "UnidadIdealease":
                    UnidadBOF unidad = (UnidadBOF)selecto ?? new UnidadBOF();
                    if (unidad.NumeroSerie != null)
                    {
                        vista.NumeroSerie = unidad.NumeroSerie;
                        vista.UnidadID = unidad.UnidadID;
                        vista.TipoEnllantable = ETipoEnllantable.Unidad;
                    }
                    else
                    {
                        vista.NumeroSerie = string.Empty;
                        vista.UnidadID = null;
                        vista.TipoEnllantable = null;
                    }

                    break;
                case "Sucursal":
                    SucursalBO sucursal = (SucursalBO)selecto;
                    if (sucursal != null && sucursal.Id != null)
                        this.vista.SucursalID = sucursal.Id;
                    else
                        this.vista.SucursalID = null;

                    if (sucursal != null && sucursal.Nombre != null)
                        this.vista.SucursalNombre = sucursal.Nombre;
                    else
                        this.vista.SucursalNombre = null;
                    break;
            }
        }

        /// <summary>
        /// Prepara un BO para la Búsqueda en su respectivo catalogo
        /// </summary>
        /// <param name="catalogo">catalogo donde se realizara la búsqueda</param>
        /// <returns></returns>
        public object PrepararBOBuscador(string catalogo)
        {
            object obj = null;

            switch (catalogo)
            {
                case "UnidadIdealease":
                    UnidadBOF unidad = new UnidadBOF();

                    if (!string.IsNullOrEmpty(vista.NumeroSerie))
                        unidad.NumeroSerie = vista.NumeroSerie;

                    obj = unidad;
                    break;
                case "Sucursal":
                    SucursalBOF sucursal = new SucursalBOF();
                    sucursal.UnidadOperativa = new UnidadOperativaBO();
                    sucursal.UnidadOperativa.Id = this.vista.UnidadOperativaID;
                    sucursal.Nombre = this.vista.SucursalNombre;
                    sucursal.Usuario = new UsuarioBO() { Id = this.vista.UsuarioID };
                    obj = sucursal;
                    break;
            }

            return obj;
        }
        #endregion
        #endregion
    }
}