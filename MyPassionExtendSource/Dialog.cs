using UnityEngine;
using Verse;

namespace MyPassionExtendSource;

/**
 * 一个基础的对话框类，描述对话框的基本结构和行为，具体逻辑由子类实现
 */
public class Dialog : Window {
    /**
     * 对话框的标题高度
     */
    private float headerHeight = 32f;
    /**
     * 内容区域的内边距
     */
    private float wrapperPadding = 16f;

    /**
     * 内容区域的滚动高度
     */
    protected float scrollHeight;
    /**
     * 内容区域的滚动位置
     */
    protected Vector2 scrollPosition;

    /**
     * 获取对话框的标题
     */
    public virtual string GetTitle() {
        return null;
    }

    /**
     * 检查是否可以确认
     */
    public virtual bool CanConfirm() {
        return false;
    }

    /**
     * 取消当前选择
     */
    public virtual void OnCancel() {
    }

    /**
     * 确认当前选择
     */
    public virtual void OnConfirm() {
    }

    /**
     * 在对话框内容区域绘制具体的内容
     * @param inRect 内容区域的矩形
     */
    public virtual void DoDialogContents(Rect inRect) {
    }

    /**
     * 实现Window类的DoWindowContents方法，绘制对话框的窗口内容
     */
    public override void DoWindowContents(Rect rect) {
        // 设置字体大小
        Text.Font = GameFont.Medium;
        // 绘制标题
        Widgets.Label(rect, GetTitle());

        // 调整矩形区域，留出标题和按钮空间
        rect.yMin += headerHeight;
        rect.yMax -= CloseButSize.y;

        // 创建一个矩形区域用于包裹内容
        Rect wrapper = new(0f, headerHeight, rect.width, rect.height - 6f);
        wrapper.xMin += wrapperPadding;

        GUI.BeginGroup(wrapper);

        // 绘制滚动视图
        Rect content = new(0f, 0f, wrapper.width - wrapperPadding, scrollHeight);
        Widgets.BeginScrollView(wrapper.AtZero(), ref scrollPosition, content);
        DoDialogContents(content with { y = scrollPosition.y, height = wrapper.height });
        Widgets.EndScrollView();

        GUI.EndGroup();

        // 添加关闭按钮
        if (Widgets.ButtonText(new Rect(rect.xMax - CloseButSize.x, rect.yMax, CloseButSize.x, CloseButSize.y),
                "Close".Translate())) {
            OnCancel();
            Close();
        }

        // 添加确认按钮
        if (Widgets.ButtonText(
                new Rect(rect.xMax - CloseButSize.x * 2 - 6f, rect.yMax, CloseButSize.x, CloseButSize.y),
                "Confirm".Translate(), active: CanConfirm())) {
            OnConfirm();
            Close();
        }

    }
}