# Unity-University-project
Academic Project in Unity using C# : The player is spawned inside the ground floor of a maze (with multiple floors), constructed by cubes with different colors and textures. The goal is to escape, reaching the top floor of the maze. The player carries a small number of hammers (that can withstand only a few hits) which help him break some cubes. Every cube breaks into a random number of smaller cubes after 3 hits and there is a chance that a new hammer will spawn in the cube's location for the player to pick up. There are also 2 "special", more transparent cubes in every floor. Once the player gets inside one of those, he instantly teleports to the other cube of the same floor. Every action, such as the animation of the hammer (swing, hit, break), teleportation, jump etc is followed by relevant sound effects. Lastly, the player has only a limited amount of time to escape which is shown on the upper side of the screen along with the number of remaining hammers and his score. 

R : Rotates the camera around the maze  
V : Change shader and make cubes transparent  
X : Instantly ends the game  
E : Press E when on top floor to decide if you won or lost the game depending on your score  
Arrows : move camera while in "God" mode (when not in first person)  
Scroll Wheel : zoom in and out while in "God" mode  
ESC : close the application  
