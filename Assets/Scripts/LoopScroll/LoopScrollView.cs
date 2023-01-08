using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoopScrollView : MonoBehaviour
{

    #region �ֶ�

    private GameObject ChildItemPrefab;//������Ԥ����
    private GridLayoutGroup contentLayout;
    private ContentSizeFitter contentSizeFitter;
    private RectTransform content;
    private DataManager dataManager;

    #endregion

    #region �ص�����

    void Awake()
    {
        contentLayout = transform.Find("Viewport/Content").GetComponent<GridLayoutGroup>();
        contentSizeFitter = transform.Find("Viewport/Content").GetComponent<ContentSizeFitter>();
        ChildItemPrefab = Resources.Load<GameObject>("Prefabs/Item/Item1");
        content = transform.Find("Viewport/Content").GetComponent<RectTransform>();
        dataManager = new DataManager();

        //��������
        List<LoopDataItem> loopDataItems = new List<LoopDataItem>();

        for(int i=1;i<=40;i++)
        {
            string itemName = "item" + i.ToString();
            loopDataItems.Add(new LoopDataItem(i,itemName));
        }
        dataManager.InitData(loopDataItems);

    }
    // Start is called before the first frame update
    void Start()
    {
        OnAddHead();
        Invoke("enabledContent", 0.1f);
    }

    // Update is called once per frame
    void Update()
    {

    }
    #endregion

    #region ����
    GameObject GetChildItem()
    {
        //���ҿ�������  
        for(int i=0;i<content.childCount;i++)
        {
            GameObject item=content.GetChild(i).gameObject;
            //����״̬û����  ˵���䱻���� ����ֱ��������
            if(!item.activeSelf)
            {
                item.SetActive(true);
                return item;
            }
        }
        //���޿��õ�����  ����һ��
        GameObject childItem = GameObject.Instantiate(ChildItemPrefab, content.transform);
        //��ʼ������
        childItem.transform.localScale = Vector3.one;
        childItem.transform.localPosition = Vector3.zero;
        childItem.GetComponent<RectTransform>().anchorMin = new Vector2(0, 1);
        childItem.GetComponent<RectTransform>().anchorMax = new Vector2(0, 1);
        childItem.GetComponent<RectTransform>().sizeDelta = contentLayout.cellSize;
        LoopItem loopItem=childItem.GetComponent<LoopItem>();
        //���¼�
        loopItem.OnAddHead += OnAddHead;
        loopItem.OnRemoveHead += OnRemoveHead;
        loopItem.OnAddLast += OnAddLast;
        loopItem.OnRemoveLast += OnRemoveLast;
        return childItem;
    }

    //���ͷ
    void OnAddHead()
    {
        LoopDataItem loopDataItem = dataManager.GetHeadData();
        if(loopDataItem!=null)
        {
            Transform first = FindFirst();//�ҵ���ǰͷ��������������ڵ�ǰͷ��ǰ��

            GameObject obj = GetChildItem();//��ȡһ������
            obj.transform.SetAsFirstSibling();//��Ϊ�ײ�
            //��������
            SetData(obj, loopDataItem);
            //����λ�� 
            if (first != null)
            {
                obj.transform.localPosition = first.localPosition
                    + new Vector3(0, contentLayout.cellSize.y + contentLayout.spacing.y);
            }
        }
    }

    //ȥ��ͷ
    void OnRemoveHead()
    {
        if(dataManager.RemoveHeadData())
        {
            Transform first = FindFirst();
            if (first != null)
            {
                first.gameObject.SetActive(false);
            }
        }
    }

    //���β
    void OnAddLast()
    {
        LoopDataItem loopDataItem = dataManager.GetLastData();
        if(loopDataItem!=null)
        {
            Transform last = FindLast();//�ҵ���ǰͷ��������������ڵ�ǰͷ��ǰ��

            GameObject obj = GetChildItem();//��ȡһ������
            obj.transform.SetAsLastSibling();//��Ϊβ��
            //��������
            SetData(obj, loopDataItem);
            //����λ�� 
            if (last != null)
            {
                obj.transform.localPosition = last.localPosition
                    - new Vector3(0, contentLayout.cellSize.y + contentLayout.spacing.y, 0);
            }
            //�ж�Ҫ��Ҫ���content�߶�
            if (IsNeedAddContentHeight(obj.transform))
            {
                //���Ӹ߶�
                content.sizeDelta += new Vector2(0, contentLayout.cellSize.y + contentLayout.spacing.y);
            }
        }
        
    }

    //ȥ��β
    void OnRemoveLast()
    {
        if(dataManager.RemoveLastData())
        {
            Transform last = FindLast();
            if (last != null)
            {
                last.gameObject.SetActive(false);
            }
        }  
    }

    //�ҵ�Ŀǰ�ײ�����
    Transform FindFirst()
    {
        for(int i=0;i<content.childCount;i++)
        {
            GameObject item = content.GetChild(i).gameObject;
            if(item.activeSelf)
            {
                return item.transform;
            }
        }
        return null;
    }

    //�ҵ�Ŀǰβ������
    Transform FindLast()
    {
        for (int i =content.childCount-1; i >=0; i--)
        {
            GameObject item = content.GetChild(i).gameObject;
            if (item.activeSelf)
            {
                return item.transform;
            }
        }
        return null;
    }

    //����content���
    void enabledContent()
    {
        contentLayout.enabled = false;
        contentSizeFitter.enabled = false;
    }

    //�ж�Ҫ��Ҫ����content�߶�
    bool IsNeedAddContentHeight(Transform item)
    {
        Vector3[] itemCorners=new Vector3[4];
        Vector3[] contentCorners=new Vector3[4];
        //��ȡ��β�������content���ĸ������� �Ƚ�����
        item.GetComponent<RectTransform>().GetWorldCorners(itemCorners);
        content.GetComponent<RectTransform>().GetWorldCorners(contentCorners);
        if(itemCorners[0].y<contentCorners[0].y)
        {
            return true;
        }
        return false;
    }

    //��������
    public void SetData(GameObject childItem,LoopDataItem data)
    {
        childItem.transform.Find("Text").GetComponent<Text>().text = "��Ʒ"+data.id.ToString();
        childItem.GetComponent<Image>().sprite = Resources.Load<Sprite>("Image/" + data.itemName);
    }
    #endregion
}
