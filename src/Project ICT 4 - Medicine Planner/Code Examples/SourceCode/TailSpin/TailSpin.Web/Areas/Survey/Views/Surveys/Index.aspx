﻿<%--
===============================================================================
 Microsoft patterns & practices
 Windows Phone 7 Developer Guide
===============================================================================
 Copyright © Microsoft Corporation.  All rights reserved.
 This code released under the terms of the 
 Microsoft patterns & practices license (http://wp7guide.codeplex.com/license)
===============================================================================
--%>

<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" 
         Inherits="System.Web.Mvc.ViewPage<TailSpin.Web.Models.TenantPageViewData<IEnumerable<TailSpin.Web.Survey.Shared.Models.Survey>>>" %>
<%@ Import Namespace="TailSpin.Web.Utility" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MenuContent" runat="server">
    <ul>
        <li><%:Html.ActionLink("New Survey", "New", "Surveys")%></li>
        <li class="current"><a>My Surveys</a></li>
        <li><%:Html.ActionLink("My Account", "Index", "Account", new {area = string.Empty}, null)%></li>
    </ul>
    <div class="clear">
    </div>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="breadcrumbs">
        My Surveys
    </div>
    <% if (this.Model.ContentModel.Count() == 0) { %>
    <div class="noResults">
        <h3>There are no surveys.</h3>
    </div>
    <% } else { %>
    <%using (Html.BeginForm("Delete", "Surveys")) {%>
    <%:Html.AntiForgeryToken() %>
    <table border="0" cellspacing="0" cellpadding="0" class="tableGrid">
        <tr>
            <th>
                Title
            </th>
            <th>
                Created On
            </th>
            <th>
                Analyze
            </th>
            <th>
                Delete
            </th>
        </tr>
        <% foreach (var survey in this.Model.ContentModel) %>
        <% { %>
        <tr>
            <td>
                <%:Html.SurveyLink(survey.Title, this.ViewData["tenant"].ToString(), survey.SlugName)%>
            </td>
            <td>
                <%:survey.CreatedOn.ToLongDateString()%>
            </td>
            <td>
                <%:Html.ActionLink("Analyze", "Analyze", "Surveys", new { surveySlug = survey.SlugName }, new { })%>
            </td>
            <td>
                <a href="#" onclick="javascript: submitToDelete('<%:Url.Action("Delete", "Surveys", new { surveySlug = survey.SlugName })%>')">Delete</a>
            </td>
        </tr>
        <% } %>
    </table>
    <% } } %>

    <script type="text/javascript">
        function submitToDelete(url) {
            document.forms[0].action = url 
            document.forms[0].submit();
        }
    </script>
</asp:Content>
