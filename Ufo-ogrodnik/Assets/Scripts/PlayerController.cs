using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public GameObject drop;
    //public GameObject flowerBed;
    private Rigidbody rigidBodyPlayer;
    private List<GameObject> drops;

    void Start()
    {
        rigidBodyPlayer = GetComponent<Rigidbody>();
        rigidBodyPlayer.useGravity = false;
        drops = new List<GameObject>();
    }

    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        this.rigidBodyPlayer.AddForce(movement * speed);
        transform.Rotate(new Vector3(0, 0, 200) * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            for (int i = 0; i < 50; i++)
            {
                Vector3 offset = new Vector3(Random.Range(-5, 5)/10f, Random.Range(-5, 5) / 10f, Random.Range(-5, 5) / 10f);
                
                var ObjectSpawnPosition = this.transform.position + offset;
                var newDrop = Instantiate(drop, ObjectSpawnPosition, Quaternion.identity);                
                this.drops.Add(newDrop);
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
        }

        if (this.drops.Count > 0)
        {
            this.DropCollider();
        }           
    }

    private void DropCollider()
    {
        for (int i = this.drops.Count ; i != 0; i--)
        {
            if (this.drops[i-1].transform.position.y /* - flowerBed.transform.position.y*/ < 0)
            {
                var tempDropHandle = this.drops[i-1];
                this.drops.Remove(this.drops[i-1]);
                if (tempDropHandle != null)
                {
                    Destroy(tempDropHandle);
                }                
            }
        }         
    }
}
