using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour {
    public GameObject streetBlock;
    public GameObject floorBlock;

    public int level_grid_size = 200;
    public List<StreetCrawler> streetCrawlers;
    List<Roomie> roomies;
    GameObject[,] levelGrid;
    Coroutine currentGeneration;
    Coroutine currentPlop;
	
    // Use this for initialization
	void Start () {
        streetCrawlers = new List<StreetCrawler>();
        roomies = new List<Roomie>();
        levelGrid = new GameObject[level_grid_size, level_grid_size];
        //Test();
        currentGeneration = StartCoroutine(Generate());
       
    }

    public void NewVillage()
    {
        StopCoroutine(currentGeneration);
        StopCoroutine(currentPlop);
        for(int x = 0; x < level_grid_size; x++)
        {
            for(int y = 0; y<level_grid_size; y++)
            {
                if(levelGrid[x,y]!= null)
                {
                    Destroy(levelGrid[x, y]);
                }
            }
        }
        currentGeneration = StartCoroutine(Generate());
    }

    IEnumerator Generate()
    {
        StreetCrawler sc = new StreetCrawler(this, 5, new Vector2(level_grid_size/2, 0), 0, streetBlock);
        streetCrawlers.Add(sc);
        for (int x = 0; x < 1000; x++)
        {
            yield return new WaitForSeconds(.01f);

            for (int i = 0; i < streetCrawlers.Count; i++)
            {
                streetCrawlers[i].Cycle();
            }

            if(streetCrawlers.Count == 0)
            {
                x = 1000;
            }
        }

        RoomPlopper rm = new RoomPlopper(this, floorBlock, floorBlock);
        currentPlop = StartCoroutine(rm.PlopRooms());
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
        PlaceBlock(x, y, block, Color.black);
    }

    public void PlaceBlock(int x, int y, GameObject block, Color c)
    {

        if (x > -1 && y > -1 && x < level_grid_size && y < level_grid_size)
        {


            if (levelGrid[x, y] != null)
            {
                Destroy(levelGrid[x, y]);
            }

            GameObject newBlock = Instantiate(block, transform);
            newBlock.transform.Translate(new Vector2(x, y));
            levelGrid[x, y] = newBlock;
            newBlock.GetComponent<SpriteRenderer>().color = new Color(c.r, c.g, c.b, Random.Range(0.5f, 1.0f));
            newBlock.GetComponent<SpriteRenderer>().transform.localScale = new Vector2(Random.Range(.125f, .15f), Random.Range(.125f, .15f));
            
        }
    }

    public string GetTileTag(int x, int y)
    {
        if(x > -1 && y > -1 && x < level_grid_size && y < level_grid_size)
        {
            if(levelGrid[x,y] == null)
            {
                return "none";
            }
            else
            {
                return levelGrid[x, y].tag;
            }
        }

        return "outofbounds";
    }

    public void PlaceFloor(int x, int y, GameObject floor, Color c)
    {

        if (x > -1 && y > -1 && x < level_grid_size && y < level_grid_size)
        {


            if (levelGrid[x, y] != null)
            {
                Destroy(levelGrid[x, y]);
            }

            GameObject newBlock = Instantiate(floor, transform);
            newBlock.transform.Translate(new Vector2(x, y));
            levelGrid[x, y] = newBlock;
            newBlock.GetComponent<SpriteRenderer>().color = new Color(c.r, c.g, c.b, Random.Range(0.5f, 1.0f));
        }
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
