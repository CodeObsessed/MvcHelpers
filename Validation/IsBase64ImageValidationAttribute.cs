using System;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.IO;
using System.Net.Mime;
using System.Runtime.CompilerServices;
using ValueObjects;

namespace MvcHelpers.Validation
{
    public sealed class IsBase64ImageValidationAttribute : ValidationAttribute
    {
        public string PropertyName { get; private set; }

        public IsBase64ImageValidationAttribute([CallerMemberName] string propertyName = null)
            : base(String.Format("{0} is not a valid Base64 Image.", propertyName))
        {
            PropertyName = propertyName;
        }

        private string AppendValue(string value)
        {
            return ErrorMessage + "Value: '" + value + "'";
        }

        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return true;
            }

            var inValue = value as string;

            if (inValue.Replace(" ", "").Length % 4 != 0)
            {
                return false;
            }

            return Base64Image.IsValid(inValue);
        }
    }
}