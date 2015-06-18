using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace MvcHelpers.Validation
{
    public sealed class IsDateValidationAttribute : ValidationAttribute
    {
        public string PropertyName { get; private set; }

        public IsDateValidationAttribute([CallerMemberName] string propertyName = null)
            : base(String.Format("{0} is not a valid date", propertyName))
        {
            PropertyName = propertyName;
        }

        public override bool IsValid(object value)
        {
            if(value == null)
            {
                return true;
            }

            var inValue = value as String;

            DateTime outValue;

            var success = DateTime.TryParse(inValue, out outValue);

            return outValue != new DateTime() && success;
        }
    }
}