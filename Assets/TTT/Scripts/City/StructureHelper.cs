using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TTT.Scripts.City
{
    public class StructureHelper : MonoBehaviour
    {
        public BuildingType[] buildingTypes;
        public GameObject[] naturePrefabs;
        public bool randomNaturePlacement = false;
        [Range(0,1)]
        public float randomNaturePlacementTreshold = 0.3f;
        public GameObject prefab;
        public Dictionary<Vector3Int, GameObject> structureDisctionary = new Dictionary<Vector3Int, GameObject>();
        public Dictionary<Vector3Int, GameObject> natureDictionary = new Dictionary<Vector3Int, GameObject>();
        public int seedValue = 9;
        public IEnumerator PlaceStructuresAroundRoad(List<Vector3Int> roadPositions)
        {    
            Dictionary<Vector3Int, Direction> freeEstateSpots = FindSpacesAroundRoad(roadPositions);
            List<Vector3Int> blockedPositions = new List<Vector3Int>();
            
            foreach (KeyValuePair<Vector3Int, Direction> freeSpot in freeEstateSpots)
            {
                if (blockedPositions.Contains(freeSpot.Key))
                {
                    continue;
                }
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
                        rotation = Quaternion.Euler(0, 180,0);
                        break;
                    default:
                        break;
                }

                for (int i = 0; i < buildingTypes.Length; i++)
                {
                    if (buildingTypes[i].quantity == -1) // place infinitely many 
                    {
                        if (randomNaturePlacement)
                        {
                            float random = UnityEngine.Random.value;
                            if (random < randomNaturePlacementTreshold)
                            {
                                GameObject nature =
                                    SpawnPrefab(naturePrefabs[UnityEngine.Random.Range(0, naturePrefabs.Length)],
                                        freeSpot.Key, rotation);
                                natureDictionary.Add(freeSpot.Key, nature);
                                break;
                            }

                        }
                        var building = SpawnPrefab(buildingTypes[i].GetPrefab(), freeSpot.Key, rotation);
                        structureDisctionary.Add(freeSpot.Key, building);
                        break;
                    }

                    if (buildingTypes[i].IsBuildingAvailable())
                    {
                        if (buildingTypes[i].sizeRequired > 1)
                        {
                            int halfSize = Mathf.FloorToInt(buildingTypes[i].sizeRequired / 2.0f);
                            List<Vector3Int> tempPositionsBlocked = new List<Vector3Int>();
                            if (VerifyIfBuildingFits(halfSize, freeEstateSpots, freeSpot, blockedPositions, ref tempPositionsBlocked))
                            {
                                blockedPositions.AddRange((tempPositionsBlocked));
                                GameObject building = SpawnPrefab(buildingTypes[i].GetPrefab(), freeSpot.Key, rotation);
                                structureDisctionary.Add(freeSpot.Key, building);
                                foreach (var pos in tempPositionsBlocked)
                                {
                                    if (!structureDisctionary.ContainsKey(pos))
                                    {
                                        structureDisctionary.Add(pos, building);
                                    }
                                }
                            }
                        }
                        else
                        {
                            GameObject building = SpawnPrefab(buildingTypes[i].GetPrefab(), freeSpot.Key, rotation);
                            structureDisctionary.Add(freeSpot.Key, building);
                        }

                        break;
                    }
                }
                yield return new WaitForSeconds(0.01f);
            }
        }

        private bool VerifyIfBuildingFits(int halfSize, 
            Dictionary<Vector3Int, Direction> freeEstateSpots, 
            KeyValuePair<Vector3Int, Direction> freeSpot, 
            List<Vector3Int> blockedPositions,
            ref List<Vector3Int> tempPositionsBlocked)
        {
            Vector3Int direction = Vector3Int.zero;
            if (freeSpot.Value == Direction.Down || freeSpot.Value == Direction.Up )
            {
                direction = Vector3Int.right;
            }
            else
            {
                direction = new Vector3Int(0, 0 , 1);
            }

            for (int i = 1; i <= halfSize; i++)
            {
                Vector3Int pos1 = freeSpot.Key + direction * i;
                Vector3Int pos2 = freeSpot.Key - direction * i;
                if (!freeEstateSpots.ContainsKey(pos1) || !freeEstateSpots.ContainsKey(pos2) ||
                    blockedPositions.Contains(pos1) || blockedPositions.Contains(pos2))
                {
                    return false;
                }
                tempPositionsBlocked.Add(pos1);
                tempPositionsBlocked.Add(pos2);
            }

            return true;
        }

        private GameObject SpawnPrefab(GameObject prefab, Vector3Int position, Quaternion rotation)
        {
            return Instantiate(prefab, position, rotation, transform);
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
