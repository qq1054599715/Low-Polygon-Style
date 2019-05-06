using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamelogic.Extensions.Internal.KDTree;

public class PossionStyle {

    public PossionStyle()
    {
        
    }
    
    public Vector2 Remap(Vector2 fromX, Vector2 fromY, Vector2 toX, Vector2 toY, Vector2 p)
    {
        Vector2 a = (p - fromX);
        Vector2 b = (fromY - fromX);
        Vector2 c = (toY - toX);
        Vector2 d = new Vector2(a.x / b.x, a.y / b.y);
        Vector2 e = new Vector2(d.x * c.x, d.y * c.y);
        return e + toX;
    }
    
    public Texture2D makePossionStyle(Texture2D src, Vector2 minCellSize, Vector2 maxCellSize, float progress)
    {
        Texture2D dst = new Texture2D(src.width, src.height);
        int srcHeight = src.height;
        int srcWidth = src.width;
        Vector2 baseCellSize = new Vector2(srcWidth,srcHeight);
        Vector2 currentCellSize = Remap(new Vector2(0, 0), new Vector2(1, 1), 
            maxCellSize, minCellSize,
            new Vector2(progress, progress));

        float minDst = currentCellSize.x > currentCellSize.y ? currentCellSize.x : currentCellSize.y;
        PossionDisc possionDisc = new PossionDisc(baseCellSize.x,baseCellSize.y,minDst,30);
        Vector2 PossionSize = possionDisc.Begin(0,0);
       // List<Vector2> finalPoint = new List<Vector2>();
        KDTree<Vector2> kdTree = new KDTree<Vector2>(2);
        IDistanceFunction distanceFunction = new ManhattanDistanceFunction();
        do
        {
            bool isOver = false;
            bool hasSpwan = false;
            Vector3 spwanPoint = new Vector3(0,0,0);
            int x = 0;
            int y = 0;
            possionDisc.Next(ref isOver, ref hasSpwan, ref spwanPoint);
            if (hasSpwan == true)
            {
          //      finalPoint.Add(spwanPoint);
                kdTree.AddPoint(spwanPoint,spwanPoint);
            }
            if (isOver == true)
           {
               break;
           }
        } while (true);
        
        for (int i = 0; i < srcHeight; i++)
        {
            for (int j = 0; j < srcWidth; j++)
            {
                Vector2 uv = new Vector2(j ,i);
                int idI = 0;
                int idJ = 0;
                float minDistance = float.MaxValue;

                var neighbors = kdTree.NearestNeighbors(uv, distanceFunction, 16, minDst*2);

                while (neighbors.MoveNext()==true)
                {
                    Vector2 currentPosition = neighbors.Current;
                    int idX = Mathf.FloorToInt(currentPosition.x);
                    int idY = Mathf.FloorToInt(currentPosition.y);
                    float distance = Vector3.Distance(currentPosition,uv);
                    if(distance<minDistance){
                        minDistance = distance;
                        idI = idX;
                        idJ = idY;
                    }
                }

                Color currentColor = src.GetPixel(idI,idJ);
                dst.SetPixel(j, i, currentColor);
                
            }
        }
        dst.Apply();
        return dst;
    }
    
}
