using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace GradeBook
{
    [Table("grades")]
    public class Grades
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string StudentName { get; set; }

        public string CourseName { get; set; }

        public string GradeType { get; set; }

        public string AssignName { get; set; }

        public string PointsEarned { get; set; }

        public string TotalPoints { get; set; }


        public override string ToString()
        {
            return string.Format("{0} : {1} : {2} -- {3} ( {4} / {5} )", StudentName, CourseName, GradeType, AssignName, PointsEarned, TotalPoints);
        }

        
        public static Grades ParseCSVgrades(string line)
        {
            string[] toks = line.Split(',');
            Grades grades = new Grades
            {
                StudentName = toks[0],
                CourseName = toks[2],
                GradeType = "",
                AssignName = "",
            };
            return grades;
        }
         
    }
}
