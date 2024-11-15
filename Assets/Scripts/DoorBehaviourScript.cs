using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBehaviourScript : MonoBehaviour
{
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
     animator = GetComponent<Animator>();   
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        animator.SetBool("DoorOpens", true);
    }
    private void OnTriggerExit(Collider other)
    {
        animator.SetBool("DoorOpens", false);
    }
}
