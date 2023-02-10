using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using m = WpfApp3.models;

namespace WpfApp3
{
    /// <summary>
    /// Логика взаимодействия для Table.xaml
    /// </summary>
    public partial class Table : Page
    {
        ApplicationContext db;
        ObservableCollection<m.ScheduleOfClassEl> memberData;
        ObservableCollection<m.GroupEl> grData;
        ObservableCollection<m.TeacherAudienceEl> teachData;

        public Table()
        {
            InitializeComponent();

            db = new ApplicationContext();
            memberData = new ObservableCollection<m.ScheduleOfClassEl>();
            grData = new ObservableCollection<m.GroupEl>();
            teachData = new ObservableCollection<m.TeacherAudienceEl>();

            LoadData();
        }

        private void LoadData()
        {
            if (memberData != null)
                memberData.Clear();
            if (grData != null)
                grData.Clear();
            if (teachData != null)
                teachData.Clear();

            var ScheduleOfClasses = from scheduleOfClasses in db.ScheduleOfClasses
                                    join days in db.DaysOfTheWeek on scheduleOfClasses.DaysOfTheWeekId equals days.Id
                                    join teacherAudiences in db.TeacherAudiences on scheduleOfClasses.TeacherAudienceId equals teacherAudiences.Id
                                    join teachers in db.Teachers on teacherAudiences.TeacherId equals teachers.Id
                                    join drobes in db.Fractions on scheduleOfClasses.FractionId equals drobes.Id
                                    join audesa in db.TeacherAudiences on scheduleOfClasses.TeacherAudienceId equals audesa.Id
                                    join groups in db.Groups on scheduleOfClasses.GroupId equals groups.Id
                                    join subdv in db.Subdivisions on groups.SubdivisionId equals subdv.Id
                                    join couple in db.Couples on scheduleOfClasses.CouplesId equals couple.Id
                                    join discipline in db.Disciplines on scheduleOfClasses.DisciplinesId equals discipline.Id
                                    select new
                                    {
                                        Id = scheduleOfClasses.Id,
                                        DrobId = drobes.Id,
                                        TeachAudId = teacherAudiences.Id,
                                        WeekId = days.Id,
                                        TeacherId = teachers.Id,
                                        GroupId = groups.Id,
                                        SubdvId = subdv.Id,
                                        Fraction = drobes.Name,
                                        DayWeek = days.Name,
                                        Subdv = subdv.Number,
                                        Group = groups.Name,
                                        Number = audesa.Number,
                                        Surname = teachers.Surname,
                                        Name = teachers.Name,
                                        LastName = teachers.LastName,
                                        Couple = couple.Number,
                                        Discipline = discipline.Name,
                                        CoupleId = couple.Id,
                                        DisciplineId = discipline.Id
                                    };

            foreach (var item in ScheduleOfClasses)
                memberData.Add(new m.ScheduleOfClassEl
                {
                    Id = item.Id,
                    DrobId = item.DrobId,
                    WeekId = item.WeekId,
                    CoupleId = item.CoupleId,
                    DisciplineId = item.DisciplineId,
                    TeacherId = item.TeacherId,
                    GroupId = item.GroupId,
                    SubdvId = item.SubdvId,
                    Fraction = item.Fraction,
                    DayWeek = item.DayWeek,
                    Subdv = item.Subdv,
                    Group = item.Group,
                    Number = item.Number,
                    Surname = item.Surname,
                    Name = item.Name,
                    LastName = item.LastName,
                    TeachAudId = item.TeachAudId,
                    Couple = item.Couple,
                    Discipline = item.Discipline
                });

            Grid.DataContext = memberData;


            var group = from gr in db.Groups
                        join subid in db.Subdivisions on gr.SubdivisionId equals subid.Id
                        select new
                        {
                            Id = gr.Id,
                            Name = gr.Name,
                            SubId = subid.Id,
                            Sub = subid.Number
                        };

            foreach (var item in group)
                grData.Add(new m.GroupEl
                {
                    Id = item.Id,
                    Name = item.Name,
                    SubdivisionId = item.SubId,
                    SubNumber = item.Sub
                });

            groupCb.DisplayMemberPath = "Name";
            groupCb.SelectedValuePath = "Id";
            groupCb.ItemsSource = grData.ToList();

            var data = from teacherAudiences in db.TeacherAudiences
                       join teachers in db.Teachers on teacherAudiences.TeacherId equals teachers.Id
                       select new
                       {
                           Id = teacherAudiences.Id,
                           TeacherId = teachers.Id,
                           LastName = teachers.LastName,
                           Name = teachers.Name,
                           Surname = teachers.Surname,
                           Number = teacherAudiences.Number
                       };

            foreach (var item in data)
                teachData.Add(new m.TeacherAudienceEl
                {
                    Id = item.Id,
                    Name = item.Surname + " " + item.Name + " " + item.LastName,
                    TeacherId= item.TeacherId,
                    Surname = item.Surname,
                    LastName = item.LastName,
                    Number = item.Number
                });

            teachCb.DisplayMemberPath = "Name";
            teachCb.SelectedValuePath = "Id";
            teachCb.ItemsSource = teachData;

            disCb.DisplayMemberPath = "Name";
            disCb.SelectedValuePath = "Id";
            disCb.ItemsSource = db.Disciplines.ToList();

            coupleCb.DisplayMemberPath = "Number";
            coupleCb.SelectedValuePath = "Id";
            coupleCb.ItemsSource = db.Couples.ToList();

            weekCb.DisplayMemberPath = "Name";
            weekCb.SelectedValuePath = "Id";
            weekCb.ItemsSource = db.DaysOfTheWeek.ToList();

            dayCb.DisplayMemberPath = "Name";
            dayCb.SelectedValuePath = "Id";
            dayCb.ItemsSource = db.Fractions.ToList();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Teacher());
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Audit());
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Disciplinesp());
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Group());
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Podr());
        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            if (Grid.SelectedItem == null)
            {
                MessageBox.Show("Выберите элемент из списка для удаления");
                return;
            }

            if (groupCb.SelectedIndex > -1)
                groupCb.SelectedIndex = -1;
            if (teachCb.SelectedIndex > -1)
                teachCb.SelectedIndex = -1;
            if (disCb.SelectedIndex > -1)
                disCb.SelectedIndex = -1;
            if (coupleCb.SelectedIndex > -1)
                coupleCb.SelectedIndex = -1;
            if (weekCb.SelectedIndex > -1)
                weekCb.SelectedIndex = -1;
            if (dayCb.SelectedIndex > -1)
                dayCb.SelectedIndex = -1;

            m.ScheduleOfClassEl items = Grid.SelectedItem as m.ScheduleOfClassEl;

            m.ScheduleOfClass cur = db.ScheduleOfClasses.FirstOrDefault(c => c.Id == items.Id);

            db.ScheduleOfClasses.Remove(cur);
            db.SaveChanges();

            LoadData();
        }

        private void UpdateBtn_Click(object sender, RoutedEventArgs e)
        {
            if (Grid.SelectedItem == null)
            {
                MessageBox.Show("Выберите элемент из списка для удаления");
                return;
            }

            if (groupCb.SelectedIndex > -1 && teachCb.SelectedIndex > -1 && weekCb.SelectedIndex > -1 && dayCb.SelectedIndex > -1 && disCb.SelectedIndex > -1 && coupleCb.SelectedIndex > -1)
            {
                m.TeacherAudienceEl selectedTeachAud = teachCb.SelectedItem as m.TeacherAudienceEl;
                var curTeachAud = db.TeacherAudiences.FirstOrDefault(p => p.Id == selectedTeachAud.Id);
                m.GroupEl selectedGroup = groupCb.SelectedItem as m.GroupEl;
                var curGroup = db.Groups.FirstOrDefault(p => p.Id == selectedGroup.Id);
                m.Discipline selectedDiscipline = disCb.SelectedItem as m.Discipline;
                var curDiscipline = db.Disciplines.FirstOrDefault(p => p.Id == selectedDiscipline.Id);
                m.Couple selectedCouple = coupleCb.SelectedItem as m.Couple;
                var curCouple = db.Couples.FirstOrDefault(p => p.Id == selectedCouple.Id);
                m.DayOfTheWeek selectedDayOfTheWeek = weekCb.SelectedItem as m.DayOfTheWeek;
                var curDayOfTheWeek = db.DaysOfTheWeek.FirstOrDefault(p => p.Id == selectedDayOfTheWeek.Id);
                m.Fraction selectedFraction = dayCb.SelectedItem as m.Fraction;
                var curFraction = db.Fractions.FirstOrDefault(p => p.Id == selectedFraction.Id);

                //var uniqField = db.ScheduleOfClasses.FirstOrDefault(p => p.GroupId == curGroup.Id && p.DaysOfTheWeekId == curDayOfTheWeek.Id && p.FractionId == curFraction.Id);

                //if (uniqField != null)
                //{
                //    MessageBox.Show("Такая запись уже существует");
                //    return;
                //}

                m.ScheduleOfClassEl curAud = Grid.SelectedItem as m.ScheduleOfClassEl;
                var aud = db.ScheduleOfClasses.FirstOrDefault(p => p.Id == curAud.Id);

                aud.TeacherAudience = curTeachAud;
                aud.Group = curGroup;
                aud.Couple = curCouple;
                aud.Discipline = curDiscipline;
                aud.Fraction = curFraction;
                aud.DayOfTheWeek = curDayOfTheWeek;

                db.ScheduleOfClasses.Update(aud);
                db.SaveChanges();

                LoadData();

                if (groupCb.BorderBrush == Brushes.Red)
                    groupCb.BorderBrush = Brushes.LightGray;
                if (teachCb.BorderBrush == Brushes.Red)
                    teachCb.BorderBrush = Brushes.LightGray;

                groupCb.SelectedIndex = -1;
                teachCb.SelectedIndex = -1;
                disCb.SelectedIndex = -1;
                coupleCb.SelectedIndex = -1;
                dayCb.SelectedIndex = -1;
                weekCb.SelectedIndex = -1;
            }
            else
            {
                if (groupCb.SelectedIndex > -1)
                    groupCb.BorderBrush = Brushes.Red;
                if (disCb.SelectedIndex > -1)
                    disCb.BorderBrush = Brushes.Red;
                if (teachCb.SelectedIndex > -1)
                    teachCb.BorderBrush = Brushes.Red;
                if (coupleCb.SelectedIndex > -1)
                    coupleCb.BorderBrush = Brushes.Red;
                if (weekCb.SelectedIndex > -1)
                    weekCb.BorderBrush = Brushes.Red;
                if (dayCb.SelectedIndex > -1)
                    dayCb.BorderBrush = Brushes.Red;
            }
        }

        private void CraeteBtn_Click(object sender, RoutedEventArgs e)
        {
            if (groupCb.SelectedIndex > -1 && teachCb.SelectedIndex > -1 && weekCb.SelectedIndex > -1 && dayCb.SelectedIndex > -1 && disCb.SelectedIndex > -1 && coupleCb.SelectedIndex > -1)
            {
                m.TeacherAudienceEl selectedTeachAud = teachCb.SelectedItem as m.TeacherAudienceEl;
                var curTeachAud = db.TeacherAudiences.FirstOrDefault(p => p.Id == selectedTeachAud.Id);
                m.GroupEl selectedGroup = groupCb.SelectedItem as m.GroupEl;
                var curGroup = db.Groups.FirstOrDefault(p => p.Id == selectedGroup.Id);
                m.Discipline selectedDiscipline = disCb.SelectedItem as m.Discipline;
                var curDiscipline = db.Disciplines.FirstOrDefault(p => p.Id == selectedDiscipline.Id);
                m.Couple selectedCouple = coupleCb.SelectedItem as m.Couple;
                var curCouple = db.Couples.FirstOrDefault(p => p.Id == selectedCouple.Id);
                m.DayOfTheWeek selectedDayOfTheWeek = weekCb.SelectedItem as m.DayOfTheWeek;
                var curDayOfTheWeek = db.DaysOfTheWeek.FirstOrDefault(p => p.Id == selectedDayOfTheWeek.Id);
                m.Fraction selectedFraction = dayCb.SelectedItem as m.Fraction;
                var curFraction = db.Fractions.FirstOrDefault(p => p.Id == selectedFraction.Id);

/*                var uniqField = db.ScheduleOfClasses.FirstOrDefault(p => p.GroupId == curGroup.Id && p.TeacherAudienceId == curTeachAud.Id && p.CouplesId == curCouple.Number);

                if (uniqField != null)
                {
                    MessageBox.Show("Такая запись уже существует");
                    return;
                }*/

                m.ScheduleOfClass aud = new m.ScheduleOfClass { TeacherAudience = curTeachAud, Group = curGroup, Couple = curCouple, Discipline = curDiscipline, DayOfTheWeek = curDayOfTheWeek, Fraction = curFraction };
                db.ScheduleOfClasses.Add(aud);
                db.SaveChanges();

                LoadData();

                if (teachCb.BorderBrush == Brushes.Red)
                    teachCb.BorderBrush = Brushes.LightGray;
                if (groupCb.BorderBrush == Brushes.Red)
                    groupCb.BorderBrush = Brushes.LightGray;

                teachCb.SelectedIndex = -1;
                groupCb.SelectedIndex = -1;
                disCb.SelectedIndex = -1;
                coupleCb.SelectedIndex = -1;
                dayCb.SelectedIndex = -1;
                weekCb.SelectedIndex = -1;
            }
            else
            {
                if (teachCb.SelectedIndex < 0)
                    teachCb.BorderBrush = Brushes.Red;
                if (groupCb.SelectedIndex < 0)
                    groupCb.BorderBrush = Brushes.Red;
            }
        }

        private void Grid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            m.ScheduleOfClassEl item = Grid.SelectedItem as m.ScheduleOfClassEl;

            if (item != null)
            {
                teachCb.SelectedValue = item.TeachAudId;
                groupCb.SelectedValue = item.GroupId;
                disCb.SelectedValue = item.DisciplineId;
                coupleCb.SelectedValue = item.CoupleId;
                dayCb.SelectedValue = item.DrobId;
                weekCb.SelectedValue = item.WeekId;
            }
            else
                return;
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
