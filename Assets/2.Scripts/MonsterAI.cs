using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterAI : MonoBehaviour
{
    // --- 설정 변수 ---
    [Header("Wander Settings")]
    public float wanderRadius = 10f;        // 배회 반경 (시작 지점 기준 이 거리 안에서만 이동)
    public float minIdleTime = 1.0f;        // 최소 대기 시간
    public float maxIdleTime = 4.0f;        // 최대 대기 시간
    public float searchArea;

    // --- 내부 변수 ---
    private NavMeshAgent agent;     
    private Vector3 startPosition; // 몬스터의 초기 위치 (배회 중심점)
    //private Animator animator;

    // --- 상태 관련 ---
    private enum AIState { Idle, Wandering } // 상태 정의
    private AIState currentState;           // 현재 몬스터 상태
    private float idleTimer;                // 대기 상태 타이머
    private int maxSampleTries = 10; // 유효 위치 찾기 최대 시도 횟수

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();         
        startPosition = transform.position; // 시작 위치 저장
        //animator = GetComponent<Animator>(); //

        
        NavMeshHit hit;
        if (!NavMesh.SamplePosition(startPosition, out hit, searchArea, NavMesh.AllAreas))
        {
            Debug.LogError($"{gameObject.name}: 시작 위치가 NavMesh 위에 있지 않습니다! AI가 작동하지 않을 수 있습니다.");
            enabled = false; 
            return;
        }
        
        SetState(AIState.Idle);
    }

    void Update()
    {
        // 현재 상태에 따라 로직 실행
        switch (currentState)
        {
            case AIState.Idle:
                UpdateIdleState();
                break;
            case AIState.Wandering:
                UpdateWanderingState();
                break;
        }               
        
    }

    // 상태 변경 함수
    private void SetState(AIState newState)
    {
        currentState = newState;

        switch (currentState)
        {
            case AIState.Idle:
                if (agent.isOnNavMesh) agent.isStopped = true; // 이동 멈춤 (NavMesh 위에 있을 때만)
                idleTimer = Random.Range(minIdleTime, maxIdleTime); // 랜덤 대기 시간 설정
                break;
            case AIState.Wandering:
                if (agent.isOnNavMesh) agent.isStopped = false; // 이동 시작 (NavMesh 위에 있을 때만)
                break;
        }
        Debug.Log($"상태 변경: {currentState}");
    }

    // --- 상태별 업데이트 로직 ---

    private void UpdateIdleState()
    {
        idleTimer -= Time.deltaTime;
        if (idleTimer <= 0f)
        {
            // 대기 시간이 끝나면 새로운 목적지 탐색 및 이동 시작
            if (TrySetNewBoundedWanderDestination())
            {
                SetState(AIState.Wandering);
            }
            else
            {
                // 만약 목적지를 못 찾으면 다시 Idle 상태 유지 (타이머 재설정)
                // 유효 지점을 못 찾는 경우는 반경 내 NavMesh가 없거나 매우 작을 때 발생 가능
                idleTimer = Random.Range(minIdleTime, maxIdleTime); // 다시 대기
                Debug.LogWarning($"{gameObject.name}: 유효한 배회 지점을 찾지 못했습니다. 반경({wanderRadius}m) 내 NavMesh를 확인하세요. 다시 대기합니다.");
            }
        }
    }

    private void UpdateWanderingState()
    {
        // NavMeshAgent가 활성화되어 있고, 경로 계산이 완료되었으며, 남은 거리가 정지 거리 이하일 때
        if (agent.isOnNavMesh && !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            // 도착했으면 다시 Idle 상태로 변경
            SetState(AIState.Idle);
        }
        
    }

    // --- 보조 함수 ---

    // 지정된 반경 내에서 새로운 배회 목적지 설정 시도
    private bool TrySetNewBoundedWanderDestination()
    {
        for (int i = 0; i < maxSampleTries; i++) // 여러 번 시도하여 유효 지점 찾기 확률 높임
        {
            // 1. 시작 위치 기준, wanderRadius 반경 내의 랜덤한 2D 방향과 거리 생성
            Vector2 randomPoint2D = Random.insideUnitCircle * wanderRadius;
            Vector3 randomPoint = startPosition + new Vector3(randomPoint2D.x, 0f, randomPoint2D.y); // Y축은 시작 높이 유지

            // 2. 생성된 랜덤 위치 근처의 NavMesh 상 가장 가까운 유효 지점 찾기
            NavMeshHit hit;            
            if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
            {
                // 3. 찾은 지점(hit.position)이 정말로 시작 위치 기준 wanderRadius 반경 내에 있는지 최종 확인
                if (Vector3.Distance(startPosition, hit.position) <= wanderRadius)
                {
                    // 유효한 위치를 찾았으면 Agent의 목적지로 설정
                    if (agent.isOnNavMesh) // 안전 확인
                    {
                        agent.SetDestination(hit.position);
                         Debug.Log($"새 목적지 설정 (시도 {i+1}): {hit.position}");
                        return true; // 성공
                    }
                }
                
            }
        }
                
        return false; // 실패
    }   
    

    
    void OnDrawGizmosSelected()
    {
        // 배회 반경(범위) 시각화
        if (Application.isPlaying) // 실행 중에만 시작 위치 기준으로 그림
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(startPosition, wanderRadius);
        }
        else 
        {
            Gizmos.color = Color.gray;
            Gizmos.DrawWireSphere(transform.position, wanderRadius);
        }


        // 현재 목적지 시각화 (이동 중일 때)
        if (agent != null && agent.isOnNavMesh && agent.hasPath)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(agent.destination, 0.5f); // 목적지에 작은 구 표시
            Gizmos.DrawLine(transform.position, agent.destination); // 현재 위치에서 목적지까지 선 표시
        }
    }
}
