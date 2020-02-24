//Satisface al caso de uso CU031 - Imprimir Contrato de mantenimiento
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using BPMO.Basicos.BO;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Comun.BOF;
using BPMO.SDNI.Contratos.BO;
using BPMO.SDNI.Contratos.Mantto.BO;
using DevExpress.XtraReports.UI;

namespace BPMO.SDNI.Contratos.Mantto.CM.RPT
{
	public partial class ContratoCMRPT : DevExpress.XtraReports.UI.XtraReport
	{
		private bool pintarTelefono = false;
		private int PaginaInicioFirmas;
		private float UltimoAlto;
		private int lastPage = 0;
		private int numFirmaActual;
		private int numFirmas;
		private string xmlFirmas = "BPMO.SDNI.Reportes.Contrato.Mantto.CM.Firmas.xml";
		private string xmlUrl;
		private bool esFisico;

		#region Propiedades

		private int PageCount
		{
			get { return PrintingSystem.Document.PageCount; }
		}

		private float UltimaPosicion
		{
			get
			{
				return CantidadControles == 0 ? 0 : this.dtlFirmas.Controls[CantidadControles - 1].LocationF.Y;
			}
		}

		/// <summary>
		/// Devuelve la cantidad de controles que estén en el layout
		/// </summary>
		private int CantidadControles
		{
			get
			{
				return this.dtlFirmas.Controls.Count;
			}
		}

		/// <summary>
		/// Devuelve los márgenes horizontales
		/// </summary>
		private float MargenesHorizontales
		{
			get { return this.Margins.Left + this.Margins.Right + 1; }
		}

		#endregion

		public ContratoCMRPT()
		{
			try
			{
				InitializeComponent();
			}
			catch { throw; }
		}

		public ContratoCMRPT(Dictionary<string, object> ht, string url)
		{
			try
			{
				InitializeComponent();
				if (!ht.ContainsKey("PersonaFisica"))
					throw new Exception("Es necesario especificar el tipo de régimen del cliente al que se le esta generando el contrato");

				esFisico = (bool)Convert.ChangeType(ht["PersonaFisica"], typeof(bool));
				if (string.IsNullOrEmpty(url) || string.IsNullOrWhiteSpace(url))
					throw new Exception("Es necesario especificar la url de la plantilla para el contrato.");
				ht["pathContrato"] = url;
				xmlUrl = url;
				this.ImprimirPersona(ht);
			}
			catch (Exception ex)
			{
				throw new Exception("ContratoMaestroRPT.ContratoMaestroRPT" + ex.Message);
			}
		}

		private void ImprimirPersona(Dictionary<string, object> ht)
		{
			StringBuilder html = null;
			#region Validaciones
			string path = ht["pathContrato"].ToString();
			path += "BPMO.SDNI.Reportes.Contrato.Mantto.CM.xml";
			ContratoManttoBO contrato = (ContratoManttoBO)ht["Contrato"];
			if (contrato.Cliente == null)
				throw new Exception("ContratoMaestroRPT.ImprimirPersonaFisica: Es necesario proporcionar un cliente para el contrato");
			if (contrato.Cliente.Cliente == null)
				throw new Exception("ContratoMaestroRPT.ImprimirPersonaFisica: Es necesario proporcionar un cliente para el contrato");
			ConfiguracionUnidadOperativaBO conf= new ConfiguracionUnidadOperativaBO();
			if (ht.ContainsKey("ConfiguracionUOP"))
			{
				conf = ht["ConfiguracionUOP"] as ConfiguracionUnidadOperativaBO;
			}

			#endregion

			if (File.Exists(path))
			{
				#region Estatus
				if (contrato.Estatus == EEstatusContrato.Borrador)
					this.Watermark.Text = "BORRADOR";
				if (contrato.Estatus == EEstatusContrato.Cancelado)
					this.Watermark.Text = "CANCELADO";
				#endregion

				XDocument xmlDoc = XDocument.Load(path);

				#region Plantilla
				var titulo = from reps in xmlDoc.Descendants("reporte")
							 select new
								 {
									 Titulo = reps.Element("titulo").Value,
									 Leyenda = new
										 {
											 Obligatorio = (from ob in xmlDoc.Descendants("leyenda")
															select ob.Element("obligatorio")).FirstOrDefault(),
											 Obligados = (from los in xmlDoc.Descendants("leyenda").Descendants("opcionales")
														  select los.Element("obligados")).FirstOrDefault()
										 },
									 PlantillaDeclaraciones = from pd in xmlDoc.Descendants("declaraciones").Descendants("plantillas")
															  select new
															  {
																  Padre = pd.Element("padre").Value,
																  Hijo = from pdh in xmlDoc.Descendants("declaraciones").Descendants("plantillas").Descendants("hijo")
																		 select new
																			 {
																				 Cabecera = pdh.Element("cabecera").Value,
																				 Item = pdh.Element("elemento").Value
																			 }
															  },
									 ContenidoDeclaraciones = from cd in xmlDoc.Descendants("declaraciones").Descendants("contenido")
															  select new
															  {
																  Cliente = new
																	  {
																		  DeclaracionesFisicas = from idecf in xmlDoc.Descendants("declaraciones").Descendants("contenido").Descendants("cliente").Descendants("declaracion").Where
																									 (idecf => (string)idecf.Attribute("tipo") == "F")
																								 select new
																									 {
																										 Valor = idecf.Element("valor").Value
																									 },
																		  DeclaracionesMorales = from idecf in xmlDoc.Descendants("declaraciones").Descendants("contenido").Descendants("cliente").Descendants("declaracion").Where
																									 (idecf => (string)idecf.Attribute("tipo") == "M")
																								 select new
																								 {
																									 Valor = idecf.Element("valor").Value,
																									 Opcionales = from idm in xmlDoc.Descendants("declaraciones").Descendants("contenido").Descendants("cliente").Descendants("opcionales").Descendants("declaracion").Where
																									 (idm => (string)idm.Attribute("tipo") == "OM")
																												  select new
																													  {
																														  Valor = idm.Element("valor").Value,
																														  tipo = idm.Element("tipo").Value
																													  }
																								 },
																		  DeclaracionesGlobales = from idecg in xmlDoc.Descendants("declaraciones").Descendants("contenido").Descendants("cliente").Descendants("declaracion").Where
																									 (idecg => (string)idecg.Attribute("tipo") == "G")
																								  select new
																								  {
																									  Valor = idecg.Element("valor").Value
																								  },
																		  DeclaracionesOpcionales = (from idecg in xmlDoc.Descendants("declaraciones").Descendants("contenido").Descendants("cliente").Descendants("declaracion").Where
																									   (idecg => (string)idecg.Attribute("tipo") == "O")
																									 select new
																									 {
																										 Valor = idecg.Element("valor").Value
																									 }).FirstOrDefault()
																	  },
																  Distribuidor = cd.Element("distribuidor").Value,
																  ObligadosDepositarios = new
																  {
																	  ClausulasA = from dosda in xmlDoc.Descendants("declaraciones").Descendants("contenido").Descendants("obligadosDepositarios").Descendants("declaracionOS").Where
																									   (dosda => (string)dosda.Attribute("grupo") == "A")
																				   select new
																					   {
																						   Valor = dosda.Element("valor").Value,
																						   Tipo = dosda.Element("tipo").Value
																					   },
																	  ClausulasB = from dosdb in xmlDoc.Descendants("declaraciones").Descendants("contenido").Descendants("obligadosDepositarios").Descendants("declaracionOS").Where
																									   (dosdb => (string)dosdb.Attribute("grupo") == "B")
																				   select new
																					   {
																						   Valor = dosdb.Element("valor").Value,
																						   Tipo = dosdb.Element("tipo").Value
																					   },
																	  ClausulasC = (from dosdc in xmlDoc.Descendants("declaraciones").Descendants("contenido").Descendants("obligadosDepositarios").Descendants("declaracionOS").Where
																									   (dosdc => (string)dosdc.Attribute("grupo") == "C")
																					select new
																						{
																							Valor = dosdc.Element("valor").Value,
																							Tipo = dosdc.Element("tipo").Value
																						}).FirstOrDefault()
																  },
																  EClausualas = cd.Element("eClausulas").Value
															  },
									 PlantillaClausulas = new
									 {
										 Padre = from pcp in xmlDoc.Descendants("clausulas").Descendants("plantillas").Descendants("padre")
												 select new
													 {
														 Cabecera = pcp.Element("cabecera").Value,
														 Item = pcp.Element("elemento").Value
													 },
										 Hijo = from pch in xmlDoc.Descendants("clausulas").Descendants("plantillas").Descendants("hijo")
												select new
													{
														Cabecera = pch.Element("cabecera").Value,
														Item = pch.Element("elemento").Value
													}
									 },
									 ContenidoClausulas = from pci in xmlDoc.Descendants("clausulas").Descendants("contenido").Descendants("clausula")
														  select new
														  {
															  Cabecera = pci.Element("cabecera").Value,
															  Valor = pci.Element("valor").Value,
															  ID = pci.Attribute("id").Value
															  
														  },
									 ContenidoFinal = from pcfo in xmlDoc.Descendants("clausulas").Descendants("finales").Descendants("parrafo")
													  select new
													  {
														  Valor = pcfo.Element("valor").Value
													  },
									 PlantillaNotificantes = (from pcfo in xmlDoc.Descendants("firmantes")
															  select new
															  {
																  Valor = pcfo.Element("plantilla").Value
															  }).FirstOrDefault()
								 };
				#endregion

				foreach (var rep in titulo)
				{
					#region Título
					html = new StringBuilder(rep.Titulo);
					if (!string.IsNullOrEmpty(contrato.NumeroContrato) && !string.IsNullOrWhiteSpace(contrato.NumeroContrato))
						html.Replace("[NUMERO_CONTRATO]", contrato.NumeroContrato);
					this.lblTitulo.Html = html.ToString();
					html = null;
					#endregion

					#region Declaraciones
					#region plantilla
					string pdhcabecera = string.Empty;
					string pdhhijo = string.Empty;
					foreach (var pde in rep.PlantillaDeclaraciones)
					{
						//Obtiene la plantilla padre
						html = new StringBuilder(pde.Padre);
						//Obtiene la plantilla hijo
						foreach (var pdhi in pde.Hijo)
						{
							pdhcabecera = pdhi.Cabecera;
							pdhhijo = pdhi.Item;
						}
					}
					#endregion

					foreach (var cde in rep.ContenidoDeclaraciones)
					{
						#region Cliente

						StringBuilder declaracionesCli = new StringBuilder(pdhcabecera);
						StringBuilder elementos = new StringBuilder();
						//contendra las declaracionees del cliente ya sea físico o moral
						if (contrato.Cliente.Cliente.Fisica.Value)
						{
							foreach (var cdih in cde.Cliente.DeclaracionesFisicas)
							{
								elementos.Append(pdhhijo);
								elementos.Replace("[TEXTO]", cdih.Valor);
							}
						}
						else
						{
							string actaIgual = string.Empty;
							string actaDiferente = string.Empty;
							foreach (var cdmh in cde.Cliente.DeclaracionesMorales)
							{
								elementos.Append(pdhhijo);
								elementos.Replace("[TEXTO]", cdmh.Valor);
								foreach (var icdm in cdmh.Opcionales)
								{
									if (icdm.tipo.CompareTo("RLAI") == 0)
										actaIgual = icdm.Valor;
									if (icdm.tipo.CompareTo("RLAD") == 0)
										actaDiferente = icdm.Valor;

								}
							}
							if (contrato.Cliente != null)
							{
								if (contrato.Cliente.ActaConstitutiva != null && contrato.RepresentantesLegales != null)
								{
									bool actasIguales = this.EsMismaActaRepresentanteLegal(contrato.RepresentantesLegales.ConvertAll(x => (RepresentanteLegalBO)x), contrato.Cliente.ActaConstitutiva);
									if (actasIguales)
									{
										elementos.Append(pdhhijo);
										elementos.Replace("[TEXTO]", actaIgual);
										elementos.Replace("[ESC_PLU]", string.Empty);
									}
									else
									{
										elementos.Append(pdhhijo);
										elementos.Replace("[TEXTO]", actaDiferente);
										elementos.Replace("[ESC_PLU]", "s");
									}
								}
							}
							//sustituciones declaraciones morales
							#region Acta Constitutiva Cliente
							if (contrato.Cliente.ActaConstitutiva != null)
							{
								if (!string.IsNullOrEmpty(contrato.Cliente.ActaConstitutiva.NumeroEscritura) && !string.IsNullOrWhiteSpace(contrato.Cliente.ActaConstitutiva.NumeroEscritura))
									elementos.Replace("[NUMERO_ESCRITURA_CLIENTE]", contrato.Cliente.ActaConstitutiva.NumeroEscritura);
								if (contrato.Cliente.ActaConstitutiva.FechaEscritura.HasValue)
									elementos.Replace("[FECHA_ESCRITURA_CLIENTE]", contrato.Cliente.ActaConstitutiva.FechaEscritura.Value.ToShortDateString());
								if (!string.IsNullOrEmpty(contrato.Cliente.ActaConstitutiva.NombreNotario) && !string.IsNullOrWhiteSpace(contrato.Cliente.ActaConstitutiva.NombreNotario))
									elementos.Replace("[NOMBRE_NOTARIO_CLIENTE]", contrato.Cliente.ActaConstitutiva.NombreNotario);
								if (!string.IsNullOrEmpty(contrato.Cliente.ActaConstitutiva.NumeroNotaria) && !string.IsNullOrWhiteSpace(contrato.Cliente.ActaConstitutiva.NumeroNotaria))
									elementos.Replace("[NUMERO_NOTARIA_CLIENTE]", contrato.Cliente.ActaConstitutiva.NumeroNotaria);
								if (!string.IsNullOrEmpty(contrato.Cliente.ActaConstitutiva.LocalidadNotaria) && !string.IsNullOrWhiteSpace(contrato.Cliente.ActaConstitutiva.LocalidadNotaria))
									elementos.Replace("[UBICACION_NOTARIA]", contrato.Cliente.ActaConstitutiva.LocalidadNotaria);
							}
							#endregion
							#region Acta Constitutiva Representantes
							if (contrato.RepresentantesLegales != null)
							{
								if (contrato.RepresentantesLegales.Count > 0)
								{
									elementos.Replace("[REPRESENTANTES_LEGALES]", this.ObtenerNombreRepresentantesLegales(contrato.RepresentantesLegales));
									if (contrato.RepresentantesLegales.Count > 1)
									{
										RepresentanteLegalBO firstRepresentante = (RepresentanteLegalBO)contrato.RepresentantesLegales[0];
										if (firstRepresentante.ActaConstitutiva != null)
										{
											if (firstRepresentante.ActaConstitutiva.FechaEscritura.HasValue)
												elementos.Replace("[FECHA_ESCRITURA]", firstRepresentante.ActaConstitutiva.FechaEscritura.Value.ToShortDateString());
											if (!string.IsNullOrEmpty(firstRepresentante.ActaConstitutiva.NombreNotario) && !string.IsNullOrWhiteSpace(firstRepresentante.ActaConstitutiva.NombreNotario))
												elementos.Replace("[NOMBRE_NOTARIO]", firstRepresentante.ActaConstitutiva.NombreNotario);
											if (!string.IsNullOrEmpty(firstRepresentante.ActaConstitutiva.NumeroNotaria) && !string.IsNullOrWhiteSpace(firstRepresentante.ActaConstitutiva.NumeroNotaria))
												elementos.Replace("[NUMERO_NOTARIA]", firstRepresentante.ActaConstitutiva.NumeroNotaria);
											if (!string.IsNullOrEmpty(firstRepresentante.ActaConstitutiva.LocalidadNotaria) && !string.IsNullOrWhiteSpace(firstRepresentante.ActaConstitutiva.LocalidadNotaria))
												elementos.Replace("[LOCALIDAD_NOTARIA]", firstRepresentante.ActaConstitutiva.LocalidadNotaria);
										}
										elementos.Replace("[NUMEROS_ACTAS]", this.ObtenerNumeroActas(contrato.RepresentantesLegales.ConvertAll(x => (RepresentanteLegalBO)x)));
										List<RepresentanteLegalBO> repFaltantes = contrato.RepresentantesLegales.ConvertAll(x => (RepresentanteLegalBO)x);
										elementos.Replace("[ACTAS_CLIENT_ADD]", this.ObtenerInformacionAdicionalActas(repFaltantes, cde.Cliente.DeclaracionesOpcionales.Valor, false));
									}
									else
									{
										RepresentanteLegalBO repUnique = (RepresentanteLegalBO)contrato.RepresentantesLegales[0];
										elementos.Replace("[ACTAS_CLIENT_ADD]", string.Empty);
										if (repUnique.ActaConstitutiva != null)
										{
											if (!string.IsNullOrEmpty(repUnique.ActaConstitutiva.NumeroEscritura) && !string.IsNullOrWhiteSpace(repUnique.ActaConstitutiva.NumeroEscritura))
												elementos.Replace("[NUMEROS_ACTAS]", repUnique.ActaConstitutiva.NumeroEscritura);
											if (repUnique.ActaConstitutiva.FechaEscritura.HasValue)
												elementos.Replace("[FECHA_ESCRITURA]", repUnique.ActaConstitutiva.FechaEscritura.Value.ToShortDateString());
											if (!string.IsNullOrEmpty(repUnique.ActaConstitutiva.NombreNotario) && !string.IsNullOrWhiteSpace(repUnique.ActaConstitutiva.NombreNotario))
												elementos.Replace("[NOMBRE_NOTARIO]", repUnique.ActaConstitutiva.NombreNotario);
											if (!string.IsNullOrEmpty(repUnique.ActaConstitutiva.NumeroNotaria) && !string.IsNullOrWhiteSpace(repUnique.ActaConstitutiva.NumeroNotaria))
												elementos.Replace("[NUMERO_NOTARIA]", repUnique.ActaConstitutiva.NumeroNotaria);
											if (!string.IsNullOrEmpty(repUnique.ActaConstitutiva.LocalidadNotaria) && !string.IsNullOrWhiteSpace(repUnique.ActaConstitutiva.LocalidadNotaria))
												elementos.Replace("[LOCALIDAD_NOTARIA]", repUnique.ActaConstitutiva.LocalidadNotaria);
										}

									}
								}
							}
							#endregion
						}
						//Declaraciones Globales
						foreach (var cdihg in cde.Cliente.DeclaracionesGlobales)
						{
							elementos.Append(pdhhijo);
							elementos.Replace("[TEXTO]", cdihg.Valor);
						}
						declaracionesCli.Replace("[ELEMENTOS]", elementos.ToString());
						html.Replace("[DECLARACION_CLIENTE]", declaracionesCli.ToString());
						#region ActaConstitutiva
						if (contrato.Cliente != null)
						{
							if (!string.IsNullOrEmpty(contrato.Cliente.Nombre) && !string.IsNullOrWhiteSpace(contrato.Cliente.Nombre))
								html.Replace("[NOMBRE_CLIENTE]", contrato.Cliente.Nombre);
							if (!string.IsNullOrEmpty(contrato.Cliente.CURP) && !string.IsNullOrWhiteSpace(contrato.Cliente.CURP))
								html.Replace("[CURP_CLIENTE]", contrato.Cliente.CURP);
							if (contrato.Cliente.FechaRegistroHacienda.HasValue)
								html.Replace("[FECHA_REGISTRO_HDA]", contrato.Cliente.FechaRegistroHacienda.Value.ToShortDateString());
							if (!string.IsNullOrEmpty(contrato.Cliente.GiroEmpresa) && !string.IsNullOrWhiteSpace(contrato.Cliente.GiroEmpresa))
								html.Replace("[GIRO_EMPRESA_CLIENTE]", contrato.Cliente.GiroEmpresa);
							if (!string.IsNullOrEmpty(contrato.Cliente.Cliente.RFC) && !string.IsNullOrWhiteSpace(contrato.Cliente.Cliente.RFC))
								html.Replace("[RFC_CLIENTE]", contrato.Cliente.Cliente.RFC);
							#region Direccion
							if (contrato.Cliente.Direcciones.Count > 0)
							{
								DireccionClienteBO direccion = contrato.Cliente.Direcciones.Find(p => p.Primaria.Value == true);
								if (direccion == null)
									direccion = contrato.Cliente.Direcciones[0];

								if (!string.IsNullOrEmpty(direccion.NumExt) && !string.IsNullOrWhiteSpace(direccion.NumExt))
									html.Replace("[NUMERO_HAB_CLIENTE]", direccion.NumExt);
								if (!string.IsNullOrEmpty(direccion.Direccion) && !string.IsNullOrWhiteSpace(direccion.Direccion))
								{
									html.Replace("[CALLE_CLIENTE]", string.Format("{0}, Colonia {1}", direccion.Calle, direccion.Colonia ?? string.Empty));
								}
								if (!string.IsNullOrEmpty(direccion.CodigoPostal) && !string.IsNullOrWhiteSpace(direccion.CodigoPostal))
									html.Replace("[CODIGO_POSTAL]", direccion.CodigoPostal);
								string ubicacion = string.Empty;
								if (direccion.Ubicacion != null)
								{
									if (direccion.Ubicacion.Ciudad != null)
										if (!string.IsNullOrEmpty(direccion.Ubicacion.Ciudad.Codigo) && !string.IsNullOrWhiteSpace(direccion.Ubicacion.Ciudad.Codigo))
											ubicacion += " , " + direccion.Ubicacion.Ciudad.Codigo;
									if (direccion.Ubicacion.Municipio != null)
										if (!string.IsNullOrEmpty(direccion.Ubicacion.Municipio.Codigo) && !string.IsNullOrWhiteSpace(direccion.Ubicacion.Municipio.Codigo))
											ubicacion += " , " + direccion.Ubicacion.Municipio.Codigo;
									if (direccion.Ubicacion.Estado != null)
										if (!string.IsNullOrEmpty(direccion.Ubicacion.Estado.Codigo) && !string.IsNullOrWhiteSpace(direccion.Ubicacion.Estado.Codigo))
											ubicacion += " , " + direccion.Ubicacion.Estado.Codigo;
									if (direccion.Ubicacion.Pais != null)
										if (!string.IsNullOrEmpty(direccion.Ubicacion.Pais.Codigo) && !string.IsNullOrWhiteSpace(direccion.Ubicacion.Pais.Codigo))
											ubicacion += " , " + direccion.Ubicacion.Pais.Codigo;
								}
								if (!string.IsNullOrEmpty(ubicacion) && !string.IsNullOrWhiteSpace(ubicacion))
									html.Replace("[UBICACION_CLIENTE]", ubicacion.Substring(2));
							}
							#endregion
						}
						#endregion
						#endregion
						#region Distribuidor
						html.Replace("[DECLARACION_DISTRIBUIDOR]", cde.Distribuidor);
						if (!string.IsNullOrEmpty(contrato.Representante) && !string.IsNullOrWhiteSpace(contrato.Representante))
							html.Replace("[REPRESENTANTE_LEGAL_UNIDAD_OPERATIVA]", contrato.Representante);
						#endregion
						#region Obligados y depositarios
						StringBuilder declaraciones = new StringBuilder(pdhcabecera);
						elementos = new StringBuilder();
						if (contrato.Cliente != null)
						{
							if (!contrato.Cliente.EsFisico.HasValue)
								contrato.Cliente.EsFisico = true;
							if (contrato.Cliente.EsFisico.Value)
							{
								#region Cliente físico
								List<ObligadoSolidarioBO> listaObligadosFisicos =
															contrato.ObligadosSolidarios.ConvertAll(x => (ObligadoSolidarioBO)x)
																	.FindAll(y => y.TipoObligado.Value == ETipoObligadoSolidario.Fisico);
								List<ObligadoSolidarioBO> listaObligadosMorales =
									contrato.ObligadosSolidarios.ConvertAll(x => (ObligadoSolidarioBO)x)
											.FindAll(y => y.TipoObligado.Value == ETipoObligadoSolidario.Moral);

								#region obligados físicos
								if (listaObligadosFisicos != null)
								{
									if (listaObligadosFisicos.Count > 0)
									{
										foreach (var osf in listaObligadosFisicos)
										{
											foreach (var clausulasa in cde.ObligadosDepositarios.ClausulasA)
											{
												if (clausulasa.Tipo.CompareTo("OSPF") == 0)
												{
													elementos.Append(pdhhijo);
													elementos.Replace("[TEXTO]", clausulasa.Valor);
												}
											}
											elementos.Replace("[NOMBRE_OS_FISICO]", osf.Nombre);
										}
									}
								}
								#endregion
								#region Obligados Morales
								if (listaObligadosMorales != null)
								{
									if (listaObligadosMorales.Count > 0)
									{
										foreach (var osm in listaObligadosMorales)
										{
											foreach (var clausulasa in cde.ObligadosDepositarios.ClausulasA)
											{
												if (clausulasa.Tipo.CompareTo("OSPM") == 0)
												{
													elementos.Append(pdhhijo);
													elementos.Replace("[TEXTO]", clausulasa.Valor);
												}
											}
											elementos.Replace("[NOMBRE_OS_MORAL]", osm.Nombre);
											#region actaconstitutiva
											if (osm.ActaConstitutiva != null)
											{
												if (!string.IsNullOrEmpty(osm.ActaConstitutiva.NumeroEscritura) && !string.IsNullOrWhiteSpace(osm.ActaConstitutiva.NumeroEscritura))
													elementos.Replace("[NUMERO_ESCRITURA_OSM]", osm.ActaConstitutiva.NumeroEscritura);
												if (osm.ActaConstitutiva.FechaEscritura.HasValue)
													elementos.Replace("[FECHA_ESCRITURA_OSM]", osm.ActaConstitutiva.FechaEscritura.Value.ToShortDateString());
												if (!string.IsNullOrEmpty(osm.ActaConstitutiva.NombreNotario) && !string.IsNullOrWhiteSpace(osm.ActaConstitutiva.NombreNotario))
													elementos.Replace("[NOMBRE_NOTARIO_OSM]", osm.ActaConstitutiva.NombreNotario);
												if (!string.IsNullOrEmpty(osm.ActaConstitutiva.NumeroNotaria) && !string.IsNullOrWhiteSpace(osm.ActaConstitutiva.NumeroNotaria))
													elementos.Replace("[NUMERO_NOTARIA_OSM]", osm.ActaConstitutiva.NumeroNotaria);
											}
											#endregion

											bool mismaActa = this.EsMismaActaObligadoSolidario((ObligadoSolidarioMoralBO)osm, osm.ActaConstitutiva);

											if (mismaActa)
											{
												foreach (var clausulasa in cde.ObligadosDepositarios.ClausulasB)
												{
													if (clausulasa.Tipo.CompareTo("ROSAI") == 0)
													{
														elementos.Append(pdhhijo);
														elementos.Replace("[TEXTO]", clausulasa.Valor);
													}
												}
												elementos.Replace("[REPRESENTANTES_OS]", this.ObtenerNombreRepresentantesObligadoSolidario(((ObligadoSolidarioMoralBO)osm).Representantes));
											}
											else
											{
												foreach (var clausulasa in cde.ObligadosDepositarios.ClausulasB)
												{
													if (clausulasa.Tipo.CompareTo("ROSAD") == 0)
													{
														elementos.Append(pdhhijo);
														elementos.Replace("[TEXTO]", clausulasa.Valor);
													}

													if (((ObligadoSolidarioMoralBO)osm).Representantes.Count > 1)
													{
														var repsOS = ((ObligadoSolidarioMoralBO)osm).Representantes.ConvertAll(x => (RepresentanteLegalBO)x);
														elementos.Replace("[REP_OS_PLU]", "s");
														elementos.Replace("[ACTA_REP_OS_PLU]", "s");
														RepresentanteLegalBO firstRepresentante = repsOS[0];
														if (firstRepresentante.ActaConstitutiva != null)
														{
															if (firstRepresentante.ActaConstitutiva.FechaEscritura.HasValue)
																elementos.Replace("[FECHA_ESCRITURA]", firstRepresentante.ActaConstitutiva.FechaEscritura.Value.ToShortDateString());
															if (!string.IsNullOrEmpty(firstRepresentante.ActaConstitutiva.NombreNotario) && !string.IsNullOrWhiteSpace(firstRepresentante.ActaConstitutiva.NombreNotario))
																elementos.Replace("[NOMBRE_NOTARIO]", firstRepresentante.ActaConstitutiva.NombreNotario);
															if (!string.IsNullOrEmpty(firstRepresentante.ActaConstitutiva.NumeroNotaria) && !string.IsNullOrWhiteSpace(firstRepresentante.ActaConstitutiva.NumeroNotaria))
																elementos.Replace("[NUMERO_NOTARIA]", firstRepresentante.ActaConstitutiva.NumeroNotaria);
															if (!string.IsNullOrEmpty(firstRepresentante.ActaConstitutiva.LocalidadNotaria) && !string.IsNullOrWhiteSpace(firstRepresentante.ActaConstitutiva.LocalidadNotaria))
																elementos.Replace("[LOCALIDAD_NOTARIA]", firstRepresentante.ActaConstitutiva.LocalidadNotaria);
														}
														elementos.Replace("[NUMEROS_ACTAS]", this.ObtenerNumeroActas(repsOS));
														
													}
													else
													{
														elementos.Replace("[REP_OS_PLU]", string.Empty);
														elementos.Replace("[ACTA_REP_OS_PLU]", string.Empty);
														elementos.Replace("[ACTAS_COMOP_REP_OS]", string.Empty);
														#region ActaCosntitutiva
														if (((ObligadoSolidarioMoralBO)osm).Representantes.Count == 1)
														{
															ActaConstitutivaBO acta = ((ObligadoSolidarioMoralBO)osm).Representantes[0].ActaConstitutiva;
															if (acta != null)
															{
																if (!string.IsNullOrEmpty(acta.NumeroEscritura) && !string.IsNullOrWhiteSpace(acta.NumeroEscritura))
																	elementos.Replace("[NUMEROS_ACTAS]", acta.NumeroEscritura);
																if (!string.IsNullOrEmpty(acta.NombreNotario) && !string.IsNullOrWhiteSpace(acta.NombreNotario))
																	elementos.Replace("[NOMBRE_NOTARIO]", acta.NombreNotario);
																if (!string.IsNullOrEmpty(acta.NumeroNotaria) && !string.IsNullOrWhiteSpace(acta.NumeroNotaria))
																	elementos.Replace("[NUMERO_NOTARIA]", acta.NumeroNotaria);
																if (!string.IsNullOrEmpty(acta.LocalidadNotaria) && !string.IsNullOrWhiteSpace(acta.LocalidadNotaria))
																	elementos.Replace("[LOCALIDAD_NOTARIA]", acta.LocalidadNotaria);
																if (acta.FechaEscritura.HasValue)
																	elementos.Replace("[FECHA_ESCRITURA]", acta.FechaEscritura.Value.ToShortDateString());
															}
														}
														#endregion
													}
												}

												elementos.Replace("[REPRESENTANTES_OS]", this.ObtenerNombreRepresentantesObligadoSolidario(((ObligadoSolidarioMoralBO)osm).Representantes));
											}
										}
									}
								}
								#endregion
								elementos.Append(pdhhijo);
								elementos.Replace("[TEXTO]", cde.ObligadosDepositarios.ClausulasC.Valor);
								#endregion
							}
							else
							{
								#region cliente Moral
								if (contrato.SoloRepresentantes.HasValue)
								{
									if (contrato.SoloRepresentantes.Value)
									{
										foreach (var clausulasa in cde.ObligadosDepositarios.ClausulasA)
										{
											if (clausulasa.Tipo == "NAOSNAD")
											{
												elementos.Append(pdhhijo);
												elementos.Replace("[TEXTO]", clausulasa.Valor);
											}
										}
									}
									else
									{
										List<ObligadoSolidarioBO> listaObligadosFisicos = contrato.ObligadosSolidarios.ConvertAll(x => (ObligadoSolidarioBO)x).FindAll(y => y.TipoObligado.Value == ETipoObligadoSolidario.Fisico);
										List<ObligadoSolidarioBO> listaObligadosMorales = contrato.ObligadosSolidarios.ConvertAll(x => (ObligadoSolidarioBO)x).FindAll(y => y.TipoObligado.Value == ETipoObligadoSolidario.Moral);

										#region obligados físicos
										if (listaObligadosFisicos != null)
										{
											if (listaObligadosFisicos.Count > 0)
											{
												foreach (var osf in listaObligadosFisicos)
												{
													foreach (var clausulasa in cde.ObligadosDepositarios.ClausulasA)
													{
														if (clausulasa.Tipo.CompareTo("OSPF") == 0)
														{
															elementos.Append(pdhhijo);
															elementos.Replace("[TEXTO]", clausulasa.Valor);
														}
													}
													elementos.Replace("[NOMBRE_OS_FISICO]", osf.Nombre);
												}
											}
											else if (contrato.RepresentantesLegales != null && listaObligadosMorales.Count <= 0)
											{
												if (contrato.RepresentantesLegales.Count > 1)
												{
													foreach (var osf in contrato.RepresentantesLegales)
													{
														if (((RepresentanteLegalBO)osf).EsDepositario.HasValue)
														{
															if (!((RepresentanteLegalBO)osf).EsDepositario.Value)
															{
																var texto = cde.ObligadosDepositarios.ClausulasA.ToList().FirstOrDefault(x => x.Tipo == "OSPF");
																elementos.Append(pdhhijo);
																elementos.Replace("[TEXTO]", texto.Valor);
																elementos.Replace("[NOMBRE_OS_FISICO]", osf.Nombre);
															}
														}
														foreach (var clausulasa in cde.ObligadosDepositarios.ClausulasA)
														{
															if (clausulasa.Tipo.CompareTo("OSPF") == 0)
															{
																elementos.Append(pdhhijo);
																elementos.Replace("[TEXTO]", clausulasa.Valor);
															}
														}
														elementos.Replace("[NOMBRE_OS_FISICO]", osf.Nombre);
													}
												}
											}
										}
										#endregion
										#region Obligados Morales
										if (listaObligadosMorales != null)
										{
											if (listaObligadosMorales.Count > 0)
											{
												foreach (var osm in listaObligadosMorales)
												{
													List<RepresentanteLegalBO> representantesOS = ((ObligadoSolidarioMoralBO)osm).Representantes;
													#region ClausulaA
													foreach (var clausulasa in cde.ObligadosDepositarios.ClausulasA)
													{
														if (clausulasa.Tipo.CompareTo("OSPM") == 0)
														{
															elementos.Append(pdhhijo);
															elementos.Replace("[TEXTO]", clausulasa.Valor);
														}
													}
													#endregion
													elementos.Replace("[NOMBRE_OS_MORAL]", osm.Nombre);
													#region actaconstitutiva
													if (osm.ActaConstitutiva != null)
													{
														if (!string.IsNullOrEmpty(osm.ActaConstitutiva.NumeroEscritura) && !string.IsNullOrWhiteSpace(osm.ActaConstitutiva.NumeroEscritura))
															elementos.Replace("[NUMERO_ESCRITURA_OSM]", osm.ActaConstitutiva.NumeroEscritura);
														if (osm.ActaConstitutiva.FechaEscritura.HasValue)
															elementos.Replace("[FECHA_ESCRITURA_OSM]", osm.ActaConstitutiva.FechaEscritura.Value.ToShortDateString());
														if (!string.IsNullOrEmpty(osm.ActaConstitutiva.NombreNotario) && !string.IsNullOrWhiteSpace(osm.ActaConstitutiva.NombreNotario))
															elementos.Replace("[NOMBRE_NOTARIO_OSM]", osm.ActaConstitutiva.NombreNotario);
														if (!string.IsNullOrEmpty(osm.ActaConstitutiva.NumeroNotaria) && !string.IsNullOrWhiteSpace(osm.ActaConstitutiva.NumeroNotaria))
															elementos.Replace("[NUMERO_NOTARIA_OSM]", osm.ActaConstitutiva.NumeroNotaria);
													}
													#endregion

													bool mismaActa = this.EsMismaActaObligadoSolidario((ObligadoSolidarioMoralBO)osm, osm.ActaConstitutiva);

													if (mismaActa)
													{
														#region plantilla
														foreach (var clausulasa in cde.ObligadosDepositarios.ClausulasB)
														{
															if (clausulasa.Tipo.CompareTo("ROSAI") == 0)
															{
																elementos.Append(pdhhijo);
																elementos.Replace("[TEXTO]", clausulasa.Valor);
															}
														}
														#endregion
													}
													else
													{
														#region plantilla
														foreach (var clausulasa in cde.ObligadosDepositarios.ClausulasB)
														{
															if (clausulasa.Tipo.CompareTo("ROSAD") == 0)
															{
																elementos.Append(pdhhijo);
																elementos.Replace("[TEXTO]", clausulasa.Valor);
															}
														}
														#endregion
														if (((ObligadoSolidarioMoralBO)osm).Representantes.Count > 1)
														{
															RepresentanteLegalBO firstRepresentante = representantesOS[0];
															if (firstRepresentante.ActaConstitutiva != null)
															{
																elementos.Replace("[REP_OS_PLU]", "s");
																elementos.Replace("[ACTA_REP_OS_PLU]", "s");

																if (firstRepresentante.ActaConstitutiva.FechaEscritura.HasValue)
																	elementos.Replace("[FECHA_ESCRITURA]", firstRepresentante.ActaConstitutiva.FechaEscritura.Value.ToShortDateString());
																if (!string.IsNullOrEmpty(firstRepresentante.ActaConstitutiva.NombreNotario) && !string.IsNullOrWhiteSpace(firstRepresentante.ActaConstitutiva.NombreNotario))
																	elementos.Replace("[NOMBRE_NOTARIO]", firstRepresentante.ActaConstitutiva.NombreNotario);
																if (!string.IsNullOrEmpty(firstRepresentante.ActaConstitutiva.NumeroNotaria) && !string.IsNullOrWhiteSpace(firstRepresentante.ActaConstitutiva.NumeroNotaria))
																	elementos.Replace("[NUMERO_NOTARIA]", firstRepresentante.ActaConstitutiva.NumeroNotaria);
																if (!string.IsNullOrEmpty(firstRepresentante.ActaConstitutiva.LocalidadNotaria) && !string.IsNullOrWhiteSpace(firstRepresentante.ActaConstitutiva.LocalidadNotaria))
																	elementos.Replace("[LOCALIDAD_NOTARIA]", firstRepresentante.ActaConstitutiva.LocalidadNotaria);
															}
															elementos.Replace("[NUMEROS_ACTAS]", this.ObtenerNumeroActas(representantesOS));
															//Si no se ha cambiado se asigan vacio
															elementos.Replace("[REP_OS_PLU]", string.Empty);
															elementos.Replace("[ACTA_REP_OS_PLU]", string.Empty);
														}
														else
														{
															elementos.Replace("[REP_OS_PLU]", string.Empty);
															elementos.Replace("[ACTA_REP_OS_PLU]", string.Empty);
															elementos.Replace("[ACTAS_COMOP_REP_OS]", string.Empty);
															#region ActaCosntitutiva

															if (((ObligadoSolidarioMoralBO)osm).Representantes.Count == 1)
															{
																ActaConstitutivaBO acta = ((ObligadoSolidarioMoralBO)osm).Representantes[0].ActaConstitutiva;
																if (acta != null)
																{
																	if (!string.IsNullOrEmpty(acta.NumeroEscritura) && !string.IsNullOrWhiteSpace(acta.NumeroEscritura))
																		elementos.Replace("[NUMEROS_ACTAS]", acta.NumeroEscritura);
																	if (!string.IsNullOrEmpty(acta.NombreNotario) && !string.IsNullOrWhiteSpace(acta.NombreNotario))
																		elementos.Replace("[NOMBRE_NOTARIO]", acta.NombreNotario);
																	if (!string.IsNullOrEmpty(acta.NumeroNotaria) && !string.IsNullOrWhiteSpace(acta.NumeroNotaria))
																		elementos.Replace("[NUMERO_NOTARIA]", acta.NumeroNotaria);
																	if (!string.IsNullOrEmpty(acta.LocalidadNotaria) && !string.IsNullOrWhiteSpace(acta.LocalidadNotaria))
																		elementos.Replace("[LOCALIDAD_NOTARIA]", acta.LocalidadNotaria);
																	if (acta.FechaEscritura.HasValue)
																		elementos.Replace("[FECHA_ESCRITURA]", acta.FechaEscritura.Value.ToShortDateString());
																	elementos.Replace("[ACTAS_COMOP_REP_OS]", string.Empty);
																}
															}
															else
															{
																List<RepresentanteLegalBO> repsObligado = ((ObligadoSolidarioMoralBO)osm).Representantes;
																RepresentanteLegalBO firstRepresentante = repsObligado[0];
																if (firstRepresentante.ActaConstitutiva != null)
																{
																	if (firstRepresentante.ActaConstitutiva.FechaEscritura.HasValue)
																		elementos.Replace("[FECHA_ESCRITURA]", firstRepresentante.ActaConstitutiva.FechaEscritura.Value.ToShortDateString());
																	if (!string.IsNullOrEmpty(firstRepresentante.ActaConstitutiva.NombreNotario) && !string.IsNullOrWhiteSpace(firstRepresentante.ActaConstitutiva.NombreNotario))
																		elementos.Replace("[NOMBRE_NOTARIO]", firstRepresentante.ActaConstitutiva.NombreNotario);
																	if (!string.IsNullOrEmpty(firstRepresentante.ActaConstitutiva.NumeroNotaria) && !string.IsNullOrWhiteSpace(firstRepresentante.ActaConstitutiva.NumeroNotaria))
																		elementos.Replace("[NUMERO_NOTARIA]", firstRepresentante.ActaConstitutiva.NumeroNotaria);
																	if (!string.IsNullOrEmpty(firstRepresentante.ActaConstitutiva.LocalidadNotaria) && !string.IsNullOrWhiteSpace(firstRepresentante.ActaConstitutiva.LocalidadNotaria))
																		elementos.Replace("[LOCALIDAD_NOTARIA]", firstRepresentante.ActaConstitutiva.LocalidadNotaria);
																}
																elementos.Replace("[NUMEROS_ACTAS]", this.ObtenerNumeroActas(repsObligado));
																elementos.Replace("ACTAS_COMOP_REP_OS]", this.ObtenerInformacionAdicionalActas(repsObligado, cde.Cliente.DeclaracionesOpcionales.Valor, false));
															}
															#endregion
														}
													}
													elementos.Replace("[REPRESENTANTES_OS]", this.ObtenerNombreRepresentantesObligadoSolidario(representantesOS));
												}
											}
										}
										#endregion

										elementos.Append(pdhhijo);
										elementos.Replace("[TEXTO]", cde.ObligadosDepositarios.ClausulasC.Valor);

									}
								}
								#endregion
							}
							#region plurales_y_articulos
							#region ObligadosSolidarios
							if (contrato.ObligadosSolidarios != null)
							{
								if (contrato.ObligadosSolidarios.Count > 0)
								{
									html.Replace("[OBLIG_PLURAL]", contrato.ObligadosSolidarios.Count > 1 ? "s" : string.Empty);
									html.Replace("[OBLIG_N]", contrato.ObligadosSolidarios.Count > 1 ? "n" : string.Empty);
									html.Replace("[ARTICULO_OBLIG]", contrato.ObligadosSolidarios.Count > 1 ? "los" : "el");
									html.Replace("[OBLIGADOS_SOLIDARIOS]", this.ObtenerNombresObligadosSolidarios(contrato.ObligadosSolidarios));
								}
								else if (contrato.RepresentantesLegales != null)
								{
									html.Replace("[OBLIG_PLURAL]", contrato.RepresentantesLegales.Count > 1 ? "s" : string.Empty);
									html.Replace("[OBLIG_N]", contrato.RepresentantesLegales.Count > 1 ? "n" : string.Empty);
									html.Replace("[ARTICULO_OBLIG]", contrato.RepresentantesLegales.Count > 1 ? "los" : "el");
									html.Replace("[OBLIGADOS_SOLIDARIOS]", this.ObtenerNombreRepresentantesLegales(contrato.RepresentantesLegales));
								}
								else
								{
									html.Replace("[OBLIG_PLURAL]", string.Empty);
									html.Replace("[OBLIG_N]", string.Empty);
									html.Replace("[ARTICULO_OBLIG]", "el");
								}
							}
							else if (contrato.RepresentantesLegales != null)
							{
								html.Replace("[OBLIG_PLURAL]", contrato.RepresentantesLegales.Count > 1 ? "s" : string.Empty);
								html.Replace("[OBLIG_N]", contrato.RepresentantesLegales.Count > 1 ? "n" : string.Empty);
								html.Replace("[ARTICULO_OBLIG]", contrato.RepresentantesLegales.Count > 1 ? "los" : "el");
								html.Replace("[OBLIGADOS_SOLIDARIOS]", this.ObtenerNombreRepresentantesLegales(contrato.RepresentantesLegales));
							}
							else
							{
								html.Replace("[OBLIG_PLURAL]", string.Empty);
								html.Replace("[OBLIG_N]", string.Empty);
								html.Replace("[ARTICULO_OBLIG]", "el");
							}
							#endregion

							#region Representantes Legales

							if (contrato.Cliente.EsFisico.HasValue)
							{
								if (contrato.Cliente.EsFisico.Value)
								{
									html.Replace("[DEPO_PLURAL]", string.Empty);
									html.Replace("[ARTICULO_DEP]", "el");
								}
							}
							List<RepresentanteLegalBO> declaRepLegales = contrato.RepresentantesLegales.ConvertAll(x => (RepresentanteLegalBO)x);

							if (declaRepLegales != null)
							{
								if (declaRepLegales.Count > 0)
								{
									if (contrato.Cliente.EsFisico.HasValue)
									{
										if (contrato.Cliente.EsFisico.Value)
											html.Replace("[DEPOSITARIOS]", contrato.Cliente.Nombre);
									}

									#region Depositarios
									var depositarios = declaRepLegales.FindAll(x => x.EsDepositario.Value == true);
									if (depositarios != null)
									{
										html.Replace("[DEPO_PLURAL]", depositarios.Count > 1 ? "s" : string.Empty);
										html.Replace("[ARTICULO_DEP]", depositarios.Count > 1 ? "los" : "[ARTICULO_DEP]");
									}
									#endregion
								}
							}
							#endregion

							if (contrato.Cliente.EsFisico.HasValue)
							{
								if (contrato.Cliente.EsFisico.Value)
								{
									html.Replace("[ARTICULO_DEP]", "el");
									html.Replace("[DEPO_PLURAL]", string.Empty);
								}
								else
								{
									if (contrato.RepresentantesLegales != null)
									{
										if (contrato.RepresentantesLegales.Count > 0)
										{
											var cDeps = contrato.RepresentantesLegales.ConvertAll(x => (RepresentanteLegalBO)x);
											var cDep = cDeps.Count(x => x.EsDepositario.Value);
											if (cDep > 1)
											{
												html.Replace("[ARTICULO_DEP]", "los");
												html.Replace("[DEPO_PLURAL]", "s");
											}
											else
											{
												html.Replace("[ARTICULO_DEP]", "el");
												html.Replace("[DEPO_PLURAL]", string.Empty);
											}
										}
									}
								}
							}
							else
							{
								html.Replace("[ARTICULO_DEP]", "el");
								html.Replace("[DEPO_PLURAL]", string.Empty);
							}
							#endregion
						}
						declaraciones.Replace("[ELEMENTOS]", elementos.ToString());
						html.Replace("[DECLARACION_OBLIG_DEP]", declaraciones.ToString());
						#endregion

						#region EClausulas
						html.Replace("[ECLAUSULAS]", cde.EClausualas);
						#endregion
					}
					if (contrato.ObligadosSolidarios != null)
					{
						if (contrato.ObligadosSolidarios.Count > 0)
							html.Replace("[OBLIG_PLURAL]", contrato.ObligadosSolidarios.Count > 1 ? "s" : string.Empty);
						else if (contrato.RepresentantesLegales != null)
							html.Replace("[OBLIG_PLURAL]", contrato.RepresentantesLegales.Count > 1 ? "s" : string.Empty);
						else html.Replace("[OBLIG_PLURAL]", string.Empty);
					}
					else html.Replace("[OBLIG_PLURAL]", string.Empty);


					html.Replace("[NOMBRE_CORTO_UNIDAD_OPERATIVA]", conf.Alias);
					html.Replace("[TELEFONO_EMERGENCIA]", conf.TelefonoAuxilioCarretero);
					html.Replace("[TELEFONO_EMERGENCIA_2]", conf.TelefonoAuxilioCarretero2);
					this.lblDeclaraciones.Html = html.ToString();
					html = null;
					#endregion

					#region Clausulas
					#region plantilla
					string ipcp = string.Empty;
					foreach (var pcp in rep.PlantillaClausulas.Padre)
					{
						html = new StringBuilder(pcp.Cabecera);
						ipcp = pcp.Item;
					}
					#endregion
					#region Contenido
					StringBuilder clausulas = new StringBuilder();
					foreach (var cc in rep.ContenidoClausulas)
					{
						StringBuilder item = new StringBuilder(ipcp);
						item.Replace("[TEXTO_CLAUSULA]", cc.Cabecera);
						item.Replace("[VALOR_CLAUSULA]", cc.Valor);
						clausulas.Append(item);
					}
					html.Replace("[CLAUSULAS]", clausulas.ToString());
					#endregion
					#region Sustituciones
					if (contrato.ObligadosSolidarios != null)
					{
						if (contrato.ObligadosSolidarios.Count > 0)
						{
							html.Replace("[OBLIG_PLURAL]", contrato.ObligadosSolidarios.Count > 1 ? "s" : string.Empty);
							html.Replace("[PREP_OBLIGADO]", contrato.ObligadosSolidarios.Count > 1 ? "n" : string.Empty);
							html.Replace("[CARGO_OBLIGADO]",
										 contrato.ObligadosSolidarios.Count > 1 ? "de los primeros" : "del primero");
						}
						else if (contrato.RepresentantesLegales != null)
						{
							html.Replace("[OBLIG_PLURAL]", contrato.RepresentantesLegales.Count > 1 ? "s" : string.Empty);
							html.Replace("[PREP_OBLIGADO]", contrato.RepresentantesLegales.Count > 1 ? "n" : string.Empty);
							html.Replace("[CARGO_OBLIGADO]", contrato.RepresentantesLegales.Count > 1 ? "de los primeros" : "del primero");
						}
						else
						{
							html.Replace("[OBLIG_PLURAL]", string.Empty);
							html.Replace("[PREP_OBLIGADO]", string.Empty);
						}
					}
					else if (contrato.RepresentantesLegales != null)
					{
						html.Replace("[OBLIG_PLURAL]", contrato.RepresentantesLegales.Count > 1 ? "s" : string.Empty);
						html.Replace("[PREP_OBLIGADO]", contrato.RepresentantesLegales.Count > 1 ? "n" : string.Empty);
						html.Replace("[CARGO_OBLIGADO]", contrato.RepresentantesLegales.Count > 1 ? "de los primeros" : "del primero");
					}
					else
					{
						html.Replace("[OBLIG_PLURAL]", string.Empty);
						html.Replace("[PREP_OBLIGADO]", string.Empty);
					}

					#endregion

					html.Replace("[NOMBRE_CORTO_UNIDAD_OPERATIVA]", conf.Alias);
					html.Replace("[TELEFONO_EMERGENCIA]", conf.TelefonoAuxilioCarretero);
					html.Replace("[TELEFONO_EMERGENCIA_2]", conf.TelefonoAuxilioCarretero2);
					this.lblClausulas.Html = html.ToString();
					#region firmantes
					#region Distribuidor
					if (contrato.Sucursal != null)
					{
						if (contrato.Sucursal.UnidadOperativa != null)
						{
							if (contrato.Sucursal.UnidadOperativa.Empresa != null)
							{
								if (!string.IsNullOrEmpty(contrato.Sucursal.UnidadOperativa.Empresa.Nombre) && !string.IsNullOrWhiteSpace(contrato.Sucursal.UnidadOperativa.Empresa.Nombre))
									this.lblClaUnidadOperativa.Text = contrato.Sucursal.UnidadOperativa.Empresa.Nombre;
							}
							if (contrato.Sucursal.DireccionesSucursal != null)
							{
								var direcUO = contrato.Sucursal.DireccionesSucursal.Find(x => x.Primaria != null && x.Primaria.Value == true);
								if (direcUO != null)
								{
									if (!string.IsNullOrEmpty(direcUO.Telefono) &&
										!string.IsNullOrWhiteSpace(direcUO.Telefono))
									{
										this.lblTelefonoUO.Text = direcUO.Telefono;
										if (!string.IsNullOrEmpty(direcUO.Calle) &&
											!string.IsNullOrWhiteSpace(direcUO.Calle))
											this.lblDireccionUO.Text = direcUO.Calle;
									}
									else
									{
										if (!string.IsNullOrEmpty(direcUO.Calle) &&
											!string.IsNullOrWhiteSpace(direcUO.Calle))
											this.lblTelefonoUO.Text = direcUO.Calle;
										this.lblDireccionUO.Text = string.Empty;
									}
								}
							}
						}
					}
					if (!string.IsNullOrEmpty(contrato.Representante) && !string.IsNullOrWhiteSpace(contrato.Representante))
						this.lblClauRepLegUO.Text = contrato.Representante;
					#endregion
					#region Cliente
					if (contrato.Cliente != null)
					{
						if (contrato.Cliente.EsFisico.HasValue)
						{
							this.lblNombreCliente.Text = contrato.Cliente.Nombre;
							var direcCli = contrato.Cliente.Direcciones.Find(x => x.Primaria.Value);
							if (contrato.Cliente.EsFisico.Value)
							{
								if (direcCli != null)
								{
									if (!string.IsNullOrEmpty(direcCli.Telefono) &&
										!string.IsNullOrWhiteSpace(direcCli.Telefono))
									{
										this.lbTelRepCliente.Text = "Telefono " + direcCli.Telefono;
										pintarTelefono = true;
									}
									else
									{
										this.lbTelRepCliente.Visible = false;
									}
								}

								this.lblTeleDirecCliente.Text = contrato.Cliente.Direccion;
								this.lblDireccionCliente.Text = string.Empty;
							}
							else
							{
								this.lbTelRepCliente.Text = this.ObtenerNombreRepresentantesLegales(contrato.RepresentantesLegales);
								if (direcCli != null)
								{
									if (!string.IsNullOrEmpty(direcCli.Telefono) && !string.IsNullOrWhiteSpace(direcCli.Telefono))
									{
										this.lblTeleDirecCliente.Text = "Telefono " + direcCli.Telefono;
										if (!string.IsNullOrEmpty(contrato.Cliente.Direccion) && !string.IsNullOrWhiteSpace(contrato.Cliente.Direccion))
											this.lblDireccionCliente.Text = contrato.Cliente.Direccion;
									}
									else
									{
										if (!string.IsNullOrEmpty(contrato.Cliente.Direccion) && !string.IsNullOrWhiteSpace(contrato.Cliente.Direccion))
											this.lblTeleDirecCliente.Text = contrato.Cliente.Direccion;
										this.lblDireccionCliente.Text = string.Empty;
									}
								}


							}
						}
					}
					#endregion
					#region Obligados Solidarios

					this.lblNotificantes.Html = this.ObtenerNotificantes(rep.PlantillaNotificantes.Valor, contrato);

					#endregion
					#endregion

					#region Clausulas finales
					html = null;
					html = new StringBuilder();
					foreach (var ifin in rep.ContenidoFinal)
					{
						html.Append(ifin.Valor);
					}

					html.Replace("[NOMBRE_CORTO_UNIDAD_OPERATIVA]", conf.Alias);
					html.Replace("[TELEFONO_EMERGENCIA]", conf.TelefonoAuxilioCarretero);
					html.Replace("[TELEFONO_EMERGENCIA_2]", conf.TelefonoAuxilioCarretero2);
					this.lblFinNotificacion.Html = html.ToString();
					html = null;
					#endregion
					#endregion

					#region Leyenda
					html = new StringBuilder(rep.Leyenda.Obligatorio.Value);

					#region plantilla
					if (contrato.Cliente != null)
					{
						if (contrato.Cliente.EsFisico.HasValue)
						{
							if (contrato.Cliente.EsFisico.Value)
							{
								html.Replace("[LEYENDA_OBLIGADOS]", rep.Leyenda.Obligados.Value);
								
							}
							else
							{
								if (contrato.SoloRepresentantes.HasValue)
								{
									if (!contrato.SoloRepresentantes.Value)
									{
										html.Replace("[LEYENDA_OBLIGADOS]", rep.Leyenda.Obligados.Value);
										
									}
									else
									{
										html.Replace("[LEYENDA_OBLIGADOS]", string.Empty);
										
									}
								}
								else
								{
									html.Replace("[LEYENDA_OBLIGADOS]", rep.Leyenda.Obligados.Value);
									
								}
							}
						}
					}
					#endregion

					if (contrato.FechaContrato.HasValue)
						html.Replace("[FECHA_CONTRATO]", contrato.FechaContrato.Value.ToShortDateString());
					if (contrato.Cliente != null)
						if (!string.IsNullOrEmpty(contrato.Cliente.Nombre) && !string.IsNullOrWhiteSpace(contrato.Cliente.Nombre))
							html.Replace("[NOMBRE_CLIENTE]", contrato.Cliente.Nombre);
					if (contrato.Sucursal != null)
						if (contrato.Sucursal.UnidadOperativa != null)
							if (contrato.Sucursal.UnidadOperativa.Empresa != null)
								if (!string.IsNullOrEmpty(contrato.Sucursal.UnidadOperativa.Empresa.Nombre) && !string.IsNullOrWhiteSpace(contrato.Sucursal.UnidadOperativa.Empresa.Nombre))
									html.Replace("[UNIDAD_OPERATIVA]", contrato.Sucursal.UnidadOperativa.Empresa.Nombre);
					#region ObligadosSolidarios
					if (contrato.ObligadosSolidarios != null)
					{
						if (contrato.ObligadosSolidarios.Count > 0)
						{
							html.Replace("[OBLIG_PLURAL]", contrato.ObligadosSolidarios.Count > 1 ? "s" : string.Empty);
							html.Replace("[OBLIGADOS_SOLIDARIOS]", this.ObtenerNombresObligadosSolidarios(contrato.ObligadosSolidarios));
						}
						else if (contrato.RepresentantesLegales != null)
						{
							html.Replace("[OBLIG_PLURAL]", contrato.RepresentantesLegales.Count > 1 ? "s" : string.Empty);
							html.Replace("[OBLIGADOS_SOLIDARIOS]", this.ObtenerNombreRepresentantesLegales(contrato.RepresentantesLegales));
						}
						else { html.Replace("[OBLIG_PLURAL]", string.Empty); }
					}
					else if (contrato.RepresentantesLegales != null)
					{
						html.Replace("[OBLIG_PLURAL]", contrato.RepresentantesLegales.Count > 1 ? "s" : string.Empty);
						html.Replace("[OBLIGADOS_SOLIDARIOS]", this.ObtenerNombreRepresentantesLegales(contrato.RepresentantesLegales));
					}
					else html.Replace("[OBLIG_PLURAL]", string.Empty);

					#endregion


						html.Replace("[NOMBRE_CORTO_UNIDAD_OPERATIVA]", conf.Alias);
						html.Replace("[TELEFONO_EMERGENCIA]", conf.TelefonoAuxilioCarretero);
						html.Replace("[TELEFONO_EMERGENCIA_2]", conf.TelefonoAuxilioCarretero2);
					
					this.lblLeyenda.Html = html.ToString();
					html = null;
					#endregion

					#region Firmas
					Dictionary<string, List<Dictionary<string, string>>> firmas =
					ht["firmas"] as Dictionary<string, List<Dictionary<string, string>>>;
					List<Dictionary<string, string>> Representantes = firmas["Representantes"];
					List<Dictionary<string, string>> Obligados = firmas["ObligadosSolidarios"];
					
					List<Dictionary<string, string>> Distribuidor = firmas["Distribuidor"];
					int cDIst = Distribuidor != null ? Distribuidor.Count : 0;
					numFirmas = Representantes.Count + Obligados.Count + cDIst;
					if (Distribuidor != null)
						ProcesaFirmas(Distribuidor, contrato);
					ProcesaFirmas(Representantes, contrato);
					ProcesaFirmas(Obligados, contrato);
					
					#endregion
				}

				#region Estatus
				if (contrato.Estatus == EEstatusContrato.Borrador)
					this.Watermark.Text = "BORRADOR";
				if (contrato.Estatus == EEstatusContrato.Cancelado)
					this.Watermark.Text = "CANCELADO";
				#endregion
			}
			else
			{
				this.lblTitulo.Html = @"<p style=""text-align:center"">La plantilla no pudo ser cargada</p>";
				throw new Exception("ContratoMaestroRPTImprimirPersonaFisica: La plantilla no pudo ser cargada");
			}
		}

		private string ObtenerInformacionAdicionalActas(List<RepresentanteLegalBO> repFaltantes, string leyenda, bool usarPrimero)
		{
			StringBuilder actas = new StringBuilder();
			int i = 0;
			ActaConstitutivaBO primeraActa = null;

			if (repFaltantes[0].ActaConstitutiva != null)
				primeraActa = repFaltantes[0].ActaConstitutiva;

			foreach (var rep in repFaltantes)
			{
				if (i == 0 && !usarPrimero)
				{
					i++;
					continue;
				}

				if (!this.MismasActasConstitutivas(primeraActa, rep.ActaConstitutiva))
				{
					actas.Append(leyenda);
					if (rep.ActaConstitutiva.FechaEscritura.HasValue)
						actas.Replace("[FECHA_ESCRITURA]", rep.ActaConstitutiva.FechaEscritura.Value.ToShortDateString());
					if (!string.IsNullOrEmpty(rep.ActaConstitutiva.NombreNotario) &&
						!string.IsNullOrWhiteSpace(rep.ActaConstitutiva.NombreNotario))
						actas.Replace("[NOMBRE_NOTARIO]", rep.ActaConstitutiva.NombreNotario);
					if (!string.IsNullOrEmpty(rep.ActaConstitutiva.NumeroNotaria) &&
						!string.IsNullOrWhiteSpace(rep.ActaConstitutiva.NumeroNotaria))
						actas.Replace("[NUMERO_NOTARIA]", rep.ActaConstitutiva.NumeroNotaria);
					if (!string.IsNullOrEmpty(rep.ActaConstitutiva.LocalidadNotaria) &&
						!string.IsNullOrWhiteSpace(rep.ActaConstitutiva.LocalidadNotaria))
						actas.Replace("[LOCALIDAD_NOTARIA]", rep.ActaConstitutiva.LocalidadNotaria);
				}
				i++;
			}
			return actas.ToString();
		}

		private bool MismasActasConstitutivas(ActaConstitutivaBO primeraActa, ActaConstitutivaBO actaConstitutivaBO)
		{
			if (primeraActa.FechaEscritura.Value.ToString().CompareTo(actaConstitutivaBO.FechaEscritura.Value.ToString()) == 0
						&& primeraActa.LocalidadNotaria.CompareTo(actaConstitutivaBO.LocalidadNotaria) == 0
						&& primeraActa.NumeroEscritura.CompareTo(actaConstitutivaBO.NumeroEscritura) == 0
						&& primeraActa.NumeroNotaria.CompareTo(actaConstitutivaBO.NumeroNotaria) == 0)
				return true;
			else
				return false;
		}



		private string ObtenerNumeroActas(List<RepresentanteLegalBO> list)
		{
			StringBuilder actas = new StringBuilder();
			var query = from counts in list
						group counts by new
						{
							counts.ActaConstitutiva.NumeroEscritura,
							counts.ActaConstitutiva.NumeroNotaria,
							counts.ActaConstitutiva.FechaEscritura,
							counts.ActaConstitutiva.LocalidadNotaria
						}
							into item
							select new
							{
								numeroEscrituras = item.Key.NumeroEscritura,
								numeroNotaria = item.Key.NumeroNotaria,
								fechaInscripcion = item.Key.FechaEscritura,
								localidadInscripcion = item.Key.LocalidadNotaria,
								notario = item.First().ActaConstitutiva.NombreNotario,
								count = item.Count()
							};

			foreach (var rep in query)
			{
				actas.Append(", " + rep.numeroEscrituras);
			}
			return actas.ToString().Substring(2);
		}

		private string ObtenerNotificantes(string p, ContratoManttoBO contrato)
		{
			StringBuilder notificantes = new StringBuilder();
			if (contrato.ObligadosSolidarios != null)
			{
				List<PersonaBO> personasOS = new List<PersonaBO>();
				var ObligadosTemp = from pe in contrato.ObligadosSolidarios
									orderby ((ObligadoSolidarioBO)pe).TipoObligado descending
									select pe;

				foreach (PersonaBO per in ObligadosTemp)
				{
					ObligadoSolidarioBO ose = (ObligadoSolidarioBO)per;
					if (ose.TipoObligado == ETipoObligadoSolidario.Moral)
					{
						foreach (RepresentanteLegalBO repo in ((ObligadoSolidarioMoralBO)ose).Representantes)
						{
							RepresentanteLegalBOF rep = new RepresentanteLegalBOF(repo);
							rep.ObligadoSolidario = ose.Nombre;
							rep.EsDepositario = false;
							personasOS.Add(rep);
						}
					}
					else
					{
						personasOS.Add(per);
					}
				}

				





				if (personasOS.Count > 0)
				{
					if (personasOS.Count == 1)
					{
						PersonaBO personaIzquierda = personasOS[0];
						StringBuilder lineaNotificante = new StringBuilder(p);
						if (personaIzquierda == null) //Si persona Izquierda es null
						{
							lineaNotificante.Replace("[TITLE_LEFTH]", string.Empty);
							lineaNotificante.Replace("[NAME_LEFTH]", string.Empty);
							lineaNotificante.Replace("[NAME_REP_LEFTH]", string.Empty);
							lineaNotificante.Replace("[DIREC_REP_LEFTH]", string.Empty);
						}
						else
						{
							if (personaIzquierda.TipoPersona.Value == ETipoPersona.ObligadoSolidario)
							{

								lineaNotificante.Replace("[TITLE_LEFTH]", "<b>OBLIGADO SOLIDARIO:</b>");
								lineaNotificante.Replace("[NAME_LEFTH]", personaIzquierda.Nombre);
								lineaNotificante.Replace("[NAME_REP_LEFTH]", personaIzquierda.Direccion);
								lineaNotificante.Replace("[DIREC_REP_LEFTH]", string.Empty);


							}
							else if (personaIzquierda.TipoPersona == ETipoPersona.RepresentanteLegal)
							{
								RepresentanteLegalBO repIzq = personaIzquierda as RepresentanteLegalBO;

									lineaNotificante.Replace("[TITLE_LEFTH]", "<b>OBLIGADO SOLIDARIO:</b>");
									lineaNotificante.Replace("[NAME_LEFTH]", ((RepresentanteLegalBOF)repIzq).ObligadoSolidario);
									lineaNotificante.Replace("[NAME_REP_LEFTH]", "Rep. Legal: " + repIzq.Nombre);
									lineaNotificante.Replace("[DIREC_REP_LEFTH]", repIzq.Direccion);
								
							}
						}

						lineaNotificante.Replace("[TITLE_RIGTH]", string.Empty);
						lineaNotificante.Replace("[NAME_RIGTH]", string.Empty);
						lineaNotificante.Replace("[NAME_REP_RIGTH]", string.Empty);
						lineaNotificante.Replace("[DIREC_REP_RIGTH]", string.Empty);

						notificantes.Append(lineaNotificante);
					}
					else
					{
						int espar = personasOS.Count % 2;
						int limite = personasOS.Count - 2;
						for (int i = 0; i <= limite; i += 2)
						{
							PersonaBO personaIzquierda = personasOS[i], personaDerecha = personasOS[i + 1];
							StringBuilder lineaNotificante = new StringBuilder(p);
							if (personaIzquierda == null) //Si persona Izquierda es null
							{
								lineaNotificante.Replace("[TITLE_LEFTH]", string.Empty);
								lineaNotificante.Replace("[NAME_LEFTH]", string.Empty);
								lineaNotificante.Replace("[NAME_REP_LEFTH]", string.Empty);
								lineaNotificante.Replace("[DIREC_REP_LEFTH]", string.Empty);
							}
							else
							{
								if (personaIzquierda.TipoPersona.Value == ETipoPersona.ObligadoSolidario)
								{

									lineaNotificante.Replace("[TITLE_LEFTH]", "<b>OBLIGADO SOLIDARIO:</b>");
									lineaNotificante.Replace("[NAME_LEFTH]", personaIzquierda.Nombre);
									lineaNotificante.Replace("[NAME_REP_LEFTH]", personaIzquierda.Direccion);
									lineaNotificante.Replace("[DIREC_REP_LEFTH]", string.Empty);


								}
								else if (personaIzquierda.TipoPersona == ETipoPersona.RepresentanteLegal)
								{
									RepresentanteLegalBO repIzq = personaIzquierda as RepresentanteLegalBO;

										lineaNotificante.Replace("[TITLE_LEFTH]", "<b>OBLIGADO SOLIDARIO:</b>");
										lineaNotificante.Replace("[NAME_LEFTH]", ((RepresentanteLegalBOF)repIzq).ObligadoSolidario);
										lineaNotificante.Replace("[NAME_REP_LEFTH]", "Rep. Legal: " + repIzq.Nombre);
										lineaNotificante.Replace("[DIREC_REP_LEFTH]", repIzq.Direccion);
									
								}
							}


							if (personaDerecha == null) //Si persona Derecha es null
							{
								lineaNotificante.Replace("[TITLE_RIGTH]", string.Empty);
								lineaNotificante.Replace("[NAME_RIGTH]", string.Empty);
								lineaNotificante.Replace("[NAME_REP_RIGTH]", string.Empty);
								lineaNotificante.Replace("[DIREC_REP_RIGTH]", string.Empty);
							}
							else
							{
								if (personaDerecha.TipoPersona.Value == ETipoPersona.ObligadoSolidario)
								{

									lineaNotificante.Replace("[TITLE_RIGTH]", "<b>OBLIGADO SOLIDARIO:</b>");
									lineaNotificante.Replace("[NAME_RIGTH]", personaDerecha.Nombre);
									lineaNotificante.Replace("[NAME_REP_RIGTH]", personaDerecha.Direccion);
									lineaNotificante.Replace("[DIREC_REP_RIGTH]", string.Empty);


								}
								else if (personaDerecha.TipoPersona == ETipoPersona.RepresentanteLegal)
								{
									RepresentanteLegalBO repDer = personaDerecha as RepresentanteLegalBO;

										lineaNotificante.Replace("[TITLE_RIGTH]", "<b>OBLIGADO SOLIDARIO:</b>");
										lineaNotificante.Replace("[NAME_RIGTH]", ((RepresentanteLegalBOF)repDer).ObligadoSolidario);
										lineaNotificante.Replace("[NAME_REP_RIGTH]", "Rep. Legal: " + repDer.Nombre);
										lineaNotificante.Replace("[DIREC_REP_RIGTH]", repDer.Direccion);
									
								}
							}


							notificantes.Append(lineaNotificante);


						}

						if (espar != 0)
						{
							PersonaBO personaIzquierda = personasOS[personasOS.Count - 1];
							StringBuilder lineaNotificante = new StringBuilder(p);
							if (personaIzquierda == null) //Si persona Izquierda es null
							{
								lineaNotificante.Replace("[TITLE_LEFTH]", string.Empty);
								lineaNotificante.Replace("[NAME_LEFTH]", string.Empty);
								lineaNotificante.Replace("[NAME_REP_LEFTH]", string.Empty);
								lineaNotificante.Replace("[DIREC_REP_LEFTH]", string.Empty);
							}
							else
							{
								if (personaIzquierda.TipoPersona.Value == ETipoPersona.ObligadoSolidario)
								{

									lineaNotificante.Replace("[TITLE_LEFTH]", "<b>OBLIGADO SOLIDARIO:</b>");
									lineaNotificante.Replace("[NAME_LEFTH]", personaIzquierda.Nombre);
									lineaNotificante.Replace("[NAME_REP_LEFTH]", personaIzquierda.Direccion);
									lineaNotificante.Replace("[DIREC_REP_LEFTH]", string.Empty);


								}
								else if (personaIzquierda.TipoPersona == ETipoPersona.RepresentanteLegal)
								{
									RepresentanteLegalBO repIzq = personaIzquierda as RepresentanteLegalBO;

										lineaNotificante.Replace("[TITLE_LEFTH]", "<b>OBLIGADO SOLIDARIO:</b>");
										lineaNotificante.Replace("[NAME_LEFTH]", ((RepresentanteLegalBOF)repIzq).ObligadoSolidario);
										lineaNotificante.Replace("[NAME_REP_LEFTH]", "Rep. Legal: " + repIzq.Nombre);
										lineaNotificante.Replace("[DIREC_REP_LEFTH]", repIzq.Direccion);
									
								}
							}

							lineaNotificante.Replace("[TITLE_RIGTH]", string.Empty);
							lineaNotificante.Replace("[NAME_RIGTH]", string.Empty);
							lineaNotificante.Replace("[NAME_REP_RIGTH]", string.Empty);
							lineaNotificante.Replace("[DIREC_REP_RIGTH]", string.Empty);

							notificantes.Append(lineaNotificante);


						}
					}
				}

			}
			return notificantes.ToString();
		}

		private string ObtenerNombreRepresentantesObligadoSolidario(List<RepresentanteLegalBO> list)
		{
			StringBuilder nombres = new StringBuilder();
			foreach (var representanteLegalBo in list)
			{
				nombres.Append(", " + representanteLegalBo.Nombre);
			}
			return nombres.ToString().Substring(2);
		}

		private bool EsMismaActaObligadoSolidario(ObligadoSolidarioMoralBO obligadoSolidarioMoralBO, ActaConstitutivaBO actaPadre)
		{
			bool igual = true;
			int c = 0;
			var query = from counts in obligadoSolidarioMoralBO.Representantes
						group counts by new
							{
								counts.ActaConstitutiva.NumeroEscritura,
								counts.ActaConstitutiva.NumeroNotaria,
								counts.ActaConstitutiva.FechaEscritura,
								counts.ActaConstitutiva.LocalidadNotaria
							}
							into item
							select new
								{
									numeroEscrituras = item.Key.NumeroEscritura,
									numeroNotaria = item.Key.NumeroNotaria,
									fechaInscripcion = item.Key.FechaEscritura,
									localidadInscripcion = item.Key.LocalidadNotaria,
									notario = item.First().ActaConstitutiva.NombreNotario,
									count = item.Count()
								};
			string numesc = string.Empty;
			string numnot = string.Empty;
			string fec = string.Empty;
			string loc = string.Empty;

			c = query.Count();

			if (c > 1)
			{
				igual = false;
			}
			else
			{
				if (actaPadre != null && c == 1)
				{
					if (!actaPadre.FechaEscritura.HasValue || string.IsNullOrEmpty(actaPadre.LocalidadNotaria) ||
						string.IsNullOrEmpty(actaPadre.NumeroEscritura) || string.IsNullOrEmpty(actaPadre.NumeroNotaria))
						return false;
					foreach (var item in query)
					{
						fec = item.fechaInscripcion.ToString();
						loc = item.localidadInscripcion;
						numesc = item.numeroEscrituras;
						numnot = item.numeroNotaria;
					}
					if (actaPadre.FechaEscritura.Value.ToString().CompareTo(fec) == 0
						&& actaPadre.LocalidadNotaria.CompareTo(loc) == 0
						&& actaPadre.NumeroEscritura.CompareTo(numesc) == 0
						&& actaPadre.NumeroNotaria.CompareTo(numnot) == 0)
						igual = true;
					else
						igual = false;
				}
			}
			return igual;
		}

		private bool EsMismaActaRepresentanteLegal(List<RepresentanteLegalBO> representantes, ActaConstitutivaBO actaPadre)
		{
			Tuple<bool, string, string, string, string> acta = null;
			bool igual = true;
			int c = 0;
			var query = from counts in representantes
						group counts by new
						{
							counts.ActaConstitutiva.NumeroEscritura,
							counts.ActaConstitutiva.NumeroNotaria,
							counts.ActaConstitutiva.FechaEscritura,
							counts.ActaConstitutiva.LocalidadNotaria
						}
							into item
							select new
							{
								numeroEscrituras = item.Key.NumeroEscritura,
								numeroNotaria = item.Key.NumeroNotaria,
								fechaInscripcion = item.Key.FechaEscritura,
								localidadInscripcion = item.Key.LocalidadNotaria,
								notario = item.First().ActaConstitutiva.NombreNotario,
								count = item.Count()
							};
			string numesc = string.Empty;
			string numnot = string.Empty;
			string fec = string.Empty;
			string loc = string.Empty;

			c = query.Count();

			if (c > 1)
			{
				if (actaPadre != null)
				{
					if (!actaPadre.FechaEscritura.HasValue || string.IsNullOrEmpty(actaPadre.LocalidadNotaria) || string.IsNullOrEmpty(actaPadre.NumeroEscritura) || string.IsNullOrEmpty(actaPadre.NumeroNotaria))
						igual = false;
					foreach (var item in query)
					{
						fec = item.fechaInscripcion.ToString();
						loc = item.localidadInscripcion;
						numesc = item.numeroEscrituras;
						numnot = item.numeroNotaria;

						if (actaPadre.FechaEscritura.Value.ToString().CompareTo(fec) != 0
							|| actaPadre.LocalidadNotaria.CompareTo(loc) != 0
							|| actaPadre.NumeroEscritura.CompareTo(numesc) != 0
							|| actaPadre.NumeroNotaria.CompareTo(numnot) != 0)
						{
							igual = false;
							break;
						}
					}
				}
				else
					igual = false;
			}
			else
			{
				if (actaPadre != null && c == 1)
				{
					if (!actaPadre.FechaEscritura.HasValue || string.IsNullOrEmpty(actaPadre.LocalidadNotaria) ||
						string.IsNullOrEmpty(actaPadre.NumeroEscritura) || string.IsNullOrEmpty(actaPadre.NumeroNotaria))
						igual = false;
					foreach (var item in query)
					{
						fec = item.fechaInscripcion.ToString();
						loc = item.localidadInscripcion;
						numesc = item.numeroEscrituras;
						numnot = item.numeroNotaria;
					}
					if (actaPadre.FechaEscritura.Value.ToString().CompareTo(fec) == 0
						&& actaPadre.LocalidadNotaria.CompareTo(loc) == 0
						&& actaPadre.NumeroEscritura.CompareTo(numesc) == 0
						&& actaPadre.NumeroNotaria.CompareTo(numnot) == 0)
						igual = true;
					else
						igual = false;
				}
			}
			return igual;
		}

		private string ObtenerNombreRepresentantesLegales(IEnumerable<PersonaBO> list)
		{
			StringBuilder nombres = new StringBuilder();
			foreach (PersonaBO representante in list)
			{
				nombres.Append(", " + representante.Nombre);
			}
			if (nombres.Length > 0)
				return nombres.ToString().Substring(2);

			return nombres.ToString();
		}

		private string ObtenerNombresObligadosSolidarios(IEnumerable<PersonaBO> list)
		{
			StringBuilder nombres = new StringBuilder();
			var obligados = from pe in list
							orderby ((ObligadoSolidarioBO)pe).TipoObligado descending
							select pe;
			foreach (var obligado in obligados)
			{
				nombres.Append(", " + obligado.Nombre);
			}
			if (nombres.Length > 0)
				return nombres.ToString().Substring(2);

			return nombres.ToString();
		}

		protected XmlDocument ObtenerXMLDocumento(string urlXML)
		{
			try
			{
				string path = xmlUrl + urlXML;
				if (File.Exists(path) != true)
					throw new Exception("La plantilla correspondiente no se encuentra disponible", new Exception("El archivo " + urlXML + " no se encuentra en la ubicación necesaria."));
				else
				{
					XmlDocument xDoc = new XmlDocument();
					xDoc.Load(path);
					return xDoc;
				}
			}
			catch (Exception)
			{
				throw new Exception("El formato del archivo xml es incorrecto");
			}
		}

		public string ObtenerLeyendaFirmas(ContratoManttoBO contrato)
		{
			StringBuilder leyendaFirma = new StringBuilder();
			XmlDocument xDoc = ObtenerXMLDocumento(xmlFirmas);
			if (esFisico)
			{
				XmlNodeList textoLeyenda = xDoc.GetElementsByTagName("LeyendaFisico");
				if (textoLeyenda.Count < 1) throw new Exception("El formato del archivo xml es incorrecto");

				leyendaFirma.Append(textoLeyenda[0].InnerText);

				//Reemplazo de Partes de la Leyenda
				if (!string.IsNullOrEmpty(contrato.NumeroContrato) && !string.IsNullOrWhiteSpace(contrato.NumeroContrato))
					leyendaFirma.Replace("[DOCUMENTO]", string.Empty);
				else leyendaFirma.Replace("[DOCUMENTO]", string.Empty);
				if (contrato.FechaContrato.HasValue)
					leyendaFirma.Replace("[FECHA_CONTRATO]", contrato.FechaContrato.Value.ToShortDateString());
				if (contrato.Sucursal != null)
					if (contrato.Sucursal.UnidadOperativa != null)
						if (contrato.Sucursal.UnidadOperativa.Empresa != null)
							if (!string.IsNullOrEmpty(contrato.Sucursal.UnidadOperativa.Empresa.Nombre) && !string.IsNullOrWhiteSpace(contrato.Sucursal.UnidadOperativa.Empresa.Nombre))
								leyendaFirma.Replace("[UNIDAD_OPERATIVA]", contrato.Sucursal.UnidadOperativa.Empresa.Nombre);
				if (contrato.ObligadosSolidarios != null)
				{
					if (contrato.ObligadosSolidarios.Count > 0)
					{
						leyendaFirma.Replace("[S]", contrato.ObligadosSolidarios.Count > 1 ? "S" : string.Empty);
						leyendaFirma.Replace("[N]", contrato.ObligadosSolidarios.Count > 1 ? "N" : string.Empty);
						leyendaFirma.Replace("[OBLIGADOS_SOLIDARIOS]", this.ObtenerNombresObligadosSolidarios(contrato.ObligadosSolidarios));
					}
					else if (contrato.RepresentantesLegales != null)
					{
						leyendaFirma.Replace("[S]", contrato.RepresentantesLegales.Count > 1 ? "S" : string.Empty);
						leyendaFirma.Replace("[N]", contrato.RepresentantesLegales.Count > 1 ? "N" : string.Empty);
						leyendaFirma.Replace("[OBLIGADOS_SOLIDARIOS]", this.ObtenerNombreRepresentantesLegales(contrato.RepresentantesLegales));
					}
				}
				else if (contrato.RepresentantesLegales != null)
				{
					leyendaFirma.Replace("[S]", contrato.RepresentantesLegales.Count > 1 ? "S" : string.Empty);
					leyendaFirma.Replace("[N]", contrato.RepresentantesLegales.Count > 1 ? "N" : string.Empty);
					leyendaFirma.Replace("[OBLIGADOS_SOLIDARIOS]", this.ObtenerNombreRepresentantesLegales(contrato.RepresentantesLegales));
				}
				else leyendaFirma.Replace("[S]", string.Empty);

				if (!string.IsNullOrEmpty(contrato.Representante) && !string.IsNullOrWhiteSpace(contrato.Representante))
					leyendaFirma.Replace("[REPRESENTANTE_UNIDAD]", contrato.Representante);
				if (contrato.Cliente != null)
				{
					if (!string.IsNullOrEmpty(contrato.Cliente.Nombre) && !string.IsNullOrWhiteSpace(contrato.Cliente.Nombre))
						leyendaFirma.Replace("[NOMBRE_CLIENTE]", contrato.Cliente.Nombre);
				}
			}
			else
			{
				XmlNodeList textoLeyenda = xDoc.GetElementsByTagName("LeyendaMoral");
				if (textoLeyenda.Count < 1) throw new Exception("El formato del archivo xml es incorrecto");

				leyendaFirma.Append(textoLeyenda[0].InnerText);
				if (contrato.SoloRepresentantes.HasValue)
				{
					if (contrato.SoloRepresentantes.Value)
						leyendaFirma.Replace("[SOLOREPRESENTANTES]", ".");
					else
					{
						XmlNodeList textoAdicional = xDoc.GetElementsByTagName("SoloRepresentantes");
						if (textoAdicional.Count < 1)
							throw new Exception("El formato del archivo xml es incorrecto para los textos opcionales");
						leyendaFirma.Replace("[SOLOREPRESENTANTES]", textoAdicional[0].InnerText);
					}
				}

				//Reemplazo de Partes de la Leyenda
				if (!string.IsNullOrEmpty(contrato.NumeroContrato) && !string.IsNullOrWhiteSpace(contrato.NumeroContrato))
					leyendaFirma.Replace("[DOCUMENTO]", string.Empty);
				else leyendaFirma.Replace("[DOCUMENTO]", string.Empty);
				if (contrato.FechaContrato.HasValue)
					leyendaFirma.Replace("[FECHA_CONTRATO]", contrato.FechaContrato.Value.ToShortDateString());
				if (contrato.Sucursal != null)
					if (contrato.Sucursal.UnidadOperativa != null)
						if (contrato.Sucursal.UnidadOperativa.Empresa != null)
							if (!string.IsNullOrEmpty(contrato.Sucursal.UnidadOperativa.Empresa.Nombre) && !string.IsNullOrWhiteSpace(contrato.Sucursal.UnidadOperativa.Empresa.Nombre))
								leyendaFirma.Replace("[UNIDAD_OPERATIVA]", contrato.Sucursal.UnidadOperativa.Empresa.Nombre);
				if (contrato.ObligadosSolidarios != null)
				{
					if (contrato.ObligadosSolidarios.Count > 0)
					{
						leyendaFirma.Replace("[S]", contrato.ObligadosSolidarios.Count > 1 ? "S" : string.Empty);
						leyendaFirma.Replace("[N]", contrato.ObligadosSolidarios.Count > 1 ? "N" : string.Empty);
						leyendaFirma.Replace("[OBLIGADOS_SOLIDARIOS]",
											 this.ObtenerNombresObligadosSolidarios(contrato.ObligadosSolidarios));
					}
					else if (contrato.RepresentantesLegales != null)
					{
						leyendaFirma.Replace("[S]", contrato.RepresentantesLegales.Count > 1 ? "S" : string.Empty);
						leyendaFirma.Replace("[N]", contrato.RepresentantesLegales.Count > 1 ? "N" : string.Empty);
						leyendaFirma.Replace("[OBLIGADOS_SOLIDARIOS]", this.ObtenerNombreRepresentantesLegales(contrato.RepresentantesLegales));
					}
					else
					{
						leyendaFirma.Replace("[S]", string.Empty);
						leyendaFirma.Replace("[N]", string.Empty);
					}
				}
				else if (contrato.RepresentantesLegales != null)
				{
					leyendaFirma.Replace("[S]", contrato.RepresentantesLegales.Count > 1 ? "S" : string.Empty);
					leyendaFirma.Replace("[N]", contrato.RepresentantesLegales.Count > 1 ? "N" : string.Empty);
					leyendaFirma.Replace("[OBLIGADOS_SOLIDARIOS]", this.ObtenerNombreRepresentantesLegales(contrato.RepresentantesLegales));
				}
				else
					leyendaFirma.Replace("[OBLIG_PLURAL]", string.Empty);

				if (!string.IsNullOrEmpty(contrato.Representante) && !string.IsNullOrWhiteSpace(contrato.Representante))
					leyendaFirma.Replace("[REPRESENTANTE_UNIDAD]", contrato.Representante);
				if (contrato.Cliente != null)
				{
					if (!string.IsNullOrEmpty(contrato.Cliente.Nombre) && !string.IsNullOrWhiteSpace(contrato.Cliente.Nombre))
						leyendaFirma.Replace("[NOMBRE_CLIENTE]", contrato.Cliente.Nombre);
				}
				if (contrato.RepresentantesLegales != null)
				{
					if (contrato.RepresentantesLegales.Count > 0)
					{
						leyendaFirma.Replace("[REPRESENTANTE_LEGALES_CLIENTE]", this.ObtenerNombreRepresentantesLegales(contrato.RepresentantesLegales));

					}
				}
			}
			return leyendaFirma.ToString();
		}

		protected string ObtenerFirma(Dictionary<string, string> firma1, Dictionary<string, string> firma2 = null)
		{
			string firma;
			XmlDocument xDoc = ObtenerXMLDocumento(xmlFirmas);

			if (firma2 != null)
			{
				XmlNodeList textoFirma = xDoc.GetElementsByTagName("Firmas");
				if (textoFirma.Count < 1) throw new Exception("El formato del archivo xml es incorrecto");

				firma = textoFirma[0].InnerText;

				//Firma Izquierda
				firma = firma.Replace("[TITULO_IZQUIERDA]", firma1["Titulo"]);
				firma = firma.Replace("[NOMBRE_IZQUIERDA]", firma1["Nombre"]);
				firma = firma.Replace("[NOMBRE_REP_IZQUIERDA]", firma1["NombreRepresentante"]);
				firma = firma.Replace("[DIRECCION_IZQUIERDA]", firma1["Direccion"]);

				//Firma Derecha
				firma = firma.Replace("[TITULO_DERECHA]", firma2["Titulo"]);
				firma = firma.Replace("[NOMBRE_DERECHA]", firma2["Nombre"]);
				firma = firma.Replace("[NOMBRE_REP_DERECHA]", firma2["NombreRepresentante"]);
				firma = firma.Replace("[DIRECCION_DERECHA]", firma2["Direccion"]);
			}
			else
			{
				XmlNodeList textoFirma = xDoc.GetElementsByTagName("FirmasCentro");
				if (textoFirma.Count < 1) throw new Exception("El formato del archivo xml es incorrecto");

				firma = textoFirma[0].InnerText;

				firma = firma.Replace("[TITULO_CENTRO]", firma1["Titulo"]);
				firma = firma.Replace("[NOMBRE_CENTRO]", firma1["Nombre"]);
				firma = firma.Replace("[NOMBRE_REP_CENTRO]", firma1["NombreRepresentante"]);
				firma = firma.Replace("[DIRECCION_CENTRO]", firma1["Direccion"]);
			}
			return firma;
		}

		protected void ProcesaFirmas(List<Dictionary<string, string>> firmas, ContratoManttoBO contrato)
		{
			int cantFirmas = firmas.Count;
			if (cantFirmas == 0) return;

			if (cantFirmas == 1)
			{
				numFirmaActual += 1;
				SetFirma(ObtenerFirma(firmas[0]), numFirmaActual == 1, numFirmas == numFirmaActual, contrato);
				return;
			}
			bool esPar = cantFirmas % 2 == 0;
			int firmaActual = 2;
			while (firmaActual <= cantFirmas + 1)
			{
				if (esPar)
				{
					numFirmaActual += 2;
					SetFirma(ObtenerFirma(firmas[firmaActual - 2], firmas[firmaActual - 1]), numFirmaActual == 1, numFirmas == numFirmaActual, contrato);
				}
				else
				{
					if (firmaActual > cantFirmas)
					{
						numFirmaActual += 1;
						SetFirma(ObtenerFirma(firmas[cantFirmas - 1]), numFirmaActual == 1, numFirmas == numFirmaActual, contrato);
					}
					else
					{
						numFirmaActual += 2;
						SetFirma(ObtenerFirma(firmas[firmaActual - 2], firmas[firmaActual - 1]), numFirmaActual == 1, numFirmas == numFirmaActual, contrato);
					}
				}
				firmaActual += 2;
			}
		}

		protected void SetFirma(string htmlFirmas, bool firstElement, bool lastElement, ContratoManttoBO contrato)
		{
			CreateDocument();
			//Se obtiene la página actual antes de crear el documento
			int currentPage = PageCount;
			if (firstElement) PaginaInicioFirmas = currentPage;
			XRRichText richText = new XRRichText();
			richText.Html = htmlFirmas;
			richText.KeepTogether = true;
			richText.LocationF = new PointF(4, UltimaPosicion + UltimoAlto + 3);
			richText.WidthF = PageSize.Width - MargenesHorizontales - 10;
			richText.Html = htmlFirmas;
			this.dtlFirmas.Controls.Add(richText);
			UltimoAlto = richText.HeightF;
			lastPage = currentPage;
			//Una vez agregado el control se vuelve a generar para saber donde quedó
			CreateDocument();
			currentPage = PageCount;
			if (currentPage > lastPage)
				richText.Html = htmlFirmas.Replace("[Leyenda]", ObtenerLeyendaFirmas(contrato));
			else richText.Html = htmlFirmas.Replace("[Leyenda]", string.Empty);

			if (lastElement)
				CreateDocument();
		}
	}
}