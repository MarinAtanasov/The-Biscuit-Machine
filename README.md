# The-Biscuit-Machine

This repository requires [.NET SDK version 6.0.401](https://dotnet.microsoft.com/en-us/download/dotnet/6.0).

To start the BiscuitMachine™, start BiscuitMachine.ConsoleApp.

## Operation
- When switched on, the machine must wait for the  oven to warm up before starting the conveyor belt.
- Biscuits must be cooked at a temperature of 220 - 240°C (the oven will overheat if the heating element is on all the time).
- ~~If the operator selects "Pause", all movement must be stopped immediately but the oven should be kept heated.~~
- ~~When "Off" is selected, the machine should be shut down leaving nothing on the conveyor belt.~~
- If the operator selects "Pause", the machine should finish its current batch, then stop all movement while keeping the oven heated.
- When "Off" is selected, the machine should stop all components, including the oven.
- To stop the machine normally, the operator needs to pause the machine to empty the conveyor, then turn it off.
- To stop the machine in case of an emergency, the operator needs to switch the machine off, then manually clean the conveyor.
