using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace MvcHelpers.Validation
{
    public sealed class PropertyRequiredAttribute : RequiredAttribute
    {
        public string PropertyName { get; private set; }

        public PropertyRequiredAttribute([CallerMemberName] string propertyName = null)
        {
            PropertyName = propertyName;

            AllowEmptyStrings = false;

            ErrorMessage = String.Format("{0} required", PropertyName);
        }
    }
}