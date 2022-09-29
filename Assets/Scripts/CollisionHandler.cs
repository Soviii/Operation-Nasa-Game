using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] AudioClip crashSound;
    [SerializeField] AudioClip successSound;
    [SerializeField] ParticleSystem crashParticles;
    [SerializeField] ParticleSystem successParticles;

    AudioSource audioSource;
    float delayInSeconds = 1f;
    bool isTransitioning = false;
    bool haltCollisions = false;

    void Start(){
        audioSource = GetComponent<AudioSource>();
    }

    void Update(){
        DisableCollision();
    }

    void DisableCollision(){
        if (Input.GetKeyDown(KeyCode.C)){
            haltCollisions = !haltCollisions;
            Debug.Log("Collision cheat is now: " + haltCollisions);
        }
    }
    void OnCollisionEnter(Collision other) {
        if(isTransitioning || haltCollisions){ return; }
        
        switch(other.gameObject.tag){
            case "Friendly":
                // Debug.Log("This thing is friendly!");
                break;
            case "Enemy":
                // Debug.Log("AHHHH AN ENEMY!");
                StartCrashSequence();
                break;
            case "Finish":
                // Debug.Log("Congrats, you finished!");
                StartSuccessSequence();
                break;
            default:
                StartCrashSequence();
                break;
        }
    }

    void StartSuccessSequence(){
        isTransitioning = true; //*don't need to set back to false since reloading scene assigns it to false 
        audioSource.Stop();
        successParticles.Play(successParticles);
        GetComponent<Movement>().enabled = false;
        audioSource.PlayOneShot(successSound);
        Invoke("LoadNextLevel", delayInSeconds);
    }
    void StartCrashSequence(){
        isTransitioning = true; //*don't need to set back to false since reloading scene assigns it to false 
        audioSource.Stop();
        crashParticles.Play(crashParticles);
        GetComponent<Movement>().enabled = false;
        audioSource.PlayOneShot(crashSound);
        Invoke("ReloadLevel", delayInSeconds);
    }

    void ReloadLevel(){
        //SceneManager.LoadScene(0);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    void LoadNextLevel(){
        int nextScene = (SceneManager.GetActiveScene().buildIndex + 1) % SceneManager.sceneCountInBuildSettings;
        SceneManager.LoadScene(nextScene);
    }
}
