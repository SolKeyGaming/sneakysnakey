using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TailController : MonoBehaviour
{

    Vector2 position { get { return transform.position; } }
    Vector2 previousPosition;

    // Entregado por el MovementController del GameObject 'Serpiente'
    public Vector2 NextWayPoint;
    
    public MoveDirection Direction;
    public MoveDirection PreviousDirection;

    public void Move(Vector2 direccion)
    {
        // Almacenar la posicion anterior
        previousPosition = position;
        // Mover Objeto
        transform.position += (Vector3)direccion;
        // Actualizar direccion
        UpdateDirection();
    }

    // Use this for initialization
    void Start()
    {
        previousPosition = position;
    }

    // Update is called once per frame
    void Update()
    {
    }

    void UpdateDirection()
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

        // Rotar objeto si ha cambiado de direccion
        if (PreviousDirection != Direction)
        {
            RotateByDirection(Direction);
        }
    }

    void RotateByDirection(MoveDirection d)
    {
        Quaternion q = new Quaternion();
        int rotation = GetAngleByDirection(d);
        q.eulerAngles = new Vector3(0, 0, rotation);
        transform.rotation = q;
    }

    public int GetAngleByDirection(MoveDirection d)
    {
        int angle;
        //TODO: esto se repite en BehaviorController, limpiar
        switch (d)
        {
            case (MoveDirection.Up):
                angle = 0;
                break;
            case (MoveDirection.Right):
                angle = -90;
                break;
            case (MoveDirection.Down):
                angle = 180;
                break;
            case (MoveDirection.Left):
                angle = 90;
                break;
            default:
                throw new Exception("MovementController.RotateByDirection(): Bad direction value");
        }
        return angle;
    }
}