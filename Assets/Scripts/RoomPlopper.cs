using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomPlopper {
    LevelGenerator levelGenerator;
    int gridSize = 0;
    int maxSize = 12;
    int minSize = 5;
    GameObject floor;
    GameObject wall;
    float doorChance = .1f;

	// Use this for initialization
	public RoomPlopper(LevelGenerator lg, GameObject f, GameObject w)
    {
        levelGenerator = lg;
        gridSize = levelGenerator.level_grid_size;
        floor = f;
        wall = w;
    }
	
	public void PlopRooms()
    {
        

        for (int y = 0; y < gridSize; y++)
        {
            for (int x = 0; x < gridSize; x++)
            {
                if (Random.Range(0.0f, 1.0f) > 0.75f)
                {
                    if (!levelGenerator.SpaceIsFree(x, y) && levelGenerator.GetTileTag(x, y) == "street")
                    {
                        List<int> availableDirections = GetAvailableDirections(x, y);

                        if (availableDirections.Count > 0)
                        {
                            int directionChoice = Random.Range(0, availableDirections.Count);
                            int direction = availableDirections[directionChoice];
                            Plop(x, y, direction);
                            //yield return new WaitForSeconds(0);
                        }
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
                        rightBound = i;
                    }
                }

                //explore left

                for (int i = 0; i < leftBound; i++)
                {
                    if (!levelGenerator.SpaceIsFree((posX - i) - 1, posY + (velY * j)))
                    {
                        leftBound = i;
                    }
                }
                
                if(rightBound + leftBound < minSize || !levelGenerator.SpaceIsFree(posX, posY + (velY * j)) || !levelGenerator.SpaceIsFree(posX, posY + (velY * j) + (velY / Mathf.Abs(velY))))
                {
                    j = maxSize + 1;
                }
                else
                {
                    thirdBound = j-1;
                }

            }


            //place tiles if appropriate area found
            if(thirdBound >= minSize && rightBound + leftBound >= minSize && leftBound > 1 && rightBound > 1)
            {
                for (int j = 0; j < thirdBound; j++)
                {
                    //place right

                    for (int i = 0; i < rightBound; i++)
                    {
                           
                        if(i == rightBound - 1 || j == 0 || j == thirdBound - 1)
                        {
                           levelGenerator.PlaceWall(posX + i, posY + (velY * j), wall, Color.red);
                        }
                        else
                        {
                            levelGenerator.PlaceFloor(posX + i, posY + (velY * j), floor, Color.white);
                        }
                    }

                    //place left

                    for (int i = 0; i < leftBound; i++)
                    {

                        if (i == leftBound - 1 || j == 0 || j == thirdBound - 1)
                        {
                            levelGenerator.PlaceWall(posX - i, posY + (velY * j), wall, Color.red);
                            

                        }
                        else
                        {
                            levelGenerator.PlaceFloor(posX - i, posY + (velY * j), floor, Color.white);
                        }
                    }

                }

                levelGenerator.PlaceFloor(posX + ((rightBound - leftBound)/2), posY, floor, Color.blue);
                
            }
            
        }

        if (d == 1 || d == 3)
        {
            for (int j = 0; j < maxSize; j++)
            {
                //explore up

                for (int i = 0; i < upBound; i++)
                {
                    if (!levelGenerator.SpaceIsFree(posX + (velX * j), posY + i + 1))
                    {
                        upBound = i;
                    }
                }

                //explore down

                for (int i = 0; i < downBound; i++)
                {
                    if (!levelGenerator.SpaceIsFree(posX + (velX * j), (posY - i) - 1))
                    {
                        downBound = i;
                    }
                }

                if (rightBound + leftBound < minSize || !levelGenerator.SpaceIsFree(posX + (velX * j), posY) || !levelGenerator.SpaceIsFree(posX + (velY * j) + (velX / Mathf.Abs(velX)), posY))
                {
                    j = maxSize + 1;
                }
                else
                {
                    thirdBound = j - 1;
                }

            }

            //Debug.Log("LEFT: " + leftBound + " " + "RIGHT: " + rightBound + " THIRD: " + thirdBound);

            //place tiles if appropriate area found
            if (thirdBound >= minSize && upBound + downBound >= minSize && upBound > 1 && downBound > 1)
            {
                for (int j = 0; j < thirdBound; j++)
                {
                    //place right

                    for (int i = 0; i < upBound; i++)
                    {
                        if (i == upBound - 1 || j == 0 || j == thirdBound - 1)
                        {
                            levelGenerator.PlaceWall(posX + (velX * j), posY + i, wall, Color.red);
                        }
                        else
                        {
                            levelGenerator.PlaceFloor(posX + (velX * j), posY + i, floor, Color.white);
                        }

                    }

                    //explore left

                    for (int i = 0; i < downBound; i++)
                    {
                        if (i == downBound - 1 || j == 0 || j == thirdBound - 1)
                        {
                            levelGenerator.PlaceWall(posX + (velX * j), posY - i, wall, Color.red);
                        }
                        else
                        {
                            levelGenerator.PlaceFloor(posX + (velX * j), posY - i, floor, Color.white);
                        }

                       
                    }

                }
                levelGenerator.PlaceFloor(posX, posY + ((upBound - downBound) / 2), floor, Color.blue);
            }

        }


    }

    List<int> GetAvailableDirections(int x, int y)
    {
        List<int> output = new List<int>();



            if (levelGenerator.SpaceIsFree(x, y + 1) && levelGenerator.SpaceIsFree(x, y + 2))
            {
                output.Add(0);
            }

            if (levelGenerator.SpaceIsFree(x + 1, y) && levelGenerator.SpaceIsFree(x + 2, y))
            {
                output.Add(1);
            }

            if (levelGenerator.SpaceIsFree(x, y - 1) && levelGenerator.SpaceIsFree(x, y - 1))
            {
                output.Add(2);
            }

            if (levelGenerator.SpaceIsFree(x - 1, y))
            {
                output.Add(3);
            }
        
    

        return output;
    }

}
