
using UnityEngine;

public static class ItemData
{

    public static Item CreateItem(int ItemID)
    {
        //tjis is the stuff we need
        int id = 0;
        string name = "";
        string description = "";
        int value = 0;
        int damage = 0;
        int armour = 0;
        int amount = 0;
        int heal = 0;
        string mesh = "";
        string icon = "";
        ItemTypes type = ItemTypes.Armour;

        //this is where the item is set
        switch (ItemID)
        {
            #region consumable 0-99
            case 0:
                name = "Apple";
                description = "Munchies and Crunchies";
                value = 5;
                damage = 0;
                armour = 0;
                amount = 1;
                heal = 10;
                mesh = "Apple_Mesh";
                icon = "Apple_Icon";
                type = ItemTypes.Consumables;
                break;

            case 1:
                name = "Meat";
                description = "A suspicously tough leg of meat";
                value = 3;
                damage = 0;
                armour = 0;
                amount = 1;
                heal = 5;
                mesh = "Meat_Mesh";
                icon = "Meat_Icon";
                type = ItemTypes.Consumables;
                break;

            case 2:
                name = "Health Potion";
                description = "Slurp";
                value = 10;
                damage = 0;
                armour = 0;
                amount = 1;
                heal = 10;
                mesh = "HealthPotion_Mesh";
                icon = "HealthPotion" +
                    "_Icon";
                type = ItemTypes.Consumables;
                break;


            #endregion
            #region Armour 100-199
            case 100:
                name = "Leather Soulders";
                description = "If they aim for your shoulders you MIGHT be fine";
                value = 4;
                damage = 0;
                armour = 2;
                amount = 1;
                heal = 0;
                mesh = "LeatherShoulders_Mesh";
                icon = "LeatherShoulders_Icon";
                type = ItemTypes.Armour;
                break;

            case 101:
                name = "Iron Helmet";
                description = "Dented and worn, but up to the task";
                value = 6;
                damage = 0;
                armour = 4;
                amount = 1;
                heal = 0;
                mesh = "IronHelmet_Mesh";
                icon = "IronHelmet_Icon";
                type = ItemTypes.Armour;
                break;

            case 102:
                name = "Leather Armour";
                description = "Your skin wont protect against much, but this one might";
                value = 6;
                damage = 0;
                armour = 6;
                amount = 1;
                heal = 0;
                mesh = "LeatherArmour_Mesh";
                icon = "LeatherArmour_Icon";
                type = ItemTypes.Armour;
                break;
            #endregion
            #region Weapons 200-299
            case 200:
                name = "Iron Axe";
                description = "A hefty iron axe";
                value = 5;
                damage = 6;
                armour = 0;
                amount = 1;
                heal = 0;
                mesh = "IronAxe_Mesh";
                icon = "IronAxe_Icon";
                type = ItemTypes.Weapon;
                break;

            case 201:
                name = "Iron Sword";
                description = "A fading iron blade, slightly blunt but still effective";
                value = 5;
                damage = 5;
                armour = 0;
                amount = 1;
                heal = 0;
                mesh = "IronSword_Mesh";
                icon = "IronSword_Icon";
                type = ItemTypes.Weapon;
                break;

            case 202:
                name = "Bow";
                description = "A sturdy bow, tightly strung";
                value = 7;
                damage = 7;
                armour = 0;
                amount = 1;
                heal = 0;
                mesh = "Bow_Mesh";
                icon = "Bow_Icon";
                type = ItemTypes.Weapon;
                break;
            #endregion
            #region Craftable 300-399
            case 300:
                name = "Ring";
                description = "An unimpressive ring with the potential to be enchanted";
                value = 3;
                damage = 0;
                armour = 0;
                amount = 1;
                heal = 0;
                mesh = "Ring_Mesh";
                icon = "Ring_Icon";
                type = ItemTypes.Craftable;
                break;

            case 301:
                name = "Iron Ingot";
                description = "Dense bar of smelted iron";
                value = 2;
                damage = 0;
                armour = 0;
                amount = 1;
                heal = 0;
                mesh = "IronIngot_Mesh";
                icon = "IronIngot_Icon";
                type = ItemTypes.Craftable;
                break;

            case 302:
                name = "Steel Ingot";
                description = "Dense bar of smelted steel";
                value = 3;
                damage = 0;
                armour = 0;
                amount = 1;
                heal = 0;
                mesh = "SteelIngot_Mesh";
                icon = "SteelIngot_Icon";
                type = ItemTypes.Craftable;
                break;
            #endregion
            #region Misc 400-499
            case 400:
                name = "Money";
                description = "Makes the world go round";
                value = 1;
                damage = 0;
                armour = 0;
                amount = 1;
                heal = 0;
                mesh = "Money_Mesh";
                icon = "Money_Icon";
                type = ItemTypes.Misc;
                break;

            case 401:
                name = "Scroll";
                description = "Ancient texts, understood by few";
                value = 2;
                damage = 0;
                armour = 0;
                amount = 1;
                heal = 0;
                mesh = "Scroll_Mesh";
                icon = "Scroll_Icon";
                type = ItemTypes.Misc;
                break;

            case 402:
                name = "Gem";
                description = "A gleaming stone, capable of great power";
                value = 20;
                damage = 0;
                armour = 0;
                amount = 1;
                heal = 0;
                mesh = "Gem_Mesh";
                icon = "Gem_Icon";
                type = ItemTypes.Misc;
                break;
            #endregion
            default:
                ItemID = 0;
                name = "Apple";
                description = "Munchies and Crunchies";
                value = 5;
                damage = 0;
                armour = 0;
                amount = 1;
                heal = 10;
                mesh = "Apple_Mesh";
                icon = "Apple_Icon";
                type = ItemTypes.Consumables;
                break;

        }
        //Item temp = new Item(id, name, description, value, mesh, type);

        //this is where the item is created
        Item temp = new Item
        {
            Name = name,
            Description = description,
            Value = value,
            Id = id,
            Damage = damage,
            Armour = armour,
            Amount = amount,
            Heal = heal,
            Mesh = mesh,
            Type = type,
            Icon = Resources.Load("Icons/" + icon) as Texture2D

        };

       

        return temp;
    }
}
