using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*�e�g���~�m�̏����������Ă�����*/
public class Tetrimino
{
    private Vector2Int basePosition;

    public Game.BlockType BlockType { get; private set; }


    public void Initialize(Game.BlockType blockType = Game.BlockType.None)
    {
        //blockType�������Ȃ������ꍇ�A
        if (blockType == Game.BlockType.None)
        {
            //TODO�����_���Ƀe�g���~�m�̃^�C�v������
            blockType = Game.BlockType.TetriminoI;
        }

        basePosition  = Vector2Int.zero;
        BlockType     = blockType;
    }

    /// <summary>
    /// �c��4���񂾃e�g���~�m�����B
    /// ��
    /// ��
    /// ��
    /// ���̃C���[�W
    /// </summary>
    /// <returns></returns>
    public Vector2Int[] GetBlockPositions()
    {
        return new Vector2Int[]
        {
            basePosition,                                       //( 0 �C0 )
            new Vector2Int(basePosition.x , basePosition.y + 1),//( 0 , 1 )
            new Vector2Int(basePosition.x , basePosition.y + 2),//( 0 , 2 )
            new Vector2Int(basePosition.x , basePosition.y + 3),//( 0 , 3 )
        };
    }

    /// <summary>
    /// ���������������鏈��
    /// </summary>
    /// <param name="x"> �w���W�̕ϐ� </param>
    /// <param name="y"> �x���W�̕ϐ� </param>
    public void FallingMove(int x , int y)
    {
        basePosition.Set(basePosition.x + x , basePosition.y + y);
    }
}
