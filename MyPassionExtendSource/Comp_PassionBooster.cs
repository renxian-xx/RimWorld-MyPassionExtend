using System;
using System.Reflection;
using RimWorld;
using Verse;

namespace MyPassionExtendSource;

/**
 * 兴趣提升器的使用效果组件，继承自CompUseEffect
 */
public class Comp_PassionBooster : CompUseEffect {

    public Comp_PassionBooster() {
        
    }

    /**
     * 使用效果：选择使用者的一个技能，并提升其兴趣级别
     * @param usedBy 使用者
     */
    public override void DoEffect(Pawn usedBy) {
        base.DoEffect(usedBy);


        // 打开兴趣选择对话框
        Find.WindowStack.Add(new Dialog_SkillSelector(skill => {
            // 获取所选技能的兴趣等级
            Passion passion = usedBy.skills.GetSkill(skill).passion;
            
            // 级别加一，直至最高级别
            if (passion == Passion.None)
                passion = Passion.Minor;
            else if (passion == Passion.Minor)
                passion = Passion.Major;

            // 设置技能的兴趣级别
            usedBy.skills.GetSkill(skill).passion = passion;

            // 销毁使用的物品
            parent.SplitOff(1).Destroy();
        }, filter: skillDef => { // 过滤掉已被完全禁用或已是最高兴趣级别的技能
            return !usedBy.skills.GetSkill(skillDef).TotallyDisabled
                   && usedBy.skills.GetSkill(skillDef).passion != Passion.Major;
        }));
    }
}