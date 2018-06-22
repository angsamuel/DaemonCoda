using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomPlopper {
    LevelGenerator levelGenerator;
    int gridSize = 0;
    int maxSize = 10;
    int minSize = 3;
    GameObject floor;
    GameObject wall;

	// Use this for initialization
	public RoomPlopper(LevelGenerator lg, GameObject f, GameObject w)
    {
        levelGenerator = lg;
        gridSize = levelGenerator.level_grid_size;
        floor = f;
        wall = w;
    }
	
	public IEnumerator PlopRooms()
    {
        for (int y = 0; y < gridSize; y++)
        {
            for (int x = 0; x < gridSize; x++)
            {
                if (!levelGenerator.SpaceIsFree(x, y))
                {
                    List<int> availableDirections = GetAvailableDirections(x,y);

                    if(availableDirections.Count > 0)
                    {
                        int directionChoice = Random.Range(0, availableDirections.Count);
                        int direction = availableDirections[directionChoice];
                        Plop(x, y, direction);
                        yield return new WaitForSeconds(.01f);
                    }
                }
            }
        }
    }

    void Plop(int x, int y, int d)
    {
        
        int posX = x;
        int posY = y;
        int velX = 0;
        int velY = 0;

        

        switch (d)
        {
            case 0:
                velY = 1;
                posY += 2;
                break;
            case 1:
                posX += 2;
                velX = 1;
                break;
            case 2:
                posY -= 2;
                velY = -1;
                break;
            case 3:
                posX -= 2;
                velX = -1;
                break;
        }

        int rightBound = maxSize;
        int leftBound = maxSize;
        int upBound = maxSize;
        int downBound = maxSize;
        int thirdBound = 0;

        bool lock1 = false;
        bool lock2 = false;

        if(d == 0 || d == 2)
        {
            for (int j = 0; j < maxSize; j++)
            {
                //explore right

                for (int i = 0; i < rightBound; i++)
                {
                    if (!levelGenerator.SpaceIsFree(posX + i + 1, posY + (velY * j)))
                    {
                        rightBound = i-1;
                    }
                }

                //explore left

                for (int i = 0; i < leftBound; i++)
                {
                    if (!levelGenerator.SpaceIsFree((posX - i) - 1, posY + (velY * j)))
                    {
                        leftBound = i-1;
                    }
                }
                
                if(rightBound + leftBound < minSize)
                {
                    j = maxSize + 1;
                }
                else
                {
                    thirdBound = j-1;
                }

            }

            Debug.Log("LEFT: " + leftBound + " " + "RIGHT: " + rightBound + " THIRD: " + thirdBound);

            //place tiles if appropriate area found
            if(thirdBound >= minSize && rightBound + leftBound >= minSize)
            {
                for (int j = 0; j < thirdBound; j++)
                {
                    //place right

                    for (int i = 0; i < rightBound; i++)
                    {
                            levelGenerator.PlaceFloor(posX + i, posY + (velY * j), floor, Color.white);
                    }

                    //explore left

                    for (int i = 0; i < leftBound; i++)
                    {

                            levelGenerator.PlaceFloor(posX - i, posY + (velY * j), floor, Color.white);
                    }

                }
            }
            
        }

        
    }

    List<int> GetAvailableDirections(int x, int y)
    {
        List<int> output = new List<int>();
        if (levelGenerator.SpaceIsFree(x-1, y+1))
        {
            if (levelGenerator.SpaceIsFree(x, y + 1) && levelGenerator.SpaceIsFree(x, y + 2))
            {
                output.Add(0);
            }
        }

        if (levelGenerator.SpaceIsFree(x + 1, y) && levelGenerator.SpaceIsFree(x + 2, y))
        {
            output.Add(1);
        }

        if(levelGenerator.SpaceIsFree(x, y - 1) && levelGenerator.SpaceIsFree(x, y - 1))
        {
            output.Add(2);
        }

        if (levelGenerator.SpaceIsFree(x - 1, y - 1))
        {

            if (levelGenerator.SpaceIsFree(x - 1, y))
            {
                output.Add(3);
            }
        }

        return output;
    }

}
