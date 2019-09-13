using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MoreDescriptiveGreeting
{
    partial class Program
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr GetStdHandle(int nStdHandle);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        static extern bool GetCurrentConsoleFontEx(
               IntPtr consoleOutput,
               bool maximumWindow,
               ref CONSOLE_FONT_INFO_EX lpConsoleCurrentFontEx);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool SetCurrentConsoleFontEx(
               IntPtr consoleOutput,
               bool maximumWindow,
               CONSOLE_FONT_INFO_EX consoleCurrentFontEx);

        private const int STD_OUTPUT_HANDLE = -11;
        private const int TMPF_TRUETYPE = 4;
        private const int LF_FACESIZE = 32;
        private static IntPtr INVALID_HANDLE_VALUE = new IntPtr(-1);
        private const string EXIT_KEYWORD = "Exit";
        static unsafe void Main(string[] args)
        {
            string fontName = "Lucida Console";
            IntPtr hnd = GetStdHandle(STD_OUTPUT_HANDLE);
            if (hnd != INVALID_HANDLE_VALUE)
            {
                CONSOLE_FONT_INFO_EX info = new CONSOLE_FONT_INFO_EX();
                info.cbSize = (uint)Marshal.SizeOf(info);
                // Set console font to Lucida Console.
                CONSOLE_FONT_INFO_EX newInfo = new CONSOLE_FONT_INFO_EX();
                newInfo.cbSize = (uint)Marshal.SizeOf(newInfo);
                newInfo.FontFamily = TMPF_TRUETYPE;
                IntPtr ptr = new IntPtr(newInfo.FaceName);
                Marshal.Copy(fontName.ToCharArray(), 0, ptr, fontName.Length);
                // Get some settings from current font.
                newInfo.dwFontSize = new COORD(info.dwFontSize.X, info.dwFontSize.Y);
                newInfo.FontWeight = info.FontWeight;
                SetCurrentConsoleFontEx(hnd, false, newInfo);
            }
            string strName;
            do
            {
                Console.WriteLine("Please enter your name to have a more descriptive greeting than the greeting that Mr. Letts's program provides (Enter \"" + EXIT_KEYWORD + "\" without the quotation marks to exit out of the program):");
                strName = Console.ReadLine();
                Console.WriteLine(GenerateMoreDescriptiveGreeting(strName));
            } while (strName != EXIT_KEYWORD);
            Console.WriteLine("See you later, random person!");
            Console.ReadKey();
        }

        private static string GenerateMoreDescriptiveGreeting(string strGreetingName)
        {
            string strMoreDescriptiveGreeting = "";

            if (strGreetingName != EXIT_KEYWORD)
            {
                Random rndRandomGenerator = new Random();
                int intRandomNum = rndRandomGenerator.Next(1, 101);
                if (intRandomNum % 2 == 0)
                    strMoreDescriptiveGreeting = "Welcome user " + strGreetingName + ". This is your special, unique, more-descriptive greeting. I am generating this truly descriptive greeting because I feel like Mr. Letts wasn't that descriptive in his Greeting program, and being descrptive is the most important thing in programming, right? \n";
                else
                    strMoreDescriptiveGreeting = "Are you really sure that your name is " + strGreetingName + ", or are you just trying to test the program out to argue that it \"isn't really that descriptive?\" Because I'm 100% sure that this program is gonna generate the most descriptive greeting that you will ever see in your entire life. Don't believe me? Just try it and see what will happen. \n";
            }
            return strMoreDescriptiveGreeting;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct COORD
        {
            internal short X;
            internal short Y;

            internal COORD(short x, short y)
            {
                X = x;
                Y = y;
            }
        }
        [StructLayout(LayoutKind.Sequential)]
        internal unsafe struct CONSOLE_FONT_INFO_EX
        {
            internal uint cbSize;
            internal uint nFont;
            internal COORD dwFontSize;
            internal int FontFamily;
            internal int FontWeight;
            internal fixed char FaceName[LF_FACESIZE];
        }
    }
}
