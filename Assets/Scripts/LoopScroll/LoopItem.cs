using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopItem : MonoBehaviour
{
    #region �ֶ�
    public RectTransform rect;
    public RectTransform viewRect;
    public Vector3[] rectCorners;
    public Vector3[] viewCorners;
    public bool isf;

    #endregion

    #region ί�С��¼�
    public Action OnAddHead;
    public Action OnRemoveHead;
    public Action OnAddLast;
    public Action OnRemoveLast;

    #endregion

    #region �ص�����
    void Awake()
    {
        viewRect = GameObject.Find("Canvas/Scroll View").GetComponent<RectTransform>();
        rectCorners = new Vector3[4];
        viewCorners = new Vector3[4];
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        ListenerCorners();
    }
    #endregion

    #region ����
    void ListenerCorners()
    {
        rect.GetWorldCorners(rectCorners);
        viewRect.GetWorldCorners(viewCorners);

        //Ϊ�ײ�  �ж��Ƿ���ɾͷ��
        if(isFirst())
        {
            //������scroll��ʾ��Χ  ɾ��ͷ��
            if(rectCorners[0].y>viewCorners[1].y)
            {
                if (OnRemoveHead != null)
                {
                    OnRemoveHead();
                }
            } 
            //������scroll��ʾ��Χ��  ���ͷ��
            if(rectCorners[1].y<viewCorners[1].y)
            {
                if (OnAddHead != null)
                {
                    OnAddHead();                   
                }
            }
        }

        //Ϊβ��  �ж��Ƿ���ɾβ��
        if(isLast())
        {
            //������scroll��ʾ��Χ  ɾ��β��
            if (rectCorners[1].y < viewCorners[0].y)
            {
                if(OnRemoveLast!=null)
                {
                    OnRemoveLast();
                }
            }
            //������scroll��ʾ��Χ��  ���β��
            if (rectCorners[0].y > viewCorners[0].y)
            {
                if(OnAddLast!=null)
                {
                    OnAddLast();
                }
            }
        }
    }

    /// <summary>
    /// �ж��Ƿ�Ϊĩβ����
    /// </summary>
    bool isLast()
    {
        for (int i = transform.parent.childCount-1; i >= 0 ;i-- )
        {
            GameObject item = transform.parent.GetChild(i).gameObject;//�±�i����Ӧ��������
            //�±�i����Ӧ������Ϊ����״̬
            if (item.activeSelf)
            {
                //��һ��״̬Ϊ�����������������˵������Ϊβ��  ����true
                if (item == transform.gameObject)
                {
                    return true;
                }
                break;//�׸�����״̬�����岻������  ��ô��������Ϊβ��  ֱ���˳�ѭ��
            }
        }
        return false;
    }

    /// <summary>
    /// �ж��Ƿ��ײ�����
    /// </summary>
    bool isFirst()
    {
        for (int i = 0; i < transform.parent.childCount; i++)
        {
            GameObject item = transform.parent.GetChild(i).gameObject;//�±�i����Ӧ��������
            //�±�i����Ӧ������Ϊ����״̬
            if(item.activeSelf)
            {
                //��һ��״̬Ϊ�����������������˵������Ϊ�ײ�  ����true
                if(item==transform.gameObject)
                {
                    isf = true;
                    return true;
                }
                break;//�׸�����״̬�����岻������  ��ô��������Ϊ�ײ�  ֱ���˳�ѭ��
            }
        }
        isf = false;
        return false;
    }
    #endregion


}
