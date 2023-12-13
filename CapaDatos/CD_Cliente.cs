using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaEntidad;

namespace CapaDatos
{
    public class CD_Cliente
    {
        public List<Cliente> Listar()
        {
            List<Cliente> lista = new List<Cliente>();

            using (SqlConnection oConexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    StringBuilder query = new StringBuilder();
                    query.AppendLine("SELECT IdCliente, Documento, NombreCompleto, Correo, Telefono, Estado FROM CLIENTE");

                    SqlCommand cmd = new SqlCommand(query.ToString(), oConexion);
                    cmd.CommandType = CommandType.Text;

                    oConexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Cliente()
                            {
                                IdCliente = Convert.ToInt32(dr["IdCliente"]),
                                Documento = dr["Documento"].ToString(),
                                NombreCompleto = dr["NombreCompleto"].ToString(),
                                Correo = dr["Correo"].ToString(),
                                Telefono = dr["Telefono"].ToString(),
                                Estado = Convert.ToBoolean(dr["Estado"])
                            });
                        }
                    }

                }
                catch (Exception ex)
                {
                    lista = new List<Cliente>();
                }
            }
            return lista;
        }


        public int Registrar(Cliente objCliente, out string Mensaje)
        {
            int IdClienteGenerado = 0;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oConexion = new SqlConnection(Conexion.cadena))
                {
                    SqlCommand cmd = new SqlCommand("SP_REGISTRARCLIENTE", oConexion);
                    cmd.Parameters.AddWithValue("Documento", objCliente.Documento);
                    cmd.Parameters.AddWithValue("NombreCompleto", objCliente.NombreCompleto);
                    cmd.Parameters.AddWithValue("Correo", objCliente.Correo);
                    cmd.Parameters.AddWithValue("Telefono", objCliente.Telefono);
                    cmd.Parameters.AddWithValue("Estado", objCliente.Estado);
                    cmd.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;

                    cmd.CommandType = CommandType.StoredProcedure;

                    oConexion.Open();

                    cmd.ExecuteNonQuery();

                    IdClienteGenerado = Convert.ToInt32(cmd.Parameters["Resultado"].Value);
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();
                    
                }
            }
            catch (Exception ex)
            {
                IdClienteGenerado = 0;
                Mensaje = ex.Message;
            }
            return IdClienteGenerado;
        }


        public bool Editar(Cliente objCliente, out string Mensaje)
        {
            bool respuesta = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oConexion = new SqlConnection(Conexion.cadena))
                {
                    SqlCommand cmd = new SqlCommand("SP_EDITARCLIENTE", oConexion);
                    cmd.Parameters.AddWithValue("IdCliente", objCliente.IdCliente);
                    cmd.Parameters.AddWithValue("Documento", objCliente.Documento);
                    cmd.Parameters.AddWithValue("NombreCompleto", objCliente.NombreCompleto);
                    cmd.Parameters.AddWithValue("Correo", objCliente.Correo);
                    cmd.Parameters.AddWithValue("Telefono", objCliente.Telefono);
                    cmd.Parameters.AddWithValue("Estado", objCliente.Estado);
                    cmd.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;

                    cmd.CommandType = CommandType.StoredProcedure;

                    oConexion.Open();

                    cmd.ExecuteNonQuery();

                    respuesta = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();

                }
            }
            catch (Exception ex)
            {
                respuesta = false;
                Mensaje = ex.Message;
            }
            return respuesta;

        }

        public bool Eliminar(Cliente objCliente, out string Mensaje)
        {
            bool respuesta = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oConexion = new SqlConnection(Conexion.cadena))
                {
                    SqlCommand cmd = new SqlCommand("SP_ELIMINARCLIENTE", oConexion);
                    cmd.Parameters.AddWithValue("IdCliente", objCliente.IdCliente);
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;

                    cmd.CommandType = CommandType.StoredProcedure;

                    oConexion.Open();

                    respuesta = cmd.ExecuteNonQuery() > 0 ? true : false;

                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();

                }
            }
            catch (Exception ex)
            {
                respuesta = false;
                Mensaje = ex.Message;
            }
            return respuesta;

        }
        

        


    }
}
