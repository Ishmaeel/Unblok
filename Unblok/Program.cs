using System;
using System.IO;
using System.Windows.Forms;
using Trinet.Core.IO.Ntfs;

namespace Unblok
{
    internal static class Program
    {
        [STAThread]
        private static void Main(params string[] args)
        {
            try
            {
                if (args.Length == 0)
                {
                    ShowUsage();
                }
                else
                {
                    UnblokAll(args);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Oops.", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static void ShowUsage()
        {
            MessageBox.Show("Gimme a file or folder parameter to remove zone identifier data streams.", "Hello.", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private static void UnblokAll(string[] args)
        {
            foreach (var oneArg in args)
            {
                if (File.Exists(oneArg))
                {
                    UnblokFile(oneArg, true);
                }

                if (Directory.Exists(oneArg))
                {
                    UnblokDirectory(oneArg);
                }
            }
        }

        private static void UnblokDirectory(string oneArg)
        {
            var dirInfo = new DirectoryInfo(oneArg);
            var allFiles = dirInfo.GetFiles();

            foreach (var oneFile in allFiles)
            {
                UnblokFile(oneFile, false);
            }
        }

        private static void UnblokFile(string oneArg, bool siblingsAlso)
        {
            var fileInfo = new FileInfo(oneArg);

            UnblokFile(fileInfo, siblingsAlso);
        }

        private static void UnblokFile(FileInfo fileInfo, bool siblingsAlso)
        {
            if (siblingsAlso)
            {
                var directory = Path.GetDirectoryName(fileInfo.FullName);
                UnblokDirectory(directory);
            }
            else
            {
                var s = fileInfo.DeleteAlternateDataStream("Zone.Identifier");
            }
        }
    }
}