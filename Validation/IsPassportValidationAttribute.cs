using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using ValueObjects;

namespace MvcHelpers.Validation
{
    public sealed class IsPassportValidationAttribute : ValidationAttribute
    {
        private const int maxLength = 16;

        public string PropertyName { get; private set; }

        public IsPassportValidationAttribute([CallerMemberName] string propertyName = null)
            : base(String.Format("{0} is not a valid passport number", propertyName))
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

            return Passport.IsValid(inValue);
        }
    }
}