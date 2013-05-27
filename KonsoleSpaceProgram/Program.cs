using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KonsoleSpaceProgram
{
    class Program
    {
        static void Main(string[] args)
        {
            bool crashed = false;

            double gravity = 10;

            double eccentricity = 0;
            double atmosphericDensity = 0;
            double atmosphericCeiling = 70000;
            double verticalSpeed = 0;
            double horizontalSpeed = 0;
            double altitude = 0;
            double verticalDrag = 0;
            double horizontalDrag = 0;
            double centrifugalForce = 0;

            bool running = true;
            new Thread(() =>
            {
                while (running)
                {
                    if (altitude < 0)
                    {
                        if (verticalSpeed < -10 || horizontalSpeed < -10 || horizontalSpeed > 10)
                        {
                            crashed = true;
                        }
                    }

                    gravity = 3.53 / Math.Pow(0.6 + altitude / 100000, 2);
                    atmosphericDensity = Math.Log(Math.Pow(atmosphericCeiling, 2) / Math.Pow(altitude, 2));
                    if (atmosphericDensity < 0)
                    {
                        atmosphericDensity = 0;
                    }

                    verticalDrag = (0.5 * atmosphericDensity * Math.Pow(verticalSpeed, 2) * 1.2) / 10000;
                    horizontalDrag = (0.5 * atmosphericDensity * Math.Pow(Clamp(horizontalSpeed), 2) * 1.2) / 10000;
                    centrifugalForce = Clamp(horizontalSpeed) / Math.Sqrt(altitude);
                    eccentricity = Math.Cos(centrifugalForce / gravity);

                    if (altitude > 0)
                    {
                        if (crashed)
                        {
                            crashed = false;
                        }

                        verticalSpeed -= (gravity + verticalDrag - centrifugalForce) * 0.1;

                        if (horizontalSpeed > 0)
                        {
                            horizontalSpeed -= horizontalDrag + (eccentricity * (centrifugalForce - gravity)) * 0.1;
                        }

                        if (horizontalSpeed < 0)
                        {
                            horizontalSpeed += horizontalDrag + (eccentricity * (centrifugalForce - gravity)) * 0.1;
                        }
                    }

                    if (altitude < 0)
                    {
                        verticalSpeed = 0;
                    }

                    if (altitude != 0)
                    {
                        altitude--;

                        if (altitude < 0)
                        {
                            altitude = 0;
                        }
                    }

                    altitude += verticalSpeed * 0.1;

                    Console.Clear();
                    if (crashed)
                    {
                        Console.WriteLine("YOU HAVE CRASH LANDED!");
                        Console.WriteLine();
                    }
                    Console.WriteLine("Altititude: " + altitude.ToString("0.00") + " Metres");
                    Console.WriteLine("Vertical Speed: " + verticalSpeed.ToString("0.00") + " m/s");
                    Console.WriteLine("Horizontal Speed: " + horizontalSpeed.ToString("0.00") + " m/s");
                    Console.WriteLine("Vertical Drag: " + verticalDrag.ToString("0.00") + " m/s2");
                    Console.WriteLine("Horizontal Drag: " + horizontalDrag.ToString("0.00") + " m/s2");
                    Console.WriteLine("Atsmospheric Density: " + atmosphericDensity.ToString("0.00"));
                    Console.WriteLine("Gravity: " + gravity.ToString("0.00") + " m/s2");
                    Console.WriteLine("Centrifugal Force: " + centrifugalForce.ToString("0.00") + " m/s2");
                    Console.WriteLine("Eccentricity: " + eccentricity.ToString("0.00"));
                    Console.WriteLine("");
                    Console.WriteLine("Tap arrow keys to fly...  Escape to exit!");
                    Thread.Sleep(100);
                }
            }).Start();

            ConsoleKeyInfo keyInfo;

            do
            {
                keyInfo = Console.ReadKey();

                if (keyInfo.Key == ConsoleKey.UpArrow)
                {
                    verticalSpeed++;
                }

                if (keyInfo.Key == ConsoleKey.DownArrow)
                {
                    verticalSpeed--;
                }

                if (keyInfo.Key == ConsoleKey.LeftArrow)
                {
                    horizontalSpeed--;
                }

                if (keyInfo.Key == ConsoleKey.RightArrow)
                {
                    horizontalSpeed++;
                }

            } while (keyInfo.Key != ConsoleKey.Escape);

            running = false;
        }

        static double Clamp(double number)
        {
            if (number < 0)
            {
                number = -number;
            }

            return number;
        }
    }
}