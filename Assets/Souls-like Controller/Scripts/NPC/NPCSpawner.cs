using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class NPCSpawner : MonoBehaviour
{
    [SerializeField] private GameObject npcPrefab;
    [SerializeField] private Transform spawnTransform;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
            Instantiate(npcPrefab, spawnTransform.position, Quaternion.identity);
    }
}
