using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectInsideShape : MonoBehaviour
{

    /*
     * 
     * Find if a point is inside a shape
     * Modified to work with Unity, source from:
     * https://www.geeksforgeeks.org/how-to-check-if-a-given-point-lies-inside-a-polygon/
     * 
     */


    public struct Point
    {
        public float x, y;
    };

    public struct line
    {
        public Point p1, p2;
    };

    static bool onLine(line l1, Point p)
    {
        // Check whether p is on the line or not
        if (p.x <= Mathf.Max(l1.p1.x, l1.p2.x)
            && p.x <= Mathf.Min(l1.p1.x, l1.p2.x)
            && (p.y <= Mathf.Max(l1.p1.y, l1.p2.y)
                && p.y <= Mathf.Min(l1.p1.y, l1.p2.y)))
            return true;

        return false;
    }

    static int direction(Point a, Point b, Point c)
    {
        float val = (b.y - a.y) * (c.x - b.x)
                  - (b.x - a.x) * (c.y - b.y);

        if (val == 0)

            // Colinear
            return 0;

        else if (val < 0)

            // Anti-clockwise direction
            return 2;

        // Clockwise direction
        return 1;
    }

    static bool isIntersect(line l1, line l2)
    {
        // Four direction for two lines and points of other line
        int dir1 = direction(l1.p1, l1.p2, l2.p1);
        int dir2 = direction(l1.p1, l1.p2, l2.p2);
        int dir3 = direction(l2.p1, l2.p2, l1.p1);
        int dir4 = direction(l2.p1, l2.p2, l1.p2);

        // When intersecting
        if (dir1 != dir2 && dir3 != dir4)
            return true;

        // When p2 of line2 are on the line1
        if (dir1 == 0 && onLine(l1, l2.p1))
            return true;

        // When p1 of line2 are on the line1
        if (dir2 == 0 && onLine(l1, l2.p2))
            return true;

        // When p2 of line1 are on the line2
        if (dir3 == 0 && onLine(l2, l1.p1))
            return true;

        // When p1 of line1 are on the line2
        if (dir4 == 0 && onLine(l2, l1.p2))
            return true;

        return false;
    }

    public static Point vecToPoint(Vector3 p)
    {
        Point ret = new Point();
        ret.x = p.x;
        ret.y = p.z;
        return ret;
    }

    public static bool checkInside(Vector3[] poly, int n, Vector3 p)
    {

        // When polygon has less than 3 edge, it is not polygon
        if (n < 3)
            return false;

        // Create a point at infinity, y is same as point p
        line exline = new line();
        exline.p1 = vecToPoint(p);
        exline.p2 = new Point();
        exline.p2.x = 9999;
        exline.p2.y = p.y;
        int count = 0;
        int i = 0;
        do
        {

            // ForMathf.Ming a line from two consecutive points of
            // poly
            //line side = { poly[i], poly[(i + 1) % n] };
            line side = new line();
            side.p1 = vecToPoint(poly[i]);
            side.p2 = vecToPoint(poly[(i + 1) % n]);
            if (isIntersect(side, exline))
            {

                // If side is intersects exline
                if (direction(side.p1, vecToPoint(p), side.p2) == 0)
                    return onLine(side, vecToPoint(p));
                count++;
            }
            i = (i + 1) % n;
        } while (i != 0);

        // When count is odd
        return (count % 2 != 0);
    }

    // Driver code
    
}
