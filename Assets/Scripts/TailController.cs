using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Extensions;

public class TailController : MovementManager
{

    //Vector2 position { get { return transform.position; } }
    //Vector2 previousPosition;

    //// Entregado por el MovementController del GameObject 'Serpiente'
    //public Vector2 NextWayPoint;

    //public MoveDirection Direction;
    //public MoveDirection PreviousDirection;

    //public void Move(Vector2 direccion)
    //{
    //    // Almacenar la posicion anterior
    //    previousPosition = position;
    //    // Mover Objeto
    //    transform.position += (Vector3)direccion;
    //    // Actualizar direccion
    //    UpdateDirection();
    //}

    // Use this for initialization
    void Start()
    {
        //previousPosition = position;
    }

    public new void Move(Vector2 d)
    {
        base.Move(d);
        UpdateDirection();

        // Rotar objeto si ha cambiado de direccion
        if (PreviousDirection != Direction)
        {
            transform.rotation = Direction.ToQuaternion();
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}