using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow2DTarget : MonoBehaviour
{
    public Transform player;
    public float speed = 2.0f;

    void FixedUpdate()
    {
        float interpolation = speed * Time.deltaTime;

        Vector3 position = this.transform.position;
        position.y = Mathf.Lerp(this.transform.position.y, player.transform.position.y, interpolation);
        position.x = Mathf.Lerp(this.transform.position.x, player.transform.position.x, interpolation);

        this.transform.position = position;
    }
}
