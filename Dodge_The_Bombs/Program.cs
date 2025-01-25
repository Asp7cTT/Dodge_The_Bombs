using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace ConsoleApp1
{
    internal class Program
    {
        // I Decalred these variables here so when we update the statics such as score & etc...
        // it changes globally so the entire program knows the new value
        static int score = 0;
        static int counter = 0;
        static int playerPosX = 20;
        static int playerPosY = 20;
        static int[] bombPosX = new int[5];
        static int[] bombPosY = { 2, 2, 2, 2, 2 };
        static int[] healthPos = new int[2];
        static int[] shieldPos = new int[2];
        static int[] bombSpeed = new int[5];
        static int gameSpeed = 100;
        static int level = 1;
        static int health = 3;
        static bool gameOver = false;
        static bool gameStarted = false;
        static bool healthAvailable = false;
        static bool shieldAvailable = false;
        static bool playerHasShield = false;
        static char player = '@';
        static string bomb = "*";
        static string addHp = "H";
        static string shield = "S";
        static void UpdateTheGameCondition()
        {
            Console.Clear();
            //Draw the stats
            Console.WriteLine($"Score: {score}|Level: {level}|HP: {health} Shield: {playerHasShield}");
            Console.WriteLine("_________________________________________");
            Console.WriteLine("");
            //Draw the bombs
            for (int i = 0; i < 5; i++)
            {
                Console.SetCursorPosition(bombPosX[i], bombPosY[i]);
                Console.WriteLine(bomb);
            }
            //Draw the player
            Console.SetCursorPosition(playerPosX, playerPosY);
            Console.WriteLine(player);
            //Draw health
            if (healthAvailable)
            {
                Console.SetCursorPosition(healthPos[0], healthPos[1]);
                Console.WriteLine(addHp);
            }
            //Draw shield
            if (shieldAvailable)
            {
                Console.SetCursorPosition(shieldPos[0], shieldPos[1]);
                Console.WriteLine(shield);
            }
        }
        static void Input()
        {
            //Console.KeyAvailable Checks if a key has been pressed.
            //If a key has been pressed then the conditions inside the if statement runs
            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo input = Console.ReadKey();
                // Player moves to the left by pressing A
                if (input.Key == ConsoleKey.A && playerPosX > 0)
                {
                    playerPosX--;
                }
                // Player moves to the right by pressing D
                if (input.Key == ConsoleKey.D && playerPosX < 42)
                {
                    playerPosX++;
                }
            }
        }
        static void Logic()
        {
            Random random = new Random();
            for (int i = 0; i < 5; i++)
            {
                bombSpeed[i] = random.Next(1, 3);
                counter++;
            }
            for (int i = 0; i < 5; i++)
            {
                //If the random number was 1, add 1 to the bomb Y cordinate
                if (bombSpeed[i] == 1)
                {
                    bombPosY[i]++;
                }
                //If the random number was 2, add 2 to the bomb Y cordinate
                if (bombSpeed[i] == 2)
                {
                    bombPosY[i] = bombPosY[i] + 2;
                }
                //If the bomb's Y Cordinate was 19 then always add to cordinate by 1 unit
                //This perevents a bug where bombs would have a collision with player
                //but the player would've never lose HP
                if (bombPosY[i] == 19)
                {
                    bombPosY[i]++;
                }
            }

            Random cordinate = new Random();
            //Remove the bomb & add to score when the bomb went below the player Y Cordinate
            for (int i = 0; i < 5; i++)
            {
                if (bombPosY[i] > 23)
                {
                    score++;
                    bombPosX[i] = cordinate.Next(1, 42);
                    bombPosY[i] = 2;
                    counter--;
                }
            }
            //Move the health down
            if (healthAvailable)
            {
                healthPos[1]++;
            }
            //Remove the health at the bottom of the screen
            if (healthPos[1] == 23)
            {
                healthAvailable = false;
                healthPos[0] = 0;
                healthPos[1] = 0;
            }
            //Add to 1 to the HP of the player when player was colisioned by "H"
            if (healthPos[0] == playerPosX && healthPos[1] == playerPosY)
            {
                if (health < 3)
                {
                    health++;
                    Console.Beep(800, 50);
                }
                healthAvailable = false;
                healthPos[0] = 0;
                healthPos[1] = 0;
            }
            //Move the shield down
            if (shieldAvailable)
            {
                shieldPos[1]++;
            }
            //Remove the shield at the bottom of the screen
            if (shieldPos[1] == 23)
            {
                shieldAvailable = false;
                shieldPos[0] = 0;
                shieldPos[1] = 0;
            }
            //Set the shield status of the player when player was colisioned by "S"
            if (shieldPos[0] == playerPosX && shieldPos[1] == playerPosY)
            {
                playerHasShield = true;
                Console.Beep(700, 50);
                shieldPos[0] = 0;
                shieldPos[1] = 0;
            }

            //Level 2
            if (score == 20 && level < 2)
            {
                level++;
                gameSpeed = gameSpeed - 10;
            }
            // Level 3
            if (score == 40 && level < 3)
            {
                level++;
                gameSpeed = gameSpeed - 20;
            }
            // Level 4
            if (score == 60 && level < 4)
            {
                level++;
                gameSpeed = gameSpeed - 30;
            }
            // level 5
            if (score == 100 && level < 5)
            {
                level++;
                gameSpeed = gameSpeed - 30;
            }
            //Check if player was damaged by the bomb
            for (int i = 0; i < 5; i++)
            {
                if (bombPosX[i] == playerPosX && bombPosY[i] == playerPosY)
                {
                    //If player didnt have shield then minus the health by 1 point
                    if (!playerHasShield)
                    {
                        health--;
                    }
                    playerHasShield = false;
                    bombPosX[i] = cordinate.Next(1, 42);
                    bombPosY[i] = 2;
                    counter--;
                    score--;
                    Console.Beep(1000, 50);
                }
            }
            //Checking if HP of the player is 0. if True the game ends
            if (health == 0)
            {
                gameOver = true;
            }
            //If health was not created then check the conditons to create one 
            if (!healthAvailable)
            {
                GenerateHealth();
            }
            //If shield was not created then check the conditons to create one 
            {
                GenerateSheild();
            }

        }
        static void GenerateHealth()
        {
            Random random = new Random();
            if (!healthAvailable)
            {
                int powerUp = random.Next(1, 10);
                if (powerUp == 5)
                {
                    healthPos[0] = random.Next(1, 42);
                    healthAvailable = true;
                }
            }
        }
        static void GenerateSheild()
        {
            Random random = new Random();
            if (!shieldAvailable)
            {
                int powerUp = random.Next(1, 10);
                if (powerUp == 6)
                {
                    shieldPos[0] = random.Next(1, 42);
                    shieldAvailable = true;
                }
            }
        }

        static void Main(string[] args)
        {
            Random random = new Random();
            //This "for" statement is for the first wave of the game X Cordinates to be randomized
            for (int i = 0; i < 5; i++)
            {
                if (!gameStarted)
                {
                    bombPosX[i] = random.Next(1, 42);
                    counter++;
                }
                if (counter == 5)
                {
                    gameStarted = true;
                }
            }
        l:
            //Disable the cursor
            Console.CursorVisible = false;
            //Set the windows size
            Console.SetWindowSize(42, 25);
            //While gameOver is false do these tasks
            while (!gameOver)
            {
                UpdateTheGameCondition();
                Input();
                Logic();
                Thread.Sleep(gameSpeed);
            }
            //While gameOver is true do these tasks
            Console.Clear();
            Console.WriteLine($"You Lost :(");
            Console.WriteLine($"Score: {score} Level: {level}");
            Console.WriteLine("_________________________________________");
            Console.WriteLine("Press Enter if you wanna play again");
            Console.WriteLine("Press Escape if you wanna quit");
            ConsoleKeyInfo key = Console.ReadKey();
            //Press Enter to replay
            if (key.Key == ConsoleKey.Enter)
            {
                score = 0;
                level = 1;
                health = 3;
                gameOver = false;
                gameSpeed = 100;
                healthPos[0] = 0;
                healthPos[1] = 0;
                healthAvailable = false;
                goto l;
            }
            //Press Escape to quit
            if (key.Key == ConsoleKey.Escape)
            {
                Environment.Exit(0);
            }

            Console.ReadKey();
        }
    }
}