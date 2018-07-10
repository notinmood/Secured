<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div id="subheader">
        <h1>
            Costs & Fees
        </h1>
    </div>
    <div id="main">
        <%--    @*本页主要信息*@--%>
        <div id="content" style="margin-left: 5px;">
            <p>
                <span class="strong">Comparison Rate Schedule</span>
                Cash loans provided by Golden Bridge Enterprises (Aust) Pty Ltd <br />
                Date of issue: 01/07/2011
            </p>
           <div class="contentDetail">
                <table>
                    <tr>
                        <th style="width: 30%">
                            Loan Amount
                        </th>
                        <th>
                            Loan term
                        </th>
                        <th>
                            Comparison Rate p.a.
                        </th>
                        <th>
                            Annual Percentage Rate
                        </th>
                    </tr>
                    <tr>
                        <td>
                            $1,000
                        </td>
                        <td>
                            6 months
                        </td>
                        <td>
                           30.00%
                        </td>
                        <td>
                           30.00%
                        </td>
                    </tr>
                    <tr>
                        <td>
                            $3,000(secured)
                        </td>
                        <td>
                             2 years
                        </td>
                        <td>
                           30.00%
                        </td>
                        <td>
                           30.00%
                        </td>
                    </tr>
                     <tr>
                        <td>
                            $10,000(secured)
                        </td>
                        <td>
                            2 years
                        </td>
                        <td>
                           30.00%
                        </td>
                        <td>
                           30.00%
                        </td>
                    </tr>
                </table>
            </div>
             <br />
             <p>
                <span class="strong">WARNING: </span>
                This Comparison Rate applies only to the example or examples given. Different amounts and terms will result in different comparison rates. Costs such as redraw fees or early repayment fees, and cost savings such as fee waivers, are not included in the Comparison Rate but may influence the cost of the loan.
            </p>
             <p>
                <span class="strong">Charges may become payable</span>
                Payment Dishonour Fee: $25 for each payment dishonoured <br />
                Late Fee: $50 for any installment is not paid in full within 5 days following its scheduled due date.<br />
                Collection Fee: 25% of outstanding debt
            </p>
        </div>
        <%--    @*联系方式*@--%>
        <% Html.RenderAction("ContactExpress", "UserControl", new { picName = string.Empty }); %>
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            //Change current menu item style for selected
            $("#nav li #costsAndFees").addClass("actived");
        })
    </script>
</asp:Content>