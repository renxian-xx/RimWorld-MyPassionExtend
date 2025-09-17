using System;
using RimWorld;
using UnityEngine;
using Verse;

namespace MyPassionExtendSource;

/**
 * 用于选择人物的技能的对话框
 */
public class Dialog_SkillSelector : Dialog {
    /**
     * 所选技能
     */
    private SkillDef selectedSkill;
    /**
     * 确认选择的回调
     */
    private Action<SkillDef> confirmed;
    /**
     * 取消选择的回调
     */
    private Action cancelled;
    /**
     * 过滤技能的函数
     * 如果为null，则不过滤
     */
    private Func<SkillDef, bool> filter;

    public Dialog_SkillSelector(Action<SkillDef> confirmed = null, Action cancelled = null, Func<SkillDef, bool> filter = null) {
        this.confirmed = confirmed;
        this.cancelled = cancelled;
        this.filter = filter;
        // 打开对话框时，暂停游戏
        forcePause = true;
    }

    /**
     * 获取对话框的标题
     */
    public override string GetTitle() {
        return "SelectSkill".Translate(); // 进行翻译，Languages下的Keyed中
    }

    /**
     * 没有选择技能时，不能确认
     */
    public override bool CanConfirm() {
        return selectedSkill != null;
    }

    /**
     * 确认时调用回调函数，并传入所选技能，恢复游戏
     */
    public override void OnConfirm() {
        confirmed?.Invoke(selectedSkill);
        forcePause = false;
    }

    /**
     * 取消时调用回调函数，恢复游戏
     */
    public override void OnCancel() {
        cancelled?.Invoke();
        forcePause = false;
    }

    /**
     * 在对话框内容区域绘制具体的内容
     */
    public override void DoDialogContents(Rect inRect) {
        Text.Font = GameFont.Small;
        float y = 0f;
        // 遍历所有技能定义
        foreach (SkillDef skill in DefDatabase<SkillDef>.AllDefsListForReading) {
            // 进行过滤
            if (filter != null && !filter(skill)) {
                continue;
            }

            // 绘制技能的选择按钮
            if (Widgets.RadioButton(new Vector2(0f, y), skill == selectedSkill)) {
                selectedSkill = skill;
            }

            // 绘制技能的标签
            Widgets.Label(new Rect(30f, y, inRect.width * 0.8f, 30f), skill.LabelCap);
            y += 30f;
        }


        if (Event.current.type == EventType.Layout)
            scrollHeight = y;
    }

}