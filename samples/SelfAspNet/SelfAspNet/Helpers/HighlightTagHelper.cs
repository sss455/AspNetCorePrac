using Microsoft.AspNetCore.Razor.TagHelpers;

namespace SelfAspNet.Helpers;

// [HtmlTargetElement("highlight")]
[HtmlTargetElement(Attributes = "asp-highlight")]
// [HtmlTargetElement("highlight", Attributes = "asp-highlight")]
public class HighlightTagHelper : TagHelper
{
    [HtmlAttributeName("asp-highlight")]
    public string? BackgroundColor { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        if (output.TagName == "highlight") { output.TagName = "span"; }
        output.Attributes.Add("style",
          $"background-color: {BackgroundColor ?? "#ff0"}");
    }
}
