using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

namespace Systems {
  partial struct UnitMoveSystem : ISystem {
    [BurstCompile]
    public void OnUpdate(ref SystemState state) {
      foreach ((RefRW<LocalTransform> localTransform, RefRO<MoveSpeed> movementSpeed, RefRW<PhysicsVelocity> velocity) in SystemAPI.Query<RefRW<LocalTransform>, RefRO<MoveSpeed>, RefRW<PhysicsVelocity>>()) {
        float3 targetPosition = localTransform.ValueRO.Position + new float3(10, 0, 0);
        float3 movementDirection = targetPosition - localTransform.ValueRO.Position;
        movementDirection = math.normalize(movementDirection);
        localTransform.ValueRW.Rotation = quaternion.LookRotation(movementDirection, math.up());
        velocity.ValueRW.Linear = movementDirection * movementSpeed.ValueRO.value;
        velocity.ValueRW.Angular = float3.zero;
        // localTransform.ValueRW.Position += movementDirection * movementSpeed.ValueRO.value * SystemAPI.Time.DeltaTime;
      }
    }
  }
}