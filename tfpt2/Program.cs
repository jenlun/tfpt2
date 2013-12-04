namespace TFSPowerTools2
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    public class Program
    {
        public static void Main(string[] args)
        {
            if (args[0].Equals("annotate"))
            {
                // get cached change sets
                var changeSets = new ChangeSets();

                // get annotated original
                var annotateOutput = ExecTfpt(args);
                TransformAnnotation transform = null;
                try
                {
                    transform = new TransformAnnotation(changeSets, annotateOutput);
                }
                catch (KeyNotFoundException)
                {
                    var newArgs = (string[])args.Clone();
                    newArgs[0] = "history";
                    var list = new List<string>(newArgs) { "/itemmode" };

                    var historyOutput = ExecTf(list.ToArray());
                    changeSets.AddHistory(historyOutput);
                    transform = new TransformAnnotation(changeSets, annotateOutput);
                }

                Console.Out.Write(transform.GetTransformedAnnotation());
            }
            else
            {
                var output = ExecTfpt(args);
                Console.Out.Write(output);
            }

            Console.Out.Flush();
        }

        private static string ExecTf(string[] args)
        {
            return Exec("tf.exe", args);
        }

        private static string ExecTfpt(string[] args)
        {
            return Exec(@"C:\Program Files (x86)\Microsoft Team Foundation Server 2010 Power Tools\TFPT.exe", args);
        }

        private static string Exec(string filename, string[] args)
        {
            string output;
            using (var p = new Process { StartInfo = { UseShellExecute = false, RedirectStandardOutput = true, FileName = filename, Arguments = GetArgs(args) } })
            {
                p.Start();

                output = p.StandardOutput.ReadToEnd();
                p.WaitForExit();
            }

            return output;
        }

        private static string GetArgs(string[] args)
        {
            return string.Join(" ", args);
        }
    }
}
