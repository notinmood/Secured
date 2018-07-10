<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div id="subheader">
        <h1>
            Privacy Statement
        </h1>
    </div>
    <div id="main">
        <%--    @*本页主要信息*@--%>
        <div id="content" style="margin-left: 5px;">
            <p>
                <b>PRIVACY ACT AUTHORISATION - AUTHORISATION BY APPLICANT/GUARANTOR/INDEMNIFIER:</b><br />
                <i>Commonwealth Privacy Act 1988 Section 18E(8)(c), 18E(1), 18L (4), 18K (1)(b), 18$
                    (1)(b),</i>
            </p>
            <p style="text-indent:20px;">
                The Applicant/s & Guarantor/s acknowledges that the Privacy Act allows any Credit
                Provider to give a Credit Reporting Agency certain personal information about the
                application for finance. The information that may be given to an agency includes;
                <ul style="text-indent: 30px;">
                    <li>• such permitted particulars about the Applicant/s & Guarantor/s which allows the
                        Applicant/Guarantor to be identified;</li>
                    <li>• the fact that the Applicant/s & Guarantor/s have applied for finance and the amount;</li>
                    <li>• the fact that Service Finance are a current Credit provider to the Applicant/s
                        & Guarantor/s;</li>
                    <li>• payments which become overdue more than 60 days, and for which collection action
                        has commenced;</li>
                    <li>• advise that payments are no longer overdue;</li>
                    <li>• cheques drawn by the Applicant/s & Guarantor/s which have been dishonoured more
                        than once;</li>
                    <li>• in specified circumstances, that in the opinion of Golden Bridge Finance Pty Ltd
                        the Applicant/s & Guarantor/s have committed a serious credit infringement;</li>
                    <li>• that finance provided to the Applicant/s & Guarantor/s by the Golden Bridge Finance
                        Pty Ltd has been paid or otherwise discharged</li>
                </ul>
            </p>
            <br />
            <p style="text-indent:20px;">
                The Applicant/s & Guarantor/s agrees that, if it is considered relevant in assessing
                the application for personal, Service Finance may obtain a report about the commercial
                credit worthiness of person/s. The Applicant/s & Guarantor/s agrees that, if it
                is considered relevant in assessing the Applicant/s & Guarantor/s application for
                commercial credit, Golden Bridge Finance Pty Ltd may obtain from a Credit Reporting
                Agency a credit report containing personal credit information about the Applicant/s
                & Guarantor/s.
            </p>
            <br />
            <p style="text-indent:20px;">
                The Applicant/s & Guarantor/s agrees that Golden Bridge Finance Pty Ltd may give
                to and seek from any Credit provider/s named in the accompanying finance application
                and any Credit Provider/s that may be named in a personal or commercial credit report
                issued by a Credit reporting Agency or a commercial credit reporting agency respectively,
                information about the Applicant/s & Guarantor/s personal or commercial credit arrangements
                for the purpose of assessing the Applicant/s & Guarantor/s finance application or
                collecting in overdues; the Applicant/s & Guarantor/s understands that this information
                can include any information about the Applicant/s & Guarantor/s credit worthiness,
                credit standing, credit history or credit capacity that Credit Provider/s are allowed
                to give or receive from each other under the Privacy Act.
            </p>
        </div>
        <%--    @*联系方式*@--%>
        <% Html.RenderAction("ContactExpress", "UserControl", new { picName = string.Empty }); %>
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            //Change current menu item style for selected
            $("#nav li #home").addClass("actived");
        })
    </script>
</asp:Content>
