using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace Tourplaner.Infrastructure
{
    public abstract class ValidatedViewModel<T> : ViewModel<T>, IDataErrorInfo
    {
        string IDataErrorInfo.Error => throw new NotSupportedException("IDataErrorInfo.Error is not supported, use IDataErrorInfo.this[propertyName] instead.");

        string IDataErrorInfo.this[string propertyName] => GetErrorInfo(propertyName);

        public bool IsValid
        {
            get
            {
                List<ValidationResult> results = new List<ValidationResult>();
                return Validator.TryValidateObject(this, new ValidationContext(this), results);
            }
        }

        public string GetErrorInfo(string propertyName)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
            {
                throw new ArgumentException($"'{nameof(propertyName)}' cannot be null or whitespace", nameof(propertyName));
            }

            string error = string.Empty;

            object value = GetValue(propertyName);
            List<ValidationResult> results = new List<ValidationResult>(1);
            
            bool result = Validator.TryValidateProperty(
                value,
                new ValidationContext(this, null, null)
                {
                    MemberName = propertyName
                },
                results
            );

            if (!result)
            {
                ValidationResult validationResult = results.First();
                error = validationResult.ErrorMessage;
            }

            return error;
        }

        private object GetValue(string propertyName)
        {
            PropertyInfo propInfo = GetType().GetProperty(propertyName);
            return propInfo.GetValue(this);
        }
    }
}
