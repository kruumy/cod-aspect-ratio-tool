using CallofDutyAspectRatioTool.Core.Utils;
using System;
using System.Collections.Generic;
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



                byte[] iwEngine = { 0xCD, 0xCC, 0xCC, 0x3F };
                byte[] bo2 = { 0x00, 0x50, 0x43, 0xC7 };

                if (GameName.Contains("t6"))
                {
                    return gameBin.IndexOf(bo2, 1).FirstOrDefault() - 4;
                }
                else
                {
                    return gameBin.IndexOf(iwEngine, 1).FirstOrDefault() - 4;
                }
            }
        }
        public BinaryModifier(string pathToBinary) : base(pathToBinary)
        {
        }
    }
}
