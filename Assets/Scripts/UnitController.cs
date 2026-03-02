using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitController : MonoBehaviour
{
    [SerializeField] float movementSpeed = 1f;

    [SerializeField] Transform unit;

    List<Node> path = new List<Node>();

    GridManager gridManager;
    Pathfinding pathFinder;
    
    Animator animator;

    void Start()
    {
        gridManager = FindObjectOfType<GridManager>();
        pathFinder = FindObjectOfType<Pathfinding>();
        
        Vector2Int startCoords = gridManager.GetCoordinatesFromPosition(unit.position);
        unit.position = gridManager.GetPositionFromCoordinates(startCoords);
        
        animator = unit.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            bool hasHit = Physics.Raycast(ray, out hit);



            if (hasHit)
            {
                if (hit.transform.tag == "Tile")
                {
                    
                        Vector2Int targetCords = hit.transform.GetComponent<Tile>().cords;
                        Vector2Int startCords = gridManager.GetCoordinatesFromPosition(unit.position);

                        if (IsAdjacent(startCords, targetCords)) //written by chatgpt
                        {
                            pathFinder.SetNewDestination(startCords, targetCords);
                            RecalculatePath(true);
                        }
                        else
                        {
                            Debug.Log("Tile not adjacent.");
                        }
                }
            }
        }
    }

    void RecalculatePath(bool resetPath)
    {
        Vector2Int coordinates = new Vector2Int();
        if (resetPath)
        {
            coordinates = pathFinder.StartCords;
        }
        else
        {
            coordinates = gridManager.GetCoordinatesFromPosition(transform.position);
        }

        StopAllCoroutines();
        path.Clear();
        path = pathFinder.GetNewPath(coordinates);
        StartCoroutine(FollowPath());
    }

    IEnumerator FollowPath()
    {
        animator.SetBool("isMoving", true);
        
        for (int i = 1; i < path.Count; i++)
        {
            Vector3 startPosition = unit.position;
            Vector3 endPosition = gridManager.GetPositionFromCoordinates(path[i].cords);
            float travelPercent = 0f;

            unit.LookAt(endPosition);

            while (travelPercent < 1f)
            {
                travelPercent += Time.deltaTime * movementSpeed;
                unit.position = Vector3.Lerp(startPosition, endPosition, travelPercent);
                yield return new WaitForEndOfFrame();
            }
        }
        animator.SetBool("isMoving", false);
    }

    //written by chatgpt
    bool IsAdjacent(Vector2Int a, Vector2Int b)
    {
        int xDiff = Mathf.Abs(a.x - b.x);
        int yDiff = Mathf.Abs(a.y - b.y);

        // Only allow direct neighbors (no diagonals)
        return (xDiff == 1 && yDiff == 0) || (xDiff == 0 && yDiff == 1);
    }
}