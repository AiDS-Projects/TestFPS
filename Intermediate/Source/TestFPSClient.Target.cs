using UnrealBuildTool;

public class TestFPSClientTarget : TargetRules
{
	public TestFPSClientTarget(TargetInfo Target) : base(Target)
	{
		DefaultBuildSettings = BuildSettingsVersion.Latest;
		IncludeOrderVersion = EngineIncludeOrderVersion.Latest;
		Type = TargetType.Client;
		ExtraModuleNames.Add("TestFPS");
	}
}
