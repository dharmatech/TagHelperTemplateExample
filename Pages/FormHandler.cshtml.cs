using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using WebApp.Models;

namespace WebApp.Pages
{
    public class FormHandlerModel : PageModel
    {
        public ModelExpressionProvider ModelExpressionProvider { get; set; }

        private DataContext context;

        public FormHandlerModel(DataContext dbContext, ModelExpressionProvider modelExpressionProvider)
        {
            context = dbContext;
            ModelExpressionProvider = modelExpressionProvider;
        }

        public ModelExpression create(string expr)
        {
            // var abc = ViewData;

            //var xyz = new ViewDataDictionary<string>(ViewData);

            var xyz = new ViewDataDictionary<FormHandlerModel>(ViewData);

            return ModelExpressionProvider.CreateModelExpression(xyz, expr);
        }

        public Product Product { get; set; }

        public async Task OnGetAsync(long id = 1)
        {
            Product = await context.Products.Include(p => p.Category)
                .Include(p => p.Supplier).FirstAsync(p => p.ProductId == id);
        }

        public IActionResult OnPost()
        {
            foreach (string key in Request.Form.Keys
                    .Where(k => !k.StartsWith("_")))
            {
                TempData[key] = string.Join(", ", Request.Form[key]);
            }
            return RedirectToPage("FormResults");
        }
    }

    //public class FormHandlerModelAlt : PageModel
    //{
    //    public ModelExpressionProvider ModelExpressionProvider { get; set; }

    //    public FormHandlerModel(ModelExpressionProvider modelExpressionProvider)
    //    {
    //        ModelExpressionProvider = modelExpressionProvider;
    //    }

    //    public ModelExpression create(string expr)
    //    {
    //        // var abc = ViewData;

    //        var xyz = new ViewDataDictionary<string>(ViewData);

    //        return ModelExpressionProvider.CreateModelExpression(xyz, expr);
    //    }

    //    public void OnGet()
    //    {
    //    }
    //}
}
