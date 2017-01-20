using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    private float _speed;
    private Trace _testLine;

	// Use this for initialization
	void Start () {
        _speed = 2f;
        _testLine = GetComponent<Trace>();
	}
	
	// Update is called once per frame
	void Update () {
        bool arrowPressed = false;

        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.position += Vector3.up * _speed * Time.deltaTime;
            arrowPressed = true;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.position += Vector3.down * _speed * Time.deltaTime;
            arrowPressed = true;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.position += Vector3.right * _speed * Time.deltaTime;
            arrowPressed = true;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.position += Vector3.left * _speed * Time.deltaTime;
            arrowPressed = true;
        }

        if (arrowPressed)
            _testLine.addPoint(transform.position);
    }
}
