using System;

namespace ROK
{
    public struct Pos2
    {
        public int x;

        public int y;

        public Pos2(int _x, int _y)
        {
            this.x = _x;
            this.y = _y;
        }

        public static bool operator ==(Pos2 p1, Pos2 p2)
        {
            return p1.x == p2.x && p1.y == p2.y;
        }

        public static bool operator !=(Pos2 p1, Pos2 p2)
        {
            return p1.x != p2.x || p1.y != p2.y;
        }

        public static Pos2 operator +(Pos2 p1, Pos2 p2)
        {
            return new Pos2(p1.x + p2.x, p1.y + p2.y);
        }

        public static Pos2 operator -(Pos2 p1, Pos2 p2)
        {
            return new Pos2(p1.x - p2.x, p1.y - p2.y);
        }

        public static Pos2 operator /(Pos2 p1, int t)
        {
            return new Pos2(p1.x / t, p1.y / t);
        }

        public static Pos2 operator *(Pos2 p1, int t)
        {
            return new Pos2(p1.x * t, p1.y * t);
        }
    }
}