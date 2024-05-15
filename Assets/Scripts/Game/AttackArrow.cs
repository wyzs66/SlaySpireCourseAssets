using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �����ࣺ��������ָ���ͷ��ͨ�����ױ��������߿��Ƽ�ͷ����������
/// </summary>
public class AttackArrow : MonoBehaviour
{
    [SerializeField] private GameObject bodyPrefab; //����Ԥ����
    [SerializeField] private GameObject headPrefab; //��ͷԤ����

    //ѡ����˰�Χ��Ԥ����
    [SerializeField] private GameObject topLeftFrame;
    [SerializeField] private GameObject topRightFrame;
    [SerializeField] private GameObject bottomLeftFrame;
    [SerializeField] private GameObject bottomRightFrame;

    //ѡ����˰�Χ������
    private GameObject topLeftPoint;
    private GameObject topRightPoint;
    private GameObject bottomLeftPoint;
    private GameObject bottomRightPoint;

    [SerializeField] private LayerMask enemyLayer; //���˲�
    private GameObject selectedEnemy; //��ѡ�е���

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
    /// �ж��Ƿ���ʾ��ͷ
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

        //�ж�����Ƿ�ѡ�����
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

        //���㱴�������ߵ��������Ƶ�
        var controlAx = centerX - (mouseX - centerX) * 0.3f;
        var controlAy = centerY + (mouseY - centerY) * 0.8f;
        var controlBx = centerX + (mouseX - centerX) * 0.1f;
        var controlBy = centerY + (mouseY - centerY) * 1.4f;

        for( int i = 0;i < parts.Count; i++)
        {

            var part = parts[i];

            //��������
            //���ױ��������ߣ�
            //���Ϊѡ�񹥻��ƺ�λ�Ƶص������λ�ã�P0(centerX,centerY)
            //�յ�Ϊ�������Ļ�е�λ�ã�P3(mouseX, mouseY)
            //�������Ƶ�ͨ�����㶯̬��ȡ P1(controlAx, controlAy) P2(controlBx, controlBy)
            //���������߹�ʽ��B(t) = P0 * (1-t)^3 + 3 * P1 * t * (1-t)^2 + 3 * P2 * t^2 * (1-t) + P3 * t^3 , 0 < t < 1

            //��ͬ�����������ֵ���õ���ͬ��Tֵ
            //Խ������β��tԽС
            //Խ������ͷ��tԽ��
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

            //����ÿ����ͷ�ĳ���ͨ����������һ����ͷ����λ��֮����������õ���ת�Ƕ�
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

            // ����ÿ����ͷ�Ĵ�С�����С���յ��
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

        //ͨ��BoxCollider2D�Ĵ�Сsize��ƫ����offset �����Χ���ĸ�����λ��
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
