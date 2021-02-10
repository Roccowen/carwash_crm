using System.Text.RegularExpressions;

namespace carwash.Services
{
    public static class ValidService
    {
        public static Regex numberCheck = new Regex(@"(\+7\([0-9]{3,3}\)[0-9]{3,3}-[0-9]{2,2}-[0-9]{2,2})");
        public static Regex simpleNumberCheck = new Regex(@"\+7[0-9]{10,10}");
        public static Regex passwordCheck = new Regex(@"[\w\d]{6,}");
        public static Regex nameCheck = new Regex(@"^(([a-zA-Z' -]{1,40})|([а-яА-ЯЁёІіЇїҐґЄє' -]{1,40}))$");
        public static string ClearPhone(string phone) => phone.Replace("+7", "").Replace("(", "").Replace(")", "").Replace("-", "");
    }
}
