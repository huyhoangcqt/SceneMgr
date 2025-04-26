namespace Snowwolf
{
    /// <summary>
    /// Marks in use in excel work sheet.
    /// </summary>
    public static class ExcelReadMarks
    {
        /// <summary>
        /// This mark indicate the start of valid data row.
        /// </summary>
        public const string payloadStartMark = "#------>";

        /// <summary>
        /// This mark indicate the end of valid data row.
        /// </summary>
        public const string payloadStopMark = "#<------";

        /// <summary>
        /// This mark indicate the specify row or column to be comment.
        /// </summary>
        public const string commentMark = "//";

        /// <summary>
        /// This mark indicate the column only works on client.
        /// </summary>
        public const string headerClientOnly = "[ClientOnly]";

        /// <summary>
        /// This mark indicate the column only works on server.
        /// </summary>
        public const string headerServerOnly = "[ServerOnly]";
    }
}
