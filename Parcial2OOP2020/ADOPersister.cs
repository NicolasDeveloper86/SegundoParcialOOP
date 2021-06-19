/*******
* Apellido y Nombre: Mendez Nicolas
********/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace Parcial2OOP2020
{
    public class ADOPersister : IPersistible
    {
        /// <summary>
        /// Utilice esta propiedad para conectarse a la base de datos en 
        /// cada uno de los métodos.
        /// </summary>
        public static string ConnectionString { set; get; }

        public List<Cliente> SearchByApellido(string partOfApellido)
        {
            List<Cliente> clientes = new List<Cliente>();

            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.Connection.Open();
                    sqlCommand.CommandText = @"SELECT * FROM Clientes WHERE Apellido LIKE '%' + @apellido + '%'";

                    sqlCommand.Parameters.AddWithValue("@apellido", partOfApellido);

                    SqlDataAdapter dataAdapter = new SqlDataAdapter();
                    dataAdapter.SelectCommand = sqlCommand;
                    DataTable table = new DataTable();
                    dataAdapter.Fill(table);

                    if (table.Rows.Count == 0)
                    {
                        throw new ClienteNoEncontradoException(partOfApellido);
                    }

                    foreach (DataRow row in table.Rows)
                    {
                        Cliente cliente = new Cliente
                        {
                            Id = Convert.ToInt32(row["ID"]),
                            Apellido = row["Apellido"].ToString(),
                            Deuda = Convert.ToInt32(row["Deuda"]),
                            DNI = Convert.ToInt32(row["DNI"])
                        };

                        clientes.Add(cliente);
                    }
                }
            }

            return clientes;

        }

        public Cliente Get(int id)
        {
            Cliente cliente = new Cliente();

            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.Connection.Open();
                    sqlCommand.CommandText = @"SELECT * FROM Clientes WHERE Id = @Id";

                    sqlCommand.Parameters.AddWithValue("@Id", id);

                    SqlDataAdapter dataAdapter = new SqlDataAdapter
                    {
                        SelectCommand = sqlCommand
                    };

                    DataTable table = new DataTable();
                    dataAdapter.Fill(table);

                    if (table.Rows.Count == 0)
                    {
                        throw new ClienteNoEncontradoException(id.ToString());
                    }

                    cliente = new Cliente
                    {
                        Id = Convert.ToInt32(table.Rows[0]["ID"]),
                        Apellido = table.Rows[0]["Apellido"].ToString(),
                        Deuda = Convert.ToInt32(table.Rows[0]["Deuda"]),
                        DNI = Convert.ToInt32(table.Rows[0]["DNI"])
                    };


                }
            }

            return cliente;
        }

        public void Remove(int id)
        {
            Cliente clienteARemover = Get(id);

            if (clienteARemover.Deuda > 0)
            {
                throw new ClienteConDeudaException(clienteARemover.Deuda.ToString());
            }

            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.Connection.Open();
                    sqlCommand.CommandText = @"DELETE Clientes WHERE ID = @Id";

                    sqlCommand.Parameters.AddWithValue("@Id", id);

                    int recordsAffected = sqlCommand.ExecuteNonQuery();

                    if (recordsAffected != 1)
                    {
                        throw new ClienteNoEncontradoException(id.ToString());
                    }
                }
            }
        }

        public void Remove(Cliente cliente)
        {
            if (cliente == null)
            {
                throw new ArgumentNullException();
            }

            if (cliente.Id == null)
            {
                throw new ClienteIdIsNullException();
            }

            Remove(cliente.Id.Value);
        }

        public List<Exception> Insert(List<Cliente> Clientes)
        {
            List<Exception> errors = new List<Exception>();
            if (Clientes != null)
            {
                foreach (Cliente cliente in Clientes)
                {
                    try
                    {
                        InsertOne(cliente);
                        errors.Add(null);
                    }
                    catch (Exception ex)
                    {
                        errors.Add(ex);
                    }
                }
            }
            return errors;
        }

        private void InsertOne(Cliente cliente)
        {
            SqlConnection sqlConnection = new SqlConnection(ConnectionString);
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.Connection = sqlConnection;
            sqlCommand.Connection.Open();
            sqlCommand.CommandText = @"INSERT INTO Clientes
                                    ([Apellido]
                                    ,[DNI]
                                    ,[Deuda])
                                    VALUES
                                        (@Apellido,
                                        @DNI,
                                        @Deuda)";

            sqlCommand.Parameters.AddWithValue("@Apellido", cliente.Apellido);
            sqlCommand.Parameters.AddWithValue("@DNI", cliente.DNI);
            sqlCommand.Parameters.AddWithValue("@Deuda", cliente.Deuda);

            sqlCommand.ExecuteNonQuery();

            sqlCommand.CommandText = "Select @@IDENTITY";
            sqlCommand.Parameters.Clear();

            object objID = sqlCommand.ExecuteScalar();

            int id = Convert.ToInt32(objID);

            cliente.Id = id;

            sqlCommand.Connection.Close();
        }

    }
}