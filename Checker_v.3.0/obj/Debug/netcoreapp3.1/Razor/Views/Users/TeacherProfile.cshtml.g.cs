#pragma checksum "C:\Users\dacky\OneDrive\Рабочий стол\Kursach\2021\Checker\Checker_v.3.0\Views\Users\TeacherProfile.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "bd32f9e7d80dd7bf978d4bc7337e686461e474f9"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Users_TeacherProfile), @"mvc.1.0.view", @"/Views/Users/TeacherProfile.cshtml")]
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
#line 1 "C:\Users\dacky\OneDrive\Рабочий стол\Kursach\2021\Checker\Checker_v.3.0\Views\_ViewImports.cshtml"
using Checker_v._3._0;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\dacky\OneDrive\Рабочий стол\Kursach\2021\Checker\Checker_v.3.0\Views\_ViewImports.cshtml"
using Checker_v._3._0.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "C:\Users\dacky\OneDrive\Рабочий стол\Kursach\2021\Checker\Checker_v.3.0\Views\_ViewImports.cshtml"
using Checker_v._3._0.ViewModels;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"bd32f9e7d80dd7bf978d4bc7337e686461e474f9", @"/Views/Users/TeacherProfile.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"9e5216894ffc3f2ee193a0d81c5a118e23cbf7a5", @"/Views/_ViewImports.cshtml")]
    public class Views_Users_TeacherProfile : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<TeacherViewModel>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n<h2>Преподаватель - ");
#nullable restore
#line 3 "C:\Users\dacky\OneDrive\Рабочий стол\Kursach\2021\Checker\Checker_v.3.0\Views\Users\TeacherProfile.cshtml"
               Write(Model.FullName);

#line default
#line hidden
#nullable disable
            WriteLiteral("</h2>\r\n\r\n<div class=\"TeacherInfo\">\r\n    <div>\r\n        <label id=\"FullName\">Ф.И.О: ");
#nullable restore
#line 7 "C:\Users\dacky\OneDrive\Рабочий стол\Kursach\2021\Checker\Checker_v.3.0\Views\Users\TeacherProfile.cshtml"
                               Write(Model.FullName);

#line default
#line hidden
#nullable disable
            WriteLiteral("</label>\r\n    </div>\r\n    <div>\r\n        <label id=\"Email\">Email: ");
#nullable restore
#line 10 "C:\Users\dacky\OneDrive\Рабочий стол\Kursach\2021\Checker\Checker_v.3.0\Views\Users\TeacherProfile.cshtml"
                            Write(Model.Email);

#line default
#line hidden
#nullable disable
            WriteLiteral("</label>\r\n    </div>\r\n</div>\r\n");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<TeacherViewModel> Html { get; private set; }
    }
}
#pragma warning restore 1591
