using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public Object drop;
    private Rigidbody rigidBodyPlayer;

    void Start()
    {
        rigidBodyPlayer = GetComponent<Rigidbody>();
        rigidBodyPlayer.useGravity = false;
    }

    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        rigidBodyPlayer.AddForce(movement * speed);
        transform.Rotate(new Vector3(0,0,200)* Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            var ObjectSpawnPosition = this.transform.position; /*rigidBodyPlayer.transform.position + (rigidBodyPlayer.transform.forward /** distance*/;

            Instantiate(drop, ObjectSpawnPosition, Quaternion.identity);

        }

    }
}
