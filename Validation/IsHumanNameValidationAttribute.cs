using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using ValueObjects;

namespace MvcHelpers.Validation
{
    public sealed class IsHumanNameValidationAttribute : ValidationAttribute
    {
        private const int maxLength = 35;

        public string PropertyName { get; private set; }

        public IsHumanNameValidationAttribute([CallerMemberName] string propertyName = null)
            : base(String.Format("{0} cannot be empty or over {1} characters", propertyName, maxLength))
        {
            PropertyName = propertyName;
        }

        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return true;
            }

            var input = value as string;

            return HumanName.IsValid(input);
        }
    }
}