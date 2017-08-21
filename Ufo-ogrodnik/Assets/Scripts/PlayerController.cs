using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public GameObject drop;
    public GameObject plant;
    private Rigidbody rigidBodyPlayer;
    private List<GameObject> drops;

    public GameObject flowerBed;
    public GameObject ufo; // to mosz
    public float planeDetectionCircleRadious;
    private List<GameObject> childrenPlanes;
    private Renderer ufoRenderer;
    private GameObject currentPlane;
    private Queue<GameObject> pouredPlanes;
    //private GameObject pouredPlane;
    private float scale;

    void Start()
    {
        rigidBodyPlayer = GetComponent<Rigidbody>();
        rigidBodyPlayer.useGravity = false;
        drops = new List<GameObject>();
        //flowerBeds = new List<GameObject>();

        currentPlane = null;
        pouredPlanes = new Queue<GameObject>();
        //pouredPlane = null;

        ufoRenderer = this.gameObject.GetComponent<Renderer>();
        childrenPlanes = new List<GameObject>();
        foreach (Transform tran in flowerBed.transform)
        {
            childrenPlanes.Add(tran.gameObject);
        }

        scale = 0.1f;
    }

    void FixedUpdate()
    {
        AddForce();
        Rotate();
        HandleSpecialKeys();
        UpdateShader();

        scale += 0.0001f;

        plant.transform.localScale = new Vector3(scale, scale, scale);

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

        this.rigidBodyPlayer.AddForce(movement * speed);
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

            if(currentPlane != null)
            {
                pouredPlanes.Enqueue(currentPlane);
                Invoke("GreenUpPlane", 1);//this will happen after x seconds   

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
        var planeMaterial = pouredPlanes.Dequeue().GetComponent<Renderer>().material;
        var prevColor = planeMaterial.color;

        if (prevColor.g < 255f && prevColor.r > 0f && prevColor.b > 0f)
        {            
            var newColor = new Color(prevColor.r - 0.05f, prevColor.b - 0.05f, prevColor.g + 0.2f);

            planeMaterial.SetColor("_Color", newColor);
        }
        
        //material.color = newColor(255f, 255f, 255f, 1);
        //pouredPlanes.Dequeue().SetActive(false);       
    }
}
