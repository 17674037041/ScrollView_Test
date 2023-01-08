using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager
{
    #region �ֶ�
    public List<LoopDataItem> allData = new List<LoopDataItem>();//������������
    public LinkedList<LoopDataItem> currentShowData = new LinkedList<LoopDataItem>();//��ǰ��ʾ������

    #endregion


    #region ����
    //��ȡͷ������
    public LoopDataItem GetHeadData()
    {
        if(allData.Count==0)
        {
            return null;
        }

        //��ǰ��ʾ�����������κ�����
        if(currentShowData.Count==0)
        {
            LoopDataItem head = allData[0];
            currentShowData.AddFirst(head);
            return head;
        }

        //��ǰ��ʾ���ݵ���һ������
        LoopDataItem obj = currentShowData.First.Value;
        //�ҵ���ǰ�������������е�λ��
        int index = allData.IndexOf(obj);
        if(index!=0)
        {
            LoopDataItem head = allData[index - 1];
            //�ӵ���ǰ��ʾ����������
            currentShowData.AddFirst(head);
            return head;
        }
        return null;
    }

    //�Ƴ�ͷ������
    public bool RemoveHeadData()
    {
        //��ǰû���κ����� �����Ƴ�
        if(currentShowData.Count==0)
        {
            return false;
        }
        //�Ƴ���ǰ������ʾ���ݵĵ�һ������
        currentShowData.RemoveFirst();
        return true;
    }

    //��ȡβ������
    public LoopDataItem GetLastData()
    {
        if (allData.Count == 0)
        {
            return null;
        }

        //��ǰ��ʾ�����������κ�����
        if (currentShowData.Count == 0)
        {
            LoopDataItem last = allData[0];
            currentShowData.AddLast(last);
            return last;
        }
        //��ȡ��ǰ������ʾ���ݵ���һ������
        LoopDataItem obj = currentShowData.Last.Value;
        int index = allData.IndexOf(obj);
        //�жϸ������Ƿ�Ϊ�ܵ����ݵ����һ������  �������ȡ���������� �������ݶ���������
        if(index!=allData.Count-1)
        {
            LoopDataItem last=allData[index+1];
            currentShowData.AddLast(last);
            return last;
        }
        return null;
    }

    //�Ƴ�β������
    public bool RemoveLastData()
    {
        //�Ƴ���ǰ������ʾ���ݵ����һ������
        if(currentShowData.Count==0)
        {
            return false;
        }
        currentShowData.RemoveLast();
        return true;
    }

    public void InitData(LoopDataItem[] obj)
    {
        allData.Clear();
        currentShowData.Clear();
        allData.AddRange(obj);
    }

    public void InitData(List<LoopDataItem> obj)
    {
        InitData(obj.ToArray());
    }

    public void AddData(LoopDataItem[] obj)
    {
        allData.AddRange(obj);
    }

    public void AddData(List<LoopDataItem> obj)
    {
        AddData(obj.ToArray());
    }
    #endregion

    
}
