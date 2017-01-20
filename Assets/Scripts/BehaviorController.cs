using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BehaviorController : MonoBehaviour
{

    public GameObject BodyPrefab;

    GameObject _bodyGroup;
    GameObject _tail;
    MovementController _movementController;
    MoveDirection _direction { get { return _movementController.Direction; } }
    float _spriteCellDimension = 0.64f;

    // Use this for initialization
    void Start()
    {
        _bodyGroup = GameObject.Find("Cuerpos");
        _tail = GameObject.Find("Cola");
        _movementController = GetComponent<MovementController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            AddBody();
    }

    public void AddBody()
    {
        // posicion y rotacion de la cola
        Vector2 position = _tail.transform.position;
        Quaternion rotation = _tail.transform.rotation;

        TailController tailController = _tail.GetComponent<TailController>();

        // TODO: el nuevo cuerpo viene con las direcciones correctas, pro no actualiza su rotacion acorde, sino en el prox MovementController.Move()

        // Instanciado del Cuerpo prefabricado donde esta la cola
        var body = Instantiate(BodyPrefab, position, Quaternion.identity, _bodyGroup.transform);

        BodyController bodyControl = body.GetComponent<BodyController>();

        // Direccion del nuevo cuerpo
        bodyControl.PreviousDirection = tailController.Direction;
        bodyControl.Direction = (_bodyGroup.transform.GetChild(
            _bodyGroup.transform.childCount - 1).GetComponent<BodyController>()).PreviousDirection;

        //bodyControl.HandleDirection();

        // Calculo de la nueva posicion de la cola
        MoveDirection tailDirection = _movementController.GetDirectionByAngle((int)rotation.eulerAngles.z);
        Vector2 newPosition;
        switch (tailDirection)
        {
            case (MoveDirection.Up):
                newPosition = new Vector2(0, -_spriteCellDimension);
                break;
            case (MoveDirection.Right):
                newPosition = new Vector2(-_spriteCellDimension, 0);
                break;
            case (MoveDirection.Down):
                newPosition = new Vector2(0, _spriteCellDimension);
                break;
            case (MoveDirection.Left):
                newPosition = new Vector2(_spriteCellDimension, 0);
                break;
            default:
                throw new Exception("MovementController.RotateByDirection(): Bad direction value");
        }
        // Cambio de posicion de la cola
        _tail.transform.position += (Vector3)newPosition;

        // Creacion del nuevo waypoint en la nueva posicion de la cola
        _movementController.WayPoints.Insert(0, newPosition);

        // TODO: quisas alterar una variable de puntaje o interna al agregar un espacio
    }
    

}
