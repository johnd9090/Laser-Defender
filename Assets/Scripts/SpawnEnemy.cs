using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour {

    public GameObject enemyPrefab;

    public float boxWidth;
    public float boxHeight;
    public float speed;
    public float padding;
    public float spawnDelay;

    private float xMin;
    private float xMax;
    private float yMin;
    private float yMax;
    private bool movingRight;


	void Start () {

        float distance = transform.position.z - Camera.main.transform.position.z;
        Vector3 leftMostPos = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, distance));
        Vector3 rightMostPos = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, distance));
        xMin = leftMostPos.x + padding;
        xMax = rightMostPos.x - padding;

        SpawnUntilFull();
	}

    void SpawnEnemies()
    {
        foreach (Transform child in transform)
        {
            GameObject enemy = Instantiate(enemyPrefab, child.transform.position, Quaternion.identity) as GameObject;
            enemy.transform.parent = child;
        }
    }

    void SpawnUntilFull()
    {
        Transform freePosition = NextFreePosition();
        if(freePosition != null)
        {
            GameObject enemy = Instantiate(enemyPrefab, freePosition.position, Quaternion.identity) as GameObject;
            enemy.transform.parent = freePosition;
        }
        if (NextFreePosition())
        {
            Invoke("SpawnUntilFull", spawnDelay);
        }
    }

    public void OnDrawGizmos ()
    {
        Gizmos.DrawWireCube(new Vector3(transform.position.x, transform.position.y + 1), new Vector3(boxWidth, boxHeight));
    }



    void Update () {
        
        if(movingRight)
        {
            transform.position += Vector3.right * speed * Time.deltaTime;
        } else
        {
            transform.position += Vector3.left * speed * Time.deltaTime;
        }

        float rightEdge = transform.position.x + (0.46f * boxWidth);
        float leftEdge = transform.position.x - (0.46f * boxWidth);

        if(leftEdge < xMin)
        {
            movingRight = true;
        } else if (rightEdge > xMax) {
            movingRight = false;
        }

        if(AllMembersDead())
        {
            SpawnUntilFull();
        }

    }

    Transform NextFreePosition()
    {
        foreach (Transform childPositionGameObject in transform)
        {
            if (childPositionGameObject.childCount == 0)
            {
                return childPositionGameObject;
            }
        }
        return null;
    }

    bool AllMembersDead()
    {
        foreach(Transform childPositionGameObject in transform)
        {
            if (childPositionGameObject.childCount > 0)
            {
                return false;
            }
        }
        return true;
    }
}
