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