using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roomie {

    GameObject block;
    LevelGenerator levelGenerator;
    int spawnerDirection = 0;
    int minSize = 0;
    int maxSize = 0;
    int posX = 0;
    int posY = 0;
    int horzSize = 0;
    int vertSize = 0;



    public Roomie(LevelGenerator lg, int x, int y, int sd, int min, int max, GameObject b) {
        levelGenerator = lg;
        spawnerDirection = sd;
        minSize = min;
        maxSize = max;
        posX = x;
        posY = y;
        block = b;
    }

    public void SetPos(int x, int y)
    {
        posX = x;
        posY = y;
    }

    public void SetSpawnerDirection(int sd)
    {
        spawnerDirection = sd;
    }

    public void PlopRoom()
    {
        Scope();
    }

    public void Grow()
    {

    }

    void Scope()
    {
        int vertSize = Random.Range(minSize, maxSize + 1);
        int horzSize = Random.Range(minSize, maxSize + 1);
        Vector2 velocity = new Vector2(0, 0);

        switch (spawnerDirection)
        {
            case 0:
                velocity = new Vector2(0, -1);
                break;
            case 1:
                velocity = new Vector2(-1, 0);
                break;
            case 2:
                velocity = new Vector2(0, 1);
                break;
            case 3:
                velocity = new Vector2(1, 0);
                break;
            default:
                break;

        }

        bool lockOne = false;
        bool lockTwo = false;

        if (spawnerDirection == 0 || spawnerDirection == 2)
        {
            
            for (int j = 0; j < vertSize; j++)
            {
                lockOne = false;
                lockTwo = false;
                if (levelGenerator.SpaceIsFree(posX, posY) && levelGenerator.SpaceIsFree(posX + 1, posY) && levelGenerator.SpaceIsFree(posX - 1, posY))
                {
                    levelGenerator.PlaceStreet(posX, posY, block, Color.red);

                    for (int i = 1; i <= horzSize / 2; i++)
                    {
                        if (levelGenerator.SpaceIsFree(posX + i, posY) && levelGenerator.SpaceIsFree(posX + i + 1, posY) && !lockOne)
                        {
                            levelGenerator.PlaceStreet(posX + i, posY, block, Color.red);
                        }
                        else
                        {
                            lockOne = true;
                        }

                        if (levelGenerator.SpaceIsFree(posX - i, posY) && levelGenerator.SpaceIsFree((posX - i) - 1, posY) && !lockTwo)
                        {
                            levelGenerator.PlaceStreet(posX - i, posY, block, Color.red);
                        }
                        else
                        {
                            lockTwo = true;
                        }
                    }
                    posX += (int)velocity.x;
                    posY += (int)velocity.y;
                }
            }
        }
        else if (spawnerDirection == 1 || spawnerDirection == 3)
        {
            
            for (int j = 0; j < horzSize; j++)
            {
                lockOne = false;
                lockTwo = false;

                if (levelGenerator.SpaceIsFree(posX, posY) && levelGenerator.SpaceIsFree(posX, posY + 1) && levelGenerator.SpaceIsFree(posX, posY - 1))
                {
                    levelGenerator.PlaceStreet(posX, posY, block, Color.red);

                    for (int i = 1; i <= vertSize / 2; i++)
                    {
                        if (levelGenerator.SpaceIsFree(posX, posY + i) && levelGenerator.SpaceIsFree(posX, posY + i + 1) && !lockOne)
                        {
                            levelGenerator.PlaceStreet(posX, posY + i, block, Color.red);
                        }
                        else
                        {
                            lockOne = true;
                        }

                        if (levelGenerator.SpaceIsFree(posX, posY - i) && levelGenerator.SpaceIsFree(posX, (posY - i) - 1) && !lockTwo)
                        {
                            levelGenerator.PlaceStreet(posX, posY - i, block, Color.red);
                        }
                        else
                        {
                            lockTwo = true;
                        }
                    }
                    posX += (int)velocity.x;
                    posY += (int)velocity.y;
                }
                    
            }
        }

    }
  
}
