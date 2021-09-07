using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Kuhpik {
    public static class MathExtensions {
        public const double EPS = 1E-7;
        public static Vector3 nullVector = new Vector3(1234f, 1234f, 1234f);
        
        private static LineXZ cachedLine1 = new LineXZ(Vector3.zero, Vector3.zero);
        private static LineXZ cachedLine2 = new LineXZ(Vector3.zero, Vector3.zero);

        public class LineXZ {
            public float A, B, C;
            public float y;
            public LineXZ(Vector3 firstPoint, Vector3 secondPoint) {
                UpdateParams(firstPoint, secondPoint);
            }

            public LineXZ UpdateParams(Vector3 firstPoint, Vector3 secondPoint) {
                y = firstPoint.y;
                A = (firstPoint.z - secondPoint.z);
                B = (secondPoint.x - firstPoint.x);
                C = (firstPoint.x * secondPoint.z - secondPoint.x * firstPoint.z);
                return this;
            }

            public override string ToString() {
                return $"{A}x + {B}y + {C} = 0";
            }
        }

        public static Vector3 IntersectTwoLinesXZ(LineXZ line1, LineXZ line2) {
            if ((line1.A * line2.B - line2.A * line1.B) == 0) {
                return nullVector;
            }
            
            Vector3 result = Vector3.up * line1.y;

            result.x = - (line1.C * line2.B - line2.C * line1.B) / (line1.A * line2.B - line2.A * line1.B);
            result.z = - (line1.A * line2.C - line2.A * line1.C) / (line1.A * line2.B - line2.A * line1.B);

            return result;
        }
        
        private static Vector3 upVec = Vector3.up;

        private static bool CheckIfPointBelongsSegment(Vector3 startPoint, Vector3 endPoint, Vector3 pointForCheck) {
            //float angle = Vector3.SignedAngle(startPoint - pointForCheck, endPoint - pointForCheck, upVec);
            float dst1 = Vector3.Distance(startPoint, pointForCheck);
            float dst2 = Vector3.Distance(endPoint, pointForCheck);
            
            if (Mathf.Abs(Vector3.Distance(startPoint, endPoint) - (dst1 + dst2)) < EPS) {
            }

            return Mathf.Abs(Vector3.Distance(startPoint, endPoint) - (dst1 + dst2)) < EPS;
        }

        public static Vector3 IntersectTwoSegmentsXZ(Vector3 s1, Vector3 s2, Vector3 e1, Vector3 e2) {
            Vector3 intersectionPoint =
                IntersectTwoLinesXZ(cachedLine1.UpdateParams(s1, s2), cachedLine2.UpdateParams(e1, e2));

            if (intersectionPoint != nullVector) {
                if (CheckIfPointBelongsSegment(s1, s2, intersectionPoint) &&
                    CheckIfPointBelongsSegment(e1, e2, intersectionPoint)) {
                    return intersectionPoint;
                }
            }

            return nullVector;
        }

        public static Vector3 GetMiddlePoint(List<Vector3> points) {
            Vector3 middlePoint = points[0];
            for (int i = 1; i < points.Count; ++i) {
                middlePoint += points[i];
            }

            middlePoint /= points.Count;
            
            return middlePoint;
        }

        public static void ShaffleList<T>(ref List<T> list) { 
            list.Sort((a, b)=> 1 - 2 * Random.Range(0, 1)); 
        }
        
        public static float AreaOfTriangleByVertices(Vector3 v1, Vector3 v2, Vector3 v3) {
            float a = Vector3.Distance(v1, v2);
            float b = Vector3.Distance(v2, v3);
            float c = Vector3.Distance(v3, v1);
            float s = (a + b + c) / 2;
            return (float)Math.Sqrt(s * (s-a) * (s-b) * (s-c));
        }
    }
}