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
            var ObjectSpawnPosition = this.transform.position; /*rigidBodyPlayer.transform.position + (rigidBodyPlayer.transform.forward /** distance*/;

            this.drops.Add(Instantiate(drop, ObjectSpawnPosition, Quaternion.identity));
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
