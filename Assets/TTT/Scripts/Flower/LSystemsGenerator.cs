using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using System;
using Random = System.Random;

public class LSystemsGenerator : MonoBehaviour
{
    public static int NUM_OF_TREES = 8;
    public static int MAX_ITERATIONS = 7;

    public int title = 1;
    public int iterations = 4;
    public float angle = 30f;
    public float width = 0.02f;
    public float length = 0.03f;
    public float variance = 10f;
    public GameObject Tree = null;

    [SerializeField]
    private GameObject treeParent;

    [SerializeField]
    private GameObject branch;

    [SerializeField]
    private GameObject leaf;
    //[SerializeField] private HUDScript HUD;

    private const string axiom = "X";

    private Dictionary<char, string> rules;
    private Stack<TransformInfo> transformStack;
    private int titleLastFrame;
    private int iterationsLastFrame;
    private float angleLastFrame;
    private float widthLastFrame;
    private float lengthLastFrame;
    private string currentString = string.Empty;
    private Vector3 initialPosition = Vector3.zero;
    private float[] randomRotationValues = new float[100];

    public List<GameObject> listOfGO = new List<GameObject>();
    
    private UnityEngine.Random random;
    public int seed;

    public bool alwaysShow = true;
    private void Start()
    {
        listOfGO = new List<GameObject>();
        random = new UnityEngine.Random();
        UnityEngine.Random.InitState(seed);

        angle = UnityEngine.Random.Range(20f, 40f);

        titleLastFrame = title;
        iterationsLastFrame = iterations;
        angleLastFrame = angle;
        widthLastFrame = width;
        lengthLastFrame = length;

        for (int i = 0; i < randomRotationValues.Length; i++)
        {
            randomRotationValues[i] = UnityEngine.Random.Range(-1f, 1f);
        }

        transformStack = new Stack<TransformInfo>();

        int randInt = UnityEngine.Random.Range(0, 2);
        String rulesStr = "[F[-X+F[+FX]][*-X+F[+FX]][/-X+F[+FX]-X]]";
        if (randInt == 0)
        {
            rulesStr = "[F[-X+F[+FX]][*-X+F[+FX]][/-X+F[+FX]-X]]";
        }
        else if (randInt == 1)
        {
            rulesStr = "[F[+FX][*+FX][/+FX]]";
        }
        else if (randInt == 2)
        {
            rulesStr = "[F*+X]X[+FX][/+F-FX]";
        }

        rules = new Dictionary<char, string>
        {
            {'X', rulesStr},
            {'F', "FF"}
        };

        //351 MS for 2k
        //print(Time.time);
        Generate(1);
        //print(Time.time);
        //StartCoroutine(Generate());
    }


    private void Update()
    {
        // if (iterationsLastFrame != iterations ||
        //         angleLastFrame != angle ||
        //         widthLastFrame != width ||
        //         lengthLastFrame != length)
        // {
        //     ResetFlags();
        //     Generate(1);
        // }
        if (Input.GetKeyUp(KeyCode.W))
        {
            print("W");
            foreach (var go in listOfGO)
            {
                go.SetActive(false);
            }
        }

        if (Input.GetKeyUp(KeyCode.S))
        {
            print("S");
            foreach (var go in listOfGO)
            {
                go.SetActive(true);
            }
        }

        if (Input.GetKeyUp(KeyCode.A))
        {
            //print("Start Time: " + Time.time);
            //print("A");
            StartCoroutine(ShowTree(true));
        }

        if (Input.GetKeyUp(KeyCode.D))
        {
            //print("Start Time: " + Time.time);
            //print("D");
            StartCoroutine(ShowTree(false));
        }
    }

    private bool addedTime = false;
    private float timeElapsed = 0;
    public float lerpDuration = 3f;
    private int generateTimes = 0;
    IEnumerator ShowTree(bool bShow)
    {
        if (alwaysShow)
        {
            yield return 0;
        }
        float t = 0;
        var goalIndex = (int)Mathf.Lerp(0, listOfGO.Count, t);
        
        for (var i = 0; i < listOfGO.Count; i++)
        {
            if (!addedTime)
            {
                timeElapsed += Time.deltaTime;
                addedTime = true;
            }
            
            listOfGO[i].SetActive(bShow);
            
            if (i > goalIndex)
            {
                
                t = timeElapsed / lerpDuration;
                goalIndex = (int)Mathf.Lerp(0, listOfGO.Count, t);
                
                addedTime = false;
//                print("i " + i + " vs " + goalIndex);
                yield return new WaitForSeconds(0.01f);                
            }
        }

        timeElapsed = 0;
        //print("Finish Time: " + Time.time);
    }


    IEnumerator Generate()
    {
        if (iterationsLastFrame != iterations ||
            angleLastFrame != angle ||
            widthLastFrame != width ||
            lengthLastFrame != length)
        {
            ResetFlags();
        }
        else
        {
            yield return null;
        }

        Destroy(Tree);

        Tree = Instantiate(treeParent);

        currentString = axiom;

        print(System.DateTime.Now);
        StringBuilder sb = new StringBuilder();

        for (int i = 0; i < iterations; i++)
        {
            foreach (char c in currentString)
            {
                sb.Append(rules.ContainsKey(c) ? rules[c] : c.ToString());
            }

            currentString = sb.ToString();
            sb = new StringBuilder();
        }
        
        for (int i = 0; i < currentString.Length; i++)
        {
            switch (currentString[i])
            {
                case 'F':
                    initialPosition = transform.position;
                    transform.Translate(Vector3.up * 2 * length);


                    GameObject fLine =
                        currentString[(i + 1) % currentString.Length] == 'X' ||
                        currentString[(i + 3) % currentString.Length] == 'F' &&
                        currentString[(i + 4) % currentString.Length] == 'X'
                            ? Instantiate(leaf)
                            : Instantiate(branch);
                    listOfGO.Add(fLine);
                    fLine.transform.SetParent(Tree.transform);
                    var fLineRenderer = fLine.GetComponent<LineRenderer>();
                    fLineRenderer.SetPosition(0, initialPosition);
                    fLineRenderer.SetPosition(1, transform.position);
                    fLineRenderer.startWidth = width;
                    fLineRenderer.endWidth = width;

                    break;

                case 'X':
                    break;

                case '+':
                    transform.Rotate(Vector3.back * angle *
                                     (1 + variance / 100 * randomRotationValues[i % randomRotationValues.Length]));
                    break;

                case '-':
                    transform.Rotate(Vector3.forward * angle *
                                     (1 + variance / 100 * randomRotationValues[i % randomRotationValues.Length]));
                    break;

                case '*':
                    transform.Rotate(Vector3.up * 120 *
                                     (1 + variance / 100 * randomRotationValues[i % randomRotationValues.Length]));
                    break;

                case '/':
                    transform.Rotate(Vector3.down * 120 *
                                     (1 + variance / 100 * randomRotationValues[i % randomRotationValues.Length]));
                    break;

                case '[':
                    transformStack.Push(new TransformInfo()
                    {
                        position = transform.position,
                        rotation = transform.rotation
                    });
                    break;

                case ']':
                    TransformInfo ti = transformStack.Pop();
                    transform.position = ti.position;
                    transform.rotation = ti.rotation;
                    break;

                default:
                    throw new InvalidOperationException("Invalid L-tree operation");
            }

                yield return new WaitForSeconds(0.01f);
        }

        int randRotate = UnityEngine.Random.Range(0, 359);
        Tree.transform.rotation = Quaternion.Euler(0, randRotate, 0);

        print(listOfGO.Count);
    }

    private void Generate(int h)
    {
        Destroy(Tree);

        Tree = Instantiate(treeParent);

        currentString = axiom;

        StringBuilder sb = new StringBuilder();

        for (int i = 0; i < iterations; i++)
        {
            foreach (char c in currentString)
            {
                sb.Append(rules.ContainsKey(c) ? rules[c] : c.ToString());
            }

            currentString = sb.ToString();
            sb = new StringBuilder();
        }

//        Debug.Log(currentString);

        for (int i = 0; i < currentString.Length; i++)
        {
            switch (currentString[i])
            {
                case 'F':
                    initialPosition = transform.position;
                    transform.Translate(Vector3.up * 2 * length);

                    GameObject fLine =
                        currentString[(i + 1) % currentString.Length] == 'X' ||
                        currentString[(i + 3) % currentString.Length] == 'F' &&
                        currentString[(i + 4) % currentString.Length] == 'X'
                            ? Instantiate(leaf)
                            : Instantiate(branch);
                    fLine.transform.SetParent(Tree.transform);
                    listOfGO.Add(fLine);
                    var fLineRenderer = fLine.GetComponent<LineRenderer>();
                    fLineRenderer.SetPosition(0, initialPosition);
                    fLineRenderer.SetPosition(1, transform.position);
                    fLineRenderer.startWidth = width;
                    fLineRenderer.endWidth = width;

                    break;

                case 'X':
                    break;

                case '+':
                    transform.Rotate(Vector3.back * angle *
                                     (1 + variance / 100 * randomRotationValues[i % randomRotationValues.Length]));
                    break;

                case '-':
                    transform.Rotate(Vector3.forward * angle *
                                     (1 + variance / 100 * randomRotationValues[i % randomRotationValues.Length]));
                    break;

                case '*':
                    transform.Rotate(Vector3.up * 120 *
                                     (1 + variance / 100 * randomRotationValues[i % randomRotationValues.Length]));
                    break;

                case '/':
                    transform.Rotate(Vector3.down * 120 *
                                     (1 + variance / 100 * randomRotationValues[i % randomRotationValues.Length]));
                    break;

                case '[':
                    transformStack.Push(new TransformInfo()
                    {
                        position = transform.position,
                        rotation = transform.rotation
                    });
                    break;

                case ']':
                    TransformInfo ti = transformStack.Pop();
                    transform.position = ti.position;
                    transform.rotation = ti.rotation;
                    break;

                default:
                    throw new InvalidOperationException("Invalid L-tree operation");
            }
        }

        //
        // int randRotate = UnityEngine.Random.Range(0, 359);
        // print(randRotate);
        // Tree.transform.rotation = Quaternion.Euler(0, randRotate, 0);
    }

    private void ResetRandomValues()
    {
        for (int i = 0; i < randomRotationValues.Length; i++)
        {
            randomRotationValues[i] = UnityEngine.Random.Range(-1f, 1f);
        }
    }

    private void ResetFlags()
    {
        iterationsLastFrame = iterations;
        angleLastFrame = angle;
        widthLastFrame = width;
        lengthLastFrame = length;
    }

    private void ResetTreeValues()
    {
        iterations = 5;
        angle = 30f;
        width = 0.02f;
        length = 0.03f;
        variance = 10f;
    }

    IEnumerator WaitFunction(int i)
    {
        yield return new WaitForSeconds(0.01f);
        GameObject fLine =
            currentString[(i + 1) % currentString.Length] == 'X' ||
            currentString[(i + 3) % currentString.Length] == 'F' && currentString[(i + 4) % currentString.Length] == 'X'
                ? Instantiate(leaf)
                : Instantiate(branch);
        fLine.transform.SetParent(Tree.transform);
        fLine.GetComponent<LineRenderer>().SetPosition(0, initialPosition);
        fLine.GetComponent<LineRenderer>().SetPosition(1, transform.position);
        fLine.GetComponent<LineRenderer>().startWidth = width;
        fLine.GetComponent<LineRenderer>().endWidth = width;
    }
}