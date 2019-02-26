using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakePickUps : MonoBehaviour
{
    public GameObject hammerAttackGameObject;

    private HammerAttack hammerAttack;
    private Transform childObj;

    void Start()
    {
        childObj = transform.Find("Hammer");
        hammerAttack = hammerAttackGameObject.GetComponent<HammerAttack>();
    }
        
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PickUp")
        {
            LoadMazeFromTxt.K++;            
            Destroy(other.gameObject);

            if (LoadMazeFromTxt.K != 0 && childObj.transform.GetChild(0).gameObject.activeInHierarchy == false)
            {
                childObj.transform.GetChild(0).gameObject.SetActive(true);

                hammerAttack.setHealth();
                hammerAttack.setColor();
                hammerAttack.playAudioSource();
            }
        }
    }
}
