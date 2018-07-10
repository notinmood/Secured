<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<%
    string SecuredLoansAmountMin = ((decimal)this.ViewData["SecuredLoansAmountMin"]).ToString("0,000");
    string SecuredLoansAmountMax = ((decimal)this.ViewData["SecuredLoansAmountMax"]).ToString("0,000");

    string UnSecuredLoansAmountMin = ((decimal)this.ViewData["UnSecuredLoansAmountMin"]).ToString("0,000");
    string UnSecuredLoansAmountMax = ((decimal)this.ViewData["UnSecuredLoansAmountMax"]).ToString("0,000");
     %>
    <div id="subheader">
        <h1>
            UNSECURED LOANS
        </h1>
    </div>
    <div id="main">
        <%--    @*本页主要信息*@--%>
        <div id="content" style="margin-left: 5px;">
            <div class="contentDigest">
                <ul>
                    <li>No financials?</li>
                    <li>Refused by banks?</li>
                    <li>Business cash flow crisis?</li>
                    <li>Opportunity too good to miss?</li>
                </ul>
                <p>
                    No matter it is for start-up cost, marketing, investment, or cash flow crisis, We
                    can help.
                </p>
            </div>
            <div class="contentDetail">
                <table>
                    <tr>
                        <th style="width: 30%">
                            Item
                        </th>
                        <th>
                            Description
                        </th>
                    </tr>
                    <tr>
                        <td>
                            Loan Amount
                        </td>
                        <td>
                            $<%=UnSecuredLoansAmountMin%> to $<%=UnSecuredLoansAmountMax%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Loan Term
                        </td>
                        <td>
                            1 ~ 12 months
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Key Features
                        </td>
                        <td>
                            <ul>
                                <li>Unsecured</li>
                                <li>Fast and easy application</li>
                                <li>No application fee</li>
                                <li>Cash overnight</li>
                                <li>Same day approval</li>
                            </ul>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Credit Check
                        </td>
                        <td>
                            Yes
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Document Required
                        </td>
                        <td>
                            <ul>
                                <li>Company Certificate</li>
                                <li>100 point ID for each director</li>
                                <li>6 months bank statement of the company</li>
                                <li>Proof of address for each director</li>
                            </ul>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Asset
                        </td>
                        <td>
                            Not required
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Cost
                        </td>
                        <td>
                            <ul>
                                <li>Interest: 12%pcm (Capitalised)</li>
                                <li>Documentation fee: $110</li>
                            </ul>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Early repayment
                        </td>
                        <td>
                            No penalty
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <%--    @*联系方式*@--%>
        <% Html.RenderAction("ContactExpress", "UserControl", new { picName = string.Empty }); %>
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            //Change current menu item style for selected
            $("#nav li #businessLoans").addClass("actived");
        })
    </script>
</asp:Content>
