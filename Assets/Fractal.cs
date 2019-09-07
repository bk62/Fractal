using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fractal : MonoBehaviour {
    public Mesh mesh;
    public Material material;

    public int maxDepth;

    private int depth;

    public float childScale;

    public bool randomDelay = true;
    public float delayMin = 0.1f;
    public float delayMax = 0.5f; //use delayMax if randomDelay = false

    private static Vector3[] childDirections = {
        Vector3.up,
        Vector3.right,
        Vector3.left,
        Vector3.forward,
        Vector3.back
    };

    // rotate children so their local up points away from the parent
    private static Quaternion[] childOrientations = {
        Quaternion.identity,
        Quaternion.Euler (0f, 0f, -90f),
        Quaternion.Euler (0f, 0f, 90f),
        Quaternion.Euler(90f, 0f, 0f),
        Quaternion.Euler(-90f, 0f, 0f)
    };

    void Start () {
        gameObject.AddComponent<MeshFilter> ().mesh = mesh;
        MeshRenderer mr = gameObject.AddComponent<MeshRenderer> ();
        mr.material = material;
        mr.material.color = Color.Lerp(Color.white, Color.yellow, (float) depth/maxDepth);
        if (depth < maxDepth) {
            StartCoroutine (CreateChildren ());
        }
    }

    private IEnumerator CreateChildren () {

        for (int i = 0; i < childDirections.Length; i++) {
            float delay = randomDelay ? Random.Range(delayMin, delayMax) : delayMax;
            yield return new WaitForSeconds (delay);
            new GameObject ("Fractal Child").AddComponent<Fractal> ()
                .Initialize (this, i);
        }
    }

    private void Initialize (Fractal parent, int childIndex) {
        Vector3 direction = childDirections[childIndex];
        Quaternion orientation = childOrientations[childIndex];

        mesh = parent.mesh;
        material = parent.material;
        maxDepth = parent.maxDepth;
        depth = parent.depth + 1;
        childScale = parent.childScale;
        transform.parent = parent.transform;
        transform.localScale = Vector3.one * childScale;
        // half the parent's length plus hald the child's so they touch
        transform.localPosition = direction * (0.5f + 0.5f * childScale);
        transform.localRotation = orientation;
    }
}