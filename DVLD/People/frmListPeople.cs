using DVLD.GlobalHelpers;
using DVLD_Buisness;
using System;
using System.Data;
using System.Windows.Forms;

namespace DVLD
{
    public partial class frmListPeople : Form
    {
        private DataTable _dtAllPeople;


        public frmListPeople()
        {
            InitializeComponent();
        }

        private void _RefreshPeopleList()
        {
            _dtAllPeople = clsPerson.GetAllPeople();

            if (_dtAllPeople != null)
            {
                dgvPeople.DataSource = _dtAllPeople;
                lblRecordsCount.Text = dgvPeople.Rows.Count.ToString();

                if (dgvPeople.Rows.Count > 0)
                {
                    dgvPeople.Columns[0].Width = 150;
                    dgvPeople.Columns[1].Width = 180;
                    dgvPeople.Columns[2].Width = 150;
                    dgvPeople.Columns[3].Width = 170;
                    dgvPeople.Columns[4].Width = 170;
                    dgvPeople.Columns[5].Width = 170;
                    dgvPeople.Columns[6].Width = 135;
                    dgvPeople.Columns[7].Width = 250;
                    dgvPeople.Columns[8].Width = 155;
                    dgvPeople.Columns[9].Width = 135;
                    dgvPeople.Columns[10].Width = 175;
                    dgvPeople.Columns[11].Width = 298;
                }

                if (dgvPeople.Columns.Contains("ImagePath"))
                {
                    dgvPeople.Columns["ImagePath"].Visible = false;
                }
            }
            else
            {
                dgvPeople.DataSource = null;
                lblRecordsCount.Text = "0";
            }
        }

        private void btnAddPerson_Click(object sender, EventArgs e)
        {
            frmAddEditPerson frm = new frmAddEditPerson();

            frm.DataBack += Frm_DataBack;

            frm.ShowDialog();
        }

        private void frmListPeople_Load(object sender, EventArgs e)
        {
            _RefreshPeopleList();

            cbFilterBy.SelectedIndex = 0;
            txtFilterValue.Visible = false;
        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFilterValue.Text = "";
            _dtAllPeople.DefaultView.RowFilter = "";

            if (cbFilterBy.Text == "Gender")
            {
                txtFilterValue.Visible = false;
                cbGender.Visible = true;
                cbGender.SelectedIndex = 0;
            }
            else if (cbFilterBy.Text == "None")
            {
                txtFilterValue.Visible = false;
                cbGender.Visible = false;
            }
            else
            {
                txtFilterValue.Visible = true;
                cbGender.Visible = false;
                txtFilterValue.Focus();
            }

            lblRecordsCount.Text = dgvPeople.Rows.Count.ToString();
        }

        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {
            string filterColumn = cbFilterBy.Text;
            string filterValue = txtFilterValue.Text.Trim();

            if (string.IsNullOrEmpty(filterValue) || filterColumn == "None")
            {
                _dtAllPeople.DefaultView.RowFilter = "";
                lblRecordsCount.Text = dgvPeople.Rows.Count.ToString();
                return;
            }

            if (filterColumn == "Person ID")
            {
                if (int.TryParse(filterValue, out int PersonID))
                    _dtAllPeople.DefaultView.RowFilter = $"[{filterColumn}] = {PersonID}";
                else
                    _dtAllPeople.DefaultView.RowFilter = "1 = 0";
            }
            else
            {
                _dtAllPeople.DefaultView.RowFilter = $"[{filterColumn}] LIKE '{filterValue}%'";
            }

            lblRecordsCount.Text = dgvPeople.Rows.Count.ToString();
        }

        private void cbGender_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedGender = cbGender.Text;

            if (selectedGender == "All")
                _dtAllPeople.DefaultView.RowFilter = "";
            else
                _dtAllPeople.DefaultView.RowFilter = $"Gender = '{selectedGender}'";

            lblRecordsCount.Text = dgvPeople.Rows.Count.ToString();
        }

        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cbFilterBy.Text == "Person ID")
            {
                clsValidation.AllowOnlyNumbers(e);
            }
        }

        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvPeople.CurrentRow == null || dgvPeople.CurrentRow.Cells["Person ID"].Value == null)
                return;

            int selectedPersonID = Convert.ToInt32(dgvPeople.CurrentRow.Cells["Person ID"].Value);

            frmShowPersonInfo frm = new frmShowPersonInfo(selectedPersonID);

            frm.PersonCard.OnPersonUpdated += (PersonID) => _RefreshPeopleList();

            frm.ShowDialog();
        }

        private void addNewPersonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAddEditPerson frm = new frmAddEditPerson();

            frm.DataBack += Frm_DataBack;

            frm.ShowDialog();
        }

        private void editPersonInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvPeople.CurrentRow == null || dgvPeople.CurrentRow.Cells["Person ID"].Value == null)
                return;

            int selectedPersonID = Convert.ToInt32(dgvPeople.CurrentRow.Cells["Person ID"].Value);

            frmAddEditPerson frm = new frmAddEditPerson(selectedPersonID);

            frm.DataBack += Frm_DataBack;

            frm.ShowDialog();
        }

        private void Frm_DataBack(object sender, int PersonID)
        {
            _RefreshPeopleList();
        }

        private void deletePersonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvPeople.CurrentRow == null || dgvPeople.CurrentRow.Cells["Person ID"].Value == null)
                return;

            int selectedPersonID = Convert.ToInt32(dgvPeople.CurrentRow.Cells["Person ID"].Value);

            DialogResult dialogResult = MessageBox.Show($"Are you sure you want to delete person [{selectedPersonID}]?", "Confirm Delete",
                                                MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);

            if (dialogResult == DialogResult.OK)
            {
                if (clsPerson.DeletePerson(selectedPersonID))
                {
                    MessageBox.Show($"Person with ID [{selectedPersonID}] deleted successfully.", "Success",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                    _RefreshPeopleList();
                }
                else
                {
                    MessageBox.Show("Person was not deleted because it is linked to other data in the system.", "Error",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void _ShowUnderConstructionMessage()
        {
            MessageBox.Show("This feature is currently under development and will be available soon.",
                            "Feature Under Construction",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
        }

        private void sendEmailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _ShowUnderConstructionMessage();
        }

        private void phoneCallToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _ShowUnderConstructionMessage();
        }
    }
}
