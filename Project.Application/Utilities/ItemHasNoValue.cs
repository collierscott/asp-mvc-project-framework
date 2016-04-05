using System;

namespace Project.Application.Utilities
{
    public static class ItemHasNoValue
    {
        public static bool Check(string value)
        {
            return string.IsNullOrWhiteSpace(value) || value.Equals("ALL", StringComparison.OrdinalIgnoreCase)
                   || value.Equals("NONE", StringComparison.OrdinalIgnoreCase);
        }

    }
}