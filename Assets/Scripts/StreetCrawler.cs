using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StreetCrawler {
    GameObject block;
    LevelGenerator levelGenerator;
    int width;
    int prevWidth;
    int speed = 10;
    float roomieSpawnChance;
    float streetCrawlerSpawnChance;
    int direction;
    int stoptimer = 50;

    //mutation parameters
    float chanceToTurn = .8f;
    float chanceToShrink = .8f;
    float chanceToSpawnRoomie;
    float chanceToSpawnStreetCrawler;

    //space between roads 
    int padding = 0;

    Vector2 gridPos;

    public StreetCrawler(LevelGenerator lg, int sw, Vector2 pos, int orientation, GameObject b)
    {
        levelGenerator = lg;
        width = sw;
        gridPos = pos;
        block = b;
    }

    public void Cycle()
    {
        Debug.Log("Taking Cycle");
        for (int i = 0; i < speed; i++) {
            
            Build();
            Move();
        }
        Mutate();
        Birth();
        Debug.Log(gridPos);
    }


    void Move()
    {
        switch (direction)
        {
            case 0:
                gridPos += new Vector2(0, 1);
                break;
            case 1:
                gridPos += new Vector2(1, 0);
                break;
            case 2:
                gridPos += new Vector2(0, -1);
                break;
            case 3:
                gridPos += new Vector2(-1, 0);
                break;
        }

    }

    void MoveBack()
    {
        switch (direction)
        {
            case 0:
                gridPos -= new Vector2(0, 1);
                break;
            case 1:
                gridPos -= new Vector2(1, 0);
                break;
            case 2:
                gridPos -= new Vector2(0, -1);
                break;
            case 3:
                gridPos -= new Vector2(-1, 0);
                break;
        }
    }

    void Build()
    {
        bool vertical = false;
        Vector2 orientationCheck = new Vector2(0, 1);
        if (direction == 0 || direction == 2)
        {
            //place vertically
            vertical = true;
            orientationCheck = new Vector2(1, 0);
        }

        //building a road will not destroy existing objects or leave the map
        bool goodToBuild = true;

        for (int i = 0; i <= width / 2; i++)
        {
            if (!levelGenerator.SpaceIsFree((int)gridPos.x + (i * (int)orientationCheck.x) , (int)gridPos.y + (i * (int)orientationCheck.y)) || 
                !levelGenerator.SpaceIsFree((int)gridPos.x - (i * (int)orientationCheck.x), (int)gridPos.y - (i * (int)orientationCheck.y)))
            {
                goodToBuild = false;
            }
        }

        if (goodToBuild)
        {
            levelGenerator.PlaceBlock((int)gridPos.x, (int)gridPos.y, block);
            for (int i = 0; i <= width / 2; i++)
            {
                levelGenerator.PlaceBlock((int)gridPos.x + (i * (int)orientationCheck.x), (int)gridPos.y + (i * (int)orientationCheck.y), block);
                levelGenerator.PlaceBlock((int)gridPos.x - (i * (int)orientationCheck.x), (int)gridPos.y - (i * (int)orientationCheck.y), block);
            }
        }

    }

    void Mutate()
    {
        //shrink
        float shrinkRoll = Random.Range(0.0f, 1.0f);
        if (shrinkRoll < chanceToShrink)
        {
            Debug.Log("shrinking");
            RandomShrink();
        }

        //turn
        float turnRoll = Random.Range(0.0f, 1.0f);
        if(turnRoll < chanceToTurn)
        {
            Debug.Log("turning");
            RandomTurn();   
        }
    }

   void RandomShrink()
    {
        prevWidth = width;
        width = Random.Range(1, width);
    }

    void RandomTurn()
    {
        for (int i = 0; i < (width / 2) + 1; i++)
        {
            MoveBack();
        }

        if (direction == 0 || direction == 2)
        {
            int[] choices = { 1, 3 };
            direction = choices[Random.Range(0,2)];
        }
        else if(direction == 1 || direction == 3)
        {
            int[] choices = { 0, 2 };
            direction = choices[Random.Range(0, 2)];
        }
        for (int i = 0; i < prevWidth / 2; i++)
        {
            Move();
        }
    }

    //produce new agents
    void Birth()
    {

    }
}
