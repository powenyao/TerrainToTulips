using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using System;

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
    public float cooldownTime = 0.01f;
    public GameObject Tree = null;

    [SerializeField] private GameObject treeParent;
    [SerializeField] private GameObject branch;
    [SerializeField] private GameObject leaf;
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
    private List<GameObject> gameObjects;

    
    private void Start()
    {
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
            { 'X', rulesStr },
            { 'F', "FF" }
        };

        //Generate(1);
        StartCoroutine(Generate());
    }



    private void Update()
    {
        if (iterationsLastFrame != iterations ||
                angleLastFrame != angle ||
                widthLastFrame != width ||
                lengthLastFrame != length)
        {
            ResetFlags();
            //Generate(1);
            StartCoroutine(Generate());
        }
    }

   

    IEnumerator Generate()
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

        //Debug.Log(currentString);
        
        
        for (int i = 0; i < currentString.Length; i++)
        {
            switch (currentString[i])
            {
                case 'F':                    
                    initialPosition = transform.position;
                    transform.Translate(Vector3.up * 2 * length);

                    /*
                    float currTime = Time.time;
                    float prevTime = currTime;
                    while (currTime < prevTime + cooldownTime)
                    {
                        currTime = Time.time;
                    }
                    */
             
                    GameObject fLine = currentString[(i + 1) % currentString.Length] == 'X' || currentString[(i + 3) % currentString.Length] == 'F' && currentString[(i + 4) % currentString.Length] == 'X' ? Instantiate(leaf) : Instantiate(branch);
                    fLine.transform.SetParent(Tree.transform);
                    fLine.GetComponent<LineRenderer>().SetPosition(0, initialPosition);
                    fLine.GetComponent<LineRenderer>().SetPosition(1, transform.position);
                    fLine.GetComponent<LineRenderer>().startWidth = width;
                    fLine.GetComponent<LineRenderer>().endWidth = width;
                    yield return new WaitForSeconds(0.01f);
                    //StartCoroutine(WaitFunction(i));

                    break;

                case 'X':                
                    break;

                case '+':
                    transform.Rotate(Vector3.back * angle * (1 + variance / 100 * randomRotationValues[i % randomRotationValues.Length]));
                    break;

                case '-':                                      
                    transform.Rotate(Vector3.forward * angle * (1 + variance / 100 * randomRotationValues[i % randomRotationValues.Length]));
                    break;

                case '*':
                    transform.Rotate(Vector3.up * 120 * (1 + variance / 100 * randomRotationValues[i % randomRotationValues.Length]));
                    break;

                case '/':
                    transform.Rotate(Vector3.down* 120 * (1 + variance / 100 * randomRotationValues[i % randomRotationValues.Length]));
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

     

        int randRotate = UnityEngine.Random.Range(0, 359);
        Tree.transform.rotation = Quaternion.Euler(0, randRotate, 0);

       
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

        //Debug.Log(currentString);


        for (int i = 0; i < currentString.Length; i++)
        {
            switch (currentString[i])
            {
                case 'F':
                    initialPosition = transform.position;
                    transform.Translate(Vector3.up * 2 * length);

                    /*
                    float currTime = Time.time;
                    float prevTime = currTime;
                    while (currTime < prevTime + cooldownTime)
                    {
                        currTime = Time.time;
                    }
                    */

                    GameObject fLine = currentString[(i + 1) % currentString.Length] == 'X' || currentString[(i + 3) % currentString.Length] == 'F' && currentString[(i + 4) % currentString.Length] == 'X' ? Instantiate(leaf) : Instantiate(branch);
                    fLine.transform.SetParent(Tree.transform);
                    fLine.GetComponent<LineRenderer>().SetPosition(0, initialPosition);
                    fLine.GetComponent<LineRenderer>().SetPosition(1, transform.position);
                    fLine.GetComponent<LineRenderer>().startWidth = width;
                    fLine.GetComponent<LineRenderer>().endWidth = width;
                    //yield return new WaitForSeconds(0.01f);
                    //StartCoroutine(WaitFunction(i));

                    break;

                case 'X':
                    break;

                case '+':
                    transform.Rotate(Vector3.back * angle * (1 + variance / 100 * randomRotationValues[i % randomRotationValues.Length]));
                    break;

                case '-':
                    transform.Rotate(Vector3.forward * angle * (1 + variance / 100 * randomRotationValues[i % randomRotationValues.Length]));
                    break;

                case '*':
                    transform.Rotate(Vector3.up * 120 * (1 + variance / 100 * randomRotationValues[i % randomRotationValues.Length]));
                    break;

                case '/':
                    transform.Rotate(Vector3.down * 120 * (1 + variance / 100 * randomRotationValues[i % randomRotationValues.Length]));
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



        int randRotate = UnityEngine.Random.Range(0, 359);
        Tree.transform.rotation = Quaternion.Euler(0, randRotate, 0);


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
        GameObject fLine = currentString[(i + 1) % currentString.Length] == 'X' || currentString[(i + 3) % currentString.Length] == 'F' && currentString[(i + 4) % currentString.Length] == 'X' ? Instantiate(leaf) : Instantiate(branch);
        fLine.transform.SetParent(Tree.transform);
        fLine.GetComponent<LineRenderer>().SetPosition(0, initialPosition);
        fLine.GetComponent<LineRenderer>().SetPosition(1, transform.position);
        fLine.GetComponent<LineRenderer>().startWidth = width;
        fLine.GetComponent<LineRenderer>().endWidth = width;
    }
}