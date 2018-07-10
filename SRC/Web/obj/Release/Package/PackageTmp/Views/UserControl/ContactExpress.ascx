<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%--    @*联系方式*@--%>
<%    
    var picName = string.Empty;
    if (this.ViewData["picName"] != null)
    {
        picName = this.ViewData["picName"].ToString();
    }

    if (string.IsNullOrWhiteSpace(picName))
    {
        picName = "contactpic.jpg";
    }

    picName = string.Format("~/Content/images/{0}", picName);
%>
<div id="process" style="float: right;">
    <h1 class="phoneicon">
        Golden Bridge Cash Solution
    </h1>
    <ul>
        <li class="contactdetials">Tel: 1300 137 906</li>
        <li class="contactdetials">Fax: 1300 138 916</li>
        <li class="contactdetials">Email: info@gbcash.com.au</li>
        <li class="contactdetials">PO Box 347</li>
        <li class="contactdetials">Collins Street West, VIC 8007</li>
        <li class="contactdetials">ABN: 92 112 483 226</li>
        <li class="contactdetials">Australian Credit Licence No.: 388601</li>
        <li class="contactdetials">COSL member No.: 0002491</li>
        <li class="contactdetials">
            <img style="padding-top: 20px;" alt="ContactUs" src="<%=Url.Content(picName) %>"
                width="255" height="170" />
        </li>
    </ul>
</div>
