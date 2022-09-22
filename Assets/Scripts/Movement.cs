using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] float mainThrust = 100f;
    [SerializeField] float rotateSpeed = 100f;
    [SerializeField] AudioClip engineSound;
    Rigidbody rocketRB;
    AudioSource rocketBoost;

    // Start is called before the first frame update
    void Start()
    {
        rocketRB = GetComponent<Rigidbody>();
        rocketBoost = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessInput();
        ProcessRotation();
        ProcessThrust();
        ResetPosition();
    }

    void ProcessInput(){

        if(Input.GetKey(KeyCode.Space)){
            Debug.Log("Whatup!");
        }

    }

    void ProcessRotation(){

        if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D)){
            Debug.Log("go straight!");
        }
        else if(Input.GetKey(KeyCode.A)){
            ApplyRotation(rotateSpeed);
            //Debug.Log("to the left!");
        }
        else if(Input.GetKey(KeyCode.D)){
            ApplyRotation(-rotateSpeed);
            //Debug.Log("to the right!");
        }  

    }
    
    void ProcessThrust() {
        if (Input.GetKey(KeyCode.Space)){
            rocketRB.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);
            if (!rocketBoost.isPlaying){
                rocketBoost.PlayOneShot(engineSound);
            }
        } else {
            rocketBoost.Stop();
        }
    }

    void ApplyRotation(float rotationThisFrame){
        rocketRB.freezeRotation = true; //* freezing rotation so we can manually rotate (so physics does not fully take over)
        transform.Rotate(Vector3.forward * rotationThisFrame * Time.deltaTime);
        rocketRB.freezeRotation = false;
    }
    void ResetPosition(){
        if (Input.GetKey(KeyCode.R)){
            transform.eulerAngles = Vector3.zero;
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            transform.position = new Vector3(-14.9f, 1.65f, -0.01f);
        }
    }

}
