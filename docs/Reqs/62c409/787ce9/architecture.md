# 架构设计 - 创建 C++ NPC 角色类

## 架构模式
incremental

## 技术栈

- **engine**: Unreal Engine 5.7
- **language**: C++
- **template_base**: TP_Blank
- **build_system**: UnrealBuildTool
- **plugins**: EnhancedInput, StateTree, GameplayStateTree
- **existing_modules**: TestFPS (module class: TP_Blank)
- **new_dependencies**: AIModule, UMG

## 模块设计

### 
职责: 

### 
职责: 

### 
职责: 

## 关键决策
- {'decision': 'Inherit ANPCCharacter from ACharacter rather than APawn', 'reason': 'ACharacter provides built-in CapsuleComponent, CharacterMovementComponent, and SkeletalMeshComponent — all necessary for an NPC that can walk and receive damage. Prototype WalkSpeed maps directly to UCharacterMovementComponent::MaxWalkSpeed.', 'tradeoff': 'Slightly heavier than bare APawn, but eliminates boilerplate component setup.'}
- {'decision': 'IsAlive is a derived getter (BlueprintCallable), not a stored UPROPERTY', 'reason': 'Prototype pseudo-code shows IsAlive as a function of CurrentHP. Avoids desync bugs where IsAlive != (CurrentHP > 0). Matches Engine pattern (AActor::IsPendingKill).', 'tradeoff': 'Slightly more CPU per query (trivial float compare), eliminates state synchronization bug class.'}
- {'decision': 'Use UAIPerceptionComponent with sight sense only (no hearing/damage sense yet)', 'reason': "Prototype specifies SightRadius=1500cm and LoseSightRadius=1600cm. These are the minimum for 'can AIControl' requirement. Additional senses can be added incrementally without breaking existing config.", 'tradeoff': 'Less awareness than full AI, but simpler to test and validate the core loop first.'}
- {'decision': 'Health bar uses UWidgetComponent with a separate BP_WBP_HealthBar widget (created in Content)', 'reason': 'Decouples C++ logic from visual design. Blueprint designer can iterate the widget independently. UWidgetComponent::SetWidget() links at BeginPlay.', 'tradeoff': "Requires a corresponding Blueprint widget asset to be created. Health bar will show 'missing widget' placeholder until that asset exists."}
- {'decision': 'OnDeathDelegate is BlueprintAssignable multicast delegate, not C++ virtual method override only', 'reason': "Maps to prototype's OnDeath() behavior. Allows Blueprint-level reactions (VFX, audio, quest triggers, loot spawning) without subclassing C++. Matches UE event-driven pattern.", 'tradeoff': 'Dynamic multicast has slight overhead vs direct virtual call, acceptable for infrequent event (once per NPC lifetime).'}
- {'decision': 'Add AIModule and UMG to Build.cs PublicDependencyModuleNames', 'reason': 'Public because ANPCCharacter.h includes UAIPerceptionComponent and UWidgetComponent types. These are standard UE modules available without extra plugin enablement.', 'tradeoff': 'Marginally increases module dependency footprint. Both modules are core UE and always loaded, so no runtime cost.'}
- {'decision': 'Dedicated LogNPC log category (DEFINE_LOG_CATEGORY_STATIC) instead of LogTemp', 'reason': 'Per UE C++ coding standard: LogTemp is forbidden for production code. LogNPC allows filtering NPC-specific logs in Output Log.', 'tradeoff': 'Trivial additional line in .cpp, zero runtime cost.'}
