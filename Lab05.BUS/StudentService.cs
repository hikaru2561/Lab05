using Lab05.DAL.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab05.BUS
{
    public class StudentService
    {
        public List<Student> GetAll()
        {
            QuanLySinhVienDB context = new QuanLySinhVienDB();
            return context.Students.ToList();
        }

        public List<Student> GetAllHasNoMajor()
        {
            QuanLySinhVienDB context = new QuanLySinhVienDB();
            return context.Students.Where(p => p.MajorID == 0).ToList();
        }

        public List<Student> GetAllHasNoMajor(int facultyID) 
        {
            QuanLySinhVienDB context = new QuanLySinhVienDB();
            return context.Students.Where(p => p.MajorID == 0 && p.FacultyID == facultyID).ToList();
        }

        public Student FindById(string id)
        {
            QuanLySinhVienDB context = new QuanLySinhVienDB();
            return context.Students.FirstOrDefault(p => p.StudentID == id);
        }

        public void InsertUpdate(Student student)
        {
            QuanLySinhVienDB context = new QuanLySinhVienDB();
            context.Students.AddOrUpdate(student);
            context.SaveChanges();
        }

        public void DeleteUpdate(string studentID) 
        {
            QuanLySinhVienDB context = new QuanLySinhVienDB();
            Student s = context.Students.FirstOrDefault(p => p.StudentID == studentID);
            context.Students.Remove(s);
            context.SaveChanges();
        }

        public string fileFath(string studentId)
        {
            QuanLySinhVienDB context = new QuanLySinhVienDB();
            string flie = context.Students.Where(s => s.StudentID == studentId).Select(s => s.Avatar).FirstOrDefault();

            return flie;
        }
    }
}
