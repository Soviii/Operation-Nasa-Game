using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Movement : MonoBehaviour
{
    [SerializeField] float mainThrust = 100f;
    [SerializeField] float rotateSpeed = 100f;
    [SerializeField] AudioClip engineSound;
    [SerializeField] ParticleSystem leftThrusterParticles;
    [SerializeField] ParticleSystem rightThrusterParticles;
    [SerializeField] ParticleSystem boosterParticles;

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
        ForceNextLevel();
        ProcessRotation();
        ProcessThrust();
        ResetPosition();
    }
    void ForceNextLevel(){
        if(Input.GetKeyDown(KeyCode.L)){
            SceneManager.LoadScene((SceneManager.GetActiveScene().buildIndex+1)%SceneManager.sceneCountInBuildSettings);
        }
    }

    void ProcessRotation(){

        if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D))
        {
            BothThrustersOn();
        }
        else if(Input.GetKey(KeyCode.A))
        {
            LeftThrusterOn();
            ApplyRotation(rotateSpeed);
            //Debug.Log("to the left!");
        }
        else if(Input.GetKey(KeyCode.D))
        {
            RightThrusterOn();
            ApplyRotation(-rotateSpeed);
            //Debug.Log("to the right!");
        }
        else
        {
            //* Turn off both thrusters
            leftThrusterParticles.Stop();
            rightThrusterParticles.Stop();
        }
    }

    void ProcessThrust() {
        if (Input.GetKey(KeyCode.Space)){
            rocketRB.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);
            
            if (!boosterParticles.isPlaying){
                boosterParticles.Play();
            }
            if (!rocketBoost.isPlaying){
                rocketBoost.PlayOneShot(engineSound);
            }
        } else {
            rocketBoost.Stop();
            boosterParticles.Stop();
        }
    }
    
    void ResetPosition(){
        if (Input.GetKey(KeyCode.R)){
            transform.eulerAngles = Vector3.zero;
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            transform.position = new Vector3(-14.9f, 1.65f, -0.01f);
        }
    }

    void RightThrusterOn()
    {
        if (!rightThrusterParticles.isPlaying)
        {
            rightThrusterParticles.Play();
        }
        leftThrusterParticles.Stop();
    }

    void LeftThrusterOn()
    {
        if (!leftThrusterParticles.isPlaying)
        {
            leftThrusterParticles.Play();
        }
        rightThrusterParticles.Stop();
    }

    void BothThrustersOn()
    {
        if (!leftThrusterParticles.isPlaying)
        {
            leftThrusterParticles.Play();
        }
        else if (!rightThrusterParticles.isPlaying)
        {
            rightThrusterParticles.Play();
        }
        Debug.Log("go straight!");
    }

    

    void ApplyRotation(float rotationThisFrame){
        rocketRB.freezeRotation = true; //* freezing rotation so we can manually rotate (so physics does not fully take over when colliding)
        transform.Rotate(Vector3.forward * rotationThisFrame * Time.deltaTime);
        rocketRB.freezeRotation = false;
    }


}
