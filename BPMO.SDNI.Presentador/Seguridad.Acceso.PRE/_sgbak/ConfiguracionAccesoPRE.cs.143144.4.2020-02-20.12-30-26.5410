//Satisface al CU061 - Acceso al Sistema
//Satisface a la Solicitud de Cambios SC0004 de BEP1401
using System;
using System.Collections.Generic;
using System.Linq;
using BPMO.Basicos.BO;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Primitivos.Enumeradores;
using BPMO.Facade.SDNI.BR;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Comun.BR;
using BPMO.SDNI.Seguridad.Acceso.VIS;

namespace BPMO.SDNI.Seguridad.Acceso.PRE
{
    public class ConfiguracionAccesoPRE
    {
        #region Atributos
        private IConfiguracionAccesoVIS vista;
        private IDataContext dataContext = null;
        //RQM 14078, Lista que se utiliza para validar mas de una unidad operativa y es de carácter global.
        private List<AdscripcionBO> lstAdscripciones = new List<AdscripcionBO>();
        #endregion

        #region Constructores
        public ConfiguracionAccesoPRE(IConfiguracionAccesoVIS vistaActual) {
            this.vista = vistaActual;
            try {
                dataContext = FacadeBR.ObtenerConexion();
            } catch (Exception ex) {
                this.vista.MostrarMensaje("Inconsistencias en los parámetros de configuración", ETipoMensajeIU.ERROR, ex.Message);
            }
            
        }
        #endregion

        #region Métodos
        public void ObtenerDatosAdscripcion() {
            try
            {
                #region listaAdscripcion

                #region SC0004
                var adscripcionParam = vista.Adscripcion ?? new AdscripcionBO(); 
                #endregion
                
                List<AdscripcionBO> lstAdscripcion = FacadeBR.ConsultarAdscripcionSeguridad(dataContext, adscripcionParam, this.vista.Usuario);
                if (lstAdscripcion.Count > 0)
                {
                    //Valida si solo hay 1 unidad operativa diferente entre las adscripciones para poder pasar directo
                    if (lstAdscripcion.GroupBy( i => i.UnidadOperativa.Id, (key, group) => group.First()).ToList().Count == 1)
                    {
                        #region SC0004
                        // Ubicar la Adscripcion con Sucursal y Departamento Nulos
                        var adscripcionSeleccionada =
                            lstAdscripcion.FirstOrDefault(x =>
                                (x.Sucursal == null || x.Sucursal.Id == null) &&
                                (x.Departamento == null || x.Departamento.Id == null));

                        // Si no Existe, generar una adscripcion con Sucursal y Departamento Nulos.
                        if (adscripcionSeleccionada == null) adscripcionSeleccionada = new AdscripcionBO { UnidadOperativa = lstAdscripcion.First().UnidadOperativa, Sucursal = new SucursalBO(), Departamento = new DepartamentoBO()}; 
                        #endregion
                        //RQM 14078
                        lstAdscripciones.Add(adscripcionSeleccionada); 
                        SeleccionarAdscripcion(adscripcionSeleccionada);
                    }
                    else {
                        //RQM 14078
                        lstAdscripciones = lstAdscripcion;

                        vista.Adscripciones = lstAdscripcion;
                        this.vista.CargarDatosAdscripcion();
                    }
                }
                else
                {
                    this.vista.MostrarMensaje("No tiene permisos configurados para trabajar con el sistema o sus permisos han caducado." +
                    "Para mas información póngase en contacto con el administrador del sistema", ETipoMensajeIU.INFORMACION);
                }
                #endregion
            } catch (Exception ex) {
                this.vista.MostrarMensaje("Surgió un error al obtener las adscripciones, si el problema persiste, contacte con el administrador del sistema",
                    ETipoMensajeIU.ERROR, ex.Message);
            }
        }
        public void SeleccionarAdscripcion(AdscripcionBO adscripcion = null) {
            try
            {
                if (adscripcion == null)
                {
                    #region Se selecciona la adscripción que el usuario manualmente eligió
                    if (this.vista.UnidadOperativa != null)
                    {
                        AdscripcionBO adscripcionBO = new AdscripcionBO
                        {
                            UnidadOperativa = new UnidadOperativaBO { Id = this.vista.UnidadOperativa }
                        };

                        #region Verficar si la Adscripcion cambio
                        if (!VerificarCambioAdscripcion(adscripcionBO))
                        {
                            this.vista.Adscripciones = null;
                            this.vista.EnviarAInicio();
                            return;
                        }
                        #endregion

                        adscripcionBO.UnidadOperativa = FacadeBR.ConsultarUnidadOperativaCompleto(dataContext, adscripcionBO.UnidadOperativa).FirstOrDefault();

                        if (adscripcionBO.UnidadOperativa != null && adscripcionBO.UnidadOperativa.Id != null)
                        {
                            lstAdscripciones.Add(adscripcionBO);
                            this.vista.Adscripciones = null;
                            this.vista.ListadoProcesos = null;
                            this.vista.Adscripcion = adscripcionBO;

                            this.EstablecerConfiguracionModulo(adscripcionBO);

                            this.vista.EnviarAInicio();
                        }
                        else
                        {
                            this.vista.MostrarMensaje("Adscripcion no válida", ETipoMensajeIU.ERROR,
                                "Es probable que usted no cuente con una adscripcion válida, si el problema persiste, por favor contacte al administrador del sistema");
                        }
                    }
                    else
                    {
                        this.vista.MostrarMensaje("EL campo Unidad Operativa es obligatorio", ETipoMensajeIU.INFORMACION);
                    }
                    #endregion
                }
                else
                {
                    #region Se selecciona la adscripción enviada. Posiblemente porque sea la única configurada
                    if (adscripcion.UnidadOperativa != null && adscripcion.Sucursal != null && adscripcion.Departamento != null)
                    {
                        AdscripcionBO adsBO = new AdscripcionBO
                        {
                            UnidadOperativa = new UnidadOperativaBO { Id = adscripcion.UnidadOperativa.Id },
                            Sucursal = new SucursalBO { Id = adscripcion.Sucursal.Id },
                            Departamento = new DepartamentoBO { Id = adscripcion.Departamento.Id }
                        };

                        #region Verificar si la Adscripcion cambio
                        if (!VerificarCambioAdscripcion(adsBO))
                        {
                            this.vista.Adscripciones = null;
                            this.vista.EnviarAInicio();
                            return;
                        }
                        #endregion

                        adsBO.UnidadOperativa = FacadeBR.ConsultarUnidadOperativaCompleto(this.dataContext, adsBO.UnidadOperativa).FirstOrDefault();

                        //Verifica que los datos de la adscripción estén correctos antes del redireccionamiento a la página de inicio
                        if (adsBO.UnidadOperativa != null && adsBO.UnidadOperativa.Id != null)
                        {
                            lstAdscripciones.Add(adsBO);
                            this.vista.Adscripciones = null;
                            this.vista.ListadoProcesos = null;
                            this.vista.Adscripcion = adsBO;

                            this.EstablecerConfiguracionModulo(adsBO);

                            this.vista.EnviarAInicio();
                        }
                        else
                        {
                            this.vista.MostrarMensaje("Adscripcion no válida", ETipoMensajeIU.ERROR,
                                "Es probable que usted no cuente con una adscripción válida, Si el problema persiste, por favor contacte al administrador del sistema");
                        }
                    }
                    else
                    {
                        this.vista.MostrarMensaje("Los campos unidad operativa, sucursal y taller son obligatorios", ETipoMensajeIU.INFORMACION);
                    }
                    #endregion
                }
            }
            catch (Exception ex) {
                this.vista.MostrarMensaje("Surgió un problema al cargar la Unidad Operativa, por favor refresque la página",
                    ETipoMensajeIU.ERROR, ex.Message);
            }
        }

        private bool VerificarCambioAdscripcion(AdscripcionBO adscripcion) {
            if (this.vista.Adscripcion != null)
            {
                if (this.vista.Adscripcion.UnidadOperativa.Id == adscripcion.UnidadOperativa.Id)
                {
                    return false;
                }
            }
            return true;
        }

        private void EstablecerConfiguracionModulo(AdscripcionBO adscripcion)
        {
            try
            {
                ConfiguracionModuloBO configuracion = null;
                int? ModuloIDEmpresas = this.vista.ModuloID;

                //RQM 14078, se añade el modulo en cero para que en la consulta pueda recuperar la información del identificador del modulo.
                if (lstAdscripciones != null){
                    if (lstAdscripciones.Any())
                        ModuloIDEmpresas = 0;
                }
                

                if (this.vista.ModuloID == null)
                    throw new Exception("No se encontró el identificador del módulo en la configuración del sistema");

                if (adscripcion != null && adscripcion.UnidadOperativa != null && adscripcion.UnidadOperativa.Id != null)
                {
                    List<ModuloBO> lst = new ModuloBR().ConsultarCompleto(dataContext, new ModuloBO() { ModuloID = ModuloIDEmpresas }, adscripcion.UnidadOperativa.Id.Value);
                    if (lst != null && lst.Count > 0){
                        configuracion = lst[0].ObtenerConfiguracion(adscripcion.UnidadOperativa.Id.Value);
                        this.vista.ModuloID = lst[0].ModuloID;
                    }
                }

                this.vista.EstablecerConfiguracionModulo(configuracion);
            }
            catch (Exception ex)
            {
                throw new Exception("ConfiguracionAccesoPRE.EstablecerConfiguracionModulo: " + ex.Message);
            }
        }
        #endregion
    }
}
