using UnityEngine;
public class AIEngine
{
    private int originDepth;
    public int[] FindBestMove(Board board, int depth)
    {
        originDepth = depth;
        int[] bestMove = new int[2] { int.MinValue, int.MinValue };
        board.ResetBoardBounds();
        var canEndMove = SearchWinningMove(board, bestMove);
        if (!canEndMove) canEndMove = SearchLosingMove(board, bestMove);
        if (!canEndMove)
        {
            int availableMoves = 0;
            sbyte[] indexs = new sbyte[256];
            for (int i = 1 - board.size; i < board.size; i++)
            {
                for (int j = 1 - board.size; j < board.size; j++)
                {
                    if (board.GetCell(i, j) != 0) continue;
                    if (board.IsCellEmpty(i, j - 1)
                && board.IsCellEmpty(i, j + 1)
                && board.IsCellEmpty(i + 1, j)
                && board.IsCellEmpty(i + 1, j + 1)
                && board.IsCellEmpty(i - 1, j - 1)
                && board.IsCellEmpty(i - 1, j))
                        continue;
                    indexs[availableMoves * 2] = (sbyte)i;
                    indexs[availableMoves * 2 + 1] = (sbyte)j;
                    availableMoves++;
                }
            }
            float alpha = float.MinValue;
            float bestScore = float.MinValue;
            for (int i = availableMoves - 1; i > -1; i--)
            {
                int cellIndex = indexs[i * 2] * board.boardWidth + board.offset + indexs[i * 2 + 1];
                board.cells[cellIndex] = 1;//max player alway hold blue piece
                var tmpscore = AlphaBetaPrunning(board, alpha, float.MaxValue, false, depth - 1);
                board.cells[cellIndex] = 0;//undo move
                if (alpha < tmpscore)
                    alpha = tmpscore;
                if (tmpscore > bestScore)
                {
                    bestScore = tmpscore;
                    bestMove[0] = indexs[i * 2];
                    bestMove[1] = indexs[i * 2 + 1];
                }
            }
        }
        return bestMove;
    }
    private bool SearchWinningMove(Board board, int[] moves)
    {
        for (int i = 1 - board.size; i < board.size; i++)
        {
            for (int j = 1 - board.size; j < board.size; j++)
            {
                if (board.GetCell(i, j) != 0) continue;
                if (board.IsCellEmpty(i, j - 1)
                    && board.IsCellEmpty(i, j + 1)
                    && board.IsCellEmpty(i + 1, j)
                    && board.IsCellEmpty(i + 1, j + 1)
                    && board.IsCellEmpty(i - 1, j - 1)
                    && board.IsCellEmpty(i - 1, j))
                    continue;
                int cellIndex = i * board.boardWidth + board.offset + j;
                board.cells[cellIndex] = 1;//max player alway hold blue piece
                var tmpscore = board.EvaluateScore(false, false);
                board.cells[cellIndex] = 0;//undo move
                if (tmpscore >= board.WIN_SCORE)
                {
                    moves[0] = i;
                    moves[1] = j;
                    return true;
                }
            }
        }
        return false;
    }
    private bool SearchLosingMove(Board board, int[] moves)
    {
        for (int i = 1 - board.size; i < board.size; i++)
        {
            for (int j = 1 - board.size; j < board.size; j++)
            {
                if (board.GetCell(i, j) != 0) continue;
                if (board.IsCellEmpty(i, j - 1)
                    && board.IsCellEmpty(i, j + 1)
                    && board.IsCellEmpty(i + 1, j)
                    && board.IsCellEmpty(i + 1, j + 1)
                    && board.IsCellEmpty(i - 1, j - 1)
                    && board.IsCellEmpty(i - 1, j))
                    continue;
                int cellIndex = i * board.boardWidth + board.offset + j;
                board.cells[cellIndex] = -1;//min player alway hold yellow piece
                var tmpscore = board.EvaluateScore(true, true);
                board.cells[cellIndex] = 0;//undo move
                if (tmpscore >= board.WIN_SCORE)
                {
                    moves[0] = i;
                    moves[1] = j;
                    return true;
                }
            }
        }
        return false;
    }
    protected float AlphaBetaPrunning(Board board, float alpha, float beta, bool isMax, int depth)
    {
        board.BuildSmallBoard();
        float yellowScore = board.EvaluateScore(true, !isMax);
        float blueScore = board.EvaluateScore(false, !isMax);
        if (yellowScore == 0) yellowScore = 1;
        if (depth == 0 || blueScore >= board.WIN_SCORE || yellowScore >= board.WIN_SCORE || (board.totalMoves + originDepth - depth) >= board.totalCells) return blueScore / yellowScore;
        sbyte iMax = board.iMax, jMin = board.jMin, jMax = board.jMax;
        if (isMax)
        {
            float bestScore = float.MinValue;
            for (sbyte i = board.iMin; i <= iMax; i++)
            {
                for (sbyte j = jMin; j <= jMax; j++)
                {
                    if (board.GetCell(i, j) != 0) continue;
                    if (
                    board.IsCellEmpty(i - 1, j - 1)
                    && board.IsCellEmpty(i - 1, j)
                    && board.IsCellEmpty(i, j - 1)
                    && board.IsCellEmpty(i, j + 1)
                    && board.IsCellEmpty(i + 1, j)
                    && board.IsCellEmpty(i + 1, j + 1)
                    )
                        continue;
                    board.cells[i * board.boardWidth + board.offset + j] = 1;//max player alway hold blue piece
                    var tmpscore = AlphaBetaPrunning(board, alpha, beta, false, depth - 1);
                    board.cells[i * board.boardWidth + board.offset + j] = 0;//undo move
                    if (alpha < tmpscore)
                        alpha = tmpscore;
                    if (alpha >= beta)
                        return alpha;
                    if (tmpscore > bestScore)
                        bestScore = tmpscore;
                }
            }
            return bestScore;
        }
        else
        {
            float bestScore = float.MaxValue;
            for (sbyte i = board.iMin; i <= iMax; i++)
            {
                for (sbyte j = jMin; j <= jMax; j++)
                {
                    if (board.GetCell(i, j) != 0) continue;
                    if (
                    board.IsCellEmpty(i - 1, j - 1)
                    && board.IsCellEmpty(i - 1, j)
                    && board.IsCellEmpty(i, j - 1)
                    && board.IsCellEmpty(i, j + 1)
                    && board.IsCellEmpty(i + 1, j)
                    && board.IsCellEmpty(i + 1, j + 1)
                    )
                        continue;
                    board.cells[i * board.boardWidth + board.offset + j] = -1;//min player alway hold yellow piece
                    var tmpscore = AlphaBetaPrunning(board, alpha, beta, true, depth - 1);
                    board.cells[i * board.boardWidth + board.offset + j] = 0;//undo move
                    if (beta > tmpscore)
                        beta = tmpscore;
                    if (beta <= alpha)
                        return beta;
                    if (tmpscore < bestScore)
                        bestScore = tmpscore;
                }
            }
            return bestScore;
        }
    }
}
