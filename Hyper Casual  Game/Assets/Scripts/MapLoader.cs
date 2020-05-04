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
    public GameObject lastSpawned;


    public void Stop()
    {
        foreach (var segment in GetComponentsInChildren<Segment>())
        {
            segment.stop = true;
        }
        foreach (var movingPlatform in GetComponentsInChildren<MovingPlatform>())
        {
            movingPlatform.stop = true;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        if (mode == RunMode.tutorial)
        {
            loadedSegments.Add(Instantiate(tutorialSegment, transform));
        }
        else if (mode == RunMode.test)
        {
            loadedSegments.Add(Instantiate(startSegment, transform));
            for (int i = 0; i < 10; i++)
            {
                float posZ = loadedSegments[loadedSegments.Count - 1].transform.position.z;
                float size = loadedSegments[loadedSegments.Count - 1].GetComponent<Segment>().sizeOfSegment;
                loadedSegments.Add(Instantiate(testSegment, new Vector3(0, 0, posZ + size), new Quaternion(), transform));
            }
        }
        else if (mode == RunMode.proceduralGeneration)
        {
            loadedSegments.Add(Instantiate(startSegment, transform));
            for (int i = 0; i < 10; i++)
            {
                float posZ = loadedSegments[loadedSegments.Count - 1].transform.position.z;
                float size = loadedSegments[loadedSegments.Count - 1].GetComponent<Segment>().sizeOfSegment;
                loadedSegments.Add(Instantiate(segments[Random.Range(0, segments.Length)], new Vector3(0, 0, posZ + size), new Quaternion(), transform));
            }
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (mode == RunMode.tutorial)
        {
            // skip.
        }
        else if (mode == RunMode.test)
        {
            if (loadedSegments[0].transform.position.z < -(loadedSegments[0].GetComponent<Segment>().sizeOfSegment + 5f))
            {
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
            if (loadedSegments[0].transform.position.z < -(loadedSegments[0].GetComponent<Segment>().sizeOfSegment + 5f))
            {
                Destroy(loadedSegments[0]);
                loadedSegments.RemoveAt(0);
            }
            if (loadedSegments[loadedSegments.Count - 1].transform.position.z < 100f)
            {
                float posZ = loadedSegments[loadedSegments.Count - 1].transform.position.z;
                float size = loadedSegments[loadedSegments.Count - 1].GetComponent<Segment>().sizeOfSegment;
                loadedSegments.Add(Instantiate(segments[Random.Range(0, segments.Length)], new Vector3(0, 0, posZ + size), new Quaternion(), transform));
            }
        }
    }
}
