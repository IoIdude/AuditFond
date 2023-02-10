using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp3.models
{
    public class ScheduleOfClassEl
    {
        public int Id { get; set; }
        public int DrobId { get; set; }
        public int WeekId { get; set; }
        public int TeacherId { get; set; }
        public int GroupId { get; set; }
        public int DisciplineId { get; set; }
        public int CoupleId { get; set; }
        public int SubdvId { get; set; }
        public string Fraction { get; set; }
        public string DayWeek { get; set; }
        public string Subdv { get; set; }
        public string Group { get; set; }
        public int Number { get; set; }
        public string Surname { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public int TeachAudId { get; set; }
        public string Discipline { get; set; }
        public int Couple { get; set; }
    }
}
