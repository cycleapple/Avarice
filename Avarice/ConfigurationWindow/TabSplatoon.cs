using Dalamud.Interface.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avarice.ConfigurationWindow
{
    internal static class TabSplatoon
    {
        internal static void Draw()
        {
        if(ImGui.Checkbox("啟用 Splatoon IPC", ref P.config.SplatoonUnsafePixel))
            {
                WriteRequest();
            }
        ImGuiComponents.HelpMarker("依目前位置是否位於標記為「危險」的預設範圍內，變更玩家受傷判定點的色彩。");
        ImGui.ColorEdit4("危險判定點色彩", ref P.config.SplatoonPixelCol, ImGuiColorEditFlags.NoInputs);
        ImGuiComponents.HelpMarker("站在已設定的危險區域時，玩家受傷判定點將變更為此色彩。必須先啟用玩家受傷判定點功能。");
        ImGuiEx.TextWrapped("6.5 以前的預設可能無法使用此功能，因為預設作者必須在中繼資料中加入「Dangerous」屬性供 Avarice 讀取。此外，也必須在 Splatoon 的一般設定中啟用此功能。");
        }

        internal static void WriteRequest()
        {
            var array = Svc.PluginInterface.GetOrCreateData<HashSet<string>>("Splatoon.UnsafeElementRequesters", () => []);
            array.Add(Svc.PluginInterface.InternalName);
        }

        internal static bool IsUnsafe()
        {
            if(!P.config.SplatoonUnsafePixel) return false;
            if (Svc.PluginInterface.TryGetData<bool[]>("Splatoon.IsInUnsafeZone", out var data)) return data[0];
            return false;
        }
    }
}
