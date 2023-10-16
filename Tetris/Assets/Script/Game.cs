using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting.FullSerializer.Internal;

/*�Q�[���̊�ՂƂȂ郍�W�b�N�������Ă�����*/
public class Game : MonoBehaviour
{
    //�t�B�[���h�̑傫�����`���Ă��
    const int FieldXLength     = 10;
    const int FieldYLength     = 20;
    const int NextFieldXLength =  4;//�ǉ�
    const int NextFieldYLength =  4;//�ǉ�

    [Tooltip("���b���ŗ��������邩�ݒ�o����")]
    public float FallInterval = 0.3f;//���b���ŗ���������̂����`

    private DateTime      lastFallTime;
    private DateTime lastControlleTime;


    //�Q�[���I�u�W�F�N�g���i�[�����
    [SerializeField]
    private GameObject field;

    //����̃Q�[���I�u�W�F�N�g���i�[����B�i���񂾂�����Squere�v���n�u�j
    [SerializeField]
    private SpriteRenderer squerePrefab;

    //���ɗ����Ă���~�m���i�[�����B
    [SerializeField]
    private GameObject nextField;

    //�u���b�N��X���AY���ɂǂꂭ�炢�������邩�ǂ������i�[����z
    private SpriteRenderer[,] blockReadereObjects;
    private SpriteRenderer[,]    nextBlockObjects;
    
    private Tetrimino tetrimino     = new Tetrimino();
    private Tetrimino nextTetrimino = new Tetrimino();
    
    
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
        ControlTetrimino();
        Draw();

        DateTime now = DateTime.UtcNow;//���݂̎��Ԃ��擾

        //���݂̎�������Ō�ɗ����������Ԃ�b�ɕϊ����AFallInterval������������������ɏ����������Ȃ킸�ɏ����𔲂���B
        if ((now - lastFallTime).TotalSeconds < FallInterval) { return; }

        lastFallTime = now;

        if (!TryMoveTetrimino(0, 1))
        {
            Vector2Int[] positions = tetrimino.GetBlockPositions();

            foreach(Vector2Int position in positions)
            {
                fieldBlocks[position.y, position.x] = nextTetrimino.BlockType;
            }

            tetrimino.Initialize(nextTetrimino.BlockType);
            nextTetrimino.Initialize();
        }
    }

    /// <summary>
    /// �e�g���~�m�����[�U�[�̓��͂��瑀�삳���Ă�Ƃ���B
    /// </summary>
    private bool ControlTetrimino()
    {
        DateTime now = DateTime.UtcNow;

        if ((now - lastControlleTime).TotalSeconds < 0.1f) { return false; }


        if (Input.GetKey(KeyCode.LeftArrow))
        {
            if(TryMoveTetrimino(-1 , 0))
            {
                lastControlleTime = now;
                return true;
            }
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            if (TryMoveTetrimino(1, 0))
            {
                lastControlleTime = now;
                return true;
            }
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            if (TryMoveTetrimino(0, 1))
            {
                lastControlleTime = now;
                return true;
            }
        }
        return false;
    }



    /// <summary>
    /// �������Ă鏊
    /// </summary>
    /// <returns></returns>
    private bool TryMoveTetrimino(int moveX , int moveY)
    {
        if(CanMoveTetrimino(moveX , moveY))
        {
            tetrimino.FallingMove(moveX , moveY);
            return true;
        }
        return false;

    }



    /// <summary>
    /// ��������͈͂̔���
    /// </summary>
    /// <param name="moveX"> X�������ɂǂꂭ�炢�������� </param>
    /// <param name="moveY"> Y�������ɂǂꂭ�炢�������� </param>
    /// <returns></returns>
    private bool CanMoveTetrimino(int moveX , int moveY)
    {
        Vector2Int[] blockPositions = tetrimino.GetBlockPositions();

        foreach(Vector2 blockPosition in blockPositions)
        {
            float x = blockPosition.x + moveX;
            float y = blockPosition.y + moveY;

            //��ʊO�ɍs���Ȃ��悤�ɔ���
            if      (x < 0 || FieldXLength <= x) { return false; }
            else if (y < 0 || FieldYLength <= y) { return false; }

            if (fieldBlocks[(int)y, (int)x] != BlockType.None) { return false; }
        }
        return true;
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

        //�ǉ�
        nextBlockObjects = new SpriteRenderer[NextFieldYLength , NextFieldXLength];
        for(int y = 0; y < NextFieldYLength; y++)
        {
            for (int x = 0; x < NextFieldXLength; x++)
            {
                SpriteRenderer block = Instantiate(squerePrefab, nextField.transform);
                block.transform.localPosition = new Vector3(x- 1.5f, y - 1.5f, 0);//��p����
                block.transform.localRotation = Quaternion.identity;//���������Ƃ��ɕςȕ����������Ȃ��悤�Ƀ��[�e�[�V�����̒l���Œ�
                block.transform.localScale = Vector3.one;         //�X�P�[�����S��1�̂܂܂ł����̂�1�ɐݒ�
                block.color = Color.black;
                nextBlockObjects[y, x] = block;
            }
        }
        fieldBlocks      = new BlockType[FieldYLength, FieldXLength];
    }



    /// <summary>
    /// 0.3�b���Ƃ�1�u���b�N�����Ƃ��Ă����@�ǉ������Ƃ�
    /// </summary>
    private void UpdateLocation()
    {
        tetrimino.Initialize();
        nextTetrimino.Initialize();
        lastFallTime      = DateTime.UtcNow;
        lastControlleTime = DateTime.UtcNow;

        for(int y = 0; y < FieldYLength; y++)
        {
            for(int x = 0; x < FieldXLength; x++)
            {
                fieldBlocks[y, x] = BlockType.None;
            }
        }
    }



    /// <summary>
    /// �e�g���~�m��`�悵�Ă�Ƃ���@�ǉ������Ƃ�
    /// </summary>
    private void Draw()
    {
        //�t�B�[���h��`��
        for (int y = 0; y < FieldYLength; y++)
        {
            for (int x = 0; x < FieldXLength; x++)
            {
                SpriteRenderer blockObj  = blockReadereObjects[y , x];
                Game.BlockType blockType = fieldBlocks[y , x];

                blockObj.color = GetBlockColor(blockType);
            }
        }


        //�e�g���~�m��`��
        {
            Vector2Int[] positions = tetrimino.GetBlockPositions();
            foreach (Vector2Int position in positions)
            {
                SpriteRenderer tetriminoBlock = blockReadereObjects[position.y, position.x];
                tetriminoBlock.color = GetBlockColor(tetrimino.BlockType);
            }
        }


        //Next�t�B�[���h��`��
        for(int y = 0; y < NextFieldXLength; y++)
        {
            for(int x = 0; x < NextFieldYLength; x++)
            {
                nextBlockObjects[y, x].color = GetBlockColor(BlockType.None);
            }
        }


        //Next�e�g���~�m��`��
        {
            Vector2Int[] nextTetrimino_Positions = nextTetrimino.GetBlockPositions();
            
            foreach(Vector2Int position in nextTetrimino_Positions)
            {
                SpriteRenderer tetriminoBlock = nextBlockObjects[position.y , position.x + 1];
                tetriminoBlock.color = GetBlockColor(nextTetrimino.BlockType);
            }
        }
    }



    /// <summary>
    /// �u���b�N�ɐF��t���Ă�Ƃ��� �ǉ������Ƃ�
    /// </summary>
    /// <param name="blockType"> �u���b�N�̎�ނ������Ŏ��� </param>
    /// <returns></returns>
    private Color GetBlockColor(BlockType blockType)
    {
        switch (blockType)
        {
            //�e�`�̃e�g���~�m�̐F�����i�e�g���X�̋K���ɂ̂��Ƃ��Đݒ�j
            case BlockType.None       : return Color.black;
            case BlockType.TetriminoI : return Color.cyan;
            case BlockType.TetriminoO : return Color.yellow;
            case BlockType.TetriminoT : return Color.magenta;
            case BlockType.TetriminoJ : return Color.blue;
            case BlockType.TetriminoL : return new Color(1.0f , 0.5f , 0.0f);
            case BlockType.TetriminoS : return Color.green;
            case BlockType.TetriminoZ : return Color.red;

            default : return Color.white;//��O����
        }
    }


}
