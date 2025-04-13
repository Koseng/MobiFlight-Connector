using System;
using System.IO;
using System.Windows.Forms;

namespace MobiFlight.Scripts
{
    internal class ExternalFacade : IExternalFacade
    {
        public string GetEnvironmentVariable(string variable)
        {
            return Environment.GetEnvironmentVariable(variable);
        }

        public string[] GetFilesInSubDirectory(string subDirectory, string searchPattern, SearchOption searchOption)
        {
            return Directory.GetFiles(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, subDirectory), searchPattern, searchOption);                 
        }

        public DialogResult ShowMessageBox(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            return MessageBox.Show(text, caption, buttons, icon);
        }

        public string ReadTextFromFile(string path)
        {
            return File.ReadAllText(path);
        }

    }
}
