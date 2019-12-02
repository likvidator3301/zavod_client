using System.Collections.Generic;
using UnityEngine;

namespace Systems
{
    internal static class UnitsPlacementHelpert
    {
        public static List<Vector3> PlaceUnits(Vector3 startPoint, Vector3 endPoint, int unitsCount, int maxUnitsInRow, float unitWidth)
        {
            var placedUnits = new List<Vector3>();

            for (var i = 0; i < Mathf.Ceil((float)unitsCount / maxUnitsInRow); i++)
            {
                var pointOfRowCenter = endPoint + (startPoint - endPoint).normalized * unitWidth * 1.2f * i;
                placedUnits.AddRange(
                    CreateAndRotateRow(pointOfRowCenter, getPerpendicularVector(startPoint - endPoint), maxUnitsInRow, unitWidth));
            }

            return placedUnits;
        }

        private static Vector3 getPerpendicularVector(Vector3 vec) => new Vector3(vec.z, vec.y, -vec.x);

        private static List<Vector3> CreateAndRotateRow(Vector3 centerPoint, Vector3 parralelRowVector, int maxUnitsInRow, float unitWitdh)
        {
            var unitsRow = new List<Vector3> { centerPoint };

            for (var i = 1; i <= Mathf.Floor(maxUnitsInRow / 2f); i++)
            {
                unitsRow.Add(centerPoint + -i * parralelRowVector.normalized * unitWitdh * 1.2f);
                unitsRow.Add(centerPoint + i * parralelRowVector.normalized * unitWitdh * 1.2f);
            }

            return unitsRow;
        }
    }
}
