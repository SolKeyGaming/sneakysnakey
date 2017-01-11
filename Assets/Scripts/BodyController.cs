using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BodyController : MonoBehaviour
{

    Vector2 position { get { return transform.position; } }
    Vector2 previousPosition;
    MoveDirection direction;
    MoveDirection previousDirection;
    SpriteRenderer sprites;

    // Visualizacion de la direccion
    public string Direction;
    public string PreviousDirection { get { return previousDirection.ToString(); } }

    // Entregado por el MovementController del GameObject 'Serpiente'
    public Vector2 NextWayPoint;

    // Sprites TODO: externalizar
    public Sprite StraightBodySprite;
    public Sprite CornerBodySprite;

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
        sprites = GetComponent<SpriteRenderer>();
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
        previousDirection = direction;

        // Calcular proxima direccion
        if (position.x > NextWayPoint.x)
            direction = MoveDirection.Left;

        else if (position.x < NextWayPoint.x)
            direction = MoveDirection.Right;

        else if (position.y > NextWayPoint.y)
            direction = MoveDirection.Down;

        else if (position.y < NextWayPoint.y)
            direction = MoveDirection.Up;

        // Rotar objeto si ha cambiado de direccion
        if (previousDirection != direction)
        {
            ChangeToCornerSprite();
        }
        else
        {
            RotateByDirection(direction);
            ChangeToStraightSprite();
        }

        Direction = direction.ToString();
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

    void ChangeToCornerSprite()
    {
        // Cambiar sprite
        sprites.sprite = CornerBodySprite;
        // Restaurar rotacion
        transform.rotation = new Quaternion(0, 0, 0, 0);

        // Si va hacia arriba y a la izquierda, o hacia la derecha y abajo
        if ((previousDirection == MoveDirection.Up && direction == MoveDirection.Left)
            || (previousDirection == MoveDirection.Right && direction == MoveDirection.Down))
        {
            sprites.flipX = false;
            sprites.flipY = true;
        }
        // Si va hacia arriba y a la derecha, o hacia la izquierda y abajo
        else if ((previousDirection == MoveDirection.Up && direction == MoveDirection.Right)
            || (previousDirection == MoveDirection.Left && direction == MoveDirection.Down))
        {
            sprites.flipY = true;
            sprites.flipX = true;
        }
        // Si va hacia abajo y a la derecha, o hacia la izquierda y arriba
        else if ((previousDirection == MoveDirection.Down && direction == MoveDirection.Right)
            || (previousDirection == MoveDirection.Left && direction == MoveDirection.Up))
        {
            sprites.flipX = true;
            sprites.flipY = false;
        }
        // Si va hacia abajo y a la izquierda, o hacia la derecha y arriba
        else if ((previousDirection == MoveDirection.Down && direction == MoveDirection.Left)
            || (previousDirection == MoveDirection.Right && direction == MoveDirection.Up))
        {
            sprites.flipX = false;
            sprites.flipY = false;
        }

    }

    void ChangeToStraightSprite()
    {
        if (sprites.sprite != StraightBodySprite)
            sprites.sprite = StraightBodySprite;
        sprites.flipX = false;
        sprites.flipY = false;
    }
}