using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class VoiceMovement : MonoBehaviour
{
    private KeywordRecognizer keywordRecogniser;
    private Dictionary<string, Action> actions = new Dictionary<string, Action>();

    public float speed = 5f;          // Speed of movement
    private Rigidbody rb;             // Reference to Rigidbody
    private bool isMoving = false;    // Track whether the object is moving

    private void Start()
    {
        foreach (var device in Microphone.devices)
        {
            Debug.Log("Name: " + device);
        }

        // Get the Rigidbody component
        rb = GetComponent<Rigidbody>();

        // Set up voice commands
        actions.Add("up", Up);
        actions.Add("down", Down);
        actions.Add("right", Right);
        actions.Add("left", Left);

        keywordRecogniser = new KeywordRecognizer(actions.Keys.ToArray());
        keywordRecogniser.OnPhraseRecognized += RecognisedSpeech;
        keywordRecogniser.Start();
    }

    private void RecognisedSpeech(PhraseRecognizedEventArgs speech)
    {
        Debug.Log(speech.text);
        actions[speech.text].Invoke();
    }

    private void Up()
    {
        //transform.Translate(0, 0, 1);
        Move(Vector3.forward);
    }

    private void Down()
    {
        //transform.Translate(0, 0, -1);
        Move(Vector3.back);
    }

    private void Right()
    {
        //transform.Translate(1, 0, 0);
        Move(Vector3.right);
    }

    private void Left()
    {
        //transform.Translate(-1, 0, 0);
        Move(Vector3.left);
    }

    private void Move(Vector3 direction)
    {
        // Set velocity in the specified direction
        rb.velocity = direction * speed;
        isMoving = true;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Left();
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Right();
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Up();
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Down();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Stop the object on collision
        if (isMoving)
        {
            rb.velocity = Vector3.zero;
            isMoving = false;
            Debug.Log("Collision detected with " + collision.gameObject.name);
        }
    }
}
