using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float ufoSpeed;

    public GameObject drop;
    public Text winText;

    private Rigidbody rigidBodyPlayer;
    private List<GameObject> drops;

    public GameObject flowerBed;
    public float planeDetectionCircleRadious;
    private List<GameObject> childrenPlanes;
    private List<GameObject> plants;

    private Renderer ufoRenderer;
    private GameObject currentPlane;
    private Queue<GameObject> planesToGreenUp;
    private Queue<GameObject> planesToGreenDown;
    private float scale;
    private float growthSpeed;
    Color dryColor;
    Color greenColor;

    void Start()
    {
        winText.text = "";

        rigidBodyPlayer = GetComponent<Rigidbody>();
        rigidBodyPlayer.useGravity = false;
        drops = new List<GameObject>();

        currentPlane = null;
        planesToGreenUp = new Queue<GameObject>();
        planesToGreenDown = new Queue<GameObject>();

        childrenPlanes = new List<GameObject>();
        plants = new List<GameObject>();
        foreach (Transform tran in flowerBed.transform)
        {
            childrenPlanes.Add(tran.gameObject);

            var plant = tran.Find("plant");
            if (plant != null)
            {
                plants.Add(plant.gameObject);
            }
        }
            

        scale = 0.1f;
        growthSpeed = 0.0001f;

        greenColor = new Color(0.2f, 0.3f, 0.1f);

        dryColor = childrenPlanes[0].GetComponent<Renderer>().material.color;
    }

    void FixedUpdate()
    {
        AddForce();
        Rotate();
        HandleSpecialKeys();
        UpdateShader();
        ResizePlants();      
        DropCollider();
      
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
                plane.GetComponent<Renderer>().material.shader = Shader.Find("Standard");
            }
        }
    }

    private void ResizePlants()
    {     
        foreach (var plant in plants)
        {
            var plant1FlowerBedColor = plant.transform.parent.gameObject.GetComponent<Renderer>().material.color;

            var localScaleX = plant.transform.localScale.x;
            float newLocalScale = 0f;

            if (plant1FlowerBedColor.Equals(greenColor) && localScaleX < 0.25f)
            {
                newLocalScale = localScaleX + growthSpeed;                             
            }
            else if (localScaleX > 0.01f)
            {
                newLocalScale = localScaleX - growthSpeed;            }

            plant.transform.localScale = new Vector3(newLocalScale, newLocalScale, newLocalScale);
        }

        if (plants.TrueForAll(p => p.transform.localScale.x > 0.22f))
        {
            winText.text = "Victory!";
        }        
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

            foreach (var plane in childrenPlanes)
            {
                planesToGreenUp.Enqueue(plane);
                Invoke("GreenUpPlane", 1.3f);//this will happen after x seconds   
            }            
        }
    }

    private void DropCollider()
    {
        if (this.drops.Count > 0)
        {
            for (int i = this.drops.Count; i != 0; i--)
            {
                if (this.drops[i - 1].transform.position.y < 0)
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
    }
}
