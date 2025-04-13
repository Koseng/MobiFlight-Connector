using System.IO;
using System.Windows.Forms;

namespace MobiFlight.Scripts
{
    internal interface IExternalFacade
    {
        DialogResult ShowMessageBox(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon);

        string GetEnvironmentVariable(string variable);

        string[] GetFilesInSubDirectory(string subDirectory, string searchPattern, SearchOption searchOption);

        string ReadTextFromFile(string path);        
    }
}
