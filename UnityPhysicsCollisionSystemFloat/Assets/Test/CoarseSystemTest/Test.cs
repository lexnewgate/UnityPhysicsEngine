using System.Collections;
using System.Collections.Generic;
using CjLib;
using CoarseSystem;
using CollisionSystem;
using UnityEngine;
using BoundingSphere = CollisionSystem.BoundingSphere;
using Collider = CollisionSystem.Collider;

public class Test : MonoBehaviour
{
    // BVHNode<CollisionSystem.BoundingSphere> staticBVH = null;
    // BVHNode<BoundingSphere> dynamicBVH = null;
    GameObject player;
    Collider playerCollider = new Collider { name = "player" };
    BoundingSphere playerBoundingSphere = new BoundingSphere(Vector3.zero, 5);
    void Start()
    {
        GameObject root = new GameObject("GameEntities");

        for (int i = 0; i < 1000; i++)
        {
            var go = GameObject.CreatePrimitive(PrimitiveType.Capsule) as GameObject;
            go.name = i.ToString();
            go.transform.SetParent(root.transform);
            go.transform.position = new Vector3(Random.Range(-100, 100), Random.Range(-100, 100), Random.Range(-100, 100));
            go.AddComponent<DrawMeshInfo>();

            var boundingSphere = new BoundingSphere(go.transform.position, 5);
            var collider = new Collider { name = go.name };

            CoarseSystemManager<BoundingSphere>.Instance.AddEntity(true, collider, boundingSphere);
        }

        player = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        CoarseSystemManager<BoundingSphere>.Instance.AddEntity(false, playerCollider, playerBoundingSphere);
    }




    void OnDrawGizmos()
    {
        foreach (var node in CoarseSystemManager<BoundingSphere>.Instance.staticIterator())
        {
            var volume = node.volume as BoundingSphere;
            var x = volume.center.x;
            var y = volume.center.y;
            var z = volume.center.z;
            GizmosUtil.DrawSphere(volume.center, volume.radius, (int)volume.radius, (int)volume.radius,
                new Color { r = x % 1, g = y % 1, b = z % 1, a = 0.5f },
                GizmosUtil.Style.Wireframe);
        }


        //draw sphere
        if (player != null)
        {
            GizmosUtil.DrawSphere(player.transform.position, Quaternion.identity, 1, 10, 10, Color.blue, GizmosUtil.Style.Wireframe);
        }
    }


    List<PotentialContact> potentialContacts = new List<PotentialContact>();
    // Update is called once per frame
    void Update()
    {

        playerBoundingSphere.center = player.transform.position;
        CoarseSystemManager<BoundingSphere>.Instance.UpdateEntity(false, playerCollider, playerBoundingSphere);

        potentialContacts.Clear();
        // Debug.Log("run update");

        if (CoarseSystemManager<BoundingSphere>.Instance.GetPotentialContacts(ref potentialContacts) > 0)
        {
            foreach (var potentialContact in potentialContacts)
            {
                Debug.Log($"Potential Collision Pair: {potentialContact.colliderPair[0].name} {potentialContact.colliderPair[1].name}");
            }
        }
    }
}
