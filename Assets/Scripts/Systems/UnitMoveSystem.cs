using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Systems {
  internal partial struct UnitMoveSystem : ISystem {
    [BurstCompile]
    public void OnUpdate(ref SystemState state) {
      foreach ((RefRW<LocalTransform> localTransform, RefRO<MoveSpeed> movementSpeed) in SystemAPI.Query<RefRW<LocalTransform>, RefRO<MoveSpeed>>()) {
        localTransform.ValueRW.Position = localTransform.ValueRO.Position + new float3(movementSpeed.ValueRO.value, 0, 0) * SystemAPI.Time.DeltaTime;
      }
    }
  }
}