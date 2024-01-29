using CapaEntidad;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class CD_Compra
    {
        public int ObtenerCorrelativo()
        {
            int idCorrelativo = 0;

            using (SqlConnection oConexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    StringBuilder query = new StringBuilder();
                    query.AppendLine("SELECT COUNT(*) + 1 FROM COMPRA");
                    SqlCommand cmd = new SqlCommand(query.ToString(), oConexion);
                    cmd.CommandType = CommandType.Text;

                    oConexion.Open();

                    idCorrelativo = Convert.ToInt32(cmd.ExecuteScalar());
                }
                catch (Exception ex) 
                {
                    idCorrelativo = 0;
                }

            }

            return idCorrelativo;
        }

        public bool Registrar(Compra obj, DataTable DetalleCompra, out string Mensaje)
        {
            bool Respuesta = false;
            Mensaje = string.Empty;

            using (SqlConnection oConexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("SP_REGISTRARCOMPRA", oConexion);
                    cmd.Parameters.AddWithValue("IdUsuario", obj.oUsuario.IdUsuario);
                    cmd.Parameters.AddWithValue("IdProveedor", obj.oproveedor.IdProveedor);
                    cmd.Parameters.AddWithValue("TipoDocumento", obj.TipoDocumento);
                    cmd.Parameters.AddWithValue("NumeroDocumento", obj.NumeroDocumento);
                    cmd.Parameters.AddWithValue("MontoTotal", obj.MontoTotal);
                    cmd.Parameters.AddWithValue("DetalleCompra", DetalleCompra);

                    cmd.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;

                    cmd.CommandType = CommandType.StoredProcedure;

                    oConexion.Open();

                    cmd.ExecuteNonQuery();

                    

                    Respuesta = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();


                }
                catch (Exception ex)
                {
                    Respuesta = false;
                    Mensaje = ex.Message;
                }
            }
            return Respuesta;
        }


        public Compra ObtenerCompra(string numero)
        {
            Compra obj = new Compra();

            using (SqlConnection oConexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    StringBuilder query = new StringBuilder();
                    query.AppendLine("SELECT c.IdCompra,");
                    query.AppendLine("u.NombreCompleto,");
                    query.AppendLine("pr.Documento, pr.RazonSocial,");
                    query.AppendLine("c.TipoDocumento, c.NumeroDocumento, c.MontoTotal, CONVERT(char(10), c.FechaRegistro, 103)[FechaRegistro]");
                    query.AppendLine("FROM COMPRA c");
                    query.AppendLine("INNER JOIN USUARIO u ON u.IdUsuario = c.IdUsuario");
                    query.AppendLine("INNER JOIN PROVEEDOR pr ON pr.IdProveedor = c.IdProveedor");
                    query.AppendLine("WHERE c.NumeroDocumento = @numero");

                    SqlCommand cmd = new SqlCommand(query.ToString(), oConexion);
                    cmd.Parameters.AddWithValue("@numero", numero);
                    cmd.CommandType = CommandType.Text;

                    oConexion.Open();
                    //cmd.ExecuteNonQuery();

                    using (SqlDataReader dr =  cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            obj = new Compra()
                            {
                                IdCompra = Convert.ToInt32(dr["IdCompra"]),
                                oUsuario = new Usuario()
                                {
                                    NombreCompleto = dr["NombreCompleto"].ToString()
                                },
                                oproveedor = new Proveedor() {
                                    Documento = dr["Documento"].ToString(),
                                    RazonSocial = dr["RazonSocial"].ToString()
                                },
                                TipoDocumento = dr["TipoDocumento"].ToString(),
                                NumeroDocumento = dr["NumeroDocumento"].ToString(),
                                MontoTotal = Convert.ToDecimal(dr["MontoTotal"].ToString()),
                                FechaRegistro = dr["FechaRegistro"].ToString()
                                
                            };
                        }
                    }

                }
                catch (Exception ex)
                {
                    obj = new Compra();
                }

            }

            return obj;
        }

        public List<Detalle_Compra> ObtenerDetalleCompra(int IdCompra)
        {
            List<Detalle_Compra> oLista = new List<Detalle_Compra>();

            try
            {
                using (SqlConnection oConexion = new SqlConnection(Conexion.cadena))
                {
                    oConexion.Open();
                    StringBuilder query = new StringBuilder();

                    query.AppendLine("SELECT p.Nombre, dc.PrecioCompra, dc.Cantidad, dc.MontoTotal FROM DETALLE_COMPRA dc");
                    query.AppendLine("INNER JOIN PRODUCTO p ON p.IdProducto = dc.IdProducto");
                    query.AppendLine("WHERE dc.IdCompra = @IdCompra");

                    SqlCommand cmd = new SqlCommand(query.ToString(), oConexion);
                    cmd.Parameters.AddWithValue("@IdCompra", IdCompra);
                    cmd.CommandType = CommandType.Text;

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            oLista.Add(new Detalle_Compra()
                            {
                                oProducto = new Producto() { Nombre = dr["Nombre"].ToString() },
                                PrecioCompra = Convert.ToDecimal(dr["PrecioCompra"].ToString()),
                                Cantidad = Convert.ToInt32(dr["Cantidad"].ToString()),
                                MontoTotal = Convert.ToDecimal(dr["MontoTotal"].ToString()),
                            });
                        }

                    }

                }

            }
            catch (Exception ex)
            {
                oLista = new List<Detalle_Compra>();
            }
                
            return oLista;

        }
        
    }
}
