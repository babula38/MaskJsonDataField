using System.Collections.Generic;

namespace JsonMasking.Services
{
    public interface ISensitiveAuditingFields
    {
        string Get(string featureName);

        Dictionary<string, string> GetValues();
    }
}
