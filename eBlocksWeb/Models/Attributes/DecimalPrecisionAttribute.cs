using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace eBlocksWeb.Models.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
    public sealed class DecimalPrecisionAttribute : ValidationAttribute
    {
        private readonly uint _decimalPrecision;

        public DecimalPrecisionAttribute(uint decimalPrecision)
        {
            _decimalPrecision = decimalPrecision;
        }

        public override bool IsValid(object value)
        {
            return value is null || (value is decimal d && HasPrecision(d, _decimalPrecision));
        }

        private static bool HasPrecision(decimal value, uint precision)
        {
            string valueStr = value.ToString(CultureInfo.InvariantCulture);
            int indexOfDot = valueStr.IndexOf('.');
            if (indexOfDot == -1)
            {
                return true;
            }

            return valueStr.Length - indexOfDot - 1 <= precision;
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format("Maximum of {0} decimal places", _decimalPrecision);
        }
    }
}
