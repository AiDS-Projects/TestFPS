using UnrealBuildTool;

public class TestFPSEditorTarget : TargetRules
{
	public TestFPSEditorTarget(TargetInfo Target) : base(Target)
	{
		DefaultBuildSettings = BuildSettingsVersion.Latest;
		IncludeOrderVersion = EngineIncludeOrderVersion.Latest;
		Type = TargetType.Editor;
		ExtraModuleNames.Add("TestFPS");
	}
}
