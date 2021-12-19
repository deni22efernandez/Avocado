using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Avocado.WEB.CustomTagHelper
{
	[HtmlTargetElement("div", Attributes = "pagination")]
	public class PaginationTagHelper : TagHelper
	{
		public PaginationModel pagination { get; set; }
		public bool ClasesEnabled { get; set; }
		public string PageClass { get; set; }
		public string PageClassNormal { get; set; }
		public string PageClassSelected { get; set; }
		public override void Process(TagHelperContext context, TagHelperOutput output)
		{
			TagBuilder result = new TagBuilder("div");
			for (int i = 1; i <= pagination.TotalPages; i++)
			{
				TagBuilder ancorTag = new TagBuilder("a");
				string url = pagination.Uri.Replace(":", i.ToString());
				ancorTag.Attributes["href"] = url;
				if (ClasesEnabled)
				{
					ancorTag.AddCssClass(PageClass);
					ancorTag.AddCssClass(i == pagination.CurrentPage? PageClassSelected : PageClassNormal);
				}
				ancorTag.InnerHtml.Append(i.ToString());
				result.InnerHtml.AppendHtml(ancorTag);
			}
			output.Content.AppendHtml(result.InnerHtml);
		}
	}
}
