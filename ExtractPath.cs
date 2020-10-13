namespace call_and_set
{
    class ExtractPath
    {
        public ExtractPath(string input)
        {
            var parts = input.Split('~');

            Path = parts[0];
            EnvVar = parts[1];
        }

        /// <summary>
        /// The path use to extract the value from the command output.
        /// </summary>
        public string Path { get; private set; }

        /// <summary>
        /// The name of the environment variable to set with the value extracted from path.
        /// </summary>
        public string EnvVar { get; private set; }
    }
}
