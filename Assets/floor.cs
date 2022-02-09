using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class floor : MonoBehaviour
{
    [SerializeField] float downspeed = 2f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log(transform.parent.childCount);//7
        transform.Translate(0, downspeed * Time.deltaTime, 0);
        if (
            gameObject.transform.position.y > 5.5f
        )
        {

            // Transform.Destroy(gameObject);

            transform.parent.GetComponent<floormanager>().addFloor();
            Destroy(gameObject);
        }
    }
}
