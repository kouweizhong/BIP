<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SlideShow.aspx.cs" EnableViewState="false" Inherits="Wmb.TestWeb.SlideShow" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Slide Show Sample</title>
    <link rel="stylesheet" type="text/css" href="ClientScript/fancybox/jquery.fancybox.css" media="screen" />
    <script type="text/javascript" src="ClientScript/jquery-1.3.2.min.js" ></script>
    <script type="text/javascript" src="ClientScript/jquery.easing.1.3.js" ></script>
    <script type="text/javascript" src="ClientScript/fancybox/jquery.fancybox-1.2.1.js"></script>
    <script type="text/javascript">
        $(function() {
            $("a.ImageLink").fancybox();
        });
	</script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="content" style="width:250px;">
        <asp:ListView ID="ListView1" runat="server">
            <LayoutTemplate><asp:PlaceHolder runat="server" ID="itemPlaceholder" /></LayoutTemplate>
            <ItemTemplate><wmb:ImageLink class="ImageLink" rel="SampleGallery" NavigateUrl='<%# Eval("Path") %>' ToolTip='<%# Eval("ToolTip") %>' runat="server">
                    <wmb:BetterImage ID="BetterImage1" ImageUrl='<%# Eval("Path") %>' runat="server">
                        <ImageSettings
                            MaxHeight="80"
                            MaxWidth="80"
                            Clip="true" />
                    </wmb:BetterImage>
                </wmb:ImageLink>
            </ItemTemplate>
        </asp:ListView>
    </div>
    </form>
</body>
</html>