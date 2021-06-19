using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ClassLibraryADOPersister;

namespace WebAppPrueba
{
    public partial class WebFormMain : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void ButtonAlta_Click(object sender, EventArgs e)
        {
            LabelHora.Text = DateTime.Now.ToLongTimeString();

            Producto producto = new Producto();
            producto.Marca = TextBoxMarca.Text;
            producto.Descripcion = TextBoxDescripcion.Text;
            producto.Precio = Convert.ToDouble(TextBoxPrecio.Text);

            ADOPersister perister = new ADOPersister();
            perister.Save(producto);

            LabelID.Text = producto.Id.Value.ToString();

        }

        protected void ButtonEliminar_Click(object sender, EventArgs e)
        {

        }
    }
}