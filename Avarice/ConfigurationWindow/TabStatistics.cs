using Lumina.Excel.Sheets;

namespace Avarice.ConfigurationWindow;

internal static class TabStatistics
{
    static InfoBox StatsGlobal = new()
    {
        Label = "總計統計",
        ContentsAction = delegate
        {
            ImGui.BeginTable("##table", 4, ImGuiTableFlags.RowBg | ImGuiTableFlags.SizingFixedFit, new Vector2(ImGui.GetContentRegionAvail().X - 20, 0));
            ImGui.TableSetupColumn(" Job ", ImGuiTableColumnFlags.WidthStretch);
            ImGui.TableSetupColumn(" Hits ");
            ImGui.TableSetupColumn(" Total ");
        ImGui.TableSetupColumn("成功率  ");
            ImGui.TableHeadersRow();
            Stats total = new();
            foreach(var x in P.currentProfile.Stats)
            {
                DrawStatsRow(x.Key, x.Value);
                total.Hits += x.Value.Hits;
                total.Missed += x.Value.Missed;
            }
        if(total.Hits > 0 || total.Missed > 0) DrawStatsRow(0, total, "總計：");
            ImGui.EndTable();
        if(ImGui.SmallButton("清除資料（按住 Shift+Ctrl）"))
            {
                if(ImGui.GetIO().KeyShift && ImGui.GetIO().KeyCtrl)
                {
                    P.currentProfile.Stats = new();
                }
            }
        }
    };

    static void DrawStatsRow(uint job, Stats x, string colName = null)
    {
        ImGui.TableNextRow();
        ImGui.TableNextColumn();
        ImGuiEx.Text(colName ?? Svc.Data.GetExcelSheet<ClassJob>().GetRowOrDefault(job)?.NameEnglish.ToString());
        ImGui.TableNextColumn();
        ImGuiEx.Text($"{x.Hits}");
        ImGui.TableNextColumn();
        var total = x.Hits + x.Missed;
        ImGuiEx.Text($"{total}");
        ImGui.TableNextColumn();
        var success = (int)(100f * (float)x.Hits / (float)total);
        ImGuiEx.Text(ImGuiEx.GetParsedColor(success), $"{success}%");
    }

    static InfoBox StatsCurrent = new()
    {
        Label = "目前戰鬥",
        ContentsAction = delegate
        {
            var x = P.currentProfile.CurrentEncounterStats;
            var total = x.Hits + x.Missed;
            if (total == 0)
            {
            ImGuiEx.Text("沒有資料");
            }
            else
            {
                var success = (int)(100f * (float)x.Hits / (float)total);
            ImGuiEx.Text($"命中：{x.Hits}/{total} - ");
                ImGui.SameLine(0, 0);
                ImGuiEx.Text(ImGuiEx.GetParsedColor(success), $"{success}%");
            if (ImGui.SmallButton("清除資料"))
                {
                    P.currentProfile.CurrentEncounterStats = new();
                }
                ImGui.SameLine();
                if (P.currentProfile.CurrentEncounterStats.Finished)
                {
            StatsCurrent.Label = "最近一次戰鬥";
            ImGuiEx.Text(ImGuiColors.DalamudRed, "下次使用身位技能時將重設統計");
                }
                else
                {
            StatsCurrent.Label = "目前戰鬥";
            if (ImGui.SmallButton("結算"))
                    {
                        P.currentProfile.CurrentEncounterStats.Finished = true;
                    }
                }
            }
        }
    };

    internal static void Draw()
    {
        ImGuiHelpers.ScaledDummy(5f);
        StatsGlobal.DrawStretched();
        StatsCurrent.DrawStretched();
    }
}
