using dominio;
using negocio;
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
using System.Configuration;

namespace Front
{
    public partial class frmAltaArt : Form
    {
        private Articulo articulo = null;
        public frmAltaArt()
        {
            InitializeComponent();
        }
        public frmAltaArt(Articulo articulo)
        {
            InitializeComponent();
            this.articulo = articulo;
            Text = "Modificar articulo";
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            ArticuloNegocio negocio = new ArticuloNegocio();
            try
            {
                if (articulo == null)
                    articulo = new Articulo();




                if(articulo.Id != 0 ) //Compruebo que si articulo existe.
                {
                    negocio.Modificar(articulo);
                    MessageBox.Show("Modificado exitosamente");
                }
                else
                {
                    if (txtCodigo.Text != "" && txtNombre.Text != "" && txtDescripcion.Text != "" && txtUrlImagen.Text != "" && (Categoria)cboCategoria.SelectedItem != null && (Marca)cboMarca.SelectedItem != null && txtPrecio.Text != "")
                    {
                        articulo.Codigo = txtCodigo.Text;
                        articulo.Nombre = txtNombre.Text;
                        articulo.Descripcion = txtDescripcion.Text;
                        articulo.ImagenUrl = txtUrlImagen.Text;
                        articulo.Category = (Categoria)cboCategoria.SelectedItem;
                        articulo.Mark = (Marca)cboMarca.SelectedItem;
                        articulo.Precio = decimal.Parse(txtPrecio.Text);
                        negocio.Agregar(articulo);
                        MessageBox.Show("Agregado correctamente");
                        Close();
                    }
                    else
                    {
                        MessageBox.Show("Te faltan llenar campos");
                    }

                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void frmAltaDisco_Load(object sender, EventArgs e)
        {
            CategoriaNegocio categoriaNegocio = new CategoriaNegocio();
            MarcaNegocio marcaNegocio = new MarcaNegocio();
            try
            {
                cboCategoria.DataSource = categoriaNegocio.Listar();
                cboCategoria.ValueMember = "Id";
                cboCategoria.DisplayMember = "Descripcion";

                cboMarca.DataSource = marcaNegocio.Listar();
                cboCategoria.ValueMember = "Id";
                cboCategoria.DisplayMember = "Descripcion";

                if (articulo != null)
                {
                    txtCodigo.Text = articulo.Codigo;
                    txtNombre.Text = articulo.Nombre;
                    txtDescripcion.Text = articulo.Descripcion;
                    txtUrlImagen.Text = articulo.ImagenUrl;
                    cargaImagen(articulo.ImagenUrl);
                    txtPrecio.Text = (articulo.Precio.ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


        private void txtUrlImagen_Leave(object sender, EventArgs e)
        {
            cargaImagen(txtUrlImagen.Text);
        }

        private void cargaImagen(string imagen)
        {
            try
            {
                pbxArticulos.Load(imagen);
            }
            catch (Exception ex)
            {

                pbxArticulos.Load("https://media.istockphoto.com/id/1147544807/es/vector/no-imagen-en-miniatura-gr%C3%A1fico-vectorial.jpg?s=612x612&w=0&k=20&c=Bb7KlSXJXh3oSDlyFjIaCiB9llfXsgS7mHFZs6qUgVk=");
            }
        }

        private void btnAgregarImagen_Click(object sender, EventArgs e)
        {
            OpenFileDialog archivo = new OpenFileDialog();
            archivo.Filter = "jpg|*.jpg|png|*.png";
            if(archivo.ShowDialog() == DialogResult.OK)
            {
                txtUrlImagen.Text = archivo.FileName;
                cargaImagen(archivo.FileName);

                File.Copy(archivo.FileName, ConfigurationManager.AppSettings["Images"] + archivo.SafeFileName);

            }
        }

        private void txtPrecio_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                MessageBox.Show("Solo se admiten valores numéricos");
                e.Handled = true;
            }
        }

        private void txtPrecio_TextChanged(object sender, EventArgs e)
        {
            formato_moneda(Convert.ToDecimal(txtPrecio.Text));
            txtPrecio.Select(txtPrecio.Text.Length - 3, 0);
        }
        private void formato_moneda(decimal numero)
        {
            txtPrecio.Text = numero.ToString("n2");
        }
    }
}
