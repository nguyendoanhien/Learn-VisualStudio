using System.Text.RegularExpressions;

namespace Utilities
{
    public class Phone
    {
        public static bool IsPhoneNumber(string number)
        {
            return Regex.Match(number, @"^(\+[0-9]{9})$").Success;
        }
    }
}