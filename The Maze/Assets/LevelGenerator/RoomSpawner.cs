using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//
// @BlackthornProd
// RANDOM DUNGEON GENERATOR
// ~suchencjusz
//

public class RoomSpawner : MonoBehaviour
{
    public int openingDirection;

    // 1 --> need bottom door
    // 2 --> need top door
    // 3 --> need left door
    // 4 --> need right door

    private RoomTemplates templates;
    private int rand;
    private bool spawned = false;

    public float waitTime = 4f;

    IEnumerator RemoveClosedBuggedAfter3Seconds()
    {
        yield return new WaitForSeconds(4);

        GameObject[] closedRooms = GameObject.FindGameObjectsWithTag("Closed");

        GameObject starter = GameObject.FindGameObjectWithTag("Starter");

        foreach (GameObject closedRoom in closedRooms)
        {
            if (closedRoom.transform.position == starter.transform.position)
            {
                Destroy(closedRoom);
            }
        }
    }

    void Start()
    {
        // remove objects with tag Closed after 3 seconds with same transform position

        StartCoroutine(RemoveClosedBuggedAfter3Seconds());

        Destroy(gameObject, waitTime);
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
        Invoke("Spawn", 0.1f);
    }

    void Spawn()
    {
        if (spawned == false)
        {
            if (openingDirection == 1)
            {
                // Need to spawn a room with a BOTTOM door.
                rand = Random.Range(0, templates.bottomRooms.Length);
                Instantiate(templates.bottomRooms[rand], transform.position, Quaternion.identity);
            }
            else if (openingDirection == 2)
            {
                // Need to spawn a room with a TOP door.
                rand = Random.Range(0, templates.topRooms.Length);
                Instantiate(templates.topRooms[rand], transform.position, Quaternion.identity);
            }
            else if (openingDirection == 3)
            {
                // Need to spawn a room with a LEFT door.
                rand = Random.Range(0, templates.leftRooms.Length);
                Instantiate(templates.leftRooms[rand], transform.position, Quaternion.identity);
            }
            else if (openingDirection == 4)
            {
                // Need to spawn a room with a RIGHT door.
                rand = Random.Range(0, templates.rightRooms.Length);
                Instantiate(templates.rightRooms[rand], transform.position, Quaternion.identity);
            }
            spawned = true;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("SpawnPoint"))
        {
            if (other.GetComponent<RoomSpawner>().spawned == false && spawned == false)
            {
                // Spawn walls blocking the door.
                Instantiate(templates.closedRoom, transform.position, Quaternion.identity);
                if (other.CompareTag("Player"))
                {
                    Destroy(gameObject);
                }
            }
            spawned = true;
        }
    }
}
