using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BrainenceCore_Task.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DiagnosticsViewPage.Views;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BrainenceCore_Task.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationContext _context;
        private IHostingEnvironment _environment;
        public IndexModel(IHostingEnvironment environment, ApplicationContext db)
        {
            _environment = environment;
            _context = db;
        }

        [BindProperty]
        public IFormFile Upload { get; set; }

        [BindProperty]
        public string path { get; set; }

        [BindProperty]
        public Sentences sentences { get; set; }

        [BindProperty]
        public string[] split_text { get; set; }

        public string search_Text { get; set; }
        public string output { get; set; }
        public string[] result { get; set; }

        public async Task OnPost()
        {
            if (Upload != null)
            {
                path = Path.Combine(_environment.ContentRootPath, "App_Data", Upload.FileName);
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                  await  Upload.CopyToAsync(fileStream);
                }
                string text = System.IO.File.ReadAllText(path);//зчитуємо увесь текст з файлу в змінну
                search_Text = Request.Form["search_Text"];
                split_text = text.Split(new char[] { '.' });       // сплітимо текст , розділяючи його на речення (місце від крапки до крапки)
                if (StringHelper.CorrectString(search_Text))           // перевірка на пробіли
                {
                    foreach (string s in split_text)    // починаємо перебирати речення 
                    {
                        string sentence = s;   //так як s - коефіцієнт ітерації, і доступ до його використання обмежений , то створюємо нову змінну
                        sentence += ".";       // спліт видаляє символ , яким розділяє, тому кожному реченню потрібно додати крапку.
                        if ((sentence.ToLower().Contains((search_Text).ToLower() + " ")) ||
                            (sentence.ToLower().Contains((search_Text).ToLower() + ".")) ||
                            (sentence.ToLower().Contains((search_Text).ToLower() + ",")) ||   // перевірка на те , що знайдений текст - слово
                            (sentence.ToLower().Contains((search_Text).ToLower() + "?")) ||   // оскільки після слова заважди йдуть символи, які є в перевірці
                            (sentence.ToLower().Contains((search_Text).ToLower() + "!")) ||   // також робимо шуканий та текст у реченнях нижнього регістру
                            (sentence.ToLower().Contains((search_Text).ToLower() + "...")))   // це дозволить уникнути конфлікту регістрів і зберегти сенс слова  
                        {
                            output += sentence;
                            sentences.text  = StringHelper.ReverseString(sentence);   
                                if (ModelState.IsValid)
                                {

                                    _context.Text.Add(sentences);
                                    await _context.SaveChangesAsync();
                                }
                        }
                    }
                    result = output.Split(new char[] { '.' });
                }
            }
        }
    }

    static class StringHelper
    {
        public static string ReverseString(string s)
        {
            char[] arr = s.ToCharArray();
            Array.Reverse(arr);
            return new string(arr);
        }

        public static bool CorrectString(string b)
        {
            bool chk;
            if ((string.IsNullOrEmpty(b)) || (string.IsNullOrWhiteSpace(b)))
            {
                chk = false;
            }
            else
            {
                chk = true;
            }
            return chk;
        }
    }

}


