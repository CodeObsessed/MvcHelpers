using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace MvcHelpers.Validation
{
    public sealed class IsNumberValidationAttribute : ValidationAttribute
    {
        public string PropertyName { get; private set; }

        public IsNumberValidationAttribute([CallerMemberName] string propertyName = null)
            : base(String.Format("{0} is not numerical", propertyName))
        {
            PropertyName = propertyName;
        }

        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return true;
            }

            var match = Regex.Match(value.ToString(), @"^\d+$");

            return match.Success;
        }
    }
}
