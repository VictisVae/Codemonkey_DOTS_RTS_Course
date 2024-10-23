using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

namespace Systems {
  partial struct UnitMoveSystem : ISystem {
    [BurstCompile]
    public void OnUpdate(ref SystemState state) {
      foreach ((RefRW<LocalTransform> localTransform, RefRO<UnitMover> unitMover, RefRW<PhysicsVelocity> velocity) in SystemAPI
                 .Query<RefRW<LocalTransform>, RefRO<UnitMover>, RefRW<PhysicsVelocity>>()) {
        float3 movementDirection = unitMover.ValueRO.targetPosition - localTransform.ValueRO.Position;
        movementDirection = math.normalize(movementDirection);

        localTransform.ValueRW.Rotation = math.slerp(localTransform.ValueRO.Rotation, quaternion.LookRotation(movementDirection, math.up()),
          SystemAPI.Time.DeltaTime * unitMover.ValueRO.rotationSpeed);

        velocity.ValueRW.Linear = movementDirection * unitMover.ValueRO.moveSpeed;
        velocity.ValueRW.Angular = float3.zero;
        // localTransform.ValueRW.Position += movementDirection * movementSpeed.ValueRO.value * SystemAPI.Time.DeltaTime;
      }
    }
  }
}