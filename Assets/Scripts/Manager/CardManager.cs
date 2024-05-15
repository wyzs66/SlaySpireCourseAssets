using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

/// <summary>
/// 卡牌管理器：卡牌实例创建和维护一个卡牌列表
/// </summary>
public class CardManager : MonoBehaviour
{
    public GameObject cardPrefab;
    public int size;
    private readonly Stack<GameObject> _instances = new Stack<GameObject>();

    private void Awake()
    {
        Assert.IsNotNull(cardPrefab);
    }

    /// <summary>
    /// 创建卡牌列表
    /// </summary>
    public void Initialize()
    {
        for(int i = 0;  i < size; i++)
        {
            var obj = CreateCard();
            obj.SetActive(false);
            _instances.Push(obj);
        }
        
    }

    /// <summary>
    /// 创建卡牌实例
    /// </summary>
    /// <returns></returns>
    private GameObject CreateCard()
    {
        var cardObject = Instantiate(cardPrefab, transform, true);
        return cardObject;
    }

    public GameObject GetCardObject()
    {
        var obj = _instances.Count > 0 ? _instances.Pop() : CreateCard();
        obj.SetActive(true);
        return obj;
    }

    internal void ReturnObject(GameObject gameObject)
    {
        var pooledObject = gameObject.GetComponent<ManagerPooleObject>();
        Assert.IsNotNull(pooledObject);
        Assert.IsTrue(pooledObject.cardManager == this);

        gameObject.SetActive(false);
         if(!_instances.Contains(gameObject))
        {
            _instances.Push(gameObject);
        }
    }

    public class ManagerPooleObject : MonoBehaviour
    {
        public CardManager cardManager;
    }
}
