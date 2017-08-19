using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerBedController : MonoBehaviour {

    public GameObject flowerBed;
    public GameObject ufo;
    private List<GameObject> childrenPlanes;
    private Renderer ufoRenderer;

    // Use this for initialization
    void Start () {

        ufoRenderer = ufo.GetComponent<Renderer>();

        childrenPlanes = new List<GameObject>();
        foreach (Transform tran in flowerBed.transform)
        {
            childrenPlanes.Add(tran.gameObject);
        }

        //ufo.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {

        //if (Input.GetKeyDown(KeyCode.X))
        //{
            foreach (var plane in childrenPlanes)
            {
                if ((Mathf.Pow(ufo.transform.position.x - plane.transform.position.x, 2)) + (Mathf.Pow(ufo.transform.position.z - plane.transform.position.z, 2)) < 2f)
                {
                    plane.GetComponent<Renderer>().material.shader = Shader.Find("Diffuse");
                }
            else
            {
                plane.GetComponent<Renderer>().material.shader = Shader.Find("Standard");
            }
                
                
            }

            //ufoRenderer.material.shader = Shader.Find("Self-Illumin/Outlined Diffuse");
        //}
        
            //foreach (var plane in childrenPlanes) 
            //{
            //    // plane.GetComponent<Renderer>().material.shader = Shader.Find("Self - Illumin / Outlined Diffuse");
            //    plane.GetComponent<Renderer>().material.shader = Shader.Find("Standard");
            //}
                    
        }




       
    }    

