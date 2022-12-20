namespace CallofDutyAspectRatioTool.Core.Utils
{
    public static class FloatExtensions
    {
        public static bool IsRepeating(this float f)
        {
            string fstring = (f).ToString();
            if (fstring.Length <= 4)
                return false;
            int v1 = int.Parse(fstring[fstring.Length - 1].ToString());
            int v2 = int.Parse(fstring[fstring.Length - 2].ToString());
            int v3 = int.Parse(fstring[fstring.Length - 3].ToString());
            return v2 == v3 && v1 >= v2;
        }

        public static double RepeatingFloatToDouble(this float f)
        {
            char[] fstring = f.ToString().ToCharArray();
            char lastNum = fstring[fstring.Length - 2];
            fstring[fstring.Length - 1] = lastNum;
            string fstrings = new string(fstring);
            for (int i = 0; i < 16; i++)
                fstrings += lastNum;
            return double.Parse(fstrings);
        }

        public static double ToDouble(this float f)
        {
            if (f.IsRepeating())
                return f.RepeatingFloatToDouble();
            else
                return ((double)f);
        }
    }
}