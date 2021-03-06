﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LineSpawner : FormationSpawner
{
    enum Orientation
    {
        Vertical,
        Horizontal
    }

    enum StartPoint
    {
        PositiveEnd,
        Center,
        NegativeEnd
    }

    [SerializeField, Tooltip("Orientation of the line")]
    Orientation orient;

    [SerializeField, Tooltip("What location the origin represents in this line")]
    StartPoint start;

    protected override void GenerateSpawnPoints()
    {
        switch(start)
        {
            case StartPoint.Center:
                GenerateFromCenter();
                break;
            case StartPoint.PositiveEnd:
            case StartPoint.NegativeEnd:
            default:
                GenerateFromEnd();
                break;
        }
    }

    void GenerateFromCenter()
    {
        if (start == StartPoint.PositiveEnd || start == StartPoint.NegativeEnd)
        {
            Debug.LogError("What the fuck are we doing in here");
            return;
        }

        List<Vector3> newPoints = new List<Vector3>();

        float startOffset = FindMidpointOffset();
        float nextOffset = startOffset;
        float distanceBetween = GetDistanceBetweenPoints();

        // Add the odd one if necessary
        if (startOffset == 0)
        {
            newPoints.Add(GenerateOffsetVector(startOffset));
            nextOffset = (Mathf.Abs(nextOffset) + distanceBetween);
        }
        
        while(newPoints.Count < numEnemies)
        {
            newPoints.Add(GenerateOffsetVector(nextOffset));
            newPoints.Add(GenerateOffsetVector(-nextOffset));
            nextOffset = (Mathf.Abs(nextOffset) + distanceBetween);
        }

        spawnPoints = newPoints;
    }

    void GenerateFromEnd()
    {
        if(start == StartPoint.Center)
        {
            Debug.LogError("What the fuck are we doing in here");
            return;
        }

        List<Vector3> newPoints = new List<Vector3>();

        float distanceBetween = GetDistanceBetweenPoints();
        float direction = (start == StartPoint.NegativeEnd) ? 1 : -1;
        distanceBetween *= direction;

        float currentOffset = 0;
        while(newPoints.Count < numEnemies)
        {
            newPoints.Add(GenerateOffsetVector(currentOffset));
            currentOffset += distanceBetween;
        }

        spawnPoints = newPoints;
    }

    float GetDistanceBetweenPoints()
    {
        return formationSize / numEnemies;
    }

    float FindMidpointOffset()
    {
        if(numEnemies%2 != 0)
        {
            // Odd formation start dead center
            return 0f;
        }
        else
        {
            return GetDistanceBetweenPoints() / 2;
        }
    }

    Vector3 GenerateOffsetVector(float offset)
    {
        Vector3 thisPos = this.transform.position;
        switch (orient)
        {
            case Orientation.Horizontal:
                return new Vector3(thisPos.x + offset, thisPos.y);
            case Orientation.Vertical:
            default:
                return new Vector3(thisPos.x, thisPos.y + offset);
        }
    }

    protected override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();

        Gizmos.color = Color.magenta;
        Vector3 from, to;
        float offset;
        Vector3 thisPos = this.transform.position;

        if(start == StartPoint.Center)
        {
            offset = formationSize / 2f;
            if(orient == Orientation.Horizontal)
            {
                from = new Vector3(thisPos.x - offset, thisPos.y);
                to = new Vector3(thisPos.x + offset, thisPos.y);
            }
            else
            {
                from = new Vector3(thisPos.x, thisPos.y - offset);
                to = new Vector3(thisPos.x, thisPos.y + offset);
            }
        }
        else
        {
            from = thisPos;
            offset = (start == StartPoint.NegativeEnd) ? formationSize : -formationSize;

            if (orient == Orientation.Horizontal)
            {
                to = new Vector3(thisPos.x + offset, thisPos.y);
            }
            else
            {
                to = new Vector3(thisPos.x, thisPos.y + offset);
            }
        }

        Gizmos.DrawLine(from, to);
    }
}
