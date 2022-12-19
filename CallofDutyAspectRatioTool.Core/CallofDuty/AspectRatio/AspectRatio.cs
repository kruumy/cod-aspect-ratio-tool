using System;
using System.IO;

namespace CallofDutyAspectRatioTool.Core.CallofDuty.AspectRatio
{
    public sealed class AspectRatio
    {
        public string GamePath { get; }
        public string Name { get; }
        public Modifiers.Types ModifierType { get; }
        public Modifiers.Base Modifier { get; }

        public AspectRatio(string pathToBinary)
        {
            if (pathToBinary == null)
                throw new ArgumentNullException(nameof(pathToBinary));
            if (!File.Exists(pathToBinary))
                throw new FileNotFoundException(nameof(pathToBinary));

            GamePath = pathToBinary;
            Name = Path.GetFileName(GamePath);

            Modifiers.Types? arType = Modifiers.ModifierTypesList.GetGameType(GamePath);
            if (arType != null)
                ModifierType = (Modifiers.Types)arType;
            else
                throw new ArgumentException(nameof(GamePath));

            switch (ModifierType)
            {
                case Modifiers.Types.Binary:
                    {
                        Modifier = new Modifiers.Binary(GamePath);
                        break;
                    }
                case Modifiers.Types.Dvar:
                    {
                        Modifier = new Modifiers.Dvar(GamePath);
                        break;
                    }
            }
        }
    }
}
