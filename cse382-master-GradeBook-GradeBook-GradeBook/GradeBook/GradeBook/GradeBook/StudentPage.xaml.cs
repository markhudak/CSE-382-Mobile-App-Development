using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using SQLite;

namespace GradeBook
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StudentPage : ContentPage
    {

        string student;
        double gpa;
        string gpA;
        double gpaPerc;
        double testWeight;
        double quizWeight;
        double hwWeight;
        string crs;

        public StudentPage(string name, string course)
        {
            InitializeComponent();

            crs = course;
            double val = 0;
            testWeight = Preferences.Get("Test" + course, val);
            quizWeight = Preferences.Get("Quiz" + course, val);
            hwWeight = Preferences.Get("HW" + course, val);
            weights.Text = "Test : " + testWeight + " ; Quiz : " + quizWeight + " ; HW : " + hwWeight;
            student = name;
            namLab.Text = name;

            crsLab.Text = course;
            lv.ItemsSource = from Grades in DB.conn.Table<Grades>()
                             where Grades.StudentName == student && Grades.CourseName == crs
                             select Grades.AssignName.ToString();
            calculateGPA();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            lv.ItemsSource = from Grades in DB.conn.Table<Grades>()
                             where Grades.StudentName == student && Grades.CourseName == crs
                             select Grades.AssignName.ToString();
            calculateGPA();

        }

        private void delButton_Clicked(object sender, EventArgs e)
        {
            if (lv.SelectedItem != null)
            {

                string name = lv.SelectedItem.ToString();

                var pk = from Grades in DB.conn.Table<Grades>()
                         where Grades.AssignName == name && Grades.StudentName == student && Grades.CourseName == crs
                         select Grades;

                Grades grad = null;

                if (pk.ToList().Count >= 1)
                {
                    grad = pk.ToList()[0];
                }


                if (grad != null)
                {

                    int v = DB.conn.Delete(grad);
                    if (v > 0)
                    {
                        lv.SelectedItem = null;
                        lv.ItemsSource = from Grades in DB.conn.Table<Grades>()
                                         where Grades.CourseName == crs && Grades.StudentName == student
                                         select Grades.AssignName;

                        // lvList.Remove(name);
                        calculateGPA();
                    }
                }
            }
        }

        private void lv_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            ListView which = (ListView)sender;

            if (which.SelectedItem != null)
            {
                string str = "";
          //  ListView which = (ListView)sender;
           
                str = which.SelectedItem.ToString();



                IEnumerable<Tuple<string, string>> numes = from Grades in DB.conn.Table<Grades>()
                                                           where Grades.StudentName == student
                                                           && Grades.AssignName == str && Grades.CourseName == crs
                                                           select new Tuple<string, string>(Grades.PointsEarned.ToString(), Grades.TotalPoints.ToString());

                List<Tuple<string, string>> numesList = (numes.ToList());

                info.Text = numesList[0].Item1.ToString() + " / " + numesList[0].Item2.ToString();
            }
        }

        private void calculateGPA()
        {   

            IEnumerable<Tuple<string, string>> testPointsIE = from Grades in DB.conn.Table<Grades>()
                                         where Grades.StudentName == student
                                         && Grades.GradeType == "Test"
                                         select new Tuple<string, string>(Grades.PointsEarned.ToString(), Grades.TotalPoints.ToString());
            List<Tuple<string, string>> testPointsL = testPointsIE.ToList();

            IEnumerable<Tuple<string, string>> quizPointsIE = from Grades in DB.conn.Table<Grades>()
                                         where Grades.StudentName == student
                                         && Grades.GradeType == "Quiz"
                                         select new Tuple<string, string>(Grades.PointsEarned.ToString(), Grades.TotalPoints.ToString());
            List<Tuple<string, string>> quizPointsL = quizPointsIE.ToList();

            IEnumerable<Tuple<string, string>> hwPointsIE = from Grades in DB.conn.Table<Grades>()
                                         where Grades.StudentName == student
                                         && Grades.GradeType == "HW"
                                         select new Tuple<string, string>(Grades.PointsEarned.ToString(), Grades.TotalPoints.ToString());
            List<Tuple<string, string>> hwPointsL = hwPointsIE.ToList();

            double tpEarned = 0, tpTot = 0, qpEarned = 0, qpTot = 0, hwpEarned = 0, hwpTot = 0, total = 0, totalEarned = 0;

           for (int i = 0; i < testPointsL.Count; i++)
            {
                double xEarned = Double.Parse(testPointsL[i].Item1);  
                double xTot = Double.Parse(testPointsL[i].Item2);   
                tpEarned += xEarned;
                tpTot += xTot;
            }

            for (int i = 0; i < quizPointsL.Count; i++)
            {
                double xEarned = Double.Parse(quizPointsL[i].Item1);  
                double xTot = Double.Parse(quizPointsL[i].Item2);  
                qpEarned += xEarned;
                qpTot += xTot;
            }

            for (int i = 0; i < hwPointsL.Count; i++)
            {
                double xEarned = Double.Parse(hwPointsL[i].Item1);   
                double xTot = Double.Parse(hwPointsL[i].Item2);   
                hwpEarned += xEarned;
                hwpTot += xTot;
            }

            // If calculation errors - ensure weights are set for the course ***

            if (testWeight == 0 && quizWeight == 0 && hwWeight == 0)
            {
                empty.Text = "Set the course weights, please.";
                note.Text = "See ruler (image) on course page.";
                gpA = "?";
            }

            total = (tpTot * testWeight) + (qpTot * quizWeight) + (hwpTot * hwWeight);
            totalEarned = (tpEarned * testWeight) + (qpEarned * quizWeight) + (hwpEarned * hwWeight);

            if (total != 0)
            {
                gpaPerc = 100 * (totalEarned / total);
                string strGPA = gpaPerc.ToString();

                if (gpaPerc.ToString().Length >= 5)
                {
                    strGPA = strGPA.Substring(0, 5);
                }  
                
                if (gpaPerc.ToString().Length < 5 && gpaPerc.ToString().Length > 2)
                {
                    strGPA = strGPA.Substring(0, gpaPerc.ToString().Length);
                }


                gpaPercLab.Text = strGPA + "%";
            }
            if (gpaPerc >= 97)
            {
                gpA = "A+";
            }
            else if (97 > gpaPerc && gpaPerc >= 93)
            {
                gpA = "A";
            }
            else if (93 > gpaPerc && gpaPerc >= 90)
            {
                gpA = "A-";

            }
            else if (90 > gpaPerc && gpaPerc >= 87)
            {
                gpA = "B+";
            }
            else if (87 > gpaPerc && gpaPerc >= 83)
            {
                gpA = "B";
            }
            else if (83 > gpaPerc && gpaPerc >= 80)
            {
                gpA = "B-";
            }
            else if (80 > gpaPerc && gpaPerc >= 77)
            {
                gpA = "C+";
            }
            else if (77 > gpaPerc && gpaPerc >= 73)
            {
                gpA = "C";
            }
            else if (73 > gpaPerc && gpaPerc >= 70)
            {
                gpA = "C-";
            }
            else if (70 > gpaPerc && gpaPerc >= 67)
            {
                gpA = "D+";
            }
            else if (67 > gpaPerc && gpaPerc >= 63)
            {
                gpA = "D";
            }
            else if (63 > gpaPerc && gpaPerc >= 60)
            {
                gpA = "D-";
            }
            else if (60 > gpaPerc && gpaPerc > 0.0 && (quizPointsL.Count != 0 || testPointsL.Count != 0 || hwPointsL.Count != 0))
            {
                gpA = "F";
            } else if (quizPointsL.Count == 0 && testPointsL.Count == 0 && hwPointsL.Count == 0 && gpaPerc == 0)
            {
                gpA = "?";
                empty.Text = "Put in my grades!";
            }

            gpaLab.Text = gpA;

        }
    }
}