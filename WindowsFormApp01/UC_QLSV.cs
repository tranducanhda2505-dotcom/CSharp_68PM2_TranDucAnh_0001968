using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp01
{
    public partial class UC_QLSV : UserControl
    {
        QLsinhvienDataContext db = new QLsinhvienDataContext();
        int pageSize = 10;
        int currentPage = 1;
        int totalPages = 1;
        string currentSearchStr = "";
        public UC_QLSV()
        {
            InitializeComponent();
        }
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void UC_QLSV_Load(object sender, EventArgs e)
        {
            LoadLopHoc();
            LoadData();
        }


        private void btn_add_Click(object sender, EventArgs e)
        {
            String mssv = txt_mssv.Text;
            String name = txt_name.Text;
            String gioitinh = cboGioiTinh.Text;
            DateTime dateTime = dtpNgaySinh.Value;

            Student sv = new Student();

            sv.MSSV = mssv;
            sv.FullName = name;
            sv.Gender = gioitinh;
            sv.DateOfBirth = dateTime;
            if (cbxLopHoc.SelectedValue == null)
            {
                MessageBox.Show("Danh sách lớp học đang trống hoặc bạn chưa chọn lớp! Vui lòng kiểm tra lại CSDL.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // lấy mã lớp học từ combo box, vì combo box đã được thiết lập ValueMember là "Id" nên khi lấy SelectedValue sẽ trả về mã lớp học
            sv.ClassId = cbxLopHoc.SelectedValue.ToString();

            try
            {
                // thực hiện truy vấn thêm sinh viên vào database
                db.Students.InsertOnSubmit(sv);
                db.SubmitChanges();

                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }

        }

        public void LoadData()
        {
            List<Student> dsSinhVien = db.Students.ToList();
            dgvSinhVien.DataSource = dsSinhVien;
            if (dgvSinhVien.Columns["Class"] != null)
            {
                dgvSinhVien.Columns["Class"].Visible = false;
            }
            // bỏ qua cột IsDeleted 
            if (dgvSinhVien.Columns["IsDeleted"] != null)
            {
                dgvSinhVien.Columns["IsDeleted"].Visible = false;
            }
            lblPageInfo.Text = $"Trang {currentPage}/{totalPages} | {totalRecords} bản ghi";
        }
        public void LoadLopHoc()
        {
            List<Class> dsLopHoc = db.Classes.ToList();
            cbxLopHoc.DataSource = dsLopHoc;

            // hiển thị tên , lúc lấy về lấy mã lớp 
            cbxLopHoc.DisplayMember = "ClassName";
            cbxLopHoc.ValueMember = "ClassId";
        }
        private void btn_edit_Click(object sender, EventArgs e)
        {
            try
            {
                string mssv = txt_mssv.Text;
                Student sv = db.Students.FirstOrDefault(s => s.MSSV == mssv);

                if (sv != null)
                {
                    sv.FullName = txt_name.Text;
                    sv.Gender = cboGioiTinh.Text;
                    sv.DateOfBirth = dtpNgaySinh.Value;
                    sv.ClassId = cbxLopHoc.SelectedValue?.ToString();

                    db.SubmitChanges();
                    MessageBox.Show("Cập nhật thông tin thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                }
                else
                {
                    MessageBox.Show("Vui lòng chọn sinh viên cần sửa!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void btn_clear_Click(object sender, EventArgs e)
        {
            txt_mssv.Text = "";
            txt_name.Text = "";
            cboGioiTinh.SelectedIndex = -1;
            cbxLopHoc.SelectedIndex = 0;

            dtpNgaySinh.Value = DateTime.Now;

            txt_mssv.Enabled = true;

            currentSearchStr = "";
            if (text_search != null) text_search.Text = ""; // Xóa text ở ô tìm kiếm nếu có
            currentPage = 1;

            LoadData();
        }
        private void btn_head_Click(object sender, EventArgs e)
        {
            currentPage = 1;
            LoadData();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                LoadData();
            }
        }

        private void btn_next_click_Click(object sender, EventArgs e)
        {
            if (currentPage < totalPages)
            {
                currentPage++;
                LoadData();
            }
        }

        private void btn_tail_Click(object sender, EventArgs e)
        {
            currentPage = totalPages;
            LoadData();
        }


        private void btn_search_Click(object sender, EventArgs e)
        {
            // Cập nhật từ khóa và ép load lại từ trang 1
            currentSearchStr = text_search.Text.Trim();
            currentPage = 1;
            LoadData();
        }
        private void dgvSinhVien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {

                DataGridViewRow row = dgvSinhVien.Rows[e.RowIndex];

                txt_mssv.Text = row.Cells["MSSV"].Value?.ToString();
                txt_name.Text = row.Cells["FullName"].Value?.ToString();
                cboGioiTinh.Text = row.Cells["Gender"].Value?.ToString();

                if (row.Cells["DateOfBirth"].Value != null)
                {
                    dtpNgaySinh.Value = Convert.ToDateTime(row.Cells["DateOfBirth"].Value);
                }

                cbxLopHoc.SelectedValue = row.Cells["ClassId"].Value?.ToString();

                txt_mssv.Enabled = false;
            }
        }
    }
}