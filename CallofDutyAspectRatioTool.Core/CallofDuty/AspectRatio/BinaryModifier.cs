using CallofDutyAspectRatioTool.Core.Utils;
using System;
using System.IO;
using System.Linq;

namespace CallofDutyAspectRatioTool.Core.CallofDuty.AspectRatio
{
    public sealed class BinaryModifier : ModifierBase
    {
        public override Fraction AspectRatio
        {
            get
            {
                byte[] gameBin = File.ReadAllBytes(GamePath);
                float result = BitConverter.ToSingle(gameBin.Skip((int)AspectRatioIndexInBinary).Take(4).ToArray(), 0);
                return new Fraction()
                {
                    Decimal = result
                };
            }
            set
            {
                byte[] gameBin = File.ReadAllBytes(GamePath);
                byte[] toWrite = BitConverter.GetBytes(value.Decimal);
                toWrite.CopyTo(gameBin, AspectRatioIndexInBinary);
                File.WriteAllBytes(GamePath, gameBin);
            }
        }
        private uint AspectRatioIndexInBinary
        {
            get
            {
                byte[] gameBin = File.ReadAllBytes(GamePath);
                byte[] data = { 0xCD, 0xCC, 0xCC, 0x3F };
                return gameBin.IndexOf(data, 1).FirstOrDefault() - 4;
            }
        }
        public BinaryModifier(string pathToBinary) : base(pathToBinary)
        {
        }
    }
}
