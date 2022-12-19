using CallofDutyAspectRatioTool.Core.Utils;
using System.IO;

namespace CallofDutyAspectRatioTool.Core.CallofDuty.AspectRatio.Modifiers
{
    public abstract class Base
    {
        internal string GamePath { get; }
        public string GameName => Path.GetFileName(GamePath);
        public abstract Fraction AspectRatio { get; set; }
        public Base(string pathToBinary)
        {
            if (File.Exists(pathToBinary))
            {
                GamePath = pathToBinary;
            }
            else
                throw new FileNotFoundException(nameof(pathToBinary));
        }
    }
}
