using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic
{
    // The following extension methods can be accessed by instances of any
    // class that implements IMyInterface.
    public static class Shared
    {
        public static string ReadAllTextFromFileAsBytes(byte[] embeddedFileAsBytes)
        {
            string text = string.Empty;
            using (Stream stream = new MemoryStream(embeddedFileAsBytes))
            using (StreamReader reader = new StreamReader(stream))
            {
                text = reader.ReadToEnd();
            }
            return text;
        }
    }
}
