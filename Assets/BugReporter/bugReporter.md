# 记录在这个项目中遇到的一些坑和bug

## 1.实现卡牌拖曳时遇到的bug。

实现思路：

在CardView中实现OnMouseDown、OnMouseDrag、OnMouseUp三个方法，这三个方法是Mono中的方法，Unity会自动进行调用。调用时机：

OnMouseDown：当用户在 [Collider](https://docs.unity.cn/cn/2020.3/ScriptReference/Collider.html) 上按下鼠标按钮时，将调用 [OnMouseDown](https://docs.unity.cn/cn/2020.3/ScriptReference/MonoBehaviour.OnMouseDown.html)。此事件将发送到具有 [Collider](https://docs.unity.cn/cn/2020.3/ScriptReference/Collider.html) 或 GUIElement 的 GameObject 的所有脚本。父对象或子对象的脚本不会收到此事件。

OnMouseDrag：当用户单击 [Collider](https://docs.unity.cn/cn/2020.3/ScriptReference/Collider.html) 并仍然按住鼠标时，将调用 OnMouseDrag。在按住鼠标按钮的情况下，每帧调用一次 OnMouseDrag。

OnMouseUp：当用户松开鼠标按钮时，将调用 OnMouseUp。请注意，即使鼠标不在按下鼠标时所在的 [Collider](https://docs.unity.cn/cn/2020.3/ScriptReference/Collider.html) 上，也会调用 OnMouseUp。 有关类似于按钮的行为，请参阅：[OnMouseUpAsButton](https://docs.unity.cn/cn/2020.3/ScriptReference/MonoBehaviour.OnMouseUpAsButton.html)。

### 存在bug的实现

```c#
private void OnMouseDown()
{
    if (!Interactions.Instance.PlayerCanInteract()) return;
    Interactions.Instance.PlayerIsDragging = true;
    Wrapper.SetActive(true);
    CardViewHover.Instance.Hide();
    cardStartPos = transform.position;
    Debug.Log("记录卡牌初始位置" + cardStartPos);
    cardStartRot = transform.rotation;
    transform.position = MouseUtil.GetMousePositionInWorldSpace(-1f);
    transform.rotation = Quaternion.Euler(0, 0, 0);
}

private void OnMouseDrag()
{
    if (!Interactions.Instance.PlayerCanInteract()) return;
    Debug.Log("正在拖动卡牌");
    transform.position = MouseUtil.GetMousePositionInWorldSpace(-1f);
}

private void OnMouseUp()
{
    if (!Interactions.Instance.PlayerCanInteract()) return;
    //if (Physics.Raycast(transform.position, Vector3.forward, out RaycastHit hit, 100f))
    //{
    //    //Play Card
    //}
    else
    {
        transform.position = cardStartPos;
        transform.rotation = cardStartRot;
        Debug.Log("放下卡牌"+ cardStartPos);
    }
    Interactions.Instance.PlayerIsDragging = false;
}
```

### bug产生原因

查看Interactions中的代码

```c#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactions : BaseManager<Interactions>
{
    private Interactions() { }

    public bool PlayerIsDragging { get; set; } = false;

    public bool PlayerCanInteract()
    {
        if (!ActionSystem.Instance.IsPerforming) return true;
        else return false;
    }

    public bool PlayerCanHover()
    {
        if(PlayerIsDragging) return false;
        else return true;
    }
}
```

PlayerCanInteract()这个方法的返回值在**行为播放结束后**可以正常交互，配合上面三个方法，乍看一眼没有问题。但是如果我在还在发牌的过程中按下了鼠标（比如要发5张牌，在发到第三张牌的时候我在第三张牌按下了鼠标），正常应该触发OnMouseDown，但是由于发牌行为没有结束，所以我们不能进入这个方法。在发牌结束后，我们的鼠标没有松开，所以不会再次执行OnMouseDown方法，但是根据上面三个方法的执行描述，**只要在一个collider中触发了OnMouseDown方法，那么一定会触发一次OnMouseDrag方法和OnMouseUp方法**。而这个时候发牌动作已经结束了，OnMouseDown方法不会执行，但是OnMouseDrag会进行，这个时候就会看到卡牌随着我们的鼠标进行移动。在我们松开鼠标后，卡牌始终被重置到（0，0,0）点，这是因为没有执行OnMouseDown方法，我们没有对开始位置和旋转进行初始化，所以始终都是原点。

### 修复思路：

在执行OnMouseDrag方法和OnMouseUp方法之前一定要执行OnMouseDown方法。所以我们必须再OnMouseDrag方法和OnMouseUp方法中添加一层判断，因为我们在OnMouseDown方法中设置了Interactions.Instance.PlayerIsDragging = true;所以我们可以在OnMouseDrag方法和OnMouseUp方法中添加这个判断，if Interactions.Instance.PlayerIsDragging = false return;

### 修改后代码

```c#
private void OnMouseDown()
{
    if (!Interactions.Instance.PlayerCanInteract()) return;
    Interactions.Instance.PlayerIsDragging = true;
    Wrapper.SetActive(true);
    CardViewHover.Instance.Hide();
    cardStartPos = transform.position;
    Debug.Log("记录卡牌初始位置" + cardStartPos);
    cardStartRot = transform.rotation;
    transform.position = MouseUtil.GetMousePositionInWorldSpace(-1f);
    transform.rotation = Quaternion.Euler(0, 0, 0);
}

private void OnMouseDrag()
{
    //这里存在的bug，如果我在发牌的过程中，鼠标已经按下了卡牌，那么在发牌完成后，
    //会直接执行OnMouseDrag方法而不会执行OnMouseDown方法，这会导致初始的位置没有记录，
    //导致下面执行OnMouseUp方法的时候使用错误的初始位置。
    //修复bug：必须要保证OnMouseDown方法执行在OnMouseDrag方法之前
    //添加判断!Interactions.Instance.PlayerIsDragging,玩家不处于拖曳状态时，直接返回
    if (!Interactions.Instance.PlayerCanInteract() || 
        !Interactions.Instance.PlayerIsDragging) return;
    Debug.Log("正在拖动卡牌");
    transform.position = MouseUtil.GetMousePositionInWorldSpace(-1f);
}

private void OnMouseUp()
{
    //这里也是同样的bug，必须要执行OnMouseDown、OnMouseDrag之后才能执行OnMouseUp方法，
    //否则会导致初始位置没有记录，导致放下卡牌时位置错误。
    if (!Interactions.Instance.PlayerCanInteract() ||
        !Interactions.Instance.PlayerIsDragging) return;
    //if (Physics.Raycast(transform.position, Vector3.forward, out RaycastHit hit, 100f))
    //{
    //    //Play Card
    //}
    else
    {
        transform.position = cardStartPos;
        transform.rotation = cardStartRot;
        Debug.Log("放下卡牌"+ cardStartPos);
    }
    Interactions.Instance.PlayerIsDragging = false;
}
```

