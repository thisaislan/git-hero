using System;
using System.IO;
using System.Linq;
using System.Threading;

namespace Githero.Ultils
{
    public class ReaderFileUtils
    {

        public void Read(string filePath, int skip, Action<string> getLineAction)
        {
            new Thread(delegate ()
            {
                try
                {
                    var line = File.ReadLines(filePath).Skip(skip).Take(1).First();
                    getLineAction.Invoke(line);
                }
                catch (InvalidOperationException)
                {
                    getLineAction.Invoke(null);
                }

            }).Start();
        }

    }

}
