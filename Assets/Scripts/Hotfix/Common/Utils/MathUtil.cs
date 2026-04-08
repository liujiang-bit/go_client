using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public struct MapUIRect
    {
        public float x1;
        public float y1;
        public float x2;
        public float y2;
        public MapUIRect(float x1, float y1, float x2, float y2)
        {
            this.x1 = x1;
            this.y1 = y1;
            this.x2 = x2;
            this.y2 = y2;
        }
    }
    public class MathUtil
    {
        public static Vector2 FindClosestPoint(float x1, float y1, float x2, float y2)
        {
            Vector2 result = Vector2.zero;
            if (x2 == 0.5f && y2 == 0.5f)
            {
                result = new Vector2(x1, y1);
            }
            else
            {
                float d1 = (float)((x1 - 0.5) * (x1 - 0.5) + (y1 - 0.5) * (y1 - 0.5));
                float d2 = (float)((x2 - 0.5) * (x2 - 0.5) + (y2 - 0.5) * (y2 - 0.5));

                result = d2 < d1 ? new Vector2(x2, y2) : new Vector2(x1, y1);
            }
            return result;
        }
        public static List<Vector2> RectIntersectSegment(MapUIRect rect, Vector2 p1, Vector2 p2)
        {
            Vector2 LD = new Vector2(rect.x1, rect.y1);
            Vector2 LT = new Vector2(rect.x1, rect.y2);
            Vector2 RD = new Vector2(rect.x2, rect.y1);
            Vector2 RT = new Vector2(rect.x2, rect.y2);
            List<Vector2> points = new List<Vector2>();
            List<Vector2> returnPoints = new List<Vector2>();
            if (RectContainsPoint(rect, p1))
            {
                points = GetIntersectPoint(p1, p2, LT, LD, RT, RD);
                if (points.Count <= 0)
                {
                    returnPoints.Add(p1);
                    returnPoints.Add(p2);
                    return returnPoints;
                }
                else
                {
                    returnPoints.Add(points[0]);
                    returnPoints.Add(p1);
                    return returnPoints;
                }
            }
            if (RectContainsPoint(rect, p2))
            {
                points = GetIntersectPoint(p1, p2, LT, LD, RT, RD);
                if (points.Count <= 0)
                {
                    returnPoints.Add(p1);
                    returnPoints.Add(p2);
                    return returnPoints;
                }
                else
                {
                    returnPoints.Add(points[0]);
                    returnPoints.Add(p2);
                    return returnPoints;
                }
            }

            points = GetIntersectPoint(p1, p2, LT, LD, RT, RD);
            int np = points.Count;
            if (np <= 0)
            {
                return returnPoints;
            }
            else if (np == 1)
            {
                returnPoints.Add(points[0]);
                returnPoints.Add(points[0]);
                return returnPoints;
            }
            else
            {
                returnPoints.Add(points[0]);
                returnPoints.Add(points[1]);
                return returnPoints;
            }
        }
        /// <summary>
        /// 获取相交点
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="LT"></param>
        /// <param name="LD"></param>
        /// <param name="RT"></param>
        /// <param name="RD"></param>
        /// <returns></returns>
        public static List<Vector2> GetIntersectPoint(Vector2 p1, Vector2 p2, Vector2 LT, Vector2 LD, Vector2 RT, Vector2 RD)
        {
            Vector2 IntersectPoint = Vector2.zero;
            List<Vector2> points = new List<Vector2>();
            bool pa = SegmentsIntersect(p1, p2, LT, LD, out IntersectPoint);
            if (pa)
            {
                points.Add(IntersectPoint);
            }
            bool pb = SegmentsIntersect(p1, p2, LT, RT, out IntersectPoint);
            if (pb)
            {
                points.Add(IntersectPoint);
            }
            bool pc = SegmentsIntersect(p1, p2, RT, RD, out IntersectPoint);
            if (pc)
            {
                points.Add(IntersectPoint);
            }
            bool pd = SegmentsIntersect(p1, p2, LD, RD, out IntersectPoint);
            if (pd)
            {
                points.Add(IntersectPoint);
            }
            return points;
        }
        /// <summary>
        /// 线段的相交
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <param name="d"></param>
        /// <param name="交点"></param>
        /// <returns> 是否相交</returns>
        public static bool SegmentsIntersect(Vector2 a, Vector2 b, Vector2 c, Vector2 d, out Vector2 intersectPoint)
        {
            intersectPoint = Vector2.zero;
            float area_abc = (a.x - c.x) * (b.y - c.y) - (a.y - c.y) * (b.x - c.x);
            float area_abd = (a.x - d.x) * (b.y - d.y) - (a.y - d.y) * (b.x - d.x);
            if (area_abc * area_abd > 0)
            {
                return false;
            }
            float area_cda = (c.x - a.x) * (d.y - a.y) - (c.y - a.y) * (d.x - a.x);
            float area_cdb = area_cda + area_abc - area_abd;
            if (area_cda * area_cdb > 0)
            {
                return false;
            }
            float t = area_cda / (area_abd - area_abc);
            float dx = t * (b.x - a.x);
            float dy = t * (b.y - a.y);
            intersectPoint = new Vector2(a.x + dx, a.y + dy);
            return true;
        }

        public static bool RectContainsPoint(MapUIRect rect, Vector2 point)
        {
            if (rect.x1 <= point.x && point.x <= rect.x2 && rect.y1 <= point.y && point.y <= rect.y2)
            {
                return true;
            }
            return false;
        }
    }
}


