//Satisface al CU089 - Bitácora de Llantas
using System;
using System.Collections.Generic;
using BPMO.Basicos.BO;
using BPMO.Facade.SDNI.BR;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Comun.BR;
using BPMO.SDNI.Equipos.BO;
using BPMO.SDNI.Equipos.BR;
using BPMO.SDNI.Equipos.VIS;

namespace BPMO.SDNI.Equipos.PRE
{
    public class EliminarLlantaPRE
    {
        #region Atributos
        private LlantaBR controladorLlanta;
        private IDataContext dctx = FacadeBR.ObtenerConexion();

        private IEliminarLlantaVIS vista;

        private string nombreClase = "EliminarLlantaPRE";

        #endregion 

        #region Constructores

        public EliminarLlantaPRE(IEliminarLlantaVIS vista)
        {
            this.vista = vista;

            this.controladorLlanta = new LlantaBR();
        }

        #endregion

        #region Metodos

		#region SC_0008
		public void ValidarAcceso()
		{
            try
            {
                //se valida que los datos del usuario y la unidad operativa no sean nulos
                if (this.vista.Usuario == null) throw new Exception("El identificador del usuario no debe ser nulo");
                if (this.vista.UnidadOperativaID == null) throw new Exception("El identificador de la Unidad Operativa no debe ser nulo ");

                //Se crea el objeto de seguridad
                UsuarioBO usuario = vista.Usuario;
                AdscripcionBO adscripcion = new AdscripcionBO() { UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaID } };
                SeguridadBO seguridadBO = new SeguridadBO(Guid.Empty, usuario, adscripcion);

                //Se valida si el usuario tiene permiso a la acción principal
                if (!FacadeBR.ExisteAccion(this.dctx, "ACTUALIZARCOMPLETO", seguridadBO))
                    this.vista.RedirigirSinPermisoAcceso();
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".ValidarAcceso:" + ex.Message);
            }
		}

		private bool ExisteAccion(List<CatalogoBaseBO> acciones, string nombreAccion)
		{
			if (acciones != null && acciones.Count > 0 && acciones.Exists(p => p != null && p.Nombre != null && p.Nombre.Trim().ToUpper() == nombreAccion.Trim().ToUpper()))
				return true;

			return false;
		}
		#endregion

		public bool ValidarDatosSesion()
		{
			try
			{
				return vista.UsuarioID != null;
			}
			catch (Exception ex)
			{
				return false;
			}
		}

		public void Baja()
        {
            string s = "";
            if ((s = this.ValidarCampos()) != null)
            {
				this.vista.MostrarMensaje(s,ETipoMensajeIU.INFORMACION);
                return;
            }

            try
            {
                BO.LlantaBO bo = (BO.LlantaBO)this.UltimaLlantaANuevaLlanta();
                LlantaBR llantaBR = new LlantaBR();

				this.controladorLlanta.ActualizarCompleto(dctx, bo, this.vista.UltimoObjetoLlanta, new SeguridadBO(Guid.Empty, new UsuarioBO() { Id = this.vista.UsuarioID }, new AdscripcionBO() { UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaID } }));

                List<LlantaBO> lst = llantaBR.ConsultarCompleto(dctx, bo);

                if (lst.Count > 0)
                {
                    vista.EstablecerDatosNavegacion("LlantaBO", lst[0]);
                    vista.EstablecerDatosNavegacion("LlantaActualizada", true);
                    vista.MostrarMensaje("Se ha dado de baja la llanta exitosamente.",ETipoMensajeIU.EXITO);
                }

                this.vista.RedirigirDetalleLlanta();
            }
            catch (Exception ex)
            {
                vista.MostrarMensaje(nombreClase + ".Baja: " + ex.Message,ETipoMensajeIU.ERROR);
            }
        }

        public void InicializarControles()
        {
            vista.InicializarControles(ConsultarTiposArchivo());
            this.Consultar();
        }

        public int ObtenerNumeroArchivos()
        {
            return vista.CantidadArchivos;
        }

        private List<TipoArchivoBO> ConsultarTiposArchivo()
        {
            List<TipoArchivoBO> tipos = new List<TipoArchivoBO>();
            TipoArchivoBR tipoArchivoBR = new TipoArchivoBR();
            foreach (string extension in vista.Extensiones)
            {
                List<TipoArchivoBO> tiposTmp = tipoArchivoBR.Consultar(dctx, new TipoArchivoBO { Extension = extension });
                if (tiposTmp.Count > 0) tipos.Add(tiposTmp[0]);
            }
            return tipos;
        }

        /// <summary>
        /// Asignar los tipos de archivos
        /// </summary>
        public void AsignarTiposArchivos()
        {
            try
            {
                TipoArchivoBR tiposBR = new TipoArchivoBR();

                TipoArchivoBO tipoBO = new TipoArchivoBO { EsImagen = false };

                this.vista.TiposArchivos = tiposBR.Consultar(dctx, tipoBO);
            }
            catch (Exception ex)
            {
				vista.MostrarMensaje("Inconsistencia al asignar los tipos de archivos",ETipoMensajeIU.INFORMACION);
            }
        }

        public void Consultar()
        {
            try
            {
                LlantaBO bo = new LlantaBO() { LlantaID = this.vista.LlantaID };

                List<BO.LlantaBO> lst = controladorLlanta.ConsultarCompleto(dctx, bo);

                if (lst.Count < 1)
                    throw new Exception("No se encontró ningún registro que corresponda a la información proporcionada.");
                if (lst.Count > 1)
                    throw new Exception("La consulta devolvió más de un registro.");

                this.vista.UltimoObjetoLlanta = lst[0];

            }
            catch (Exception ex)
            {
                vista.MostrarMensaje(this.nombreClase + ".Consultar:" + ex.Message,ETipoMensajeIU.ERROR);
            }
        }

        private object UltimaLlantaANuevaLlanta()
        {
            BO.LlantaBO bo = (BO.LlantaBO)this.vista.UltimoObjetoLlanta;
            BO.LlantaBO nuevaLlanta = new LlantaBO();

            if (bo.Auditoria == null)
                bo.Auditoria = new Basicos.BO.AuditoriaBO();

            nuevaLlanta.Auditoria = new Basicos.BO.AuditoriaBO();

            nuevaLlanta.LlantaID = bo.LlantaID;
            nuevaLlanta.Marca = bo.Marca;
            nuevaLlanta.Modelo = bo.Modelo;
            nuevaLlanta.Medida = bo.Medida;
            nuevaLlanta.Codigo = bo.Codigo;
            nuevaLlanta.Profundidad = bo.Profundidad;
            nuevaLlanta.Revitalizada = bo.Revitalizada;
            nuevaLlanta.Activo = false;
            nuevaLlanta.Stock = bo.Stock;
            nuevaLlanta.Sucursal = bo.Sucursal;
            nuevaLlanta.Auditoria.UC = bo.Auditoria.UC;
            nuevaLlanta.Auditoria.UUA = this.vista.UUA;
            nuevaLlanta.Auditoria.FC = bo.Auditoria.FC;
            nuevaLlanta.Auditoria.FUA = this.vista.FUA;

            //Se agrega archivos adjuntos a la baja de llanta
            List<ArchivoBO> adjuntos = vista.DocumentosAdjuntos;
            foreach (ArchivoBO adjuntoLlantaBO in adjuntos)
            {
                adjuntoLlantaBO.TipoAdjunto = ETipoAdjunto.Llanta;
            }
            nuevaLlanta.ArchivosAdjuntos = adjuntos;

            foreach (ArchivoBO adjunto in nuevaLlanta.ArchivosAdjuntos)
            {
                adjunto.Auditoria = new Basicos.BO.AuditoriaBO();
                adjunto.Auditoria.FC = vista.FC;
                adjunto.Auditoria.FUA = vista.FUA;
                adjunto.Auditoria.UC = vista.UC;
                adjunto.Auditoria.UUA = vista.UUA;
            }

            return nuevaLlanta;
        }

        public void EditarLlanta()
        {
            BO.LlantaBO bo = (BO.LlantaBO)this.UltimaLlantaANuevaLlanta();
            LlantaBR llantaBR = new LlantaBR();

			this.controladorLlanta.ActualizarCompleto(dctx, bo, this.vista.UltimoObjetoLlanta, new SeguridadBO(Guid.Empty, new UsuarioBO() { Id = this.vista.UsuarioID }, new AdscripcionBO() { UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaID } }));

            List<LlantaBO> lst = llantaBR.ConsultarCompleto(dctx, bo);

            if (lst.Count > 0)
            {
                vista.EstablecerDatosNavegacion("LlantaBO", lst[0]);
                vista.EstablecerDatosNavegacion("LlantaActualizada", true);
				vista.MostrarMensaje("Se ha guardado la llanta exitosamente.",ETipoMensajeIU.EXITO);
            }
        }

        public void EliminarLlanta()
        {
            this.EditarLlanta();
        }

        private string ValidarCampos()
        {
            if (this.vista.CantidadArchivos < 1)
                return "Es necesario agregar cuando menos un archivo para poder continuar";

            return null;
        }

        #endregion 
    }
}