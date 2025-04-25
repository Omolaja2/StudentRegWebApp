using System.Collections.Generic;
using Student_MVC.Models;

namespace Student_MVC.Data
{
    public static class StudentRepository
    {
        private static List<StudentsModel> students = [];

        public static List<StudentsModel> GetAll()
        {
            return students;
        }

        public static void Add(StudentsModel student)
        {
            students.Add(student);
        }
    }
}
