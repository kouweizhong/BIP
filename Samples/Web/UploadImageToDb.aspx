﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UploadImageToDb.aspx.cs" Inherits="Wmb.TestWeb.UploadImageToDb" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        Select an image to upload: <asp:FileUpload ID="FileUpload1" runat="server" /><br/>
        <asp:Button ID="Button1" runat="server" Text="Upload" onclick="Button1_Click" />
    </div>
    </form>
</body>
</html>