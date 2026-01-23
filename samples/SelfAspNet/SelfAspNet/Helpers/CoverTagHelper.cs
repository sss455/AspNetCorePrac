using Microsoft.AspNetCore.Razor.TagHelpers;

namespace SelfAspNet.Helpers;

[HtmlTargetElement("cover", Attributes="isbn",
  TagStructure = TagStructure.WithoutEndTag)]
public class CoverTagHelper : TagHelper
{
    [HtmlAttributeName("isbn")]
    public string Isbn { get; set; } = string.Empty;

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "img";
        output.Attributes.SetAttribute(
          "src", $"https://wings.msn.to/books/{Isbn}/{Isbn}.jpg");
        output.Attributes.SetAttribute("alt", Isbn);
    }
}