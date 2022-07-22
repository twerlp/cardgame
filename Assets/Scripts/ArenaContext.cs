using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaContext : MonoBehaviour
{
    GridCombatSystem gcs;

    private void OnEnable()
    {
        gcs = FindObjectOfType<GridCombatSystem>();
    }

    public void AttackButton() 
    {
        ArenaHandler.Instance.selectState = ArenaHandler.SelectState.Attack;
        ArenaHandler.Instance.DestroyContextMenu();
    }
    public void MoveButton()
    {
        ArenaHandler.Instance.selectState = ArenaHandler.SelectState.Movement;
        ArenaHandler.Instance.DestroyContextMenu();
    }
    public void AOEButton()
    {
        ArenaHandler.Instance.selectState = ArenaHandler.SelectState.AOE;
        ArenaHandler.Instance.DestroyContextMenu();
    }
    public void HealButton()
    {
        ArenaHandler.Instance.selectState = ArenaHandler.SelectState.Health;
        ArenaHandler.Instance.DestroyContextMenu();
    }
}
