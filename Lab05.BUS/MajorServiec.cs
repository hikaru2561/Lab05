using Lab05.DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace Lab05.BUS
{
    public class MajorService
    {
        public List<Major> GetAllByFaculty(int  factultyID) 
        {
            QuanLySinhVienDB context = new QuanLySinhVienDB();
            return context.Majors.Where(p=> p.FacultyID == factultyID).ToList();
        }
        

        
    }
}
