using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GridBuildingSystem;
using UnityEngine.Tilemaps;

public class BuildingSystem : MonoBehaviour
{
    public static BuildingSystem current;

    public GridLayout gridLayout;
    private Grid grid;
    [SerializeField] private Tilemap MainTilemap;
    [SerializeField] private TileBase whiteTile;
    //Shortcut key for each are:
    public GameObject tavern;   // 1
    public GameObject townHall; // 2
    public GameObject farm;     // 3
    public GameObject sawMill;  // 4
    public GameObject mine;     // 5
    public GameObject house;    // 6

    public string notEnoughResources;

    public bool isEnoughResourceToBuild;

    private PlaceableObject objectToPlace;

    private ResourceManager resourceManager;

    #region Unity methods
    private void Awake()
    {
        current = this;
        grid = gridLayout.gameObject.GetComponent<Grid>();
    }
    #endregion

    private void Update() //Temp with buttons to spawn buildings
    {
        CameraMovement cameraMovement = Camera.main.GetComponent<CameraMovement>();
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            InitializeWithObject(tavern);
            cameraMovement.offset = new Vector3(0, 45f, -15f);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            InitializeWithObject(townHall);
            cameraMovement.offset = new Vector3(0, 45f, -15f);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            InitializeWithObject(farm);
            cameraMovement.offset = new Vector3(0, 45f, -15f);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            InitializeWithObject(sawMill);
            cameraMovement.offset = new Vector3(0, 45f, -15f);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            InitializeWithObject(mine);
            cameraMovement.offset = new Vector3(0, 45f, -15f);
            
            if (resourceManager.wood >= 10)
            {
                isEnoughResourceToBuild = true;
            }
            else
            {
                isEnoughResourceToBuild = false;
                //notEnoughResources = "Requires" + ;
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            InitializeWithObject(house);
            cameraMovement.offset = new Vector3(0, 45f, -15f);
        }
        if (!objectToPlace)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            objectToPlace.Rotate();
        }
        else if (Input.GetKeyDown(KeyCode.P)) //maybemouse click
        {
            if (CanBePlaced(objectToPlace) && isenoughResourceToBuild)
            {
                objectToPlace.Place();
                Vector3Int start = gridLayout.WorldToCell(objectToPlace.GetStartPosition());
                //TakeArea(start, objectToPlace.Size);
                TakeArea(start, Vector3Int.FloorToInt(objectToPlace.Size));
                cameraMovement.offset = new Vector3(0, 10f, -15f);
            }
            else
            {
                Destroy(objectToPlace.gameObject);
                cameraMovement.offset = new Vector3(0, 10f, -15f);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Escape)) //mayberightclick
        {
            Destroy(objectToPlace.gameObject);
            cameraMovement.offset = new Vector3(0, 10f, -15f);
        }
    }

    #region Utils

    public static Vector3 GetMouseWorlPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out RaycastHit raycastHit))
        {
            return raycastHit.point;
        }
        else
        {
            return Vector3.zero;
        }
    }
    public Vector3 SnapCoordinateToGrid(Vector3 position)
    {
        Vector3Int cellPos = gridLayout.WorldToCell(position);
        position = grid.GetCellCenterWorld(cellPos);
        return position;
    }
    #endregion

    private static TileBase[] GetTilesBlock(BoundsInt area,Tilemap tilemap)
    {
        TileBase[] array = new TileBase[area.size.x * area.size.y * area.size.z];
        int counter = 0;

        foreach (var v in area.allPositionsWithin)
        {
            Vector3Int pos = new Vector3Int(v.x,v.y, 0);
            array[counter] = tilemap.GetTile(pos);
            counter++;
        }
        return array;
    }

    #region Building Placement

    public void InitializeWithObject(GameObject prefab)
    {
        Vector3 position = SnapCoordinateToGrid(Vector3.zero);

        GameObject obj = Instantiate(prefab,position,Quaternion.identity);
        objectToPlace = obj.GetComponent<PlaceableObject>();
        obj.AddComponent<ObjectDrag>();
    }

    /*private bool CanBePlaced(PlaceableObject placeableObject)
    {
        BoundsInt area = new BoundsInt();
        area.position = gridLayout.WorldToCell(objectToPlace.GetStartPosition());
        area.size = placeableObject.Size;

        TileBase[] baseArray = GetTilesBlock(area, MainTilemap);

        foreach (var b in baseArray)
        {
            if (b == whiteTile)
            {
                return false;
            }
        }
        return true;
    }*/
    private bool CanBePlaced(PlaceableObject placeableObject)
    {
        BoundsInt area = new BoundsInt();
        area.position = gridLayout.WorldToCell(objectToPlace.GetStartPosition()); // Corrected to use Vector3Int
        //area.size = placeableObject.Size;
        area.size = Vector3Int.FloorToInt(placeableObject.Size);

        TileBase[] baseArray = GetTilesBlock(area, MainTilemap);

        foreach (var b in baseArray)
        {
            if (b == whiteTile)
            {
                return false;
            }
        }
        return true;
    }

    public void TakeArea(Vector3Int start, Vector3Int size)
    {
        //MainTilemap.BoxFill(start,whiteTile,start.x,start.y,
        //start.x + size.x, start.y + size.y);

        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                Vector3Int tilePosition = new Vector3Int(start.x + x, start.y + y, 0);
                MainTilemap.SetTile(tilePosition, whiteTile);
            }
        }
    }

    #endregion

}
