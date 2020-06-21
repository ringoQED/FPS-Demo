using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Enemy : MonoBehaviour
{

    public Transform Player;
    public float MoveSpeed = 4.0f;
    public float MaxDist = 20.0f;
    public float MinDist = 10.0f;
    private float Dist;
    private float hitDist = 1000.0f;
    private Animator anim;
    private bool inSight = false;
         
    void Start()
    {
        anim = GetComponent<Animator>();        
    }

    void Update()
    {        
        Dist = Vector3.Distance(transform.position, Player.position); //culculate the distance between the player and the zombie

        Debug.Log("Distance = "+ Dist);

        Ray eyeSight = new Ray(transform.position, Player.position);

        if (Physics.Raycast(eyeSight, out RaycastHit hit, hitDist))
        {
            if (hit.collider.CompareTag("Player"))   //Player seen by zombie
            {
                inSight = true;
                Debug.Log("Player seen!");
            }

        }

        if (inSight)
        {

            if ((Dist > MinDist) && (Dist <= MaxDist))
            {
                //change from idle to walking and chase player
                anim.SetTrigger("Walk");
                transform.LookAt(Player);
                transform.position += transform.forward * MoveSpeed * Time.deltaTime;
            }

            if (Dist > MaxDist)
            {
                //change to idle mode
                anim.SetTrigger("Idle");
            }

            if (Dist <= MinDist)
            {
                //change to attack mode
                transform.LookAt(Player);
                transform.position += transform.forward * MoveSpeed * Time.deltaTime;
                anim.SetTrigger("Attack");
            }
        }
        else  //Player not seen
        {
            //change to idle
            anim.SetTrigger("Idle");
        }
    }
}