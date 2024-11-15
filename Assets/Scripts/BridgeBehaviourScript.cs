using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeBehaviourScript : MonoBehaviour
{
    Animator animator;
    AudioSource sound;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        sound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        animator.SetBool("bridgeOpen", true);
        sound.Play();
    }
    private void OnTriggerExit(Collider other)
    {
        animator.SetBool("bridgeOpen", false);        
        sound.PlayDelayed(0.3f);
    }
}
