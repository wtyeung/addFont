using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;



namespace addFont
{

    class Program
    {
        [DllImport("gdi32.dll", EntryPoint = "AddFontResourceW", SetLastError = true)]
        public static extern int AddFontResource([In][MarshalAs(UnmanagedType.LPWStr)]string lpFileName);
        //public static extern int AddFontResource(string lpFileName);

        [DllImport("gdi32.dll", EntryPoint = "RemoveFontResourceW", SetLastError = true)]
        public static extern int RemoveFontResource([In][MarshalAs(UnmanagedType.LPWStr)]string lpFileName);

        static void Main(string[] args)
        {
            int result = -1;

            // Try install the font.
            if (args.Length != 1)
            {
                Console.WriteLine("Usage: addFont <fontFile>");
                Console.WriteLine("Usage: for font contains 2 files (pfm, pfb), place both file in same directory and specify either one");
                Console.WriteLine("addFont for Current Session, By Tim Yeung (1-APR-2019)");
                Console.WriteLine(" based on https://brutaldev.com/post/installing-and-removing-fonts-using-c ");
                Environment.Exit(-1);
            }
            else
            {

                string fontFile = args[0];
                string fontFileFullPath = Path.Combine(Environment.CurrentDirectory, fontFile);

                // check for font that has two parts, pfb and pfm, user can specify either one. 
                string ext = Path.GetExtension(fontFileFullPath).ToLower();
                string fontFileFullPathPart2 = "";
                if (ext == ".pfb") fontFileFullPathPart2 = Path.ChangeExtension(fontFileFullPath, "pfm");
                if (ext == ".pfm") fontFileFullPathPart2 = Path.ChangeExtension(fontFileFullPath, "pfb");
                if (File.Exists(fontFileFullPath))
                {
                    if (fontFileFullPathPart2.Length > 0)
                    {
                        if (File.Exists(fontFileFullPathPart2))
                        {
                            Console.WriteLine("addFontResource to current session two 2 component " + fontFileFullPath + "|" + fontFileFullPathPart2);
                            result = AddFontResource(fontFileFullPath + "|" + fontFileFullPathPart2);
                        }
                        else
                        {
                            Console.WriteLine("2nd part of font file not found: " + fontFileFullPathPart2);
                            Environment.Exit(-1);
                        }
                    }
                    else // font type with single file only
                    {
                        Console.WriteLine("addFontResource to current session " + fontFileFullPath);
                        result = AddFontResource(fontFileFullPath);

                    }
                }
                else
                {
                    Console.WriteLine("Font File not found");
                    Environment.Exit(-1);
                }

            }
            Console.WriteLine((result == 0) ? "Failed to install Font" : "Font installed OK");
        }
    }
}
