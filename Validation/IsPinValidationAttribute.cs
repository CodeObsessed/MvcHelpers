using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;

namespace MvcHelpers.Validation
{
    public sealed class IsPinValidationAttribute : ValidationAttribute
    {
        private const int expectedPinLength = 5;
        
        public string PropertyName { get; private set; }

        public IsPinValidationAttribute([CallerMemberName] string propertyName = null)
            : base(String.Format("{0} is not a valid pin", propertyName))
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

            return !(inValue.Length != expectedPinLength || inValue.Any(x => !Char.IsDigit(x)));
        }
    }
}