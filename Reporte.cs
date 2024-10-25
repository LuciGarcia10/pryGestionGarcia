using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace pryGestionGarcia
{
    public partial class Reporte : Form
    {
        public Reporte()
        {
            //Llama a un método que inicializa los componentes de la interfaz de usuario.
            //Esto es típico en aplicaciones de Windows Forms para configurar controles como botones, cuadros de texto, gráficos, etc.
            InitializeComponent();
            //Llama a otro método definido en esta clase que carga los datos del gráfico de inventario
            CargarDatosGrafico();
            //Crea una nueva instancia de la clase ConexionBD, que probablemente se encarga de manejar la conexión a la base de datos
            ConexionBD conexion = new ConexionBD();
            //argar productos que están por debajo de un cierto umbral de stock en lstControl
            conexion.CargarProductosBajoStock(lstControl);
        }

        private void Reporte_Load(object sender, EventArgs e)
        {

        }

        private void CargarDatosGrafico()
        {
            ConexionBD conexion = new ConexionBD();
            // Obtener los datos de la base de datos
            DataTable dt = conexion.ObtenerDatosInventario();

            // Crear el gráfico y configurarlo
            chartInventario.Series.Clear();
            //Crea una nueva serie de datos para el gráfico
            Series serie = new Series();
            serie.ChartType = SeriesChartType.Column; // Puedes cambiar el tipo de gráfico
            serie.Name = "Inventario";            //Asigna un nombre a la serie, que puede ser utilizado para referencias y leyendas
            chartInventario.Series.Add(serie);    //Agrega la serie al gráfico

            //Itera a través de cada fila de datos en la tabla dt.
            foreach (DataRow row in dt.Rows)
            {
                //Agrega un punto a la serie para cada producto, usando su nombre como el eje X y su stock como el eje Y
                serie.Points.AddXY(row["Nombre"], row["Stock"]);
            }

            // Personalizar el gráfico
            chartInventario.Titles.Add("Reporte de Inventario");       //Establece un título para el gráfico
            chartInventario.ChartAreas[0].AxisX.Title = "Producto";    //Establece un titulo del eje X
            chartInventario.ChartAreas[0].AxisY.Title = "Cantidad";    //Establece un titulo del eje Y
        }

        private void buscarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Abre un formulario para buscar un producto.
            Buscar_Producto frm = new Buscar_Producto();
            frm.ShowDialog();
        }

        private void agregarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Abre un formulario para agregar un nuevo producto
            Agregar_producto frm = new Agregar_producto();
            frm.ShowDialog();
        }

        private void modificarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Abre un formulario para modificar un producto existente
            Modifica_producto frm = new Modifica_producto();
            frm.ShowDialog();
        }

        private void eliminarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Abre un formulario para eliminar un producto
            Eliminar_Producto frm = new Eliminar_Producto();
            frm.ShowDialog();
        }
    }
}
