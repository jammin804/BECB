using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CustomerInteraction
{
    // public enum Direction
    // {
    //     Up,
    //     Down,
    //     Left,
    //     Right
    // }

    public class Customer : MonoBehaviour
    {
        public List<Direction> orderCombo = new List<Direction>();
        public float moveSpeed = 2.0f;
    
        //Order
        public void SetOrder(List<Direction> newOrder)
        {
            orderCombo = newOrder;
        }
    
        //Leave or destroy Give Money or Score
        public void FulfillOrder()
        {
            Debug.Log("Order fulfilled! Customer leaving.");
            Destroy(gameObject);
        }

        //Move
        public IEnumerator MoveTo(Vector3 targetPos)
        {
            while (Vector3.Distance(transform.position, targetPos) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
                yield return null;
            }
        }
    }
}