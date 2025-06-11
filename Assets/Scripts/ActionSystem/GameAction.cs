using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAction
{
    public List<GameAction> PreReactions { get; private set; } = new List<GameAction>();
    public List<GameAction> PerformReactions { get; private set; } = new List<GameAction>();
    public List<GameAction> PostReactions { get; private set; } = new List<GameAction>();

}
