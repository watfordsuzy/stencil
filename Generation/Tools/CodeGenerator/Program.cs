using System;
using System.IO;
using System.Windows.Forms;

namespace CodeGenerator
{
    public static class Program
    {
        public static TextWriter StandardOutput { get; set; } = Console.Out;

        [STAThread]
        private static int Main(string[] args)
        {
            if (args?.Length > 0)
            {
                return RunConsole(args);
            }
            else
            {
                return RunApp();
            }
        }

        private static int RunApp()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(defaultValue: false);
            Application.Run(new PrimaryForm());

            return Environment.ExitCode;
        }

        private static int RunConsole(string[] args)
        {
            Environment.ExitCode = 1;
            try
            {
                var translator = new Translator();
                var controller = new CodeGeneratorController(translator);

                translator.Notice += (sender, e) => StandardOutput.WriteLine($"[INFO] {e.Message}");
                translator.Error += (sender, e) => StandardOutput.WriteLine($"[ERROR] {e.Message}");
                translator.Progress += (sender, e) => StandardOutput.WriteLine($"[INFO] {e.Progress:0.0} % complete."); ;

                StandardOutput.WriteLine($"[INFO] Reading options from {args[0]}");
                controller.LoadOptions(args[0]);

                StandardOutput.WriteLine($"[INFO] Generating files");
                controller.GenerateFiles();

                Environment.ExitCode = 0;
            }
            catch (Exception ex)
            {
                StandardOutput.WriteLine($"[ERROR] {ex}");
            }

            return Environment.ExitCode;
        }
    }
}
