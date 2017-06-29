<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebApplication1._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>BetterImageProcessor</title>
    <meta http-equiv="Page-Exit" content="blendTrans(Duration=0.0)" />
    <link href="CSS/Default.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" type="text/css" href="ClientScript/fancybox/jquery.fancybox.css"
        media="screen" />

    <script type="text/javascript" src="ClientScript/jquery-1.3.2.min.js"></script>

    <script type="text/javascript" src="ClientScript/jquery.easing.1.3.js"></script>

    <script type="text/javascript" src="ClientScript/fancybox/jquery.fancybox-1.2.1.js"></script>

    <script type="text/javascript" src="ClientScript/jquery.cycle.all.min.js"></script>

    <script type="text/javascript">
        $(function() {
            $("a.fancyBox").fancybox();
            $("#cycle1").cycle({
                fx: 'shuffle',
                delay: -4000,
                pause: 1
            });
            $("#cycle2").cycle({
                fx: 'fade',
                delay: -4000,
                pause: 1
            });
            $("#cycle3").cycle({
                fx: 'scrollUp',
                delay: -4000,
                pause: 1
            });
        });
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager runat="server" />
    <div id="content">
        <h1>
            BetterImageProcessor</h1>
        <p>
            The best combination of server controls, and an imageprocessor which makes it possible
            to edit your images on the fly without the use of any code! You can create thumbnails,
            grayscales, negatives, sepias. You can adjust brightness, contrast and opacity.
            Have imagebuttons, imagelinks and imagecontrols showing images which are generated
            on the fly. All this - and more - by simply dragging-and-dropping one of these controls
            on your form.</p>
        <p>
            Don't forget it's absolutely 100% free so: Have fun and read on!</p>
        <div class="breaker">
            <wmb:BetterImage runat="server" ImageUrl="~/imgs/Sample.jpg" AlternateText="Resized and protected image. Copyright text is added on the fly, src of the image ecrypted and right mousebutton and deeplinking is disabled."
                ToolTip="Resized and protected image. Copyright text is added on the fly, src of the image ecrypted and right mousebutton and deeplinking is disabled.">
                <ImageSettings MaxWidth="150" MaxHeight="150" DisableRightClick="true" EncryptSrc="true"
                    Copyright="Copyright to protect images" />
            </wmb:BetterImage>
            <wmb:BetterImage runat="server" ImageUrl="~/imgs/Sample.jpg" AlternateText="Grayscaled"
                ToolTip="Grayscaled">
                <ImageSettings MaxWidth="150" MaxHeight="150" Grayscale="true" />
            </wmb:BetterImage>
            <wmb:BetterImage runat="server" ImageUrl="~/imgs/Sample.jpg" AlternateText="Sepia"
                ToolTip="Sepia">
                <ImageSettings MaxWidth="150" MaxHeight="150" Sepia="true" />
            </wmb:BetterImage>
            <wmb:BetterImage runat="server" ImageUrl="~/imgs/Sample.jpg" AlternateText="Negative"
                ToolTip="Negative">
                <ImageSettings MaxWidth="150" MaxHeight="150" Negative="true" />
            </wmb:BetterImage>
        </div>
        <h2>
            Why, what and how...</h2>
        <p>
            I'm being asked to create a website by friends of mine pretty often. Problem is
            that neither of these people are webdevelopers and most of them don't know how to
            create thumbnails, let alone slideshows. Since I can't update their websites all
            the time and - let's be honest - I don't want to update their website all the time
            either, I was 'forced' to create a simple and cheap way of doing this stuff for
            them.</p>
        <p>
            So I started off with a piece of code that would resize an image to a thumbnail
            with decent quality. Intentionally to resize the images in an Upload folder.</p>
        <p>
            While I was working with it I noticed that it was doing it's job very fast. So I
            decided to make it implement the IHttpHandler interface to be able to create these
            thumbnails on the fly.</p>
        <p>
            When browsing GDI+ I saw a lot of features that were easy to implement so I did
            that as well.</p>
        <p>
            I ended up with a very sweet HttpHandler that could:</p>
        <ul>
            <li>Resize</li>
            <li>Create a grayscale</li>
            <li>Create a negative</li>
            <li>Create a sepia</li>
            <li>Set brightness</li>
            <li>Set contrast</li>
            <li>Quantize</li>
            <li>And many more...</li>
        </ul>
        <p>
            <strong>It was perfect!!! It did all this on the fly and very fast!!!</strong></p>
        <div class="breaker">
            <wmb:ImageLink runat="server" CssClass="fancyBox" rel="secondBreaker" NavigateUrl="~/imgs/Sample.jpg"
                ToolTip="Default" ImageSettings-MaxWidth="800" ImageSettings-MaxHeight="600">
                    <wmb:BetterImage runat="server" ImageUrl="~/imgs/Sample.jpg"
                        AlternateText="Default"
                        ToolTip="Default">
                        <ImageSettings
                            MaxHeight="100"
                            MaxWidth="100"
                            Clip="true" />
                    </wmb:BetterImage>
            </wmb:ImageLink>
            <wmb:ImageLink runat="server" CssClass="fancyBox" rel="secondBreaker" NavigateUrl="~/imgs/Sample.jpg"
                ToolTip="Lighter grayscaled" ImageSettings-MaxWidth="800" ImageSettings-MaxHeight="600"
                ImageSettings-Grayscale="true" ImageSettings-Brightness="0.2">
                    <wmb:BetterImage runat="server" ImageUrl="~/imgs/Sample.jpg"
                        AlternateText="Lighter grayscaled"
                        ToolTip="Lighter grayscaled">
                        <ImageSettings
                            MaxHeight="100"
                            MaxWidth="100"
                            Clip="true"
                            Grayscale="true"
                            Brightness="0.2" />
                    </wmb:BetterImage>
            </wmb:ImageLink>
            <wmb:ImageLink runat="server" CssClass="fancyBox" rel="secondBreaker" NavigateUrl="~/imgs/Sample.jpg"
                ToolTip="Adjusted brightness" ImageSettings-MaxWidth="800" ImageSettings-MaxHeight="600"
                ImageSettings-Brightness="0.5">
                    <wmb:BetterImage runat="server" ImageUrl="~/imgs/Sample.jpg"
                        AlternateText="Adjusted brightness"
                        ToolTip="Adjusted brightness">
                        <ImageSettings
                            MaxHeight="100"
                            MaxWidth="100"
                            Clip="true"
                            Brightness="0.5" />
                    </wmb:BetterImage>
            </wmb:ImageLink>
            <wmb:ImageLink runat="server" CssClass="fancyBox" rel="secondBreaker" NavigateUrl="~/imgs/Sample.jpg"
                ToolTip="Adjusted contrast" ImageSettings-MaxWidth="800" ImageSettings-MaxHeight="600"
                ImageSettings-Contrast="1.2">
                    <wmb:BetterImage runat="server" ImageUrl="~/imgs/Sample.jpg"
                        AlternateText="Adjusted contrast"
                        ToolTip="Adjusted contrast">
                        <ImageSettings
                            MaxHeight="100"
                            MaxWidth="100"
                            Clip="true"
                            Contrast="1.2" />
                    </wmb:BetterImage>
            </wmb:ImageLink>
            <wmb:ImageLink runat="server" CssClass="fancyBox" rel="secondBreaker" NavigateUrl="~/imgs/Sample.jpg"
                ToolTip="Lighter Sepia" ImageSettings-MaxWidth="800" ImageSettings-MaxHeight="600"
                ImageSettings-Sepia="true" ImageSettings-Brightness="0.2">
                    <wmb:BetterImage runat="server" ImageUrl="~/imgs/Sample.jpg"
                        AlternateText="Lighter Sepia"
                        ToolTip="Lighter Sepia">
                        <ImageSettings
                            MaxHeight="100"
                            MaxWidth="100"
                            Clip="true"
                            Sepia="true"
                            Brightness="0.2" />
                    </wmb:BetterImage>
            </wmb:ImageLink>
            <wmb:ImageLink runat="server" CssClass="fancyBox" rel="secondBreaker" NavigateUrl="~/imgs/Sample.jpg"
                ToolTip="Negative" ImageSettings-MaxWidth="800" ImageSettings-MaxHeight="600"
                ImageSettings-Negative="true">
                    <wmb:BetterImage runat="server" ImageUrl="~/imgs/Sample.jpg"
                        AlternateText="Negative"
                        ToolTip="Negative">
                        <ImageSettings
                            MaxHeight="100"
                            MaxWidth="100"
                            Clip="true"
                            Negative="true" />
                    </wmb:BetterImage>
            </wmb:ImageLink>
        </div>
        <h2>
            Very nice but...</h2>
        <p>
            How to add all the querystring values without making typo's? How to use this library
            inside a windows forms application? I decided it was time to make some serverside
            controls to create that querystrings for me and to split the library up into different
            dll's and namspaces.</p>
        <p>
            So finally I ended up with an Image manipulation library, an ImageHandler, and some
            WebControls with which I could:
        </p>
        <ul>
            <li>Create resized images;</li>
            <li>Create grayscaled images;</li>
            <li>Create negatives of images;</li>
            <li>Create sepia images;</li>
            <li>Set the brightness on images;</li>
            <li>Set the contrast on images;</li>
            <li>Quantize GIF images;</li>
            <li>Add copyright text;</li>
            <li>Protect your images;</li>
            <li>Prevent leeching / deeplinking;</li>
            <li>All this without the use of any code!</li>
            <li>All this with designer support!</li>
        </ul>
        <div class="breaker">
            <div class="cycleContainer">
                <asp:ListView ID="CycleList1" runat="server" EnableViewState="false">
                    <LayoutTemplate>
                        <div id="cycle1" class="cycle">
                            <asp:PlaceHolder runat="server" ID="itemPlaceholder" />
                        </div>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <wmb:ImageLink CssClass="fancyBox" rel="cycle1" NavigateUrl='<%# Eval("Path") %>'
                            ToolTip='<%# Eval("ToolTip") %>' runat="server">
                            <wmb:BetterImage CssClass="cycleImage" ImageUrl='<%# Eval("Path") %>' BorderWidth="1px" runat="server">
                                <ImageSettings
                                    MaxHeight="150"
                                    MaxWidth="150"
                                    Clip="true"/>
                            </wmb:BetterImage>
                        </wmb:ImageLink>
                    </ItemTemplate>
                </asp:ListView>
                <asp:ListView ID="CycleList2" runat="server" EnableViewState="false">
                    <LayoutTemplate>
                        <div id="cycle2" class="cycle">
                            <asp:PlaceHolder runat="server" ID="itemPlaceholder" />
                        </div>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <wmb:ImageLink ID="ImageLink1" CssClass="fancyBox" rel="cycle2" NavigateUrl='<%# Eval("Path") %>'
                            ToolTip='<%# Eval("ToolTip") %>' runat="server" ImageSettings-Grayscale="true">
                            <wmb:BetterImage ID="BetterImage2" CssClass="cycleImage" ImageUrl='<%# Eval("Path") %>' BorderWidth="1px" runat="server">
                                <ImageSettings
                                    MaxHeight="150"
                                    MaxWidth="150"
                                    Grayscale="true"
                                    Clip="true"/>
                            </wmb:BetterImage>
                        </wmb:ImageLink>
                    </ItemTemplate>
                </asp:ListView>
                <asp:ListView ID="CycleList3" runat="server" EnableViewState="false">
                    <LayoutTemplate>
                        <div id="cycle3" class="cycle">
                            <asp:PlaceHolder runat="server" ID="itemPlaceholder" />
                        </div>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <wmb:ImageLink CssClass="fancyBox" rel="cycle3" NavigateUrl='<%# Eval("Path") %>'
                            ToolTip='<%# Eval("ToolTip") %>' runat="server" ImageSettings-Negative="true">
                            <wmb:BetterImage CssClass="cycleImage" ImageUrl='<%# Eval("Path") %>' BorderWidth="1px" runat="server">
                                <ImageSettings
                                    MaxHeight="150"
                                    MaxWidth="150"
                                    Negative="true"
                                    Clip="true"/>
                            </wmb:BetterImage>
                        </wmb:ImageLink>
                    </ItemTemplate>
                </asp:ListView>
                <div style="clear: both">
                </div>
            </div>
        </div>
        <h2>
            Is it finished?</h2>
        <p>
            No! That's up to you! The code is fully extensible so be my guest and add your own
            image transformations or your own output cache provider.</p>
        <div class="breaker">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" RenderMode="Inline">
                <ContentTemplate>
                    <wmb:BetterImage ID="BetterImage1" runat="server" ImageUrl="~/imgs/QuantizeSample.gif"
                        AlternateText="This is a gif image that is resized and uses quantization to optimize the palette."
                        ToolTip="This is a gif image that is resized and uses quantization to optimize the palette.">
                        <ImageSettings MaxHeight="100" SaveName="Please Save Me" Quantize="true" />
                    </wmb:BetterImage>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="BetterImageButton1" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
            <wmb:BetterImage runat="server" ImageUrl="~/imgs/QuantizeSample.gif" AlternateText="This is a gif image that is resized without losing it's palette and thus maintaining transparency"
                ToolTip="This is a gif image that is resized without losing it's palette and thus maintaining transparency">
                <ImageSettings MaxHeight="100" SaveName="Please Save Me" MaintainPalette="true" />
            </wmb:BetterImage>
            <wmb:BetterImage runat="server" ImageUrl="placeholder.jpg" AlternateText="This image is stored inside a database"
                ToolTip="This image is stored inside a database">
                <ImageSettings MaxHeight="100" Grayscale="true" ImageRetriever="SqlRetriever" CustomData="1" />
            </wmb:BetterImage>
            <wmb:BetterImage runat="server" ImageUrl="~/imgs/Confidential.png" AlternateText="This is a transparent image of which the opcaty is changed"
                ToolTip="This is a transparent image of which the opacity is changed">
                <ImageSettings MaxHeight="100" Opacity="0.5" />
            </wmb:BetterImage>
            <wmb:BetterImageButton ID="BetterImageButton1" runat="server" ImageUrl="~/imgs/Sample.jpg"
                OnClick="BetterImageButton1_Click" AlternateText="This button uses a custom transform to add the watermark."
                ToolTip="This button uses a custom transform to add the watermark.">
                <ImageSettings MaxHeight="100" MaxWidth="100" CustomTransform="WaterMark" CustomData="0.8F"
                    Clip="true" OutputQuality="80" />
            </wmb:BetterImageButton>
        </div>
        <h2>
            Why put it on the internet for free?</h2>
        <p>
            It would be a waste of time to put like 200+ hours in a library and then use it
            by myself and myself alone to save me 40 hours;-) So you can actually make this
            project worthwhile by simply using it!</p>
        <h2>
            This page</h2>
        <p>
            The samples on this page don't showoff all the controls and are just the tip of
            the iceberg of what can be accomplished with this suite.</p>
        <p>
            The combinations are 'endless' and that's why I finally named it <strong>BetterImageProcessor!</strong></p>
        <p>
            Just download it and enjoy the ease of use!</p>
        <p>
            Cheers, <a href="mailto:websware@hotmail.com?subject=BetterImageProcessor">Wesley Bakker</a></p>
    </div>
    </form>
</body>
</html>
