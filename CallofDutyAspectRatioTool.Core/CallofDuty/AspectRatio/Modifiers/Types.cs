using System.Diagnostics;
using System.IO;

namespace CallofDutyAspectRatioTool.Core.CallofDuty.AspectRatio.Modifiers
{
    public enum Types
    {
        Binary,
        Dvar
    }

    public static class ModifierTypesList
    {
        public static string[] Dvar =
        {
            "iw3xo",
            "iw4x",
        };

        public static string[] Binary =
        {
            "t6mpv43",
            "t6zmv41",
            "t6zm",
            "t6mp",
            "iw5mp",
            "iw5sp",
            "iw3mp",
            "iw3sp",
            "iw4mp",
            "BlackOpsMP",
            "BlackOps",
            "CoDWaW",
            "CoDWaWmp",
            "iw4m"
        };

        internal static Types? GetGameType(string gamePath)
        {
            string gameName = Path.GetFileName(gamePath);
            // this check if here because people often rename their iw4x to iw4m for codmvm
            if (gameName.Contains("iw4m"))
            {
                if (FileVersionInfo.GetVersionInfo(gamePath).OriginalFilename == "iw4x.exe")
                {
                    return Types.Dvar;
                }
            }
            foreach (string key in Dvar)
            {
                if (gameName.Contains(key))
                {
                    return Types.Dvar;
                }
            }
            foreach (string key in Binary)
            {
                if (gameName.Contains(key))
                {
                    return Types.Binary;
                }
            }
            return null;
        }
    }
}