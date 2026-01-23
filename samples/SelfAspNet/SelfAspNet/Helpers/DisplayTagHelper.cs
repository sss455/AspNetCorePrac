using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace SelfAspNet.Helpers;

[HtmlTargetElement("span", Attributes = "asp-for")]
public class ValueDisplayTagHelper : TagHelper
{
    [HtmlAttributeName("asp-for")]
    public ModelExpression? For { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.Content.SetHtmlContent($@"{For?.Name} -> {For?.Model}");
    }
}