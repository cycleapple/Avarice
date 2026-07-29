using static Avarice.ConfigurationWindow.ConfigWindow;

namespace Avarice.ConfigurationWindow;

internal static unsafe class TabAnticipation
{
	private static readonly InfoBox BoxAnticipated = new()
	{
        Label = "預判區段指示器",
		ContentsAction = delegate
		{
			ImGui.SetNextItemWidth(SelectWidth);
        _ = ImGui.Checkbox("預判區段指示器", ref P.currentProfile.EnableAnticipatedPie);
			//if (P.currentPrfile.EnableAnticipatedPie)
			{
				ImGui.PushID("AnticipatedPieSettings");
				ImGui.SameLine();
				ImGui.SetNextItemWidth(150f);
				_ = ImGuiEx.EnumCombo($"##1", ref P.currentProfile.AnticipatedPieSettings.DisplayCondition);
        ImGuiEx.TextV("背面：");

				//DrawUnfilledSettings("", ref P.currentProfile.AnticipatedPieSettings);

				ImGuiEx.InvisibleButton(3);
				ImGui.SameLine();
				P.currentProfile.AnticipatedPieSettings.Fill = Vector4.Zero;
        ImGuiEx.Text("粗細：");
				ImGui.SameLine();
				ImGui.SetNextItemWidth(50f);
				_ = ImGui.DragFloat($"##2", ref P.currentProfile.AnticipatedPieSettings.Thickness, 0.1f, 0f, 10f);
				ImGui.SameLine();
				ImGuiEx.Text($"  Color:");
				ImGui.SameLine();
				_ = ImGui.ColorEdit4($"##3", ref P.currentProfile.AnticipatedPieSettings.Color, ImGuiColorEditFlags.NoInputs);
				ImGui.PopID();

        ImGuiEx.TextV("側面：");
				ImGuiEx.InvisibleButton(3);
				ImGui.SameLine();

				//DrawUnfilledSettings("AnticipatedPieSettingsFlank", ref P.currentProfile.AnticipatedPieSettingsFlank, false);
				ImGui.PushID("AnticipatedPieSettingsFlank");
				P.currentProfile.AnticipatedPieSettingsFlank.Fill = Vector4.Zero;
        ImGuiEx.Text("粗細：");
				ImGui.SameLine();
				ImGui.SetNextItemWidth(50f);
				_ = ImGui.DragFloat($"##2", ref P.currentProfile.AnticipatedPieSettingsFlank.Thickness, 0.1f, 0f, 10f);
				ImGui.SameLine();
				ImGuiEx.Text($"  Color:");
				ImGui.SameLine();
				_ = ImGui.ColorEdit4($"##3", ref P.currentProfile.AnticipatedPieSettingsFlank.Color, ImGuiColorEditFlags.NoInputs);
				ImGui.PopID();

				P.currentProfile.AnticipatedPieSettingsFlank.DisplayCondition = P.currentProfile.AnticipatedPieSettings.DisplayCondition;
        _ = ImGui.Checkbox("真北作用中時停用", ref P.currentProfile.AnticipatedDisableTrueNorth);
			}
		}
	};

	private static readonly InfoBox BoxMnk = new()
	{
            Label = "武僧",
		ContentsAction = delegate
		{

		}
	};

	private static readonly InfoBox BoxDrg = new()
	{
            Label = "龍騎士",
		ContentsAction = delegate
		{

		}
	};

	private static readonly InfoBox BoxNin = new()
	{
            Label = "忍者",
		ContentsAction = delegate
		{
        _ = ImGui.Checkbox("攻其不備可用時預判背面身位", ref P.currentProfile.TrickAttack);
		}
	};

	private static readonly InfoBox BoxSam = new()
	{
            Label = "武士",
		ContentsAction = delegate
		{
        _ = ImGui.Checkbox("明鏡止水作用中時停用預判", ref P.currentProfile.Meikyo);
		}
	};

	private static readonly InfoBox BoxRpr = new()
	{
            Label = "奪魂者",
		ContentsAction = delegate
		{
        ImGui.Text("優先預判背面或側面？");
        _ = ImGui.RadioButton("背面", ref P.currentProfile.Reaper, 0);
        _ = ImGui.RadioButton("側面", ref P.currentProfile.Reaper, 1);
		}
	};

	private static readonly InfoBox BoxVpr = new()
	{
            Label = "蝰蛇劍士",
		ContentsAction = delegate
		{

		}
	};

	private static readonly InfoBox BoxRotationSolver = new() {
            Label = "Rotation Solver 整合",
		ContentsAction = delegate
		{
        _ = ImGui.Checkbox("使用 Rotation Solver 預判身位", ref P.currentProfile.UseRotationSolver);
		}
	};

	internal static void Draw()
	{
		ImGuiHelpers.ScaledDummy(5f);
		BoxAnticipated.DrawStretched();
		//BoxMnk.DrawStretched();
		//BoxDrg.DrawStretched();
		BoxNin.DrawStretched();
		BoxSam.DrawStretched();
		BoxRpr.DrawStretched();
		//BoxVpr.DrawStretched();

		if (P.currentProfile.UseRotationSolver || P.RotationSolverWatcher.Available)
		{
			BoxRotationSolver.DrawStretched();
		}
	}
}
