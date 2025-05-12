using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Extensions
{
    public static class ReportExtensions
    {
        public static string GetPropertyValue(this object obj, string propertyName)
        {
            return obj?.GetType().GetProperty(propertyName)?.GetValue(obj, null)?.ToString();
        }
    }
}
