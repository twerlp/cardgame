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
        Destroy(this);
    }
    public void MoveButton()
    {
        ArenaHandler.Instance.selectState = ArenaHandler.SelectState.Movement;
        Destroy(this);
    }
    public void AOEButton()
    {
        ArenaHandler.Instance.selectState = ArenaHandler.SelectState.AOE;
        Destroy(this);
    }
    public void HealButton()
    {
        ArenaHandler.Instance.selectState = ArenaHandler.SelectState.Health;
        Destroy(this);
    }
}
