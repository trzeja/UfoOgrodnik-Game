using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float ufoSpeed;

    public GameObject drop;

    public GameObject plant1;
    public GameObject plant2;
    public GameObject plant3;

    private Rigidbody rigidBodyPlayer;
    private List<GameObject> drops;

    public GameObject flowerBed;
    public GameObject ufo; // to mosz
    public float planeDetectionCircleRadious;
    private List<GameObject> childrenPlanes;
    private Renderer ufoRenderer;
    private GameObject currentPlane;
    private Queue<GameObject> planesToGreenUp;
    private Queue<GameObject> planesToGreenDown;
    //private GameObject pouredPlane;
    private float scale;
    private float growthSpeed;
    Color dryColor;
    Color greenColor;
    Color wetterColor;

    void Start()
    {
        rigidBodyPlayer = GetComponent<Rigidbody>();
        rigidBodyPlayer.useGravity = false;
        drops = new List<GameObject>();
        //flowerBeds = new List<GameObject>();

        currentPlane = null;
        planesToGreenUp = new Queue<GameObject>();
        planesToGreenDown = new Queue<GameObject>();
        //pouredPlane = null;

        //ufoRenderer = this.gameObject.GetComponent<Renderer>();
        childrenPlanes = new List<GameObject>();
        foreach (Transform tran in flowerBed.transform)
        {
            childrenPlanes.Add(tran.gameObject);
        }

        scale = 0.1f;
        //growthSpeed = 0.0001f;
        growthSpeed = 0.001f;

        greenColor = new Color(0.2f, 0.3f, 0.1f);
        wetterColor = new Color(0.2f, 0.2f, 0.2f);

        dryColor = childrenPlanes[0].GetComponent<Renderer>().material.color;
        //dryColor = new Color(0.393f, 0.137f, 0.041f);
    }

    void FixedUpdate()
    {
        AddForce();
        Rotate();
        HandleSpecialKeys();
        UpdateShader();
        ResizePlants();

        if (this.drops.Count > 0)
        {
            this.DropCollider();
        }
    }

    private void AddForce()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        this.rigidBodyPlayer.AddForce(movement * ufoSpeed);
    }

    private void Rotate()
    {
        transform.Rotate(new Vector3(0, 0, 200) * Time.deltaTime);
    }

    private void UpdateShader()
    {
        foreach (var plane in childrenPlanes)
        {
            if (GetGameObjectsXZDistance(this.gameObject, plane) < planeDetectionCircleRadious)
            {
                plane.GetComponent<Renderer>().material.shader = Shader.Find("Diffuse");
                currentPlane = plane;
            }
            else
            {
                //if (plane.GetComponent<Renderer>() != null)
                //{
                    plane.GetComponent<Renderer>().material.shader = Shader.Find("Standard");
                //}
            }
        }
    }

    private void ResizePlants()
    {
        scale += growthSpeed;

        if (scale < 0.25f)
        {          
            plant1.transform.localScale = new Vector3(scale, scale, scale);
        }

        if (scale < 0.75f)
        {
            plant2.transform.localScale = new Vector3(scale / 3f, scale / 3f, scale / 3f);
        }

        var pulse = Mathf.Abs(Mathf.Sin(scale *3 )) / 5f;
        plant3.transform.localScale = new Vector3(pulse, pulse, pulse);
    }

    private float GetGameObjectsXZDistance(GameObject a, GameObject b)
    {
        float distance = (Mathf.Pow(a.transform.position.x - b.transform.position.x, 2))
            + (Mathf.Pow(a.transform.position.z - b.transform.position.z, 2));

        return distance;
    }

    private void HandleSpecialKeys()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            for (int i = 0; i < 50; i++)
            {
                Vector3 offset = new Vector3(Random.Range(-5, 5) / 10f, Random.Range(-5, 5) / 10f, Random.Range(-5, 5) / 10f);

                var ObjectSpawnPosition = this.transform.position + offset;
                var newDrop = Instantiate(drop, ObjectSpawnPosition, Quaternion.identity);
                this.drops.Add(newDrop);
            }

            if (currentPlane != null)
            {
                planesToGreenUp.Enqueue(currentPlane);
                Invoke("GreenUpPlane", 1.3f);//this will happen after x seconds   

            }

            //if (pouredPlane == null)
            //{
            //    pouredPlane = currentPlane;

            //}

        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            for (int i = 0; i < 500; i++)
            {
                Vector3 offset = new Vector3(Random.Range(-10, 10), Random.Range(-10, 10), Random.Range(-10, 10));

                var ObjectSpawnPosition = this.transform.position + offset;
                var newDrop = Instantiate(drop, ObjectSpawnPosition, Quaternion.identity);
                this.drops.Add(newDrop);
            }
        }
    }

    private void DropCollider()
    {
        for (int i = this.drops.Count; i != 0; i--)
        {
            if (this.drops[i - 1].transform.position.y /* - flowerBed.transform.position.y*/ < 0)
            {
                var tempDropHandle = this.drops[i - 1];
                this.drops.Remove(this.drops[i - 1]);
                if (tempDropHandle != null)
                {
                    Destroy(tempDropHandle);
                }
            }
        }
    }

    private void GreenUpPlane()
    {
        var plane = planesToGreenUp.Dequeue();
        var planeMaterial = plane.GetComponent<Renderer>().material;
        var prevColor = planeMaterial.color;

        if (prevColor.Equals(dryColor))
        {
            planeMaterial.SetColor("_Color", greenColor);
        }
        //else if (prevColor.Equals(wetColor))
        //{
        //    planeMaterial.SetColor("_Color", wetterColor);
        //}

        planesToGreenDown.Enqueue(plane);
        Invoke("GreenDownPlane", 30f);//this will happen after x seconds         
    }

    private void GreenDownPlane()
    {
        var plane = planesToGreenDown.Dequeue();
        var planeMaterial = plane.GetComponent<Renderer>().material;
        var prevColor = planeMaterial.color;

        if (prevColor.Equals(greenColor))
        {
            planeMaterial.SetColor("_Color", dryColor);            
        }
        //else if (prevColor.Equals(wetterColor))
        //{
        //    planeMaterial.SetColor("_Color", wetColor);            
        //}

        //planesToGreenDown.Enqueue(plane);
        //Invoke("GreenDownPlane", 5f);//this will happen after x seconds 
        
    }
}
