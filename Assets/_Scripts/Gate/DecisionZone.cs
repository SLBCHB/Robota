using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class DecisionZone : MonoBehaviour
{
    [Header("Zone Settings")]
    public string zoneName = "Accept Zone";
    public Vector2 exitVelocity = new Vector2(30f, 0f);

    [Header("Events")]
    public UnityEvent<SubjectEntity> OnSubjectProcessed;
    
    private void Start()
    {
        Collider2D col = GetComponent<Collider2D>();
        col.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log($"{zoneName}: {collision.gameObject.name}");
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out SubjectEntity subject))
        {
            if (subject.IsProcessed || subject.IsBeingDragged || subject.IsSliding) return;

            ProcessSubject(subject);
        }
    }

    private void ProcessSubject(SubjectEntity subject)
    {
        subject.IsProcessed = true;
        subject.gameObject.tag = "Untagged"; 

        Rigidbody2D rb = subject.GetComponent<Rigidbody2D>();
        rb.linearDamping = 0f;
        rb.linearVelocity = exitVelocity;

        Debug.Log($"<color=orange>✅ {subject.gameObject.name} WAS PROCESSED BY {zoneName}!</color>");
        
        OnSubjectProcessed?.Invoke(subject); 
    }
}