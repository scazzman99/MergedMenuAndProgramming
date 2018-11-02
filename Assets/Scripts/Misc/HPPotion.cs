using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HPPotion : MonoBehaviour {

    public float HPValue;
    public int time;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Potion"))
        {
            other.gameObject.SetActive(false);
            HPValue = 50f;
            time = 2;

            this.GetComponent<CharHealthHandler>().GetHealValue(HPValue, time); //gets the CharHealthHandler script from this object and sends Heal value into script to be calculated
           
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Fountain"))
        {
            HPValue = 50f;
            time = 1;
            this.GetComponent<CharHealthHandler>().GetHealValue(HPValue, time); //gets the CharHealthHandler script from this object and sends Heal value into script to be calculated
        }

        if (other.CompareTag("Fire"))
        {
            HPValue = -5f;
            time = 1;
            this.GetComponent<CharHealthHandler>().GetHealValue(HPValue, time);
        }
    }


}
