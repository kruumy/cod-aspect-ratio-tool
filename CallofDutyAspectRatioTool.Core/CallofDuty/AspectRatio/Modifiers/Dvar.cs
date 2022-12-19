using CallofDutyAspectRatioTool.Core.Utils;
using System;
using System.IO;

namespace CallofDutyAspectRatioTool.Core.CallofDuty.AspectRatio.Modifiers
{
    public sealed class Dvar : Base
    {
        private Config.Editor[] ConfigEditors { get; }

        public override Fraction AspectRatio
        {
            get
            {
                Fraction result = new Fraction();
                foreach (Config.Editor editor in ConfigEditors)
                {
                    string dvar = "r_customMode";
                    if (!editor.DoesDvarExist(dvar) || string.IsNullOrEmpty(editor.ReadDvar(dvar)))
                    {
                        editor.WriteDvar(dvar, "1920x1080");
                    }
                    string[] res = editor.ReadDvar(dvar).Split('x');
                    result.Numerator = Convert.ToInt32(res[0]);
                    result.Denominator = Convert.ToInt32(res[1]);
                }
                return result;
            }
            set
            {
                foreach (Config.Editor editor in ConfigEditors)
                {
                    editor.WriteDvar("r_customMode", value.ToString().Replace('/', 'x'));
                    editor.WriteDvar("r_fullscreen", "0");
                    editor.WriteDvar("r_aspectRatio", "custom");

                    if (GameName.Contains("iw3xo"))
                    {
                        editor.WriteDvar("r_aspectRatio_custom", value.Decimal.ToString());
                    }
                    else if (GameName.Contains("iw4"))
                    {
                        editor.WriteDvar("r_customAspectRatio", value.Decimal.ToString());
                    }
                }
            }
        }

        public Dvar(string pathToBinary) : base(pathToBinary)
        {
            string gameDir = Directory.GetParent(pathToBinary).FullName;
            string playerDir = Path.Combine(gameDir, "players");
            if (!Directory.Exists(playerDir))
                throw new DirectoryNotFoundException(playerDir);

            string[] configs = Directory.GetFiles(playerDir, "*.cfg", SearchOption.AllDirectories);
            ConfigEditors = new Config.Editor[configs.Length];
            for (int i = 0; i < configs.Length; i++)
            {
                ConfigEditors[i] = new Config.Editor(configs[i]);
            }
        }
    }
}