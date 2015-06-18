using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using ValueObjects;

namespace MvcHelpers.Validation
{
    public sealed class IsEmailValidationAttribute : ValidationAttribute
    {
        public string PropertyName { get; private set; }

        public IsEmailValidationAttribute([CallerMemberName] string propertyName = null)
            : base(String.Format("{0} is not a valid email", propertyName))
        {
            PropertyName = propertyName;
        }

        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return true;
            }

            var text = value.ToString();

            return String.IsNullOrEmpty(text) || Email.IsValid(text);
        }
    }
}