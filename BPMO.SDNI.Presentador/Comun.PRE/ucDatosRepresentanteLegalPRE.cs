//Satisface al caso de uso CU068 - Catálogo de Clientes
using System;
using System.Collections.Generic;
using BPMO.Basicos.BO;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Comun.VIS;

namespace BPMO.SDNI.Comun.PRE
{
	public class ucDatosRepresentanteLegalPRE
	{
		#region Atributos

		private IucDatosRepresentanteLegalVIS vista;

		#endregion Atributos

		#region Constructor

		public ucDatosRepresentanteLegalPRE(IucDatosRepresentanteLegalVIS vista)
		{
			this.vista = vista;
		}

		#endregion Constructor

		#region Métodos

		public void MostrarDatosRepresentanteLegal(RepresentanteLegalBO representante)
		{
			if (representante == null)
			{
				PrepararNuevo();
				return;
			}

			vista.Nombre = representante.Nombre;
			vista.Direccion = representante.DireccionPersona.Calle;
			vista.Telefono = representante.Telefono;
			vista.Depositario = representante.EsDepositario;
			vista.RepresentanteID = representante.Id;
			vista.ActaConstitutiva = representante.ActaConstitutiva;
            vista.RFC = representante.RFC;
		    vista.HabilitarCampos(true);
		}

		public RepresentanteLegalBO ObtenerRepresentanteLegal()
		{
			RepresentanteLegalBO representante = new RepresentanteLegalBO();

			representante.DireccionPersona = new DireccionPersonaBO();
			representante.Nombre = vista.Nombre;
			representante.DireccionPersona.Calle = vista.Direccion;
			representante.Telefono = vista.Telefono;
			representante.EsDepositario = vista.Depositario;
			representante.Id = vista.RepresentanteID;
			representante.Activo = true;
			representante.ActaConstitutiva = vista.ActaConstitutiva;
            representante.UnidadOperativaID = vista.UnidadOperatiaId;
            representante.EsDepositario = vista.Depositario;
            representante.RFC = vista.RFC;
			return representante;
		}

		public void PrepararNuevo()
		{
			vista.Nombre = null;
			vista.Direccion = null;
			vista.Telefono = null;
			vista.Depositario = null;
			vista.RepresentanteID = null;
			vista.ActaConstitutiva = null;
            vista.RFC = null;
		}

        /// <summary>
        /// Validación de campos requeridos
        /// </summary>
        /// <param name="validarfc">Indica si el rfc es validado en la captura</param>
        /// <returns></returns>
		public string ValidarCampos(bool? validarfc=true, bool? validaescitura = false)
		{
		    ETipoEmpresa empresa = (ETipoEmpresa)this.vista.UnidadOperatiaId;
			string s = String.Empty;
			if (vista.Nombre == null)
				s += ", Nombre Completo";
			if (vista.Direccion == null)
				s += ", Dirección";
			if (vista.Telefono == null)
				s += ", Teléfono";
			if (vista.Depositario == null)
				s += ", Depositario";
            if ((empresa == ETipoEmpresa.Construccion || empresa == ETipoEmpresa.Equinova ||
                empresa == ETipoEmpresa.Generacion) && validarfc==true)
		    {
                if (this.vista.RFC == null || this.vista.RFC == String.Empty)
		            s += ", RFC";
		    }

            s += vista.ValidarActaConstitutiva(validaescitura, true);
			return s;
		}

        /// <summary>
        /// Método que establece las acciones del usuario a la vista del control de datos de obligado solidario
        /// </summary>
        /// <param name="lstAcciones"></param>
	    public void EstablecerAcciones(List<CatalogoBaseBO> lstAcciones)
	    {
            this.vista.EstablecerAcciones(lstAcciones, true);
	    }

        public void HabilitarCampos()
        {
           this.vista.HabilitarCampos(true, true);
        }
		#endregion Métodos
	}
}
