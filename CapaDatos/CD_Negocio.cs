using CapaEntidad;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Remoting.Messaging;

namespace CapaDatos
{
    public class CD_Negocio
    {
        public Negocio ObtenerDatos()
        {
            Negocio obj = new Negocio();

            try
            {
                using (SqlConnection oConexion = new SqlConnection(Conexion.cadena))
                {
                    oConexion.Open();

                    string query = "SELECT IdNegocio, Nombre, RUC, Direccion FROM NEGOCIO WHERE IdNegocio = 1";
                    SqlCommand cmd = new SqlCommand(query, oConexion);
                    cmd.CommandType = CommandType.Text;

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            obj = new Negocio()
                            {
                                IdNegocio = int.Parse(dr["IdNegocio"].ToString()),
                                Nombre = dr["Nombre"].ToString(),
                                RUC = dr["RUC"].ToString(),
                                Direccion = dr["Direccion"].ToString()
                            };
                        }
                    }
                }
            }
            catch
            {
                obj = new Negocio();
            }

            return obj;
        }

        public bool GuardarDatos(Negocio objeto, out string Mensaje)
        {
            Mensaje = string.Empty;
            bool Respuesta = true;

            try
            {
                using (SqlConnection oConexion = new SqlConnection(Conexion.cadena))
                {
                    oConexion.Open();

                    StringBuilder query = new StringBuilder();
                    query.AppendLine("UPDATE NEGOCIO SET Nombre = @nombre,");
                    query.AppendLine("RUC = @ruc,");
                    query.AppendLine("Direccion = @direccion");
                    query.AppendLine("WHERE IdNegocio = 1");


                    SqlCommand cmd = new SqlCommand(query.ToString(), oConexion);
                    cmd.Parameters.AddWithValue("@nombre", objeto.Nombre);
                    cmd.Parameters.AddWithValue("@ruc", objeto.RUC);
                    cmd.Parameters.AddWithValue("@direccion", objeto.Direccion);

                    cmd.CommandType = CommandType.Text;

                    if (cmd.ExecuteNonQuery() < 1)
                    {
                        Mensaje = "No se cargaron los datos!";
                        Respuesta = false;
                    }

                }
            }
            catch (Exception ex)
            {
                Mensaje = ex.Message;
                Respuesta = false;

            }

            return Respuesta;

        }

        public byte[] ObtenerLogo(out bool obtenido)
        {
            obtenido = true;
            byte[] logoBytes = new byte[0];

            try
            {
                using (SqlConnection oConexion = new SqlConnection(Conexion.cadena))
                {
                    oConexion.Open();

                    string query = "SELECT Logo FROM NEGOCIO WHERE IdNegocio = 1";

                    SqlCommand cmd = new SqlCommand(query.ToString(), oConexion);
                    cmd.CommandType = CommandType.Text;

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            logoBytes = (byte[])dr["Logo"];
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                obtenido = false;
                logoBytes = new byte[0];

            }
            return logoBytes;
        }

        public bool ActualizarLogo(byte[] image, out string Mensaje)
        {
            Mensaje = string.Empty;
            bool respuesta = true;

            try
            {
                using (SqlConnection oConexion = new SqlConnection(Conexion.cadena))
                {
                    oConexion.Open();

                    StringBuilder query = new StringBuilder();
                    query.AppendLine("UPDATE NEGOCIO SET LOGO = @imagen");
                    query.AppendLine("WHERE IdNegocio = 1;");

                    SqlCommand cmd = new SqlCommand(query.ToString(), oConexion);
                    cmd.Parameters.AddWithValue("@imagen", image);
                    cmd.CommandType = CommandType.Text;

                    if (cmd.ExecuteNonQuery() < 1)
                    {
                        Mensaje = "No se actualizó el logo";
                        respuesta = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Mensaje = ex.Message;
                respuesta = false;
            }

            return respuesta;

        }

        
    }
}
