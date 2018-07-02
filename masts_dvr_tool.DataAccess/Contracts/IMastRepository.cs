using masts_dvr_tool.Types;

namespace masts_dvr_tool.DataAccess.Contracts
{
    public interface IMastRepository
    {
        Mast GetMastData(string sharing, string UID, string mast = "0");
    }
}
