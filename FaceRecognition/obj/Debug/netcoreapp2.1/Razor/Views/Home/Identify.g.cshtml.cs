#pragma checksum "E:\Asp.net_Tutorial\FaceRecognition\FaceRecognition\Views\Home\Identify.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "c675567509653d5fd3ab6018fd482fa7072cb06f"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Home_Identify), @"mvc.1.0.view", @"/Views/Home/Identify.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/Home/Identify.cshtml", typeof(AspNetCore.Views_Home_Identify))]
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
#line 1 "E:\Asp.net_Tutorial\FaceRecognition\FaceRecognition\Views\_ViewImports.cshtml"
using FaceRecognition;

#line default
#line hidden
#line 2 "E:\Asp.net_Tutorial\FaceRecognition\FaceRecognition\Views\_ViewImports.cshtml"
using FaceRecognition.Models;

#line default
#line hidden
#line 1 "E:\Asp.net_Tutorial\FaceRecognition\FaceRecognition\Views\Home\Identify.cshtml"
using Microsoft.ProjectOxford.Face.Contract;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"c675567509653d5fd3ab6018fd482fa7072cb06f", @"/Views/Home/Identify.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"4971722f4b27d57be4a2cf285f23d1677bcfc16d", @"/Views/_ViewImports.cshtml")]
    public class Views_Home_Identify : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<IdentifyFacesModel>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("name", "_UploadWithPersonGroups", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
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
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.PartialTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_PartialTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            BeginContext(74, 2, true);
            WriteLiteral("\r\n");
            EndContext();
#line 4 "E:\Asp.net_Tutorial\FaceRecognition\FaceRecognition\Views\Home\Identify.cshtml"
  
    ViewBag.Title = "Identify";
    var groups = (PersonGroup[])ViewBag.PersonGroups;

#line default
#line hidden
            BeginContext(171, 126, true);
            WriteLiteral("<style>\r\n    .row { margin-top: 30px; }\r\n</style>\r\n<div class=\"row\">\r\n    <div class=\"col-md-12\">\r\n        <h2>Identify</h2>\r\n");
            EndContext();
#line 14 "E:\Asp.net_Tutorial\FaceRecognition\FaceRecognition\Views\Home\Identify.cshtml"
         if (Model.Error != null)
        {

#line default
#line hidden
            BeginContext(343, 20, true);
            WriteLiteral("            <strong>");
            EndContext();
            BeginContext(364, 11, false);
#line 16 "E:\Asp.net_Tutorial\FaceRecognition\FaceRecognition\Views\Home\Identify.cshtml"
               Write(Model.Error);

#line default
#line hidden
            EndContext();
            BeginContext(375, 11, true);
            WriteLiteral("</strong>\r\n");
            EndContext();
#line 17 "E:\Asp.net_Tutorial\FaceRecognition\FaceRecognition\Views\Home\Identify.cshtml"
        }

#line default
#line hidden
            BeginContext(397, 8, true);
            WriteLiteral("        ");
            EndContext();
            BeginContext(405, 57, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("partial", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "1f7cf15012f949c397da7896f9de8e14", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_PartialTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.PartialTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_PartialTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_PartialTagHelper.Name = (string)__tagHelperAttribute_0.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
#line 18 "E:\Asp.net_Tutorial\FaceRecognition\FaceRecognition\Views\Home\Identify.cshtml"
__Microsoft_AspNetCore_Mvc_TagHelpers_PartialTagHelper.Model = Model;

#line default
#line hidden
            __tagHelperExecutionContext.AddTagHelperAttribute("model", __Microsoft_AspNetCore_Mvc_TagHelpers_PartialTagHelper.Model, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            EndContext();
            BeginContext(462, 2, true);
            WriteLiteral("\r\n");
            EndContext();
#line 19 "E:\Asp.net_Tutorial\FaceRecognition\FaceRecognition\Views\Home\Identify.cshtml"
         if (Model.ImageDump != null)
        {

#line default
#line hidden
            BeginContext(514, 22, true);
            WriteLiteral("            <br /><img");
            EndContext();
            BeginWriteAttribute("src", " src=\"", 536, "\"", 558, 1);
#line 21 "E:\Asp.net_Tutorial\FaceRecognition\FaceRecognition\Views\Home\Identify.cshtml"
WriteAttributeValue("", 542, Model.ImageDump, 542, 16, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(559, 52, true);
            WriteLiteral(" width=\"600\" style=\"border: 1px solid darkgray\" />\r\n");
            EndContext();
            BeginContext(613, 28, true);
            WriteLiteral("            <h3>Faces</h3>\r\n");
            EndContext();
#line 24 "E:\Asp.net_Tutorial\FaceRecognition\FaceRecognition\Views\Home\Identify.cshtml"
            foreach (var face in Model.IdentifiedFaces)
            {

#line default
#line hidden
            BeginContext(713, 20, true);
            WriteLiteral("                <div");
            EndContext();
            BeginWriteAttribute("style", " style=\"", 733, "\"", 826, 5);
            WriteAttributeValue("", 741, "display:inline-block;", 741, 21, true);
            WriteAttributeValue(" ", 762, "width:10px;", 763, 12, true);
            WriteAttributeValue(" ", 774, "height:10px;", 775, 13, true);
            WriteAttributeValue(" ", 787, "background-color:", 788, 18, true);
#line 26 "E:\Asp.net_Tutorial\FaceRecognition\FaceRecognition\Views\Home\Identify.cshtml"
WriteAttributeValue(" ", 805, face.GetHtmlColor(), 806, 20, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(827, 9, true);
            WriteLiteral("></div>\r\n");
            EndContext();
            BeginContext(859, 71, false);
#line 27 "E:\Asp.net_Tutorial\FaceRecognition\FaceRecognition\Views\Home\Identify.cshtml"
                 Write(string.Join(", ", face.PersonCandidates.Select(p => p.Value).ToArray()));

#line default
#line hidden
            EndContext();
            BeginContext(937, 8, true);
            WriteLiteral("<br />\r\n");
            EndContext();
#line 28 "E:\Asp.net_Tutorial\FaceRecognition\FaceRecognition\Views\Home\Identify.cshtml"
            }
        }

#line default
#line hidden
            BeginContext(971, 20, true);
            WriteLiteral("    </div>\r\n</div>\r\n");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<IdentifyFacesModel> Html { get; private set; }
    }
}
#pragma warning restore 1591