using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class CN_Cliente
    {
        private CD_Cliente objcd_cliente = new CD_Cliente();

        public List<Cliente> Listar()
        {
            return objcd_cliente.Listar();
        }

        public int Registrar(Cliente objCliente, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (objCliente.Documento == "")
            {
                Mensaje += "Se necesita el DNI del cliente\n";
            }

            if (objCliente.NombreCompleto == "")
            {
                Mensaje += "Se necesita el Nombre Completo del cliente\n";
            }

            if (objCliente.Correo == "")
            {
                Mensaje += "Se necesita la Correo del cliente\n";
            }

            if (Mensaje != string.Empty)
            {
                return 0;
            }
            else
            {
                return objcd_cliente.Registrar(objCliente, out Mensaje);
            }
        }

        public bool Editar(Cliente objCliente, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (objCliente.Documento == "")
            {
                Mensaje += "Se necesita el DNI del cliente\n";
            }

            if (objCliente.NombreCompleto == "")
            {
                Mensaje += "Se necesita el Nombre Completo del cliente\n";
            }

            if (objCliente.Correo == "")
            {
                Mensaje += "Se necesita la Correo del cliente\n";
            }

            if (Mensaje != string.Empty)
            {
                return false;
            }
            else
            {
                return objcd_cliente.Editar(objCliente, out Mensaje);
            }

        }

        public bool Eliminar(Cliente objCliente, out string Mensaje)
        {
            return objcd_cliente.Eliminar(objCliente, out Mensaje);
        }
    }
}
