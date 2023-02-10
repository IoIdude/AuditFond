using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using m = WpfApp3.models;
using Excel = Microsoft.Office.Interop.Excel;
using WpfApp3.models;

namespace WpfApp3
{
    /// <summary>
    /// Логика взаимодействия для CreateFond.xaml
    /// </summary>
    public partial class CreateFond : Page
    {
        ApplicationContext db;
        ObservableCollection<m.FondEl> memberData;

        public CreateFond()
        {
            InitializeComponent();

            db = new ApplicationContext();
            memberData = new ObservableCollection<m.FondEl>();

            LoadData();
        }

        private void LoadData()
        {
            var day = db.DaysOfTheWeek.ToList();

            dayCb.DisplayMemberPath = "Name";
            dayCb.SelectedValuePath = "Id";
            dayCb.ItemsSource = day;

            var fract = db.Fractions.ToList();

            drobCb.DisplayMemberPath = "Name";
            drobCb.SelectedValuePath = "Id";
            drobCb.ItemsSource = fract;
        }


        private void FormBtn_Click(object sender, RoutedEventArgs e)
        {
            if (memberData.Count != 0)
                memberData.Clear();

            var ScheduleOfClasses = from scheduleOfClasses in db.ScheduleOfClasses
                                    join daysOfTheWeek in db.DaysOfTheWeek on scheduleOfClasses.DaysOfTheWeekId equals daysOfTheWeek.Id
                                    join teacherAudiences in db.TeacherAudiences on scheduleOfClasses.TeacherAudienceId equals teacherAudiences.Id
                                    join teachers in db.Teachers on teacherAudiences.TeacherId equals teachers.Id
                                    join fractionId in db.Fractions on scheduleOfClasses.FractionId equals fractionId.Id
                                    join audesa in db.TeacherAudiences on scheduleOfClasses.TeacherAudienceId equals audesa.Id
                                    join groups in db.Groups on scheduleOfClasses.GroupId equals groups.Id
                                    join subdv in db.Subdivisions on groups.SubdivisionId equals subdv.Id
                                    join couple in db.Couples on scheduleOfClasses.CouplesId equals couple.Id
                                    join discipline in db.Disciplines on scheduleOfClasses.DisciplinesId equals discipline.Id
                                    select new
                                    {
                                        Id = scheduleOfClasses.Id,
                                        DrobId = daysOfTheWeek.Id,
                                        TeachAudId = teacherAudiences.Id,
                                        WeekId = fractionId.Id,
                                        TeacherId = teachers.Id,
                                        GroupId = groups.Id,
                                        SubdvId = subdv.Id,
                                        Fraction = fractionId.Name,
                                        DayWeek = daysOfTheWeek.Name,
                                        Subdv = subdv.Number,
                                        Group = groups.Name,
                                        Number = audesa.Number,
                                        Surname = teachers.LastName,
                                        Name = teachers.Name,
                                        LastName = teachers.Surname,
                                        Couple = couple.Number,
                                        Discipline = discipline.Name,
                                        CoupleId = couple.Id,
                                        DisciplineId = discipline.Id
                                    };

            if (drobCb.SelectedIndex == -1 && dayCb.SelectedIndex == -1)
            {
                foreach (var item in ScheduleOfClasses)
                    memberData.Add(new m.FondEl
                    {
                        Id = item.Id,
                        DrobId = item.DrobId,
                        WeekId = item.WeekId,
                        CoupleId = item.CoupleId,
                        GroupId = item.GroupId,
                        SubdvId = item.SubdvId,
                        Fraction = item.Fraction,
                        DayWeek = item.DayWeek,
                        Subdv = item.Subdv,
                        Group = item.Group,
                        Number = item.Number,
                        Couple = item.Couple
                    });
            }
            else
            {
                m.DayOfTheWeek selectedDay = dayCb.SelectedItem as m.DayOfTheWeek;
                m.Fraction selectedDrob = drobCb.SelectedItem as m.Fraction;

                foreach (var item in ScheduleOfClasses)
                    if (item.DayWeek == selectedDay.Name && item.Fraction == selectedDrob.Name)
                    { 
                        memberData.Add(new m.FondEl
                        {
                            Id = item.Id,
                            DrobId = item.DrobId,
                            WeekId = item.WeekId,
                            CoupleId = item.CoupleId,
                            GroupId = item.GroupId,
                            SubdvId = item.SubdvId,
                            Fraction = item.Fraction,
                            DayWeek = item.DayWeek,
                            Subdv = item.Subdv,
                            Group = item.Group,
                            Number = item.Number,
                            Couple = item.Couple
                        });
                    }
            }

            Grid.DataContext = memberData;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void refreshBtn_Click(object sender, RoutedEventArgs e)
        {
            drobCb.SelectedIndex = -1;
            dayCb.SelectedIndex = -1;

            if (memberData.Count != 0)
                memberData.Clear();
        }

        public static DataTable DataViewAsDataTable(DataView dv)
        {
            DataTable dt = dv.Table.Clone();

            foreach (DataRowView drv in dv)
                dt.ImportRow(drv.Row);

            return dt;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (memberData.Count == 0)
            {
                MessageBox.Show("Сформируйте аудиторный фонд");
                return;
            }

            Excel.Application excel = null;
            Excel.Workbook wb = null;

            object missing = Type.Missing;
            Excel.Worksheet ws = null;
            Excel.Range rng = null;

            excel = new Excel.Application();
            wb = excel.Workbooks.Add();
            ws = (Excel.Worksheet)wb.ActiveSheet;

            List<Subdivision> sub = db.Subdivisions.ToList();
            List<m.Group> gr = db.Groups.ToList();
            List<DayOfTheWeek> day = db.DaysOfTheWeek.ToList();
            List<m.Fraction> week = db.Fractions.ToList();
            List<m.Couple> couples = db.Couples.ToList();

            int index_gr = 0;
            int index_sub = 0;
            int start = 0;


            for (int f = 0; f < week.Count(); f++)
            {
                for (int d = 0; d < day.Count(); d++)
                {
                    for (int k = 0; k < sub.Count(); k++)
                    {
                        for (int i = 0; i < gr.Count(); i++)
                        {
                            for (int g = 0; g < memberData.Count; g++)
                            {
                                if (memberData[g].DayWeek == day[d].Name && memberData[g].Fraction == week[f].Name)
                                {
                                    ws.Range["A1"].Offset[2 + start, 0].Value = "Пара";
                                    ws.Range["A1"].Offset[0 + start, 0].Value = "День: ";
                                    ws.Range["D1"].Offset[0 + start, 0].Value = "Неделя: ";
                                    ws.Range["B1"].Offset[0 + start, 0].Value = memberData[g].DayWeek;
                                    ws.Range["E1"].Offset[0 + start, 0].Value = memberData[g].Fraction;

                                    if (memberData[g].Subdv == sub[k].Number)
                                    {
                                        ws.Range["B1"].Offset[1 + start, index_sub].Value = memberData[g].Subdv;

                                        if (memberData[g].Group == gr[i].Name)
                                        {
                                            for (int p = 0; p < couples.Count(); p++)
                                            {
                                                ws.Range["A1"].Offset[3 + p + start, 0].Value = couples[p].Number;

                                                if (memberData[g].Couple == couples[p].Number)
                                                {
                                                    ws.Range["B1"].Offset[3 + p + start, index_gr].Value = memberData[g].Number;
                                                }
                                            }

                                            ws.Range["B1"].Offset[2 + start, index_gr].Value = memberData[g].Group;
                                        }
                                    }
                                }

                                if (g == memberData.Count() - 1)
                                {
                                    int q = 1 + i;

                                    if (q < gr.Count())
                                    {
                                        if (gr[q].SubdivisionId == sub[k].Id)
                                        {
                                            index_gr++;
                                        }
                                    }
                                }
                            }

                            if (i == gr.Count() - 1)
                            {
                                index_sub = index_gr + 1;
                            }
                        }
                    }

                    index_sub = 0;
                    index_gr = 0;
                    start += 11;

                    int check_d = d + 1;
                    if (check_d < day.Count())
                    {
                        var result = memberData.FirstOrDefault(s => s.DayWeek == day[check_d].Name && s.Fraction == week[f].Name);

                        if (result == null)
                            d++;
                    }
                }
            }

            excel.Visible = true;
            wb.Activate();
        }
    }
}
