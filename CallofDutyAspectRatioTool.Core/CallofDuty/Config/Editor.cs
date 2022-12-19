using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CallofDutyAspectRatioTool.Core.CallofDuty.Config
{
    internal class Editor
    {
        public string ConfigPath { get; }
        public string[] RawLines { get; private set; }

        public Editor(string pathToConfig)
        {
            if (!File.Exists(pathToConfig))
                throw new FileNotFoundException(nameof(pathToConfig));

            ConfigPath = pathToConfig;
            RawLines = File.ReadAllLines(pathToConfig);
        }
        public void WriteDvar(string dvar, string value)
        {
            string fullLine = $"seta {dvar} \"{value}\"";

            bool alreadyExists = false;
            for (int i = 0; i < RawLines.Length; i++)
            {
                if (RawLines[i].Contains(dvar))
                {
                    RawLines[i] = fullLine;
                    alreadyExists = true;
                }
            }
            if (!alreadyExists)
            {
                RawLines = RawLines.Concat(new string[] { fullLine }).ToArray();
            }
            Save();
        }
        public string ReadDvar(string dvar)
        {
            for (int i = 0; i < RawLines.Length; i++)
            {
                if (RawLines[i].Contains(dvar))
                {
                    int qi = RawLines[i].IndexOf('"') + 1;
                    int lqi = RawLines[i].LastIndexOf('"');
                    return RawLines[i].Substring(qi, lqi - qi);
                }
            }
            throw new KeyNotFoundException();
        }
        public bool DoesDvarExist(string dvar)
        {
            for (int i = 0; i < RawLines.Length; i++)
            {
                if (RawLines[i].Contains(dvar))
                {
                    return true;
                }
            }
            return false;
        }
        private void Save()
        {
            File.WriteAllLines(ConfigPath, RawLines);
        }
    }
}
