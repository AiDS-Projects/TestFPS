using UnrealBuildTool;

public class TestFPSTarget : TargetRules
{
	public TestFPSTarget(TargetInfo Target) : base(Target)
	{
		DefaultBuildSettings = BuildSettingsVersion.Latest;
		IncludeOrderVersion = EngineIncludeOrderVersion.Latest;
		Type = TargetType.Game;
		ExtraModuleNames.Add("TestFPS");
	}
}
