using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator anim;
    private PlayerController player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
        player = GetComponent<PlayerController>();
    }
    private void ChooseAnimation(PlayerController p)
    {
        anim.SetBool("IsIdle", false);
        anim.SetBool("IsMove", false);

        switch(p.State)
        {
             case PlayerState.Idle:
            anim.SetBool("IsIdle", true);
            break;
            case PlayerState.Move:
            anim.SetBool("IsMove", true);
            break;

        }

    }

    // Update is called once per frame
    void Update()
    {
        ChooseAnimation(player);
    }
}
