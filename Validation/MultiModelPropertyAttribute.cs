using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using MvcHelpers.Interfaces;
using MvcHelpers.Models.Enums;

namespace MvcHelpers.Validation
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public sealed class MultiModelPropertyAttribute : ValidationAttribute
    {
        public int ModelType { get; private set; }
        public bool Required { get; set; }
        public bool Valid { get; set; }
        public string PropertyName { get; private set; }

        public MultiModelPropertyAttribute(int modelType, [CallerMemberName] string propertyName = null)
            : this(modelType, false, propertyName)
        {
        }

        public MultiModelPropertyAttribute(int modelType, bool isRequired, [CallerMemberName] string propertyName = null)
        {
            ModelType = modelType;

            Required = isRequired;

            PropertyName = propertyName;
            
            ErrorMessage = String.Format("{0} required", PropertyName);
        }
        public override bool IsValid(object value)
        {
            Valid = (value != null) && !String.IsNullOrEmpty(value.ToString());

            return true;
        }
    }
}