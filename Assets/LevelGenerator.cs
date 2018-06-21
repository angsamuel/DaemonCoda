using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour {
    public GameObject streetBlock;

    public int level_grid_size = 100;
    public List<StreetCrawler> streetCrawlers;
    List<Roomie> roomies;
    GameObject[,] levelGrid;
	
    // Use this for initialization
	void Start () {
        streetCrawlers = new List<StreetCrawler>();
        roomies = new List<Roomie>();
        levelGrid = new GameObject[level_grid_size, level_grid_size];
        //Test();
        Generate();
    }

    void Generate()
    {
        StreetCrawler sc = new StreetCrawler(this, 5, new Vector2(50, 0), 0, streetBlock);
        streetCrawlers.Add(sc);
        for (int x = 0; x < 1000; x++)
        {
            for (int i = 0; i < streetCrawlers.Count; i++)
            {
                streetCrawlers[i].Cycle();
            }

            if(streetCrawlers.Count == 0)
            {
                x = 1000;
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
       
	}

    public void Test()
    {
        StreetCrawler sc = new StreetCrawler(this, 5, new Vector2(50, 0), 0, streetBlock);
        for(int i = 0; i < 9; i++)
        {
            sc.Cycle();
        }
    }

    public void PlaceBlock(int x, int y, GameObject block)
    {
        PlaceBlock(x, y, block, Color.white);
    }

    public void PlaceBlock(int x, int y, GameObject block, Color c)
    {
        if (levelGrid[x, y] != null)
        {
            Destroy(levelGrid[x, y]);
        }

        GameObject newBlock = Instantiate(block, transform);
        newBlock.transform.Translate(new Vector2(x, y));
        levelGrid[x, y] = newBlock;
        newBlock.GetComponent<SpriteRenderer>().color = c;
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
