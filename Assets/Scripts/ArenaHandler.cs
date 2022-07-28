using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class ArenaHandler : MonoBehaviour
{
    public static ArenaHandler Instance { get; private set; }

    public Pathfinding pathfinding;
    private Grid<PathNode> grid;
    
    public Tilemap obstacleMap;
    public GameObject ArenaContextMenu;
    private GameObject prevMenu = null;

    public int mapWidth;
    public int mapHeight;
    public float cellSize;
    public Vector3 origin;

    public SelectState selectState;                                         // What menu item the player selected recently.

    public Grid tileGrid;
    [SerializeField] private Tilemap interactiveMap;                 // Tilemap used to show the player where their mouse is.
    [SerializeField] private Tilemap actionMap;                      // Tilemap used to show the player where they have decided to attack, move, heal, etc.
    [SerializeField] private Tile hoverTile;                         // Tile used as hovering visual.
    [SerializeField] private Tile attackTile;                        // Tile used as attack visual.
    [SerializeField] private Tile moveTile;                          // Tile used as movement visual.

    private Vector3Int previousMousePos = new Vector3Int();

    public enum SelectState
    {
        Default,
        Attack,
        Movement,
        AOE,
        Health
    }

    private void Awake()
    {
        Instance = this;
        pathfinding = new Pathfinding(mapWidth, mapHeight, cellSize, origin);
        selectState = SelectState.Default;
        grid = pathfinding.GetGrid();
        pathfinding.UpdateObstacles(obstacleMap);
    }

    void Update()
    {
        UpdateInteractionMap();
    }

    public Grid<PathNode> GetGrid()
    { 
        return grid;
    }

    public void UpdateActionMap(Vector3 worldPosition, SelectState selectState)
    {
        Vector3Int pos = tileGrid.WorldToCell(worldPosition);
        switch (selectState) {
            case SelectState.Attack:
                actionMap.SetTile(pos, attackTile);
                break;
            case SelectState.Movement:
                actionMap.SetTile(pos, moveTile);
                break;
            default:
                actionMap.SetTile(pos, null);
                break;
        }
        
    }

    private void UpdateInteractionMap()
    {
        // Mouse over -> highlight tile
        Vector3Int mousePos = tileGrid.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        if (!mousePos.Equals(previousMousePos))
        {
            interactiveMap.SetTile(previousMousePos, null); // Remove old hoverTile
            interactiveMap.SetTile(mousePos, hoverTile);
            previousMousePos = mousePos;
        }
    }

    public void SpawnContextMenu(Vector3 worldPosition) // Might make more complex, depending on where the camera is
    {
        if (prevMenu != null) DestroyContextMenu();
        prevMenu = Instantiate(ArenaContextMenu, new Vector3 (worldPosition.x + 0.1f, worldPosition.y, 0), Quaternion.identity, GameObject.Find("Canvas").transform); // Adding 5 to x as a base, just moving it over to the side for visibility
    }

    public void SpawnContextMenu(PathNode node) 
    {
        SpawnContextMenu(grid.GetWorldPosition(node.x, node.y));
    }

    public void DestroyContextMenu()    // Separate in case I need to add something to this, visually.
    {
        Destroy(prevMenu);
        prevMenu = null;
    }

    public class EmptyGridObject
    {

        private Grid<EmptyGridObject> grid;
        private int x;
        private int y;

        public EmptyGridObject(Grid<EmptyGridObject> grid, int x, int y)
        {
            this.grid = grid;
            this.x = x;
            this.y = y;

            Vector3 worldPos00 = grid.GetWorldPosition(x, y);
            Vector3 worldPos10 = grid.GetWorldPosition(x + 1, y);
            Vector3 worldPos01 = grid.GetWorldPosition(x, y + 1);
            Vector3 worldPos11 = grid.GetWorldPosition(x + 1, y + 1);

            Debug.DrawLine(worldPos00, worldPos01, Color.white, 999f);
            Debug.DrawLine(worldPos00, worldPos10, Color.white, 999f);
            Debug.DrawLine(worldPos01, worldPos11, Color.white, 999f);
            Debug.DrawLine(worldPos10, worldPos11, Color.white, 999f);
        }

        public override string ToString()
        {
            return "";
        }
    }
}
