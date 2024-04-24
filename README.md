# Caterpillar Control System

This is an implementation of the caterpillar control system that adheres to the rules described. 
The caterpillar class includes methods for moving the head, growing/shrinking, and checking for obstacles, spices, and boosters. 

## Commands

* U: Move Up
* D: Move Down
* L: Move Left 
* R: Move Right
* G: Grow Caterpillar
* S: Shrink Caterpillar

The rider enters a command and will be prompted to enter steps if the command is U/D/L/R 
```c-sharp
ExecuteRiderCommand(char riderCommand, int steps = 0)
```

## 