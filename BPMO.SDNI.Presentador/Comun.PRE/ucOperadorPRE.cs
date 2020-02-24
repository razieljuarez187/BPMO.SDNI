// Satisface al CU092 - Catálogo de Operadores
using System;
using System.Collections.Generic;
using BPMO.Basicos.BO;
using BPMO.Facade.SDNI.BR;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Comun.BOF;
using BPMO.SDNI.Comun.VIS;
using BPMO.SDNI.Contratos.BO;
using BPMO.SDNI.Contratos.RD.BO;
using BPMO.SDNI.Contratos.RD.BOF;
using BPMO.SDNI.Contratos.RD.BR;

namespace BPMO.SDNI.Comun.PRE
{
    public class ucOperadorPRE
    {
        #region Atributos
        private IDataContext dctx = null;
        private IucOperadorVIS vista;
        private string nombreClase = "ucOperadorPRE";
        #endregion

        #region Constructores
        public ucOperadorPRE(IucOperadorVIS vista)
        {
            try
            {
                this.vista = vista;
                this.dctx = FacadeBR.ObtenerConexion();
            }
            catch (Exception ex)
            {
                this.vista.MostrarMensaje("Inconsistencia en los parámetros de configuración", ETipoMensajeIU.ERROR, this.nombreClase + ".ucOperadorPRE:" + ex.Message);
            }
        }
        #endregion

        #region Métodos
        public void PrepararNuevo()
        {
            this.LimpiarSesion();
            this.vista.PrepararNuevo();

            this.vista.PermitirSeleccionarCuentaCliente(true);
        }
        public void PrepararEdicion()
        {
            this.vista.PrepararEdicion();

            var s = this.ValidarUsoContrato();

            if(!string.IsNullOrEmpty(s) && !string.IsNullOrWhiteSpace(s))//SC_0051
                this.vista.PermitirSeleccionarCuentaCliente(false);
            else
                this.vista.PermitirSeleccionarCuentaCliente(true);
        }
        public void PrepararVisualizacion()
        {
            this.vista.PrepararVisualizacion();

            this.vista.PermitirSeleccionarCuentaCliente(false);
        }
        
        public string ValidarCampos()
        {
            string s = String.Empty;

            if (this.vista.CuentaClienteID == null)
                s += "Cliente, ";
            if (this.vista.Nombre == null)
                s += "Nombre, ";
            if (this.vista.AñosExperiencia == null)
                s += "Años experiencia, ";
            if (this.vista.FechaNacimiento == null)
                s += "Fecha nacimiento, ";
            if (string.IsNullOrEmpty(this.vista.DireccionCalle) || string.IsNullOrWhiteSpace(this.vista.DireccionCalle))
                s += "Calle, ";
            if (string.IsNullOrEmpty(this.vista.DireccionCiudad) || string.IsNullOrWhiteSpace(this.vista.DireccionCiudad))
                s += "Ciudad, ";
            if (string.IsNullOrEmpty(this.vista.DireccionEstado) || string.IsNullOrWhiteSpace(this.vista.DireccionEstado))
                s += "Estado, ";
            if (string.IsNullOrEmpty(this.vista.DireccionCP) || string.IsNullOrWhiteSpace(this.vista.DireccionCP))
                s += "Código postal, ";
            if (this.vista.LicenciaTipoID == null)
                s += "Tipo licencia, ";
            if (this.vista.LicenciaFechaExpiracion == null)
                s += "Expiración licencia, ";
            if (string.IsNullOrEmpty(this.vista.LicenciaNumero) || string.IsNullOrWhiteSpace(this.vista.LicenciaNumero))
                s += "Número licencia, ";
            if (string.IsNullOrEmpty(this.vista.LicenciaEstado) || string.IsNullOrWhiteSpace(this.vista.LicenciaEstado))
                s += "Estado licencia, ";

            if (s != null && s.Trim().CompareTo("") != 0)
                return "Los siguientes campos no pueden estar vacíos: \n" + s.Substring(0, s.Length - 2);

            if ((s = this.ValidarFechaExpiracionLicencia()) != null)
                return s;

            if ((s = this.ValidarFechaNacimiento()) != null)
                return s;

            if (this.vista.FechaNacimiento != null && this.vista.AñosExperiencia != null)
            {
                int edad = DateTime.Today.Year - this.vista.FechaNacimiento.Value.Year;
                if (new DateTime(DateTime.Today.Year, this.vista.FechaNacimiento.Value.Month, this.vista.FechaNacimiento.Value.Day) > DateTime.Today)
                    edad--; //No ha cumplido años

                if (this.vista.AñosExperiencia >= edad)
                    return "La edad del operador no puede ser menor (ni igual) a los años de experiencia";
            }

            return null;
        }
        public string ValidarFechaExpiracionLicencia()
        {
            if (this.vista.LicenciaFechaExpiracion != null)
            {
                if (this.vista.LicenciaFechaExpiracion.Value.Date <= DateTime.Today)
                    return "La licencia del operador se encuentra vencida; su fecha de expiración es menor a la fecha de hoy.";
            }
            return null;
        }
        public string ValidarFechaNacimiento()
        {
            if (this.vista.FechaNacimiento != null && this.vista.FechaNacimiento.Value.Date >= DateTime.Now.Date)
                return "La fecha de nacimiento no puede ser mayor o igual a la fecha actual.";

            return null;
        }
        /// <summary>
        /// Valida que el operador que se desea editar no se encuentre en un contrato que este actualmente EN CURSO
        /// </summary>
        /// <returns>Mensaje informando el estatus del operador.</returns>
        public string ValidarUsoContrato()//SC_0051
        {
            if(!this.vista.OperadorID.HasValue)
                throw new Exception("Es necesario especificar el identificador del operador que deseas modificar.");

            ContratoRDBR ctrl = new ContratoRDBR();
            
            var contratoBOF = new ContratoRDBOF {Operador = new OperadorBO {OperadorID = this.vista.OperadorID}};
            contratoBOF.EstatusContrato = new List<EEstatusContrato>();
            contratoBOF.EstatusContrato.Add(EEstatusContrato.EnCurso);
            contratoBOF.EstatusContrato.Add(EEstatusContrato.EnPausa);
            contratoBOF.EstatusContrato.Add(EEstatusContrato.Borrador);

            var lstContratos = ctrl.Consultar(dctx,contratoBOF);

            if (lstContratos.Count > 0)
                return  string.Format(@"Actualmente el operador esta siendo usado en el contrato # {0} con estatus ""{1}""", lstContratos[0].NumeroContrato, lstContratos[0].EstatusText);

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