using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

namespace Systems {
  internal partial struct UnitMoveSystem : ISystem {
    [BurstCompile]
    public void OnUpdate(ref SystemState state) {
      UnitMoverJob unitMoverJob = new() {
        deltaTime = SystemAPI.Time.DeltaTime
      };

      unitMoverJob.ScheduleParallel();

      // foreach ((RefRW<LocalTransform> localTransform, RefRO<UnitMover> unitMover, RefRW<PhysicsVelocity> velocity) in SystemAPI
      //            .Query<RefRW<LocalTransform>, RefRO<UnitMover>, RefRW<PhysicsVelocity>>()) {
      //   float3 movementDirection = unitMover.ValueRO.targetPosition - localTransform.ValueRO.Position;
      //   movementDirection = math.normalize(movementDirection);
      //
      //   localTransform.ValueRW.Rotation = math.slerp(localTransform.ValueRO.Rotation, quaternion.LookRotation(movementDirection, math.up()),
      //     SystemAPI.Time.DeltaTime * unitMover.ValueRO.rotationSpeed);
      //
      //   velocity.ValueRW.Linear = movementDirection * unitMover.ValueRO.moveSpeed;
      //   velocity.ValueRW.Angular = float3.zero;
      // }
    }

    [BurstCompile]
    public partial struct UnitMoverJob : IJobEntity {
      public float deltaTime;

      private void Execute(ref LocalTransform localTransform, in UnitMover unitMover, ref PhysicsVelocity velocity) {
        float3 movementDirection = unitMover.targetPosition - localTransform.Position;
        float unitReachedDistance = 1.0f;

        if (math.length(movementDirection) < unitReachedDistance) {
          velocity.Linear = float3.zero;
          velocity.Angular = float3.zero;
          return;
        }

        movementDirection = math.normalize(movementDirection);

        localTransform.Rotation = math.slerp(localTransform.Rotation, quaternion.LookRotation(movementDirection, math.up()),
          deltaTime * unitMover.rotationSpeed);

        velocity.Linear = movementDirection * unitMover.moveSpeed;
        velocity.Angular = float3.zero;
      }
    }
  }
}