using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Extensions;

public class MovementManager : MonoBehaviour
{

    Vector2 position { get { return transform.position; } }

    public Vector2 NextWayPoint;

    public MoveDirection Direction;
    public MoveDirection PreviousDirection;


    public void Move(Vector2 direction)
    {
        // Mover Objeto
        transform.position += (Vector3)direction;
    }

    public void UpdateDirection()
    {
        // Almacenar direccion anterior
        PreviousDirection = Direction;

        // Calcular proxima direccion
        if (position.x > NextWayPoint.x)
        {
            Direction = MoveDirection.Left;
        }
        else if (position.x < NextWayPoint.x)
            Direction = MoveDirection.Right;

        else if (position.y > NextWayPoint.y)
            Direction = MoveDirection.Down;

        else if (position.y < NextWayPoint.y)
            Direction = MoveDirection.Up;

    }

}
