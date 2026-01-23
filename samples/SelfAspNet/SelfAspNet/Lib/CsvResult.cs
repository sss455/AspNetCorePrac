using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace SelfAspNet.Lib;

public class CsvResult : ActionResult
{
    readonly IEnumerable<object> _list;

    public CsvResult(IEnumerable<object> list)
    {
        _list = list;
    }

    public override void ExecuteResult(ActionContext context)
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        var res = context.HttpContext.Response;
        res.Headers.ContentType = "text/csv; charset=sjis";
        res.Headers.ContentDisposition = "attachment; filename=\"result.csv\"";
        res.WriteAsync(CreateCSV(_list), Encoding.GetEncoding("Shift-JIS"));
    }

    private static string CreateCSV(IEnumerable<object> list)
    {
        var sb = new StringBuilder();
        foreach (var obj in list)
        {
            var rows = new List<string?>();
            foreach (var prop in obj.GetType().GetProperties())
            {
                var type = prop.PropertyType;
                if (type.IsPrimitive ||
                  type == typeof(String) || type == typeof(DateTime))
                {
                    rows.Add(prop?.GetValue(obj)?.ToString());
                }
            }
            sb.AppendLine(string.Join(",", rows.ToArray()));
        }
        return sb.ToString();
    }
}