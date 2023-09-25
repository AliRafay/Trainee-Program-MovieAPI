using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Movies.WebAPI.CustomAttributes
{
    public class ContainsSubstringAttribute : ValidationAttribute
    {
        private readonly string _substring;

        public ContainsSubstringAttribute(string substring)
        {
            _substring = substring;
        }

        public override bool IsValid(object? value)
        {
            if (value == null)
            {
                return false;
            }
            ErrorMessage = $"The field name does not contain {_substring}";
            return value.ToString().Contains(_substring);
        }
    }

    public class PhoneNumberAttribute : ValidationAttribute
    {
        //private readonly string _phoneNumber;
        //public PhoneNumberAttribute(string phoneNumber)
        //{
        //    _phoneNumber = phoneNumber;
        //}

        public override bool IsValid(object? value) 
        { 
            var regex = new Regex("^[0-9]{3}-[0-9]{3}-[0-9]{4}$");
            return regex.IsMatch(value.ToString());
        }
    }

}
