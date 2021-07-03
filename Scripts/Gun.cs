using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public GameObject Spawnpoint;
    public float shootDistance;
    public GameObject BulletHole;
    public GameObject MuzzleFire;
    private float fireTime = 0.1f;
    public GameObject BloodShed;
    private float dmg = 20.0f;
     
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;

            Ray bullet = new Ray(Spawnpoint.transform.position, transform.forward);

            if (Physics.Raycast(bullet, out hit, shootDistance))
            {
                if (hit.collider.tag == "Wall")
                {
                    Instantiate(BulletHole, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal));
                }
                else if (hit.collider.tag == "Enemy")
                {
                    Instantiate(BloodShed, hit.point,  Quaternion.FromToRotation(Vector3.forward, hit.normal));
                    hit.transform.gameObject.GetComponent<Enemy>().Damage(dmg);
                    hit.transform.gameObject.GetComponent<EnemyHealth>().Damage(dmg);
                    
                }

            }

            StartCoroutine(FireEffect());    

        }

        


    }

    IEnumerator FireEffect()
    {
        MuzzleFire.SetActive(true);
        yield return new WaitForSeconds(fireTime);
        MuzzleFire.SetActive(false);

    }
        

}
