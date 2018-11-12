using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour {
   
    // Public variables
    [Header("Movement Settings")]
    public float speed = 10.0f;

    [Header("Weapon Settings")]
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public float bulletSpeed = 20.0f;

    // Private variables
    private Rigidbody2D rBody;

	// Use this for initialization
	void Start () {
        rBody = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void FixedUpdate()
    {
        if(!isLocalPlayer)
        {
            return;
        }

        float horiz = Input.GetAxis("Horizontal");

        Vector2 movement = new Vector2(horiz, 0);
        movement.x *= speed; // Scale the speed value to have faster movements.

        // Assign the movement to the players velocity.
        rBody.velocity = movement;

        if(Input.GetButton("Fire1"))
        {
            CmdFire();
        }
    }

    [Command]
    void CmdFire()
    {
        // Instantiate the bullet
        var bullet = (GameObject)Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);
        // Move the Bullet
        bullet.GetComponent<Rigidbody2D>().velocity = transform.right * bulletSpeed;
        // Inform the network to spawn our bullet prefab to all clients
        NetworkServer.Spawn(bullet);
        // Destroy the bullet after ______ seconds
        Destroy(bullet, 2.0f);
    }

    // Executes whenever the local player connects to the game
    public override void OnStartLocalPlayer()
    {
        GetComponent<SpriteRenderer>().color = Color.blue;
    }
}
