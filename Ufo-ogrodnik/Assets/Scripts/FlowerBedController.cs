using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerBedController : MonoBehaviour
{

    //public GameObject flowerBed;
    //public GameObject ufo;
    //public float planeDetectionCircleRadious;
    //private List<GameObject> childrenPlanes;
    //private Renderer ufoRenderer;

    // Use this for initialization
    void Start()
    {

        //ufoRenderer = ufo.GetComponent<Renderer>();

        //childrenPlanes = new List<GameObject>();
        //foreach (Transform tran in flowerBed.transform)
        //{
        //    childrenPlanes.Add(tran.gameObject);
        //}

    }

    // Update is called once per frame
    void Update()
    {

        //if (Input.GetKeyDown(KeyCode.X))
        //{
        //foreach (var plane in childrenPlanes)
        //{
        //    if (GetGameObjectsXZDistance(ufo,plane) < planeDetectionCircleRadious)
        //    {
        //        plane.GetComponent<Renderer>().material.shader = Shader.Find("Diffuse");
        //    }
        //    else
        //    {
        //        plane.GetComponent<Renderer>().material.shader = Shader.Find("Standard");
        //    }
        //}
    }

    //private float GetGameObjectsXZDistance(GameObject a, GameObject b)
    //{
    //    float distance = (Mathf.Pow(a.transform.position.x - b.transform.position.x, 2))
    //        + (Mathf.Pow(a.transform.position.z - b.transform.position.z, 2));

    //    return distance;
    //}
}

