using System.Numerics;

namespace DGD203_2
{
    public class Game
    {
        #region VARIABLES
        #region Game Constants

        private const int _defaultMapWidth = 5;
        private const int _defaultMapHeight = 5;

        #endregion

        #region Game Variables

        #region Player Variables

        public Player Player { get; set; }
        public Inventory Inventory { get; set; }
        public NPC _NPC { get; set; }

        private string _playerName;
        private List<Item> _loadedItems;

        #endregion

        #region World Variables

        private Location[] _locations;
        public Vector2 bladeOfOlympusLocation = new Vector2(1, 2);

        #endregion

        public bool _gameRunning;
        public Map _gameMap;
        private string _playerInput;
        public string _answer;
        public bool canTalk = false;
        public bool canTake = true;

        #endregion

        #endregion

        #region METHODS

        #region Initialization

        public void StartGame(Game gameInstanceReference)
        {
            CreateNewMap();

            LoadGame();

            CreatePlayer();

            _NPC = new NPC(_answer, new Vector2(1, 2), canTake);

            InitializeGameConditions();

            _gameRunning = true;

            StartGameLoop();
        }

        private void CreateNewMap()
        {
            _gameMap = new Map(this, _defaultMapWidth, _defaultMapHeight);
        }

        private void CreatePlayer()
        {
            if (_playerName == null)
            {
                GetPlayerName();
            }

            Player = new Player(_playerName, _loadedItems);
            Console.WriteLine($"{_playerName} health is:" + Player.Health.ToString());
        }

        private void GetPlayerName()
        {
            Console.WriteLine("Welcome soldier to this mighty quest !");
            Console.WriteLine("can you give me your name?");
            _playerName = Console.ReadLine();

            Console.WriteLine($"Pleased to meet you {_playerName}, we will have a great dangerous adventure together!,in these journy you find your self in the greek odyssey,A dark odyssey,go in these strange lands find the blade of olympus and kill zeus,he brought enough chaos to this land time for someone to stop him\n\nif you need any help with commands type 'help'");
        }

        private void InitializeGameConditions()
        {
            _gameMap.CheckForLocation(_gameMap.GetCoordinates());
        }

        #endregion

        #region Game Loop

        private void StartGameLoop()
        {
            while (_gameRunning)
            {
                GetInput();
                ProcessInput();
                NpcStart();

                if (Player.Health == 0)
                {
                    Dead();
                }
            }
        }

        public void Dead()
        {
            Console.WriteLine($"dead??!!!,dont let that stop you{_playerName} try again until you finish the misery of these lands");
            exiting();
        }

        private void GetInput()
        {
            _playerInput = Console.ReadLine();
        }

        public void NpcStart()
        {
            if (_gameMap.GetCoordinates() == new Vector2(1, 2))
            {
                canTalk = true;
            }
            else
            {
                canTalk = false;
            }
        }

        private void ProcessInput()
        {
            if (_playerInput == "" || _playerInput == null)
            {
                Console.WriteLine("Give me a command!");
                return;
            }

            switch (_playerInput)
            {
                case "n":
                    _gameMap.MovePlayer(0, 1);
                    break;
                case "s":
                    _gameMap.MovePlayer(0, -1);
                    break;
                case "e":
                    _gameMap.MovePlayer(1, 0);
                    break;
                case "w":
                    _gameMap.MovePlayer(-1, 0);
                    break;
                case "exit":
                    exiting();
                    break;
                case "save":
                    SaveGame();
                    Console.WriteLine("Game saved");
                    break;
                case "load":
                    LoadGame();
                    Console.WriteLine("Game loaded");
                    break;
                case "help":
                    Console.WriteLine(HelpMessage());
                    break;
                case "where":
                    _gameMap.CheckForLocation(_gameMap.GetCoordinates());
                    break;
                case "clear":
                    Console.Clear();
                    break;
                case "who":
                    Console.WriteLine($"You are {Player.Name}, the greek champion");
                    break;
                case "take":
                    if (_gameMap.GetCoordinates() == _NPC.npcLoc)
                    {
                        if (!_NPC.cantake)
                        {
                            _gameMap.TakeItem(Player, _gameMap.GetCoordinates());
                        }
                        else
                        {
                            Console.WriteLine("Cant take the bomb");
                        }
                    }
                    else
                        _gameMap.TakeItem(Player, _gameMap.GetCoordinates());
                    break;
                case "talk":
                    if (_gameMap.GetCoordinates() == _NPC.npcLoc)
                    {
                        _NPC.start();
                    }
                    break;
                case "blade":
                    if (_gameMap.GetCoordinates() == new Vector2(2, -2))
                    {
                        if (Player.playerHasblade(Item.blade))
                        {
                            Player.playerHasblade(Item.blade);
                            Console.WriteLine($"great,with zeus not around we can see hope here, thank you {_playerName}");
                            exiting();
                        }
                        else
                        {
                            Dead();
                        }
                    }
                    else
                    {
                        Console.WriteLine("Command not recognized. Please type 'help' for a list of available commands");
                    }
                    break;
                case "bag":
                    Player.CheckInventory();
                    break;
                default:
                    Console.WriteLine("Command not recognized. Please type 'help' for a list of available commands");
                    break;
            }
        }

        public void exiting()
        {
            Console.WriteLine($"see you later {_playerName}");
            Console.WriteLine("made by:Ahmed naffa");
            _gameRunning = false;
        }

        #endregion

        #region Save Management

        private void LoadGame()
        {
            string path = SaveFilePath();

            if (!File.Exists(path)) return;

            // Reading the file contents
            string[] saveContent = File.ReadAllLines(path);

            // Set the player name
            _playerName = saveContent[0];

            // Set player coordinates
            List<int> coords = saveContent[1].Split(',').Select(int.Parse).ToList();
            Vector2 coordArray = new Vector2(coords[0], coords[1]);

            // Set player inventory
            _loadedItems = new List<Item>();

            List<string> itemStrings = saveContent[2].Split(',').ToList();

            for (int i = 0; i < itemStrings.Count; i++)
            {
                if (Enum.TryParse(itemStrings[i], out Item result))
                {
                    Item item = result;
                    _loadedItems.Add(item);
                    _gameMap.RemoveItemFromLocation(item);
                }
            }

            _gameMap.SetCoordinates(coordArray);

        }

        private void SaveGame()
        {
            // Player Coordinates
            string xCoord = _gameMap.GetCoordinates()[0].ToString();
            string yCoord = _gameMap.GetCoordinates()[1].ToString();
            string playerCoords = $"{xCoord},{yCoord}";

            // Player inventory
            List<Item> items = Player.Inventory.Items;
            string playerItems = "";
            for (int i = 0; i < items.Count; i++)
            {
                playerItems += items[i].ToString();

                if (i != items.Count - 1)
                {
                    playerItems += ",";
                }
            }

            string saveContent = $"{_playerName}{Environment.NewLine}{playerCoords}{Environment.NewLine}{playerItems}";

            string path = SaveFilePath();

            File.WriteAllText(path, saveContent);
        }

        private string SaveFilePath()
        {
            // Get the save file path
            string workingDirectory = Environment.CurrentDirectory;
            string projectDirectory = Directory.GetParent(workingDirectory).Parent.Parent.FullName;
            string path = projectDirectory + @"\save.txt";

            return path;
        }

        #endregion

        #region Miscellaneous

        private string HelpMessage()
        {
            return @"Here are the current commands:
n: go north
s: go south
w: go west
e: go east
where: find your current location
who: what is your name
Clear: clear the screen
blade: to put the blade inside zeus heart
take: take an item 
talk: talk with a npc
bag: see what items you have
load: Load saved game
save: save current game
exit: exit the game";
        }
        #endregion

        #endregion
    }
}