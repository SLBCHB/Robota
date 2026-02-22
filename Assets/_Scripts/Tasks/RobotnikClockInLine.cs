using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotnikClockInLine : MonoBehaviour
{
    [SerializeField] private float spaceBetween;
    [SerializeField] private float speed;

    [SerializeField] private Transform lineStop;
    [SerializeField] private Transform lineEnd;
    [SerializeField] private Transform declinePos;

    [SerializeField] private RobotnikInfoDisplayer infoDisplayer;

    public bool onStop = false;
    public int currentOnStop = -1;

    private List<GameObject> robotnici;

    void Start()
    {
        robotnici = RobotnikManager.Instance.robotnici;
        infoDisplayer.displayDateTime(DifficultyManager.Instance.getCurrentDifficulty().currentDate, DifficultyManager.Instance.getCurrentDifficulty().currentTime);
        NextInLine();
    }

    public void MoveOn(int index, Vector3 targetPosition, float moveSpeed)
    {
        if (index < 0 || index >= robotnici.Count)
        {
            Debug.LogWarning($"[ClockInLine] Dělník s indexem {index} neexistuje ve frontě!");
            return;
        }

        GameObject robotnik = robotnici[index];

        StartCoroutine(MoveRoutine(robotnik.transform, targetPosition, moveSpeed));
    }

    private IEnumerator MoveRoutine(Transform targetTransform, Vector3 targetPosition, float maxSpeed)
    {
        float currentSpeed = 0f; 

        float acceleration = maxSpeed * 6f;
        float decelerationDistance = 0.5f; 

        while (Vector3.Distance(targetTransform.position, targetPosition) > 0.01f)
        {
            float distanceToTarget = Vector3.Distance(targetTransform.position, targetPosition);

            if (distanceToTarget <= decelerationDistance)
            {
                float slowdownFactor = distanceToTarget / decelerationDistance;

                currentSpeed = Mathf.Lerp(0.5f, maxSpeed, slowdownFactor);
            }
            else
            {
                currentSpeed = Mathf.MoveTowards(currentSpeed, maxSpeed, acceleration * Time.deltaTime);
            }

            targetTransform.position = Vector3.MoveTowards(targetTransform.position, targetPosition, currentSpeed * Time.deltaTime);

            yield return null;
        }

        targetTransform.position = targetPosition;
    }

    public void NextInLine()
    {
        if (RoomController.Instance.getDeskCount() <= RoomController.Instance.getSittingCount())
            return;

        if(!onStop)
            StartCoroutine(AcceptedMoveRoutine());
    }

    public void DeclineInLine()
    {
        StartCoroutine(DeclineMoveRoutine());
    }

    private IEnumerator DeclineMoveRoutine()
    {
        onStop = true;
        MoveOn(currentOnStop, new Vector3(declinePos.position.x, declinePos.position.y), speed);
        currentOnStop++;

        StartCoroutine(NextInLineRoutine());
        yield return null;
    }

    private IEnumerator AcceptedMoveRoutine()
    {
        onStop = true;
        if (currentOnStop < robotnici.Count && currentOnStop >= 0)
        {
            RoomController.Instance.sitNextRobotnik(robotnici[currentOnStop]);
        }
        currentOnStop++;

        StartCoroutine(NextInLineRoutine());
        yield return null;
    }

    private IEnumerator NextInLineRoutine()
    {
        
        for (int i = currentOnStop; i < robotnici.Count; i++)
        {
            int positionInQueue = i - currentOnStop;

            Vector3 targetPos = lineStop.position + (Vector3.left * (positionInQueue * spaceBetween));

            MoveOn(i, targetPos, speed);

            yield return new WaitForSeconds(0.2f);
            if (i == currentOnStop)
                infoDisplayer.displayInfo(robotnici[currentOnStop].GetComponent<Robotnik>());
        }

        onStop = false;
       
    }
}