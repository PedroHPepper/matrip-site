#pragma checksum "C:\Users\Cliente\OneDrive\Área de Trabalho\Projetos\Projeto Matrip\Matrip\Matrip.Web\Areas\Guide\Views\Trip\ViewTripList.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "ab1e68943d075da614535c19edc08b2e82fb9065"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Areas_Guide_Views_Trip_ViewTripList), @"mvc.1.0.view", @"/Areas/Guide/Views/Trip/ViewTripList.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Areas/Guide/Views/Trip/ViewTripList.cshtml", typeof(AspNetCore.Areas_Guide_Views_Trip_ViewTripList))]
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
#line 3 "C:\Users\Cliente\OneDrive\Área de Trabalho\Projetos\Projeto Matrip\Matrip\Matrip.Web\Areas\Guide\Views\_ViewImports.cshtml"
using X.PagedList.Mvc.Core;

#line default
#line hidden
#line 4 "C:\Users\Cliente\OneDrive\Área de Trabalho\Projetos\Projeto Matrip\Matrip\Matrip.Web\Areas\Guide\Views\_ViewImports.cshtml"
using X.PagedList;

#line default
#line hidden
#line 6 "C:\Users\Cliente\OneDrive\Área de Trabalho\Projetos\Projeto Matrip\Matrip\Matrip.Web\Areas\Guide\Views\_ViewImports.cshtml"
using Matrip;

#line default
#line hidden
#line 7 "C:\Users\Cliente\OneDrive\Área de Trabalho\Projetos\Projeto Matrip\Matrip\Matrip.Web\Areas\Guide\Views\_ViewImports.cshtml"
using Matrip.Domain.Models;

#line default
#line hidden
#line 8 "C:\Users\Cliente\OneDrive\Área de Trabalho\Projetos\Projeto Matrip\Matrip\Matrip.Web\Areas\Guide\Views\_ViewImports.cshtml"
using Matrip.Domain.Models.Entities;

#line default
#line hidden
#line 9 "C:\Users\Cliente\OneDrive\Área de Trabalho\Projetos\Projeto Matrip\Matrip\Matrip.Web\Areas\Guide\Views\_ViewImports.cshtml"
using Matrip.Domain.Models.AccountModels;

#line default
#line hidden
#line 10 "C:\Users\Cliente\OneDrive\Área de Trabalho\Projetos\Projeto Matrip\Matrip\Matrip.Web\Areas\Guide\Views\_ViewImports.cshtml"
using Matrip.Domain.Models.HomeModels;

#line default
#line hidden
#line 11 "C:\Users\Cliente\OneDrive\Área de Trabalho\Projetos\Projeto Matrip\Matrip\Matrip.Web\Areas\Guide\Views\_ViewImports.cshtml"
using Matrip.Domain.Models.Payment;

#line default
#line hidden
#line 12 "C:\Users\Cliente\OneDrive\Área de Trabalho\Projetos\Projeto Matrip\Matrip\Matrip.Web\Areas\Guide\Views\_ViewImports.cshtml"
using Matrip.Domain.Models.TripModel;

#line default
#line hidden
#line 13 "C:\Users\Cliente\OneDrive\Área de Trabalho\Projetos\Projeto Matrip\Matrip\Matrip.Web\Areas\Guide\Views\_ViewImports.cshtml"
using Matrip.Domain.Models.TripPurchase;

#line default
#line hidden
#line 14 "C:\Users\Cliente\OneDrive\Área de Trabalho\Projetos\Projeto Matrip\Matrip\Matrip.Web\Areas\Guide\Views\_ViewImports.cshtml"
using Matrip.Domain.Libraries.Text;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"ab1e68943d075da614535c19edc08b2e82fb9065", @"/Areas/Guide/Views/Trip/ViewTripList.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"1e473379225778e9bbc498bb3b5e73f55afe885e", @"/Areas/Guide/Views/_ViewImports.cshtml")]
    public class Areas_Guide_Views_Trip_ViewTripList : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-action", "", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        #line hidden
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
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
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            BeginContext(0, 2, true);
            WriteLiteral("\r\n");
            EndContext();
#line 2 "C:\Users\Cliente\OneDrive\Área de Trabalho\Projetos\Projeto Matrip\Matrip\Matrip.Web\Areas\Guide\Views\Trip\ViewTripList.cshtml"
  
    ViewData["Title"] = "Passeios";
    Layout = "~/Areas/Administrator/Views/Shared/_Layout.cshtml";

#line default
#line hidden
            BeginContext(113, 38, true);
            WriteLiteral("\r\n<h1>Lista dos Seus Passeios</h1>\r\n\r\n");
            EndContext();
#line 9 "C:\Users\Cliente\OneDrive\Área de Trabalho\Projetos\Projeto Matrip\Matrip\Matrip.Web\Areas\Guide\Views\Trip\ViewTripList.cshtml"
 foreach (ma05trip ma05trip in Model)
{
    

#line default
#line hidden
            BeginContext(198, 17, false);
#line 11 "C:\Users\Cliente\OneDrive\Área de Trabalho\Projetos\Projeto Matrip\Matrip\Matrip.Web\Areas\Guide\Views\Trip\ViewTripList.cshtml"
Write(ma05trip.ma05name);

#line default
#line hidden
            EndContext();
            BeginContext(217, 252, true);
            WriteLiteral("    <br />\r\n    <div class=\"results_container\">\r\n        <!-- Result Item -->\r\n        <div class=\"grid-item result coffee\">\r\n            <div class=\"listing\">\r\n                <div class=\"listing_image\">\r\n                    <div class=\"listing_icon\">");
            EndContext();
            BeginContext(469, 40, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "ab1e68943d075da614535c19edc08b2e82fb90656820", async() => {
                BeginContext(486, 19, true);
                WriteLiteral("<img src=\"\" alt=\"\">");
                EndContext();
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Action = (string)__tagHelperAttribute_0.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            EndContext();
            BeginContext(509, 1149, true);
            WriteLiteral(@"</div>
                    <img src=""images/listing_1.jpg"" alt="""">
                </div>
                <div class=""listing_title_container"">
                    <div class=""listing_title""><a href=""listing.html"">The Meal</a></div>
                    <div class=""listing_info d-flex flex-row align-items-center justify-content-between"">

                        <div class=""listing_price"">$$$</div>
                        <div class=""listing_divider"">|</div>
                        <div class=""listing_type"">Restaurant</div>
                        <div class=""listing_divider"">|</div>
                        <div class=""listing_status"">Closed</div>
                    </div>
                </div>
                <div class=""listing_testimonial"">
                    <div class=""d-flex flex-row align-items-center justify-content-start"">

                        <div class=""testimonial_text"">
                            <p>Great place to visit, the food is awesome, I really love it.</p>
       ");
            WriteLiteral("                 </div>\r\n                    </div>\r\n                </div>\r\n            </div>\r\n        </div>\r\n    </div>\r\n");
            EndContext();
#line 43 "C:\Users\Cliente\OneDrive\Área de Trabalho\Projetos\Projeto Matrip\Matrip\Matrip.Web\Areas\Guide\Views\Trip\ViewTripList.cshtml"

}

#line default
#line hidden
            BeginContext(1663, 4, true);
            WriteLiteral("\r\n\r\n");
            EndContext();
            BeginContext(1668, 81, false);
#line 47 "C:\Users\Cliente\OneDrive\Área de Trabalho\Projetos\Projeto Matrip\Matrip\Matrip.Web\Areas\Guide\Views\Trip\ViewTripList.cshtml"
Write(Html.PagedListPager((IPagedList)Model, page => Url.Action("Index", new { page })));

#line default
#line hidden
            EndContext();
            BeginContext(1749, 4, true);
            WriteLiteral("\r\n\r\n");
            EndContext();
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; }
    }
}
#pragma warning restore 1591