using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ValueObjects;

namespace MvcHelpers.Validation
{
    public sealed class IsColourValidationAttribute : ValidationAttribute
    {
        public string PropertyName { get; private set; }

        public IsColourValidationAttribute([CallerMemberName] string propertyName = null)
            : base(String.Format("{0} is not a valid date", propertyName))
        {
            PropertyName = propertyName;
        }

        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return true;
            }

            var inValue = value as String;

            return Colour.IsColourFormat(inValue);
        }
    }
}
