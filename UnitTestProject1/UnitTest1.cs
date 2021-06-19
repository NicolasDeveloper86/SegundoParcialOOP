using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Data.Linq.SqlClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Parcial2OOP2020;
using UnitTestProject1.DAL;
using System.Data.SqlClient;

namespace UnitTestcliXject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestInitialize]
        public void TestInitialize()
        {           
            ClientesDbContext context = new ClientesDbContext();
            context.Database.ExecuteSqlCommand("DELETE FROM Clientes");

            string dataSource = context.Database.Connection.DataSource;
            string initialCatalog = context.Database.Connection.Database;

            string connectionString = @"Data Source = {0}; 
                                      Initial Catalog = {1};
                                      Integrated Security = True;";

            connectionString = string.Format(connectionString, dataSource, initialCatalog);

            ADOPersister.ConnectionString = connectionString;
        }

        [TestMethod]
        public void Insert_00()
        {
            //Arrange            
            List<Cliente> Clientes = null;
            
            //Act
            IPersistible adoPersister = new ADOPersister();
            List<Exception> errores = adoPersister.Insert(Clientes);

            //Assert            
            Assert.AreEqual(0, errores.Count);            
        }

        [TestMethod]
        public void Insert_01()
        {
            //Arrange
            Cliente cliXA = new Cliente();
            cliXA.Apellido = Guid.NewGuid().ToString();
            cliXA.DNI = new Random().Next(1,1000);
            cliXA.Deuda = new Random().Next(1, 1000);

            Cliente cliXB = new Cliente();
            cliXB.Apellido = Guid.NewGuid().ToString();
            cliXB.DNI = cliXA.DNI + 10;
            cliXB.Deuda = new Random().Next(1, 1000);

            List<Cliente> Clientes = new List<Cliente>();
            Clientes.Add(cliXA);
            Clientes.Add(cliXB);

            //Act
            IPersistible adoPersister = new ADOPersister();
            List<Exception> errores = adoPersister.Insert(Clientes);

            //Assert            
            Assert.AreEqual(2, errores.Count);
            Assert.AreEqual(null, errores[0]);
            Assert.AreEqual(null, errores[1]);
        }

        [TestMethod]
        public void Insert_02()
        {
            //Arrange
            Cliente cliXA = new Cliente();
            cliXA.Apellido = Guid.NewGuid().ToString();
            cliXA.DNI = new Random().Next(1, 1000);
            cliXA.Deuda = new Random().Next(1, 100);

            Cliente cliXB = new Cliente();
            cliXB.Apellido = Guid.NewGuid().ToString();
            cliXB.DNI = cliXA.DNI + 10;
            cliXB.Deuda = new Random().Next(1, 100);

            List<Cliente> clientes = new List<Cliente>();
            clientes.Add(cliXA);
            clientes.Add(cliXB);

            //Act
            IPersistible adoPersister = new ADOPersister();
            List<Exception> errores = adoPersister.Insert(clientes);

            //Assert   
            ClientesDbContext context = new ClientesDbContext();
            
            Assert.AreEqual(2, context.Clientes.Count());
            int contA = context.Clientes.Count(p => p.Apellido == cliXA.Apellido);
            int contB = context.Clientes.Count(p => p.Apellido == cliXB.Apellido);

            Assert.AreEqual(1, contA);
            Assert.AreEqual(1, contB);
        }

        [TestMethod]
        public void Insert_03()
        {
            //Arrange
            Cliente cliXA = new Cliente();
            cliXA.Apellido = Guid.NewGuid().ToString();
            cliXA.DNI = new Random().Next(1, 1000);
            cliXA.Deuda = new Random().Next(1, 100);

            List<Cliente> clientes = new List<Cliente>();
            clientes.Add(cliXA);

            //Act
            IPersistible adoPersister = new ADOPersister();
            List<Exception> errores = adoPersister.Insert(clientes);

            //Assert   
            ClientesDbContext context = new ClientesDbContext();

            Assert.AreEqual(1, context.Clientes.Count());
            int contA = context.Clientes.Count(p => p.Apellido == cliXA.Apellido);
            
            Assert.AreEqual(1, contA);            
        }

        [TestMethod]
        public void Insert_04()
        {
            //Arrange
            Cliente clienteX = new Cliente();
            clienteX.Apellido = Guid.NewGuid().ToString();
            clienteX.DNI = new Random().Next(1, 1000);
            clienteX.Deuda = new Random().Next(1, 100);

            List<Cliente> clientes = new List<Cliente>();
            clientes.Add(clienteX);
            clientes.Add(clienteX);
            clientes.Add(clienteX);

            //Act
            IPersistible adoPersister = new ADOPersister();
            List<Exception> errores = adoPersister.Insert(clientes);

            //Assert   
            ClientesDbContext context = new ClientesDbContext();

            Assert.AreEqual(1, context.Clientes.Count());
            int contA = context.Clientes.Count(p => p.Apellido == clienteX.Apellido);
            Assert.AreEqual(1, contA);

            Assert.AreEqual(3, errores.Count);
            Assert.IsNull(errores[0]);
            Assert.IsNotNull(errores[1]);
            Assert.IsNotNull(errores[2]);
        }

        [TestMethod]
        public void Insert_05()
        {
            //Arrange
            Cliente clienteA = new Cliente();
            clienteA.Apellido = Guid.NewGuid().ToString();
            clienteA.DNI = new Random().Next(1, 1000);
            clienteA.Deuda = new Random().Next(1, 100);

            Cliente clienteB = new Cliente();
            clienteB.Apellido = Guid.NewGuid().ToString();
            clienteB.DNI = clienteA.DNI + 1;
            clienteB.Deuda = new Random().Next(1, 100);

            List<Cliente> Clientes = new List<Cliente>();
            Clientes.Add(clienteA); //Se inserta correctamente
            Clientes.Add(clienteA); //El DNI ya existe, no se inserta en la base.
            Clientes.Add(clienteA); //El DNI ya existe, no se inserta en la base.
            Clientes.Add(clienteB); //Se inserta correctamente

            //Act
            IPersistible adoPersister = new ADOPersister();
            List<Exception> errores = adoPersister.Insert(Clientes);

            //Assert   
            ClientesDbContext context = new ClientesDbContext();

            Assert.IsNull(errores[0]);
            Assert.IsTrue(errores[1].Message.Contains("UniqueDNI_sin_repetidos"));
            Assert.IsTrue(errores[2].Message.Contains("UniqueDNI_sin_repetidos"));
            Assert.IsNull(errores[3]);
        }

        [TestMethod]
        public void Insert_06()
        {
            //Arrange
            Cliente cliXA = new Cliente();
            cliXA.Apellido = Guid.NewGuid().ToString();
            cliXA.DNI = new Random().Next(1, 1000);
            cliXA.Deuda = new Random().Next(1, 100);

            List<Cliente> Clientes = new List<Cliente>();
            Clientes.Add(cliXA);
            Clientes.Add(cliXA);
            Clientes.Add(cliXA);

            //Act
            IPersistible adoPersister = new ADOPersister();
            List<Exception> errores = adoPersister.Insert(Clientes);

            //Assert   
            ClientesDbContext context = new ClientesDbContext();

            Assert.AreEqual(2601, ((SqlException)errores[1]).Number);
            Assert.AreEqual(2601, ((SqlException)errores[2]).Number);
        }

        [TestMethod]
        public void Insert_07()
        {
            //Arrange
            Cliente cliXA = new Cliente();
            cliXA.Id = null;
            cliXA.Apellido = Guid.NewGuid().ToString();
            cliXA.DNI = new Random().Next(1, 1000);
            cliXA.Deuda = new Random().Next(1, 1000);

            Cliente cliXB = new Cliente();
            cliXB.Id = null;
            cliXB.Apellido = Guid.NewGuid().ToString();
            cliXB.DNI = cliXA.DNI + 10;
            cliXB.Deuda = new Random().Next(1, 1000);

            List<Cliente> clientes = new List<Cliente>();
            clientes.Add(cliXA);
            clientes.Add(cliXB);

            //Act
            IPersistible adoPersister = new ADOPersister();
            List<Exception> errores = adoPersister.Insert(clientes);

            //Assert        
            ClientesDbContext context = new ClientesDbContext();
            int idClienteA = context.Clientes.First(c => c.Apellido == cliXA.Apellido).ID;
            int idClienteB = context.Clientes.First(c => c.Apellido == cliXB.Apellido).ID;
            Assert.AreEqual(idClienteA, cliXA.Id.Value);
            Assert.AreEqual(idClienteB, cliXB.Id.Value);
        }

        [TestMethod]
        public void Remove_01()
        {
            //Arrange
            ClienteEF cliX = new ClienteEF();
            cliX.Apellido = Guid.NewGuid().ToString();
            cliX.DNI = new Random().Next(1, 1000);
            cliX.Deuda = 0;  //Sin deuda

            ClientesDbContext context = new ClientesDbContext();
            context.Clientes.Add(cliX);
            context.SaveChanges();

            //Act
            IPersistible adoPersister = new ADOPersister();
            adoPersister.Remove(cliX.ID);

            //Assert
            context = new ClientesDbContext();            
            Assert.AreEqual(0, context.Clientes.Count());
        }

        [TestMethod]
        public void Remove_02()
        {
            //Arrange
            ClienteEF cliX = new ClienteEF();
            cliX.Apellido = Guid.NewGuid().ToString();
            cliX.DNI = new Random().Next(1, 1000);
            cliX.Deuda = 0;  //Sin deuda

            ClientesDbContext context = new ClientesDbContext();
            context.Clientes.Add(cliX);
            context.SaveChanges();

            Cliente cliente = new Cliente();
            cliente.Id = cliX.ID;
            cliente.Apellido = cliX.Apellido;
            cliente.DNI = cliX.DNI;
            cliente.Deuda = 0;  //Sin deuda

            //Act
            IPersistible adoPersister = new ADOPersister();
            adoPersister.Remove(cliente);

            //Assert
            context = new ClientesDbContext();
            Assert.AreEqual(0, context.Clientes.Count());
        }

        [ExpectedException(typeof(ClienteNoEncontradoException))]
        [TestMethod]
        public void Remove_03()
        {
            //Arrange
            ClienteEF cliX = new ClienteEF();
            cliX.Apellido = Guid.NewGuid().ToString();
            cliX.DNI = new Random().Next(1, 10000);
            cliX.Deuda = new Random().Next(1, 100);

            ClientesDbContext context = new ClientesDbContext();
            context.Clientes.Add(cliX);
            context.SaveChanges();

            //Act
            IPersistible adoPersister = new ADOPersister();
            adoPersister.Remove(-9999); //EL ID suministrado no existe

            //Assert            
        }

        [TestMethod]
        public void Remove_04()
        {
            //Arrange
            
            //Act
            ClienteNoEncontradoException exception = null;
            try
            {
                IPersistible adoPersister = new ADOPersister();
                adoPersister.Remove(-31416);
            }
            catch (ClienteNoEncontradoException error)
            {
                exception = error;
            }

            //Assert
            Assert.IsTrue(exception.Message.Contains("-31416"));
        }

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void Remove_05()
        {
            //Arrange

            //Act            
            IPersistible adoPersister = new ADOPersister();
            adoPersister.Remove(cliente: null);
            
            //Assert            
        }

        [ExpectedException(typeof(ClienteIdIsNullException))]
        [TestMethod]
        public void Remove_06()
        {
            //Arrange
            Cliente cliente = new Cliente();
            cliente.Id = null;

            //Act            
            IPersistible adoPersister = new ADOPersister();
            adoPersister.Remove(cliente);

            //Assert            
        }

        [ExpectedException(typeof(ClienteConDeudaException))]
        [TestMethod]
        public void Remove_07()
        {
            //Arrange
            //El cliente tiene deuda entonces, no puede ser eliminado
            ClienteEF cliDeudorAux = new ClienteEF();
            cliDeudorAux.Deuda = 10;
            cliDeudorAux.Apellido = new Guid().ToString();
            cliDeudorAux.DNI = DateTime.Now.Millisecond;

            ClientesDbContext context = new ClientesDbContext();
            context.Clientes.Add(cliDeudorAux);
            context.SaveChanges();

            Cliente cliDeudor = new Cliente();
            cliDeudor.Id = cliDeudorAux.ID;

            //Act            
            IPersistible adoPersister = new ADOPersister();
            adoPersister.Remove(cliDeudor);

            //Assert            
        }

        [ExpectedException(typeof(ClienteConDeudaException))]
        [TestMethod]
        public void Remove_08()
        {
            //Arrange
            //El cliente tiene deuda entonces, no puede ser eliminado
            ClienteEF cliDeudorAux = new ClienteEF();
            cliDeudorAux.Deuda = 10;
            cliDeudorAux.Apellido = new Guid().ToString();
            cliDeudorAux.DNI = DateTime.Now.Millisecond;

            ClientesDbContext context = new ClientesDbContext();
            context.Clientes.Add(cliDeudorAux);
            context.SaveChanges();

            //Act            
            IPersistible adoPersister = new ADOPersister();
            adoPersister.Remove(cliDeudorAux.ID);

            //Assert            
        }
        
        [TestMethod]
        public void Remove_09()
        {
            //Arrange
            //El cliente tiene deuda entonces, no puede ser eliminado
            ClienteEF cliDeudorAux = new ClienteEF();
            cliDeudorAux.Deuda = new Random().Next(1,1000);
            cliDeudorAux.Apellido = new Guid().ToString();
            cliDeudorAux.DNI = DateTime.Now.Millisecond;

            ClientesDbContext context = new ClientesDbContext();
            context.Clientes.Add(cliDeudorAux);
            context.SaveChanges();

            Cliente cliDeudor = new Cliente();
            cliDeudor.Id = cliDeudorAux.ID;

            //Act
            string msgError = "";
            try
            {
                IPersistible adoPersister = new ADOPersister();
                adoPersister.Remove(cliDeudor);
            }
            catch(Exception ex)
            {
                msgError = ex.Message;
            }

            //Assert            
            Assert.IsTrue(msgError.Contains(cliDeudorAux.Deuda.ToString()));
        }

        [TestMethod]
        public void Get_01()
        {
            //Arrange
            ClienteEF cliXEF = new ClienteEF();
            cliXEF.Apellido = Guid.NewGuid().ToString();
            cliXEF.DNI = new Random().Next(1, 10000);
            cliXEF.Deuda = new Random().Next(1, 100);

            ClientesDbContext context = new ClientesDbContext();
            context.Clientes.Add(cliXEF);
            context.SaveChanges();

            //Act
            IPersistible adoPersister = new ADOPersister();
            Cliente clienteByLoad = adoPersister.Get(cliXEF.ID);

            //Assert
            context = new ClientesDbContext();
            Assert.AreEqual(1, context.Clientes.Count());

            ClienteEF clienteEF = context.Clientes.First();
            Assert.AreEqual(cliXEF.ID, clienteByLoad.Id.Value);
            Assert.AreEqual(cliXEF.Apellido, clienteByLoad.Apellido);
            Assert.AreEqual(cliXEF.DNI, clienteByLoad.DNI);
            Assert.AreEqual(cliXEF.Deuda, clienteByLoad.Deuda);
        }

        [ExpectedException(typeof(ClienteNoEncontradoException))]
        [TestMethod]
        public void Get_02()
        {
            //Arrange
            
            //Act
            IPersistible adoPersister = new ADOPersister();
            Cliente clienteByLoad = adoPersister.Get(-2131);

            //Assert            
        }

        [TestMethod]
        public void Get_03()
        {
            //Arrange

            //Act
            ClienteNoEncontradoException exception = null;
            try
            {
                IPersistible adoPersister = new ADOPersister();
                Cliente clienteByLoad = adoPersister.Get(-98765);
            }
            catch (ClienteNoEncontradoException cliXNoEncontradoException)
            {
                exception = cliXNoEncontradoException;
            }

            //Assert
            Assert.IsTrue(exception.Message.Contains("-98765"));
        }

        [TestMethod]
        public void SearchByApellido_01()
        {
            //Arrange
            ClienteEF cliXA = new ClienteEF();
            cliXA.Apellido = "Sadosky";
            cliXA.DNI = new Random().Next(1, 100);
            cliXA.Deuda = new Random().Next(1, 100);

            ClienteEF cliXB = new ClienteEF();
            cliXB.Apellido = "Favaloro";
            cliXB.DNI = cliXA.DNI + 1;
            cliXB.Deuda = new Random().Next(1, 100);

            ClientesDbContext context = new ClientesDbContext();
            context.Clientes.Add(cliXA);
            context.Clientes.Add(cliXB);
            context.SaveChanges();

            //Act
            IPersistible adoPersister = new ADOPersister();
            List<Cliente> clientes = adoPersister.SearchByApellido("dos");
            
            //Assert          
            Assert.AreEqual(1, clientes.Count());

            Assert.AreEqual(cliXA.ID, clientes[0].Id.Value);
            Assert.AreEqual(cliXA.Apellido, clientes[0].Apellido);
            Assert.AreEqual(cliXA.DNI, clientes[0].DNI);
            Assert.AreEqual(cliXA.Deuda, clientes[0].Deuda);
        }

        [TestMethod]
        public void SearchByApellido_02()
        {
            //Arrange
            ClienteEF cliXA = new ClienteEF();
            cliXA.Apellido = "aaaaaazazabbbbbbbbbbbbb";
            cliXA.DNI = 112345;
            cliXA.Deuda = new Random().Next(1, 100);

            ClienteEF cliXB = new ClienteEF();
            cliXB.Apellido = "xxxzazazzzzzzzzzz";
            cliXB.DNI = 232423;
            cliXB.Deuda = new Random().Next(1, 100);

            ClienteEF cliXC = new ClienteEF();
            cliXC.Apellido = Guid.NewGuid().ToString();
            cliXC.DNI = 2433; 
            cliXC.Deuda = new Random().Next(1, 100);

            ClientesDbContext context = new ClientesDbContext();
            context.Clientes.Add(cliXA);
            context.Clientes.Add(cliXB);
            context.Clientes.Add(cliXC);
            context.SaveChanges();

            //Act
            IPersistible adoPersister = new ADOPersister();
            List<Cliente> clientesEncintrados = adoPersister.SearchByApellido("zaza");

            //Assert          
            Assert.AreEqual(cliXA.ID + cliXB.ID, clientesEncintrados[0].Id + clientesEncintrados[1].Id);
            Assert.AreEqual(2, clientesEncintrados.Count());
        }

        [ExpectedException(typeof(ClienteNoEncontradoException))]
        [TestMethod]
        public void SearchByApellido_03()
        {
            //Arrange
            ClienteEF cliXA = new ClienteEF();
            cliXA.Apellido = Guid.NewGuid().ToString();
            cliXA.DNI = 111;
            cliXA.Deuda = new Random().Next(1, 100);

            ClienteEF cliXB = new ClienteEF();
            cliXB.Apellido = Guid.NewGuid().ToString();
            cliXB.DNI = 2222;
            cliXB.Deuda = new Random().Next(1, 100);

            ClienteEF cliXC = new ClienteEF();
            cliXC.Apellido = Guid.NewGuid().ToString();
            cliXC.DNI = 3333;
            cliXC.Deuda = new Random().Next(1, 100);

            ClientesDbContext context = new ClientesDbContext();
            context.Clientes.Add(cliXA);
            context.Clientes.Add(cliXB);
            context.Clientes.Add(cliXC);
            context.SaveChanges();

            //Act
            IPersistible adoPersister = new ADOPersister();
            List<Cliente> ClientesConA = adoPersister.SearchByApellido(Guid.NewGuid().ToString());

            //Assert                      
        }

        [TestMethod]
        public void SearchByApellido_04()
        {
            //Arrange
            ClienteEF cliXA = new ClienteEF();
            cliXA.Apellido = Guid.NewGuid().ToString();
            cliXA.DNI = new Random().Next(1, 1000);
            cliXA.Deuda = new Random().Next(1, 100);

            ClienteEF cliXB = new ClienteEF();
            cliXB.Apellido = Guid.NewGuid().ToString();
            cliXB.DNI = cliXA.DNI + 1;
            cliXB.Deuda = new Random().Next(1, 100);

            ClienteEF cliXC = new ClienteEF();
            cliXC.Apellido = Guid.NewGuid().ToString();
            cliXC.DNI = cliXA.DNI + 2;
            cliXC.Deuda = new Random().Next(1, 100);

            ClientesDbContext context = new ClientesDbContext();
            context.Clientes.Add(cliXA);
            context.Clientes.Add(cliXB);
            context.Clientes.Add(cliXC);
            context.SaveChanges();

            //Act
            ClienteNoEncontradoException exception = null;
            try
            {
                IPersistible adoPersister = new ADOPersister();
                List<Cliente> ClientesConA = adoPersister.SearchByApellido("aaaaa");
            }
            catch(ClienteNoEncontradoException cliXNoEncontradoException)
            {
                exception = cliXNoEncontradoException;
            }

            //Assert
            Assert.IsTrue(exception.Message.Contains("aaaaa"));
        }
    }
}
