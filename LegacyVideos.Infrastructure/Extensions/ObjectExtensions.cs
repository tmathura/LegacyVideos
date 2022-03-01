namespace LegacyVideos.Infrastructure.Extensions
{
    public static class ObjectExtensions
    {
        /// <summary>
        /// Convert object to DBNull.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static object ToSqlNull(this object input)
        {
            if (input == null)
            {
                return DBNull.Value;
            }

            if (input is string)
            {
                if (string.IsNullOrWhiteSpace(input as string))
                {
                    return DBNull.Value;
                }

                return input;
            }

            return input;
        }
    }
}