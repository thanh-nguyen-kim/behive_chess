using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Board
{
    public int WIN_SCORE = 10000000, WIN_GUARANTEE = 1000000;
    public sbyte[] cells = default;
    public int size = 10, totalMoves, firstColor;
    public int boardWidth, offset;
    public int totalCells = 0;
    public sbyte iMin, iMax, jMin, jMax;
    public Board(int firstColor)
    {
        this.firstColor = firstColor;
        totalMoves = 0;
        boardWidth = 2 * size - 1;
        offset = 2 * size * (size - 1);
    }
    public sbyte GetCell(int x, int z)
    {
        return cells[x * boardWidth + z + offset];
    }
    public void SetCell(int x, int z, sbyte color)
    {
        cells[x * boardWidth + z + offset] = color;
    }
    public bool IsCellEmpty(int x, int z)
    {
        if (x >= size || x <= -size || z >= size || z <= -size) return true;
        sbyte cellVal = GetCell(x, z);
        return cellVal == 0 || cellVal == sbyte.MinValue;
    }
    public bool HasAvailableMove()
    {
        return totalMoves < totalCells;
    }
    public bool IsMachineTurn()
    {
        return totalMoves % 2 == 1;
    }
    public float EvaluateZRow(bool forYellow, bool yellowTurn)
    {
        int consecutive = 0;
        int blocks = 2;
        float score = 0;
        sbyte refCell = (sbyte)(forYellow ? -1 : 1);
        sbyte cell;
        int _imax = iMax + size, _jmax = jMax + size;
        for (int i = iMin + size - 1; i < _imax; i++)
        {
            for (int j = jMin + size - 1; j < _jmax; j++)
            {
                cell = cells[i * boardWidth + j];
                if (cell == refCell)
                    consecutive++;
                else if (cell == 0)
                {
                    if (consecutive > 0)
                    {
                        blocks--;
                        score += CalculateConsecutiveSet(consecutive, blocks, forYellow == yellowTurn);
                        consecutive = 0;
                        blocks = 1;
                    }
                    else
                        blocks = 1;
                }
                else if (consecutive > 0)
                {
                    score += CalculateConsecutiveSet(consecutive, blocks, forYellow == yellowTurn);
                    consecutive = 0;
                    blocks = 2;
                }
                else blocks = 2;
            }
            if (consecutive > 0)
                score += CalculateConsecutiveSet(consecutive, blocks, forYellow == yellowTurn);
            consecutive = 0;
            blocks = 2;
        }
        return score; ;
    }
    public float EvaluateXRow(bool forYellow, bool yellowTurn)
    {
        int consecutive = 0;
        int blocks = 2;
        float score = 0;
        sbyte refCell = (sbyte)(forYellow ? -1 : 1);
        sbyte cell;
        int _imax = iMax + size, _jmax = jMax + size;
        for (int i = jMin + size - 1; i < _jmax; i++)
        {
            for (int j = iMin + size - 1; j < _imax; j++)
            {
                cell = cells[j * boardWidth + i];
                if (cell == refCell)
                    consecutive++;
                else if (cell == 0)
                {
                    if (consecutive > 0)
                    {
                        blocks--;
                        score += CalculateConsecutiveSet(consecutive, blocks, forYellow == yellowTurn);
                        consecutive = 0;
                        blocks = 1;
                    }
                    else
                        blocks = 1;
                }
                else if (consecutive > 0)
                {
                    score += CalculateConsecutiveSet(consecutive, blocks, forYellow == yellowTurn);
                    consecutive = 0;
                    blocks = 2;
                }
                else blocks = 2;
            }
            if (consecutive > 0)
                score += CalculateConsecutiveSet(consecutive, blocks, forYellow == yellowTurn);
            consecutive = 0;
            blocks = 2;
        }
        return score;
    }
    public float EvaluateXZRow(bool forYellow, bool yellowTurn)
    {
        int consecutive = 0;
        int blocks = 2;
        float score = 0;
        sbyte refCell = (sbyte)(forYellow ? -1 : 1);
        int _size = Mathf.Abs(iMin);
        if (Mathf.Abs(iMax) > _size) _size = Mathf.Abs(iMax);
        if (Mathf.Abs(jMin) > _size) _size = Mathf.Abs(jMin);
        if (Mathf.Abs(jMax) > _size) _size = Mathf.Abs(jMax);
        for (int i = -_size; i <= _size; i++)
        {
            int start = -_size + 1 - Mathf.Min(i, 0);
            int end = _size - Mathf.Max(i, 0);
            for (int j = start; j < end; j++)
            {
                sbyte cell = GetCell(j, i + j);
                if (cell == refCell)
                    consecutive++;
                else if (cell == 0)
                {
                    if (consecutive > 0)
                    {
                        blocks--;
                        score += CalculateConsecutiveSet(consecutive, blocks, forYellow == yellowTurn);
                        consecutive = 0;
                        blocks = 1;
                    }
                    else
                        blocks = 1;
                }
                else if (consecutive > 0)
                {
                    score += CalculateConsecutiveSet(consecutive, blocks, forYellow == yellowTurn);
                    consecutive = 0;
                    blocks = 2;
                }
                else
                    blocks = 2;
            }
            if (consecutive > 0)
            {
                score += CalculateConsecutiveSet(consecutive, blocks, forYellow == yellowTurn);
            }
            consecutive = 0;
            blocks = 2;
        }
        return score;
    }
    public float CalculateConsecutiveSet(int consecutive, int blocks, bool currentTurn)
    {
        if (blocks == 2 && consecutive < 5) return 0;
        switch (consecutive)
        {
            case 5: return WIN_SCORE;
            case 4:
                if (blocks == 0)
                {
                    if (currentTurn)
                        return WIN_GUARANTEE;
                    else
                        return WIN_GUARANTEE / 3;
                }
                else
                {
                    if (currentTurn)
                        return WIN_GUARANTEE;
                    else
                        return 200;
                }
            case 3:
                if (blocks == 0)
                {
                    if (currentTurn) return 50000;
                    else return 200;
                }
                else
                {
                    if (currentTurn) return 10;
                    else return 5;
                }
            case 2:
                if (blocks == 0)
                {
                    if (currentTurn) return 7;
                    else return 5;
                }
                else
                    return 3;
            case 1: return 1;
            case 0: return 0;
            default: return WIN_SCORE;
        }
    }
    // <summary>
    /// This method calculate the current consecutive at the given coordinates.
    /// Hàm này có nhiệm vụ đếm số lượng ô hiện tại tạo thành 1 vòng hoàn chỉnh (6 ô)  <summary>
    ///với tham số truyền vào là vị trí của 1 ô trên màn chơi và màu cần đếm
    /// </summary>
    /// <param name="i">the z-coordinate.</param>
    /// <param name="j">the x-coordinate.</param>
    /// <param name="refCell">the value of reference cell,
    /// refCell = -1 if we need to find RingConsecutive of yellow player in position(j,i),
    /// refCell = 1 if we need to find RingConsecutive of green player in position(j,i) </param>
    private int GetRingConsecutive(int i, int j, sbyte refCell)
    {
        int consecutive = 0;
        //todo: implement logic to calculate the current ring consecutive at the given (j,i) coordinates.
        //You can take a look at EvaluateZRow(), EvaluateXRow(),EvaluateXZRow()  fucntion to get the idea.
        return consecutive;
    }
    // <summary>
    /// This method calculate the value of current board base on total possible consecutive ring
    /// Hàm này tính điểm hiện tại của màn chơi dựa trên tổng điểm của toàn bộ các vòng hoàn chỉnh có thể tạo thành
    /// </summary>
    /// <param name="forYellow">forYellow == true mean calculate for yellow player</param>
    /// <param name="yellowTurn">yellowTurn == true mean this move perform by yellow player.</param>
    public float EvaluateRing(bool forYellow, bool yellowTurn)
    {
        int consecutive = 0;
        float score = 0;
        sbyte refCell = (sbyte)(forYellow ? -1 : 1);
        for (int j = jMin; j < jMax - 2; j++)
        {
            for (int i = iMin; i < iMax - 2; i++)
            {
                consecutive = GetRingConsecutive(j, i, refCell);
                if (consecutive > 0)
                {
                    var _score = 0;
                    //todo: Implement logic to calcultate ring score base on consecutive value
                    //You can take a look at CalculateConsecutiveSet() fucntion to get the idea.
                    score += _score;
                    _score = 0;
                }
                consecutive = 0;
            }
        }
        return score;
    }
    public void BuildSmallBoard()
    {
        iMin = (sbyte)(1 - size);
        iMax = (sbyte)(size - 1);
        jMin = (sbyte)(1 - size);
        jMax = (sbyte)(size - 1);
        //todo: optimize board size to lowwer search time
    }
    public void ResetBoardBounds()
    {
        iMin = (sbyte)(1 - size);
        iMax = (sbyte)(size - 1);
        jMin = (sbyte)(1 - size);
        jMax = (sbyte)(size - 1);
    }
    public float EvaluateScore(bool forYellow, bool yellowTurn)//-1 mean yellow win, 1 mean green win, 0 mean tie game
    {
        return EvaluateXRow(forYellow, yellowTurn)
            + EvaluateZRow(forYellow, yellowTurn)
             + EvaluateXZRow(forYellow, yellowTurn)
             + EvaluateRing(forYellow, yellowTurn)
             ;
    }
    public int GetWinner()//-1 mean yellow win, 1 mean green win, 0 mean tie game
    {
        BuildSmallBoard();
        if (EvaluateScore(true, false) >= WIN_SCORE) return -1;
        if (EvaluateScore(false, true) >= WIN_SCORE) return 1;
        return 0;
    }
}
