using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class CollectableCount : MonoBehaviour
{
   TMPro.TMP_Text text;
   int count;
   public GameObject escapeObject;

   void Awake()
   {
      text = GetComponent<TMPro.TMP_Text>();
      UpdateCount();
   }

   void start() => UpdateCount();
   void OnEnable() => Collectable.OnCollected += OnCollectableCollected;
   void OnDisable() => Collectable.OnCollected -= OnCollectableCollected;
   void OnCollectableCollected()
   {
      count++;
      UpdateCount();
      
      if (count == Collectable.total)
      {
         EscapeActive();
      }
   }
   void EscapeActive()
   {
      escapeObject.SetActive(true);
      text.text = "Escape!";
   }

   public void OnEscape()
   {
      text.text = "You Win!";
   }
   void UpdateCount()
   {
      text.text = $"{count} / {Collectable.total}";}
   }



