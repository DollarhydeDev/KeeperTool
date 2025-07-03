using KeeperTool.Services.Implementations;
using System.Threading.Tasks;

namespace KeeperTool
{
    internal class Program
    {
        static async Task Main()
        {
            // Login with master password
            string username = "YOUR_USERNAME";
            string password = "YOUR_PASSWORD";
            var keeperService = await KeeperService.CreateAsync(username, password);

            // Get a keeper record using it's UID
            var exampleRecordId = "YOUR_KEEPER_RECORD_ID";
            var exampleKeeperRecord = await keeperService.GetKeeperRecord(exampleRecordId);

            // Access the fields of the keeper record
            var usernameField = exampleKeeperRecord.Username;
            var passwordField = exampleKeeperRecord.Password;
            var customField = exampleKeeperRecord.GetCustomField("custom_field_name");
        }
    }
}
