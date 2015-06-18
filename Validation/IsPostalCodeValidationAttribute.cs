using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace MvcHelpers.Validation
{
    public sealed class IsPostalCodeValidationAttribute : ValidationAttribute
    {
        public string PropertyName { get; private set; }

        public IsPostalCodeValidationAttribute([CallerMemberName] string propertyName = null)
            : base(String.Format("{0} is not a valid City", propertyName))
        {
            PropertyName = propertyName;
        }

        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return true;
            }

            var inValue = value as string;

            if (String.IsNullOrEmpty(inValue))
            {
                return true;
            }
            if (inValue.Length > 15)
            {
                return false;
            }
            return true;
        }
    }
}