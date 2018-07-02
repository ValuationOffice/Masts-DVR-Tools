using masts_dvr_tool.DataAccess.Contracts;
using masts_dvr_tool.Models;
using masts_dvr_tool.Services.Contract;
using masts_dvr_tool.Types;

namespace masts_dvr_tool.Services.Implementation
{
    public class DataManager : IDataManager
    {
        private readonly IMastRepository mastRepository;

        public DataManager(string[] args, IMastRepository mastRepository)
        {
            this.mastRepository = mastRepository;

            //If default entry point contains no arguements
            if (args == null || args.Length == 0)
                args = new string[1] { "No value" };

            switch (args[0].ToUpper())
            {
                case "MASTS":
                    DatabaseValues = mastRepository.GetMastData(args[1], args[3]);
                    MainWindow = new MainWindow<IVOAType>
                    {
                        DataType = DatabaseValues,
                        ExternalFilePath = args[4]
                    };
                    break;

                default:
                    MainWindow = new MainWindow<IVOAType>();
                    break;
            }

        }

        public IVOAType DatabaseValues { get; set; }

        public MainWindow<IVOAType> MainWindow { get; set; }
    }
}
