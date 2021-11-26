using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace GradeBook
{

    [Table("course")]
    public class Course
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string CourseName { get; set; }

        public override string ToString()
        {
            return string.Format("{0}", CourseName);
        }

        public static Course ParseCSVcourse(string line)
        {
            string[] toks = line.Split(',');
            Course course = new Course
            {
                CourseName = toks[2],
            };
            return course;
        }
    }
}
