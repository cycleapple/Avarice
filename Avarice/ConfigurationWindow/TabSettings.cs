using Avarice.ConfigurationWindow.Player;
using Dalamud.Interface.Components;
using static Avarice.ConfigurationWindow.ConfigWindow;

namespace Avarice.ConfigurationWindow;

internal static class TabSettings
{
    /*internal static Dictionary<ClassDisplayCondition, string> ClassDisplayConditionNames = new()
    {
        { ClassDisplayCondition.Do_not_display, "永不顯示" },
        { ClassDisplayCondition.Display_on_positional_jobs, "僅近戰職業" },
        { ClassDisplayCondition.Display_on_all_jobs, "所有戰鬥／製作／採集職業" },
    };*/

    static InfoBox BoxGeneral = new()
    {
        ContentsAction = delegate
        {
            // Drawing controls section
        ImGui.Text("繪製控制：");

            // Profile-specific drawing toggle with styled command hint
        ImGui.Checkbox("啟用繪製", ref P.currentProfile.DrawingEnabled);
            ImGui.SameLine();
            // Add a little spacing
            ImGui.SetCursorPosX(ImGui.GetCursorPosX() + 5);
            // Show command in a softer color as a shortcut hint
            ImGuiEx.Text(new Vector4(0.7f, 0.7f, 1.0f, 1.0f), "(/avarice draw)");
        ImGuiComponents.HelpMarker("切換所有顯示層繪製功能，也可透過 /avarice draw 指令切換。");

            // New option to only draw for positional targets
            bool prevOnlyPositional = P.config.OnlyDrawIfPositional;
        if (ImGui.Checkbox("僅對需要身位的目標顯示", ref P.config.OnlyDrawIfPositional) && prevOnlyPositional != P.config.OnlyDrawIfPositional)
            {
                // Save change to config
                Safe(() => Svc.PluginInterface.SavePluginConfig(P.config));
            }
        ImGuiComponents.HelpMarker("啟用後，只有目標需要身位攻擊時才會顯示顯示層。");

            ImGui.Separator();

            // Visual Feedback Settings
        ImGui.Text("視覺回饋設定：");
            
        ImGui.Checkbox("身位失敗時顯示回饋", ref P.currentProfile.EnableVFXFailure);
        ImGuiComponents.HelpMarker("身位攻擊失敗時，在角色上方顯示紅色叉號。");
        ImGui.Checkbox("身位成功時顯示回饋", ref P.currentProfile.EnableVFXSuccess);
        ImGuiComponents.HelpMarker("身位攻擊成功時，在角色上方顯示綠色勾號。");
            
            // Visual feedback customization
            if (P.currentProfile.EnableVFXFailure || P.currentProfile.EnableVFXSuccess)
            {
                ImGui.Indent();
                
                if (P.config.VisualFeedbackSettings == null)
                    P.config.VisualFeedbackSettings = new VisualFeedbackSettings();
                
                var settings = P.config.VisualFeedbackSettings;
                
                // Icon size
                var iconSize = settings.IconSize;
        if (ImGui.SliderFloat("圖示大小", ref iconSize, 20f, 100f))
                {
                    settings.IconSize = iconSize;
                    Safe(() => Svc.PluginInterface.SavePluginConfig(P.config));
                }
                
        ImGui.Text("色彩：");
                
                // Only show Hit color if hits are enabled
                if (P.currentProfile.EnableVFXSuccess)
                {
                    ImGui.AlignTextToFramePadding();
        ImGui.Text("成功：");
                    ImGui.SameLine();
                    var successColor = settings.SuccessColor;
                    if (ImGui.ColorEdit4("##successColor", ref successColor, ImGuiColorEditFlags.NoInputs))
                    {
                        settings.SuccessColor = successColor;
                        Safe(() => Svc.PluginInterface.SavePluginConfig(P.config));
                    }
                }
                
                // Only show Miss color if misses are enabled
                if (P.currentProfile.EnableVFXFailure)
                {
                    ImGui.AlignTextToFramePadding();
        ImGui.Text("失敗：");
                    ImGui.SameLine();
                    var failureColor = settings.FailureColor;
                    if (ImGui.ColorEdit4("##failureColor", ref failureColor, ImGuiColorEditFlags.NoInputs))
                    {
                        settings.FailureColor = failureColor;
                        Safe(() => Svc.PluginInterface.SavePluginConfig(P.config));
                    }
                }
                
                // Test buttons - only show for enabled types
                if (P.currentProfile.EnableVFXSuccess)
                {
        if (ImGui.Button("測試成功回饋"))
                        VisualFeedbackManager.TestFeedback(true);
                    if (P.currentProfile.EnableVFXFailure)
                        ImGui.SameLine();
                }
                if (P.currentProfile.EnableVFXFailure)
                {
        if (ImGui.Button("測試失敗回饋"))
                        VisualFeedbackManager.TestFeedback(false);
                }
                
                ImGui.Unindent();
            }
            
            ImGui.Separator();
        ImGui.Checkbox("身位失敗時將回饋輸出至聊天欄", ref P.currentProfile.EnableChatMessagesFailure);
        ImGuiComponents.HelpMarker("身位攻擊成功或失敗時，在聊天欄顯示對應訊息。");
        ImGui.Checkbox("身位成功時也將回饋輸出至聊天欄", ref P.currentProfile.EnableChatMessagesSuccess);
        ImGui.Checkbox("輸出戰鬥表現摘要", ref P.currentProfile.Announce);
        ImGuiComponents.HelpMarker("脫離戰鬥時，在聊天欄顯示整場戰鬥與身位成功／失敗的摘要。");
        },
            Label = "一般設定"
    };

    static InfoBox BoxCurrentSegment = new()
    {
            Label = "目前區段醒目提示設定",
        ContentsAction = delegate
        {
            //ImGui.SetNextItemWidth(SelectWidth);
        ImGui.Checkbox("目前區段醒目提示", ref P.currentProfile.EnableCurrentPie);
            //if (P.currentProfile.EnableCurrentPie)
            {
                ImGui.SameLine();
                ImGui.SetNextItemWidth(150f);
                ImGuiEx.EnumCombo($"##cb1", ref P.currentProfile.CurrentPieSettings.DisplayCondition);
                ImGuiEx.InvisibleButton(3);
                ImGui.SameLine();
        ImGuiEx.Text("背面色彩：");
                ImGui.SameLine();
                ImGui.ColorEdit4($"##ca1", ref P.currentProfile.CurrentPieSettings.Fill, ImGuiColorEditFlags.NoInputs);
                ImGuiEx.InvisibleButton(3);
                ImGui.SameLine();
        ImGuiEx.Text("側面色彩：");
                ImGui.SameLine();
                ImGui.ColorEdit4($"##ca1f", ref P.currentProfile.CurrentPieSettingsFlank.Fill, ImGuiColorEditFlags.NoInputs);
            }
        }
    };

    static InfoBox BoxFront = new()
    {
            Label = "正面區段指示器",
        ContentsAction = delegate
        {
            ImGui.SetNextItemWidth(200f);
        ImGui.Checkbox("正面區段指示器", ref P.currentProfile.EnableFrontSegment);
            //if (P.currentProfile.EnableFrontSegment)
            {
                ImGui.SameLine();
                ImGui.SetNextItemWidth(150f);
                ImGuiEx.EnumCombo($"##cb2", ref P.currentProfile.FrontSegmentIndicator.DisplayCondition);
                ImGuiEx.InvisibleButton(3);
                ImGui.SameLine();
        ImGuiEx.Text("色彩：");
                ImGui.SameLine();
                ImGui.ColorEdit4($"##ca2", ref P.currentProfile.FrontSegmentIndicator.Fill, ImGuiColorEditFlags.NoInputs);
            }
        }
    };

    static InfoBox BoxMeleeRing = new()
    {
            Label = "敵人距離指示器",
        ContentsAction = delegate
        {
            ImGui.SetNextItemWidth(SelectWidth);
        ImGui.Checkbox("敵人距離指示器", ref P.currentProfile.EnableMaxMeleeRing);
            //if (P.currentProfile.EnableMaxMeleeRing)
            {
                ImGui.SameLine();
                ImGui.SetNextItemWidth(150f);
                ImGuiEx.EnumCombo($"##mrd", ref P.currentProfile.MaxMeleeSettingsN.DisplayCondition);
                ImGuiEx.InvisibleButton(3);
                ImGui.SameLine();
                ImGuiEx.Text("Radius 3y:");
                ImGui.SameLine();
                ImGui.Checkbox("##r3", ref P.currentProfile.Radius3);
                ImGui.SameLine();
                ImGuiEx.Text("Radius 2y:");
                ImGui.SameLine();
                ImGui.Checkbox("##r2", ref P.currentProfile.Radius2);
                ImGuiEx.InvisibleButton(3);
                ImGui.SameLine();
        ImGuiEx.Text("分隔線：");
                ImGui.SameLine();
                ImGui.Checkbox("##lines", ref P.currentProfile.DrawLines);
                DrawUnfilledSettings("mr", ref P.currentProfile.MaxMeleeSettingsN, true);
            }
        }
    };

    static InfoBox BoxAnticipation = new()
    {
            Label = "身位預判設定",
        ContentsAction = delegate
        {
            ImGui.SetNextItemWidth(SelectWidth);
        ImGui.Checkbox("身位預判", ref P.currentProfile.EnableAnticipatedPie);
            //if(P.currentProfile.EnableAnticipatedPie)
            {
                ImGui.SameLine();
                ImGui.SetNextItemWidth(150f);
                ImGuiEx.EnumCombo($"##adt", ref P.currentProfile.AnticipatedPieSettings.DisplayCondition);
                ImGuiEx.InvisibleButton(3);
                ImGui.SameLine();
        ImGuiEx.Text("色彩：");
                ImGui.SameLine();
                ImGui.ColorEdit4($"##ca3", ref P.currentProfile.AnticipatedPieSettings.Fill, ImGuiColorEditFlags.NoInputs);
                ImGuiEx.InvisibleButton(3);
                ImGui.SameLine();
        ImGui.Checkbox("真北作用中時停用", ref P.currentProfile.AnticipatedDisableTrueNorth);
            }
        }
    };


    static InfoBox BoxHitboxSettings = new()
    {
            Label = "近戰距離選項",
        ContentsAction = delegate
        {
            ImGui.SetNextItemWidth(50f);
        ImGui.DragFloat("能力技／戰技距離", ref P.currentProfile.MeleeSkillAtk, 0.01f, 0.1f, 10f);
            ImGuiEx.InvisibleButton(3);
            ImGui.SameLine();
        ImGui.Checkbox("包含碰撞箱##1", ref P.currentProfile.MeleeSkillIncludeHitbox);
            ImGui.SetNextItemWidth(50f);
        ImGui.DragFloat("近戰自動攻擊距離", ref P.currentProfile.MeleeAutoAtk, 0.01f, 0.1f, 10f);
            ImGuiEx.InvisibleButton(3);
            ImGui.SameLine();
        ImGui.Checkbox("包含碰撞箱##2", ref P.currentProfile.MeleeAutoIncludeHitbox);
        }
    };

    static InfoBox BoxPlayerDot = new()
    {
            Label = "玩家受傷判定點",
        ContentsAction = delegate
        {
            ImGuiEx.TextWrapped("Displays the player's damage hitbox, which in reality is a small pixel between your feet. " +
                "Whilst you can customize the size of this feature with the \"Thickness\" " +
                "parameter, it's recommended to leave it at the default value.");
            ImGui.SetNextItemWidth(SelectWidth);
        ImGui.Checkbox("玩家受傷判定點", ref P.currentProfile.EnablePlayerDot);
            //if (P.currentProfile.EnablePlayerDot)
            {
                DrawUnfilledSettings("dot", ref P.currentProfile.PlayerDotSettings);
            }
        }
    };

    static InfoBox BoxPlayerDotOthers = new()
    {
            Label = "其他角色受傷判定點",
        ContentsAction = delegate
        {
        ImGui.Checkbox("小隊成員", ref P.currentProfile.PartyDot);
            if (P.currentProfile.PartyDot)
            {
                DrawUnfilledSettings("dotp", ref P.currentProfile.PartyDotSettings);
            }
        ImGui.Checkbox("所有玩家", ref P.currentProfile.AllDot);
            if (P.currentProfile.AllDot)
            {
                DrawUnfilledSettings("dota", ref P.currentProfile.AllDotSettings);
            }
        }
    };

    static InfoBox BoxPlayerHitbox = new()
    {
            Label = "玩家攻擊距離外框",
        ContentsAction = delegate
        {
            ImGuiEx.TextWrapped("Displays a ring around the player character, allowing you to see the reach of auto attacks.");
            ImGui.SetNextItemWidth(SelectWidth);
        ImGui.Checkbox("玩家攻擊距離外框", ref P.currentProfile.EnablePlayerRing);
            //if (P.currentProfile.EnablePlayerRing)
            {
                DrawUnfilledSettings("hitbox", ref P.currentProfile.PlayerRingSettings);
            }
        }
    };

    internal static void Draw()
    {
        ImGuiEx.EzTabBar("settingsbar2",
            ("玩家", delegate
            {
                ImGuiHelpers.ScaledDummy(5f);
                BoxGeneral.DrawStretched();
                BoxPlayerDot.DrawStretched();
                BoxCompass.Draw();
                BoxPlayerHitbox.DrawStretched();
                BoxPlayerDotOthers.DrawStretched();
                //ImGui.Checkbox("Debug Mode", ref P.currentProfile.Debug);
                //ImGuiComponents.HelpMarker("Displays the debug menu tab, for development purposes.");
            }, null, true),
            ("目標", delegate
            {
                ImGuiHelpers.ScaledDummy(5f);
                BoxCurrentSegment.DrawStretched();
                BoxFront.DrawStretched();
                BoxMeleeRing.DrawStretched();
                BoxHitboxSettings.DrawStretched();
            }, null, true),
            ("Duty Centralisation", TabTank.Draw, null, true),
            (Svc.PluginInterface.TryGetData<bool[]>("Splatoon.IsInUnsafeZone", out _) ? "Splatoon" : null, TabSplatoon.Draw, null, true)
        );
    }
}
