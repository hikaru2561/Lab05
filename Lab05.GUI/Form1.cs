using Lab05.BUS;
using Lab05.DAL.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Linq;

namespace Lab05.GUI
{
    public partial class Form1 : Form
    {
        private readonly StudentService StudentService = new StudentService();
        private readonly FacultyService facultyService = new FacultyService();
        private string folderPath;
        public Form1()
        {
            InitializeComponent();
            this.dgvStudent.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(dgvStudent_CellContentClick);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                setGridViewStyle(dgvStudent);
                var listFacultys = facultyService.GetAll();
                var listStudents = StudentService.GetAll();
                FillFalcultyCombobox(listFacultys);
                cmbFaculty.SelectedIndex = 0;
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
        //Hàm binding gridView từ list sinh viên
        private void BindGrid(List<Student> listStudent)
        {
            dgvStudent.Rows.Clear();
            foreach (var item in listStudent)
            {
                int index = dgvStudent.Rows.Add();
                dgvStudent.Rows[index].Cells[0].Value = item.StudentID;
                dgvStudent.Rows[index].Cells[1].Value = item.FullName;
                if (item.Faculty != null)
                    dgvStudent.Rows[index].Cells[2].Value =
                    item.Faculty.FacultyName;
                dgvStudent.Rows[index].Cells[3].Value = item.AverageScore +
                "";
                if (item.MajorID.ToString() != null)
                    dgvStudent.Rows[index].Cells[4].Value = item.Major.Name +
                    "";
                ShowAvatar(item.Avatar);
            }
        }
        private void ShowAvatar(string ImageName)
        {
            if (string.IsNullOrEmpty(ImageName))
            {
                picAvatar.Image = null;
            }
            else
            {
                string parentDirectory =
                Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.FullName;
                string imagePath = Path.Combine(parentDirectory, "Images",
                ImageName);
                picAvatar.Image = Image.FromFile(imagePath);
                picAvatar.Refresh();
            }
        }
        public void setGridViewStyle(DataGridView dgview)
        {
            dgview.BorderStyle = BorderStyle.None;
            dgview.DefaultCellStyle.SelectionBackColor = Color.DarkTurquoise;
            dgview.CellBorderStyle =
            DataGridViewCellBorderStyle.SingleHorizontal;
            dgview.BackgroundColor = Color.White;
            dgview.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            var listStudents = new List<Student>();
            if (this.chkUnregisterMajor.Checked)
                listStudents = StudentService.GetAllHasNoMajor();
            else
                listStudents = StudentService.GetAll();
            BindGrid(listStudents);
        }

        private void Clear()
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            cmbFaculty.SelectedIndex = 0;
        }

        private void Add()
        {
            Student s = new Student();
            s.StudentID = textBox1.Text;
            s.FullName = textBox2.Text;
            s.FacultyID = int.Parse(cmbFaculty.SelectedValue.ToString());
            s.AverageScore = double.Parse(textBox3.Text);
            s.MajorID = 0;

            StudentService.InsertUpdate(s);
            var listStudents = StudentService.GetAll();
            BindGrid(listStudents);
            Clear();
            MessageBox.Show("Thêm dữ liệu thành công!!!", "Thông Báo", MessageBoxButtons.OK);
        }

        private void Update(Student s)
        {  
            s.StudentID = textBox1.Text;
            s.FullName = textBox2.Text;
            s.AverageScore = double.Parse(textBox3.Text);

            StudentService.InsertUpdate(s);
            var listStudents = StudentService.GetAll();
            BindGrid(listStudents);
            Clear();
            MessageBox.Show("Sửa dữ liệu thành công!!!", "Thông Báo", MessageBoxButtons.OK);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Student s = StudentService.FindById(textBox1.Text);
            if (s == null)
            {
                Add();
            }
            else if (s.FacultyID != int.Parse(cmbFaculty.SelectedValue.ToString()))
                Add();
            else
                Update(s);
        }

        private void button2_Click(object sender, EventArgs e)
        {   
            StudentService.DeleteUpdate(textBox1.Text);
            var listStudents = StudentService.GetAll();
            BindGrid(listStudents);
            Clear();
            MessageBox.Show("Xoá dữ liệu thành công!!", "Thông Báo", MessageBoxButtons.OK);
        }

        private void dgvStudent_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            Dgv_CellClick(sender, e);
        }

        private void Dgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dgvStudent.Rows.Count -1)
            { 
                DataGridViewRow selectedRow = dgvStudent.Rows[e.RowIndex];
                textBox1.Text = selectedRow.Cells[0].Value.ToString();
                textBox2.Text = selectedRow.Cells[1].Value.ToString();                    
                cmbFaculty.Text = selectedRow.Cells[2].Value.ToString();
                textBox3.Text = selectedRow.Cells[3].Value.ToString();
            }
            else
                MessageBox.Show("Đối tượng không hợp lệ!!", "Thông Báo", MessageBoxButtons.OK);
        }
        
        private void btnDKCN_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmRegister frmRegister = new frmRegister();
            frmRegister.ShowDialog();

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnPic_Click(object sender, EventArgs e)
        {
            folderPath = Path.Combine(Application.StartupPath, @"..\..\Images");
            if (!Directory.Exists(folderPath))
            {
                MessageBox.Show("Thư mục ảnh không tồn tại!");
            }
            OpenFileDialog open = new OpenFileDialog();
            open.InitialDirectory = folderPath;
            open.Filter = "Image File | *.jpg; *.png; *.gif; *.jpeg";

            if (open.ShowDialog() == DialogResult.OK)
            {
                picAvatar.ImageLocation = open.FileName;
            }
        }

        private void cmbFaculty_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
    
}
