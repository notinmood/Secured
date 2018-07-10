<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div id="subheader">
        <h1>
            PAYROLL FINANCE
        </h1>
    </div>
    <div id="main">
        <%--    @*本页主要信息*@--%>
        <div id="content" style="margin-left: 5px;">
            <div class="contentDigest">
                <p>
                    Need positive cash flow to make your business fly?
                </p>
                <p>
                    It works well for other small and large businesses – now it can work for you!
                </p>
                <ul>
                    <li>New method of funding</li>
                    <li>Online overdraft facility</li>
                    <li>Easy to use</li>
                </ul>
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
                            Up to twice of your monthly wage bill
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Loan Term
                        </td>
                        <td>
                            56 days
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Key Features
                        </td>
                        <td>
                            <ul>
                                <li>New method of funding</li>
                                <li>Online overdraft facility </li>
                                <li>Self managed online</li>
                                <li>Ad-hoc or rolling basis</li>
                                <li>No financials</li>
                                <li>No tax returns required</li>
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
                                <li>Current Rates Notice(s) for all security properties or car rego papers</li>
                                <li>100 points of ID for each Director</li>
                                <li>6 months bank statement of the business</li>
                                <li>6 months bank statement in which your repayment is going to be drawn out of (if Applicable)</li>
                            </ul>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Acceptable Security
                        </td>
                        <td>
                            Real estate property (mortgaged accepted)
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
                                <li>Interest: 9%pcm (Capitalised)</li>
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
