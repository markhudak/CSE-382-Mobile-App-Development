using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace GradeBook
{

    [Table("student")]
    public class Students
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }

        public string Course { get; set; }


        public override string ToString()
        {
            return string.Format("{0} {1}", Name, Course);
        }

        public static Students ParseCSVstud(string line)
        {
            string[] toks = line.Split(',');
            Students student = new Students
            {
                Name = toks[0],
                Course = toks[2],
            };
            return student;
        }
    }
}
