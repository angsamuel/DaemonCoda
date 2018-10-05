using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StreetCrawler {
    GameObject block;
    LevelGenerator levelGenerator;
    int width;
    int prevWidth;
    int speed = 15;
    
    public int direction;
    int stoptimer = 50;

    //mutation parameters
    float roomieSpawnChance = .0f;
    float streetCrawlerSpawnChance;
    float chanceToTurn = .0f;
    float chanceToShrink = .0f;
    float intersectionChance = .5f;
    float deadEndChance = 0.0f;
    

    //space between roads 
    int padding = 0;

    Vector2 gridPos;

    public StreetCrawler(LevelGenerator lg, int sw, Vector2 pos, int orientation, GameObject b)
    {
        levelGenerator = lg;
        width = sw;
        prevWidth = width;
        gridPos = pos;
        block = b;
        direction = orientation;
        

        if (width == 5)
        {
            streetCrawlerSpawnChance = .5f;
            chanceToTurn = 0.05f;
            speed = 10;

        } else if (width == 3)
        {
            streetCrawlerSpawnChance = 1f;
            chanceToTurn = .35f;
            speed = 10;
        }else if(width == 1)
        {
            streetCrawlerSpawnChance = 0.35f;
            chanceToTurn = .25f;
            deadEndChance = 0.5f;
            speed = 5;
        }

    }
    int cycleCount = 0;
    int moveCount = 0;
    PatrolRoute pr;
    public void Cycle()
    {
        moveCount = 0;
        if(width > 1 && cycleCount < 1){
            pr = levelGenerator.CreatePatrolRoute();
        }

        
        
        for (int i = 0; i < speed; i++) {
           
            if (Build()) {
                if(width > 1 && pr != null && ((moveCount < 1 && cycleCount < 1) || (width == 3 && moveCount == speed - 2) || (width == 5 && moveCount == speed - 3) )){
                    levelGenerator.CreateCheckpoint(pr, (int)gridPos.x, (int)gridPos.y);
                }
                Move();
                moveCount += 1;
            }
             
        }

        
        if (width != 5 || gridPos.y < levelGenerator.level_grid_size)
        {
            Mutate();
        }
        cycleCount += 1;

    }

    void Move(int times)
    {
        for(int i = 0; i<times; i++)
        {
            Move();
        }

    }

    void MoveBack(int times)
    {
        for (int i = 0; i < times; i++)
        {
            MoveBack();
        }

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

    bool Build()
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
            levelGenerator.PlaceStreet((int)gridPos.x, (int)gridPos.y, block);
            for (int i = 0; i <= width / 2; i++)
            {
                levelGenerator.PlaceStreet((int)gridPos.x + (i * (int)orientationCheck.x), (int)gridPos.y + (i * (int)orientationCheck.y), block);
                levelGenerator.PlaceStreet((int)gridPos.x - (i * (int)orientationCheck.x), (int)gridPos.y - (i * (int)orientationCheck.y), block);
            }
        }

        if (!goodToBuild)
        {
            levelGenerator.streetCrawlers.Remove(this);
            return false;
        }
        return true;

    }

    void Mutate()
    {

        if(width == 5)
        { 
            bool turned = false;

            //spawn smaller roads
            float roll = Random.Range(0.0f, 1.0f);

            if (chanceToTurn > roll)
            {
                //Debug.Log("Turn");
                //make a turn
                RandomTurn();
                turned = true;
            }
            else if(roll < streetCrawlerSpawnChance)
            {
                //spawn or intersection
                roll = Random.Range(0.0f, 1.0f);
                if(roll < intersectionChance)
                {
                    StreetCrawler newCrawler1 = new StreetCrawler(levelGenerator, 3, gridPos, direction, block);
                    StreetCrawler newCrawler2 = new StreetCrawler(levelGenerator, 3, gridPos, direction, block);

                    newCrawler1.MoveBack(2);
                    newCrawler2.MoveBack(2);

                    //spawn two roads in opposite directions
                    if (direction == 0 || direction == 2)
                    {
                        newCrawler1.direction = 1;
                        newCrawler2.direction = 3;
                    }
                    else
                    {
                        newCrawler1.direction = 0;
                        newCrawler2.direction = 2;
                    }
                    newCrawler1.Move(3);
                    newCrawler2.Move(3);

                    levelGenerator.streetCrawlers.Add(newCrawler1);
                    levelGenerator.streetCrawlers.Add(newCrawler2);
                    
                }
                else
                {
                    //spawn one road from selection of directions
                    StreetCrawler newCrawler = new StreetCrawler(levelGenerator, 3, gridPos, direction, block);
                    newCrawler.MoveBack(2);
                    newCrawler.direction = GetNewRandomDirection(direction);
                    newCrawler.Move(3);
                    levelGenerator.streetCrawlers.Add(newCrawler);
                }

            }


        }else if(width == 3)
        {
           
            bool turned = false;
            float roll1 = Random.Range(0.0f, 1.0f);
            float roll2 = Random.Range(0.0f, 1.0f);
            if (chanceToTurn > roll1)
            {
                //Debug.Log("Turn");
                //make a turn
                RandomTurn();
                turned = true;
            }
            else if(streetCrawlerSpawnChance > roll2)
            {
                //spawn smaller roads
                float roll = Random.Range(0.0f, 1.0f);
                if (roll < streetCrawlerSpawnChance)
                {
                    //spawn or intersection
                    roll = Random.Range(0.0f, 1.0f);
                    if (roll < intersectionChance)
                    {
                        StreetCrawler newCrawler1 = new StreetCrawler(levelGenerator, 1, gridPos, direction, block);
                        StreetCrawler newCrawler2 = new StreetCrawler(levelGenerator, 1, gridPos, direction, block);


                        //newCrawler1.Move(2);
                        //newCrawler2.MoveBack(2);

                        newCrawler1.MoveBack();
                        newCrawler2.MoveBack();

                        //spawn two roads in opposite directions
                        if (direction == 0 || direction == 2)
                        {
                            newCrawler1.direction = 1;
                            newCrawler2.direction = 3;
                        }
                        else
                        {
                            newCrawler1.direction = 0;
                            newCrawler2.direction = 2;
                        }
                        newCrawler1.Move(2);
                        newCrawler2.Move(2);

                        levelGenerator.streetCrawlers.Add(newCrawler1);
                        levelGenerator.streetCrawlers.Add(newCrawler2);

                    }
                    else
                    {
                        //spawn one road from selection of directions
                        StreetCrawler newCrawler = new StreetCrawler(levelGenerator, 1, gridPos, direction, block);
                        newCrawler.MoveBack();
                        newCrawler.direction = GetNewRandomDirection(direction);
                        newCrawler.Move(2);
                        levelGenerator.streetCrawlers.Add(newCrawler);
                    }

                    float deadEndRoll = Random.Range(0.0f, 1.0f);
                    if(deadEndRoll < deadEndChance)
                    {
                        levelGenerator.streetCrawlers.Remove(this);
                    }

                }
            }

            //spawn equal or smaller roads

            //or change direction



        }else if(width == 1)
        {
            //spawn equal or smaller roads

            //or change direction
            bool turned = false;
            float roll1 = Random.Range(0.0f, 1.0f);
            float roll2 = Random.Range(0.0f, 1.0f);
            if (chanceToTurn > roll1)
            {
                //make a turn
                RandomTurn();
                turned = true;
            }
            else if (streetCrawlerSpawnChance > roll2)
            {
                //spawn smaller roads
                float roll = Random.Range(0.0f, 1.0f);
                if (roll < streetCrawlerSpawnChance)
                {
                    //spawn or intersection
                    roll = Random.Range(0.0f, 1.0f);
                    if (roll < intersectionChance)
                    {
                        StreetCrawler newCrawler1 = new StreetCrawler(levelGenerator, 1, gridPos, direction, block);
                        StreetCrawler newCrawler2 = new StreetCrawler(levelGenerator, 1, gridPos, direction, block);

                        newCrawler1.MoveBack();
                        newCrawler2.MoveBack();

                        //spawn two roads in opposite directions
                        if (direction == 0 || direction == 2)
                        {
                            newCrawler1.direction = 1;
                            newCrawler2.direction = 3;
                        }
                        else
                        {
                            newCrawler1.direction = 0;
                            newCrawler2.direction = 2;
                        }
                        newCrawler1.Move();
                        newCrawler2.Move();

                        levelGenerator.streetCrawlers.Add(newCrawler1);
                        levelGenerator.streetCrawlers.Add(newCrawler2);

                    }
                    else
                    {
                        //spawn one road from selection of directions
                        StreetCrawler newCrawler = new StreetCrawler(levelGenerator, 1, gridPos, direction, block);
                        newCrawler.MoveBack();
                        newCrawler.direction = GetNewRandomDirection(direction);
                        newCrawler.Move();
                        levelGenerator.streetCrawlers.Add(newCrawler);
                    }

                }
            }

        }


    }

   void RandomShrink()
    {
        prevWidth = width;
        width = Random.Range(1, width);
    }

    int GetNewRandomDirection(int currentDirection)
    {

        int newDirection = -1;
        if (currentDirection == 0 || currentDirection == 2)
        {
            int[] choices = { 1, 3 };
            newDirection = choices[Random.Range(0, 2)];
        }
        else if (currentDirection == 1 || currentDirection == 3)
        {
            int[] choices = { 0, 2 };
            newDirection = choices[Random.Range(0, 2)];
        }
        return newDirection;
    }


    void RandomTurn()
    {

        MoveBack((width / 2) + 1);
        /*
        for (int i = 0; i < (width / 2) + 1; i++)
        {
            MoveBack();
        }
        */

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

        Move((prevWidth / 2) + 1);
        /*
        for (int i = 0; i < (prevWidth / 2) + 1; i++)
        {
            Move();
            Debug.Log("p");
        }*/
       // levelGenerator.PlaceBlock((int)gridPos.x, (int)gridPos.y, block, Color.blue);
    }

    void Birth()
    {
        float roomieRoll = Random.Range(0.0f, 1.0f);
        if(roomieRoll < roomieSpawnChance)
        {
            BirthRoomie();
        }
        float crawlRoll = Random.Range(0.0f, 1.0f);
    }

    //produce new agents
    void BirthRoomie()
    {
        int spawnDirection = 0;

        int spawnOffsetChoice = Random.Range(0, 2);
        int spawnOffset = 0;
        if (spawnOffsetChoice == 0)
        {
            spawnOffset = width / 2;
        }
        else
        {
            spawnOffset = -width / 2;
        }

        if(spawnOffset < 0)
        {
            spawnOffset -= 2;
        }else if(spawnOffset > 0)
        {
            spawnOffset += 2;
        }

        Roomie newRoomie = new Roomie(levelGenerator, (int)gridPos.x, (int)gridPos.y, 1, 5, 10, block);
        if (direction == 0 || direction == 2)
        {
            //spawn roomie on either side

            newRoomie.SetPos((int)gridPos.x + spawnOffset, (int)gridPos.y);
           
            if (spawnOffset < 0)
            {
                spawnDirection = 1;
            }
            else
            {
                spawnDirection = 3;
            }

        } else if(direction == 1 || direction == 3)
        {
            newRoomie.SetPos((int)gridPos.x, (int)gridPos.y + spawnOffset);
            //spawn room up or down
            if (spawnOffset < 0)
            {
                spawnDirection = 0;
            }
            else
            {
                spawnDirection = 2;
            }

        }

        newRoomie.SetSpawnerDirection(spawnDirection);
        newRoomie.PlopRoom();

    }
}
