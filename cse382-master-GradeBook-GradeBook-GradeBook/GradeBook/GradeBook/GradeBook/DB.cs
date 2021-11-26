using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using Xamarin.Essentials;

namespace GradeBook
{
  
    public class DB
    {

        private static string DBName = "gradebook.db";
        public static SQLiteConnection conn;

        public static void OpenConnection()
        {
            string libFolder = FileSystem.AppDataDirectory;
            string fname = System.IO.Path.Combine(libFolder, DBName);
            conn = new SQLiteConnection(fname);
            conn.CreateTable<Students>();
            conn.CreateTable<Course>();
            conn.CreateTable<Grades>();
           // LoadStudents();
           // LoadCourses();
           // LoadGrades();
        }

        public static void DeleteTableContents(string tableName)
        {
            int v = conn.Execute("DELETE FROM " + tableName);
        }

        public static void LoadStudents()
        {
            try
            {
                //DeleteTableContents("student");

                var assembly = IntrospectionExtensions.GetTypeInfo(typeof(MainPage)).Assembly;
                Stream stream = assembly.GetManifestResourceStream("GradeBook.Students.txt");
                StreamReader input = new StreamReader(stream);
                while (!input.EndOfStream)
                {
                    string line = input.ReadLine();

                    Students student = Students.ParseCSVstud(line);

                    conn.Insert(student);
                }
            }
            catch (Exception e)
            {
            }
        }

        public static void LoadCourses()
        {

            try
            {
                DeleteTableContents("course");

                var assembly = IntrospectionExtensions.GetTypeInfo(typeof(MainPage)).Assembly;
                Stream stream = assembly.GetManifestResourceStream("GradeBook.Students.txt");
                StreamReader input = new StreamReader(stream);
                while (!input.EndOfStream)
                {
                    string line = input.ReadLine();

                    Course course = Course.ParseCSVcourse(line);
                    string strcrs = Course.ParseCSVcourse(line).ToString();

                    List<string> crsList = new List<string>();
                    if (crsList.Contains(strcrs))
                    {
                    } else { 
                        crsList.Add(strcrs);
                        conn.Insert(course);
                    }


                }
            }
            catch (Exception e)
            {
            }
        }

        public static void LoadGrades()
        {
            try
            {
                DeleteTableContents("grades");

                var assembly = IntrospectionExtensions.GetTypeInfo(typeof(MainPage)).Assembly;
                Stream stream = assembly.GetManifestResourceStream("GradeBook.Students.txt");
                StreamReader input = new StreamReader(stream);
                while (!input.EndOfStream)
                {
                    string line = input.ReadLine();

                    Grades grades = Grades.ParseCSVgrades(line);

                    conn.Insert(grades);
                }
            }
            catch (Exception e)
            {
            }
        }

        public static void LoadStudents(string file)
        {
            // all code obtained from referencing Microsoft docs

            string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), file);
            if (File.Exists(fileName))
            {
                DeleteTableContents("student");
                Stream strm = File.OpenRead(fileName);
                StreamReader input = new StreamReader(strm);
                while (!input.EndOfStream)
                {
                    string line = input.ReadLine();
                    Students student = Students.ParseCSVstud(line);
                    conn.Insert(student);
                }


            }

           
        }

    }
        
}


