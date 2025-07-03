using KeeperSecurity.Vault;

namespace KeeperTool.Models.Credentials
{
    public class KeeperRecord
    {
        private readonly TypedRecord _keeperRecord;

        public string Username => GetUsernameField();
        public string Password => GetPasswordField();
        public string KeeperId => _keeperRecord.Uid.ToString();

        public KeeperRecord(TypedRecord keeperRecord)
        {
            _keeperRecord = keeperRecord;
        }

        private string GetUsernameField()
        {
            var usernameField = _keeperRecord.Fields.FirstOrDefault(f => f.FieldName == "login");
            if (usernameField == null) throw new KeyNotFoundException($"Username field not found in record with ID: {_keeperRecord.Uid}");

            var usernameText = usernameField.ObjectValue.ToString();
            if (string.IsNullOrEmpty(usernameText)) throw new InvalidOperationException($"Username field is empty or not found in record with ID: {_keeperRecord.Uid}");

            return usernameText;
        }

        private string GetPasswordField()
        {
            var passwordField = _keeperRecord.Fields.FirstOrDefault(f => f.FieldName == "password");
            if (passwordField == null) throw new KeyNotFoundException($"Password field not found in record with ID: {_keeperRecord.Uid}");

            var passwordText = passwordField.ObjectValue.ToString();
            if (string.IsNullOrEmpty(passwordText)) throw new InvalidOperationException($"Password field is empty or not found in record with ID: {_keeperRecord.Uid}");

            return passwordText;
        }

        public string GetCustomField(string customFieldName)
        {
            var customField = _keeperRecord.Custom.FirstOrDefault(f => f.FieldLabel.ToLower().Trim() == customFieldName.ToLower().Trim());
            if (customField == null) throw new KeyNotFoundException($"Custom field '{customFieldName}' not found in record with ID: {_keeperRecord.Uid}");

            var customFieldText = customField.ObjectValue.ToString();
            if (string.IsNullOrEmpty(customFieldText)) throw new InvalidOperationException($"Custom field '{customFieldName}' is empty or not found in record with ID: {_keeperRecord.Uid}");

            return customFieldText;
        }
    }
}