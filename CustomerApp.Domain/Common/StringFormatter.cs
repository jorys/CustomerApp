﻿using System.Globalization;

namespace CustomerApp.Domain.Common;

internal static class StringFormatter
{
    internal static string FormatAsTitle(this string value)
    {
        return CultureInfo.InvariantCulture.TextInfo.ToTitleCase(value.ToLowerInvariant());
    }
}
