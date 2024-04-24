# Caterpillar Control System

This is an implementation of the caterpillar control system that adheres to the rules described. 
The caterpillar class includes methods for moving the head, growing/shrinking, and checking for obstacles, spices, and boosters. 

## Environment Requirements

* .NET 8

Build and Run the ```CaterpillarControlSystem.App```

## Commands

* U: Move Up
* D: Move Down
* L: Move Left 
* R: Move Right
* G: Grow Caterpillar
* S: Shrink Caterpillar
* Z: Undo Command
* Y: Redo Command

The rider enters a command and will be prompted to enter steps if the command is either ```U, D, L or R```



## Logging

This application uses Serilog as the Logging Provider. The logs are written to a file in the path below within the ```CaterpillarControlSystem.App``` folder

```curl
\bin\Debug\net8.0\logs.txt
```