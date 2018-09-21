using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CustomisationGet : MonoBehaviour {

    [Header("Character")]
    public Renderer charMesh;
    public CharHealthHandler charH;
    public bool isPlayer;
    public int skinMax, hairMax, mouthMax, eyesMax, armourMax, clothesMax;
    
	// Use this for initialization
	void Start () {
        if(gameObject.tag == "Player")
        {
            isPlayer = true;
            
        }
        charMesh = gameObject.GetComponentInChildren<SkinnedMeshRenderer>();
        LoadTexture();
        if (isPlayer)
        {
            SetStats();
        }
    }
	

    #region LoadTexture
    //check if playerprefs has a key (save)
    //if not load a cusotmset level
    //if it does then load setTexture for everything
    public void LoadTexture()
    {
        if (isPlayer)
        {
            if (!PlayerPrefs.HasKey("SkinIndex"))
            {
                //load the customisation scene
                SceneManager.LoadScene(1);
            }
            SetTexture("Skin", PlayerPrefs.GetInt("SkinIndex"));
            SetTexture("Hair", PlayerPrefs.GetInt("HairIndex"));
            SetTexture("Mouth", PlayerPrefs.GetInt("MouthIndex"));
            SetTexture("Eyes", PlayerPrefs.GetInt("EyesIndex"));
            SetTexture("Armour", PlayerPrefs.GetInt("ArmourIndex"));
            SetTexture("Clothes", PlayerPrefs.GetInt("ClothesIndex"));
            gameObject.name = PlayerPrefs.GetString("CharacterName");
        } else
        {
            SetTexture("Skin", Random.Range(0, skinMax - 1));
            SetTexture("Hair", Random.Range(0, hairMax - 1));
            SetTexture("Mouth", Random.Range(0, mouthMax - 1));
            SetTexture("Eyes", Random.Range(0, eyesMax - 1));
            SetTexture("Armour", Random.Range(0, armourMax - 1));
            SetTexture("Clothes", Random.Range(0, clothesMax - 1));

        }
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
        int matIndex = 0;
        Texture2D tex = null;
        

        #region Switch Material

        switch (type)
        {
            //case skin
            case "Skin":
                //load the skin from the resource file and set it as a texture2d
                tex = Resources.Load("Character/Skin_" + dir.ToString()) as Texture2D;
                //set the material index to where the case is in the material array.
                matIndex = 1;
                break;

            //same idea for subsequent cases.
            case "Hair":
                tex = Resources.Load("Character/Hair_" + dir.ToString()) as Texture2D;
                matIndex = 2;
                break;

            case "Mouth":
                tex = Resources.Load("Character/Mouth_" + dir.ToString()) as Texture2D;
                matIndex = 3;
                break;

            case "Eyes":
                tex = Resources.Load("Character/Eyes_" + dir.ToString()) as Texture2D;
                matIndex = 4;
                break;

            case "Armour":
                tex = Resources.Load("Character/Armour_" + dir.ToString()) as Texture2D;
                matIndex = 5;
                break;

            case "Clothes":
                tex = Resources.Load("Character/Clothes_" + dir.ToString()) as Texture2D;
                matIndex = 6;
                break;
        }
        #endregion
        #region OutSide Switch

        //outside our switch statement
        //index plus equals our direction
     
        //Material array is equal to our characters material list
        Material[] mat = charMesh.materials;
        //our material arrays current material index's main texture is equal to our texture arrays current index
        mat[matIndex].mainTexture = tex;
        //our characters materials are equal to the material array
        charMesh.materials = mat;
        //create another switch that is goverened by the same string name of our material
        #endregion
      
    }
    #endregion

    public void SetStats()
    {
        int[] stats = charH.statVals;
        string[] statNames = charH.stats;
        for(int i = 0; i < statNames.Length; i++)
        {
            stats[i] = PlayerPrefs.GetInt(statNames[i]);
        }
        charH.statVals = stats;
    }
}
