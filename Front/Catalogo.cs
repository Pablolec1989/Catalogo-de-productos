using negocio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using dominio;

namespace Front
{
    public partial class Catalogo : Form
    {
        private List<Articulo> listaArticulo;

        public Catalogo()
        {
            InitializeComponent();
        }

        private void Catalogo_Load(object sender, EventArgs e)
        {
            cargar();

            cboCampo.Items.Add("Codigo");
            cboCampo.Items.Add("Nombre");
            cboCampo.Items.Add("Categoria");
            cboCampo.Items.Add("Marca");
            cboCampo.Items.Add("Precio");
        }

        private void dgvArticulos_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvArticulos.CurrentRow != null)
            {
                Articulo seleccionado = (Articulo)dgvArticulos.CurrentRow.DataBoundItem;
                cargaImagen(seleccionado.ImagenUrl);
            }
        }

        private void cargar()
        {
            ArticuloNegocio negocio = new ArticuloNegocio();
            try
            {
                listaArticulo = negocio.Listar();
                dgvArticulos.DataSource = listaArticulo;
                dgvArticulos.Columns["Precio"].DefaultCellStyle.Format = "C2";
                ocultarColumnas();
                cargaImagen(listaArticulo[0].ImagenUrl);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void ocultarColumnas()
        {
            dgvArticulos.Columns["Id"].Visible = false;
            dgvArticulos.Columns["ImagenUrl"].Visible = false;
        }

        private void cargaImagen(string imagen)
        {
            try
            {
                pbxArticulo.Load(imagen);
            }
            catch (Exception ex)
            {

                pbxArticulo.Load("https://media.istockphoto.com/id/1147544807/es/vector/no-imagen-en-miniatura-gr%C3%A1fico-vectorial.jpg?s=612x612&w=0&k=20&c=Bb7KlSXJXh3oSDlyFjIaCiB9llfXsgS7mHFZs6qUgVk=");
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            frmAltaArt alta = new frmAltaArt();
            alta.ShowDialog();
            cargar();
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            if(dgvArticulos.CurrentRow != null)
            {
                Articulo seleccionado;
                seleccionado = (Articulo)dgvArticulos.CurrentRow.DataBoundItem;
                frmAltaArt modificar = new frmAltaArt(seleccionado);
                modificar.ShowDialog();
                cargar();
            }
            else
            {
                MessageBox.Show("No hay articulo seleccionado, por favor selecciona uno.");
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            ArticuloNegocio negocio = new ArticuloNegocio();
            Articulo seleccionado;
            try
            {
                DialogResult respuesta = MessageBox.Show("Seguro quieres eliminarlo?", "Eliminando", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                if (respuesta == DialogResult.Yes)
                {
                    seleccionado = (Articulo)dgvArticulos.CurrentRow.DataBoundItem;
                    negocio.Eliminar(seleccionado.Id);
                    cargar();
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private bool validarFiltro()
        {
            if (cboCampo.SelectedIndex < 0)
            {
                MessageBox.Show("Por favor seleccione el campo para filtrar");
                return true;
            }
            return false;
        }

        private void btnFiltro_Click(object sender, EventArgs e)
        {
            ArticuloNegocio negocio = new ArticuloNegocio();
            try
            {
                if (validarFiltro())
                    return;
                string campo = cboCampo.SelectedItem.ToString();
                string criterio = cboCriterio.SelectedItem.ToString();
                string filtro = txtFiltroAvanzado.Text;
                dgvArticulos.DataSource = negocio.filtrar(campo, criterio, filtro);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void txtFiltro_TextChanged(object sender, EventArgs e)
        {
            List<Articulo> listaFiltrada;
            string filtro = txtFiltro.Text;

            if (filtro.Length >1)
            {
                listaFiltrada = listaArticulo.FindAll(x => x.Codigo.ToLower().Contains(filtro.ToLower()) ||
                x.Nombre.ToLower().Contains(filtro.ToLower()) ||
                x.Descripcion.ToLower().Contains(filtro.ToLower()) ||
                x.Mark.Descripcion.ToLower().Contains(filtro.ToLower()) ||
                x.Category.Descripcion.ToLower().Contains(filtro.ToLower()) ||
                x.Precio.ToString().ToLower().Contains(filtro.ToLower()));
            }
            else
            {
                listaFiltrada = listaArticulo;
            }
            dgvArticulos.DataSource = null;
            dgvArticulos.DataSource = listaFiltrada;
            ocultarColumnas();
        }

        private void cboCampo_SelectedIndexChanged(object sender, EventArgs e)
        {
            string opcion = cboCampo.SelectedItem.ToString();

            if (opcion == "Categoria")
            {
                txtFiltroAvanzado.Enabled = false;
                cboCriterio.Items.Clear();
                cboCriterio.Items.Add("Celulares");
                cboCriterio.Items.Add("Televisores");
                cboCriterio.Items.Add("Media");
                cboCriterio.Items.Add("Audio");
                cboCriterio.SelectedIndex = 0;

            } else if (opcion == "Marca")
            {
                txtFiltroAvanzado.Enabled = false;
                cboCriterio.Items.Clear();
                cboCriterio.Items.Add("Samsung");
                cboCriterio.Items.Add("Apple");
                cboCriterio.Items.Add("Sony");
                cboCriterio.Items.Add("Huawei");
                cboCriterio.Items.Add("Motorola");
                cboCriterio.SelectedIndex = 0;

            } else if (opcion == "Precio")
            {
                txtFiltroAvanzado.Enabled = false;
                cboCriterio.Items.Clear();
                cboCriterio.Items.Add("$ 0 - 100.000");
                cboCriterio.Items.Add("$ 101.000 - 200.000");
                cboCriterio.Items.Add("$ 201.000 o mas");
                cboCriterio.SelectedIndex = 0;
            }
            else if(opcion == "Nombre")
            {
                txtFiltroAvanzado.Enabled = true;
                cboCriterio.Items.Clear();
                cboCriterio.Items.Add("Comienza con");
                cboCriterio.Items.Add("Termina con");
                cboCriterio.Items.Add("Contiene");
                cboCriterio.SelectedIndex = 0;
            }
            else
            {
                txtFiltroAvanzado.Enabled = true;
                cboCriterio.Items.Clear();
                cboCriterio.Items.Add("Comienza con");
                cboCriterio.Items.Add("Termina con");
                cboCriterio.Items.Add("Contiene");
                cboCriterio.SelectedIndex = 0;
            }
        }

        private void btnLimpiarFiltro_Click(object sender, EventArgs e)
        {
            cargar();
            txtFiltroAvanzado.Clear();
            cboCampo.SelectedIndex = 0;
            cboCriterio.SelectedIndex = 0;
        }
    }
    
}
