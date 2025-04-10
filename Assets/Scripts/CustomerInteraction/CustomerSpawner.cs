using System.Collections.Generic;
using UnityEngine;

namespace CustomerInteraction
{
   public class CustomerSpawner : MonoBehaviour
   {
      public GameObject customerPrefab;
      public Transform[] queuePositions;
      public int customersPerLevel = 5;
   
      private Queue<Customer> _customerQueue = new Queue<Customer>();
      private int _spawnedCount = 0;

      void Start()
      {
         SpawnNextCustomer();
      }

      void SpawnNextCustomer()
      {
         if (_spawnedCount >= customersPerLevel) return;
      
         GameObject obj = Instantiate(customerPrefab, queuePositions[^1].position, Quaternion.identity);
         Customer newCustomer = obj.GetComponent<Customer>();

         newCustomer.SetOrder(GetRandomCombo(3));
         _customerQueue.Enqueue(newCustomer);

         UpdateQueuePositions();
         _spawnedCount++;
      }
      
      public Customer GetFrontCustomer()
      {
         if (_customerQueue.Count == 0)
            return null;

         return _customerQueue.Peek();
      }

      public void FulfiillFrontCustomer()
      {
         if (_customerQueue.Count == 0) return;
      
         Customer front = _customerQueue.Dequeue();
         front.FulfillOrder();
         UpdateQueuePositions();
         SpawnNextCustomer();
      }

      void UpdateQueuePositions()
      {
         int i = 0;
         foreach (Customer customer in _customerQueue)
         {
            StartCoroutine(customer.MoveTo(queuePositions[i].position));
            i++;
         }
     
     
      }

      List<Direction> GetRandomCombo(int length)
      {
         var combo = new List<Direction>();
         for (int i = 0; i < length; i++)
         {
            combo.Add((Direction)Random.Range(0, 4));
         }
         return combo;
      }
   }
}
