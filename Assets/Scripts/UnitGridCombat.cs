using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitGridCombat : MonoBehaviour
{
    private GameObject selectedGameObject;
    private MovePositionPathfinding movePosition;
    private Pathfinding pathfinding;
    private StatsSystem stats;

    private State state;
    [SerializeField] private Team team;

    public List<ValidNode> validNodes;

    public enum Team
    {
        Player,
        AI
    }

    private enum State
    {
        Normal,
        Moving,
        Attacking
    }

    private void Start()
    {
        movePosition = GetComponent<MovePositionPathfinding>();
        pathfinding = ArenaHandler.Instance.pathfinding;
        //SetSelectedVisible(false);
        state = State.Normal;
    }

    private void Update()
    {
        switch (state)
        {
            case State.Normal:
                break;
            case State.Moving:
                break;
            case State.Attacking:
                break;
        }
    }
    public void MoveTo(Vector3 targetPosition, Action onReachedPosition)
    {
        state = State.Moving;
        movePosition.SetMovePosition(targetPosition + new Vector3(1, 1), () => {
            state = State.Normal;
            onReachedPosition();
        });
    }

    public bool CanMoveTile(PathNode selected) 
    {
        foreach (ValidNode valid in validNodes) {
            if (valid.node.Equals(selected)) return true;
        }
        return false;
    }

    public bool CanAttackUnit(UnitGridCombat unitGridCombat)
    {
        return Vector3.Distance(GetPosition(), unitGridCombat.GetPosition()) < 50f;
    }

    /*
    public void AttackUnit(UnitGridCombat unitGridCombat, Action onAttackComplete)
    {
        state = State.Attacking;

        ShootUnit(unitGridCombat, () => {
            if (!unitGridCombat.IsDead())
            {
                ShootUnit(unitGridCombat, () => {
                    if (!unitGridCombat.IsDead())
                    {
                        ShootUnit(unitGridCombat, () => {
                            if (!unitGridCombat.IsDead())
                            {
                                ShootUnit(unitGridCombat, () => {
                                    state = State.Normal;
                                    onAttackComplete();
                                });
                            }
                            else { state = State.Normal; onAttackComplete(); }
                        });
                    }
                    else { state = State.Normal; onAttackComplete(); }
                });
            }
            else { state = State.Normal; onAttackComplete(); }
        });
    }

    private void ShootUnit(UnitGridCombat unitGridCombat, Action onShootComplete)
    {
        GetComponent<MoveVelocity>().Disable();
        Vector3 attackDir = (unitGridCombat.GetPosition() - transform.position).normalized;
        //UtilsClass.ShakeCamera(.6f, .1f);
        GameHandler_GridCombatSystem.Instance.ScreenShake();

        characterBase.PlayShootAnimation(attackDir, (Vector3 vec) => {
            Shoot_Flash.AddFlash(vec);
            WeaponTracer.Create(vec, unitGridCombat.GetPosition() + UtilsClass.GetRandomDir() * UnityEngine.Random.Range(-2f, 4f));
            unitGridCombat.Damage(this, UnityEngine.Random.Range(4, 12));
        }, () => {
            characterBase.PlayIdleAnim();
            GetComponent<IMoveVelocity>().Enable();

            onShootComplete();
        });
    }

    
    public void Damage(UnitGridCombat attacker, int damageAmount)
    {
        Vector3 bloodDir = (GetPosition() - attacker.GetPosition()).normalized;
        Blood_Handler.SpawnBlood(GetPosition(), bloodDir);

        DamagePopup.Create(GetPosition(), damageAmount, false);
        healthSystem.Damage(damageAmount);
        if (healthSystem.IsDead())
        {
            FlyingBody.Create(GameAssets.i.pfEnemyFlyingBody, GetPosition(), bloodDir);
            Destroy(gameObject);
        }
        else
        {
            // Knockback
            //transform.position += bloodDir * 5f;
        }
    }

    public bool IsDead()
    {
        return healthSystem.IsDead();
    }
    */
    public void UpdateValidity(int range)
    {
        validNodes.Clear();
        List<PathNode> validList = pathfinding.GetValidNodes(transform.position, range);

        foreach (PathNode node in validList) {
            validNodes.Add(new ValidNode(node,node.distanceCost));
        }
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public Team GetTeam()
    {
        return team;
    }

    public bool IsEnemy(UnitGridCombat unitGridCombat)
    {
        return unitGridCombat.GetTeam() != team;
    }

    public int GetDistance(PathNode node) 
    {
        ValidNode v = GetValidNodeFromPath(node);
        return v.distCost;
    }

    public ValidNode GetValidNodeFromPath(PathNode node) 
    {
        foreach (ValidNode valid in validNodes)
        {
            if (valid.node.Equals(node)) return valid;
        }
        return null;
    }

    // Valid node finding
    public class ValidNode 
    {
        public PathNode node;
        public int distCost;

        public ValidNode(PathNode node, int distCost)
        {
            this.node = node;
            this.distCost = distCost;
        }
    }
}