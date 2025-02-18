using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public float maxPower = 8f;
    public float maxPullDistance = 2.5f;
    public float maxSpeed = 7f;
    public float speedDamping = 0.98f;

    private Vector2 startPos, endPos, force;
    private bool isDragging = false;
    private Rigidbody2D rb;
    private LineRenderer lineRenderer;
    public GameObject arrowPrefab;
    private GameObject arrowInstance;
    private Color pullColor;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        lineRenderer = GetComponent<LineRenderer>();

        if (lineRenderer != null)
        {
            lineRenderer.positionCount = 2;
            lineRenderer.startWidth = 0.1f;
            lineRenderer.endWidth = 0.05f;
        }


        if (arrowPrefab != null)
        {
            arrowInstance = Instantiate(arrowPrefab, transform.position, Quaternion.identity);
            arrowInstance.SetActive(false);
        }
    }

    void Update()
    {
        if (!TurnManager.instance.IsPlayerTurn(gameObject)) return;

        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (Vector2.Distance(mousePos, transform.position) < 0.5f)
            {
                startPos = mousePos;
                isDragging = true;

                if (lineRenderer != null)
                {
                    lineRenderer.enabled = true;
                }

                if (arrowInstance != null)
                {
                    arrowInstance.SetActive(true);
                }
            }
        }

        if (Input.GetMouseButton(0) && isDragging)
        {
            Vector2 currentPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            force = startPos - currentPos;


            float pullMagnitude = force.magnitude;
            if (pullMagnitude > maxPullDistance)
            {
                force = force.normalized * maxPullDistance;
                pullMagnitude = maxPullDistance;
            }

            if (lineRenderer != null)
            {
                lineRenderer.SetPosition(0, transform.position);
                lineRenderer.SetPosition(1, transform.position + (Vector3)force);

                pullColor = Color.Lerp(Color.green, Color.red, pullMagnitude / maxPullDistance);
                lineRenderer.startColor = pullColor;
                lineRenderer.endColor = pullColor;
            }

            if (arrowInstance != null)
            {
                Vector3 arrowPos = transform.position + (Vector3)force * 0.75f; 
                arrowInstance.transform.position = arrowPos;

                float angle = Mathf.Atan2(force.y, force.x) * Mathf.Rad2Deg;
                arrowInstance.transform.rotation = Quaternion.Euler(0, 0, angle);

                arrowInstance.GetComponent<SpriteRenderer>().color = pullColor;

                float scaleMultiplier = 0.2f + (pullMagnitude / maxPullDistance) * 0.5f;
                arrowInstance.transform.localScale = new Vector3(scaleMultiplier, 0.2f, 0.2f);
            }
        }

        if (Input.GetMouseButtonUp(0) && isDragging)
        {
            endPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            force = startPos - endPos;

            if (force.magnitude > maxPullDistance)
            {
                force = force.normalized * maxPullDistance;
            }

            rb.velocity = Vector2.zero;
            rb.AddForce(force * maxPower, ForceMode2D.Impulse);

            if (lineRenderer != null)
            {
                lineRenderer.enabled = false;
            }

            if (arrowInstance != null)
            {
                arrowInstance.SetActive(false);
            }

            isDragging = false;
            TurnManager.instance.MarkTurnChange();
        }
    }

    void FixedUpdate()
    {
        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity *= speedDamping;
        }
    }
}