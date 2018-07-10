<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<%@ Import Namespace="HiLand.Utility4.MVC.Controls" %>
<%@ Import Namespace="GBFinance.Web.Control" %>
<%@ Import Namespace="HiLand.Utility.Finance" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%
        List<Payment> paymentList = new List<Payment>();
        object paymentListObject = this.ViewData["paymentList"];

        if (paymentListObject != null)
        {
            paymentList = paymentListObject as List<Payment>;
        }
    %>
    <div id="subheader">
        <h1>
            CALCULATOR
        </h1>
<%--        <link href="<%= Url.Content("~/Content/toasts/toast.css") %>" rel="stylesheet" type="text/css" />--%>
    </div>
    <div id="main">
        <div id="content" style="margin-left: 5px;">
            <%  this.Html.BeginForm();
            %>
            <label for="loanAmount">
                Amount Required:</label>
            <input name="loanAmount" id="loanAmount" placeholder="Loan Requested Amount" />
            <label for="loanTerms">
                Loan Terms</label>
            <%= Html.GBFLoanTermCountDropDownList().Name("loanTerms").IsUseSelfContainer(false).Render()%>
            <input type="submit" name="submit" value="Calculate" class="butt blue" />
            <%  this.Html.EndForm(); %>
            <% if (paymentList.Count > 0)
               {
            %>
            <div class="contentDetail">
                <table>
                    <tr>
                        <th style="width: 30%">
                            Payment Date
                        </th>
                        <th>
                            Payment Amount
                        </th>
                    </tr>
                    <%
                   for (int i = 0; i < paymentList.Count; i++)
                   {
                       var currentAmount= paymentList[i].Amount;  
                       var currentDate= paymentList[i].PaymentDate;
                    %>
                    <tr>
                        <td>
                            <%= GBFinance.Web.Models.Miscs.ConvertToAustrliaFormatStringFromDate( currentDate)%>
                        </td>
                        <td>
                            <%=currentAmount.ToString("0.00") %>
                        </td>
                    </tr>
                    <%}%>
                </table>
            </div>
            <%} %>
        </div>
        <% Html.RenderAction("ContactExpress", "UserControl", new { picName = string.Empty }); %>
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            //Change current menu item style for selected
            $("#nav li #loadnCalculator").addClass("actived");
        })
    </script>
</asp:Content>
