using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]

public class Tiling : MonoBehaviour
{
    public int offsetX = 10;
    public bool hasRightBuddy = false;
    public bool hasLeftBuddy = false;

    public bool reverseScale = false;

    private float spriteWidth = 0f;

    private Camera cam;
    private Transform myTransform;

    private void Awake()
    {
        cam = Camera.main;
        myTransform = transform;
    }

    // Use this for initialization
    void Start ()
    {
        SpriteRenderer sRenderer = GetComponent<SpriteRenderer>();
        spriteWidth = sRenderer.sprite.bounds.size.x;
	}
	
	// Update is called once per frame
	void Update ()
    {
        // does it still need buddies? if not continue
        if (!hasLeftBuddy || !hasRightBuddy)
        {
            // calculate the cameras extend (half the width) of what the camera can see in the world coordinates
            float camHorizontalExtend = (cam.orthographicSize * Screen.width) / Screen.height;

            // calculate the x position where the camera can see the edge of the sprite (element)
            float edgeVisiblePositionRight = (myTransform.position.x + (spriteWidth / 2)) - camHorizontalExtend;
            float edgeVisiblePositionLeft = (myTransform.position.x - (spriteWidth / 2)) + camHorizontalExtend;

            // checking if we can see the edge of an element and then calling MakeNewBuddy if we cannot
            if ((cam.transform.position.x >= edgeVisiblePositionRight - offsetX) && !hasRightBuddy)
            {
                MakeNewBuddy(1);
                hasRightBuddy = true;
            }
            else if ((cam.transform.position.x <= edgeVisiblePositionLeft + offsetX) && !hasLeftBuddy)
            {
                MakeNewBuddy(-1);
                hasLeftBuddy = true;
            }
        }
	}

    // a function that makes new buddy on the required side
    private void MakeNewBuddy(int rightOrLeft)
    {
        // calculating the new position for our new buddy
        Vector3 newPosition = new Vector3(
            myTransform.position.x + (spriteWidth * rightOrLeft),
            myTransform.position.y,
            myTransform.position.z
        );
        // instantiating our new buddy and storing him in new variable
        Transform newBuddy = Instantiate(myTransform, newPosition, myTransform.rotation) as Transform;

        // if not tileble let's reverse the x size of our object to get rid of ugly seams
        /*if (reverseScale)
        {
            newBuddy.localScale = new Vector3(
                newBuddy.localScale.x*-1,
                newBuddy.localScale.y,
                newBuddy.localScale.z
                );
        }*/

        newBuddy.parent = myTransform.parent;
        if (rightOrLeft > 0)
        {
            newBuddy.GetComponent<Tiling>().hasLeftBuddy = true;
        }
        else
        {
            newBuddy.GetComponent<Tiling>().hasRightBuddy = true;
        }

    }
}
