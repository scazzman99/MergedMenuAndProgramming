//dont need array so dont need generic
using UnityEngine;

//no inheritance 
public class Item{


    #region PrivateVars
    //private variable names start with underscore
    private int _id;
    private string _name;
    private string _description;
    private int _value;
    private int _damage;
    private int _armour;
    private int _amount;
    private int _heal;
    private Texture2D _icon;
    //cant use gmae object here as we are not using monobehaviour
    private string _mesh;
    private ItemTypes _type;
    #endregion

    #region Properties
    //captial var name as this is a property. These allow use to access private vars in the same script from outside thru the property
    
    public string Name
    {
        //This allows us to read the private var from outside using this property. Can be removed
        get { return _name; }
        //This allows us to write the private var from outside using this property. Can be removed
        //this is the value of the data we are using not the variable _value
        set { _name = value; }
    }

    public string Description
    {
        get { return _description; }
        set { _description = value; }
    }

    public int Id
    {
        get { return _id; }
        set { _id = value; }
    }

    public int Value
    {
        get { return _value; }
        set { _value = value; }
    }

    public int Damage
    {
        get { return _damage; }
        set { _damage = value; }
    }

    public int Armour
    {
        get { return _armour; }
        set { _armour = value; }
    }

    public int Amount
    {
        get { return _amount; }
        set { _amount = value; }
    }

    public int Heal
    {
        get { return _heal; }
        set { _heal = value; }
    }

    public Texture2D Icon
    {
        get { return _icon; }
        set { _icon = value; }
    }

    public string Mesh
    {
        get { return _mesh; }
        set { _mesh = value; }
    }

    public ItemTypes Type
    {
        get { return _type; }
        set { _type = value; }
    }


    #endregion

    //constructor to set default information
    public Item()
    {
        //default information
        _id = 0;
        _name = "unknown";
        _description = "???";
        _value = 0;
        _mesh = "MeshName";
        _type = ItemTypes.Quest;
    }

    //constructor to set specific values
    public Item(int id, string name, string description, int value, string meshName, ItemTypes type)
    {
        _id = id;
        _name = name;
        _description = description;
        _value = value;
        _mesh = meshName;
        _type = type;
       

    }
}

//enum is outside the class so it is referencable anywhere
public enum ItemTypes
{
    Consumables,
    Armour,
    Weapon,
    Craftable,
    Money,
    Quest,
    Misc
}
