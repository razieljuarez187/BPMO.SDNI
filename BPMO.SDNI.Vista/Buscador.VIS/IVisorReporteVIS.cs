//Satisface al  Caso de Uso CU016 Imprimir Constancia de Entrega de Bienes
//Satisface al CU019 Imprimir Anexo A
//Satisface al CU019 - Reporte de Flota Activa de RD Registrados
//Satisface el CU022 – Reporte Contratos de Servicio Dedicado Registrad

using System.Collections.Generic;
using BPMO.Primitivos.Enumeradores;
using System;

namespace BPMO.SDNI.Buscador.VIS
{
	public interface IVisorReporteVIS
	{
		Dictionary<string, object> DatosReporte { get; }

		string NombreReporte { get; }

		string MapRuta(string ruta);

	    string ObtenerDirectorioResportes();

		void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string msjDetalle = null);

        void EstablecerFormatosAExportar(params String[] saveFormats);
	}
}