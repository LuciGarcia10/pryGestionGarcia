using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace pryGestionGarcia
{
    public partial class Agregar_producto : Form
    {
        public Agregar_producto()
        {
            InitializeComponent();
        }

        private void Agregar_producto_Load(object sender, EventArgs e)
        {
            //Crea una instancia de la clase ConexionBD, maneja la conexión a la base de datos y las operaciones de consulta.
            ConexionBD conexion = new ConexionBD();
            //Llama al método listarProductos, llena dgvProductos con los productos de la base de datos
            conexion.listarProductos(dgvProductos);
            //Llama al método LlenarcmbCategorias, carga el control cmbCategorias con las categorías de productos desde la base de datos
            conexion.LlenarcmbCategorias(cmbCategorias);
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            //Crea un nuevo objeto Productos para almacenar los datos del nuevo producto
            Productos nuevoProducto = new Productos();
            //Crea otra instancia de ConexionBD 
            ConexionBD conexion = new ConexionBD();

            // No asignar idProducto aquí, porque es autoincremental
            nuevoProducto.codigo = int.Parse(txtCodigo.Text);
            nuevoProducto.nombre = txtNombre.Text;
            nuevoProducto.categoria = cmbCategorias.Text;
            nuevoProducto.descripcion = txtDescripcion.Text;
            nuevoProducto.precio = double.Parse(txtPrecio.Text);
            nuevoProducto.stock = Convert.ToInt32(nupStock.Value);

            try
            {
                //Llama al método AgregarProducto para insertar nuevoProducto en la base de datos
                conexion.AgregarProducto(nuevoProducto);
                //Actualiza a dgvProductos para mostrar el producto recién agregado
                conexion.listarProductos(dgvProductos);
                //Llama a llenarListbox para agregar la descripción del producto en el lstDescripcion
                conexion.llenarListbox(lstDescripcion, nuevoProducto.nombre, nuevoProducto.descripcion, (double)nuevoProducto.precio, nuevoProducto.stock);
                //Llama al método Limpiar para borrar los campos de texto del formulario
                Limpiar();

            }//Atrapa cualquier excepción que pueda ocurrir y muestra un mensaje de error
            catch (Exception ex)
            {
                MessageBox.Show("Error al agregar el producto: " + ex.Message);
            }
        }


        //Establece en vacío los campos txtCodigo, txtNombre, txtDescripcion, y txtPrecio
        public void Limpiar()
        {
            txtCodigo.Text = "";
            txtNombre.Text = "";
            txtDescripcion.Text = "";
            txtPrecio.Text = "";
        }

        //se crea y muestra el formulario Modifica_producto para modificar productos
        private void agregarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Modifica_producto frm = new Modifica_producto();
            frm.ShowDialog();
        }

        //se crea y muestra el formulario Eliminar_Producto para eliminar productos
        private void eliminarToolStripMenuItem_Click(object sender, EventArgs e)
        {

            Eliminar_Producto frm = new Eliminar_Producto();
            frm.ShowDialog();
        }

        //se crea y muestra el formulario Reporte para generar un informe de los productos
        private void reporteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Reporte frm = new Reporte();
            frm.ShowDialog();
        }
    }
}
