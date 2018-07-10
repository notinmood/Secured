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
            SECURED LOANS
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
                            $<%=SecuredLoansAmountMin%> to $<%=SecuredLoansAmountMax %>
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
                                <li>No upfront fees</li>
                                <li>No financials required</li>
                                <li>No tax returns required</li>
                                <li>Bad credit considered</li>
                                <li>Same day approval</li>
                            </ul>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Credit Check
                        </td>
                        <td>
                            No
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Document Required
                        </td>
                        <td>
                            <ul>
                                <li>Current Rates Notice(s) for all security properties or car rego papers</li>
                                <li>100 points of ID for each borrower</li>
                                <li>6 months bank statement of the business</li>
                                <li>6 months bank statement in which your repayment is going to be drawn out of (if
                                    Applicable) </li>
                            </ul>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Acceptable Security
                        </td>
                        <td>
                            Registered motor vehicle (no finance) OR Real estate property (mortgaged accepted)
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Max LVR
                        </td>
                        <td>
                            75%
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Cost
                        </td>
                        <td>
                            <ul>
                                <li>Interest: 6%pcm (Capitalised)</li>
                                <li>Establishment fee: 6% of the loan (Capitalised)</li>
                                <li>Legal fees: $1200 incl. GST (Capitalised)</li>
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
