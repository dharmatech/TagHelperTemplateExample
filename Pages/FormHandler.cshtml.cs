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
        private ModelExpressionProvider _modelExpressionProvider { get; set; }

        private DataContext context;

        public FormHandlerModel(DataContext dbContext, ModelExpressionProvider modelExpressionProvider)
        {
            context = dbContext;
            _modelExpressionProvider = modelExpressionProvider;
        }

        public ModelExpression CreateModelExpression(string expr)
        {
            var dict = new ViewDataDictionary<FormHandlerModel>(ViewData);
            
            return _modelExpressionProvider.CreateModelExpression(dict, expr);
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
}
