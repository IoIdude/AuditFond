using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp3.models
{
    public class TeacherAudienceEl
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public int TeacherId { get; set; }
        public string Surname { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
    }
}
