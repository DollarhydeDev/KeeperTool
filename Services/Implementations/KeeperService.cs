using KeeperTool.Services.Interfaces;
using KeeperSecurity.Authentication;
using KeeperSecurity.Authentication.Sync;
using KeeperSecurity.Configuration;
using KeeperSecurity.Vault;

namespace KeeperTool.Services.Implementations
{
    public class KeeperService : IKeeperService
    {
        private readonly JsonConfigurationStorage _jsonConfigurationStorage;
        private readonly InMemoryConfigurationStorage _keeperStorage;
        private readonly VaultOnline _keeperVault;

        private KeeperService(JsonConfigurationStorage jsonConfigurationStorage, InMemoryConfigurationStorage keeperStorage, VaultOnline keeperVault)
        {
            _jsonConfigurationStorage = jsonConfigurationStorage;
            _keeperStorage = keeperStorage;
            _keeperVault = keeperVault;
        }

        public static async Task<KeeperService> CreateAsync(string username, string password)
        {
            var jsonConfigurationStorage = new JsonConfigurationStorage("config.json");
            var keeperStorage = new InMemoryConfigurationStorage();

            AuthSync auth = new AuthSync(keeperStorage) { AlternatePassword = true,  Username = username };

            try
            {
                await auth.Login(username, password);
                if (auth.IsAuthenticated())
                {
                    var keeperVault = new VaultOnline(auth);
                    return new KeeperService(jsonConfigurationStorage, keeperStorage, keeperVault);
                }

                throw new InvalidOperationException("Authentication failed. Please check your credentials.");
            }
            catch (Exception innerException)
            {
                throw new InvalidOperationException("An error occurred while creating the KeeperService instance.", innerException);
            }
        }

        public async Task<Models.Credentials.KeeperRecord> GetKeeperRecord(string keeperRecordId)
        {
            await _keeperVault.SyncDown();

            var record = _keeperVault.GetRecord(keeperRecordId);
            if (record == null) throw new KeyNotFoundException($"No record found with ID: {keeperRecordId}");

            var typedRecord = record as TypedRecord;
            if (typedRecord == null) throw new InvalidOperationException($"Record with ID {keeperRecordId} is not a TypedRecord.");

            return new Models.Credentials.KeeperRecord(typedRecord);
        }
    }
}