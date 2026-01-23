using System;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SelfAspNetCore.Models;

public class SelectView
{
    public Book Book { get; set; } = default!;
    public SelectList Publishers { get; set; } = default!;
}
