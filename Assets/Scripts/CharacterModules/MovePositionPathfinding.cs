using System;
using System.Collections.Generic;
using UnityEngine;

public class MovePositionPathfinding : MonoBehaviour
{
    private Action onReachedTargetPosition;
    private List<Vector3> pathVectorList;
    private int pathIndex = -1;

    public void SetMovePosition(Vector3 movePosition, Action onReachedTargetPosition) {
        this.onReachedTargetPosition = onReachedTargetPosition;
        pathVectorList = Pathfinding.Instance.FindPath(transform.position, movePosition);

        if (pathVectorList.Count > 0)
        {
            pathIndex = 0;
        }
        else
        {
            pathIndex = -1;
        }
    }

    private void Update() {
        if (pathIndex != -1) {
            // Move to next path pos
            Vector3 nextPathPosition = pathVectorList[pathIndex];
            Vector3 moveVelocity = (nextPathPosition - transform.position).normalized;
            GetComponent<MoveVelocity>().SetVelocity(moveVelocity);

            float reachedPathPositionDistance = 1f;
            if (Vector3.Distance(transform.position, nextPathPosition) < reachedPathPositionDistance)
            {
                pathIndex++;
                if (pathIndex >= pathVectorList.Count)
                {
                    // End of path
                    pathIndex = -1;
                    onReachedTargetPosition();
                }
            }
        }
        else {
            // Idle
            GetComponent<MoveVelocity>().SetVelocity(Vector3.zero);
        
        }
    }
}
