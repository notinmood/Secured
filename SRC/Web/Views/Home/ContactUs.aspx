<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div id="subheader">
        <h1>
            CONTACT US
        </h1>
    </div>
    <div id="main">
        <%--    @*本页主要信息*@--%>
        <div id="content" style="margin-left: 5px;">
        <p>
                <span class="strong">Contact us page content as below:</span>

We are dedicated to making sure your Golden Bridge Cash SolutionTM experience is enjoyable. If there are any questions, comments or concerns please contact us. 
</p>
<p>
<span class="strong">Telephone : </span>
1300 137 906 </p>

<p>
<span class="strong">Facsimile : </span>
1300 138 916</p>

<p>
<span class="strong">Our customer service hours are : </span>
Monday - Friday: 9:00 a.m. - 5:30 p.m. EST
Exclude weekends & public holidays in Victoria.
</p>
<p>
<span class="strong">Postal address: </span>
P.O. Box 347 
Collins Street West, VIC 8007
</p>
<p>
<span class="strong">Email address : </span>
Loans and extension requests: apply@cashsolution.com.au <br />

Payments on loans and extensions: payment@cashsolution.com.au <br />

General requests or concerns: info@cashsolution.com.au <br />
</p>

        </div>
        <%--    @*联系方式*@--%>
        <% Html.RenderAction("ContactExpress", "UserControl",new {picName=string.Empty}); %>
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            //Change current menu item style for selected
            $("#nav li #contackUs").addClass("actived");
        })
    </script>
</asp:Content>
