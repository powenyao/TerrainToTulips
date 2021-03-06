﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

namespace TTT.Scripts.City
{
    public class RoadHelper : MonoBehaviour
    {
        public Action finishedCoroutine;
        public GameObject roadStraight, roadCorner, road3way, road4way, roadEnd;
        Dictionary<Vector3Int, GameObject> roadDictionary = new Dictionary<Vector3Int, GameObject>();
        HashSet<Vector3Int> fixRoadCandidates = new HashSet<Vector3Int>();

        public float delay = 0.01f;
        public List<Vector3Int> GetRoadPositions()
        {
            return roadDictionary.Keys.ToList();
        }
        public IEnumerator PlaceStreetPositions(Vector3 startPosition, Vector3Int direction, int length)
        {
            Quaternion rotation = Quaternion.identity;
            if (direction.x == 0)
            {
                rotation = Quaternion.Euler(0, 90, 0);
            }

            for (int i = 0; i < length; i++)
            {
                Vector3Int position = Vector3Int.RoundToInt(startPosition + direction * i);
                if (roadDictionary.ContainsKey(position))
                {
                    continue;
                }
                GameObject road = Instantiate(roadStraight, position, rotation, transform);
                roadDictionary.Add(position, road);
                if (i == 0 || i==length -1)
                {
                    fixRoadCandidates.Add(position);
                }
                yield return new WaitForSeconds(delay);
            }
            finishedCoroutine?.Invoke();
        }

        public void FixRoad()
        {
            foreach (Vector3Int position in fixRoadCandidates)
            {
                List<Direction> neighborDirections = PlacementHelper.FindNeighbor(position, roadDictionary.Keys);
                Quaternion rotation = Quaternion.identity;

                if (neighborDirections.Count == 1)
                {
                    Destroy(roadDictionary[position]);
                    if (neighborDirections.Contains(Direction.Down))
                    {
                        rotation = Quaternion.Euler(0, 90, 0);
                    } else if (neighborDirections.Contains(Direction.Left))
                    {
                        rotation = Quaternion.Euler(0, 180, 0);
                    }else if (neighborDirections.Contains(Direction.Up))
                    {
                        rotation = Quaternion.Euler(0, -90, 0);
                    }

                    roadDictionary[position] = Instantiate(roadEnd, position, rotation, transform);
                }else if (neighborDirections.Count == 2)
                {
                    if (neighborDirections.Contains(Direction.Up) && neighborDirections.Contains(Direction.Down) ||
                        neighborDirections.Contains(Direction.Right) && neighborDirections.Contains(Direction.Left))
                    {
                        continue;
                    }
                    Destroy(roadDictionary[position]);
                    if (neighborDirections.Contains(Direction.Up) && neighborDirections.Contains(Direction.Right))
                    {
                        rotation = Quaternion.Euler(0, 90, 0);
                    } else if (neighborDirections.Contains(Direction.Right) && neighborDirections.Contains(Direction.Down))
                    {
                        rotation = Quaternion.Euler(0, 180, 0);
                    }else if (neighborDirections.Contains(Direction.Down) && neighborDirections.Contains(Direction.Left))
                    {
                        rotation = Quaternion.Euler(0, -90, 0);
                    }
                    roadDictionary[position] = Instantiate(roadCorner, position, rotation, transform);
                }
                
                else if (neighborDirections.Count == 3)
                {
                    Destroy(roadDictionary[position]);
                    if (neighborDirections.Contains(Direction.Right) 
                        && neighborDirections.Contains(Direction.Down)
                        && neighborDirections.Contains(Direction.Left))
                    {
                        rotation = Quaternion.Euler(0, 90, 0);
                    } else if (neighborDirections.Contains(Direction.Down) 
                               && neighborDirections.Contains(Direction.Left)
                               && neighborDirections.Contains(Direction.Up))
                    {
                        rotation = Quaternion.Euler(0, 180, 0);
                    }else if (neighborDirections.Contains(Direction.Left) 
                              && neighborDirections.Contains(Direction.Up)
                              && neighborDirections.Contains(Direction.Right))
                    {
                        rotation = Quaternion.Euler(0, -90, 0);
                    }
                    roadDictionary[position] = Instantiate(road3way, position, rotation, transform);
                }
                else
                {
                    Destroy((roadDictionary[position]));
                    roadDictionary[position] = Instantiate(road4way, position, rotation, transform);
                }
            }
        }
    }

}
