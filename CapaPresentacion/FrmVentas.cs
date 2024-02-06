using CapaEntidad;
using CapaNegocio;
using CapaPresentacion.Modales;
using CapaPresentacion.Utilidades;
using DocumentFormat.OpenXml.Bibliography;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CapaPresentacion
{
    public partial class FrmVentas : Form
    {
        private Usuario _usuario;

        public FrmVentas(Usuario oUsuario = null)
        {
            _usuario = oUsuario;
            InitializeComponent();
        }

        private void FrmVentas_Load(object sender, EventArgs e)
        {
            cbTipoDoc.Items.Add(new OpcionCombo() { Valor = "Boleta", Texto = "Boleta" });
            cbTipoDoc.Items.Add(new OpcionCombo() { Valor = "Factura", Texto = "Factura" });
            cbTipoDoc.DisplayMember = "Texto";
            cbTipoDoc.ValueMember = "Valor";
            cbTipoDoc.SelectedIndex = 0;

            txtFecha.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtIdProducto.Text = "0";
            txtPagoCon.Text = "";
            txtCambio.Text = "";
            txtTotalPagar.Text = "0";
        }

        private void btnBuscarCliente_Click(object sender, EventArgs e)
        {
            using (var modal = new mdCliente())
            {
                var result = modal.ShowDialog();

                if (result == DialogResult.OK)
                {
                    txtDocCliente.Text = modal._cliente.Documento;
                    txtNomCliente.Text = modal._cliente.NombreCompleto;
                    txtCodProducto.Select();
                }
                else
                {
                    txtDocCliente.Select();
                }

            }
        }

        private void btnBuscarProducto_Click(object sender, EventArgs e)
        {
            using (var modal = new mdProducto())
            {
                var result = modal.ShowDialog();

                if (result == DialogResult.OK)
                {
                    txtIdProducto.Text = modal._producto.IdProducto.ToString();
                    txtCodProducto.Text = modal._producto.Codigo;
                    txtNomProducto.Text = modal._producto.Nombre;
                    txtPrecio.Text = modal._producto.PrecioVenta.ToString("0.00");
                    txtStock.Text = modal._producto.Stock.ToString();
                    nudCantidad.Select();
                }
                else
                {
                    txtCodProducto.Select();
                }

            }
        }

        private void txtCodProducto_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                Producto oProducto = new CN_Producto().Listar().Where(p => p.Codigo == txtCodProducto.Text && p.Estado == true).FirstOrDefault();

                if (oProducto != null)
                {
                    txtCodProducto.BackColor = Color.Honeydew;
                    txtIdProducto.Text = oProducto.IdProducto.ToString();
                    txtNomProducto.Text = oProducto.Nombre;
                    txtPrecio.Text = oProducto.PrecioVenta.ToString("0.00");
                    txtStock.Text = oProducto.Stock.ToString();
                    nudCantidad.Select();
                }
                else
                {
                    txtCodProducto.BackColor = Color.MistyRose;
                    txtIdProducto.Text = "0";
                    txtNomProducto.Text = "";
                    txtPrecio.Text = "";
                    txtStock.Text = "";
                    nudCantidad.Value = 1;
                }

            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            decimal precio = 0;
            bool producto_existe = false;

            if (int.Parse(txtIdProducto.Text) == 0)
            {
                MessageBox.Show("Selecciona un producto por favor", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (!decimal.TryParse(txtPrecio.Text, out precio))
            {
                MessageBox.Show("Precio - formato de moneda incorrecto", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtPrecio.Select();
                return;
            }

            if (Convert.ToInt32(txtStock.Text) < Convert.ToInt32(nudCantidad.Value.ToString()))
            {
                MessageBox.Show("La cantidad de productos supera a la del stock", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            foreach (DataGridViewRow fila in dgvData.Rows)
            {
                if (fila.Cells["IdProducto"].Value.ToString() == txtIdProducto.Text)
                {
                    producto_existe = true;
                    break;
                }
            }

            if (!producto_existe)
            {
                //string Mensaje = string.Empty;

                bool Respuesta = new CN_Venta().RestarStock(
                    Convert.ToInt32(txtIdProducto.Text),
                    Convert.ToInt32(nudCantidad.Value.ToString())
                    );

                if (Respuesta)
                {
                    dgvData.Rows.Add(new object[] {
                        txtIdProducto.Text,
                        txtNomProducto.Text,
                        precio.ToString("0.00"),
                        nudCantidad.Value.ToString(),
                        (nudCantidad.Value * precio).ToString("0.00")
                    });

                    calcularTotal();
                    limpiarProducto();
                    txtCodProducto.Select();
                }

                /*dgvData.Rows.Add(new object[] {
                        txtIdProducto.Text,
                        txtNomProducto.Text,
                        precio.ToString("0.00"),
                        nudCantidad.Value.ToString(),
                        (nudCantidad.Value * precio).ToString("0.00")
                });

                calcularTotal();
                limpiarProducto();
                txtCodProducto.Select();*/

            }

        }

        private void calcularTotal()
        {
            decimal total = 0;
            if (dgvData.Rows.Count > 0)
            {
                foreach (DataGridViewRow row in dgvData.Rows)
                {
                    total += Convert.ToDecimal(row.Cells["SubTotal"].Value.ToString());
                }
            }
            txtTotalPagar.Text = total.ToString("0.00");

        }

        private void limpiarProducto()
        {
            txtIdProducto.Text = "0";
            txtCodProducto.Text = "";
            txtNomProducto.Text = "";
            txtPrecio.Text = "";
            txtStock.Text = "";
            nudCantidad.Value = 1;
        }

        private void dgvData_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            if (e.ColumnIndex == 5)
            {
                e.Paint(e.CellBounds, DataGridViewPaintParts.All);

                var w = Properties.Resources.trash.Width;
                var h = Properties.Resources.trash.Height;
                var x = e.CellBounds.Left + (e.CellBounds.Width - w) / 2;
                var y = e.CellBounds.Top + (e.CellBounds.Height - h) / 2;

                e.Graphics.DrawImage(Properties.Resources.trash, new Rectangle(x, y, w, h));
                e.Handled = true;

            }
        }

        private void dgvData_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvData.Columns[e.ColumnIndex].Name == "btnEliminar")
            {
                int index = e.RowIndex;
                if (index >= 0)
                {
                    bool Respuesta = new CN_Venta().SumarStock(
                        Convert.ToInt32(dgvData.Rows[index].Cells["IdProducto"].Value.ToString()),
                        Convert.ToInt32(dgvData.Rows[index].Cells["Cantidad"].Value.ToString())
                        );

                    if (Respuesta)
                    {
                        dgvData.Rows.RemoveAt(index);
                        calcularTotal();
                    }

                }
            }
        }

        private void txtPrecio_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar))
            {
                e.Handled = false;
            }
            else
            {
                if (txtPrecio.Text.Trim().Length == 0 && e.KeyChar.ToString() == ",")
                {
                    e.Handled = true;
                }
                else
                {
                    if (Char.IsControl(e.KeyChar) || e.KeyChar.ToString() == ",")
                    {
                        e.Handled = false;
                    }
                    else
                    {
                        e.Handled = true;
                    }
                }
            }
        }

        private void txtPagoCon_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar))
            {
                e.Handled = false;
            }
            else
            {
                if (txtPagoCon.Text.Trim().Length == 0 && e.KeyChar.ToString() == ",")
                {
                    e.Handled = true;
                }
                else
                {
                    if (Char.IsControl(e.KeyChar) || e.KeyChar.ToString() == ",")
                    {
                        e.Handled = false;
                    }
                    else
                    {
                        e.Handled = true;
                    }
                }
            }
        }

        private void calcVuelto()
        {
            if (txtTotalPagar.Text.Trim() == "")
            {
                MessageBox.Show("No hay productos en la venta", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            decimal pagacon;
            decimal total = Convert.ToDecimal(txtTotalPagar.Text);

            if (txtPagoCon.Text.Trim() == "")
            {
                txtPagoCon.Text = "0";
            }

            if (decimal.TryParse(txtPagoCon.Text.Trim(), out pagacon))
            {
                if (pagacon < total)
                {
                    txtCambio.Text = "0.00";
                }
                else
                {
                    decimal vuelto = pagacon - total;
                    txtCambio.Text = vuelto.ToString();
                }
            }

        }

        private void txtPagoCon_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                calcVuelto();
            }
        }

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            if (txtDocCliente.Text == "")
            {
                MessageBox.Show("Debes ingresar el DNI del cliente", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (txtNomCliente.Text == "")
            {
                MessageBox.Show("Debes ingresar el nombre del cliente", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (dgvData.Rows.Count < 1)
            {
                MessageBox.Show("Debes ingresar al menos un producto a la venta", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            //Código para solucionar 

            /*int IdProducto = Convert.ToInt32(txtIdProducto.Text);
            int Cantidad = Convert.ToInt32(nudCantidad.Value.ToString());

            bool Resultado = new CN_Venta().RestarStock(IdProducto, Cantidad);
            
            if (Resultado)
            {
                DataTable Detalle_Venta = new DataTable();

                Detalle_Venta.Columns.Add("IdProducto", typeof(string));
                Detalle_Venta.Columns.Add("PrecioVenta", typeof(decimal));
                Detalle_Venta.Columns.Add("Cantidad", typeof(int));
                Detalle_Venta.Columns.Add("SubTotal", typeof(decimal));

                foreach (DataGridViewRow row in dgvData.Rows)
                {
                    Detalle_Venta.Rows.Add(new object[]
                    {
                    row.Cells["IdProducto"].Value.ToString(),
                    row.Cells["Precio"].Value.ToString(),
                    row.Cells["Cantidad"].Value.ToString(),
                    row.Cells["SubTotal"].Value.ToString()
                    });
                }

                int IdCorrelativo = new CN_Venta().ObtenerCorrelativo();
                string numeroDocumento = string.Format("{0:0000000000}", IdCorrelativo);
                calcVuelto();

                Venta oVenta = new Venta()
                {
                    oUsuario = new Usuario() { IdUsuario = _usuario.IdUsuario },
                    TipoDocumento = ((OpcionCombo)cbTipoDoc.SelectedItem).Texto,
                    NumeroDocumento = numeroDocumento,
                    DocumentoCliente = txtDocCliente.Text,
                    NombreCliente = txtNomCliente.Text,
                    MontoPago = Convert.ToDecimal(txtPagoCon.Text),
                    MontoCambio = Convert.ToDecimal(txtCambio.Text),
                    MontoTotal = Convert.ToDecimal(txtTotalPagar.Text)

                };

                string Mensaje = string.Empty;
                bool Respuesta = new CN_Venta().Registrar(oVenta, Detalle_Venta, out Mensaje);

                if (Respuesta)
                {
                    var Result = MessageBox.Show("El número de venta ha sido generado:\n" + numeroDocumento + "\n\n¿Desea copiar al portapapeles?", "Mensaje", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    //El número de compra ha sido generado:\n" + numeroDocumento + "\n\n¿Desea copiar al portapapeles?", "Mensaje", MessageBoxButtons.YesNo, MessageBoxIcon.Information
                    if (Result == DialogResult.Yes)
                    {
                        Clipboard.SetText(numeroDocumento);
                    }

                    txtDocCliente.Text = "";
                    txtNomCliente.Text = "";
                    dgvData.Rows.Clear();
                    txtPagoCon.Text = "";
                    txtCambio.Text = "";



                }
                else
                {
                    MessageBox.Show(Mensaje, "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            else
            {
                MessageBox.Show("No se pudo disminuir el stock del producto", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }*/

            //Fin Código

            DataTable Detalle_Venta = new DataTable();

            Detalle_Venta.Columns.Add("IdProducto", typeof(string));
            Detalle_Venta.Columns.Add("PrecioVenta", typeof(decimal));
            Detalle_Venta.Columns.Add("Cantidad", typeof(int));
            Detalle_Venta.Columns.Add("SubTotal", typeof(decimal));

            foreach (DataGridViewRow row in dgvData.Rows)
            {
                Detalle_Venta.Rows.Add(new object[]
                {
                    row.Cells["IdProducto"].Value.ToString(),
                    row.Cells["Precio"].Value.ToString(),
                    row.Cells["Cantidad"].Value.ToString(),
                    row.Cells["SubTotal"].Value.ToString()
                });
            }

            int IdCorrelativo = new CN_Venta().ObtenerCorrelativo();
            string numeroDocumento = string.Format("{0:0000000000}", IdCorrelativo);
            calcVuelto();

            Venta oVenta = new Venta()
            {
                oUsuario = new Usuario() { IdUsuario = _usuario.IdUsuario },
                TipoDocumento = ((OpcionCombo)cbTipoDoc.SelectedItem).Texto,
                NumeroDocumento = numeroDocumento,
                DocumentoCliente = txtDocCliente.Text,
                NombreCliente = txtNomCliente.Text,
                MontoPago = Convert.ToDecimal(txtPagoCon.Text),
                MontoCambio = Convert.ToDecimal(txtCambio.Text),
                MontoTotal = Convert.ToDecimal(txtTotalPagar.Text)

            };

            string Mensaje = string.Empty;
            bool Respuesta = new CN_Venta().Registrar(oVenta, Detalle_Venta, out Mensaje);

            if (Respuesta)
            {
                var Result = MessageBox.Show("El número de venta ha sido generado:\n" + numeroDocumento + "\n\n¿Desea copiar al portapapeles?", "Mensaje", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                //El número de compra ha sido generado:\n" + numeroDocumento + "\n\n¿Desea copiar al portapapeles?", "Mensaje", MessageBoxButtons.YesNo, MessageBoxIcon.Information
                if (Result == DialogResult.Yes)
                {
                    Clipboard.SetText(numeroDocumento);
                }

                txtDocCliente.Text = "";
                txtNomCliente.Text = "";
                dgvData.Rows.Clear();
                txtPagoCon.Text = "";
                txtCambio.Text = "";



            }
            else
            {
                MessageBox.Show(Mensaje, "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }


        }


    }
}
