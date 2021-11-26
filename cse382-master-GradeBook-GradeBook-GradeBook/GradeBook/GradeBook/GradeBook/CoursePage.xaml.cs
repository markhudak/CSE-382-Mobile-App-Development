using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using GradeBook.Models;
using SQLite;

[assembly: ExportFont("SchoolTeacher-Regular.ttf", Alias = "SchoolTeacher")]


namespace GradeBook
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CoursePage : ContentPage
    {
        CharityPage charPage;
        List<string> lvList;
        List<string> cleaned;
        List<string> pkrList;
        List<string> current;

        ClassPage classPage;


        public CoursePage()
        {
            InitializeComponent();

            pkrList = new List<string> { "", "KG", "1st", "2nd", "3rd", "4th", "5th", "6th", "7th", "8th", "9th", "10th", "11th", "12th" };
            lvList = new List<string>(from Course in DB.conn.Table<Course>()
                                      select Course.CourseName);
            cleaned = new List<string>();
            for (int i = 0; i < lvList.Count; i++)
            {
                if (!cleaned.Contains(lvList[i]))
                {
                    cleaned.Add(lvList[i]);
                }
            }

            current = cleaned;

        }

        private async void Weight_Clicked(object sender, EventArgs e)
        {
            if (lv.SelectedItem != null)
            {
                WeightPage wp = new WeightPage(lv.SelectedItem.ToString());
                await Navigation.PushAsync(wp, true);
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            lvList = new List<string>(from course in DB.conn.Table<Course>()
                                      select course.CourseName);

            cleaned = new List<string>();
            for (int i = 0; i < lvList.Count; i++)
            {
                if (!cleaned.Contains(lvList[i]))
                {
                    cleaned.Add(lvList[i]);
                }
            }
            current = cleaned;
            lv.ItemsSource = current;

            pickr.ItemsSource = pkrList;
            
        }

        private void addButton_Clicked(object sender, EventArgs e)
        {

            if (entry.Text != null && entry.Text != "")
            {
                string name = entry.Text;

                if (!cleaned.Contains(name))
                {

                    Course courset = new Course
                    {
                        CourseName = name,
                    };
                   
                    DB.conn.Insert(courset);

                    lvList = new List<string>(from course in DB.conn.Table<Course>()
                                              select course.CourseName);

                    cleaned = new List<string>();
                    for (int i = 0; i < lvList.Count; i++)
                    {
                        if (!cleaned.Contains(lvList[i]))
                        {
                            cleaned.Add(lvList[i]);
                        }
                    }
                    current = cleaned;
                    lv.ItemsSource = current;
                }
            }
            else
            {
                {
                    string name = "";

                    if (pickr.SelectedItem != null)
                    {
                        name = pickr.SelectedItem.ToString();
                    }

                    if (!cleaned.Contains(name))
                    {

                        Course courset = new Course
                        {
                            CourseName = name,
                        };


                        DB.conn.Insert(courset);

                        lvList = new List<string>(from Course in DB.conn.Table<Course>()
                                                  select Course.CourseName);

                        cleaned = new List<string>();
                        for (int i = 0; i < lvList.Count; i++)
                        {
                            if (!cleaned.Contains(lvList[i]))
                            {
                                cleaned.Add(lvList[i]);
                            }
                        }

                        current = cleaned;
                        lv.ItemsSource = current;
                    }
                }

            }
        } 
    


        private void delButton_Clicked(object sender, EventArgs e)
        {
            if (lv.SelectedItem != null) { 
            string name = lv.SelectedItem.ToString();

                var pk = from Course in DB.conn.Table<Course>()
                         where Course.CourseName == name
                         select Course;

                Course crse = null;

                if (pk.ToList().Count == 1)
                {
                    crse = pk.ToList()[0];
                }

                if (crse != null)
                {

                    int v = DB.conn.Delete(crse);
                    if (v > 0)
                    {
                        lv.SelectedItem = null;
                        lvList = new List<string>(from Course in DB.conn.Table<Course>()
                                                  where Course.CourseName != name
                                                  select Course.CourseName);

                        cleaned = new List<string>();

                        for (int i = 0; i < lvList.Count; i++)
                        {
                            if (cleaned.Contains(lvList[i]))
                            { }
                            else
                            {
                                cleaned.Add(lvList[i]);
                            }
                        }

                        current = cleaned;
                        lv.ItemsSource = current;

                    }
                }
            }
        }

        private async void goButton_Clicked(object sender, EventArgs e)
        {
            if (lv.SelectedItem != null)
            {
                string course = lv.SelectedItem.ToString();
                classPage = new ClassPage(course);
                await Navigation.PushAsync(classPage, true);
            }
        }

        private void lv_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
          //  course = ((ListView)sender).SelectedItem as Course;
        }

        private void loadButton_Clicked(object sender, EventArgs e)
        {
            string file = entry.Text;
            DB.LoadStudents(file);
        }

        private void but_Clicked(object sender, EventArgs e)
        {
            if (url.Text != "" && url.Text != null)
            {
                string str = "http://" + url.Text;
                Browser.OpenAsync(str);
            }
        }

    }
}