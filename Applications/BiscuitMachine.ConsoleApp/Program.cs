using AppBrix;
using AppBrix.Configuration.Memory;
using BiscuitMachine.ConsoleApp;
using BiscuitMachine.Controller;
using System;

var app = App.Start<MainModule>(new MemoryConfigService());
var controller = app.GetController();

Console.WriteLine("Welcome to The Biscuit Machine. Operation instructions:");
Console.WriteLine("You can turn the machine O(n), Of(f), (P)ause, or (Q)uit.");

var isDone = false;
while (!isDone)
{
    var key = Console.ReadKey(true);
    switch (key.Key)
    {
        case ConsoleKey.N:
            Console.WriteLine("Turning on the machine.");
            controller.TurnOn();
            break;
        case ConsoleKey.F:
            Console.WriteLine("Turning off the machine.");
            controller.TurnOff();
            break;
        case ConsoleKey.P:
            Console.WriteLine("Pausing machine operation.");
            controller.Pause();
            break;
        case ConsoleKey.Q:
            Console.WriteLine("Exiting application.");
            isDone = true;
            break;
    }
}

app.Stop();
