using System;
using System.IO;

namespace CodeGenerator
{
    public class CodeGeneratorController
    {
        private readonly Translator _translator;

        public CodeGeneratorController(Translator translator)
        {
            _translator = translator ?? throw new ArgumentNullException(nameof(translator));
        }

        public void LoadOptions(string path)
        {
            Options options;

            try
            {
                options = Utility.DeserializeFromXml<Options>(new FileInfo(path));
            }
            catch (FileNotFoundException)
            {
                return;
            }

            if (options == null)
            {
                return;
            }

            _translator.DataFile = options.DataFile;
            _translator.OutputFolder = options.OutputFolder;

            string baseDirectory = Path.GetDirectoryName(path);
            foreach (string selectedFile in options.SelectedFiles)
            {
                _translator.Templates.Add(
                    new Template(
                        selectedFile, 
                        GetRootedPath(baseDirectory, selectedFile), 
                        isSelected: true));
            }

            foreach (string unSelectedFile in options.UnSelectedFiles)
            {
                _translator.Templates.Add(
                    new Template(
                        unSelectedFile,
                        GetRootedPath(baseDirectory, unSelectedFile),
                        isSelected: false));
            }
        }

        private static string GetRootedPath(string baseDirectory, string path)
        {
            string rootedPath = path;
            if (!Path.IsPathRooted(rootedPath))
            {
                rootedPath = Path.Combine(baseDirectory, rootedPath);
            }

            return Path.GetFullPath(rootedPath);
        }

        public void GenerateFiles()
            => _translator.GenFiles();
    }
}
