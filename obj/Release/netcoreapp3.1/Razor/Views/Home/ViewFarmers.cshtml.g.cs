#pragma checksum "C:\WORK\Projects\AlliedBlock\eKitabu\Mtama\Code\mtama3\Views\Home\ViewFarmers.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "57b6dedd946255c4e1937825bc260f259cfdd98b"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Home_ViewFarmers), @"mvc.1.0.view", @"/Views/Home/ViewFarmers.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "C:\WORK\Projects\AlliedBlock\eKitabu\Mtama\Code\mtama3\Views\_ViewImports.cshtml"
using Microsoft.AspNetCore.Identity;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\WORK\Projects\AlliedBlock\eKitabu\Mtama\Code\mtama3\Views\_ViewImports.cshtml"
using Mtama;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "C:\WORK\Projects\AlliedBlock\eKitabu\Mtama\Code\mtama3\Views\_ViewImports.cshtml"
using Mtama.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "C:\WORK\Projects\AlliedBlock\eKitabu\Mtama\Code\mtama3\Views\_ViewImports.cshtml"
using Mtama.Models.AccountViewModels;

#line default
#line hidden
#nullable disable
#nullable restore
#line 5 "C:\WORK\Projects\AlliedBlock\eKitabu\Mtama\Code\mtama3\Views\_ViewImports.cshtml"
using Mtama.Models.ManageViewModels;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"57b6dedd946255c4e1937825bc260f259cfdd98b", @"/Views/Home/ViewFarmers.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"14237a70e6f2f138ebb07976a57a0a51cdc899d7", @"/Views/_ViewImports.cshtml")]
    public class Views_Home_ViewFarmers : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<IEnumerable<Mtama.Models.ApplicationUser>>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("href", new global::Microsoft.AspNetCore.Html.HtmlString("~/Latest-DataTable/dataTables.bootstrap.min.css"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("rel", new global::Microsoft.AspNetCore.Html.HtmlString("stylesheet"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_2 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("src", new global::Microsoft.AspNetCore.Html.HtmlString("~/Latest-DataTable/jquery.dataTables.min.js"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_3 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("href", new global::Microsoft.AspNetCore.Html.HtmlString("~/Latest-DataTable/jquery.dataTables.min.css"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_4 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-area", "", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_5 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-controller", "Home", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_6 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-action", "InitatePayment", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_7 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("method", "post", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        #line hidden
        #pragma warning disable 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        #pragma warning restore 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 3 "C:\WORK\Projects\AlliedBlock\eKitabu\Mtama\Code\mtama3\Views\Home\ViewFarmers.cshtml"
  
    ViewData["Title"] = "View Farmers";
    //Layout = null;
    //Html.ActionLink("linkText", "Profile", "Manage");


#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n\r\n\r\n\r\n");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("link", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "57b6dedd946255c4e1937825bc260f259cfdd98b7025", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_0);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_1);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("script", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "57b6dedd946255c4e1937825bc260f259cfdd98b8139", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_2);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("link", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "57b6dedd946255c4e1937825bc260f259cfdd98b9178", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_3);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_1);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n\r\n");
#nullable restore
#line 17 "C:\WORK\Projects\AlliedBlock\eKitabu\Mtama\Code\mtama3\Views\Home\ViewFarmers.cshtml"
  
    var index = 1;

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n\r\n\r\n<input type=\"hidden\" id=\"AggId\" name=\"AggId\"");
            BeginWriteAttribute("value", " value=\"", 551, "\"", 573, 1);
#nullable restore
#line 23 "C:\WORK\Projects\AlliedBlock\eKitabu\Mtama\Code\mtama3\Views\Home\ViewFarmers.cshtml"
WriteAttributeValue("", 559, ViewBag.AggId, 559, 14, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" />\r\n<table id=\"ComplianceList\" class=\"table table-striped table-bordered\">\r\n    <thead>\r\n        <tr>\r\n            <th>\r\n                Index\r\n            </th>\r\n            <th>\r\n                ");
#nullable restore
#line 31 "C:\WORK\Projects\AlliedBlock\eKitabu\Mtama\Code\mtama3\Views\Home\ViewFarmers.cshtml"
           Write(Html.DisplayNameFor(model => model.FirstName));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n            </th>\r\n            <th>\r\n                ");
#nullable restore
#line 34 "C:\WORK\Projects\AlliedBlock\eKitabu\Mtama\Code\mtama3\Views\Home\ViewFarmers.cshtml"
           Write(Html.DisplayNameFor(model => model.LastName));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n            </th>\r\n            <th>\r\n                Link Farmer\r\n            </th>\r\n            <th>\r\n                Initiate Payment\r\n            </th>\r\n\r\n        </tr>\r\n    </thead>\r\n    <tbody>\r\n\r\n");
#nullable restore
#line 47 "C:\WORK\Projects\AlliedBlock\eKitabu\Mtama\Code\mtama3\Views\Home\ViewFarmers.cshtml"
         if (signInManager.IsSignedIn(User) && User.IsInRole("Aggregator"))
        {
            

#line default
#line hidden
#nullable disable
#nullable restore
#line 49 "C:\WORK\Projects\AlliedBlock\eKitabu\Mtama\Code\mtama3\Views\Home\ViewFarmers.cshtml"
             foreach (var item in Model)
            {
                if (item.UserRole == "Farmer")
                {

#line default
#line hidden
#nullable disable
            WriteLiteral("                    <tr>\r\n                        <td>\r\n                            ");
#nullable restore
#line 55 "C:\WORK\Projects\AlliedBlock\eKitabu\Mtama\Code\mtama3\Views\Home\ViewFarmers.cshtml"
                       Write(index);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                        </td>\r\n                        <td>\r\n                            ");
#nullable restore
#line 58 "C:\WORK\Projects\AlliedBlock\eKitabu\Mtama\Code\mtama3\Views\Home\ViewFarmers.cshtml"
                       Write(Html.DisplayFor(modelItem => item.FirstName));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                        </td>\r\n                        <td>\r\n                            ");
#nullable restore
#line 61 "C:\WORK\Projects\AlliedBlock\eKitabu\Mtama\Code\mtama3\Views\Home\ViewFarmers.cshtml"
                       Write(Html.DisplayFor(modelItem => item.LastName));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                        </td>\r\n                        <td>\r\n");
#nullable restore
#line 64 "C:\WORK\Projects\AlliedBlock\eKitabu\Mtama\Code\mtama3\Views\Home\ViewFarmers.cshtml"
                             if (item.AggregatorLinked == true)
                            {

#line default
#line hidden
#nullable disable
            WriteLiteral("                                <input type=\"checkbox\" checked style=\"height:20px;width:20px\"");
            BeginWriteAttribute("id", " id=\"", 1947, "\"", 1960, 1);
#nullable restore
#line 66 "C:\WORK\Projects\AlliedBlock\eKitabu\Mtama\Code\mtama3\Views\Home\ViewFarmers.cshtml"
WriteAttributeValue("", 1952, item.Id, 1952, 8, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            BeginWriteAttribute("onclick", " onclick=\"", 1961, "\"", 1993, 3);
            WriteAttributeValue("", 1971, "LinkFarmer(\'", 1971, 12, true);
#nullable restore
#line 66 "C:\WORK\Projects\AlliedBlock\eKitabu\Mtama\Code\mtama3\Views\Home\ViewFarmers.cshtml"
WriteAttributeValue("", 1983, item.Id, 1983, 8, false);

#line default
#line hidden
#nullable disable
            WriteAttributeValue("", 1991, "\')", 1991, 2, true);
            EndWriteAttribute();
            WriteLiteral(" />\r\n");
#nullable restore
#line 67 "C:\WORK\Projects\AlliedBlock\eKitabu\Mtama\Code\mtama3\Views\Home\ViewFarmers.cshtml"
                            }
                            else
                            {

#line default
#line hidden
#nullable disable
            WriteLiteral("                                <input type=\"checkbox\" style=\"height:20px;width:20px\"");
            BeginWriteAttribute("id", " id=\"", 2180, "\"", 2193, 1);
#nullable restore
#line 70 "C:\WORK\Projects\AlliedBlock\eKitabu\Mtama\Code\mtama3\Views\Home\ViewFarmers.cshtml"
WriteAttributeValue("", 2185, item.Id, 2185, 8, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            BeginWriteAttribute("onclick", " onclick=\"", 2194, "\"", 2226, 3);
            WriteAttributeValue("", 2204, "LinkFarmer(\'", 2204, 12, true);
#nullable restore
#line 70 "C:\WORK\Projects\AlliedBlock\eKitabu\Mtama\Code\mtama3\Views\Home\ViewFarmers.cshtml"
WriteAttributeValue("", 2216, item.Id, 2216, 8, false);

#line default
#line hidden
#nullable disable
            WriteAttributeValue("", 2224, "\')", 2224, 2, true);
            EndWriteAttribute();
            WriteLiteral(" />\r\n");
#nullable restore
#line 71 "C:\WORK\Projects\AlliedBlock\eKitabu\Mtama\Code\mtama3\Views\Home\ViewFarmers.cshtml"
                            }

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                        </td>\r\n\r\n                        <td>\r\n");
#nullable restore
#line 76 "C:\WORK\Projects\AlliedBlock\eKitabu\Mtama\Code\mtama3\Views\Home\ViewFarmers.cshtml"
                             if (item.AggregatorLinked == true)
                            {


#line default
#line hidden
#nullable disable
            WriteLiteral("                            ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("form", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "57b6dedd946255c4e1937825bc260f259cfdd98b16793", async() => {
                WriteLiteral("\r\n                                <input type=\"hidden\" id=\"AggId1\" name=\"AggId1\"");
                BeginWriteAttribute("value", " value=\"", 2616, "\"", 2638, 1);
#nullable restore
#line 80 "C:\WORK\Projects\AlliedBlock\eKitabu\Mtama\Code\mtama3\Views\Home\ViewFarmers.cshtml"
WriteAttributeValue("", 2624, ViewBag.AggId, 2624, 14, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(" />\r\n                                <input type=\"hidden\" id=\"UserId\" name=\"UserId\"");
                BeginWriteAttribute("value", " value=\"", 2722, "\"", 2738, 1);
#nullable restore
#line 81 "C:\WORK\Projects\AlliedBlock\eKitabu\Mtama\Code\mtama3\Views\Home\ViewFarmers.cshtml"
WriteAttributeValue("", 2730, item.Id, 2730, 8, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(" />\r\n                                <button type=\"submit\" class=\"btn btn-primary\">Initate Payment</button>\r\n                            ");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.Area = (string)__tagHelperAttribute_4.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_4);
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.Controller = (string)__tagHelperAttribute_5.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_5);
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.Action = (string)__tagHelperAttribute_6.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_6);
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.Method = (string)__tagHelperAttribute_7.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_7);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n");
#nullable restore
#line 84 "C:\WORK\Projects\AlliedBlock\eKitabu\Mtama\Code\mtama3\Views\Home\ViewFarmers.cshtml"
                            }

#line default
#line hidden
#nullable disable
            WriteLiteral("                        </td>\r\n\r\n\r\n\r\n\r\n");
#nullable restore
#line 90 "C:\WORK\Projects\AlliedBlock\eKitabu\Mtama\Code\mtama3\Views\Home\ViewFarmers.cshtml"
                           index++;

#line default
#line hidden
#nullable disable
            WriteLiteral("                    </tr>\r\n");
#nullable restore
#line 92 "C:\WORK\Projects\AlliedBlock\eKitabu\Mtama\Code\mtama3\Views\Home\ViewFarmers.cshtml"
                }
            }

#line default
#line hidden
#nullable disable
#nullable restore
#line 93 "C:\WORK\Projects\AlliedBlock\eKitabu\Mtama\Code\mtama3\Views\Home\ViewFarmers.cshtml"
             
        }

#line default
#line hidden
#nullable disable
            WriteLiteral(@"    </tbody>
</table>



<script>
    $(document).ready(function () {
        $('#ComplianceList').DataTable();
        //$('#ComplianceList_info').css(""padding"", ""20px"");

    });

    function LinkFarmer(UserId) {

        //alert(""Linked"");
        var AggId = document.getElementById(""AggId"").value;
        var link = ""Unlink"";
        if (document.getElementById(UserId).checked) {
            link = ""Link"";
        } 


        console.log(AggId);
        $.ajax({
            type: ""POST"",
            contentType: ""application/json; charset=utf-8"",
            url: ""/Home/LinkAggregatorToFarmer?UserId="" + UserId + ""&"" + ""AggId="" + AggId + ""&""  +""link="" + link,

            success: function (response) {
                location.reload();


            },
            error: function (error) {
                alert(error);
            }
        });
    }




</script>



");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public SignInManager<ApplicationUser> signInManager { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<IEnumerable<Mtama.Models.ApplicationUser>> Html { get; private set; }
    }
}
#pragma warning restore 1591
