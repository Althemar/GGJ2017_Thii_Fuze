using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionalAreaController : MonoBehaviour, IDirectionalArea {

    [Header("Configuration")]
    [Range(-1, 1)]
    public float direction;

    public float velocity = 1;

    public bool active = true;

    public enum Direction : int
    {
        NONE    = 0,
        UP      = 1,
        DOWN    = -1
    }

    public Direction targetDirection;

    public float maxTransitionDuration;

    private void Start()
    {
        //direction = Random.Range(-1f, 1f);
        //direction = GetTargetDirection();
        //SetTargetDirection(Mathf.Lerp(-1, 1, Mathf.RoundToInt(Random.value)));
    }

    private void FixedUpdate()
    {
        // Smooth direction
        float targetDirection = GetTargetDirection();
        direction = Mathf.MoveTowards(direction, targetDirection, 2 / maxTransitionDuration * Time.fixedDeltaTime);
    }

    public void SetTargetDirection(float targetDirection)
    {
        this.targetDirection = (Direction)targetDirection;
    }


    public float GetTargetDirection()
    {
        return (float)targetDirection;
    }

    public float GetDirection()
    {
        return direction;
    }

    public float GetVelocity()
    {
        return velocity;
    }

    public bool IsActive()
    {
        return active;
    }
}
