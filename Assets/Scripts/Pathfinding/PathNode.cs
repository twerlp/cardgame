using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode {
    // Using this as a general grid object now
    private Grid<PathNode> grid;
    public int x;
    public int y;

    public int gCost; // A* Pathfinding
    public int hCost; // A* Pathfinding
    public int fCost; // A* Pathfinding
    public int distanceCost; // Breadth First Search

    public bool isWalkable;
    public bool isValid;
    public PathNode cameFromNode;

    private UnitGridCombat unitGridCombat;

    public PathNode(Grid<PathNode> grid, int x, int y) {
        this.grid = grid;
        this.x = x;
        this.y = y;
        isWalkable = true;
    }

    public void CalculateFCost() {
        fCost = gCost + hCost;
    }

    public void SetIsWalkable(bool isWalkable) {
        this.isWalkable = isWalkable;
        grid.TriggerGridObjectChanged(x, y);
    }

    public void SetIsValid(bool isValid)
    {
        this.isValid = isValid;
        grid.TriggerGridObjectChanged(x, y);
    }

    public void SetUnitGridCombat(UnitGridCombat unitGridCombat)
    {
        this.unitGridCombat = unitGridCombat;
    }

    public void ClearUnitGridCombat()
    {
        SetUnitGridCombat(null);
    }

    public UnitGridCombat GetUnitGridCombat()
    {
        return unitGridCombat;
    }

    public override string ToString() {
        return x + "," + y;
    }

}
