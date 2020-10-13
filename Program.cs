using CommandLine;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace call_and_set
{
    class Program
    {
        static void Main(string[] args)
        {
            var extractPaths = new List<ExtractPath>();
            var commandArgs = new List<string>();

            for (var i = 0; i < args.Length; i++)
            {
                // Parse an ExtractPath.
                if (args[i].Equals("--exp", StringComparison.OrdinalIgnoreCase))
                {
                    extractPaths.Add(new ExtractPath(args[i + 1]));
                    i++;
                }
                else
                {
                    commandArgs.Add(args[i]);
                }
            }

            // Call process and capture output.
            var output = CallCommand(commandArgs);

            // Extract values from output and set environment variables.
            ExtractAndSet(output, extractPaths);
        }

        static string CallCommand(List<string> args)
        {
            var appName = args[0];
            var appArgs = string.Join(" ", args.Skip(1));

            var startInfo = new ProcessStartInfo(appName, appArgs)
            {
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true
            };

            using var proc = new Process
            {
                StartInfo = startInfo
            };
            proc.Start();

            using var stdout = proc.StandardOutput;
            return stdout.ReadToEnd();
        }

        static void ExtractAndSet(string output, List<ExtractPath> exps)
        {
            var jo = JObject.Parse(output);

            foreach (var exp in exps)
            {
                var value = jo.SelectToken(exp.Path);
                Environment.SetEnvironmentVariable(exp.EnvVar, value.ToString());

                Console.WriteLine($"Set env {exp.EnvVar}.");
            }
        }
    }
}
