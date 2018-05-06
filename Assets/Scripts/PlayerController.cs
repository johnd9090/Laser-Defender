using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float speed;
    public GameObject projectile;
    public float projectileSpeed;
    public float fireRate;
    public float health;
    public AudioClip fireSound;

    private float xMin;
    private float xMax;
    private float padding = 0.55f;

    void Start () {

        float distance = transform.position.z - Camera.main.transform.position.z;

        Vector3 leftMostPos = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, distance));
        Vector3 rightMostPos = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, distance));
        xMin = leftMostPos.x + padding;
        xMax = rightMostPos.x - padding;
    }

    void Fire()
    {
        Vector3 offset = new Vector3(0, 1, 0);
        GameObject laser = Instantiate(projectile, transform.position+offset, Quaternion.identity);
        laser.GetComponent<Rigidbody2D>().velocity = new Vector3(0, projectileSpeed, 0);
        AudioSource.PlayClipAtPoint(fireSound, transform.position);
    }

	void Update () {

        if(Input.GetKeyDown(KeyCode.Space))
        {
            InvokeRepeating("Fire", 0.000001f, fireRate);
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            CancelInvoke("Fire");
        }

		if(Input.GetKey(KeyCode.LeftArrow))
        {
            transform.position += Vector3.left * speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.position += Vector3.right * speed * Time.deltaTime;
        }

        //Restricts player to the gamespace.
        float newX = Mathf.Clamp(transform.position.x, xMin, xMax);
        transform.position = new Vector3(newX, transform.position.y, transform.position.z);

    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        Projectile projectile = collider.gameObject.GetComponent<Projectile>();
        if (projectile)
        {
            health -= projectile.GetDamage();
            projectile.Hit();
            if (health <= 0)
            {
                Die();
            }
        }

    }

    void Die()
    {
        LevelManager levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        levelManager.LoadLevel("Win Screen");
        Destroy(gameObject);
    }
}
