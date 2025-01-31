using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public enum MissleType
{
    BlueSprit,
    RedSprit,
    GreenSprit,
    MagicMissle
}
public class SpritMissle : MonoBehaviour
{
    [SerializeField]
    private MissleType missleType;
    [SerializeField]


    private Transform target; 
    public float speed = 5f;
    public float maxX;
    public float minX;
    public float maxY;
    public float minY;
    public float maxZ;
    public float minZ;

    private float tParam = 0f;
    private Vector3 missileStartPosition;
    private Vector3 controlPoint;

    void Start()
    {
        if (missleType == MissleType.MagicMissle)
        {
            target = GameObject.FindGameObjectWithTag("Boss").GetComponent<BossController>().BloodPosition;
        }
        else
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
        }
        missileStartPosition = transform.position;
        Vector3 midPoint = new Vector3(0,0,0);
        if (target != null)
        {
            midPoint = (missileStartPosition + target.position) / 2;
        }
        controlPoint = midPoint + CalculateRandomOffset();
    }

    void Update()
    {
        if (tParam < 1 && target != null)
        {
            tParam += Time.deltaTime * speed;
            transform.position = CalculateBezierPoint(tParam, missileStartPosition, controlPoint, target.position);
        }

    }

    // Reference: https://gooning.wordpress.com/2017/04/07/bezier-curves-for-your-games-a-tutorial/
    // Calculate a point on the Bezier curve using given control points.
    // This method is used to interpolate missile movement.
    Vector3 CalculateBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;

        Vector3 p = uu * p0;
        p += 2 * u * t * p1;
        p += tt * p2;

        return p;
    }

    // Calculate a random offset for the Bezier curve control point.
    Vector3 CalculateRandomOffset()
    {
        float randomX = Random.Range(minX,maxX);
        float randomY = Random.Range(minY,maxY);
        float randomZ = Random.Range(minZ, maxZ);

        return new Vector3(randomX, randomY, randomZ);
    }

    // Handle collision events when the missile interacts with other objects.
    // The specific action taken depends on the missile type (e.g., MagicMissle, RedSprit).
private void OnTriggerEnter(Collider other)
    {
        PlayerController playerController;
        BossController bossController;
        switch (missleType)
        {
            case MissleType.MagicMissle:
                if (other.tag == "Boss")
                {
                    bossController = other.GetComponent<BossController>();
                    bossController.TakeDamage(50f);

                    Destroy(this.gameObject);
                }
                break;
            case MissleType.RedSprit:
                if (other.tag == "Player")
                {
                    playerController = other.GetComponent<PlayerController>();
                    playerController.AddSpiritNum("red");

                    Destroy(this.gameObject);
                }
                
                break;
            case MissleType.BlueSprit:
                if (other.tag == "Player")
                {
                    playerController = other.GetComponent<PlayerController>();
                    playerController.AddSpiritNum("blue");
                    Destroy(this.gameObject);
                }
                break;
            case MissleType.GreenSprit:
                if (other.tag == "Player")
                {
                    playerController = other.GetComponent<PlayerController>();
                    playerController.AddSpiritNum("green");

                    Destroy(this.gameObject);
                }
                break;
        }
    }
}
