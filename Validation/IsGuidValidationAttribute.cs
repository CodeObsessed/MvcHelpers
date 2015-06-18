using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace MvcHelpers.Validation
{
    public sealed class IsGuidValidationAttribute : ValidationAttribute
    {
        public string PropertyName { get; private set; }

        public IsGuidValidationAttribute([CallerMemberName] string propertyName = null)
            : base(String.Format("{0} is not a valid Guid", propertyName))
        {
            PropertyName = propertyName;
        }

        public override bool IsValid(object value)
        {
            if(value == null)
            {
                return true;
            }

            var inValue = value as string;

            Guid outValue;

            return inValue != new Guid ().ToString() && Guid.TryParse(inValue, out outValue);
        }
    }
}