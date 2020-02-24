// Satisface al CU092 - Catálogo de Operadores
using System;
using System.Collections.Generic;
using BPMO.Basicos.BO;
using BPMO.Facade.SDNI.BR;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Comun.BOF;
using BPMO.SDNI.Comun.BR;
using BPMO.SDNI.Comun.VIS;

namespace BPMO.SDNI.Comun.PRE
{
    public class ConsultarOperadorPRE
    {
        #region Atributos
        private IDataContext dctx;
        private IConsultarOperadorVIS vista;
        private OperadorBR controlador;
        private string nombreClase = "ConsultarOperadorPRE";
        #endregion

        #region Constructor
        public ConsultarOperadorPRE(IConsultarOperadorVIS vista)
        {
            try
            {
                this.vista = vista;
                this.dctx = FacadeBR.ObtenerConexion();
                this.controlador = new OperadorBR();
            }
            catch (Exception ex)
            {
                this.vista.MostrarMensaje("Inconsistencias en la configuracion", ETipoMensajeIU.ERROR, nombreClase + ex.Message);
            }

        }
        #endregion

        #region Métodos
        public void PrepararBusqueda()
        {
            this.vista.LimpiarSesion();

            this.vista.CuentaClienteID = null;
            this.vista.CuentaClienteNombre = null;
            this.vista.Estatus = null;
            this.vista.LicenciaNumero = null;
            this.vista.Nombre = null;

            this.EstablecerFiltros();

            this.EstablecerSeguridad();
        }

        private void EstablecerFiltros()
        {
            try
            {
                Dictionary<string, object> paquete = this.vista.ObtenerPaqueteNavegacion("FiltrosOperador") as Dictionary<string, object>;
                if (paquete != null)
                {
                    if (paquete.ContainsKey("ObjetoFiltro"))
                    {
                        if (paquete["ObjetoFiltro"].GetType() == typeof(OperadorBO))
                            this.DatoAInterfazUsuario(paquete["ObjetoFiltro"]);
                        else
                            throw new Exception("Se esperaba un objeto OperadorBO, el objeto proporcionado no cumple con esta característica, intente de nuevo por favor.");
                    }
                    if (paquete.ContainsKey("Bandera"))
                    {
                        if ((bool)paquete["Bandera"])
                            this.ConsultarOperadores();
                    }
                }
                this.vista.LimpiarPaqueteNavegacion("FiltrosOperador");
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".EstablecerFiltros: " + ex.Message);
            }
        }

        private void EstablecerSeguridad()
        {
            try
            {
                //se valida que los datos del usuario y la unidad operativa no sean nulos
                if (this.vista.UsuarioID == null) throw new Exception("El identificador del usuario no debe ser nulo");
                if (this.vista.UnidadOperativaID == null) throw new Exception("El identificador de la Unidad Operativa no debe ser nulo ");

                UsuarioBO usr = new UsuarioBO { Id = this.vista.UsuarioID };
                AdscripcionBO adscripcion = new AdscripcionBO { UnidadOperativa = new UnidadOperativaBO { Id = this.vista.UnidadOperativaID } };
                SeguridadBO seguridadBO = new SeguridadBO(Guid.Empty, usr, adscripcion);

                //Se obtienen las acciones a las cuales el usuario tiene permiso en este proceso
                List<CatalogoBaseBO> lst = FacadeBR.ConsultarAccion(dctx, seguridadBO);

                //Se valida si el usuario tiene permiso para consultar
                if (!this.ExisteAccion(lst, "CONSULTAR"))
                    this.vista.RedirigirSinPermisoAcceso();
                //Se valida si el usuario tiene permiso para registrar
                if (!this.ExisteAccion(lst, "INSERTARCOMPLETO"))
                    this.vista.PermitirRegistrar(false);
            }
            catch (Exception ex)
            {
                throw new Exception(nombreClase + ".EstablecerSeguridad:" + ex.Message);
            }
        }
        private bool ExisteAccion(List<CatalogoBaseBO> acciones, string accion)
        {
            if (acciones != null && acciones.Count > 0 && acciones.Exists(p => p != null && p.Nombre != null && p.Nombre.Trim().ToUpper() == accion.Trim().ToUpper()))
                return true;

            return false;
        }

        private Object InterfazUsuarioADato()
        {
            OperadorBO bo = new OperadorBO();
            bo.Cliente = new CuentaClienteIdealeaseBO();
            bo.Cliente.UnidadOperativa = new UnidadOperativaBO();
            bo.Licencia = new LicenciaBO();

            bo.Cliente.Id = this.vista.CuentaClienteID;
            bo.Cliente.Nombre = this.vista.CuentaClienteNombre;
            bo.Cliente.UnidadOperativa.Id = this.vista.UnidadOperativaID;
            bo.Estatus = this.vista.Estatus;
            bo.Nombre = this.vista.Nombre;
            bo.Licencia.Numero = this.vista.LicenciaNumero;

            return bo;
        }
        private void DatoAInterfazUsuario(Object obj)
        {
            OperadorBO bo = (OperadorBO)obj;
            if (bo.Cliente == null)
                bo.Cliente = new CuentaClienteIdealeaseBO();
            if (bo.Licencia == null)
                bo.Licencia = new LicenciaBO();

            this.vista.CuentaClienteID = bo.Cliente.Id;
            this.vista.CuentaClienteNombre = bo.Cliente.Nombre;
            this.vista.Estatus = bo.Estatus;
            this.vista.LicenciaNumero = bo.Licencia.Numero;
            this.vista.Nombre = bo.Nombre;
        }

        public void ConsultarOperadores()
        {
            string s;
            if ((s = this.ValidarCampos()) != null)
            {
                this.vista.MostrarMensaje(s, ETipoMensajeIU.ADVERTENCIA, null);
                return;
            }

            try
            {
                OperadorBO bo = (OperadorBO)this.InterfazUsuarioADato();
                List<OperadorBO> lst = this.controlador.ConsultarCompleto(this.dctx, bo);

                if (lst.Count < 1)
                    this.vista.MostrarMensaje("La búsqueda no produjo resultados", ETipoMensajeIU.INFORMACION,
                        "No se encontraron coincidencias con los criterios ingresados. Verifique sus datos");

                this.vista.EstablecerResultado(lst);
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".ConsultarOperadores: " + ex.Message);
            }
        }

        private string ValidarCampos()
        {
            string s = "";

            if (this.vista.UnidadOperativaID == null)
                s += "UnidadOperativaID, ";

            if (s != null && s.Trim().CompareTo("") != 0)
                return "Los siguientes campos no pueden estar vacíos: \n" + s.Substring(0, s.Length - 2);

            return null;
        }

        public void IrADetalle(int? operadorID)
        {
            try
            {
                if (operadorID == null)
                    throw new Exception("No se encontró el registro seleccionado.");

                OperadorBO bo = new OperadorBO { OperadorID = operadorID };

                this.vista.LimpiarSesion();

                Dictionary<string, object> paquete = new Dictionary<string, object>();
                paquete.Add("ObjetoFiltro", this.InterfazUsuarioADato());
                paquete.Add("Bandera", true);

                this.vista.EstablecerPaqueteNavegacion("FiltrosOperador", paquete);
                this.vista.EstablecerPaqueteNavegacion("OperadorBO", bo);

                this.vista.RedirigirADetalle();
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".IrADetalle: " + ex.Message);
            }
        }

        #region MetodosBuscador
        /// <summary>
        /// Prepara un BO para la Busqueda en su respectivo catalogo
        /// </summary>
        /// <param name="catalogo">catalogo donde se realizara la busqueda</param>
        /// <returns></returns>
        public object PrepararBOBuscador(string catalogo)
        {
            object obj = null;

            switch (catalogo)
            {
                case "CuentaClienteIdealease":
                    var cliente = new CuentaClienteIdealeaseBOF { Cliente = new ClienteBO() };
                    cliente.Nombre = this.vista.CuentaClienteNombre;
                    cliente.UnidadOperativa = new UnidadOperativaBO();
                    cliente.UnidadOperativa.Id = this.vista.UnidadOperativaID;
                    obj = cliente;
                    break;
            }
            return obj;
        }

        /// <summary>
        /// Despliega el Resultado del Buscador
        /// </summary>
        /// <param name="catalogo">Catalogo en el que se realizo la busqueda</param>
        /// <param name="selecto">Objeto Resultante</param>
        public void DesplegarResultadoBuscador(string catalogo, object selecto)
        {
            switch (catalogo)
            {
                case "CuentaClienteIdealease":
                    CuentaClienteIdealeaseBOF cliente = (CuentaClienteIdealeaseBOF)selecto ?? new CuentaClienteIdealeaseBOF();
                    if (cliente == null)
                        cliente = new CuentaClienteIdealeaseBOF();

                    this.vista.CuentaClienteID = cliente.Id;
                    this.vista.CuentaClienteNombre = cliente.Nombre;
                    break;
            }
        }
        #endregion
        #endregion
    }
}
