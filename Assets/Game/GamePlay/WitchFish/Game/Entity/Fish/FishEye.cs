using System;
using Game;
using UnityEngine;

namespace WitchFish
{
    public class FishEye : MonoBehaviour
    {
        private void Update()
        {
            Vector2 mousePosition = Input.mousePosition;
            Vector3 mouse3D = Global.cameraSystem.mainCamera.ScreenToWorldPoint(mousePosition);
            Vector2 direction = (mouse3D - transform.position);
            float angel = Vector2.Angle(direction, Vector2.up);
            if (mouse3D.x > transform.position.x)
            {
                angel = - angel;
            }
            transform.eulerAngles = new Vector3(0, 0, angel);
        }
        //
        // int BetweenLineAndCircle(Vector2 circleCenter, float circleRadius, Vector2 point1, Vector2 point2,
        //     out Vector2 intersection1, out Vector2 intersection2)
        // {
        //     float t;
        //
        //     var dx = point2.x - point1.x;
        //     var dy = point2.y - point1.y;
        //
        //     var a = dx * dx + dy * dy;
        //     var b = 2 * (dx * (point1.x - circleCenter.x) + dy * (point1.y - circleCenter.y));
        //     var c = (point1.x - circleCenter.x) * (point1.x - circleCenter.x) +
        //         (point1.y - circleCenter.y) * (point1.y - circleCenter.y) - circleRadius * circleRadius;
        //
        //     var determinate = b * b - 4 * a * c;
        //     if ((a <= 0.0000001) || (determinate < -0.0000001))
        //     {
        //         //没有交点的情况
        //         intersection1 = Vector2.zero;
        //         intersection2 = Vector2.zero;
        //         return 0;
        //     }
        //
        //     if (determinate < 0.0000001 && determinate > -0.0000001)
        //     {
        //         //一个交点的情况
        //         t = -b / (2 * a);
        //         intersection1 = new Vector2(point1.x + t * dx, point1.y + t * dy);
        //         intersection2 = Vector2.zero;
        //         return 1;
        //     }
        //
        //     //两个交点的情况
        //     t = ((-b + Mathf.Sqrt(determinate)) / (2 * a));
        //     intersection1 = new Vector2(point1.x + t * dx, point1.y + t * dy);
        //     t = ((-b - Mathf.Sqrt(determinate)) / (2 * a));
        //     intersection2 = new Vector2(point1.x + t * dx, point1.y + t * dy);
        //
        //     return 2;
        // }
    }
}