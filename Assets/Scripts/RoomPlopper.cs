using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomPlopper {
    LevelGenerator levelGenerator;
    public GameObject room;
    public GameObject street;
    int gridSize = 0;
    int maxSize = 10;
    int minSize = 5;
    GameObject floor;
    GameObject wall;

    List<int> xs;
    List<int> ys;
    List<Vector2> coords;

	// Use this for initialization
	public RoomPlopper(LevelGenerator lg, GameObject f, GameObject w)
    {
        levelGenerator = lg;
        gridSize = levelGenerator.level_grid_size;
        floor = f;
        wall = w;
        xs = new List<int>();
        ys = new List<int>();
        coords = new List<Vector2>();
        for(int x = 0; x<gridSize; x++){
            for(int y = 0; y<gridSize; y++){
                coords.Add(new Vector2(x,y));
            }
        }
    }
	
	public void PlopRooms()
    {
        
        for(int i = 0; i<(gridSize*gridSize); i++){
            int index = Random.Range(0,coords.Count);
            //Debug.Log(xs.Count);
            int x = (int)coords[index].x;
            int y = (int)coords[index].y;

            if (Random.Range(0.0f, 1.0f) > 0.0f)
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

        int rightBound = maxSize/2;
        int leftBound = maxSize/2;
        int upBound = maxSize/2;
        int downBound = maxSize/2;
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
                GameObject newRoom = GameObject.Instantiate(room, new Vector3(0,0,0), Quaternion.identity);
                
                //int interiorLayout = Random.Range(0,4);
                


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
                            levelGenerator.PlaceFloor(posX + i, posY + (velY * j), floor, Color.white, newRoom);
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
                            levelGenerator.PlaceFloor(posX - i, posY + (velY * j), floor, Color.white, newRoom);
                        }
                    }
                }

                int interiorLayout = Random.Range(0,4);
                //place interior walls
                if(interiorLayout == 1 && rightBound+leftBound>6 ){ //line accross
                    for(int i = 0; i<leftBound; i++){
                        levelGenerator.PlaceWall(posX - i, posY + (velY*(thirdBound/2)), wall, Color.white);
                    }
                    for(int i = 0; i<rightBound; i++){
                         levelGenerator.PlaceWall(posX + i, posY + (velY*(thirdBound/2)), wall, Color.white);
                    }
                    levelGenerator.PlaceDoor(Random.Range(posX - leftBound + 3, posX + rightBound-2), posY + (velY*(thirdBound/2)));
                }else if(interiorLayout == 2 && rightBound+leftBound>6 && leftBound > 2 && rightBound > 2){
                    for(int i = 0; i<leftBound; i++){
                        levelGenerator.PlaceWall(posX - i, posY + (velY*(thirdBound/2)), wall, Color.white);
                    }
                    for(int i = 0;i<thirdBound/2; i++){
                        levelGenerator.PlaceWall(posX, (posY + ((thirdBound/2) * velY)) + (i * velY), wall, Color.white);
                    }
                    levelGenerator.PlaceDoor(posX, posY + velY *  Random.Range((thirdBound/2)+1,(thirdBound)-1));
                }else if(interiorLayout == 3 && rightBound+leftBound>6 && rightBound > 2 && leftBound > 2){
                    for(int i = 0; i<rightBound; i++){
                        levelGenerator.PlaceWall(posX + i, posY + (velY*(thirdBound/2)), wall, Color.white);
                    }
                    for(int i = 0;i<thirdBound/2; i++){
                        levelGenerator.PlaceWall(posX, (posY + ((thirdBound/2) * velY)) + (i * velY), wall, Color.white);
                    }
                    levelGenerator.PlaceDoor(posX, posY + velY *  Random.Range((thirdBound/2)+1,(thirdBound)-1));
                }


                //levelGenerator.PlaceStreet(posX + ((rightBound - leftBound)/2), posY, street);  

                if(levelGenerator.SpaceIsFree(posX + ((rightBound - leftBound)/2), posY-1)){
                    levelGenerator.PlaceStreet(posX + ((rightBound - leftBound)/2), posY-1, street);    
                }else{
                    levelGenerator.PlaceStreet(posX + ((rightBound - leftBound)/2), posY+1, street);
                }
                
               levelGenerator.PlaceDoor(posX + ((rightBound - leftBound)/2), posY);
                
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

            //place tiles if appropriate area found
            if (thirdBound >= minSize && upBound + downBound >= minSize && upBound > 1 && downBound > 1)
            {
                //room is big enough to place
                GameObject newRoom = GameObject.Instantiate(room, new Vector3(0,0,0), Quaternion.identity);

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
                            levelGenerator.PlaceFloor(posX + (velX * j), posY + i, floor, Color.white, newRoom);
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
                            levelGenerator.PlaceFloor(posX + (velX * j), posY - i, floor, Color.white, newRoom);
                        }

                       
                    }

                }


                int interiorLayout = Random.Range(0,4);

                //place interior walls
                if(interiorLayout == 1 && upBound+downBound>6 ){ //line accross
                    for(int i = 0; i<downBound; i++){
                        levelGenerator.PlaceWall(posX + (velX*(thirdBound/2)), posY - i, wall, Color.white);
                     }
                    for(int i = 0; i<upBound; i++){
                         levelGenerator.PlaceWall(posX + (velX*(thirdBound/2)), posY + i , wall, Color.white);
                    }
                    levelGenerator.PlaceDoor(posX + (velX*(thirdBound/2)), Random.Range(posY - downBound + 3, posY + upBound-2));
                }else if(interiorLayout == 2 && upBound+downBound>6 && downBound > 2 && upBound > 2){
                    for(int i = 0; i<downBound; i++){
                        levelGenerator.PlaceWall(posX + (velX*(thirdBound/2)), posY - i , wall, Color.white);
                    }
                    for(int i = 0;i<thirdBound/2; i++){
                        levelGenerator.PlaceWall((posX + ((thirdBound/2) * velX)) + (i * velX), posY, wall, Color.white);
                    }
                    levelGenerator.PlaceDoor(posX + velX *  Random.Range((thirdBound/2)+1,(thirdBound)-1), posY);
                }else if(interiorLayout == 3 && upBound+downBound>6 && upBound > 2 && downBound > 2){
                    for(int i = 0; i<upBound; i++){
                        levelGenerator.PlaceWall(posX + (velX*(thirdBound/2)), posY + i, wall, Color.white);
                    }
                     for(int i = 0;i<thirdBound/2; i++){
                        levelGenerator.PlaceWall((posX + ((thirdBound/2) * velX)) + (i * velX), posY, wall, Color.white);
                    }
                     levelGenerator.PlaceDoor(posX + velX *  Random.Range((thirdBound/2)+1,(thirdBound)-1), posY);
                }





                //levelGenerator.PlaceStreet(posX, posY + ((upBound - downBound) / 2), street);
                if(levelGenerator.SpaceIsFree(posX-1, posY + ((upBound - downBound) / 2))){
                    levelGenerator.PlaceStreet(posX-1, posY + ((upBound - downBound) / 2), street);
                }else{
                    levelGenerator.PlaceStreet(posX+1, posY + ((upBound - downBound) / 2), street);
                }
                levelGenerator.PlaceDoor(posX, posY + ((upBound - downBound) / 2));
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
