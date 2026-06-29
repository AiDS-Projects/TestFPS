# PRD — 创建 C++ NPC 角色类

> 所属需求：创建一个NPC角色

## 用户故事
As a game designer, I want to create a C++ NPC character base class (ANPCCharacter) so that all NPC types in the game share consistent base attributes, AI perception capabilities, and can be reused and extended via Blueprint.

## 功能需求
1. 创建继承自 ACharacter 的 ANPCCharacter C++ 类
2. 包含基础生命属性：CurrentHP、MaxHP [假设: MaxHP=100]
3. 包含移动速度属性：WalkSpeed [假设: WalkSpeed=400 cm/s]
4. 集成 UAIPerceptionComponent 感知组件，默认开启视觉感知 [假设: 视觉半径=1500 cm, 失去视觉半径=1600 cm]
5. 所有基础属性暴露为 BlueprintReadWrite，供策划按 NPC 类型调整
6. 包含 TakeDamage 事件处理，接受 float/AActor*/FDamageEvent 参数，扣减 CurrentHP
7. 包含 OnDeath 事件，当 CurrentHP ≤ 0 时自动触发
8. 包含 IsAlive 布尔标记，每次 HP 变化时更新
9. 暴露血条相关属性（是否显示 HealthBar、Z 轴偏移量等）[假设: 血条偏移 Z=120 cm]
10. 支持被 AIController 接管并响应基础移动指令

## 验收标准
- [ ] ANPCCharacter 类在 UE5.3+ 环境下编译成功，零 Warning
- [ ] 类在 Create Blueprint 对话框中作为有效父类出现
- [ ] 基于 ANPCCharacter 创建的蓝图子类可拖入关卡，不崩溃
- [ ] 默认 MaxHP = 100，可在蓝图 Details 面板中读取验证
- [ ] 默认 WalkSpeed = 400 cm/s，通过 GetCharacterMovement()->MaxWalkSpeed 验证
- [ ] 受到 30 点伤害后：CurrentHP 从 100 降至 70，IsAlive 保持 true
- [ ] 受到 ≥100 点伤害后：CurrentHP 降至 0，IsAlive 变为 false，OnDeath 事件在 1 帧内触发
- [ ] UAIPerceptionComponent 默认启用视觉感知，半径 1500 cm
- [ ] 所有 BlueprintReadWrite 属性在蓝图 Details 面板中可见且可编辑
- [ ] 血条偏移属性默认 Z=120 cm
- [ ] ANPCCharacter 被 AIController 接管后，接受 MoveTo 指令并移动至目标点（同关卡内，无寻路错误）
- [ ] 类不包含武器、战斗、技能/能力系统代码
- [ ] 类文件命名遵循 UE 规范：ANPCCharacter.h / ANPCCharacter.cpp，路径位于 Source/ 模块目录

## 边界条件（不做的事）
不包含：武器系统、战斗逻辑、技能/能力系统（属于后续 GA_/GE_ 工单）
不包含：动画蓝图、动画资产（ABP/AS/AM 由动画工单负责）
不包含：UI 血条 Widget 实现（只暴露属性接口，UI 由 UI 工单负责）
不包含：导航/寻路算法实现（由 UE 内置 MovementComponent + NavMesh 处理）
不包含：网络复制属性设置（Replication 由后续网络工单负责，当前单机基础实现）
不包含：音效触发、脚步声（由音频工单负责）
不包含：骨骼网格体、材质等视觉资产（由美术工单负责）
暂不支持：多人在线、专用服务器部署场景

## 资产需求线索
图标：NPC 蓝图默认缩略图，暂无特殊需求
图片：暂无
音效：暂无
动画：暂无（后续工单补充 AS_Idle/AS_Walk/AS_Death）
模型：暂无（后续工单补充 SK_NPC_Base 骨骼网格体）
粒子特效：暂无（后续工单补充受击/死亡 VFX）

---
## 待确认事项
1. MaxHP=100 是否合理？不同 NPC 类型是否需要不同基础值？
2. WalkSpeed=400 cm/s 是否与玩家移速匹配？需要快于/慢于玩家？
3. 视觉感知半径 1500 cm 是否合适？（与关卡尺度相关）
4. 是否需要听觉感知（Hearing Sense）？如需，触发距离多少？
5. 是否需要伤害类型过滤（物理/火/毒）？
6. OnDeath 触发后角色行为：直接销毁还是保留尸体/布娃娃？
7. TakeDamage 是否需要返回实际造成的伤害值？
