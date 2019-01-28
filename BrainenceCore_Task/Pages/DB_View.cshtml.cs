using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BrainenceCore_Task.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BrainenceCore_Task.Pages
{
    public class DB_ViewModel : PageModel
    {
        private readonly ApplicationContext _context;
        public List<Sentences> Text { get; set; }
        public DB_ViewModel(ApplicationContext db)
        {
            _context = db;
        }
        public void OnGet()
        {
            Text = _context.Text.ToList();
        }
    }
}