using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PoissonDiscSamplingService {

    //TODO:
    //Variant point radii

    public static List<Vector2> GeneratePoints(float pointRadius, float minDistanceFromCenter,
        float maxDistanceFromCenter, int numSamplesBeforeRejection ) {

        float sampleRegionSize = maxDistanceFromCenter * 2 + 1;
        float cellSize = pointRadius / Mathf.Sqrt(2);

        int[,] grid = new int[Mathf.CeilToInt(sampleRegionSize / cellSize),
            Mathf.CeilToInt(sampleRegionSize / cellSize)];
        List<Vector2> points = new List<Vector2>();
        List<Vector2> spawnPoints = new List<Vector2>();
        float sqrMinDistanceFromCenter = minDistanceFromCenter * minDistanceFromCenter;
        float sqrMaxDistanceFromCenter = maxDistanceFromCenter * maxDistanceFromCenter;
        Vector2 areaCenter = Vector2.one * sampleRegionSize / 2;

        spawnPoints.Add(areaCenter + Vector2.up * minDistanceFromCenter);
        while(spawnPoints.Count > 0) {
            int spawnIndex = Random.Range(0, spawnPoints.Count);
            Vector2 spawnCenter = spawnPoints[spawnIndex];
            bool candidateAccepted = false;

            for(int i = 0; i < numSamplesBeforeRejection; i++) {
                float angle = Random.value * Mathf.PI * 2;
                Vector2 dir = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle));
                Vector2 candidate = spawnCenter + dir * Random.Range(pointRadius, 2 * pointRadius);

                if(IsValid(candidate, sampleRegionSize, areaCenter, sqrMinDistanceFromCenter,
                    sqrMaxDistanceFromCenter, cellSize, pointRadius, points, grid)) {
                    spawnPoints.Add(candidate);

                    points.Add(candidate);
                    grid[(int)(candidate.x / cellSize), (int)(candidate.y / cellSize)]
                        = points.Count;
                    candidateAccepted = true;
                    break;
                }
            }

            if(!candidateAccepted) {
                spawnPoints.RemoveAt(spawnIndex);
            }
        }

        return points;
    }

    private static bool IsValid(Vector2 candidate, float sampleRegionRadius, Vector2 areaCenter,
        float sqrMinDistanceFromCenter, float sqrMaxDistanceFromCenter, float cellSize,
        float pointRadius, List<Vector2> points, int[,] grid) {

        if(candidate.x >= 0 && candidate.x < sampleRegionRadius
            && candidate.y >= 0 && candidate.y < sampleRegionRadius) {

            float sqrDstToCenter = (candidate - areaCenter).sqrMagnitude;
            if(sqrDstToCenter < sqrMinDistanceFromCenter
                || sqrDstToCenter > sqrMaxDistanceFromCenter) {
                return false;
            }

            int cellX = (int)(candidate.x / cellSize);
            int cellY = (int)(candidate.y / cellSize);

            int searchStartX = Mathf.Max(0, cellX - 2);
            int searchEndX = Mathf.Min(cellX + 2, grid.GetLength(0) - 1);
            int searchStartY = Mathf.Max(0, cellY - 2);
            int searchEndY = Mathf.Min(cellY + 2, grid.GetLength(1) - 1);

            for(int x = searchStartX; x <= searchEndX; x++) {
                for(int y = 0; y <= searchEndY; y++) {
                    int pointIndex = grid[x, y] - 1;
                    if(pointIndex != -1) {
                        float sqrDstToOtherPoint = (candidate - points[pointIndex]).sqrMagnitude;
                        if(sqrDstToOtherPoint < pointRadius * pointRadius) {
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        return false;
    }
}
