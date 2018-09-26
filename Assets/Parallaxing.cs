using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallaxing : MonoBehaviour
{
    public Transform[] backgrounds; // array of all the backgrounds and foregrounds to be parallaxed

    private float[] paralaxScales; // The proportions of the camera's movement to move the backgrounds by

    public float smoothing = 1f; // How smooth the parallax is going to be. Must be above 0

    private Transform cam; // reference to the main camera's transform

    private Vector3 previousCamPosition; // position of the main camera in the previous frame

    // Is called before Start(). Great for references
    private void Awake()
    {
        // set up the camera reference
        cam = Camera.main.transform;
    }
    
    // Use this for initialization
    void Start ()
    {
        // the previous frame had the current frame's camera position
        previousCamPosition = cam.position;

        // assigning corresponding parallax scales
        paralaxScales = new float[backgrounds.Length];
        for (int i = 0; i < backgrounds.Length; i++)
        {
            paralaxScales[i] = backgrounds[i].position.z * (-1);
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
        // for each background
        for (int i = 0; i < backgrounds.Length; i++)
        {
            // the parallax is the opposite of the camera movement because the previous frame multiplied by the scale
            float parallax = (previousCamPosition.x - cam.position.x) * paralaxScales[i];

            // set a target x position which is the current position plus the parallax
            float backgroundTargetPositionX = backgrounds[i].position.x + parallax;

            // create a target position which is the background's current position with its target x position
            Vector3 backgroundTargetPosition = new Vector3(
                backgroundTargetPositionX,
                backgrounds[i].position.y,
                backgrounds[i].position.z
                );

            // fade between current position and the target position using lerp
            backgrounds[i].position = Vector3.Lerp(backgrounds[i].position, backgroundTargetPosition, smoothing * Time.deltaTime);
        }

        // set the previous camera position to the camera position at the end of the frame
        previousCamPosition = cam.position;
	}
}
