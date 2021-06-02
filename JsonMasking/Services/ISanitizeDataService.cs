using Newtonsoft.Json;

namespace JsonMasking.Services
{
    public interface ISanitizeDataService
    {
        string Sanitize(object value);
    }

    public class SanitizeDataService : ISanitizeDataService
    {
        private readonly SensitiveAuditingFieldsContext _sensitiveAuditingFields;
        private readonly IJsonMasking _jsonMasking;
        public SanitizeDataService(SensitiveAuditingFieldsContext sensitiveAuditingFields)
        {
            _sensitiveAuditingFields = sensitiveAuditingFields;
        }

        public string Sanitize(object value)
        {
            if (_sensitiveAuditingFields.Values == null || _sensitiveAuditingFields.Values.Length == 0)
                return JsonConvert.SerializeObject(value);

            string jsonMaskedResult = value.MaskFields(_sensitiveAuditingFields.Values);

            return jsonMaskedResult;
        }
    }

    public interface IJsonMasking
    {
        string Mask(object value, string sensitiveFileds);
    }
}
