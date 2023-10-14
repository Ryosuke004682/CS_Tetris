using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/*�Q�[���̊�ՂƂȂ郍�W�b�N�������Ă�����*/
public class Game : MonoBehaviour
{
    //�t�B�[���h�̑傫�����`���Ă��
    const int FieldXLength   = 10;
    const int FieldYLength   = 20;

    const float FallInterval = 0.3f;//���b���ŗ���������̂����`

    private DateTime lastFallTime;

    //�Q�[���I�u�W�F�N�g���i�[�����
    [SerializeField]
    private GameObject field;

    //����̃Q�[���I�u�W�F�N�g���i�[����B�i���񂾂�����Squere�v���n�u�j
    [SerializeField]
    private SpriteRenderer squerePrefab;

    //�u���b�N��X���AY���ɂǂꂭ�炢�������邩�ǂ������i�[����z
    private SpriteRenderer[,] blockReadereObjects;
    
    private Tetrimino tetrimino = new Tetrimino();
    
    
    private BlockType[,] fieldBlocks;
    //enum = �񋓌^�B���O�̒ʂ菈����񋓏o���܂��B
    public enum BlockType
    {
        None       = 0,
        TetriminoI = 1,
        TetriminoO = 2,
        TetriminoS = 3,
        TetriminoZ = 4,
        TetriminoJ = 5,
        TetriminoL = 6,
        TetriminoT = 7
    }


    private void Start()
    {
        GameInitialSetting();
        UpdateLocation();
        Draw();
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

        fieldBlocks = new BlockType[FieldYLength, FieldXLength];

    }


    /// <summary>
    /// 0.3�b���Ƃ�1�u���b�N�����Ƃ��Ă���
    /// </summary>
    private void UpdateLocation()
    {
        tetrimino.Initialize();

        lastFallTime = DateTime.UtcNow;

        for(int y = 0; y < FieldYLength; y++)
        {
            for(int x = 0; x < FieldXLength; x++)
            {
                fieldBlocks[y, x] = BlockType.None;
            }
        }
    }


    /// <summary>
    /// ���Ƃ�����ŕ`�悳��ĂȂ��ƍ��邩��`�揈����������
    /// </summary>
    private void Draw()
    {
        for (int y = 0; y < FieldYLength; y++)
        {
            for (int x = 0; x < FieldXLength; x++)
            {
                SpriteRenderer blockObj  = blockReadereObjects[y , x];
                Game.BlockType blockType = fieldBlocks[y , x];

                blockObj.color = GetBlockColor(blockType);//�`�悳����
            }
        }

        Vector2Int[] positions = tetrimino.GetBlockPositions();

        foreach (Vector2Int position in positions)
        {
            SpriteRenderer tetriminoBlock = blockReadereObjects[position.y , position.x];
            tetriminoBlock.color = GetBlockColor(tetrimino.BlockType);
        }

    }


    /// <summary>
    /// �u���b�N�ɐF��t���Ă�Ƃ���
    /// </summary>
    /// <param name="blockType"> �u���b�N�̎�ނ������Ŏ��� </param>
    /// <returns></returns>
    private Color GetBlockColor(BlockType blockType)
    {
        switch (blockType)
        {
            case BlockType.None       : return Color.black;
            case BlockType.TetriminoI : return Color.cyan;

            default : return Color.white;//��O����
        }
    }
}
