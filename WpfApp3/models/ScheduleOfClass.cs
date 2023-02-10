using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp3.models
{
    public class ScheduleOfClass
    {
        [Key]
        public int Id { get; set; }
        public int TeacherAudienceId { get; set; }
        [ForeignKey("TeacherAudienceId")]
        public TeacherAudience TeacherAudience { get; set; }
        public int GroupId { get; set; }
        [ForeignKey("GroupId")]
        public Group Group { get; set; }
        public int DisciplinesId { get; set; }
        [ForeignKey("DisciplinesId")]
        public Discipline Discipline { get; set; }
        public int CouplesId { get; set; }
        [ForeignKey("CouplesId")]
        public Couple Couple { get; set; }
        public int DaysOfTheWeekId { get; set; }
        [ForeignKey("DaysOfTheWeekId")]
        public DayOfTheWeek DayOfTheWeek { get; set; }
        public int FractionId { get; set; }
        [ForeignKey("FractionId")]
        public Fraction Fraction { get; set; }
    }
}
