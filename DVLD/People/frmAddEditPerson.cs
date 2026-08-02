using DVLD.GlobalHelpers;
using DVLD.Properties;
using DVLD_Buisness;
using System;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace DVLD
{
    public partial class frmAddEditPerson : Form
    {
        private enum enMode { AddNew, Update};
        private enMode _Mode = enMode.AddNew;

        private int _PersonID = -1;
        private clsPerson _Person = new clsPerson();

        private DataTable _dtAllCountries;

        public delegate void DataBackEventHandler(object sender, int PersonID);
        public event DataBackEventHandler DataBack;


        public frmAddEditPerson()
        {
            InitializeComponent();

            _Mode = enMode.AddNew;
        }

        public frmAddEditPerson(int PersonID)
        {
            InitializeComponent();

            _PersonID = PersonID;
            _Mode = enMode.Update;
        }

        private void _FillCountriesInComboBox()
        {
            _dtAllCountries = clsCountry.GetAllCountries();

            if (_dtAllCountries != null && _dtAllCountries.Rows.Count > 0)
            {
                cbCountry.DataSource = _dtAllCountries;
                cbCountry.DisplayMember = "CountryName";
                cbCountry.ValueMember = "CountryID";
            }
        }

        private void _LoadPersonData(int PersonID)
        {
            _FillCountriesInComboBox();

            if (_Mode == enMode.AddNew)
            {
                lblTitle.Text = "Add New Person";
                cbCountry.SelectedIndex = cbCountry.FindStringExact("Egypt");
                _Person = new clsPerson();
                return;
            }

            lblTitle.Text = "Update Person Info";

            _Person = clsPerson.Find(PersonID);

            if (_Person == null)
            {
                MessageBox.Show($"No person with ID [{PersonID}] was found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

            lblPersonID.Text = _Person.PersonID.ToString();
            txtFirstName.Text = _Person.FirstName;
            txtSecondName.Text = _Person.SecondName;
            txtThirdName.Text = _Person.ThirdName;
            txtLastName.Text = _Person.LastName;
            txtNationalNo.Text = _Person.NationalNo;
            dtpDateOfBirth.Value = _Person.DateOfBirth;

            if (_Person.Gender == clsPerson.enGender.Male)
                rbMale.Checked = true;
            else
                rbFemale.Checked = true;

            txtPhone.Text = _Person.PhoneNumber;
            txtEmail.Text = _Person.Email;
            txtAddress.Text = _Person.Address;

            clsCountry country = clsCountry.Find(_Person.CountryID);

            if (country != null)
            {
                cbCountry.SelectedIndex = cbCountry.FindStringExact(country.CountryName);
            }

            if (!string.IsNullOrEmpty(_Person.ImagePath) && File.Exists(_Person.ImagePath))
            {
                pbPersonImage.ImageLocation = _Person.ImagePath;
                llRemoveImage.Visible = true;
            }
            else
            {
                llRemoveImage.Visible = false;
            }
        }

        private void frmAddEditPerson_Load(object sender, EventArgs e)
        {
            dtpDateOfBirth.MaxDate = DateTime.Now.AddYears(-18);
            dtpDateOfBirth.Value = dtpDateOfBirth.MaxDate;

            llRemoveImage.Visible = false;
           

            _LoadPersonData(_PersonID);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!this.ValidateChildren())
            {
                MessageBox.Show("Some fields are not valid! Hover over the red icons to see the error.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!_HandlePersonImage())
                return;

            _Person.FirstName = txtFirstName.Text.Trim();
            _Person.SecondName = txtSecondName.Text.Trim();
            _Person.ThirdName = txtThirdName.Text.Trim();
            _Person.LastName = txtLastName.Text.Trim();
            _Person.NationalNo = txtNationalNo.Text.Trim();
            _Person.Email = txtEmail.Text.Trim();
            _Person.DateOfBirth = dtpDateOfBirth.Value;
            _Person.PhoneNumber = txtPhone.Text.Trim();
            _Person.Address = txtAddress.Text.Trim();

            _Person.Gender = (rbMale.Checked) ? clsPerson.enGender.Male : clsPerson.enGender.Female;
            _Person.CountryID = Convert.ToInt32(cbCountry.SelectedValue);

            if (_Person.Save())
            {
                MessageBox.Show("Data Saved Successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                _Mode = enMode.Update;
                _PersonID = _Person.PersonID;
                lblTitle.Text = "Update Person";
                lblPersonID.Text = _PersonID.ToString();

                DataBack?.Invoke(this, _PersonID);
            }
            else
            {
                MessageBox.Show("Error: Data Was Not Saved Successfully.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtNationalNo_Validating(object sender, CancelEventArgs e)
        {
            string NationalNo = txtNationalNo.Text.Trim();

            if (string.IsNullOrWhiteSpace(NationalNo))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtNationalNo, "The national ID number must be entered!");
                return;
            }

            bool isExist = false;

            if (_Mode == enMode.AddNew)
            {
                isExist = clsPerson.IsPersonExist(NationalNo);
            }
            else
            {
                isExist = clsPerson.IsPersonExist(NationalNo, _PersonID);
            }

            if (isExist)
            {
                e.Cancel = true;
                errorProvider1.SetError(txtNationalNo, "This national ID number is already registered to another person!");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtNationalNo, "");
            }
        }

        private void rbMale_CheckedChanged(object sender, EventArgs e)
        {
            if (rbMale.Checked && string.IsNullOrEmpty(pbPersonImage.ImageLocation))
            {
                pbPersonImage.Image = Resources.Male_512;
            }
        }

        private void rbFemale_CheckedChanged(object sender, EventArgs e)
        {
            if (rbFemale.Checked && string.IsNullOrEmpty(pbPersonImage.ImageLocation))
            {
                pbPersonImage.Image = Resources.Female_512;
            }
        }

        private void txtEmail_Validating(object sender, CancelEventArgs e)
        {
            string email = txtEmail.Text.Trim();

            if (string.IsNullOrWhiteSpace(email))
            {
                e.Cancel = false;
                errorProvider1.SetError(txtEmail, "");
                return;
            }

            if (!clsValidation.IsValidEmail(email))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtEmail, "Invalid email format! (Example: name@domain.com)");
                return;
            }

            int currentPersonID = (_Mode == enMode.AddNew) ? -1 : _PersonID;

            if (clsPerson.IsEmailExist(email, currentPersonID))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtEmail, "This email address is already in use by someone else!");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtEmail, "");
            }
        }

        private void llSetImage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp;*.jfif";
            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string selectedFilePath = openFileDialog.FileName;

                pbPersonImage.Load(selectedFilePath);

                llRemoveImage.Visible = true;
            }
        }

        private bool _HandlePersonImage()
        {
            string oldImagePath = _Person.ImagePath;

            if (string.IsNullOrEmpty(pbPersonImage.ImageLocation))
            {
                _Person.ImagePath = "";

                if (!string.IsNullOrEmpty(oldImagePath) && File.Exists(oldImagePath))
                {
                    try
                    {
                        if (pbPersonImage.Image != null)
                        {
                            pbPersonImage.Image.Dispose();
                            pbPersonImage.Image = null;
                        }

                        File.Delete(oldImagePath);
                    }
                    catch (Exception ex)
                    {
                        clsLogger.LogToTextFile(ex);
                    }
                }

                return true;
            }

            if (pbPersonImage.ImageLocation == _Person.ImagePath)
            {
                return true;
            }

            string DestinationFolder = Path.Combine(Application.StartupPath, "DVLD_People_Images");

            if (!Directory.Exists(DestinationFolder))
            {
                try
                {
                    Directory.CreateDirectory(DestinationFolder);
                }
                catch (Exception ex)
                {
                    clsLogger.LogToTextFile(ex);

                    MessageBox.Show($"Error creating image directory: {ex.Message}", "Error",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);

                    return false;
                }
            }

            string sourceFilePath = pbPersonImage.ImageLocation;
            string newFileName = clsUtil.GenerateGUID() + Path.GetExtension(sourceFilePath);
            string destinationFilePath = Path.Combine(DestinationFolder, newFileName);

            try
            {
                File.Copy(sourceFilePath, destinationFilePath, true);

                if (!string.IsNullOrEmpty(oldImagePath) && File.Exists(oldImagePath))
                {
                    try
                    {
                        if (pbPersonImage.Image != null)
                        {
                            pbPersonImage.Image.Dispose();
                            pbPersonImage.Image = null;
                        }

                        File.Delete(oldImagePath);
                    }
                    catch(Exception ex)
                    {
                        clsLogger.LogToTextFile(ex);
                    }
                }

                _Person.ImagePath = destinationFilePath;

                pbPersonImage.ImageLocation = destinationFilePath;

                return true;
            }
            catch (IOException ex)
            {
                MessageBox.Show($"Error copying image file: {ex.Message}", "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);

                clsLogger.LogToTextFile(ex);

                return false;
            }
        }

        private void llRemoveImage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            pbPersonImage.ImageLocation = null;

            if (rbMale.Checked)
                pbPersonImage.Image = Properties.Resources.Male_512;
            else
                pbPersonImage.Image = Properties.Resources.Female_512;

            llRemoveImage.Visible = false;
        }

        private void txtTextBox_Validating(object sender, CancelEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            string text = textBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(text))
            {
                e.Cancel = true;
                errorProvider1.SetError(textBox, "This field is required!");
                return;
            }

            e.Cancel = false;
            errorProvider1.SetError(textBox, "");
        }

        private void txtPhone_Validating(object sender, CancelEventArgs e)
        {
            string phone = txtPhone.Text.Trim();

            if (string.IsNullOrWhiteSpace(phone))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtPhone, "The phone number must be entered!");
                return;
            }

            if (phone.Length != 11 || !phone.All(char.IsDigit))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtPhone, "The phone number must consist of exactly 11 digits!");
                return;
            }

            e.Cancel = false;
            errorProvider1.SetError(txtPhone, "");
        }
    }
}
