using CallofDutyAspectRatioTool.Core.CallofDuty.AspectRatio;
using System;
using System.IO;

namespace CallofDutyAspectRatioTool.Core.CallofDuty
{
    public sealed class Game
    {
        public string GamePath { get; }
        public string Name => Path.GetFileName(GamePath);
        public ModifierTypes AspectRatioModifierType { get; }
        public ModifierBase AspectRatioModifier { get; }

        public Game(string pathToBinary)
        {
            if (pathToBinary == null)
                throw new ArgumentNullException(nameof(pathToBinary));
            if (!File.Exists(pathToBinary))
                throw new FileNotFoundException(nameof(pathToBinary));

            GamePath = pathToBinary;

            ModifierTypes? arType = ModifierTypesList.GetGameType(GamePath);
            if (arType != null)
                 AspectRatioModifierType = (ModifierTypes)arType;
            else
                throw new ArgumentException(nameof(GamePath));

            switch (AspectRatioModifierType)
            {
                case ModifierTypes.Binary:
                    {
                        AspectRatioModifier = new BinaryModifier(GamePath);
                        break;
                    }
                case ModifierTypes.Dvar:
                    {
                        AspectRatioModifier = new DvarModifier(GamePath);
                        break;
                    }
            }
        }
    }
}
