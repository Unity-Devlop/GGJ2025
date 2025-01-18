using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    public float speed = 1f;
    public float maxTop = 8.0f;
    public bool Enabled = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void Explore()
    {
        Destroy(gameObject);
    }

    private void Move()
    {
        if (!Enabled) return;
        if (transform.position.y > maxTop)
        {
            Explore();
        }

        var deltaTime = Time.deltaTime;
        var direction = Vector3.up;
        transform.Translate(deltaTime * speed * direction);
    }

    private void OnMouseDown()
    {
        Explore();
    }

}
