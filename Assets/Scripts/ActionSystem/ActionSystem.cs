using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// 定义反应触发时机的枚举类型
public enum ReactionTiming
{
    PRE,
    POST
}

// 单例模式的ActionSystem类，用于管理游戏中的动作
public class ActionSystem : SingletonMono<ActionSystem>
{
    // 存储当前动作的反应列表
    private List<GameAction> reactions = new List<GameAction>();

    // 表示是否正在执行某个动作
    public bool IsPerforming { get; private set; } = false;
    // 存储在动作执行前的订阅者列表
    private static Dictionary<Type, List<Action<GameAction>>> preSubs = new();
    // 存储在动作执行后的订阅者列表
    private static Dictionary<Type, Func<GameAction, IEnumerator>> performers = new();
    // 存储在动作执行后的订阅者列表
    private static Dictionary<Type, List<Action<GameAction>>> postSubs = new();

    // 执行指定的动作，并在动作完成后调用回调函数
    public void Perform(GameAction action, System.Action OnPerformFinished = null)
    {
        // 如果已经在执行动作，则输出警告信息并返回
        if (IsPerforming)
        {
            Debug.LogWarning("ActionSystem is already performing an action.");
            return;
        }
        // 设置IsPerforming为true，表示开始执行动作
        IsPerforming = true;
        // 启动协程执行动作流程，并在流程完成后设置IsPerforming为false并调用回调函数
        StartCoroutine(Flow(action, () =>
        {
            IsPerforming = false;
            OnPerformFinished?.Invoke();
        }));
    }

    // 协程方法，用于执行动作的完整流程
    private IEnumerator Flow(GameAction action, Action OnFlowFinished = null)
    {
        // 设置反应列表为动作执行前的反应列表，并执行这些反应
        reactions = action.PreReactions;
        PerformSubscribers(action, preSubs);
        yield return PerformReactions();

        // 设置反应列表为动作执行中的反应列表，并执行这些反应
        reactions = action.PerformReactions;
        yield return PerformPerformer(action);
        yield return PerformReactions();

        // 设置反应列表为动作执行后的反应列表，并执行这些反应
        reactions = action.PostReactions;
        PerformSubscribers(action, postSubs);
        yield return PerformReactions();

        // 执行流程完成后调用回调函数
        OnFlowFinished?.Invoke();
    }

    // 协程方法，用于执行指定类型的动作
    private IEnumerator PerformPerformer(GameAction action)
    {
        // 获取动作的类型
        Type type = action.GetType();
        // 如果存在该类型的执行者，则调用执行者
        if (performers.ContainsKey(type))
        {
            yield return performers[type](action);
        }
    }

    // 执行指定类型动作的订阅者
    private void PerformSubscribers(GameAction action, Dictionary<Type, List<Action<GameAction>>> subs)
    {
        // 获取动作的类型
        Type type = action.GetType();
        // 如果存在该类型的订阅者，则调用这些订阅者
        if (subs.ContainsKey(type))
        {
            foreach (var sub in subs[type])
            {
                sub(action);
            }
        }
    }

    // 协程方法，用于执行反应列表中的所有反应
    private IEnumerator PerformReactions()
    {
        // 遍历反应列表并逐个执行
        foreach (var reaction in reactions)
        {
            yield return Flow(reaction);
        }
    }

    // 为指定类型的动作附加一个执行者
    public static void AttachPerformer<T>(Func<T, IEnumerator> performer) where T : GameAction
    {
        // 获取动作类型的Type对象
        Type type = typeof(T);
        // 包装执行者方法，使其可以接受GameAction类型参数
        IEnumerator wrappedPerformer(GameAction action) => performer((T)action);
        // 如果该类型的执行者已经存在，则更新；否则添加新的执行者
        if (performers.ContainsKey(type))
        {
            performers[type] = wrappedPerformer;
        }
        else
        {
            performers.Add(type, wrappedPerformer);
        }
    }

    // 移除指定类型的动作的执行者
    public static void DetachPerformer<T>() where T : GameAction
    {
        // 获取动作类型的Type对象
        Type type = typeof(T);
        // 如果存在该类型的执行者，则移除
        if (performers.ContainsKey(type))
        {
            performers.Remove(type);
        }
    }

    // 订阅指定类型的动作的反应
    public static void SubscribeReaction<T>(Action<T> reaction, ReactionTiming timing) where T : GameAction
    {
        // 根据触发时机选择合适的订阅者列表
        Dictionary<Type, List<Action<GameAction>>> subs = (timing == ReactionTiming.PRE ? preSubs : postSubs);
        // 包装反应方法，使其可以接受GameAction类型参数
        void wrappedReaction(GameAction action)
        {
            reaction((T)action);
        }
        // 如果该类型的订阅者列表已经存在，则添加新的反应；否则创建新的订阅者列表并添加反应
        if (subs.ContainsKey(typeof(T)))
        {
            subs[typeof(T)].Add(wrappedReaction);
        }
        else
        {
            subs.Add(typeof(T), new List<Action<GameAction>> { wrappedReaction });
        }
    }

    // 取消订阅指定类型的动作的反应
    public static void UnsubscribeReaction<T>(Action<T> reaction, ReactionTiming timing) where T : GameAction
    {
        // 根据触发时机选择合适的订阅者列表
        Dictionary<Type, List<Action<GameAction>>> subs = timing == ReactionTiming.PRE ? preSubs : postSubs;
        // 如果存在该类型的订阅者列表，则移除指定的反应
        if (subs.ContainsKey(typeof(T)))
        {
            void wrappedReaction(GameAction action)
            {
                reaction((T)action);
            }
            subs[typeof(T)].Remove(wrappedReaction);
        }
    }
}
