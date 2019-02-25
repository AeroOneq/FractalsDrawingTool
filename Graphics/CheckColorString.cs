namespace Graphics
{
    public static class CheckColorString
    {
        private static string AllowedSymbols { get; set; } = "#1234567890ABCDEF";
        public static bool Check(string input)
        {
            if (input.Length != 7)
            {
                return false;
            }
            for (int i = 0; i < input.Length; i++)
            {
                if (AllowedSymbols.IndexOf(input[i]) < 0)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
