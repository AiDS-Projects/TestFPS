using UnrealBuildTool;

public class TestFPSServerTarget : TargetRules
{
	public TestFPSServerTarget(TargetInfo Target) : base(Target)
	{
		DefaultBuildSettings = BuildSettingsVersion.Latest;
		IncludeOrderVersion = EngineIncludeOrderVersion.Latest;
		Type = TargetType.Server;
		ExtraModuleNames.Add("TestFPS");
	}
}
