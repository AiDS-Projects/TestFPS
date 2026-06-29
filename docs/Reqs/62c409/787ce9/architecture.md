# 架构设计 - 创建 C++ NPC 角色类

## 架构模式
Incremental — single new C++ class (ANPCCharacter) extending ACharacter with perception and attribute systems. No existing code to refactor; project is a blank UE5.7 template.

## 技术栈

- **engine**: Unreal Engine 5.7
- **language**: C++ (UE5 C++ conventions: UCLASS/USTRUCT/UPROPERTY/UFUNCTION macros)
- **build_system**: UnrealBuildTool (ModuleRules .Build.cs)
- **modules_added**: AIModule, NavigationSystem
- **plugins_enabled**: EnhancedInput, StateTree, GameplayStateTree
- **target_platform**: Win64 (Development / Editor)

## 模块设计

### 
职责: 

## 关键决策
- ETeam as standalone header (ETeam.h) rather than nested in NPCCharacter.h — allows reuse by future AIController, GameMode, weapon system without header coupling.
- AIModule added as PublicDependencyModuleNames (not Private) — AIPerceptionComponent is exposed in public header via UPROPERTY, so consumers need the module symbols.
- Die() separated from OnDeath() — Die() handles state machine (ragdoll, collision, timer), OnDeath is BlueprintNativeEvent for designer override (VFX/sound). Prevents Blueprint from accidentally skipping critical C++ cleanup.
- Armor absorbs damage before health (not as damage multiplier/reduction) — matches PRD spec and is the simplest damage pipeline; avoids introducing damage formula complexity in base class.
- Perception config values hardcoded in constructor defaults (not .ini) — matches UE best practice of defining component defaults in C++ constructor; designers can override per-instance in Blueprint/Details panel.
- No AIController created in this scope — PRD explicitly excludes it. AIPerceptionComponent is attached directly to ANPCCharacter (valid UE pattern; future AIController can reference it).
- DeathDestroyDelay default 10.0f seconds — matches prototype and PRD spec, gives enough time for death animation/ragdoll to play before cleanup.
