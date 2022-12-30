using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    const string LEFT = "left";
    const string RIGHT = "right";

    [SerializeField] Transform castPos;
    [SerializeField]float baseCastDist;

    Rigidbody2D rb;
    [SerializeField]
    float moveSpeed = 5f;

    string facingDirection;

    Vector3 baseScale;

    // Start is called before the first frame update
    void Start()
    {
        facingDirection = RIGHT;
        baseScale = transform.localScale;
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        float velX = moveSpeed;

        if(facingDirection == LEFT)
        {
            velX = -moveSpeed;
        }

        rb.velocity = new Vector2(velX, rb.velocity.y);

        if (IsHittingWall() || IsNearEdge())
        {
            if(facingDirection == LEFT)
            {
                ChangeFacingDirection(RIGHT);
            }
            else if(facingDirection == RIGHT)
            {
                ChangeFacingDirection(LEFT);
            }
        }
    }

    void ChangeFacingDirection(string newDirection)
    {
        Vector3 newScale = baseScale;

        if(newDirection == LEFT)
        {
            newScale.x = -baseScale.x;
        }
        else if(newDirection == RIGHT)
        {
            newScale.x = baseScale.x;
        }

        transform.localScale = newScale;
        facingDirection = newDirection;
    }

    bool IsHittingWall()
    {
        bool val = false;

        float castDist = baseCastDist;
        if (facingDirection == LEFT)
        {
            castDist = -baseCastDist;
        }
        else
        {
            castDist = baseCastDist;
        }

        Vector3 targetPos = castPos.position;
        targetPos.x += castDist;

        Debug.DrawLine(castPos.position, targetPos, Color.blue);

        if (Physics2D.Linecast(castPos.position, targetPos, 1 << LayerMask.NameToLayer("Ground")))
        {
            val = true;
        }
        else val = false;
        return val;
    }

    bool IsNearEdge()
    {
        bool val = true;

        float castDist = baseCastDist;

        Vector3 targetPos = castPos.position;
        targetPos.y += -castDist;

        Debug.DrawLine(castPos.position, targetPos, Color.blue);

        if (Physics2D.Linecast(castPos.position, targetPos, 1 << LayerMask.NameToLayer("Ground")))
        {
            val = false;
        }
        else val = true;
        return val;
    }
}
