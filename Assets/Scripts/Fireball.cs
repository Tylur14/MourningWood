using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Tyler J. Sims
// Fireball class for Fire Away, DEC 09 2019
public class Fireball : MonoBehaviour
{
    public Vector3 moveDir;
    public float moveSpeed;
    public float damageOut;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(
            transform.position.x + moveDir.x * Time.deltaTime * moveSpeed,
            transform.position.y + moveDir.y * Time.deltaTime * moveSpeed,
            transform.position.z + moveDir.z * Time.deltaTime * moveSpeed
            );
    }

    private void OnTriggerEnter(Collider other)
    {
        print(other.tag);
        if (other.tag == "Player")
        { other.gameObject.GetComponent<Player>().TakeDamage(damageOut); Destroy(this.gameObject); }
        else if (other.tag != "Entity" && other.tag != "Door")
            Destroy(this.gameObject);
    }
}
