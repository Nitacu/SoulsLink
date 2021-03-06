﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class DummyController : MonoBehaviour
{
    public enum roomOfDummy
    {
        ROOM1,
        ROOM2,
        ROOM3,
        ROOM4,
        ROOM5
    }

    public roomOfDummy room;

    [Header("Room 2 and 3 dummy settings")]
    public Transform movingPoint1 = null;
    public Transform movingPoint2 = null;
    public float disappearingRate = 1.5f;
    public float disappearingTime = 0.5f;
    private Transform target;

    [Header("Room 4 dummy settings")]
    public List<Transform> spawnPoints = new List<Transform>();
    public float movePointTime = 0.8f;

    private int currentPosition = 0;

    private void Start()
    {
        if (room == roomOfDummy.ROOM2)
        {
            target = movingPoint1;
        }

        if (room == roomOfDummy.ROOM3)
        {
            target = movingPoint1;
            Invoke("disappearTargetRoom3", disappearingRate);
        }

        if (room == roomOfDummy.ROOM4)
        {
            GetComponent<PhotonTransformView>().enabled = false;
            Invoke("changeLocationOfDummy", movePointTime);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            switch (room)
            {
                case roomOfDummy.ROOM2:
                    room2DummyBehaviour();
                    break;
                case roomOfDummy.ROOM3:
                    room3DummyBehaviour();
                    break;
                case roomOfDummy.ROOM4:
                    room4DummyBehaviour();
                    break;
                case roomOfDummy.ROOM5:
                    room5DummyBehaviour();
                    break;
            }
        }
           
    }

    private void room2DummyBehaviour()
    {
        float speed = 4;
        // Move our position a step closer to the target.
        float step = speed * Time.deltaTime; // calculate distance to move
        transform.position = Vector3.MoveTowards(transform.position, target.position, step); // move

        //Checks if it's relatively close to target
        if (Vector3.Distance(transform.position, target.position) < 0.001f)
        {
            // Swap target
            if(target == movingPoint1)
            {
                target = movingPoint2;
            }
            else
            {
                target = movingPoint1;
            }
        }
    }

    private void room3DummyBehaviour()
    {
        if (target != null)
        {
            float speed = 4f;
            // Move our position a step closer to the target.
            float step = speed * Time.deltaTime; // calculate distance to move
            transform.position = Vector3.MoveTowards(transform.position, target.position, step); // move

            //Checks if it's relatively close to target
            if (Vector3.Distance(transform.position, target.position) < 0.001f)
            {
                // Swap target
                if (target == movingPoint1)
                {
                    target = movingPoint2;
                }
                else
                {
                    target = movingPoint1;
                }
            }
        }
    }

    private void disappearTargetRoom3()
    {
        Color transparent = GetComponent<SpriteRenderer>().color;
        transparent.a = 0.2f;
        GetComponent<SpriteRenderer>().color = transparent;
        GetComponent<BoxCollider2D>().enabled = false;
        Invoke("reappearTargetRoom3", disappearingTime);
    }

    private void reappearTargetRoom3()
    {
        Color notTransparent = GetComponent<SpriteRenderer>().color;
        notTransparent.a = 1f;
        GetComponent<SpriteRenderer>().color = notTransparent;
        GetComponent<BoxCollider2D>().enabled = true;
        Invoke("disappearTargetRoom3", disappearingRate);
    }

    private void changeLocationOfDummy()
    {
        currentPosition++;
        if(currentPosition > spawnPoints.Count - 1)
        {
            currentPosition = 0;
        }

        if (PhotonNetwork.IsMasterClient)
        {
            if (gameObject.transform.position == spawnPoints[currentPosition].position)
            {
                changeLocationOfDummy();
            }
            else
            {
                GetComponent<PhotonEnemyMultiplayerController>().setNextPosition(currentPosition);
                transform.position = spawnPoints[currentPosition].position;
                Invoke("changeLocationOfDummy", movePointTime);
            }
        }
    }

    //lo llama el multiplayer
    public void changeLocation(int index)
    {
        transform.position = spawnPoints[index].position;
    }

    private void room4DummyBehaviour()
    {

    }

    private void room5DummyBehaviour()
    {

    }

    public void killDummy()
    {
        //Sends room controller info of dummy's death to see if any door has to be opened
        GetComponentInParent<Transform>().gameObject.GetComponentInParent<RoomController>().checkDoorsToOpen(gameObject, room);
        //Adds score
        GetComponentInParent<Transform>().gameObject.GetComponentInParent<ScoreAndTimeController>().updateScore();
        //Destroy dummy
        Destroy(gameObject);
    }
}
