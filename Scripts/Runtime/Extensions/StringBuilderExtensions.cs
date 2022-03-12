namespace System.Text
{
    public static class StringBuilderExtensions
    {
        public static StringBuilder AppendCode(this StringBuilder stringBuilder, string text) =>
            stringBuilder.Append(text).Append("\r\n");

        public static StringBuilder AppendCode(this StringBuilder stringBuilder) =>
            stringBuilder.Append("\r\n");
    }
}
