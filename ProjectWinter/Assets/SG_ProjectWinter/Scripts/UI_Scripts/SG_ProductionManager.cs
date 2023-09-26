using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SG_ProductionManager : MonoBehaviourPun
{

    // �Ʒ� �ΰ��� Gameobject �� �������� ����־��ٶ��� �󽽷��� ������ Instance�� ���ֱ� ���� ���� ����
    private GameObject madeItemObj;
    private GameObject madeItemObjClone;

    // ������ Instance �ҋ��� ������ Position �� Rotaition
    Vector3 spawnPosition;
    Quaternion spawnRotation;

    private bool isCanCraftItem;    // ������ ������ �����ʴٸ� false�� �Ǿ �Լ��� �������� ����    

    private int tempItemp001Count;
    private int tempItemp002Count;

    // ���ʿ��� for���� ���̱����� ����
    private int tempItem001SlotCount;   // ��������1 �� ������������ ������ ������ ��ǥ�� ����� ����
    private int tempItem002SlotCount;   // ��������2 �� ������������ ������ ������ ��ǥ�� ����� ����

    private bool checkMaterial001MethodClear;   // ù��° �˼� ��� �ߴ��� üũ�� bool ����
    private bool checkMaterial002MethodClear;   // �ι�° �˼� ��� �ߴ��� üũ�� bool ����

    private bool checkItemEnoughCount;          // ù��° �ι�° ��ᰡ �����ҽ� ������ 2���� �����ϴ��� üũ�ϴ� �Լ�

    private bool isMaterialItem002Null;     // ��� ������2�� �������� ������� �˼��� ��� �Ҹ�Pass ������ bool ���� (�������)


    private int inItSlotIndex;    // �������� �־��� ������ Indenx
    private int inItCount;        // �ڷ�ƾ���� �������� ������ ���鶧�� ������ �ε����������� ����

    private Coroutine itemSetUpdatecoroutine;    // �ڷ�ƾ �ڽ�
    private Coroutine weaponColorCoroutine; // ���� �÷� ���� �ڷ�ƾ

    public void StartProduction(SG_Inventory _PlayerInven, SG_WorkStationRecipeImage _ProductionRecipe) // ���� ��ư Ŭ���� ���� �Լ�
    {
        ResetVariable();    // ���� �ʱ�ȭ ���ִ� �Լ�

        CheckMaterialItem002Null(_ProductionRecipe);       // ��������2�� �����ϴ��� üũ�ϴ� �Լ�

        CheckMaterial001(_PlayerInven,_ProductionRecipe);  // �÷��̾� �κ��丮�� ������ ù��° ��ᰡ �����ϴ��� üũ�ϴ� �Լ�
        if (checkMaterial001MethodClear == false) { Debug.Log("ù��° �˼� ����"); return; }  else { /*PASS*/ } // ù��° �������� ���ٸ� ������ ������������ return
        
        CheckMaterial002(_PlayerInven,_ProductionRecipe);  // �÷��̾� �κ��丮�� ������ �ι��� ��ᰡ �����ϴ��� üũ�ϴ� �Լ�
        if (checkMaterial002MethodClear == false) { Debug.Log("�ι�° �˼� ����"); return; }  else { /*PASS*/ } // �ι�° �������� ���ٸ� ������ ������������ return

        CheckItemEnoughCount(_PlayerInven,_ProductionRecipe); // ù��° ��� �ι�° ��ᰡ �����ҽ� �������� 2�� �ʿ����ٵ� �׶��� �÷��̾� �κ��丮�� �������� 2���� �����ϴ��� üũ�ϴ� �Լ�
        if (checkItemEnoughCount == false)        { Debug.Log("������ ����̰� ������������ ����"); return; }  else { /*PASS*/ } // ���������� �����ѵ� ��᰹���� �����ϸ� return

        DeductPlayerItemCount(_PlayerInven,_ProductionRecipe);// �÷��̾��� ���Կ��� ���������� ���ۿ� �ʿ��� ��ŭ �������ִ� �Լ�
        CleanUpPlayerSlot(_PlayerInven);                      // ������ �Ҹ�� �÷��̾� �������� ������ 0����� ���� ����ִ� �Լ�
        InItMadeItem(_PlayerInven, _ProductionRecipe);        // ���� �������� �󽽷��� �����Ѵٸ� �÷��̾� ���Կ� �־��ְ� ���ٸ� �÷��̾� ��ġ�� Instance���ִ� �Լ�        

    }   // StartProduction()

    private void CheckMaterialItem002Null(SG_WorkStationRecipeImage _ProductionRecipe) // 2���� ������ ��ᰡ �ʿ����� �˼��ϴ� �Լ�
    {
        if(_ProductionRecipe.itemRecipe.item002 == null)
        {
            isMaterialItem002Null = true;
        }
        else { /*PASS*/ }
    }

    // ù��° �������� ������ �ִ���,���� �ִ��� Ȯ���ϴ� �Լ�
    private void CheckMaterial001(SG_Inventory _PlayerInven, SG_WorkStationRecipeImage _ProductionRecipe) // ù��° �������� ������ �ִ���,���� �ִ��� Ȯ��
    {
        for(int i = 0; i < _PlayerInven.slots.Length; i++)
        {
            if (_PlayerInven.slots[i].item == _ProductionRecipe.itemRecipe.item001)
            {
                checkMaterial001MethodClear = true;
                tempItem001SlotCount = i;
                return;
            }
            else { checkMaterial001MethodClear = false; }
        }

    }   // CheckMaterial001()

    // �ι�° �������� ������ �ִ���,���� �ִ��� Ȯ��
    private void CheckMaterial002(SG_Inventory _PlayerInven, SG_WorkStationRecipeImage _ProductionRecipe) // �ι�° �������� ������ �ִ���,���� �ִ��� Ȯ��
    {
        if (isMaterialItem002Null == true)
        {
            return;
        }
        else { /*PASS*/ }
        
        for (int i = 0; i < _PlayerInven.slots.Length; i++)
        {
            if (_PlayerInven.slots[i].item == _ProductionRecipe.itemRecipe.item002)
            {
                checkMaterial002MethodClear = true;
                tempItem002SlotCount = i;
                return;
            }
            else { checkMaterial002MethodClear = false; }
        }

    }   // CheckMaterial002()

    // ù��° ��� �ι�° ��ᰡ �����ҽ� �������� 2�� �ʿ����ٵ� �׶��� �÷��̾� �κ��丮�� �������� 2���� �����ϴ��� üũ�ϴ� �Լ�
    private void CheckItemEnoughCount(SG_Inventory _PlayerInven, SG_WorkStationRecipeImage _ProductionRecipe)
    {
        if (_ProductionRecipe.itemRecipe.item001 == _ProductionRecipe.itemRecipe.item002)    // �������� �������� ������ Ȯ��
        {
            for (int i = 0; i < _PlayerInven.slots.Length; i++)  // �÷��̾� ���Ը�ŭ ���鼭 ����üũ
            {
                if (_PlayerInven.slots[i].item == _ProductionRecipe.itemRecipe.item001) // ������ �������� ���� �������
                {
                    if (_PlayerInven.slots[i].itemCount < 2)    // ������ ������ 2�� �̻��ִ��� Ȯ��  �������� 0~1 ���϶��� false�ϸ� break
                    {
                        checkItemEnoughCount = false;
                        break;
                    }
                    else { /*PASS*/ }
                }
                else { /*PASS*/ }
            }
        }
        else { /*PASS*/ }   // �����ǰ� ������ 1 ������ 2�� �������������� Pass

    }   // CheckItemEnoughCount()

    // ��� �������� �������ִ� �Լ�     // Master�� �ؾ��� ����
    private void DeductPlayerItemCount(SG_Inventory _PlayerInven, SG_WorkStationRecipeImage _ProductionRecipe)
    {
        _PlayerInven.slots[tempItem001SlotCount].itemCount -= 1;    // ù��° ������ ��� ����

        _PlayerInven.slots[tempItem002SlotCount].itemCount -= 1;    // �ι�° ������ ��� ����

        // ���⼭ ����� ī��Ʈ ������Ʈ
        if (_PlayerInven.slots[tempItem001SlotCount].itemCount > 0)
        {
            _PlayerInven.slots[tempItem001SlotCount].TextUpdate();
        }
        else { /*PASS*/ }

        if (isMaterialItem002Null == true)
        {
            return;
        }
        else { /*PASS*/ }


        if (_PlayerInven.slots[tempItem002SlotCount].itemCount > 0)
        {
            _PlayerInven.slots[tempItem002SlotCount].TextUpdate();
        }
        else { /*PASS*/ }

    }   // DeductPlayerItemCount()


    private void CleanUpPlayerSlot(SG_Inventory _PlayerInven)   // ������ �Ҹ�� �÷��̾� �������� ������ 0����� ���� ����ִ� �Լ�
    {
        // ù��°���Ҹ� ���� ���� û��
        if (_PlayerInven.slots[tempItem001SlotCount].itemCount <= 0)
        {
            _PlayerInven.slots[tempItem001SlotCount].DisconnectedItem();
        }
        else { /*PASS*/ }

        if (isMaterialItem002Null == true)
        {
            return;
        }
        else { /*PASS*/ }


        // ������ �� �������� ������ī��Ʈ�� �Ҹ�Ǿ� ù���� û�ҿ� ������ �������� Null �Ǿ������� ����ϴ� ����ó��
        if (_PlayerInven.slots[tempItem002SlotCount].item == null)  // 2��° ����� ���� �������� ����ִٸ� �׳� �ѱ��
        {
            return;
        }
        else { /*PASS*/ }


        // �ι�°���Ҹ� ���� ���� û��
        if (_PlayerInven.slots[tempItem002SlotCount].itemCount <= 0)
        {
            _PlayerInven.slots[tempItem002SlotCount].DisconnectedItem();
        }
        else { /*PASS*/ }
    }   // CleanUpPlayerSlot()


    // �̻��
    private void InItItemSlotPick(SG_Inventory _PlayerInven, SG_WorkStationRecipeImage _ProductionRecipe) //������� ��� ������ üũ���� �Լ�
    {
        for(int i = 0; i < _PlayerInven.slots.Length; i++)
        {
            if (_PlayerInven.slots[i].item != null) // �ش� ���Կ� �������� �����Ѵٸ� �� ����
            {
                if (_PlayerInven.slots[i].item.itemType == SG_Item.ItemType.Weapon) // �װ��� �ִ°��� ������ �� ����
                {
                    continue;
                }
                else { /*PASS*/ }

                if (_PlayerInven.slots[i].item == _ProductionRecipe.itemRecipe.madeItem) // ���� �����۰� ������ �������� ������ �ִٸ�
                {
                    if(_PlayerInven.slots[i].itemCount >= 3)    // ������ ������������ �����۰����� �ִ�ġ�� �Ѿ�� �� �Լ�
                    {
                        continue;
                    }

                    else if(_PlayerInven.slots[i].itemCount > 3)    // ������ �������� ������ �ְ� ������ ������ �ִ�ġ�� �����ʴ´ٸ�
                    {
                        inItSlotIndex = i;  //�־��� ������ ��ǥ
                        break;
                    }
                }
                else { /*PASS*/ }
            }
        }        

    }

    // �÷��̾�� ������� �������� �ִ� �Լ�
    private void InItMadeItem(SG_Inventory _PlayerInven, SG_WorkStationRecipeImage _ProductionRecipe) // �÷��̾�� ������� �������� �ִ� �Լ�
    {
        for(int i = 0; i < _PlayerInven.slots.Length; i++)
        {
            if (_PlayerInven.slots[i].item == null) //������ ����ִٸ�
            {
                _PlayerInven.slots[i].item = _ProductionRecipe.itemRecipe.madeItem; // �ϼ� ������ �־��ֱ�
                _PlayerInven.slots[i].itemCount = _ProductionRecipe.itemRecipe.madeItemCount;   // �ϼ��� ������ ���� ����

                inItCount = i;

                _PlayerInven.slots[i].ImageObjInstance();   // �ڽĿ�����Ʈ�� Item Image Instace�� �ڵ����� �ʿ��� Component �־��ִ� �Լ� Call                
                itemSetUpdatecoroutine = StartCoroutine(PlayerItemSetUpdate(_PlayerInven)); // �÷��̾� ������ǥ�� ������Ʈ�� 1������ �ڿ� ���� �ڷ�ƾ                
                

                if (_ProductionRecipe.itemRecipe.madeItem.itemType == SG_Item.ItemType.Weapon)  // ���� �������� �����ϰ�� ���Լ�
                {                    
                    weaponColorCoroutine = StartCoroutine(WeaponColorUpdate(_PlayerInven));
                }
                else { /*PASS*/ }
                return; // �󽽷Կ� �־��־����� �Լ� Ż��
            }
            else { /*PASS*/ }
        }

        // �̾Ʒ��� �´ٸ� ������ ����� ���ٴ� �ǹ�
        madeItemObj = _ProductionRecipe.itemRecipe.madeItem.itemPrefab;

        // ������ ��ġ�� �κ��丮 ������ �÷��̾��� ��ġ�� �����
        spawnPosition = _PlayerInven.slots[0].slotTopParentObj.transform.position;
        spawnRotation = _PlayerInven.slots[0].slotTopParentObj.transform.rotation;

        // ���� �Ƹ����� �÷��̾��� ��ġ�� Instance
        madeItemObjClone = Instantiate(madeItemObj, spawnPosition, spawnRotation);
        


    }   // InItMadeItem()




    private void ResetVariable()    // ���� �ʱ�ȭ ���ִ� �Լ�    �Ŵ��� �����,����� �� 2�� �ʱ�ȭ
    {
        isCanCraftItem = true;
        checkMaterial001MethodClear = true;
        checkMaterial002MethodClear = true;
        checkItemEnoughCount = true;
        isMaterialItem002Null = false;
    }



    IEnumerator PlayerItemSetUpdate(SG_Inventory _PlayerInven)  // �÷��̾� ������ǥ�� ������Ʈ�� 1������ �ڿ� ���� �ڷ�ƾ
    {
        yield return null;
        _PlayerInven.slots[inItCount].MoveItemSet();        
    }
    IEnumerator WeaponColorUpdate(SG_Inventory _PlayerInven)
    {
        yield return null;
        _PlayerInven.slots[inItCount].WeaponColorSet();
    }



}
