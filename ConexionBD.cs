using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.CodeDom.Compiler;
using System.Windows.Forms.DataVisualization.Charting;
using System.Collections;
using System.Security.Cryptography;

namespace pryGestionGarcia
{
    public class ConexionBD
    {
        //Se declaran las variables conexion, comando y adaptador para manejar la conexión a la base de datos,
        //los comandos SQL y el adaptador de datos
        OleDbConnection conexion;
        OleDbCommand comando;
        OleDbDataAdapter adaptador;

        //cadena almacena la cadena de conexión para acceder a la base de datos de Access
        string cadena;

        //Constructor de la clase ConexionBD que inicializa cadena con la ruta de la base de datos ProductosGarcia.accdb.
        public ConexionBD()
        {
            cadena = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source= ProductosGarcia.accdb";
        }

        public void listarProductos(DataGridView dgvProductos)
        {
            try
            {
                // Intenta conectar y traer los productos
                conexion = new OleDbConnection(cadena);
                comando = new OleDbCommand();

                comando.Connection = conexion;
                comando.CommandType = CommandType.Text;
                comando.CommandText = "SELECT * FROM Productos";// Selecciona todos los productos.  

                DataTable tablaProductos = new DataTable(); // Crea un DataTable para almacenar los datos.

                adaptador = new OleDbDataAdapter(comando);// Adaptador para llenar el DataTable
                adaptador.Fill(tablaProductos); // Llenar el DataTable con datos de la base de datos

                dgvProductos.DataSource = tablaProductos; // Asignar el DataTable al DataGridView para mostrar los productos.
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void listarProductosPorCodigo(DataGridView dgvProductos, Productos idProducto)
        {
            //Usa la instrucción using para inicializar un objeto OleDbConnection llamado conexion,
            //que se conecta a la base de datos usando la cadena de conexión cadena.
            //La instrucción using garantiza que la conexión se cierre automáticamente después de que el bloque termine
            using (OleDbConnection conexion = new OleDbConnection(cadena))
            {
                //Se crea un objeto OleDbCommand llamado comando. Este comando se usará para ejecutar una consulta SQL en la base de datos
                using (OleDbCommand comando = new OleDbCommand())
                {
                    //Dentro de un bloque try, se establece que:
                    //comando.Connection sea igual a conexion, asociando el comando con la conexión a la base de datos.
                    //comando.CommandType = CommandType.Text indica que el comando es una consulta SQL en forma de texto.
                    try
                    {
                        comando.Connection = conexion;
                        comando.CommandType = CommandType.Text;

                        //se establece en una consulta SQL que selecciona todos los campos de la tabla Productos
                        //donde el campo Codigo coincide con un valor de parámetro llamado @idProducto.
                        comando.CommandText = "SELECT * FROM Productos WHERE Codigo = @idProducto";
                        //agrega un parámetro al comando SQL, asignando @idProducto con el valor de idProducto.codigo.
                        //Esto evita el riesgo de inyección SQL al pasar parámetros en lugar de concatenar el valor directamente en la consulta.
                        comando.Parameters.AddWithValue("@idProducto", idProducto.codigo);

                        //Declara y crea una instancia de DataTable llamada tablaProductos
                        //que se usará para almacenar los resultados de la consulta.
                        DataTable tablaProductos = new DataTable();
                        //Se asigna un OleDbDataAdapter al comando, llamado adaptador.
                        //adaptador.Fill(tablaProductos) llena tablaProductos con los datos obtenidos de la consulta ejecutada en la base de datos.
                        adaptador = new OleDbDataAdapter(comando);
                        adaptador.Fill(tablaProductos);

                        // Limpiar el DataGridView antes de asignar los nuevos datos
                        //dgvProductos.Rows.Clear();
                        dgvProductos.DataSource = tablaProductos;


                        //Si ocurre un error durante la ejecución del comando, se captura en el bloque catch
                        //y se muestra un mensaje de error con ex.Message.
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);


                    } //El bloque finally asegura que conexion.Close() se ejecute siempre,
                      //cerrando la conexión a la base de datos para liberar recursos.
                    finally
                    {
                        conexion.Close();
                    }
                }
            }
        }

        public DataTable ObtenerDatosInventario()
        {
            //Declara y crea una instancia de DataTable llamada dt.
            //Este objeto servirá para almacenar los datos que se obtienen de la base de datos.
            DataTable dt = new DataTable();
            //Declara una cadena consulta que contiene la instrucción SQL. En este caso,
            //se seleccionan los campos Nombre y Stock de la tabla Productos, obteniendo así el nombre y el stock de cada producto.
            string consulta = "SELECT Nombre, Stock FROM Productos";

            //Usando la instrucción using, se crea una instancia de OleDbConnection llamada conexion,
            //que se inicializa con la cadena de conexión cadena.
            //La instrucción using asegura que la conexión se cierre automáticamente cuando el bloque termine.
            using (OleDbConnection conexion = new OleDbConnection(cadena))
            {
              //Se crea un OleDbDataAdapter llamado adaptador con dos argumentos:
              //consulta: la consulta SQL a ejecutar.
              //conexion: la conexión a la base de datos.
              //Este adaptador servirá para ejecutar la consulta y transferir los datos al DataTable.
                using (OleDbDataAdapter adaptador = new OleDbDataAdapter(consulta, conexion))
                {
                    //Se usa adaptador.Fill(dt) para ejecutar la consulta y llenar el DataTable dt con los resultados obtenidos
                    //(los nombres y cantidades en stock de los productos)
                    adaptador.Fill(dt);
                }
            }

            //se retorna el DataTable dt, que contiene los datos de inventario de la base de datos
            return dt;
        }

        public void CargarProductosBajoStock(ListBox listBox)
        {
            //using para inicializar una instancia de OleDbConnection llamada conexion,
            //con la cadena de conexión cadena. using garantiza que la conexión se cierre automáticamente al final del bloque
            using (OleDbConnection conexion = new OleDbConnection(cadena))
            {
                //Se crea un objeto OleDbCommand llamado comando que se utilizará para ejecutar una consulta SQL en la base de datos
                using (OleDbCommand comando = new OleDbCommand())
                {
                    //comando.Connection = conexion establece la conexión que usará el comando.
                    //comando.CommandText define la consulta SQL que selecciona el Nombre y Stock de los productos cuya cantidad en Stock es menor que el valor de @umbral.
                    comando.Connection = conexion;
                    comando.CommandText = "SELECT Nombre, Stock FROM Productos WHERE Stock < @umbral";
                    //AddWithValue agrega el parámetro @umbral y le asigna el valor 51. Esto filtra los productos con stock inferior a 51.
                    comando.Parameters.AddWithValue("@umbral", 51); // Ajusta el umbral

                    //Abre la conexión con la base de datos para ejecutar la consulta
                    conexion.Open();

                    //Se crea un OleDbDataReader llamado reader que ejecuta el comando y permite leer los resultados de la consulta,
                    //fila por fila.
                    using (OleDbDataReader reader = comando.ExecuteReader())
                    {
                        //Antes de cargar nuevos datos, se limpia el ListBox para asegurar que no contenga elementos anteriores
                        listBox.Items.Clear();

                        //Un bucle while (reader.Read()) recorre cada fila de los resultados.
                        //reader.GetString(0) obtiene el valor del primer campo(Nombre) y reader.GetInt32(1) obtiene el valor del segundo campo(Stock).
                        //Los valores nombre y stock se concatenan en un formato amigable y se agregan al ListBox, con una etiqueta “(Bajo)” para indicar el bajo stock.
                        while (reader.Read())
                        {
                            string nombre = reader.GetString(0);
                            int stock = reader.GetInt32(1);
                            listBox.Items.Add($"{nombre} - Stock: {stock} (Bajo)");
                        }
                    }
                }
            }
        }

        public void llenarListbox(ListBox listBox, string nombre, string descripcion, double precio, int stock)
        {
            try
            {
                // Limpiar los elementos actuales del ListBox.
                listBox.Items.Clear();

                // Añadir el nuevo producto al ListBox.
                listBox.Items.Add($"{nombre} - {descripcion} - {precio:C} - Stock: {stock}");
            }
            catch (Exception ex)
            {
                // Mostrar mensaje en caso de error.
                MessageBox.Show(ex.Message);
            }
        }

        public void LlenarcmbCategorias(ComboBox cmbCategorias)
        {
            //Se utiliza un bloque try-catch para manejar posibles excepciones
            //Se crea una nueva conexión conexion usando la cadena de conexión cadena, que contiene la ruta a la base de datos
            //Se crea un OleDbCommand llamado comando para ejecutar consultas en la base de datos
            try
            {
                conexion = new OleDbConnection(cadena);
                comando = new OleDbCommand();

                //establece la conexión de la base de datos que se usará para ejecutar el comando
                comando.Connection = conexion;
                //define que el tipo de comando es texto
                comando.CommandType = CommandType.Text;
                //establece la consulta SQL para seleccionar categorías distintas de la tabla Productos
                comando.CommandText = "SELECT DISTINCT Categoria FROM Productos";

                //La conexión a la base de datos se abre para permitir la ejecución de la consulta
                conexion.Open();

                //El método limpia el ComboBox de categorías para eliminar cualquier dato que estuviera cargado previamente
                cmbCategorias.Items.Clear();

                //Un OleDbDataReader llamado reader ejecuta el comando y obtiene los resultados de la consulta.
                //Un bucle while (reader.Read()) recorre cada fila devuelta.
                //reader.GetString(0) obtiene el valor de la categoría y lo agrega a cmbCategorias usando cmbCategorias.Items.Add()
                using (OleDbDataReader reader = comando.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        cmbCategorias.Items.Add(reader.GetString(0));
                    }
                }

                //El bloque catch captura cualquier excepción que ocurra y muestra un mensaje de error con MessageBox.Show.
               // El bloque finally se usa para operaciones de limpieza, aunque aquí el cierre de conexión está comentado
               // (debería estar abierto al inicio del método y cerrado al final).
                            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                //conexion.Close();
            }
        }


        public void AgregarProducto(Productos producto)
        {
            //Se usa un bloque using para inicializar una instancia de OleDbConnection llamada conexion con la cadena de conexión cadena.
            //Este bloque garantiza que la conexión se cierre automáticamente al final.
            using (OleDbConnection conexion = new OleDbConnection(cadena))
            {
                //OleDbCommand llamado comando se utiliza para definir la instrucción SQL.
                using (OleDbCommand comando = new OleDbCommand())
                {
                    //establece la conexión que usará el comando.
                    comando.Connection = conexion;
                    //indica que el tipo de comando es una consulta SQL de texto.
                    comando.CommandType = CommandType.Text;
                    //contiene la instrucción SQL para insertar un nuevo registro en la tabla Productos, utilizando parámetros para cada columna
                    comando.CommandText = @"INSERT INTO Productos (Codigo, Nombre, Categoria, Descripcion, Precio, Stock)
                                    VALUES (@codigo,@nombre, @categoria, @descripcion, @precio, @stock)";

                    // Agregar los parámetros
                    comando.Parameters.AddWithValue("@codigo", producto.codigo);
                    comando.Parameters.AddWithValue("@nombre", producto.nombre);
                    comando.Parameters.AddWithValue("@categoria", producto.categoria);
                    comando.Parameters.AddWithValue("@descripcion", producto.descripcion);
                    comando.Parameters.AddWithValue("@precio", producto.precio);
                    comando.Parameters.AddWithValue("@stock", producto.stock);

                    try
                    {
                        conexion.Open();
                        comando.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Error al agregar el producto: " + ex.Message);
                    }
                }
            }
        }

        public void ModificarProductos(Productos producto)
        {

            using (OleDbConnection conexion = new OleDbConnection(cadena))
            {
                using (OleDbCommand comando = new OleDbCommand())
                {
                    comando.Connection = conexion;
                    comando.CommandType = CommandType.Text;
                    comando.CommandText
                         = @"UPDATE Productos
                                 SET Codigo = @nuevoCodigo,
                                     Nombre = @nuevoNombre,
                                     Descripcion = @nuevaDescripcion,
                                     Precio = @nuevoPrecio,
                                     Stock = @nuevoStock
                                 WHERE Codigo = @codigo"; // Modificamos la condición WHERE

                    // Agregar los parámetros
                    comando.Parameters.AddWithValue("@nuevoCodigo", producto.codigo);
                    comando.Parameters.AddWithValue("@nombre", producto.nombre);
                    comando.Parameters.AddWithValue("@descripcion", producto.descripcion);

                    comando.Parameters.AddWithValue("@precio", producto.precio);
                    comando.Parameters.AddWithValue("@stock", producto.stock);
                    comando.Parameters.AddWithValue("@codigo", producto.codigo);


                    try
                    {
                        conexion.Open();
                        int filasAfectadas = comando.ExecuteNonQuery();

                        if (filasAfectadas > 0)
                        {
                            MessageBox.Show("Producto modificado correctamente.");
                        }
                        else
                        {
                            MessageBox.Show("No se encontró ningún producto con el ID especificado.");
                        }

                        //Si ocurre un error al agregar el producto,
                        //se lanza una excepción con un mensaje que indica que ocurrió un error, junto con el mensaje detallado del error original (ex.Message).
                        //Al final del bloque using, se cierran automáticamente la conexión y el comando, liberando los recursos usados
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al modificar el producto: " + ex.Message);
                    }
                }
            }
        }

        public void EliminarProducto(int codigo)
        {
            //Se usa un bloque using para crear una instancia de OleDbConnection llamada conexion con la cadena de conexión cadena.
            //Este bloque garantiza que la conexión se cierre automáticamente al final del bloque using.
            using (OleDbConnection conexion = new OleDbConnection(cadena))
            {
                //Se crea un objeto OleDbCommand llamado comando para definir la instrucción SQL
                using (OleDbCommand comando = new OleDbCommand())
                {
                    //establece que este comando usará la conexión conexion
                    comando.Connection = conexion;
                    //indica que la consulta es de tipo texto SQL.
                    comando.CommandType = CommandType.Text;
                    //contiene la consulta SQL DELETE, que eliminará un registro de la tabla Productos donde
                    //el campo Codigo coincida con el valor del parámetro @codigo
                    comando.CommandText = @"DELETE FROM Productos WHERE Codigo = @codigo";

                    // Agregar el parámetro
                    comando.Parameters.AddWithValue("@codigo", codigo);

                    try
                    {
                        //abre la conexión a la base de datos
                        conexion.Open();
                        //ejecuta la instrucción SQL y devuelve el número de filas afectadas. Si el valor es mayor a 0, significa que un producto fue eliminado
                        int filasAfectadas = comando.ExecuteNonQuery();

                        //Si filasAfectadas es mayor a 0, se muestra un mensaje indicando que el producto fue eliminado correctamente
                        //Si es 0, significa que no se encontró ningún producto con el código especificado y se muestra un mensaje de advertencia
                        //Si ocurre un error al ejecutar, mostrando un mensaje con el detalle del error 
                        if (filasAfectadas > 0)
                        {
                            MessageBox.Show("Producto eliminado correctamente.");
                        }
                        else
                        {
                            MessageBox.Show("No se encontró ningún producto con el ID especificado.");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al eliminar el producto: " + ex.Message);
                    }
                }
            }
        }
    }
}
