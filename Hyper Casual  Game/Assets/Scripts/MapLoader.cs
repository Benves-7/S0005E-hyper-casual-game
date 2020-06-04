using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapLoader : MonoBehaviour
{
    public enum RunMode { proceduralGeneration, tutorial, test }
    public RunMode mode;

    public GameObject startSegment;
    public GameObject testSegment;
    public GameObject tutorialSegment;

    public List<GameObject> loadedSegments;
    public GameObject[] segments;
    private Options options;
    public List<int> sequence;

    public bool stop = false;

    public Movement player;

    public int speed = 5;


    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Movement>();
        try
        {
            options = GameObject.FindGameObjectWithTag("Options").GetComponent<Options>();
            mode = options.mode;
            foreach (int index in options.sequence)
            {
                sequence.Add(index);
            }
        }
        catch (System.Exception)
        {
            print("ERROR: No options detected.");
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        if (options != null)
        {
            if (sequence.Count > 0)
            {
                loadedSegments.Add(Instantiate(startSegment, transform));
                for (int i = 0; i < 10; i++)
                {
                    if (sequence.Count > 0)
                    {
                        float posZ = loadedSegments[loadedSegments.Count - 1].transform.position.z;
                        float size = loadedSegments[loadedSegments.Count - 1].GetComponent<Segment>().sizeOfSegment;
                        loadedSegments.Add(Instantiate(segments[sequence[0]], new Vector3(0, 0, posZ + size), new Quaternion(), transform));
                        sequence.RemoveAt(0);
                    }
                    else
                    {
                        float posZ = loadedSegments[loadedSegments.Count - 1].transform.position.z;
                        float size = loadedSegments[loadedSegments.Count - 1].GetComponent<Segment>().sizeOfSegment;
                        loadedSegments.Add(Instantiate(segments[Random.Range(0, segments.Length)], new Vector3(0, 0, posZ + size), new Quaternion(), transform));
                    }
                }
            }
            else if (mode == RunMode.tutorial)
            {
                loadedSegments.Add(Instantiate(tutorialSegment, transform));
            }
            else if (mode == RunMode.proceduralGeneration)
            {
                options.sequence = new List<int>();
                loadedSegments.Add(Instantiate(startSegment, transform));
                for (int i = 0; i < 10; i++)
                {
                    float posZ = loadedSegments[loadedSegments.Count - 1].transform.position.z;
                    float size = loadedSegments[loadedSegments.Count - 1].GetComponent<Segment>().sizeOfSegment;
                    int index = Random.Range(0, segments.Length);
                    options.sequence.Add(index);
                    loadedSegments.Add(Instantiate(segments[index], new Vector3(0, 0, posZ + size), new Quaternion(), transform));
                }
            }
            else
            {
                mode = RunMode.test;
                loadedSegments.Add(Instantiate(startSegment, transform));
                for (int i = 0; i < 10; i++)
                {
                    float posZ = loadedSegments[loadedSegments.Count - 1].transform.position.z;
                    float size = loadedSegments[loadedSegments.Count - 1].GetComponent<Segment>().sizeOfSegment;
                    loadedSegments.Add(Instantiate(testSegment, new Vector3(0, 0, posZ + size), new Quaternion(), transform));
                }
            }
        }
        else
        {
            mode = RunMode.test;
            loadedSegments.Add(Instantiate(startSegment, transform));
            for (int i = 0; i < 10; i++)
            {
                float posZ = loadedSegments[loadedSegments.Count - 1].transform.position.z;
                float size = loadedSegments[loadedSegments.Count - 1].GetComponent<Segment>().sizeOfSegment;
                loadedSegments.Add(Instantiate(testSegment, new Vector3(0, 0, posZ + size), new Quaternion(), transform));
            }
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (!stop)
        {
            if (mode == RunMode.tutorial)
            {
                // skip.
            }
            else if (mode == RunMode.test)
            {
                if (loadedSegments[0].transform.position.z < -(loadedSegments[0].GetComponent<Segment>().sizeOfSegment + 5f))
                {
                    player.points += loadedSegments[0].GetComponent<Segment>().points;
                    Destroy(loadedSegments[0]);
                    loadedSegments.RemoveAt(0);
                }
                if (loadedSegments[loadedSegments.Count - 1].transform.position.z < 100f)
                {
                    float posZ = loadedSegments[loadedSegments.Count - 1].transform.position.z;
                    float size = loadedSegments[loadedSegments.Count - 1].GetComponent<Segment>().sizeOfSegment;
                    loadedSegments.Add(Instantiate(testSegment, new Vector3(0, 0, posZ + size), new Quaternion(), transform));
                }
            }
            else if (mode == RunMode.proceduralGeneration)
            {
                // Removes segments.
                if (loadedSegments[0].transform.position.z < -(loadedSegments[0].GetComponent<Segment>().sizeOfSegment + 5f))
                {
                    player.points += loadedSegments[0].GetComponent<Segment>().points;
                    Destroy(loadedSegments[0]);
                    loadedSegments.RemoveAt(0);
                }
                // Adds new segments.
                if (loadedSegments[loadedSegments.Count - 1].transform.position.z < 100f)
                {
                    float posZ, size;

                    if (sequence.Count > 0)
                    {
                        posZ = loadedSegments[loadedSegments.Count - 1].transform.position.z;
                        size = loadedSegments[loadedSegments.Count - 1].GetComponent<Segment>().sizeOfSegment;
                        loadedSegments.Add(Instantiate(segments[sequence[0]], new Vector3(0, 0, posZ + size), new Quaternion(), transform));
                        sequence.RemoveAt(0);
                    }
                    else
                    {
                        posZ = loadedSegments[loadedSegments.Count - 1].transform.position.z;
                        size = loadedSegments[loadedSegments.Count - 1].GetComponent<Segment>().sizeOfSegment;
                        int index = Random.Range(0, segments.Length);
                        options.sequence.Add(index);
                        loadedSegments.Add(Instantiate(segments[index], new Vector3(0, 0, posZ + size), new Quaternion(), transform));
                    }
                }
            }
            foreach (var segment in loadedSegments)
            {
                segment.transform.Translate(Vector3.forward * -speed * Time.deltaTime);
            }
        }
        else
        {

        }
    }
}
