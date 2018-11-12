using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    void OnCollisionEnter2D(Collision2D other)
    {
        var hit = other.gameObject;
        var health = hit.GetComponent<Health>();
        if(health != null)
        {
            health.TakeDamage(10);
        }

        Destroy(this.gameObject);
    }
}
