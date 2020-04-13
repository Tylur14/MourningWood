using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("Weapon Info")]
    [SerializeField] string weaponName;
    public int weaponID;
    [Header("Weapon Settings")]
    public float damageOut = 0.05f;
    public float shotRange = 7.0f;

    public float fireRate;
    public float fireTimer = 0.0f;

    public GameObject wallBulletHit;
    public Transform playerTransform;

    public void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void FireOut(Vector3 dir)
    {
        RaycastHit hit;
        Vector3 shootPos = new Vector3(playerTransform.position.x, playerTransform.position.y + 0.5f, playerTransform.position.z);
        Debug.DrawRay(shootPos, dir, Color.green, 0.1f);
        if (Physics.Raycast(shootPos, dir, out hit, shotRange))
        {
            if (hit.transform.gameObject.tag == "Entity") // Really this just applies to treemons?
            {
                DealDamage(hit.transform.gameObject,hit.point);
            }
            else if (hit.transform.gameObject.tag == "Player")
                hit.transform.gameObject.GetComponent<Player>().TakeDamage(damageOut);

            else if (hit.transform.gameObject.tag == "Wall" && wallBulletHit != null)
                Instantiate(wallBulletHit, new Vector3(hit.point.x + (hit.normal.x * 0.01f), hit.point.y, hit.point.z + (hit.normal.z * 0.01f)), Quaternion.LookRotation(hit.normal));
            //Debug.LogError("Halt! You've violated the law! : " + hit.transform.gameObject);
        }

    }
    void DealDamage(GameObject target, Vector3 pos)
    {
        if (target.GetComponent<Entity>() != null)
        {
            target.GetComponent<Entity>().TakeDamage(damageOut);
            target.GetComponent<Entity>().DisplayHit(pos);
            if (target.GetComponent<TreemonMotor>() != null)
            {
                target.GetComponent<TreemonMotor>().focused = true;
                target.GetComponent<TreemonMotor>().YellForHelp();
            }
        }
    }
}
