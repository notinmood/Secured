<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style type="text/css">
        .strongText
        {
            font-family: Georgia, "Times New Roman" , Times, serif;
            font-size:13px;
            color: #000000; 
            font-family: Georgia,Times,serif;
            line-height: 20px;
        }
    </style>
    <div id="subheader">
        <h1>
            How It Works
        </h1>
    </div>
    <div id="main">
        <%--    @*本页主要信息*@--%>
        <div id="content" style="margin-left: 5px;">
           <div class="contentDigest">
                <ul>
                    <li>Lower Interest rate</li>
                    <li>Bad credit considered</li>
                    <li>Same day approval</li>
                </ul>
            </div>
           <div class="contentDetail">
                <table>
                    <tr>
                        <td  style="width: 30%">
                            Loan Amount
                        </td>
                        <td>
                            <span class="strongText">$1,000 to $20,000</span>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Loan Term
                        </td>
                        <td>
                            <span class="strongText">3 months ~ 2 years</span>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Availability
                        </td>
                        <td>
                            <span class="strongText">All states</span>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Key Features
                        </td>
                        <td>
                            <ul>
                                <li>Secured</li>
                                <li>Fast and easy application</li>
                                <li>Bad credit considered </li>
                                <li>Same day approval</li>
                                <li>Cash overnight</li>
                            </ul>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Credit Check
                        </td>
                        <td>
                           <span class="strongText">Yes</span>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Document Required
                        </td>
                        <td>
                            <ul>
                                <li>Two latest pay slips</li>
                                <li>3 months bank statement received in the last 30 days</li>
                                <li>Your Identification card (usually Driver's Licence or Passport)</li>
                                <li>A utility bill or mortgage documents or lease agreement. Each document to include current address and be current one month before you submit the loan application</li>
                            </ul>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Acceptable Security
                        </td>
                        <td>
                            <span class="strongText">Registered motor vehicle (no finance) OR Real estate property (mortgaged accepted)</span>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Max LVR
                        </td>
                        <td>
                            <span class="strongText">75%</span>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Costs
                        </td>
                        <td>
                            <ul>
                                <li>Interest: 2.5% per month</li>
                                <li>Legal fees: $360 or 6% of the loan amount whichever is higher incl. GST (Capitalised)</li>
                            </ul>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Early repayment
                        </td>
                        <td>
                            <span class="strongText">No penalty</span>
                        </td>
                    </tr>
                </table>
                <br />
            </div>
        </div>
        <%--    @*联系方式*@--%>
        <% Html.RenderAction("ContactExpress", "UserControl", new { picName = string.Empty }); %>
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            //Change current menu item style for selected
            $("#nav li #howItWorks").addClass("actived");
        })
    </script>
</asp:Content>
