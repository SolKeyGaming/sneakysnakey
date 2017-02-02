using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Extensions
{

    public static class MoveDirectionExtensions
    {

        /// <summary>
        /// Returns the vector of magnitude 1 of this direction
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        public static Vector2 ToVector2(this MoveDirection direction)
        {
            Vector2 resultingVector;

            switch (direction)
            {
                case (MoveDirection.Up):
                    resultingVector = new Vector2(0, 1);
                    break;
                case (MoveDirection.Left):
                    resultingVector = new Vector2(-1, 0);
                    break;
                case (MoveDirection.Down):
                    resultingVector = new Vector2(0, -1);
                    break;
                case (MoveDirection.Right):
                    resultingVector = new Vector2(1, 0);
                    break;
                default:
                    throw new Exception("Bad direction value");
            }

            return resultingVector;
        }

        /// <summary>
        /// Returns the vector of this direction with the specified magnitude
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        public static Vector2 ToVector2(this MoveDirection direction, float magnitude)
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

        /// <summary>
        /// Returns the quaternion equivalent to this direction
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        public static Quaternion ToQuaternion(this MoveDirection direction)
        {
            Quaternion q = new Quaternion();
            int rotation = direction.GetAngle();
            q.eulerAngles = new Vector3(0, 0, rotation);
            return q;
        }

        /// <summary>
        /// Converts the Direction to his number equivalent
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        static int GetAngle(this MoveDirection direction)
        {
            int angle;
            //TODO: esto se repite en BehaviorController, limpiar
            switch (direction)
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

}
