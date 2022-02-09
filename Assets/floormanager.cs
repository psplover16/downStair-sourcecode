using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class floormanager : MonoBehaviour
{
    [SerializeField] GameObject[] floorObject;
    public void addFloor()
    {
        int r = Random.Range(0, floorObject.Length);
        // Instantiate實例化(要生成的物件, 物件位置, 物件旋轉值);
        //  自訂   Position : new Vector3(3,0,0)
        //  自訂   Rotation : new Quaternion(0,90,0,0)
        GameObject gameobj = Instantiate(floorObject[r], transform);
        // Debug.Log(transform.childCount);//8
        // Debug.Log(transform.Find("normal"));
        // Debug.Log(transform.GetChild(0));
        gameobj.transform.position = new Vector3(Random.Range(-7f, 8f), -3.1f, 0);

    }


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
