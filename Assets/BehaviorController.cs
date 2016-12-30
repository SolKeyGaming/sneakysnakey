using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviorController : MonoBehaviour
{

    public GameObject BodyPrefab;


    GameObject _bodyGroup;
    GameObject _tail;
    MovementController _movementController;
    Direction _direction { get { return _movementController.Direction; } }
    float _spriteCellDimension = 0.64f;

    // cantidad de unidades de crecimiento de la serpiente
    int _growth;

    // Use this for initialization
    void Start()
    {
        _growth = 0;
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

    void AddBody()
    {
        // posicion y rotacion de la cola
        Vector2 position = _tail.transform.position;
        Quaternion rotation = _tail.transform.rotation;

        // Instanciado del Cuerpo prefabricado donde esta la cola
        Instantiate(BodyPrefab, position, rotation, _bodyGroup.transform);

        // Calculo de la nueva posicion de la cola
        Vector2 newPosition;
        switch (_direction)
        {
            case (Direction.Up):
                newPosition = new Vector2(0, -_spriteCellDimension);
                break;
            case (Direction.Right):
                newPosition = new Vector2(-_spriteCellDimension, 0);
                break;
            case (Direction.Down):
                newPosition = new Vector2(0, _spriteCellDimension);
                break;
            case (Direction.Left):
                newPosition = new Vector2(_spriteCellDimension, 0);
                break;
            default:
                throw new Exception("MovementController.RotateByDirection(): Bad direction value");
        }
        // Cambio de posicion de la cola
        _tail.transform.position += (Vector3)newPosition;

        // se agrega una unidad de crecimiento al parametro
        _growth++;
    }

}
