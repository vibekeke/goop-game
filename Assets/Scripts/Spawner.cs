using UnityEngine;
using System.Collections.Generic;

public class Spawner : MonoBehaviour
{
    [Header("Settings")]
    public GameObject newObject;

    [Header("References")]
    public Camera mainCamera; // Optional, can auto-detect


    void Start()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0f;


            SpawnObject(mousePosition);

        }

        void SpawnObject(Vector3 position)
        {
            GameObject obj = Instantiate(newObject, position, Quaternion.identity);
        }
    }
}

