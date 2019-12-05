using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitCameraController : MonoBehaviour
{
    public Transform player1;
    public Transform player2;
    public float splitDistance;
    private float maxSplitDistance = 0;
    private float minSplitDistance = 2f;

    public GameObject screenDivider;

    public GameObject camera1;
    public GameObject camera2;

    private Vector3 midPoint;

    private Vector3 offset;

    private float offsetPosition;

    public GameObject splitter;

    private float hundredPercent = 1;
    private float offsetPercent = 0.5f;

    private float xQuad = 0;
    private float yQuad = 0;
    private float zCameraOffset = 0;
    private float offsetMultiplier = 1;

    // Start is called before the first frame update
    void Start()
    {
        splitter.GetComponent<Renderer>().sortingOrder = 2;
        //splitter.transform.localPosition = Vector3.forward;
        maxSplitDistance = splitDistance;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float distance = Vector3.Distance(player1.position, player2.transform.position);


       if (distance >= (maxSplitDistance / 2))
        {
            if (distance < maxSplitDistance && distance > minSplitDistance)
            {
                splitDistance = distance;
            }
            else
            {
                splitDistance = maxSplitDistance;
            }

            
        }

        if (distance >= maxSplitDistance/2)
        {
            midPoint = new Vector3((player1.position.x + player2.position.x) / 2, (player1.position.y + player2.position.y) / 2, 0);
            float slope = (Mathf.Abs(player1.position.y - player2.position.y)) / (Mathf.Abs(player1.position.x - player2.position.x));
            float slopeReciprocal = Mathf.Pow(slope, -1) * -1;              // m from y  = mx+b
            float bSlope = (slopeReciprocal * midPoint.x - midPoint.y) * -1; // b from y = mx+b

            float yDistance = player1.position.y - player2.transform.position.y;

            float angle;
            if (player1.transform.position.x <= player2.transform.position.x)
            {
                angle = Mathf.Rad2Deg * Mathf.Acos(yDistance / distance);

            }
            else
            {
                angle = Mathf.Rad2Deg * Mathf.Asin(yDistance / distance) - 90;
            }

            getOffsetMultiplier(distance);

            findSplitterLocation(angle);

            

            if (player1.transform.position.x <= player2.transform.position.x)
            {
                offsetPosition = -1;
            }
            else
            {
                offsetPosition = 1;
            }

            splitter.SetActive(true);
            camera2.SetActive(true);

            

            //Rotates the splitter according to the new angle.
            splitter.transform.localEulerAngles = new Vector3(0, 0, angle);

            Vector3 offset = midPoint - player1.position;
            offset.x = Mathf.Clamp(offset.x, -splitDistance / 2, splitDistance / 2);
            offset.y = Mathf.Clamp(offset.y, -splitDistance / 2, splitDistance / 2);
            //camera1.transform.position = new Vector3(player1.transform.position.x + offset.x, player1.transform.position.y + (offset.y / 2), -10);

          

            float movingPercent = ((Vector3.Distance(player1.position, midPoint)/2)*100)/(maxSplitDistance / 2);
            Vector3 newOffset = new Vector3(offset.x, (offsetMultiplier * offset.y) / 2, offset.z);
            //Vector3 newCamera1Position = camera1.transform.position + ((movingPercent/100)*(player1.transform.position - camera1.transform.position));
            //camera1.transform.position = new Vector3(newCamera1Position.x + offset.x, newCamera1Position.y + (offset.y / 2), -10);
            Vector3 newCamera1Position = camera1.transform.position + ((movingPercent/100)*(player1.transform.position - camera1.transform.position + offset));
            camera1.transform.position = new Vector3(newCamera1Position.x, newCamera1Position.y, -10);

            Debug.Log("distance: " + distance);
            if (movingPercent > 50)
            {
                if (splitDistance > 4)
                {
                    screenDivider.SetActive(true);
                }
                screenDivider.GetComponent<RectTransform>().localEulerAngles = new Vector3(0, 0, angle + 90);
            }
            else
            {
                screenDivider.SetActive(false);
            }


            Vector3 offset2 = midPoint - player2.position;
            offset2.x = Mathf.Clamp(offset2.x, -splitDistance / 2, splitDistance / 2);
            offset2.y = Mathf.Clamp(offset2.y, -splitDistance / 2, splitDistance / 2);


           
            float moving2Percent = ((Vector3.Distance(player2.position, midPoint) / 2) * 100) / (maxSplitDistance / 2);
            Vector3 newOffset2 = new Vector3(offset2.x, (offsetMultiplier * offset2.y) / 2, offset2.z);
            Vector3 newCamera2Position = camera2.transform.position + ((moving2Percent / 100) * (player2.transform.position - camera2.transform.position + offset2));
            camera2.transform.position = new Vector3(newCamera2Position.x, newCamera2Position.y, -10);
            //Vector3 newCamera2Position = camera2.transform.position + ((moving2Percent / 100) * (player2.transform.position - camera2.transform.position));
            //camera2.transform.position = new Vector3(newCamera2Position.x + offset2.x, newCamera2Position.y + (offset2.y / 2), -10);

            if (splitDistance >= maxSplitDistance)
            {
                camera1.transform.position = new Vector3(player1.transform.position.x + offset.x, player1.transform.position.y + (offset.y ), -10);
                camera2.transform.position = new Vector3((player2.transform.position.x + offset2.x), player2.transform.position.y + (offset2.y ), -10);
            }

            splitter.transform.localPosition = new Vector3(xQuad, yQuad, Camera.main.nearClipPlane);

        }
        else
        {
            midPoint = new Vector3((player1.position.x + player2.position.x) / 2, (player1.position.y + player2.position.y) / 2, 0);
            camera1.transform.position = new Vector3(midPoint.x, midPoint.y, -10);
            splitter.SetActive(false);
            camera2.transform.position = camera1.transform.position;
            camera2.SetActive(false);
            screenDivider.SetActive(false);
            splitDistance = 10;
        }
    }


    private void findSplitterLocation(float angle)
    {

        double wantedSpot = 0.5 * (Mathf.Sin(((angle - 90) * Mathf.Deg2Rad)));
        double wantedXSpot = 0.5 * (Mathf.Sin(((angle) * Mathf.Deg2Rad)));

        yQuad = (float)wantedSpot;
        xQuad = (float)wantedXSpot;


        if (Mathf.Abs(Mathf.Floor(angle)) == 180)
        {
            xQuad = 0;
            yQuad = 0.5f;
        }

        if (Mathf.Abs(Mathf.Floor(angle)) == 0)
        {
            xQuad = 0;
            yQuad = -0.5f;
        }

        if (Mathf.Floor(angle) == 90)
        {
            xQuad = 0.5f;
            yQuad = 0;
        }

        if (Mathf.Floor(angle) == -90)
        {
            xQuad = -0.5f;
            yQuad = 0;
        }



    }

    private void getOffsetMultiplier(float distance)
    {
        float offsetOfDistance = maxSplitDistance - (maxSplitDistance / 2);
        if (distance > maxSplitDistance)
        {
            distance = maxSplitDistance;
        }
        float distanceToConvert = distance - (maxSplitDistance / 2) - (offsetOfDistance/2); 
        if(distanceToConvert < 0)
        {
            distanceToConvert = 0;
        }
        float percentOfDistance = (distanceToConvert*100)/ (offsetOfDistance/2);

        offsetMultiplier = (percentOfDistance/100) + 1;
    }
}
