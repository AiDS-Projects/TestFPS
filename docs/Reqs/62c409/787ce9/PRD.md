# PRD — 创建 C++ NPC 角色类

> 所属需求：创建一个NPC角色

## 用户故事
As a game designer, I want to create a base NPC character class in C++, So that all enemy and neutral AI characters share a unified attribute system, death handling, and perception framework, reducing duplicate code and enabling rapid NPC variant creation.

## 功能需求
1. C++ 类 ANPCCharacter，继承自 ACharacter，放置于 Source/TestFPS/Characters/NPC/ 目录
2. 基础属性：MaxHealth（float，默认 100）、CurrentHealth（float，受伤害时扣减，≤0 时触发死亡）、Armor（float，默认 0，先于 HP 吸收伤害）、WalkSpeed（float，默认 600 cm/s，继承自 ACharacter）
3. 队伍标识：ETeam 枚举（Player / Enemy / Neutral），属性 Team（BlueprintReadWrite）
4. 死亡状态：bIsDead（bool，BlueprintReadOnly），死亡时转为 Ragdoll、禁用碰撞、禁用 AI Controller，延迟 DeathDestroyDelay 秒后从世界移除
5. AIPerceptionComponent 组件：
   - Sight Config：半径 1500、丢失视野半径 2000、半视角 45°（全角 90°）、按 Team 感知过滤
   - Hearing Config：范围 3000、按 Team 感知过滤
   - Damage Config：按 Team 感知过滤
6. OnPerceptionUpdated 回调绑定，输出被感知 Actor 列表及刺激类型，供 Behavior Tree 使用
7. 受伤/治疗/死亡事件：OnTakeDamage / OnHealed / OnDeath（BlueprintNativeEvent，支持蓝图覆写）
8. 蓝图中可覆写：GetMaxWalkSpeed()、CanBeDamaged()、Die()
9. [假设] 当前阶段仅创建 C++ 类及头文件，不生成 Behavior Tree / Blackboard / AI Controller
10. [假设] 不包含武器系统、攻击系统、动画蓝图——这些由后续工单实现

## 验收标准
1. ANPCCharacter.h 和 ANPCCharacter.cpp 文件存在于 Source/TestFPS/Characters/NPC/ 目录
2. USceneComponent 层级正确：Root=UCapsuleComponent → USkeletalMeshComponent → UAIPerceptionComponent（作为非 Scene 组件附加至 Root）
3. MaxHealth / CurrentHealth / Armor / WalkSpeed 属性在编辑器中可编辑（Details 面板可见、BlueprintReadWrite），修改默认值后编译通过
4. CurrentHealth ≤ 0 时，500ms 内执行 Die() 并触发 OnDeath 事件
5. OnDeath 触发后：bIsDead == true；Capsule 碰撞禁用（SetCollisionEnabled=NoCollision）500ms 内生效；SkeletalMesh 转为 Ragdoll（SetSimulatePhysics=true）1s 内生效
6. DeathDestroyDelay 默认 10s，死亡后精确延迟后 Actor 被 Destroy()
7. UAIPerceptionComponent 配置验证：Sight 半径 1500 ±10、Hearing 范围 3000 ±10、半视角 45° ±1，三项可通过编辑器 Details 面板读取
8. 在关卡中放置 ANPCCharacter，BeginPlay 后 AIPerceptionComponent 成功注册（日志输出确认），无 Error 或 Warning
9. Armor > 0 时，Damage 先扣减 Armor 再扣减 CurrentHealth；Armor ≤ 0 时 Damage 全额扣减 HP
10. ETeam 枚举包含 Player / Enemy / Neutral 三个值，ANPCCharacter 的 Team 属性在编辑器中显示为下拉枚举
11. .h 文件中 UFUNCTION(BlueprintNativeEvent) 包括 OnTakeDamage / OnHealed / OnDeath，.cpp 有 _Implementation 默认实现
12. GetMaxWalkSpeed() 在蓝图中可覆写，覆写后 NPC 实际移动速度与之匹配（误差 ≤ 5 cm/s）
13. 编译无错误（Error=0，Warning≤2 且均为引擎侧已知 Warning）

## 边界条件（不做的事）
1. 不包含 AI Controller（AAIController）子类——仅挂载 AIPerceptionComponent
2. 不包含 Behavior Tree / Blackboard / EQS——后续工单实现
3. 不包含武器系统、攻击逻辑、伤害计算——伤害仅通过外部 TakeDamage 调用验证
4. 不包含动画蓝图（ABP_NPC）或动画资产——SkeletalMesh 使用引擎默认 Mannequin SK_Mannequin
5. 不包含 UI / 血条 / 名字显示
6. 不包含团队协作 AI（编队、支援等）或巡逻路径
7. 不包含难度分级（普通/精英/Boss）——通过 Blueprint 子类后续覆盖属性实现
8. 不包含 LOD / 剔除距离优化——默认引擎行为不变
9. [假设] 当前不实现自动回血机制，CurrentHealth 仅通过外部调用 Heal() 增加
10. [假设] 不生成 NPC 蓝图子类（BP_NPC_Base），仅创建 C++ 基类

## 资产需求线索
1. 暂无——本工单仅涉及 C++ 代码，无 2D/3D 资产需求
2. 后续工单（创建 BP_NPC 蓝图）将需要：SkeletalMesh（SK_Mannequin 复用）、ABP_NPC 动画蓝图、行为树 BT_NPC
3. 后续工单（NPC 视觉）将需要：材质 MI_NPC_Base、受伤闪红效果 NIAGARA VFX
4. 音效（脚步声、受伤音效、死亡音效）——后续音效工单统一创建
