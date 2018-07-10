<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div id="subheader">
        <h1>
            FAQs
        </h1>
    </div>
    <div id="main">
        <div id="content" style="margin-left: 5px;">
            <p>
                <span class="strong">Am I eligible?</span> To be eligible you should be
                <ul>
                    <li>currently employed and take home at least $350 per week.</li>
                    <li>own a registered motor vehicle (no finance) OR a real estate property (whether mortgaged or not is ok).</li>
                </ul>
            </p>
            <p>
                <span class="strong">What I should know before entering a credit contract?</span> 
                Please read the <a href="../Information/Credit-Guide.doc">Credit Guide</a> before entering a credit contract.
            </p>
            <p>
                <span class="strong">How much can I borrow?</span>
                It depends on your ability to repay the loan and vehicle/property value. As a motor vehicle secured loan, it can be up to 50% of your motor vehicle trade-in value. If it is property secured, loan amount in some cases may be up to 95% of the property value less any credit owing.
            </p>
            <p>
                <span class="strong">How long does the approval process take?</span>
                So long as your application and all the required documentation are received, approval takes only a few hours - applications received by 2 p.m. EST will be processed the same day.
            </p>
            <p>
                <span class="strong">How can I speed up the loan approval process? </span>
                Make sure application form filled in full and all necessary documents are provided to avoid delay.
            </p>
            <p>
                <span class="strong">How do I maximise my chances of approval? </span>
                Simply make sure the application form is fully filled, and provide clear supporting documents. Usually joint applications have a higher chance of approval.
            </p>
            <p>
                <span class="strong">What documentation do I need?</span> 
                <ul>
                    <li>100 points of ID </li>
                    <li>Last two payslips</li>
                    <li>3 months bank statement received in 30 days</li>
                    <li>A utility bill and mortgage documents or lease agreement. Each document to include current address and be current one month before you submit the loan application.</li>
                    <li>Current Rates Notice(s) or car rego papers</li>
                </ul>
            </p>
            <p>
                <span class="strong">What happen after I apply?</span>
                <ul>
                    <li>When we receive your application, you will receive a courtesy call or email from one of our staff to acknowledge that we have received it and are starting work on it right away.</li>
                    <li>Within 2 working hours, you will receive an answer, and if approved, you will receive the letter of offer straight away.</li>
                    <li>Once the letter of offer is faxed back to us and all conditions are met, we will finalise the due diligence and instruct our solicitors to prepare the loan contracts, which will be sent to you via email.</li>
                    <li>Within 2 working hours of receiving the loan contracts back, we will settle the loan. Our staff will call you to advise that settlement has taken place.</li>
                </ul>
            </p>
            <p>
                <span class="strong">What is the interest rate?</span>
                Please refer to our Costs & Fees section for more information.
            </p>
            <p>
                <span class="strong">Can I repay my loan early?</span>
                Yes, you can pay-off your loan early without penalty and save interest. But, we will require one month notice of your required payment.
            </p>
            <p>
                <span class="strong">Will a credit check be listed on my file?</span>
                A credit check will be done on applicants only during final review of your application. If you decide not to proceed after pre-approval, no credit check will be carried out.
            </p>
            <p>
                <span class="strong">Will my privacy be protected?</span>
                Absolutely. At all times your privacy is protected. Golden Bridge Cashsolution™ operates a strict Privacy Policy preventing 3rd parties or associated companies from accessing your personal information unless you give them express permission to do so.
            </p>
            <p>
                <span class="strong">Why has my application been rejected?</span>
                There are many reasons why your loan may be rejected, our staffs are unable to give specific reasons for this, but wherever possible they will try and get your loan approved.
            </p>
        </div>
        <%--    @*联系方式*@--%>
        <% Html.RenderAction("ContactExpress", "UserControl", new { picName = "faqPic.jpg" }); %>
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            //Change current menu item style for selected
            $("#nav li #faqs").addClass("actived");
        })
    </script>
</asp:Content>
