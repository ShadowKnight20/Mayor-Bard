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

    public GameObject tavern;
    public GameObject townHall;

    private PlaceableObject objectToPlace;

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
        if (Input.GetKeyDown(KeyCode.T))
        {
            InitializeWithObject(tavern);
            cameraMovement.offset = new Vector3(0, 35f, -15f);
        }
        else if (Input.GetKeyDown(KeyCode.H))
        {
            InitializeWithObject(townHall);
            cameraMovement.offset = new Vector3(0, 35f, -15f);
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
            if (CanBePlaced(objectToPlace))
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
