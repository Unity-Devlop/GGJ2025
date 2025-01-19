using System;
using UnityEngine;

namespace WitchFish
{
    public class Cloud : MonoBehaviour
    {
        public float moveSpeed;

        [SerializeField] private Transform spawnPos;
        [SerializeField] private Transform destroyPos;
        

        private void Update()
        {
            transform.position += new Vector3(1, 0, 0) * (moveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, destroyPos.position) < 0.1f)
            {
                transform.position = spawnPos.position;
            }
        }
    }
}