using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    Rigidbody2D m_Rigidbody2D;
    bool m_FacingRight = true;
    bool facingRight = true;
    bool isHitted = false;

    [SerializeField]
    float speed = 5f;

    [SerializeField]
    float life = 10;
    [SerializeField]
    bool isInvincible = false;

    [SerializeField]
    GameObject enemy;
    float distToPlayer;
    float distToPlayerY;
    float meleeDist = 1f;
    float rangeDist = 5f;
    bool canAttack = true;
    Transform attackCheck;
    float dmgValue = 4;

    [SerializeField]
    GameObject throwableObject;

    float randomDecision = 0;
    bool doOnceDecision = true;
    bool endDecision = false;
    Animator anim;

    string currentAnimation;

    private void Awake()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
       // attackCheck = transform.Find("AttackCheck").transform;
        anim = transform.Find("Model").GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if(life <= 0)
        {
            //death
        }
        else if(enemy != null)
        {
            if (!isHitted)
            {
                distToPlayer = enemy.transform.position.x - transform.position.x;
                distToPlayerY = enemy.transform.position.y - transform.position.y;

                if(Mathf.Abs(distToPlayer) < .25f)
                {
                    GetComponent<Rigidbody2D>().velocity = new Vector2(0, m_Rigidbody2D.velocity.y);
                    ChangeAnimation("Mushroom_Idle");
                }
                else if(Mathf.Abs(distToPlayer) > .25f && Mathf.Abs(distToPlayer) < meleeDist && Mathf.Abs(distToPlayerY) < 2f)
                {
                    GetComponent<Rigidbody2D>().velocity = new Vector2(0f, m_Rigidbody2D.velocity.y);

                    // nguoi choi vao vung phat hien se quay mat lai
                    if (((distToPlayer > 0f && transform.localScale.x < 0f) || (distToPlayer < 0f && transform.localScale.x > 0f)) && Mathf.Abs(distToPlayerY) < 2f)
                        Flip();
                    if (canAttack)
                    {
                        //MeleeAttack();
                    }
                }
                else if (Mathf.Abs(distToPlayer) > meleeDist && Mathf.Abs(distToPlayer) < rangeDist)
                {
                    // di den vi tri nguoi choi de tan cong
                    m_Rigidbody2D.velocity = new Vector2(distToPlayer / Mathf.Abs(distToPlayer) * speed, m_Rigidbody2D.velocity.y);
                }
                else
                {
                    if (!endDecision)
                    {
                        if ((distToPlayer > 0f && transform.localScale.x < 0f) || (distToPlayer < 0f && transform.localScale.x > 0f))

                        Flip();

                        //if (randomDecision < 0.4f)
                        //    Run();
                        //else if (randomDecision >= 0.4f && randomDecision < 0.6f)
                        //    Jump();
                        //else if (randomDecision >= 0.6f && randomDecision < 0.8f)
                        //    StartCoroutine(Dash());
                        //else if (randomDecision >= 0.8f && randomDecision < 0.95f)
                        //    RangeAttack();
                        //else
                        //    Idle();
                    }
                    else
                    {
                        endDecision = false;
                    }
                }
            }
            else if (isHitted)
            {
                // khi bi tan cong se chay animation va bi day ve sau
                if ((distToPlayer > 0f && transform.localScale.x > 0f) || (distToPlayer < 0f && transform.localScale.x < 0f))
                {
                    Flip();
                    //StartCoroutine(Dash());
                }
               // else
                   // StartCoroutine(Dash());
            }
        }

        if (transform.localScale.x * m_Rigidbody2D.velocity.x > 0 && !m_FacingRight && life > 0)
        {
            // ... flip the player.

            Flip();
        }
        // Otherwise if the input is moving the player left and the player is facing right...
        else if (transform.localScale.x * m_Rigidbody2D.velocity.x < 0 && m_FacingRight && life > 0)
        {
            // ... flip the player.

            Flip();
        }
    }

    void Flip()
    {
        // Switch the way the player is labelled as facing.
        facingRight = !facingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    protected virtual void ChangeAnimation(string newAnimation)
    {
        if (currentAnimation == newAnimation) return;
        anim.Play(newAnimation);
        currentAnimation = newAnimation;
    }
}
