using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp3.models
{
    public class Subdivision
    {
        [Key]
        public int Id { get; set; }
        public string Number { get; set; }
    }
}
