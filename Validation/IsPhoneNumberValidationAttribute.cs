using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace MvcHelpers.Validation
{
    public sealed class IsPhoneNumberValidationAttribute : ValidationAttribute
    {
        private const int minimumNumberLength = 9;

        public string PropertyName { get; private set; }

        public IsPhoneNumberValidationAttribute([CallerMemberName] string propertyName = null)
            : base(String.Format("{0} is not a valid phone number", propertyName))
        {
            PropertyName = propertyName;
        }

        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return true;
            }

            var number = value.ToString();

            var cleanNumber = Regex.Replace(number, @"[^\d]", String.Empty);

            return !(String.IsNullOrEmpty(number) || cleanNumber.Length > minimumNumberLength || cleanNumber.Any(x => !Char.IsDigit(x)));
        }
    }
}