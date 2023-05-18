/*
#. 준비하기

    1. 에셋 스토어 파일 다운로드
    2. 임포트
*/

/*
#. 지형 만들기

    1. 큐브 만들기
    2. Lighting Setting에서 Generate Lighting
    3. 바닥 지형과 경계선 지형을 만든다.
    4. 화면 방향의 경계선 지형은 렌더러 컴포넌트를 비활성화 한다.
    5. 지형 재질을 만든다.
        Albedo 타일 지정
        Tileing 갯수 지정
        색 지정
*/

/*
#. 플레이어 만들기

    1. 플레이어 프리팹을 가져온다.
    2. 리지드바디, 콜라이더, 스크립트 컴포넌트를 부착한다.
*/

/*
1. 플레이어 이동

    1. 필요 속성 : 수평 값, 수직 값, 방향값 Vector
    2. 입력 값을 받고 방향값에 정규화 하여 저장
    3. 이동 = 현재 위치 + 방향 * 속도 * 시간
    4. 리지드바디 Rotation x, z축 잠금
    5. 플레이어의 이동은 Update()함수에 실행된다. 그런데 경계선에 충돌하는 Physics는 FixedUpdate()함수에서 실행된다.
        프레임 비율이 다르기 때문에 경계선을 뚫고 가는 경우가 생길 수 있다.
        리지드바디 Collision Detection -> Continuous : CPU를 좀더 사용하지만 물리적 계산을 더 한다.
*/

/*
2. 플레이어 애니메이션

    1. 애니매이터 컨트롤러 생성
    2. 플레이어 객체의 자식으로 있는 매쉬 오브젝트에 부착
    3. 컨트롤러에 애니매이션 클립 Idle, Walk, Run을 넣는다.
    4. 트랜지션을 연결한다.
    5. bool 파라미터 지정
    6. 필요 속성 : 애니매이터, 걷기 bool 변수
        애니매이터 컴포넌트는 플레이어의 자식이 가지고 있으므로 GetComponentInChildren으로 초기화 한다.
    7. 기본 움직임을 Run으로 지정하여 방향 속성이 Vector3.zero가 아님을 파라미터로 전달한다.
    8. Project Setting에서 Input Manager를 추가한다.
        left shift로 walk를 추가한다.
        다른 호환키는 지우도록 한다.
    9. 쉬프트키 입력을 bool 변수로 받고
    10. 파라미터로 bool 변수를 전달한다.
    11. 걷기 이동속도를 낮춘다.
        bool 변수가 true일 때 실수를 곱해준다.
*/

/*
3. 플레이어 회전

    1. LookAt() 함수를 이용하여 지정된 Vector방향으로 회전하도록 한다.
*/

/*
4. 카메라 무브

    1. 카메라 위치를 조정한다.
    2. 스크립트를 만들고 부착한다.
    3. 필요 속성 : 카메라가 따라다닐 타겟의 위치, 카메라 고정 위치 오프셋
    4. 카메라의 위치는 타겟 위치 + 오프셋
*/

/*
5. 코드 정리

    1. 입력받는 기능을 하는 로직을 함수로 묶는다.
    2. 움직이는 기능을 하는 로직을 함수로 묶는다.
    3. 회전하는 기능을 하는 로직을 함수로 묶는다.
*/

/*
6. 점프 구현

    1. 점프 함수를 만든다.
    2. 필요 속성 : 점프 하였는지 체크할 bool 변수, 리지드 바디, 점프 중 임을 체크할 bool 변수
    3. 점프키가 눌렸는지 bool 변수로 저장 받는다.
    4. 점프키가 눌렸다면 AddForce를 활용하여 y값으로 힘을 가한다.
    5. 점프를 하는 중에는 true값을 bool 변수에 둔다.
    6. 점프키가 눌렸다면, 제어문에 현재 점프 중인지도 체크한다.
    7. 충돌 함수를 만든다. OnCollisionEnter
    8. 충돌한 collision의 태그를 통해 바닥을 인지하고 bool 변수에 false를 준다.
    9. 바닥 오브젝트에 tag를 추가한다.
*/

/*
7. 점프 애니매이션

    1. 애니매이터에 점프, 착지, 회피 애니메이션 클립을 추가한다.
    2. 트랜지션을 연결한다.
    3. 파라미터로 점프 트리거와 회피 트리거를 만든다.
    4. 점프의 경우 점프 중임을 체크할 플래그가 필요하므로 bool 파라미터를 추가한다.
    5. 점프를 할 때 플래그와 트리거에 파라미터를 전달한다.
    6. 바닥에 착지할 때 플래그에 다시 파라미터를 전달한다.
    7. 중력 값을 증가시킨다.
        ProjectSetting Physics
*/

/*
#. 지형 물리 강화

    1. 지형 오브젝트를 static으로 전환한다.
        플레이어의 리지드바디로 설정한 Collision Dection : Continuous를 효과적으로 사용하기 위해 충돌하는 오브젝트를 static으로 전환시킨다.
    2. 지형 오브젝트에 리지드 바디를 부착하고 중력을 무시한다.
        스크립트 이외의 계산으로는 움직이지 않도록 isKinematic 체크
    3. Physics Material을 추가한다.
        마찰력을 최소화 한다.
*/

/*
8. 회피

    1. 회피 함수를 만든다.
    2. 필요 속성 : 현재 회피 중 인지 확인할 bool 변수
    3. 점프와 동일한 제어문을 만든다.
    4. 속도 값을 2배로 증가 시킨다.
    5. 회피 파라미터를 전달한다.
    6. 현재 회피 중 true
    7. 회피 종료 함수를 만든다.
        속도를 원상태로 되돌리고 회피 중 false
    8. 회피할 때 Invoke로 회피 종료 함수를 호출한다.
    9. 점프 함수 제어문에 현재 움직임이 zero일 때를 추가한다.
    10. 반대로 회피는 움직이고 있을 때를 추가한다.
    11. 점프 중일 때는 회피가 불가능하고 회피 중일 때는 점프가 불가능 하도록 제어문에 추가해 준다.
    12. 회피 도중에 방향을 바꾸지 못하도록 한다.
        새로운 방향 Vector3를 만든다.
    13. 회피를 할 때 새로운 방향 벡터에 현재 방향 값을 저장한다.
    14. 이동 함수에서 방향값을 일반화 한 뒤 제어문으로 현재 회피 중인지를 체크한다.
        회피 중이라면 현재 방향 값을 새로운 벡터로 배정한다.
*/

/*
#. 아이템 준비

    1. 아이템 프리팹을 객체로 옮긴다.
    2. 메쉬가 포함된 오브젝트 위치와 각도를 조정한다.
*/

/*
#. 라이트 이펙트

    1. 아이템의 자식으로 빈 오브젝트를 만든다.
    2. 빈 오브젝트에 Light 컴포넌트를 부착한다.
        type은 point로, 빛의 세기 intensity 조정, 빛의 범위 range 조정
*/

/*
#. 파티클 이펙트

    1. 아이템의 자식으로 빈 오브젝트를 만든다.
    2. Particle System 컴포넌트를 부착한다.
        Renderer -> Material
        Emission
        Shape
        Color Over LifeTime
        Size Over LifeTime
        Limit Velocity Over LifeTime -> Drag
        Start LifeTime -> Random Between Two Constants
        Start Speed
*/

/*
9. 파티클 로직

    1. 아이템에 리지드바디와 콜라이더를 부착
        콜라이더는 땅에 착지용, 플레이어와 충돌용 총 두 개를 부착한다.
            충돌용은 트리거
    2. 아이템 스크립트 생성 및 부착
    3. 필요 속성 : 아이템 타입을 열거형으로, 타입을 저장할 변수, 아이템의 값을 저장할 변수
    4. 아이템을 회전 시킨다.
*/

/*
#. 아이템을 프리팹으로 저장

    1. 아이템, 무기 태그를 만든다.
    2. 프리팹 폴더에 등록
*/

/*
10. 오브젝트 감지

    1. Player가 아이템을 감지하기 위해 OnTrigger 함수를 활용한다.
        OnTriggerStay, OnTriggerExit
    2. Player가 감지한 오브젝트를 저장하기 위한 게임 오브젝트 변수
    3. 접촉한 오브젝트의 태그가 무기일 경우 변수에 저장
    4. 접촉한 오브젝트에서 벗어났을 때는 변수에 null을 저장
*/

/*
11. 무기 입수

    1. 상호 작용 버튼을 InputManager에 등록한다.
    2. 필요 속성 : 상호 작용 버튼이 눌렸는지 체크할 bool 변수, 무기를 담을 오브젝트 배열, 보유하고 있는 무기를 체크할 bool 배열
    3. 상호 작용 버튼 입력을 bool 변수에 저장한다.
    4. 상호 작용 함수를 만든다.
        만약 현재 상호 작용 변수가 true이고 가까이에 오브젝트가 있다면,
        또 점프 중, 회피 중이 아닐 경우에
            그리고 만약 가까이에 있는 오브젝트가 무기라면,
                해당 오브젝트로 부터 아이템 스크립트를 받아서 저장한다.
                무기의 value값을 인덱스 변수에 저장한다.
                인덱스를 무기 bool에 넣고 true로 배정한다.
                아이템을 획득 했으므로 가까이에 있는 오브젝트는 제거한다.
*/

/*
#. 무기 장착

    1. 플레이어 오브젝트의 자식인 Mesh Object를 열어서 오른 손의 자식으로 실린더를 만든다.
    2. 실린더의 기존 콜라이더는 제거하고 렌더러는 비활성화
    3. 실린더의 자식으로 무기 오브젝트를 등록한다.
    4. Player 스크립트에 무기 오브젝트를 등록한다.
*/

/*
12. 무기 교체

    1. 필요 속성 : 무기를 교체할 플래그, 장착하고 있는 무기를 저장할 게임 오브젝트, 무기 교체 중임을 체크할 bool 변수, 무기 번호 변수
    2. InputManager에서 무기 3개의 각각 교체 버튼을 만든다.
    3. 무기 교체 함수를 만든다.
        만약 무기 교체기 3개중 한 개라도 눌렀다면,
        단 점프 중이거나 회피 중일 때는 바꾸지 못한다.
        교체 버튼에 따라 정수 형 변수에 인덱스 값을 저장한다.
            무기 오브젝트 배열에 인덱스 값을 이용하여 활성화 시킨다.
    4. 무기를 교체하면 기존에 들었던 무기는 빼야 한다.
        게임 오브젝트를 비활성화 한다.
            단 처음 게임을 시작할 때는 게임 오브젝트가 null이므로 제어문을 만들어서 null이 아닐 때만 비활성화 한다.
        버튼을 누른 무기를 게임 오브젝트에 저장하고 그 게임 오브젝트를 활성화 하도록 한다.
    5. 무기 교체 애니매이션을 추가한다.
    6. 트리거 파라미터 추가
    7. 무기를 교체할 때 애니매이터에 파라미터를 전달한다.
    8. 무기를 교체 중일 때는 다른 어떠한 움직임도 허용하지 않겠다.
    9. 무기를 교체할 때 교체 중 변수를 true로 배정한다.
    10. 교체가 끝날때 교체 중 변수를 false로 만들 함수를 만든다.
    11. 교체 중일 때는 점프도, 회피도, 움직임도 못하도록 제어문을 추가한다.
    12. 획득 하지 못한 무기는 들을 수 없게, 이미 들고 있는 무기는 교체 애니매이션이 않나오게 제어한다.
        누른 무기 스왑 버튼이 bool 배열에 저장 되었는지, 현재 이미 장비하고 있는지에 따라 반환한다.
        무기를 바꿀 때 현재 무기 번호 변수에 번호를 저장한다.
*/

/*
13. 아이템 먹기 준비

    1. 필요 속성 : 탄약 갯수 변수, 동전 갯수 변수, 체력 변수, 폭탄 갯수 변수, 각 수치의 최대값 변수
    2. 씬에서 초기 값 지정
*/

/*
14. 아이템 입수

    1. 충돌시 아이템을 입수하도록 한다. OnTriggerEnter
    2. 태그를 통해 아이템임을 구별하고 아이템 스크립트를 받아온다.
    3. 아이템의 타입에 따라 switch문으로 로직을 작성한다.
        아이템을 먹으면 변수에 해당 아이템의 value값을 더하고 만약 최대치일 경우 최대치를 배정한다.
    4. 먹은 아이템은 제거한다.
*/

/*
15. 공전 물체 만들기

    1. 필요 속성 : 수류탄 용 게임 오브젝트 배열,
    2. 빈 게임 오브젝트를 만든다.
        4 방향으로 공전할 빈 게임 오브젝트를 만든다.
            위치를 지정한다.
    3. 부모 오브젝트의 위치를 플레이어에 맞춘다.
    4. 방향 마다 자식 오브젝트로 수류탄을 배치한다.
        마테리얼을 교체한다.
        Light 컴포넌트를 추가한다.
        파티클을 추가한다.
            Emmision -> RateOverDistance로 설정하여 움직임 거리에 따라 파티클을 생성하도록 한다.
            Simulation Space를 local에서 World로 수정하여 오브젝트 움직임과 별개로 파티클이 생성되도록 한다. 
    5. 공전 스크립트 생성
        필요 속성 : 공전의 중심이 되어 줄 위치, 공전 속도, 중심과의 거리를 저장할 벡터
    6. RotateAround() 함수를 통해 지정된 목표를 중심으로 회전하도록 한다.
    7. 시작할 때 폭탄 오브젝트의 위치와 플레이어의 위치를 빼서 벡터에 거리를 저장한다.
    8. 매 프레임 마다 폭탄의 위치를 목표의 위치 + 벡터 거리를 연산하여 배정한다.
    9. 이동을 하면서 벡터의 거리 값을 매번 업데이트 해준다.
    10. Player가 보유하고 있는 수류탄 갯수만 활성화 되도록 모든 수류탄을 비활성화 한다.
    11. 미리 만들어 두었던 public 수류탄 오브젝트에 수류탄을 등록한다.
    12. 플레이어가 수류탄을 먹을 때 인덱스 0부터 게임 오브젝트를 활성화 시킨다.
*/

/*
16. 무기 공격 준비

    1. 무기 스크립트를 만든다.
    2. 비활성화 시켜놓았던 플레이어의 자식 -> 무기에 각각 부착
    3. 필요 속성 : 근접인지 원거리인지 열거형 타입, 타입을 저장할 변수, 공격력, 공격 속도, 공격 범위 콜라이더, 공격 효과
    4. 근접 무기에 박스 콜라이더 부착
        공격 범위 크기만큼 위치와 크기 지정
    5. 만들어 두었던 범위 변수에 자기 자신을 넣어 준다.
    6. 근접 태그를 만든다.
    7. 근접 무기의 자식으로 빈 오브젝트를 만든다.
        TrailRenderer 컴포넌트를 부착하여 잔상을 그린다.
            기본 마테리얼 지정
            Width 에 key를 추가하여 커브를 준다.
            Time을 지정
            Min Vertex Distance 설정
            Color 지정
            빈 오브젝트의 위치를 이펙트가 나가는 곳으로 이동
    8. 빈 오브젝트를 만들어 두었던 공격 효과에 부착
    9. 무기의 박스 콜라이더와 TrailRenderer 컴포넌트를 비활성화 시켜 놓는다.
*/

/*
17. 공격 로직

    1. 무기 사용 함수를 만든다.
    2. 무기의 타입이 근접일 경우 휘두르기 함수를 호출한다.
    3. 휘두르기 함수를 만든다.
        휘두르기 함수는 코루틴을 사용하여 사용 함수와 함께 실행되도록 한다.
        yield로 결과를 전달한다.
    4. 한 줄을 실행 -> 한 줄을 대기 -> 다음 한 줄 실행
        대기에서 WaitForSeconds로 값만큼 대기하도록 한다.
    5. 이제 플레이어가 공격을 실행하도록 한다.
        필요 속성 : 공격키가 눌렸는지 플래그, 공격 딜레이, 현재 공격 가능 상태 플래그, 장착 무기로 지정해 둔 게임 오브젝트를 Weapon 스크립트로 변경
    6. 장착 무기 로직을 수정한다.
    7. 공격키를 입력 받는다.
    8. 공격 함수를 만든다.
    9. 매 프레임 마다 공격 딜레이를 추가 연산하면서 장착한 무기의 공격 속도와 비교한다.
    10. 공격할 때 Weapon스크립트의 공격 함수를 호출하고 애니매이션 파라미터를 전달한다.
    11. 트리거 파라미터를 추가하고 공격 애니매이션을 등록한다.
*/

/*
18. 총알 탄피 만들기

    1. 빈 게임 오브젝트를 만들어서 권총 총알을 만든다.
        TrailRenderer 부착
        리지드바디, 콜라이더 부착
    2. 날아가는 총알을 복사하여 SMG 총알을 만든다.
    3. 탄피 오브젝트를 등록한다.
        리지드바디, 콜라이더 부착
    4. 총알 스크립트를 만든다.
    5. 필요 속성 : 데미지
    6. 총알이 충돌한 영역이 바닥이면 3초 뒤에 없어진다.
    7. 총알이 충돌한 영역이 벽이면 바로 없어진다.
*/

/*
19. 발사 구현

    1. 애니매이션 클립 등록
    2. 트리거 파라미터 등록
    3. 플레이어가 공격할 때 애니매이션 파라미터를 전달했었다. 이 때 무기의 타입을 확인하여 다른 파라미터를 전달하도록 한다.
    4. 플레이어 자식 오브젝트로 둔 총에게 발사 속도를 부여한다.
    5. 플레이어가 총을 쏘기 위해 총알이 나갈 위치와 탄피가 나갈 위치가 필요하다.
    6. 총이 발사되는 위치 지정
    7. 탄피가 배출되는 위치 지정
*/

/*
20. 재장전 구현

    1. 필요 속성 : 전체 보유 가능 탄약, 장전된 탄약
    2. 재장전 키를 등록한다.
    3. 애니매이션 클립과 트리거 파라미터 추가
    4. 장전 키를 입력 받는다.
    5. 장전 할 수 없는 상황에서는 반환한다.
    6. 장전키가 눌림 + 점프중X + 회피중X + 무기교체중X + 전투 준비 상태에서만 무기를 장전한다.
    7. 일정 시간 후 무기 장전이 완료되고 총알이 채워지고 보유 총알은 줄어든다.
*/

/*
21. 마우스 회전

    1. 필요 속성 : 카메라 오브젝트
    2. 마우스로 Ray를 쏜 좌표를 받는다.
    3. 해당 좌표를 바라보는 방향 값을 구한다.
    4. 플레이어가 방향 값에 따라 회전한다.
*/

/*
22. 플레이어 물리 문제 수정

    1. 플레이어가 외부의 오브젝트와 충돌할 때 리지드바디에 회전 속력이 생기는 현상
        FixedUpdate()함수에서 회전을 체크하도록 한다.
        회전을 멈추는 함수를 만든다.
            물리적 회전속도를 0으로 지정한다.
    2. 플레이어가 불필요한 충돌을 하지 않도록 충돌 레이어를 설정
        바닥 레이어 지정
        플레이어 레이어 지정
        플레이어 총알 레이어 지정
        플레이어 총알 탄피 레이어 지정
        세팅에서 레이어 간의 충돌을 설정
    3. 플레이어가 벽을 뚫고 간다.
        벽 앞에서 멈추는 함수를 만든다.
        Ray를 통해 플레이어 앞으로 벽을 감지하도록 한다.
        경계선에 닿았는지 체크할 플래그를 만든다.
        플래그에 RayCast값을 반환 받아서 Move에서 제한하도록 한다.
            해당 방향으로는 이동 값을 더하지 않도록 한다.
    4. 드랍 아이템과 충돌을 맊는다.
        아이템의 리지드 바디와 콜라이더를 불러온다.
        충돌 이벤트 함수를 만든다.
        바닥에 닿은 상태일 때 isKinematic을 활성화 한다.
        그 이후 콜라이더를 비활성화 시킨다.
*/

/*
23. 피격 테스터 생성

    1. 큐브를 생성한다.
        적 스크립트 생성, 부착
    2. 충돌 이벤트를 만든다.
        최대 체력, 현재 체력, 리지드바디, 콜라이더가 필요하다.
        트리거로 충돌 이벤트를 만든다.
        태그를 통해 충돌 콜라이더로 부터 데미지를 받는다. Melee, Bullet
        충돌체로부터 Weapon스크립트를 받아온다.
            체력을 줄인다.
        총알이 충돌하는 이벤트를 Collision으로 만들었었다.
            벽에 충돌하는 로직은 Collider이벤트로 옮겨준다.
    3. 피격 로직을 담을 코루틴 함수를 만든다.
        피격 충돌을 받을때 코루틴 함수를 호출한다.
        피격됐을 때 시각적인 변화를 주기 위해 메쉬랜더러로부터 마테리얼을 받아온다.
        색을 바꾸고, 0.1초 뒤에 체력이 남아있는지 체크
        남아있다면 색을 원상태로 되돌린다.
        없다면 색을 회색으로 변경 후 4초뒤에 제거
        시체매너를 위해 적 레이어와 죽은 적 레이어를 만든다.
            죽은 적은 땅과 벽이외에는 충돌하지 않는다.
        죽을 때 레이어를 바꿔준다.
    4. 적이 죽을 때 뒤로 튕겨저 나간다.
        튕겨져 나가는 방향을 구하기 위해 현재 적의 위치에서 피격 위치를 뺀다.
        매개변수로 위치 값을 코루틴 함수에 전달한다.
        튕겨져 나가는 방향을 일반화 시키고 살짝 위로 뜨게끔 y축 값을 더한다.
        리지드바디를 통해 넉백 시킨다.
        총알이 적을 관통하지 않도록 제거한다.
*/

/*
24. 수류탄 구현

    1. 수류탄을 씬에 등록하고 자식으로 폭발 이펙트 프리팹을 등록
        수류탄에 리지드바디와 콜라이더를 부착
        물리재질(PhysicsMaterial)을 만들어서 부착
        수류탄의 메쉬 오브젝트에 잔상 효과를 나타내기 위한 Trail Renderer을 부착
        수튜탄에 레이어로 플레이어 총알을 지정하여 플레이어와 수류탄이 충돌하지 않도록 한다.
        수류탄을 프리팹으로 만든다.
    2. 플레이어 스크립트에서 폭탄을 던지도록 한다.
        필요 속성 : 수류탄 프리팹을 담을 게임 오브젝트, 수류탄 버튼 플래그
        버튼을 입력 받는다.
        수류탄 투척 함수를 만든다.
        기본적인 제어문, 수류탄 갯수, 점프 중, 회피 중, 스왑 중
        Ray를 발사하여 마우스 좌표 지점으로 수류탄을 던지도록 한다.
        수류탄은 리지드 바디를 통해 던지도록 한다.
        수류탄을 던졌다면 갯수가 줄어든다.
        플레이어를 공전하던 수류탄 오브젝트 한 개가 비활성화 된다.
    3. 수류탄 폭발 스크립트를 만들어 폭발을 제어한다.
        필요 속성 : 폭탄 메쉬와 폭발 파티클을 활성화, 비활성화 시키기 위해 게임 오브젝트로 받아온다. 리지드 바디
        수류탄은 던지자 마자 터지지 않기 때문에 코루틴 함수로 구현한다.
        일정 시간 후 터지는데 이 때 수류탄 메쉬는 비활성화 되고 파티클은 활성화 된다.
        또한 폭발할 때 폭탄의 이동속도와 회전 속도를 0으로 지정해 준다.
        폭탄 스크립트는 던져지는 수류탄에 부착되기 때문에 로직의 실행은 Start함수에서 진행한다.
    4. 수류탄에 피격된 몬스터들을 RaycastHit로 모두 날려버린다.
        수류탄 폭발에 휘말린 모든 몬스터 정보를 받아와야 하기 때문에 RaycastHit배열로 만든다.
        폭발 위치와 반지름, 방향, 길이, 레이어를 매개변수로 전달한다.
        배열로 받은 오브젝트 정보들을 순회하며 적 스크립트를 받는다.
            적 스크립트로 가서 폭탄에 맞는 함수를 만들고 매개변수로 폭탄의 위치를 받는다.
            폭탄에 맞으면 더 멀리 튕겨 나간다.
            이전에 만들어 두었던 OnDamage 함수를 호출한다. 이 때 폭탄에 맞아 죽은 적은 회전을 하면서 죽을 예정이므로 매개 변수로 플래그를 추가한다.
        폭탄이 터진 후에는 폭탄 오브젝트를 제거한다.
*/

/*
25. 추적 AI

    1. 적 프리팹을 씬에 등록
        리지드바디, 콜라이더, 스크립트 부착
        테스트 에너미는 메쉬 랜더러 컴포넌트를 본인지 가지고 있었지만 적 객체는 자식이 가지고 있다.
            GetComponentInChildren로 마테리얼을 초기화 한다.
        태그와 레이어를 적으로 지정한다.
    2. 적 객체에 Nav A.I 컴포넌트를 부착한다.
        적 스크립트에 플레이어 정보를 추가한다.
            필요 속성 : 플레이어 위치, AI,
        Update()함수에 매 프레임 마다 목표를 향해 가도록 SetDestination()함수를 사용한다.
        NavMesh를 지정한다. Bake
        적 객체가 다른 오브젝트와 충돌할 때 물리적인 힘이 발생하여 알 수 없는 움직임을 보인다.
            플레이어 스크립트에 만들어 두었던 FreezeRotation함수를 그대로 사용한다.
        Navigation bake
    3. 애니메이션을 등록한다.
        애니매이션 클립마다 트랜지션을 연결한다. 
            bool 타입의 걷기, 공격 파라미터와 트리거 죽음을 만들어 준다.
        필요 속성 : 애니메이터( 적또한 자식이 애니메이터를 가지고 있다. ), 플래그
        추적 중인 상태에서만 Nav를 작동하도록 한다.
        추적 플래그를 활성화 시킬 함수를 만든다.
            걷기 애니매이션 출력
        적이 죽을 때 플래그를 비활성화 시킨다.
        적이 죽을 때 추적을 중지하여 죽는 물리효과에 방해되지 않도록 한다.
        적이 죽을 때 죽기 애니매이션 출력
        추적 중일 때만 물리적인 속도를 0으로 설정하도록 제어한다.
*/

/*
26. 플레이어 피격

    1. 플레이어 충돌 이벤트 함수에 적 총알 태그와 충돌시도 추가한다.
        적 총알로 부터 총알 스크립트를 받아와서 체력을 뺀다.
    2. 피격 이벤트 함수를 코루틴으로 만든다.
        무적 플래그 변수를 만든다.
    3. 피격 상태임을 플래그 변수에 담는다.
    4. 반대로 적 총알과 충돌하는 로직은 피격 상태가 아닐 때만 실행되도록 제어한다.
    5. 일정 시간동안 무적 타입을 지정하고 그 후 다시 무적 플래그 변수를 false로
        그리고 피격되었을 때 잠깐 플레이어의 색을 바꿔줄 것이다.
            매쉬렌더러를 배열로 받아온다.( 플레이어 파츠 별로 )
        배열을 순회하며 색을 바꿔준다.
        무적시간이 지단 뒤에 색을 원상 복구 한다.
*/

/*
27. 일반 타입 몬스터 구성

    1. 근접 공격 범위를 빈 오브젝트에 콜라이더와 총알 스크립트를 부착해 위치에 놓는다.
        적 총알이라는 태그와 레이어를 부착한다.
    2. 콜라이더를 담을 변수와 공격 상태를 확인할 플래그가 필요하다.
    3. 현재 플레이어의 위치와 얼만큼 가까이에 있는지 타겟팅할 필요가 있다.
        타겟팅 함수를 만든다.
            Ray를 스피어 캐스트 형태로 발사하여 타겟팅 할 예정
                반지름과 거리 변수를 만든다.
            수류탄과 마찬가지로 배열로 광선 구조체를 받는 스피어 캐스트를 발사한다.
            RaycastHit에 플레이어가 들어왔는지 확인하는 제어문을 만든다.
                거리를 통해 플레이어가 공격 위치까지 왔다면 코루틴 공격 함수 호출
    4. 공격 함수를 코루틴으로 만든다.
        멈추었다가 공격을 한다.
        공격 애니매이션 출력
        공격 범위를 활성화 한다.
*/

/*
28. 돌격 타입 몬스터 구성

    1. 일반 타입과 대부분 같은 컴포넌트와 같은 애니매이션을 사용한다.
    2. 다만, 이동 속도가 다르기 때문에 Nav컴포넌트에서 속도,회전,가속도를 조정해 준다.
    3. 필요 속성 : 몬스터 타입
    4. switch문을 통해 몬스터 타입에 따라 반지름과 감지 거리를 조정한다.
    5. 공격도 switch문에 따라 각기 다른 공격을 진행하도록 한다.
    6. 돌격 타입은 타겟을 감지하면 그 방향으로 본인이 발싸된다.
    7. 그리고 멈추었다가 다시 타겟을 감지하면 반복
*/

/*
29. 원거리 타입 몬스터 구성

    1. 몬스터에게 기본적인 컴포넌트 부착
    2. 미사일 프리팹을 씬에 생성
    3. 미사일 스크립트 만들고 미사일 자식에 부착
        매 프레임 마다 좌측 기준으로 회전
        미사일 자식으로 빈 오브젝트 생성
            파티클 부착
        미사일에도 콜라이더 부착, 트리거
        리지드 바디도 부착, 중력 무시
        총알 스크립트 부착
        미사일을 프리팹화 한다.
        방향 조정
    4. 애니매이터 컨트롤러 생성 및 부착
    5. 필요 속성 : 미사일 프리팹 변수
    6. 타겟팅은 길고 정확해야 한다.
    7. 돌진은 본인이 날라갔다면 원거리는 총알을 만들어서 날려보낸다.
        인스턴트화 시킨 후에 리지드 바디를 받아와 날린다.
        이때 총알의 발사 위치 때문에 가장 먼저 충돌하는 객체는 적 자신일 것이다.
        레이어 설정을 통해 적 총알은 적과 충돌하지 않게 한다.
    8. 미사일이 플레이어와 충돌 하였다면 미사일을 제거하도록 한다.
        플레이어 스크립트로 가서 적 총알과 충돌할 때 충돌체에 리지드 바디가 있는지 확인한다.
        이전의 적 객체 두 종류의 충돌 박스에는 리지드 바디가 없다.
    9. 총알 스크립트에 보면 벽이나 바닥에 충돌했을 때 총알이 제거되도록 해놓았다.
        플레이어가 벽에 붙어 있을 때 적이 공격할 경우 히트 박스가 벽에 충돌하면 적의 히트 박스가 사라질 수 있다.
        그러므로 히트 박스가 근접인지 플래그를 만든다.
            그리고 충돌 제어문에 근접이 아닌 것만 제거되도록 한다.
*/

/*
#. 보스 기본 세팅

    1. 보스 자식에 애니매이터 등록 및 애니매이션 등록
        기존 bool 파라미터 제거
        공격 타입별 트리거 등록
    2. 보스에게 리지드바디와 콜라이더 등록
    3. Nav.AI 등록, 보스는 오직 점프만 하기 때문에 회전 속도를 0으로 맞추어 준다.
    4. 미사일이 발사될 위치를 빈 오브젝트를 만들어 잡아 준다.
    5. 점프 공격 범위를 빈 오브젝트로 만들고 박스 콜라이더를 부착한다. 트리거
*/

/*
30. 유도 미사일 준비

    1. 보스 미사일과 보스 주먹을 씬에 등록
    2. 보스 미사일의 자식에 미사일 스크립트 부착
        미사일 이펙트 생성
    3. 보스 미사일에 리지드바디와, 콜라이더 부착, 유도 미사일이기 때문에 Nav.AI도 부착
    4. 총알 스크립트를 상속 받을 보스 미사일 스크립트를 만든다.
        MonoBechaviour 대신 Bullet으로 교체
        미사일에 네비를 사용하기 위해 using AI 추가
    5. 필요 속성 : 플레이어 위치, Nav.Ai
    6. 매 프레임마다 플레이어를 추적
    7. 보스 미사일에 스크립트 추가
*/

/*
31. 보스 주먹 준비

    1. 보스 주먹은 구를 예정이므로 리지드바디 컴포넌트가 필요하다.
        무게를 10정도 주고, 회전 저항을 0으로, x축을 기준으로만 회전하기 때문에 y, z는 잠근다.
    2. 스피어 콜라이더도 추가한다.
    3. 보스 주먹 스크립트를 만든다.
    4. 필요 속성 : 리지드바디, 회전 속도, 크기, 기를 모으고 쏘는 타이밍을 체크할 플래그
    5. 기를 모아서 발싸하기 때문에 코루틴 함수가 필요하다.
        기를 모으는 코루틴, 발사하는 코루틴 함수를 만든다.
        기를 모으는 동안 대기하고 플래그에 true
        기를 모으기 전까지는 반복 루프
            회전 속도, 크기를 반복하며 키운다.
            자체 좌표 공간에서의 크기를 서서히 키워준다.
            주먹의 질량에 따른 가속도값을 서서히 기워준다.
        두 개의 코루틴 함수를 Awake에서 실행한다.
    6. Bullet 클래스를 상속 받는다.
    7. 태그와 레이어를 적 총알로 지정한다.
    8. 총알은 바닥에 닿으면 제거되는 로직이 있다.
        보스 주먹인지 확인할 플래그를 만든다.
        바닥에 닿았을 때 보스 주먹이 아닌 경우에만 총알을 제거하도록 한다.
    9. 현재 보스 주먹에는 구르기 위한 물리 담당 콜라이더만이 존재 한다.
        로직으로 충돌을 처리한 콜라이더를 추가한다.
    10. 미사일과 주먹은 프리팹화 한다.
*/

/*
32. 보스 기본 로직

    1. 적 스크립트로 가서 열거형으로 타입을 만들고 보스가 하지 않을 로직에 제어문을 걸어 준다.
    2. 우선 Awake에서 적이 플레이어를 추적하였는데 보스는 추적하지 않는다.
    3. 보스가 아닐때만 타겟팅 한다.
    4. 보스를 잡는 순간 스테이지 종료이기 때문에 보스를 제거될 필요가 없다.
    5. 보스가 피격될 때 모든 파츠를 받아오기 위해 기본의 Material컴포넌트 대신 MeshRenderer[]로 바꾼다.
        반복문으로 모든 파츠를 순회하며 MeshRenderer에 있는 material의 색을 바꾼다.
    6. 보스 스크립트를 만들고 보스 객체에 부착
    7. 적을 상속 받는다.
    8. 필요 속성 : 미사일 프리팹, 미사일 발사 위치, 플레이어 움직임을 예측하는 벡터, 내려찍기할 위치 벡터, 플레이어를 바라보는 플래그
    9. 매 프레임 마다 플레이어를 바라보는지 확인하고, 플레이어의 다음 위치를 예측한다.
        가로 세로 입력값을 받는다.
        입력 받은 값을 바탕으로 다음 위치를 저장한다.
        보스가 바라보는 방향은 현재 플레이어 위치 + 다음 위치
*/