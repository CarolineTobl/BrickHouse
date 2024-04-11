using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using BrickHouse.Models.ViewModels;

namespace BrickHouse.Infrastructure
{
    [HtmlTargetElement("div", Attributes = "page-model")]
    public class PaginationTagHelper : TagHelper
    {
        private IUrlHelperFactory urlHelperFactory;

        public PaginationTagHelper(IUrlHelperFactory temp)
        {
            urlHelperFactory = temp;
        }

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext? ViewContext { get; set; }

        public string? PageAction { get; set; }

        public string? CurrentProductType { get; set; }

        public string? CurrentColor { get; set; }

        public int ItemsPerPage { get; set; }

        public int CurrentPage { get; set; }

        public int TotalPages { get; set; }

        public bool PageClassesEnabled { get; set; } = false;

        public string PageClass { get; set; } = String.Empty;
        public string PageClassNormal { get; set; } = String.Empty;
        public string PageClassSelected { get; set; } = String.Empty;

        [HtmlAttributeName(DictionaryAttributePrefix = "page-url-")]
        public Dictionary<string, object> PageUrlValues { get; set; } = new Dictionary<string, object>();

        public PaginationInfo PageModel { get; set; }

        /*public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (ViewContext != null && PageModel != null)
            {
                IUrlHelper urlHelper = urlHelperFactory.GetUrlHelper(ViewContext);

                TagBuilder result = new TagBuilder("div");

                for (int i = 1; i <= PageModel.TotalPages; i++)
                {
                    TagBuilder tag = new TagBuilder("a");
                    PageUrlValues["pageNum"] = i;
                    Console.WriteLine(PageUrlValues["pageNum"]);
                    // Include the Category parameter (ensure it's lowercase)
                    PageUrlValues["category"] = PageModel.CurrentProductType;
                    Console.WriteLine(PageUrlValues["category"]);
                    PageUrlValues["color"] = PageModel.CurrentColor;
                    Console.WriteLine(PageUrlValues["color"]);
                    PageUrlValues["pageSize"] = PageModel.ItemsPerPage;
                    Console.WriteLine(PageUrlValues["pageSize"]);
                    

                    tag.Attributes["href"] = urlHelper.Action(PageAction, PageUrlValues);

                    if (PageClassesEnabled)
                    {
                        tag.AddCssClass(PageClass);
                        tag.AddCssClass(i == PageModel.CurrentPage ? PageClassSelected : PageClassNormal);
                    }
                    tag.InnerHtml.Append(i.ToString());

                    result.InnerHtml.AppendHtml(tag);
                    Console.WriteLine(result);
                }

                output.Content.AppendHtml(result.InnerHtml);
            }*/
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (ViewContext != null && PageModel != null)
            {
                IUrlHelper urlHelper = urlHelperFactory.GetUrlHelper(ViewContext);

                TagBuilder result = new TagBuilder("div");

                for (int i = 1; i <= PageModel.TotalPages; i++)
                {
                    TagBuilder tag = new TagBuilder("a");
                    PageUrlValues["pageNum"] = i;
                    // Include the Category parameter (ensure it's lowercase)
                    PageUrlValues["category"] = PageModel.CurrentProductType;
                    PageUrlValues["color"] = PageModel.CurrentColor;
                    PageUrlValues["pageSize"] = PageModel.ItemsPerPage;

                    tag.Attributes["href"] = urlHelper.Action(PageAction, PageUrlValues);

                    if (PageClassesEnabled)
                    {
                        tag.AddCssClass(PageClass);
                        tag.AddCssClass(i == PageModel.CurrentPage ? PageClassSelected : PageClassNormal);
                    }
                    tag.InnerHtml.Append(i.ToString());

                    result.InnerHtml.AppendHtml(tag);
                }

                output.Content.AppendHtml(result.InnerHtml);
            }
        }

    }
}
