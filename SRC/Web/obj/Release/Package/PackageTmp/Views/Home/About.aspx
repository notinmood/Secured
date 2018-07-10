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
        </div>
        <%--    @*联系方式*@--%>
        <% Html.RenderAction("ContactExpress", "UserControl",new {picName=string.Empty}); %>
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            //Change current menu item style for selected
            $("#nav li #faqs").addClass("actived");
        })
    </script>
</asp:Content>
