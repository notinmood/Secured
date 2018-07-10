<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript" src="<%= Url.Content("~/Scripts/SWFObject.js")%>"></script>
    <!--start homebanner-->
    <div id="homebanner">
        <div class="slideshow">
            <div id="flashcontent">
                <p>
                    In order to view this page you need JavaScript and Flash Player 8+ support!</p>
            </div>
        </div>
        <ul class="tickbox">
            <li>Fast and easy </li>
            <li>Same day approval</li>
            <li>Bad credit considered</li>
            <li class="end">Secured and confidential</li>
        </ul>
    </div>
    <!--start homebanner-->
    <div id="homeboxcontainer">
        <!--start box1-->
        <div class="homebox">
            <h1 class="box1hd">
                Payday Loans</h1>
            <div class="fimg">
                <img src="<%=Url.Content("~/Content/imagesN/box1pic.jpg")%>" width="284" height="86" /></div>
            <h2>
                From $50 to $1,500<br />
                Up to 62 days</h2>
            <ul class="homeboxinfo">
                <li>Fast and easy application</li>
                <li>No application fee</li>
                <li>Same day approval</li>
            </ul>
            <ul class="btns">
                <li class="btn2"><a href="http://gbcash.com.au/apply/">Apply Now</a></li>
            </ul>
        </div>
        <!--start box2-->
                <div class="homebox" style="margin-right: 0px;">
            <h1 class="box2hd">
                Secured Personal Loans</h1>
            <div class="fimg">
                <img src="<%=Url.Content("~/Content/imagesN/box3pic.jpg")%>" width="284" height="86" /></div>
            <h2>
                From 1,000 to $20,000<br />
                3 months to 2 years</h2>
            <ul class="homeboxinfo">
                <li>Lower interest rate than an unsecured loan </li>
                <li>Bad credit considered </li>
                <li class="nobg"></li>
            </ul>
            <ul class="btns">
                <li class="btn2"><a href="<%=Url.Action("Pre-qualification","Secured")%>">Apply Now</a></li>
            </ul>
        </div>
        
        
        <!--start box3-->
        <div class="homebox" style="margin-right: 0px;">
            <h1 class="box3hd">
                Sell Your Old Gold Jewellery</h1>
            <div class="fimg">
                <img src="<%=Url.Content("~/Content/imagesN/box2pic.jpg")%>" width="284" height="86" /></div>
            <h2>
                Safe. Easy. Confidential </h2>

            <ul class="homeboxinfo">
                <li class="nobghead" style="padding-bottom:7px">We Buy:</li>
                <li>  Gold </li>
                <li>  Silver </li>
                <li>  Platinum </li>
                <li class="nobghead">Today's Price: <input type="text" size="5" class="price"  /> <a href="http://gbcash.com.au/Sell-Your-Old-Gold-Jewellery/How-To-Sell.htm" class="inlinebtns" target="_blank">Learn More</a></li>
            </ul>
        </div>
    </div>
    <!--end homebanner-->
    <script type="text/javascript">
        $(document).ready(function () {
            //Change current menu item style for selected
            $("#nav li #home").addClass("actived");
        })
    </script>
    <script type="text/javascript">
    var tvc = new SWFObject("<%=Url.Content("~/Content/swfobjectN/slideshow.swf")%>", "feature", "707", "260", "8");
    tvc.addParam('menu', 'true');
    tvc.addParam('wmode', 'Opaque');
    tvc.write("flashcontent");
    </script>
</asp:Content>
