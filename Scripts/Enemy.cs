using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{

    public Transform Player;
    public float MoveSpeed = 2.0f;
    public float MaxDist = 20.0f;
    public float MinDist = 10.0f;
    public AudioClip zombie_roar;
    public AudioClip zombie_hurt;
    public AudioClip zombie_die;

    private float Dist;
    private float hitDist = 1000.0f;
    private Animator anim;
    private bool inSight = false;
    private bool roared = false;
    private AudioSource AS;
    private NavMeshAgent NM;
    public float health = 100.0f;
    private bool dead = false;

    void Start()
    {
        Player = GameObject.FindWithTag("Player").transform;
        anim = GetComponent<Animator>();
        AS = GetComponent<AudioSource>();
        NM = GetComponent<NavMeshAgent>();
        NM.enabled = false;
        health = 100.0f;
    }

    void Update()
    {

        Dist = Vector3.Distance(transform.position, Player.position); //calculate the distance between the player and the zombie

        //Debug.Log("Distance = "+ Dist);
        Debug.Log("Enemy health : " + health);

        Ray eyeSight = new Ray(transform.position, Player.position);
        //int layerMask = LayerMask.GetMask("Player");

        if (Physics.Raycast(eyeSight, out RaycastHit hit, hitDist))
        //if (Physics.Raycast(eyeSight, out RaycastHit hit, hitDist, layerMask))
        {

            if (hit.collider.CompareTag("Player"))   //Player seen by zombie
            {
                inSight = true;
                Debug.Log("Player seen! inSight is " + inSight);
            } 

        }

        if (inSight)    //Player seen by zombie
        {

            if ((Dist > MinDist) && (Dist <= MaxDist))
            {                
                if (!roared)
                {
                    AS.PlayOneShot(zombie_roar);
                    roared = true;
                }

                //change from idle to walking
                anim.SetTrigger("Walk");
                NM.enabled = true;
                NM.SetDestination(Player.position);
                //transform.position += transform.forward  * MoveSpeed * Time.deltaTime;
                
            }

            if (Dist > MaxDist)
            {
                //change to idle mode
                anim.SetTrigger("Idle");
                roared = false;
            }

            if (Dist <= MinDist)
            {
                //change to attack mode             
                NM.enabled = false;
                
                Vector3 temPos = Player.position;
                temPos.y = transform.position.y;
                transform.LookAt(temPos);                
                anim.SetTrigger("Attack");
                //transform.position += transform.forward * MoveSpeed * Time.deltaTime;


            }
        }
        else  //Player not seen by zombie
        {
            //change to idle
            anim.SetTrigger("Idle");            
        }
    }

    internal void Damage(float dmg)
    {
        if (health > 0)
        {
            AS.PlayOneShot(zombie_hurt);
            anim.SetTrigger("Hurt");
            health -= dmg;

            if (health <=0)
            {

                dead = true;
                AS.PlayOneShot(zombie_die);
                anim.SetTrigger("Die");
                NM.enabled = false;
                this.enabled = false;

            }

            Debug.Log("Enemy health : " + health);
        }
        
        
    }


}