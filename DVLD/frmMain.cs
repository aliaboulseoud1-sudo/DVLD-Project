using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void _ShowUnderConstructionMessage()
        {
            MessageBox.Show("This feature is currently under development and will be available soon.",
                            "Feature Under Construction",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
        }

        private void localLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _ShowUnderConstructionMessage();
        }

        private void internationalLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _ShowUnderConstructionMessage();
        }

        private void renewDrivingLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _ShowUnderConstructionMessage();
        }

        private void replacementForLostOrDamagedLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _ShowUnderConstructionMessage();
        }

        private void releaseDetainedDrivingLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _ShowUnderConstructionMessage();
        }

        private void retakeTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _ShowUnderConstructionMessage();
        }

        private void localDrivingLicenseApplicationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _ShowUnderConstructionMessage();
        }

        private void internationalLicenseApplicationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _ShowUnderConstructionMessage();
        }

        private void manageDetainedLicensesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _ShowUnderConstructionMessage();
        }

        private void detainLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _ShowUnderConstructionMessage();
        }

        private void releaseDetainedLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _ShowUnderConstructionMessage();
        }

        private void manageApplicationTypesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _ShowUnderConstructionMessage();
        }

        private void manageTestTypesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _ShowUnderConstructionMessage();
        }

        private void peopleToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            frmListPeople frm = new frmListPeople();
            frm.ShowDialog();
        }

        private void driversToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            _ShowUnderConstructionMessage();
        }

        private void employeesToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            _ShowUnderConstructionMessage();
        }

        private void currentUserInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _ShowUnderConstructionMessage();
        }

        private void changePasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _ShowUnderConstructionMessage();
        }

        private void signOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _ShowUnderConstructionMessage();
        }
    }
}
