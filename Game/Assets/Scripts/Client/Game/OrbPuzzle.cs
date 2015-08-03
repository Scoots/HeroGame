using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Holoville.HOTween;
using Common.Data;
using GameEngine.Engine;
using Client.Managers.Game;
using System.Linq;

public struct OrbPoint
{
    public int x, y;
    public OrbPoint(int px, int py)
    {
        x = px;
        y = py;
    }
    public override string ToString()
    {
        return "(" + x + ", " + y + ")";
    }
}

public class Cell
{
    public Enumerations.OrbColor CellType { get; set; }
    public bool IsSuperOrb { get; set; }
    public bool IsEmpty { get { return CellType == Enumerations.OrbColor.Empty; } }
    public void SetRandomOrb(int total)
    {
        this.CellType = (Enumerations.OrbColor)UnityEngine.Random.Range(0, total) + 1;
    }
}

public class OrbPuzzle : MonoBehaviour
{
    public UISlider timeSlider;

    private float turnStartTime = 0;

    public int timeSecondsDrag = 10;

    public bool isDragging = false;

    /// <summary>
    ///	Maximum amount of orbs you can have on the board
    /// </summary>
    public int maxOrbCount = 30;

    public const int maxWidth = 6;

    public const int maxHeight = 5;

    public GameObject orbTemplate;

    public GameObject Grid;

    public static GameObject CurrentOrb;

    private Cell[,] cells = new Cell[maxWidth, maxHeight];

    List<OrbGem> orbs;

    private bool IsMatching = false;

    public UIButton endTurnButton;

    public bool HasMatched = false;

    public AnimatedOrbGem[] animationOrbs;

    public int animationCounter = 0;

    public bool IsBoardTargeting { get; set; }

    public void InitOrbGrid()
    {
        for (int x = 0; x < maxWidth; x++)
        {
            for (var y = 0; y < maxHeight; y++)
            {
                cells[x, y] = new Cell();
                cells[x, y].SetRandomOrb(6);
            }
        }
    }

    public virtual void OnDragOver(GameObject obj)
    {
        var cardObject = obj.GetComponent<OrbCard>();
        if (cardObject == null)
        {
            return;
        }

        cardObject.BoardDrop = Enumerations.DragTargets.Board;
        // We have a card object!
        Debug.Log("Puzzle - We have a card");
    }

    public void CreateOrbGrid()
    {
        for (int x = 0; x < maxWidth; x++)
        {
            for (var y = 0; y < maxHeight; y++)
            {
                cells[x, y] = new Cell();
            }
        }
    }

    OrbGem FindOrb(OrbPoint point)
    {
        foreach (OrbGem orb in orbs)
        {
            if (orb.Loc.Equals(point)) return orb;
        }
        return null;
    }

    /// <summary>
    /// Swaps one orb with another on the board
    /// </summary>
    /// <param name="a">The first orb gem we are swapping</param>
    /// <param name="b">The second orb gem we are swapping</param>
    /// <param name="manualSwap">Designates if this is a player-initiated move, or a board move</param>
    public void DoSwapOrb(OrbGem a, OrbGem b)
    {
        OrbPoint p1 = a.Loc;
        OrbPoint p2 = b.Loc;

        Cell cell = cells[p1.x, p1.y];
        cells[p1.x, p1.y] = cells[p2.x, p2.y];
        cells[p2.x, p2.y] = cell;

        a.Loc = p2;
        b.Loc = p1;
    }

    public void DoAnimatedSwap(GameObject newGame, GameObject oldGem)
    {
        var posB = oldGem.transform.localPosition;

        oldGem.gameObject.transform.localPosition = newGame.gameObject.transform.localPosition;

        animationOrbs[animationCounter].Init(posB, newGame);
        animationOrbs[animationCounter].Move();

        if (++animationCounter >= animationOrbs.Length)
            animationCounter = 0;
    }

    private void DoEmptyDown()
    {
        for (var x = 0; x < maxWidth; x++)
        {
            for (var y = 0; y < maxHeight; y++)
            {
                var thiscell = cells[x, y];
                if (!thiscell.IsEmpty) continue;
                int y1;
                for (y1 = y; y1 > 0; y1--)
                {
                    DoSwapOrb(FindOrb(new OrbPoint(x, y1)), FindOrb(new OrbPoint(x, y1 - 1)));
                }
            }
        }
        for (var x = 0; x < maxWidth; x++)
        {
            int y;
            for (y = maxHeight - 1; y >= 0; y--)
            {
                var thiscell = cells[x, y];
                if (thiscell.IsEmpty) break;
            }
            if (y < 0) continue;
            var y1 = y;
            for (y = 0; y <= y1; y++)
            {
                OrbGem orb = FindOrb(new OrbPoint(x, y));
                orb.transform.localPosition = new Vector3(x * 120, (y - (y1 + 1)) * -120, 0f);
                orb.RefreshOrb();
            }
        }

        foreach (OrbGem orb in orbs)
        {
            Vector3 pos = new Vector3(orb.Loc.x * 120, orb.Loc.y * -120);
            float dist = Vector3.Distance(orb.transform.localPosition, pos) * 0.01f;
            dist = 1f;
            TweenParms parms = new TweenParms().Prop("localPosition", pos).Ease(EaseType.EaseOutQuart);
            HOTween.To(orb.transform, 0.5f * dist, parms);
        }

        StartCoroutine(CheckMatchOrb(0.5f));
    }

    IEnumerator FillEmpty(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        DoEmptyDown();
    }

    void CheckMatch(Dictionary<OrbPoint, Enumerations.OrbColor> stack)
    {
        List<OrbGem> destroyList = new List<OrbGem>();
        foreach (KeyValuePair<OrbPoint, Enumerations.OrbColor> item in stack)
        {
            destroyList.Add(FindOrb(item.Key));
        }
        foreach (OrbGem item in destroyList)
        {
            int type = (int)item.CellType;

            item.CellType = Enumerations.OrbColor.Empty;
            item.GetComponent<UISprite>().enabled = false;
        }

        StartCoroutine(FillEmpty(0.5f));
    }

    IEnumerator CheckMatchOrb(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        Dictionary<OrbPoint, Enumerations.OrbColor> stack = FindMatch(cells);
        if (stack.Count > 0)
        {
            CheckMatch(stack);
        }
        else
        {
            IsMatching = false;
        }
    }

    public void ResolveTurn()
    {
        if (IsMatching)
            return;

        IsMatching = true;
        isDragging = false;
        HasMatched = true;

        timeSlider.value = 0;

        OrbPuzzle.CurrentOrb.GetComponent<UISprite>().alpha = 1.0f;

        OrbPuzzle.CurrentOrb = null;

        OrbCursor.Clear();

        StartCoroutine(CheckMatchOrb(0.5f));
    }

    public bool CanDrag()
    {
        if (IsMatching || isDragging || HasMatched)
            return false;

        return true;
    }

    private Dictionary<OrbPoint, Enumerations.OrbColor> FindMatch(Cell[,] cells)
    {
        Dictionary<OrbPoint, Enumerations.OrbColor> stack = new Dictionary<OrbPoint, Enumerations.OrbColor>();
        for (var x = 0; x < maxWidth; x++)
        {
            for (var y = 0; y < maxHeight; y++)
            {
                var thiscell = cells[x, y];
                if (thiscell.IsEmpty) continue;
                int matchCount = 0;
                int y2 = Mathf.Min(maxHeight - 1, y + 2);
                int y1;
                int superOrbModifier = 0;
                for (y1 = y + 1; y1 <= y2; y1++)
                {
                    if (cells[x, y1].IsEmpty || thiscell.CellType != cells[x, y1].CellType) break;
                    if (cells[x, y1].IsSuperOrb) superOrbModifier += 1;
                    matchCount++;
                }
                if (matchCount >= 2)
                {
                    y1 = Mathf.Min(maxHeight - 1, y1 - 1);
                    for (var y3 = y; y3 <= y1; y3++)
                    {
                        if (!stack.ContainsKey(new OrbPoint(x, y3)))
                            stack.Add(new OrbPoint(x, y3), cells[x, y3].CellType);
                    }

                    int totalCount = matchCount + superOrbModifier + 1;
                    BattleController.Instance.OnMatchMade(cells[x, y1].CellType, totalCount);
                }
            }
        }
        for (var y = 0; y < maxHeight; y++)
        {
            for (var x = 0; x < maxWidth; x++)
            {
                var thiscell = cells[x, y];
                if (thiscell.IsEmpty) continue;
                int matchCount = 0;
                int x2 = Mathf.Min(maxWidth - 1, x + 3);
                int x1;
                for (x1 = x + 1; x1 <= x2; x1++)
                {
                    if (cells[x1, y].IsEmpty || thiscell.CellType != cells[x1, y].CellType) break;
                    matchCount++;
                }
                if (matchCount >= 2)
                {
                    x1 = Mathf.Min(maxWidth - 1, x1 - 1);
                    for (var x3 = x; x3 <= x1; x3++)
                    {
                        if (!stack.ContainsKey(new OrbPoint(x3, y)))
                            stack.Add(new OrbPoint(x3, y), cells[x3, y].CellType);
                    }

                    BattleController.Instance.OnMatchMade(cells[x1, y].CellType, matchCount + 1);
                }
            }
        }

        return stack;
    }

    public void Init()
    {
        if (orbTemplate != null)
        {
            CreateOrbGrid();

            // we do a check to make sure we don't have any matches
            while (true)
            {
                InitOrbGrid();
                Dictionary<OrbPoint, Enumerations.OrbColor> stack = FindMatch(cells);
                if (stack.Count < 1) break;
            }

            DisplayOrbs();

        }

        InitAnimationOrbs();

        StartTurn();
    }

    private void InitAnimationOrbs()
    {
        animationOrbs = new AnimatedOrbGem[7];
        for (int i = 0; i < animationOrbs.Length; i++)
        {
            var anOrb = NGUITools.AddChild(Grid, orbTemplate).gameObject.AddComponent<AnimatedOrbGem>();
            anOrb.gameObject.layer = LayerMask.NameToLayer("Board");

            animationOrbs[i] = anOrb;
            animationOrbs[i].Start();
        }
    }

    private void StartTurn()
    {
        EventDelegate.Add(endTurnButton.onClick, EndTurn);
    }

    private void EndTurn()
    {
        Debug.Log("end Turn");
        HasMatched = false;
        BattleController.Instance.OnTurnEnd();
        BattleController.Instance.OnTurnStart();
    }

    private void DisplayOrbs()
    {
        orbs = new List<OrbGem>();

        for (int y = 0; y < maxHeight; ++y)
        {
            for (int x = 0; x < maxWidth; ++x)
            {
                int type = (int)cells[x, y].CellType;

                GameObject go = NGUITools.AddChild(Grid, orbTemplate);
                Transform t = go.transform;

                t.localScale = Vector3.one * 1.0f;
                t.localPosition = new Vector3(x * 120, y * -120, 0f);

                var orb = go.GetComponent<OrbGem>();
                orb.Cell = cells[x, y];
                orb.Loc = new OrbPoint(x, y);
                orb.Puzzle = this;
                orb.UpdateSprite();
                orbs.Add(orb);
            }
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (isDragging)
        {
            if (timeSlider.value == 1.0f)
            {
                ResolveTurn();
            }
            else
            {
                var delta = Time.time - turnStartTime;
                timeSlider.value = delta / timeSecondsDrag;
            }
        }
    }

    public void StartMatching()
    {
        timeSlider.value = 0;
        turnStartTime = Time.time;
        isDragging = true;
    }

    #region BoardTargetData
    /// <summary>
    /// The board ability that is stored for use in later functions
    /// </summary>
    private CardBoardAbility _currentBoardAbility;

    /// <summary>
    /// The previous list of gems that were highlighted - used to speed up our re-color
    /// </summary>
    private List<OrbGem> _previousList = new List<OrbGem>();

    /// <summary>
    /// The currently selected list of gems , used for highlighting and populating _selectedList when the orb is clicked
    /// </summary>
    private List<OrbGem> _currentList = new List<OrbGem>();

    /// <summary>
    /// The list of orbs that are currently targeted to be modified
    /// </summary>
    private List<OrbGem> _selectedList = new List<OrbGem>();

    /// <summary>
    /// Integer representation of the number of selections that are left for this ability
    /// </summary>
    private int _selectionsLeft;

    /// <summary>
    /// Fires when the board ability is used from a card
    /// </summary>
    /// <param name="boardAbility">The board ability that is being used</param>
    public void StartBoardAbility(CardBoardAbility boardAbility)
    {
        this._currentBoardAbility = boardAbility;

        // Only start the targeting shading if it is not an automatic board target
        if (this._currentBoardAbility.NeedBoardTarget)
        {
            this.IsBoardTargeting = true;
            this._selectionsLeft = this._currentBoardAbility.NumOfSelections;
            foreach (OrbGem orb in this.orbs)
            {
                if (!orb.IsTargeted)
                {
                    orb.SetColor(Color.gray);
                }
            }

            // Selecting the upper left gem for default targeting
            OrbGem og = this.orbs.FirstOrDefault(o => o.Loc.x == 0 && o.Loc.y == 0);
            this.SetupTarget(og);
        }
        else
        {
            switch (this._currentBoardAbility.BoardAbilityTarget)
            {
                case BoardAbilityData.BoardTargetType.Color:
                    this._currentBoardAbility.ActivateAbility(this.orbs.FindAll(x => x.CellType == _currentBoardAbility.TargetColor));
                    break;
                case BoardAbilityData.BoardTargetType.RandomSelection:
                    this._currentBoardAbility.ActivateAbility(this.GetRandomOrbSelection(this._currentBoardAbility.NumOfSelections));
                    break;
                case BoardAbilityData.BoardTargetType.All:
                    this._currentBoardAbility.ActivateAbility(this.orbs);
                    break;
                default:
                    Debug.LogError("Error: Didn't recognize target type as either manual or automatic selection");
                    break;
            }

            this._currentBoardAbility = null;
        }
    }

    /// <summary>
    /// Highlights the appropriate list on the board based on the type of board ability and the current gem placement
    /// </summary>
    /// <param name="gem"></param>
    public void SetupTarget(OrbGem gem)
    {
        switch (this._currentBoardAbility.BoardAbilityTarget)
        {
            case BoardAbilityData.BoardTargetType.Grid:
                this.HighlightList(this.GetGemGrid(gem, this._currentBoardAbility.BoardGridSizeX, this._currentBoardAbility.BoardGridSizeY));
                break;
            case BoardAbilityData.BoardTargetType.Row:
                this.HighlightList(this.GetGemRow(gem));
                break;
            case BoardAbilityData.BoardTargetType.Column:
                this.HighlightList(this.GetGemColumn(gem));
                break;
            case BoardAbilityData.BoardTargetType.Selection:
                this.HighlightList(this.GetGemGrid(gem, 1, 1));
                break;
            default:
                UnityEngine.Debug.LogError("Error: Attempted to set up a target board without an appropriate target type");
                break;
        }
    }

    /// <summary>
    /// Activates the current board ability and resets the lists and targeting data
    /// </summary>
    public void ActivateAbility()
    {
        this._currentBoardAbility.ActivateAbility(this._selectedList);
        this._currentList.Clear();
        this._previousList.Clear();
        this._selectedList.Clear();
        this.IsBoardTargeting = false;

        foreach (OrbGem gem in this.orbs)
        {
            gem.ResetColor();
            gem.IsTargeted = false;
        }
    }

    /// <summary>
    /// Called when a target is select (when and OrbGem is clicked on)
    /// This will, generally, immediately call activate ability, except in the case of manual selection
    /// </summary>
    public void SelectTarget()
    {
        foreach (OrbGem gem in this._currentList)
        {
            this._selectedList.Add(gem);
        }

        if (this._currentBoardAbility.BoardAbilityTarget == BoardAbilityData.BoardTargetType.Selection)
        {
            this._selectionsLeft -= 1;
            if (this._selectionsLeft == 0)
            {
                this.ActivateAbility();
            }
        }
        else
        {
            this.ActivateAbility();
        }

    }

    /// <summary>
    /// Highlights the submitted list of gems
    /// Turns the previous list of gems gray, and then resets the color of the ones in the submitted group
    /// </summary>
    /// <param name="gems">The gems that would be selected by clicking</param>
    private void HighlightList(List<OrbGem> gems)
    {
        this._currentList = gems;
        foreach (OrbGem orb in this._previousList)
        {
            if (!orb.IsTargeted)
            {
                orb.SetColor(Color.gray);
            }
        }

        this._previousList.Clear();
        foreach (OrbGem orb in gems)
        {
            orb.ResetColor();
            this._previousList.Add(orb);
        }
    }

    /// <summary>
    /// Returns the list of gems in a grid
    /// </summary>
    /// <param name="gem">The upper left corner of the grid</param>
    /// <param name="gridX">Amount the grid extends in the X direction</param>
    /// <param name="gridY">Amount the grid extends in the Y direction</param>
    /// <returns>List of grid gems</returns>
    private List<OrbGem> GetGemGrid(OrbGem gem, int gridX, int gridY)
    {
        return this.orbs.FindAll(x =>
            x.Loc.x < gem.Loc.x + gridX
            && x.Loc.x >= gem.Loc.x
            && x.Loc.y < gem.Loc.y + gridY
            && x.Loc.y >= gem.Loc.y);
    }

    /// <summary>
    /// Returns the list of gems in the column
    /// </summary>
    /// <param name="gem">The gem in the column we wish to select</param>
    /// <returns>List of column gems</returns>
    private List<OrbGem> GetGemColumn(OrbGem gem)
    {
        return this.orbs.FindAll(x => x.Loc.x == gem.Loc.x);
    }

    /// <summary>
    /// Returns the list of gems in the row
    /// </summary>
    /// <param name="gem">The gem in the row we wish to select</param>
    /// <returns>List of row gems</returns>
    private List<OrbGem> GetGemRow(OrbGem gem)
    {
        return this.orbs.FindAll(x => x.Loc.y == gem.Loc.y);
    }

    /// <summary>
    /// Helper function for getting a random selection of gems
    /// </summary>
    /// <param name="amount">Number of selections to grab</param>
    /// <returns>List of gems to modify</returns>
    private List<OrbGem> GetRandomOrbSelection(int amount)
    {
        if (amount >= orbs.Count)
        {
            Debug.LogError("Error: Random selection count is greater than total orbs");
            return orbs;
        }

        List<OrbGem> retList = new List<OrbGem>();
        List<int> checkList = new List<int>();

        while (amount > 0)
        {
            int val = UnityEngine.Random.Range(0, orbs.Count);
            if (checkList.Contains(val))
            {
                continue;
            }

            retList.Add(orbs[val]);
            --amount;
        }

        return retList;
    }
    #endregion
}
