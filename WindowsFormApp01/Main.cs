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
    public partial class Main : Form
    {
        // Khai báo các thành phần giao diện
        private Panel panelMenu;
        private Panel panelContainer;
        private Button btnQLLH;
        private Button btnQLSV;
        private Button btnDangXuat;

        // Khởi tạo sẵn 2 UserControl (Đảm bảo bạn đã tạo 2 file UC_QLLH và UC_QLSV nhé)
        private UC_QLLH uc_qllh = new UC_QLLH();
        private UC_QLSV uc_qlsv = new UC_QLSV();

        public Main()
        {
            InitializeComponent(); // Gọi hàm này để Visual Studio nhận diện Form
            TaoGiaoDienBangCode();   // Tự động dựng giao diện đẹp bằng code của bạn
        }

        private void Main_Load(object sender, EventArgs e)
        {
            // Có thể để trống hoặc viết code xử lý khi vừa chạy Form
        }

        private void TaoGiaoDienBangCode()
        {
            // 1. Cấu hình Form chính
            this.Size = new Size(1100, 700);
            this.Text = "Quản lý Sinh Viên";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.White;

            // 2. Tạo thanh Menu ngang bên trên
            panelMenu = new Panel();
            panelMenu.Dock = DockStyle.Top;
            panelMenu.Height = 45;
            panelMenu.BackColor = Color.White;

            // 3. Tạo vùng chứa nội dung bên dưới
            panelContainer = new Panel();
            panelContainer.Dock = DockStyle.Fill;
            panelContainer.BackColor = Color.White;

            // 4. Khởi tạo các nút điều hướng
            btnQLSV = TaoNutMenu("Quản lý Sinh Viên", true);
            btnQLSV.Click += new EventHandler(btnQLSV_Click);

            btnQLLH = TaoNutMenu("Quản lý Lớp Học", false);
            btnQLLH.Click += new EventHandler(btnQLLH_Click);

            btnDangXuat = TaoNutMenu("Đăng xuất", false);
            btnDangXuat.ForeColor = Color.IndianRed;
            btnDangXuat.Click += new EventHandler(btnDangXuat_Click); // Thêm sự kiện click đăng xuất

            // 5. Ráp nút vào Menu (Add sau thì nằm bên phải do Dock = Left)
            panelMenu.Controls.Add(btnDangXuat);
            panelMenu.Controls.Add(btnQLLH);
            panelMenu.Controls.Add(btnQLSV);

            // 6. Đưa các Panel chính vào Form
            this.Controls.Add(panelContainer);
            this.Controls.Add(panelMenu);

            // Vừa mở giao diện lên thì hiện màn hình Sinh viên trước
            ShowUserControl(uc_qlsv);
        }

        // --- HÀM PHỤ: Tạo nút bấm giao diện phẳng giống ảnh ---
        private Button TaoNutMenu(string text, bool isActive)
        {
            Button btn = new Button();
            btn.Text = text;
            btn.Dock = DockStyle.Left;
            btn.AutoSize = true;
            btn.Padding = new Padding(15, 0, 15, 0);
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseOverBackColor = Color.WhiteSmoke;
            btn.FlatAppearance.MouseDownBackColor = Color.White;
            btn.BackColor = Color.White;
            btn.ForeColor = Color.Black;
            btn.Cursor = Cursors.Hand;

            if (isActive)
                btn.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            else
                btn.Font = new Font("Segoe UI", 10F, FontStyle.Regular);

            return btn;
        }

        // --- CÁC HÀM XỬ LÝ CHUYỂN TRANG PANEL ---
        private void ShowUserControl(UserControl uc)
        {
            panelContainer.Controls.Clear();
            uc.Dock = DockStyle.Fill;
            panelContainer.Controls.Add(uc);
        }

        private void btnQLLH_Click(object sender, EventArgs e)
        {
            ShowUserControl(uc_qllh);
            DoiTrangThaiNut(btnQLLH);
        }

        private void btnQLSV_Click(object sender, EventArgs e)
        {
            ShowUserControl(uc_qlsv);
            DoiTrangThaiNut(btnQLSV);
        }

        private void btnDangXuat_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc chắn muốn đăng xuất?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.Close(); // Đóng giao diện chính
            }
        }

        // Hàm đổi Font chữ bôi đậm cho nút điều hướng hoạt động
        private void DoiTrangThaiNut(Button activeBtn)
        {
            btnQLSV.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            btnQLLH.Font = new Font("Segoe UI", 10F, FontStyle.Regular);

            activeBtn.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        }
    }
}