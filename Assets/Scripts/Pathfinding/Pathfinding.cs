using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

// CodeMonkey heavily referenced, almost all code from him.
public class Pathfinding {

    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;

    public static Pathfinding Instance { get; private set; }

    private Grid<PathNode> grid;
    private List<PathNode> openList;
    private List<PathNode> closedList;
    
    public Pathfinding(int width, int height, float cellSize, Vector3 originPosition) {
        Instance = this;
        grid = new Grid<PathNode>(width, height, cellSize, originPosition, (Grid<PathNode> g, int x, int y) => new PathNode(g, x, y));
    }

    public Grid<PathNode> GetGrid() {
        return grid;
    }
    
    // Use Breadth First Search to get a list of all nodes within the walking (or fighting) range
    public List<PathNode> GetValidNodes(Vector3 startWorldPosition, int range) {
        // Init
        List<PathNode> validTiles = new List<PathNode>();
        List<PathNode> exploredTiles = new List<PathNode>();
        grid.GetXY(startWorldPosition, out int startX, out int startY);

        for (int x = 0; x < grid.GetWidth(); ++x)
        {
            for (int y = 0; y < grid.GetHeight(); ++y)
            {
                grid.GetGridObject(x,y).distanceCost = Int32.MaxValue;
            }
        }

        // Begin loop, starting with start node, which has lowest values. https://en.wikipedia.org/wiki/Breadth-first_search
        grid.GetGridObject(startX, startY).distanceCost = 0;
        exploredTiles.Add(grid.GetGridObject(startX, startY));

        Queue<PathNode> queue = new Queue<PathNode>();
        queue.Enqueue(grid.GetGridObject(startX, startY));

        while (queue.Count > 0)
        {
            PathNode current = queue.Dequeue();
            foreach (PathNode neighbour in GetNeighbourList(current)) // Check all of current node's neighbors.
            {
                if (!exploredTiles.Contains(neighbour)) {
                    neighbour.distanceCost = current.distanceCost + 1;
                    if (neighbour.isWalkable && neighbour.distanceCost <= range) {
                        queue.Enqueue(neighbour); // This node is valid, continue process
                        validTiles.Add(neighbour);
                    }
                    exploredTiles.Add(neighbour);
                }

            }
        }
        return validTiles;
    }

    // Returns a list of vector3's so that a character may actually follow the path to get to where they want to go.
    public List<Vector3> FindPath(Vector3 startWorldPosition, Vector3 endWorldPosition) {
        grid.GetXY(startWorldPosition, out int startX, out int startY);
        grid.GetXY(endWorldPosition, out int endX, out int endY);

        Debug.Log("Start X: " + startX + "\nStart Y: " + startY + "\nEnd X: " + endX + "\nEnd Y: " + endY);
        ArenaHandler.Instance.UpdateActionMap(grid.GetWorldPosition(endX, endY), ArenaHandler.SelectState.Attack);

        List<PathNode> path = FindPath(startX, startY, endX, endY);
        if (path == null) {
            return null;
        } else {
            List<Vector3> vectorPath = new List<Vector3>();
            foreach (PathNode pathNode in path) {
                vectorPath.Add(new Vector3(pathNode.x, pathNode.y) * grid.GetCellSize() + Vector3.one * grid.GetCellSize() * .5f + grid.GetOriginPosition());
            }
            return vectorPath;
        }
    }

    // Find a path with A* pathfinding. https://en.wikipedia.org/wiki/A*_search_algorithm
    public List<PathNode> FindPath(int startX, int startY, int endX, int endY) {
        PathNode startNode = grid.GetGridObject(startX, startY);
        PathNode endNode = grid.GetGridObject(endX, endY);

        if (startNode == null || endNode == null) {
            // Invalid Path
            return null;
        }

        openList = new List<PathNode> { startNode };
        closedList = new List<PathNode>();

        for (int x = 0; x < grid.GetWidth(); x++) {
            for (int y = 0; y < grid.GetHeight(); y++) {
                PathNode pathNode = grid.GetGridObject(x, y);
                pathNode.gCost = 99999999;  // Consider changing to Int32.MaxValue
                pathNode.CalculateFCost();
                pathNode.cameFromNode = null;
            }
        }

        startNode.gCost = 0;
        startNode.hCost = CalculateDistanceCost(startNode, endNode);
        startNode.CalculateFCost();
        

        while (openList.Count > 0) {
            PathNode currentNode = GetLowestFCostNode(openList);
            if (currentNode == endNode) {
                // Reached final node
                return CalculatePath(endNode);
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            foreach (PathNode neighbourNode in GetNeighbourList(currentNode)) {
                if (closedList.Contains(neighbourNode)) continue;
                if (!neighbourNode.isWalkable) {
                    closedList.Add(neighbourNode);
                    continue;
                }

                int tentativeGCost = currentNode.gCost + CalculateDistanceCost(currentNode, neighbourNode);
                if (tentativeGCost < neighbourNode.gCost) {
                    neighbourNode.cameFromNode = currentNode;
                    neighbourNode.gCost = tentativeGCost;
                    neighbourNode.hCost = CalculateDistanceCost(neighbourNode, endNode);
                    neighbourNode.CalculateFCost();

                    if (!openList.Contains(neighbourNode)) {
                        openList.Add(neighbourNode);
                    }
                }
            }
        }

        // Out of nodes on the openList
        return null;
    }

    public List<PathNode> GetNeighbourList(PathNode currentNode) {
        List<PathNode> neighbourList = new List<PathNode>();

        if (currentNode.x - 1 >= 0) {
            // Left
            neighbourList.Add(GetNode(currentNode.x - 1, currentNode.y));
            // Left Down
            if (currentNode.y - 1 >= 0) neighbourList.Add(GetNode(currentNode.x - 1, currentNode.y - 1));
            // Left Up
            if (currentNode.y + 1 < grid.GetHeight()) neighbourList.Add(GetNode(currentNode.x - 1, currentNode.y + 1));
        }
        if (currentNode.x + 1 < grid.GetWidth()) {
            // Right
            neighbourList.Add(GetNode(currentNode.x + 1, currentNode.y));
            // Right Down
            if (currentNode.y - 1 >= 0) neighbourList.Add(GetNode(currentNode.x + 1, currentNode.y - 1));
            // Right Up
            if (currentNode.y + 1 < grid.GetHeight()) neighbourList.Add(GetNode(currentNode.x + 1, currentNode.y + 1));
        }
        // Down
        if (currentNode.y - 1 >= 0) neighbourList.Add(GetNode(currentNode.x, currentNode.y - 1));
        // Up
        if (currentNode.y + 1 < grid.GetHeight()) neighbourList.Add(GetNode(currentNode.x, currentNode.y + 1));

        return neighbourList;
    }

    public PathNode GetNode(int x, int y) {
        return grid.GetGridObject(x, y);
    }

    public PathNode GetNode(Vector3 worldPosition) {
        grid.GetXY(worldPosition, out int x, out int y);
        return GetNode(x, y);
    }

    private List<PathNode> CalculatePath(PathNode endNode) {
        List<PathNode> path = new List<PathNode>();
        path.Add(endNode);
        PathNode currentNode = endNode;
        while (currentNode.cameFromNode != null) {
            path.Add(currentNode.cameFromNode);
            ArenaHandler.Instance.UpdateActionMap(grid.GetWorldPosition(currentNode.x, currentNode.y), ArenaHandler.SelectState.Attack);
            currentNode = currentNode.cameFromNode;
        }
        path.Reverse();
        return path;
    }

    private int CalculateDistanceCost(PathNode a, PathNode b) {
        int xDistance = Mathf.Abs(a.x - b.x);
        int yDistance = Mathf.Abs(a.y - b.y);
        int remaining = Mathf.Abs(xDistance - yDistance);
        return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;
    }

    private PathNode GetLowestFCostNode(List<PathNode> pathNodeList) {
        PathNode lowestFCostNode = pathNodeList[0];
        for (int i = 1; i < pathNodeList.Count; i++) {
            if (pathNodeList[i].fCost < lowestFCostNode.fCost) {
                lowestFCostNode = pathNodeList[i];
            }
        }
        return lowestFCostNode;
    }

    // Update the obstacle tiles
    public void UpdateObstacles(Tilemap obstacleMap) {
        for (int x = 0; x < grid.GetWidth(); x++)
        {
            for (int y = 0; y < grid.GetHeight(); y++)
            {
                if (obstacleMap.HasTile(obstacleMap.WorldToCell(grid.GetWorldPosition(x, y)))) {
                    grid.GetGridObject(x,y).SetIsWalkable(false);
                }
                else {
                    grid.GetGridObject(x, y).SetIsWalkable(true);
                }
            }
        }
    }
}
