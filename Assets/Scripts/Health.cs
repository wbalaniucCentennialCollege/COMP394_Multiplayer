using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Health : NetworkBehaviour {

    public const int maxHealth = 100;
    [SyncVar(hook = "OnChangeHealth")]
    public int currentHealth = maxHealth;
    public RectTransform healthBar;
    public bool destroyOnDeath;
    public NetworkStartPosition[] spawnPositions;

    void Start()
    {
        if(isLocalPlayer)
        {
            spawnPositions = FindObjectsOfType<NetworkStartPosition>();
        }
    }

    public void TakeDamage(int amount)
    {
        if(!isServer)
        {
            return;
        }

        currentHealth -= amount;
        if(currentHealth <= 0) // ME DEAD
        {
            if (destroyOnDeath)
            {
                Destroy(gameObject);
            }
            else
            {
                currentHealth = maxHealth;
                // Debug.Log("Dead");
                RpcRespawn();
            }
        }

    }

    void OnChangeHealth(int health)
    {
        healthBar.sizeDelta = new Vector2(health, healthBar.sizeDelta.y);
    }

    [ClientRpc]
    void RpcRespawn()
    {
        if(isLocalPlayer)
        {
            Vector2 spawnPoint = Vector2.zero;

            if(spawnPositions != null && spawnPositions.Length > 0)
            {
                spawnPoint = spawnPositions[Random.Range(0, spawnPositions.Length)].transform.position;
            }

            transform.position = spawnPoint;
        }
    }
}
