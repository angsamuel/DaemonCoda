using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour {
    public GameObject streetBlock;

    public int level_grid_size = 100;
    List<StreetCrawler> streetCrawlers;
    List<Roomie> roomies;
    GameObject[,] levelGrid;
	
    // Use this for initialization
	void Start () {
        streetCrawlers = new List<StreetCrawler>();
        roomies = new List<Roomie>();
        levelGrid = new GameObject[level_grid_size, level_grid_size];

        Test();
    }
	
	// Update is called once per frame
	void Update () {
       
	}

    public void Test()
    {
        StreetCrawler sc = new StreetCrawler(this, 5, new Vector2(10, 0), 0, streetBlock);
        for(int i = 0; i < 9; i++)
        {
            sc.Cycle();
        }
    }

    public void PlaceBlock(int x, int y, GameObject block)
    {
        if(levelGrid[x,y] != null)
        {
            Destroy(levelGrid[x, y]);
        }

        GameObject newBlock = Instantiate(block, transform);
        newBlock.transform.Translate(new Vector2(x, y));
        levelGrid[x, y] = newBlock;
    }

    public bool SpaceIsFree(int x, int y)
    {
        return x < level_grid_size && y < level_grid_size && x > -1 && y > -1 && levelGrid[x, y] == null;
    }

    public bool InBounds(int x, int y)
    {
        return x < level_grid_size && y < level_grid_size;
    }



}
