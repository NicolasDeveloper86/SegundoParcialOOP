<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebFormMain.aspx.cs" Inherits="WebAppPrueba.WebFormMain" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Button ID="ButtonAlta" runat="server" OnClick="ButtonAlta_Click" Text="Alta" />
            <asp:Label ID="LabelHora" runat="server" Text="????"></asp:Label>
            <br />
            <br />
            <asp:Label ID="Label2" runat="server" Text="ID:"></asp:Label>
            <asp:Label ID="LabelID" runat="server" Text="??"></asp:Label>
            <br />
            <asp:Label ID="Label1" runat="server" Text="Marca:"></asp:Label>
            <asp:TextBox ID="TextBoxMarca" runat="server"></asp:TextBox>
            <br />
            <asp:Label ID="Label3" runat="server" Text="Descripcion:"></asp:Label>
            <asp:TextBox ID="TextBoxDescripcion" runat="server"></asp:TextBox>
            <br />
            <asp:Label ID="Label4" runat="server" Text="Precio:"></asp:Label>
            <asp:TextBox ID="TextBoxPrecio" runat="server"></asp:TextBox>
            <br />
            <br />
            <br />
            <br />
            <asp:Label ID="Label5" runat="server" Text="ID:"></asp:Label>
            <asp:TextBox ID="TextBoxIDAEliminar" runat="server"></asp:TextBox>
            <asp:Button ID="ButtonEliminar" runat="server" OnClick="ButtonEliminar_Click" Text="Eliminar" />
        </div>
    </form>
</body>
</html>
