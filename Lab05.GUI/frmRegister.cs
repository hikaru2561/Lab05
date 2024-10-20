using Lab05.BUS;
using Lab05.DAL.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Lab05.GUI
{
    public partial class frmRegister : Form
    {
        private readonly StudentService studentService = new StudentService();
        private readonly FacultyService facultyService = new FacultyService();
        private readonly MajorService majorService = new MajorService();
        public frmRegister()
        {
            InitializeComponent();
            this.dgvStudent.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(dgvStudent_CellContentClick);
        }

        private void frmRegister_Load(object sender, EventArgs e)
        {
            try
            {
                var listFacultys = facultyService.GetAll();
                Student s = studentService.FindById(itemID);
                FillFalcultyCombobox(listFacultys);  
                var listStudents = studentService.GetAllHasNoMajor();
                BindGrid(listStudents);
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FillFalcultyCombobox(List<Faculty> listFacultys)
        {
            this.cmbFaculty.DataSource = listFacultys;
            this.cmbFaculty.DisplayMember = "FacultyName";
            this.cmbFaculty.ValueMember = "FacultyID";
        }

        private void FillMajorCombobox(List<Major> listMajor)
        {
            this.cmbMajor.DataSource = listMajor;
            this.cmbMajor.DisplayMember = "Name";
            this.cmbMajor.ValueMember = "MajorID";
        }

        private void BindGrid(List<Student> students)
        {
            dgvStudent.Rows.Clear();
            foreach (Student s in students)
            {
                int index = dgvStudent.Rows.Add();
                dgvStudent.Rows[index].Cells[0].Value = s.StudentID;
                dgvStudent.Rows[index].Cells[1].Value = s.FullName;
                dgvStudent.Rows[index].Cells[2].Value = s.Faculty.FacultyName;
                dgvStudent.Rows[index].Cells[3].Value = s.AverageScore;

            }
        }


        private void cmbFaculty_SelectedIndexChanged(object sender, EventArgs e)
        {
            Faculty selectedFaculty = cmbFaculty.SelectedItem as Faculty;
            if (selectedFaculty != null)
            {
                    var listMajor = majorService.GetAllByFaculty(selectedFaculty.FacultyID);
                    FillMajorCombobox(listMajor);       
                    var listStudents = studentService.GetAllHasNoMajor(selectedFaculty.FacultyID);
                    BindGrid(listStudents);

                foreach( var row in dgvStudent.Rows)
                {
                    if (row.Equals(cmbFaculty.Equals(selectedFaculty))) ;
                    Visible = true;
                }
                
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

            //var listStudent = studentService.GetAll();
            //foreach ( var s in listStudent)
            //{
            //    if (itemID == null)
            //        MessageBox.Show("Đối tượng không hợp lệ!!!", "Thông Báo", MessageBoxButtons.OK);
            //    else
            //    {
            //        s.MajorID = int.Parse(cmbMajor.SelectedValue.ToString());
            //        studentService.InsertUpdate(s);
            //        MessageBox.Show("Thêm chuyên ngành thành công!!!", "Thông Báo", MessageBoxButtons.OK);
            //    }
            //}
            if (itemID != null)
            {
                Student s = studentService.FindById(itemID);
                s.MajorID = int.Parse(cmbMajor.SelectedValue.ToString());
                studentService.InsertUpdate(s);
                MessageBox.Show("Thêm chuyên ngành thành công!!!", "Thông Báo", MessageBoxButtons.OK);
                cmbFaculty_SelectedIndexChanged(sender, e);
            }
            else
                MessageBox.Show("Đối tượng không hợp lệ!!!", "Thông Báo", MessageBoxButtons.OK);
        }

        string itemID;
        private void dgvStudent_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow selectedRow = dgvStudent.Rows[e.RowIndex];  
                itemID = selectedRow.Cells[0].Value.ToString();  
            }  
        }

        private void btnDong_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form1 form1 = new Form1();
            form1.ShowDialog();
        }
    }
}
