using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BrainenceCore_Task.Models
{
    public class Sentences
    {
        [Key]
        public string text { get; set; }
    }
}
