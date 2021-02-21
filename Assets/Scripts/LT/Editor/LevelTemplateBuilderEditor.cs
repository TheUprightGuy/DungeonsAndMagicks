using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LevelTemplateBuilder))]
public class LevelTemplateBuilderEditor : Editor
{
    public enum Direction
    {
        none,
        north,
        east,
        south,
        west
    }

    //Arrow Settings
    private const float arrowSize = 0.5f;
    private const float CenterOffset = 1.5f;

    //Current tile and selected direction
    int ActiveIndex = -1;
    private Direction selectedDirection = Direction.none;
    
    //ID for Cone selections
    private int leftId, rightId, forwardId, backId;
    private bool cached = false;

    //.Net timers ended up being way simpler
    private static System.Timers.Timer aTimer;
    public bool ResetCol = false; //Used to mark end timer

    protected virtual void OnSceneGUI()
    {
        //Ensure that the tiles cannot be de-selected, because thats just annoying
        HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive)); 
        LevelTemplateBuilder t = target as LevelTemplateBuilder;


        if (t.Template != null)
        {
            if (t.Template.LevelLayout.Count == 0) //Atleast one tile is mandatory
            {
                t.Template.LevelLayout.Add(CreateTile(Vector2Int.zero, t.Template));
            }



            //Lightweight plane cast for point on the plane of rectangles
            /********************************************************************/
            Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
            Plane plane = new Plane(Vector3.up, t.transform.position);

            float dist = 0.0f; ;
            plane.Raycast(ray, out dist);

            Vector3 point = ray.GetPoint(dist);
            Vector2 flatPoint = new Vector2(point.x, point.z); //Convert to 2D for easy use with rect


            //Tile Drawing
            /********************************************************************/
            int count = 0; //Keep active count to set into the ActiveIndex
            foreach (TileInfo item in t.Template.LevelLayout)
            {
                float size = 1.0f;//Magic numbers can kiss my ass

                Vector2 size2 = new Vector2(size * 2, size * 2); //Don't ask why the multiplier is here, I don't know either
                Vector2 centre = new Vector2(t.transform.position.x + item.PosIndex.x * 2, //All I know is its important for positioning
                                            t.transform.position.z + item.PosIndex.y * 2);

                //Rects have the inbuilt Contains function for any point (flatpoint) 
                Rect handleRect = new Rect(centre - (size2 / 2), size2); 
                Handles.color = Color.cyan;
                if (handleRect.Contains(flatPoint))
                {
                    Handles.color = Color.blue;
                    ActiveIndex = count;
                }

                //The handle call does take a rect, but theres no documentation on setting the normal, so its orientated the wrong direction
                Vector3[] verts = new Vector3[]
                {
                    new Vector3(centre.x - size, t.transform.position.y, centre.y - size),
                    new Vector3(centre.x - size, t.transform.position.y, centre.y + size),
                    new Vector3(centre.x + size, t.transform.position.y, centre.y + size),
                    new Vector3(centre.x + size, t.transform.position.y, centre.y - size)
                };
                Handles.DrawSolidRectangleWithOutline(verts, new Color(0.5f, 0.5f, 0.5f, 0.1f), new Color(0, 0, 0, 1));

                count++;
            }

            //When a tile is moused over, ActiveIndex will be the index of the tile moused over, else none it will equal -1
            if (ActiveIndex >= 0) 
            {
                TileInfo activeTile = t.Template.LevelLayout[ActiveIndex];
                Vector3 currentActivePos = t.transform.position;
                currentActivePos.x += activeTile.PosIndex.x * 2; //Again, don't ask why the multiplier is need here
                currentActivePos.z += activeTile.PosIndex.y * 2;


                //First boot setup
                /************************************************************************/
                if (!cached)
                {
                    leftId = GUIUtility.GetControlID(FocusType.Passive);
                    rightId = GUIUtility.GetControlID(FocusType.Passive);
                    forwardId = GUIUtility.GetControlID(FocusType.Passive);
                    backId = GUIUtility.GetControlID(FocusType.Passive);

                    cached = true;
                }

                //All this crap is what it takes to draw a fucking cone onto Scene
                //The bools in each tileinfo are set based on the surrounding tiles, so
                //you cannot created a tile ontop of another
                /************************************************************************/
                if (Event.current.type == EventType.Repaint)
                {
                    if (activeTile.WestOpen)
                    {
                        Vector3 pos = currentActivePos + Vector3.left * CenterOffset;
                        Handles.color = selectedDirection == Direction.west ? Color.magenta : Color.yellow;
                        Handles.ConeHandleCap(leftId, pos, Quaternion.LookRotation(Vector3.left), arrowSize, EventType.Repaint);
                    }
                    if (activeTile.EastOpen)
                    {
                        Vector3 pos = currentActivePos + Vector3.right * CenterOffset;
                        Handles.color = selectedDirection == Direction.east ? Color.magenta : Color.red;
                        Handles.ConeHandleCap(rightId, pos, Quaternion.LookRotation(Vector3.right), arrowSize, EventType.Repaint);
                    }
                    if (activeTile.NorthOpen)
                    {
                        Vector3 pos = currentActivePos + Vector3.forward * CenterOffset;
                        Handles.color = selectedDirection == Direction.north ? Color.magenta : Color.blue;
                        Handles.ConeHandleCap(forwardId, pos, Quaternion.LookRotation(Vector3.forward), arrowSize, EventType.Repaint);
                    }
                    if (activeTile.SouthOpen)
                    {
                        Vector3 pos = currentActivePos + Vector3.back * CenterOffset;
                        Handles.color = selectedDirection == Direction.south ? Color.magenta : Color.cyan;
                        Handles.ConeHandleCap(backId, pos, Quaternion.LookRotation(Vector3.back), arrowSize, EventType.Repaint);
                    }
                }
                else if (Event.current.type == EventType.Layout)
                {
                    if (activeTile.WestOpen)
                    {
                        Vector3 pos = currentActivePos + Vector3.left * CenterOffset;
                        Handles.ConeHandleCap(leftId, pos, Quaternion.LookRotation(Vector3.left), arrowSize, EventType.Layout);
                    }
                    if (activeTile.EastOpen)
                    {
                        Vector3 pos = currentActivePos + Vector3.right * CenterOffset;
                        Handles.ConeHandleCap(rightId, pos, Quaternion.LookRotation(Vector3.right), arrowSize, EventType.Layout);
                    }
                    if (activeTile.NorthOpen)
                    {
                        Vector3 pos = currentActivePos + Vector3.forward * CenterOffset;
                        Handles.ConeHandleCap(forwardId, pos, Quaternion.LookRotation(Vector3.forward), arrowSize, EventType.Layout);
                    }
                    if (activeTile.SouthOpen)
                    {
                        Vector3 pos = currentActivePos + Vector3.back * CenterOffset;
                        Handles.ConeHandleCap(backId, pos, Quaternion.LookRotation(Vector3.back), arrowSize, EventType.Layout);
                    }
                }

                //Handle clicks onto handles
                /************************************************************************/
                else if (Event.current.type == EventType.MouseDown)
                {
                    //Get ID clicked
                    int id = HandleUtility.nearestControl;

                    //set direction based on ID
                    if (id == leftId) selectedDirection = Direction.west;
                    else if (id == rightId) selectedDirection = Direction.east;
                    else if (id == forwardId) selectedDirection = Direction.north;
                    else if (id == backId) selectedDirection = Direction.south;
                    else selectedDirection = Direction.none;

                    //Debug.Log(selectedDirection);


                    if (selectedDirection != Direction.none)
                    {
                        Vector2Int newPosIndex = activeTile.PosIndex; //Set new index based on current active tile pos and direction clicked
                        switch (selectedDirection)
                        {
                            case Direction.north:
                                newPosIndex.y++;
                                break;
                            case Direction.east:
                                newPosIndex.x++;
                                break;
                            case Direction.south:
                                newPosIndex.y--;
                                break;
                            case Direction.west:
                                newPosIndex.x--;
                                break;
                            default:
                                break;
                        }
                        t.Template.LevelLayout.Add(CreateTile(newPosIndex, t.Template));

                        for (int i = 0; i < t.Template.LevelLayout.Count; i++)
                        {
                            t.Template.LevelLayout[i] = SetTileBools(t.Template.LevelLayout[i], t.Template);
                        }

                        // Create a timer and set a two second interval.
                        aTimer = new System.Timers.Timer();
                        aTimer.Interval = 100;

                        // Hook up the Elapsed event for the timer. 
                        aTimer.Elapsed += OnTimedEvent;

                        // Have the timer fire repeated events (true is the default)
                        aTimer.AutoReset = true;

                        // Start the timer
                        aTimer.Enabled = true;
                    }

                }


                //Deleting a tile with the D key
                /************************************************************************/
                else if (Event.current.isKey && 
                        Event.current.type.Equals(EventType.KeyDown) &&
                        Event.current.keyCode == KeyCode.X) //X for delete
                {
                    Debug.Log("d");
                    t.Template.LevelLayout.RemoveAt(ActiveIndex); //Remove at current index
                    ActiveIndex = -1; //Reset the activeindex to none
                    for (int i = 0; i < t.Template.LevelLayout.Count; i++) //refresh the tilebools
                    {
                        t.Template.LevelLayout[i] = SetTileBools(t.Template.LevelLayout[i], t.Template);
                    }
                }

                if (ResetCol)//If timer done this bool will set
                {
                    ResetCol = false;
                    selectedDirection = Direction.none;
                }

            }
        }
        Repaint();
    }

    //Timer event
    private void OnTimedEvent(object source, System.Timers.ElapsedEventArgs e)
    {
        //Debug.Log("The Elapsed event was raised at " + e.SignalTime);
        aTimer.Stop();
        aTimer.Dispose();
        ResetCol = true;
    }

    //Create tileinfo with default values based on _PosIndex
    //Returns the new TileInfo struct
    TileInfo CreateTile(Vector2Int _PosIndex, LevelTemplate target)
    {
        TileInfo StartTile = new TileInfo();

        StartTile = SetTileBools(StartTile, target);

        StartTile.PosIndex = _PosIndex;
        return StartTile;
    }
    TileInfo SetTileBools(TileInfo _tileInfo, LevelTemplate target)
    {
        Vector2Int _PosIndex = _tileInfo.PosIndex;
        TileInfo returnTile = _tileInfo;

        returnTile.NorthOpen = true;
        returnTile.EastOpen = true;
        returnTile.SouthOpen = true;
        returnTile.WestOpen = true;

        foreach (TileInfo item in target.LevelLayout)
        {
            Vector2Int temp = _PosIndex;
            temp.y++;
            //North
            if (item.PosIndex == temp)
            {
                returnTile.NorthOpen = false;
            }

            temp = _PosIndex;
            temp.x++;
            //West
            if (item.PosIndex == temp)
            {
                returnTile.EastOpen = false;
            }

            temp = _PosIndex;
            temp.y--;
            //South
            if (item.PosIndex == temp)
            {
                returnTile.SouthOpen = false;
            }

            temp = _PosIndex;
            temp.x--;
            //East
            if (item.PosIndex == temp)
            {
                returnTile.WestOpen = false;
            }

        }
            return (returnTile);
    }

    //Handling creating new levels
    public override void OnInspectorGUI()
    {
        LevelTemplateBuilder t = target as LevelTemplateBuilder;


        base.OnInspectorGUI();

        GUILayout.Space(5);


        if (GUILayout.Button("Create New"))
        {
            string filepath = "Assets/" + t.FilePath +
                    ((t.FilePath == "") ? ("") : ("/"))
                    + t.LevelName
                    + ".asset";

            if ((LevelTemplate)AssetDatabase.LoadAssetAtPath(filepath, typeof(LevelTemplate)) == null)
            {
                LevelTemplate NewTemplate = ScriptableObject.CreateInstance<LevelTemplate>();
                AssetDatabase.CreateAsset(NewTemplate, filepath);
                AssetDatabase.SaveAssets();

                t.Template = (LevelTemplate)AssetDatabase.LoadAssetAtPath(filepath, typeof(LevelTemplate));
            }
            else
            {
                Debug.Log("File Already Exists");
            }
        }
        GUI.enabled = true;

    }
}
   
