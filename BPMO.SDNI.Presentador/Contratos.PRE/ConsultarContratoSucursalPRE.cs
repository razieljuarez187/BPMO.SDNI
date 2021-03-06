﻿using System;
using System.Collections.Generic;
using System.Linq;
using BPMO.Basicos.BO;
using BPMO.Facade.SDNI.BR;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Comun.BOF;
using BPMO.SDNI.Comun.BR;
using BPMO.SDNI.Contratos.BO;
using BPMO.SDNI.Contratos.BR;
using BPMO.SDNI.Contratos.VIS;

namespace BPMO.SDNI.Contratos.PRE
{
    /// <summary>
    /// Presentador para Consultar los Contratos antes de Cambiarlos de Sucursal
    /// </summary>
    public class ConsultarContratoSucursalPRE
    {
        #region Atributos
        /// <summary>
        /// Vista que contiene las propiedades y metodos de presentacion
        /// </summary>
        private readonly IConsultarContratoSucursalVIS vista;
        /// <summary>
        /// DataContext que provee acceso a la base de datos
        /// </summary>
        private readonly IDataContext dataContext;
        /// <summary>
        /// Controlador de información General de Contratos
        /// </summary>
        private readonly ContratoBR controlador;
        private static readonly string nombreClase = typeof(ConsultarContratoSucursalPRE).Name;
        #endregion
        #region Consutructores
        /// <summary>
        /// Constructor del presentador
        /// </summary>
        /// <param name="view">Vista con la cual iteractuara el presentador</param>
        public ConsultarContratoSucursalPRE(IConsultarContratoSucursalVIS view)
        {
            try
            {
                if(view != null) vista = view;
                else
                    throw new Exception("La vista proporcionada no puede ser Nula.");

                dataContext = FacadeBR.ObtenerConexion();
                controlador = new ContratoBR();
            }
            catch(Exception ex)
            {
                vista.MostrarMensaje("Inconsistencias en la construcción del presentador", ETipoMensajeIU.ERROR, nombreClase + ".ConsultarContratoSucursalPRE: " + ex.Message);
            }
        }
        #endregion
        #region Metodos
        /// <summary>
        /// Se ejecuta al Iniciar la página, prepara la interfaz para realizar la primera búsqueda
        /// </summary>
        public void PrepararBusqueda()
        {
            this.LimpiarCampos();
            this.vista.LimpiarSesion();
            this.EstablecerSeguridad("CONSULTAR");
        }
        /// <summary>
        /// Limpia los campos usados por la vista
        /// </summary>
        private void LimpiarCampos()
        {
            this.vista.ClienteId = null;
            this.vista.ClienteNombre = null;
            this.vista.SucursalId = null;
            this.vista.SucursalNombre = null;
            this.vista.ContratoId = null;
            this.vista.NumeroContrato = null;
            this.vista.TipoContrato = null;
        }
        /// <summary>
        /// Valida si tiene permiso para acceder a la interfaz de consulta
        /// </summary>
        private void EstablecerSeguridad(String accion)
        {
            try
            {
                //se valida que los datos del usuario y la unidad operativa no sean nulos
                if(this.vista.UsuarioID == null) throw new Exception("El identificador del usuario no debe ser nulo");
                if(this.vista.UnidadOperativaID == null) throw new Exception("El identificador de la Unidad Operativa no debe ser nulo ");
                //Se crea el objeto de Seguridad
                SeguridadBO seguridadBO = this.CrearObjetoSeguridad();
                //Se valida si el usuario tiene permisos para realizar la acción
                if(!FacadeBR.ExisteAccion(this.dataContext, accion, seguridadBO))
                    this.vista.RedirigirSinPermisoAcceso();
            }
            catch(Exception ex)
            {
                throw new Exception(nombreClase + ".EstablecerSeguridad: " + ex.Message);
            }
        }
        /// <summary>
        /// Crea un objeto de seguridad con los datos actuales de la sesión de usuario en curso
        /// </summary>
        /// <returns>Objeto de tipo seguridad</returns>
        private SeguridadBO CrearObjetoSeguridad()
        {
            UsuarioBO usuario = new UsuarioBO() { Id = this.vista.UsuarioID };
            AdscripcionBO adscripcion = new AdscripcionBO() { UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaID } };
            SeguridadBO seguridadBO = new SeguridadBO(Guid.Empty, usuario, adscripcion);

            return seguridadBO;
        }
        /// <summary>
        /// Consulta los Contratos en CURSO segun los filtros
        /// </summary>
        public void Consultar()
        {
            try
            {
                var contratoBo = (ContratoProxyBO)this.InterfazUsuarioDato();
                contratoBo.Estatus = EEstatusContrato.EnCurso;
                var listaContratos = controlador.Consultar(this.dataContext, contratoBo);
                if(!listaContratos.Any())
                {
                    this.vista.PresentarResultadoConsulta(listaContratos.ConvertAll(x=>(ContratoBO)x));
                    this.vista.MostrarMensaje("La búsqueda no produjo resultados", ETipoMensajeIU.INFORMACION, "No se encontraron coincidencias con los criterios ingresados. Verifique sus datos");
                    return;
                }

                String sucursalesNoEncontradas = String.Empty;
                String clientesNoEncontrados = String.Empty;

                var listaClientes = new List<CuentaClienteIdealeaseBO>();
                CuentaClienteIdealeaseBR cuentaClienteBr = new CuentaClienteIdealeaseBR();

                var listaSucursales = new List<SucursalBO>();
                foreach(ContratoProxyBO bo in listaContratos)
                {
                    if(listaSucursales.Any(x => x.Id == bo.Sucursal.Id))
                        bo.Sucursal = listaSucursales.First(x => x.Id == bo.Sucursal.Id);
                    else
                    {
                        var listaTempSucursal = FacadeBR.ConsultarSucursal(this.dataContext, new SucursalBO() { Id = bo.Sucursal.Id, UnidadOperativa = new UnidadOperativaBO{ Id = this.vista.UnidadOperativaID} });
                        if(listaTempSucursal.Any())
                        {
                            listaSucursales.Add(listaTempSucursal.First());
                            bo.Sucursal = listaTempSucursal.First();
                        }
                        else
                            sucursalesNoEncontradas = String.IsNullOrEmpty(sucursalesNoEncontradas) ? bo.Sucursal.Id.Value.ToString() : sucursalesNoEncontradas + ", " + bo.Sucursal.Id.Value.ToString();
                    }

                    if(listaClientes.Any(x => x.Id == bo.Cliente.Id))
                        bo.Cliente = listaClientes.First(x => x.Id == bo.Cliente.Id);
                    else
                    {
                        var listaTempClientes = cuentaClienteBr.Consultar(this.dataContext, new CuentaClienteIdealeaseBO() { Id = bo.Cliente.Id, UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaID } });
                        if(listaTempClientes.Any())
                        {
                            listaClientes.Add(listaTempClientes.First());
                            bo.Cliente = listaTempClientes.First();
                        }
                        else
                            clientesNoEncontrados = String.IsNullOrEmpty(clientesNoEncontrados) ? bo.Cliente.Id.Value.ToString() : clientesNoEncontrados + ", " + bo.Cliente.Id.Value.ToString();
                    }
                }

                var contratosRd = listaContratos.Where(x => x.Tipo == ETipoContrato.RD).ToList().OrderBy(y => y.NumeroContrato).ToList();
                var contratosFsl = listaContratos.Where(x => x.Tipo == ETipoContrato.FSL).ToList().OrderBy(y => y.NumeroContrato).ToList();
                var contratosCm = listaContratos.Where(x => x.Tipo == ETipoContrato.CM).ToList().OrderBy(y => y.NumeroContrato).ToList();
                var contratosSd = listaContratos.Where(x => x.Tipo == ETipoContrato.SD).ToList().OrderBy(y => y.NumeroContrato).ToList();
                listaContratos.Clear();
                listaContratos.AddRange(contratosRd);
                listaContratos.AddRange(contratosFsl);
                listaContratos.AddRange(contratosCm);
                listaContratos.AddRange(contratosSd);

                this.vista.PresentarResultadoConsulta(listaContratos.ConvertAll(x => (ContratoBO)x));

                string mensaje = String.Empty;
                if(!String.IsNullOrEmpty(sucursalesNoEncontradas))
                    mensaje += "No se encontraron las Sucursales con los siguientes identificadores: " + sucursalesNoEncontradas;
                if(!String.IsNullOrEmpty(clientesNoEncontrados))
                    mensaje += "No se encontraron los Clientes con los siguientes identificadores (CuentaClienteId): " + clientesNoEncontrados;

                if(!String.IsNullOrEmpty(mensaje))
                    this.vista.MostrarMensaje(mensaje, ETipoMensajeIU.INFORMACION);

            }
            catch(Exception ex)
            {
                throw new Exception(nombreClase + ".Consultar: " + ex.Message);
            }
        }
        /// <summary>
        /// Realiza los pasos para solicitar redireccion a la pantalla de Detalle
        /// </summary>
        public void RedirigirADetalle()
        {
            this.EstablecerSeguridad("CAMBIARSUCURSALCONTRATO");
            ContratoProxyBO contratoBo = (ContratoProxyBO)this.InterfazUsuarioDato();
            this.vista.EstablecerPaqueteNavegacion("ContratoProxyBO", contratoBo);
            this.vista.LimpiarSesion();
            this.vista.RedirigirADetalle();
        }
        /// <summary>
        /// Convierte los datos de la interfaz en un Objeto
        /// </summary>
        /// <returns>El objeto devuelto con los datos de la interfaz</returns>
        private Object InterfazUsuarioDato()
        {
            ContratoProxyBO contratoBo = new ContratoProxyBO()
            {
                Cliente = new CuentaClienteIdealeaseBO(),
                Sucursal = new SucursalBO() { UnidadOperativa = new UnidadOperativaBO() },
                Estatus = EEstatusContrato.EnCurso
            };

            if(this.vista.ContratoId != null)
                contratoBo.ContratoID = this.vista.ContratoId;
            if(this.vista.ClienteId != null)
                contratoBo.Cliente.Id = this.vista.ClienteId;
            if(this.vista.UnidadOperativaID != null)
                contratoBo.Sucursal.UnidadOperativa.Id = this.vista.UnidadOperativaID;
            if(this.vista.SucursalId != null)
                contratoBo.Sucursal.Id = this.vista.SucursalId;
            if(!String.IsNullOrEmpty(this.vista.NumeroContrato))
                contratoBo.NumeroContrato = this.vista.NumeroContrato;
            if(this.vista.TipoContrato != null)
                contratoBo.Tipo = this.vista.TipoContrato;

            return contratoBo;
        }
        #region Métodos para el Buscador
        /// <summary>
        /// Prepara un BO para la Busqueda en su respectivo catalogo
        /// </summary>
        /// <param name="catalogo">catalogo donde se realizara la busqueda</param>
        /// <returns>Objeto con los parámetros de búsqueda</returns>
        public object PrepararBOBuscador(string catalogo)
        {
            object obj = null;

            switch(catalogo)
            {
                case "CuentaClienteIdealease":
                    CuentaClienteIdealeaseBOF cliente = new CuentaClienteIdealeaseBOF { Nombre = vista.ClienteNombre, UnidadOperativa = vista.UnidadOperativa, Cliente = new ClienteBO(), Activo = true };
                    obj = cliente;
                    break;
                case "Sucursal":
                    Facade.SDNI.BOF.SucursalBOF sucursal = new Facade.SDNI.BOF.SucursalBOF();
                    sucursal.UnidadOperativa = this.vista.UnidadOperativa;
                    sucursal.Nombre = this.vista.SucursalNombre;
                    sucursal.Usuario = new UsuarioBO() { Id = this.vista.UsuarioID };
                    obj = sucursal;
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
            switch(catalogo)
            {
                case "CuentaClienteIdealease":
                    CuentaClienteIdealeaseBOF cliente = (CuentaClienteIdealeaseBOF)selecto ?? new CuentaClienteIdealeaseBOF();

                    if(cliente.Cliente == null)
                        cliente.Cliente = new ClienteBO();

                    vista.ClienteId = cliente.Id;

                    vista.ClienteNombre = !string.IsNullOrEmpty(cliente.Nombre) ? cliente.Nombre : string.Empty;
                    break;
                case "Sucursal":
                    SucursalBO sucursal = (SucursalBO)selecto;
                    if(sucursal != null && sucursal.Id != null)
                        this.vista.SucursalId = sucursal.Id;
                    else
                        this.vista.SucursalId = null;

                    if(sucursal != null && sucursal.Nombre != null)
                        this.vista.SucursalNombre = sucursal.Nombre;
                    else
                        this.vista.SucursalNombre = null;
                    break;
            }
        }
        #endregion

        #endregion
    }
}
