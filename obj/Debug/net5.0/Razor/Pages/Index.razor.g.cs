#pragma checksum "C:\Users\aj_ch\Documents\College\CSCI 4950\DataCaculatorPrototype\DataCalculator1\DataCalculator_New\Pages\Index.razor" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "234301cfbb7b9dc9c4d61247471e7a1caf7bbb51"
// <auto-generated/>
#pragma warning disable 1591
namespace DataCalculator_New.Pages
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Components;
#nullable restore
#line 1 "C:\Users\aj_ch\Documents\College\CSCI 4950\DataCaculatorPrototype\DataCalculator1\DataCalculator_New\_Imports.razor"
using System.Net.Http;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\aj_ch\Documents\College\CSCI 4950\DataCaculatorPrototype\DataCalculator1\DataCalculator_New\_Imports.razor"
using Microsoft.AspNetCore.Authorization;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "C:\Users\aj_ch\Documents\College\CSCI 4950\DataCaculatorPrototype\DataCalculator1\DataCalculator_New\_Imports.razor"
using Microsoft.AspNetCore.Components.Authorization;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "C:\Users\aj_ch\Documents\College\CSCI 4950\DataCaculatorPrototype\DataCalculator1\DataCalculator_New\_Imports.razor"
using Microsoft.AspNetCore.Components.Forms;

#line default
#line hidden
#nullable disable
#nullable restore
#line 5 "C:\Users\aj_ch\Documents\College\CSCI 4950\DataCaculatorPrototype\DataCalculator1\DataCalculator_New\_Imports.razor"
using Microsoft.AspNetCore.Components.Routing;

#line default
#line hidden
#nullable disable
#nullable restore
#line 6 "C:\Users\aj_ch\Documents\College\CSCI 4950\DataCaculatorPrototype\DataCalculator1\DataCalculator_New\_Imports.razor"
using Microsoft.AspNetCore.Components.Web;

#line default
#line hidden
#nullable disable
#nullable restore
#line 7 "C:\Users\aj_ch\Documents\College\CSCI 4950\DataCaculatorPrototype\DataCalculator1\DataCalculator_New\_Imports.razor"
using Microsoft.AspNetCore.Components.Web.Virtualization;

#line default
#line hidden
#nullable disable
#nullable restore
#line 8 "C:\Users\aj_ch\Documents\College\CSCI 4950\DataCaculatorPrototype\DataCalculator1\DataCalculator_New\_Imports.razor"
using Microsoft.JSInterop;

#line default
#line hidden
#nullable disable
#nullable restore
#line 9 "C:\Users\aj_ch\Documents\College\CSCI 4950\DataCaculatorPrototype\DataCalculator1\DataCalculator_New\_Imports.razor"
using DataCalculator_New;

#line default
#line hidden
#nullable disable
#nullable restore
#line 10 "C:\Users\aj_ch\Documents\College\CSCI 4950\DataCaculatorPrototype\DataCalculator1\DataCalculator_New\_Imports.razor"
using DataCalculator_New.Shared;

#line default
#line hidden
#nullable disable
    [Microsoft.AspNetCore.Components.RouteAttribute("/")]
    public partial class Index : Microsoft.AspNetCore.Components.ComponentBase
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
            __builder.AddMarkupContent(0, "<h1>Data Processing Calculator</h1>\r\n\r\nWelcome to our Data Processing Calculator.\r\n\r\n");
            __builder.OpenComponent<DataCalculator_New.Shared.SurveyPrompt>(1);
            __builder.AddAttribute(2, "Title", "How is Blazor working for you?");
            __builder.CloseComponent();
        }
        #pragma warning restore 1998
    }
}
#pragma warning restore 1591
