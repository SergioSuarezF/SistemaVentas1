using CapaEntidad;
using CapaNegocio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CapaPresentacion
{
    public partial class FrmNegocio : Form
    {
        public FrmNegocio()
        {
            InitializeComponent();
        }

        public Image ByteToImage(byte[] imageBytes)
        {
            MemoryStream ms = new MemoryStream();
            ms.Write(imageBytes, 0, imageBytes.Length);
            Image imagen = new Bitmap(ms);

            return imagen;
        }

        private void FrmNegocio_Load(object sender, EventArgs e)
        {
            bool obtenido = true;
            byte[] byteImage = new CN_Negocio().ObtenerLogo(out obtenido);

            if (obtenido)
            {
                piclogo.Image = ByteToImage(byteImage);

            }

            Negocio datos = new CN_Negocio().ObtenerDatos();

            txtNombre.Text = datos.Nombre;
            txtRUC.Text = datos.RUC;
            txtDireccion.Text = datos.Direccion;


        }

        private void btnCargar_Click(object sender, EventArgs e)
        {
            string Mensaje = string.Empty;

            OpenFileDialog ofd = new OpenFileDialog();

            ofd.FileName = "Files|*.bak;*.jpg;*.jpeg;*.png";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                byte[] byteImage = File.ReadAllBytes(ofd.FileName);
                bool respuesta = new CN_Negocio().ActualizarLogo(byteImage, out Mensaje);

                if (respuesta)
                {
                    piclogo.Image = ByteToImage(byteImage);
                }
                else
                {
                    MessageBox.Show(Mensaje, "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }

        private void btnGuardarCambios_Click(object sender, EventArgs e)
        {
            string Mensaje = string.Empty;

            Negocio obj = new Negocio()
            {
                Nombre = txtNombre.Text,
                RUC = txtRUC.Text,
                Direccion = txtDireccion.Text
            };

            bool respuesta = new CN_Negocio().GuardarDatos(obj, out Mensaje);
            
            if (respuesta)
            {
                MessageBox.Show("Los cambios han sido guardados", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("No se guardaron los cambios", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }
    }
}
