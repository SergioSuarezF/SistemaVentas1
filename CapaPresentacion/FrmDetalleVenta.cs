using CapaEntidad;
using CapaNegocio;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
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
    public partial class FrmDetalleVenta : Form
    {
        public FrmDetalleVenta()
        {
            InitializeComponent();
        }

        private void FrmDetalleVenta_Load(object sender, EventArgs e)
        {
            txtBuscarDoc.Select();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            Venta oVenta = new CN_Venta().ObtenerVenta(txtBuscarDoc.Text);

            if (oVenta.IdVenta != 0)
            {
                txtNumDoc.Text = oVenta.NumeroDocumento;
                txtFecha.Text = oVenta.FechaRegistro;
                txtTipoDocumento.Text = oVenta.TipoDocumento;
                txtUsuario.Text = oVenta.oUsuario.NombreCompleto;
                txtDocCliente.Text = oVenta.DocumentoCliente;
                txtNomCliente.Text = oVenta.NombreCliente;

                dgvData.Rows.Clear();

                foreach (Detalle_Venta dv in oVenta.oDetalle_Venta)
                {
                    dgvData.Rows.Add(new object[] { dv.oProducto.Nombre, dv.PrecioVenta, dv.Cantidad, dv.SubTotal });
                }

                txtMontoTotalVenta.Text = oVenta.MontoTotal.ToString("0.00");
                txtMontoPagoVenta.Text = oVenta.MontoPago.ToString("0.00");
                txtMontoCambioVenta.Text = oVenta.MontoCambio.ToString("0.00");

            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            txtFecha.Text = "";
            txtTipoDocumento.Text = "";
            txtUsuario.Text = "";
            txtDocCliente.Text = "";
            txtNomCliente.Text = "";

            dgvData.Rows.Clear();

            txtMontoTotalVenta.Text = "0.00";
            txtMontoPagoVenta.Text = "0.00";
            txtMontoCambioVenta.Text = "0.00";
        }

        private void btnPdf_Click(object sender, EventArgs e)
        {
            if (txtTipoDocumento.Text == "")
            {
                MessageBox.Show("No se encontraron resultados", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            string Texto_HTML = Properties.Resources.PlantillaVenta.ToString();

            Negocio oDatos = new CN_Negocio().ObtenerDatos();

            //Estructura de la Información del Negocio
            Texto_HTML = Texto_HTML.Replace("@nombrenegocio", oDatos.Nombre.ToUpper());
            Texto_HTML = Texto_HTML.Replace("@docnegocio", oDatos.RUC);
            Texto_HTML = Texto_HTML.Replace("@direcnegocio", oDatos.Direccion);

            //Estructura de la Clasificación del Documento
            Texto_HTML = Texto_HTML.Replace("@tipodocumento", txtTipoDocumento.Text.ToUpper());
            Texto_HTML = Texto_HTML.Replace("@numerodocumento", txtNumDoc.Text);

            //Estructura de la Información del Proveedor
            Texto_HTML = Texto_HTML.Replace("@doccliente", txtDocCliente.Text);
            Texto_HTML = Texto_HTML.Replace("@nombrecliente", txtNomCliente.Text);
            Texto_HTML = Texto_HTML.Replace("@fecharegistro", txtFecha.Text);
            Texto_HTML = Texto_HTML.Replace("@usuarioregistro", txtUsuario.Text);

            string filas = string.Empty;
            foreach (DataGridViewRow row in dgvData.Rows)
            {
                filas += "<tr>";
                filas += "<td>" + row.Cells["Producto"].Value.ToString() + "</td>";
                filas += "<td>" + row.Cells["Precio"].Value.ToString() + "</td>";
                filas += "<td>" + row.Cells["Cantidad"].Value.ToString() + "</td>";
                filas += "<td>" + row.Cells["SubTotal"].Value.ToString() + "</td>";
                filas += "</tr>";
            }

            //Estructura de las Tablas 
            Texto_HTML = Texto_HTML.Replace("@filas", filas);
            Texto_HTML = Texto_HTML.Replace("@montototal", txtMontoTotalVenta.Text);
            Texto_HTML = Texto_HTML.Replace("@pagocon", txtMontoTotalVenta.Text);
            Texto_HTML = Texto_HTML.Replace("@cambio", txtMontoTotalVenta.Text);

            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.FileName = string.Format("Venta_{0}.pdf", txtNumDoc.Text);
            saveFile.Filter = "Pdf Files|*.pdf";


            if (saveFile.ShowDialog() == DialogResult.OK)
            {
                using (FileStream stream = new FileStream(saveFile.FileName, FileMode.Create))
                {
                    Document pdfDoc = new Document(PageSize.A4, 25, 25, 25, 25);

                    PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);
                    pdfDoc.Open();

                    bool obtenido = true;
                    byte[] byteImage = new CN_Negocio().ObtenerLogo(out obtenido);

                    if (obtenido)
                    {
                        iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(byteImage);
                        //Ojito con esto 
                        img.ScaleToFit(60, 60);
                        img.Alignment = iTextSharp.text.Image.UNDERLYING;
                        img.SetAbsolutePosition(pdfDoc.Left, pdfDoc.GetTop(51));
                        pdfDoc.Add(img);
                    }

                    using (StringReader sr = new StringReader(Texto_HTML))
                    {
                        XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
                    }

                    pdfDoc.Close();
                    stream.Close();

                    //Generamos el PDF
                    MessageBox.Show("Documento Generado con Éxito!", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
            }
        }
    }
}
