using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SubjectManager : MonoBehaviour
{
    [Header("Positions")]
    public Transform activeSpot;
    public Transform[] queueSpots; 

    [Header("Spawning Setup")]
    public GameObject[] subjectPrefabs; 

    [Header("Sorting Orders")]
    [Tooltip("Sorting order for the person currently at the desk")]
    public int activeSortingOrder = 0;
    [Tooltip("Sorting order for the people waiting in line")]
    public int queueSortingOrder = 12;

    [Header("Timing")]
    public float delayBeforeMove = 1.0f;
    public float destroyDelay = 3.0f; 

    private SubjectEntity _activeSubject;
    private List<SubjectEntity> _queueList = new List<SubjectEntity>();
    private GameObject _activeCard;

    private void Start()
    {
        _activeSubject = SpawnSubjectAt(activeSpot);
        _activeSubject.gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "Default";

        if (_activeSubject != null) 
        {
            _activeSubject.SetInteractable(true);
            SetSortingOrder(_activeSubject, activeSortingOrder);
        }

        for (int i = 0; i < queueSpots.Length; i++)
        {
            SubjectEntity qPerson = SpawnSubjectAt(queueSpots[i]);
            if (qPerson != null) 
            {
                qPerson.SetInteractable(false); 
                SetSortingOrder(qPerson, queueSortingOrder);
            }
            _queueList.Add(qPerson);
        }
    }

    public void HandleSubjectProcessed(SubjectEntity processedSubject)
    {
        if (processedSubject != null)
        {
            Destroy(processedSubject.gameObject, destroyDelay);
        }
        
        if (_activeCard != null)
        {
            Destroy(_activeCard);
        }

        StartCoroutine(AdvanceQueueRoutine());
    }

    private IEnumerator AdvanceQueueRoutine()
    {
        yield return new WaitForSeconds(delayBeforeMove);

        if (_queueList.Count > 0)
        {
            _activeSubject = _queueList[0];
            _queueList.RemoveAt(0); 
            
            if (_activeSubject != null && activeSpot != null)
            {
                _activeSubject.MoveToNewSpot(activeSpot); 
                _activeSubject.gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "Default";
                _activeSubject.SetInteractable(true); 
                
                SetSortingOrder(_activeSubject, activeSortingOrder);
                
                StartCoroutine(WaitAndTossCard(_activeSubject));
            }

            for (int i = 0; i < _queueList.Count; i++)
            {
                if (_queueList[i] != null && queueSpots[i] != null)
                {
                    _queueList[i].MoveToNewSpot(queueSpots[i]);
                }
            }

            if (queueSpots.Length > 0)
            {
                Transform lastSpot = queueSpots[queueSpots.Length - 1];
                SubjectEntity newPerson = SpawnSubjectAt(lastSpot);
                if (newPerson != null) 
                {
                    newPerson.SetInteractable(false);
                    SetSortingOrder(newPerson, queueSortingOrder);
                    _queueList.Add(newPerson);
                }
            }
        }
    }

    private IEnumerator WaitAndTossCard(SubjectEntity subject)
    {
        yield return new WaitForSeconds(0.5f);
        if (subject != null)
        {
            _activeCard = subject.TossIDCard();
        }
    }

    private SubjectEntity SpawnSubjectAt(Transform spot)
    {
        if (subjectPrefabs == null || subjectPrefabs.Length == 0 || spot == null) return null;

        int randomIndex = Random.Range(0, subjectPrefabs.Length);
        GameObject newObj = Instantiate(subjectPrefabs[randomIndex], spot.position, Quaternion.identity);
        
        SubjectEntity entity = newObj.GetComponent<SubjectEntity>();
        if (entity != null)
        {
            entity.basePosition = spot;
        }
        
        return entity;
    }

    private void SetSortingOrder(SubjectEntity subject, int order)
    {
        if (subject != null)
        {
            SpriteRenderer sr = subject.GetComponentInChildren<SpriteRenderer>();
            if (sr != null)
            {
                sr.sortingOrder = order;
            }
        }
    }
}