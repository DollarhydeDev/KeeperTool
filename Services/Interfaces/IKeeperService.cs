using KeeperTool.Models.Credentials;

namespace KeeperTool.Services.Interfaces
{
    public interface IKeeperService
    {
        Task<KeeperRecord> GetKeeperRecord(string keeperRecordId);
    }
}
