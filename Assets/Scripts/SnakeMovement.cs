using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Extensions;

public class SnakeMovement : MonoBehaviour
{
    /// <summary>
    /// asd
    /// </summary>
    public float Step;
    public float SecondsPerMove;
    public List<Vector2> WayPoints;
    public Rect PlayArea { get { return new Rect(-3, -5, 6, 10); } }

    // para manejar cambios de direccion
    public MoveDirection Direction;
    public MoveDirection PreviousDirection;

    // Elementos
    GameObject _cabeza;
    GameObject _cuerpos;
    GameObject _cola;
    Vector2 _posicionAntCabeza;

    string input;

    // Para visualizacion de waypoints
    //public GameObject WaypointPrefab;

    // Use this for initialization
    void Start()
    {
        _cabeza = GameObject.Find("Cabeza");
        _cuerpos = GameObject.Find("Cuerpos");
        _cola = GameObject.Find("Cola");

        //Posicionar cola segun el primer cuerpo (el mas alejado de la cabeza)
        //_cola.transform.position = ListarCuerpos()[0].position - new Vector3(0,-0.64f,0);

        Direction = MoveDirection.Up;

        // Inicializacion de los waypoints
        WayPoints = new List<Vector2>() { _cabeza.transform.position };

        foreach (Transform t in ListarCuerpos())
        {
            WayPoints.Add(t.position);
        }

        WayPoints.Add(_cola.transform.position);
        WayPoints.Reverse();

        // mostrar waypoints como un cuadrado blanco
        //foreach(Vector2 v in WayPoints)
        //{
        //    Instantiate(WaypointPrefab, v, Quaternion.identity);
        //}

        // Invocacion del Metodo 'Move' cada 'SecondsPerMove' segundos
        InvokeRepeating("Move", 0, SecondsPerMove);

    }

    // Update is called once per frame
    void Update()
    {
        // Si hubo input, se almacena hasta ser consumido
        if (Input.anyKeyDown)
            input = Input.inputString;
    }

    /// <summary>
    /// Cambia la direccion de la serpiente segun el input entregado (WASD)
    /// </summary>
    void HandleInput()
    {
        // Manejar cambio de Direccion
        //TODO: cambiar input a swipes
        PreviousDirection = Direction;

        if (input == "w" && Direction != MoveDirection.Down)
            Direction = MoveDirection.Up;

        if (input == "d" && Direction != MoveDirection.Left)
            Direction = MoveDirection.Right;

        if (input == "s" && Direction != MoveDirection.Up)
            Direction = MoveDirection.Down;

        if (input == "a" && Direction != MoveDirection.Right)
            Direction = MoveDirection.Left;

        if(input == "f")
        {
            RotateByDirection(transform, MoveDirection.Up);
        }

        input = "";
    }


    /// <summary>
    /// Checks if a gameobject can be moved outside of the Rect 'playArea'
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="direction"></param>
    /// <param name="magnitude"></param>
    /// <returns></returns>
    bool CanMove(Transform transform, MoveDirection direction, float magnitude)
    {
        Vector2 v_direction = direction.ToVector2(magnitude);
        Vector2 position = (Vector2)transform.position + v_direction;
        if (position.x > PlayArea.xMax || position.x < PlayArea.xMin
            || position.y > PlayArea.yMax || position.y < PlayArea.yMin)
        {
            return false;
        }
        return true;
    }

    void Move()
    {
        // Manejar input para cambiar la direccion de la serpiente
        // TODO: limpiar y reestructurar la logica de los inputs
        HandleInput();

        // Si el objeto no se puede mover, se intenta con su direccion anterior
        if(!CanMove(_cabeza.transform, Direction, Step))
        {
            // Reestablecer direccion
            Direction = PreviousDirection;
            // Si no se puede mover en la direccion anterior, no se mueve
            if (!CanMove(_cabeza.transform, Direction, Step))
            {
                return;
            }
        }

        // Calculo del vector movimiento de la cabeza
        Vector2 headMovement = Direction.ToVector2(Step);

        // Rotar cabeza
        if (PreviousDirection != Direction)
            RotateByDirection(_cabeza.transform);

        // Mover la cabeza
        MoverObjeto(_cabeza, headMovement);

        // Agregar la nueva posicion de la cabeza a los waypoints
        WayPoints.Add(_cabeza.transform.position);

        // Preparar datos para mover los cuerpos
        int wpIndex = WayPoints.Count - 2; // -1 por el indice empezando de 0 y -2 porque el primero es el nuevo
        List<Transform> listaDeCuerpos = ListarCuerpos();

        // Mover cada cuerpo en _cuerpos
        for (int i = 0; i <= listaDeCuerpos.Count - 1; i++)
        {
            // Obtener datos para calculo de direccion
            Vector2 posicionCuerpo = listaDeCuerpos[i].position;
            Vector2 posicionWayPoint = WayPoints[wpIndex];

            // Obtener el vector direccion
            Vector2 vectorDireccion = new Vector2(posicionWayPoint.x - posicionCuerpo.x, posicionWayPoint.y - posicionCuerpo.y);

            // Obtener script BodyController del cuerpo
            BodyController bodyControl = (BodyController)listaDeCuerpos[i].gameObject.GetComponent(typeof(BodyController));

            // Almacenar proximo waypoint para la rotacion del cuerpo
            bodyControl.NextWayPoint = WayPoints[wpIndex + 1];

            // Mover cuerpo
            bodyControl.Move(vectorDireccion);

            // Decrecer el contador de waypoints
            wpIndex--;
        }
        
        // Mover cola
        Vector2 posicionCola = _cola.transform.position;
        Vector2 posicionWayPointCola = WayPoints[1];       // el waypoint de la cola siempre sera 0, por lo que le sigue el 1

        Vector2 vDireccionCola = new Vector2(posicionWayPointCola.x - posicionCola.x, posicionWayPointCola.y - posicionCola.y);
        TailController tailControl = (TailController)_cola.gameObject.GetComponent(typeof(TailController));
        tailControl.NextWayPoint = WayPoints[2];
        tailControl.Move(vDireccionCola);

        // Eliminar waypoint en el que estaba la cola
        WayPoints.RemoveAt(0);
    }
    void MoverObjeto(GameObject go, Vector2 direccion)
    {
        Vector3 posicion = go.transform.position + (Vector3)direccion;
        go.transform.position = posicion;
    }

    public void SwipeDetected(TKSwipeRecognizer recognizer)
    {
        switch (recognizer.completedSwipeDirection)
        {
            // TODO: cambiar manera en que se almacenan los inputs, quisas una enumeracion de tipos de inputs? 
            case (TKSwipeDirection.Up):
                input = "w";
                break;
            case (TKSwipeDirection.UpRight):
                // La diferencia en x es mayor a la diferencia en y: va hacia la derecha
                if(Math.Abs(recognizer.startPoint.x - recognizer.endPoint.x) > Math.Abs(recognizer.startPoint.y - recognizer.endPoint.y))
                {
                    input = "d";
                }
                // Va hacia arriba
                else
                {
                    input = "w";
                }
                break;
            case (TKSwipeDirection.Right):
                input = "d";
                break;
            case (TKSwipeDirection.DownRight):
                // La diferencia en x es mayor a la diferencia en y: va hacia la derecha
                if (Math.Abs(recognizer.startPoint.x - recognizer.endPoint.x) > Math.Abs(recognizer.startPoint.y - recognizer.endPoint.y))
                {
                    input = "d";
                }
                // Va hacia abajo
                else
                {
                    input = "s";
                }
                break;
            case (TKSwipeDirection.Down):
                input = "s";
                break;
            case (TKSwipeDirection.DownLeft):
                // La diferencia en x es mayor a la diferencia en y: va hacia la izquierda
                if (Math.Abs(recognizer.startPoint.x - recognizer.endPoint.x) > Math.Abs(recognizer.startPoint.y - recognizer.endPoint.y))
                {
                    input = "a";
                }
                // Va hacia abajo
                else
                {
                    input = "s";
                }
                break;
            case (TKSwipeDirection.Left):
                input = "a";
                break;
            case (TKSwipeDirection.UpLeft):
                // La diferencia en x es mayor a la diferencia en y: va hacia la izquierda
                if (Math.Abs(recognizer.startPoint.x - recognizer.endPoint.x) > Math.Abs(recognizer.startPoint.y - recognizer.endPoint.y))
                {
                    input = "a";
                }
                // Va hacia arriba
                else
                {
                    input = "w";
                }
                break;

        }
    }

    /// <summary>
    /// Entrega un vector que representa la magnitud y direccion
    /// del desplazamiento de un cuerpo
    /// </summary>
    /// <param name="magnitude">la magnitud del desplazamiento</param>
    /// <param name="direction">la direccion del desplazamiento</param>
    /// <returns>vector director del movimiento</returns>
    Vector2 DirectionVector(float magnitude, MoveDirection direction)
    {
        Vector2 resultingVector;

        switch (direction)
        {
            case (MoveDirection.Up):
                resultingVector = new Vector2(0, magnitude);
                break;
            case (MoveDirection.Left):
                resultingVector = new Vector2(-magnitude, 0);
                break;
            case (MoveDirection.Down):
                resultingVector = new Vector2(0, -magnitude);
                break;
            case (MoveDirection.Right):
                resultingVector = new Vector2(magnitude, 0);
                break;
            default:
                throw new Exception("Bad direction value");
        }

        return resultingVector;

    }

    void RotateByDirection(Transform t)
    {
        Quaternion q = new Quaternion();
        int rotation = GetAngleByDirection(Direction);

        q.eulerAngles = new Vector3(0, 0, rotation);
        t.rotation = q;
    }
    void RotateByDirection(Transform t, MoveDirection d)
    {
        Quaternion q = new Quaternion();
        int rotation = GetAngleByDirection(d);

        q.eulerAngles = new Vector3(0, 0, rotation);
        t.rotation = q;
    }

    List<Transform> ListarCuerpos()
    {
        List<Transform> lista = new List<Transform>();

        for (int i = 0; i <= _cuerpos.transform.childCount - 1; i++)
        {
            // Si el GameObject es uno activo en la jerarquía de la escena
            if (_cuerpos.transform.GetChild(i).gameObject.activeInHierarchy)
            {
                lista.Add(_cuerpos.transform.GetChild(i));
            }
        }
        return lista;
    }

    //helper
    public int GetAngleByDirection(MoveDirection d)
    {
        int angle;
        //TODO: esto se repite en BehabiourController, limpiar
        switch (Direction)
        {
            case (MoveDirection.Up):
                angle = 0;
                break;
            case (MoveDirection.Right):
                angle = 270;
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

    public MoveDirection GetDirectionByAngle(int angle)
    {
        MoveDirection direction;
        //TODO: esto se repite en BehabiourController, limpiar
        switch (angle)
        {
            case (0):
                direction = MoveDirection.Up;
                break;
            case (270):
                direction = MoveDirection.Right;
                break;
            case (180):
                direction = MoveDirection.Down;
                break;
            case (90):
                direction = MoveDirection.Left;
                break;
            default:
                throw new Exception("MovementController.RotateByDirection(): Bad direction value = " + angle);
        }
        return direction;
    }


    //helper
    void PrintWayPoints()
    {
        string s = "";
        foreach (Vector2 v in WayPoints)
        {
            s += v.ToString() + " ";
        }
    }
}



// Implementar? es mejor que mi algoritmo
//var x = transform.localPosition.x;
//var y = transform.localPosition.y;

//		switch (_currentDirection)
//		{
//			case MoveDirection.Left:
//				x--;
//				break;
//			case MoveDirection.Up:
//				y++;
//				break;
//			case MoveDirection.Right:
//				x++;
//				break;
//			case MoveDirection.Down:
//				y--;
//				break;
//			case MoveDirection.None:
//				throw new ArgumentOutOfRangeException();
//			default:
//				throw new ArgumentOutOfRangeException();
//		}