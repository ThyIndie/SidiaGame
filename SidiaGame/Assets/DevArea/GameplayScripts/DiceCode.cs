using SidiaGame.GM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceCode : MonoBehaviour
{
    Rigidbody Dice;
    public int Result;
    public GameplayController GM;
    private void Start()
    {
        StartCoroutine(SetBody());
    }
    IEnumerator SetBody()
    {
        yield return new WaitForSeconds(1);
        Dice = GetComponent<Rigidbody>();
        GM = FindObjectOfType<GameplayController>();
    }
    private void Update()
    {
       if(Dice != null)
            if(Dice.velocity == Vector3.zero)
            {
                if (ForwardColision())
                    Result = 4;
                else if (BackwardColision())
                    Result = 3;
                else if (RightColision())
                    Result = 1;
                else if (LeftColision())
                    Result = 6;
                else if (UpColision())
                    Result = 5;
                else if (DownColision())
                    Result = 2;
                GM.ConfirmStop();
                Dice = null;
            }

    }

    bool ForwardColision()
    {
        return Physics.Raycast(transform.position, transform.forward, 1);
    }
    bool BackwardColision()
    {
        return Physics.Raycast(transform.position, -transform.forward, 1);
    }
    bool RightColision()
    {
        return Physics.Raycast(transform.position, transform.right, 1);
    }
    bool LeftColision()
    {
        return Physics.Raycast(transform.position, -transform.right, 1);
    }
    bool UpColision()
    {
        return Physics.Raycast(transform.position, transform.up, 1);
    }
    bool DownColision()
    {
        return Physics.Raycast(transform.position, -transform.up, 1);
    }


}
