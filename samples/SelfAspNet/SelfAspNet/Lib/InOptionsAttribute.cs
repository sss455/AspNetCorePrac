using System.ComponentModel.DataAnnotations;
using System.Globalization;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace SelfAspNet.Lib;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class InOptionsAttribute : ValidationAttribute, IClientModelValidator
{
    private string _options;

    public InOptionsAttribute(string options)
    {
      _options = options;
      ErrorMessage =  "{0} は「{1}」のいずれかの値で指定します。";
    }

    public override string FormatErrorMessage(string name)
    {
      return String.Format(CultureInfo.CurrentCulture,
                           ErrorMessageString, name, _options);
    }

    public override bool IsValid(object? value)
    {
      var v = value as string;
      if (string.IsNullOrEmpty(v)) { return true; }
      if(_options.Split(",").Any(opt => opt.Trim() == v))
      {
        return true;
      }
      return false;
    }

    public void AddValidation(ClientModelValidationContext context)
    {
        var attrs = context.Attributes;
        attrs.TryAdd("data-val", "true");
        attrs.TryAdd("data-val-inoptions",
            FormatErrorMessage(context.ModelMetadata.GetDisplayName()));
        attrs.TryAdd("data-val-inoptions-opts", _options);
    }
}
