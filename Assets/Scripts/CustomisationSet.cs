﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//you will need to change Scenes
public class CustomisationSet : MonoBehaviour {

    #region Variables
    [Header("Texture List")]
    //Texture2D List for skin,hair, mouth, eyes
    public List<Texture2D> skin = new List<Texture2D>();
    public List<Texture2D> hair = new List<Texture2D>();
    public List<Texture2D> mouth = new List<Texture2D>();
    public List<Texture2D> eyes = new List<Texture2D>();
    public List<Texture2D> clothes = new List<Texture2D>();
    public List<Texture2D> armour = new List<Texture2D>();
    [Header("Index")]
    //index numbers for our current skin, hair, mouth, eyes textures
    public int skinIndex;
    public int hairIndex, mouthIndex, eyesIndex, clothesIndex, armourIndex;
    [Header("Renderer")]
    //renderer for our character mesh so we can reference a material list. Can also be a renderer!
    public MeshRenderer charMesh;
    [Header("Max Index")]
    //max amount of skin, hair, mouth, eyes textures that our lists are filling with
    public int skinMax;
    public int hairMax, mouthMax, eyesMax, clothesMax, armourMax;
    [Header("Character Name")]
    //name of our character that the user is making
    public string charName = "Adventurer";
    [Header("Stats")]
    //base stats affect the character
    public int str;
    public int dex, chr, con, intel, wis;
    public int strMin, dexMin, chrMin, conMin, intelMin, wisMin;
    //points used to increase our stats
    public int points = 10;
    public CharacterClass charClass = CharacterClass.Barbarian;
    public Renderer character;
    [Header("DropDown")]
    public Vector2 scrollArea;
    public int dropIndex;
    public bool showDrop;
    [Header("StatPanel")]
    public bool statPanel;
    
    #endregion

    #region Start

    private void Start()
    {

        #region for loops to fill lists
        //for loop looping from 0 to less than the max amount of skin textures we need
        //creating a temp Texture2D that it grabs using Resources.Load from the Character File looking for Skin_#
        //add our temp texture that we just found to the skin List
        for (int i = 0; i < skinMax; i++)
        {
            Texture2D temp = Resources.Load("Character/Skin_" + i) as Texture2D;
            skin.Add(temp);
        }

        //for loop looping from 0 to less than the max amount of hair textures we need
        //creating a temp Texture2D that it grabs using Resources.Load from the Character File looking for Hair_#
        //add our temp texture that we just found to the hair List
        for (int i = 0; i < hairMax; i++)
        {
            Texture2D temp = Resources.Load("Character/Hair_" + i) as Texture2D;
            hair.Add(temp);
        }

        //for loop looping from 0 to less than the max amount of mouth textures we need    
        //creating a temp Texture2D that it grabs using Resources.Load from the Character File looking for Mouth_#
        //add our temp texture that we just found to the mouth List
        for (int i = 0; i < mouthMax; i++)
        {
            Texture2D temp = Resources.Load("Character/Mouth_" + i) as Texture2D;
            mouth.Add(temp);
        }

        //for loop looping from 0 to less than the max amount of eyes textures we need
        //creating a temp Texture2D that it grabs using Resources.Load from the Character File looking for Eyes_#
        //add our temp texture that we just found to the eyes List 
        for (int i = 0; i < eyesMax; i++)
        {
            Texture2D temp = Resources.Load("Character/Eyes_" + i) as Texture2D;
            eyes.Add(temp);
        }

        for(int i = 0; i < clothesMax; i++)
        {
            Texture2D temp = Resources.Load("Character/Clothes_" + i) as Texture2D;
            clothes.Add(temp);
        }

        for (int i = 0; i < armourMax; i++)
        {
            Texture2D temp = Resources.Load("Character/Armour_" + i) as Texture2D;
            armour.Add(temp);
        }

        #endregion

        //connect and find the SkinnedMeshRenderer thats in the scene to the variable we made for Renderer
        character = GameObject.Find("Mesh").GetComponent<SkinnedMeshRenderer>();

        //SetTexture skin, hair, mouth, eyes to the first texture 0
        SetTexture("Skin", 0);
        SetTexture("Hair", 0);
        SetTexture("Mouth", 0);
        SetTexture("Eyes", 0);
        SetTexture("Armour", 0);
        SetTexture("Clothes", 0);
        GetClass(0);



    }
    #endregion

    #region SetTexture
    //Create a function that is called SetTexture it should contain a string and int
    //the string is the name of the material we are editing, the int is the direction we are changing
   

    public void SetTexture(string type, int dir)
    {
        //we need variables that exist only within this function
        //these are ints index numbers, max numbers, material index and Texture2D array of textures
        //inside a switch statement that is swapped by the string name of our material
        int index = 0, max = 0, matIndex = 0;
        Texture2D[] textures = new Texture2D[0];

        #region Switch Material
     
        switch (type)
        {
            //case skin
            case "Skin":
                //index is the same as our skin index
                index = skinIndex;
                //max is the same as our skin max
                max = skinMax;
                //textures is our skin list .ToArray()
                textures = skin.ToArray();
                //material index element number is 1
                matIndex = 1;
                break;

                //same idea for subsequent cases.
            case "Hair":
                index = hairIndex;
                max = hairMax;
                textures = hair.ToArray();
                matIndex = 2;
                break;

            case "Mouth":
                index = mouthIndex;
                max = mouthMax;
                textures = mouth.ToArray();
                matIndex = 3;
                break;

            case "Eyes":
                index = eyesIndex;
                max = eyesMax;
                textures = eyes.ToArray();
                matIndex = 4;
                break;

            case "Armour":
                index = armourIndex;
                max = armourMax;
                textures = armour.ToArray();
                matIndex = 5;
                break;

            case "Clothes":
                index = clothesIndex;
                max = clothesMax;
                textures = clothes.ToArray();
                matIndex = 6;
                break;
        }
        #endregion
        #region OutSide Switch

        //outside our switch statement
        //index plus equals our direction
        index += dir;
        //cap our index to loop back around if is is below 0 or above max take one
        if(index < 0)
        {
            index = max - 1;
        }
        if(index > max - 1)
        {
            index = 0;
        }
        //Material array is equal to our characters material list
        Material[] mat = character.materials;
        //our material arrays current material index's main texture is equal to our texture arrays current index
        mat[matIndex].mainTexture = textures[index];
        //our characters materials are equal to the material array
        character.materials = mat;
        //create another switch that is goverened by the same string name of our material
        #endregion
        #region Set Material Switch
        
        switch (type)
        {
            //case skin
            case "Skin":
                //skin index equals our index
                skinIndex = index;
                //break
                break;
                //Same for rest of the cases
            case "Hair":
                hairIndex = index;
                break;

            case "Mouth":
                mouthIndex = index;
                break;

            case "Eyes":
                eyesIndex = index;
                break;

            case "Armour":
                armourIndex = index;
                break;

            case "Clothes":
                clothesIndex = index;
                break;
        }
        #endregion
    }
    #endregion

    #region Save
    //Function called Save this will allow us to save our indexes to PlayerPrefs
    //SetInt for SkinIndex, HairIndex, MouthIndex, EyesIndex
    //SetString CharacterName
    public void Save()
    {
        PlayerPrefs.SetInt("SkinIndex", skinIndex);
        PlayerPrefs.SetInt("HairIndex", hairIndex);
        PlayerPrefs.SetInt("MouthIndex", mouthIndex);
        PlayerPrefs.SetInt("EyesIndex", eyesIndex);
        PlayerPrefs.SetInt("ArmourIndex", armourIndex);
        PlayerPrefs.SetInt("ClothesIndex", clothesIndex);
        PlayerPrefs.SetString("CharacterName", charName);
    }
    #endregion

    #region OnGUI
    //Function for our GUI elements
    //create the floats scrW and scrH that govern our 16:9 ratio
    //create an int that will help with shuffling your GUI elements under eachother

    private void OnGUI()
    {
       float scrW = Screen.width / 16;
       float scrH = Screen.height / 9;
       int order = 0;

        if (!statPanel)
        {
            #region Skin

            //GUI button on the left of the screen with the contence <
            if (GUI.Button(new Rect(0.25f * scrW, scrH + order * (0.5f * scrH), 0.5f * scrW, 0.5f * scrH), "<"))
            {
                //when pressed the button will run SetTexture and grab the Skin Material and move the texture index in the direction  -1
                SetTexture("Skin", -1);
            }

            //GUI Box or Lable on the left of the screen with the contence Skin
            GUI.Button(new Rect(0.75f * scrW, scrH + order * (0.5f * scrH), 1f * scrW, 0.5f * scrH), "Skin");

            //GUI button on the left of the screen with the contence >
            if (GUI.Button(new Rect(1.75f * scrW, scrH + order * (0.5f * scrH), 0.5f * scrW, 0.5f * scrH), ">"))
            {
                //when pressed the button will run SetTexture and grab the Skin Material and move the texture index in the direction  1
                SetTexture("Skin", 1);
            }

            //move down the screen with the int using ++ each grouping of GUI elements are moved using this
            #endregion

            order++;

            #region Hair
            if (GUI.Button(new Rect(0.25f * scrW, scrH + order * (0.5f * scrH), 0.5f * scrW, 0.5f * scrH), "<"))
            {
                //when pressed the button will run SetTexture and grab the Hair Material and move the texture index in the direction  -1
                SetTexture("Hair", -1);
            }

            GUI.Button(new Rect(0.75f * scrW, scrH + order * (0.5f * scrH), 1f * scrW, 0.5f * scrH), "Hair");

            if (GUI.Button(new Rect(1.75f * scrW, scrH + order * (0.5f * scrH), 0.5f * scrW, 0.5f * scrH), ">"))
            {
                //when pressed the button will run SetTexture and grab the Skin Material and move the texture index in the direction  1
                SetTexture("Hair", 1);
            }
            #endregion

            order++;

            #region Mouth
            if (GUI.Button(new Rect(0.25f * scrW, scrH + order * (0.5f * scrH), 0.5f * scrW, 0.5f * scrH), "<"))
            {
                //when pressed the button will run SetTexture and grab the Hair Material and move the texture index in the direction  -1
                SetTexture("Mouth", -1);
            }

            GUI.Button(new Rect(0.75f * scrW, scrH + order * (0.5f * scrH), 1f * scrW, 0.5f * scrH), "Mouth");

            if (GUI.Button(new Rect(1.75f * scrW, scrH + order * (0.5f * scrH), 0.5f * scrW, 0.5f * scrH), ">"))
            {
                //when pressed the button will run SetTexture and grab the Skin Material and move the texture index in the direction  1
                SetTexture("Mouth", 1);
            }
            #endregion

            order++;

            #region Eyes
            if (GUI.Button(new Rect(0.25f * scrW, scrH + order * (0.5f * scrH), 0.5f * scrW, 0.5f * scrH), "<"))
            {
                //when pressed the button will run SetTexture and grab the Hair Material and move the texture index in the direction  -1
                SetTexture("Eyes", -1);
            }

            GUI.Button(new Rect(0.75f * scrW, scrH + order * (0.5f * scrH), 1f * scrW, 0.5f * scrH), "Eyes");

            if (GUI.Button(new Rect(1.75f * scrW, scrH + order * (0.5f * scrH), 0.5f * scrW, 0.5f * scrH), ">"))
            {
                //when pressed the button will run SetTexture and grab the Skin Material and move the texture index in the direction  1
                SetTexture("Eyes", 1);
            }
            #endregion

            order++;

            #region Armour
            if (GUI.Button(new Rect(0.25f * scrW, scrH + order * (0.5f * scrH), 0.5f * scrW, 0.5f * scrH), "<"))
            {
                //when pressed the button will run SetTexture and grab the Skin Material and move the texture index in the direction  -1
                SetTexture("Armour", -1);
            }

            //GUI Box or Lable on the left of the screen with the contence Skin
            GUI.Button(new Rect(0.75f * scrW, scrH + order * (0.5f * scrH), 1f * scrW, 0.5f * scrH), "Armour");

            //GUI button on the left of the screen with the contence >
            if (GUI.Button(new Rect(1.75f * scrW, scrH + order * (0.5f * scrH), 0.5f * scrW, 0.5f * scrH), ">"))
            {
                //when pressed the button will run SetTexture and grab the Skin Material and move the texture index in the direction  1
                SetTexture("Armour", 1);
            }
            #endregion

            order++;

            #region Clothes

            if (GUI.Button(new Rect(0.25f * scrW, scrH + order * (0.5f * scrH), 0.5f * scrW, 0.5f * scrH), "<"))
            {
                //when pressed the button will run SetTexture and grab the Skin Material and move the texture index in the direction  -1
                SetTexture("Clothes", -1);
            }

            //GUI Box or Lable on the left of the screen with the contence Skin
            GUI.Button(new Rect(0.75f * scrW, scrH + order * (0.5f * scrH), 1f * scrW, 0.5f * scrH), "Clothes");

            //GUI button on the left of the screen with the contence >
            if (GUI.Button(new Rect(1.75f * scrW, scrH + order * (0.5f * scrH), 0.5f * scrW, 0.5f * scrH), ">"))
            {
                //when pressed the button will run SetTexture and grab the Skin Material and move the texture index in the direction  1
                SetTexture("Clothes", 1);
            }
            #endregion

            order++;

            #region ResetAndRandom
            //Random will feed a random amount to the direction 
            if (GUI.Button(new Rect(0.25f * scrW, scrH + order * (0.5f * scrH), scrW * 1.5f, scrH * 0.5f), "RANDOM"))
            {
                SetTexture("Skin", Random.Range(0, skinMax - 1)); //generate random number
                SetTexture("Hair", Random.Range(0, hairMax - 1));
                SetTexture("Mouth", Random.Range(0, mouthMax - 1));
                SetTexture("Eyes", Random.Range(0, eyesMax - 1));
                SetTexture("Armour", Random.Range(0, armourMax - 1));
                SetTexture("Clothes", Random.Range(0, clothesMax - 1));
            }

            //move down the screen with the int using ++ each grouping of GUI elements are moved using this
            order++;

            //reset will set all to 0 both use SetTexture
            if (GUI.Button(new Rect(0.25f * scrW, scrH + order * (scrH * 0.5f), scrW * 1.5f, scrH * 0.5f), "RESET"))
            {
                SetTexture("Skin", skinIndex = 0);
                SetTexture("Hair", hairIndex = 0);
                SetTexture("Mouth", mouthIndex = 0);
                SetTexture("Eyes", eyesIndex = 0);
                SetTexture("Armour", armourIndex = 0);
                SetTexture("Clothes", clothesIndex = 0);
            }

            #endregion

            order++;

            #region Character Name Save And Play
            //name of our character equals a GUI TextField that holds our character name and limit of characters
            charName = GUI.TextField(new Rect(0.25f * scrW, scrH + order * (scrH * 0.5f), scrW * 2f, scrH * 0.5f), charName, 16);

            order++;

            if (GUI.Button(new Rect(0.25f * scrW, scrH + order * (scrH * 0.5f), scrW * 2f, scrH * 0.5f), "Save & Play"))
            {
                //this button will run the save function and also load into the game level
                Save();
                SceneManager.LoadScene(2);
            }
            #endregion

            order++;

            #region ClassDrop
            if (GUI.Button(new Rect(0.25f * scrW, scrH + order * (scrH * 0.5f), scrW * 2f, scrH * 0.5f), "ClassDropdown"))
            {
                //button flips drop status
                showDrop = !showDrop;
            }

            order++;

            if (showDrop)
            {
                //set up a scroll position
                scrollArea = GUI.BeginScrollView(new Rect(0.25f * scrW, scrH + order * (scrH * 0.5f), scrW * 3f, scrH * 1f), scrollArea, new Rect(0, 0, scrW * 2f, scrH * 3.5f), false, true);

                for (int i = 0; i < 8; i++)
                {
                    //set the temp value to the CharacterClass cast of int i
                    CharacterClass temp = (CharacterClass)i;
                    //make an entry for each class
                    if (GUI.Button(new Rect(0, 0 + (scrH * 0.5f) * i, scrW * 3f, scrH * 0.5f), temp.ToString()))
                    {
                        GetClass(i);
                    }
                }
                GUI.EndScrollView();
            }


            #endregion

            order++;

            if (GUI.Button(new Rect(0.25f * scrW, scrH + order * (0.5f * scrH), scrW * 1.5f, scrH * 0.5f), "CHANGE STATS"))
            {
                statPanel = true;
            }

            #region BackToMainButton

            if (GUI.Button(new Rect(scrW * 0.25f, scrH * 8f, scrW, scrH * 0.5f), "Back"))
            {
                SceneManager.LoadScene(0);
            }

            #endregion

            order = 0;

            #region statDisplay
            GUI.Label(new Rect(scrW * 3f, scrH + order * (scrH * 1f), scrW * 3f, scrH * 0.5f), "Strength: " + str);
            GUI.Label(new Rect(scrW * 6f, scrH + order * (scrH * 1f), scrW * 3f, scrH * 0.5f), "Dexterity: " + dex);
            order++;
            GUI.Label(new Rect(scrW * 3f, scrH + order * (scrH * 1f), scrW * 3f, scrH * 0.5f), "Charisma: " + chr);
            GUI.Label(new Rect(scrW * 6f, scrH + order * (scrH * 1f), scrW * 3f, scrH * 0.5f), "Constitution: " + con);
            order++;
            GUI.Label(new Rect(scrW * 3f, scrH + order * (scrH * 1f), scrW * 3f, scrH * 0.5f), "Intelligence: " + intel);
            GUI.Label(new Rect(scrW * 6f, scrH + order * (scrH * 1f), scrW * 3f, scrH * 0.5f), "Wisdom: " + wis);
            order++;
            GUI.Label(new Rect(scrW * 3f, scrH + order * (scrH * 1f), scrW * 3f, scrH * 0.5f), "Points Left: " + points);
            #endregion

        } 
        else
        {
            order = 0;
            #region Strength

            GUI.Label(new Rect(scrW * 6f, scrH + order * (scrH * 0.5f), scrW * 4f, scrH * 0.5f), "Strength");
            order++;
            if (GUI.Button(new Rect(6f * scrW, scrH + order * (.5f * scrH), 0.5f * scrW, 0.5f * scrH), "<"))
            {
                if (points >= 0)
                {
                    if (str-- >= strMin)
                    {
                        //take away a point of strenth and a stat point
                        str--;
                        points--;
                    }
                }
                
            }

            GUI.Button(new Rect(6.75f * scrW, scrH + order * (0.5f * scrH), 1f * scrW, 0.5f * scrH), str.ToString());

            if (GUI.Button(new Rect(7.75f * scrW, scrH + order * (0.5f * scrH), 0.5f * scrW, 0.5f * scrH), ">"))
            {
                if(points >= 0)
                {
                    str++;
                    points--;
                }
            }

            #endregion
            
        }


    }

    #endregion

    #region SetBaseStats

    public void GetClass(int index)
    {
        switch (index)
        {
            case 0:
                charClass = CharacterClass.Barbarian;
                SetBaseStats(charClass);
                break;
            case 1:
                charClass = CharacterClass.Bard;
                SetBaseStats(charClass);
                break;
            case 2:
                charClass = CharacterClass.Druid;
                SetBaseStats(charClass);
                break;
            case 3:
                charClass = CharacterClass.Monk;
                SetBaseStats(charClass);
                break;
            case 4:
                charClass = CharacterClass.Paladin;
                SetBaseStats(charClass);
                break;
            case 5:
                charClass = CharacterClass.Ranger;
                SetBaseStats(charClass);
                break;
            case 6:
                charClass = CharacterClass.Sorcerer;
                SetBaseStats(charClass);
                break;
            case 7:
                charClass = CharacterClass.Warlock;
                SetBaseStats(charClass);
                break;

        }
    }

    public void SetBaseStats(CharacterClass pickedClass)
    {
        switch (pickedClass)
        {
            case CharacterClass.Barbarian:
                strMin = 12; dexMin = 8; chrMin = 6; conMin = 12; intelMin = 9; wisMin = 6;
                str = strMin; dex = dexMin; chr = chrMin; con = conMin; intel = intelMin; wis = wisMin;
                break;
            case CharacterClass.Bard:
                strMin = 8; dexMin = 8; chrMin = 12; conMin = 8; intelMin = 10; wisMin = 9;
                str = strMin; dex = dexMin; chr = chrMin; con = conMin; intel = intelMin; wis = wisMin;
                break;
            case CharacterClass.Druid:
                strMin = 10; dexMin = 8; chrMin = 7; conMin = 10; intelMin = 9; wisMin = 12;
                str = strMin; dex = dexMin; chr = chrMin; con = conMin; intel = intelMin; wis = wisMin;
                break;
            case CharacterClass.Monk:
                strMin = 10; dexMin = 10; chrMin = 8; conMin = 12; intelMin = 9; wisMin = 9;
                str = strMin; dex = dexMin; chr = chrMin; con = conMin; intel = intelMin; wis = wisMin;
                break;
            case CharacterClass.Paladin:
                strMin = 10; dexMin = 8; chrMin = 8; conMin = 10; intelMin = 9; wisMin = 8;
                str = strMin; dex = dexMin; chr = chrMin; con = conMin; intel = intelMin; wis = wisMin;
                break;
            case CharacterClass.Ranger:
                strMin = 8; dexMin = 10; chrMin = 8; conMin = 8; intelMin = 10; wisMin = 8;
                str = strMin; dex = dexMin; chr = chrMin; con = conMin; intel = intelMin; wis = wisMin;
                break;
            case CharacterClass.Sorcerer:
                strMin = 6; dexMin = 8; chrMin = 9; conMin = 8; intelMin = 12; wisMin = 12;
                str = strMin; dex = dexMin; chr = chrMin; con = conMin; intel = intelMin; wis = wisMin;
                break;
            case CharacterClass.Warlock:
                strMin = 8; dexMin = 6; chrMin = 6; conMin = 10; intelMin = 12; wisMin = 12;
                str = strMin; dex = dexMin; chr = chrMin; con = conMin; intel = intelMin; wis = wisMin;
                break;
        }
    }


    #endregion

    #region AdjustStats



    #endregion

}

public enum CharacterClass
{
    Barbarian,
    Bard,
    Druid,
    Monk,
    Paladin,
    Ranger,
    Sorcerer,
    Warlock
}
