using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 攻击类：攻击卡牌指向箭头，通过三阶贝塞尔曲线控制箭头的曲线弯曲
/// </summary>
public class AttackArrow : MonoBehaviour
{
    [SerializeField] private GameObject bodyPrefab; //箭身预制体
    [SerializeField] private GameObject headPrefab; //箭头预制体

    //选择敌人包围框预制体
    [SerializeField] private GameObject topLeftFrame;
    [SerializeField] private GameObject topRightFrame;
    [SerializeField] private GameObject bottomLeftFrame;
    [SerializeField] private GameObject bottomRightFrame;

    //选择敌人包围框区域
    private GameObject topLeftPoint;
    private GameObject topRightPoint;
    private GameObject bottomLeftPoint;
    private GameObject bottomRightPoint;

    [SerializeField] private LayerMask enemyLayer; //敌人层
    private GameObject selectedEnemy; //被选中敌人

    private Camera mainCamera;

    private const int AttackArrowPartsNumber = 17;
    private readonly List<GameObject> parts = new(AttackArrowPartsNumber);

    private bool isArrowEnable;


    private void Start()
    {
        mainCamera = Camera.main;
        for(int i = 0; i < AttackArrowPartsNumber - 1; i++)
        {
            var body = Instantiate(bodyPrefab, transform);
            parts.Add(body);
        }
        var head = Instantiate(headPrefab, transform);
        parts.Add(head);

        foreach(var part in parts)
        {
            part.SetActive(false);
        }
        
        topLeftPoint = Instantiate(topLeftFrame, transform);
        topRightPoint = Instantiate(topRightFrame, transform);
        bottomLeftPoint = Instantiate(bottomLeftFrame, transform);
        bottomRightPoint = Instantiate(bottomRightFrame, transform);
    }

    /// <summary>
    /// 判断是否显示箭头
    /// </summary>
    /// <param name="enableArrow"></param>
    public void ArrowEnable(bool enableArrow)
    {
        isArrowEnable = enableArrow;
        foreach(var part in parts)
        {
            part.SetActive(enableArrow);
        }
        if(!isArrowEnable)
        {
            UnEnableSelectedBox();
        }
    }

    private void LateUpdate()
    {
        if(!isArrowEnable)
            return;
        
        var mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        var mouseX = mousePosition.x;
        var mouseY = mousePosition.y;

        //判断鼠标是否选择敌人
        var hitInfo = Physics2D.Raycast(mousePosition, Vector3.forward, Mathf.Infinity, enemyLayer);
        if(hitInfo.collider != null)
        {
            if(hitInfo.collider.gameObject != selectedEnemy || selectedEnemy == null)
            {
                SelectedEnemy(hitInfo.collider.gameObject);
            }
        }
        else
        {
            UnEnableSelectedBox();
            selectedEnemy = null;
        }

        const float centerX = -15.0f;
        const float centerY = -1.0f;

        //计算贝塞尔曲线的两个控制点
        var controlAx = centerX - (mouseX - centerX) * 0.3f;
        var controlAy = centerY + (mouseY - centerY) * 0.8f;
        var controlBx = centerX + (mouseX - centerX) * 0.1f;
        var controlBy = centerY + (mouseY - centerY) * 1.4f;

        for( int i = 0;i < parts.Count; i++)
        {

            var part = parts[i];

            //计算曲线
            //三阶贝塞尔曲线，
            //起点为选择攻击牌后位移地点的中心位置，P0(centerX,centerY)
            //终点为鼠标在屏幕中的位置，P3(mouseX, mouseY)
            //两个控制点通过计算动态获取 P1(controlAx, controlAy) P2(controlBx, controlBy)
            //贝塞尔曲线公式：B(t) = P0 * (1-t)^3 + 3 * P1 * t * (1-t)^2 + 3 * P2 * t^2 * (1-t) + P3 * t^3 , 0 < t < 1

            //不同箭身根据索引值，得到不同的T值
            //越靠近箭尾，t越小
            //越靠近箭头，t越大
            var t = (i + 1) * 1.0f / parts.Count;
            var tt = t * t;
            var ttt = tt * t;

            var u = 1.0f - t;
            var uu = u * u;
            var uuu = uu * u;

            var arrowX = uuu * centerX 
                + 3 * t * uu * controlAx 
                + 3 * tt * u * controlBx 
                + mouseX * ttt;

            var arrowY = uuu * centerY 
                + 3 * t * uu * controlAy 
                + 3 * tt * u * controlBy 
                + mouseY * ttt;

            parts[i].transform.position = new Vector3(arrowX, arrowY, 0.0f);

            //计算每个箭头的朝向，通过计算与下一个箭头两个位置之间的向量，得到旋转角度
            float directionX;
            float directionY;

            if(i > 0)
            {
                directionX = parts[i].transform.position.x - parts[i - 1].transform.position.x;
                directionY = parts[i].transform.position.y - parts[i - 1].transform.position.y;
            }
            else
            {
                directionX = parts[i + 1].transform.position.x - parts[i].transform.position.x;
                directionY = parts[i + 1].transform.position.y - parts[i].transform.position.y;
            }
            part.transform.rotation = Quaternion.Euler(0, 0 , -Mathf.Atan2(directionX, directionY) * Mathf.Rad2Deg);

            // 设置每个箭头的大小，起点小，终点大
            part.transform.localScale = new Vector3(
                1.0f - 0.03f * (parts.Count - 1 - i),
                1.0f - 0.03f * (parts.Count - 1 - i),
                0); 

        }
    }

    private void SelectedEnemy(GameObject gameObject)
    {
        selectedEnemy = gameObject;

        var enemyBox = selectedEnemy.GetComponent<BoxCollider2D>();
        var size = enemyBox.size;
        var offset = enemyBox.offset;

        //通过BoxCollider2D的大小size和偏移量offset 计算包围框四个顶点位置
        var topLeftLocal = offset + new Vector2(-size.x * 0.5f, size.y * 0.5f);
        var topLeftWorld = gameObject.transform.TransformPoint(topLeftLocal);

        var topRightLocal = offset + new Vector2(size.x * 0.5f, size.y * 0.5f);
        var topRightWorld = gameObject.transform.TransformPoint(topRightLocal);

        var bottomLeftLocal = offset + new Vector2(-size.x * 0.5f, -size.y * 0.5f);
        var bottomLeftWorld = gameObject.transform.TransformPoint(bottomLeftLocal);

        var bottomRightLocal = offset + new Vector2(size.x * 0.5f, -size.y * 0.5f);
        var bottomRightWorld = gameObject.transform.TransformPoint(bottomRightLocal);

        topLeftPoint.transform.position = topLeftWorld;
        topRightPoint.transform.position = topRightWorld;
        bottomLeftPoint.transform.position = bottomLeftWorld;
        bottomRightPoint.transform.position = bottomRightWorld;

        EnableSelectedBox();
    }

    private void EnableSelectedBox()
    {
        topLeftPoint.SetActive(true);
        topRightPoint.SetActive(true);
        bottomLeftPoint.SetActive(true);
        bottomRightPoint.SetActive(true);

        foreach(var part in parts)
        {
           part.GetComponent<SpriteRenderer>().color = Color.red;
            
        }
    }

    private void UnEnableSelectedBox()
    {
        topLeftPoint.SetActive(false);
        topRightPoint.SetActive(false);
        bottomLeftPoint.SetActive(false);
        bottomRightPoint.SetActive(false);
        foreach (var part in parts)
        {
            part.GetComponent<SpriteRenderer>().color = Color.white;

        }
    }
}
