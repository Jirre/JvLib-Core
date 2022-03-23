namespace System.Text
{
    public static class StringBuilderExtensions
    {
        public static StringBuilder AppendCode(this StringBuilder stringBuilder, int pIndent, string text)
        {
            for (int i = 0; i < pIndent; i++)
            {
                stringBuilder.Append("    ");
            }
            return stringBuilder.Append(text).Append("\r\n");
        }
        
        public static StringBuilder AppendCode(this StringBuilder stringBuilder, string text) =>
            stringBuilder.Append(text).Append("\r\n");

        public static StringBuilder AppendCode(this StringBuilder stringBuilder, int pIndent)
        {
            for (int i = 0; i < pIndent; i++)
            {
                stringBuilder.Append("    ");
            }
            return stringBuilder.Append("\r\n");
        }
        
        public static StringBuilder AppendCode(this StringBuilder stringBuilder) =>
            stringBuilder.Append("\r\n");
    }
}
