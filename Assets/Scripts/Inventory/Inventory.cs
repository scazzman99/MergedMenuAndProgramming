using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {

    #region Variables
    public List<Item> inv = new List<Item>();
    public static bool showInv = false; //show/hide inventory. Only one will exist
    public Item selectedItem; //item we are interacting with
    public int money; //current money
    public Vector2 scr = Vector2.zero; //screen ratio 16:9
    public Vector2 scrollPos = Vector2.zero; //scroll bar position (in scrolling area not screen)
    #endregion

    void Start () {
        //added item from ID number
        inv.Add(ItemData.CreateItem(0));
        inv.Add(ItemData.CreateItem(1));
        inv.Add(ItemData.CreateItem(201));
        inv.Add(ItemData.CreateItem(100));
        inv.Add(ItemData.CreateItem(401));
        inv.Add(ItemData.CreateItem(300));
        inv.Add(ItemData.CreateItem(999));

        //debugging
        for(int i = 0; i < inv.Count; i++)
        {
            Debug.Log(inv[i].Name);
        }
    }
	
	
	void Update () {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleInv();
        }
	}

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

    private void OnGUI()
    {
        //if the game isnt paused
        if (!PauseMenu.paused)
        {
            //if the inventory is showing
            if (showInv)
            {
                if(scr.x != Screen.width / 16 && scr.y != Screen.height / 9)
                {
                    scr.x = Screen.width / 16;
                    scr.y = Screen.height / 9;
                }
                //Space for the inventory (takes up screen)
                GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "Inventory");
                //if items fit in alloted space
                #region NonScrollInventory
                if(inv.Count <= 35)
                {
                    for(int i = 0; i < inv.Count; i++)
                    {
                        //show button for each item and set up condition if pressed
                        if(GUI.Button(new Rect(0.5f*scr.x, 0.25f*scr.y + (i * 0.25f * scr.y), 3*scr.x, 0.25f*scr.y ), inv[i].Name))
                        {
                            //if this item is selected, set it as the selected item
                            selectedItem = inv[i];
                            Debug.Log(selectedItem.Name);
                        }
                    }
                }
                #endregion
            //if items need aditional space
                #region ScrollInventory

                #endregion
            }
        }
    }
}
