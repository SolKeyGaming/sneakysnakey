﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BodyController : MonoBehaviour
{

    Vector2 position { get { return transform.position; } }
    Vector2 previousPosition;
    MoveDirection direction;

    // Visualizacion de la direccion
    public string Direction;

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
        Direction = "";
    }

    // Update is called once per frame
    void Update()
    {
    }

    void UpdateDirection()
    {
        // Almacenar direccion anterior
        MoveDirection previousDirection = direction;

        // Calcular nueva direccion
        if (previousPosition.x > position.x)
            direction = MoveDirection.Left;

        else if (previousPosition.x < position.x)
            direction = MoveDirection.Right;

        else if (previousPosition.y > position.y)
            direction = MoveDirection.Down;

        else if (previousPosition.y < position.y)
            direction = MoveDirection.Up;

        // Rotar objeto si ha cambiado de direccion
        if (previousDirection != direction)
            RotateByDirection();

        Direction = direction.ToString();
    }

    void RotateByDirection()
    {
        Quaternion q = new Quaternion();
        int rotation = GetAngleByDirection(direction);
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