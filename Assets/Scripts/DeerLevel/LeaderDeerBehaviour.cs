using System.Collections.Generic;
using UnityEngine;

public class LeaderDeerBehaviour : TargetBasedSteerBehaviour
{
    [SerializeField] private float targetReachedSquaredDistance = 4f;
    private List<GameObject> targets = new List<GameObject>();
    private int currentTarget = 0;
    private Animator animator;
    private float speedDecrement = 0f;

    public void DecreseSpeed(float decrement)
    {
        speedDecrement += decrement;
    }

    protected override void Start()
    {
        base.Start();
        animator = GetComponentInChildren<Animator>();
    }

    void OnEnable()
    {
        animator = GetComponentInChildren<Animator>();
    }

    public void StartMove(List<GameObject> waypoints)
    {
        targets.AddRange(waypoints);
    }

    protected override float GetSpeed()
    {
        return speed - speedDecrement;
    }

    protected override bool ShouldJump()
    {
        return false;
    }

    protected override void FixedUpdate()
    {
        if (currentTarget == targets.Count)
        {
            targetPosition = transform.position;

        }
        else
        {
            targetPosition = targets[currentTarget].transform.position;
            if (Utils.DistanceToTargetWithinThreshold(transform.position, targets[currentTarget].transform.position, targetReachedSquaredDistance))
            {
                currentTarget++;
                if (currentTarget == targets.Count)
                {
                    EventManager.Instance.InvokeDeerLevelEvent(new GameObject[] { }, EventManager.DeerLevelEvent.ArriveAtDestination);
                }
            }
        }
        base.FixedUpdate();
        if (steerDirection != Vector3.zero)
        {
            animator.SetBool(Constants.AnimatorState.IsRunning, true);
        }
        else
        {
            animator.SetBool(Constants.AnimatorState.IsRunning, false);
        }
    }
}
