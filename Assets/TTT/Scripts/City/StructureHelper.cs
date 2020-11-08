using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TTT.Scripts.City
{
    public class StructureHelper : MonoBehaviour
    {
        public GameObject prefab;
        public Dictionary<Vector3Int, GameObject> structureDisctionary = new Dictionary<Vector3Int, GameObject>();

        public void PlaceStructuresAroundRoad(List<Vector3Int> roadPositions)
        {
            Dictionary<Vector3Int, Direction> freeEstateSpots = FindSpacesAroundRoad(roadPositions);
            foreach (KeyValuePair<Vector3Int, Direction> freeSpot in freeEstateSpots)
            {
                Quaternion rotation = Quaternion.identity;
                switch (freeSpot.Value)
                {
                    case Direction.Up:
                        rotation = Quaternion.Euler(0, 90,0);
                        break;
                    case Direction.Down:
                        rotation = Quaternion.Euler(0, -90,0);
                        break;
                    case Direction.Right:
                        rotation = Quaternion.Euler(0, -90,0);
                        break;
                    default:
                        break;
                }
                Instantiate(prefab, freeSpot.Key, rotation, transform);
            }
        }

        private Dictionary<Vector3Int, Direction> FindSpacesAroundRoad(List<Vector3Int> roadPositions)
        {
            Dictionary<Vector3Int, Direction> freeSpaces = new Dictionary<Vector3Int, Direction>();
            foreach (Vector3Int position in roadPositions)
            {
                List<Direction> neighborDirections = PlacementHelper.FindNeighbor(position, roadPositions);
                foreach (Direction direction in Enum.GetValues(typeof(Direction)))
                {
                    if (neighborDirections.Contains(direction) == false)
                    {
                        Vector3Int newPosition = position + PlacementHelper.GetOffsetFromDirection(direction);
                        if (freeSpaces.ContainsKey(newPosition))
                        {
                            continue;
                        }

                        freeSpaces.Add(newPosition, PlacementHelper.GetReverseDirection(direction));
                    }
                }
            }

            return freeSpaces;
        }
    }
}
