using SharpDX.XInput;
using System;
using System.Threading;

namespace X360Ctrl.Example
{
    internal static class Program
    {
        private static void Main()
        {
            var x360ctrl = new XInputController(UserIndex.One);
            if (!x360ctrl.IsConnected)
            {
                Console.WriteLine("Controller is not Connected or UserIndex is wrong");
                Console.ReadKey();
                return;
            }
            char keyChar = ' ';

            do
            {
                Console.Clear();
                Console.WriteLine("Press x to exit");
                if (Console.KeyAvailable)
                {
                    keyChar = Console.ReadKey().KeyChar;
                    if (keyChar == 'x')
                        break;
                }

                if (!x360ctrl.Update())
                {
                    Console.WriteLine("Controller update Failed");
                }

                Console.WriteLine(x360ctrl.ToString());
                Thread.Sleep(100);
            } while (keyChar != 'x');
        }
    }
}