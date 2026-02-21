using NavMeshPlus.Components;
using UnityEngine;

public class BakeAtRuntime : MonoBehaviour 
{
    public NavMeshSurface Surface2D;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {   
        Surface2D = GetComponent<NavMeshSurface>();
        Surface2D.BuildNavMeshAsync();
    }
}
