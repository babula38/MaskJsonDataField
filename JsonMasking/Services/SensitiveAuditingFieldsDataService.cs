using System.Collections.Generic;

namespace JsonMasking.Services
{
    public class SensitiveAuditingFields : ISensitiveAuditingFields
    {
        private readonly Dictionary<string, string> _values;
        public SensitiveAuditingFields()
        {
            _values = new Dictionary<string, string>()
            {
                {"WeatherForecastController","Password" }
            };
        }
        public string Get(string featureName)
        {
            return _values.TryGetValue(featureName, out string fields) ? fields : string.Empty;

        }

        public Dictionary<string, string> GetValues() => _values;
    }
}
