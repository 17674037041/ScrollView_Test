using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScalePageScrollView : PageScrollView
{
    #region �ֶ�
    private Transform[] items;//�洢content�µ�������

    private float currentScale = 1f;//��ǰҳ��С
    private float otherScale = 0.6f;//����ҳ��С

    private int lastPage;//��һҳ
    private int nextPage;//��һҳ
    private float percent= 0;//�ٷֱ�
    private float rotation = 30;//��ת����
    #endregion


    #region �ص�����
    void Awake()
    {
        base.Awake();
        items = new Transform[Item_num];
        for (int i = 0; i < Item_num; i++)
        {
            items[i] = transform.Find("Viewport/Content").GetChild(i);
        }

    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
        ListenerScale();
        ListenerRotation();
    }
    #endregion

    #region ����
    /// <summary>
    /// ��������
    /// </summary>
    public void ListenerScale()
    {
        //�ҵ���ǰλ�õ���һҳ����һҳ
        for(int i=0;i<Page_Pos.Length;i++)
        {
            if(Page_Pos[i]<=rect.horizontalNormalizedPosition)
            {
                lastPage = i;
            }
        }

        for(int i=0;i<Page_Pos.Length;i++)
        {
            //ĳһҳˮƽ�����ȵ�ǰλ��ˮƽ������ʱ�˳�ѭ��
            if(Page_Pos[i]>rect.horizontalNormalizedPosition)
            {
                nextPage = i;
                break;
            }
        }
        //��ĩ����
        if(lastPage==nextPage)
        {
            return;
        }
        percent = (rect.horizontalNormalizedPosition - Page_Pos[lastPage]) / (Page_Pos[nextPage] - Page_Pos[lastPage]);
        items[lastPage].localScale = Vector3.Lerp(Vector3.one * currentScale, Vector3.one * otherScale, percent);
        items[nextPage].localScale = Vector3.Lerp(Vector3.one * currentScale, Vector3.one * otherScale, 1 - percent);

        //���˵�ǰҳ��СΪ1�� �����Ϊ0.6
        for(int i=0;i<items.Length;i++)
        {
            if(i!=lastPage&&i!=nextPage)
            {
                items[i].localScale = Vector3.one * otherScale;
            }
        }
    }

    /// <summary>
    /// ������ת
    /// </summary>
    public void ListenerRotation()
    {

        if(lastPage==nextPage)
        {
            return;
        }
        items[lastPage].localRotation = Quaternion.Euler(Vector3.Lerp(Vector3.zero,new Vector3(0,-rotation,0),percent));
        items[nextPage].localRotation = Quaternion.Euler(Vector3.Lerp(Vector3.zero, new Vector3(0, rotation, 0), 1-percent));
        
        //����ҳ
        for(int i=0;i<items.Length;i++)
        {
            if(i!=lastPage&&i!=nextPage)
            {
                //��ǰҳ��� ��ʱ����תrotation��
                if (i < currentPage)
                {
                    items[i].rotation = Quaternion.Euler(0, -rotation, 0);
                }
                //��ǰҳ
                if (i == currentPage)
                {
                    items[i].rotation = Quaternion.Euler(0, 0, 0);
                }
                //��ǰҳ�ұ�
                if (i > currentPage)
                {
                    items[i].rotation = Quaternion.Euler(0, rotation, 0);
                }
            }  
        }
    }
    #endregion
}
