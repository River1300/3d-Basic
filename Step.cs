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