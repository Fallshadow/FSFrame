using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ST : Singleton<ST>
{
    public ST()
    {
        Debug.Log("ST");
    }

    public string name = "HA";
}

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(ST.Instance.name);
        Debug.Log("1");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
