using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*�Q�[���̊�ՂƂȂ郍�W�b�N�������Ă�����*/
public class Game : MonoBehaviour
{

    //�t�B�[���h�̑傫�����`���Ă��
    const int FieldXLength = 10;
    const int FieldYLength = 20;


    //�Q�[���I�u�W�F�N�g���i�[�����
    [SerializeField]
    private GameObject field;

    //����̃Q�[���I�u�W�F�N�g���i�[����B�i���񂾂�����Squere�v���n�u�j
    [SerializeField]
    private SpriteRenderer squerePrefab;

    //�u���b�N�̌^���`���Ă��
    private SpriteRenderer[,] blockReadereObjects;


    private void Start()
    {
        GameInitialSetting();
    }

    private void Update()
    {
        
    }

    /// <summary>
    /// �����ݒ肵�Ă�Ƃ�
    /// �I�u�W�F�N�g�̐����Ƃ�
    /// </summary>
    private void GameInitialSetting()
    {
        //�w�肳�ꂽ�ՖʂɐF��t����
        blockReadereObjects = new SpriteRenderer[FieldYLength , FieldXLength];

        //�I�u�W�F�N�g��200���炩���ߐ������Ă����B���R�͉��̃R�����g
        for (int y = 0; y < FieldYLength; y++)
        {
            for(int x = 0; x < FieldXLength; x++)
            {
                //�������ď����āA�������ď����Ă��Əd������̂ōŏ����烁�������J������݌v������B

                SpriteRenderer block          = Instantiate(squerePrefab, field.transform);
                block.transform.localPosition = new Vector3(x - 4.5f, y - 9.5f, 0);//��p����
                block.transform.localRotation = Quaternion .identity;//���������Ƃ��ɕςȕ����������Ȃ��悤�Ƀ��[�e�[�V�����̒l���Œ�
                block.transform.localScale    = Vector3.one;         //�X�P�[�����S��1�̂܂܂ł����̂�1�ɐݒ�
                block.color                   = Color.black;
                blockReadereObjects[y, x]     = block;
            }
        }

    }

}
