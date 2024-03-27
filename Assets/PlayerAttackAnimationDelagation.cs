using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackAnimationDelagation : MonoBehaviour
{
    public GameObject HB_leftHand, HB_rightHand, HB_leftFoot, HB_rightFoot;

   
    void Left_Hand_Attack_ON() 
    {
        HB_leftHand.SetActive(true);
    }

    void Right_Hand_Attack_ON()
    {
        HB_rightHand.SetActive(true);
    }

    void Left_Foot_Attack_ON()
    {
        HB_leftFoot.SetActive(true);
    }

    void Right_Foot_Attack_ON()
    {
        HB_rightFoot.SetActive(true);
    }

    void HB_Attack_off() 
    {
        if (HB_leftFoot.activeInHierarchy){
            HB_leftFoot.SetActive(false);
        }

        if (HB_rightFoot.activeInHierarchy)
        {
            HB_rightFoot.SetActive(false);
        }

        if (HB_leftHand.activeInHierarchy)
        {
            HB_leftHand.SetActive(false);
        }

        if (HB_rightHand.activeInHierarchy)
        {
            HB_rightHand.SetActive(false);
        }

    }


}
