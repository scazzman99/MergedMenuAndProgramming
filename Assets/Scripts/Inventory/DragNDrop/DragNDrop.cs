using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragNDrop : MonoBehaviour {

    #region Variables

    [Header("Inventory")]
    public bool showInv; //toggle ui
    public List<Item> inventory = new List<Item>(); //hold items
    public int slotX, slotY; //size x and y
    private Rect inventorySize;
    [Header("Dragging")]
    public bool isDragging; //are we dragging an item
    public Item draggedItem;
    public int draggedFrom; //where the item is being dragged from
    public GameObject droppedItem;
    [Header("Tool Tips")]
    public int toolTipItem; //index reference
    public bool showToolTip;
    private Rect toolTipRect;
    [Header("Reference and locations")]
    private Vector2 scr;
    #endregion

    #region ClampToScreen
    private Rect ClampToScreen(Rect r)
    {
        //make it so the rect cannot leave the screen in any way, hence screen width - rect width
        r.x = Mathf.Clamp(r.x, 0f, Screen.width - r.width);
        r.y = Mathf.Clamp(r.y, 0f, Screen.height - r.height);
        return r;
    }
    #endregion

    #region AddItem
    public void AddItem(int itemID)
    {
        for (int i = 0; i < inventory.Count; i++)
        {
            //find first spot in inventory that is empty and add item. leave function
            if (inventory[i].Name == null)
            {
                inventory[i] = ItemData.CreateItem(itemID);
                Debug.Log("Add item: " + inventory[i].Name);
                return; 
            }
        }
    }
    #endregion

    #region DropItem
    public void DropItem(int itemID)
    {
        //grabbing item from its ID
        droppedItem = Resources.Load("Prefabs/" + ItemData.CreateItem(itemID).Mesh) as GameObject;
        //creating the item we made and dropping it infront of us. Remember what the item is by assigning it to droppedItem
        droppedItem = Instantiate(droppedItem, transform.position + transform.forward * 3f, Quaternion.identity);
        //add a rigid body
        droppedItem.AddComponent<Rigidbody>().useGravity = true;
        //setting dropped item to null
        droppedItem = null;
    }
    #endregion

    #region Draw Item
    void DrawItem(int windowID)
    {
        if (draggedItem.Icon != null)
        {
            GUI.DrawTexture(new Rect(0, 0, scr.x * 0.5f, scr.y * 0.5f), draggedItem.Icon);
        }
    }
    #endregion

    #region ToolTip

    #region ToolTipContent
    private string ToolTipText(int itemID)
    {
        string toolTipText = "Name: " + inventory[itemID].Name + "Description: " + inventory[itemID].Description + "Type: " + inventory[itemID].Type +
            "Value: " + inventory[itemID].Value;

        return toolTipText;
    }
    #endregion

    #region ToolTipWindow
    void DrawToolTip(int windowID)
    {
        //have box text be text from toolTipItem
        GUI.Box(new Rect(0, 0, scr.x * 2, scr.y * 3), ToolTipText(toolTipItem));
    }
    #endregion

    #endregion

    #region ToggleInventory
    public bool ToggleInv()
    {
        if (showInv)
        {
            showInv = false;
            Time.timeScale = 1;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            return (false);
        }
        else
        {
            showInv = true;
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            return (true);

        }
    }
    #endregion

    #region Drag Inventory
    void InventoryDrag(int windowID)
    {
        GUI.Box(new Rect(0, scr.y * 0.25f, scr.x * 6f, scr.y * 0.5f), "Banner");
        GUI.Box(new Rect(0, scr.y * 4.25f, scr.x * 6f, scr.y * 0.5f), "Gold n Exp");
        showToolTip = false;

        #region NestedForLoop(gross)
        Event e = Event.current;
        int i = 0;
        for (int y = 0; y < slotY; y++)
        {
            for (int x = 0; x < slotX; x++)
            {
                Rect slotLocation = new Rect(scr.x * 0.125f + x * (scr.x * 0.75f), scr.y * 0.75f + y * (scr.y * 0.65f), scr.x * 0.75f, scr.y * 0.65f);
                GUI.Box(slotLocation, "");

                #region Pick Up Item
                /*
                    if we are interacting with leftmouse AND interaction was click down AND mousecursor is over item slot WHILE NOT dragging and item
                    AND item name at point in inv is NOT null AND we arent holding leftshift
                 */

                if(e.button == 0 && e.type == EventType.MouseDown && slotLocation.Contains(e.mousePosition) && !isDragging &&
                    inventory[i].Name != null && !Input.GetKey(KeyCode.LeftShift))
                {
                    //pick up item
                    draggedItem = inventory[i];
                    //inv slot is now empty
                    inventory[i] = new Item();
                    //we are holding an item
                    isDragging = true;
                    //we remember where this item came from
                    draggedFrom = i;
                    //debug
                    Debug.Log("Dragging: " + draggedItem.Name);
                }
                #endregion
                #region Swap Item

                /*
                 * if we lift up left mouse button AND we are dragging an item over a slot that has an item in it
                 */
                if (e.button == 0 && e.type == EventType.MouseUp && slotLocation.Contains(e.mousePosition) && isDragging &&
                    inventory[i].Name != null && !Input.GetKey(KeyCode.LeftShift))
                {
                    Debug.Log("Swapping: " + draggedItem.Name + " With: " + inventory[i].Name);
                    //the slot that is full now moves to where our dragged item had come from
                    inventory[draggedFrom] = inventory[i];
                    //slot we are dropping into now filled with dragged item
                    inventory[i] = draggedItem;
                    //dragged item is now empty (if we null it will throw nullreferenceexception)
                    draggedItem = new Item();
                    //we are no longer dragging
                    isDragging = false;
                }
                #endregion
                #region Place Item
                /*
                 * if we lift up left mouse button AND we are dragging an item over a slot that has NO item in it
                 */
                if (e.button == 0 && e.type == EventType.MouseUp && slotLocation.Contains(e.mousePosition) && isDragging &&
                    inventory[i].Name == null && !Input.GetKey(KeyCode.LeftShift))
                {
                    Debug.Log("Placing: " + draggedItem.Name + " Into: " + i);
                    //slot we are dropping into is filled with dragged item
                    inventory[i] = draggedItem;
                    //item we used to drag is empty
                    draggedItem = new Item();
                    //we are no longer holding an item
                    isDragging = false;
                }
                #endregion
                #region Return Item
                /*
                * if we lift up left mouse button AND we are dragging an item over anything that is not a slot
                * slotX*slotY -1 is the maximum i should ever reach
                */
                if (e.button == 0 && e.type == EventType.MouseUp && slotLocation.Contains(e.mousePosition) && i == ((slotX*slotY)-1) && isDragging)
                {
                    //put that item back where it came from or so help me
                    inventory[draggedFrom] = draggedItem;
                    //dragged item is now empty
                    draggedItem = new Item();
                    //we are no longer dragging
                    isDragging = false;

                    
                }
                #endregion
                #region Draw Item Icon
                if (inventory[i].Name != null)
                {
                    GUI.DrawTexture(slotLocation, inventory[i].Icon);
                    #region Set ToolTip on Mouse Hover
                    if(slotLocation.Contains(e.mousePosition) && !isDragging && showInv)
                    {
                        toolTipItem = i;
                        showToolTip = true;
                    }
                    #endregion
                }
                #endregion
                i++;
            }
        }
        #endregion

        #region Drag Points
        //these are less windows and more points to grab and drag the window
        GUI.DragWindow(new Rect(0,0,scr.x * 6f, scr.y*0.5f));//TOP drag
        GUI.DragWindow(new Rect(0,scr.y*0.5f,scr.x*0.25f,scr.y*3.5f));//LEFT drag
        GUI.DragWindow(new Rect(scr.x * 5.75f, scr.y * 0.25f, scr.x * 0.25f, scr.y * 3.5f));//RIGHT drag
        GUI.DragWindow(new Rect(0, scr.y * 4f, scr.x * 6f, scr.y * 0.5f));//BOT drag
        #endregion
    }
    #endregion

    #region Start
    private void Start()
    {
        scr.x = Screen.width / 16f;
        scr.y = Screen.height / 9f;
        inventorySize = new Rect(scr.x, scr.y, scr.x * 6f, scr.y * 4.5f);
        for (int i = 0; i < (slotX*slotY); i++)
        {
            inventory.Add(new Item());
        }

        //add some on start
        AddItem(3);
        AddItem(1);
        AddItem(100);
        AddItem(201);
        AddItem(300);
        AddItem(401);
    }
    #endregion

    #region Update
    private void Update()
    {
        //scale ui if screen size changes
        if(scr.x != Screen.width / 16f || scr.y != Screen.height / 9f)
        {
            scr.x = Screen.width / 16f;
            scr.y = Screen.height / 9f;
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleInv();
        }
    }
    #endregion

    #region OnGUI
    private void OnGUI()
    {
        Event e = Event.current;
        #region Inventory if showInv is true
        if (showInv)
        {
            inventorySize = ClampToScreen(GUI.Window(1, inventorySize, InventoryDrag, "Drag Inventory"));
        }
        #endregion

        #region ToolTip
        if(showToolTip && showInv)
        {
            toolTipRect = new Rect(e.mousePosition.x + 0.01f, e.mousePosition.y + 0.001f, scr.x * 2, scr.y * 3);
            //made 15 to make it ontop of everything (probs)
            GUI.Window(15, toolTipRect, DrawToolTip, "");

        }
        #endregion

        #region Drop Item (MouseUp || !showInv)
        //drop item if mouse up while dragging
        if((e.button == 0 && e.type == EventType.MouseUp && isDragging) || (!showInv && isDragging))
        {
            DropItem(draggedItem.Id);
            Debug.Log("Dropped: " + draggedItem.Name);
            draggedItem = new Item();
            isDragging = false;
        }
        #endregion

        #region Draw Item on Mouse
        if (isDragging)
        {
            //there is a part of a frame in which we should be dragging nothing so this covers us
            if(draggedItem != null)
            {
                Rect mouseLocation = new Rect(e.mousePosition.x + 0.125f, e.mousePosition.y + 0.125f, scr.x * 0.5f, scr.y * 0.5f);
                GUI.Window(2, mouseLocation, DrawItem, "");

            }
        }
        #endregion
    }
    #endregion
}
