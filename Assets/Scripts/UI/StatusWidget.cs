using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusWidget : MonoBehaviour
{
    [SerializeField] private GameObject statusElementPrefab;
    private List<StatusElementWidget> elements = new List<StatusElementWidget>();

    /// <summary>
    /// 角色身上BUFF状态的改变，没有时创建BUFF，结束时销毁BUFF，存在时改变BUFF层数
    /// </summary>
    /// <param name="status">需要变更的BUFF</param>
    /// <param name="value">变更的数值</param>
    public void OnStatusChanged(StatusTempelate status, int value)
    {
        var foundElement = false;
        foreach (var element in elements)
        {
            if(element.Type == status.Name)
            {
                if(value > 0)
                {
                    element.UpdateModefier(value);
                    foundElement = true;
                    break;
                }

                elements.Remove(element);
                element.FadeAndDestroy();
                foundElement = true;
                break;
            }
        }

        if(!foundElement)
        {
            var newElement = Instantiate(statusElementPrefab, transform, false);
            var widget = newElement.GetComponent<StatusElementWidget>();
            widget.Initialize(status, value);
            widget.Show();
            elements.Add(widget);
        }
    }

    public void OnHpChange(int value)
    {
        if (value <= 0)
        {
            foreach (var element in elements)
            {
                element.FadeAndDestroy();
            }
        }
    }
}
