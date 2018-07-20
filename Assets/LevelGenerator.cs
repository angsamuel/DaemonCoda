﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour {
        float blockScale = .12f;
        float colorVariance = .1f;
    public LevelController levelController;
    public GameObject room;
    public GameObject shadeBlock;
    public GameObject streetBlock;
    public GameObject floorBlock;
    public GameObject wallBlock;
    public GameObject doorBlock;
    

    public Color streetColor;
    public Color floorColor;
    public Color wallColor;

    public GameObject husk;

    public int level_grid_size = 200;
    public List<StreetCrawler> streetCrawlers;
    List<Roomie> roomies;
    GameObject[,] levelGrid;
    Coroutine currentGeneration;
    Coroutine currentPlop;

    float spawnHuskChance = 0.01f;

    // Use this for initialization
	void Start () {
        streetCrawlers = new List<StreetCrawler>();
        roomies = new List<Roomie>();
        levelGrid = new GameObject[level_grid_size, level_grid_size];
        //Test();
        currentGeneration = StartCoroutine(Generate());
       
    }

    public void Populate()
    {
        for(int y = 0; y < level_grid_size; y++)
        {
            for(int x = 0; x < level_grid_size; x++)
            {
                if(levelGrid[x,y] != null)
                {
                    if(levelGrid[x,y].tag == "floor" || levelGrid[x,y].tag == "street")
                    {
                        //spawn husk
                        if(Random.Range(0.0f, 1.0f) < spawnHuskChance)
                        {
                            Instantiate(husk, levelGrid[x, y].transform.position, Quaternion.identity);
                        }
                    }
                }
            }
        }
    }

    public void NewVillage()
    {
        StopCoroutine(currentGeneration);
        if(currentPlop != null)
        {
            StopCoroutine(currentPlop);
        }
        
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

        for(int i = 0; i<levelController.enemyControllers.Count; i++)
        {
            Destroy(levelController.enemyControllers[i]);
        }
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

        RoomPlopper rm = new RoomPlopper(this, floorBlock, wallBlock);
        rm.street = streetBlock;
        rm.room = room;
        rm.PlopRooms();
        Populate();

       //StartCoroutine(levelController.TableConstructionRoutine());
    }
	
	// Update is called once per frame
	void Update () {
       
	}

    public string GetTileTag(int x, int y)
    {
        if (x > -1 && y > -1 && x < level_grid_size && y < level_grid_size)
        {
            if (levelGrid[x, y] == null)
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
    public void Test()
    {
        StreetCrawler sc = new StreetCrawler(this, 5, new Vector2(50, 0), 0, streetBlock);
        for(int i = 0; i < 9; i++)
        {
            sc.Cycle();
        }
    }

    public void PlaceWall(int x, int y, GameObject wall, Color c)
    {
        if (x > -1 && y > -1 && x < level_grid_size && y < level_grid_size)
        {


            if (levelGrid[x, y] != null)
            {
                Destroy(levelGrid[x, y]);
            }

            GameObject newBlock = Instantiate(wall, transform);
            newBlock.transform.Translate(new Vector2(x, y));
            levelGrid[x, y] = newBlock;
            newBlock.GetComponent<SpriteRenderer>().color = wallColor;

            float variance = Random.Range(-colorVariance, colorVariance);
            newBlock.GetComponent<SpriteRenderer>().color = new Color(wallColor.r + variance, wallColor.g + variance, wallColor.b + variance, 1f);
            //newBlock.GetComponent<SpriteRenderer>().transform.localScale = new Vector2(Random.Range(.125f, .15f), Random.Range(.125f, .15f));
            newBlock.GetComponent<SpriteRenderer>().transform.localScale = new Vector2(blockScale, blockScale);
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
            float variance = Random.Range(-colorVariance, colorVariance);

            newBlock.GetComponent<SpriteRenderer>().color = new Color(streetColor.r + variance, streetColor.g + variance, streetColor.b + variance, 1.0f);
            //newBlock.GetComponent<SpriteRenderer>().transform.localScale = new Vector2(Random.Range(.125f, .15f), Random.Range(.125f, .15f));
            newBlock.GetComponent<SpriteRenderer>().transform.localScale = new Vector2(blockScale, blockScale);

        }
    }


    public void PlaceFloor(int x, int y, GameObject floor, Color c, GameObject ro)
    {
        Room r = ro.GetComponent<Room>();

        if (x > -1 && y > -1 && x < level_grid_size && y < level_grid_size)
        {


            if (levelGrid[x, y] != null)
            {
                Destroy(levelGrid[x, y]);
            }

            GameObject newBlock = Instantiate(floor, transform);
            newBlock.transform.Translate(new Vector2(x, y));
            levelGrid[x, y] = newBlock;
            newBlock.GetComponent<SpriteRenderer>().color = floorColor;
            newBlock.GetComponent<SpriteRenderer>().color = new Color(floorColor.r, floorColor.g, floorColor.b, Random.Range(0.5f, 1.0f));
            newBlock.GetComponent<SpriteRenderer>().transform.localScale = new Vector2(.12f, .12f);

            //set interior floor reference to proper room
            newBlock.GetComponent<InteriorFloor>().room = r;
            
            
            GameObject newShadeBlock = Instantiate(shadeBlock, transform);
            float grey = Random.Range(0.0f, 0.0f);
            newShadeBlock.GetComponent<SpriteRenderer>().color = new Color(grey,grey,grey,Random.Range(1.0f, 1.0f));

            newShadeBlock.transform.position = newBlock.transform.position;
            r.AddShadow(newShadeBlock);
            //Debug.Log("size " + r.shadeBlocks.Count);


            //newBlock.GetComponent<SpriteRenderer>().transform.localScale = new Vector2(Random.Range(.125f, .15f), Random.Range(.125f, .15f));
            //newBlock.GetComponent<SpriteRenderer>().color = new Color(floorColor.r, floorColor.g, floorColor.b, Random.Range(0.5f, 1.0f));
            //newBlock.GetComponent<SpriteRenderer>().transform.localScale = new Vector2(Random.Range(.125f, .15f), Random.Range(.125f, .15f));
        }
    }
    public void PlaceDoor(int x, int y){
        GameObject newDoor = Instantiate(doorBlock, transform);
        newDoor.transform.Translate(new Vector2(x, y));
    
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
