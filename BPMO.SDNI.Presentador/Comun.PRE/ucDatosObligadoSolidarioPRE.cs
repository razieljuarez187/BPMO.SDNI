//Satisface al caso de uso CU068 - Catálogo de Clientes
using System;
using System.Collections.Generic;
using BPMO.Basicos.BO;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Comun.VIS;

namespace BPMO.SDNI.Comun.PRE
{
	public class ucDatosObligadoSolidarioPRE
	{
		#region Atributos

		private string nombreClase = "DatosObligadoSolidarioPRE";
		private IucDatosObligadoSolidarioVIS vista;

		#endregion Atributos

		#region Constructores

		public ucDatosObligadoSolidarioPRE(IucDatosObligadoSolidarioVIS vista)
		{
			this.vista = vista;
		}

		#endregion Constructores

		#region Métodos

		public void EliminarRepresentantes()
		{
			vista.RepresentantesLegales = null;
			vista.ActualizarRepresentantesLegales();
		}

		public void ModoCreacion()
		{
			vista.ModoCreacion();
		}

		public void ModoEdicion()
		{
			vista.ModoEdicion();
		}

		public void MostrarDatos(ObligadoSolidarioBO obligado)
		{
			if (obligado == null) throw new Exception("Obligado Solidario no debe estar vacío");
			if (obligado.DireccionPersona == null) throw new Exception("la dirección del obligado solidario no debe estar vacía");
			vista.Nombre = obligado.Nombre;
			vista.Telefono = obligado.Telefono;
			vista.Direccion = obligado.DireccionPersona.Calle;
			vista.ObligadoID = obligado.Id;
			vista.TipoObligadoSolidario = obligado.TipoObligado;
		    vista.RFC = obligado.RFC;
			#region SC0005

			if (obligado.TipoObligado == ETipoObligadoSolidario.Moral)
			{
				vista.RepresentantesInactivos = new List<RepresentanteLegalBO>();
				vista.RepresentantesLegales = new List<RepresentanteLegalBO>(((ObligadoSolidarioMoralBO)obligado).Representantes);
				vista.ActualizarRepresentantesLegales();
				vista.ActaConstitutiva = obligado.ActaConstitutiva;
			    vista.EstablecerAcciones(true);
			}

			#endregion SC0005
		}

		public ObligadoSolidarioBO ObtenerDatos()
		{
			ObligadoSolidarioBO obligado;

			if (vista.TipoObligadoSolidario == ETipoObligadoSolidario.Fisico)
			{
				obligado = new ObligadoSolidarioFisicoBO();
			}
			else
			{
				//SC0005
				obligado = new ObligadoSolidarioMoralBO { Representantes = new List<RepresentanteLegalBO>(vista.RepresentantesLegales) };
				foreach (RepresentanteLegalBO representanteLegalBO in vista.RepresentantesInactivos)
				{
					representanteLegalBO.Activo = false;
					((ObligadoSolidarioMoralBO)obligado).Representantes.Add(representanteLegalBO);
				}

				//SC0007
				obligado.ActaConstitutiva = vista.ActaConstitutiva;
			}

			obligado.DireccionPersona = new DireccionPersonaBO();
			obligado.Nombre = vista.Nombre;
			obligado.DireccionPersona.Calle = vista.Direccion;
			obligado.Telefono = vista.Telefono;
			obligado.Id = vista.ObligadoID;
		    obligado.RFC = vista.RFC;
			return obligado;
		}

		public void PrepararNuevo()
		{
		    ETipoEmpresa empresa = (ETipoEmpresa) vista.UnidadOperativaId;
			vista.Nombre = null;
			vista.Direccion = null;
			vista.Telefono = null;
			vista.ObligadoID = null;
			vista.RepresentantesLegales = null;
			vista.RepresentantesInactivos = null;
			vista.TipoObligadoSolidario = null;
			vista.ActaConstitutiva = null;
            if (empresa == ETipoEmpresa.Generacion ||
                empresa == ETipoEmpresa.Construccion || empresa == ETipoEmpresa.Equinova)
		        vista.RFC = null;
		}

		public void QuitarRepresentanteLegal(RepresentanteLegalBO representante)
		{
			if (representante != null)
			{
				if (representante.Id != null)
					vista.RepresentantesInactivos.Add(representante);

				vista.RepresentantesLegales.Remove(representante);
			}
			else
				throw new Exception("Se requiere un Representante Legal válido para la operación");
		}

		public string ValidarDatos(bool? validarEscritura=true)
		{
			string s = string.Empty;
		    
			if (vista.Nombre == null)
				s += ", Nombre";
			if (vista.Telefono == null)
				s += ", Teléfono";
			if (vista.Direccion == null)
				s += ", Dirección";
			if (vista.TipoObligadoSolidario == null)
				s += ", Tipo de obligado solidario";
			else if (vista.TipoObligadoSolidario == ETipoObligadoSolidario.Moral)
			{
			    if (vista.RepresentantesLegales.Count < 1)
			        s += ", Representantes legales del obligado solidario";
                s += vista.ValidarActaConstitutiva(validarEscritura, true);
			}
            if (this.vista.UnidadOperativaId == (int)ETipoEmpresa.Construccion || this.vista.UnidadOperativaId == (int)ETipoEmpresa.Generacion || this.vista.UnidadOperativaId == (int)ETipoEmpresa.Equinova)
                if (vista.RFC == null || vista.RFC == String.Empty)
                    s += ", RFC";
		        

			return s;
		}

        /// <summary>
        /// Método que establece las acciones del usuario a la vista del control de datos de obligado solidario
        /// </summary>
        /// <param name="listaAcciones"></param>
	    public void EstablecerAcciones(List<CatalogoBaseBO> listaAcciones)
	    {
            this.vista.ListaAcciones = listaAcciones;
            this.vista.EstablecerAcciones(true);
	    }
		#endregion Métodos
	}
}