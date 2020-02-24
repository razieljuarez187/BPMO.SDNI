//Satisface a la solicitud de Cambio SC0001
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using BPMO.Basicos.BO;
using BPMO.Facade.SDNI.BR;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Comun.BR;
using BPMO.SDNI.Facturacion.AplicacionesFacturacion.VIS;

namespace BPMO.SDNI.Facturacion.AplicacionesFacturacion.PRE
{
    public class ConsultarMasterFacturacionPRE
    {
        #region Atributos
        private const string nombreClase = "ConsultarMasterFacturacionPRE";
        private readonly IConsultarMasterFacturacionVIS Vista;
        private readonly IDataContext dataContext;
        #endregion 

        #region Constructores
        public ConsultarMasterFacturacionPRE(IConsultarMasterFacturacionVIS vista)
        {
            if(vista == null) throw new ArgumentNullException("La vista proprocionada no puede ser nula.");
            Vista = vista;

            dataContext = FacadeBR.ObtenerConexion();
        }
        #endregion

        #region Metodos

        /// <summary>
        /// Consulta la URL de la página maestra de Facturación
        /// </summary>
        public void ConsultarUrl()
        {
            try
            {
                var moduloBr = new ModuloBR();
                var configuracionUnidadOperativa = new ConfiguracionUnidadOperativaBO {UnidadOperativa = new UnidadOperativaBO {Id = Vista.UnidadOperativaID}};
                string moduloId = string.Empty;
                if (this.Vista.ModuloID == null)
                    throw new Exception("Es necesario definir el ModuloID");
                else
                    moduloId = this.Vista.ModuloID.ToString();
                var listaConfiguracion = moduloBr.ConsultarConfiguracionUnidadOperativa(dataContext, configuracionUnidadOperativa, Int32.Parse(moduloId));

                if (listaConfiguracion.Any()){
                    if (listaConfiguracion[0].ConfiguracionModulo != null)
                    {
                        var url = listaConfiguracion[0].ConfiguracionModulo.UrlMasterFacturacion;
                        if (!String.IsNullOrEmpty(url))
                        {
                            var SucId = ConsultarSucursalUnica();


                            Vista.RedirigirMaster(url, Vista.UnidadOperativaID.ToString(),
                                SucId != null ? SucId.ToString() : string.Empty,
                                Vista.UsuarioID.ToString(), Vista.Ambiente.ToString());
                        }
                        else
                            throw new Exception("No se encuentra configurada la dirección de la Master de Facturación");
                    }
                    else
                        throw new Exception("No se encontraron configuraciones para la Unidad Operativa");
                }
                else
                    throw new Exception("No se encontraron configuraciones para la Unidad Operativa");
            }
            catch (Exception ex)
            {
                var strMetodo = new StackFrame().GetMethod().Name;
                var strMsg = string.Format("{0}.{1}: {2}", nombreClase, strMetodo, ex.Message);
                Vista.MostrarMensaje("Inconsistencias al Redirigir a la Master de Facturación", ETipoMensajeIU.ERROR, strMsg);
            }
        }


        private int? ConsultarSucursalUnica()
        {
            var seguridad = new SeguridadBO(Guid.Empty,
                new UsuarioBO {Id = Vista.UsuarioID},
                new AdscripcionBO
                {
                    UnidadOperativa = new UnidadOperativaBO {Id = Vista.UnidadOperativaID}
                });

            List<SucursalBO> resultado = FacadeBR.ConsultarSucursalesSeguridad(dataContext, seguridad) ??
                                          new List<SucursalBO>();

            return resultado.Count == 1 ? resultado.First().Id : null;
        }
        #endregion

        public void ValidarAcceso()
        {
            try
            {
                //Valida que usuario y la unidad operativa no sean nulos
                if (Vista.UsuarioID == null) throw new Exception("El identificador del usuario no debe ser nulo");
                if (Vista.UnidadOperativaID == null) throw new Exception("El identificador de la Unidad Operativa no debe ser nulo");
                //Se crea el objeto de seguridad                
                SeguridadBO seguridadBO = this.CrearObjetoSeguridad();

                //Se valida si el usuario tiene permiso a la acción principal
                if (!FacadeBR.ExisteAccion(this.dataContext, "CONSULTAR", seguridadBO))
                    Vista.RedirigirSinPermisoAcceso();
            }
            catch (Exception ex)
            {
                throw new Exception(nombreClase + ".ValidarAcceso: " + ex.GetBaseException().Message);
            }
        }

        /// <summary>
        /// Crea un objeto de seguridad con los datos actuales de la sesión de usuario en curso
        /// </summary>
        /// <returns>Objeto de tipo seguridad</returns>
        private SeguridadBO CrearObjetoSeguridad()
        {
            var usuario = new UsuarioBO { Id = Vista.UsuarioID };
            var adscripcion = new AdscripcionBO { UnidadOperativa = new UnidadOperativaBO() { Id = Vista.UnidadOperativaID } };
            var seguridadBO = new SeguridadBO(Guid.Empty, usuario, adscripcion);

            return seguridadBO;
        }
    }
}