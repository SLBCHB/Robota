using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

public class SubjectManager : MonoBehaviour
{
    [Header("Gen")]
    [SerializeField] private RobotnikGenerator robotnikGenerator;

    [Header("Positions")]
    public Transform activeSpot;
    public Transform[] queueSpots;

    [Header("Spawning Setup")]
    public GameObject[] subjectPrefabs; // (Pokud je už nepoužíváš a taháš jen z prequeListu, můžeš tohle smazat)

    [Header("Sorting Orders")]
    [Tooltip("Sorting order for the person currently at the desk")]
    public int activeSortingOrder = 0;
    [Tooltip("Sorting order for the people waiting in line")]
    public int queueSortingOrder = 12;

    [Header("Timing")]
    public float delayBeforeMove = 1.0f;
    public float destroyDelay = 3.0f;

    private GameObject _activeSubject;
    public List<GameObject> _queueList = new List<GameObject>();
    public List<GameObject> _prequeList = new List<GameObject>();
    private GameObject _activeCard;

    public void Init()
    {
        // 1. Spawne prvního na přepážku 
        _activeSubject = SpawnSubjectAt(activeSpot);
        if (_activeSubject != null)
        {
            _activeSubject.GetComponent<SubjectEntity>().SetInteractable(true);
            SetSortingOrder(_activeSubject, activeSortingOrder);
            _activeSubject.GetComponent<Robotnik>().dir = RobotnikDirection.Front;
            _activeSubject.GetComponent<Robotnik>().setVizual();
            _activeSubject.GetComponent<SubjectEntity>().canBeProcessed = true;
        }

        // 2. Naplní zbytek fronty
        for (int i = 0; i < queueSpots.Length; i++)
        {
            GameObject qPerson = SpawnSubjectAt(queueSpots[i]);

            if (qPerson != null)
            {
                qPerson.GetComponent<SubjectEntity>().SetInteractable(false);
                SetSortingOrder(qPerson, queueSortingOrder);
                _queueList.Add(qPerson);
            }
        }
    }

    public void HandleSubjectProcessedPassed(SubjectEntity processedSubject)
    {
        Debug.Log("TADY + DATA", processedSubject);
        RobotMan.Instance.HireEmployee(processedSubject.gameObject.GetComponent<Robotnik>());
        if (processedSubject != null)
        {
            Debug.Log("funggujeto" + processedSubject.gameObject.name);
            RobotMan.Instance.HireEmployee(processedSubject.gameObject.GetComponent<Robotnik>());
            
        }

        if (_activeCard != null)
        {
            Destroy(_activeCard);
        }

        StartCoroutine(AdvanceQueueRoutine());
    }

    public void HandleSubjectProcessedReject(SubjectEntity processedSubject)
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
                _activeSubject.GetComponent<SubjectEntity>().MoveToNewSpot(activeSpot);
                _activeSubject.GetComponent<SubjectEntity>().isActiveInQueue = true;
                _activeSubject.GetComponent<SpriteRenderer>().sortingLayerName = "Default";
                SetSortingOrder(_activeSubject, activeSortingOrder);
                _activeSubject.GetComponent<SubjectEntity>().SetInteractable(true);
                _activeSubject.GetComponent<Robotnik>().dir = RobotnikDirection.Front;
                _activeSubject.GetComponent<Robotnik>().setVizual();
                _activeSubject.GetComponent<SubjectEntity>().canBeProcessed = true;

                StartCoroutine(WaitAndTossCard(_activeSubject.GetComponent<SubjectEntity>()));
            }

            for (int i = 0; i < _queueList.Count; i++)
            {
                if (_queueList[i] != null && queueSpots[i] != null)
                {
                    _queueList[i].GetComponent<SubjectEntity>().MoveToNewSpot(queueSpots[i]);
                }
            }

            if (queueSpots.Length > 0 && _prequeList.Count > 0)
            {
                Transform lastSpot = queueSpots[queueSpots.Length - 1];
                GameObject qPerson = SpawnSubjectAt(lastSpot);

                if (qPerson != null)
                {
                    qPerson.GetComponent<SubjectEntity>().SetInteractable(false);
                    SetSortingOrder(qPerson, queueSortingOrder);
                    qPerson.GetComponent<SubjectEntity>().MoveToNewSpot(lastSpot);
                    _queueList.Add(qPerson);
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

    private GameObject SpawnSubjectAt(Transform spot)
    {
        if (_prequeList == null || _prequeList.Count == 0 || spot == null)
        {
            Debug.LogWarning("[SubjectManager] Došli dělníci v _prequeList nebo chybí spot!");
            return null;
        }

        GameObject obj = _prequeList[0];
        _prequeList.RemoveAt(0);

        obj.SetActive(true);

        SubjectEntity entity = obj.GetComponent<SubjectEntity>();
        if (entity != null)
        {
            entity.basePosition = spot;
            obj.transform.position = spot.position;
        }

        return obj;
    }

    private void SetSortingOrder(GameObject subject, int order)
    {
        if (subject != null)
        {
            SpriteRenderer sr = subject.GetComponentInChildren<SpriteRenderer>();
            if (sr != null)
            {
                sr.sortingOrder = order;
            }

            foreach (var srr in sr.GetComponentsInChildren<SpriteRenderer>())
            {
                srr.sortingOrder = order;
            }
        }
    }
}