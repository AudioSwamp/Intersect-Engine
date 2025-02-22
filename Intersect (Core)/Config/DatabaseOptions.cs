﻿using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Intersect.Config
{
    public partial class DatabaseOptions
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public DatabaseType Type { get; set; } = DatabaseType.SQLite;

        public string Server { get; set; } = "localhost";

        public ushort Port { get; set; } = 3306;

        public string Database { get; set; } = "";

        public string Username { get; set; } = "";

        public string Password { get; set; } = "";

        [JsonConverter(typeof(StringEnumConverter))]
        [JsonProperty(
            NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Populate
        )]
        [DefaultValue(Logging.LogLevel.Error)]
        public Logging.LogLevel LogLevel { get; set; } = Logging.LogLevel.Error;
    }
}
