using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using WcfService.Models;

namespace WcfService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "StudentService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select StudentService.svc or StudentService.svc.cs at the Solution Explorer and start debugging.
    public class StudentService : IStudentService
    {
        public void AddStudents(student obj)
        {
            SQLProjectEntities db = new SQLProjectEntities();
            db.students.Add(new student { Name = obj.Name, Age = obj.Age });
            db.SaveChanges();

        }

        public List<student> GetStudents()
        {
            SQLProjectEntities db = new SQLProjectEntities();
            return db.students.Select(s => new student { Name = s.Name, Age = s.Age }).ToList();
        }

        //public StudentContract GetStudentByID(string StudentName)
        //{
        //    SQLProjectEntities db = new SQLProjectEntities();
        //    var matchedStudent = db.students.FirstOrDefault(s => s.Name == StudentName);
        //    if (matchedStudent != null)
        //    {
        //        return new StudentContract
        //        {
        //            Name = matchedStudent.Name,
        //            Age = matchedStudent.Age
        //        };
        //    }
        //    return null;
        //}

        //public void UpdateStudent(StudentContract studentcontractor)
        //{
        //    SQLProjectEntities db = new SQLProjectEntities();
        //    try
        //    {
        //        var existingStudent = db.students.Where(s => s.Name == studentcontractor.Name).FirstOrDefault();
        //        if (existingStudent != null)
        //        {

        //            existingStudent.Age = studentcontractor.Age;
        //            db.SaveChanges();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //    }
        //}

        //public void DeleteStudent(string StudentName)
        //{
        //    SQLProjectEntities db = new SQLProjectEntities();
        //    var matchStudent = db.students.FirstOrDefault(s => s.Name == StudentName);
        //    if (matchStudent != null)
        //    {
        //        db.students.Remove(matchStudent);
        //        db.SaveChanges();
        //    }
        //}

    }
}
