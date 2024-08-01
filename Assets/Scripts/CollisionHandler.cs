using System;
using UnityEngine;
using UnityEngine.SceneManagement;
public class CollisionHandler : MonoBehaviour
{
    [SerializeField] private float levelLoadDelay = 1f;

    AudioSource audioSource;
    [SerializeField] AudioClip crashAudio;
    [SerializeField] AudioClip succeseAudio;

    [SerializeField] ParticleSystem crashParticle;
    [SerializeField] ParticleSystem successParticle;

    private Movement movementScript;
    [SerializeField] public State state;

    bool collisionDisabled = false;

    public enum State
    {
        Playing,
        Transitioning
    }

    private void Start()
    {
        state = State.Playing;
        audioSource = GetComponent<AudioSource>();
        movementScript = GetComponent<Movement>();
    }

    private void Update()
    {
        RespondToDebugKey();
    }

    void RespondToDebugKey()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            StartNextLevelSequence();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            collisionDisabled = !collisionDisabled;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (collisionDisabled)
        {
            return;
        }
        switch (other.gameObject.tag)
        {
            case "Finish":
                if(state != State.Transitioning)
                {
                    StartNextLevelSequence();
                }
                break;
            case "Friendly":
                //do nothing.
                Debug.Log("hitted a friendly object");
                break;
            case "Fuel":
                //Rocket.fuel ++;
                Debug.Log("fill in a fuel");
                break;
            case "Obstacle":
                //Rocket.health--;
                Debug.Log("hitted an obstacle");
                break;
            default:
                if(state != State.Transitioning)
                {
                    StartCrashSequence();
                }
                break;
        }
    }

    void StartNextLevelSequence()
    {
        //TODO: add particale effect
        state = State.Transitioning;
        audioSource.Stop();
        audioSource.PlayOneShot(succeseAudio);
        successParticle.Play();
        movementScript.enabled = false;
        Invoke("LoadNextScene", levelLoadDelay);
    }

    void StartCrashSequence()
    {
        //TODO: add particale effect
        state = State.Transitioning;
        audioSource.Stop();
        audioSource.PlayOneShot(crashAudio);
        crashParticle.Play();
        movementScript.enabled = false;
        Invoke("ReloadScene", levelLoadDelay);
    }

    void ReloadScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex; //get the active scene index
        SceneManager.LoadScene(currentSceneIndex);
    }

    void LoadNextScene()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }
        SceneManager.LoadScene(nextSceneIndex); 
    }
}