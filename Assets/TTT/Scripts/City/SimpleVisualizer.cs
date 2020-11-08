using System;
using System.Collections;
using System.Collections.Generic;
using TTT.Scripts.City;
using UnityEngine;

namespace TTT.Scripts.City
{
    
    public class SimpleVisualizer : MonoBehaviour
    {
        public LSystemGenerator lsystem;
        List<Vector3> positions = new List<Vector3>();
        public GameObject prefab;
        public Material lineMaterial;
        private int length = 8;
        private float angle = 90;

        public int Length
        {
            get {
                if (length > 0)
                {
                    return length;
                }

                return 1;
            }
            set => length = value;
        }

        private void Start()
        {
            string sequence = lsystem.GenerateSententce();
            VisualizeSequence(sequence);
        }

        private void VisualizeSequence(string sequence)
        {
            Stack<Parameters> savePoints = new Stack<Parameters>();
            Vector3 currentPosition = Vector3.zero;
            Vector3 direction = Vector3.forward; // z axis
            Vector3 tempPostion = Vector3.zero;

            positions.Add(currentPosition);
            foreach (char letter in sequence)
            {
                EncodingLetters encoding = (EncodingLetters) letter;
                switch (encoding)
                {
                    case EncodingLetters.unknown:
                        
                    case EncodingLetters.save:
                        savePoints.Push(new Parameters{position = currentPosition, direction = direction, length = Length});
                        break;
                    case EncodingLetters.load:
                        if (savePoints.Count > 0)
                        {
                            Parameters parameters = savePoints.Pop();
                            currentPosition = parameters.position;
                            direction = parameters.direction;
                            Length = parameters.length;
                        }
                        else
                        {
                            throw new System.Exception("Don't have saved point in our stack");
                        }
                        break;
                    case EncodingLetters.draw:
                        tempPostion = currentPosition;
                        currentPosition += direction * length;
                        DrawLine(tempPostion, currentPosition, Color.red);
                        Length -= 2;
                        positions.Add(currentPosition);
                        break;
                    case EncodingLetters.turnRight:
                        direction = Quaternion.AngleAxis(angle, Vector3.up) * direction; 
                        break;
                    case EncodingLetters.turnLeft:
                        direction = Quaternion.AngleAxis(-angle, Vector3.up) * direction; 
                        break;
                    default:
                        break;
                }
            }

            foreach (Vector3 position in positions)
            {
                Instantiate(prefab, position, Quaternion.identity);
            }
        }

        private void DrawLine(Vector3 start, Vector3 end, Color color)
        {
            GameObject line = new GameObject("line");
            line.transform.position = start;
            LineRenderer lineRenderer = line.AddComponent<LineRenderer>();
            lineRenderer.material = lineMaterial;
            lineRenderer.startColor = color;
            lineRenderer.startWidth = 0.1f;
            lineRenderer.endWidth = 0.1f;
            lineRenderer.SetPosition(0, start);
            lineRenderer.SetPosition(1, end);
        }

        public enum EncodingLetters
        {
            unknown = '1',
            save = '[',
            load= ']',
            draw = 'F',
            turnRight ='+',
            turnLeft = '-'
        }
    }

}