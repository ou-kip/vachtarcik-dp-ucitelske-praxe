namespace Core.Database.Utils
{
    /// <summary>
    /// The class for executing and providing SQL in the migrations
    /// </summary>
    public static class SqlExecuter
    {
        /// <summary>
        /// Gets the SQL from the sql file
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GetSql(string fileName)
        {
            var basePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Database/Sql");
            var filePath = Path.Combine(basePath, fileName + ".sql");

            return File.ReadAllText(filePath);
        }
    }
}
