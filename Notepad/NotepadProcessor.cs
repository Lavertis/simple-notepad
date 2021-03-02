using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notepad
{
    internal static class NotepadProcessor
    {
        internal static string ReturnMessageFromFormat(string type)
        {
            switch (type)
            {
                case "ino":
                    return "Arduino";
                case "cs":
                    return "C#";
                case "cpp":
                    return "C++";
                case "c":
                    return "C";
                case "btwo":
                    return "Braintwo";
                case "json":
                    return "Json";
                case "xml":
                    return "Xml";
                case "html":
                    return "HTML";
                case "css":
                    return "CSS";
                case "js":
                    return "JavaScript";
                default:
                    return "Text";

            }
        }
    }
}
