using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Sources;
using Turn_based_combat;

namespace turn_based_combat
{
    class Program
    {
        static Stack<Attack> attackActions = new Stack<Attack>();

        public static void Main()
        {
            

            Army enemyArmy = new Army("Enemy army", Team.Enemy);
            enemyArmy.AddUnit(new Unit("Skeleton", 15, 15, 15));
            enemyArmy.AddUnit(new Unit("Imp", 20, 10, 10));
            enemyArmy.AddUnit(new Unit("Stalker", 1, 1, 1));

            

            Army playerArmy = new Army("Player army", Team.Player);


            Random randomGenerator = new Random();

            Console.WriteLine("Fiends VS Cowboys!");
            Console.WriteLine();
            Console.WriteLine("I wish you luck traveller....");
            Console.WriteLine();
            Console.WriteLine("Press any key to continue to the army selectection screen!");
            Console.ReadKey();
            Console.Clear();


            int armyChoice;
            bool isValidChoice = false;

            while (!isValidChoice)
            {
                
                ColorPrinter.ColorWriteLine("Select your army by pressing number (1 or 2):", ColorPrinter.noteColor);
                Console.WriteLine("");


                
                ColorPrinter.ColorWriteLine("1. Cowboys:", ColorPrinter.noteColor);
                ColorPrinter.ColorWriteLine("Unit name: Cowboy DMG: 15 HP: 150", ColorPrinter.playerColor);
                ColorPrinter.ColorWriteLine("Unit name: Maiden DMG: 20 HP: 50", ColorPrinter.playerColor);
                ColorPrinter.ColorWriteLine("Unit name: Drunkie DMG: 90 HP: 90", ColorPrinter.playerColor);
                ColorPrinter.ColorWriteLine("", ColorPrinter.noteColor);

                ColorPrinter.ColorWriteLine("2. Fiends: ", ColorPrinter.noteColor);
                ColorPrinter.ColorWriteLine("Unit name: Skeleton DMG: 50 HP: 50", ColorPrinter.playerColor);
                ColorPrinter.ColorWriteLine("Unit name: Imp DMG: 99 HP: 99", ColorPrinter.playerColor);
                ColorPrinter.ColorWriteLine("Unit name: Stalker DMG: 1 HP: 1", ColorPrinter.playerColor);

                
                isValidChoice = int.TryParse(Console.ReadLine(), out armyChoice);

                
                switch (armyChoice)
                {
                    case 1:
                        Console.WriteLine("Cowboys selected");
                        playerArmy.AddUnit(new Unit("Cowboy", 15, 150, 150));
                        playerArmy.AddUnit(new Unit("Maiden", 20, 50, 50));
                        playerArmy.AddUnit(new Unit("Drunkie", 90, 99, 99));
                        break;
                    case 2:
                        Console.WriteLine("Fiends selected");
                        playerArmy.AddUnit(new Unit("Skeleton", 50, 50, 50));
                        playerArmy.AddUnit(new Unit("Imp", 99, 99, 99));
                        playerArmy.AddUnit(new Unit("Stalker", 1, 1, 1));
                        break;
                    default:
                        Console.WriteLine("1 or 2 jackass!");
                        isValidChoice = false;
                        break;
                }

                
                Console.Clear();
            }




            ColorPrinter.ColorWriteLine("Who do you want to attack as (1,2,3) // Select the target you want to attack (1,2,3) to undo press Z ", ColorPrinter.noteColor);
            ColorPrinter.PrintArmy(playerArmy.Name, playerArmy);
            ColorPrinter.PrintArmy(enemyArmy.Name, enemyArmy);

            while (true)
            {
                
                if (playerArmy.AreAllDead())
                {
                    break;
                }
                Unit attacker = selectUnit("Select attacker:", playerArmy);
                Unit defender = selectUnit("Select target:", enemyArmy);
                attack(attacker, defender);


                
                if (enemyArmy.AreAllDead())
                {
                    break;
                }
                attacker = randomizeUnit(enemyArmy, randomGenerator);
                defender = randomizeUnit(playerArmy, randomGenerator);
                attack(attacker, defender);
            }
            Console.Clear();
            ColorPrinter.ColorWriteLine("Battle finished, to claim your xp press enter two times", ColorPrinter.noteColor);
            Console.ReadLine();
        }
        public static Unit selectUnit(string prompt, Army army)
        {
            bool hasMoved = false;

            while (true)
            {
                ColorPrinter.PrintArmy(prompt, army);
                string valinta = Console.ReadLine();

                if (valinta == "z")
                {
                    if (hasMoved = true)
                    {
                        UndoLastAttack();
                        hasMoved = false;
                    }
                    else
                    {
                        ColorPrinter.ColorWriteLine("Cannot undo without making a move first.", ColorPrinter.errorColor);
                    }
                }
                else
                {
                    int numero = Convert.ToInt32(valinta);
                    if (numero > 0 && numero <= army.Units.Count)
                    {
                        int index = numero - 1;
                        Unit selected = army.Units[index];
                        if (selected.hitpoints > 0)
                        {
                            
                            hasMoved = true;
                            return selected;
                        }
                        else
                        {
                            ColorPrinter.ColorWriteLine("Cannot choose a dead unit.", ColorPrinter.errorColor);
                        }
                    }
                    else
                    {
                        ColorPrinter.ColorWriteLine("Invalid unit number. Try again.", ColorPrinter.errorColor);
                    }
                }
            }
        }

        private static void UndoLastAttack()
        {
            if (attackActions.Count > 0)
            {
                Attack last = attackActions.Pop();
                last.Undo();
            }
            else
            {
                ColorPrinter.ColorWriteLine("No moves to undo.", ColorPrinter.errorColor);
            }
        }

        public static Unit randomizeUnit(Army army, Random random)
        {
            if (army.AreAllDead())
            {
                ColorPrinter.ColorWriteLine("Error, cannot select from dead army", ColorPrinter.errorColor);
                return army.Units[0];
            }
            while (true)
            {
                int index = random.Next(army.Units.Count);
                Unit selected = army.Units[index];
                if (selected.hitpoints > 0)
                {
                    
                    return selected;
                }
            }
        }

        public static void attack(Unit attacker, Unit defender)
        {
            if (attacker.hitpoints > 0)
            {
                defender.hitpoints -= attacker.damage;
                ColorPrinter.PrintAttack(attacker, defender);

               
                Attack attack = new Attack(attacker, defender);
                attackActions.Push(attack);
            }
        }
    }
}
