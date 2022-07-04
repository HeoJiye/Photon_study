using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoPlatform : MonoBehaviour
{
    public Vector3 vector1;
    public Vector3 vector2;

    public float speed;

    private Vector3 toward;

    void Start() 
    {
        transform.position = vector1;
        toward = vector2;
    } 

    void Update()
    {
        if(transform.position == toward)
            toward = toward == vector1 ? vector2 : vector1;
        else
            transform.position = Vector3.MoveTowards(transform.position, toward, speed);
    }
}
