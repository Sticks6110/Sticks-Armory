using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crater
{
    
    public static void Create(Vector3 pos, float size, Transform craterObject, float craterDepthMutliplier = 0.5f, float craterDepth = 2)
    {

        MeshFilter meshFilter = craterObject.GetComponent<MeshFilter>();

        Vector3[] verts = meshFilter.mesh.vertices;

        for (int i = 0; i < verts.Length; i++)
        {

            Vector3 p = craterObject.TransformPoint(verts[i]);

            float dist = Vector3.Distance(p, pos);

            if (dist <= size)
            {
                verts[i] = new Vector3(verts[i].x, verts[i].y - Mathf.Min((craterDepthMutliplier * Mathf.Pow(size - dist, 2)), craterDepth), verts[i].z); //Exponential
                //verts[i] = new Vector3(verts[i].x, verts[i].y - (size - dist), verts[i].z); //Linear
            }

        }

        meshFilter.mesh.vertices = verts;
        meshFilter.mesh.RecalculateNormals();

    }

}
