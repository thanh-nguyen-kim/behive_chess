using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixelplacement;
public class HexGrid : Singleton<HexGrid>
{
    public const string WIN_COUNT = "win_count";
    public const float RADIUS = 1.1f;
    public const float INNER_RADIUS = RADIUS * 0.866025404f;
    public Board board;
    public List<GameObject> platforms;
    public List<Cell> cells;
    public GameObject platformPrefab;
    public Cell cellPrefab;
    public StateMachine stateMachine;
    public Material skyboxMat;
    [SerializeField] private AudioClip cellPlaceClip, boardClip;
    private AIEngine aiEngine;
    private ThreadWorker worker;
    private int[] machineMove = new int[2];
    public int winnerID = 0;
    private Cell currentCell;
    public Cell pendingCell;
    public bool humanMoveFirst = true;
    private Cell[] cellsRecord = new Cell[4];
    private int currentRecordIndex = 0;
    private int originDepth = 5;
    protected override void OnRegistration()
    {
        aiEngine = new AIEngine();
        worker = new ThreadWorker();
        int winCount = Mathf.Max(PlayerPrefs.GetInt(WIN_COUNT, 0), 0);
        originDepth = Mathf.Min(3 + winCount / 2, 5);
        if (winCount < 2)
            humanMoveFirst = true;
        else
            humanMoveFirst = Random.Range(0, 10) > 5 ? true : false;
        currentRecordIndex = 0;
    }
    public void StartGame()
    {
        if (!humanMoveFirst)
        {
            board.totalMoves++;
            Tween.ShaderColor(skyboxMat, "_Color1", Color.green, 1f, 0);
            stateMachine.ChangeState(2);
        }
        else
        {
            stateMachine.ChangeState(1);
            Tween.ShaderColor(skyboxMat, "_Color1", Color.yellow, 1f, 0);
        }
    }
    public void ConstructBoard()
    {
        Cell cell;
        GameObject platform;
        board = new Board(-1);
        sbyte[] cellsData = new sbyte[(2 * board.size - 1) * (2 * board.size - 1)];
        for (int i = cellsData.Length - 1; i > -1; i--)
            cellsData[i] = sbyte.MinValue;
        int totalCells = 0;
        AudioController.Instance.PlaySfx(boardClip, 0.5f);
        for (int x = 1 - board.size; x < board.size; x++)
        {
            for (int z = -board.size + 1 + Mathf.Max(x, 0); z < board.size + Mathf.Min(x, 0); z++)
            {
                totalCells++;
                cellsData[(x + board.size - 1) * (2 * board.size - 1) + z + board.size - 1] = 0;
                platform = Instantiate(platformPrefab, transform);
                Vector3 target = new Vector3((2 * x - z) * INNER_RADIUS, 0, z * 1.5f * RADIUS);
                Tween.LocalPosition(platform.transform, target, 1, totalCells / board.size * Time.deltaTime, Tween.EaseOutBack, Tween.LoopType.None, null, null, true);
                platforms.Add(platform);
                cell = Instantiate(cellPrefab, transform);
                cell.transform.localPosition = target;
                cell.Init(x, z);
                cells.Add(cell);
            }
        }
        board.totalCells = totalCells;
        board.cells = cellsData;
    }
    public Cell GetCell(int xPos, int zPos)
    {
        foreach (var cell in cells)
            if (cell.xPos == xPos && cell.zPos == zPos)
                return cell;
        return null;
    }
    private void PlaceCell(int xPos, int zPos)
    {
        if (currentCell != null)
            currentCell.StopEffect();
        pendingCell = currentCell = GetCell(xPos, zPos);
        if (board.IsMachineTurn())
        {
            board.SetCell(xPos, zPos, 1);
            currentCell.SetColor(-board.firstColor);
        }
        else
        {
            board.SetCell(xPos, zPos, -1);
            currentCell.SetColor(board.firstColor);
        }
        AudioController.Instance.PlaySfx(cellPlaceClip);
        currentCell.PlayEffect();
        winnerID = board.GetWinner();
        if (winnerID != 0)
        {
            if (winnerID == -1)
            {
                int winCount = PlayerPrefs.GetInt(WIN_COUNT, 0) + 1;//yellow is -1, green is 1
                PlayerPrefs.SetInt(WIN_COUNT, Mathf.Max(winCount, 0));
            }
            stateMachine.ChangeState("EndGame");
        }
    }
    public void OnCellClick(int xPos, int zPos)
    {
        if (winnerID != 0 || board.GetCell(xPos, zPos) != 0) return;
        if (pendingCell != null)//undo pending move
        {
            pendingCell.SetColor(0);
            board.SetCell(pendingCell.xPos, pendingCell.zPos, 0);
        }
        else
            stateMachine.currentState.SendMessage("OnPlayerReady");
        PlaceCell(xPos, zPos);
    }
    public void MachineMove()
    {
        StartCoroutine(_MachineMove());
    }
    private IEnumerator _MachineMove()
    {
        float startTime = Time.time;
        if (board.totalMoves == 1 && !humanMoveFirst)
        {
            machineMove[0] = 0;
            machineMove[1] = 0;
        }
        else
        {
            worker.Start(Thinking());
            do
            {
                yield return null;
            }
            while (worker.IsRunning());
        }
        var xPos = machineMove[0];
        var zPos = machineMove[1];
        if (Time.time - startTime < 4)
            yield return new WaitForSeconds(4 + startTime - Time.time);
        if (worker.IsCompleted() || (board.totalMoves == 1 && !humanMoveFirst))
            PlaceCell(xPos, zPos);
        if (winnerID == 0)
            NextTurn();
    }
    public void NextTurn()
    {
        board.totalMoves++;
        cellsRecord[currentRecordIndex] = currentCell;//record human move
        currentRecordIndex = (currentRecordIndex + 1) % cellsRecord.Length;
        pendingCell = null;
        if (winnerID == 0)
        {
            if (board.totalMoves % 2 == 1)
            {
                Tween.ShaderColor(skyboxMat, "_Color1", Color.green, 1f, 0);
                stateMachine.ChangeState(2);
            }
            else
            {
                stateMachine.ChangeState(1);
                Tween.ShaderColor(skyboxMat, "_Color1", Color.yellow, 1f, 0);
            }
        }
    }
    public void Undo()
    {
        if (pendingCell != null)//undo pending move
        {
            pendingCell.SetColor(0);
            board.SetCell(pendingCell.xPos, pendingCell.zPos, 0);
        }
        currentRecordIndex = (currentRecordIndex + cellsRecord.Length - 1) % cellsRecord.Length;
        cellsRecord[currentRecordIndex].SetColor(0);//undo machine record
        board.SetCell(cellsRecord[currentRecordIndex].xPos, cellsRecord[currentRecordIndex].zPos, 0);
        board.totalMoves--;
        cellsRecord[currentRecordIndex] = null;
        currentRecordIndex = (currentRecordIndex + cellsRecord.Length - 1) % cellsRecord.Length;
        cellsRecord[currentRecordIndex].SetColor(0);//undo human record
        board.SetCell(cellsRecord[currentRecordIndex].xPos, cellsRecord[currentRecordIndex].zPos, 0);
        cellsRecord[currentRecordIndex] = null;
        board.totalMoves--;
        int indexBeforeLast = (currentRecordIndex + cellsRecord.Length - 1) % cellsRecord.Length;
        if (cellsRecord[indexBeforeLast] != null)
        {
            currentCell = cellsRecord[indexBeforeLast];
            cellsRecord[indexBeforeLast].PlayEffect();
        }
    }
    public bool CanUndo()
    {
        int lastRecordIndex = (currentRecordIndex + cellsRecord.Length - 1) % cellsRecord.Length;
        int theIndexBeforeLast = (currentRecordIndex + cellsRecord.Length - 2) % cellsRecord.Length;
        return cellsRecord[lastRecordIndex] != null && cellsRecord[theIndexBeforeLast] != null;
    }
    private IEnumerator Thinking()
    {
        yield return null;
        int depth = Mathf.Max(originDepth - board.totalMoves / 20, originDepth - 1);
        machineMove = aiEngine.FindBestMove(board, depth);
    }
}
