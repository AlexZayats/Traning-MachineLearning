using System;

namespace JARVIS.Core.Networking.Extensions
{
    public static class UriBuilderExtensions
    {
        public static void AddParameter(this UriBuilder builder, string name, object value)
        {
            builder.Query += (!string.IsNullOrEmpty(builder.Query) ? "&" : "") + $"{name}={value}";
        }
    }
}
