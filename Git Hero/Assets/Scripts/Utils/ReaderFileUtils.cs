using System;
using System.ComponentModel;
using System.IO;
using System.Linq;

namespace Githero.Ultils
{
    public class ReaderFile
    {
        public void ReadLine(string filePath, int skip, Action<string> getLineAction)
        {
            var backgroundWorker = new BackgroundWorker();

            backgroundWorker.DoWork += (sender, args) => {
                try
                {
                    var line = File.ReadLines(filePath).Skip(skip).Take(1).First();
                    args.Result = line;
                }
                catch
                {
                    args.Result = null;
                }
            };

            backgroundWorker.RunWorkerCompleted += (sender, args) =>
             {
                 getLineAction.Invoke((string)args.Result);
             };

            backgroundWorker.RunWorkerAsync();
        }

    }

}
