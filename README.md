# Game1377_Exercise4


Assignment: Asteroids Game

Objective:
Implement a prototype of the game Asteroids. Be sure to set your Aspect Ratio to 4:3 in the Game view

Part 1:
You should be able to move according to Asteroids control, using W to increase thrust and A/D to rotate,
and use left shift to teleport to a random location on screen. The player should also wrap around the screen
(code for this is provided). 

Part 2:
The player should also be able to shoot bullets using the space bar and die when colliding with an asteroids
The asteroids should spawn randomly on screen at the start and move in random directions. The asteroids should also
wrap around the screen, and should also split into smaller asteroids when shot until they are the smallest size. 

The scripts provided are:
1. SpaceshipController.cs
2. ScreenWrap.cs 
3. ScreenBounds.cs
4. Bullet.cs
5. AsteroidSpawner.cs
6. Asteroid.cs

Prefabs provided are:
1. Asteroid (along with Asteroid Large and Asteroid Small Prefab Variants)
2. Spaceship
3. Bullet

Requirements
1. Complete the implementation of the provided scripts to create a functional asteroids game prototype.
2. You should not need to create any new scripts or delete any existing code.
3. Add the proper scripts needed to the appropriate prefabs or GameObjects in the scene. 

Hints
- There is an enum called AsteroidSize in the Asteroid.cs. Be sure to use that in your implementation. 
- The ScreenBounds.cs script has variables for the screen bounds. You can use those for random spawning and teleporting.
- The ScreenWrap.cs script has code for wrapping around the screen. 
