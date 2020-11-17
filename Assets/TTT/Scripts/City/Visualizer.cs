using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TTT.Scripts.City
{
    public class Visualizer : MonoBehaviour
{
    public LSystemGenerator lsystem;
        List<Vector3> positions = new List<Vector3>();
        private int length = 10;
        private float angle = 90;
        public RoadHelper roadHelper;
        public StructureHelper structureHelper;
        private bool waitingForTheRoad = false;
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
            roadHelper.finishedCoroutine += () => waitingForTheRoad = false;
            string sequence = lsystem.GenerateSententce();
            StartCoroutine(VisualizeSequence(sequence));
            
        }
        public Vector3 currentPosition = Vector3.zero;
        private IEnumerator VisualizeSequence(string sequence)
        {
            Stack<Parameters> savePoints = new Stack<Parameters>();
            
            Vector3 direction = Vector3.forward; // z axis
            Vector3 tempPostion = Vector3.zero;

            positions.Add(currentPosition);
            foreach (char letter in sequence)
            {
                if (waitingForTheRoad){
                    yield return new WaitForEndOfFrame();
                }
                SimpleVisualizer.EncodingLetters encoding = (SimpleVisualizer.EncodingLetters) letter;
                switch (encoding)
                {
                    case SimpleVisualizer.EncodingLetters.save:
                        savePoints.Push(new Parameters{position = currentPosition, direction = direction, length = Length});
                        break;
                    case SimpleVisualizer.EncodingLetters.load:
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
                    case SimpleVisualizer.EncodingLetters.draw:
                        tempPostion = currentPosition;
                        currentPosition += direction * length;
                        StartCoroutine(roadHelper.PlaceStreetPositions(tempPostion, Vector3Int.RoundToInt(direction), length));
                        waitingForTheRoad = true;
                        yield return new WaitForEndOfFrame();
                        // positions.Add(currentPosition);
                        break;
                    case SimpleVisualizer.EncodingLetters.turnRight:
                        direction = Quaternion.AngleAxis(angle, Vector3.up) * direction; 
                        break;
                    case SimpleVisualizer.EncodingLetters.turnLeft:
                        direction = Quaternion.AngleAxis(-angle, Vector3.up) * direction; 
                        break;
                    default:
                        break;
                }
            }
            yield return new WaitForSeconds(0.1f);
            roadHelper.FixRoad();
            yield return new WaitForSeconds(0.8f);
            StartCoroutine(structureHelper.PlaceStructuresAroundRoad(roadHelper.GetRoadPositions()));
        }
}

}
