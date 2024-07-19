using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebApplication1_temp.Models;
using e_commerce.Data;

namespace WebApplication1_temp.Pages.Categories
{
    public class IndexModel : PageModel
    {
        private readonly e_commerce.Data.ApplicationDbContext _context;

        public IndexModel(e_commerce.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Category> Category { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Category = await _context.categories.ToListAsync();
        }
    }
}
