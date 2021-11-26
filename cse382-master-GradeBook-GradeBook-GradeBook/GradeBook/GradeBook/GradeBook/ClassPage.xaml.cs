using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SQLite;
using Xamarin.Essentials;


namespace GradeBook
{

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ClassPage : ContentPage
    {
        List<string> weight;
        List<string> lvList;
        List<string> pickerList;

        string course;
        StudentPage studPage;



        public ClassPage(string courseT)
        {
            InitializeComponent();

            course = courseT;

            crsLab.Text = course;
            pickerList = new List<string>();

            for (int i = 0; i <= 100; i++)
            {
                pickerList.Add(i.ToString());
            }

            
            weight = new List<string> { "Test", "Quiz", "HW" };

            lv.ItemsSource = from student in DB.conn.Table<Students>()
                             where student.Course == course
                             select student.Name;

            lvList = new List<string>(from Students in DB.conn.Table<Students>()
                                      select Students.Name + Students.Course);
            p2.ItemsSource = weight;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
           
            lv.ItemsSource = from student in DB.conn.Table<Students>()
                             where student.Course == course
                             select student.Name;

            pick1.ItemsSource = pickerList;
            pick2.ItemsSource = pickerList;

        }

        private void addButton_Clicked(object sender, EventArgs e)
        {

            if (entry.Text != null && entry.Text != "")
            {
                string name = entry.Text;

                if (!lvList.Contains(name+course))
                {

                    Students students = new Students
                    {
                        Name = name,
                        Course = course,
                    };



                    DB.conn.Insert(students);
                    lvList.Add(name+course);
                    lv.ItemsSource = from student in DB.conn.Table<Students>()
                                     where student.Course == course
                                     select student.Name;
                }
            }

        }

        private void sendBut_Clicked(object sender, EventArgs e)
        {
            string message = "";

            IEnumerable<string> numes = from Grades in DB.conn.Table<Grades>()
                                        where Grades.CourseName == course
                                        select Grades.ToString();
            List<string> list = numes.ToList();
            foreach (string str in list)
            {
                message += System.Environment.NewLine + str;
            }

            List<string> address = new List<string>();
            if (email.Text != "" && email.Text != null)
            {
                address.Add(email.Text);
            }

            SendEmail(course, message, address);
        }

        public async Task SendEmail(string subject, string body, List<string> recipients)
        {

            try
            {
                var message = new EmailMessage
                {
                    Subject = subject,
                    Body = body,
                    To = recipients,
                    //Cc = ccRecipients,
                    //Bcc = bccRecipients
                };
                await Email.ComposeAsync(message);
            }
            catch (FeatureNotSupportedException fbsEx)
            {
                // Email is not supported on this device
            }
            catch (Exception ex)
            {
                // Some other exception occurred
            }
        }

        private void delButton_Clicked(object sender, EventArgs e)
        {
            if (lv.SelectedItem != null)
            {

                string name = lv.SelectedItem.ToString();

                var pk = from student in DB.conn.Table<Students>()
                         where student.Name == name && student.Course == course
                         select student;

                var pk2 = from Grades in DB.conn.Table<Grades>()
                         where Grades.StudentName == name && Grades.CourseName == course
                         select Grades;

                Students stud = null;
                Grades grad = null;

                if (pk.ToList().Count == 1)
                {
                    stud = pk.ToList()[0];
                    grad = pk2.ToList()[0];
                }


                if (stud != null)
                {

                    int v = DB.conn.Delete(stud);
                    int x = DB.conn.Delete(grad);
                    if (v > 0)
                    {
                        lv.SelectedItem = null;
                        lv.ItemsSource = from student in DB.conn.Table<Students>()
                                         where student.Course == course
                                         select student.Name;

                        lvList.Remove(name+course);

                    }
                }
            }
        }

        private async void goButton_Clicked(object sender, EventArgs e)
        {
            if (lv.SelectedItem != null)
            {
                string name = lv.SelectedItem.ToString();
                studPage = new StudentPage(name, course);
                await Navigation.PushAsync(studPage, true);
            }
        }

        private void addGradeButton_Clicked(object sender, EventArgs e)
        {
            if (lv.SelectedItem != null)
            {
                string studName = lv.SelectedItem.ToString();
                List<string> lvList = new List<string>(from Grades in DB.conn.Table<Grades>()
                                                       where Grades.StudentName == studName && Grades.CourseName == course
                                                       select Grades.AssignName);
                if (!(lvList.Contains(assgnmnt.Text)))
                {
                    string name;
                    if (assgnmnt.Text != null)
                    {
                        name = assgnmnt.Text;
                    }
                    else
                    {
                        name = "";
                    }

                    if (lv.SelectedItem != null && p2.SelectedItem != null)
                    {
                        Grades grades = new Grades
                        {
                            StudentName = lv.SelectedItem.ToString(),
                            CourseName = course,
                            GradeType = p2.SelectedItem.ToString(),
                            AssignName = name,
                            PointsEarned = pick1.SelectedItem.ToString(),
                            TotalPoints = pick2.SelectedItem.ToString(),

                        };

                        DB.conn.Insert(grades);
                    }
                }
            }
        }

    }
}