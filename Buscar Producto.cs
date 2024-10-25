using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pryGestionGarcia
{
    public partial class Buscar_Producto : Form
    {
        public Buscar_Producto()
        {
            InitializeComponent();
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        //Crea una instancia de Modifica_producto
        //y la muestra en una ventana de diálogo modal 
        private void modiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Modifica_producto frm = new Modifica_producto();
            frm.ShowDialog();
        }

        //Crea y muestra una instancia del Agregar_producto para agregar un nuevo producto
        private void agregarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Agregar_producto frm = new Agregar_producto();
            frm.ShowDialog();
        }

        //Crea una instancia de Eliminar_Producto y la muestra permitiendo eliminar un producto
        private void eliminarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Eliminar_Producto frm = new Eliminar_Producto();
            frm.ShowDialog();
        }

        //Crea y muestra una instancia de Reporte para generar y ver el reporte de productos
        private void reporteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Reporte frm = new Reporte();
            frm.ShowDialog();

        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            //Crea una instancia de Productos, que representará el producto a buscar.
            // Declara una variable idProducto para almacenar el código del producto.
            Productos productos = new Productos();
            int idProducto;

            //Verifica si el valor en txtCodigo (un TextBox que contiene el código del producto a buscar)
            //es un número entero válido. Si no lo es, muestra un mensaje y finaliza el método.
            if (!int.TryParse(txtCodigo.Text, out idProducto))
            {
                MessageBox.Show(" Ingrese un ID de producto válido (número entero).");
                return;
            }

            //Asigna el código del producto (idProducto) al objeto productos para que sea utilizado en la búsqueda
            productos.codigo = idProducto;

            //Crea una instancia de la clase ConexionBD para acceder a la base de datos
            ConexionBD conexion = new ConexionBD();

            try
            {
                //Llama al  listarProductosPorCodigo, filtra los productos en la base de datos con el código ingresado
                //y los muestra en el dgvProductos 
                conexion.listarProductosPorCodigo(dgvProductos, productos);


                // Mostrar mensaje si no se encuentra el producto
                if (dgvProductos.Rows.Count == 0)
                {
                    MessageBox.Show("No se encontró ningún producto con este código ingresado.");
                }

               //muestra un mensaje de error con detalles
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error buscando producto: " + ex.Message);
            }
        }

        private void Buscar_Producto_Load(object sender, EventArgs e)
        {

        }
    }
}
