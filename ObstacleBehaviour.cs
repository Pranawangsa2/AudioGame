using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleBehaviour : MonoBehaviour
{
    public float speed = 1.0f;

    // Update is called once per frame
    void Update()
    {
        transform.position += ((Vector3.left * speed) * Time.deltaTime);
    }
}
