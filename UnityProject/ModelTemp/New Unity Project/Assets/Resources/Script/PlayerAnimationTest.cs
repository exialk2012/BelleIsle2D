using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationTest : MonoBehaviour {
    public Animator animator;
    public const string attack = "Attack";
    public bool isAttack = false;

    // Use this for initialization
    private void Awake()
    {
        animator = gameObject.GetComponent<Animator>();
    }
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.Q))
        {
            isAttack = !isAttack;
            animator.SetTrigger(attack);
        } 
	}
}
