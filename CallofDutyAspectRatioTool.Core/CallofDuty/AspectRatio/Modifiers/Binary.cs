using CallofDutyAspectRatioTool.Core.Utils;
using System;
using System.IO;
using System.Linq;

namespace CallofDutyAspectRatioTool.Core.CallofDuty.AspectRatio.Modifiers
{
    public sealed class Binary : Base
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
                // TODO: temp fix i gotta get a better way
                byte[] gameBin = File.ReadAllBytes(GamePath);

                byte[] cod4mw2 = { 0xCD, 0xCC, 0xCC, 0x3F };
                byte[] bo2 = { 0x00, 0x50, 0x43, 0xC7 };
                byte[] mw3 = { 0xCD, 0xCC, 0xCC, 0x3F };
                byte[] mw3sp = { 0x00, 0x00, 0x88, 0x40 };
                byte[] bo1 = { 0xD2, 0x53, 0xFB, 0x40 };
                byte[] waw = { 0x2E, 0xBA, 0xE8, 0x3E };
                byte[] wawsp = { 0x00, 0xFF, 0x7F, 0x3F };

                if (GameName.Contains("t6"))
                    return gameBin.IndexOf(bo2, 1).FirstOrDefault() - 4;
                else if (GameName.Contains("iw5mp"))
                    return gameBin.IndexOf(mw3, 1).FirstOrDefault() + 4;
                else if (GameName.Contains("iw5sp"))
                    return gameBin.IndexOf(mw3sp, 1).FirstOrDefault() + 4;
                else if (GameName.Contains("BlackOps"))
                    return gameBin.IndexOf(bo1, 1).FirstOrDefault() + 4;
                else if (GameName.Contains("CoDWaWmp"))
                    return gameBin.IndexOf(waw, 1).FirstOrDefault() + 4;
                else if (GameName.Contains("CoDWaW"))
                    return gameBin.IndexOf(wawsp, 1).FirstOrDefault() - 4;
                else
                    return gameBin.IndexOf(cod4mw2, 1).FirstOrDefault() - 4;
            }
        }
        public Binary(string pathToBinary) : base(pathToBinary)
        {
        }
    }
}
