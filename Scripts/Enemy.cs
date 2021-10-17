using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{

    public Transform Player, WeaponCam;
    public float MoveSpeed = 2.0f;
    public float MaxDist = 50.0f;
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

    public GameObject fb, fb_clone;
    public float PushForce = 500.0f;
    public bool Busy = false;
    public float WaitTime = 1.0f;

    void Start()
    {
        Player = GameObject.FindWithTag("Player").transform;
        anim = GetComponent<Animator>();
        AS = GetComponent<AudioSource>();
        NM = GetComponent<NavMeshAgent>();
        NM.enabled = false;
        health = 100.0f;

        fb = GameObject.Find("Fireball");
        Player = GameObject.Find("Player").transform;
        WeaponCam = GameObject.Find("WeaponCam").transform;

    }

    void Update()
    {

        Dist = Vector3.Distance(transform.position, Player.position); //calculate the distance between the player and the zombie

        Ray eyeSight = new Ray(transform.position, Player.position);

        if (Physics.Raycast(eyeSight, out RaycastHit hit, hitDist))
        {

            if (hit.collider.CompareTag("Player"))   //Player seen by zombie
            {

                inSight = true;

            }

        }

        if (inSight)    //Player seen by zombie
        {

            transform.LookAt(WeaponCam.position);   // Make zombie to  look at player's direction

            if (!Busy)
            {
                StartCoroutine("Eject");
            }


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

    IEnumerator Eject()
    {
        Busy = true;

        fb_clone = Instantiate(fb, GameObject.Find("FBSpawnPoint").transform.position, GameObject.Find("FBSpawnPoint").transform.rotation) as GameObject;

        fb_clone.transform.LookAt(WeaponCam.position);

        fb_clone.GetComponent<Rigidbody>().AddForce(fb_clone.transform.forward * PushForce);

        yield return new WaitForSeconds(WaitTime);

        Busy = false;
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

                Destroy(fb);    // Destroy the Fireball game object to stop fireball action

            }

        }
        
        
    }


}