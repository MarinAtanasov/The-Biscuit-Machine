using AppBrix;
using AppBrix.Configuration.Memory;
using BiscuitMachine.ConsoleApp;
using BiscuitMachine.Controller;
using BiscuitMachine.Controller.Contracts;
using System;

var app = App.Start<MainModule>(new MemoryConfigService());
var controller = app.GetController();
var conveyor = app.GetConveyor();

Console.WriteLine("Welcome to The Biscuit Machine. Operation instructions:");
Console.WriteLine("You can turn the machine O(n), Of(f), (P)ause, (C)lear or (Q)uit.");
const string dirtyConveyorMessage = "The conveyor is not empty. Please clear the conveyor.";

var isDone = false;
while (!isDone)
{
    var key = Console.ReadKey(true);
    switch (key.Key)
    {
        case ConsoleKey.C:
            Console.WriteLine("Clearing the conveyor.");
            conveyor.Clear();
            break;
        case ConsoleKey.N:
            if (controller.State == ControllerState.Off && conveyor.HasBiscuits)
            {
                Console.WriteLine(dirtyConveyorMessage);
                break;
            }
            Console.WriteLine("Turning on the machine.");
            controller.TurnOn();
            break;
        case ConsoleKey.F:
            Console.WriteLine("Turning off the machine.");
            controller.TurnOff();
            if (conveyor.HasBiscuits)
            {
                Console.WriteLine(dirtyConveyorMessage);
            }
            break;
        case ConsoleKey.P:
            if (controller.State == ControllerState.Off && conveyor.HasBiscuits)
            {
                Console.WriteLine(dirtyConveyorMessage);
                break;
            }
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
