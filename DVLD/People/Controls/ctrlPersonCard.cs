using DVLD_Buisness;
using System;
using System.IO;
using System.Windows.Forms;

namespace DVLD
{
    public partial class ctrlPersonCard : UserControl
    {
        private clsPerson _Person = new clsPerson();
        private int _PersonID = -1;

        public int PersonID => _PersonID;
        public clsPerson SelectedPersonInfo => _Person;

        public event Action<int> OnPersonUpdated;
        protected virtual void PersonUpdated(int PersonID)
        {
            Action<int> handler = OnPersonUpdated;
            handler?.Invoke(PersonID);
        }

        public ctrlPersonCard()
        {
            InitializeComponent();
        }

        public void ResetPersonInfo()
        {
            _Person = null;
            _PersonID = -1;

            lblPersonID.Text = "[????]";
            lblFullName.Text = "[????]";
            lblNationalNo.Text = "[????]";
            lblGender.Text = "[????]";
            lblEmail.Text = "[????]";
            lblAddress.Text = "[????]";
            lblDateOfBirth.Text = "[????]";
            lblPhone.Text = "[????]";
            lblCountry.Text = "[????]";
            pbPersonImage.Image = Properties.Resources.Male_512;
        }

        public void LoadPersonInfo(int PersonID)
        {
            _Person = clsPerson.Find(PersonID);

            if (_Person == null)
            {
                ResetPersonInfo();
                MessageBox.Show($"No Person with ID = {PersonID} was found.", "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _PersonID = _Person.PersonID;

            lblPersonID.Text = _PersonID.ToString();

            lblFullName.Text = $"{_Person.FirstName} {_Person.SecondName} {(string.IsNullOrWhiteSpace(_Person.ThirdName) ? "" : _Person.ThirdName + " ")}{_Person.LastName}";

            lblNationalNo.Text = _Person.NationalNo;
            lblGender.Text = (_Person.Gender == clsPerson.enGender.Male) ? "Male" : "Female";

            lblEmail.Text = string.IsNullOrWhiteSpace(_Person.Email) ? "N/A" : _Person.Email;
            lblAddress.Text = _Person.Address;
            lblDateOfBirth.Text = _Person.DateOfBirth.ToShortDateString();
            lblPhone.Text = _Person.PhoneNumber;

            clsCountry country = clsCountry.Find(_Person.CountryID);
            lblCountry.Text = (country != null) ? country.CountryName : "N/A";

            _LoadPersonImage();
        }

        private void _LoadPersonImage()
        {
            if (_Person.Gender == clsPerson.enGender.Male)
                pbPersonImage.Image = Properties.Resources.Male_512;
            else
                pbPersonImage.Image = Properties.Resources.Female_512;

            string ImagePath = _Person.ImagePath;

            if (!string.IsNullOrWhiteSpace(ImagePath))
            {
                if (File.Exists(ImagePath))
                    pbPersonImage.ImageLocation = ImagePath;
                else
                    MessageBox.Show($"Could not find this image: {ImagePath}", "Error",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void llEditPersonInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (_PersonID == -1) return;

            frmAddEditPerson frm = new frmAddEditPerson(_PersonID);

            frm.DataBack += (s, PersonID) =>
            {
                LoadPersonInfo(PersonID);
                OnPersonUpdated?.Invoke(PersonID);
            };

            frm.ShowDialog();
        }
    }
}
