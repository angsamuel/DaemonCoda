using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour {
    public string villageName;
    public NameWizard nameWizard;

    public GameObject leaveLevelBarrier;

    float blockScale = .14f;
    float colorVariance = .0f;
    public LevelController levelController;
    public GameObject patrolRoute;
    [HideInInspector] public List<PatrolRoute> patrolRoutes = new List<PatrolRoute>();
    public GameObject checkpoint;
    public GameObject room;
    public GameObject shadeBlock;
    public GameObject streetBlock;
    public GameObject floorBlock;
    public GameObject wallBlock;
    public GameObject doorBlock;
    public GameObject medPak;
    public GameObject mealPak;

    public List<GameObject> plants;
    public List<GameObject> furniture;

    public Color streetColor;
    public Color floorColor;
    public Color wallColor;
    public Color plantColor;

    public GameObject husk;

    public int level_grid_size = 200;
    public List<StreetCrawler> streetCrawlers;
    List<Roomie> roomies;
    public GameObject[,] levelGrid;
    Coroutine currentGeneration;
    Coroutine currentPlop;

    float spawnHuskChance = 0.01f;
    float spawnMedPakChance = 0.01f;
    float spawnMealPakChance = 0.01f;
    float blockOffset = 1.12f;
    public LevelPopulator lp;

    // Use this for initialization
	void Start () {
        villageName = nameWizard.GenerateVillageName();
        streetCrawlers = new List<StreetCrawler>();
        roomies = new List<Roomie>();
        levelGrid = new GameObject[level_grid_size, level_grid_size];
        Generate();
        lp.Populate();
        ScaleLeaveLevelBarrier();
    }

    void ScaleLeaveLevelBarrier(){
        switch(level_grid_size){
            case 50:
                leaveLevelBarrier.transform.localScale = new Vector3(9,9,1);
                leaveLevelBarrier.transform.localPosition = new Vector3(28,28);
                break;
            case 75:
                leaveLevelBarrier.transform.localScale = new Vector3(12,12,1);
                leaveLevelBarrier.transform.localPosition = new Vector3(41.5f,41.5f);
                break;
            case 100:
                leaveLevelBarrier.transform.localScale = new Vector3(15,15,1);
                leaveLevelBarrier.transform.localPosition = new Vector3(55,55);
                break;
            case 125:
                leaveLevelBarrier.transform.localScale = new Vector3(17.5f,17.5f,1);
                leaveLevelBarrier.transform.localPosition = new Vector3(69.5f,69.5f);
                break;
            case 150:
                leaveLevelBarrier.transform.localScale = new Vector3(20,20,1);
                leaveLevelBarrier.transform.localPosition = new Vector3(83,83);
                break;
            default:
                break;
        }
    }

   public PatrolRoute CreatePatrolRoute(){
       PatrolRoute pr = Instantiate(patrolRoute, transform).GetComponent<PatrolRoute>();
       patrolRoutes.Add(pr);
       return pr;
   }

   public void CreateCheckpoint(PatrolRoute pr, int x, int y){
       if(levelGrid[x,y] !=null){
            GameObject newCheckPoint = Instantiate(checkpoint, levelGrid[x,y].transform);
            newCheckPoint.transform.localPosition = new Vector3(0,0,0);
            pr.checkpoints.Add(newCheckPoint);
       }
   }

    public void Populate()
    {
        for(int y = 0; y < level_grid_size; y++)
        {
            for(int x = 0; x < level_grid_size; x++)
            {
                if(levelGrid[x,y] != null)
                {
                    if(levelGrid[x,y].tag == "street")
                    {
                        //spawn husk
                            if(Random.Range(0.0f, 1.0f) < spawnHuskChance)
                            {
                                Instantiate(husk, levelGrid[x, y].transform.position, Quaternion.identity);
                            }
                        
                    }else if(levelGrid[x,y].tag == "floor"){
                        if(levelGrid[x-1,y].tag != "wall" && levelGrid[x+1,y].tag != "wall" && levelGrid[x,y-1].tag != "wall" && levelGrid[x,y+1].tag != "wall"){
                            if(Random.Range(0.0f, 1.0f) < spawnHuskChance)
                            {
                                Instantiate(husk, levelGrid[x, y].transform.position, Quaternion.identity);
                            }
                        }
                    }
                }
            }
        }
    }
    float spawnFurnitureChance = 0.05f;
    public void FillWithLoot(){
        
        for(int y = 0; y < level_grid_size; y++)
        {
            for(int x = 0; x < level_grid_size; x++)
            {
                if(levelGrid[x,y] != null && levelGrid[x,y].tag=="floor"){
                    int medOrFood = Random.Range(0,2);
                    float roll = Random.Range(0.0f, 1.0f);
                    if(medOrFood == 0){
                        if(roll <= spawnMedPakChance){
                            Instantiate(medPak,levelGrid[x,y].transform.position, Quaternion.identity);
                        }
                    }else{
                        if(roll <= spawnMealPakChance){
                            Instantiate(mealPak,levelGrid[x,y].transform.position, Quaternion.identity);
                        }
                    }
                    if(roll < spawnFurnitureChance){
                        Instantiate(furniture[Random.Range(0,furniture.Count)],levelGrid[x,y].transform.position + new Vector3(Random.Range(-.5f, .5f), Random.Range(-.5f, .5f)), Quaternion.Euler(0.0f, 0.0f, Random.Range(0.0f, 360.0f)));
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
        //currentGeneration = StartCoroutine(Generate());
        Generate();
    }

    void Generate()
    {

        StreetCrawler sc = new StreetCrawler(this, 5, new Vector2(level_grid_size/2, 0), 0, streetBlock);
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

        RoomPlopper rm = new RoomPlopper(this, floorBlock, wallBlock);
        rm.street = streetBlock;
        rm.room = room;
        rm.PlopRooms();
        PlacePlants();
        transform.localScale = new Vector2(1.2f, 1.2f);

        //Populate();
        FillWithLoot();
        
        GameObject player = GameObject.Find("PlayerInputController");
        player.transform.position = levelGrid[level_grid_size/2,0].transform.position;
        player.transform.Translate(new Vector2(0,-8));
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
            newBlock.transform.Translate(new Vector2(x, y) * blockOffset);
            levelGrid[x, y] = newBlock;
            newBlock.GetComponent<SpriteRenderer>().color = wallColor;

            float variance = Random.Range(-colorVariance, colorVariance);
            newBlock.GetComponent<SpriteRenderer>().color = new Color(wallColor.r + variance, wallColor.g + variance, wallColor.b + variance, 1f);
            //newBlock.GetComponent<SpriteRenderer>().transform.localScale = new Vector2(Random.Range(.125f, .15f), Random.Range(.125f, .15f));
            newBlock.GetComponent<SpriteRenderer>().transform.localScale = new Vector2(blockScale, blockScale);
        }
    }

    GameObject PlaceBlock(int x, int y, GameObject block, Color c){
       
        if (x > -1 && y > -1 && x < level_grid_size && y < level_grid_size)
        {
            if(levelGrid[x,y] !=null){
                Destroy(levelGrid[x,y]);
            }   
            GameObject newBlock = Instantiate(block, transform);
            newBlock.GetComponent<SpriteRenderer>().color = c;
            newBlock.transform.Translate(new Vector2(x, y) * blockOffset);
            newBlock.transform.localScale = new Vector2(blockScale, blockScale);
            newBlock.GetComponent<SpriteRenderer>().color = c;
            newBlock.GetComponent<SpriteRenderer>().transform.localScale = new Vector2(blockScale, blockScale);
            levelGrid[x,y] = newBlock;
            return newBlock;
        }
        return null;
    }

    float spawnPlantChance = 0.40f;
     void PlacePlants(){
        for(int y = 0; y < level_grid_size; y++)
        {
            for(int x = 0; x < level_grid_size; x++)
            {
                
                if(levelGrid[x,y] == null){
                    if(Random.Range(0.0f, 1.0f) < spawnPlantChance){
                        PlaceBlock(x,y,plants[Random.Range(0,plants.Count)], plantColor);
                    }
                 //   GameObject newPlant = Instantiate(plants[Random.Range(0,plants.Count)], transform);
                 //   newPlant.transform.Translate(new Vector2(x, y) * blockOffset);
                 //   newPlant.transform.localScale = new Vector2(blockScale, blockScale);
                 //   newPlant.GetComponent<SpriteRenderer>().color = plantColor;
                }
            }
        }
    }

    public GameObject PlaceStreet(int x, int y, GameObject block)
    {
        return PlaceStreet(x, y, block, Color.black);
    }

    public GameObject PlaceStreet(int x, int y, GameObject block, Color c)
    {
       return PlaceBlock(x,y,block,streetColor);
    }


    public void PlaceFloor(int x, int y, GameObject floor, Color c, GameObject ro)
    {
        Room r = ro.GetComponent<Room>();
        GameObject newBlock = PlaceBlock(x,y,floor,floorColor);
        newBlock.GetComponent<InteriorFloor>().room = r;
       // GameObject newShadeBlock = Instantiate(shadeBlock, transform);
       // newShadeBlock.GetComponent<SpriteRenderer>().color = Color.black;

       // newShadeBlock.transform.position = newBlock.transform.position;
       // newShadeBlock.transform.localScale = new Vector2(blockScale,blockScale);
       // r.AddShadow(newShadeBlock);
    }

    public void PlaceDoor(int x, int y){
        GameObject newDoor = Instantiate(doorBlock, transform);
        if(levelGrid[x,y] != null){
            Destroy(levelGrid[x,y]);
        }
        levelGrid[x,y] = newDoor;
        newDoor.GetComponent<SpriteRenderer>().transform.localScale = new Vector2(blockScale, blockScale);
        newDoor.transform.Translate(new Vector2(x, y) * blockOffset);
    
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
