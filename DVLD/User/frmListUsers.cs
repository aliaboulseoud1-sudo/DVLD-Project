using DVLD.GlobalHelpers;
using DVLD_Buisness;
using System;
using System.Data;
using System.Windows.Forms;

namespace DVLD.User
{
    public partial class frmListUsers : Form
    {
        private DataTable _dtAllUsers;

        public frmListUsers()
        {
            InitializeComponent();
        }

        private void _RefreshUsersList()
        {
            _dtAllUsers = clsUser.GetAllUsers();

            if (_dtAllUsers != null)
            {
                dgvUsers.DataSource = _dtAllUsers;
                lblRecordsCount.Text = dgvUsers.Rows.Count.ToString();

                if (dgvUsers.Rows.Count > 0)
                {
                    dgvUsers.Columns[0].Width = 150;
                    dgvUsers.Columns[1].Width = 180;
                    dgvUsers.Columns[2].Width = 400;
                    dgvUsers.Columns[3].Width = 200;
                    dgvUsers.Columns[4].Width = 170;
                }

                if (dgvUsers.Columns.Contains("ImagePath"))
                {
                    dgvUsers.Columns["ImagePath"].Visible = false;
                }
            }
            else
            {
                dgvUsers.DataSource = null;
                lblRecordsCount.Text = "0";
            }
        }

        private void frmListUsers_Load(object sender, EventArgs e)
        {
            _RefreshUsersList();

            cbFilterBy.SelectedIndex = 0;
            txtFilterValue.Visible = false;
        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFilterValue.Text = "";
            _dtAllUsers.DefaultView.RowFilter = "";

            if (cbFilterBy.Text == "Is Active")
            {
                txtFilterValue.Visible = false;
                cbIsActive.Visible = true;
                cbIsActive.SelectedIndex = 0;
            }
            else if (cbFilterBy.Text == "None")
            {
                txtFilterValue.Visible = false;
                cbIsActive.Visible = false;
            }
            else
            {
                txtFilterValue.Visible = true;
                cbIsActive.Visible = false;
                txtFilterValue.Focus();
            }

            lblRecordsCount.Text = dgvUsers.Rows.Count.ToString();
        }

        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {
            string filterColumn = cbFilterBy.Text;
            string filterValue = txtFilterValue.Text.Trim();

            if (string.IsNullOrWhiteSpace(filterValue) || filterColumn == "None")
            {
                _dtAllUsers.DefaultView.RowFilter = "";
                lblRecordsCount.Text = dgvUsers.Rows.Count.ToString();
                return;
            }

            if (filterColumn == "User ID")
            {
                if (int.TryParse(filterValue, out int UserID))
                    _dtAllUsers.DefaultView.RowFilter = $"[{filterColumn}] = {UserID}";
                else
                    _dtAllUsers.DefaultView.RowFilter = "1 = 0";
            }
            else if (filterColumn == "Person ID")
            {
                if (int.TryParse(filterValue, out int PersonID))
                    _dtAllUsers.DefaultView.RowFilter = $"[{filterColumn}] = {PersonID}";
                else
                    _dtAllUsers.DefaultView.RowFilter = "1 = 0";
            }
            else
            {
                _dtAllUsers.DefaultView.RowFilter = $"[{filterColumn}] LIKE '{filterValue}%'";
            }

            lblRecordsCount.Text = dgvUsers.Rows.Count.ToString();
        }

        private void cbIsActive_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedStatus = cbIsActive.Text;

            if (selectedStatus == "All")
                _dtAllUsers.DefaultView.RowFilter = "";
            else if (selectedStatus == "Yes")
                _dtAllUsers.DefaultView.RowFilter = $"[Is Active] = {1}";
            else if (selectedStatus == "No")
                _dtAllUsers.DefaultView.RowFilter = $"[Is Active] = {0}";

            lblRecordsCount.Text = dgvUsers.Rows.Count.ToString();
        }

        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cbFilterBy.Text == "User ID" || cbFilterBy.Text == "Person ID")
            {
                clsValidation.AllowOnlyNumbers(e);
            }
        }

        private void btnAddUser_Click(object sender, EventArgs e)
        {
            frmAddEditUser frmAddEditUser = new frmAddEditUser();
            frmAddEditUser.ShowDialog();

            // Future Imrovement: replaced with delegate.
            _RefreshUsersList();
        }
    }
}
