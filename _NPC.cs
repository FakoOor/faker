using DGD203_2;
using System.Numerics;

public class NPC
{
    public Player Player { get; set; }
    public Game Game { get; set; }
    public Map Map { get; set; }
    public Location Location { get; set; }
    public Inventory Inventory { get; set; }

    public string answer;
    public Vector2 npcLoc;
    public string name;
    public bool cantake = true;
    public int heatlh;

    public NPC(string answer, Vector2 npclocation, bool cantake)
    {
        this.answer = answer;
        npcLoc = npclocation;
        this.cantake = cantake;
    }

    public void start()
    {
        Player = new Player(name, null);
        Inventory = new Inventory();
        Game = new Game();
        Map = new Map(Game, 5, 5);
        heatlh = Player.Health;
        Console.WriteLine("hello,is someone there i have a bat and iam not afraid to use it,oh hello sorry i thout you are a thief but from your look you dont seem like one, or at least a good one,anyway how can i help you,did you came for the blades,will you wont be the first one to come but who knows maybe you are the first one to take it,to take it you need to answer me this question and i will give you but if you fail the curse of the blade will be on you and kill you so be carfull.");
        question();
        while (cantake)
        {
            getInput();
            handleInput();
        }
    }

    public void question()
    {
        Console.WriteLine("so,my question is,\n\nwhat is the name of zeus son who is the god of war\nA.a Ares\nB.Apollo\nC.Kronos\nD.Hades\nE.poseidon");
    }

    public void getInput()
    {
        answer = Console.ReadLine();
    }

    public void handleInput()
    {
        if (answer != null)
        {
            if (heatlh > 0)
            {
                if (cantake)
                {
                    switch (answer)
                    {
                        case "a":
                            Console.WriteLine("good,you are really a smart soldier not just strong one, here,take your blade and move for greatniss");
                            Player.playerHasblade(Item.blade);
                            cantake = false;
                            break;
                        case "b":
                            if (heatlh == 100)
                            {
                                Console.WriteLine("false you have one chance left");
                                Console.WriteLine("Hint:he was the main villain in god of war 1");
                            }
                            else if (heatlh == 50)
                            {
                                Console.WriteLine("false you have a last chance left");
                            }
                            break;
                        case "c":
                            if (heatlh == 100)
                            {
                                Console.WriteLine("false you have one chance left");
                                Console.WriteLine("Hint:he was the main villain in god of war 1");
                            }
                            else if (heatlh == 50)
                            {
                                Console.WriteLine("false you have a last chance left");
                            }
                            break;
                        case "d":
                            if (heatlh == 100)
                            {
                                Console.WriteLine("false you have one chance left");
                                Console.WriteLine("Hint:he was the main villain in god of war 1");
                            }
                            else if(heatlh == 50)
                            {
                                Console.WriteLine("false you have a last chance left");
                            }
                            break;
                        case "e":
                            if (heatlh == 100)
                            {
                                Console.WriteLine("false you have one chance left");
                                Console.WriteLine("Hint:he was the main villain in god of war 1");
                            }
                            else if (heatlh == 50)
                            {
                                Console.WriteLine("false you have a last chance left");
                            }
                            break;
                        default:
                            Console.WriteLine("wrong input!!");
                            break;
                    }
                    heatlh = heatlh - 50;
                }
            }
            else
            {
                Game.Dead();
                exit();
            }
        }
    }
    public void exit()
    {
        Environment.Exit(0);
    }


}
