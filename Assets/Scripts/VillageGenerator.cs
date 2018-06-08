using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillageGenerator : MonoBehaviour
{
    public int width;
    public int height;
    public int plot_size;
    public GameObject building;
    // Use this for initialization
    void Start()
    {
        GenerateVillage();
    }

    void GenerateVillage()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Instantiate(building, new Vector3(x, y, 0) * plot_size, Quaternion.identity);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
