using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;

namespace MvcHelpers.Validation
{
    public sealed class StringLengthValidationAttribute : StringLengthAttribute
    {
        public string PropertyName { get; private set; }

        public int MaxLength { get; private set; }

        public StringLengthValidationAttribute(int maxLength, [CallerMemberName] string propertyName = null)
            : base(maxLength)
        {
            PropertyName = propertyName;
            MaxLength = maxLength;

            ErrorMessage = String.Format("{0} cannot be longer than {1} characters", PropertyName, MaxLength);
        }
    }
}