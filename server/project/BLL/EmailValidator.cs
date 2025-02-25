using System.Text.RegularExpressions;

namespace project.BLL
{
    public class EmailValidator
    {
        public bool IsValidEmail(string email)
        {
            // בדיקת פורמט כתובת הדוא"ל
            if (!Regex.IsMatch(email, @"^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$"))
            {
                return false;
            }
            return true;
        }
    }
}
