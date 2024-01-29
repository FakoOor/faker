using DGD203_2;
using System.Numerics;

public class Map
{
    private Game _theGame;

    private Vector2 _coordinates;

    private int[] _widthBoundaries;
    private int[] _heightBoundaries;

    private Location[] _locations;
    public NPC _npc;
    public Game Game;

    public Map(Game game, int width, int height)
    {
        _theGame = game;

        int widthBoundary = (width - 1) / 2;

        _widthBoundaries = new int[2];
        _widthBoundaries[0] = -widthBoundary;
        _widthBoundaries[1] = widthBoundary;

        int heightBoundary = (height - 1) / 2;

        _heightBoundaries = new int[2];
        _heightBoundaries[0] = -heightBoundary;
        _heightBoundaries[1] = heightBoundary;

        _coordinates = new Vector2(0, 0);

        GenerateLocations();

        Console.WriteLine($"Created map with size {width}x{height}");
    }

    #region Coordinates

    public Vector2 GetCoordinates()
    {
        return _coordinates;
    }

    public void SetCoordinates(Vector2 newCoordinates)
    {
        _coordinates = newCoordinates;
    }

    #endregion

    #region Movement

    public void MovePlayer(int x, int y)
    {
        int newXCoordinate = (int)_coordinates[0] + x;
        int newYCoordinate = (int)_coordinates[1] + y;

        if (!CanMoveTo(newXCoordinate, newYCoordinate))
        {
            Console.WriteLine("You can't go that way Turn Back");
            return;
        }

        _coordinates[0] = newXCoordinate;
        _coordinates[1] = newYCoordinate;

        CheckForLocation(_coordinates);
    }

    private bool CanMoveTo(int x, int y)
    {
        return !(x < _widthBoundaries[0] || x > _widthBoundaries[1] || y < _heightBoundaries[0] || y > _heightBoundaries[1]);
    }

    #endregion

    #region Locations

    private void GenerateLocations()
    {
        _locations = new Location[5];

        Vector2 zeusLocation = new Vector2(2, -2);
        Location zeus = new Location("zeus", "\nlord of the lords,but a bad one", LocationType.Combat, zeusLocation);
        _locations[0] = zeus;

        Vector2 hadesLocation = new Vector2(-2, 1);
        Location hades = new Location("hades layout", "the lord of hell, angry man but good one deep down", LocationType.City, hadesLocation);
        _locations[1] = hades;

        Vector2 pandoraLocation = new Vector2(1, 2);
        List<Item> pandoraItem = new List<Item>();
        pandoraItem.Add(Item.blade);
        Location pandora = new Location("pandora", "for decades pandora was holding the blades of olympos,waiting for a worthy soldier who can end zeus but she aint easy one", LocationType.npc, pandoraLocation, pandoraItem);
        _locations[2] = pandora;

        Vector2 poseidonLocation = new Vector2(-2, -1);
        List<Item> poseidonItem = new List<Item>();
        poseidonItem.Add(Item.Coin);
        Location poseidon = new Location("poseidon", "lord of the seas,yet drowned in his life and isolated alone,but he is really rich", LocationType.City, poseidonLocation, poseidonItem);
        _locations[3] = poseidon;

        Vector2 athenaLocation = new Vector2(2, 0);
        List<Item> athenaItem = new List<Item>();
        athenaItem.Add(Item.Rune);
        Location athena = new Location("athena", "a wise one,a good one,but not a strong one so she always alone keeping herself in old runes study", LocationType.City, athenaLocation, athenaItem);
        _locations[4] = athena;
    }

    public void CheckForLocation(Vector2 coordinates)
    {
        Console.WriteLine($"You are now standing on {_coordinates[0]},{_coordinates[1]}");

        if (IsOnLocation(_coordinates, out Location location))
        {
            if (location.Type == LocationType.Combat)
            {
                Console.WriteLine("Prepare to fight!,use the BLADE of olympas");
                Combat combat = new Combat(_theGame);
            }
            else if (location.Type == LocationType.City)
            {
                Console.WriteLine($"You are in {location.Name} {location.Type}");
                Console.WriteLine(location.Discription);

                if (HasItem(location))
                {
                    Console.WriteLine($"There is a {location.ItemsOnLocation[0]} here TAKE it");
                }
            }
            else if (location.Type == LocationType.npc)
            {
                Console.WriteLine("you see pandora TALK to her.");
            }
        }
    }

    private bool IsOnLocation(Vector2 coords, out Location foundLocation)
    {
        for (int i = 0; i < _locations.Length; i++)
        {
            if (_locations[i].Coordinates == coords)
            {
                foundLocation = _locations[i];
                return true;
            }
        }
        foundLocation = null;
        return false;
    }

    private bool HasItem(Location location)
    {
        return location.ItemsOnLocation.Count != 0;
    }

    public void TakeItem(Player player, Vector2 coordinates)
    {
        if (IsOnLocation(coordinates, out Location location))
        {
            if (HasItem(location))
            {
                Item itemOnLocation = location.ItemsOnLocation[0];

                player.TakeItem(itemOnLocation);
                location.RemoveItem(itemOnLocation);

                Console.WriteLine($"You took the {itemOnLocation}");

                return;
            }
        }
        Console.WriteLine("There is nothing to take here!");
    }

    public void RemoveItemFromLocation(Item item)
    {
        for (int i = 0; i < _locations.Length; i++)
        {
            if (_locations[i].ItemsOnLocation.Contains(item))
            {
                _locations[i].RemoveItem(item);
            }
        }
    }

    #endregion
}