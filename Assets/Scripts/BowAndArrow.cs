using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowAndArrow : MonoBehaviour
{
    LineRenderer bow_wire;
    AudioSource sound;
    
    // bow string points
    public GameObject PointA;
    public GameObject PointB;
    public GameObject PointC;

    // arrows game objects
    public GameObject Arrow;
    public GameObject ArrowInTarget;
    public GameObject eye;
    public GameObject Target;

    // bow variables
    bool buttonIsPressed;
    int framesCounter;
    int maxCounter = 150;
    float delta = 0.005f;

    // Start is called before the first frame update
    void Start()
    {
        bow_wire = GetComponent<LineRenderer>();
        sound = GetComponent<AudioSource>();
        bow_wire.SetWidth(0.01f, 0.01f); // set the width of the bow wire 
        
        // set position for point B in the midpoint of the wire
        PointB.transform.position = new Vector3((PointA.transform.position.x + PointC.transform.position.x) / 2,
            (PointA.transform.position.y + PointC.transform.position.y) / 2,
            (PointA.transform.position.z + PointC.transform.position.z) / 2);
    }

    // Update is called once per frame
    void Update()
    {

        shootingArrow();
        
        if(Input.GetMouseButtonDown(1)) // if the right mouse button is clicked
        {
            Arrow.SetActive(true); // reloads the arrow in the bow
        }

        if (buttonIsPressed && framesCounter < maxCounter) 
        {
            // move point B back along the X-axis to show the wire pulling
            PointB.transform.Translate(- delta, 0,0);
            framesCounter++;
        }
    }

    private void shootingArrow()
    {
        if (Input.GetMouseButtonDown(0)) // if the left mouse button is clicked
        {
            if (Arrow.activeSelf)
            {
                buttonIsPressed = true;
                framesCounter = 0;
            }
        }
        else if (Input.GetMouseButtonUp(0) && Arrow.activeSelf) // if the left mouse button is released
        {
            buttonIsPressed = false;

            // reset poit B to the midpoint
            PointB.transform.position = new Vector3((PointA.transform.position.x + PointC.transform.position.x) / 2,
                (PointA.transform.position.y + PointC.transform.position.y) / 2,
                (PointA.transform.position.z + PointC.transform.position.z) / 2);
            Arrow.SetActive(false);
            sound.Play();


            RaycastHit hit;
            if (Physics.Raycast(eye.transform.position, eye.transform.forward, out hit))
            {
                // set where an arrow hitting a target
                ArrowInTarget.SetActive(true);
                ArrowInTarget.transform.position = hit.point;
                Target.transform.position = hit.point;
                ArrowInTarget.transform.rotation = Arrow.transform.rotation;
            }
        }
    }

    // runs after all updates have finished to make sure the wire is drawn correctly
    private void LateUpdate()
    {
        bow_wire.SetPosition(0, PointA.transform.position);
        bow_wire.SetPosition(1, PointB.transform.position);
        bow_wire.SetPosition(2, PointC.transform.position);
    }
}
