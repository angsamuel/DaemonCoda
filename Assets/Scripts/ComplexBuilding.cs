using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComplexBuilding : MonoBehaviour
{
    public int max_dimension = 20;
    float block_unit = 1.6f;
    public GameObject[,] placed_blocks;
    public List<Vector2> rectangle_ranges;
    public List<Vector2> rectangle_pivots;
    public bool fill = false;

    public GameObject horz_block;
    public GameObject vert_block;
    // Use this for initialization
    void Start()
    {
        placed_blocks = new GameObject[max_dimension, max_dimension];
        rectangle_ranges = new List<Vector2>();
        rectangle_pivots = new List<Vector2>();

        if (!fill)
        {
            GenerateSelf();
        }
        else
        {
            FillSpace();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FillSpace()
    {
        for(int x = 0; x < max_dimension; x++)
        {
            for (int y = 0; y < max_dimension; y++)
            {
                PlaceBlock(x, y, vert_block);
            }
        }
    }

    void GenerateSelf()
    {

        int rectangles = Random.Range(1, 5);
        for (int r = 0; r < rectangles; r++)
        {
            BuildRectangle();
        }

        ClearInteriorWalls();
    }

    public int min_rect_size = 5;
    public int max_rect_size = 10;

    //build from bottom left corner
    void BuildRectangle()
    {
        int width = Random.Range(min_rect_size, max_rect_size + 1);
        int height = Random.Range(min_rect_size, max_rect_size + 1);

        int pivot_x = Random.Range(0, max_dimension - (width));
        int pivot_y = Random.Range(0, max_dimension - (height));

        //build horizontal wall
        for (int x = 0; x < width; x++)
        {
            PlaceBlock(pivot_x + x, pivot_y, horz_block);
            PlaceBlock(pivot_x + x, pivot_y + height - 1, horz_block);
        }

        //build vertical walls
        for (int y = 0; y < height; y++)
        {
            PlaceBlock(pivot_x, pivot_y + y, vert_block);
            PlaceBlock(pivot_x + width - 1, pivot_y + y, vert_block);
        }

        //add ranges
        print(new Vector2(pivot_x, pivot_y));
        rectangle_ranges.Add(new Vector2(width, height));
        rectangle_pivots.Add(new Vector2(pivot_x, pivot_y));
    }

    void PlaceBlock(int x, int y, GameObject building_block)
    {

        GameObject new_block = Instantiate(building_block, transform);
        new_block.transform.localPosition = new Vector2(-max_dimension/2, -max_dimension/2);
        new_block.transform.Translate(new Vector2(x, y) * block_unit);

        //slot block into grid
        if (placed_blocks[x, y] != null)
        {
            Destroy(placed_blocks[x, y]);
        }

        placed_blocks[x, y] = new_block;
    }

    void PlaceFloor(int x, int y, GameObject building_block)
    {

        GameObject new_block = Instantiate(building_block, transform);
        new_block.transform.localPosition = new Vector2(-max_dimension / 2, -max_dimension / 2);
        new_block.transform.Translate(new Vector2(x, y) * block_unit);

        //slot block into grid
        if (placed_blocks[x, y] != null)
        {
            Destroy(placed_blocks[x, y]);
        }

        placed_blocks[x, y] = new_block;
        new_block.GetComponent<SpriteRenderer>().color = Color.black;
    }

    void DestroyBlock(int x, int y)
    {
        if (placed_blocks[x, y] != null)
        {
            Destroy(placed_blocks[x, y]);
        }
    }

    void ClearInteriorWalls()
    {
        for (int i = 0; i < rectangle_ranges.Count; i++)
        {
            Debug.Log("deleting interior");
            Vector2 current_pivot = rectangle_pivots[i];
            Vector2 current_range = rectangle_ranges[i];

            Debug.Log(current_pivot);
            Debug.Log(current_range);

            for (int x = (int)current_pivot.x + 1; x < current_pivot.x + current_range.x - 1; x++)
            {
                for (int y = (int)current_pivot.y + 1; y < current_pivot.y + current_range.y - 1; y++)
                {
                    DestroyBlock(x, y);
                    //PlaceFloor(x, y);
                }
            }
        }
    }


}
