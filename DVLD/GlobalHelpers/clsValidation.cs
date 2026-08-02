using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace DVLD.GlobalHelpers
{
    public static class clsValidation
    {
        public static void AllowOnlyNumbers(KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
            {
                e.Handled = true;
            }
        }

        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

            return Regex.IsMatch(email, pattern, RegexOptions.IgnoreCase);
        }



    }
}
