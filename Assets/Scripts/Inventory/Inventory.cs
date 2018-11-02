using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{

    #region Variables
    //access from anywhere using static
    public static List<Item> inv = new List<Item>();
    public static bool showInv = false; //show/hide inventory. Only one will exist
    public Item selectedItem; //item we are interacting with
    public static int money; //current money
    public Vector2 scr = Vector2.zero; //screen ratio 16:9
    public Vector2 scrollPos = Vector2.zero; //scroll bar position (in scrolling area not screen)
    string sortType = "";
    #endregion

    void Start()
    {
        //added item from ID number
        inv.Add(ItemData.CreateItem(0));
        inv.Add(ItemData.CreateItem(1));
        inv.Add(ItemData.CreateItem(201));
        inv.Add(ItemData.CreateItem(100));
        inv.Add(ItemData.CreateItem(401));
        inv.Add(ItemData.CreateItem(300));
        

        //debugging
        for (int i = 0; i < inv.Count; i++)
        {
            Debug.Log(inv[i].Name);
        }
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleInv();
        }


        //This is a debug
        /*if (Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            if (showInv)
            {
                inv.Add(ItemData.CreateItem(0));
                inv.Add(ItemData.CreateItem(1));
                inv.Add(ItemData.CreateItem(201));
                inv.Add(ItemData.CreateItem(100));
                inv.Add(ItemData.CreateItem(401));
                inv.Add(ItemData.CreateItem(300));
                inv.Add(ItemData.CreateItem(999));
            }
        }
        */
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
                if (scr.x != Screen.width / 16 && scr.y != Screen.height / 9)
                {
                    scr.x = Screen.width / 16;
                    scr.y = Screen.height / 9;
                }
                //Space for the inventory (takes up screen)
                GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "Inventory");



                //attempt better later
                #region SortingButtons 


                if (GUI.Button(new Rect(5.5f * scr.x, 0.25f * scr.y, scr.x, 0.25f * scr.y), "All"))
                {
                    sortType = "All";
                }
                if (GUI.Button(new Rect(6.5f * scr.x, 0.25f * scr.y, scr.x, 0.25f * scr.y), "Consumables"))
                {
                    sortType = "Consumables";
                }
                if (GUI.Button(new Rect(7.5f * scr.x, 0.25f * scr.y, scr.x, 0.25f * scr.y), "Weapons"))
                {
                    sortType = "Weapon";
                }
                if (GUI.Button(new Rect(8.5f * scr.x, 0.25f * scr.y, scr.x, 0.25f * scr.y), "Armour"))
                {
                    sortType = "Armour";
                }
                if (GUI.Button(new Rect(9.5f * scr.x, 0.25f * scr.y, scr.x, 0.25f * scr.y), "Craftables"))
                {
                    sortType = "Craftable";
                }
                if (GUI.Button(new Rect(10.5f * scr.x, 0.25f * scr.y, scr.x, 0.25f * scr.y), "Misc"))
                {
                    sortType = "Misc";
                }
                if (GUI.Button(new Rect(11.5f * scr.x, 0.25f * scr.y, scr.x, 0.25f * scr.y), "Quest"))
                {
                    sortType = "Quest";
                }

                #endregion

                Debug.Log(sortType);


                DisplayInv(sortType);

                //if we have a selected item
                if (selectedItem != null)
                {
                    //show item
                    GUI.DrawTexture(new Rect(10 * scr.x, 1.5f * scr.y, 2f * scr.x, 2f * scr.y), selectedItem.Icon);

                    switch (selectedItem.Type)
                    {
                        case ItemTypes.Consumables:

                            GUI.Box(new Rect(7 * scr.x, 5f * scr.y, 8 * scr.x, 3 * scr.y), selectedItem.Name + "\n" + selectedItem.Description + "\nValue: " + selectedItem.Value
                                + "\nHeal: " + selectedItem.Heal + "\nAmount: " + selectedItem.Amount);
                            if (CharHealthHandler.currentHP < CharHealthHandler.maxHP)
                            {
                                if (GUI.Button(new Rect(12 * scr.x, 8.75f * scr.y, scr.x, 0.25f * scr.y), "Munch"))
                                {
                                    CharHealthHandler.currentHP += selectedItem.Heal;
                                    //if amount of item greater than 1
                                    if (selectedItem.Amount > 1)
                                    {
                                        //deplete amount by 1
                                        selectedItem.Amount--;
                                    }
                                    else //else
                                    {
                                        //remove item from inv and set selectedItem to null
                                        inv.Remove(selectedItem);
                                        selectedItem = null;
                                    }
                                    return;
                                }
                            }
                            break;

                        case ItemTypes.Craftable:

                            if (GUI.Button(new Rect(12 * scr.x, 8.75f * scr.y, scr.x, 0.25f * scr.y), "Slam"))
                            {

                            }
                            break;

                        case ItemTypes.Misc:

                            if (GUI.Button(new Rect(12 * scr.x, 8.75f * scr.y, scr.x, 0.25f * scr.y), "Use"))
                            {
                                //craft sys
                            }
                            break;

                        case ItemTypes.Armour:

                            GUI.Box(new Rect(7 * scr.x, 5f * scr.y, 8 * scr.x, 3 * scr.y), selectedItem.Name + "\n" + selectedItem.Description + "\nValue: " + selectedItem.Value
                                + "\nArmour: " + selectedItem.Armour);

                            if (GUI.Button(new Rect(12 * scr.x, 8.75f * scr.y, scr.x, 0.25f * scr.y), "Equip"))
                            {

                            }
                            break;

                        case ItemTypes.Weapon:

                            GUI.Box(new Rect(7 * scr.x, 5f * scr.y, 8 * scr.x, 3 * scr.y), selectedItem.Name + "\n" + selectedItem.Description + "\nValue: " + selectedItem.Value
                                + "\nDamage: " + selectedItem.Damage);

                            if (GUI.Button(new Rect(12 * scr.x, 8.75f * scr.y, scr.x, 0.25f * scr.y), "Equip"))
                            {

                            }
                            break;

                        case ItemTypes.Quest:

                            if (GUI.Button(new Rect(12 * scr.x, 8.75f * scr.y, scr.x, 0.25f * scr.y), "Inspect"))
                            {

                            }
                            break;
                    }

                    if (selectedItem.Type != ItemTypes.Quest)
                    {
                        if (GUI.Button(new Rect(13 * scr.x, 8.75f * scr.y, scr.x, 0.25f * scr.y), "Discard"))
                        {
                            //spawn item on ground
                            inv.Remove(selectedItem);
                            selectedItem = null;
                            return;
                        }
                    }

                }
            }
        }
    }

    void DisplayInv(string sortType)
    {

        if (!(sortType == "All" || sortType == ""))
        {
            ItemTypes type = (ItemTypes)System.Enum.Parse(typeof(ItemTypes), sortType);
            int a = 0; //amount of items of the type given
            int s = 0; //'slots', how many buttons we have made, incriments to put one under another when looping

            #region Check number of item type

            for (int i = 0; i < inv.Count; i++)
            {
                //if the item type of item matches given type
                if (inv[i].Type == type)
                {
                    //add to count of items of type
                    a++;
                }
            }

            #endregion

            #region Create item buttons

            //much like in our regular assignment of inventory, 35 items or less doesnt require a scroll area. Same for both below except one is encapuslated in scroll area
            #region No scroll area (35 items of type or less)
            if (a <= 35)
            {
                //search entire inventory to find items of type
                for (int i = 0; i < inv.Count; i++)
                {
                    //if type of item at i matches type
                    if (inv[i].Type == type)
                    {
                        //show button for each item and set up condition if pressed
                        //use 's' here to adjust position based on number of buttons we have made already
                        if (GUI.Button(new Rect(0.5f * scr.x, 0.25f * scr.y + (s * 0.25f * scr.y), 3 * scr.x, 0.25f * scr.y), inv[i].Name))
                        {
                            //if this item is selected, set it as the selected item
                            selectedItem = inv[i];
                            Debug.Log(selectedItem.Name);
                        }
                        //incriment s to have next button be below
                        s++;
                    }
                }
            }
            #endregion


            #region With Scroll Area (>35 items of type)

            else
            {
                //make scroll area using (a -35) to get view rect additional space for offscreen buttons
                scrollPos = GUI.BeginScrollView(new Rect(0, 0.25f * scr.y, 3.75f * scr.x, 8.75f * scr.y), scrollPos, new Rect(0, 0, 0, 8.75f * scr.y + ((a - 35) * (0.25f * scr.y))), false, true);

                for (int i = 0; i < inv.Count; i++)
                {
                    if (inv[i].Type == type)
                    {
                        //show button for each item and set up condition if pressed
                        if (GUI.Button(new Rect(0.5f * scr.x, 0.25f * scr.y + (s * 0.25f * scr.y), 3 * scr.x, 0.25f * scr.y), inv[i].Name))
                        {
                            //if this item is selected, set it as the selected item
                            selectedItem = inv[i];
                            Debug.Log(selectedItem.Name);
                        }
                        s++;
                    }
                }

                GUI.EndScrollView();
            }

            #endregion


            #endregion


        }
        else
        {
            //if items fit in alloted space
            #region NonScrollInventory
            if (inv.Count <= 35)
            {
                for (int i = 0; i < inv.Count; i++)
                {
                    //show button for each item and set up condition if pressed
                    if (GUI.Button(new Rect(0.5f * scr.x, 0.25f * scr.y + (i * 0.25f * scr.y), 3 * scr.x, 0.25f * scr.y), inv[i].Name))
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
            //else we are greater than 35
            else
            {
                //anything between beginscrollview and endscrollview will be in the scroll view

                //our moved position in scrolling. Making sure that we add the buttons worht of space for every button we go past screen limit
                scrollPos = GUI.BeginScrollView(new Rect(0, 0.25f * scr.y, 3.75f * scr.x, 8.75f * scr.y), scrollPos, new Rect(0, 0, 0, 8.75f * scr.y + ((inv.Count - 35) * (0.25f * scr.y))), false, true);

                #region Function Explination
                //begin the viewing area
                //scrollPos is the current pos
                //new Rect(0,0,0,0), is the view window
                //new Vector2(0,0), is the scroll pos
                //new Rect(0,0,0,0), is the viewable area, position is relative to the view window.
                //can we see the horizontal bar
                //can we see the vertical bar
                //end the viewing area

                #endregion

                //the buttons positions are relative to the view rect new. Start at zero so there is no gap when scrolling

                #region Items in viewing area
                for (int i = 0; i < inv.Count; i++)
                {
                    //show button for each item and set up condition if pressed
                    if (GUI.Button(new Rect(0.5f * scr.x, 0f + (i * 0.25f * scr.y), 3 * scr.x, 0.25f * scr.y), inv[i].Name))
                    {
                        //if this item is selected, set it as the selected item
                        selectedItem = inv[i];
                        Debug.Log(selectedItem.Name);
                    }
                }
                #endregion

                GUI.EndScrollView();
            }
            #endregion
        }


    }
}
