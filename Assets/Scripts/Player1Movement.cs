﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]
public class Player1Movement : MonoBehaviour {
	public float Speed = 0f;
	private float movex = 0f;
	private float movey = 0f;

	private Rigidbody2D rb;

    public Wave wave;
    private int bounce = 0;
    private Vector3 bounceAngle;

// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D>();
	}

// Update is called once per frame
	void Update () {

	    if (transform.position.x < -12)
	    {
            SceneManager.LoadScene("GameOver");
        }

	    if (bounce > 0)
	    {
	        movex = bounceAngle.x;
	        movey = bounceAngle.y;
            bounce--;

	        return;
	    }

	    if (GameManager.player1Lost)
	    {
	        movex = -1;
	        movey = 0;
	        return;
	    }
	    if (GameManager.player1Safe)
	    {
	        movex = 0;
	        movey = 0;
	        return;
	    }
		if (Input.GetKey (KeyCode.A))
			movex = -1;
		else if (Input.GetKey (KeyCode.D))
			movex = 1;
		else
			movex = 0;
		if (Input.GetKey (KeyCode.W))
			movey = 1;
		else if (Input.GetKey(KeyCode.S)){
			movey = -1;
		}else{
			movey = 0;
		}
	}

	void FixedUpdate ()
	{
		rb.velocity = new Vector2 (movex * Speed, movey * Speed);
	}

    public void player1Loses()
    {
        GameManager.player1Lost = true;
    }

    private void disableCollision()
    {
        GetComponent<Collider2D>().enabled = false;
        var Components = GetComponentsInChildren<Collider2D>();
        foreach (var component in Components)
        {
            component.enabled = false;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDocking(collision))
        {
            GameManager.player1Safe = true;
            disableCollision();

        }
        if (collision.gameObject.tag == "Waves" && !GameManager.player1Safe)
        {
            player1Loses();
            disableCollision();
        }

        if (collision.gameObject.tag == "Player2")
        {
            bounce = 3;
            bounceAngle = new Vector3(collision.contacts.First().point.x, collision.contacts.First().point.y);
            bounceAngle = transform.position - bounceAngle;
            bounceAngle = customNormalize(bounceAngle);
        }
    }

    Vector3 customNormalize(Vector3 v)
    {
        float length = v.x*v.x + v.y*v.y;
        v.x /= length;
        v.y /= length;
        return v;
    }

    bool isDocking(Collision2D collision)
    {
        return collision.gameObject.tag == "Dock1";
    }
}
