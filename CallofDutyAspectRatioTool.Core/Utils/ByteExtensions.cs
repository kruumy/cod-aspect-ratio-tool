using System.Collections.Generic;

namespace CallofDutyAspectRatioTool.Core.Utils
{
    internal static class ByteExtensions
    {
        internal static uint[] IndexOf(this byte[] bytes, byte[] searchBytes, uint maxResults = uint.MaxValue, uint start = 0, uint? end = null, bool nullAsBlank = false)
        {
            List<uint> result = new List<uint>();
            if (end == null)
                end = (uint)(bytes.Length - 2);
            for (uint i = start; i < end; i++)
            {
                if (bytes[i] != searchBytes[0])
                    continue;
                bool isFullPattern = true;
                for (uint j = 0; j < searchBytes.Length; j++)
                {
                    if (nullAsBlank && searchBytes[j] == 0x0)
                        continue;
                    if (searchBytes[j] != bytes[i + j])
                    {
                        isFullPattern = false;
                        break;
                    }
                }
                if (isFullPattern)
                {
                    result.Add(i);
                    if (result.Count >= maxResults)
                        break;
                }
            }
            return result.ToArray();
        }
    }
}