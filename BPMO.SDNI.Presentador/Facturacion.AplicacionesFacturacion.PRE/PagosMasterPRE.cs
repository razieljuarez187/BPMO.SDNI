// Satisface al caso de uso CU004 - Consulta de Pagos a Facturar
// Satisface a la solicitud de cambio SC0015
// Satisface a la solicitud de cambio SC0035
using System;
using System.Collections.Generic;
using System.Linq;
using BPMO.Basicos.BO;
using BPMO.Facade.SDNI.BOF;
using BPMO.Facade.SDNI.BR;
using BPMO.Patterns.Creational.DataContext;
using BPMO.SDNI.Comun.BOF;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Facturacion.AplicacionesFacturacion.VIS;
using BPMO.SDNI.Facturacion.InfraestructuraPagos.BO;
using BPMO.SDNI.Facturacion.InfraestructuraPagos.BOF;
using BPMO.SDNI.Facturacion.InfraestructuraPagos.BR;

namespace BPMO.SDNI.Facturacion.AplicacionesFacturacion.PRE
{
    public class PagosMasterPRE
    {
        #region Atributos
        private const string NombreClase = "PagosMasterPRE";
        private readonly IPagosMasterVIS Vista;
        private readonly IDataContext dataContext;
        private readonly PagoUnidadContratoBR Controlador;
        private readonly PagoContratoPSLBR ControladorPSL;
        #endregion

        #region Constructores

        public PagosMasterPRE(IPagosMasterVIS vista)
        {
            if (vista == null) throw new ArgumentNullException("vista");
            Vista = vista;

            dataContext = FacadeBR.ObtenerConexion();

            Controlador = new PagoUnidadContratoBR();
            ControladorPSL = new PagoContratoPSLBR();
        }
        #endregion

        #region Metodos
        /// <summary>
        /// Realiza los procesos en la primera carga del sistema
        /// </summary>
        public void PrimeraCarga()
        {
            DesplegarDepartamentos();
            ConsultarSucursales();
            ActualizarMarcadores();
        }

        /// <summary>
        /// Actualiza los marcadores de conteo de los pagos
        /// </summary>
        public void ActualizarMarcadores()
        {
            int TotalFacturar = 0;
            int TotalNoFacturado = 0;
            if (ETipoEmpresa.Idealease == (ETipoEmpresa)this.Vista.UnidadOperativaID)
            {
                //Pagos por Facturar
                var bof = new PagoUnidadContratoBOF
                {
                    Sucursales = Vista.SucursalesUsuario,
                    Activo = true,
                    EnviadoFacturacion = false,
                    FechaVencimientoFinal = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 23, 59, 59),
                    Facturado = false,
                    ReferenciaContrato = new ReferenciaContratoBO
                    {
                        UnidadOperativa = new UnidadOperativaBO { Id = Vista.UnidadOperativaID }
                    },
                    FacturaEnCeros = false,
                    BloqueadoCredito = false
                };
                TotalFacturar = Controlador.ContarPagos(dataContext, bof);

                // Pagos No Facturados
                bof = new PagoUnidadContratoBOF
                {
                    Sucursales = Vista.SucursalesUsuario,
                    Activo = true,
                    BloqueadoCredito = true,
                    ReferenciaContrato = new ReferenciaContratoBO
                    {
                        UnidadOperativa = new UnidadOperativaBO { Id = Vista.UnidadOperativaID }
                    },
                    FacturaEnCeros = false
                };
                TotalNoFacturado = Controlador.ContarPagos(dataContext, bof);
            }
            else
            {
                //Pagos por Facturar
                var bof = new PagoContratoPSLBOF
                {
                    Sucursales = Vista.SucursalesUsuario,
                    Activo = true,
                    EnviadoFacturacion = false,
                    FechaVencimientoFinal = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 23, 59, 59),
                    Facturado = false,
                    ReferenciaContrato = new ReferenciaContratoBO
                    {
                        UnidadOperativa = new UnidadOperativaBO { Id = Vista.UnidadOperativaID }
                    },
                    FacturaEnCeros = false,
                    BloqueadoCredito = false
                };
                TotalFacturar = ControladorPSL.ContarPagos(dataContext, bof);

                // Pagos No Facturados
                bof = new PagoContratoPSLBOF
                {
                    Sucursales = Vista.SucursalesUsuario,
                    Activo = true,
                    BloqueadoCredito = true,
                    ReferenciaContrato = new ReferenciaContratoBO
                    {
                        UnidadOperativa = new UnidadOperativaBO { Id = Vista.UnidadOperativaID }
                    },
                    FacturaEnCeros = false
                };
                TotalNoFacturado = ControladorPSL.ContarPagos(dataContext, bof);
            }
           
            Vista.TotalFacturar = TotalFacturar;
            Vista.TotalNoFacturado = TotalNoFacturado;
        }
        
        /// <summary>
        /// Consulta el listado de Sucursales del Usuario
        /// </summary>
        public void ConsultarSucursales() {
            try {
                if (this.Vista.SucursalesUsuario == null || this.Vista.SucursalesUsuario.Count == 0) {
                    this.Vista.SucursalesUsuario = FacadeBR.ConsultarSucursalesSeguridadSimple(dataContext,
                        new SeguridadBO(
                            Guid.Empty,
                            new UsuarioBO() { Id = this.Vista.UsuarioID },
                            new AdscripcionBO() { UnidadOperativa = new UnidadOperativaBO() { Id = this.Vista.UnidadOperativaID } }
                        )
                    );
                }
                this.Vista.CargarSucursales(this.Vista.SucursalesUsuario);
            } catch (Exception ex) {
                throw new Exception(NombreClase + ".ConsultarSucursales: Inconsistencias al consultar la lista de Sucursales del usuario." + ex.Message);
            }
        }

        /// <summary>
        /// Prepara el BO del Buscador
        /// </summary>
        /// <param name="catalogo"></param>
        /// <returns></returns>
        public object PrepararBOBuscador(string catalogo) {
            object obj = null;

            switch (catalogo) {
                case "CuentaClienteIdealease":
                    CuentaClienteIdealeaseBOF cliente = new CuentaClienteIdealeaseBOF {
                        Nombre = Vista.NombreCuentaCliente,
                        UnidadOperativa = new UnidadOperativaBO { Id = Vista.UnidadOperativaID },
                        Cliente = new ClienteBO()
                    };
                    obj = cliente;
                    break;
            }

            return obj;
        }

        /// <summary>
        /// Despliega en la Vista el Resultado del Buscador
        /// </summary>
        /// <param name="catalogo"></param>
        /// <param name="selecto"></param>
        public void DesplegarResultadoBuscador(string catalogo, object selecto) {
            switch (catalogo) {
                case "CuentaClienteIdealease":
                    CuentaClienteIdealeaseBOF cuenta = (CuentaClienteIdealeaseBOF)selecto ?? new CuentaClienteIdealeaseBOF();
                    if (cuenta.Cliente == null)
                        cuenta.Cliente = new ClienteBO();

                    Vista.CuentaClienteID = cuenta.Id;
                    Vista.NombreCuentaCliente = !string.IsNullOrEmpty(cuenta.Nombre) ? cuenta.Nombre : string.Empty;
                    break;
            }
        }

        public void DesplegarDepartamentos()
        {
            var listado = new List<EDepartamento>(Enum.GetValues(typeof(EDepartamento)).Cast<EDepartamento>());
            if (ETipoEmpresa.Idealease == (ETipoEmpresa)this.Vista.UnidadOperativaID)
            {
                listado.Remove(EDepartamento.RO);
                listado.Remove(EDepartamento.ROC);
                listado.Remove(EDepartamento.RE);
            }
            else
            {
                listado.Remove(EDepartamento.RD);
                listado.Remove(EDepartamento.FSL);
                listado.Remove(EDepartamento.CM);
                listado.Remove(EDepartamento.SD);
                listado.Remove(EDepartamento.ORDENES_SERVICIO);
            }
            Vista.CargarDepartamentos(listado);
        }
        #endregion
    }
}
