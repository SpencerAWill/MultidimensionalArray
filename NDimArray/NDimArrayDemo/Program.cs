using NDimArray;
using NDimArrayDemo.Demos;
using System;

namespace NDimArrayDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            var array = new NDimArray<string>(2, 2, 2);

            array[0, 0, 0] = "a";
            array[0, 0, 1] = "b";
            array[0, 1, 0] = "c";
            array[0, 1, 1] = "d";
            array[1, 0, 0] = "e";
            array[1, 0, 1] = "f";
            array[1, 1, 0] = "g";
            array[1, 1, 1] = "h";

            IDemo<string> planeEnum = new PlaneEnumerator<string>(array);

            planeEnum.Run((index, item) => Console.WriteLine($"[{string.Join(", ", index)}]: {item}"));

            Console.ReadLine();
        }
    }
}
