// <auto-generated/>
#pragma warning disable 1591
#pragma warning disable 0414
#pragma warning disable 0649
#pragma warning disable 0169

namespace DataCalculator_New.Shared
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
#nullable restore
#line 1 "C:\Users\aj_ch\Documents\College\CSCI 4950\DataCaculatorPrototype\DataCalculator1\DataCalculator_New\Shared\Dropdown.razor"
using Microsoft.AspNetCore.Components.Web;

#line default
#line hidden
#nullable disable
    public partial class Dropdown<TItem> : Microsoft.AspNetCore.Components.ComponentBase
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
        }
        #pragma warning restore 1998
#nullable restore
#line 16 "C:\Users\aj_ch\Documents\College\CSCI 4950\DataCaculatorPrototype\DataCalculator1\DataCalculator_New\Shared\Dropdown.razor"
       
    [Parameter]
    public RenderFragment InitialTip {get; set;}
    [Parameter]
    public RenderFragment ChildContent {get; set;}
    [Parameter]
    public EventCallback<TItem> OnSelected {get; set;}

    private bool show = false;
    private RenderFragment Tip;

    protected override void OnInitialized()
    {
        this.Tip = InitialTip;
    }

    public async Task HandleSelect(TItem item, RenderFragment<TItem> contentFragment)
    {
        this.Tip = contentFragment.Invoke(item);
        this.show = false;
        StateHasChanged();
        await this.OnSelected.InvokeAsync(item);
    }

#line default
#line hidden
#nullable disable
    }
}
#pragma warning restore 1591
